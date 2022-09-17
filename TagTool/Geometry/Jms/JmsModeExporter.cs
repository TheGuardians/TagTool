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
                    var meshReader = new MeshReader(Cache.Version, Cache.Platform, mode.Geometry.Meshes[perm.MeshIndex], Cache.Endianness);

                    var vertices = new List<ModelExtractor.GenericVertex>();
                    if(Cache.Version >= CacheVersion.HaloReach)
                        vertices = ModelExtractor.ReadVerticesReach(meshReader);
                    else
                        vertices = ModelExtractor.ReadVertices(meshReader);
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
                                    TextureCoordinates = new RealPoint2d(vertex.TexCoords.X, vertex.TexCoords.Y)
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

        struct Triangle
        {
            public List<ModelExtractor.GenericVertex> Vertices;
            public int MaterialIndex;
        }
    }
}
