using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class CollisionBSPGen3Builder
    {
        public List<leaf> initial_leaves = new List<leaf>();
        public LargeCollisionBspBlock Bsp { get; set; }
        public class polygon
        {
            public List<RealPoint3d> vertices;
            public int surface_index = -1;
            public int plane_index = -1;
            public bool is_connection = false;
            public int leaf_above = -1;
            public int leaf_below = -1;
        }

        public class polygon_plane
        {
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        }

        public class leaf_connection
        {
            public int leaf_above = -1;
            public int leaf_below = -1;
            public float connection_total;
            public float connection_vs_nonconnection;
            public float nonconnection_total;
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        };

        public class leaf
        {
            public float float10;
            public List<leaf_connection> leaf_connections = new List<leaf_connection>();
        }
        public enum planerelationship : int
        {
            Unknown = 0,
            BackofPlane = 1,
            FrontofPlane = 2,
            BothSidesofPlane = 3, //both 1 & 2 
            OnPlane = 4
        }
        public bool build_bsp()
        {
            List<polygon_plane> polygon_planes = build_polygon_planes();

            return true;
        }

        public void build_bsp3d_node(List<polygon_plane> polygon_planes, ref int node_index)
        {
            if(polygon_planes.Count <= 0)
            {
                initial_leaves.Add(new leaf());
                node_index = initial_leaves.Count - 1;
            }
            else
            {
            }
        }

        public List<polygon_plane> build_polygon_planes()
        {
            //build polygon planes
            List<polygon_plane> polygon_planes = new List<polygon_plane>();
            for (var i = 0; i < Bsp.Surfaces.Count; i++)
            {
                LargeSurface surface = Bsp.Surfaces[i];
                if (surface.Flags.HasFlag(SurfaceFlags.Invalid))
                    continue;

                polygon new_polygon = new polygon
                {
                    surface_index = i,
                    plane_index = surface.Plane,
                    vertices = surface_collect_vertices(i)
                };

                if (surface.Flags.HasFlag(SurfaceFlags.PlaneNegated))
                    new_polygon.plane_index = (int)(new_polygon.plane_index | 0x80000000);

                int matching_index = polygon_planes.FindIndex(p => p.plane_index == (surface.Plane & 0x7FFFFFFF));
                if (matching_index == -1)
                {
                    polygon_planes.Add(new polygon_plane
                    {
                        plane_index = surface.Plane,
                        polygons = new List<polygon> { new_polygon }
                    });
                }
                else
                    polygon_planes[i].polygons.Add(new_polygon);
            }
            //order polygon planes by plane index
            polygon_planes.OrderBy(poly => poly.plane_index);
            return polygon_planes;
        }

        public double plane_get_splitting_effectiveness(int plane_index, List<polygon_plane> polygon_planes)
        {
            int front_count = 0;
            int back_count = 0;
            int total_count = 0;
            foreach(var poly_plane in polygon_planes)
            {
                if (poly_plane.plane_index == plane_index)
                    continue;
                foreach(var poly in poly_plane.polygons)
                {
                    planerelationship polyrelationship = determine_polygon_plane_relationship(poly, plane_index);
                    //
                    if (polyrelationship == planerelationship.FrontofPlane)
                        front_count++;
                    else if (polyrelationship == planerelationship.BackofPlane)
                        back_count++;
                    total_count++;
                }
            }
            return 2.0d * total_count + Math.Abs(back_count - front_count);
        }

        public planerelationship determine_polygon_plane_relationship(polygon poly, int plane_index)
        {
            planerelationship polygon_plane_relationship = 0;

            //check if surface is on the plane
            if ((poly.plane_index & 0x7FFFFFFF) == plane_index)
                return planerelationship.OnPlane;

            RealPlane3d plane = Bsp.Planes[plane_index & 0x7FFFFFFF].Value;

            foreach (var vert in poly.vertices)
            {
                planerelationship vertex_plane_relationship = determine_vertex_plane_relationship(vert, plane);
                if (!vertex_plane_relationship.HasFlag(planerelationship.OnPlane))
                    polygon_plane_relationship |= vertex_plane_relationship;

                if (polygon_plane_relationship.HasFlag(planerelationship.BothSidesofPlane))
                    break;
            }
            return polygon_plane_relationship;
        }
        public planerelationship determine_vertex_plane_relationship(RealPoint3d vertex, RealPlane3d plane)
        {
            double plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

            if (plane_equation_vertex_input >= -0.00024414062)
            {
                //if the plane distance is within both of these bounds, it is considered ON the plane
                if (plane_equation_vertex_input <= 0.00024414062)
                    return planerelationship.OnPlane;
                else
                    return planerelationship.FrontofPlane;
            }
            else
            {
                return planerelationship.BackofPlane;
            }
        }
        public List<RealPoint3d> surface_collect_vertices(int surface_index)
        {
            List<RealPoint3d> pointlist = new List<RealPoint3d>();
            LargeSurface surface_block = Bsp.Surfaces[surface_index];

            int surface_edge_index = surface_block.FirstEdge;
            //collect vertices on the plane
            while (true)
            {
                LargeEdge surface_edge_block = Bsp.Edges[surface_edge_index];
                if (surface_edge_block.RightSurface == surface_index)
                {
                    pointlist.Add(Bsp.Vertices[surface_edge_block.EndVertex].Point);
                    surface_edge_index = surface_edge_block.ReverseEdge;
                }
                else
                {
                    pointlist.Add(Bsp.Vertices[surface_edge_block.StartVertex].Point);
                    surface_edge_index = surface_edge_block.ForwardEdge;
                }
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }
            return pointlist;
        }
    }
}
