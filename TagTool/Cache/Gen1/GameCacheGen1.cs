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

        public override object Deserialize(Stream stream, CachedTag instance)
        {
            throw new NotImplementedException();
        }

        public override T Deserialize<T>(Stream stream, CachedTag instance)
        {
            throw new NotImplementedException();
        }

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

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            throw new NotImplementedException();
        }
    }

    public class StringTableGen1 : StringTable
    {
        public StringTableGen1()
        {
            
        }
        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }
    }

}
