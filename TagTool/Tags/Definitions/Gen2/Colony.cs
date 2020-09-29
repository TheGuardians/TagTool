using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "colony", Tag = "coln", Size = 0x4C)]
    public class Colony : TagStructure
    {
        public FlagsValue Flags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        public Bounds<float> Radius;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding3;
        public RealArgbColor DebugColor;
        public CachedTag BaseMap;
        public CachedTag DetailMap;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            Unused = 1 << 0
        }
    }
}

