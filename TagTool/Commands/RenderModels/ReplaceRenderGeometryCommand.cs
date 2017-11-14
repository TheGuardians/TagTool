using Assimp;
using BlamCore.Cache;
using BlamCore.Common;
using BlamCore.Geometry;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Commands.RenderModels
{
    class ReplaceRenderGeometryCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public ReplaceRenderGeometryCommand(GameCacheContext cacheContext, CachedTagInstance tag, RenderModel definition) :
            base(CommandFlags.None,

                "ReplaceRenderGeometry",
                "Replaces the render_geometry of the current render_model tag.",
                
                "ReplaceRenderGeometry <COLLADA Scene>",

                "Replaces the render_geometry of the current render_model tag " +
                "with geometry compiled from a COLLADA scene file (.DAE)")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var stringIdCount = CacheContext.StringIdCache.Strings.Count;
            
            var sceneFile = new FileInfo(args[0]);

            if (!sceneFile.Exists)
                throw new FileNotFoundException(sceneFile.FullName);

            if (sceneFile.Extension.ToLower() != ".dae")
                throw new FormatException($"Input file is not COLLADA format: {sceneFile.FullName}");

            Scene scene;

            using (var importer = new AssimpContext())
            {
                scene = importer.ImportFile(sceneFile.FullName,
                    PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.GenerateNormals |
                    PostProcessSteps.SortByPrimitiveType |
                    PostProcessSteps.Triangulate);
            }

            var builder = new RenderModelBuilder(CacheContext.Version);
            var nodes = new Dictionary<string, sbyte>();
            var materialIndices = new Dictionary<string, short>();

            foreach (var node in Definition.Nodes)
                nodes[CacheContext.GetString(node.Name)] = builder.AddNode(node);
            
            foreach (var region in Definition.Regions)
            {
                builder.BeginRegion(region.Name);

                var regionName = CacheContext.GetString(region.Name);

                foreach (var permutation in region.Permutations)
                {
                    if (permutation.MeshCount > 1)
                        throw new NotSupportedException("multiple permutation meshes");

                    var permName = CacheContext.GetString(permutation.Name);
                    var meshName = $"{regionName}_{permName}Mesh";

                    var permMeshes = scene.Meshes.Where(i => i.Name == meshName).ToList();

                    if (permMeshes.Count == 0)
                        throw new Exception($"No mesh(es) found for region '{regionName}' permutation '{permName}'!");

                    permMeshes.Sort((a, b) => a.MaterialIndex.CompareTo(b.MaterialIndex));
                    
                    // Build a multipart mesh from the model data,
                    // with each model mesh mapping to a part of one large mesh and having its own material
                    ushort partStartVertex = 0;
                    ushort partStartIndex = 0;

                    var rigidVertices = new List<RigidVertex>();
                    var skinnedVertices = new List<SkinnedVertex>();

                    var indices = new List<ushort>();

                    var vertexType = Definition.Geometry.Meshes[permutation.MeshIndex].Type;
                    var rigidNode = Definition.Geometry.Meshes[permutation.MeshIndex].RigidNodeIndex;

                    builder.BeginPermutation(permutation.Name);
                    builder.BeginMesh();

                    foreach (var mesh in permMeshes)
                    {
                        for (var i = 0; i < mesh.VertexCount; i++)
                        {
                            var position = mesh.Vertices[i];
                            var normal = mesh.Normals[i];

                            Vector3D uv;

                            try
                            {
                                uv = mesh.TextureCoordinateChannels[0][i];
                            }
                            catch
                            {
                                uv = new Vector3D();
                            }

                            var tangent = mesh.Tangents.Count != 0 ? mesh.Tangents[i] : new Vector3D();
                            var bitangent = mesh.BiTangents.Count != 0 ? mesh.BiTangents[i] : new Vector3D();

                            if (vertexType == VertexType.Skinned)
                            {
                                var blendIndicesList = new List<byte>();
                                var blendWeightsList = new List<float>();

                                foreach (var bone in mesh.Bones)
                                {
                                    foreach (var vertexInfo in bone.VertexWeights)
                                    {
                                        if (vertexInfo.VertexID == i)
                                        {
                                            blendIndicesList.Add((byte)nodes[bone.Name]);
                                            blendWeightsList.Add(vertexInfo.Weight);
                                        }
                                    }
                                }

                                var blendIndices = new byte[4];
                                var blendWeights = new float[4];

                                for (int j = 0; j < blendIndicesList.Count; j++)
                                {
                                    if (j < 4)
                                        blendIndices[j] = blendIndicesList[j];
                                }

                                for (int j = 0; j < blendWeightsList.Count; j++)
                                {
                                    if (j < 4)
                                        blendWeights[j] = blendWeightsList[j];
                                }

                                skinnedVertices.Add(new SkinnedVertex
                                {
                                    Position = new RealQuaternion(position.X * 0.01f, position.Y * 0.01f, position.Z * 0.01f, 1),
                                    Texcoord = new RealVector2d(uv.X, -uv.Y),
                                    Normal = new RealVector3d(normal.X, normal.Y, normal.Z),
                                    Tangent = new RealQuaternion(tangent.X, tangent.Y, tangent.Z, 1),
                                    Binormal = new RealVector3d(bitangent.X, bitangent.Y, bitangent.Z),
                                    BlendIndices = blendIndices,
                                    BlendWeights = blendWeights
                                });
                            }
                            else
                            {
                                rigidVertices.Add(new RigidVertex
                                {
                                    Position = new RealQuaternion(position.X * 0.01f, position.Y * 0.01f, position.Z * 0.01f, 1),
                                    Texcoord = new RealVector2d(uv.X, -uv.Y),
                                    Normal = new RealVector3d(normal.X, normal.Y, normal.Z),
                                    Tangent = new RealQuaternion(tangent.X, tangent.Y, tangent.Z, 1),
                                    Binormal = new RealVector3d(bitangent.X, bitangent.Y, bitangent.Z),
                                });
                            }
                        }

                        // Build the index buffer
                        var meshIndices = mesh.GetIndices();
                        indices.AddRange(meshIndices.Select(i => (ushort)(i + partStartVertex)));

                        // Define a material and part for this mesh
                        var meshMaterial = scene.Materials[mesh.MaterialIndex];

                        short materialIndex = 0;

                        if (materialIndices.ContainsKey(meshMaterial.Name))
                            materialIndex = materialIndices[meshMaterial.Name];
                        else
                            materialIndex = materialIndices[meshMaterial.Name] = builder.AddMaterial(new RenderMaterial
                            {
                                RenderMethod = CacheContext.TagCache.Index[0x3AB0],
                            });

                        builder.BeginPart(materialIndex, partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                        builder.DefineSubPart(partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                        builder.EndPart();

                        // Move to the next part
                        partStartVertex += (ushort)mesh.VertexCount;
                        partStartIndex += (ushort)meshIndices.Length;
                    }
                    
                    // Bind the vertex and index buffers
                    if (vertexType == VertexType.Skinned)
                        builder.BindSkinnedVertexBuffer(skinnedVertices);
                    else
                        builder.BindRigidVertexBuffer(rigidVertices, rigidNode);

                    builder.BindIndexBuffer(indices, IndexBufferFormat.TriangleList);

                    builder.EndMesh();
                    builder.EndPermutation();
                }

                builder.EndRegion();
            }

            using (var resourceStream = new MemoryStream())
            {
                Console.Write("Building render_geometry...");

                var newDefinition = builder.Build(CacheContext.Serializer, resourceStream);
                Definition.Regions = newDefinition.Regions;
                Definition.Geometry = newDefinition.Geometry;
                Definition.Nodes = newDefinition.Nodes;
                Definition.Materials = newDefinition.Materials;
                
                resourceStream.Position = 0;
                CacheContext.AddResource(Definition.Geometry.Resource, ResourceLocation.ResourcesB, resourceStream);

                Console.WriteLine("done.");
            }

            //
            // TODO: Build the new render_model and update the original render_model here...
            //

            Console.Write("Writing render_model tag data...");

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var context = new TagSerializationContext(cacheStream, CacheContext, Tag);
                CacheContext.Serializer.Serialize(context, Definition);
            }

            Console.WriteLine("done.");
            
            if (stringIdCount != CacheContext.StringIdCache.Strings.Count)
            {
                Console.Write("Saving string ids...");

                using (var stream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stream);

                Console.WriteLine("done");
            }

            Console.WriteLine("Replaced render_geometry successfully.");

            return true;
        }
    }
}