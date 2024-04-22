using System;
using System.Collections.Generic;
using System.Linq;
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
using static TagTool.Tags.Definitions.PhysicsModel;
using TagTool.Commands.ModelAnimationGraphs;

namespace TagTool.Commands.Tags
{
    public class ImportLooseTagCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; set; }
        private EndianFormat TagEndianness { get; set; }
        private CacheVersion TagCache { get; set; }
        private CachePlatform TagPlatform { get; set; }

        public ImportLooseTagCommand(GameCacheHaloOnlineBase cache) : base(
            false,

            "ImportLooseTag",
            "Import a loose tag from Halo 3 MCC",

            "ImportLooseTag [Flags] [Destination Tag] <Source Path>",

            "")
        {
            Cache = cache;
            TagEndianness = EndianFormat.LittleEndian;
            TagCache = CacheVersion.Halo3Retail;
            TagPlatform = CachePlatform.MCC;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var path = args.LastOrDefault();
            var newTagName = "";
            var flags = "";

            if (!File.Exists(path))
                return new TagToolError(CommandError.FileNotFound, $"\"{path}\"");

            switch (args.Count)
            {
                case 1:
                    {
                        var split = path.Split(new string[] { "\\tags\\" }, StringSplitOptions.None).LastOrDefault();
                        if(split != path)
                            newTagName = split.Split('.').FirstOrDefault();
                        break;
                    }
                case 2:
                    newTagName = args[0];
                    break;
                case 3:
                    newTagName = args[1];
                    flags = args[0];
                    break;
            }

            byte[] tagData;
            using (var inStream = File.OpenRead(path))
            {
                tagData = new byte[inStream.Length];
                inStream.Read(tagData, 0, tagData.Length);
            }

            var singleFileTagReader = new SingleTagFileReader(new PersistChunkReader(new MemoryStream(tagData), TagEndianness));

            // read the layout
            var layout = singleFileTagReader.ReadLayout(TagEndianness);

            // read and fixup the data
            var FixupContext = new HaloOnlinePersistContext(Cache);
            var newTagData = singleFileTagReader.ReadAndFixupData(0, layout, FixupContext, out uint mainStructOffset);

            var newTagDataReader = new EndianReader(new MemoryStream(newTagData), TagEndianness);
            newTagDataReader.SeekTo(mainStructOffset);

            var deserializer = new TagDeserializer(TagCache, TagPlatform);

            Dictionary<TagGroup, Type> TagTypes;
            if (TagCache >= CacheVersion.Halo4)
                TagTypes = new Cache.Gen4.TagDefinitionsGen4().Gen4Types;
            else
                TagTypes = new Cache.Gen3.TagDefinitionsGen3().Gen3Types;

            Type looseTagType = null;
            foreach (KeyValuePair<TagGroup, Type> tagType in TagTypes)
            {
                if (tagType.Key.Tag == singleFileTagReader.Header.GroupTag)
                {
                    looseTagType = tagType.Value;
                    break;
                }
            }

            if (looseTagType == null)
                return new TagToolError(CommandError.OperationFailed, $"Tag type {singleFileTagReader.Header.GroupTag.ToString()} not valid for gen3 cache!");

            string tagname = newTagName;
            if (!tagname.Contains("."))
                tagname += $".{singleFileTagReader.Header.GroupTag}";

            if (Cache.TagCache.TryGetCachedTag(tagname, out var instance))
                return new TagToolError(CommandError.OperationFailed, "Tag name already exists in cache!");

            var info = TagStructure.GetTagStructureInfo(looseTagType, TagCache, TagPlatform);

            if (info.TotalSize != layout.RootStruct.Size)
                return new TagToolError(CommandError.OperationFailed, $"Loose tag struct size (0x{layout.RootStruct.Size:X4}) did not match known definition size (0x{info.TotalSize:X4}). Check game version.");

            DataSerializationContext context = new DataSerializationContext(newTagDataReader);
            var result = deserializer.DeserializeStruct(newTagDataReader, context, info);

            //fixup tags and resources
            FixupTag(result, FixupContext, layout, looseTagType);

            var destTag = Cache.TagCache.AllocateTag(looseTagType, newTagName.ToString());
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
                        var deserializer = new TagDeserializer(TagCache, TagPlatform);
                        var jmadInfo = TagStructure.GetTagStructureInfo(typeof(ModelAnimationTagResource), TagCache, TagPlatform);
                        ModelAnimationTagResource animationResource = (ModelAnimationTagResource)deserializer.DeserializeStruct(newResourceReader, resourceContext, jmadInfo);
                        ModelAnimationGraph jmad = (ModelAnimationGraph)tagDef;
                        animationResource.GroupMembers.AddressType = CacheAddressType.Definition;
                        jmad.ResourceGroups[i].ResourceReference = Cache.ResourceCache.CreateModelAnimationGraphResource(animationResource);
                    }                 
                    new SortModesCommand(Cache, (ModelAnimationGraph)tagDef).Execute(new List<string> { });
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
                    //fix phantom types
                    foreach(var phantomtype in phmo.PhantomTypes)
                    {
                        Enum.TryParse(phantomtype.Flags.Halo3Retail.ToString(), out phantomtype.Flags.Halo3ODST);
                    }
                    //add phantom shapes
                    SetPhantomShapeFromRigidBody(phmo);
                    //fix mass distributions
                    foreach(var rigidbody in phmo.RigidBodies)
                    {
                        Shape phmoShape = GetPhysicsShape(phmo, rigidbody.ShapeType, rigidbody.ShapeIndex);
                        if(phmoShape != null)
                        {
                            int massIndex = phmoShape.MassDistributionIndex;
                            if(massIndex != -1)
                            {
                                rigidbody.CenterOfMass = phmo.MassDistributions[massIndex].CenterOfMass;
                                rigidbody.CenterOfMassRadius = phmo.MassDistributions[massIndex].HavokWCenterOfMass;
                                rigidbody.InertiaTensorX = phmo.MassDistributions[massIndex].InertiaTensorI * rigidbody.Mass;
                                rigidbody.InertiaTensorXRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorI;
                                rigidbody.InertiaTensorY = phmo.MassDistributions[massIndex].InertiaTensorJ * rigidbody.Mass;
                                rigidbody.InertiaTensorYRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorJ;
                                rigidbody.InertiaTensorZ = phmo.MassDistributions[massIndex].InertiaTensorK * rigidbody.Mass;
                                rigidbody.InertiaTensorZRadius = phmo.MassDistributions[massIndex].HavokWInertiaTensorK;
                            }
                        }
                    }
                    break;
                default:
                    if(layout.ResourceDefinitions.Length > 0)
                        new TagToolWarning($"'{layout.ResourceDefinitions[0].Name}' import not yet supported!");
                    break;
            }

        }

        private void SetPhantomShapeFromRigidBody(PhysicsModel phmo)
        {
            phmo.Phantoms = new List<Phantom>();
            Shape shape = null;
            foreach(var rigidBody in phmo.RigidBodies)
            {
                shape = GetPhysicsShape(phmo, rigidBody.ShapeType, rigidBody.ShapeIndex);
                if (shape != null && shape.MaterialIndex != -1)
                {
                    Material material = phmo.Materials[shape.MaterialIndex];
                    if(material.PhantomType != -1)
                    {
                        shape.PhantomIndex = (sbyte)phmo.Phantoms.Count;
                        phmo.Phantoms.Add(new Phantom
                        {
                            ShapeType = rigidBody.ShapeType,
                            ShapeIndex = rigidBody.ShapeIndex
                        });
                    }
                }
            }

        }

        private Shape GetPhysicsShape(PhysicsModel phmo, Havok.BlamShapeType shapeType, int shapeIndex)
        {
            switch (shapeType)
            {
                case Havok.BlamShapeType.Sphere:
                    return shapeIndex == -1 ? null : phmo.Spheres[shapeIndex];
                case Havok.BlamShapeType.Pill:
                    return shapeIndex == -1 ? null : phmo.Pills[shapeIndex];
                case Havok.BlamShapeType.Box:
                    return shapeIndex == -1 ? null : phmo.Boxes[shapeIndex];
                case Havok.BlamShapeType.Polyhedron:
                    return shapeIndex == -1 ? null : phmo.Polyhedra[shapeIndex];
                default:
                    return null;
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
                if (!TagGroupValidForCache(groupTag, Cache.Version, Cache.Platform))
                    return null;
                if (Cache.TagCache.TryGetCachedTag($"{name}.{groupTag}", out CachedTag tag))
                    return tag;
                return null;
            }
            public bool TagGroupValidForCache(Tag group, CacheVersion cache, CachePlatform platform)
            {
                var definitions = new Cache.Gen3.TagDefinitionsGen3();
                var type = definitions.GetTagDefinitionType(group);
                if (type == null)
                    return false;
                var attribute = TagStructure.GetTagStructureAttribute(type, cache, platform);
                if (attribute == null)
                    return false;
                return true;
            }
        }
    }
}
