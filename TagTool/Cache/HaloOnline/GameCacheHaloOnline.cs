using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache.HaloOnline
{
    public class GameCacheHaloOnline : GameCacheHaloOnlineBase
    {
        public FileInfo TagsFile { get; set; }
        public FileInfo TagNamesFile { get; set; }

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

            Endianness = EndianFormat.LittleEndian;

            var names = TagCacheHaloOnline.LoadTagNames(TagNamesFile.FullName);

            using (var stream = TagsFile.OpenRead())
                TagCacheGenHO = new TagCacheHaloOnline(stream, names);

            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTimestamp(TagCacheGenHO.Header.CreationTime, out var closestVersion)))
                Version = closestVersion;

            DisplayName = Version.ToString();
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            StringTableHaloOnline = new StringTableHaloOnline(Version, Directory);
            ResourceCaches = new ResourceCachesHaloOnline(this);
        }

        public override void SaveTagNames()
        {
            TagCacheGenHO.SaveTagNames(TagNamesFile.FullName);
        }
    }
}
