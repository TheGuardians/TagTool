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
            DisplayName = BaseModPackage.Metadata.Name + ".pak";

            ResourceCaches = new ResourceCachesModPackage(BaseModPackage);
            TagCacheGenHO = new TagCacheHaloOnline(BaseModPackage.TagCachesStreams[0], BaseModPackage.TagCacheNames[0]);
            StringTableHaloOnline = BaseModPackage.StringTable;
        }

        public GameCacheModPackage(ModPackage modPackage, FileInfo file)
        {
            BaseModPackage = modPackage;
            Directory = file.Directory;
            DisplayName = BaseModPackage.Metadata.Name + ".pak";
            Version = CacheVersion.HaloOnline106708;
            Endianness = EndianFormat.LittleEndian;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
        }

        public override object Deserialize(Stream stream, CachedTag instance)
        {
            var definitionType = TagDefinition.Find(instance.Group);
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

        // soon
        public override Stream OpenCacheRead() => BaseModPackage.TagCachesStreams[0];

        public override Stream OpenCacheReadWrite() => BaseModPackage.TagCachesStreams[0];

        public override Stream OpenCacheWrite() => BaseModPackage.TagCachesStreams[0];

        public override void SaveStrings() { }
    }
}

