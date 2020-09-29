using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xC8)]
    public class DamageEffect : TagStructure
    {
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0,1]
        public FlagsValue Flags;
        public SideEffectValue SideEffect;
        public CategoryValue Category;
        public FlagsValue1 Flags1;
        /// <summary>
        /// if this is area of effect damage
        /// </summary>
        public float AoeCoreRadius; // world units
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        public Angle DmgInnerConeAngle;
        public DamageOuterConeAngleStructBlock Blah;
        /// <summary>
        /// how much more visible this damage makes a player who is active camouflaged
        /// </summary>
        public float ActiveCamouflageDamage; // [0,1]
        /// <summary>
        /// amount of stun added to damaged unit
        /// </summary>
        public float Stun; // [0,1]
        /// <summary>
        /// damaged unit's stun will never exceed this amount
        /// </summary>
        public float MaximumStun; // [0,1]
        /// <summary>
        /// duration of stun due to this damage
        /// </summary>
        public float StunTime; // seconds
        public float InstantaneousAcceleration; // [0,+inf]
        public float RiderDirectDamageScale;
        public float RiderMaximumTransferDamageScale;
        public float RiderMinimumTransferDamageScale;
        public StringId GeneralDamage;
        public StringId SpecificDamage;
        public float AiStunRadius; // world units
        public Bounds<float> AiStunBounds; // (0-1)
        public float ShakeRadius;
        public float EmpRadius;
        public List<DamageEffectPlayerResponseBlock> PlayerResponses;
        public float Duration; // seconds
        public FadeFunctionValue FadeFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public Angle Rotation; // degrees
        public float Pushback; // world units
        public Bounds<float> Jitter; // world units
        /// <summary>
        /// The wobble function and weight affects how the camera shake oscilates over time.
        /// If the weight is one, then the wobble
        /// function completely scales the translational
        /// and rotational magnitudes.  The less the weight, the less the effect wobble
        /// has.
        /// If the wobble weight is 0 then wobble is completely ignored.
        /// </summary>
        /// <summary>
        /// the effect will last for this duration.
        /// </summary>
        public float Duration1; // seconds
        /// <summary>
        /// a function to envelope the effect's magnitude over time
        /// </summary>
        public FalloffFunctionValue FalloffFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        /// <summary>
        /// random translation in all directions
        /// </summary>
        public float RandomTranslation; // world units
        /// <summary>
        /// random rotation in all directions
        /// </summary>
        public Angle RandomRotation; // degrees
        /// <summary>
        /// a function to perturb the effect's behavior over time
        /// </summary>
        public WobbleFunctionValue WobbleFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public float WobbleFunctionPeriod; // seconds
        /// <summary>
        /// a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be
        /// felt when the wobble function's value is zero.
        /// </summary>
        public float WobbleWeight;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Sound;
        /// <summary>
        /// Controls particle velocities when a damage effect shatters a materal.
        /// 
        /// All particles created within 'forward radius' will
        /// be kicked along the
        /// damage direction with a speed equivalent to 'forward velocity' at the
        /// epicenter of the damage and 0
        /// at the outer radius.  'Forward exponent'
        /// is used to modify the velocity scale.  A low exponent (like 0.5) means
        /// that
        /// particles between the epicenter and the radius will be kicked out with a speed
        /// closer to 'forward velocity' than if
        /// a higher exponent (like 2.0) was used
        /// 
        /// The outward fields work in a similar way, except instead of kicking along
        /// the
        /// damage direction, they get kick away from the damage epicenter.
        /// </summary>
        public float ForwardVelocity; // world units per second
        public float ForwardRadius; // world units
        public float ForwardExponent;
        public float OutwardVelocity; // world units per second
        public float OutwardRadius; // world units
        public float OutwardExponent;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DonTScaleDamageByDistance = 1 << 0,
            /// <summary>
            /// area of effect damage only affects players
            /// </summary>
            AreaDamagePlayersOnly = 1 << 1
        }
        
        public enum SideEffectValue : short
        {
            None,
            Harmless,
            LethalToTheUnsuspecting,
            Emp
        }
        
        public enum CategoryValue : short
        {
            None,
            Falling,
            Bullet,
            Grenade,
            HighExplosive,
            Sniper,
            Melee,
            Flame,
            MountedWeapon,
            Vehicle,
            Plasma,
            Needle,
            Shotgun
        }
        
        [Flags]
        public enum FlagsValue1 : uint
        {
            DoesNotHurtOwner = 1 << 0,
            CanCauseHeadshots = 1 << 1,
            PingsResistantUnits = 1 << 2,
            DoesNotHurtFriends = 1 << 3,
            DoesNotPingUnits = 1 << 4,
            DetonatesExplosives = 1 << 5,
            OnlyHurtsShields = 1 << 6,
            CausesFlamingDeath = 1 << 7,
            DamageIndicatorsAlwaysPointDown = 1 << 8,
            SkipsShields = 1 << 9,
            OnlyHurtsOneInfectionForm = 1 << 10,
            ObsoleteWasCanCauseMultiplayerHeadshots = 1 << 11,
            InfectionFormPop = 1 << 12,
            IgnoreSeatScaleForDirDmg = 1 << 13,
            ForcesHardPing = 1 << 14,
            DoesNotHurtPlayers = 1 << 15
        }
        
        [TagStructure(Size = 0x4)]
        public class DamageOuterConeAngleStructBlock : TagStructure
        {
            public Angle DmgOuterConeAngle;
        }
        
        [TagStructure(Size = 0x4C)]
        public class DamageEffectPlayerResponseBlock : TagStructure
        {
            public ResponseTypeValue ResponseType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// There are seven screen flash types:
            /// 
            /// NONE: DST'= DST
            /// LIGHTEN: DST'= DST(1 - A) + C
            /// DARKEN: DST'= DST(1 - A) - C
            /// MAX:
            /// DST'= MAX[DST(1 - C), (C - A)(1-DST)]
            /// MIN: DST'= MIN[DST(1 - C), (C + A)(1-DST)]
            /// TINT: DST'= DST(1 - C) + (A*PIN[2C - 1,
            /// 0, 1] + A)(1-DST)
            /// INVERT: DST'= DST(1 - C) + A)
            /// 
            /// In the above equations C and A represent the color and alpha of the
            /// screen flash, DST represents the color in the framebuffer before the screen flash is applied, and DST' represents the
            /// color after the screen flash is applied.
            /// </summary>
            public ScreenFlashDefinitionStructBlock ScreenFlash;
            public VibrationDefinitionStructBlock Vibration;
            public DamageEffectSoundEffectDefinitionBlock SoundEffect;
            
            public enum ResponseTypeValue : short
            {
                Shielded,
                Unshielded,
                All
            }
            
            [TagStructure(Size = 0x20)]
            public class ScreenFlashDefinitionStructBlock : TagStructure
            {
                public TypeValue Type;
                public PriorityValue Priority;
                public float Duration; // seconds
                public FadeFunctionValue FadeFunction;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float MaximumIntensity; // [0,1]
                public RealArgbColor Color;
                
                public enum TypeValue : short
                {
                    None,
                    Lighten,
                    Darken,
                    Max,
                    Min,
                    Invert,
                    Tint
                }
                
                public enum PriorityValue : short
                {
                    Low,
                    Medium,
                    High
                }
                
                public enum FadeFunctionValue : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class VibrationDefinitionStructBlock : TagStructure
            {
                public VibrationFrequencyDefinitionStructBlock LowFrequencyVibration;
                public VibrationFrequencyDefinitionStructBlock1 HighFrequencyVibration;
                
                [TagStructure(Size = 0xC)]
                public class VibrationFrequencyDefinitionStructBlock : TagStructure
                {
                    public float Duration; // seconds
                    public MappingFunctionBlock DirtyWhore;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class VibrationFrequencyDefinitionStructBlock1 : TagStructure
                {
                    public float Duration; // seconds
                    public MappingFunctionBlock DirtyWhore;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class DamageEffectSoundEffectDefinitionBlock : TagStructure
            {
                public StringId EffectName;
                public float Duration; // seconds
                public MappingFunctionBlock EffectScaleFunction;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        public enum FadeFunctionValue : short
        {
            Linear,
            Late,
            VeryLate,
            Early,
            VeryEarly,
            Cosine,
            Zero,
            One
        }
        
        public enum FalloffFunctionValue : short
        {
            Linear,
            Late,
            VeryLate,
            Early,
            VeryEarly,
            Cosine,
            Zero,
            One
        }
        
        public enum WobbleFunctionValue : short
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

