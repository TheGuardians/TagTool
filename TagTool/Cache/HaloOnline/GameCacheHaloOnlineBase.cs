using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Resources;

namespace TagTool.Cache
{
    public abstract class GameCacheHaloOnlineBase : GameCache
    {
        public TagCacheHaloOnline TagCacheGenHO;
        public StringTableHaloOnline StringTableHaloOnline;
        public ResourceCachesHaloOnlineBase ResourceCaches;

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

        public T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        public object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance), TagCache.TagDefinitions.GetTagDefinitionType(instance.Group));

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

        public virtual void SaveTagNames(string path = null) => throw new NotImplementedException();

        public virtual void SaveFonts(Stream fontStream) => throw new NotImplementedException();

        public virtual void AddModFile(string path, Stream file) => throw new NotImplementedException();
    }
}
