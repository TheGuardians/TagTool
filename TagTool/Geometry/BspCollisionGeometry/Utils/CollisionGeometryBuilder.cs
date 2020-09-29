using TagTool.Common;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using BSP = TagTool.Tags.Definitions.CollisionModel.Region.Permutation.Bsp;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags;

namespace TagTool.Geometry
{
    public class CollisionGeometryBuilder
    {
        /// <summary>
        /// Offset and Size values for H1CE tagblock structs
        /// </summary>
        const int
            MAIN_STRUCT_OFFSET = 64,
            MAIN_STRUCT_SIZE = 664,
            MAIN_MATERIAL_OFFSET = 564,
            MAIN_REGION_OFFSET = 576,
            MAIN_PATHF_SPHERES_OFFSET = 640,
            MAIN_NODES_OFFSET = 652,
            MATERIAL_TAGBLOCK_SIZE = 72,
            REGION_TAGBLOCK_SIZE = 84,
            REGION_PERMUTATION_OFFSET = 72,
            PERMUTATION_SIZE = 32,
            PATHF_SPHERE_SIZE = 32,
            NODE_SIZE = 64,
            BSP_BSP3DNODES_OFFSET = 0,
            BSP_PLANES_OFFSET = 12,
            BSP_LEAVES_OFFSET = 24,
            BSP_BSP2DREFERENCES_OFFSET = 36,
            BSP_BSP2DNODES_OFFSET = 48,
            BSP_SURFACES_OFFSET = 60,
            BSP_EDGES_OFFSET = 72,
            BSP_VERTICES_OFFSET = 84,
            BSP_SIZE = 96,
            BSP3DNODE_SIZE = 12,
            PLANE_SIZE = 16,
            LEAF_SIZE = 8,
            BSP2DREFERENCE_SIZE = 8,
            BSP2DNODE_SIZE = 20,
            SURFACE_SIZE = 12,
            EDGE_SIZE = 24,
            VERTEX_SIZE = 16;

        private CollisionModel Definition;

        public CollisionGeometryBuilder() { }

        /// <summary>
        /// Stub for now, creates n materials, which are named 0 to (n-1)
        /// </summary>
        public long ParseMaterials(CollisionModel coll, BinaryReader reader, int count)
        {
            coll.Materials = new List<CollisionModel.Material>();

            for (uint i = 0; i < count; ++i)
            {
                var material = new CollisionModel.Material();
                material.Name = new StringId(0x140 + i);
                coll.Materials.Add(material);
            }

            return reader.BaseStream.Position + MATERIAL_TAGBLOCK_SIZE * count;
        }

        /// <summary>
        /// Parses regions into Halo Online collision 'Region' tagblocks.
        /// Names are not preserved.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long ParseRegions(CollisionModel coll, BinaryReader reader, int count)
        {
            var originalPos = reader.BaseStream.Position;

            //The number of permutations that have been parsed in total for all regions
            var n_parsed_permutations = 0;
            //Permutations for all regions follow sequentially after all regions
            var permutations_base_addr = reader.BaseStream.Position
                + count * REGION_TAGBLOCK_SIZE;

            coll.Regions = new List<CollisionModel.Region>();

            for (uint i = 0; i < count; ++i)
            {
                //configure the current region
                CollisionModel.Region region = new CollisionModel.Region();
                region.Name = new StringId(0x140 + i);

                //set up stream for reading number of permutations in current region
                reader.BaseStream.Position = originalPos
                    + (long)(i * REGION_TAGBLOCK_SIZE)
                    + REGION_PERMUTATION_OFFSET;

                uint n_permutations = (uint)BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                region.Permutations = new List<CollisionModel.Region.Permutation>();

                for (uint j = 0; j < n_permutations; ++j)
                {
                    var permutation = new CollisionModel.Region.Permutation();
                    permutation.Name = new StringId(0x140 + j);
                    permutation.Bsps = new List<BSP>();
                    region.Permutations.Add(permutation);
                }

                coll.Regions.Add(region);
                n_parsed_permutations += (int)n_permutations;
            }

            return permutations_base_addr + (n_parsed_permutations * PERMUTATION_SIZE);
        }

        public long ParsePathFindingSpheres(CollisionModel coll, BinaryReader reader, int count)
        {
            var originalPos = reader.BaseStream.Position;

            coll.PathfindingSpheres = new List<CollisionModel.PathfindingSphere>();

            for (uint i = 0; i < count; ++i)
            {
                var pfSphere = new CollisionModel.PathfindingSphere();
                reader.BaseStream.Position = originalPos + (i * PATHF_SPHERE_SIZE);
                var node_idx = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                reader.BaseStream.Position += 14; //14 bytes between node index and sphere location,radius
                var center_x = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var center_y = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var center_z = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var radius = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);

                pfSphere.Center = new RealPoint3d(center_x, center_y, center_z);
                pfSphere.Radius = radius;
                pfSphere.Node = node_idx;

                coll.PathfindingSpheres.Add(pfSphere);
            }

            return originalPos + (count * PATHF_SPHERE_SIZE);
        }

        /// <summary>
        /// Parses all H1CE Collision Node tagblocks stored sequentially.
        /// The purpose of 'Node' is similar to 'Region' in Halo Online.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long ParseNodes(CollisionModel coll, BinaryReader reader, int count)
        {
            var originalPos = reader.BaseStream.Position;
            coll.Nodes = new List<CollisionModel.Node>();
            //destroy the old list of regions, it may not be fine-grained enough
            coll.Regions = new List<CollisionModel.Region>();

            var new_region_count = 0;
            var current_bsp_offset = originalPos + (count * NODE_SIZE);

            for (var i = 0; i < count; ++i)
            {
                var node = new CollisionModel.Node();
                node.Name = new StringId(0x140 + (uint)i);

                //offset of the parent node in the h1 ce node tagblock
                reader.BaseStream.Position = originalPos + (i * NODE_SIZE) + 32;
                short region_idx = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                short parent_node_idx = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                short next_sibling_idx = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                short first_child_idx = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);

                node.ParentNode = parent_node_idx;
                node.NextSiblingNode = next_sibling_idx;
                node.FirstChildNode = first_child_idx;

                coll.Nodes.Add(node);

                //there exists a region when the region index of the h1 ce collision node tagblock is not null
                if (region_idx >= 0)
                {
                    var region = new CollisionModel.Region();
                    coll.Regions.Add(region);
                    region.Name = new StringId(0x140 + (uint)new_region_count);
                    reader.BaseStream.Position = originalPos + (i * NODE_SIZE) + 52; //bsp tagblock count

                    //each bsp is placed into a separate permutation. In h1 ce
                    // a node referencing a region with n permutations has n bsps
                    region.Permutations = new List<CollisionModel.Region.Permutation>();
                    CollisionModel.Region.Permutation permutation = new CollisionModel.Region.Permutation();
                    region.Permutations.Add(permutation);
                    permutation.Bsps = new List<BSP>();
                    uint n_bsps = (uint)BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                    for (uint j = 0; j < n_bsps; ++j)
                    {
                        reader.BaseStream.Position = current_bsp_offset;
                        current_bsp_offset = ParseBSP(permutation, reader);
                    }
                    new_region_count++;
                }
            }

            return current_bsp_offset;
        }

        public long ParseBSP(CollisionModel.Region.Permutation permutation, BinaryReader reader)
        {
            long originalPos = reader.BaseStream.Position;
            var bsp = new BSP();
            permutation.Bsps.Add(bsp);

            reader.BaseStream.Position = originalPos + BSP_BSP3DNODES_OFFSET;
            int n_3dnodes = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_PLANES_OFFSET;
            int n_planes = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_LEAVES_OFFSET;
            int n_leaves = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_BSP2DREFERENCES_OFFSET;
            int n_2dreferences = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_BSP2DNODES_OFFSET;
            int n_2dnodes = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_SURFACES_OFFSET;
            int n_surfaces = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_EDGES_OFFSET;
            int n_edges = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_VERTICES_OFFSET;
            int n_vertices = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            reader.BaseStream.Position = originalPos + BSP_SIZE;
            reader.BaseStream.Position = ParseBSP3DNodes(bsp, reader, n_3dnodes);
            reader.BaseStream.Position = ParsePlanes(bsp, reader, n_planes);
            reader.BaseStream.Position = ParseLeaves(bsp, reader, n_leaves);
            reader.BaseStream.Position = ParseBSP2DReferences(bsp, reader, n_2dreferences);
            reader.BaseStream.Position = ParseBSP2DNodes(bsp, reader, n_2dnodes);
            reader.BaseStream.Position = ParseSurfaces(bsp, reader, n_surfaces);
            reader.BaseStream.Position = ParseEdges(bsp, reader, n_edges);
            return ParseVertices(bsp, reader, n_vertices);
        }

        public long ParseBSP3DNodes(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry = new CollisionGeometry(); // adjusted to current struct
            bsp.Geometry.Bsp3dNodes = new TagBlock<Bsp3dNode>();
            long originalPos = reader.BaseStream.Position;
            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * BSP3DNODE_SIZE);

                var plane = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var frontChild = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var backChild = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

                // Convert to small bsp3d node designator (lossy)
                var bsp3dNode = new Bsp3dNode();
                bsp3dNode.Plane = plane;
                bsp3dNode.FrontChild = Bsp3dNode.CreateChild((Bsp3dNode.ChildType)((frontChild >> 31) & 1), frontChild);
                bsp3dNode.BackChild = Bsp3dNode.CreateChild((Bsp3dNode.ChildType)((backChild >> 31) & 1), backChild);
                bsp.Geometry.Bsp3dNodes.Add(bsp3dNode);
            }

            return originalPos + (count * BSP3DNODE_SIZE);
        }

        public long ParsePlanes(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Planes = new TagBlock<Plane>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * PLANE_SIZE);
                var plane_i = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var plane_j = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var plane_k = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var plane_d = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);

                var plane = new Plane();

                // ReForge change:
                // plane.Value.I = plane_i;
                // plane.Value.J = plane_j;
                // plane.Value.K = plane_k;
                // plane.Value.D = plane_d;

                plane.Value = new RealPlane3d { Normal = new RealVector3d(plane_i, plane_j, plane_k), Distance = plane_d };

                bsp.Geometry.Planes.Add(plane);
            }

            return originalPos + (count * PLANE_SIZE);
        }

        public long ParseLeaves(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Leaves = new TagBlock<Leaf>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * LEAF_SIZE);
                var flags = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                var bsp2dRefCount = BitConverter.ToUInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                var Unknown = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var first2dRef = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);

                var leaf = new Leaf();

                leaf.Flags = (LeafFlags)flags;
                leaf.Bsp2dReferenceCount = bsp2dRefCount;
                // ReForge change:
                // leaf.FirstBsp2dReference = (int)(((ushort)Unknown << 16) | (ushort)first2dRef);
                leaf.FirstBsp2dReference = (uint)((ushort)first2dRef | (ushort)Unknown);

                bsp.Geometry.Leaves.Add(leaf);
            }

            return originalPos + (count * LEAF_SIZE);
        }

        public long ParseBSP2DReferences(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Bsp2dReferences = new TagBlock<Bsp2dReference>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * BSP2DREFERENCE_SIZE);
                var plane_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var bsp2dnode_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

                var bsp2dRef = new Bsp2dReference();

                //truncate and preserve sign
                var uplane_idx = (plane_idx & 0x7fff);
                if (plane_idx < 0)
                    uplane_idx |= 0x8000;

                bsp2dRef.PlaneIndex = (short)uplane_idx;

                var ubsp2dnode_idx = (bsp2dnode_idx & 0x7fff);
                if (bsp2dnode_idx < 0)
                    ubsp2dnode_idx |= 0x8000;

                bsp2dRef.Bsp2dNodeIndex = (short)ubsp2dnode_idx;

                bsp.Geometry.Bsp2dReferences.Add(bsp2dRef);
            }

            return originalPos + (count * BSP2DREFERENCE_SIZE);
        }

        public long ParseBSP2DNodes(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Bsp2dNodes = new TagBlock<Bsp2dNode>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * BSP2DNODE_SIZE);
                var plane_i = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var plane_j = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var plane_d = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var left_child = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var right_child = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

                var bsp2dNode = new Bsp2dNode();
                bsp2dNode.Plane.I = plane_i;
                bsp2dNode.Plane.J = plane_j;
                bsp2dNode.Plane.D = plane_d;

                //sign-compress left and right children to int16
                var uleft_child = left_child & 0x7fff;
                if (left_child < 0)
                    uleft_child |= 0x8000;

                bsp2dNode.LeftChild = (short)uleft_child;

                var uright_child = right_child & 0x7fff;
                if (right_child < 0)
                    uright_child |= 0x8000;

                bsp2dNode.RightChild = (short)uright_child;

                bsp.Geometry.Bsp2dNodes.Add(bsp2dNode);
            }

            return originalPos + (count * BSP2DNODE_SIZE);
        }

        public long ParseSurfaces(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Surfaces = new TagBlock<Surface>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * SURFACE_SIZE);
                var plane_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var first_edge = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var flags = reader.ReadByte();
                var breakable_surface = reader.ReadByte();

                // ReForge change
                // var material = BitConverter.ToInt16(reader.ReadBytes(2).Reverse().ToArray(), 0);
                var material = (short)0;

                var surface = new Surface();

                //sign-compress the plane index
                var uplane_idx = (plane_idx & 0x7fff);
                if (plane_idx < 0)
                    uplane_idx |= 0x8000;

                surface.Plane = (ushort)uplane_idx;

                surface.FirstEdge = (ushort)first_edge;
                surface.MaterialIndex = material;
                surface.BreakableSurfaceIndex = breakable_surface;
                surface.Flags = (SurfaceFlags)flags;

                bsp.Geometry.Surfaces.Add(surface);
            }
            return originalPos + (count * SURFACE_SIZE);
        }

        public long ParseEdges(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Edges = new TagBlock<Edge>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * EDGE_SIZE);
                var start_vert_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var end_vert_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var forward_edge_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var reverse_edge_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var left_surface_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var right_surface_idx = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

                var edge = new Edge();

                edge.StartVertex = (ushort)start_vert_idx;
                edge.EndVertex = (ushort)end_vert_idx;
                edge.ForwardEdge = (ushort)forward_edge_idx;
                edge.ReverseEdge = (ushort)reverse_edge_idx;
                edge.LeftSurface = (ushort)left_surface_idx;
                edge.RightSurface = (ushort)right_surface_idx;

                bsp.Geometry.Edges.Add(edge);
            }

            return originalPos + (count * EDGE_SIZE);
        }

        public long ParseVertices(BSP bsp, BinaryReader reader, int count)
        {
            bsp.Geometry.Vertices = new TagBlock<Vertex>();
            var originalPos = reader.BaseStream.Position;

            for (uint i = 0; i < count; ++i)
            {
                reader.BaseStream.Position = originalPos + (i * VERTEX_SIZE);
                var point_x = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var point_y = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var point_z = BitConverter.ToSingle(reader.ReadBytes(4).Reverse().ToArray(), 0);
                var first_edge = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

                var vert = new Vertex();

                vert.Point = new RealPoint3d(point_x, point_y, point_z);
                vert.FirstEdge = (ushort)first_edge;

                bsp.Geometry.Vertices.Add(vert);
            }

            return originalPos + (count * VERTEX_SIZE);
        }

        public long ParseMain(CollisionModel coll, BinaryReader reader)
        {
            // start of the main struct in the reader
            var originalPos = reader.BaseStream.Position;
            //location of the count of materials
            reader.BaseStream.Position += MAIN_MATERIAL_OFFSET;
            var n_materials = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
            //change pos to the offset where the 'material' tagblocks begin
            reader.BaseStream.Position = originalPos + MAIN_REGION_OFFSET;
            var n_regions = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
            reader.BaseStream.Position = originalPos + MAIN_PATHF_SPHERES_OFFSET;
            var n_pathf_spheres = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
            reader.BaseStream.Position = originalPos + MAIN_NODES_OFFSET;
            var n_nodes = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);

            var afterReadPos = originalPos + MAIN_STRUCT_SIZE;
            //get the position after all of the materials
            reader.BaseStream.Position = afterReadPos;
            afterReadPos = ParseMaterials(coll, reader, n_materials);

            //Get the position after all of the sequentially stored regions and sequentially stored permutations
            reader.BaseStream.Position = afterReadPos;

            afterReadPos = ParseRegions(coll, reader, n_regions);
            //set to beginning of sequential list of path finding spheres.
            reader.BaseStream.Position = afterReadPos;
            afterReadPos = ParsePathFindingSpheres(coll, reader, n_pathf_spheres);

            reader.BaseStream.Position = afterReadPos;
            afterReadPos = ParseNodes(coll, reader, n_nodes);

            return afterReadPos;
        }

        /// <summary>
        /// This file parser will parse Halo 1 CE 'model_collision_geometry' tags.
        /// The addresses of the tagblocks inside the tag are likely to be garbage
        /// values. The Halo 1 CE development tool 'guerilla' does not use the 
        /// reflexive address value and expects chunks to occur in the order that
        /// the reflexives occur in the parent struct.
        /// 
        /// The Halo1 CE collision tag is used due to high compatibility and 
        /// availability of 'Tool' - a program which can compile collision tags.
        /// 
        /// The parser expects the following format:
        /// h1ce coll tag format:
        ///main struct
        ///all materials sequential
        ///all regions sequential
        ///all permutations sequential
        ///all path finding spheres sequential
        ///all nodes sequential
        ///bsp 0
        ///	   bsp0 3dnodes sequential
        ///	   ...
        ///	   bsp0 vertices sequential
        ///bsp 1
        ///	   ...
        ///...
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        public bool ParseFromFile(string fpath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fpath, FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The system cannot find the file specified.");
                return false;
            }

            var reader = new BinaryReader(fs);
            var coll = new CollisionModel();

            // h1 ce tags will have a 64 byte header. The main struct is immediately after.

            var len = reader.BaseStream.Length;
            reader.BaseStream.Position = MAIN_STRUCT_OFFSET;
            var size = ParseMain(coll, reader);

            if (len != size)
            {
                Console.WriteLine("length expected was not actual.\nexpected: " + len + ", actual: " + size);
                return false;
            }

            //builder succeeded
            Definition = coll;

            return true;
        }

        public CollisionModel Build() => Definition;
    }
}
