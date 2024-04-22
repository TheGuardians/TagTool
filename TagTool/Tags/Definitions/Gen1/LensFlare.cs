using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0xF0)]
    public class LensFlare : TagStructure
    {
        public Angle FalloffAngle; // degrees
        public Angle CutoffAngle; // degrees
        [TagField(Length = 0x8)]
        public byte[] Padding;
        /// <summary>
        /// Occlusion factor affects overall lens flare brightness and can also affect scale. Occlusion never affects rotation.
        /// </summary>
        /// <summary>
        /// radius of the square used to test occlusion
        /// </summary>
        public float OcclusionRadius; // world units
        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        /// <summary>
        /// distance at which the lens flare brightness is maximum
        /// </summary>
        public float NearFadeDistance; // world units
        /// <summary>
        /// distance at which the lens flare brightness is minimum; set to zero to disable distance fading
        /// </summary>
        public float FarFadeDistance; // world units
        /// <summary>
        /// used by reflections
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        public FlagsValue Flags;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x4C)]
        public byte[] Padding3;
        /// <summary>
        /// Controls how corona rotation is affected by viewer and light angles.
        /// </summary>
        public RotationFunctionValue RotationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding4;
        public Angle RotationFunctionScale; // degrees
        [TagField(Length = 0x18)]
        public byte[] Padding5;
        /// <summary>
        /// amount to stretch the corona along the horizontal axis; 0 defaults to 1
        /// </summary>
        public float HorizontalScale;
        /// <summary>
        /// amount to stretch the corona along the vertical axis; 0 defaults to 1
        /// </summary>
        public float VerticalScale;
        [TagField(Length = 0x1C)]
        public byte[] Padding6;
        public List<LensFlareReflectionBlock> Reflections;
        [TagField(Length = 0x20)]
        public byte[] Padding7;
        
        public enum OcclusionOffsetDirectionValue : short
        {
            TowardViewer,
            MarkerForward,
            None
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            Sun = 1 << 0
        }
        
        public enum RotationFunctionValue : short
        {
            None,
            RotationA,
            RotationB,
            RotationTranslation,
            Translation
        }
        
        [TagStructure(Size = 0x80)]
        public class LensFlareReflectionBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public short BitmapIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x14)]
            public byte[] Padding2;
            /// <summary>
            /// 0 is on top of light, 1 is opposite light, 0.5 is the center of the screen, etc.
            /// </summary>
            public float Position; // along flare axis
            public float RotationOffset; // degrees
            [TagField(Length = 0x4)]
            public byte[] Padding3;
            /// <summary>
            /// interpolated by light scale
            /// </summary>
            public Bounds<float> Radius; // world units
            public RadiusScaledByValue RadiusScaledBy;
            [TagField(Length = 0x2)]
            public byte[] Padding4;
            /// <summary>
            /// interpolated by light scale
            /// </summary>
            public Bounds<float> Brightness; // [0,1]
            public BrightnessScaledByValue BrightnessScaledBy;
            [TagField(Length = 0x2)]
            public byte[] Padding5;
            /// <summary>
            /// Tinting and modulating are not the same; 'tinting' a reflection will color the darker regions but leave the white
            /// highlights, while 'modulating' will color everything uniformly (as in most games). The tint alpha controls how much the
            /// reflection is tinted as opposed to modulated (0 is modulated, 1 is tinted). If all components are zero, the reflection is
            /// fully tinted by the light color.
            /// </summary>
            /// <summary>
            /// if a=r=g=b=0 use light color instead; alpha blends between modulation and tinting
            /// </summary>
            public RealArgbColor TintColor;
            /// <summary>
            /// Causes lens flare reflection to flicker, pulse, or whatever. Animated color modulates tint color, above, while animated
            /// alpha modulates brightness. Animation is ignored if tint color is BLACK or the animation function is ONE or ZERO.
            /// </summary>
            /// <summary>
            /// if a=r=g=b=0, default to a=r=g=b=1
            /// </summary>
            public RealArgbColor ColorLowerBound;
            /// <summary>
            /// if a=r=g=b=0, default to a=r=g=b=1
            /// </summary>
            public RealArgbColor ColorUpperBound;
            public Flags1Value Flags1;
            public AnimationFunctionValue AnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float AnimationPeriod; // seconds
            public float AnimationPhase; // seconds
            [TagField(Length = 0x4)]
            public byte[] Padding6;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                AlignRotationWithScreenCenter = 1 << 0,
                RadiusNotScaledByDistance = 1 << 1,
                RadiusScaledByOcclusionFactor = 1 << 2,
                OccludedBySolidObjects = 1 << 3
            }
            
            public enum RadiusScaledByValue : short
            {
                None,
                Rotation,
                RotationAndStrafing,
                DistanceFromCenter
            }
            
            public enum BrightnessScaledByValue : short
            {
                None,
                Rotation,
                RotationAndStrafing,
                DistanceFromCenter
            }
            
            [Flags]
            public enum Flags1Value : ushort
            {
                InterpolateColorInHsv = 1 << 0,
                MoreColors = 1 << 1
            }
            
            public enum AnimationFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
        }
    }
}

