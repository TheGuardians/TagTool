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
using System.Text;
using System.Threading.Tasks;

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
            
            var renderModelName = CacheContext.GetString(Definition.Name);

            if (scene.Meshes.Count != Definition.Regions.Count)
                throw new FormatException($"Invalid mesh count in COLLADA scene: {sceneFile.FullName}");

            var builder = new RenderModelBuilder(CacheContext.Version);

            var materials = new HashSet<short>();

            for (var i = 0; i < scene.MaterialCount; i++)
                materials.Add(builder.AddMaterial(new RenderMaterial { RenderMethod = CacheContext.GetTag(0x3AB0) }));

            var nodes = new HashSet<sbyte>();

            foreach (var node in Definition.Nodes)
                nodes.Add(builder.AddNode(node));

            for (var i = 0; i < scene.MeshCount; i++)
            {
                var mesh = scene.Meshes[i];
                var tokens = mesh.Name.Split(':');

                if (tokens.Length != 2)
                    throw new FormatException($"Mesh name is an invalid format: {mesh.Name}");

                var regionName = CacheContext.GetStringId(tokens[0]);
                if (regionName == StringId.Null)
                    regionName = CacheContext.StringIdCache.AddString(tokens[0]);

                var permutationName = CacheContext.GetStringId(tokens[1]);
                if (permutationName == StringId.Null)
                    permutationName = CacheContext.StringIdCache.AddString(tokens[1]);

                builder.BeginRegion(regionName);
                builder.BeginPermutation(permutationName);
                builder.BeginMesh();

                var meshType = Definition.Geometry.Meshes[i].Type;
                
                var rigidVertices = new List<RigidVertex>();
                var skinnedVertices = new List<SkinnedVertex>();
                
                for (var v = 0; v < mesh.VertexCount; v++)
                {
                    var position = mesh.Vertices[v];
                    var normal = mesh.Normals[v];
                    var uv = mesh.TextureCoordinateChannels[0][v];

                    var tangent = mesh.Tangents.Count != 0 ? mesh.Tangents[v] : new Vector3D();
                    var bitangent = mesh.BiTangents.Count != 0 ? mesh.BiTangents[v] : new Vector3D();

                    var blendIndicesList = new List<byte>();
                    var blendIndices = new byte[4];

                    var blendWeightsList = new List<float>();
                    var blendWeights = new float[4];
                    
                    switch (meshType)
                    {
                        case VertexType.Rigid:
                            rigidVertices.Add(new RigidVertex
                            {
                                Position = new RealQuaternion(position.X, position.Y, position.Z, 1),
                                Texcoord = new RealVector2d(uv.X, -uv.Y),
                                Normal = new RealVector3d(normal.X, normal.Y, normal.Z),
                                Tangent = new RealQuaternion(tangent.X, tangent.Y, tangent.Z, 1),
                                Binormal = new RealVector3d(bitangent.X, bitangent.Y, bitangent.Z)
                            });
                            break;

                        case VertexType.Skinned:
                            foreach (var bone in mesh.Bones)
                            {
                                foreach (var vertexInfo in bone.VertexWeights.Where(vertexWeight => vertexWeight.VertexID == i))
                                {
                                    var boneName = CacheContext.GetStringId(bone.Name);
                                    blendIndicesList.Add((byte)Definition.Nodes.IndexOf(Definition.Nodes.Find(node => node.Name == boneName)));
                                    blendWeightsList.Add(vertexInfo.Weight);
                                }
                            }
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
                            break;

                        default:
                            throw new NotSupportedException(Definition.Geometry.Meshes[i].Type.ToString());
                    }
                }

                var meshIndices = mesh.GetIndices();
                
                builder.BeginPart(materials.ElementAt(mesh.MaterialIndex), 0, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                builder.DefineSubPart(0, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                builder.EndPart();

                switch (meshType)
                {
                    case VertexType.Rigid:
                        builder.BindRigidVertexBuffer(rigidVertices, nodes.ElementAt(Definition.Geometry.Meshes[i].RigidNodeIndex));
                        break;

                    case VertexType.Skinned:
                        builder.BindSkinnedVertexBuffer(skinnedVertices);
                        break;

                    default:
                        throw new NotSupportedException(meshType.ToString());
                }

                builder.BindIndexBuffer(meshIndices.Select(x => (ushort)x), Definition.Geometry.Meshes[i].IndexBufferType);

                builder.EndMesh();
                builder.EndPermutation();
                builder.EndRegion();
            }

            //
            // TODO: Update 'Definition' here...
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