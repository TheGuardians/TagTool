using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache
{
    public class ModPackage
    {
        public const int FormatVersion = 1;

        public FileInfo File { get; set; }
        public CacheVersion Version { get; set; }

        public ModPackageHeader Header { get; set; }
        public ContentItemMetadata Metadata { get; set; }

        public TagCache Tags { get; set; }
        public MemoryStream TagStream { get; set; }

        public Dictionary<int, string> TagNames { get; set; }

        public ResourceCache Resources { get; set; }
        public MemoryStream ResourceStream { get; set; }

        public List<MemoryStream> CacheStreams { get; set; }

        public ModPackage(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            if (file.Length < typeof(ModPackageHeader).GetSize())
                throw new FormatException(file.FullName);

            File = file;

            using (var stream = File.OpenRead())
            using (var reader = new EndianReader(stream))
            {
                var dataContext = new DataSerializationContext(reader);

                Header = new TagDeserializer(CacheVersion.Unknown).Deserialize<ModPackageHeader>(dataContext);

                stream.Position = Header.TagCacheOffset + 0x10;
                Version = CacheVersionDetection.DetectFromTimestamp(reader.ReadInt64(), out var _);

                var deserializer = new TagDeserializer(Version);

                Metadata = deserializer.Deserialize<ContentItemMetadata>(dataContext);

                TagStream = new MemoryStream();
                stream.Position = Header.TagCacheOffset;
                stream.CopyTo(TagStream, (int)(Header.TagNamesTableOffset - Header.TagCacheOffset));
                TagStream.Position = 0;

                TagNames = new Dictionary<int, string>();
                stream.Position = Header.TagNamesTableOffset;

                for (var i = 0; i < Header.TagNamesTableCount; i++)
                    TagNames[reader.ReadInt32()] = new string(reader.ReadChars(256)).TrimStart().TrimEnd();

                ResourceStream = new MemoryStream();
                stream.Position = Header.ResourceCacheOffset;
                stream.CopyTo(ResourceStream, (int)(Header.MapFileTableOffset - Header.ResourceCacheOffset));
                ResourceStream.Position = 0;

                CacheStreams = new List<MemoryStream>();
                stream.Position = Header.MapFileTableOffset;

                var cacheInfo = new List<(uint, uint)>();

                for (var i = 0; i < Header.MapFileTableCount; i++)
                    cacheInfo.Add((reader.ReadUInt32(), reader.ReadUInt32()));

                foreach (var info in cacheInfo)
                {
                    var cacheStream = new MemoryStream();

                    stream.Position = info.Item1;
                    stream.CopyTo(cacheStream, (int)info.Item2);
                    cacheStream.Position = 0;

                    CacheStreams.Add(cacheStream);
                }

                Tags = new TagCache(TagStream, TagNames);
                Resources = new ResourceCache(ResourceStream);
            }
        }
    }
}