using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using Assimp;

namespace TagTool.Geometry.Jms
{
    public class JmsModeExporter
    {
        GameCache Cache { get; set; }
        JmsFormat Jms { get; set; }

        public JmsModeExporter(GameCache cacheContext, JmsFormat jms)
        {
            Cache = cacheContext;
            Jms = jms;
        }

        public void Export(RenderModel mode)
        {
            //build markers
            foreach (var markergroup in mode.MarkerGroups)
            {
                var name = Cache.StringTable.GetString(markergroup.Name);
                foreach (var marker in markergroup.Markers)
                {
                    Jms.Markers.Add(new JmsFormat.JmsMarker
                    {
                        Name = name,
                        NodeIndex = marker.NodeIndex,
                        Rotation = marker.Rotation,
                        Translation = new RealVector3d(marker.Translation.X, marker.Translation.Y, marker.Translation.Z) * 100.0f,
                        Radius = marker.Scale
                    });
                }
            };

            List<JmsFormat.JmsMaterial> materialList = new List<JmsFormat.JmsMaterial>();

            //build meshes
            ModelExtractor extractor = new ModelExtractor(Cache, mode);
            var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(mode.Geometry.Resource);
            mode.Geometry.SetResourceBuffers(resource);
            List<Triangle> Triangles = new List<Triangle>();
            foreach (var region in mode.Regions)
            {
                foreach (var perm in region.Permutations)
                {
                    if (perm.MeshIndex == -1)
                        continue;
                    var mesh = mode.Geometry.Meshes[perm.MeshIndex];
                    var meshReader = new MeshReader(Cache, mode.Geometry.Meshes[perm.MeshIndex]);

                    var vertices = new List<ModelExtractor.GenericVertex>();
                    if(Cache.Version >= CacheVersion.HaloReach)
                    {
                        vertices = ModelExtractor.ReadVerticesReach(meshReader);
                        if(mesh.ReachType == VertexTypeReach.Rigid || mesh.ReachType == VertexTypeReach.RigidCompressed)
                        {
                            vertices.ForEach(v => v.Indices = new byte[4] { (byte)mesh.RigidNodeIndex, 0, 0, 0 });
                            vertices.ForEach(v => v.Weights = new float[4] { 1, 0, 0, 0 });
                        }                      
                    }
                    else
                    {
                        vertices = ModelExtractor.ReadVertices(meshReader);
                        if(mesh.Type == VertexType.Rigid)
                        {
                            vertices.ForEach(v => v.Indices = new byte[4] { (byte)mesh.RigidNodeIndex, 0, 0, 0 });
                            vertices.ForEach(v => v.Weights = new float[4] { 1, 0, 0, 0 });
                        }
                    }
                        
                    ModelExtractor.DecompressVertices(vertices, new VertexCompressor(mode.Geometry.Compression[0]));
                    for (int partIndex = 0; partIndex < mesh.Parts.Count; partIndex++)
                    {
                        int newMaterialIndex = -1;
                        if(mesh.Parts[partIndex].MaterialIndex != -1)
                        {
                            CachedTag renderMethod = mode.Materials[mesh.Parts[partIndex].MaterialIndex].RenderMethod;
                            JmsFormat.JmsMaterial newMaterial = new JmsFormat.JmsMaterial
                            {
                                Name = renderMethod == null ? "default" : renderMethod.Name.Split('\\').Last(),
                                MaterialName = $"{Cache.StringTable.GetString(perm.Name)} {Cache.StringTable.GetString(region.Name)}"
                            };
                            int existingIndex = materialList.FindIndex(m => m.Name == newMaterial.Name && m.MaterialName == newMaterial.MaterialName);
                            if (existingIndex == -1)
                            {
                                newMaterialIndex = materialList.Count;
                                materialList.Add(newMaterial);
                            }
                            else
                                newMaterialIndex = existingIndex;
                        }                        

                        var indices = ModelExtractor.ReadIndices(meshReader, mesh.Parts[partIndex]);
                        for(var j = 0; j < indices.Length; j += 3)
                        {
                            Triangles.Add(new Triangle
                            {
                                Vertices = new List<ModelExtractor.GenericVertex>
                                {
                                    vertices[indices[j]],
                                    vertices[indices[j + 1]],
                                    vertices[indices[j + 2]],
                                },
                                MaterialIndex = newMaterialIndex
                            });
                        }
                    }
                }
            }

            //add triangles and vertices to JMS
            foreach(var triangle in Triangles)
            {
                Jms.Triangles.Add(new JmsFormat.JmsTriangle
                {
                    VertexIndices = new List<int>
                                {
                                    Jms.Vertices.Count,
                                    Jms.Vertices.Count + 1,
                                    Jms.Vertices.Count + 2,
                                },
                    MaterialIndex = triangle.MaterialIndex
                });

                foreach(var vertex in triangle.Vertices)
                {
                    var newVert = new JmsFormat.JmsVertex
                    {
                        Position = new RealPoint3d(vertex.Position.X, vertex.Position.Y, vertex.Position.Z) * 100.0f,
                        Normal = new RealVector3d(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z),
                        NodeSets = new List<JmsFormat.JmsVertex.NodeSet>(),
                        UvSets = new List<JmsFormat.JmsVertex.UvSet>()
                            {
                                new JmsFormat.JmsVertex.UvSet
                                {
                                    TextureCoordinates = new RealPoint2d(vertex.TexCoords.X, 1 - vertex.TexCoords.Y)
                                }
                            }
                    };
                    if (vertex.Weights != null)
                    {
                        for (var i = 0; i < vertex.Weights.Length; i++)
                        {
                            if (vertex.Weights[i] != 0.0f)
                            {
                                newVert.NodeSets.Add(new JmsFormat.JmsVertex.NodeSet
                                {
                                    NodeIndex = vertex.Indices[i],
                                    NodeWeight = vertex.Weights[i]
                                });
                            }
                        }
                    }
                    Jms.Vertices.Add(newVert);
                }            
            }

            //add materials
            foreach (var material in materialList)
                Jms.Materials.Add(new JmsFormat.JmsMaterial
                {
                    Name = material.Name,
                    MaterialName = $"({Jms.Materials.Count + 1}) {material.MaterialName}"
                });
        }

        public void SmoothVertices(JmsFormat jms)
        {
            //build list of identical vertices
            List<List<int>> MatchingVertices = new List<List<int>>();
            for(var i = 0; i < jms.Vertices.Count; i++)
            {
                bool matchFound = false;
                foreach(var vertexset in MatchingVertices)
                {
                    if (Vector3.Distance(PointToVector(jms.Vertices[vertexset[0]].Position), PointToVector(jms.Vertices[i].Position)) < 0.01)
                    {
                        matchFound = true;
                        vertexset.Add(i);
                        break;
                    }
                }
                if(!matchFound)
                    MatchingVertices.Add(new List<int> { i });
            }

            //average vertex normals and apply
            foreach(var vertexgroup in MatchingVertices)
            {
                List<RealVector3d> normals = vertexgroup.Select(v => jms.Vertices[v].Normal).ToList();
                RealVector3d sum = new RealVector3d();
                foreach (var normal in normals)
                    sum += normal;
                RealVector3d average = sum / normals.Count;
                foreach (var vertex in vertexgroup)
                    jms.Vertices[vertex].Normal = average;
            }
        }

        private Vector3 PointToVector(RealPoint3d point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        struct Triangle
        {
            public List<ModelExtractor.GenericVertex> Vertices;
            public int MaterialIndex;
        }
    }
}
