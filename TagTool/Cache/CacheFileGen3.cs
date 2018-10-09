using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Cache
{
    public class CacheFileGen3 : CacheFile
    {
        public override string LocalesKey
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        return "";

                    case CacheVersion.HaloReach:
                        return "BungieHaloReach!";

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override string StringsKey
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        return "";

                    case CacheVersion.HaloReach:
                        return "ILikeSafeStrings";

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override string TagsKey
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        return "";

                    case CacheVersion.HaloReach:
                        return "LetsAllPlayNice!";

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override string NetworkKey
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        return "";

                    case CacheVersion.HaloReach:
                        return "SneakerNetReigns";

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override string StringMods
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                        return "+262143,-259153;+64329,-64329;+1208,+1882";

                    case CacheVersion.Halo3ODST:
                        return "+258846,-258846;+64231,-64231;+1304,+2098";

                    case CacheVersion.HaloReach:
                        return "+1174139,-1174139;+129874,-129874;+1123,+4604";

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override int LocaleGlobalsOffset
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                        return 452;

                    case CacheVersion.Halo3ODST:
                        return 508;

                    case CacheVersion.HaloReach:
                        return 656;

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public override int LocaleGlobalsSize
        {
            get
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                    case CacheVersion.HaloReach:
                        return 68;

                    default:
                        throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
                }
            }
        }

        public CacheFileGen3(HaloOnlineCacheContext cacheContext, FileInfo file, CacheVersion version, bool memory)
            : base(cacheContext, file, version, memory)
        {
            if (Header.Interop.ResourceBaseAddress == 0)
            {
                Magic = (int)(Header.Interop.RuntimeBaseAddress - Header.MemoryBufferSize);
            }
            else
            {
                Header.Magic = Header.StringIDsIndicesOffset - 0x3000;

                Header.TagNamesBufferOffset -= Header.Magic;
                Header.TagNamesIndicesOffset -= Header.Magic;
                Header.StringIDsIndicesOffset -= Header.Magic;
                Header.StringIDsBufferOffset -= Header.Magic;

                var resourcePartition = Header.Partitions[(int)CacheFilePartitionType.Resources];
                var resourceSection = Header.Interop.Sections[(int)CacheFileSectionType.Resource];
                Magic = BitConverter.ToInt32(BitConverter.GetBytes(resourcePartition.BaseAddress), 0) - (Header.Interop.DebugSectionSize + resourceSection.Size);
            }

            if (Header.TagIndexAddress == 0)
                return;

            Header.TagIndexAddress = BitConverter.ToUInt32(BitConverter.GetBytes(Header.TagIndexAddress - Magic), 0);

            IndexHeader = new CacheIndexHeader(this);
            IndexItems = new IndexTable(this);
            Strings = new StringTable(this);
            LocaleTables = new List<LocaleTable>();

            switch (version)
            {
                case CacheVersion.Halo3Retail:
                    Resolver = new StringIdResolverHalo3();
                    break;

                case CacheVersion.Halo3ODST:
                    Resolver = new StringIdResolverHalo3ODST();
                    break;

                case CacheVersion.HaloReach:
                    Resolver = new StringIdResolverHaloReach();
                    break;

                default:
                    throw new NotSupportedException(CacheVersionDetection.GetBuildName(version));
            }

            foreach (var language in Enum.GetValues(typeof(GameLanguage)))
                LocaleTables.Add(new LocaleTable(this, (GameLanguage)language));
        }

        new public class CacheIndexHeader : CacheFile.CacheIndexHeader
        {
            public CacheIndexHeader(CacheFile cache)
            {
                cache.Reader.SeekTo(cache.Header.TagIndexAddress);

                TagGroupCount = cache.Reader.ReadInt32();
                TagGroupsOffset = cache.Reader.ReadInt32() - cache.Magic;
                TagCount = cache.Reader.ReadInt32();
                TagsOffset = cache.Reader.ReadInt32() - cache.Magic;
                TagInfoHeaderCount = cache.Reader.ReadInt32();
                TagInfoHeaderOffset = cache.Reader.ReadInt32() - cache.Magic;
                TagInfoHeaderCount2 = cache.Reader.ReadInt32();
                TagInfoHeaderOffset2 = cache.Reader.ReadInt32() - cache.Magic;
            }
        }

        new public class IndexTable : CacheFile.IndexTable
        {
            public IndexTable(CacheFile cache)
            {
                var indexHeader = cache.IndexHeader;
                var cacheHeader = cache.Header;
                var reader = cache.Reader;

                ClassList = new List<TagClass>();

                #region Read Class List
                reader.SeekTo(indexHeader.TagGroupsOffset);
                for (int i = 0; i < indexHeader.TagGroupCount; i++)
                {
                    var tc = new TagClass()
                    {
                        ClassCode = reader.ReadString(4),
                        Parent = reader.ReadString(4),
                        Parent2 = reader.ReadString(4),
                        StringID = reader.ReadInt32()
                    };
                    ClassList.Add(tc);
                }
                #endregion

                #region Read Tags Info
                reader.SeekTo(indexHeader.TagsOffset);
                for (int i = 0; i < indexHeader.TagCount; i++)
                {
                    IndexItem item = new IndexItem() { Cache = cache };
                    item.ClassIndex = reader.ReadInt16();
                    item.ID = (reader.ReadInt16() << 16) | i;
                    item.Offset = reader.ReadInt32() - cache.Magic;
                    item.Index = i;
                    Add(item);
                }
                #endregion

                #region Read Indices
                reader.SeekTo(cacheHeader.TagNamesIndicesOffset);
                int[] indices = new int[indexHeader.TagCount];
                for (int i = 0; i < indexHeader.TagCount; i++)
                    indices[i] = reader.ReadInt32();
                #endregion

                #region Read Names
                reader.SeekTo(cacheHeader.TagNamesBufferOffset);

                EndianReader newReader = null;

                if (cache.TagsKey == "" || cache.TagsKey == null)
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(cacheHeader.TagNamesBufferSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = cacheHeader.TagNamesBufferOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(cacheHeader.TagNamesBufferSize, cache.TagsKey), EndianFormat.BigEndian);
                }

                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {
                        this[i].Name = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = cacheHeader.TagNamesBufferSize - indices[i];
                    else
                    {
                        if (indices[i + 1] == -1)
                        {
                            int index = -1;

                            for (int j = i + 1; j < indices.Length; j++)
                            {
                                if (indices[j] != -1)
                                {
                                    index = j;
                                    break;
                                }
                            }

                            length = (index == -1) ? cacheHeader.TagNamesBufferSize - indices[i] : indices[index] - indices[i];
                        }
                        else
                            length = indices[i + 1] - indices[i];
                    }

                    if (length == 1)
                    {
                        this[i].Name = "<blank>";
                        continue;
                    }

                    if (length < 0)
                    {
                        int i0 = indices[i];
                        int i1 = indices[i + 1];
                        int i2 = indices[i + 2];
                        int i3 = indices[i + 3];
                    }

                    this[i].Name = newReader.ReadString(length);
                }

                newReader.Close();
                newReader.Dispose();
                #endregion
            }
        }

        public override void LoadResourceTags()
        {
            TagDeserializer deserializer = new TagDeserializer(Version);

            foreach (IndexItem item in IndexItems)
            {
                if (item.GroupTag == "play")
                {
                    CacheFile cacheFile = this;
                    var blamContext = new CacheSerializationContext(ref cacheFile, item);
                    if (cacheFile.File.FullName != File.FullName)
                        throw new InvalidOperationException();

                    ResourceLayoutTable = deserializer.Deserialize<CacheFileResourceLayoutTable>(blamContext);
                    break;
                }
            }

            foreach (IndexItem item in IndexItems)
            {
                if (item.GroupTag == "zone")
                {
                    CacheFile cacheFile = this;
                    var blamContext = new CacheSerializationContext(ref cacheFile, item);
                    if (cacheFile.File.FullName != File.FullName)
                        throw new InvalidOperationException();

                    ResourceGestalt = deserializer.Deserialize<CacheFileResourceGestalt>(blamContext);

                    foreach (var tagresource in ResourceGestalt.TagResources)
                    {
                        foreach (var fixup in tagresource.ResourceFixups)
                        {
                            fixup.Offset = (int)(fixup.Address.Value & 0x0FFFFFFF);
                            fixup.Type = (int)(fixup.Address.Value >> 28) & 0xF;
                            fixup.RawAddress = (int)fixup.Address.Value;
                        }
                    }

                    break;
                }
            }
        }

        public Dictionary<string, CacheFileGen3> SharedCacheFiles { get; } = new Dictionary<string, CacheFileGen3>();

        public byte[] ReadPageData(TagResourceGen3 resource, RawPage page)
        {
            var cacheFilePath = "";
            var cache = this;

            if (page.SharedCacheIndex != -1)
            {
                cacheFilePath = ResourceLayoutTable.ExternalCacheReferences[page.SharedCacheIndex].MapPath;
                cacheFilePath = cacheFilePath.Substring(cacheFilePath.LastIndexOf('\\'));
                cacheFilePath = File.DirectoryName + cacheFilePath;

                if (cacheFilePath != File.FullName)
                {
                    if (SharedCacheFiles.ContainsKey(cacheFilePath))
                        cache = SharedCacheFiles[cacheFilePath];
                    else
                        cache = SharedCacheFiles[cacheFilePath] = new CacheFileGen3(CacheContext, new FileInfo(cacheFilePath), Version, false);
                }
            }

            var offset = BitConverter.ToInt32(BitConverter.GetBytes(cache.Header.Interop.DebugSectionSize), 0) + page.BlockOffset;

            cache.Reader.SeekTo(offset);
            var compressed = cache.Reader.ReadBytes(BitConverter.ToInt32(BitConverter.GetBytes(page.CompressedBlockSize), 0));

            if (resource.ResourceTypeIndex != -1 && Strings.GetString(ResourceGestalt.ResourceTypes[resource.ResourceTypeIndex].Name) == "sound_resource_definition")
                return compressed;

            var decompressed = new byte[page.UncompressedBlockSize];

            if (page.CompressionCodecIndex == -1)
                cache.Reader.BaseStream.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));
            else
                using (var reader = new DeflateStream(new MemoryStream(compressed), CompressionMode.Decompress))
                    reader.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));

            return decompressed;
        }

        public override byte[] GetRawFromID(int ID, int DataLength)
        {
            if (ID == -1)
                return null;

            if (ResourceLayoutTable == null || ResourceGestalt == null)
                LoadResourceTags();

            var resource = ResourceGestalt.TagResources[ID & ushort.MaxValue];

            if (resource.SegmentIndex == -1) return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.RequiredPageIndex == -1 || segment.RequiredSegmentOffset == -1)
                return null;

            int pageIndex = (segment.OptionalPageIndex != -1) ?
                segment.OptionalPageIndex :
                segment.RequiredPageIndex;

            int segmentOffset = (segment.OptionalSegmentOffset != -1) ?
                segment.OptionalSegmentOffset :
                segment.RequiredSegmentOffset;

            if (pageIndex == -1 || segmentOffset == -1)
                return null;

            if (ResourceLayoutTable.RawPages[pageIndex].BlockOffset == -1)
            {
                pageIndex = segment.RequiredPageIndex;
                segmentOffset = segment.RequiredSegmentOffset;
            }

            var page = ResourceLayoutTable.RawPages[pageIndex];
            var decompressed = ReadPageData(resource, page);

            var length = DataLength == -1 ? (page.UncompressedBlockSize - segmentOffset) : DataLength;
            var data = new byte[length];

            if (length > decompressed.Length)
                length = decompressed.Length;

			length = Math.Min(length, decompressed.Length - segmentOffset);
            Array.Copy(decompressed, segmentOffset, data, 0, length);

            return data;
        }

        public override byte[] GetSoundRaw(int ID, int size)
        {
            if (ResourceLayoutTable == null || ResourceGestalt == null)
                LoadResourceTags();

            var resource = ResourceGestalt.TagResources[ID & ushort.MaxValue];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[resource.SegmentIndex];

            if (segment.RequiredPageIndex == -1 || segment.RequiredSegmentOffset == -1 || segment.RequiredSizeIndex == -1 || segment.OptionalSizeIndex == -1)
                return null;

            var sizes = ResourceLayoutTable.Sizes[segment.OptionalSizeIndex];

            var reqPage = ResourceLayoutTable.RawPages[segment.RequiredPageIndex];
            var optPage = ResourceLayoutTable.RawPages[segment.OptionalPageIndex];

            var reqBlockSize = BitConverter.ToInt32(BitConverter.GetBytes(reqPage.CompressedBlockSize), 0);
            var optBlockSize = BitConverter.ToInt32(BitConverter.GetBytes(optPage.CompressedBlockSize), 0);

            if (size == 0)
                size = (reqBlockSize != 0) ?
                    reqBlockSize :
                    optBlockSize;

            var reqSize = size - sizes.OverallSize;
            var optSize = size - reqSize;

            byte[] data = new byte[size];

            if (reqSize > 0)
            {
                var buffer = ReadPageData(resource, reqPage);
                Array.Copy(buffer, segment.RequiredSegmentOffset, data, 0, reqSize);
            }

            if (segment.OptionalPageIndex != -1 && optSize > 0)
            {
                var buffer = ReadPageData(resource, optPage);

                if (buffer.Length > data.Length)
                    data = buffer;
                else
                    Array.Copy(buffer, segment.OptionalSegmentOffset, data, reqSize, optSize);
            }

            return data;
        }
    }
}