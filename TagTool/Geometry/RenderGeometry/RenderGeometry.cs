using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Geometry
{
    [TagStructure(Name = "render_geometry", Size = 0x84)]
    public class RenderGeometry : TagStructure
	{
        /// <summary>
        /// The runtime flags of the render geometry.
        /// </summary>
        public RenderGeometryRuntimeFlags RuntimeFlags;

        /// <summary>
        /// The meshes of the render geometry.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// The compression information of the render geometry.
        /// </summary>
        public List<RenderGeometryCompression> Compression;

        /// <summary>
        /// The bounding spheres of the render geometry.
        /// </summary>
        public List<BoundingSphere> BoundingSpheres;

        public List<UnknownBlock> Unknown2;

        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;

        public List<MoppClusterVisiblity> MeshClusterVisibility;

        /// <summary>
        /// The per-mesh node mappings of the render geometry.
        /// </summary>
        public List<PerMeshNodeMap> PerMeshNodeMaps;

        /// <summary>
        /// The per-mesh subpart visibility of the render geometry.
        /// </summary>
        public List<PerMeshSubpartVisibilityBlock> PerMeshSubpartVisibility;

        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;


        public List<StaticPerPixelLighting> InstancedGeometryPerPixelLighting;

        public TagResourceReference Resource;

        [TagStructure(Size = 0x30)]
        public class BoundingSphere : TagStructure
		{
            public RealPlane3d Plane;
            public RealPoint3d Position;
            public float Radius;

            [TagField(Length = 4)]
            public sbyte[] NodeIndices;

            [TagField(Length = 3)]
            public float[] NodeWeights;
        }

        [TagStructure(Size = 0x18)]
        public class UnknownBlock : TagStructure
		{
            public byte UnknownByte1;
            public byte UnknownByte2;
            public short Unknown2;
            public byte[] Unknown3;
        }

        [TagStructure(Size = 0x20)]
        public class MoppClusterVisiblity : TagStructure
		{
            public byte[] MoppData;
            public List<short> UnknownMeshPartIndicesCount;
        }

        [TagStructure(Size = 0xC)]
		public class PerMeshNodeMap : TagStructure
		{
            public List<NodeIndex> NodeIndices;

            [TagStructure(Size = 0x1)]
			public class NodeIndex : TagStructure
			{
                public byte Node;
            }
        }

        [TagStructure(Size = 0xC)]
        public class PerMeshSubpartVisibilityBlock : TagStructure
		{
            public List<BoundingSphere> BoundingSpheres;
        }

        [TagStructure(Size = 0x10)]
        public class StaticPerPixelLighting : TagStructure
		{
            public List<int> UnusedVertexBuffer;

            public short VertexBufferIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;

            [TagField(Flags = Runtime)]
            public VertexBufferDefinition VertexBuffer;

        }



        //
        // Methods
        //

        /// <summary>
        /// Set the runtime VertexBufferResources and IndexBufferResources fields given the resource definition
        /// </summary>
        /// <param name="resourceDefinition"></param>
        public void SetResourceBuffers(RenderGeometryApiResourceDefinition resourceDefinition)
        {

            foreach(var mesh in Meshes)
            {
                mesh.ResourceVertexBuffers =  new VertexBufferDefinition[8];
                mesh.ResourceIndexBuffers =  new IndexBufferDefinition[2];

                for(int i = 0; i < mesh.VertexBufferIndices.Length; i++)
                {
                    var vertexBufferIndex = mesh.VertexBufferIndices[i];
                    if (vertexBufferIndex != -1)
                    {
                        if (vertexBufferIndex < resourceDefinition.VertexBuffers.Count)
                            mesh.ResourceVertexBuffers[i] = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
                        else
                            mesh.ResourceVertexBuffers[i] = null; // happens on sbsp
                    }
                }

                for(int i = 0; i < mesh.IndexBufferIndices.Length; i++)
                {
                    var indexBufferIndex = mesh.IndexBufferIndices[i];
                    if (indexBufferIndex != -1)
                    {
                        if (indexBufferIndex < resourceDefinition.IndexBuffers.Count)
                            mesh.ResourceIndexBuffers[i] = resourceDefinition.IndexBuffers[indexBufferIndex].Definition;
                        else
                            mesh.ResourceIndexBuffers[i] = null; // this happens when loading particle model from gen3, the index buffers are empty but indices are set to 0
                    }
                }
            }

            for(int i = 0; i < InstancedGeometryPerPixelLighting.Count; i++)
            {
                var vertexBufferIndex = InstancedGeometryPerPixelLighting[i].VertexBufferIndex;
                if (vertexBufferIndex != -1)
                {
                    if (vertexBufferIndex < resourceDefinition.VertexBuffers.Count)
                        InstancedGeometryPerPixelLighting[i].VertexBuffer = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
                    else
                        InstancedGeometryPerPixelLighting[i].VertexBuffer = null;
                }
            }
        }

        /// <summary>
        /// Generate a valid RenderGeometryApiResourceDefinition from the mesh blocks and sets the values in IndexBufferIndices, VertexBufferIndices
        /// </summary>
        /// <returns></returns>
        public RenderGeometryApiResourceDefinition GetResourceDefinition()
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, D3DFixups should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            foreach (var mesh in Meshes)
            {

                for(int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
                {
                    var vertexBuffer = mesh.ResourceVertexBuffers[i];
                    if (vertexBuffer != null)
                    {
                        var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                        d3dPointer.Definition = vertexBuffer;
                        result.VertexBuffers.Add(d3dPointer);
                        mesh.VertexBufferIndices[i] = (short)(result.VertexBuffers.Elements.Count - 1);
                    }
                    else
                        mesh.VertexBufferIndices[i] = - 1;
                }

                for (int i = 0; i < mesh.ResourceIndexBuffers.Length; i++)
                {
                    var indexBuffer = mesh.ResourceIndexBuffers[i];
                    if (indexBuffer != null)
                    {
                        var d3dPointer = new D3DStructure<IndexBufferDefinition>();
                        d3dPointer.Definition = indexBuffer;
                        result.IndexBuffers.Add(d3dPointer);
                        mesh.IndexBufferIndices[i] = (short)(result.IndexBuffers.Elements.Count - 1);
                    }
                    else
                        mesh.IndexBufferIndices[i] = -1;
                }

                // if the mesh is unindexed the index in the index buffer should be 0, but the buffer is empty. Copying what h3\ho does.
                if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
                {
                    mesh.IndexBufferIndices[0] = 0;
                    mesh.IndexBufferIndices[1] = 0;
                }
            }

            for (int i = 0; i < InstancedGeometryPerPixelLighting.Count; i++)
            {
                var perPixel = InstancedGeometryPerPixelLighting[i];
                if (perPixel.VertexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = perPixel.VertexBuffer;
                    result.VertexBuffers.Add(d3dPointer);
                    perPixel.VertexBufferIndex = (short)(result.VertexBuffers.Elements.Count - 1);
                }
                else
                    perPixel.VertexBufferIndex = -1;
            }

            return result;
        }
    }
}