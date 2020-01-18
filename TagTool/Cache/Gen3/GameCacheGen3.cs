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
    public class GameCacheGen3 : GameCache
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

        public Dictionary<string, GameCacheGen3> SharedCacheFiles { get; } = new Dictionary<string, GameCacheGen3>();

        public GameCacheGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Endianness = EndianFormat.BigEndian;
            var interop = mapFile.Header.GetInterop();

            DisplayName = mapFile.Header.GetName() + ".map";

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
        public override Stream OpenCacheReadWrite() => CacheFile.Open(FileMode.Open, FileAccess.ReadWrite);
        public override Stream OpenCacheWrite() => CacheFile.Open(FileMode.Open, FileAccess.Write);

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

        public CachedTagGen3(int index, TagGroup group, string name = null) : base(index, group, name) { }

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
        private readonly GameCacheGen3 GameCache;

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public override CachedTag GetTag(uint ID) => GetTag((int)(ID & 0xFFFF));

        public override CachedTag GetTag(int index)
        {
            if (index > 0 && index < Tags.Count)
                return Tags[index];
            else
                return null;
        }

        public override CachedTag GetTag(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }

        public override CachedTag AllocateTag(TagGroup type, string name = null)
        {
            throw new NotImplementedException();
        }

        public override CachedTag CreateCachedTag(int index, TagGroup group, string name = null)
        {
            return new CachedTagGen3(index, group, name);
        }

        public override CachedTag CreateCachedTag()
        {
            return new CachedTagGen3(-1, TagGroup.None, null);
        }

        public override Stream OpenTagCacheRead() => GameCache.OpenCacheRead();

        public override Stream OpenTagCacheReadWrite() => GameCache.OpenCacheReadWrite();

        public override Stream OpenTagCacheWrite() => GameCache.OpenCacheWrite();

        public TagCacheGen3(GameCacheGen3 cache, EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable, int Magic)
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
                    Tag = new Tag(reader.ReadChars(4)),
                    ParentTag = new Tag(reader.ReadChars(4)),
                    GrandparentTag = new Tag(reader.ReadChars(4)),
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
        public GameCacheGen3 Cache;

        public ResourceCacheGen3(GameCacheGen3 cache, bool load = false)
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
                ResourceGestalt = Cache.Deserialize<CacheFileResourceGestalt>(cacheStream, Cache.TagCache.GetTag("there they are all standing in a row", "zone"));
                ResourceLayoutTable = Cache.Deserialize<CacheFileResourceLayoutTable>(cacheStream, Cache.TagCache.GetTag(0xE1760001));
            }

            isLoaded = true;
        }

        public TagResourceGen3 GetTagResourceFromReference(TagResourceReference resourceReference)
        {
            if (!isLoaded)
                LoadResourceCache();

            if (resourceReference == null)
                return null;

            return ResourceGestalt.TagResources[resourceReference.Gen3ResourceID.Index];
        }

        public bool IsResourceValid(TagResourceGen3 tagResource)
        {
            if (tagResource == null || tagResource.ResourceTypeIndex == -1)
                return false;
            else 
                return true;
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "bitmap_texture_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInteropResource>(resourceReference);
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "bitmap_texture_interleaved_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInterleavedInteropResource>(resourceReference);
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "render_geometry_api_resource_definition")
                return null;
            return GetResourceDefinition<RenderGeometryApiResourceDefinition>(resourceReference);
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource))
                return null;

            byte[] primaryResourceData = GetPrimaryResource(resourceReference.Gen3ResourceID);
            byte[] secondaryResourceData = GetSecondaryResource(resourceReference.Gen3ResourceID);

            if (primaryResourceData == null)
                primaryResourceData = new byte[0];

            if (secondaryResourceData == null)
                secondaryResourceData = new byte[0];

            byte[] data = new byte[primaryResourceData.Length + secondaryResourceData.Length];
            Array.Copy(primaryResourceData, 0, data, 0, primaryResourceData.Length);
            Array.Copy(secondaryResourceData, 0, data, primaryResourceData.Length, secondaryResourceData.Length);

            if (data.Length == 0)
                return null;

            // does not exist in gen3, create one.
            var resourceDef = new SoundResourceDefinition
            {
                Data = new TagData(data)
            };
            return resourceDef;
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "model_animation_tag_resource")
                return null;
            return GetResourceDefinition<ModelAnimationTagResource>(resourceReference);
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "structure_bsp_tag_resources")
                return null;
            // extra step for bsp resources
            if (ResourceLayoutTable.Segments[tagResource.SegmentIndex].RequiredPageIndex == -1)
                return null;
            return GetResourceDefinition<StructureBspTagResources>(resourceReference);
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "structure_bsp_cache_file_tag_resources")
                return null;
            // extra step for bsp resources
            if (ResourceLayoutTable.Segments[tagResource.SegmentIndex].RequiredPageIndex == -1)
                return null;
            return GetResourceDefinition<StructureBspCacheFileTagResources>(resourceReference);
        }


        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
        {
            return null;
        }

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
        {
            return null;
        }

        private string GetResourceTypeName(TagResourceGen3 tagResource)
        {
            return Cache.StringTable.GetString(ResourceGestalt.ResourceTypes[tagResource.ResourceTypeIndex].Name);
        }

        private void ApplyResourceDefinitionFixups(TagResourceGen3 tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.BigEndian))
            {
                for (int i = 0; i < tagResource.ResourceFixups.Count; i++)
                {
                    var fixup = tagResource.ResourceFixups[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        private T GetResourceDefinition<T>(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);

            T result;
            byte[] resourceDefinitionData = new byte[tagResource.DefinitionDataLength];
            Array.Copy(ResourceGestalt.DefinitionData, tagResource.DefinitionDataOffset, resourceDefinitionData, 0, tagResource.DefinitionDataLength);

            ApplyResourceDefinitionFixups(tagResource, resourceDefinitionData);

            byte[] primaryResourceData = GetPrimaryResource(resourceReference.Gen3ResourceID);
            byte[] secondaryResourceData =  GetSecondaryResource(resourceReference.Gen3ResourceID);

            if (primaryResourceData == null)
                primaryResourceData = new byte[0];

            if (secondaryResourceData == null)
                secondaryResourceData = new byte[0];

            using (var definitionDataStream = new MemoryStream(resourceDefinitionData))
            using (var dataStream = new MemoryStream(primaryResourceData))
            using (var secondaryDataStream = new MemoryStream(secondaryResourceData))
            using (var definitionDataReader = new EndianReader(definitionDataStream, EndianFormat.BigEndian))
            using (var dataReader = new EndianReader(dataStream, EndianFormat.BigEndian))
            using (var secondaryDataReader = new EndianReader(secondaryDataStream, EndianFormat.BigEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataReader, secondaryDataReader, definitionDataReader, tagResource.DefinitionAddress.Type);
                var deserializer = new ResourceDeserializer(Cache.Version);
                definitionDataReader.SeekTo(tagResource.DefinitionAddress.Offset);
                result = deserializer.Deserialize<T>(context);
            }
            return result;
        }

        private byte[] ReadSegmentData(TagResourceGen3 resource, int pageIndex, int offset, int sizeIndex)
        {
            var page = ResourceLayoutTable.RawPages[pageIndex];
            var decompressed = ReadPageData(resource, page);

            int length;
            if(sizeIndex != -1)
                length = ResourceLayoutTable.Sizes[sizeIndex].OverallSize;
            else
                length = decompressed.Length - offset;

            var data = new byte[length];
            Array.Copy(decompressed, offset, data, 0, length);
            return data;
        }

        private byte[] GetPrimaryResource(DatumIndex ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.RequiredPageIndex == -1 || segment.RequiredSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.RawPages[segment.RequiredPageIndex].BlockOffset == -1)
                return null;

            return ReadSegmentData(resource, segment.RequiredPageIndex, segment.RequiredSegmentOffset, segment.RequiredSizeIndex);
        }

        private byte[] GetSecondaryResource(DatumIndex ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.OptionalPageIndex == -1 || segment.OptionalSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.RawPages[segment.OptionalPageIndex].BlockOffset == -1)
                return null;

            return ReadSegmentData(resource, segment.OptionalPageIndex, segment.OptionalSegmentOffset, segment.OptionalSizeIndex);
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
                            cache = Cache.SharedCacheFiles[cacheFilePath] = new GameCacheGen3(newMapFile, newCache);
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
