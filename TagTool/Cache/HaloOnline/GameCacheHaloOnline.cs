using System;
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

            using (var tagsStream = TagsFile.Open(FileMode.OpenOrCreate))
            {
                FindVersion(new EndianReader(tagsStream));

                using (var stream = StringIdCacheFile.Open(FileMode.OpenOrCreate))
                    StringTableHaloOnline = new StringTableHaloOnline(Version, stream);

                var names = TagCacheHaloOnline.LoadTagNames(TagNamesFile.FullName);
                TagCacheGenHO = new TagCacheHaloOnline(tagsStream, StringTableHaloOnline, names);
            }

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

        private void FindVersion(EndianReader reader)
        {
            reader.SeekTo(0);
            // hackfix until we fix tag cache creation
            if (reader.BaseStream.Length == 0)
            {
                Version = CacheVersion.HaloOnline106708;
                return;
            }
                

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(CacheVersion.HaloOnline106708);

            TagCacheHaloOnlineHeader header = deserializer.Deserialize<TagCacheHaloOnlineHeader>(dataContext);
            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTimestamp(header.CreationTime, out var closestVersion)))
                Version = closestVersion;

            reader.SeekTo(0);
        }

        public override void SaveFonts(Stream stream)
        {
            var fontFilePath = $"{Directory.FullName}\\fonts\\font_package.bin";
            var fontFile = new FileInfo(fontFilePath);
            if (fontFile.Exists)
                fontFile.Delete();
            
            using (var fontFileStream = fontFile.Create())
            {
                stream.CopyTo(fontFileStream);
            }
        }

        public override void AddModFile(string path, Stream file)
        {
            var modFile = new FileInfo(path);
            if (modFile.Exists)
            {
                Console.WriteLine("Overwriting Existing file: " + path);
                modFile.Delete();
            }

            using (var modFileStream = modFile.Create())
            {
                file.Position = 0;
                file.CopyTo(modFileStream);
            }
        }
    }
}
