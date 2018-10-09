using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cache_file_resource_layout_table", Size = 0x3C, Tag = "play")]
    public class CacheFileResourceLayoutTable : TagStructure
	{
        public List<CompressionCodec> CompressionCodecs;
        public List<ExternalCacheReference> ExternalCacheReferences;
        public List<RawPage> RawPages;
        public List<Size> Sizes;
        public List<Segment> Segments;

        [TagStructure(Size = 0x10)]
        public class CompressionCodec : TagStructure
		{
            [TagField(Length = 0x10)]
            public byte[] Guid;
        }

        [TagStructure(Size = 0x108)]
        public class ExternalCacheReference : TagStructure
		{
            [TagField(Length = 0x12)]
            public string MapPath;

            [TagField(Length = 0xEE)]
            public byte[] UnusedData;

            public short Unknown;
            public short Unknown2;
            public uint Unknown3;
        }

        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Size : TagStructure
		{
            public int OverallSize;
            public List<Part> Parts;

            [TagStructure(Size = 0x8)]
            public class Part : TagStructure
			{
                public int Unknown;
                public int Size;
            }
        }

        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Segment : TagStructure
		{
            public short RequiredPageIndex;
            public short OptionalPageIndex;
            public int RequiredSegmentOffset;
            public int OptionalSegmentOffset;
            public short RequiredSizeIndex;
            public short OptionalSizeIndex;
        }
    }
}