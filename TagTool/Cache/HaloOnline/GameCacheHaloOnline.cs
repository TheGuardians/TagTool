using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;

namespace TagTool.Cache
{
    public class GameCacheHaloOnline : GameCache
    {
        public TagCacheHaloOnline TagCacheGenHO;
        public StringTableHaloOnline StringTableHaloOnline;
        public ResourceCachesHaloOnline ResourceCaches;

        public List<int> ModifiedTags = new List<int>();

        public override TagCache TagCache => TagCacheGenHO;
        public override StringTable StringTable => StringTableHaloOnline;
        public override ResourceCache ResourceCache => ResourceCaches;
        public override Stream OpenCacheRead() => TagCache.OpenTagCacheRead();
        public override Stream OpenCacheReadWrite() => TagCache.OpenTagCacheReadWrite();
        public override Stream OpenCacheWrite() => TagCache.OpenTagCacheWrite();

        public GameCacheHaloOnline(DirectoryInfo directory)
        {
            Directory = directory;
            Endianness = EndianFormat.LittleEndian;

            TagCacheGenHO = new TagCacheHaloOnline(directory);

            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTimestamp(TagCacheGenHO.Header.CreationTime, out var closestVersion)))
                Version = closestVersion;

            DisplayName = Version.ToString();
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            StringTableHaloOnline = new StringTableHaloOnline(Version, Directory);
            ResourceCaches = new ResourceCachesHaloOnline(this);
        }

        #region Serialization Methods

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            if (typeof(CachedTagHaloOnline) == instance.GetType())
                Serialize(stream, (CachedTagHaloOnline)instance, definition);
            else
                throw new Exception($"Try to serialize a {instance.GetType()} into an Halo Online Game Cache");
            
        }

        public void Serialize(Stream stream, CachedTagHaloOnline instance, object definition)
        {
            if (!ModifiedTags.Contains(instance.Index))
                SignalModifiedTag(instance.Index);

            Serializer.Serialize(new HaloOnlineSerializationContext(stream, this, instance), definition);
        }

        private T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        private object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance), TagDefinition.Find(instance.Group.Tag));

        public T Deserialize<T>(Stream stream, CachedTagHaloOnline instance) =>
            Deserialize<T>(new HaloOnlineSerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagHaloOnline instance) =>
            Deserialize(new HaloOnlineSerializationContext(stream, this, instance), TagDefinition.Find(instance.Group.Tag));

        #endregion

        public void SignalModifiedTag(int index) { ModifiedTags.Add(index); }

        public void SaveModifiedTagNames(string path = null)
        {
            var csvFile = new FileInfo(path ?? Path.Combine(Directory.FullName, "modified_tags.csv"));

            if (!csvFile.Directory.Exists)
                csvFile.Directory.Create();

            using (var csvWriter = new StreamWriter(csvFile.Create()))
            {
                foreach (var instance in ModifiedTags)
                {
                    var tag = TagCacheGenHO.Tags[instance];
                    string name;
                    if (tag.Name == null)
                        name = $"0x{tag.Index:X8}";
                    else
                        name = tag.Name;

                    csvWriter.WriteLine($"{name}.{tag.Group.ToString()}");
                }
            }
        }
    }
}
