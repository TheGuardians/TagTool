using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "meter", Tag = "metr", Size = 0x90)]
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
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public InterpolateColorsValue InterpolateColors;
        public AnchorColorsValue AnchorColors;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public RealArgbColor EmptyColor;
        public RealArgbColor FullColor;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        /// <summary>
        /// fade from fully masked to fully unmasked this distance beyond full (and below empty)
        /// </summary>
        public float UnmaskDistance; // meter units
        /// <summary>
        /// fade from fully unmasked to fully masked this distance below full (and beyond empty)
        /// </summary>
        public float MaskDistance; // meter units
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
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

