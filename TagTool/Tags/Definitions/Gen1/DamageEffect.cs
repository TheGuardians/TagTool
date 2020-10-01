using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0x2A0)]
    public class DamageEffect : TagStructure
    {
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0,1]
        public FlagsValue Flags;
        [TagField(Length = 0x14)]
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
        public TypeValue Type;
        public PriorityValue Priority;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        public float Duration; // seconds
        public FadeFunctionValue FadeFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x8)]
        public byte[] Padding3;
        public float MaximumIntensity; // [0,1]
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        public RealArgbColor Color;
        public float Frequency; // [0,1]
        public float Duration1; // seconds
        public FadeFunction1Value FadeFunction1;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        [TagField(Length = 0x8)]
        public byte[] Padding6;
        public float Frequency1; // [0,1]
        public float Duration2; // seconds
        public FadeFunction2Value FadeFunction2;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        [TagField(Length = 0x8)]
        public byte[] Padding8;
        [TagField(Length = 0x4)]
        public byte[] Padding9;
        [TagField(Length = 0x10)]
        public byte[] Padding10;
        public float Duration3; // seconds
        public FadeFunction3Value FadeFunction3;
        [TagField(Length = 0x2)]
        public byte[] Padding11;
        public Angle Rotation; // degrees
        public float Pushback; // world units
        public Bounds<float> Jitter; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding12;
        [TagField(Length = 0x4)]
        public byte[] Padding13;
        public Angle Angle; // degrees
        [TagField(Length = 0x4)]
        public byte[] Padding14;
        [TagField(Length = 0xC)]
        public byte[] Padding15;
        /// <summary>
        /// the effect will last for this duration.
        /// </summary>
        public float Duration4; // seconds
        /// <summary>
        /// a function to envelope the effect's magnitude over time
        /// </summary>
        public FalloffFunctionValue FalloffFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding16;
        /// <summary>
        /// random translation in all directions
        /// </summary>
        public float RandomTranslation; // world units
        /// <summary>
        /// random rotation in all directions
        /// </summary>
        public Angle RandomRotation; // degrees
        [TagField(Length = 0xC)]
        public byte[] Padding17;
        /// <summary>
        /// a function to perturb the effect's behavior over time
        /// </summary>
        public WobbleFunctionValue WobbleFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding18;
        public float WobbleFunctionPeriod; // seconds
        /// <summary>
        /// a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be
        /// felt when the wobble function's value is zero.
        /// </summary>
        public float WobbleWeight;
        [TagField(Length = 0x4)]
        public byte[] Padding19;
        [TagField(Length = 0x14)]
        public byte[] Padding20;
        [TagField(Length = 0x8)]
        public byte[] Padding21;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Sound;
        [TagField(Length = 0x70)]
        public byte[] Padding22;
        /// <summary>
        /// Controls particle velocities when a damage effect shatters a materal.
        /// </summary>
        public float ForwardVelocity; // world units per second
        public float ForwardRadius; // world units
        public float ForwardExponent;
        [TagField(Length = 0xC)]
        public byte[] Padding23;
        public float OutwardVelocity; // world units per second
        public float OutwardRadius; // world units
        public float OutwardExponent;
        [TagField(Length = 0xC)]
        public byte[] Padding24;
        public SideEffectValue SideEffect;
        public CategoryValue Category;
        public Flags1Value Flags1;
        /// <summary>
        /// if this is area of effect damage
        /// </summary>
        public float AoeCoreRadius; // world units
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        /// <summary>
        /// zero damages passengers in vehicles, one does not
        /// </summary>
        public float VehiclePassthroughPenalty; // [0,1]
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
        [TagField(Length = 0x4)]
        public byte[] Padding25;
        public float InstantaneousAcceleration; // [0,+inf]
        [TagField(Length = 0x4)]
        public byte[] Padding26;
        [TagField(Length = 0x4)]
        public byte[] Padding27;
        public float Dirt; // [0,+inf]
        public float Sand; // [0,+inf]
        public float Stone; // [0,+inf]
        public float Snow; // [0,+inf]
        public float Wood; // [0,+inf]
        public float MetalHollow; // [0,+inf]
        public float MetalThin; // [0,+inf]
        public float MetalThick; // [0,+inf]
        public float Rubber; // [0,+inf]
        public float Glass; // [0,+inf]
        public float ForceField; // [0,+inf]
        public float Grunt; // [0,+inf]
        public float HunterArmor; // [0,+inf]
        public float HunterSkin; // [0,+inf]
        public float Elite; // [0,+inf]
        public float Jackal; // [0,+inf]
        public float JackalEnergyShield; // [0,+inf]
        public float Engineer; // [0,+inf]
        public float EngineerForceField; // [0,+inf]
        public float FloodCombatForm; // [0,+inf]
        public float FloodCarrierForm; // [0,+inf]
        public float Cyborg; // [0,+inf]
        public float CyborgEnergyShield; // [0,+inf]
        public float ArmoredHuman; // [0,+inf]
        public float Human; // [0,+inf]
        public float Sentinel; // [0,+inf]
        public float Monitor; // [0,+inf]
        public float Plastic; // [0,+inf]
        public float Water; // [0,+inf]
        public float Leaves; // [0,+inf]
        public float EliteEnergyShield; // [0,+inf]
        public float Ice; // [0,+inf]
        public float HunterShield; // [0,+inf]
        [TagField(Length = 0x1C)]
        public byte[] Padding28;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DonTScaleDamageByDistance = 1 << 0
        }
        
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
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
        }
        
        public enum FadeFunction1Value : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
        }
        
        public enum FadeFunction2Value : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
        }
        
        public enum FadeFunction3Value : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
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
        public enum Flags1Value : uint
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
            CanCauseMultiplayerHeadshots = 1 << 11,
            InfectionFormPop = 1 << 12
        }
    }
}

