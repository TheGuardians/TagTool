using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "weather_particle_system", Tag = "rain", Size = 0x30)]
    public class WeatherParticleSystem : TagStructure
    {
        public FlagsValue Flags;
        [TagField(Length = 0x20)]
        public byte[] Padding;
        public List<WeatherParticleTypeBlock> ParticleTypes;
        
        public enum FlagsValue : uint
        {
        }
        
        [TagStructure(Size = 0x25C)]
        public class WeatherParticleTypeBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            /// <summary>
            /// Particles begin to fade into visibility beyond this distance
            /// </summary>
            public float FadeInStartDistance; // world units
            /// <summary>
            /// Particles become fully visible beyond this distance
            /// </summary>
            public float FadeInEndDistance; // world units
            /// <summary>
            /// Particles begin to fade out of visibility beyond this distance
            /// </summary>
            public float FadeOutStartDistance; // world units
            /// <summary>
            /// Particles become fully invisible beyond this distance
            /// </summary>
            public float FadeOutEndDistance; // world units
            /// <summary>
            /// Particles begin to fade into visibility above this height
            /// </summary>
            public float FadeInStartHeight; // world units
            /// <summary>
            /// Particles become fully visible above this height
            /// </summary>
            public float FadeInEndHeight; // world units
            /// <summary>
            /// Particles begin to fade out of visibility above this height
            /// </summary>
            public float FadeOutStartHeight; // world units
            /// <summary>
            /// Particles become fully invisible above this height
            /// </summary>
            public float FadeOutEndHeight; // world units
            [TagField(Length = 0x60)]
            public byte[] Padding;
            public Bounds<float> ParticleCount; // particles per cubic world unit
            [TagField(ValidTags = new [] { "pphy" })]
            public CachedTag Physics;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            public Bounds<float> AccelerationMagnitude;
            public float AccelerationTurningRate;
            public float AccelerationChangeRate;
            [TagField(Length = 0x20)]
            public byte[] Padding2;
            public Bounds<float> ParticleRadius; // world units
            public Bounds<float> AnimationRate; // frames per second
            public Bounds<Angle> RotationRate; // degrees per second
            [TagField(Length = 0x20)]
            public byte[] Padding3;
            public RealArgbColor ColorLowerBound;
            public RealArgbColor ColorUpperBound;
            [TagField(Length = 0x40)]
            public byte[] Padding4;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag SpriteBitmap;
            public RenderModeValue RenderMode;
            /// <summary>
            /// Render modes that depend on an direction will use this vector.
            /// </summary>
            public RenderDirectionSourceValue RenderDirectionSource;
            [TagField(Length = 0x28)]
            public byte[] Padding5;
            public ShaderFlagsValue ShaderFlags;
            public FramebufferBlendFunctionValue FramebufferBlendFunction;
            public FramebufferFadeModeValue FramebufferFadeMode;
            public MapFlagsValue MapFlags;
            [TagField(Length = 0x1C)]
            public byte[] Padding6;
            /// <summary>
            /// Optional multitextured second map
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            public AnchorValue Anchor;
            public Flags1Value Flags1;
            public UAnimationSourceValue UAnimationSource;
            public UAnimationFunctionValue UAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float UAnimationPeriod; // seconds
            public float UAnimationPhase;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float UAnimationScale; // repeats
            public VAnimationSourceValue VAnimationSource;
            public VAnimationFunctionValue VAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float VAnimationPeriod; // seconds
            public float VAnimationPhase;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float VAnimationScale; // repeats
            public RotationAnimationSourceValue RotationAnimationSource;
            public RotationAnimationFunctionValue RotationAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float RotationAnimationPeriod; // seconds
            public float RotationAnimationPhase;
            /// <summary>
            /// 0 defaults to 360
            /// </summary>
            public float RotationAnimationScale; // degrees
            public RealPoint2d RotationAnimationCenter;
            [TagField(Length = 0x4)]
            public byte[] Padding7;
            public float ZspriteRadiusScale;
            [TagField(Length = 0x14)]
            public byte[] Padding8;
            
            public enum FlagsValue : uint
            {
                InterpolateColorsInHsv,
                AlongLongHuePath,
                RandomRotation
            }
            
            public enum RenderModeValue : short
            {
                ScreenFacing,
                ParallelToDirection,
                PerpendicularToDirection
            }
            
            public enum RenderDirectionSourceValue : short
            {
                FromVelocity,
                FromAcceleration
            }
            
            public enum ShaderFlagsValue : ushort
            {
                SortBias,
                NonlinearTint,
                DonTOverdrawFpWeapon
            }
            
            public enum FramebufferBlendFunctionValue : short
            {
                AlphaBlend,
                Multiply,
                DoubleMultiply,
                Add,
                Subtract,
                ComponentMin,
                ComponentMax,
                AlphaMultiplyAdd
            }
            
            public enum FramebufferFadeModeValue : short
            {
                None,
                FadeWhenPerpendicular,
                FadeWhenParallel
            }
            
            public enum MapFlagsValue : ushort
            {
                Unfiltered
            }
            
            public enum AnchorValue : short
            {
                WithPrimary,
                WithScreenSpace,
                Zsprite
            }
            
            public enum Flags1Value : ushort
            {
                Unfiltered
            }
            
            public enum UAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum UAnimationFunctionValue : short
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
            
            public enum VAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum VAnimationFunctionValue : short
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
            
            public enum RotationAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum RotationAnimationFunctionValue : short
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

