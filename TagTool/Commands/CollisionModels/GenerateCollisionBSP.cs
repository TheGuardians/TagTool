using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    class GenerateCollisionBSPCommand : Command
    {
        private GameCache Cache { get; }
        private CollisionModel Definition { get; }
        private CollisionGeometry Bsp { get; }

        public GenerateCollisionBSPCommand(GameCache cache, CollisionModel definition) :
            base(true,

                "GenerateCollisionBSP",
                "Generates a Collision BSP for the current collision tag, removing the previous collision BSP",

                "GenerateCollisionBSP",

                "")
        {
            Cache = cache;
            Definition = definition;
            Bsp = definition.Regions[0].Permutations[0].Bsps[0].Geometry;
        }

        public override object Execute(List<string> args)
        {
            Bsp.Leaves.Clear();
            Bsp.Bsp2dNodes.Clear();
            Bsp.Bsp2dReferences.Clear();
            Bsp.Bsp3dNodes.Clear();

            return true;
        }

        [Flags]
        public enum Surface_Plane_Relationship : int
        {
            Unknown = 0,
            SurfaceFrontofPlane = 1 << 0,
            SurfaceBackofPlane = 1 << 1,
            SurfaceOnPlane = 1 << 2
        }

        private Surface_Plane_Relationship determine_surface_plane_relationship(int surface_index, int plane_index, RealPlane3d plane_block)
        {
            Surface_Plane_Relationship surface_vertex_plane_relationship = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];

            //check if surface is on the plane
            if ((surface_block.Plane & (short)0x7FFF) == plane_index)
                return Surface_Plane_Relationship.SurfaceOnPlane;

            if (plane_block == null)
                plane_block = Bsp.Planes[plane_index].Value;

            int surface_edge_index = surface_block.FirstEdge;
            while(true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                RealPoint3d edge_vertex;
                if (surface_edge_block.RightSurface == surface_index)
                    edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex].Point;
                else
                    edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex].Point;
                float plane_equation_vertex_input = edge_vertex.X * plane_block.I + edge_vertex.Y * plane_block.J + edge_vertex.Z * plane_block.K - plane_block.D;

                if (plane_equation_vertex_input >= -0.00024414062)
                {
                    //if the plane equation vertex input is within these tolerances, it is considered to be on the plane 
                    if (plane_equation_vertex_input <= 0.00024414062)
                    {
                        if (surface_edge_block.RightSurface == surface_index)
                            surface_edge_index = surface_edge_block.ReverseEdge;
                        else
                            surface_edge_index = surface_edge_block.ForwardEdge;
                        //break the loop if we have finished circulating the surface
                        if (surface_edge_index == surface_block.FirstEdge)
                            break;
                        continue;
                    }
                    else
                        surface_vertex_plane_relationship |= Surface_Plane_Relationship.SurfaceBackofPlane;
                }
                else
                    surface_vertex_plane_relationship |= Surface_Plane_Relationship.SurfaceFrontofPlane;

                if (surface_edge_block.RightSurface == surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }

            //there is code here to deal with unclear results, but it appears that it will lead to an infinite loop
            /*
            if (surface_vertex_plane_relationship == Surface_Plane_Relationship.Unknown || 
                surface_vertex_plane_relationship.HasFlag(Surface_Plane_Relationship.SurfaceFrontofPlane) && surface_vertex_plane_relationship.HasFlag(Surface_Plane_Relationship.SurfaceBackofPlane))
            {
                surface_vertex_plane_relationship = determine_surface_plane_relationship(surface_index, -1, plane_block);
            }
            */

            return surface_vertex_plane_relationship;
        }
    }
}