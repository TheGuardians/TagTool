using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Cache.Gen3;
using TagTool.BlamFile;
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

        public uint TagAddressToOffset(uint address) => address - (BaseMapFile.Header.TagBaseAddress - BaseMapFile.Header.SectionTable.GetSectionOffset(CacheFileSectionType.TagSection)); 

        public Dictionary<string, GameCacheGen3> SharedCacheFiles { get; } = new Dictionary<string, GameCacheGen3>();

        public GameCacheGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Endianness = BaseMapFile.EndianFormat;
            var interop = mapFile.Header.SectionTable;

            DisplayName = mapFile.Header.Name + ".map";

            Directory = file.Directory;

            using(var cacheStream = OpenCacheRead())
            using(var reader = new EndianReader(cacheStream, Endianness))
            {
                StringTableGen3 = new StringTableGen3(reader, BaseMapFile);
                TagCacheGen3 = new TagCacheGen3(reader, BaseMapFile, StringTableGen3);
                ResourceCacheGen3 = new ResourceCacheGen3(this);

                if(TagCacheGen3.Tags.Count > 0)
                {
                    if (BaseMapFile.Header.SectionTable.Sections[(int)CacheFileSectionType.LocalizationSection].Size == 0)
                        LocaleTables = new List<LocaleTable>();
                    else
                    {
                        var globals = Deserialize<Globals>(cacheStream, TagCacheGen3.HardcodedTags["matg"]);
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

        public override void SaveStrings()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
