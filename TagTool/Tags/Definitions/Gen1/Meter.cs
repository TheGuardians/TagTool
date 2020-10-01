using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "meter", Tag = "metr", Size = 0xAC)]
    public class Meter : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// two bitmaps specifying the mask and the meter levels
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag StencilBitmaps;
        /// <summary>
        /// optional bitmap to draw into the unmasked regions of the meter (modulated by the colors below)
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SourceBitmap;
        public short StencilSequenceIndex;
        public short SourceSequenceIndex;
        [TagField(Length = 0x10)]
        public byte[] Padding;
        [TagField(Length = 0x4)]
        public byte[] Padding1;
        public InterpolateColorsValue InterpolateColors;
        public AnchorColorsValue AnchorColors;
        [TagField(Length = 0x8)]
        public byte[] Padding2;
        public RealArgbColor EmptyColor;
        public RealArgbColor FullColor;
        [TagField(Length = 0x14)]
        public byte[] Padding3;
        /// <summary>
        /// fade from fully masked to fully unmasked this distance beyond full (and below empty)
        /// </summary>
        public float UnmaskDistance; // meter units
        /// <summary>
        /// fade from fully unmasked to fully masked this distance below full (and beyond empty)
        /// </summary>
        public float MaskDistance; // meter units
        [TagField(Length = 0x14)]
        public byte[] Padding4;
        public byte[] EncodedStencil;
        
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

