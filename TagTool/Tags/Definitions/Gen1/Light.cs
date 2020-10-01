using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x160)]
    public class Light : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// the size and shape of the light
        /// </summary>
        /// <summary>
        /// the radius where illumination is zero. (lens flare only if this is 0)
        /// </summary>
        public float Radius;
        /// <summary>
        /// how the radius changes with external scale
        /// </summary>
        public Bounds<float> RadiusModifer;
        /// <summary>
        /// the angle at which the light begins to fade.
        /// </summary>
        public Angle FalloffAngle;
        /// <summary>
        /// the angle at which the illumination is zero.
        /// </summary>
        public Angle CutoffAngle;
        public float LensFlareOnlyRadius;
        [TagField(Length = 0x18)]
        public byte[] Padding;
        public InterpolationFlagsValue InterpolationFlags;
        public RealArgbColor ColorLowerBound;
        public RealArgbColor ColorUpperBound;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// the gel map tints the light per-pixel of cube map
        /// </summary>
        /// <summary>
        /// used for diffuse and specular light
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PrimaryCubeMap;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        /// <summary>
        /// a function to control texture animation
        /// </summary>
        public TextureAnimationFunctionValue TextureAnimationFunction;
        /// <summary>
        /// time between repeats
        /// </summary>
        public float TextureAnimationPeriod; // seconds
        /// <summary>
        /// used for specular light
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SecondaryCubeMap;
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        /// <summary>
        /// a function to control rotation of the gel in yaw
        /// </summary>
        public YawFunctionValue YawFunction;
        /// <summary>
        /// time between repeats
        /// </summary>
        public float YawPeriod; // seconds
        [TagField(Length = 0x2)]
        public byte[] Padding4;
        /// <summary>
        /// a function to control rotation of the gel in roll
        /// </summary>
        public RollFunctionValue RollFunction;
        /// <summary>
        /// time between repeats
        /// </summary>
        public float RollPeriod; // seconds
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        /// <summary>
        /// a function to control rotation of the gel in pitch
        /// </summary>
        public PitchFunctionValue PitchFunction;
        /// <summary>
        /// time between repeats
        /// </summary>
        public float PitchPeriod; // seconds
        [TagField(Length = 0x8)]
        public byte[] Padding6;
        /// <summary>
        /// optional lens flare associated with this light
        /// </summary>
        [TagField(ValidTags = new [] { "lens" })]
        public CachedTag LensFlare;
        [TagField(Length = 0x18)]
        public byte[] Padding7;
        /// <summary>
        /// how the light affects the lightmaps (ignored for dynamic lights)
        /// </summary>
        public float Intensity;
        public RealRgbColor Color;
        [TagField(Length = 0x10)]
        public byte[] Padding8;
        /// <summary>
        /// if the light is created by an effect, it will animate itself as follows
        /// </summary>
        /// <summary>
        /// the light will last this long when created by an effect
        /// </summary>
        public float Duration; // seconds
        [TagField(Length = 0x2)]
        public byte[] Padding9;
        /// <summary>
        /// the scale of the light will diminish over time according to this function
        /// </summary>
        public FalloffFunctionValue FalloffFunction;
        [TagField(Length = 0x8)]
        public byte[] Padding10;
        [TagField(Length = 0x5C)]
        public byte[] Padding11;
        
        public enum FlagsValue : uint
        {
            /// <summary>
            /// dynamically illuminate interiors
            /// </summary>
            Dynamic,
            /// <summary>
            /// for dynamic lights, cast only diffuse light.
            /// </summary>
            NoSpecular,
            /// <summary>
            /// for dynamic lights, don't light the object that the light is attached to.
            /// </summary>
            DonTLightOwnObject,
            /// <summary>
            /// for dynamic lights, light every environment surface if this light is on the gun of the current
            /// window.
            /// </summary>
            SupersizeInFirstPerson,
            FirstPersonFlashlight,
            DonTFadeActiveCamouflage
        }
        
        public enum InterpolationFlagsValue : uint
        {
            /// <summary>
            /// blends colors in hsv rather than rgb space
            /// </summary>
            BlendInHsv,
            /// <summary>
            /// blends colors through more hues (goes the long way around the color wheel)
            /// </summary>
            MoreColors
        }
        
        public enum TextureAnimationFunctionValue : short
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
        
        public enum YawFunctionValue : short
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
        
        public enum RollFunctionValue : short
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
        
        public enum PitchFunctionValue : short
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
        
        public enum FalloffFunctionValue : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
        }
    }
}

