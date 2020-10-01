using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "actor", Tag = "actr", Size = 0x4F8)]
    public class Actor : TagStructure
    {
        public FlagsValue Flags;
        public MoreFlagsValue MoreFlags;
        [TagField(Length = 0xC)]
        public byte[] Padding;
        public TypeValue Type;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        /// <summary>
        /// maximum range of sight
        /// </summary>
        public float MaxVisionDistance; // world units
        /// <summary>
        /// horizontal angle within which we see targets out to our maximum range
        /// </summary>
        public Angle CentralVisionAngle; // degrees
        /// <summary>
        /// maximum horizontal angle within which we see targets at range
        /// </summary>
        public Angle MaxVisionAngle; // degrees
        [TagField(Length = 0x4)]
        public byte[] Padding2;
        /// <summary>
        /// maximum horizontal angle within which we can see targets out of the corner of our eye
        /// </summary>
        public Angle PeripheralVisionAngle; // degrees
        /// <summary>
        /// maximum range at which we can see targets our of the corner of our eye
        /// </summary>
        public float PeripheralDistance; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        /// <summary>
        /// offset of gun from feet when standing (x=forward, y=left, z=up)
        /// </summary>
        public RealVector3d StandingGunOffset;
        /// <summary>
        /// offset of gun from feet when crouch (x=forward, y=left, z=up)
        /// </summary>
        public RealVector3d CrouchingGunOffset;
        /// <summary>
        /// maximum range at which sounds can be heard
        /// </summary>
        public float HearingDistance; // world units
        /// <summary>
        /// random chance of noticing a dangerous enemy projectile (e.g. grenade)
        /// </summary>
        public float NoticeProjectileChance; // [0,1]
        /// <summary>
        /// random chance of noticing a dangerous vehicle
        /// </summary>
        public float NoticeVehicleChance; // [0,1]
        [TagField(Length = 0x8)]
        public byte[] Padding4;
        /// <summary>
        /// time required to acknowledge a visible enemy when we are already in combat or searching for them
        /// </summary>
        public float CombatPerceptionTime; // seconds
        /// <summary>
        /// time required to acknowledge a visible enemy when we have been alerted
        /// </summary>
        public float GuardPerceptionTime; // seconds
        /// <summary>
        /// time required to acknowledge a visible enemy when we are not alerted
        /// </summary>
        public float NonCombatPerceptionTime; // seconds
        [TagField(Length = 0xC)]
        public byte[] Padding5;
        [TagField(Length = 0x8)]
        public byte[] Padding6;
        /// <summary>
        /// chance of running a dive animation when moving into cover
        /// </summary>
        public float DiveIntoCoverChance; // [0,1]
        /// <summary>
        /// chance of running an emerge animation when uncovering a target
        /// </summary>
        public float EmergeFromCoverChance; // [0,1]
        /// <summary>
        /// chance of running a dive animation when moving away from a grenade
        /// </summary>
        public float DiveFromGrenadeChance; // [0,1]
        public float PathfindingRadius; // world units
        /// <summary>
        /// chance of not noticing that breakable surfaces have been destroyed
        /// </summary>
        public float GlassIgnoranceChance; // [0,1]
        /// <summary>
        /// movement distance which is considered 'stationary' for considering whether we crouch
        /// </summary>
        public float StationaryMovementDist; // world units
        /// <summary>
        /// distance which we allow sidestepping for flying units
        /// </summary>
        public float FreeFlyingSidestep; // world units
        /// <summary>
        /// we must be facing this close to our target before we start applying the throttle (default: 180 degrees)
        /// </summary>
        public Angle BeginMovingAngle; // degrees
        [TagField(Length = 0x4)]
        public byte[] Padding7;
        /// <summary>
        /// how far we can turn our weapon
        /// </summary>
        public RealEulerAngles2d MaximumAimingDeviation; // degrees
        /// <summary>
        /// how far we can turn our head
        /// </summary>
        public RealEulerAngles2d MaximumLookingDeviation; // degrees
        /// <summary>
        /// how far we can turn our head left away from our aiming vector when not in combat
        /// </summary>
        public Angle NoncombatLookDeltaL; // degrees
        /// <summary>
        /// how far we can turn our head right away from our aiming vector when not in combat
        /// </summary>
        public Angle NoncombatLookDeltaR; // degrees
        /// <summary>
        /// how far we can turn our head left away from our aiming vector when in combat
        /// </summary>
        public Angle CombatLookDeltaL; // degrees
        /// <summary>
        /// how far we can turn our head right away from our aiming vector when in combat
        /// </summary>
        public Angle CombatLookDeltaR; // degrees
        /// <summary>
        /// range in which we select random directions to aim in
        /// </summary>
        public RealEulerAngles2d IdleAimingRange; // degrees
        /// <summary>
        /// range in which we select random directions to look at
        /// </summary>
        public RealEulerAngles2d IdleLookingRange; // degrees
        /// <summary>
        /// multiplier for how long we look at interesting events (zero = unchanged)
        /// </summary>
        public Bounds<float> EventLookTimeModifier;
        /// <summary>
        /// rate at which we change facing when looking around randomly when not in combat
        /// </summary>
        public Bounds<float> NoncombatIdleFacing; // seconds
        /// <summary>
        /// rate at which we change aiming directions when looking around randomly when not in combat
        /// </summary>
        public Bounds<float> NoncombatIdleAiming; // seconds
        /// <summary>
        /// rate at which we change look around randomly when not in combat
        /// </summary>
        public Bounds<float> NoncombatIdleLooking; // seconds
        /// <summary>
        /// rate at which we change facing when looking around randomly when guarding
        /// </summary>
        public Bounds<float> GuardIdleFacing; // seconds
        /// <summary>
        /// rate at which we change aiming directions when looking around randomly when guarding
        /// </summary>
        public Bounds<float> GuardIdleAiming; // seconds
        /// <summary>
        /// rate at which we change look around randomly when guarding
        /// </summary>
        public Bounds<float> GuardIdleLooking; // seconds
        /// <summary>
        /// rate at which we change facing when looking around randomly when searching or in combat
        /// </summary>
        public Bounds<float> CombatIdleFacing; // seconds
        /// <summary>
        /// rate at which we change aiming directions when looking around randomly when searching or in combat
        /// </summary>
        public Bounds<float> CombatIdleAiming; // seconds
        /// <summary>
        /// rate at which we change look around randomly when searching or in combat
        /// </summary>
        public Bounds<float> CombatIdleLooking; // seconds
        [TagField(Length = 0x8)]
        public byte[] Padding8;
        [TagField(Length = 0x10)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag DoNotUse; //  weapon
        [TagField(Length = 0x10C)]
        public byte[] Padding10;
        [TagField(ValidTags = new [] { "proj" })]
        public CachedTag DoNotUse1; //  projectile
        /// <summary>
        /// danger level of an unreachable enemy which will trigger a retreat if it continues over time
        /// </summary>
        public UnreachableDangerTriggerValue UnreachableDangerTrigger;
        /// <summary>
        /// danger level of a vehicle-based enemy which will trigger a retreat if it continues over time
        /// </summary>
        public VehicleDangerTriggerValue VehicleDangerTrigger;
        /// <summary>
        /// danger level of an enemy player which will trigger a retreat if it continues over time
        /// </summary>
        public PlayerDangerTriggerValue PlayerDangerTrigger;
        [TagField(Length = 0x2)]
        public byte[] Padding11;
        /// <summary>
        /// how long it takes for an unopposable enemy that has the above danger level to trigger a retreat
        /// </summary>
        public Bounds<float> DangerTriggerTime; // seconds
        /// <summary>
        /// if this many of our friends are killed by an unopposable enemy, we trigger a retreat (zero = never use this as a retreat
        /// condition)
        /// </summary>
        public short FriendsKilledTrigger;
        /// <summary>
        /// if this many of our friends are retreating from an unopposable enemy, we retreat as well (zero = never use this as a
        /// retreat condition)
        /// </summary>
        public short FriendsRetreatingTrigger;
        [TagField(Length = 0xC)]
        public byte[] Padding12;
        /// <summary>
        /// how long we retreat from an unopposable enemy for
        /// </summary>
        public Bounds<float> RetreatTime; // seconds
        [TagField(Length = 0x8)]
        public byte[] Padding13;
        /// <summary>
        /// how long we hide in cover after being panicked
        /// </summary>
        public Bounds<float> CoweringTime; // seconds
        /// <summary>
        /// chance of panicking when we see a friend killed near us and the enemy is looking at us too
        /// </summary>
        public float FriendKilledPanicChance; // [0,1]
        /// <summary>
        /// if we see a friend of this type killed we have a chance of panicking
        /// </summary>
        public LeaderTypeValue LeaderType;
        [TagField(Length = 0x2)]
        public byte[] Padding14;
        /// <summary>
        /// chance of panicking when we see a leader killed
        /// </summary>
        public float LeaderKilledPanicChance; // [0,1]
        /// <summary>
        /// panic if we take this much body damage in a short period of time
        /// </summary>
        public float PanicDamageThreshold; // [0,1]
        /// <summary>
        /// the distance at which newly acknowledged props or weapon impacts are considered 'close' for surprise purposes
        /// </summary>
        public float SurpriseDistance; // world units
        [TagField(Length = 0x1C)]
        public byte[] Padding15;
        /// <summary>
        /// danger values: being aimed at: 0.7
        /// an enemy shooting in our general direction: 1.2
        /// an enemy shooting directly at us:
        /// 1.6
        /// an enemy damaging us: 2.0
        /// </summary>
        /// <summary>
        /// how long we stay behind cover after seeking cover
        /// </summary>
        public Bounds<float> HideBehindCoverTime; // seconds
        /// <summary>
        /// if this is non-zero then we will only seek cover if our target has not been visible recently
        /// </summary>
        public float HideTargetNotVisibleTime; // seconds
        /// <summary>
        /// elites and jackals only seek cover if their shield falls below this value
        /// </summary>
        public float HideShieldFraction; // [0,1]
        /// <summary>
        /// elites and jackals only come out from cover to attack if they have this much shields
        /// </summary>
        public float AttackShieldFraction; // [0,1]
        /// <summary>
        /// elites and jackals only come out from cover to pursue if they have this much shields
        /// </summary>
        public float PursueShieldFraction; // [0,1]
        [TagField(Length = 0x10)]
        public byte[] Padding16;
        public DefensiveCrouchTypeValue DefensiveCrouchType;
        [TagField(Length = 0x2)]
        public byte[] Padding17;
        /// <summary>
        /// when in attacking mode, if our crouch type is based on shields, we crouch when our shields are below this number; if our
        /// crouch type is based on danger, we crouch when our danger is above this number
        /// </summary>
        public float AttackingCrouchThreshold;
        /// <summary>
        /// when in defending mode, if our crouch type is based on shields, we crouch when our shields are below this number; if our
        /// crouch type is based on danger, we crouch when our danger is above this number
        /// </summary>
        public float DefendingCrouchThreshold;
        /// <summary>
        /// minimum time to remain standing (zero = default)
        /// </summary>
        public float MinStandTime; // seconds
        /// <summary>
        /// minimum time to remain crouching (zero = default)
        /// </summary>
        public float MinCrouchTime; // seconds
        /// <summary>
        /// how much longer we hide behind cover for when in the defending state (zero = unchanged)
        /// </summary>
        public float DefendingHideTimeModifier;
        /// <summary>
        /// when in attacking mode, we consider seeking cover or evading when our danger gets this high
        /// </summary>
        public float AttackingEvasionThreshold;
        /// <summary>
        /// when in defending mode, we consider seeking cover or evading when our danger gets this high
        /// </summary>
        public float DefendingEvasionThreshold;
        /// <summary>
        /// chance of seeking cover (otherwise we just evade)
        /// </summary>
        public float EvasionSeekCoverChance; // [0,1]
        /// <summary>
        /// minimum time period between evasion moves
        /// </summary>
        public float EvasionDelayTime; // seconds
        /// <summary>
        /// maximum distance we will consider going to find cover (zero = default)
        /// </summary>
        public float MaxSeekCoverDistance; // world units
        /// <summary>
        /// how much damage we must take before we are allowed to seek cover (zero = always allowed to)
        /// </summary>
        public float CoverDamageThreshold; // [0,1]
        /// <summary>
        /// if our target sees us for this long while we are stalking them, our cover is blown and we do something else (zero = never
        /// stop stalking)
        /// </summary>
        public float StalkingDiscoveryTime; // seconds
        /// <summary>
        /// distance outside of which we don't bother stalking
        /// </summary>
        public float StalkingMaxDistance; // world units
        /// <summary>
        /// angle outside of which we must abandon a stationary facing direction and suffer any penalties
        /// </summary>
        public Angle StationaryFacingAngle; // angle
        /// <summary>
        /// how long we must stand up for after changing our fixed stationary facing
        /// </summary>
        public float ChangeFacingStandTime; // seconds
        [TagField(Length = 0x4)]
        public byte[] Padding18;
        /// <summary>
        /// time to look at target's position after it becomes visible
        /// </summary>
        public Bounds<float> UncoverDelayTime; // seconds
        /// <summary>
        /// time we search at target's position
        /// </summary>
        public Bounds<float> TargetSearchTime; // seconds
        /// <summary>
        /// time we search at a pursuit position
        /// </summary>
        public Bounds<float> PursuitPositionTime; // seconds
        /// <summary>
        /// number of pursuit positions to check when in coordinated group search mode
        /// </summary>
        public short NumPositionsCoord; // [0,n]
        /// <summary>
        /// number of pursuit positions to check when in normal search mode
        /// </summary>
        public short NumPositionsNormal; // [0,n]
        [TagField(Length = 0x20)]
        public byte[] Padding19;
        /// <summary>
        /// how long we must wait between attempting melee attacks
        /// </summary>
        public float MeleeAttackDelay; // seconds
        /// <summary>
        /// fudge factor that offsets how far in front of the target we start our attack (negative = we try to time our attack so
        /// that we go _through_ the target). this should be close to zero, but might be bigger for suiciding units
        /// </summary>
        public float MeleeFudgeFactor; // world units
        /// <summary>
        /// how long we can stay in the charging state trying to reach our target before we give up
        /// </summary>
        public float MeleeChargeTime; // seconds
        /// <summary>
        /// we can launch leaping melee attacks at targets within these ranges (zero = can't leap)
        /// </summary>
        public Bounds<float> MeleeLeapRange; // world units
        /// <summary>
        /// how fast we spring at targets when launching a leaping melee attack
        /// </summary>
        public float MeleeLeapVelocity; // world units per tick
        /// <summary>
        /// chance of launching a leaping melee attack at a ground-based target (we always leap at flying targets)
        /// </summary>
        public float MeleeLeapChance; // [0,1]
        /// <summary>
        /// fraction that controls how ballistic our leaping melee trajectory is
        /// </summary>
        public float MeleeLeapBallistic; // [0,1]
        /// <summary>
        /// amount of body damage in a short time that makes us berserk
        /// </summary>
        public float BerserkDamageAmount; // [0,1]
        /// <summary>
        /// how low our body health must get before we will consider berserking
        /// </summary>
        public float BerserkDamageThreshold; // [0,1]
        /// <summary>
        /// if we ever get this close to a target, we berserk
        /// </summary>
        public float BerserkProximity; // world units
        /// <summary>
        /// when we are this close to a target, we check to see if they're getting away and if so blow up
        /// </summary>
        public float SuicideSensingDist; // world units
        /// <summary>
        /// chance of berserking when we have a dangerous projectile stuck to us
        /// </summary>
        public float BerserkGrenadeChance; // [0,1]
        [TagField(Length = 0xC)]
        public byte[] Padding20;
        /// <summary>
        /// time after which we decide to change guard positions (zero = never)
        /// </summary>
        public Bounds<float> GuardPositionTime; // seconds
        /// <summary>
        /// time after which we change combat firing positions
        /// </summary>
        public Bounds<float> CombatPositionTime; // seconds
        /// <summary>
        /// distance we try and stay from our last discarded firing position
        /// </summary>
        public float OldPositionAvoidDist; // world units
        /// <summary>
        /// distance we try and stay from any friends
        /// </summary>
        public float FriendAvoidDist; // world units
        [TagField(Length = 0x28)]
        public byte[] Padding21;
        /// <summary>
        /// time between idle vocalizations when we are not in combat
        /// </summary>
        public Bounds<float> NoncombatIdleSpeechTime; // seconds
        /// <summary>
        /// time between idle vocalizations when we are in combat or searching
        /// </summary>
        public Bounds<float> CombatIdleSpeechTime; // seconds
        [TagField(Length = 0x30)]
        public byte[] Padding22;
        [TagField(Length = 0x80)]
        public byte[] Padding23;
        [TagField(ValidTags = new [] { "actr" })]
        public CachedTag DoNotUse2; //  major upgrade
        [TagField(Length = 0x30)]
        public byte[] Padding24;
        
        [Flags]
        public enum FlagsValue : uint
        {
            CanSeeInDarkness = 1 << 0,
            SneakUncoveringTarget = 1 << 1,
            SneakUncoveringPursuitPosition = 1 << 2,
            Unused = 1 << 3,
            ShootAtTargetSLastLocation = 1 << 4,
            TryToStayStillWhenCrouched = 1 << 5,
            CrouchWhenNotInCombat = 1 << 6,
            CrouchWhenGuarding = 1 << 7,
            Unused1 = 1 << 8,
            MustCrouchToShoot = 1 << 9,
            PanicWhenSurprised = 1 << 10,
            AlwaysChargeAtEnemies = 1 << 11,
            GetsInVehiclesWithPlayer = 1 << 12,
            StartFiringBeforeAligned = 1 << 13,
            StandingMustMoveForward = 1 << 14,
            CrouchingMustMoveForward = 1 << 15,
            DefensiveCrouchWhileCharging = 1 << 16,
            UseStalkingBehavior = 1 << 17,
            StalkingFreezeIfExposed = 1 << 18,
            AlwaysBerserkInAttackingMode = 1 << 19,
            BerserkingUsesPanickedMovement = 1 << 20,
            Flying = 1 << 21,
            PanickedByUnopposableEnemy = 1 << 22,
            CrouchWhenHidingFromUnopposable = 1 << 23,
            AlwaysChargeInAttackingMode = 1 << 24,
            DiveOffLedges = 1 << 25,
            Swarm = 1 << 26,
            SuicidalMeleeAttack = 1 << 27,
            CannotMoveWhileCrouching = 1 << 28,
            FixedCrouchFacing = 1 << 29,
            CrouchWhenInLineOfFire = 1 << 30,
            AvoidFriendsLineOfFire = 1u << 31
        }
        
        [Flags]
        public enum MoreFlagsValue : uint
        {
            AvoidAllEnemyAttackVectors = 1 << 0,
            MustStandToFire = 1 << 1,
            MustStopToFire = 1 << 2,
            DisallowVehicleCombat = 1 << 3,
            PathfindingIgnoresDanger = 1 << 4,
            PanicInGroups = 1 << 5,
            NoCorpseShooting = 1 << 6
        }
        
        public enum TypeValue : short
        {
            Elite,
            Jackal,
            Grunt,
            Hunter,
            Engineer,
            Assassin,
            Player,
            Marine,
            Crew,
            CombatForm,
            InfectionForm,
            CarrierForm,
            Monitor,
            Sentinel,
            None,
            MountedWeapon
        }
        
        public enum UnreachableDangerTriggerValue : short
        {
            Never,
            Visible,
            Shooting,
            ShootingNearUs,
            DamagingUs,
            Unused,
            Unused1,
            Unused2,
            Unused3,
            Unused4
        }
        
        public enum VehicleDangerTriggerValue : short
        {
            Never,
            Visible,
            Shooting,
            ShootingNearUs,
            DamagingUs,
            Unused,
            Unused1,
            Unused2,
            Unused3,
            Unused4
        }
        
        public enum PlayerDangerTriggerValue : short
        {
            Never,
            Visible,
            Shooting,
            ShootingNearUs,
            DamagingUs,
            Unused,
            Unused1,
            Unused2,
            Unused3,
            Unused4
        }
        
        public enum LeaderTypeValue : short
        {
            Elite,
            Jackal,
            Grunt,
            Hunter,
            Engineer,
            Assassin,
            Player,
            Marine,
            Crew,
            CombatForm,
            InfectionForm,
            CarrierForm,
            Monitor,
            Sentinel,
            None,
            MountedWeapon
        }
        
        public enum DefensiveCrouchTypeValue : short
        {
            Never,
            Danger,
            LowShields,
            HideBehindShield,
            AnyTarget,
            FloodShamble
        }
    }
}

