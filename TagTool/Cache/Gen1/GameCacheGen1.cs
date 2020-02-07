using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.BlamFile;
using TagTool.Cache.Gen1;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public class GameCacheGen1 : GameCache
    {
        public MapFile BaseMapFile;
        public FileInfo CacheFile;

        public TagCacheGen1 TagCacheGen1;

        public override TagCache TagCache => TagCacheGen1;
        // Gen1 caches don't have stringids
        public override StringTable StringTable => null;
        public override ResourceCache ResourceCache => throw new NotImplementedException();

        public GameCacheGen1(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            CacheFile = file;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Endianness = BaseMapFile.EndianFormat;
            DisplayName = mapFile.Header.HaloName + ".map";
            Directory = file.Directory;

            using (var cacheStream = OpenCacheRead())
            using (var reader = new EndianReader(cacheStream, Endianness))
            {
                TagCacheGen1 = new TagCacheGen1(reader, mapFile);
            }
        }

        #region Serialization

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new Gen1SerializationContext(stream, this, (CachedTagGen1)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new Gen1SerializationContext(stream, this, (CachedTagGen1)instance), TagDefinition.Find(instance.Group.Tag));

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            if (typeof(CachedTagGen1) == instance.GetType())
                Serialize(stream, (CachedTagGen1)instance, definition);
            else
                throw new Exception($"Try to serialize a {instance.GetType()} into a Gen 3 Game Cache");
        }

        //TODO: Implement serialization for gen3
        public void Serialize(Stream stream, CachedTagGen1 instance, object definition)
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

        public T Deserialize<T>(Stream stream, CachedTagGen1 instance) =>
            Deserialize<T>(new Gen1SerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagGen1 instance) =>
            Deserialize(new Gen1SerializationContext(stream, this, instance), TagDefinition.Find(instance.Group.Tag));

        #endregion


        public override Stream OpenCacheRead() => CacheFile.OpenRead();

        public override Stream OpenCacheReadWrite()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenCacheWrite()
        {
            throw new NotImplementedException();
        }

        public override void SaveStrings()
        {
            throw new NotImplementedException();
        }

    }

}
