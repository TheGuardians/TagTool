using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assimp;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderModels
{
    class RenderModelTestCommand : Command
    {
        private GameCache Cache { get; }

        public RenderModelTestCommand(GameCache cache)
            : base(true,

                  "RenderModelTest",
                  "A test command for 'mode' tag resources.",

                  "RenderModelTest [location = resources] [tag index = 0x3317] <model file>",

                  "A test command for 'mode' tag resources.\n" +
                  "The model must only have a single material and no nodes.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var stringIdCount = Cache.StringTable.Count;
            var destinationTag = Cache.TagCache.GetTag(@"objects\gear\human\industrial\street_cone\street_cone", "mode");

            string vertexType = "rigid";
            if (args[0] == "skinned" || args[0] == "rigid")
            {
                vertexType = args[0];
                args.RemoveAt(0);
            }

            if (args.Count == 2)
            {
                if (!Cache.TagCache.TryGetTag(args[0], out destinationTag) || !destinationTag.IsInGroup("mode"))
                    return new TagToolError(CommandError.TagInvalid);

                args.RemoveAt(0);
            }

            RenderModel edMode = null;

            using (var cacheStream = Cache.OpenCacheReadWrite())
                edMode = Cache.Deserialize<RenderModel>(cacheStream, destinationTag);

            // Get a list of the original nodes
            var nodeIndices = new Dictionary<string, int>();

            foreach (var a in edMode.Nodes)
                nodeIndices.Add(Cache.StringTable.GetString(a.Name), edMode.Nodes.IndexOf(a));

            // Read the custom model file
            if (!File.Exists(args[0]))
                return new TagToolError(CommandError.FileNotFound);

            Console.WriteLine($"File date: {File.GetLastWriteTime(args[0])}");

            var builder = new RenderModelBuilder(Cache);

            using (var importer = new AssimpContext())
            {
                Scene model;

                if (vertexType == "skinned")
                {
                    using (var logStream = new LogStream((msg, userData) => Console.WriteLine(msg)))
                    {
                        logStream.Attach();
                        model = importer.ImportFile(args[0],
                            PostProcessSteps.CalculateTangentSpace |
                            PostProcessSteps.GenerateNormals |
                            PostProcessSteps.SortByPrimitiveType |
                            PostProcessSteps.Triangulate);
                        logStream.Detach();
                    }

                    for (var i = 0; i < model.Meshes.Count; i++)
                        if (!model.Meshes[i].HasBones)
                            return new TagToolError(CommandError.CustomError, $"Mesh \"{model.Meshes[i].Name}\" has no bones!");
                }
                else
                {
                    using (var logStream = new LogStream((msg, userData) => Console.WriteLine(msg)))
                    {
                        logStream.Attach();
                        model = importer.ImportFile(args[0],
                            PostProcessSteps.CalculateTangentSpace |
                            PostProcessSteps.GenerateNormals |
                            PostProcessSteps.JoinIdenticalVertices |
                            PostProcessSteps.SortByPrimitiveType |
                            PostProcessSteps.PreTransformVertices |
                            PostProcessSteps.Triangulate);
                        logStream.Detach();
                    }
                }

                Console.WriteLine("Assembling vertices...");

                // Add nodes
                var rigidNode = builder.AddNode(new RenderModel.Node
                {
                    Name = Cache.StringTable.GetStringId("street_cone"),
                    ParentNode = -1,
                    FirstChildNode = -1,
                    NextSiblingNode = -1,
                    DefaultRotation = new RealQuaternion(0, 0, 0, -1),
                    DefaultScale = 1,
                    InverseForward = new RealVector3d(1, 0, 0),
                    InverseLeft = new RealVector3d(0, 1, 0),
                    InverseUp = new RealVector3d(0, 0, 1),
                });

                // Build a multipart mesh from the model data,
                // with each model mesh mapping to a part of one large mesh and having its own material
                ushort partStartVertex = 0;
                ushort partStartIndex = 0;

                var rigidVertices = new List<RigidVertex>();
                var skinnedVertices = new List<SkinnedVertex>();

                var indices = new List<ushort>();

                Dictionary<string, int> newNodes = new Dictionary<string, int>();
                foreach (var mesh in model.Meshes)
                {
                    var meshIndex = model.Meshes.IndexOf(mesh);

                    Console.Write($"Enter a region name for '{mesh.Name}' (mesh index {meshIndex}): ");
                    var regionName = Console.ReadLine();
                    var regionStringId = Cache.StringTable.GetStringId(regionName);

                    if (regionStringId == StringId.Invalid)
                        regionStringId = Cache.StringTable.AddString(regionName);

                    // Begin building the default region and permutation
                    builder.BeginRegion(regionStringId);
                    builder.BeginPermutation(Cache.StringTable.GetStringId("default"));
                    builder.BeginMesh();

                    for (var i = 0; i < mesh.VertexCount; i++)
                    {
                        var position = mesh.Vertices[i];
                        var normal = mesh.Normals[i];
                        var uv = mesh.TextureCoordinateChannels[0][i];

                        var tangent = mesh.Tangents.Count != 0 ? mesh.Tangents[i] : new Vector3D();
                        var bitangent = mesh.BiTangents.Count != 0 ? mesh.BiTangents[i] : new Vector3D();

                        if (vertexType == "skinned")
                        {
                            var blendIndicesList = new List<byte>();
                            var blendWeightsList = new List<float>();
                            var bonesList = new List<string>();

                            foreach (var bone in mesh.Bones)
                            {
                                foreach (var vertexInfo in bone.VertexWeights)
                                {
                                    if (vertexInfo.VertexID == i)
                                    {
                                        bonesList.Add(bone.Name);
                                        blendIndicesList.Add((byte)nodeIndices[bone.Name]);
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
                                Position = new RealQuaternion(position.X, position.Y, position.Z, 1),
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
                                Position = new RealQuaternion(position.X, position.Y, position.Z, 1),
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
                    var material = builder.AddMaterial(new RenderMaterial
                    {
                        RenderMethod = Cache.TagCache.GetTag(@"shaders\invalid", "rmsh"),
                    });

                    builder.BeginPart(material, partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                    builder.DefineSubPart(partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                    builder.EndPart();

                    // Move to the next part
                    partStartVertex += (ushort)mesh.VertexCount;
                    partStartIndex += (ushort)meshIndices.Length;

                    // Bind the vertex and index buffers
                    if (vertexType == "skinned")
                        builder.BindSkinnedVertexBuffer(skinnedVertices);
                    else
                        builder.BindRigidVertexBuffer(rigidVertices, rigidNode);

                    builder.BindIndexBuffer(indices, IndexBufferFormat.TriangleList);
                    builder.EndMesh();
                    builder.EndPermutation();
                    builder.EndRegion();
                }
            }

            Console.Write("Building render_geometry...");

            var resourceStream = new MemoryStream();
            var renderModel = builder.Build(Cache.Serializer, resourceStream);

            if (vertexType == "skinned")
            {
                // Copy required data from the original render_model tag
                renderModel.Nodes = edMode.Nodes;
                renderModel.MarkerGroups = edMode.MarkerGroups;
                renderModel.RuntimeNodeOrientations = edMode.RuntimeNodeOrientations;
            }

            Console.WriteLine("done.");


            //
            // Serialize the new render_model tag
            //

            Console.Write("Writing render_model tag data...");

            using (var cacheStream = Cache.OpenCacheReadWrite())
                Cache.Serialize(cacheStream, destinationTag, renderModel);

            Console.WriteLine("done.");

            //
            // Save any new string ids
            //

            if (stringIdCount != Cache.StringTable.Count)
            {
                Console.Write("Saving string ids...");


                Cache.SaveStrings();

                Console.WriteLine("done");
            }

            Console.WriteLine("Model imported successfully!");

            return true;
        }
    }
}