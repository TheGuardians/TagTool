using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Cache
{
    public class GameCacheContextGen3 : GameCache
    {
        public int Magic;
        public MapFile BaseMapFile;
        public FileInfo CacheFile;
        public string NetworkKey;
        
        public StringTableGen3 StringTableGen3;
        public TagCacheGen3 TagCacheGen3;
        public ResourceCacheGen3 ResourceCacheGen3;

        public override TagCacheTest TagCache => TagCacheGen3;
        public override StringTable StringTable => StringTableGen3;
        public override ResourceCacheTest ResourceCache => ResourceCacheGen3; 

        public Dictionary<string, GameCacheContextGen3> SharedCacheFiles { get; } = new Dictionary<string, GameCacheContextGen3>();

        public GameCacheContextGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);

            var interop = mapFile.Header.GetInterop();

            Directory = file.Directory;

            if ( interop != null && interop.ResourceBaseAddress == 0)
                Magic = (int)(interop.RuntimeBaseAddress - mapFile.Header.GetMemoryBufferSize());
            else
            {
                mapFile.Header.ApplyMagic(mapFile.Header.GetStringIDsIndicesOffset() - mapFile.Header.GetHeaderSize(mapFile.Version));
                var resourcePartition = mapFile.Header.GetPartitions()[(int)CacheFilePartitionType.Resources];
                var resourceSection = interop.Sections[(int)CacheFileSectionType.Resource];
                Magic = BitConverter.ToInt32(BitConverter.GetBytes(resourcePartition.BaseAddress), 0) - (interop.DebugSectionSize + resourceSection.Size);
            }

            if (mapFile.Header.GetTagIndexAddress() == 0)
                return;

            mapFile.Header.SetTagIndexAddress(BitConverter.ToUInt32(BitConverter.GetBytes(mapFile.Header.GetTagIndexAddress() - Magic), 0));

            using(var cacheStream = OpenCacheRead())
            using(var reader = new EndianReader(cacheStream, BaseMapFile.EndianFormat))
            {
                StringTableGen3 = new StringTableGen3(reader, BaseMapFile);
                TagCacheGen3 = new TagCacheGen3(this, reader, BaseMapFile, StringTableGen3, Magic);
                LocaleTables = LocalesTableGen3.CreateLocalesTable(reader, BaseMapFile, TagCacheGen3);
                ResourceCacheGen3 = new ResourceCacheGen3(this);
            }

            // unused but kept for future uses
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    NetworkKey = "";
                    break;
                case CacheVersion.HaloReach:
                    NetworkKey = "SneakerNetReigns";
                    break;
                
            }
        }

        //
        // Overrides from abstract class
        //

        public override Stream OpenCacheRead() => CacheFile.OpenRead();
        public override FileStream OpenCacheReadWrite() => CacheFile.Open(FileMode.Open, FileAccess.ReadWrite);
        public override FileStream OpenCacheWrite() => CacheFile.Open(FileMode.Open, FileAccess.Write);

        #region Serialization

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance), TagDefinition.Find(instance.Group.Tag));

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            if (typeof(CachedTagGen3) == instance.GetType())
                Serialize(stream, (CachedTagGen3)instance, definition);
            else
                throw new Exception($"Try to serialize a {instance.GetType()} into a Gen 3 Game Cache");
        }

        //TODO: Implement serialization for gen3
        public void Serialize(Stream stream, CachedTagGen3 instance, object definition)
        {
            throw new NotImplementedException();
        }

        //
        // private methods for internal use
        //

        private T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        private object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);
        
        //
        // public methods specific to gen3
        //

        public T Deserialize<T>(Stream stream, CachedTagGen3 instance) =>
            Deserialize<T>(new Gen3SerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagGen3 instance) =>
            Deserialize(new Gen3SerializationContext(stream, this, instance), TagDefinition.Find(instance.Group.Tag));

        #endregion
    }

    public class CachedTagGen3 : CachedTag
    {
        public uint Offset;
        public int GroupIndex;
        public int Size;

        public override uint DefinitionOffset => Offset;

        public CachedTagGen3() : base() { }

        public CachedTagGen3(int groupIndex, uint id, uint offset, int index, TagGroup tagGroup, string groupName)
        {
            GroupIndex = groupIndex;
            ID = id;
            Offset = offset;
            Index = index;
            Group = tagGroup;
        }
    }

    public class TagTableHeaderGen3
    {
        public int TagGroupsOffset;
        public int TagGroupCount;
        public int TagsOffset;
        public DatumIndex ScenarioHandle;
        public DatumIndex GlobalsHandle;
        public int CRC;
        public int TagCount;
        public int TagInfoHeaderCount;
        public int TagInfoHeaderOffset;
        public int TagInfoHeaderCount2;
        public int TagInfoHeaderOffset2;
    }

    public class TagCacheGen3 : TagCacheTest
    {
        public List<CachedTagGen3> Tags;
        public TagTableHeaderGen3 TagTableHeader;
        public List<TagGroup> TagGroups;
        public string TagsKey = "";
        private readonly GameCacheContextGen3 GameCache;

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public override CachedTag GetTagByID(uint ID) => GetTagByIndex((int)(ID & 0xFFFF));

        public override CachedTag GetTagByIndex(int index)
        {
            if (index > 0 && index < Tags.Count)
                return Tags[index];
            else
                return null;
        }

        public override CachedTag GetTagByName(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }

        public override Stream OpenTagCacheRead() => GameCache.OpenCacheRead();

        public override FileStream OpenTagCacheReadWrite() => GameCache.OpenCacheReadWrite();

        public override FileStream OpenTagCacheWrite() => GameCache.OpenCacheWrite();

        public TagCacheGen3(GameCacheContextGen3 cache, EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable, int Magic)
        {
            Tags = new List<CachedTagGen3>();
            TagGroups = new List<TagGroup>();
            TagTableHeader = baseMapFile.GetTagTableHeader(reader, Magic);
            Version = baseMapFile.Version;
            GameCache = cache;

            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    TagsKey = "";
                    break;
                case CacheVersion.HaloReach:
                    TagsKey =  "LetsAllPlayNice!";
                    break;
            }

            #region Read Class List
            reader.SeekTo(TagTableHeader.TagGroupsOffset);
            for (int i = 0; i < TagTableHeader.TagGroupCount; i++)
            {
                var group = new TagGroup()
                {
                    Tag = new Tag(reader.ReadString(4)),
                    ParentTag = new Tag(reader.ReadString(4)),
                    GrandparentTag = new Tag(reader.ReadString(4)),
                    Name = new StringId(reader.ReadUInt32())
                };
                TagGroups.Add(group);
            }
            #endregion

            #region Read Tags Info
            reader.SeekTo(TagTableHeader.TagsOffset);
            for (int i = 0; i < TagTableHeader.TagCount; i++)
            {
                var groupIndex = reader.ReadInt16();
                var tagGroup = groupIndex == -1 ? new TagGroup() : TagGroups[groupIndex];
                string groupName = groupIndex == -1 ? "" : stringTable.GetString(tagGroup.Name);
                CachedTagGen3 tag = new CachedTagGen3(groupIndex, (uint)((reader.ReadInt16() << 16) | i), (uint)(reader.ReadUInt32() - Magic), i, tagGroup, groupName);
                Tags.Add(tag);
            }
            #endregion

            #region Read Indices
            reader.SeekTo(baseMapFile.Header.GetTagNamesIndicesOffset());
            int[] indices = new int[TagTableHeader.TagCount];
            for (int i = 0; i < TagTableHeader.TagCount; i++)
                indices[i] = reader.ReadInt32();
            #endregion

            #region Read Names
            reader.SeekTo(baseMapFile.Header.GetTagNamesBufferOffset());

            EndianReader newReader = null;

            if (TagsKey == "" || TagsKey == null)
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.GetTagNamesBufferSize())), EndianFormat.BigEndian);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.GetTagNamesBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.GetTagNamesBufferSize(), TagsKey), EndianFormat.BigEndian);
            }

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    Tags[i].Name = null;
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.GetTagNamesBufferSize() - indices[i];
                else
                {
                    if (indices[i + 1] == -1)
                    {
                        int index = -1;

                        for (int j = i + 1; j < indices.Length; j++)
                        {
                            if (indices[j] != -1)
                            {
                                index = j;
                                break;
                            }
                        }

                        length = (index == -1) ? baseMapFile.Header.GetTagNamesBufferSize() - indices[i] : indices[index] - indices[i];
                    }
                    else
                        length = indices[i + 1] - indices[i];
                }

                if (length == 1)
                {
                    Tags[i].Name = "<blank>";
                    continue;
                }

                Tags[i].Name = newReader.ReadString(length);
            }

            newReader.Close();
            newReader.Dispose();
            #endregion

        }
    }

    public class StringTableGen3 : StringTable
    {
        public string StringKey = "";

        public StringTableGen3(EndianReader reader, MapFile baseMapFile) : base()
        {
            Version = baseMapFile.Version;
            
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                    Resolver = new StringIdResolverHalo3();
                    break;

                case CacheVersion.Halo3ODST:
                    Resolver = new StringIdResolverHalo3ODST();
                    break;

                case CacheVersion.HaloReach:
                    Resolver = new StringIdResolverHaloReach();
                    StringKey = "ILikeSafeStrings";
                    break;

                default:
                    throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version));
            }

            reader.SeekTo(baseMapFile.Header.GetStringIDsIndicesOffset());
            int[] indices = new int[baseMapFile.Header.GetStringIDsCount()];
            for (var i = 0; i < baseMapFile.Header.GetStringIDsCount(); i++)
            {
                indices[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(baseMapFile.Header.GetStringIDsBufferOffset());

            EndianReader newReader;

            if (StringKey == "")
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.GetStringIDsBufferSize())), reader.Format);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.GetStringIDsBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.GetStringIDsBufferSize(), StringKey), reader.Format);
            }

            for (var i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    this[i] = "<null>";
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.GetStringIDsBufferSize() - indices[i];
                else
                    length = (indices[i + 1] != -1)
                        ? indices[i + 1] - indices[i]
                        : indices[i + 2] - indices[i];

                if (length == 1)
                {
                    this[i] = "";
                    continue;
                }

                this[i] = newReader.ReadString(length);
            }
            newReader.Close();
            newReader.Dispose();
        }

        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }

    public class LocalesTableGen3
    {
        public static List<LocaleTable> CreateLocalesTable(EndianReader reader, MapFile baseMapFile, TagCacheGen3 tagCache)
        {
            CachedTagGen3 matg = null;
            foreach (var tag in tagCache.Tags)
                if (tag.IsInGroup("matg"))
                {
                    matg = tag;
                    break;
                }
                    
            List < LocaleTable >  localesTable = new List<LocaleTable>();
            string localesKey = "";
            uint localeGlobalsOffset = 0;
            uint localeGlobalsSize = 0;
            uint matgOffset = matg.DefinitionOffset;
            var interop = baseMapFile.Header.GetInterop();

            switch (baseMapFile.Version)
            {
                case CacheVersion.Halo3Retail:
                    localesKey = "";
                    localeGlobalsOffset = 452;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.Halo3ODST:
                    localesKey = "";
                    localeGlobalsOffset = 508;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.HaloReach:
                    localesKey = "BungieHaloReach!";
                    localeGlobalsOffset = 656;
                    localeGlobalsSize = 68;
                    break;
            }

            foreach (var language in Enum.GetValues(typeof(GameLanguage)))
            {
                LocaleTable table = new LocaleTable();

                reader.SeekTo(matgOffset + localeGlobalsOffset + ((int)language * localeGlobalsSize));

                var localeCount = reader.ReadInt32();
                var tableSize = reader.ReadInt32();
                var indexOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);
                var tableOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);

                reader.SeekTo(indexOffset);
                var indices = new int[localeCount];

                for (var i = 0; i < localeCount; i++)
                {
                    table.Add(new CacheLocalizedStringTest(reader.ReadInt32(), "", i));
                    indices[i] = reader.ReadInt32();
                }

                reader.SeekTo(tableOffset);

                EndianReader newReader = null;

                if (localesKey == "")
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = tableOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(tableSize, localesKey));
                }

                for (var i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {
                        table[i].String = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = tableSize - indices[i];
                    else
                        length = (indices[i + 1] != -1)
                            ? indices[i + 1] - indices[i]
                            : indices[i + 2] - indices[i];

                    if (length == 1)
                    {

                        table[i].String = "<blank>";
                        continue;
                    }
                    table[i].String = newReader.ReadString(length);
                }

                newReader.Close();
                newReader.Dispose();

                localesTable.Add(table);
            }

            return localesTable;
        }
    }

    public class ResourceCacheGen3 : ResourceCacheTest
    {
        public bool isLoaded;
        public CacheFileResourceGestalt ResourceGestalt;
        public CacheFileResourceLayoutTable ResourceLayoutTable;
        public GameCacheContextGen3 Cache;

        public ResourceCacheGen3(GameCacheContextGen3 cache, bool load = false)
        {
            isLoaded = false;
            Cache = cache;

            if (load)
                LoadResourceCache();
        }

        public void LoadResourceCache()
        {
            using (var cacheStream = Cache.OpenCacheRead())
            {
                ResourceGestalt = Cache.Deserialize<CacheFileResourceGestalt>(cacheStream, Cache.TagCache.GetTagByName("there they are all standing in a row", "zone"));
                ResourceLayoutTable = Cache.Deserialize<CacheFileResourceLayoutTable>(cacheStream, Cache.TagCache.GetTagByID(0xE1760001));
            }

            isLoaded = true;
        }

        public TagResourceGen3 GetTagResourceFromReference(TagResourceReference resourceReference)
        {
            if (!isLoaded)
                LoadResourceCache();
            return ResourceGestalt.TagResources[resourceReference.Gen3ResourceID.Index];
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "bitmap_texture_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInteropResource>(tagResource);
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "bitmap_texture_interleaved_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInterleavedInteropResource>(tagResource);
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "render_geometry_api_resource_definition")
                return null;
            return GetResourceDefinition<RenderGeometryApiResourceDefinition>(tagResource);
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "model_animation_tag_resource")
                return null;
            return GetResourceDefinition<ModelAnimationTagResource>(tagResource);
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "structure_bsp_tag_resources")
                return null;
            return GetResourceDefinition<StructureBspTagResources>(tagResource);
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (GetResourceTypeName(tagResource) != "structure_bsp_cache_file_tag_resources")
                return null;
            return GetResourceDefinition<StructureBspCacheFileTagResources>(tagResource);
        }

        public override byte[] GetResourceData(TagResourceReference resourceReference)
        {
            if (!isLoaded)
                LoadResourceCache();

            var tagResource = GetTagResourceFromReference(resourceReference);

            int primarySize = 0;
            int secondarySize = 0;

            switch (GetResourceTypeName(tagResource))
            {
                case "bitmap_texture_interop_resource":
                    var bitmapDefinition = GetBitmapTextureInteropResource(resourceReference);
                    primarySize = bitmapDefinition.Texture.Definition.PrimaryResourceData.Size;
                    secondarySize = bitmapDefinition.Texture.Definition.SecondaryResourceData.Size;
                    break;

                case "bitmap_texture_interleaved_interop_resource":
                    var interleavedBitmapDefinition = GetBitmapTextureInterleavedInteropResource(resourceReference);
                    primarySize = interleavedBitmapDefinition.Texture.Definition.PrimaryResourceData.Size;
                    secondarySize = interleavedBitmapDefinition.Texture.Definition.SecondaryResourceData.Size;
                    break;

                case "sound_resource_definition":
                    var segment = ResourceLayoutTable.Segments[tagResource.SegmentIndex];
                    primarySize = segment.RequiredSizeIndex != -1 ? ResourceLayoutTable.Sizes[segment.RequiredSizeIndex].OverallSize : 0;
                    secondarySize = segment.OptionalSizeIndex != -1 ? ResourceLayoutTable.Sizes[segment.OptionalSizeIndex].OverallSize : 0;
                    break;

                case "model_animation_tag_resource":
                    var animationDefinition = GetModelAnimationTagResource(resourceReference);
                    foreach (var group in animationDefinition.GroupMembers)
                        primarySize += group.AnimationData.Size;
                    break;

                case "render_geometry_api_resource_definition":
                    var geometryDefinition = GetRenderGeometryApiResourceDefinition(resourceReference);
                    foreach(var vertexBuffer in geometryDefinition.VertexBuffers)
                        primarySize += vertexBuffer.Definition.Data.Size;
                    foreach (var indexBuffer in geometryDefinition.IndexBuffers)
                        primarySize += indexBuffer.Definition.Data.Size;
                    break;

                case "structure_bsp_tag_resources":
                case "structure_bsp_cache_file_tag_resources":
                    Console.WriteLine($"Tag resource {GetResourceTypeName(tagResource)} does not contain data in pages.");
                    return null;

                default:
                    throw new Exception($"Unsupported Resource Type {GetResourceTypeName(tagResource)}");

            }
            return GetResourceData(resourceReference, primarySize, secondarySize);
        }

        private string GetResourceTypeName(TagResourceGen3 tagResource)
        {
            return Cache.StringTable.GetString(ResourceGestalt.ResourceTypes[tagResource.ResourceTypeIndex].Name);
        }

        private void ApplyResourceFixups(TagResourceGen3 tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.BigEndian))
            {
                for (int i = 0; i < tagResource.ResourceFixups.Count; i++)
                {
                    var fixup = tagResource.ResourceFixups[i];
                    // apply fixup to the resource definition (it sets the offsets for the stuctures and resource data)
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Offset);
                }
            }
        }

        private T GetResourceDefinition<T>(TagResourceGen3 tagResource)
        {
            T result;
            byte[] resourceDefinitionData = new byte[tagResource.FixupInformationLength];
            Array.Copy(ResourceGestalt.FixupInformation, tagResource.FixupInformationOffset, resourceDefinitionData, 0, tagResource.FixupInformationLength);
            ApplyResourceFixups(tagResource, resourceDefinitionData);
            using (var fixupStream = new MemoryStream(resourceDefinitionData))
            using (var fixupReader = new EndianReader(fixupStream, EndianFormat.BigEndian))
            {
                var context = new DataSerializationContext(fixupReader);
                var deserializer = new TagDeserializer(Cache.Version);
                fixupReader.SeekTo(tagResource.DefinitionAddress.Offset);
                result = deserializer.Deserialize<T>(context);
            }

            return result;
        }

        private byte[] GetResourceData(TagResourceReference resourceReference, int PrimarySize, int SecondarySize)
        {
            
            byte[] result = new byte[PrimarySize + SecondarySize];

            if(PrimarySize != 0)
            {
                var primaryData = GetPrimaryResource(resourceReference.Gen3ResourceID, PrimarySize);
                if (primaryData != null)
                    Array.Copy(primaryData, 0, result, 0, PrimarySize);
            }
            if(SecondarySize != 0)
            {
                var secondaryData = GetSecondaryResource(resourceReference.Gen3ResourceID, SecondarySize);
                if (secondaryData != null)
                    Array.Copy(secondaryData, 0, result, PrimarySize, SecondarySize);
            }

            return result;
        }

        private byte[] ReadSegmentData(TagResourceGen3 resource, int pageIndex, int offset, int length, bool padding=true)
        {
            var page = ResourceLayoutTable.RawPages[pageIndex];
            var decompressed = ReadPageData(resource, page);

            var data = new byte[length];

            if (length > decompressed.Length || (length + offset) > decompressed.Length)
            {
                if (padding)
                {
                    length = decompressed.Length;
                    if (length + offset > decompressed.Length)
                        length = decompressed.Length - offset;
                }
                else
                    return null;
            }

            Array.Copy(decompressed, offset, data, 0, (int)length);

            return data;
        }

        private byte[] GetPrimaryResource(DatumIndex ID, int dataLength, bool padding = true)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.RequiredPageIndex == -1 || segment.RequiredSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.RawPages[segment.RequiredPageIndex].BlockOffset == -1)
                return null;

            return ReadSegmentData(resource, segment.RequiredPageIndex, segment.RequiredSegmentOffset, dataLength, padding);
        }

        private byte[] GetSecondaryResource(DatumIndex ID, int dataLength, bool padding = true)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.OptionalPageIndex == -1 || segment.OptionalSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.RawPages[segment.OptionalPageIndex].BlockOffset == -1)
                return null;

            return ReadSegmentData(resource, segment.OptionalPageIndex, segment.OptionalSegmentOffset, dataLength, padding);
        }

        private byte[] ReadPageData(TagResourceGen3 resource, RawPage page)
        {
            string cacheFilePath;

            var cache = Cache;

            if (page.SharedCacheIndex != -1)
            {
                cacheFilePath = ResourceLayoutTable.ExternalCacheReferences[page.SharedCacheIndex].MapPath;
                var pathComponent = cacheFilePath.Split('\\');
                cacheFilePath = pathComponent[pathComponent.Length - 1];
                cacheFilePath = Path.Combine(Cache.Directory.FullName, cacheFilePath);

                if (cacheFilePath != Cache.CacheFile.FullName)
                {
                    if (Cache.SharedCacheFiles.ContainsKey(cacheFilePath))
                        cache = Cache.SharedCacheFiles[cacheFilePath];
                    else
                    {
                        var newCache = new FileInfo(cacheFilePath);
                        using(var newCacheStream = newCache.OpenRead())
                        using(var newCacheReader = new EndianReader(newCacheStream))
                        {
                            var newMapFile = new MapFile();
                            newMapFile.Read(newCacheReader);
                            cache = Cache.SharedCacheFiles[cacheFilePath] = new GameCacheContextGen3(newMapFile, newCache);
                        }
                    }  
                }
            }

            var decompressed = new byte[page.UncompressedBlockSize];

            using (var cacheStream = cache.OpenCacheRead())
            using(var reader = new EndianReader(cacheStream, EndianFormat.BigEndian))
            {
                reader.SeekTo(cache.BaseMapFile.Header.GetInterop().DebugSectionSize + page.BlockOffset);
                var compressed = reader.ReadBytes(BitConverter.ToInt32(BitConverter.GetBytes(page.CompressedBlockSize), 0));

                if (resource.ResourceTypeIndex != -1 && GetResourceTypeName(resource) == "sound_resource_definition")
                    return compressed;

                if (page.CompressionCodecIndex == -1)
                    reader.BaseStream.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));
                else
                    using (var readerDeflate = new DeflateStream(new MemoryStream(compressed), CompressionMode.Decompress))
                        readerDeflate.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));

               
            }
            return decompressed;

        }

    }
}
