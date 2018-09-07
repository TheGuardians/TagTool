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

            [TagField(Local = true)]
            public List<int> SegmentOffsets = new List<int>();

            [TagField(Local = true)]
            public List<int> SegmentSizes = new List<int>();

            [TagField(Local = true)]
            public byte[] PageData = null;

            public void AddSegment(int offset)
            {
                if (UncompressedBlockSize != 0 && !SegmentOffsets.Contains(offset))
                {
                    SegmentOffsets.Add(offset);
                    SegmentSizes.Add(-1); // we'll update this when we post-process in 'CalculateSegmentSizes'
                }
            }

            public void CalculateSegmentSizes()
            {
                // If this page isn't null
                if (UncompressedBlockSize == 0) return;

                // We work backwards to find the sizes as we can just find the 
                // distances between each segment. For the last segment we can 
                // just use the page size minus the offset to find it's size
                int last_element = SegmentOffsets.Count - 1;

                // TODO: there should be at least one segment referencing this page...?
                if (last_element == -1)
                {
                    SegmentOffsets.Add(0);
                    SegmentSizes.Add(UncompressedBlockSize);
                }
                // we only have one element so no point in loopin'
                else if (last_element == 0) SegmentSizes[0] = UncompressedBlockSize;
                else
                {
                    // figure out the segment sizes via the next
                    // segment's offset after 'x' segment
                    SegmentOffsets.Sort(); // loop depends on the offsets being linear

                    // Figure out the last segment first. This used to be done 
                    // in the for loop with a check 'x == last_element' but doing 
                    // it here leaves out that boolean check and possible code 
                    // jump in the result code
                    SegmentSizes[last_element] = UncompressedBlockSize - SegmentOffsets[last_element];
                    for (int x = last_element - 1; x >= 0; x--)
                        SegmentSizes[x] = SegmentOffsets[x + 1] - SegmentOffsets[x];
                }
            }

            public int GetSegmentSize(int segment_offset)
            {
                // Offsets are aligned with Sizes so the indices are transferable
                int x = SegmentOffsets.IndexOf(segment_offset);

                if (x != -1) // make sure the size has been post-processed first
                    x = SegmentSizes[x];

                return x;
            }

            public byte[] GetSegmentData(CacheFileGen3 cf, CacheFileResourceLayoutTable owner, int segment_offset)
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

                int segment_size = GetSegmentSize(segment_offset);
                if (segment_size == -1) return null; // offset was either invalid or sizes haven't been post-processed

                byte[] segment_data = new byte[segment_size];
                // Extract the segment data from the page
                Array.Copy(PageData, segment_offset, segment_data, 0, segment_data.Length);

                return segment_data;
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
            public short PrimaryPageIndex;
            public short SecondaryPageIndex;
            public int PrimarySegmentOffset;
            public int SecondarySegmentOffset;
            public short PrimarySizeIndex;
            public short SecondarySizeIndex;
        }
    }
}