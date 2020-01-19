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
    public class GameCacheModPackage : GameCache
    {
        public FileInfo ModPackageFile;
        public ModPackage BaseModPackage;
        public StringTableHaloOnline ModPackageStringTable;
        public TagCacheHaloOnline ModPackageTagCache;
        public ResourceCachesModPackage ModPackageResourceCache;

        /// <summary>
        /// Tag cache index in the list of tag caches.
        /// </summary>
        private int CurrentTagCacheIndex = 0;

        public override StringTable StringTable => ModPackageStringTable;

        public override TagCache TagCache => ModPackageTagCache;

        public override ResourceCache ResourceCache => ModPackageResourceCache;



        public GameCacheModPackage(FileInfo file)
        {
            Version = CacheVersion.HaloOnline106708;
            Endianness = EndianFormat.LittleEndian;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Directory = file.Directory;

            // load mod package
            BaseModPackage = new ModPackage(file);
            DisplayName = BaseModPackage.Metadata.Name + ".pak";

            ModPackageResourceCache = new ResourceCachesModPackage(BaseModPackage);
            ModPackageTagCache = new TagCacheHaloOnline(BaseModPackage.TagCachesStreams[0], BaseModPackage.TagCacheNames[0]);
            ModPackageStringTable = BaseModPackage.StringTable;
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
            throw new NotImplementedException();
        }

        public override T Deserialize<T>(Stream stream, CachedTag instance)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            throw new NotImplementedException();
        }

        public override Stream OpenCacheRead() => ModPackageFile.OpenRead();

        public override Stream OpenCacheReadWrite() => ModPackageFile.Open(FileMode.Open, FileAccess.ReadWrite);

        public override Stream OpenCacheWrite() => ModPackageFile.Open(FileMode.Open, FileAccess.Write);

        public override void SaveStrings()
        {
            throw new NotImplementedException();
        }
    }

}
