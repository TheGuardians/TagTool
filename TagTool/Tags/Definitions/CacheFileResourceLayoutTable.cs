using TagTool.Serialization;
using System.Collections.Generic;
using TagTool.Cache;
using System.IO;
using System.IO.Compression;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cache_file_resource_layout_table", Size = 0x3C, Tag = "play")]
    public class CacheFileResourceLayoutTable
    {
        public List<CompressionCodec> CompressionCodecs;
        public List<ExternalCacheReference> ExternalCacheReferences;
        public List<RawPage> RawPages;
        public List<Size> Sizes;
        public List<Segment> Segments;

        [TagField(Runtime = true)]
        public bool InteropDataBuilt = false;

        public void BuildInteropData()
        {
            if (InteropDataBuilt)
                return;

            foreach (var segmentation in Segments)
                segmentation.UpdatePageData(RawPages);

            foreach (var page in RawPages)
                page.CalculateSegmentSizes();

            foreach (var segmentation in Segments)
                segmentation.UpdateSegmentData(RawPages);

            InteropDataBuilt = true;
        }

        [TagStructure(Size = 0x10)]
        public class CompressionCodec
        {
            [TagField(Length = 0x10)]
            public byte[] Guid;
        }

        [TagStructure(Size = 0x108)]
        public class ExternalCacheReference
        {
            [TagField(Length = 0x12)]
            public string MapPath;

            [TagField(Length = 0xEE)]
            public byte[] UnusedData;

            public short Unknown;
            public short Unknown2;
            public uint Unknown3;
        }

        [TagStructure(Size = 0x58, Align = 0x8)]
        public class RawPage
        {
            public short Salt;
            public byte Flags;
            public byte CompressionCodecIndex;
            public short SharedCacheIndex;
            public short Unknown;

            public int BlockOffset;
            public int CompressedBlockSize;
            public int UncompressedBlockSize;
            public uint CrcChecksum;

            [TagField(Length = 20)]
            public byte[] EntireBufferHash;

            [TagField(Length = 20)]
            public byte[] FirstChunkHash;

            [TagField(Length = 20)]
            public byte[] LastChunkHash;

            public short BlockAssetCount;
            public short Unknown3;

            [TagField(Runtime = true)]
            public List<int> SegmentOffsets = new List<int>();

            [TagField(Runtime = true)]
            public List<int> SegmentSizes = new List<int>();

            [TagField(Runtime = true)]
            public byte[] PageData = null;

            public void AddSegment(int segmentOffset)
            {
                if (UncompressedBlockSize != 0 && !SegmentOffsets.Contains(segmentOffset))
                {
                    SegmentOffsets.Add(segmentOffset);
                    SegmentSizes.Add(-1);
                }
            }

            public void CalculateSegmentSizes()
            {
                if (UncompressedBlockSize == 0)
                    return;

                var index = SegmentOffsets.Count - 1;

                if (index == -1)
                {
                    SegmentOffsets.Add(0);
                    SegmentSizes.Add(UncompressedBlockSize);
                }
                else if (index == 0)
                {
                    SegmentSizes[0] = UncompressedBlockSize;
                }
                else
                {
                    SegmentOffsets.Sort();

                    SegmentSizes[index] = UncompressedBlockSize - SegmentOffsets[index];

                    for (var i = index - 1; i >= 0; i--)
                        SegmentSizes[i] = SegmentOffsets[i + 1] - SegmentOffsets[i];
                }
            }

            public int GetSegmentSize(int segmentOffset)
            {
                var result = SegmentOffsets.IndexOf(segmentOffset);

                if (result != -1)
                    result = SegmentSizes[result];

                return result;
            }

            public byte[] GetSegmentData(CacheFileGen3 cf, CacheFileResourceLayoutTable owner, int segmentOffset)
            {
                if (PageData == null)
                {
                    if (SharedCacheIndex > -1)
                    {
                        var fName = owner.ExternalCacheReferences[SharedCacheIndex].MapPath;
                        fName = fName.Substring(fName.LastIndexOf('\\'));
                        fName = Path.Combine(cf.File.DirectoryName, fName);

                        if (fName != cf.File.FullName)
                            cf = new CacheFileGen3(cf.CacheContext, new FileInfo(fName), cf.Version, false);
                    }

                    var size = UncompressedBlockSize;

                    if (size <= 0)
                        return null;
;
                    var offset = (uint)cf.Header.Interop.Sections[(int)CacheFileSectionType.Resource].CacheOffset + (uint)BlockOffset;
                    cf.Reader.SeekTo(offset);

                    if (CompressionCodecIndex != byte.MaxValue)
                    {
                        using (var deflate = new DeflateStream(cf.Reader.BaseStream, CompressionMode.Decompress, true))
                        {
                            PageData = new byte[size];
                            deflate.Read(PageData, 0, PageData.Length);
                        }
                    }
                    else
                    {
                        PageData = cf.Reader.ReadBytes(size);
                    }
                }

                var segmentSize = GetSegmentSize(segmentOffset);

                if (segmentSize == -1)
                    return null;

                var segmentData = new byte[segmentSize];
                Array.Copy(PageData, segmentOffset, segmentData, 0, segmentData.Length);

                return segmentData;
            }
        }

        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Size
        {
            public int OverallSize;
            public List<Part> Parts;

            [TagStructure(Size = 0x8)]
            public class Part
            {
                public int Unknown;
                public int Size;
            }
        }

        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Segment
        {
            public short RequiredPageIndex;
            public short OptionalPageIndex;
            public int RequiredSegmentOffset;
            public int OptionalSegmentOffset;
            public short RequiredSizeIndex;
            public short OptionalSizeIndex;

            [TagField(Runtime = true)]
            public int RequiredSize = -1;

            [TagField(Runtime = true)]
            public int OptionalSize = -1;

            public void UpdatePageData(List<RawPage> pages)
            {
                if (RequiredPageIndex != -1)
                    pages[RequiredPageIndex].AddSegment(RequiredSegmentOffset);

                if (OptionalPageIndex != -1 && RequiredSegmentOffset != OptionalSegmentOffset)
                    pages[OptionalPageIndex].AddSegment(OptionalSegmentOffset);
            }

            public void UpdateSegmentData(List<RawPage> pages)
            {
                if (RequiredPageIndex != -1)
                    RequiredSize = pages[RequiredPageIndex].GetSegmentSize(RequiredSegmentOffset);

                if (OptionalPageIndex != -1 && RequiredSegmentOffset != OptionalSegmentOffset)
                    OptionalSize = pages[OptionalPageIndex].GetSegmentSize(OptionalSegmentOffset);
            }
        }
    }
}