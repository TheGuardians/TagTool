using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "formation", Tag = "form", Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "formation", Tag = "form", Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
    public class Formation
    {
        public StringId Name;
        public List<Primitive> Primitives;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
        
        [TagStructure(Size = 0x24)]
        public class Primitive
        {
            public FlagBits Flags;
            public short Priority;
            public short Capacity;
            [TagField(Padding = true, Length = 2)]
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
            public class Point
            {
                public Angle Angle;
                public float Offset;
            }
        }
    }
}