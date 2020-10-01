using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "particle", Tag = "part", Size = 0x164)]
    public class Particle : TagStructure
    {
        public FlagsValue Flags;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        [TagField(ValidTags = new [] { "pphy" })]
        public CachedTag Physics;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag MartyTradedHisKidsForThis;
        [TagField(Length = 0x4)]
        public byte[] Padding;
        public Bounds<float> Lifespan; // seconds
        public float FadeInTime;
        public float FadeOutTime;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag CollisionEffect;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag DeathEffect;
        /// <summary>
        /// in the distance, don't get any smaller than this size on the screen
        /// </summary>
        public float MinimumSize; // pixels
        [TagField(Length = 0x8)]
        public byte[] Padding1;
        /// <summary>
        /// when created, the radius is multiplied by the first number. at the end of the lifetime, the radius is multiplied by the
        /// second number.
        /// </summary>
        public Bounds<float> RadiusAnimation;
        [TagField(Length = 0x4)]
        public byte[] Padding2;
        /// <summary>
        /// rate at which the particle animates
        /// </summary>
        public Bounds<float> AnimationRate; // frames per second
        /// <summary>
        /// the decrease in the frame rate caused by a collision
        /// </summary>
        public float ContactDeterioration;
        /// <summary>
        /// begin fading the particle out when it's smaller than this size on the screen
        /// </summary>
        public float FadeStartSize; // pixels
        /// <summary>
        /// kill the particle when it's smaller than this size on the screen
        /// </summary>
        public float FadeEndSize; // pixels
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        /// <summary>
        /// the index of the first sequence in the bitmap group used by this particle
        /// </summary>
        public short FirstSequenceIndex;
        public short InitialSequenceCount;
        public short LoopingSequenceCount;
        public short FinalSequenceCount;
        [TagField(Length = 0xC)]
        public byte[] Padding4;
        public OrientationValue Orientation;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        [TagField(Length = 0x28)]
        public byte[] Padding6;
        public ShaderFlagsValue ShaderFlags;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        public FramebufferFadeModeValue FramebufferFadeMode;
        public MapFlagsValue MapFlags;
        [TagField(Length = 0x1C)]
        public byte[] Padding7;
        /// <summary>
        /// Optional multitextured second map
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap1;
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
        public byte[] Padding8;
        public float ZspriteRadiusScale;
        [TagField(Length = 0x14)]
        public byte[] Padding9;
        
        public enum FlagsValue : uint
        {
            CanAnimateBackwards,
            AnimationStopsAtRest,
            AnimationStartsOnRandomFrame,
            AnimateOncePerFrame,
            DiesAtRest,
            DiesOnContactWithStructure,
            TintFromDiffuseTexture,
            DiesOnContactWithWater,
            DiesOnContactWithAir,
            /// <summary>
            /// don't cast world-lights onto this particle
            /// </summary>
            SelfIlluminated,
            RandomHorizontalMirroring,
            RandomVerticalMirroring
        }
        
        public enum OrientationValue : short
        {
            ScreenFacing,
            ParallelToDirection,
            PerpendicularToDirection
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

