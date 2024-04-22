using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "contrail", Tag = "cont", Size = 0xF0)]
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
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
        [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public ShaderFlagsValue ShaderFlags;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        public FramebufferFadeModeValue FramebufferFadeMode;
        public MapFlagsValue MapFlags;
        [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        /// <summary>
        /// Optional multitextured second map
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap1;
        public AnchorValue Anchor;
        public FlagsValue1 Flags1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
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
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
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
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
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
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public float ZspriteRadiusScale;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        public List<ContrailPointStatesBlock> PointStates;
        
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
        
        [Flags]
        public enum FlagsValue1 : ushort
        {
            Unfiltered = 1 << 0
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
        
        [TagStructure(Size = 0x40)]
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

