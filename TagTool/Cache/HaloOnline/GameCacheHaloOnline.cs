using System.IO;
using TagTool.Cache.Resources;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache.HaloOnline
{
    public class GameCacheHaloOnline : GameCacheHaloOnlineBase
    {
        public FileInfo TagsFile { get; set; }
        public FileInfo TagNamesFile { get; set; }
        public FileInfo StringIdCacheFile { get; set; }

        public override TagCache TagCache => TagCacheGenHO;
        public override StringTable StringTable => StringTableHaloOnline;
        public override ResourceCache ResourceCache => ResourceCaches;

        public override Stream OpenCacheRead() => TagsFile.OpenRead();

        public override Stream OpenCacheReadWrite() => TagsFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);

        public override Stream OpenCacheWrite() => TagsFile.OpenWrite();     

        public GameCacheHaloOnline(DirectoryInfo directory)
        {
            Directory = directory;
            TagsFile = new FileInfo(Path.Combine(directory.FullName, "tags.dat"));
            TagNamesFile = new FileInfo(Path.Combine(directory.FullName, "tag_list.csv"));
            StringIdCacheFile = new FileInfo(Path.Combine(directory.FullName, "string_ids.dat"));

            Endianness = EndianFormat.LittleEndian;

            var names = TagCacheHaloOnline.LoadTagNames(TagNamesFile.FullName);

            using (var stream = TagsFile.OpenRead())
                TagCacheGenHO = new TagCacheHaloOnline(stream, names);

            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTimestamp(TagCacheGenHO.Header.CreationTime, out var closestVersion)))
                Version = closestVersion;

            using (var stream = StringIdCacheFile.OpenRead())
                StringTableHaloOnline = new StringTableHaloOnline(Version, stream);

            DisplayName = Version.ToString();
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
          
            ResourceCaches = new ResourceCachesHaloOnline(this);
        }

        public override void SaveTagNames(string path = null)
        {
            TagCacheGenHO.SaveTagNames(path ?? TagNamesFile.FullName);
        }

        public override void SaveStrings()
        {
            using (var stream = StringIdCacheFile.OpenWrite())
                StringTableHaloOnline.Save(stream);
        }
    }
}
