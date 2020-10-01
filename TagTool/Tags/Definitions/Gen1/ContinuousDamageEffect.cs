using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "continuous_damage_effect", Tag = "cdmg", Size = 0x200)]
    public class ContinuousDamageEffect : TagStructure
    {
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0,1]
        [TagField(Length = 0x18)]
        public byte[] Padding;
        public float LowFrequency; // [0,1]
        public float HighFrequency; // [0,1]
        [TagField(Length = 0x18)]
        public byte[] Padding1;
        /// <summary>
        /// random translation in all directions
        /// </summary>
        public float RandomTranslation; // world units
        /// <summary>
        /// random rotation in all directions
        /// </summary>
        public Angle RandomRotation; // degrees
        [TagField(Length = 0xC)]
        public byte[] Padding2;
        /// <summary>
        /// a function to perturb the effect's behavior over time
        /// </summary>
        public WobbleFunctionValue WobbleFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        public float WobbleFunctionPeriod; // seconds
        /// <summary>
        /// a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be
        /// felt when the wobble function's value is zero.
        /// </summary>
        public float WobbleWeight;
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        [TagField(Length = 0x14)]
        public byte[] Padding5;
        [TagField(Length = 0x8)]
        public byte[] Padding6;
        [TagField(Length = 0xA0)]
        public byte[] Padding7;
        public SideEffectValue SideEffect;
        public CategoryValue Category;
        public FlagsValue Flags;
        [TagField(Length = 0x4)]
        public byte[] Padding8;
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        /// <summary>
        /// zero damages passengers in vehicles, one does not
        /// </summary>
        public float VehiclePassthroughPenalty; // [0,1]
        [TagField(Length = 0x4)]
        public byte[] Padding9;
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
        public byte[] Padding10;
        public float InstantaneousAcceleration; // [0,+inf]
        [TagField(Length = 0x4)]
        public byte[] Padding11;
        [TagField(Length = 0x4)]
        public byte[] Padding12;
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
        public byte[] Padding13;
        
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
        
        public enum FlagsValue : uint
        {
            DoesNotHurtOwner,
            CanCauseHeadshots,
            PingsResistantUnits,
            DoesNotHurtFriends,
            DoesNotPingUnits,
            DetonatesExplosives,
            OnlyHurtsShields,
            CausesFlamingDeath,
            DamageIndicatorsAlwaysPointDown,
            SkipsShields,
            OnlyHurtsOneInfectionForm,
            CanCauseMultiplayerHeadshots,
            InfectionFormPop
        }
    }
}

