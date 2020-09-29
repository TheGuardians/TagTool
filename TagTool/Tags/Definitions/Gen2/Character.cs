using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "character", Tag = "char", Size = 0x174)]
    public class Character : TagStructure
    {
        public CharacterFlagsValue CharacterFlags;
        public CachedTag ParentCharacter;
        public CachedTag Unit;
        public CachedTag Creature; // Creature reference for swarm characters ONLY
        public CachedTag Style;
        public CachedTag MajorCharacter;
        public List<CharacterVariant> Variants;
        public List<CharacterGeneralProperties> GeneralProperties;
        public List<CharacterVitalityProperties> VitalityProperties;
        public List<CharacterPlacementProperties> PlacementProperties;
        public List<CharacterPerceptionProperties> PerceptionProperties;
        public List<CharacterLookProperties> LookProperties;
        public List<CharacterMovementProperties> MovementProperties;
        public List<CharacterSwarmProperties> SwarmProperties;
        public List<CharacterReadyProperties> ReadyProperties;
        public List<CharacterEngageProperties> EngageProperties;
        public List<CharacterChargeProperties> ChargeProperties;
        /// <summary>
        /// Danger Values
        /// </summary>
        /// <remarks>
        /// Danger values can be found in the ai-globals section of the globals tag.
        /// </remarks>
        public List<CharacterEvasionProperties> EvasionProperties;
        public List<CharacterCoverProperties> CoverProperties;
        public List<CharacterRetreatProperties> RetreatProperties;
        public List<CharacterSearchProperties> SearchProperties;
        public List<CharacterPresearchProperties> PreSearchProperties;
        public List<CharacterIdleProperties> IdleProperties;
        public List<CharacterVocalizationProperties> VocalizationProperties;
        public List<CharacterBoardingProperties> BoardingProperties;
        public List<CharacterBossProperties> BossProperties;
        public List<CharacterWeaponProperties> WeaponsProperties;
        public List<CharacterFiringPatternProperties> FiringPatternProperties;
        public List<CharacterGrenadeProperties> GrenadesProperties;
        public List<CharacterVehicleProperties> VehicleProperties;
        
        [Flags]
        public enum CharacterFlagsValue : uint
        {
            Flag1 = 1 << 0
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterVariant : TagStructure
        {
            public StringId VariantName;
            public short VariantIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public StringId VariantDesignator;
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterGeneralProperties : TagStructure
        {
            public GeneralFlagsValue GeneralFlags;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float Scariness; // the inherent scariness of the character
            
            [Flags]
            public enum GeneralFlagsValue : uint
            {
                Swarm = 1 << 0,
                Flying = 1 << 1,
                DualWields = 1 << 2,
                UsesGravemind = 1 << 3
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
                MountedWeapon,
                Brute,
                Prophet,
                Bugger,
                Juggernaut
            }
        }
        
        [TagStructure(Size = 0x70)]
        public class CharacterVitalityProperties : TagStructure
        {
            public VitalityFlagsValue VitalityFlags;
            public float NormalBodyVitality; // maximum body vitality of our unit
            public float NormalShieldVitality; // maximum shield vitality of our unit
            public float LegendaryBodyVitality; // maximum body vitality of our unit (on legendary)
            public float LegendaryShieldVitality; // maximum shield vitality of our unit (on legendary)
            public float BodyRechargeFraction; // fraction of body health that can be regained after damage
            public float SoftPingThresholdWithShields; // damage necessary to trigger a soft ping when shields are up
            public float SoftPingThresholdNoShields; // damage necessary to trigger a soft ping when shields are down
            public float SoftPingMinInterruptTime; // minimum time before a soft ping can be interrupted
            public float HardPingThresholdWithShields; // damage necessary to trigger a hard ping when shields are up
            public float HardPingThresholdNoShields; // damage necessary to trigger a hard ping when shields are down
            public float HardPingMinInterruptTime; // minimum time before a hard ping can be interrupted
            public float CurrentDamageDecayDelay; // current damage begins to fall after a time delay has passed since last the damage
            public float CurrentDamageDecayTime; // amount of time it would take for 100% current damage to decay to 0
            public float RecentDamageDecayDelay; // recent damage begins to fall after a time delay has passed since last the damage
            public float RecentDamageDecayTime; // amount of time it would take for 100% recent damage to decay to 0
            public float BodyRechargeDelayTime; // amount of time delay before a shield begins to recharge
            public float BodyRechargeTime; // amount of time for shields to recharge completely
            public float ShieldRechargeDelayTime; // amount of time delay before a shield begins to recharge
            public float ShieldRechargeTime; // amount of time for shields to recharge completely
            public float StunThreshold; // stun level that triggers the stunned state (currently, the 'stunned' behavior)
            public Bounds<float> StunTimeBounds; // seconds
            public float ExtendedShieldDamageThreshold; // %
            public float ExtendedBodyDamageThreshold; // %
            public float SuicideRadius; // when I die and explode, I damage stuff within this distance of me.
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Unknown1;
            
            [Flags]
            public enum VitalityFlagsValue : uint
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class CharacterPlacementProperties : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float FewUpgradeChanceEasy;
            public float FewUpgradeChanceNormal;
            public float FewUpgradeChanceHeroic;
            public float FewUpgradeChanceLegendary;
            public float NormalUpgradeChanceEasy;
            public float NormalUpgradeChanceNormal;
            public float NormalUpgradeChanceHeroic;
            public float NormalUpgradeChanceLegendary;
            public float ManyUpgradeChanceEasy;
            public float ManyUpgradeChanceNormal;
            public float ManyUpgradeChanceHeroic;
            public float ManyUpgradeChanceLegendary;
        }
        
        [TagStructure(Size = 0x34)]
        public class CharacterPerceptionProperties : TagStructure
        {
            public PerceptionFlagsValue PerceptionFlags;
            public float MaxVisionDistance; // world units
            public Angle CentralVisionAngle; // degrees
            public Angle MaxVisionAngle; // degrees
            public Angle PeripheralVisionAngle; // degrees
            public float PeripheralDistance; // world units
            public float HearingDistance; // world units
            public float NoticeProjectileChance; // [0,1]
            public float NoticeVehicleChance; // [0,1]
            public float CombatPerceptionTime; // seconds
            public float GuardPerceptionTime; // seconds
            public float NonCombatPerceptionTime; // seconds
            public float FirstAckSurpriseDistance; // world units
            
            [Flags]
            public enum PerceptionFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class CharacterLookProperties : TagStructure
        {
            public RealEulerAngles2d MaximumAimingDeviation; // degrees
            public RealEulerAngles2d MaximumLookingDeviation; // degrees
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public Angle NoncombatLookDeltaL; // degrees
            public Angle NoncombatLookDeltaR; // degrees
            public Angle CombatLookDeltaL; // degrees
            public Angle CombatLookDeltaR; // degrees
            public Bounds<float> NoncombatIdleLooking; // seconds
            public Bounds<float> NoncombatIdleAiming; // seconds
            public Bounds<float> CombatIdleLooking; // seconds
            public Bounds<float> CombatIdleAiming; // seconds
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterMovementProperties : TagStructure
        {
            public MovementFlagsValue MovementFlags;
            public float PathfindingRadius;
            public float DestinationRadius;
            public float DiveGrenadeChance;
            public ObstacleLeapMinSizeValue ObstacleLeapMinSize;
            public ObstacleLeapMaxSizeValue ObstacleLeapMaxSize;
            public ObstacleIgnoreSizeValue ObstacleIgnoreSize;
            public ObstacleSmashableSizeValue ObstacleSmashableSize;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public JumpHeightValue JumpHeight;
            public MovementHintsValue MovementHints;
            public float ThrottleScale;
            
            [Flags]
            public enum MovementFlagsValue : uint
            {
                DangerCrouchAllowMovement = 1 << 0,
                NoSideStep = 1 << 1,
                PreferToCombarNearFriends = 1 << 2,
                HopToCoverPathSegements = 1 << 3,
                Perch = 1 << 4,
                HasFlyingMode = 1 << 5,
                DisallowCrouch = 1 << 6
            }
            
            public enum ObstacleLeapMinSizeValue : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum ObstacleLeapMaxSizeValue : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum ObstacleIgnoreSizeValue : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum ObstacleSmashableSizeValue : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum JumpHeightValue : short
            {
                None,
                Down,
                Step,
                Crouch,
                Stand,
                Storey,
                Tower,
                Infinite
            }
            
            [Flags]
            public enum MovementHintsValue : uint
            {
                VaultStep = 1 << 0,
                VaultCrouch = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                MountStep = 1 << 5,
                MountCrouch = 1 << 6,
                MountStand = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                HoistCrouch = 1 << 11,
                HoistStand = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterSwarmProperties : TagStructure
        {
            public short ScatterKilledCount; // After the given number of deaths, the swarm scatters
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float ScatterRadius; // the distance from the target that the swarm scatters
            public float ScatterTime; // amount of time to remain scattered
            public float HoundMinDistance;
            public float HoundMaxDistance;
            public float PerlinOffsetScale; // [0-1]
            public Bounds<float> OffsetPeriod; // s
            public float PerlinIdleMovementThreshold; // [0-1]
            public float PerlinCombatMovementThreshold; // [0-1]
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterReadyProperties : TagStructure
        {
            public Bounds<float> ReadyTimeBounds; // Character will pause for given time before engaging threat
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterEngageProperties : TagStructure
        {
            public FlagsValue Flags;
            public float CrouchDangerThreshold; // When danger rises above the threshold, the actor crouches
            public float StandDangerThreshold; // When danger drops below this threshold, the actor can stand again.
            public float FightDangerMoveThreshold; // When danger goes above given level, this actor switches firing positions
            
            [Flags]
            public enum FlagsValue : uint
            {
                EngagePerch = 1 << 0,
                FightConstantMovement = 1 << 1,
                FlightFightConstantMovement = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class CharacterChargeProperties : TagStructure
        {
            public ChargeFlagsValue ChargeFlags;
            public float MeleeConsiderRange;
            public float MeleeChance; // chance of initiating a melee within a 1 second period
            public float MeleeAttackRange;
            public float MeleeAbortRange;
            public float MeleeAttackTimeout; // seconds
            public float MeleeAttackDelayTimer; // seconds
            public Bounds<float> MeleeLeapRange;
            public float MeleeLeapChance;
            public float IdealLeapVelocity;
            public float MaxLeapVelocity;
            public float MeleeLeapBallistic;
            public float MeleeDelayTimer; // seconds
            public CachedTag BerserkWeapon; // when I berserk, I pull out a ...
            
            [Flags]
            public enum ChargeFlagsValue : uint
            {
                OffhandMeleeAllowed = 1 << 0,
                BerserkWheneverCharge = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterEvasionProperties : TagStructure
        {
            public float EvasionDangerThreshold; // Consider evading when immediate danger surpasses threshold
            public float EvasionDelayTimer; // Wait at least this delay between evasions
            public float EvasionChance; // If danger is above threshold, the chance that we will evade. Expressed as chance of evading within a 1 second time period
            public float EvasionProximityThreshold; // If target is within given proximity, possibly evade
            public float DiveRetreatChance; // Chance of retreating (fleeing) after danger avoidance dive
        }
        
        [TagStructure(Size = 0x40)]
        public class CharacterCoverProperties : TagStructure
        {
            public CoverFlagsValue CoverFlags;
            public Bounds<float> HideBehindCoverTime; // seconds
            public float CoverVitalityThreshold; // When vitality drops below this level, possibly trigger a cover
            public float CoverShieldFraction; // trigger cover when shield drops below this fraction (low shield cover impulse must be enabled)
            public float CoverCheckDelay; // amount of time I will wait before trying again after covering
            public float EmergeFromCoverWhenShieldFractionReachesThreshold; // Emerge from cover when shield fraction reaches threshold
            public float CoverDangerThreshold; // Danger must be this high to cover. At a danger level of 'danger threshold', the chance of seeking cover is the cover chance lower bound (below)
            public float DangerUpperThreshold; // At or above danger level of upper threshold, the chance of seeking cover is the cover chance upper bound (below)
            /// <summary>
            /// Cover chance
            /// </summary>
            /// <remarks>
            /// The Bounds on the chance of seeking cover.
            /// The lower bound is valid when the danger is at 'danger threshold'
            /// The upper bound is valid when the danger is at or above 'danger upper threshold'.
            /// It is interpolated linearly everywhere in between.
            ///  All chances are expressed as 'chance of triggering cover in a 1 second period'.
            /// </remarks>
            public Bounds<float> CoverChance; // Bounds on the chances of seeking cover.
            public float ProximitySelfPreserve; // wus
            public float DisallowCoverDistance; // world units
            public float ProximityMeleeDistance; // When self preserving from a target less than given distance, causes melee attack (assuming proximity_melee_impulse is enabled)
            public float UnreachableEnemyDangerThreshold; // When danger from an unreachable enemy surpasses threshold, actor cover (assuming unreachable_enemy_cover impulse is enabled)
            public float ScaryTargetThreshold; // When target is aware of me and surpasses the given scariness, self-preserve (scary_target_cover_impulse)
            
            [Flags]
            public enum CoverFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class CharacterRetreatProperties : TagStructure
        {
            public RetreatFlagsValue RetreatFlags;
            public float ShieldThreshold; // When shield vitality drops below given amount, retreat is triggered by low_shield_retreat_impulse
            public float ScaryTargetThreshold; // When confronting an enemy of over the given scariness, retreat is triggered by scary_target_retreat_impulse
            public float DangerThreshold; // When perceived danger rises above the given threshold, retreat is triggered by danger_retreat_impulse
            public float ProximityThreshold; // When enemy closer than given threshold, retreat is triggered by proximity_retreat_impulse
            public Bounds<float> MinMaxForcedCowerTimeBounds; // actor cowers for at least the given amount of time
            public Bounds<float> MinMaxCowerTimeoutBounds; // actor times out of cower after the given amount of time
            public float ProximityAmbushThreshold; // If target reaches is within the given proximity, an ambush is triggered by the proximity ambush impulse
            public float AwarenessAmbushThreshold; // If target is less than threshold (0-1) aware of me, an ambush is triggered by the vulnerable enemy ambush impulse
            public float LeaderDeadRetreatChance; // If leader-dead-retreat-impulse is active, gives the chance that we will flee when our leader dies within 4 world units of us
            public float PeerDeadRetreatChance; // If peer-dead-retreat-impulse is active, gives the chance that we will flee when one of our peers (friend of the same race) dies within 4 world units of us
            public float SecondPeerDeadRetreatChance; // If peer-dead-retreat-impulse is active, gives the chance that we will flee when a second peer (friend of the same race) dies within 4 world units of us
            public Angle ZigZagAngle; // degrees
            public float ZigZagPeriod; // seconds
            public float RetreatGrenadeChance; // The likelihood of throwing down a grenade to cover our retreat
            public CachedTag BackupWeapon; // If I want to flee and I don't have flee animations with my current weapon, throw it away and try a ...
            
            [Flags]
            public enum RetreatFlagsValue : uint
            {
                ZigZagWhenFleeing = 1 << 0,
                Unused1 = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterSearchProperties : TagStructure
        {
            public SearchFlagsValue SearchFlags;
            public Bounds<float> SearchTime;
            /// <summary>
            /// Uncover
            /// </summary>
            public Bounds<float> UncoverDistanceBounds; // Distance of uncover point from target. Hard lower limit, soft upper limit.
            
            [Flags]
            public enum SearchFlagsValue : uint
            {
                CrouchOnInvestigate = 1 << 0,
                WalkOnPursuit = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterPresearchProperties : TagStructure
        {
            public PreSearchFlagsValue PreSearchFlags;
            public Bounds<float> MinPresearchTime; // seconds
            public Bounds<float> MaxPresearchTime; // seconds
            public float MinCertaintyRadius;
            public float Deprecated;
            public Bounds<float> MinSuppressingTime; // if the min suppressing time expires and the target is outside the min-certainty radius, suppressing fire turns off
            
            [Flags]
            public enum PreSearchFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterIdleProperties : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public Bounds<float> IdlePoseDelayTime; // seconds
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterVocalizationProperties : TagStructure
        {
            public float LookCommentTime; // s
            public float LookLongCommentTime; // s
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterBoardingProperties : TagStructure
        {
            public FlagsValue Flags;
            public float MaxDistance; // wus
            public float AbortDistance; // wus
            public float MaxSpeed; // wu/s
            
            [Flags]
            public enum FlagsValue : uint
            {
                AirborneBoarding = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterBossProperties : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float FlurryDamageThreshold; // [0..1]
            public float FlurryTime; // seconds
        }
        
        [TagStructure(Size = 0xE0)]
        public class CharacterWeaponProperties : TagStructure
        {
            public WeaponsFlagsValue WeaponsFlags;
            public CachedTag Weapon;
            /// <summary>
            /// Combat ranges
            /// </summary>
            public float MaximumFiringRange; // world units
            public float MinimumFiringRange; // weapon will not be fired at target closer than given distance
            public Bounds<float> NormalCombatRange; // world units
            public float BombardmentRange; // we offset our burst targets randomly by this range when firing at non-visible enemies (zero = never)
            public float MaxSpecialTargetDistance; // world units
            public Bounds<float> TimidCombatRange; // world units
            public Bounds<float> AggressiveCombatRange; // world units
            /// <summary>
            /// Ballistic Firing
            /// </summary>
            public float SuperBallisticRange; // we try to aim our shots super-ballistically if target is outside this range (zero = never)
            public Bounds<float> BallisticFiringBounds; // world units
            public Bounds<float> BallisticFractionBounds; // [0-1]
            /// <summary>
            /// Behavior
            /// </summary>
            public Bounds<float> FirstBurstDelayTime; // seconds
            public float SurpriseDelayTime; // seconds
            public float SurpriseFireWildlyTime; // seconds
            public float DeathFireWildlyChance; // [0,1]
            public float DeathFireWildlyTime; // seconds
            public RealVector3d CustomStandGunOffset; // custom standing gun offset for overriding the default in the base actor
            public RealVector3d CustomCrouchGunOffset; // custom crouching gun offset for overriding the default in the base actor
            /// <summary>
            /// special-case firing properties
            /// </summary>
            public SpecialFireModeValue SpecialFireMode; // the type of special weapon fire that we can use
            public SpecialFireSituationValue SpecialFireSituation; // when we will decide to use our special weapon fire mode
            public float SpecialFireChance; // [0,1]
            public float SpecialFireDelay; // seconds
            public float SpecialDamageModifier; // [0,1]
            public Angle SpecialProjectileError; // degrees
            /// <summary>
            /// Weapon drop when killed
            /// </summary>
            public Bounds<float> DropWeaponLoaded; // amount of ammo loaded into the weapon that we drop (in fractions of a clip, e.g. 0.3 to 0.5)
            public Bounds<short> DropWeaponAmmo; // total number of rounds in the weapon that we drop (ignored for energy weapons)
            /// <summary>
            /// Accuracy
            /// </summary>
            /// <remarks>
            /// Parameters control how accuracy changes over the duration of a series of bursts
            /// Accuracy is an analog value between 0 and 1. At zero, the parameters of the first
            /// firing-pattern block is used. At 1, the parameters in the second block is used. In
            /// between, all the values are linearly interpolated
            /// </remarks>
            public Bounds<float> NormalAccuracyBounds; // Indicates starting and ending accuracies at normal difficulty
            public float NormalAccuracyTime; // The amount of time it takes the accuracy to go from starting to ending
            public Bounds<float> HeroicAccuracyBounds; // Indicates starting and ending accuracies at heroic difficulty
            public float HeroicAccuracyTime; // The amount of time it takes the accuracy to go from starting to ending
            public Bounds<float> LegendaryAccuracyBounds; // Indicates starting and ending accuracies at legendary difficulty
            public float LegendaryAccuracyTime; // The amount of time it takes the accuracy to go from starting to ending
            public List<CharacterFiringPattern> FiringPatterns;
            public CachedTag WeaponMeleeDamage;
            
            [Flags]
            public enum WeaponsFlagsValue : uint
            {
                BurstingInhibitsMovement = 1 << 0,
                MustCrouchToShoot = 1 << 1,
                UseExtendedSafeToSaveRange = 1 << 2
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
            
            [TagStructure(Size = 0x40)]
            public class CharacterFiringPattern : TagStructure
            {
                public float RateOfFire; // how many times per second we pull the trigger (zero = continuously held down)
                public float TargetTracking; // [0,1]
                public float TargetLeading; // [0,1]
                /// <summary>
                /// burst geometry
                /// </summary>
                /// <remarks>
                /// at the start of every burst we pick a random point near the target to fire at, on either the left or the right side.
                /// the burst origin angle controls whether this error is exactly horizontal or might have some vertical component.
                /// 
                /// over the course of the burst we move our projectiles back in the opposite direction towards the target. this return motion is also controlled by an angle that specifies how close to the horizontal it is.
                /// 
                /// for example if the burst origin angle and the burst return angle were both zero, and the return length was the same as the burst length, every burst would start the same amount away from the target (on either the left or right) and move back to exactly over the target at the end of the burst.
                /// </remarks>
                public float BurstOriginRadius; // world units
                public Angle BurstOriginAngle; // degrees
                public Bounds<float> BurstReturnLength; // world units
                public Angle BurstReturnAngle; // degrees
                public Bounds<float> BurstDuration; // seconds
                public Bounds<float> BurstSeparation; // seconds
                public float WeaponDamageModifier; // what fraction of its normal damage our weapon inflicts (zero = no modifier)
                public Angle ProjectileError; // degrees
                public Angle BurstAngularVelocity; // degrees per second
                public Angle MaximumErrorAngle; // degrees
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class CharacterFiringPatternProperties : TagStructure
        {
            public CachedTag Weapon;
            public List<CharacterFiringPattern> FiringPatterns;
            
            [TagStructure(Size = 0x40)]
            public class CharacterFiringPattern : TagStructure
            {
                public float RateOfFire; // how many times per second we pull the trigger (zero = continuously held down)
                public float TargetTracking; // [0,1]
                public float TargetLeading; // [0,1]
                /// <summary>
                /// burst geometry
                /// </summary>
                /// <remarks>
                /// at the start of every burst we pick a random point near the target to fire at, on either the left or the right side.
                /// the burst origin angle controls whether this error is exactly horizontal or might have some vertical component.
                /// 
                /// over the course of the burst we move our projectiles back in the opposite direction towards the target. this return motion is also controlled by an angle that specifies how close to the horizontal it is.
                /// 
                /// for example if the burst origin angle and the burst return angle were both zero, and the return length was the same as the burst length, every burst would start the same amount away from the target (on either the left or right) and move back to exactly over the target at the end of the burst.
                /// </remarks>
                public float BurstOriginRadius; // world units
                public Angle BurstOriginAngle; // degrees
                public Bounds<float> BurstReturnLength; // world units
                public Angle BurstReturnAngle; // degrees
                public Bounds<float> BurstDuration; // seconds
                public Bounds<float> BurstSeparation; // seconds
                public float WeaponDamageModifier; // what fraction of its normal damage our weapon inflicts (zero = no modifier)
                public Angle ProjectileError; // degrees
                public Angle BurstAngularVelocity; // degrees per second
                public Angle MaximumErrorAngle; // degrees
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class CharacterGrenadeProperties : TagStructure
        {
            public GrenadesFlagsValue GrenadesFlags;
            public GrenadeTypeValue GrenadeType; // type of grenades that we throw^
            public TrajectoryTypeValue TrajectoryType; // how we throw our grenades
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short MinimumEnemyCount; // how many enemies must be within the radius of the grenade before we will consider throwing there
            public float EnemyRadius; // world units
            public float GrenadeIdealVelocity; // world units per second
            public float GrenadeVelocity; // world units per second
            public Bounds<float> GrenadeRanges; // world units
            public float CollateralDamageRadius; // world units
            public float GrenadeChance; // [0,1]
            public float GrenadeThrowDelay; // seconds
            public float GrenadeUncoverChance; // [0,1]
            public float AntiVehicleGrenadeChance; // [0,1]
            /// <summary>
            /// Grenade drop when killed
            /// </summary>
            public Bounds<short> GrenadeCount; // number of grenades that we start with
            public float DontDropGrenadesChance; // [0,1]
            
            [Flags]
            public enum GrenadesFlagsValue : uint
            {
                Flag1 = 1 << 0
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
        }
        
        [TagStructure(Size = 0xC4)]
        public class CharacterVehicleProperties : TagStructure
        {
            public CachedTag Unit;
            public CachedTag Style;
            public VehicleFlagsValue VehicleFlags;
            /// <summary>
            /// Pathfinding
            /// </summary>
            public float AiPathfindingRadius; // world units
            public float AiDestinationRadius; // world units
            public float AiDecelerationDistanceworldUnits; // (All vehicles)Distance from goal at which AI starts to decelerate
            /// <summary>
            /// Turning
            /// </summary>
            public float AiTurningRadius; // (Warthog, Pelican, Ghost) Idealized average turning radius (should reflect actual vehicle physics)
            public float AiInnerTurningRadiusTr; // (Warthog-type) Idealized minimum turning radius (should reflect actual vehicle physics)
            public float AiIdealTurningRadiusTr; // (Warthogs, ghosts) Ideal turning radius for rounding turns (barring obstacles, etc.)
            /// <summary>
            /// Steering
            /// </summary>
            public Angle AiBansheeSteeringMaximum; // (Banshee)
            public float AiMaxSteeringAngle; // degrees
            public float AiMaxSteeringDelta; //  degrees
            public float AiOversteeringScale; // (Warthog, ghosts, wraiths)
            public Bounds<Angle> AiOversteeringBounds; // (Banshee) Angle to goal at which AI will oversteer
            public float AiSideslipDistance; // (Ghosts, Dropships) Distance within which Ai will strafe to target (as opposed to turning)
            public float AiAvoidanceDistance; // world units
            public float AiMinUrgency; // [0-1]
            /// <summary>
            /// Throttle
            /// </summary>
            public float AiThrottleMaximum; // (0 - 1)
            public float AiGoalMinThrottleScale; // (Warthogs, Dropships, ghosts)scale on throttle when within 'ai deceleration distance' of goal (0...1)
            public float AiTurnMinThrottleScale; // (Warthogs, ghosts) Scale on throttle due to nearness to a turn (0...1)
            public float AiDirectionMinThrottleScale; // (Warthogs, ghosts) Scale on throttle due to facing away from intended direction (0...1)
            public float AiAccelerationScale; // (0-1)
            public float AiThrottleBlend; // (0-1)
            public float TheoreticalMaxSpeed; // wu/s
            public float ErrorScale; // (dropships, warthogs) scale on the difference between desired and actual speed, applied to throttle
            /// <summary>
            /// Combat
            /// </summary>
            public Angle AiAllowableAimDeviationAngle;
            /// <summary>
            /// Behavior
            /// </summary>
            public float AiChargeTightAngleDistance; // (All vehicles) The distance at which the tight angle criterion is used for deciding to vehicle charge
            public float AiChargeTightAngle; // [0-1]
            public float AiChargeRepeatTimeout; // (All vehicles) Time delay between vehicle charges
            public float AiChargeLookAheadTime; // (All vehicles) In deciding when to abort vehicle charge, look ahead these many seconds to predict time of contact
            public float AiChargeConsiderDistance; // Consider charging the target when it is within this range (0 = infinite distance)
            public float AiChargeAbortDistance; // Abort the charge when the target get more than this far away (0 = never abort)
            public float VehicleRamTimeout; // The ram behavior stops after a maximum of the given number of seconds
            public float RamParalysisTime; // The ram behavior freezes the vehicle for a given number of seconds after performing the ram
            public float AiCoverDamageThreshold; // (All vehicles) Trigger a cover when recent damage is above given threshold (damage_vehicle_cover impulse)
            public float AiCoverMinDistance; // (All vehicles) When executing vehicle-cover, minimum distance from the target to flee to
            public float AiCoverTime; // (All vehicles) How long to stay away from the target
            public float AiCoverMinBoostDistance; // (All vehicles) Boosting allowed when distance to cover destination is greater then this.
            public float TurtlingRecentDamageThreshold; // %
            public float TurtlingMinTime; // seconds
            public float TurtlingTimeout; // seconds
            public ObstacleIgnoreSizeValue ObstacleIgnoreSize;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            
            [Flags]
            public enum VehicleFlagsValue : uint
            {
                PassengersAdoptOriginalSquad = 1 << 0
            }
            
            public enum ObstacleIgnoreSizeValue : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
        }
    }
}

