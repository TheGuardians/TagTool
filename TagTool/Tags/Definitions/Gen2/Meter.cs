using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "meter", Tag = "metr", Size = 0xAC)]
    public class Meter : TagStructure
    {
        public FlagsValue Flags;
        public CachedTag StencilBitmaps; // two bitmaps specifying the mask and the meter levels
        public CachedTag SourceBitmap; // optional bitmap to draw into the unmasked regions of the meter (modulated by the colors below)
        public short StencilSequenceIndex;
        public short SourceSequenceIndex;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        public InterpolateColorsValue InterpolateColors;
        public AnchorColorsValue AnchorColors;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding3;
        public RealArgbColor EmptyColor;
        public RealArgbColor FullColor;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding4;
        public float UnmaskDistance; // meter units
        public float MaskDistance; // meter units
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding5;
        public byte[] EncodedStencil;
        
        [Flags]
        public enum FlagsValue : uint
        {
        }
        
        public enum InterpolateColorsValue : short
        {
            Linearly,
            FasterNearEmpty,
            FasterNearFull,
            ThroughRandomNoise
        }
        
        public enum AnchorColorsValue : short
        {
            AtBothEnds,
            AtEmpty,
            AtFull
        }
    }
}

