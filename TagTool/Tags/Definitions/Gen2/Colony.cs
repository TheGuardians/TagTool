using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "colony", Tag = "coln", Size = 0x3C)]
    public class Colony : TagStructure
    {
        public FlagsValue Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public Bounds<float> Radius;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public RealArgbColor DebugColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BaseMap;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DetailMap;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            Unused = 1 << 0
        }
    }
}

