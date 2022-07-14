using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.Monolithic;
using TagTool.IO;
using System.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Tags;
using TagTool.Common;
using TagTool.Geometry;

namespace TagTool.Commands.Tags
{
    class ImportLooseTagCommand : Command
    {
        GameCacheHaloOnlineBase Cache { get; set; }
        public ImportLooseTagCommand(GameCacheHaloOnlineBase cache) : base(
            false,

            "ImportLooseTag",
            "Import a loose tag from Halo 3 MCC",

            "ImportLooseTag <Tag> <Path>",

            "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var path = args[1];

            if (!File.Exists(path))
                return new TagToolError(CommandError.FileNotFound, $"\"{path}\"");

            byte[] tagData;
            using (var inStream = File.OpenRead(path))
            {
                tagData = new byte[inStream.Length];
                inStream.Read(tagData, 0, tagData.Length);
            }

            var singleFileTagReader = new SingleTagFileReader(new PersistChunkReader(new MemoryStream(tagData), Cache.Endianness));

            // read the layout
            var layout = singleFileTagReader.ReadLayout(Cache.Endianness);

            // read and fixup the data
            var FixupContext = new HaloOnlinePersistContext(Cache);
            var newTagData = singleFileTagReader.ReadAndFixupData(0, layout, FixupContext, out uint mainStructOffset);

            var newTagDataReader = new EndianReader(new MemoryStream(newTagData), Cache.Endianness);
            newTagDataReader.SeekTo(mainStructOffset);

            var deserializer = new TagDeserializer(CacheVersion.Halo3Retail, CachePlatform.MCC);

            var definitions = new Cache.Gen3.TagDefinitionsGen3();
            Type looseTagType = null;
            foreach (KeyValuePair<TagGroup, Type> tagType in definitions.Gen3Types)
            {
                if (tagType.Key.Tag == singleFileTagReader.Header.GroupTag)
                {
                    looseTagType = tagType.Value;
                    break;
                }
            }

            if (looseTagType == null)
                return new TagToolError(CommandError.OperationFailed, $"Tag type {singleFileTagReader.Header.GroupTag.ToString()} not valid for gen3 cache!");

            string tagname = args[0];
            if (!tagname.Contains("."))
                tagname += $".{singleFileTagReader.Header.GroupTag}";

            if (Cache.TagCache.TryGetCachedTag(tagname, out var instance))
                return new TagToolError(CommandError.OperationFailed, "Tag name already exists in cache!");

            var info = TagStructure.GetTagStructureInfo(looseTagType, CacheVersion.Halo3Retail, CachePlatform.MCC);

            if (info.TotalSize != layout.RootStruct.Size)
                return new TagToolError(CommandError.OperationFailed, $"Loose tag struct size ({layout.RootStruct.Size}) did not match known definition size ({info.TotalSize}). Check game version.");

            DataSerializationContext context = new DataSerializationContext(newTagDataReader);
            var result = deserializer.DeserializeStruct(newTagDataReader, context, info);

            //fixup tags and resources
            FixupTag(result, FixupContext, layout, looseTagType);

            var destTag = Cache.TagCache.AllocateTag(looseTagType, args[0].ToString());
            using (var stream = Cache.OpenCacheReadWrite())
                Cache.Serialize(stream, destTag, result);
            Cache.SaveStrings();
            Cache.SaveTagNames();

            Console.WriteLine($"['{destTag.Group.Tag}', 0x{destTag.Index:X4}] {destTag}");

            return true;
        }

        private void FixupTag(object tagDef, HaloOnlinePersistContext FixupContext, TagLayout layout, Type looseTagType)
        {         
            switch (looseTagType.Name)
            {
                case "ModelAnimationGraph":
                    //jmad resources
                    for (var i = 0; i < FixupContext.TagResourceData.Count; i++)
                    {
                        MemoryStream dataStream = new MemoryStream(FixupContext.TagResourceData[i]);
                        var chunkReader = new PersistChunkReader(dataStream, Cache.Endianness);
                        var dataReader = new SingleTagFileDataReader(0, layout, FixupContext);
                        uint offset = dataReader.ReadResource(chunkReader, out MemoryStream outStream, layout.ResourceDefinitions[0]);
                        var newResourceReader = new EndianReader(outStream, Cache.Endianness);
                        newResourceReader.SeekTo(offset);
                        DataSerializationContext resourceContext = new DataSerializationContext(newResourceReader);
                        var deserializer = new TagDeserializer(CacheVersion.Halo3Retail, CachePlatform.MCC);
                        var jmadInfo = TagStructure.GetTagStructureInfo(typeof(ModelAnimationTagResource), CacheVersion.Halo3Retail, CachePlatform.MCC);
                        ModelAnimationTagResource animationResource = (ModelAnimationTagResource)deserializer.DeserializeStruct(newResourceReader, resourceContext, jmadInfo);
                        ModelAnimationGraph jmad = (ModelAnimationGraph)tagDef;
                        animationResource.GroupMembers.AddressType = CacheAddressType.Definition;
                        jmad.ResourceGroups[i].ResourceReference = Cache.ResourceCache.CreateModelAnimationGraphResource(animationResource);
                    }                 
                    break;
                case "Bitmap":
                    //bitmap resource
                    Bitmap bitm = (Bitmap)tagDef;
                    BitmapTextureInteropResource bitmResource = new BitmapTextureInteropResource
                    {
                        Texture = new D3DStructure<BitmapTextureInteropResource.BitmapDefinition>
                        {
                            Definition = new BitmapTextureInteropResource.BitmapDefinition
                            {
                                PrimaryResourceData = new TagData
                                {
                                    Data = bitm.ProcessedPixelData
                                }
                            },
                            AddressType = CacheAddressType.Definition
                        }
                    };
                    bitm.HardwareTextures.Add(Cache.ResourceCache.CreateBitmapResource(bitmResource));
                    bitm.ProcessedPixelData = null;
                    break;
                case "RenderModel":
                    //render model resource
                    RenderModel mode = (RenderModel)tagDef;
                    RenderGeometryApiResourceDefinition modeResource = new RenderGeometryApiResourceDefinition
                    {
                        VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                        IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                    };
                    //fixup resource data fields
                    for(var meshindex = 0; meshindex < mode.Geometry.Meshes.Count; meshindex++)
                    {
                        var mesh = mode.Geometry.Meshes[meshindex];
                        //no support for prt data right now
                        mesh.PrtType = PrtSHType.None;
                        var tagResource = mode.Geometry.GeometryTagResources[meshindex];
                        mesh.VertexBufferIndices = new short[8] { (short)meshindex, -1, -1, -1, -1, -1, -1, -1 };
                        mesh.IndexBufferIndices = new short[2] { (short)meshindex, -1 };

                        //vertex buffers
                        var vertStream = new MemoryStream();
                        var vertexWriter = VertexStreamFactory.Create(Cache.Version, Cache.Platform, vertStream);
                        switch (mesh.Type)
                        {
                            case VertexType.Rigid:
                                foreach (var vert in tagResource.RawVertices)
                                    vertexWriter.WriteRigidVertex(new RigidVertex
                                    {
                                        Position = new RealQuaternion(point_to_vector(vert.Position), 1.0f),
                                        Normal = point_to_vector(vert.Normal),
                                        Texcoord = point_to_vector(vert.Texcoord),
                                        Tangent = new RealQuaternion(point_to_vector(vert.Tangent), 1.0f),
                                        Binormal = point_to_vector(vert.Binormal)
                                    });
                                modeResource.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition>
                                {
                                    Definition = new VertexBufferDefinition
                                    {
                                        Format = VertexBufferFormat.Rigid,
                                        VertexSize = 0x38,
                                        Count = tagResource.RawVertices.Count,
                                        Data = new TagData
                                        {
                                            Data = vertStream.ToArray()
                                        }
                                    }
                                });
                                break;
                            case VertexType.Skinned:
                                foreach (var vert in tagResource.RawVertices)
                                    vertexWriter.WriteSkinnedVertex(new SkinnedVertex
                                    {
                                        Position = new RealQuaternion(point_to_vector(vert.Position), 1.0f),
                                        Normal = point_to_vector(vert.Normal),
                                        Texcoord = point_to_vector(vert.Texcoord),
                                        Tangent = new RealQuaternion(point_to_vector(vert.Tangent), 1.0f),
                                        Binormal = point_to_vector(vert.Binormal),
                                        BlendIndices = vert.NodeIndices,
                                        BlendWeights = vert.NodeWeights
                                    });
                                modeResource.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition>
                                {
                                    Definition = new VertexBufferDefinition
                                    {
                                        Format = VertexBufferFormat.Skinned,
                                        VertexSize = 0x40,
                                        Count = tagResource.RawVertices.Count,
                                        Data = new TagData
                                        {
                                            Data = vertStream.ToArray()
                                        }
                                    }
                                });
                                break;
                            default:
                                new TagToolError(CommandError.OperationFailed, $"Render model vertex buffer type '{mesh.Type}' unsupported!");
                                break;
                        }

                        //index buffers
                        using (var indexStream = new MemoryStream())
                        using (var indexWriter = new EndianWriter(indexStream))
                        {
                            foreach (var indexValue in tagResource.RawIndices)
                                indexWriter.Write(indexValue.Word);
                            modeResource.IndexBuffers.Add(new D3DStructure<IndexBufferDefinition>
                            {
                                Definition = new IndexBufferDefinition
                                {
                                    Data = new TagData
                                    {
                                        Data = indexStream.ToArray()
                                    }
                                }
                            });
                        }
                        switch (mesh.IndexBufferType)
                        {
                            case PrimitiveType.TriangleList:
                                modeResource.IndexBuffers[meshindex].Definition.Format = IndexBufferFormat.TriangleList;
                                break;
                            case PrimitiveType.TriangleStrip:
                                modeResource.IndexBuffers[meshindex].Definition.Format = IndexBufferFormat.TriangleStrip;
                                break;
                            default:
                                new TagToolError(CommandError.OperationFailed, $"Render model index buffer type '{mesh.IndexBufferType}' unsupported!");
                                break;
                        }
                        
                    }
                    mode.Geometry.Resource = Cache.ResourceCache.CreateRenderGeometryApiResource(modeResource);
                    mode.Geometry.GeometryTagResources = null;
                    break;
                case "PhysicsModel":
                    PhysicsModel phmo = (PhysicsModel)tagDef;
                    if (phmo.MassDistributions.Count != phmo.RigidBodies.Count &&
                        phmo.MassDistributions.Count > 0 && phmo.RigidBodies.Count > 0)
                    {
                        new TagToolWarning("Physics model mass distributions count != rigid bodies count!");
                        break;
                    }                      
                    for(var massIndex = 0; massIndex < phmo.MassDistributions.Count; massIndex++)
                    {
                        phmo.RigidBodies[massIndex].CenterOfMass = phmo.MassDistributions[massIndex].CenterOfMass;
                        phmo.RigidBodies[massIndex].CenterOfMassRadius = phmo.MassDistributions[massIndex].HavokWCenterOfMass;
                        phmo.RigidBodies[massIndex].InertiaTensorX = phmo.MassDistributions[massIndex].InertiaTensorI;
                        phmo.RigidBodies[massIndex].InertiaTensorXRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorI;
                        phmo.RigidBodies[massIndex].InertiaTensorY = phmo.MassDistributions[massIndex].InertiaTensorJ;
                        phmo.RigidBodies[massIndex].InertiaTensorYRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorJ;
                        phmo.RigidBodies[massIndex].InertiaTensorZ = phmo.MassDistributions[massIndex].InertiaTensorK;
                        phmo.RigidBodies[massIndex].InertiaTensorZRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorK;
                    }
                    break;
                default:
                    if(layout.ResourceDefinitions.Length > 0)
                        new TagToolWarning($"'{layout.ResourceDefinitions[0].Name}' import not yet supported!");
                    break;
            }

        }

        public RealVector3d point_to_vector(RealPoint3d point)
        {
            return new RealVector3d(point.X, point.Y, point.Z);
        }

        public RealVector2d point_to_vector(RealPoint2d point)
        {
            return new RealVector2d(point.X, point.Y);
        }

        public class HaloOnlinePersistContext : ISingleTagFilePersistContext
        {
            public GameCache Cache;
            public List<byte[]> TagResourceData = new List<byte[]>();
            public HaloOnlinePersistContext(GameCache cache)
            {
                Cache = cache;
            }

            public void AddTagResource(DatumHandle resourceHandle, TagResourceXSyncState state)
            {

            }

            public void AddTagResourceData(byte[] data)
            {
                TagResourceData.Add(data);
            }

            public StringId AddStringId(string stringvalue)
            {
                var stringId = Cache.StringTable.GetStringId(stringvalue);
                if (stringId == StringId.Invalid)
                    stringId = Cache.StringTable.AddString(stringvalue);
                return stringId;
            }

            public CachedTag GetTag(Tag groupTag, string name)
            {
                if (Cache.TagCache.TryGetCachedTag($"{name}.{groupTag}", out CachedTag tag))
                    return tag;
                return null;
            }
        }
    }
}