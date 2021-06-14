using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "meter", Tag = "metr", Size = 0xAC)]
    public class Meter : TagStructure
    {
        public MeterFlags Flags;
        [TagField(ValidTags = new [] { "bitm" })]
        // two bitmaps specifying the mask and the meter levels
        public CachedTag StencilBitmaps;
        [TagField(ValidTags = new [] { "bitm" })]
        // optional bitmap to draw into the unmasked regions of the meter (modulated by the colors below)
        public CachedTag SourceBitmap;
        public short StencilSequenceIndex;
        public short SourceSequenceIndex;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public ColorInterpolationModesEnum InterpolateColors;
        public ColorAnchorsEnum AnchorColors;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public RealArgbColor EmptyColor;
        public RealArgbColor FullColor;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        // fade from fully masked to fully unmasked this distance beyond full (and below empty)
        public float UnmaskDistance; // meter units
        // fade from fully unmasked to fully masked this distance below full (and beyond empty)
        public float MaskDistance; // meter units
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public byte[] EncodedStencil;
        
        [Flags]
        public enum MeterFlags : uint
        {
        }
        
        public enum ColorInterpolationModesEnum : short
        {
            Linearly,
            FasterNearEmpty,
            FasterNearFull,
            ThroughRandomNoise
        }
        
        public enum ColorAnchorsEnum : short
        {
            AtBothEnds,
            AtEmpty,
            AtFull
        }
    }
}
