using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;

namespace TagTool.Cache
{
    public abstract class GameCacheHaloOnlineBase : GameCache
    {
        public TagCacheHaloOnline TagCacheGenHO;
        public StringTableHaloOnline StringTableHaloOnline;
        public ResourceCachesHaloOnline ResourceCaches;

        public List<int> ModifiedTags = new List<int>();

        public override TagCache TagCache => TagCacheGenHO;
        public override StringTable StringTable => StringTableHaloOnline;
        public override ResourceCache ResourceCache => ResourceCaches;

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

        public TagCacheHaloOnline CreateTagCache(DirectoryInfo directory, out FileInfo file)
        {
            if (directory == null)
                directory = Directory;

            if (!directory.Exists)
                directory.Create();

            file = new FileInfo(Path.Combine(directory.FullName, "tags.dat"));

            TagCacheHaloOnline cache = null;

            using (var stream = file.Create())
                cache = CreateTagCache(stream);

            return cache;
        }

        public TagCacheHaloOnline CreateTagCache(Stream stream)
        {
            TagCacheHaloOnlineHeader header = new TagCacheHaloOnlineHeader
            {
                TagTableOffset = 0x20,
                CreationTime = 0x01D0631BCC791704
            };

            stream.Position = 0;
            var writer = new EndianWriter(stream, EndianFormat.LittleEndian);
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
            serializer.Serialize(dataContext, header);
            stream.Position = 0;

            return new TagCacheHaloOnline(stream, new Dictionary<int, string>());
        }

        public virtual void SaveTagNames() => throw new NotImplementedException();
    }
}
