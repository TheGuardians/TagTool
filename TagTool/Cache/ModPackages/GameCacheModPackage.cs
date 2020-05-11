using LZ4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.ModPackages;

namespace TagTool.Cache
{
    public class GameCacheModPackage : GameCacheHaloOnlineBase
    {
        public FileInfo ModPackageFile;
        public ModPackage BaseModPackage;

        private int CurrentTagCacheIndex = 0;

        public GameCacheModPackage(FileInfo file)
        {
            ModPackageFile = file;
            Version = CacheVersion.HaloOnline106708;
            Endianness = EndianFormat.LittleEndian;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Directory = file.Directory;

            // load mod package
            BaseModPackage = new ModPackage(file);

            ResourceCaches = new ResourceCachesModPackage(BaseModPackage);
            StringTableHaloOnline = BaseModPackage.StringTable;
            SetActiveTagCache(0);
        }

        public override object Deserialize(Stream stream, CachedTag instance)
        {
            var definitionType = TagCache.TagDefinitions.GetTagDefinitionType(instance.Group);
            var context = new ModPackageTagSerializationContext(stream, this, BaseModPackage, (CachedTagHaloOnline)instance);
            return Deserializer.Deserialize(context, definitionType);
        }

        public override T Deserialize<T>(Stream stream, CachedTag instance)
        {
            return Deserializer.Deserialize<T>(CreateTagSerializationContext(stream, instance));
        }

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            Serializer.Serialize(CreateTagSerializationContext(stream, instance), definition);
        }

        private ISerializationContext CreateTagSerializationContext(Stream stream, CachedTag instance)
        {
            return new ModPackageTagSerializationContext(stream, this, BaseModPackage, (CachedTagHaloOnline)instance);
        }

        public override Stream OpenCacheRead() => BaseModPackage.TagCachesStreams[CurrentTagCacheIndex];

        public override Stream OpenCacheReadWrite() => BaseModPackage.TagCachesStreams[CurrentTagCacheIndex];

        public override Stream OpenCacheWrite() => BaseModPackage.TagCachesStreams[CurrentTagCacheIndex];

        public override void SaveStrings() { }

        private int GetTagCacheCount() => BaseModPackage.TagCaches.Count;

        public void SetActiveTagCache(int index)
        {
            if( index >= 0 && index < GetTagCacheCount())
            {
                CurrentTagCacheIndex = index;
                TagCacheGenHO = new TagCacheHaloOnline(BaseModPackage.TagCachesStreams[CurrentTagCacheIndex], StringTableHaloOnline, BaseModPackage.TagCacheNames[CurrentTagCacheIndex]);
                if(GetTagCacheCount() > 1)
                    DisplayName = BaseModPackage.Metadata.Name + $"_tag_cache_{CurrentTagCacheIndex}_" + ".pak";
                else
                    DisplayName = BaseModPackage.Metadata.Name + ".pak";
            }
            else
            {
                Console.WriteLine($"Invalid tag cache index {index}, {GetTagCacheCount()} tag cache available");
            }
        }
    }
}

