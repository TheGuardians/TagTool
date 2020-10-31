using System;
using System.Collections.Generic;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache.Gen3;
using TagTool.Cache.Resources;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache
{
    public class GameCacheGen3 : GameCache
    {
        public MapFile BaseMapFile;
        public FileInfo CacheFile;
        public string NetworkKey;
        
        public StringTableGen3 StringTableGen3;
        public TagCacheGen3 TagCacheGen3;
        public ResourceCacheGen3 ResourceCacheGen3;

        public override TagCache TagCache => TagCacheGen3;
        public override StringTable StringTable => StringTableGen3;
        public override ResourceCache ResourceCache => ResourceCacheGen3;
        public override Stream OpenCacheRead() => CacheFile.OpenRead();
        public override Stream OpenCacheReadWrite() => CacheFile.Open(FileMode.Open, FileAccess.ReadWrite);
        public override Stream OpenCacheWrite() => CacheFile.Open(FileMode.Open, FileAccess.Write);

        /// <summary>
        /// Alignment of sections in the cache
        /// </summary>
        public readonly int SectionAlign = 0x1000;

        /// <summary>
        /// Alignment of resource pages in the resource section.
        /// </summary>
        public readonly int PageAlign = 0x800;

        public uint TagAddressToOffset(uint address)
        {
            var headerGen3 = (CacheFileHeaderGen3)BaseMapFile.Header;

            if (Version == CacheVersion.Halo3Beta)
                return address - (headerGen3.VirtualBaseAddress32 - headerGen3.GetTagMemoryHeader().MemoryBufferOffset);

            var baseAddress = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                headerGen3.VirtualBaseAddress64 :
                (ulong)headerGen3.VirtualBaseAddress32;

            var unpackedAddress = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                (((ulong)address << 2) + 0x50000000) :
                (ulong)address;

            return (uint)(unpackedAddress - (baseAddress - (ulong)headerGen3.SectionTable.GetSectionOffset(CacheFileSectionType.TagSection)));
        }

        public Dictionary<string, GameCacheGen3> SharedCacheFiles { get; } = new Dictionary<string, GameCacheGen3>();

        public GameCacheGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Endianness = BaseMapFile.EndianFormat;

            var headerGen3 = (CacheFileHeaderGen3)BaseMapFile.Header;

            DisplayName = mapFile.Header.GetName() + ".map";

            Directory = file.Directory;

            using(var cacheStream = OpenCacheRead())
            using(var reader = new EndianReader(cacheStream, Endianness))
            {
                StringTableGen3 = new StringTableGen3(reader, BaseMapFile);
                TagCacheGen3 = new TagCacheGen3(reader, BaseMapFile, StringTableGen3);
                ResourceCacheGen3 = new ResourceCacheGen3(this);

                if(TagCacheGen3.Instances.Count > 0)
                {
                    if (Version == CacheVersion.Halo3Beta || headerGen3.SectionTable.Sections[(int)CacheFileSectionType.LocalizationSection].Size == 0)
                        LocaleTables = new List<LocaleTable>();
                    else
                    {
                        var globals = Deserialize<Globals>(cacheStream, TagCacheGen3.GlobalInstances["matg"]);
                        LocaleTables = LocalesTableGen3.CreateLocalesTable(reader, BaseMapFile, globals);
                    }
                }
            }

            // unused but kept for future uses
            switch (Version)
            {
                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    NetworkKey = "";
                    break;
                case CacheVersion.HaloReach:
                    NetworkKey = "SneakerNetReigns";
                    break;
            }
        }

        #region Serialization

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance), TagCache.TagDefinitions.GetTagDefinitionType(instance.Group));

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
            Deserialize(new Gen3SerializationContext(stream, this, instance), TagCache.TagDefinitions.GetTagDefinitionType(instance.Group));

        #endregion

        public override void SaveStrings()
        {
            throw new NotImplementedException();
        }

        public void ResizeSection(CacheFileSectionType type, int requestedAdditionalSpace)
        {
            var sectionTable = ((CacheFileHeaderGen3)BaseMapFile.Header).SectionTable;
            var section = sectionTable.Sections[(int)type];

            var sectionFileOffset = sectionTable.GetSectionOffset(type);
            var sectionSize = section.Size;
            var shiftAmount = (requestedAdditionalSpace + SectionAlign - 1) & ~(SectionAlign - 1);
            var sectionNewSize = sectionSize + shiftAmount;


            //
            // Need to update all references to the section in the header and the section table. If it's a tag section, need to update the partitions. 
            // if it's a locale, need to update matg, if it's a resource, need to update play, if it's a string, need to update the string offsets
            // once all the updating is done, fix sections

            section.Size = sectionNewSize;

            for(int i = (int)type + 1; i < (int)CacheFileSectionType.Count; i++)
            {
                sectionTable.SectionAddressToOffsets[i] += shiftAmount;
            }
        }
    }
}