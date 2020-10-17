using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "character", Tag = "char", Size = 0xEC)]
    public class Character : TagStructure
    {
        public CharacterFlagsValue CharacterFlags;
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag ParentCharacter;
        [TagField(ValidTags = new [] { "unit" })]
        public CachedTag Unit;
        /// <summary>
        /// Creature reference for swarm characters ONLY
        /// </summary>
        [TagField(ValidTags = new [] { "crea" })]
        public CachedTag Creature;
        [TagField(ValidTags = new [] { "styl" })]
        public CachedTag Style;
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag MajorCharacter;
        public List<CharacterVariantsBlock> Variants;
        public List<CharacterGeneralBlock> GeneralProperties;
        public List<CharacterVitalityBlock> VitalityProperties;
        public List<CharacterPlacementBlock> PlacementProperties;
        public List<CharacterPerceptionBlock> PerceptionProperties;
        public List<CharacterLookBlock> LookProperties;
        public List<CharacterMovementBlock> MovementProperties;
        public List<CharacterSwarmBlock> SwarmProperties;
        public List<CharacterReadyBlock> ReadyProperties;
        public List<CharacterEngageBlock> EngageProperties;
        public List<CharacterChargeBlock> ChargeProperties;
        /// <summary>
        /// Danger values can be found in the ai-globals section of the globals tag.
        /// </summary>
        public List<CharacterEvasionBlock> EvasionProperties;
        public List<CharacterCoverBlock> CoverProperties;
        public List<CharacterRetreatBlock> RetreatProperties;
        public List<CharacterSearchBlock> SearchProperties;
        public List<CharacterPresearchBlock> PreSearchProperties;
        public List<CharacterIdleBlock> IdleProperties;
        public List<CharacterVocalizationBlock> VocalizationProperties;
        public List<CharacterBoardingBlock> BoardingProperties;
        public List<CharacterBossBlock> BossProperties;
        public List<CharacterWeaponsBlock> WeaponsProperties;
        public List<CharacterFiringPatternPropertiesBlock> FiringPatternProperties;
        public List<CharacterGrenadesBlock> GrenadesProperties;
        public List<CharacterVehicleBlock> VehicleProperties;
        
        [Flags]
        public enum CharacterFlagsValue : uint
        {
            Flag1 = 1 << 0
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterVariantsBlock : TagStructure
        {
            public StringId VariantName;
            public short VariantIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId VariantDesignator;
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterGeneralBlock : TagStructure
        {
            public GeneralFlagsValue GeneralFlags;
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// the inherent scariness of the character
            /// </summary>
            public float Scariness;
            
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
        public class CharacterVitalityBlock : TagStructure
        {
            public VitalityFlagsValue VitalityFlags;
            /// <summary>
            /// maximum body vitality of our unit
            /// </summary>
            public float NormalBodyVitality;
            /// <summary>
            /// maximum shield vitality of our unit
            /// </summary>
            public float NormalShieldVitality;
            /// <summary>
            /// maximum body vitality of our unit (on legendary)
            /// </summary>
            public float LegendaryBodyVitality;
            /// <summary>
            /// maximum shield vitality of our unit (on legendary)
            /// </summary>
            public float LegendaryShieldVitality;
            /// <summary>
            /// fraction of body health that can be regained after damage
            /// </summary>
            public float BodyRechargeFraction;
            /// <summary>
            /// damage necessary to trigger a soft ping when shields are up
            /// </summary>
            public float SoftPingThresholdWithShields;
            /// <summary>
            /// damage necessary to trigger a soft ping when shields are down
            /// </summary>
            public float SoftPingThresholdNoShields;
            /// <summary>
            /// minimum time before a soft ping can be interrupted
            /// </summary>
            public float SoftPingMinInterruptTime;
            /// <summary>
            /// damage necessary to trigger a hard ping when shields are up
            /// </summary>
            public float HardPingThresholdWithShields;
            /// <summary>
            /// damage necessary to trigger a hard ping when shields are down
            /// </summary>
            public float HardPingThresholdNoShields;
            /// <summary>
            /// minimum time before a hard ping can be interrupted
            /// </summary>
            public float HardPingMinInterruptTime;
            /// <summary>
            /// current damage begins to fall after a time delay has passed since last the damage
            /// </summary>
            public float CurrentDamageDecayDelay;
            /// <summary>
            /// amount of time it would take for 100% current damage to decay to 0
            /// </summary>
            public float CurrentDamageDecayTime;
            /// <summary>
            /// recent damage begins to fall after a time delay has passed since last the damage
            /// </summary>
            public float RecentDamageDecayDelay;
            /// <summary>
            /// amount of time it would take for 100% recent damage to decay to 0
            /// </summary>
            public float RecentDamageDecayTime;
            /// <summary>
            /// amount of time delay before a shield begins to recharge
            /// </summary>
            public float BodyRechargeDelayTime;
            /// <summary>
            /// amount of time for shields to recharge completely
            /// </summary>
            public float BodyRechargeTime;
            /// <summary>
            /// amount of time delay before a shield begins to recharge
            /// </summary>
            public float ShieldRechargeDelayTime;
            /// <summary>
            /// amount of time for shields to recharge completely
            /// </summary>
            public float ShieldRechargeTime;
            /// <summary>
            /// stun level that triggers the stunned state (currently, the 'stunned' behavior)
            /// </summary>
            public float StunThreshold;
            public Bounds<float> StunTimeBounds; // seconds
            /// <summary>
            /// Amount of shield damage sustained before it is considered 'extended'
            /// </summary>
            public float ExtendedShieldDamageThreshold; // %
            /// <summary>
            /// Amount of body damage sustained before it is considered 'extended'
            /// </summary>
            public float ExtendedBodyDamageThreshold; // %
            /// <summary>
            /// when I die and explode, I damage stuff within this distance of me.
            /// </summary>
            public float SuicideRadius;
            [TagField(Length = 0x8)]
            public byte[] Unknown;
            
            [Flags]
            public enum VitalityFlagsValue : uint
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class CharacterPlacementBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        public class CharacterPerceptionBlock : TagStructure
        {
            public PerceptionFlagsValue PerceptionFlags;
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
            /// <summary>
            /// maximum horizontal angle within which we can see targets out of the corner of our eye
            /// </summary>
            public Angle PeripheralVisionAngle; // degrees
            /// <summary>
            /// maximum range at which we can see targets our of the corner of our eye
            /// </summary>
            public float PeripheralDistance; // world units
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
            /// <summary>
            /// If a new prop is acknowledged within the given distance, surprise is registerd
            /// </summary>
            public float FirstAckSurpriseDistance; // world units
            
            [Flags]
            public enum PerceptionFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class CharacterLookBlock : TagStructure
        {
            /// <summary>
            /// how far we can turn our weapon
            /// </summary>
            public RealEulerAngles2d MaximumAimingDeviation; // degrees
            /// <summary>
            /// how far we can turn our head
            /// </summary>
            public RealEulerAngles2d MaximumLookingDeviation; // degrees
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
            /// rate at which we change look around randomly when not in combat
            /// </summary>
            public Bounds<float> NoncombatIdleLooking; // seconds
            /// <summary>
            /// rate at which we change aiming directions when looking around randomly when not in combat
            /// </summary>
            public Bounds<float> NoncombatIdleAiming; // seconds
            /// <summary>
            /// rate at which we change look around randomly when searching or in combat
            /// </summary>
            public Bounds<float> CombatIdleLooking; // seconds
            /// <summary>
            /// rate at which we change aiming directions when looking around randomly when searching or in combat
            /// </summary>
            public Bounds<float> CombatIdleAiming; // seconds
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterMovementBlock : TagStructure
        {
            public MovementFlagsValue MovementFlags;
            public float PathfindingRadius;
            public float DestinationRadius;
            public float DiveGrenadeChance;
            public ObstacleLeapMinSizeValue ObstacleLeapMinSize;
            public ObstacleLeapMaxSizeValue ObstacleLeapMaxSize;
            public ObstacleIgnoreSizeValue ObstacleIgnoreSize;
            public ObstacleSmashableSizeValue ObstacleSmashableSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
                Unknown = 1 << 2,
                Unknown1 = 1 << 3,
                Unknown2 = 1 << 4,
                MountStep = 1 << 5,
                MountCrouch = 1 << 6,
                MountStand = 1 << 7,
                Unknown3 = 1 << 8,
                Unknown4 = 1 << 9,
                Unknown5 = 1 << 10,
                HoistCrouch = 1 << 11,
                HoistStand = 1 << 12,
                Unknown6 = 1 << 13,
                Unknown7 = 1 << 14,
                Unknown8 = 1 << 15
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterSwarmBlock : TagStructure
        {
            /// <summary>
            /// After the given number of deaths, the swarm scatters
            /// </summary>
            public short ScatterKilledCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// the distance from the target that the swarm scatters
            /// </summary>
            public float ScatterRadius;
            /// <summary>
            /// amount of time to remain scattered
            /// </summary>
            public float ScatterTime;
            public float HoundMinDistance;
            public float HoundMaxDistance;
            /// <summary>
            /// amount of randomness added to creature's throttle
            /// </summary>
            public float PerlinOffsetScale; // [0-1]
            /// <summary>
            /// how fast the creature changes random offset to throttle
            /// </summary>
            public Bounds<float> OffsetPeriod; // s
            /// <summary>
            /// a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
            /// </summary>
            public float PerlinIdleMovementThreshold; // [0-1]
            /// <summary>
            /// a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
            /// </summary>
            public float PerlinCombatMovementThreshold; // [0-1]
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterReadyBlock : TagStructure
        {
            /// <summary>
            /// Character will pause for given time before engaging threat
            /// </summary>
            public Bounds<float> ReadyTimeBounds;
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterEngageBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// When danger rises above the threshold, the actor crouches
            /// </summary>
            public float CrouchDangerThreshold;
            /// <summary>
            /// When danger drops below this threshold, the actor can stand again.
            /// </summary>
            public float StandDangerThreshold;
            /// <summary>
            /// When danger goes above given level, this actor switches firing positions
            /// </summary>
            public float FightDangerMoveThreshold;
            
            [Flags]
            public enum FlagsValue : uint
            {
                EngagePerch = 1 << 0,
                FightConstantMovement = 1 << 1,
                FlightFightConstantMovement = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class CharacterChargeBlock : TagStructure
        {
            public ChargeFlagsValue ChargeFlags;
            public float MeleeConsiderRange;
            /// <summary>
            /// chance of initiating a melee within a 1 second period
            /// </summary>
            public float MeleeChance;
            public float MeleeAttackRange;
            public float MeleeAbortRange;
            /// <summary>
            /// Give up after given amount of time spent charging
            /// </summary>
            public float MeleeAttackTimeout; // seconds
            /// <summary>
            /// don't attempt again before given time since last melee
            /// </summary>
            public float MeleeAttackDelayTimer; // seconds
            public Bounds<float> MeleeLeapRange;
            public float MeleeLeapChance;
            public float IdealLeapVelocity;
            public float MaxLeapVelocity;
            public float MeleeLeapBallistic;
            /// <summary>
            /// time between melee leaps
            /// </summary>
            public float MeleeDelayTimer; // seconds
            /// <summary>
            /// when I berserk, I pull out a ...
            /// </summary>
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag BerserkWeapon;
            
            [Flags]
            public enum ChargeFlagsValue : uint
            {
                OffhandMeleeAllowed = 1 << 0,
                BerserkWheneverCharge = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterEvasionBlock : TagStructure
        {
            /// <summary>
            /// Consider evading when immediate danger surpasses threshold
            /// </summary>
            public float EvasionDangerThreshold;
            /// <summary>
            /// Wait at least this delay between evasions
            /// </summary>
            public float EvasionDelayTimer;
            /// <summary>
            /// If danger is above threshold, the chance that we will evade. Expressed as chance of evading within a 1 second time period
            /// </summary>
            public float EvasionChance;
            /// <summary>
            /// If target is within given proximity, possibly evade
            /// </summary>
            public float EvasionProximityThreshold;
            /// <summary>
            /// Chance of retreating (fleeing) after danger avoidance dive
            /// </summary>
            public float DiveRetreatChance;
        }
        
        [TagStructure(Size = 0x40)]
        public class CharacterCoverBlock : TagStructure
        {
            public CoverFlagsValue CoverFlags;
            /// <summary>
            /// how long we stay behind cover after seeking cover
            /// </summary>
            public Bounds<float> HideBehindCoverTime; // seconds
            /// <summary>
            /// When vitality drops below this level, possibly trigger a cover
            /// </summary>
            public float CoverVitalityThreshold;
            /// <summary>
            /// trigger cover when shield drops below this fraction (low shield cover impulse must be enabled)
            /// </summary>
            public float CoverShieldFraction;
            /// <summary>
            /// amount of time I will wait before trying again after covering
            /// </summary>
            public float CoverCheckDelay;
            /// <summary>
            /// Emerge from cover when shield fraction reaches threshold
            /// </summary>
            public float EmergeFromCoverWhenShieldFractionReachesThreshold;
            /// <summary>
            /// Danger must be this high to cover. At a danger level of 'danger threshold', the chance of seeking cover is the cover
            /// chance lower bound (below)
            /// </summary>
            public float CoverDangerThreshold;
            /// <summary>
            /// At or above danger level of upper threshold, the chance of seeking cover is the cover chance upper bound (below)
            /// </summary>
            public float DangerUpperThreshold;
            /// <summary>
            /// The Bounds on the chance of seeking cover.
            /// The lower bound is valid when the danger is at 'danger threshold'
            /// The upper
            /// bound is valid when the danger is at or above 'danger upper threshold'.
            /// It is interpolated linearly everywhere in
            /// between.
            ///  All chances are expressed as 'chance of triggering cover in a 1 second period'.
            /// </summary>
            /// <summary>
            /// Bounds on the chances of seeking cover.
            /// </summary>
            public Bounds<float> CoverChance;
            /// <summary>
            /// When the proximity_self_preservation impulse is enabled, triggers self-preservation when target within this distance
            /// </summary>
            public float ProximitySelfPreserve; // wus
            /// <summary>
            /// Disallow covering from visible target under the given distance away
            /// </summary>
            public float DisallowCoverDistance; // world units
            /// <summary>
            /// When self preserving from a target less than given distance, causes melee attack (assuming proximity_melee_impulse is
            /// enabled)
            /// </summary>
            public float ProximityMeleeDistance;
            /// <summary>
            /// When danger from an unreachable enemy surpasses threshold, actor cover (assuming unreachable_enemy_cover impulse is
            /// enabled)
            /// </summary>
            public float UnreachableEnemyDangerThreshold;
            /// <summary>
            /// When target is aware of me and surpasses the given scariness, self-preserve (scary_target_cover_impulse)
            /// </summary>
            public float ScaryTargetThreshold;
            
            [Flags]
            public enum CoverFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class CharacterRetreatBlock : TagStructure
        {
            public RetreatFlagsValue RetreatFlags;
            /// <summary>
            /// When shield vitality drops below given amount, retreat is triggered by low_shield_retreat_impulse
            /// </summary>
            public float ShieldThreshold;
            /// <summary>
            /// When confronting an enemy of over the given scariness, retreat is triggered by scary_target_retreat_impulse
            /// </summary>
            public float ScaryTargetThreshold;
            /// <summary>
            /// When perceived danger rises above the given threshold, retreat is triggered by danger_retreat_impulse
            /// </summary>
            public float DangerThreshold;
            /// <summary>
            /// When enemy closer than given threshold, retreat is triggered by proximity_retreat_impulse
            /// </summary>
            public float ProximityThreshold;
            /// <summary>
            /// actor cowers for at least the given amount of time
            /// </summary>
            public Bounds<float> MinMaxForcedCowerTimeBounds;
            /// <summary>
            /// actor times out of cower after the given amount of time
            /// </summary>
            public Bounds<float> MinMaxCowerTimeoutBounds;
            /// <summary>
            /// If target reaches is within the given proximity, an ambush is triggered by the proximity ambush impulse
            /// </summary>
            public float ProximityAmbushThreshold;
            /// <summary>
            /// If target is less than threshold (0-1) aware of me, an ambush is triggered by the vulnerable enemy ambush impulse
            /// </summary>
            public float AwarenessAmbushThreshold;
            /// <summary>
            /// If leader-dead-retreat-impulse is active, gives the chance that we will flee when our leader dies within 4 world units of
            /// us
            /// </summary>
            public float LeaderDeadRetreatChance;
            /// <summary>
            /// If peer-dead-retreat-impulse is active, gives the chance that we will flee when one of our peers (friend of the same
            /// race) dies within 4 world units of us
            /// </summary>
            public float PeerDeadRetreatChance;
            /// <summary>
            /// If peer-dead-retreat-impulse is active, gives the chance that we will flee when a second peer (friend of the same race)
            /// dies within 4 world units of us
            /// </summary>
            public float SecondPeerDeadRetreatChance;
            /// <summary>
            /// The angle from the intended destination direction that a zig-zag will cause
            /// </summary>
            public Angle ZigZagAngle; // degrees
            /// <summary>
            /// How long it takes to zig left and then zag right.
            /// </summary>
            public float ZigZagPeriod; // seconds
            /// <summary>
            /// The likelihood of throwing down a grenade to cover our retreat
            /// </summary>
            public float RetreatGrenadeChance;
            /// <summary>
            /// If I want to flee and I don't have flee animations with my current weapon, throw it away and try a ...
            /// </summary>
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag BackupWeapon;
            
            [Flags]
            public enum RetreatFlagsValue : uint
            {
                ZigZagWhenFleeing = 1 << 0,
                Unused1 = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterSearchBlock : TagStructure
        {
            public SearchFlagsValue SearchFlags;
            public Bounds<float> SearchTime;
            /// <summary>
            /// Distance of uncover point from target. Hard lower limit, soft upper limit.
            /// </summary>
            public Bounds<float> UncoverDistanceBounds;
            
            [Flags]
            public enum SearchFlagsValue : uint
            {
                CrouchOnInvestigate = 1 << 0,
                WalkOnPursuit = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterPresearchBlock : TagStructure
        {
            public PreSearchFlagsValue PreSearchFlags;
            /// <summary>
            /// If the min presearch time expires and the target is (actually) outside the min-certainty radius, presearch turns off
            /// </summary>
            public Bounds<float> MinPresearchTime; // seconds
            /// <summary>
            /// Presearch turns off after the given time
            /// </summary>
            public Bounds<float> MaxPresearchTime; // seconds
            public float MinCertaintyRadius;
            public float Deprecated;
            /// <summary>
            /// if the min suppressing time expires and the target is outside the min-certainty radius, suppressing fire turns off
            /// </summary>
            public Bounds<float> MinSuppressingTime;
            
            [Flags]
            public enum PreSearchFlagsValue : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterIdleBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// time range for delays between idle poses
            /// </summary>
            public Bounds<float> IdlePoseDelayTime; // seconds
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterVocalizationBlock : TagStructure
        {
            /// <summary>
            /// How long does the player look at an AI before the AI responds?
            /// </summary>
            public float LookCommentTime; // s
            /// <summary>
            /// How long does the player look at the AI before he responds with his 'long look' comment?
            /// </summary>
            public float LookLongCommentTime; // s
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterBoardingBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// maximum distance from entry point that we will consider boarding
            /// </summary>
            public float MaxDistance; // wus
            /// <summary>
            /// give up trying to get in boarding seat if entry point is further away than this
            /// </summary>
            public float AbortDistance; // wus
            /// <summary>
            /// maximum speed at which we will consider boarding
            /// </summary>
            public float MaxSpeed; // wu/s
            
            [Flags]
            public enum FlagsValue : uint
            {
                AirborneBoarding = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterBossBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// when more than x damage is caused a juggernaut flurry is triggered
            /// </summary>
            public float FlurryDamageThreshold; // [0..1]
            /// <summary>
            /// flurry lasts this long
            /// </summary>
            public float FlurryTime; // seconds
        }
        
        [TagStructure(Size = 0xCC)]
        public class CharacterWeaponsBlock : TagStructure
        {
            public WeaponsFlagsValue WeaponsFlags;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            /// <summary>
            /// we can only fire our weapon at targets within this distance
            /// </summary>
            public float MaximumFiringRange; // world units
            /// <summary>
            /// weapon will not be fired at target closer than given distance
            /// </summary>
            public float MinimumFiringRange;
            public Bounds<float> NormalCombatRange; // world units
            /// <summary>
            /// we offset our burst targets randomly by this range when firing at non-visible enemies (zero = never)
            /// </summary>
            public float BombardmentRange;
            /// <summary>
            /// Specific target regions on a vehicle or unit will be fired upon only under the given distance
            /// </summary>
            public float MaxSpecialTargetDistance; // world units
            public Bounds<float> TimidCombatRange; // world units
            public Bounds<float> AggressiveCombatRange; // world units
            /// <summary>
            /// we try to aim our shots super-ballistically if target is outside this range (zero = never)
            /// </summary>
            public float SuperBallisticRange;
            /// <summary>
            /// At the min range, the min ballistic fraction is used, at the max, the max ballistic fraction is used
            /// </summary>
            public Bounds<float> BallisticFiringBounds; // world units
            /// <summary>
            /// Controls speed and degree of arc. 0 = high, slow, 1 = low, fast
            /// </summary>
            public Bounds<float> BallisticFractionBounds; // [0-1]
            public Bounds<float> FirstBurstDelayTime; // seconds
            public float SurpriseDelayTime; // seconds
            public float SurpriseFireWildlyTime; // seconds
            public float DeathFireWildlyChance; // [0,1]
            public float DeathFireWildlyTime; // seconds
            /// <summary>
            /// custom standing gun offset for overriding the default in the base actor
            /// </summary>
            public RealVector3d CustomStandGunOffset;
            /// <summary>
            /// custom crouching gun offset for overriding the default in the base actor
            /// </summary>
            public RealVector3d CustomCrouchGunOffset;
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
            /// damage modifier for special weapon fire (applied in addition to the normal damage modifier. zero = no change)
            /// </summary>
            public float SpecialDamageModifier; // [0,1]
            /// <summary>
            /// projectile error angle for special weapon fire (applied in addition to the normal error)
            /// </summary>
            public Angle SpecialProjectileError; // degrees
            /// <summary>
            /// amount of ammo loaded into the weapon that we drop (in fractions of a clip, e.g. 0.3 to 0.5)
            /// </summary>
            public Bounds<float> DropWeaponLoaded;
            /// <summary>
            /// total number of rounds in the weapon that we drop (ignored for energy weapons)
            /// </summary>
            public Bounds<short> DropWeaponAmmo;
            /// <summary>
            /// Parameters control how accuracy changes over the duration of a series of bursts
            /// Accuracy is an analog value between 0 and
            /// 1. At zero, the parameters of the first
            /// firing-pattern block is used. At 1, the parameters in the second block is used.
            /// In
            /// between, all the values are linearly interpolated
            /// </summary>
            /// <summary>
            /// Indicates starting and ending accuracies at normal difficulty
            /// </summary>
            public Bounds<float> NormalAccuracyBounds;
            /// <summary>
            /// The amount of time it takes the accuracy to go from starting to ending
            /// </summary>
            public float NormalAccuracyTime;
            /// <summary>
            /// Indicates starting and ending accuracies at heroic difficulty
            /// </summary>
            public Bounds<float> HeroicAccuracyBounds;
            /// <summary>
            /// The amount of time it takes the accuracy to go from starting to ending
            /// </summary>
            public float HeroicAccuracyTime;
            /// <summary>
            /// Indicates starting and ending accuracies at legendary difficulty
            /// </summary>
            public Bounds<float> LegendaryAccuracyBounds;
            /// <summary>
            /// The amount of time it takes the accuracy to go from starting to ending
            /// </summary>
            public float LegendaryAccuracyTime;
            public List<CharacterFiringPatternBlock> FiringPatterns;
            [TagField(ValidTags = new [] { "jpt!" })]
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
            public class CharacterFiringPatternBlock : TagStructure
            {
                /// <summary>
                /// how many times per second we pull the trigger (zero = continuously held down)
                /// </summary>
                public float RateOfFire;
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
                /// what fraction of its normal damage our weapon inflicts (zero = no modifier)
                /// </summary>
                public float WeaponDamageModifier;
                /// <summary>
                /// error added to every projectile we fire
                /// </summary>
                public Angle ProjectileError; // degrees
                /// <summary>
                /// the maximum rate at which we can sweep our fire (zero = unlimited)
                /// </summary>
                public Angle BurstAngularVelocity; // degrees per second
                /// <summary>
                /// cap on the maximum angle by which we will miss target (restriction on burst origin radius
                /// </summary>
                public Angle MaximumErrorAngle; // degrees
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterFiringPatternPropertiesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            public List<CharacterFiringPatternBlock> FiringPatterns;
            
            [TagStructure(Size = 0x40)]
            public class CharacterFiringPatternBlock : TagStructure
            {
                /// <summary>
                /// how many times per second we pull the trigger (zero = continuously held down)
                /// </summary>
                public float RateOfFire;
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
                /// what fraction of its normal damage our weapon inflicts (zero = no modifier)
                /// </summary>
                public float WeaponDamageModifier;
                /// <summary>
                /// error added to every projectile we fire
                /// </summary>
                public Angle ProjectileError; // degrees
                /// <summary>
                /// the maximum rate at which we can sweep our fire (zero = unlimited)
                /// </summary>
                public Angle BurstAngularVelocity; // degrees per second
                /// <summary>
                /// cap on the maximum angle by which we will miss target (restriction on burst origin radius
                /// </summary>
                public Angle MaximumErrorAngle; // degrees
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class CharacterGrenadesBlock : TagStructure
        {
            public GrenadesFlagsValue GrenadesFlags;
            /// <summary>
            /// type of grenades that we throw^
            /// </summary>
            public GrenadeTypeValue GrenadeType;
            /// <summary>
            /// how we throw our grenades
            /// </summary>
            public TrajectoryTypeValue TrajectoryType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// how many enemies must be within the radius of the grenade before we will consider throwing there
            /// </summary>
            public short MinimumEnemyCount;
            /// <summary>
            /// we consider enemies within this radius when determining where to throw
            /// </summary>
            public float EnemyRadius; // world units
            /// <summary>
            /// how fast we LIKE to throw our grenades
            /// </summary>
            public float GrenadeIdealVelocity; // world units per second
            /// <summary>
            /// the fastest we can possibly throw our grenades
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
            /// how likely we are to throw a grenade in one second
            /// </summary>
            public float GrenadeChance; // [0,1]
            /// <summary>
            /// How long we have to wait after throwing a grenade before we can throw another one
            /// </summary>
            public float GrenadeThrowDelay; // seconds
            /// <summary>
            /// how likely we are to throw a grenade to flush out a target in one second
            /// </summary>
            public float GrenadeUncoverChance; // [0,1]
            /// <summary>
            /// how likely we are to throw a grenade against a vehicle
            /// </summary>
            public float AntiVehicleGrenadeChance; // [0,1]
            /// <summary>
            /// number of grenades that we start with
            /// </summary>
            public Bounds<short> GrenadeCount;
            /// <summary>
            /// how likely we are not to drop any grenades when we die, even if we still have some
            /// </summary>
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
        
        [TagStructure(Size = 0xB4)]
        public class CharacterVehicleBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            [TagField(ValidTags = new [] { "styl" })]
            public CachedTag Style;
            public VehicleFlagsValue VehicleFlags;
            /// <summary>
            /// (Ground vehicles)
            /// </summary>
            public float AiPathfindingRadius; // world units
            /// <summary>
            /// (All vehicles) Distance within which goal is considered reached
            /// </summary>
            public float AiDestinationRadius; // world units
            /// <summary>
            /// (All vehicles)Distance from goal at which AI starts to decelerate
            /// </summary>
            public float AiDecelerationDistanceworldUnits;
            /// <summary>
            /// (Warthog, Pelican, Ghost) Idealized average turning radius (should reflect actual vehicle physics)
            /// </summary>
            public float AiTurningRadius;
            /// <summary>
            /// (Warthog-type) Idealized minimum turning radius (should reflect actual vehicle physics)
            /// </summary>
            public float AiInnerTurningRadiusTr;
            /// <summary>
            /// (Warthogs, ghosts) Ideal turning radius for rounding turns (barring obstacles, etc.)
            /// </summary>
            public float AiIdealTurningRadiusTr;
            /// <summary>
            /// (Banshee)
            /// </summary>
            public Angle AiBansheeSteeringMaximum;
            /// <summary>
            /// (Warthog, ghosts, wraiths)Maximum steering angle from forward (ultimately controls turning speed)
            /// </summary>
            public float AiMaxSteeringAngle; // degrees
            /// <summary>
            /// (pelicans, dropships, ghosts, wraiths)Maximum delta in steering angle from one tick to the next (ultimately controls turn
            /// acceleration)
            /// </summary>
            public float AiMaxSteeringDelta; //  degrees
            /// <summary>
            /// (Warthog, ghosts, wraiths)
            /// </summary>
            public float AiOversteeringScale;
            /// <summary>
            /// (Banshee) Angle to goal at which AI will oversteer
            /// </summary>
            public Bounds<Angle> AiOversteeringBounds;
            /// <summary>
            /// (Ghosts, Dropships) Distance within which Ai will strafe to target (as opposed to turning)
            /// </summary>
            public float AiSideslipDistance;
            /// <summary>
            /// (Banshee-style) Look-ahead distance for obstacle avoidance
            /// </summary>
            public float AiAvoidanceDistance; // world units
            /// <summary>
            /// (Banshees)The minimum urgency with which a turn can be made (urgency = percent of maximum steering delta)
            /// </summary>
            public float AiMinUrgency; // [0-1]
            /// <summary>
            /// (All vehicles)
            /// </summary>
            public float AiThrottleMaximum; // (0 - 1)
            /// <summary>
            /// (Warthogs, Dropships, ghosts)scale on throttle when within 'ai deceleration distance' of goal (0...1)
            /// </summary>
            public float AiGoalMinThrottleScale;
            /// <summary>
            /// (Warthogs, ghosts) Scale on throttle due to nearness to a turn (0...1)
            /// </summary>
            public float AiTurnMinThrottleScale;
            /// <summary>
            /// (Warthogs, ghosts) Scale on throttle due to facing away from intended direction (0...1)
            /// </summary>
            public float AiDirectionMinThrottleScale;
            /// <summary>
            /// (warthogs, ghosts) The maximum allowable change in throttle between ticks
            /// </summary>
            public float AiAccelerationScale; // (0-1)
            /// <summary>
            /// (dropships, sentinels) The degree of throttle blending between one tick and the next (0 = no blending)
            /// </summary>
            public float AiThrottleBlend; // (0-1)
            /// <summary>
            /// (dropships, warthogs, ghosts) About how fast I can go.
            /// </summary>
            public float TheoreticalMaxSpeed; // wu/s
            /// <summary>
            /// (dropships, warthogs) scale on the difference between desired and actual speed, applied to throttle
            /// </summary>
            public float ErrorScale;
            public Angle AiAllowableAimDeviationAngle;
            /// <summary>
            /// (All vehicles) The distance at which the tight angle criterion is used for deciding to vehicle charge
            /// </summary>
            public float AiChargeTightAngleDistance;
            /// <summary>
            /// (All vehicles) Angle cosine within which the target must be when target is closer than tight angle distance in order to
            /// charge
            /// </summary>
            public float AiChargeTightAngle; // [0-1]
            /// <summary>
            /// (All vehicles) Time delay between vehicle charges
            /// </summary>
            public float AiChargeRepeatTimeout;
            /// <summary>
            /// (All vehicles) In deciding when to abort vehicle charge, look ahead these many seconds to predict time of contact
            /// </summary>
            public float AiChargeLookAheadTime;
            /// <summary>
            /// Consider charging the target when it is within this range (0 = infinite distance)
            /// </summary>
            public float AiChargeConsiderDistance;
            /// <summary>
            /// Abort the charge when the target get more than this far away (0 = never abort)
            /// </summary>
            public float AiChargeAbortDistance;
            /// <summary>
            /// The ram behavior stops after a maximum of the given number of seconds
            /// </summary>
            public float VehicleRamTimeout;
            /// <summary>
            /// The ram behavior freezes the vehicle for a given number of seconds after performing the ram
            /// </summary>
            public float RamParalysisTime;
            /// <summary>
            /// (All vehicles) Trigger a cover when recent damage is above given threshold (damage_vehicle_cover impulse)
            /// </summary>
            public float AiCoverDamageThreshold;
            /// <summary>
            /// (All vehicles) When executing vehicle-cover, minimum distance from the target to flee to
            /// </summary>
            public float AiCoverMinDistance;
            /// <summary>
            /// (All vehicles) How long to stay away from the target
            /// </summary>
            public float AiCoverTime;
            /// <summary>
            /// (All vehicles) Boosting allowed when distance to cover destination is greater then this.
            /// </summary>
            public float AiCoverMinBoostDistance;
            /// <summary>
            /// If vehicle turtling behavior is enabled, turtling is initiated if 'recent damage' surpasses the given threshold
            /// </summary>
            public float TurtlingRecentDamageThreshold; // %
            /// <summary>
            /// If the vehicle turtling behavior is enabled, turtling occurs for at least the given time
            /// </summary>
            public float TurtlingMinTime; // seconds
            /// <summary>
            /// The turtled state times out after the given number of seconds
            /// </summary>
            public float TurtlingTimeout; // seconds
            public ObstacleIgnoreSizeValue ObstacleIgnoreSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
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

