using TagTool.Serialization;
using System.Collections.Generic;

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