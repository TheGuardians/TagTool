using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "particle_system", Tag = "pctl", Size = 0x68)]
    public class ParticleSystem : TagStructure
    {
        [TagField(Length = 0x4)]
        public byte[] Padding;
        [TagField(Length = 0x34)]
        public byte[] Padding1;
        /// <summary>
        /// These settings affect the behavior of the system's origin.
        /// </summary>
        [TagField(ValidTags = new [] { "pphy" })]
        public CachedTag PointPhysics;
        public SystemUpdatePhysicsValue SystemUpdatePhysics;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        public PhysicsFlagsValue PhysicsFlags;
        public List<ParticleSystemPhysicsConstantsBlock> PhysicsConstants;
        public List<ParticleSystemTypesBlock> ParticleTypes;
        
        public enum SystemUpdatePhysicsValue : short
        {
            Default,
            Explosion
        }
        
        [Flags]
        public enum PhysicsFlagsValue : uint
        {
        }
        
        [TagStructure(Size = 0x4)]
        public class ParticleSystemPhysicsConstantsBlock : TagStructure
        {
            /// <summary>
            /// The meaning of this constant depends on the selected physics creation/update function.
            /// </summary>
            public float K;
        }
        
        [TagStructure(Size = 0x80)]
        public class ParticleSystemTypesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public short InitialParticleCount;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public ComplexSpriteRenderModesValue ComplexSpriteRenderModes;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            public float Radius; // world units
            [TagField(Length = 0x24)]
            public byte[] Padding2;
            /// <summary>
            /// This controls the initial placement of particles.
            /// </summary>
            public ParticleCreationPhysicsValue ParticleCreationPhysics;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            public PhysicsFlagsValue PhysicsFlags;
            public List<ParticleSystemPhysicsConstantsBlock> PhysicsConstants;
            public List<ParticleSystemTypeStatesBlock> States;
            public List<ParticleSystemTypeParticleStatesBlock> ParticleStates;
            
            [Flags]
            public enum FlagsValue : uint
            {
                TypeStatesLoop = 1 << 0,
                ForwardBackward = 1 << 1,
                ParticleStatesLoop = 1 << 2,
                ForwardBackward1 = 1 << 3,
                ParticlesDieInWater = 1 << 4,
                ParticlesDieInAir = 1 << 5,
                ParticlesDieOnGround = 1 << 6,
                /// <summary>
                /// if the complex sprite mode is rotational and this flag is set, the sideways sequence is contains an
                /// animation rather than a set of permutations.
                /// </summary>
                RotationalSpritesAnimateSideways = 1 << 7,
                Disabled = 1 << 8,
                TintByEffectColor = 1 << 9,
                InitialCountScalesWithEffect = 1 << 10,
                MinimumCountScalesWithEffect = 1 << 11,
                CreationRateScalesWithEffect = 1 << 12,
                ScaleScalesWithEffect = 1 << 13,
                AnimationRateScalesWithEffect = 1 << 14,
                RotationRateScalesWithEffect = 1 << 15,
                DonTDrawInFirstPerson = 1 << 16,
                DonTDrawInThirdPerson = 1 << 17
            }
            
            public enum ComplexSpriteRenderModesValue : short
            {
                Simple,
                Rotational
            }
            
            public enum ParticleCreationPhysicsValue : short
            {
                Default,
                Explosion,
                Jet
            }
            
            [Flags]
            public enum PhysicsFlagsValue : uint
            {
            }
            
            [TagStructure(Size = 0x4)]
            public class ParticleSystemPhysicsConstantsBlock : TagStructure
            {
                /// <summary>
                /// The meaning of this constant depends on the selected physics creation/update function.
                /// </summary>
                public float K;
            }
            
            [TagStructure(Size = 0xC0)]
            public class ParticleSystemTypeStatesBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                /// <summary>
                /// Time in this state.
                /// </summary>
                public Bounds<float> DurationBounds; // seconds
                /// <summary>
                /// Time spent in transition to next state.
                /// </summary>
                public Bounds<float> TransitionTimeBounds; // seconds
                [TagField(Length = 0x4)]
                public byte[] Padding;
                /// <summary>
                /// This value will be multiplied into the scale computed by the particles' state.
                /// </summary>
                public float ScaleMultiplier;
                /// <summary>
                /// This value will be multiplied into the animation rate computed by the particles' state.
                /// </summary>
                public float AnimationRateMultiplier;
                /// <summary>
                /// This value will be multiplied into the rotation rate computed by the particles' state.
                /// </summary>
                public float RotationRateMultiplier;
                /// <summary>
                /// This value will be multiplied into the color computed by the particles' state.
                /// </summary>
                public RealArgbColor ColorMultiplier;
                /// <summary>
                /// This value will be multiplied into the radius computed by the type.
                /// </summary>
                public float RadiusMultiplier;
                public float MinimumParticleCount;
                public float ParticleCreationRate; // particles per second
                [TagField(Length = 0x54)]
                public byte[] Padding1;
                /// <summary>
                /// This controls the placement of particles created during this state.
                /// </summary>
                public ParticleCreationPhysicsValue ParticleCreationPhysics;
                /// <summary>
                /// This controls the motion of particles during this state.
                /// </summary>
                public ParticleUpdatePhysicsValue ParticleUpdatePhysics;
                public List<ParticleSystemPhysicsConstantsBlock> PhysicsConstants;
                
                public enum ParticleCreationPhysicsValue : short
                {
                    Default,
                    Explosion,
                    Jet
                }
                
                public enum ParticleUpdatePhysicsValue : short
                {
                    Default
                }
                
                [TagStructure(Size = 0x4)]
                public class ParticleSystemPhysicsConstantsBlock : TagStructure
                {
                    /// <summary>
                    /// The meaning of this constant depends on the selected physics creation/update function.
                    /// </summary>
                    public float K;
                }
            }
            
            [TagStructure(Size = 0x178)]
            public class ParticleSystemTypeParticleStatesBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                /// <summary>
                /// Time in this state.
                /// </summary>
                public Bounds<float> DurationBounds; // seconds
                /// <summary>
                /// Time spent in transition to next state.
                /// </summary>
                public Bounds<float> TransitionTimeBounds; // seconds
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmaps;
                public short SequenceIndex;
                [TagField(Length = 0x2)]
                public byte[] Padding;
                [TagField(Length = 0x4)]
                public byte[] Padding1;
                /// <summary>
                /// Apparent size of the particles.
                /// </summary>
                public Bounds<float> Scale; // world units per pixel
                /// <summary>
                /// Rate of sprite animation.
                /// </summary>
                public Bounds<float> AnimationRate; // frames per second
                /// <summary>
                /// Rate of texture rotation.
                /// </summary>
                public Bounds<Angle> RotationRate; // degrees per second
                /// <summary>
                /// Particle will have a random color in the range determined by these two colors.
                /// </summary>
                public RealArgbColor Color1;
                /// <summary>
                /// Particle will have a random color in the range determined by these two colors.
                /// </summary>
                public RealArgbColor Color2;
                /// <summary>
                /// This value will be multiplied into the radius computed by the type.
                /// </summary>
                public float RadiusMultiplier;
                [TagField(ValidTags = new [] { "pphy" })]
                public CachedTag PointPhysics;
                [TagField(Length = 0x24)]
                public byte[] Padding2;
                [TagField(Length = 0x28)]
                public byte[] Padding3;
                public ShaderFlagsValue ShaderFlags;
                public FramebufferBlendFunctionValue FramebufferBlendFunction;
                public FramebufferFadeModeValue FramebufferFadeMode;
                public MapFlagsValue MapFlags;
                [TagField(Length = 0x1C)]
                public byte[] Padding4;
                /// <summary>
                /// Optional multitextured second map
                /// </summary>
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                public AnchorValue Anchor;
                public FlagsValue Flags;
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
                public byte[] Padding5;
                public float ZspriteRadiusScale;
                [TagField(Length = 0x14)]
                public byte[] Padding6;
                public List<ParticleSystemPhysicsConstantsBlock> PhysicsConstants;
                
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
                    AlphaMultiplyAdd
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
                public enum FlagsValue : ushort
                {
                    Unfiltered = 1 << 0
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
                
                [TagStructure(Size = 0x4)]
                public class ParticleSystemPhysicsConstantsBlock : TagStructure
                {
                    /// <summary>
                    /// The meaning of this constant depends on the selected physics creation/update function.
                    /// </summary>
                    public float K;
                }
            }
        }
    }
}

