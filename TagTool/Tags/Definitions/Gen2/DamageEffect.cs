using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xD4)]
    public class DamageEffect : TagStructure
    {
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0,1]
        public FlagsValue Flags;
        /// <summary>
        /// damage
        /// </summary>
        public SideEffectValue SideEffect;
        public CategoryValue Category;
        public FlagsValue Flags1;
        public float AoeCoreRadius; // world units
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        public Angle DmgInnerConeAngle;
        public Real Blah;
        public float ActiveCamouflageDamage; // [0,1]
        public float Stun; // [0,1]
        public float MaximumStun; // [0,1]
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
        public List<DamageEffectPlayerResponseDefinition> PlayerResponses;
        /// <summary>
        /// temporary camera impulse
        /// </summary>
        public float Duration; // seconds
        public FadeFunctionValue FadeFunction;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public Angle Rotation; // degrees
        public float Pushback; // world units
        public Bounds<float> Jitter; // world units
        /// <summary>
        /// camera shaking
        /// </summary>
        /// <remarks>
        /// The wobble function and weight affects how the camera shake oscilates over time.
        /// If the weight is one, then the wobble function completely scales the translational
        /// and rotational magnitudes.  The less the weight, the less the effect wobble has.
        /// If the wobble weight is 0 then wobble is completely ignored.
        /// </remarks>
        public float Duration2; // seconds
        public FalloffFunctionValue FalloffFunction; // a function to envelope the effect's magnitude over time
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public float RandomTranslation; // world units
        public Angle RandomRotation; // degrees
        public WobbleFunctionValue WobbleFunction; // a function to perturb the effect's behavior over time
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        public float WobbleFunctionPeriod; // seconds
        public float WobbleWeight; // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be felt when the wobble function's value is zero.
        /// <summary>
        /// sound
        /// </summary>
        public CachedTag Sound;
        /// <summary>
        /// breaking effect
        /// </summary>
        /// <remarks>
        /// Controls particle velocities when a damage effect shatters a materal.
        /// 
        /// All particles created within 'forward radius' will be kicked along the
        /// damage direction with a speed equivalent to 'forward velocity' at the
        /// epicenter of the damage and 0 at the outer radius.  'Forward exponent'
        /// is used to modify the velocity scale.  A low exponent (like 0.5) means that
        /// particles between the epicenter and the radius will be kicked out with a speed
        /// closer to 'forward velocity' than if a higher exponent (like 2.0) was used
        /// 
        /// The outward fields work in a similar way, except instead of kicking along the
        /// damage direction, they get kick away from the damage epicenter.
        /// </remarks>
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
        
        [TagStructure(Size = 0x4)]
        public class Real : TagStructure
        {
            public Angle DmgOuterConeAngle;
        }
        
        [TagStructure(Size = 0x58)]
        public class DamageEffectPlayerResponseDefinition : TagStructure
        {
            public ResponseTypeValue ResponseType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            /// <summary>
            /// screen flash
            /// </summary>
            /// <remarks>
            /// There are seven screen flash types:
            /// 
            /// NONE: DST'= DST
            /// LIGHTEN: DST'= DST(1 - A) + C
            /// DARKEN: DST'= DST(1 - A) - C
            /// MAX: DST'= MAX[DST(1 - C), (C - A)(1-DST)]
            /// MIN: DST'= MIN[DST(1 - C), (C + A)(1-DST)]
            /// TINT: DST'= DST(1 - C) + (A*PIN[2C - 1, 0, 1] + A)(1-DST)
            /// INVERT: DST'= DST(1 - C) + A)
            /// 
            /// In the above equations C and A represent the color and alpha of the screen flash, DST represents the color in the framebuffer before the screen flash is applied, and DST' represents the color after the screen flash is applied.
            /// </remarks>
            public ScreenFlashDefinition ScreenFlash;
            /// <summary>
            /// vibration
            /// </summary>
            public VibrationDefinition Vibration;
            /// <summary>
            /// sound effect
            /// </summary>
            public DamageEffectSoundEffectDefinition SoundEffect;
            
            public enum ResponseTypeValue : short
            {
                Shielded,
                Unshielded,
                All
            }
            
            [TagStructure(Size = 0x20)]
            public class ScreenFlashDefinition : TagStructure
            {
                public TypeValue Type;
                public PriorityValue Priority;
                public float Duration; // seconds
                public FadeFunctionValue FadeFunction;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            
            [TagStructure(Size = 0x20)]
            public class VibrationDefinition : TagStructure
            {
                /// <summary>
                /// low frequency vibration
                /// </summary>
                public VibrationFrequencyDefinition LowFrequencyVibration;
                /// <summary>
                /// high frequency vibration
                /// </summary>
                public VibrationFrequencyDefinition HighFrequencyVibration;
                
                [TagStructure(Size = 0x10)]
                public class VibrationFrequencyDefinition : TagStructure
                {
                    public float Duration; // seconds
                    public FunctionDefinition DirtyWhore;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public List<Byte> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class Byte : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageEffectSoundEffectDefinition : TagStructure
            {
                public StringId EffectName;
                public float Duration; // seconds
                public FunctionDefinition EffectScaleFunction;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
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

