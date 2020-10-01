using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "light_volume", Tag = "mgs2", Size = 0x14C)]
    public class LightVolume : TagStructure
    {
        /// <summary>
        /// Draws a sequence of glow bitmaps along a line. Can be used for contrail-type effects as well as volumetric lights.
        /// </summary>
        /// <summary>
        /// the marker name that the light volume should be attached to
        /// </summary>
        [TagField(Length = 32)]
        public string AttachmentMarker;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public FlagsValue Flags;
        [TagField(Length = 0x10)]
        public byte[] Padding1;
        /// <summary>
        /// Fades the effect in and out with distance, viewer angle, and external source.
        /// </summary>
        /// <summary>
        /// distance at which light volume is full brightness
        /// </summary>
        public float NearFadeDistance; // world units
        /// <summary>
        /// distance at which light volume is zero brightness
        /// </summary>
        public float FarFadeDistance; // world units
        /// <summary>
        /// brightness scale when viewed at a 90-degree angle
        /// </summary>
        public float PerpendicularBrightnessScale; // [0,1]
        /// <summary>
        /// brightness scale when viewed directly
        /// </summary>
        public float ParallelBrightnessScale; // [0,1]
        /// <summary>
        /// scales brightness based on external value
        /// </summary>
        public BrightnessScaleSourceValue BrightnessScaleSource;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x14)]
        public byte[] Padding3;
        /// <summary>
        /// Bitmap tag used to draw the light volume, repeated count times. Default is 'tags\rasterizer_textures\glow'. Note that
        /// sprite plates are not valid for light volumes.
        /// </summary>
        /// <summary>
        /// NOT A SPRITE PLATE
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Map;
        public short SequenceIndex;
        /// <summary>
        /// number of bitmaps to draw (0 causes light volume not to render)
        /// </summary>
        public short Count;
        [TagField(Length = 0x48)]
        public byte[] Padding4;
        /// <summary>
        /// Frames are descriptions of the light volume at a particular point in time, interpolated by an external source. For
        /// example, a bolt of energy can be made to stretch out and grow thinner as it is fired from a weapon.
        /// </summary>
        /// <summary>
        /// interpolates between frames based on external value
        /// </summary>
        public FrameAnimationSourceValue FrameAnimationSource;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        [TagField(Length = 0x24)]
        public byte[] Padding6;
        [TagField(Length = 0x40)]
        public byte[] Padding7;
        public List<LightVolumeFrameBlock> Frames;
        [TagField(Length = 0x20)]
        public byte[] Padding8;
        
        public enum FlagsValue : ushort
        {
            InterpolateColorInHsv,
            MoreColors
        }
        
        public enum BrightnessScaleSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum FrameAnimationSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        [TagStructure(Size = 0xB0)]
        public class LightVolumeFrameBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Padding;
            public float OffsetFromMarker; // world units
            /// <summary>
            /// 0 defaults to 1; 1 compresses light near marker, 1 compresses light near far end
            /// </summary>
            public float OffsetExponent;
            /// <summary>
            /// 0 causes light volume not to render
            /// </summary>
            public float Length; // world units
            [TagField(Length = 0x20)]
            public byte[] Padding1;
            /// <summary>
            /// radius near the marker
            /// </summary>
            public float RadiusHither; // world units
            /// <summary>
            /// radius at far end of light volume
            /// </summary>
            public float RadiusYon; // world units
            /// <summary>
            /// 0 defaults to 1; 1 values are more teardrop-shaped, 1 values are more pill-shaped
            /// </summary>
            public float RadiusExponent;
            [TagField(Length = 0x20)]
            public byte[] Padding2;
            /// <summary>
            /// tint color near the marker (alpha is brightness)
            /// </summary>
            public RealArgbColor TintColorHither;
            /// <summary>
            /// tint color at far end of light volume (alpha is brightness)
            /// </summary>
            public RealArgbColor TintColorYon;
            /// <summary>
            /// 0 defaults to 1; affects tint color only, not brightness
            /// </summary>
            public float TintColorExponent;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float BrightnessExponent;
            [TagField(Length = 0x20)]
            public byte[] Padding3;
        }
    }
}

