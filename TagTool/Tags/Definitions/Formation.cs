using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "formation", Tag = "form", Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "formation", Tag = "form", Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
    public class Formation : TagStructure
	{
        public StringId Name;
        public List<Primitive> Primitives;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused;
        
        [TagStructure(Size = 0x24)]
        public class Primitive : TagStructure
		{
            public FlagBits Flags;
            public short Priority;
            public short Capacity;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
            public float DistanceForwards;
            public float DistanceBackwards;
            public float RankSpacing;
            public float FileSpacing;
            public List<Point> Points;

            [Flags]
            public enum FlagBits : ushort
            {
                None,
                Radial = 1 << 0,
                Leader = 1 << 1
            }

            [TagStructure(Size = 0x8)]
            public class Point : TagStructure
			{
                public Angle Angle;
                public float Offset;
            }
        }
    }
}