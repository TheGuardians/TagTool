using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Cache
{
    public class CacheFileGen3 : CacheFile
    {
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
                var Reader = cache.Reader;

                #region Read Values
                XmlNode indexHeaderNode = cache.VersionInfo.ChildNodes[1];

                XmlAttribute attr = indexHeaderNode.Attributes["tagClassCount"];
                int offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagGroupCount = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoOffset"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagsOffset = Reader.ReadInt32() - cache.Magic;

                attr = indexHeaderNode.Attributes["tagClassIndexOffset"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagGroupsOffset = Reader.ReadInt32() - cache.Magic;

                attr = indexHeaderNode.Attributes["tagCount"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagCount = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoHeaderCount"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagInfoHeaderCount = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoHeaderOffset"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagInfoHeaderOffset = Reader.ReadInt32() - cache.Magic;

                attr = indexHeaderNode.Attributes["tagInfoHeaderCount2"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagInfoHeaderCount2 = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoHeaderOffset2"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                TagInfoHeaderOffset2 = Reader.ReadInt32() - cache.Magic;
                #endregion
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
                    this.Add(item);
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
                            fixup.Offset = (fixup.Address & 0x0FFFFFFF);
                            fixup.Type = (fixup.Address >> 28) & 0xF;
                            fixup.RawAddress = fixup.Address;
                        }
                    }

                    break;
                }
            }
        }

        public override byte[] GetRawFromID(int ID, int DataLength)
        {
            if (ID == -1)
                return null;

            if (ResourceLayoutTable == null || ResourceGestalt == null)
                LoadResourceTags();

            EndianReader er;

            var Entry = ResourceGestalt.TagResources[ID & ushort.MaxValue];

            if (Entry.PlaySegmentIndex == -1) return null;

            var segment = ResourceLayoutTable.Segments[Entry.PlaySegmentIndex];

            using (var dataStream = new MemoryStream())
            {
                if (segment.PrimaryPageIndex != -1 && segment.PrimarySegmentOffset != -1)
                {
                    var page = ResourceLayoutTable.RawPages[segment.PrimaryPageIndex];

                    if (page.SharedCacheIndex != -1)
                    {
                        var fName = ResourceLayoutTable.ExternalCacheReferences[page.SharedCacheIndex].MapPath;
                        fName = fName.Substring(fName.LastIndexOf('\\'));
                        fName = File.DirectoryName + fName;

                        er = (fName != File.FullName) ?
                            new EndianReader(new FileStream(fName, FileMode.Open, FileAccess.Read), EndianFormat.BigEndian) :
                            Reader;
                    }
                    else
                        er = Reader;

                    er.SeekTo(int.Parse(VersionInfo.ChildNodes[0].Attributes["rawTableOffset"].Value));
                    er.SeekTo(page.BlockOffset + er.ReadInt32());

                    var data = er.ReadBytes(page.CompressionCodecIndex == byte.MaxValue ? page.UncompressedBlockSize : page.CompressedBlockSize);

                    using (var reqReader = new BinaryReader(
                        page.CompressionCodecIndex == byte.MaxValue ?
                            (Stream)new MemoryStream(data) :
                            (Stream)new DeflateStream(new MemoryStream(data), CompressionMode.Decompress)))
                    {
                        if (segment.PrimarySegmentOffset != -1)
                            reqReader.BaseStream.Seek(segment.PrimarySegmentOffset, SeekOrigin.Current);

                        dataStream.Write(reqReader.ReadBytes(page.UncompressedBlockSize), 0, page.UncompressedBlockSize);
                    }
                }

                if (segment.SecondaryPageIndex != -1 && segment.SecondarySegmentOffset != -1)
                {
                    var page = ResourceLayoutTable.RawPages[segment.SecondaryPageIndex];

                    if (page.SharedCacheIndex != -1)
                    {
                        var fName = ResourceLayoutTable.ExternalCacheReferences[page.SharedCacheIndex].MapPath;
                        fName = fName.Substring(fName.LastIndexOf('\\'));
                        fName = File.DirectoryName + fName;

                        er = (fName != File.FullName) ?
                            new EndianReader(new FileStream(fName, FileMode.Open, FileAccess.Read), EndianFormat.BigEndian) :
                            Reader;
                    }
                    else
                        er = Reader;

                    er.SeekTo(int.Parse(VersionInfo.ChildNodes[0].Attributes["rawTableOffset"].Value));
                    er.SeekTo(page.BlockOffset + er.ReadInt32());

                    var data = er.ReadBytes(page.CompressionCodecIndex == byte.MaxValue ? page.UncompressedBlockSize : page.CompressedBlockSize);

                    using (var reqReader = new BinaryReader(
                        page.CompressionCodecIndex == byte.MaxValue ?
                            (Stream)new MemoryStream(data) :
                            (Stream)new DeflateStream(new MemoryStream(data), CompressionMode.Decompress)))
                    {
                        if (segment.SecondarySegmentOffset != -1)
                            reqReader.BaseStream.Seek(segment.SecondarySegmentOffset, SeekOrigin.Current);

                        dataStream.Write(reqReader.ReadBytes(page.UncompressedBlockSize), 0, page.UncompressedBlockSize);
                    }
                }

                return dataStream.ToArray();
            }
        }

        public override byte[] GetSoundRaw(int ID, int size)
        {
            if (ResourceLayoutTable == null || ResourceGestalt == null)
                LoadResourceTags();

            var entry = ResourceGestalt.TagResources[ID & ushort.MaxValue];

            if (entry.PlaySegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Segments[entry.PlaySegmentIndex];

            if (segment.PrimaryPageIndex == -1 || segment.PrimarySegmentOffset == -1 ||  segment.PrimarySizeIndex == -1 || segment.SecondarySizeIndex == -1)
                return null;

            var sRaw = ResourceLayoutTable.Sizes[segment.SecondarySizeIndex];
            var reqPage = ResourceLayoutTable.RawPages[segment.PrimaryPageIndex];
            var optPage = ResourceLayoutTable.RawPages[segment.SecondaryPageIndex];

            if (size == 0) size = (reqPage.CompressedBlockSize != 0) ? reqPage.CompressedBlockSize : optPage.CompressedBlockSize;

            var reqSize = size - sRaw.OverallSize;
            var optSize = size - reqSize;

            byte[] buffer;
            byte[] data = new byte[size];
            int offset;
            EndianReader er;
            string fName = "";

            #region REQUIRED
            if (reqSize > 0)
            {
                if (reqPage.SharedCacheIndex != -1)
                {
                    fName = ResourceLayoutTable.ExternalCacheReferences[reqPage.SharedCacheIndex].MapPath;
                    fName = fName.Substring(fName.LastIndexOf('\\'));
                    fName = File.DirectoryName + fName;

                    if (fName == File.FullName)
                        er = Reader;
                    else
                        er = new EndianReader(new FileStream(fName, FileMode.Open, FileAccess.Read), EndianFormat.BigEndian);
                }
                else
                    er = Reader;

                er.SeekTo(1136);
                offset = reqPage.BlockOffset + er.ReadInt32();

                er.SeekTo(offset);
                buffer = er.ReadBytes(reqPage.CompressedBlockSize);

                Array.Copy(buffer, segment.PrimarySegmentOffset, data, 0, reqSize);

                if (er != Reader)
                {
                    er.Close();
                    er.Dispose();
                }
            }
            #endregion

            #region OPTIONAL
            if (segment.SecondaryPageIndex != -1 && optSize > 0)
            {
                if (optPage.SharedCacheIndex != -1)
                {
                    fName = ResourceLayoutTable.ExternalCacheReferences[optPage.SharedCacheIndex].MapPath;
                    fName = fName.Substring(fName.LastIndexOf('\\'));
                    fName = File.DirectoryName + fName;

                    if (fName == File.FullName)
                        er = Reader;
                    else
                        er = new EndianReader(new FileStream(fName, FileMode.Open, FileAccess.Read), EndianFormat.BigEndian);
                }
                else
                    er = Reader;

                er.SeekTo(1136);
                offset = optPage.BlockOffset + er.ReadInt32();

                er.SeekTo(offset);
                buffer = er.ReadBytes(optPage.CompressedBlockSize);

                if (buffer.Length > data.Length)
                    data = buffer;
                else
                    Array.Copy(buffer, segment.SecondarySegmentOffset, data, reqSize, optSize);


                if (er != Reader)
                {
                    er.Close();
                    er.Dispose();
                }
            }
            #endregion

            return data;
        }
    }
}