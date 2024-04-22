using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using TagTool.Geometry.BspCollisionGeometry;
using static TagTool.IO.ConsoleHistory;
using TagTool.Geometry.Utils;

namespace TagTool.Geometry.Jms
{
    public class JmsCollExporter
    {
        GameCache Cache { get; set; }
        JmsFormat Jms { get; set; }

        public JmsCollExporter(GameCache cacheContext, JmsFormat jms)
        {
            Cache = cacheContext;
            Jms = jms;
        }

        public void Export(CollisionModel coll)
        {
            //geometry
            foreach (var region in coll.Regions)
            {
                foreach(var permutation in region.Permutations)
                {
                    int material_index = -1;
                    foreach(var bsp in permutation.Bsps)
                    {
                        List<Polygon> polygons = new List<Polygon>();
                        for (var i = 0; i < bsp.Geometry.Surfaces.Count; i++)
                            polygons.Add(SurfaceToPolygon(i, bsp.Geometry));

                        material_index = polygons[0].MaterialIndex;

                        //triangulate polygons as needed
                        List<Polygon> triangles = new List<Polygon>();
                        foreach(var polygon in polygons)
                        {
                            if (polygon.Vertices.Count == 3)
                                triangles.Add(polygon);
                            else
                            {
                                List<List<RealPoint3d>> newTriangles = Triangulator.Triangulate(polygon.Vertices, polygon.Plane);
                                foreach (var newTriangle in newTriangles)
                                    triangles.Add(new Polygon
                                    {
                                        Vertices = newTriangle,
                                        Plane = polygon.Plane,
                                        MaterialIndex = polygon.MaterialIndex
                                    });
                            }
                        }

                        //build vertex and index buffers
                        foreach(var triangle in triangles)
                        {
                            List<int> indices = new List<int>();
                            foreach(var point in triangle.Vertices)
                            {
                                indices.Add(Jms.Vertices.Count);
                                Jms.Vertices.Add(new JmsFormat.JmsVertex
                                {
                                    Position = point,
                                    NodeSets = new List<JmsFormat.JmsVertex.NodeSet>
                                            {
                                                new JmsFormat.JmsVertex.NodeSet
                                                {
                                                    NodeIndex = bsp.NodeIndex,
                                                    NodeWeight = 1.0f
                                                }
                                            }
                                });
                            }
                            Jms.Triangles.Add(new JmsFormat.JmsTriangle
                            {
                                VertexIndices = indices,
                                MaterialIndex = Jms.Materials.Count
                            });
                        }
                    }
                    Jms.Materials.Add(new JmsFormat.JmsMaterial
                    {
                        Name = material_index == -1 ? "default" : Cache.StringTable.GetString(coll.Materials[material_index].Name),
                        MaterialName = $"({Jms.Materials.Count + 1}) {Cache.StringTable.GetString(permutation.Name)} {Cache.StringTable.GetString(region.Name)}"
                    });
                }
            }

            //finally, transform points to node space
            foreach (var vert in Jms.Vertices)
            {
                vert.Position = TransformPointByNode(vert.Position, vert.NodeSets[0].NodeIndex);
            }
        }

        struct Polygon
        {
            public List<RealPoint3d> Vertices;
            public RealPlane3d Plane;
            public int MaterialIndex;
        }

        Polygon SurfaceToPolygon(int surface_index, CollisionGeometry bsp)
        {
            List<RealPoint3d> vertices = new List<RealPoint3d>();

            var surface = bsp.Surfaces[surface_index];
            var edge = bsp.Edges[surface.FirstEdge];

            while (true)
            {
                if (edge.LeftSurface == surface_index)
                {
                    vertices.Add(bsp.Vertices[edge.StartVertex].Point * 100.0f);

                    if (edge.ForwardEdge == surface.FirstEdge)
                        break;
                    else
                        edge = bsp.Edges[edge.ForwardEdge];
                }
                else if (edge.RightSurface == surface_index)
                {
                    vertices.Add(bsp.Vertices[edge.EndVertex].Point * 100.0f);

                    if (edge.ReverseEdge == surface.FirstEdge)
                        break;
                    else
                        edge = bsp.Edges[edge.ReverseEdge];
                }
            }

            //offset plane for later use
            RealPlane3d surfacePlane = bsp.Planes[surface.Plane & 0x7FFF].Value;
            surfacePlane = new RealPlane3d(surfacePlane.I, surfacePlane.J, surfacePlane.K, surfacePlane.D * 100.0f);

            return new Polygon
            {
                Vertices = vertices,
                MaterialIndex = surface.MaterialIndex,
                Plane = surfacePlane
            };
        }

        RealPoint3d TransformPointByNode(RealPoint3d point, int node_index)
        {
            Vector3 newPoint = new Vector3(point.X, point.Y, point.Z);
            RealQuaternion nodeRotation = Jms.Nodes[node_index].Rotation;
            RealVector3d nodeTranslation = Jms.Nodes[node_index].Position;
            Matrix4x4 nodeMatrix = Matrix4x4.CreateFromQuaternion(new Quaternion(nodeRotation.I, nodeRotation.J, nodeRotation.K, nodeRotation.W));
            nodeMatrix.Translation = new Vector3(nodeTranslation.I, nodeTranslation.J, nodeTranslation.K);
            newPoint = Vector3.Transform(newPoint, nodeMatrix);
            return new RealPoint3d(newPoint.X, newPoint.Y, newPoint.Z);
        }
    }
}
