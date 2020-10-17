using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "actor_variant", Tag = "actv", Size = 0x238)]
    public class ActorVariant : TagStructure
    {
        public FlagsValue Flags;
        [TagField(ValidTags = new [] { "actr" })]
        public CachedTag ActorDefinition;
        [TagField(ValidTags = new [] { "unit" })]
        public CachedTag Unit;
        [TagField(ValidTags = new [] { "actv" })]
        public CachedTag MajorVariant;
        [TagField(Length = 0x18)]
        public byte[] Padding;
        /// <summary>
        /// note: only the flood combat forms will ever try to switch movement types voluntarily during combat
        /// </summary>
        /// <summary>
        /// when we have a choice of movement types, which type we will use
        /// </summary>
        public MovementTypeValue MovementType;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        /// <summary>
        /// actors that start their movement try to maintain this fraction of crouched actors
        /// </summary>
        public float InitialCrouchChance; // [0,1]
        /// <summary>
        /// when switching movement types, how long we will stay crouched for before running
        /// </summary>
        public Bounds<float> CrouchTime; // seconds
        /// <summary>
        /// when switching movement types, how long we will run for before slowing to a crouch
        /// </summary>
        public Bounds<float> RunTime; // seconds
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag Weapon;
        /// <summary>
        /// we can only fire our weapon at targets within this distance
        /// </summary>
        public float MaximumFiringDistance; // world units
        /// <summary>
        /// how many times per second we pull the trigger (zero = continuously held down)
        /// </summary>
        public float RateOfFire;
        /// <summary>
        /// error added to every projectile we fire
        /// </summary>
        public Angle ProjectileError; // degrees
        public Bounds<float> FirstBurstDelayTime; // seconds
        public float NewTargetFiringPatternTime; // seconds
        public float SurpriseDelayTime; // seconds
        public float SurpriseFireWildlyTime; // seconds
        public float DeathFireWildlyChance; // [0,1]
        public float DeathFireWildlyTime; // seconds
        public Bounds<float> DesiredCombatRange; // world units
        /// <summary>
        /// custom standing gun offset for overriding the default in the base actor
        /// </summary>
        public RealVector3d CustomStandGunOffset;
        /// <summary>
        /// custom crouching gun offset for overriding the default in the base actor
        /// </summary>
        public RealVector3d CustomCrouchGunOffset;
        /// <summary>
        /// how well our bursts track moving targets. 0.0= fire at the position they were standing when we started the burst. 1.0=
        /// fire at current position
        /// </summary>
        public float TargetTracking; // [0,1]
        /// <summary>
        /// how much we lead moving targets. 0.0= no prediction. 1.0= predict completely.
        /// </summary>
        public float TargetLeading; // [0,1]
        /// <summary>
        /// what fraction of its normal damage our weapon inflicts (zero = no modifier)
        /// </summary>
        public float WeaponDamageModifier;
        /// <summary>
        /// only used if weapon damage modifier is zero... how much damage we should deliver to the target per second while firing a
        /// burst at them (zero = use weapon default)
        /// </summary>
        public float DamagePerSecond;
        /// <summary>
        /// at the start of every burst we pick a random point near the target to fire at, on either the left or the right side.
        /// the
        /// burst origin angle controls whether this error is exactly horizontal or might have some vertical component.
        /// 
        /// over the
        /// course of the burst we move our projectiles back in the opposite direction towards the target. this return motion is also
        /// controlled by an angle that specifies how close to the horizontal it is.
        /// 
        /// for example if the burst origin angle and the
        /// burst return angle were both zero, and the return length was the same as the burst length, every burst would start the
        /// same amount away from the target (on either the left or right) and move back to exactly over the target at the end of the
        /// burst.
        /// </summary>
        /// <summary>
        /// how far away from the target the starting point is
        /// </summary>
        public float BurstOriginRadius; // world units
        /// <summary>
        /// the range from the horizontal that our starting error can be
        /// </summary>
        public Angle BurstOriginAngle; // degrees
        /// <summary>
        /// how far the burst point moves back towards the target (could be negative)
        /// </summary>
        public Bounds<float> BurstReturnLength; // world units
        /// <summary>
        /// the range from the horizontal that the return direction can be
        /// </summary>
        public Angle BurstReturnAngle; // degrees
        /// <summary>
        /// how long each burst we fire is
        /// </summary>
        public Bounds<float> BurstDuration; // seconds
        /// <summary>
        /// how long we wait between bursts
        /// </summary>
        public Bounds<float> BurstSeparation; // seconds
        /// <summary>
        /// the maximum rate at which we can sweep our fire (zero = unlimited)
        /// </summary>
        public Angle BurstAngularVelocity; // degrees per second
        [TagField(Length = 0x4)]
        public byte[] Padding2;
        /// <summary>
        /// damage modifier for special weapon fire (applied in addition to the normal damage modifier. zero = no change)
        /// </summary>
        public float SpecialDamageModifier; // [0,1]
        /// <summary>
        /// projectile error angle for special weapon fire (applied in addition to the normal error)
        /// </summary>
        public Angle SpecialProjectileError; // degrees
        /// <summary>
        /// a firing pattern lets you modify the properties of an actor's burst geometry. actors choose which firing pattern to use
        /// based on their current state:
        ///      'new-target' when the target just appeared
        ///      'moving' when the actor is moving
        ///     
        /// 'berserk' if the actor is berserk
        /// if none of these apply, no firing pattern is used.
        /// 
        /// the default values in the burst
        /// geometry are multiplied by any non-zero modifiers in the firing pattern.
        /// </summary>
        /// <summary>
        /// burst duration multiplier for newly appeared targets (zero = unchanged)
        /// </summary>
        public float NewTargetBurstDuration;
        /// <summary>
        /// burst separation multiplier for newly appeared targets (zero = unchanged)
        /// </summary>
        public float NewTargetBurstSeparation;
        /// <summary>
        /// rate-of-fire multiplier for newly appeared targets (zero = unchanged)
        /// </summary>
        public float NewTargetRateOfFire;
        /// <summary>
        /// error multiplier for newly appeared targets (zero = unchanged)
        /// </summary>
        public float NewTargetProjectileError;
        [TagField(Length = 0x8)]
        public byte[] Padding3;
        /// <summary>
        /// burst duration multiplier when the actor is moving (zero = unchanged)
        /// </summary>
        public float MovingBurstDuration;
        /// <summary>
        /// burst separation multiplier when the actor is moving (zero = unchanged)
        /// </summary>
        public float MovingBurstSeparation;
        /// <summary>
        /// rate-of-fire multiplier when the actor is moving (zero = unchanged)
        /// </summary>
        public float MovingRateOfFire;
        /// <summary>
        /// error multiplier when the actor is moving (zero = unchanged)
        /// </summary>
        public float MovingProjectileError;
        [TagField(Length = 0x8)]
        public byte[] Padding4;
        /// <summary>
        /// burst duration multiplier when the actor is berserk (zero = unchanged)
        /// </summary>
        public float BerserkBurstDuration;
        /// <summary>
        /// burst separation multiplier when the actor is berserk (zero = unchanged)
        /// </summary>
        public float BerserkBurstSeparation;
        /// <summary>
        /// rate-of-fire multiplier when the actor is berserk (zero = unchanged)
        /// </summary>
        public float BerserkRateOfFire;
        /// <summary>
        /// error multiplier when the actor is berserk (zero = unchanged)
        /// </summary>
        public float BerserkProjectileError;
        [TagField(Length = 0x8)]
        public byte[] Padding5;
        /// <summary>
        /// we try to aim our shots super-ballistically if target is outside this range (zero = never)
        /// </summary>
        public float SuperBallisticRange;
        /// <summary>
        /// we offset our burst targets randomly by this range when firing at non-visible enemies (zero = never)
        /// </summary>
        public float BombardmentRange;
        /// <summary>
        /// any custom vision range that this actor variant has (zero = normal)
        /// </summary>
        public float ModifiedVisionRange;
        /// <summary>
        /// the type of special weapon fire that we can use
        /// </summary>
        public SpecialFireModeValue SpecialFireMode;
        /// <summary>
        /// when we will decide to use our special weapon fire mode
        /// </summary>
        public SpecialFireSituationValue SpecialFireSituation;
        /// <summary>
        /// how likely we are to use our special weapon fire mode
        /// </summary>
        public float SpecialFireChance; // [0,1]
        /// <summary>
        /// how long we must wait between uses of our special weapon fire mode
        /// </summary>
        public float SpecialFireDelay; // seconds
        /// <summary>
        /// how close an enemy target must get before triggering a melee charge
        /// </summary>
        public float MeleeRange; // world units
        /// <summary>
        /// if our target gets this far away from us, we stop trying to melee them
        /// </summary>
        public float MeleeAbortRange; // world units
        /// <summary>
        /// if we are outside maximum range, we advance towards target, stopping when we reach minimum range
        /// </summary>
        public Bounds<float> BerserkFiringRanges; // world units
        /// <summary>
        /// while berserking, how close an enemy target must get before triggering a melee charge
        /// </summary>
        public float BerserkMeleeRange; // world units
        /// <summary>
        /// while berserking, if our target gets this far away from us, we stop trying to melee them
        /// </summary>
        public float BerserkMeleeAbortRange; // world units
        [TagField(Length = 0x8)]
        public byte[] Padding6;
        /// <summary>
        /// type of grenades that we throw
        /// </summary>
        public GrenadeTypeValue GrenadeType;
        /// <summary>
        /// how we throw our grenades
        /// </summary>
        public TrajectoryTypeValue TrajectoryType;
        /// <summary>
        /// what causes us to consider throwing a grenade
        /// </summary>
        public GrenadeStimulusValue GrenadeStimulus;
        /// <summary>
        /// how many enemies must be within the radius of the grenade before we will consider throwing there
        /// </summary>
        public short MinimumEnemyCount;
        /// <summary>
        /// we consider enemies within this radius when determining where to throw
        /// </summary>
        public float EnemyRadius; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding7;
        /// <summary>
        /// how fast we can throw our grenades
        /// </summary>
        public float GrenadeVelocity; // world units per second
        /// <summary>
        /// ranges within which we will consider throwing a grenade
        /// </summary>
        public Bounds<float> GrenadeRanges; // world units
        /// <summary>
        /// we won't throw if there are friendlies around our target within this range
        /// </summary>
        public float CollateralDamageRadius; // world units
        /// <summary>
        /// how likely we are to throw a grenade
        /// </summary>
        public float GrenadeChance; // [0,1]
        /// <summary>
        /// for continuous stimuli (e.g. visible target), how often we check to see if we want to throw a grenade
        /// </summary>
        public float GrenadeCheckTime; // seconds
        /// <summary>
        /// we cannot throw grenades if someone else in our encounter threw one this recently
        /// </summary>
        public float EncounterGrenadeTimeout; // seconds
        [TagField(Length = 0x14)]
        public byte[] Padding8;
        /// <summary>
        /// equipment item to drop when we die
        /// </summary>
        [TagField(ValidTags = new [] { "eqip" })]
        public CachedTag Equipment;
        /// <summary>
        /// number of grenades that we start with
        /// </summary>
        public Bounds<short> GrenadeCount;
        /// <summary>
        /// how likely we are not to drop any grenades when we die, even if we still have some
        /// </summary>
        public float DontDropGrenadesChance; // [0,1]
        /// <summary>
        /// amount of ammo loaded into the weapon that we drop (in fractions of a clip, e.g. 0.3 to 0.5)
        /// </summary>
        public Bounds<float> DropWeaponLoaded;
        /// <summary>
        /// total number of rounds in the weapon that we drop (ignored for energy weapons)
        /// </summary>
        public Bounds<short> DropWeaponAmmo;
        [TagField(Length = 0xC)]
        public byte[] Padding9;
        [TagField(Length = 0x10)]
        public byte[] Padding10;
        /// <summary>
        /// maximum body vitality of our unit
        /// </summary>
        public float BodyVitality;
        /// <summary>
        /// maximum shield vitality of our unit
        /// </summary>
        public float ShieldVitality;
        /// <summary>
        /// how far away we can drain the player's shields
        /// </summary>
        public float ShieldSappingRadius; // world units
        /// <summary>
        /// if nonzero, overrides the unit's shader permutation
        /// </summary>
        public short ForcedShaderPermutation;
        [TagField(Length = 0x2)]
        public byte[] Padding11;
        [TagField(Length = 0x10)]
        public byte[] Padding12;
        [TagField(Length = 0xC)]
        public byte[] Padding13;
        public List<ActorVariantChangeColorsBlock> ChangeColors;
        
        [Flags]
        public enum FlagsValue : uint
        {
            CanShootWhileFlying = 1 << 0,
            InterpolateColorInHsv = 1 << 1,
            HasUnlimitedGrenades = 1 << 2,
            MoveswitchStayWFriends = 1 << 3,
            ActiveCamouflage = 1 << 4,
            SuperActiveCamouflage = 1 << 5,
            CannotUseRangedWeapons = 1 << 6,
            PreferPassengerSeat = 1 << 7
        }
        
        public enum MovementTypeValue : short
        {
            AlwaysRun,
            AlwaysCrouch,
            SwitchTypes
        }
        
        public enum SpecialFireModeValue : short
        {
            None,
            Overcharge,
            SecondaryTrigger
        }
        
        public enum SpecialFireSituationValue : short
        {
            Never,
            EnemyVisible,
            EnemyOutOfSight,
            Strafing
        }
        
        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma
        }
        
        public enum TrajectoryTypeValue : short
        {
            Toss,
            Lob,
            Bounce
        }
        
        public enum GrenadeStimulusValue : short
        {
            Never,
            VisibleTarget,
            SeekCover
        }
        
        [TagStructure(Size = 0x20)]
        public class ActorVariantChangeColorsBlock : TagStructure
        {
            public RealRgbColor ColorLowerBound;
            public RealRgbColor ColorUpperBound;
            [TagField(Length = 0x8)]
            public byte[] Padding;
        }
    }
}

