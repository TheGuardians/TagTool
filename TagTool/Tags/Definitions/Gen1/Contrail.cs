using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "contrail", Tag = "cont", Size = 0x144)]
    public class Contrail : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// these flags determine which fields are scaled by the contrail density
        /// </summary>
        public ScaleFlagsValue ScaleFlags;
        /// <summary>
        /// this many points are generated per second
        /// </summary>
        public float PointGenerationRate; // points per second
        /// <summary>
        /// velocity added to each point's initial velocity
        /// </summary>
        public Bounds<float> PointVelocity; // world units per second
        /// <summary>
        /// initial velocity is inside the cone defined by the marker's forward vector and this angle
        /// </summary>
        public Angle PointVelocityConeAngle; // degrees
        /// <summary>
        /// fraction of parent object's velocity that is inherited by contrail points.
        /// </summary>
        public float InheritedVelocityFraction;
        /// <summary>
        /// this specifies how the contrail is oriented in space
        /// </summary>
        public RenderTypeValue RenderType;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        /// <summary>
        /// texture repeats per contrail segment
        /// </summary>
        public float TextureRepeatsU;
        /// <summary>
        /// texture repeats across contrail width
        /// </summary>
        public float TextureRepeatsV;
        /// <summary>
        /// the texture along the contrail is animated by this value
        /// </summary>
        public float TextureAnimationU; // repeats per second
        /// <summary>
        /// the texture across the contrail is animated by this value
        /// </summary>
        public float TextureAnimationV; // repeats per second
        public float AnimationRate; // frames per second
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        public short FirstSequenceIndex;
        public short SequenceCount;
        [TagField(Length = 0x40)]
        public byte[] Padding1;
        [TagField(Length = 0x28)]
        public byte[] Padding2;
        public ShaderFlagsValue ShaderFlags;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        public FramebufferFadeModeValue FramebufferFadeMode;
        public MapFlagsValue MapFlags;
        [TagField(Length = 0x1C)]
        public byte[] Padding3;
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
        public byte[] Padding4;
        public float ZspriteRadiusScale;
        [TagField(Length = 0x14)]
        public byte[] Padding5;
        public List<ContrailPointStatesBlock> PointStates;
        
        public enum FlagsValue : ushort
        {
            FirstPointUnfaded,
            LastPointUnfaded,
            PointsStartPinnedToMedia,
            PointsStartPinnedToGround,
            PointsAlwaysPinnedToMedia,
            PointsAlwaysPinnedToGround,
            EdgeEffectFadesSlowly
        }
        
        public enum ScaleFlagsValue : ushort
        {
            PointGenerationRate,
            PointVelocity,
            PointVelocityDelta,
            PointVelocityConeAngle,
            InheritedVelocityFraction,
            SequenceAnimationRate,
            TextureScaleU,
            TextureScaleV,
            TextureAnimationU,
            TextureAnimationV
        }
        
        public enum RenderTypeValue : short
        {
            VerticalOrientation,
            HorizontalOrientation,
            MediaMapped,
            GroundMapped,
            ViewerFacing,
            DoubleMarkerLinked
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
        
        [TagStructure(Size = 0x68)]
        public class ContrailPointStatesBlock : TagStructure
        {
            /// <summary>
            /// the time a point spends in this state
            /// </summary>
            public Bounds<float> Duration; // seconds:seconds
            /// <summary>
            /// the time a point takes to transition to the next state
            /// </summary>
            public Bounds<float> TransitionDuration; // seconds
            [TagField(ValidTags = new [] { "pphy" })]
            public CachedTag Physics;
            [TagField(Length = 0x20)]
            public byte[] Padding;
            /// <summary>
            /// contrail width at this point
            /// </summary>
            public float Width; // world units
            /// <summary>
            /// contrail color at this point
            /// </summary>
            public RealArgbColor ColorLowerBound;
            /// <summary>
            /// contrail color at this point
            /// </summary>
            public RealArgbColor ColorUpperBound;
            /// <summary>
            /// these flags determine which fields are scaled by the contrail density
            /// </summary>
            public ScaleFlagsValue ScaleFlags;
            
            public enum ScaleFlagsValue : uint
            {
                Duration,
                DurationDelta,
                TransitionDuration,
                TransitionDurationDelta,
                Width,
                Color
            }
        }
    }
}

