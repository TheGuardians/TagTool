using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using Assimp;
using TagTool.Pathfinding;

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
                        Radius = marker.Scale <= 0.0 ? 1.0f : marker.Scale //needs to not be zero
                    });
                }
            };

            List<JmsFormat.JmsMaterial> materialList = new List<JmsFormat.JmsMaterial>();

            //build meshes
            ModelExtractor extractor = new ModelExtractor(Cache, mode);
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
                    //assign rigid node indices from mesh for rigid meshes
                    if (Cache.Version >= CacheVersion.HaloReach)
                    {
                        vertices = ModelExtractor.ReadVerticesReach(meshReader);
                        if (mesh.ReachType == VertexTypeReach.Rigid || mesh.ReachType == VertexTypeReach.RigidCompressed)
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
                            string renderMaterialName = "default";
                            CachedTag renderMethod = mode.Materials[mesh.Parts[partIndex].MaterialIndex].RenderMethod;
                            if (renderMethod != null)
                            {
                                string renderMethodName = renderMethod.Name;
                                if (renderMethodName == null)
                                    renderMethodName = $"{renderMethod.Group.Tag}_" + $"0x{renderMethod.Index:X4}";
                                List<string> delimiters = new List<string> { "\\shaders\\", "\\materials\\", "\\" };
                                foreach(var delimiter in delimiters)
                                {
                                    string[] nameParts = renderMethodName.Split(new string[] { delimiter }, StringSplitOptions.None);
                                    if (nameParts.Length > 1)
                                    {
                                        renderMaterialName = nameParts.Last();
                                        break;
                                    }
                                    renderMaterialName = renderMethodName;
                                }             
                            }

                            JmsFormat.JmsMaterial newMaterial = new JmsFormat.JmsMaterial
                            {
                                Name = renderMaterialName,
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

                        //recalculate vertex normals
                        Vector3[] vertexPositions = vertices.Select(v => Vector3fromVector3D(v.Position)).ToArray();
                        Vector3[] vertexNormals = CalculateVertexNormals(vertexPositions, indices);
                        for (var v = 0; v < vertexNormals.Length; v++)
                            vertices[v].Normal = Vector3DfromVector3(vertexNormals[v]);

                        for(var j = 0; j < indices.Length; j += 3)
                        {
                            var newTriangle = new Triangle
                            {
                                Vertices = new List<ModelExtractor.GenericVertex>
                                {
                                    vertices[indices[j]],
                                    vertices[indices[j + 1]],
                                    vertices[indices[j + 2]],
                                },
                                MaterialIndex = newMaterialIndex
                            };
                            Triangles.Add(newTriangle);
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

            //add skylights
            foreach (var skylight in mode.LightgenLights)
                Jms.Skylights.Add(new JmsFormat.JmsSkylight
                {
                    Direction = skylight.Direction,
                    RadiantIntensity = new RealVector3d(skylight.RadiantIntensity.Red, skylight.RadiantIntensity.Green, skylight.RadiantIntensity.Blue),
                    SolidAngle = skylight.Magnitude
                });
        }

        private Vector3 PointToVector(RealPoint3d point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        private class Triangle
        {
            public List<int> VertexIndices = new List<int>();
            public List<ModelExtractor.GenericVertex> Vertices = new List<ModelExtractor.GenericVertex>();
            public Vector3 Normal;
            public int MaterialIndex;
        }

        public static Vector3[] CalculateVertexNormals(Vector3[] vertices, ushort[] indices)
        {
            Vector3[] vertexNormals = new Vector3[vertices.Length];

            // Initialize all vertex normals to zero
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i] = Vector3.Zero;
            }

            // Calculate face normals and add them to vertex normals
            for (int i = 0; i < indices.Length; i += 3)
            {
                int index1 = indices[i];
                int index2 = indices[i + 1];
                int index3 = indices[i + 2];

                Vector3 v1 = vertices[index1];
                Vector3 v2 = vertices[index2];
                Vector3 v3 = vertices[index3];

                Vector3 faceNormal = Vector3.Cross(v2 - v1, v3 - v1);

                vertexNormals[index1] += faceNormal;
                vertexNormals[index2] += faceNormal;
                vertexNormals[index3] += faceNormal;
            }

            // Normalize vertex normals
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i] = Vector3.Normalize(vertexNormals[i]);
            }

            return vertexNormals;
        }

        private Vector3 Vector3fromVector3D(Vector3D input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }
        private Vector3D Vector3DfromVector3(Vector3 input)
        {
            return new Vector3D(input.X, input.Y, input.Z);
        }
    }
}
