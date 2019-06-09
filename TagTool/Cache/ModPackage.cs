using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache
{
    public class ModPackage
    {
        public const int FormatVersion = 1;

        public TagDeserializer Deserializer { get; private set; }
        public TagSerializer Serializer { get; private set; }

        private CacheVersion _version = CacheVersion.Unknown;
        public CacheVersion Version
        {
            get => _version;
            set
            {
                _version = value;
                Deserializer = new TagDeserializer(value);
                Serializer = new TagSerializer(value);
            }
        }

        public ModPackageHeader Header { get; set; } = new ModPackageHeader();
        public ContentItemMetadata Metadata { get; set; } = new ContentItemMetadata();

        public TagCache Tags { get; set; } = null;
        public MemoryStream TagsStream { get; set; } = new MemoryStream();

        public Dictionary<int, string> TagNames { get; set; } = new Dictionary<int, string>();

        public ResourceCache Resources { get; set; } = null;
        public MemoryStream ResourcesStream { get; set; } = new MemoryStream();

        public List<MemoryStream> CacheStreams { get; set; } = new List<MemoryStream>();

        public ModPackage(FileInfo file = null)
        {
            if (file != null)
                Load(file);
        }

        public void Load(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            if (file.Length < typeof(ModPackageHeader).GetSize())
                throw new FormatException(file.FullName);

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream, leaveOpen: true))
            {
                var dataContext = new DataSerializationContext(reader);

                Header = new TagDeserializer(CacheVersion.Unknown).Deserialize<ModPackageHeader>(dataContext);

                stream.Position = Header.TagCacheOffset + 0x10;
                Version = CacheVersionDetection.DetectFromTimestamp(reader.ReadInt64(), out var _);

                var deserializer = new TagDeserializer(Version);

                Metadata = deserializer.Deserialize<ContentItemMetadata>(dataContext);

                TagsStream = new MemoryStream();
                stream.Position = Header.TagCacheOffset;
                stream.CopyTo(TagsStream, (int)(Header.TagNamesTableOffset - Header.TagCacheOffset));
                TagsStream.Position = 0;

                TagNames = new Dictionary<int, string>();
                stream.Position = Header.TagNamesTableOffset;

                for (var i = 0; i < Header.TagNamesTableCount; i++)
                    TagNames[reader.ReadInt32()] = new string(reader.ReadChars(256)).TrimStart().TrimEnd();

                ResourcesStream = new MemoryStream();
                stream.Position = Header.ResourceCacheOffset;
                stream.CopyTo(ResourcesStream, (int)(Header.MapFileTableOffset - Header.ResourceCacheOffset));
                ResourcesStream.Position = 0;

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

                Tags = new TagCache(TagsStream, TagNames);
                Resources = new ResourceCache(ResourcesStream);
            }
        }

        public void Save(FileInfo file)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var packageStream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var writer = new EndianWriter(packageStream, leaveOpen: true))
            {
                var serializer = new TagSerializer(Version);
                var dataContext = new DataSerializationContext(writer);

                packageStream.SetLength(0);

                //
                // reserve header space
                //

                writer.Write(new byte[64]);

                //
                // write content item metadata
                //

                serializer.Serialize(dataContext, Metadata);

                //
                // write tag cache
                //

                Header.TagCacheOffset = (uint)packageStream.Position;

                TagsStream.Position = 0;
                StreamUtil.Copy(TagsStream, packageStream, (int)TagsStream.Length);
                StreamUtil.Align(packageStream, 4);

                //
                // write tag names table
                //

                var names = new Dictionary<int, string>();

                foreach (var entry in Tags.Index)
                    if (entry != null && entry.Name != null)
                        names[entry.Index] = entry.Name;

                Header.TagNamesTableOffset = (uint)packageStream.Position;
                Header.TagNamesTableCount = names.Count;

                foreach (var entry in names)
                {
                    writer.Write(entry.Key);

                    var chars = new char[256];

                    for (var i = 0; i < entry.Value.Length; i++)
                        chars[i] = entry.Value[i];

                    writer.Write(chars);
                }

                //
                // write resource cache
                //

                Header.ResourceCacheOffset = (uint)packageStream.Position;

                ResourcesStream.Position = 0;
                StreamUtil.Copy(ResourcesStream, packageStream, (int)ResourcesStream.Length);
                StreamUtil.Align(packageStream, 4);

                //
                // write map file table
                //

                Header.MapFileTableOffset = (uint)packageStream.Position;
                Header.MapFileTableCount = CacheStreams.Count;

                var mapFileInfo = new List<(uint, uint)>();
                writer.Write(new byte[8 * Header.MapFileTableCount]);

                foreach (var mapFileStream in CacheStreams)
                {
                    mapFileStream.Position = 0;
                    mapFileInfo.Add(((uint)packageStream.Position, (uint)mapFileStream.Length));
                    StreamUtil.Copy(mapFileStream, packageStream, (int)mapFileStream.Length);
                    StreamUtil.Align(packageStream, 4);
                }

                packageStream.Position = Header.MapFileTableOffset;

                foreach (var entry in mapFileInfo)
                {
                    writer.Write(entry.Item1);
                    writer.Write(entry.Item2);
                }

                //
                // calculate package sha1
                //

                packageStream.Position = 64;
                Header.SHA1 = new SHA1Managed().ComputeHash(packageStream);

                //
                // update package header
                //

                packageStream.Position = 0;
                serializer.Serialize(dataContext, Header);
            }
        }
    }
}