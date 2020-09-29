using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "contrail", Tag = "cont", Size = 0x104)]
    public class Contrail : TagStructure
    {
        public FlagsValue Flags;
        public ScaleFlagsValue ScaleFlags; // these flags determine which fields are scaled by the contrail density
        /// <summary>
        /// point creation
        /// </summary>
        public float PointGenerationRate; // points per second
        public Bounds<float> PointVelocity; // world units per second
        public Angle PointVelocityConeAngle; // degrees
        public float InheritedVelocityFraction; // fraction of parent object's velocity that is inherited by contrail points.
        /// <summary>
        /// rendering
        /// </summary>
        public RenderTypeValue RenderType; // this specifies how the contrail is oriented in space
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public float TextureRepeatsU; // texture repeats per contrail segment
        public float TextureRepeatsV; // texture repeats across contrail width
        public float TextureAnimationU; // repeats per second
        public float TextureAnimationV; // repeats per second
        public float AnimationRate; // frames per second
        public CachedTag Bitmap;
        public short FirstSequenceIndex;
        public short SequenceCount;
        [TagField(Flags = Padding, Length = 40)]
        public byte[] Padding2;
        public ShaderFlagsValue ShaderFlags;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        public FramebufferFadeModeValue FramebufferFadeMode;
        public MapFlagsValue MapFlags;
        [TagField(Flags = Padding, Length = 28)]
        public byte[] Padding3;
        /// <summary>
        /// Secondary Map
        /// </summary>
        /// <remarks>
        /// Optional multitextured second map
        /// </remarks>
        public CachedTag Bitmap1;
        public AnchorValue Anchor;
        public FlagsValue Flags2;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding4;
        public UAnimationFunctionValue UAnimationFunction;
        public float UAnimationPeriod; // seconds
        public float UAnimationPhase;
        public float UAnimationScale; // repeats
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding5;
        public VAnimationFunctionValue VAnimationFunction;
        public float VAnimationPeriod; // seconds
        public float VAnimationPhase;
        public float VAnimationScale; // repeats
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding6;
        public RotationAnimationFunctionValue RotationAnimationFunction;
        public float RotationAnimationPeriod; // seconds
        public float RotationAnimationPhase;
        public float RotationAnimationScale; // degrees
        public RealPoint2d RotationAnimationCenter;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding7;
        public float ZspriteRadiusScale;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding8;
        public List<ContrailPointState> PointStates;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            FirstPointUnfaded = 1 << 0,
            LastPointUnfaded = 1 << 1,
            PointsStartPinnedToMedia = 1 << 2,
            PointsStartPinnedToGround = 1 << 3,
            PointsAlwaysPinnedToMedia = 1 << 4,
            PointsAlwaysPinnedToGround = 1 << 5,
            EdgeEffectFadesSlowly = 1 << 6,
            DontTInheritObjectChangeColor = 1 << 7
        }
        
        [Flags]
        public enum ScaleFlagsValue : ushort
        {
            PointGenerationRate = 1 << 0,
            PointVelocity = 1 << 1,
            PointVelocityDelta = 1 << 2,
            PointVelocityConeAngle = 1 << 3,
            InheritedVelocityFraction = 1 << 4,
            SequenceAnimationRate = 1 << 5,
            TextureScaleU = 1 << 6,
            TextureScaleV = 1 << 7,
            TextureAnimationU = 1 << 8,
            TextureAnimationV = 1 << 9
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
        
        [Flags]
        public enum ShaderFlagsValue : ushort
        {
            SortBias = 1 << 0,
            NonlinearTint = 1 << 1,
            DonTOverdrawFpWeapon = 1 << 2
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
            AlphaMultiplyAdd,
            ConstantColorBlend,
            InverseConstantColorBlend,
            None
        }
        
        public enum FramebufferFadeModeValue : short
        {
            None,
            FadeWhenPerpendicular,
            FadeWhenParallel
        }
        
        [Flags]
        public enum MapFlagsValue : ushort
        {
            Unfiltered = 1 << 0
        }
        
        public enum AnchorValue : short
        {
            WithPrimary,
            WithScreenSpace,
            Zsprite
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
        
        [TagStructure(Size = 0x48)]
        public class ContrailPointState : TagStructure
        {
            /// <summary>
            /// state timing
            /// </summary>
            public Bounds<float> Duration; // seconds:seconds
            public Bounds<float> TransitionDuration; // seconds
            /// <summary>
            /// point variables
            /// </summary>
            public CachedTag Physics;
            public float Width; // world units
            public RealArgbColor ColorLowerBound; // contrail color at this point
            public RealArgbColor ColorUpperBound; // contrail color at this point
            public ScaleFlagsValue ScaleFlags; // these flags determine which fields are scaled by the contrail density
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                Duration = 1 << 0,
                DurationDelta = 1 << 1,
                TransitionDuration = 1 << 2,
                TransitionDurationDelta = 1 << 3,
                Width = 1 << 4,
                Color = 1 << 5
            }
        }
    }
}

