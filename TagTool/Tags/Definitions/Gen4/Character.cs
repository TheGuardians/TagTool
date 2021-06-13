using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "character", Tag = "char", Size = 0x33C)]
    public class Character : TagStructure
    {
        public CharacterFlags CharacterFlags1;
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag ParentCharacter;
        [TagField(ValidTags = new [] { "unit" })]
        public CachedTag Unit;
        [TagField(ValidTags = new [] { "crea" })]
        // Creature reference for swarm characters ONLY
        public CachedTag Creature;
        [TagField(ValidTags = new [] { "styl" })]
        public CachedTag Style;
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag MajorCharacter;
        public List<CharacterVariantsBlock> Variants;
        public List<CharacterVoicePropertiesBlock> Voice;
        public List<CharacterGeneralBlock> GeneralProperties;
        public List<CharacterProtoSpawnBlock> ProtoSpawnProperties;
        public List<CharacterInteractBlock> InteractProperties;
        public List<CharacterEmotionsBlock> EmotionProperties;
        public List<CharacterVitalityBlock> VitalityProperties;
        public List<CharacterPlacementBlock> PlacementProperties;
        public List<CharacterPerceptionBlockStruct> PerceptionProperties;
        public List<CharacterTargetBlockStruct> TargetProperties;
        public List<CharacterLookBlock> LookProperties;
        public List<CharacterHoppingBlock> HoppingProperties;
        public List<CharacterMovementBlock> MovementProperties;
        public List<CharacterThrottleStyleBlock> ThrottleStyles;
        public List<CharacterMovementSetBlock> MovementSets;
        public List<CharacterFlockingBlock> FlockingProperties;
        public List<CharacterSwarmBlock> SwarmProperties;
        public List<CharacterFiringPointEvaluatorBlockStruct> FiringPointEvaluatorProperties;
        public List<CharacterReadyBlock> ReadyProperties;
        public List<CharacterEngageBlock> EngageProperties;
        public List<CharacterChargeBlock> ChargeProperties;
        public List<CharacterEvasionBlock> EvasionProperties;
        public List<CharacterCoverBlock> CoverProperties;
        public List<CharacterRetreatBlock> RetreatProperties;
        public List<CharacterSearchBlock> SearchProperties;
        public List<CharacterPresearchBlock> PreSearchProperties;
        public List<CharacterIdleBlock> IdleProperties;
        public List<CharacterVocalizationBlock> VocalizationProperties;
        public List<CharacterBoardingBlock> BoardingProperties;
        public List<CharacterKungfuBlock> KungfuProperties;
        public List<CharacterBunkerBlock> BunkerProperties;
        public List<CharacterGuardianBlock> GuardianProperties;
        public List<CharacterCombatformBlock> CombatformProperties;
        public List<CharacterEngineerBlock> EngineerProperties;
        public List<CharacterInspectBlock> InspectProperties;
        public List<CharacterScarabBlock> ScarabProperties;
        public List<CharacterWeaponsBlock> WeaponsProperties;
        public List<CharacterFiringPatternPropertiesBlock> FiringPatternProperties;
        public List<CharacterFiringPatternPropertiesBlock> ExtremeRangeFiringPatternProperties;
        public List<CharacterGrenadesBlock> GrenadesProperties;
        public List<CharacterVehicleBlock> VehicleProperties;
        public List<CharacterFlyingMovementBlockStruct> FlyingMovementProperties;
        public List<CharacterMorphBlock> MorphProperties;
        public List<CharacterEquipmentBlock> EquipmentDefinitions;
        public List<CharacterStimuliResponseBlock> StimuliResponses;
        public List<CampaignMetagameBucketBlock> CampaignMetagameBucket;
        public List<CharacterActivityObjectBlock> ActivityObjects;
        public List<CharacterPainScreenBlock> PainScreenProperties;
        public List<CharacterBishopBlock> BishopProperties;
        public List<CharacterCombotronParentBlock> CombotronParentProperties;
        public List<CharacterCombotronChildBlock> CombotronChildProperties;
        public List<CharacterHandleDismembermentBlock> HandleDismembermentProperties;
        public List<CharacterCoverFightBlock> FightFromCover;
        public List<CharacterEmergeBlock> Emerge;
        public List<DynamicTaskBlock> DynamicTask;
        public List<CharacterAdvanceBlock> AdvanceProperties;
        public List<CharacterCoverEvasionBlock> CoverEvasion;
        public List<CharacterPackStalkBlock> PackStalk;
        public List<CharacterFightCircleBlock> FightCircle;
        public List<CharacterHamstringChargeBlock> Hamstring;
        public List<CharacterForerunnerBlock> Forerunner;
        public List<CharacterGravityJumpBlock> GravityJump;
        
        [Flags]
        public enum CharacterFlags : uint
        {
            Flag1 = 1 << 0
        }
        
        [TagStructure(Size = 0x18)]
        public class CharacterVariantsBlock : TagStructure
        {
            public StringId VariantName;
            public short VariantIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<CharacterVoiceBlock> Voices;
            // gets applied if the vocalization has no dialogue effect id.
            public StringId DefaultDialogueEffectId;
            
            [TagStructure(Size = 0x24)]
            public class CharacterVoiceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "udlg" })]
                public CachedTag Dialogue;
                public StringId Designator;
                public float Weight;
                public List<CharacterVoiceRegionFilterBlockStruct> RegionFilters;
                
                [TagStructure(Size = 0x10)]
                public class CharacterVoiceRegionFilterBlockStruct : TagStructure
                {
                    public StringId RegionName;
                    public List<CharacterVoiceRegionPermutationFilterBlockStruct> PermutationFilters;
                    
                    [TagStructure(Size = 0x4)]
                    public class CharacterVoiceRegionPermutationFilterBlockStruct : TagStructure
                    {
                        public StringId PermutationName;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterVoicePropertiesBlock : TagStructure
        {
            public List<CharacterVoiceBlock> Voices;
            // gets applied if the vocalization has no dialogue effect id.
            public StringId DefaultDialogueEffectId;
            
            [TagStructure(Size = 0x24)]
            public class CharacterVoiceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "udlg" })]
                public CachedTag Dialogue;
                public StringId Designator;
                public float Weight;
                public List<CharacterVoiceRegionFilterBlockStruct> RegionFilters;
                
                [TagStructure(Size = 0x10)]
                public class CharacterVoiceRegionFilterBlockStruct : TagStructure
                {
                    public StringId RegionName;
                    public List<CharacterVoiceRegionPermutationFilterBlockStruct> PermutationFilters;
                    
                    [TagStructure(Size = 0x4)]
                    public class CharacterVoiceRegionPermutationFilterBlockStruct : TagStructure
                    {
                        public StringId PermutationName;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class CharacterGeneralBlock : TagStructure
        {
            public GeneralFlags GeneralFlags1;
            public ActorTypeEnum Type;
            // the rank of this character, helps us work out who in a squad should be a leader (0 is lowly, 32767 is highest)
            public short Rank;
            // where should my followers try and position themselves when I am their leader?
            public CombatPositioningEnum FollowerPositioning;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // don't let my combat range get outside this distance from my leader when in combat (if 0 then defaults to 4wu)
            public float MaxLeaderDist;
            // never select firing positions outside this range from the leader (if 0 then defaults to 8wu)
            public float AbsoluteMaxLeaderDist;
            // never play dialogue if all players are outside of this range (if 0 then defaults to 20wu)
            public float MaxPlayerDialogueDist;
            // the inherent scariness of the character
            public float Scariness;
            public GlobalAiGrenadeTypeEnum DefaultGrenadeType;
            public BehaviorTreeRootEnum BehaviorTreeRoot;
            public List<DisallowedWeaponsForTradingBlock> DisallowedWeaponsFromTrading;
            [TagField(ValidTags = new [] { "weap" })]
            // Overrides initial primary weapon set in unit tag.
            public CachedTag InitialPrimaryWeapon;
            [TagField(ValidTags = new [] { "weap" })]
            // Overrides initial secondary weapon set in unit tag.
            public CachedTag InitialSecondaryWeapon;
            [TagField(ValidTags = new [] { "eqip" })]
            // Fallback used if initial equipment not specified as drop item or in scenario.
            public CachedTag InitialEquipment;
            
            [Flags]
            public enum GeneralFlags : uint
            {
                Swarm = 1 << 0,
                Flying = 1 << 1,
                DualWields = 1 << 2,
                UsesGravemind = 1 << 3,
                GravemindChorus = 1 << 4,
                DonTTradeWeapon = 1 << 5,
                DonTStowWeapon = 1 << 6,
                HeroCharacter = 1 << 7,
                LeaderIndependentPositioning = 1 << 8,
                HasActiveCamo = 1 << 9,
                UseHeadMarkerForLooking = 1 << 10,
                SpaceCharacter = 1 << 11,
                DoNotDropEquipment = 1 << 12,
                DoNotAllowCrouch = 1 << 13,
                DoNotAllowMovingCrouch = 1 << 14,
                CriticalBetrayal = 1 << 15,
                DeathlessCriticalBetrayal = 1 << 16
            }
            
            public enum ActorTypeEnum : short
            {
                None,
                Player,
                Marine,
                Crew,
                Spartan,
                Elite,
                Jackal,
                Grunt,
                Brute,
                Hunter,
                Prophet,
                Bugger,
                Scarab,
                Engineer,
                Skirmisher,
                Bishop,
                Knight,
                Pawn,
                Rook,
                Mule,
                MountedWeapon,
                Octopus
            }
            
            public enum CombatPositioningEnum : short
            {
                InFrontOfMe,
                BehindMe,
                Tight
            }
            
            public enum GlobalAiGrenadeTypeEnum : short
            {
                None,
                HumanGrenade,
                CovenantPlasma,
                BruteClaymore,
                Firebomb
            }
            
            public enum BehaviorTreeRootEnum : short
            {
                Default,
                Scarab,
                Flying
            }
            
            [TagStructure(Size = 0x10)]
            public class DisallowedWeaponsForTradingBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "weap" })]
                public CachedTag Weapon;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class CharacterProtoSpawnBlock : TagStructure
        {
            // Can be used to automatically setup a character to be spawned by another character.
            public ProtoSpawnTypeEnum ProtoSpawnType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum ProtoSpawnTypeEnum : short
            {
                None,
                Limbo,
                Spawner,
                Birther
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class CharacterInteractBlock : TagStructure
        {
            public float DefaultMaximumObjectInteractRange; // wu
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterEmotionsBlock : TagStructure
        {
            public List<CharacterEmotionsSituationalDangerBlock> SituationalDanger;
            // How many seconds until it rises up to half between its current and target value
            public float PerceivedDangerIncreaseHalfLife; // seconds
            // How many seconds until it decays to half between its current and target values
            public float PerceivedDangerDecayHalfLife; // seconds
            
            [TagStructure(Size = 0x8)]
            public class CharacterEmotionsSituationalDangerBlock : TagStructure
            {
                // The prop class that this block is describing
                public PropClassEnum HighestPropClass;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // What should be the situational danger level for the prop class selected above
                public float SituationalDanger;
                
                public enum PropClassEnum : short
                {
                    NoneActorHasNoTargetsWhatsoever,
                    DeadEnemyMyOnlyTargetIsADeadEnemy,
                    NonAttackingEnemyIAmDebilitatedAndTheEnemyIsNotAttackingMe,
                    DisregardedOrphanDidNotFindMyTargetAndGaveUpSearching,
                    InspectedOrphanCheckedLastKnowPositionAndDidnTFindTargetStillSearching,
                    UninspectedOrphanCanTSeeTargetButHaveAGoodIdeaWhereTheyMayBe,
                    CertainOrphanCanTSeeTargetButKnowExactlyWhereTheyAre,
                    VisibleEnemyICanSeeTheTarget,
                    NearbyEnemyICanSeeTheTargetAndItSClose,
                    PotentiallyDangerousEnemyTargetIsNearAndIsFacingMe,
                    DangerousEnemyTargetIsFacingMyWayAndFighting,
                    AttackingEnemyTargetIsAimingAtMeAndShootingMe,
                    VeryCloseEnemyEnemyIsReallyClose,
                    DamagingEnemyEnemyIsDamagingMe
                }
            }
        }
        
        [TagStructure(Size = 0x68)]
        public class CharacterVitalityBlock : TagStructure
        {
            public VitalityFlags VitalityFlags1;
            // maximum body vitality of our unit
            public float NormalBodyVitality;
            // maximum shield vitality of our unit
            public float NormalShieldVitality;
            // maximum body vitality of our unit (on legendary)
            public float LegendaryBodyVitality;
            // maximum shield vitality of our unit (on legendary)
            public float LegendaryShieldVitality;
            // fraction of body health that can be regained after damage
            public float BodyRechargeFraction;
            // THIS IS NOW AN ABSOLUTE VALUE, NOT A FRACTION
            public float SoftPingThreshold; // damage necessary to trigger a soft ping when shields are up
            // THIS IS NOW AN ABSOLUTE VALUE, NOT A FRACTION
            public float SoftPingThreshold1; // damage necessary to trigger a soft ping when shields are down
            // THIS IS NOW AN ABSOLUTE VALUE, NOT A FRACTION
            public float HardPingThreshold; // damage necessary to trigger a hard ping when shields are up
            // THIS IS NOW AN ABSOLUTE VALUE, NOT A FRACTION
            public float HardPingThreshold1; // damage necessary to trigger a hard ping when shields are down
            // minimum time before another hard ping can be triggered
            public float HardPingCooldownTime;
            // amount of time delay before a shield begins to recharge
            public float BodyRechargeDelayTime;
            // amount of time for shields to recharge completely
            public float BodyRechargeTime;
            // amount of time delay before a shield begins to recharge
            public float ShieldRechargeDelayTime;
            // amount of time for shields to recharge completely
            public float ShieldRechargeTime;
            // Amount of shield damage sustained before it is considered 'extended'
            public float ExtendedShieldDamageThreshold; // %
            // Amount of body damage sustained before it is considered 'extended'
            public float ExtendedBodyDamageThreshold; // %
            // when I die and explode, I damage stuff within this distance of me.
            public float SuicideRadius;
            public float RuntimeBodyRechargeVelocity;
            public float RuntimeShieldRechargeVelocity;
            [TagField(ValidTags = new [] { "weap" })]
            // If I'm being automatically resurrected then I pull out a ...
            public CachedTag ResurrectWeapon;
            // If the player is hurting me, scale the damage by this amount. (0 value defaults to 1)
            public float PlayerDamageScale; // [0-1]
            public float ProjectileAttachedDetonationTimeScale;
            
            [Flags]
            public enum VitalityFlags : uint
            {
                AutoResurrect = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class CharacterPlacementBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float FewUpgradeChance;
            public float FewUpgradeChance1;
            public float FewUpgradeChance2;
            public float FewUpgradeChance3;
            public float NormalUpgradeChance;
            public float NormalUpgradeChance1;
            public float NormalUpgradeChance2;
            public float NormalUpgradeChance3;
            public float ManyUpgradeChance;
            public float ManyUpgradeChance1;
            public float ManyUpgradeChance2;
            public float ManyUpgradeChance3;
        }
        
        [TagStructure(Size = 0xA0)]
        public class CharacterPerceptionBlockStruct : TagStructure
        {
            public ActorPerceptionModeDefinition PerceptionMode;
            public PerceptionFlags Flags;
            // maximum range of sight
            public float MaximumVisionDistance; // world units
            // reliable range of sight
            public float ReliableVisionDistance; // world units
            // maximum range of peripheral vision
            public float MaximumPeripheralVisionDistance; // world units
            // reliable range of peripheral vision
            public float ReliablePeripheralVisionDistance; // world units
            // minimum range of peripheral vision (at peripheral vision angle)
            public float MinimumPeripheralVisionDistance; // world units
            // minimum reliable range of peripheral vision (at peripheral vision angle)
            public float MinimumReliablePeripheralVisionDistance; // world units
            // If a new prop is acknowledged within the given distance, surprise is registered
            public float SurpriseDistance; // world units
            // horizontal angle within which we see targets out to our maximum range
            public Angle FocusInteriorAngle; // degrees
            // horizontal angle within which we see targets at a range in between maximum and maximum peripheral
            public Angle FocusExteriorAngle; // degrees
            // maximum horizontal angle within which we can see targets out of the corner of our eye up to maximum peripheral
            // vision
            public Angle PeripheralVisionAngle; // degrees
            // maximum range at which sounds can be heard
            public float HearingDistance; // world units
            // random chance of noticing a dangerous enemy projectile (e.g. grenade)
            public float NoticeProjectileChance; // [0,1]
            // random chance of noticing a dangerous vehicle
            public float NoticeVehicleChance; // [0,1]
            // time required to acknowledge a visible enemy at optimal range
            public float PerceptionTime; // seconds
            // How aware of you while acknowledging an AI must be to glance at you
            public float AwarenessGlanceLevel01;
            // While acknowledging, the awareness delta at which an AI will glance at you
            public float AwarenessGlanceDelta;
            // The chance that an AI identifies a unit is actually a hologram
            public float IdentifyHologramChance; // [0, 1]
            // The time after which we will ignore the hologram once seen
            public Bounds<float> HologramIgnoreTimer; // seconds
            // The number of seconds taken off of the ignore timer each time the hologram is shot
            public float HologramIgnoreTimerShotPenalty; // seconds
            // Distance below which the AI becomes aware of you even if you are camouflaged, normal difficulty
            public float CamouflagedEnemyVisibleDistance; // wu
            // Distance below which the AI becomes aware of you even if you are camouflaged, lengendary difficulty
            public float CamouflagedEnemyVisibleDistance1; // wu
            public MappingFunction Mapping;
            public ActiveCamoPerceptionProperties NormalActiveCamoPerception;
            public ActiveCamoPerceptionProperties LegendaryActiveCamoPerception;
            
            public enum ActorPerceptionModeDefinition : short
            {
                Idle,
                Alert,
                Combat,
                Search,
                Patrol,
                VehicleIdle,
                VehicleAlert,
                VehicleCombat
            }
            
            [Flags]
            public enum PerceptionFlags : ushort
            {
                CharacterCanSeeInDarkness = 1 << 0,
                IgnoreTrackingProjectiles = 1 << 1,
                IgnoreMinorTrackingProjectiles = 1 << 2
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
            
            [TagStructure(Size = 0x18)]
            public class ActiveCamoPerceptionProperties : TagStructure
            {
                // this amount of active camouflage makes a target 'partially invisible'
                public float PartialInvisAmount; // [0,1]
                // maximum vision distance for partially invisible targets. 0= unlimited
                public float PartialInvisVisionDistance; // world units
                // multiplier on our awareness speed for partially invisible targets. 0= no change. Should be in (0, 1].
                public float PartialInvisAwarenessMultiplier; // [0,1]
                // this amount of active camouflage makes a target 'fully invisible'
                public float FullInvisAmount; // [0,1]
                // maximum vision distance for fully invisible targets. 0= unlimited
                public float FullInvisVisionDistance; // world units
                // multiplier on our awareness speed for fully invisible targets. 0= no change. Should be in (0, 1].
                public float FullInvisAwarenessMultiplier; // [0,1]
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class CharacterTargetBlockStruct : TagStructure
        {
            // How interested in the player I am on a scale of 0-1
            public float PlayerPreference; // [0-1]
        }
        
        [TagStructure(Size = 0x50)]
        public class CharacterLookBlock : TagStructure
        {
            // how far we can turn our weapon
            public RealEulerAngles2d MaximumAimingDeviation; // degrees
            // how far we can turn our head
            public RealEulerAngles2d MaximumLookingDeviation; // degrees
            public RealEulerAngles2d RuntimeAimingDeviationCosines;
            public RealEulerAngles2d RuntimeLookingDeviationCosines;
            // how far we can turn our head left away from our aiming vector when not in combat
            public Angle NoncombatLookDeltaL; // degrees
            // how far we can turn our head right away from our aiming vector when not in combat
            public Angle NoncombatLookDeltaR; // degrees
            // how far we can turn our head left away from our aiming vector when in combat
            public Angle CombatLookDeltaL; // degrees
            // how far we can turn our head right away from our aiming vector when in combat
            public Angle CombatLookDeltaR; // degrees
            // rate at which we change look around randomly when not in combat
            public Bounds<float> NoncombatIdleLooking; // seconds
            // rate at which we change aiming directions when looking around randomly when not in combat
            public Bounds<float> NoncombatIdleAiming; // seconds
            // rate at which we change look around randomly when searching or in combat
            public Bounds<float> CombatIdleLooking; // seconds
            // rate at which we change aiming directions when looking around randomly when searching or in combat
            public Bounds<float> CombatIdleAiming; // seconds
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterHoppingBlock : TagStructure
        {
            public HoppingFlags HoppingFlags1;
            public List<CharacterhopDefinitionBlock> HoppingDefinition;
            
            [Flags]
            public enum HoppingFlags : uint
            {
                ToCoverPathSegements = 1 << 0,
                ToEndOfPath = 1 << 1,
                ForwardOnly = 1 << 2
            }
            
            [TagStructure(Size = 0x48)]
            public class CharacterhopDefinitionBlock : TagStructure
            {
                public CharacterHopStruct Default;
                public CharacterHopStruct Passive;
                public CharacterHopStruct Aggressive;
                
                [TagStructure(Size = 0x18)]
                public class CharacterHopStruct : TagStructure
                {
                    // Pathing shorter than this, no hopping
                    public float MinHopDistance;
                    // Pathing shorter than this, no hopping to end of path
                    public float MinHopDistanceToPathEnd;
                    // Character will wait this random ranged timer before hopping again.(Seconds)
                    public Bounds<float> HopWaitTimerMinMax;
                    // Pathing longer than this, no hopping.
                    public float MaxHopDistance;
                    public float Pad;
                }
            }
        }
        
        [TagStructure(Size = 0x108)]
        public class CharacterMovementBlock : TagStructure
        {
            public MovementFlags MovementFlags1;
            public float PathfindingRadius;
            // If 0, uses pathfinding radius.
            public float AvoidanceRadius;
            public float DestinationRadius;
            // Chance the AI will use their armor lock equipment, assuming they have it
            public float ArmorLockChance;
            // Chance the AI will use their armor lock equipment if they have been stuck with a grenade, assuming they have it
            public float GrenadeStuckArmorLockChance;
            // The number of seconds we will stay in armor lock for after danger has passed (default 1 second)
            public float ArmorLockSafetyDuration;
            // The longest we will stay in armor lock for regardless of danger (default 5 seconds)
            public float ArmorLockMaxDuration;
            // We won't go into armor lock again for this many seconds (default 0 seconds)
            public float ArmorLockCooldown;
            public float DiveGrenadeChance;
            public float BraceGrenadeChance;
            public ObstacleIgnoreEnum ObstacleLeapMinSize;
            public ObstacleIgnoreEnum ObstacleLeapMaxSize;
            public ObstacleIgnoreEnum ObstacleIgnoreSize;
            public ObstacleIgnoreEnum ObstacleSmashableSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public GlobalAiJumpHeightEnum JumpHeight;
            // .How high can a crate be for this unit to leap it.
            public float MaximumLeapHeight; // wus
            // How close to the obstacle should the actor be before leaping 1- too close, 0- as soon as he becomes aware of it
            public float LeapProximityFraction; // [0-1]
            // The maximum distance penalty applied to an avoidance volume search path if we're facing away from the path. 1000 wu
            // good for space, 5 wu good for ground.
            public float AvoidanceVolumeTurnPenaltyDistance; // wus
            public MovementHintEnum MovementHints;
            // We will move at least this long in a single direction when starting movement
            public int MinimumMovementTicks; // ticks
            // If the character changes movement direction by more than this angle, he will have to move for at least minimum
            // movement ticks until he can change his mind.
            public float MinimumMovementTicksResetAngle; // degrees
            public List<MovementStationaryPauseBlock> ChangeDirectionPause;
            // The character will never throttle beyond this value
            public float MaximumThrottle; // [0-1]
            // The character will not throttle below this value
            public float MinimumThrottle; // [0-1]
            public List<MovementThrottleControlBlock> MovementThrottleControl;
            // The character will consider juking at this throttle and above
            public float MinimumJukeThrottle; // [0-1]
            // If we change movement direction by more this angle, we will attempt a juke
            public Angle MinimumDirectionChangeJukeAngle; // deg
            // Probability to do a juke for a given tick, even if you are not planning to change direction (and provided you have
            // not already performed a juke within the timeout time
            public float NonDirectionChangeJukeProbability;
            // After you do a change or no change of direction juke, you cannot perform a NON directional change juke for at least
            // this many seconds. Direction change jukes will still happen
            public float NonDirectionChangeJukeTimeout; // seconds
            // How many ticks should the actor keep moving after a juke? This may lower juke frequency.
            public int MinimumPostJukeMovementTicks; // ticks
            // If this actor translates during turn animations, enter a radius that encloses the translation.
            public float StationaryTurnRadius; // [wu]
            // Distance to move as per the move_localized firing position evaluator (0 value resolves to 5wu)
            public float LocalizedMoveDistance; // [wu]
            // Distance to move as per the move_distance firing position evaluator (0 value resolves to 5wu for min, 10wu for max)
            public Bounds<float> MoveDistance; // [wu]
            // Distance to move as per the vehicle_move_distance firing position evaluator (0 value resolves to 5wu for min, 10wu
            // for max)
            public Bounds<float> VehicleMoveDistance; // [wu]
            // Actor will face away from his target and run to his destination if his target at a larger distance than this
            public float TurnAndRunDistanceFromTarget; // wus
            // Firing point must be at least this distance away from the actor for him to consider turning and running to it
            public float TurnAndRunDistanceToDestination; // wus
            // When following a unit, such as the player, this is the additional buffer outside of the task follow radius that we
            // are allowed to position ourselves before full firing position avoidance kicks in
            public float FollowUnitBufferDistance; // wus
            public float PhaseChance;
            // don't attempt again before given time since last phase
            public float PhaseDelaySeconds;
            // min distance from departure point where facing should be aligned with aim
            public float DepartureDistanceMin;
            // max distance from departure point where facing should be aligned with heading
            public float DepartureDistanceRange;
            // min distance from destination point where facing should be aligned with aim
            public float ArrivalDistanceMin;
            // max distance from destination point where facing should be aligned with heading
            public float ArrivalDistanceRange;
            // how far we will allow the facing to deviate from the preference.
            public Angle MaximumDeviationAngle;
            // Allows characters to be smooth throttle changes.
            public SmoothThrottleStruct SmoothThrottle;
            // Allows characters to slow down smoothly when stopping.
            public SmoothStoppingStruct SmoothStopping;
            
            [Flags]
            public enum MovementFlags : uint
            {
                DangerCrouchAllowMovement = 1 << 0,
                NoSideStep = 1 << 1,
                PreferToCombarNearFriends = 1 << 2,
                AllowBoostedJump = 1 << 3,
                Perch = 1 << 4,
                Climb = 1 << 5,
                PreferWallMovement = 1 << 6,
                HasFlyingMode = 1 << 7,
                DisallowCrouch = 1 << 8,
                DisallowAllMovement = 1 << 9,
                AlwaysUseSearchPoints = 1 << 10,
                KeepMoving = 1 << 11,
                CureIsolationJump = 1 << 12,
                GainElevation = 1 << 13,
                RepositionDistant = 1 << 14,
                OnlyUseAerialFiringPositions = 1 << 15,
                UseHighPriorityPathfinding = 1 << 16,
                LowerWeaponWhenNoAlertMovementOverride = 1 << 17,
                Phase = 1 << 18,
                NoOverrideWhenFiring = 1 << 19,
                NoStowDuringIdleActivities = 1 << 20,
                FlipAnyVehicle = 1 << 21
            }
            
            public enum ObstacleIgnoreEnum : short
            {
                None,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum GlobalAiJumpHeightEnum : short
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
            public enum MovementHintEnum : uint
            {
                VaultStep = 1 << 0,
                VaultCrouch = 1 << 1,
                Unused0 = 1 << 2,
                Unused1 = 1 << 3,
                Unused2 = 1 << 4,
                MountStep = 1 << 5,
                MountCrouch = 1 << 6,
                MountStand = 1 << 7,
                Unused3 = 1 << 8,
                Unused4 = 1 << 9,
                Unused5 = 1 << 10,
                HoistCrouch = 1 << 11,
                HoistStand = 1 << 12,
                Unused6 = 1 << 13,
                Unused7 = 1 << 14,
                Unused8 = 1 << 15
            }
            
            [TagStructure(Size = 0x8)]
            public class MovementStationaryPauseBlock : TagStructure
            {
                public Angle DirectionChangeAngle; // degrees
                public int StationaryChange; // ticks
            }
            
            [TagStructure(Size = 0x10)]
            public class MovementThrottleControlBlock : TagStructure
            {
                // When combat status is bigger or equal to this combat status, use the throttle settings below.
                public CombatStatusEnum CombatStatus;
                public MovementThrottleControlFlags Flags;
                public List<MovementThrottleBlock> ThrottleSettings;
                
                public enum CombatStatusEnum : short
                {
                    Asleep,
                    Idle,
                    Alert,
                    Active,
                    UninspectedOrphan,
                    DefiniteOrphan,
                    CertainOrphan,
                    VisibleEnemy,
                    ClearEnemyLos,
                    DangerousEnemy
                }
                
                [Flags]
                public enum MovementThrottleControlFlags : ushort
                {
                    ResampleDistanceEveryTick = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class MovementThrottleBlock : TagStructure
                {
                    // If AI needs to move at greater or equal to this distance, they will move at the given throttle
                    public float Distance; // wus
                    // Throttle scale between minimum and maximum throttle
                    public float ThrottleScale; // [0-1]
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class SmoothThrottleStruct : TagStructure
            {
                public CharacterSmoothMovementSettingsOptions SettingsOptions;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // interpolation weight used for the first frame of movement from a stopped position.
                public float StartingRate; // (0.0 to 1.0)
                // interpolation weight used for transitioning to a zero (stopped) throttle.
                public float StoppingRate; // (0.0 to 1.0)
                // maximum linear acceleration limit for throttle magnitude during regular movement.
                public float MaxLinearAcceleration; // throttle units per tick
                // maximum linear deceleration limit for throttle magnitude during regular movement.
                public float MaxLinearDeceleration; // throttle units per tick
                // maximum angular acceleration/deceleration limit for throttle changes.
                public Angle MaxAngularAcceleration; // degrees per tick
                // maximum linear Accel/Decel limit for throttle magnitude when reversing direction.
                public float MaxReversalLinearAcceleration; // throttle units per tick
                // maximum angular Accel/Decel limit for throttle heading when reversing direction.
                public Angle MaxReversalAngularAcceleration; // degrees per tick
                
                public enum CharacterSmoothMovementSettingsOptions : short
                {
                    UseEngineDefaultSettings,
                    UseOverrideSettingsBelow,
                    Disable
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class SmoothStoppingStruct : TagStructure
            {
                public CharacterSmoothMovementSettingsOptions SettingsOptions;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // Distance at which to begin slowing to a stop. Range provides variance.
                public Bounds<float> StoppingDistance; // [wu]
                // Throttle magnitude desired upon arrival. Should be non zero, and greater than 0.1 to avoid super-slow stops
                public Bounds<float> ArrivalThrottle; // range (0.05, 1.0)
                // Power value used to determine stopping curve. Values <1 produce sharper stops, >1 produce more ease-in.
                public Bounds<float> StoppingPower; // exponent
                // seconds to idle when stopped.
                public Bounds<float> IdleTime; // exponent
                
                public enum CharacterSmoothMovementSettingsOptions : short
                {
                    UseEngineDefaultSettings,
                    UseOverrideSettingsBelow,
                    Disable
                }
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class CharacterThrottleStyleBlock : TagStructure
        {
            public StringId StyleName;
            public float DesiredThrottle; // [0,1]
            public float AccelerationTime; // seconds
            // Defines throttle as a function of time from start of style application
            public ScalarFunctionNamedStruct AccelerationFunction;
            public float DecelerationDistance; // wu
            // Defines throttle as a function of distance from the goal
            public ScalarFunctionNamedStruct DecelerationFunction;
            public StringId Stance;
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterMovementSetBlock : TagStructure
        {
            public StringId Name;
            public List<MovementMappingBlock> Variants;
            
            [TagStructure(Size = 0x40)]
            public class MovementMappingBlock : TagStructure
            {
                public float Chance;
                public StringId Idle;
                public StringId Alert;
                public StringId Engage;
                public StringId SelfPreserve;
                public StringId Search;
                public StringId Retreat;
                public StringId Panic;
                public StringId Flank;
                public StringId Protected;
                public StringId Stunned;
                public StringId PostCombat;
                public StringId Custom1;
                public StringId Custom2;
                public StringId Custom3;
                public StringId Custom4;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class CharacterFlockingBlock : TagStructure
        {
            public float DecelerationDistance;
            public float NormalizedSpeed;
            public float BufferDistance;
            public Bounds<float> ThrottleThresholdBounds;
            public float DecelerationStopTime;
        }
        
        [TagStructure(Size = 0x38)]
        public class CharacterSwarmBlock : TagStructure
        {
            // After the given number of deaths, the swarm scatters
            public short ScatterKilledCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the distance from the target that the swarm scatters
            public float ScatterRadius;
            // amount of time to remain scattered
            public float ScatterTime;
            public float HoundMinDistance;
            public float HoundMaxDistance;
            // how long the infection form and its victim will wrestle before the point of no return
            public Bounds<float> InfectionTime; // secs
            // amount of randomness added to creature's throttle
            public float PerlinOffsetScale; // [0-1]
            // how fast the creature changes random offset to throttle
            public Bounds<float> OffsetPeriod; // s
            // a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
            public float PerlinIdleMovementThreshold; // [0-1]
            // a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
            public float PerlinCombatMovementThreshold; // [0-1]
            // how long we have to move (stuck distance) before we get deleted
            public float StuckTime;
            // how far we have to move in (stuck time) to not get deleted
            public float StuckDistance;
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterFiringPointEvaluatorBlockStruct : TagStructure
        {
            public EvaluationModes Mode;
            public List<EvaluatorDefinitionBlockStruct> Evaluators;
            
            public enum EvaluationModes : int
            {
                Fight,
                Panic,
                Cover,
                Uncover,
                Guard,
                Pursue,
                Avoid,
                VehicleCover,
                Postsearch,
                CoverFight,
                CoverEvasion
            }
            
            [TagStructure(Size = 0xC)]
            public class EvaluatorDefinitionBlockStruct : TagStructure
            {
                public EvaluatorEnum Evaluator;
                public float PreferenceWeight;
                public float AvoidanceWeight;
                
                public enum EvaluatorEnum : int
                {
                    Invalid,
                    AttackAvoidOverhead,
                    AttackDangerousenemy,
                    AttackIdealrange,
                    AttackLeaderDistance,
                    AttackSameFrameOfReference,
                    AttackWeaponrange,
                    CombatCue,
                    CombatmoveElevation,
                    CombatmoveFollowUnit,
                    CombatmoveLineoffire,
                    CombatmoveLineoffireOccluded,
                    CoverNearFriends,
                    CurrentPosition,
                    CurrentDestination,
                    CoverFight,
                    DangerZone,
                    DirectionalMovement,
                    Facing,
                    FireteamLeader,
                    FlagPreferences,
                    Flank,
                    FleeToLeader,
                    Friendly,
                    FriendBunkering,
                    GoalPointsOnly,
                    GoalPreferences,
                    Heatmap,
                    HideEquipment,
                    IdleWander,
                    InertiaPreservation,
                    Leadership,
                    MovementPlanning,
                    MoveIntoDangerZone,
                    MoveDistance,
                    MoveLocalized,
                    MultiTarget,
                    PackStalk,
                    PanicClosetotarget,
                    PanicTargetdistance,
                    Pathfinding,
                    PerchPreferences,
                    PostsearchPreferOriginal,
                    PreviouslyDiscarded,
                    PursuitSearchPattern,
                    RangedFight,
                    SquadPatrolSearch,
                    Stalking,
                    Stimulus,
                    TaskDirection,
                    ThreatAxisNearby,
                    ThreatAxisQuarter,
                    ThreatAxisSpring,
                    ThreatAxisSpringReject,
                    ThreatAxisThird,
                    Score,
                    TooFarFromLeader,
                    VehicleMoveDistance,
                    VehiclePickup,
                    WallLeanable,
                    _3dPathAvailable,
                    AttackVisible,
                    AttackVisibleLosOptional,
                    GuardCover,
                    HideCover,
                    UncoverVisible,
                    VehicleHideCover,
                    CoverFightPost,
                    Obstacle
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterReadyBlock : TagStructure
        {
            // Character will pause for given time before engaging threat
            public Bounds<float> ReadyTimeBounds;
        }
        
        [TagStructure(Size = 0x68)]
        public class CharacterEngageBlock : TagStructure
        {
            public EngageFlags Flags;
            // How long should I remain at a firing position before moving? (0 values will use the default values of 6 and 7
            // seconds)
            public Bounds<float> RepositionBounds; // s
            // When danger rises above the threshold, the actor crouches
            public float CrouchDangerThreshold;
            // When danger drops below this threshold, the actor can stand again.
            public float StandDangerThreshold;
            // When danger goes above given level, this actor switches firing positions
            public float FightDangerMoveThreshold;
            // Wait at least this many ticks before relocating due to danger
            public Bounds<short> FightDangerMoveThresholdCooldown; // ticks
            // Chance of flanking when fighting someone who isn't paying attention to me
            public float FightFlankChance; // [0-1]
            [TagField(ValidTags = new [] { "proj" })]
            // when I throw a grenade, forget what type I officially have
            public CachedTag OverrideGrenadeProjectile; // throw this type of projectile instead
            // Targets closer than this distance will not be attacked with a throw
            public float MinimumThrowDistance;
            // Targets beyond this distance will not be attacked with a throw
            public float MaximumThrowDistance;
            // How far does actor search for throwable items
            public float ThrowSearchRadius;
            // Angle (degrees) that the actor searches for throwable items (from his facing direction)
            public Angle ThrowSearchAngle;
            // Maximum throw force - it will not be used all the time
            public float MaximumThrowForce;
            // Vertical offset from target position on ground where throw is aimed
            public float ThrowTargetPointOffset;
            // How many seconds MUST pass until another throw is attempted
            public float ThrowDelayMin;
            // Up to how many seconds can elapse until another throw is attempted
            public float ThrowDelayMax;
            // If we are not holding a weapon, or we don't know how to use our weapon, use these bounds on my combat range
            public Bounds<float> DefaultCombatRange; // wus
            // If we don't know how to use our weapon, use these bounds on my firing range
            public Bounds<float> DefaultFiringRange; // wus
            // 0 will default to .3, other is ratio from min to max combat range preferred
            public float PreferredEngageFraction;
            // Number of seconds elapsed before stop firing at active shielded target.
            public float ActiveShieldFireCutoffDelay;
            
            [Flags]
            public enum EngageFlags : uint
            {
                EngagePerch = 1 << 0,
                FightConstantMovement = 1 << 1,
                FlightFightConstantMovement = 1 << 2,
                DisallowCombatCrouching = 1 << 3,
                DisallowCrouchShooting = 1 << 4,
                FightStable = 1 << 5,
                ThrowShouldLob = 1 << 6,
                AllowPositioningBeyondIdealRange = 1 << 7,
                CanSuppress = 1 << 8
            }
        }
        
        [TagStructure(Size = 0xB8)]
        public class CharacterChargeBlock : TagStructure
        {
            public ChargeFlags ChargeFlags1;
            public float MeleeConsiderRange;
            // chance of initiating a melee within a 1 second period
            public float MeleeChance;
            public float MeleeAttackRange;
            public float MeleeAbortRange;
            // Give up after given amount of time spent charging
            public float MeleeAttackTimeout; // seconds
            // don't attempt again before given time since last melee
            public float MeleeAttackDelayTimer; // seconds
            // don't attempt a melee on a recently armor locked target for this many seconds
            public float MeleeArmorLockDelay; // seconds
            public Bounds<float> MeleeLeapRange;
            public float MeleeLeapChance;
            public float IdealLeapVelocity;
            public float MaxLeapVelocity;
            // min ballistic fraction
            public float MeleeLeapBallistic;
            // time between melee leaps
            public float MeleeDelayTimer; // seconds
            // how far ahead (seconds) do we look at target for translational prediction?
            public float MeleeLeapPredictionTime;
            // chance for a leader to berserk when all his followers die (actually charge, NOT berserk, but I'm not changing the
            // name of the variable)
            public float LeaderAbandonedBerserkChance;
            // lower bound is chance to berserk at max range, upper bound is chance to berserk at min range, requires shield
            // depleted berserk impulse
            public Bounds<float> ShieldDownBerserkChance;
            public Bounds<float> ShieldDownBerserkRanges;
            // The max range at which we will go berserk if we see a friendly AI of the same type (brute, etc) get killed
            public float FriendlyKilledMaxBerserkDistance; // wu
            // Chance that we will go berserk if we see a friendly AI of the same type (brute, etc) with the same or lower
            // standing get killed
            public float PeerKilledBerserkChance; // [0,1]
            // Chance that we will go berserk if we see a friendly AI of the same type (brute, etc) with higher standing get
            // killed
            public float LeaderKilledBerserkChance; // [0,1]
            [TagField(ValidTags = new [] { "weap" })]
            // when I berserk, I pull out a ...
            public CachedTag BerserkWeapon;
            // Chance that AI will play berserk anim after getting stuck with a grenade.  Zero is 50%
            public float PlayBerserkAnimChanceWhenStuck;
            // Time that I will stay in beserk after losing my target, and then revert back to normal
            public float BeserkCooldown; // seconds
            // If our target is closer than this distance, and not (in a vehicle/larger size than us/using a melee weapon), we
            // will berserk. If our target is further than this distance, we will stop berserking. Set to 0 to disable proximity
            // berserking.
            public float ProximityBerserkRange; // world units
            // We will never go more than this far outside our firing point areas when proximity-berserking. 0= no limit.
            public float ProximityBerserkOutsideFpRange; // world units
            // If we have a target close enough to berserk, this is the chance that we will do so. If chance fails, we will try
            // again after timeout.
            public float ProximityBerserkChance; // [0,1]
            // We will not proximity-berserk unless it has been at least this long since we last stopped berserking. 0= no
            // timeout.
            public float ProximityBerserkTimeout; // seconds
            // Probability that I will run the kamikaze behaviour when my leader dies.
            public float BrokenKamikazeChance;
            // How far we will melee charge outside our firing points before starting perimeter (defaults to 5wu)
            public float PerimeterRange;
            // How far we will melee charge outside our firing points before starting perimeter when the target is close to me
            // (within 3wu) (defaults to 9wu)
            public float PerimeterRangeClose;
            // How long will we take damage from our target before either seeking cover or berserking (defaults to 3secs)
            public float PerimeterDamageTimeout; // secs
            public List<CharacterChargeDifficultyLimitsBlock> DifficultyLimits;
            public Bounds<float> BallingMeleeLeapRange;
            public float BallingMeleeLeapAttackRange;
            public float BallingMeleeLeapChance;
            // don't attempt again before given time since last melee
            public float BallingMeleeAttackDelayTimer; // seconds
            
            [Flags]
            public enum ChargeFlags : uint
            {
                OffhandMeleeAllowed = 1 << 0,
                BerserkWheneverCharge = 1 << 1,
                DonTUseBerserkMode = 1 << 2,
                DonTStowWeaponDuringBerserk = 1 << 3,
                AllowDialogueWhileBerserking = 1 << 4,
                DonTPlayBerserkAnimation = 1 << 5,
                DonTShootDuringCharge = 1 << 6,
                AllowLeapWithRangedWeapons = 1 << 7
            }
            
            [TagStructure(Size = 0x6)]
            public class CharacterChargeDifficultyLimitsBlock : TagStructure
            {
                // How many guys in a single clump can be kamikazing at one time
                public short MaxKamikazeCount;
                // How many guys in a single clump can be berserking at one time
                public short MaxBerserkCount;
                // We'd like at least this number of guys in a single clump can be berserking at one time (primarily combat forms)
                public short MinBerserkCount;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterEvasionBlock : TagStructure
        {
            // Consider evading when immediate danger surpasses threshold
            public float EvasionDangerThreshold;
            // Wait at least this delay between evasions
            public float EvasionDelayTimer;
            // If danger is above threshold, the chance that we will evade. Expressed as chance of evading within a 1 second time
            // period
            public float EvasionChance;
            // If target is within given proximity, possibly evade
            public float EvasionProximityThreshold;
            // Chance of retreating (fleeing) after danger avoidance dive
            public float DiveRetreatChance;
        }
        
        [TagStructure(Size = 0x58)]
        public class CharacterCoverBlock : TagStructure
        {
            public CoverFlags CoverFlags1;
            // how long we stay behind cover after seeking cover
            public Bounds<float> HideBehindCoverTime; // seconds
            // how long we wait in cover before using the hologram
            public Bounds<float> HologramCoverWaitTime; // seconds
            // Amount of time I will wait before trying to use hologram equipment again (0 value defaults to 5 seconds)
            public float HologramCooldownDelay; // seconds
            // Only cover when shield falls below this level
            public float CoverShieldFraction;
            // Only cover when vitality falls below this level
            public float CoverVitalityThreshold;
            // Danger must be this high to cover.
            public float CoverDangerThreshold;
            // How far from the target should we switch from aggresive to defensive covering (0 always defensive, big number
            // always offensive)
            public float MinimumDefensiveDistanceFromTarget; // wus
            // If our cover point is less than this distance, we will never consider defensive covering
            public float MinimumDefensiveDistanceFromCover; // wus
            // If the target has scarines bigger or equal to this, we will always cover defensively
            public float AlwaysDefensiveScaryThreshold;
            // Amount of time I will wait before trying again after covering (0 value defaults to 2 seconds)
            public float CoverCheckDelay; // seconds
            // Amount of time I will wait before issuing a pinned down message (0 value defaults to 2 seconds)
            public float CoverPinnedDownCheckDelay; // seconds
            // Emerge from cover when shield fraction reaches threshold
            public float EmergeFromCoverWhenShieldFractionReachesThreshold;
            // Triggers self-preservation when target within this distance (assuming proximity_self_preserve_impulse is enabled)
            public float ProximitySelfPreserve; // wus
            // When self preserving from a target less than given distance, causes melee attack (assuming proximity_melee_impulse
            // is enabled)
            public float ProximityMeleeDistance;
            // When danger from an unreachable enemy surpasses threshold, actor cover (assuming unreachable_enemy_cover impulse is
            // enabled)
            public float UnreachableEnemyDangerThreshold;
            // When target is unassailable, and danger goes over this value - cover (assuming unassailable_enemy_cover impulse is
            // enabled)
            public float UnassailableEnemyDangerThreshold;
            // When target is aware of me and surpasses the given scariness, self-preserve (assuming scary_target_cover_impulse is
            // enabled)
            public float ScaryTargetThreshold;
            // Fraction of vitality below which an equipped shield equipment (instant cover/bubbleshield) will be activated (once
            // damage has died down, and assuming shield_equipment_impulse is enabled)
            public float VitalityFractionShieldEquipment;
            // Must have less than this amount of recent body damage before we can deploy our equipped shield equipment.
            public float RecentDamageShieldEquipment;
            
            [Flags]
            public enum CoverFlags : uint
            {
                UnassailableCoverEndsOnlyWhenTargetAssailable = 1 << 0,
                UsePhasing = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class CharacterRetreatBlock : TagStructure
        {
            public CharacterRetreatFlags RetreatFlags;
            // When shield vitality drops below given amount, retreat is triggered by low_shield_retreat_impulse
            public float ShieldThreshold;
            // When confronting an enemy of over the given scariness, retreat is triggered by scary_target_retreat_impulse
            public float ScaryTargetThreshold;
            // When perceived danger rises above the given threshold, retreat is triggered by danger_retreat_impulse
            public float DangerThreshold;
            // When enemy closer than given threshold, retreat is triggered by proximity_retreat_impulse
            public float ProximityThreshold;
            // actor cowers for at least the given amount of time
            public Bounds<float> MinMaxForcedCowerTimeBounds;
            // actor times out of cower after the given amount of time
            public Bounds<float> MinMaxCowerTimeoutBounds;
            // If target reaches is within the given proximity, an ambush is triggered by the proximity ambush impulse
            public float ProximityAmbushThreshold;
            // If target is less than threshold (0-1) aware of me, an ambush is triggered by the vulnerable enemy ambush impulse
            public float AwarenessAmbushThreshold;
            // If leader-dead-retreat-impulse is active, gives the chance that we will flee when our leader dies within 4 world
            // units of us
            public float LeaderDeadRetreatChance;
            // If peer-dead-retreat-impulse is active, gives the chance that we will flee when one of our peers (friend of the
            // same race) dies within 4 world units of us
            public float PeerDeadRetreatChance;
            // If peer-dead-retreat-impulse is active, gives the chance that we will flee when a second peer (friend of the same
            // race) dies within 4 world units of us
            public float SecondPeerDeadRetreatChance;
            // Flee for no longer than this time (if there is no cover, then we will keep fleeing indefinitely). Value of 0 means
            // 'no timeout'
            public float FleeTimeout; // seconds
            // The angle from the intended destination direction that a zig-zag will cause
            public Angle ZigZagAngle; // degrees
            // How long it takes to zig left and then zag right.
            public float ZigZagPeriod; // seconds
            // The likelihood of throwing down a grenade to cover our retreat
            public float RetreatGrenadeChance;
            [TagField(ValidTags = new [] { "weap" })]
            // If I want to flee and I don't have flee animations with my current weapon, throw it away and try a ...
            public CachedTag BackupWeapon;
            
            [Flags]
            public enum CharacterRetreatFlags : uint
            {
                ZigZagWhenFleeing = 1 << 0,
                Unused1 = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class CharacterSearchBlock : TagStructure
        {
            public CharacterSearchFlags SearchFlags;
            public Bounds<float> SearchTime;
            // Maximum distance away from our firing positions that we are happy to search (0 value will default to 3wu). Does not
            // affect vehicle search distance (see maxd if you want that value too).
            public float SearchDistance;
            // Distance of uncover point from target. Hard lower limit, soft upper limit.
            public Bounds<float> UncoverDistanceBounds;
            // (0 value will default to 1.8wu)
            public float OrphanOffset; // wu
            // Minimum offset from the target point to investigate, otherwise we just use the target point itself. Not entirely
            // sure about the justification for this one...
            public float MinimumOffset; // wu
            public Bounds<float> VocalizationTime;
            // The number of seconds that must elapse before an actor will consider a search-performance again
            public float PerformanceCoolDownTime;
            
            [Flags]
            public enum CharacterSearchFlags : uint
            {
                CrouchOnInvestigate = 1 << 0,
                WalkOnPursuit = 1 << 1,
                SearchForever = 1 << 2,
                SearchExclusively = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterPresearchBlock : TagStructure
        {
            public GenericFlags PreSearchFlags;
            // Presearch turns off after the given time
            public Bounds<float> MaxPresearchTime; // seconds
            // Suppress turns off after the given time (0 defaults to 8 seconds)
            public float MaxSuppressTime; // seconds
            public float SuppressingFireWeight;
            public float UncoverWeight;
            public float LeapOnCoverWeight;
            public float DestroyCoverWeight;
            public float GuardWeight;
            public float InvestigateWeight;
            
            [Flags]
            public enum GenericFlags : uint
            {
                Flag1 = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterIdleBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // time range for delays between idle poses
            public Bounds<float> IdlePoseDelayTime; // seconds
            // time to pause at a point while wandering
            public Bounds<float> WanderDelayTime; // seconds
        }
        
        [TagStructure(Size = 0xC)]
        public class CharacterVocalizationBlock : TagStructure
        {
            public float CharacterSkipFraction; // [0,1]
            // How long does the player look at an AI before the AI responds?
            public float LookCommentTime; // s
            // How long does the player look at the AI before he responds with his 'long look' comment?
            public float LookLongCommentTime; // s
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterBoardingBlock : TagStructure
        {
            public BoardingFlags Flags;
            // maximum distance from entry point that we will consider boarding
            public float MaxDistance; // wus
            // give up trying to get in boarding seat if entry point is further away than this
            public float AbortDistance; // wus
            // maximum speed at which we will consider boarding
            public float MaxSpeed; // wu/s
            // maximum time we will melee board for
            public float BoardTime; // seconds
            // The amount of time after boarding before we'll consider boarding again
            public Bounds<float> BoardingTimeout; // seconds
            public List<CharacterVehicleBoardingBlock> VehicleSpecificProperties;
            
            [Flags]
            public enum BoardingFlags : uint
            {
                AirborneBoarding = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class CharacterVehicleBoardingBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "unit" })]
                public CachedTag Vehicle;
                public VehicleBoardingFlags Flags;
                
                [Flags]
                public enum VehicleBoardingFlags : uint
                {
                    BoardingDoesNotEnterSeat = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterKungfuBlock : TagStructure
        {
            // If the player is within this distance, open fire, even if your task is kungfu-fight disallowed
            public float KungfuOverrideDistance; // wus
            // If you are kungfu disallowed and your danger is above this level, take cover
            public float KungfuCoverDangerThreshold;
        }
        
        [TagStructure(Size = 0x60)]
        public class CharacterBunkerBlock : TagStructure
        {
            public CharacterBunkerTimingsStruct Default;
            public CharacterBunkerTimingsStruct Fight;
            public CharacterBunkerTimingsStruct Cover;
            public CharacterBunkerTimingsStruct Guard;
            
            [TagStructure(Size = 0x18)]
            public class CharacterBunkerTimingsStruct : TagStructure
            {
                // How long we should open for
                public Bounds<float> OpenTime; // seconds
                // How long we must stay closed for before opening or peeking again
                public float ClosedMinTime; // seconds
                // Force close at this danger level
                public float CloseDangerLevel;
                // What chance we have of opening per second
                public float OpenChance; // chance per second
                // What chance we have of peeking per second
                public float PeekChance; // chance per second
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class CharacterGuardianBlock : TagStructure
        {
            // length of time for which the guardian surges
            public float SurgeTime; // seconds
            // minimum enforced delay between surges
            public float SurgeDelayTime; // seconds
            // surge when our target gets closer than this to me (0 value defaults to 2wu)
            public float ProximitySurgeDistance; // wu
            // length of time it takes the guardian to get to its phase destination
            public float PhaseTime; // seconds
            // Minimum distance that I will consider phasing
            public float CurrentPositionDistance; // wu
            // Minimum distance from my target that I will phase to
            public float TargetPositionDistance; // wu
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterCombatformBlock : TagStructure
        {
            // distance at which combatform will be forced into berserk
            public float BerserkDistance; // wu
            // chance of berserking this second
            public float BerserkChance;
        }
        
        [TagStructure(Size = 0x38)]
        public class CharacterEngineerBlock : TagStructure
        {
            // try and rise this amount before dying
            public float DeathHeight; // wu
            // spend this time rising
            public float DeathRiseTime; // seconds
            // spend this time detonating
            public float DeathDetonationTime; // seconds
            // Boost the shields of allies within this radius during combat
            public float ShieldBoostRadiusMax;
            // Time between shield boost pings from the engineer
            public float ShieldBoostPeriod; // seconds
            // The name of the damage section which will be activated by the engineer shield boost
            public StringId ShieldBoostDamageSectionName;
            public float DetonationShieldThreshold;
            public float DetonationBodyVitality;
            // if target enters within this radius, either detonate or deploy equipment
            public float ProximityRadius; // wus
            // chance of detonating if target enters the drain radius radius
            public float ProximityDetonationChance;
            [TagField(ValidTags = new [] { "eqip" })]
            // if target enters radius and detonation is not chosen, deploy this equipment.
            public CachedTag ProximityEquipment;
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterInspectBlock : TagStructure
        {
            // distance from object at which to stop and turn on the inspection light
            public float StopDistance; // wu
            // time which we should inspect each object for
            public Bounds<float> InspectTime; // seconds
            // range in which we should search for objects to inspect
            public Bounds<float> SearchRange; // wu
        }
        
        [TagStructure(Size = 0x18)]
        public class CharacterScarabBlock : TagStructure
        {
            // When target within this distance, the scarab will back up
            public float FightingMinDistance; // wus
            // When target outside this distance, the scarab will chase
            public float FightingMaxDistance; // wus
            // When within these bounds distance from the target, we blend in our anticipated facing vector
            public Bounds<float> AnticipatedAimRadius; // wus
            // When moving forward within this dot of our desired facing, just move forward
            public float SnapForwardAngle; // [0-1]
            // When moving forward within this dot of our desired facing, just move forward
            public float SnapForwardAngleMax; // [0-1]
        }
        
        [TagStructure(Size = 0xCC)]
        public class CharacterWeaponsBlock : TagStructure
        {
            public WeaponFlags WeaponsFlags;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            // we can only fire our weapon at targets within this distance
            public float MaximumFiringRange; // world units
            // weapon will not be fired at target closer than given distance
            public float MinimumFiringRange;
            public Bounds<float> NormalCombatRange; // world units
            // we offset our burst targets randomly by this range when firing at non-visible enemies (zero = never)
            public float BombardmentRange;
            // Specific target regions on a vehicle or unit will be fired upon only under the given distance
            public float MaxSpecialTargetDistance; // world units
            // within this distance actor will be able to do fallback engagement firing patterns. Use for extreme range
            // engagements not otherwise permitted.
            public float MaxExtremeTargetDistance; // world units
            // we try to aim our shots super-ballistically if target is outside this range (zero = never)
            public float SuperBallisticRange;
            // At the min range, the min ballistic fraction is used, at the max, the max ballistic fraction is used
            public Bounds<float> BallisticFiringBounds; // world units
            // Controls speed and degree of arc. 0 = high, slow, 1 = low, fast
            public Bounds<float> BallisticFractionBounds; // [0-1]
            public Bounds<float> FirstBurstDelayTime; // seconds
            public float SurpriseDelayTime; // seconds
            public float SurpriseFireWildlyTime; // seconds
            public float DeathFireWildlyChance; // [0,1]
            public float DeathFireWildlyTime; // seconds
            // custom standing gun offset for overriding the default in the base actor
            public RealVector3d CustomStandGunOffset;
            // custom crouching gun offset for overriding the default in the base actor
            public RealVector3d CustomCrouchGunOffset;
            // Number of projectiles blocked before the character is considered blocked. Zero defaults to 6.
            public int BlockedShotCount;
            // the type of special weapon fire that we can use
            public ActorSpecialFireModeEnum SpecialFireMode;
            // when we will decide to use our special weapon fire mode
            public ActorSpecialFireSituationEnum SpecialFireSituation;
            // how likely we are to use our special weapon fire mode
            public float SpecialFireChance; // [0,1]
            // how long we must wait between uses of our special weapon fire mode
            public float SpecialFireDelay; // seconds
            // damage modifier for special weapon fire (applied in addition to the normal damage modifier. zero = no change)
            public float SpecialDamageModifier; // [0,1]
            // projectile error angle for special weapon fire (applied in addition to the normal error)
            public Angle SpecialProjectileError; // degrees
            // amount of ammo loaded into the weapon that we drop (in fractions of a clip, e.g. 0.3 to 0.5)
            public Bounds<float> DropWeaponLoaded;
            // total number of rounds in the weapon that we drop (ignored for energy weapons)
            public Bounds<short> DropWeaponAmmo;
            // Indicates starting and ending accuracies at normal difficulty
            public Bounds<float> NormalAccuracyBounds;
            // The amount of time it takes the accuracy to go from starting to ending
            public float NormalAccuracyTime;
            // Indicates starting and ending accuracies at heroic difficulty
            public Bounds<float> HeroicAccuracyBounds;
            // The amount of time it takes the accuracy to go from starting to ending
            public float HeroicAccuracyTime;
            // Indicates starting and ending accuracies at legendary difficulty
            public Bounds<float> LegendaryAccuracyBounds;
            // The amount of time it takes the accuracy to go from starting to ending
            public float LegendaryAccuracyTime;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag WeaponMeleeDamage;
            
            [Flags]
            public enum WeaponFlags : uint
            {
                BurstingInhibitsMovement = 1 << 0,
                MustCrouchToShoot = 1 << 1,
                UseExtendedSafeToSaveRange = 1 << 2,
                FixedAimingVector = 1 << 3,
                AimAtFeet = 1 << 4,
                // use only for weapons with really, really long barrels (bfg), do NOT use for rotating turret weapons (warthog,
                // falcon, etc)
                ForceAimFromBarrelPosition = 1 << 5
            }
            
            public enum ActorSpecialFireModeEnum : short
            {
                None,
                Overcharge,
                SecondaryTrigger
            }
            
            public enum ActorSpecialFireSituationEnum : short
            {
                Never,
                EnemyVisible,
                EnemyOutOfSight,
                Strafing
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class CharacterFiringPatternPropertiesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            public List<CharacterFiringPatternBlock> FiringPatterns;
            
            [TagStructure(Size = 0x40)]
            public class CharacterFiringPatternBlock : TagStructure
            {
                // how many times per second we pull the trigger (zero = continuously held down)
                public float RateOfFire;
                // how well our bursts track moving targets. 0.0= fire at the position they were standing when we started the burst.
                // 1.0= fire at current position
                public float TargetTracking; // [0,1]
                // how much we lead moving targets. 0.0= no prediction. 1.0= predict completely.
                public float TargetLeading; // [0,1]
                // how far away from the target the starting point is
                public float BurstOriginRadius; // world units
                // the range from the horizontal that our starting error can be
                public Angle BurstOriginAngle; // degrees
                // how far the burst point moves back towards the target (could be negative)
                public Bounds<float> BurstReturnLength; // world units
                // the range from the horizontal that the return direction can be
                public Angle BurstReturnAngle; // degrees
                // how long each burst we fire is
                public Bounds<float> BurstDuration; // seconds
                // how long we wait between bursts
                public Bounds<float> BurstSeparation; // seconds
                // what fraction of its normal damage our weapon inflicts (zero = no modifier)
                public float WeaponDamageModifier;
                // error added to every projectile we fire
                public Angle ProjectileError; // degrees
                // the maximum rate at which we can sweep our fire (zero = unlimited)
                public Angle BurstAngularVelocity; // degrees per second
                // cap on the maximum angle by which we will miss target (restriction on burst origin radius
                public Angle MaximumErrorAngle; // degrees
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class CharacterGrenadesBlock : TagStructure
        {
            public GrenadesFlags GrenadesFlags1;
            // type of grenades that we throw^
            public GlobalGrenadeTypeEnum GrenadeType;
            // how we throw our grenades
            public ActorGrenadeTrajectoryEnum TrajectoryType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // how many enemies must be within the radius of the grenade before we will consider throwing there
            public short MinimumEnemyCount;
            // we consider enemies within this radius when determining where to throw
            public float EnemyRadius; // world units
            // how fast we LIKE to throw our grenades
            public float GrenadeIdealVelocity; // world units per second
            // the fastest we can possibly throw our grenades
            public float GrenadeVelocity; // world units per second
            // ranges within which we will consider throwing a grenade
            public Bounds<float> GrenadeRanges; // world units
            // we won't throw if there are friendlies around our target within this range
            public float CollateralDamageRadius; // world units
            // how likely we are to throw a grenade in one second
            public float GrenadeChance; // [0,1]
            // Throw chance multiplied by this value when target is using active shield.
            public float ActiveShieldModifier;
            // How long we have to wait after throwing a grenade before we can throw another one
            public float GrenadeThrowDelay; // seconds
            // how likely we are to throw a grenade to flush out a target in one second
            public float GrenadeUncoverChance; // [0,1]
            // how likely we are to throw a grenade against a vehicle
            public float AntiVehicleGrenadeChance; // [0,1]
            // number of grenades that we start with
            public Bounds<short> GrenadeCount;
            // how likely we are not to drop any grenades when we die, even if we still have some
            public float DontDropGrenadesChance; // [0,1]
            
            [Flags]
            public enum GrenadesFlags : uint
            {
                DoNotThrowWhileBunkering = 1 << 0,
                AllowWhileBerserking = 1 << 1
            }
            
            public enum GlobalGrenadeTypeEnum : short
            {
                HumanFragmentation,
                CovenantPlasma,
                PulseGrenade,
                GrenadeType3,
                GrenadeType4,
                GrenadeType5,
                GrenadeType6,
                GrenadeType7
            }
            
            public enum ActorGrenadeTrajectoryEnum : short
            {
                Toss,
                Lob,
                Bounce
            }
        }
        
        [TagStructure(Size = 0x14C)]
        public class CharacterVehicleBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            [TagField(ValidTags = new [] { "styl" })]
            public CachedTag Style;
            // How much in the future should we estimate a collision (affects collision ray length)
            public float LookaheadTime; // seconds
            // How fast to control the roll avoidance
            public float RollChangeMagnitude; // degrees
            // How fast roll goes back to 0- 1 means never, 0 means instantaneously
            public float RollDecayMultiplier; // (0,1)
            // How long after a collision should the vehicle keep moving at max throttle
            public float ThrottleGracePeriod; // seconds
            // Minimum throttle that the avoidance can slow down to
            public float MinimumThrottle; // (-1,1)
            // The maximum distance penalty applied to an avoidance volume search path if we're facing away from the path. 1000 wu
            // good for space, 5 wu good for ground.
            public float AvoidanceVolumeTurnPenaltyDistance; // wus
            public VehicleFlags VehicleFlags1;
            // Distance defines a box outside of which we clamp perturbation. Value of zero causes fallback to run.
            public float HoverDecelerationDistance; // wus
            // The max radius of the XZ anchor perturbation. Value of zero causes fallback to run.
            public float HoverOffsetDistance; // wus
            // The speed the vehicle must be below for it to start running hover perturbation (default=0.4).
            public float HoverAllowPerturbationSpeed; // wu/s
            // The number of seconds for the x component of the anchor perturbation to repeat itself (default=10).
            public float HoverRandomXAxisPeriod; // sec
            // The number of seconds for the y component of the anchor perturbation to repeat itself (default=7).
            public float HoverRandomYAxisPeriod; // sec
            // The number of seconds for the z component of the anchor perturbation to repeat itself (default=5).
            public float HoverRandomZAxisPeriod; // sec
            // The radius of the anchor perturbation. (default=0)
            public float HoverRandomRadius; // wu
            // If we're traveling faster than this amount toward the anchor inside the max throttle distance, we drop throttle to
            // the min. (default=0.2)
            public float HoverAnchorApproachSpeedLimit; // wu/s
            // The distance from the anchor at which the min and max throttle scale occur. (default=[0.25, 2.0])
            public Bounds<float> HoverAnchorThrottleScaleDistance; // wu
            // The xy-throttle scale at the min and max throttle scale distances. (default=[0.0, 0.2])
            public Bounds<float> HoverAnchorXyThrottleScale; // [0,1]
            // The z-throttle scale at the min and max throttle scale distances. (default=[1.0, 1.0])
            public Bounds<float> HoverAnchorZThrottleScale; // [0,1]
            // The minimum the z component of throttle is allowed to be (default=0.2)
            public float HoverThrottleMinZ; // [-1, 1]
            public float AiPathfindingRadius; // world units (Ground vehicles)
            // If 0, uses pathfinding radius.
            public float AiAvoidanceRadius; // world units (Ground vehicles)
            // Distance within which goal is considered reached
            public float AiDestinationRadius; // world units (All vehicles)
            // Distance from goal at which AI starts to decelerate
            public float AiDecelerationDistance; // world units (All vehicles)
            public float RoughlyTheTimeItWouldTakeThisVehicleToStopDefaultIs2Seconds;
            // Idealized average turning radius (should reflect actual vehicle physics)
            public float AiTurningRadius; // world units (Warthog, Pelican, Ghost)
            // Idealized minimum turning radius (should reflect actual vehicle physics)
            public float AiInnerTurningRadius; // (Warthogs)
            // Ideal turning radius for rounding turns (barring obstacles, etc.)
            public float AiIdealTurningRadius; // (Warthogs, ghosts)
            public Angle AiBansheeSteeringMaximum; // (banshees,avoidance steering phantoms)
            // Maximum steering angle from forward (ultimately controls turning speed)
            public float AiMaxSteeringAngle; // degrees (warthogs, ghosts, wraiths)
            // Maximum delta in steering angle from one tick to the next (ultimately controls turn acceleration)
            public float AiMaxSteeringDelta; // degrees (warthogs, dropships, ghosts, wraiths)
            public float AiOversteeringScale; // (warthogs, ghosts, wraiths)
            // Angle to goal at which AI will oversteer
            public Bounds<Angle> AiOversteeringBounds; // (banshees)
            // Distance within which Ai will strafe to target (as opposed to turning)
            public float AiSideslipDistance; // (ghosts, dropships)
            // Look-ahead distance for obstacle avoidance
            public float AiAvoidanceDistance; // world units
            // The minimum urgency with which a turn can be made (urgency = percent of maximum steering delta)
            public float AiMinUrgency; // [0-1] (warthogs, banshees)
            // The angle from facing that is considered to be behind us (we do the ugly floaty slidey turn to things behind us)
            public Angle DestinationBehindAngle; // (dropships)
            // When approaching a corner at speed, we may want to skid around that corner, by turning slightly too early. This is
            // (roughly) how many seconds ahead we should start turning.
            public float SkidScale; // (warthogs)
            public Angle AimingVelocityMaximum; // degrees per second
            public Angle AimingAccelerationMaximum; // degrees per second squared
            public float AiThrottleMaximum; // (0 - 1) (all vehicles)
            // If zero, default to ai throttle maximum
            public float AiReverseThrottleMaximum; // (0 - 1) (ground only)
            // scale on throttle when within 'ai deceleration distance' of goal (0...1)
            public float AiGoalMinThrottleScale; // (warthogs, dropships, ghosts)
            // Scale on throttle due to nearness to a turn (0...1)
            public float AiTurnMinThrottleScale; // (warthogs, dropships, ghosts)
            // Scale on throttle due to facing away from intended direction (0...1)
            public float AiDirectionMinThrottleScale; // (warthogs, dropships, ghosts)
            // Scale on throttle due to skidding (0...1)
            public float AiSkidMinThrottleScale; // (warthogs, dropships, ghosts)
            // Maximise min throttle at this deviation of angles from current movement
            public Angle SkidAttentuationMaxAngle;
            // The maximum allowable change in throttle between ticks
            public float AiAccelerationScale; // (0-1)
            // The degree of throttle blending between one tick and the next (0 = no blending)
            public float AiThrottleBlend; // (0-1)
            // About how fast I can go.
            public float TheoreticalMaxSpeed; // wu/s (warthogs, dropships, ghosts)
            // scale on the difference between desired and actual speed, applied to throttle
            public float ErrorScale; // (warthogs, dropships)
            public Angle AiAllowableAimDeviationAngle;
            // The distance at which the tight angle criterion is used for deciding to vehicle charge
            public float AiChargeTightAngleDistance; // (all vehicles)
            // Angle cosine within which the target must be when target is closer than tight angle distance in order to charge
            public float AiChargeTightAngle; // [0-1] (all vehicles)
            // Time delay between vehicle charges
            public float AiChargeRepeatTimeout; // (all vehicles)
            // In deciding when to abort vehicle charge, look ahead these many seconds to predict time of contact
            public float AiChargeLookAheadTime; // (all vehicles)
            // Consider charging the target when it is within this range (0 = infinite distance)
            public float AiChargeConsiderDistance; // (all vehicles)
            // Abort the charge when the target get more than this far away (0 = never abort)
            public float AiChargeAbortDistance; // (all vehicles)
            // Abort the charge when the target gets closer than this far away (0 = 3 times destination radius for historical
            // purposes.)
            public float AiChargeAbortCloseDistance; // (all vehicles)
            // Probability that we decide to charge a target even if they are armor locked
            public float AiChargeArmorLockedTargetChance; // [0-1] (all vehicles)
            // The ram behavior stops after a maximum of the given number of seconds
            public float VehicleRamTimeout;
            // The ram behavior freezes the vehicle for a given number of seconds after performing the ram
            public float RamParalysisTime;
            // Trigger a cover when recent damage is above given threshold (damage_vehicle_cover impulse)
            public float AiCoverDamageThreshold; // (all vehicles)
            // Trigger a cover when recent shied damage is above given threshold (flying_cover behavior)
            public float AiCoverShieldDamageThreshold; // (all vehicles)
            // When executing vehicle-cover, minimum distance from the target to flee to
            public float AiCoverMinDistance; // (all vehicles)
            // How long to stay away from the target
            public float AiCoverTime; // (all vehicles)
            // Boosting allowed when distance to cover destination is greater then this.
            public float AiCoverMinBoostDistance; // (all vehicles)
            // If vehicle turtling behavior is enabled, turtling is initiated if 'recent damage' surpasses the given threshold
            public float TurtlingRecentDamageThreshold; // %
            // If the vehicle turtling behavior is enabled, turtling occurs for at least the given time
            public float TurtlingMinTime; // seconds
            // The turtled state times out after the given number of seconds
            public float TurtlingTimeout; // seconds
            public ObstacleIgnoreEnum ObstacleIgnoreSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // max number of this type of vehicle in a task who can vehicle charge at once
            public short MaxVehicleCharge;
            // min number of this type of vehicle in a task who can vehicle charge at once (soft limit, just a desired number)
            public short MinVehicleCharge;
            
            [Flags]
            public enum VehicleFlags : uint
            {
                PassengersAdoptOriginalSquad = 1 << 0,
                SnapFacingToForward = 1 << 1,
                ThrottleToTarget = 1 << 2,
                StationaryFight = 1 << 3,
                KeepMoving = 1 << 4,
                CanPathfindWithAvoidanceOnly = 1 << 5,
                UseVolumeAvoidance = 1 << 6,
                TargetEquality = 1 << 7,
                DonTFaceTarget = 1 << 8,
                OverrideAimingLimits = 1 << 9
            }
            
            public enum ObstacleIgnoreEnum : short
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
        
        [TagStructure(Size = 0xE0)]
        public class CharacterFlyingMovementBlockStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Vehicle;
            // How much our current movement vector affects our new vector selection.
            public float Facing;
            // How much random deviation is applied to our vector selection.
            public float Perturbation;
            // How strongly we avoid our containing volume.
            public float VolumeAvoidance;
            // How strongly we are randomly perturbed by our containing volume.
            public float VolumePerturbation;
            // How strongly are we attracted toward cover points.
            public float VolumeCover;
            // How much of a tendancy to flock we have.
            public float Flocking;
            // The weight of our movement towards our current target.
            public float Target;
            // The weight of our movement intended to keep our target in our tail.
            public float TargetTail;
            // The time bounds on how long we should stay in an area before moving to another area.
            public Bounds<float> AreaReselectTime; // seconds
            // How long before reselecting a destination while idling.
            public Bounds<float> IdleTime; // seconds
            // How long after being exposed in cover before we reselect new cover.
            public Bounds<float> UnsafeCoverReselectTime; // seconds
            // The frequency at which we pick a new cover point on our current piece of cover.
            public Bounds<float> CoverHeadingReselectTime; // seconds
            // The farthest from ourselves that we will search for cover.
            public float MaxCoverSearchDistance; // wu
            // The farthest our target can be from us when decided if we should go to cover.
            public float MaxCoverImpulseDistance; // wu
            // The number of seconds we wait after traveling a spline to travel a spline again.
            public Bounds<float> SplineCooldownTime; // wu
            // How far our volume influences our movement
            public float VolumeInfluenceDistance; // wu
            // What is the frequency of oscillation of our volume perturbation vector
            public float VolumePerturbationPhase; // seconds
            // How far we are allowed outside our volume before we started being forced to return (must be >= 0)
            public float VolumeBoundingDistance; // wu
            // If our target is this close to our containing volume, then start attacking him
            public float VolumeApproachDistance; // wu
            // While attacking our target, if we get this far outside our containing volume, break off the attack and return home
            public float VolumeBreakOffDistance; // wu
            // When this close to our target we will drop into the evade behaviour instead
            public float MinimumApproachDistance; // wu
            // Zero collision avoidance at the high distance, Max avoidance at the low distance.
            public Bounds<float> CollisionAvoidanceRange; // wu
            // Bounds on how long we should evade for
            public Bounds<float> EvadeTime; // seconds
            // The recent body damage we should sustain before trying to evade.
            public float EvadeBodyDamageThreshold; // [0,1]
            // The recent shield damage we should sustain before trying to evade.
            public float EvadeShieldDamageThreshold; // [0,1]
            // How long we tolerate a bogey in our six, before we retreat back to our area.
            public float BogeyRetreatTime; // seconds
            // How close a bogey has to be before we'll even consider retreating.
            public float BogeyRetreatDistance; // wu
            // Distance controls for flocking
            public Bounds<float> FlockRadius; // wu
            // How close we must be facing another friend to consider following him during flocking.
            public Angle ForwardFollowAngle; // degrees
            // The angle of the 'cone' behind a friend who I am interested in following that I must be in to consider him during
            // flocking.
            public Angle BehindFollowAngle; // degrees
            // The minimum amount of time we can be tailing
            public float MinTailingTime; // seconds
            // Distance controls for tailing
            public Bounds<float> TailingRadius; // wu
            // The angle of the 'cone' behind a foe who I am interested in tailing after approaching.
            public Angle TailingConeAngle; // degrees
            // If our target is this close to our containing volume, then start strafing him
            public float VolumeStrafeDistance; // wu
            // I need to be at least this far away from my target to consider strafing him (2D)
            public float StrafeMinDistance; // wu
            // How high above our target we will aim for when strafing
            public float StrafeAboveDistance; // wu
            // If I get this close to my target, stop strafing and retreat for a bit (2D)
            public float StrafeAbortDistance; // wu
            // How long we go between strafes
            public float StrafeTimeout; // seconds
            // The maximum angle at which we can descend.
            public Angle MaxDescendAngle; // degrees
            // The maximum angle at which we can ascend.
            public Angle MaxAscendAngle; // degrees
            // The angle of the shooting cone along the vehicle facing.
            public Angle ShootingConeAngle; // degrees
            // The chance that an AI will dodge incoming missiles.
            public float MissileDodgeChange; // percentage
            // The ideal distance a trick should take you away from danger.
            public float IdealMissileDodgeDistance; // wu
        }
        
        [TagStructure(Size = 0xE4)]
        public class CharacterMorphBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag RangedCharacter;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag TankCharacter;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag StealthCharacter;
            [TagField(ValidTags = new [] { "mffn" })]
            public CachedTag MorphMuffins;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag RangedWeapon;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag TankWeapon;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag StealthWeapon;
            // Considered damaging-outside-range when you START firing from outside this distance
            public float DistanceDamageOuterRadius;
            // Considered damaging-outside-range when you CONTINUE firing from outside this distance
            public float DistanceDamageInnerRadius;
            // Damaging tank guy from outside-range for this long causes a morph
            public float DistanceDamageTime;
            // Damage timer is reset after this long of not damaging him from outside-range
            public float DistanceDamageResetTime;
            // Throttle the tank from running (far) to walking (near) across this range of distances. (defaults to 5 and 3)
            public Bounds<float> ThrottleDistance;
            // Once current damage reaches this amount, protect your special parts until no recent damage
            public float ProtectDamageAmount;
            // How long should we protect our special parts for?
            public float ProtectTime; // seconds
            [TagField(ValidTags = new [] { "char" })]
            // What character should I throw up all over my target? Carrots?
            public CachedTag SpewInfectionCharacter;
            // Probability of throwing up a bunch of infection forms when perimeterising
            public float SpewChance;
            // From whence should the infection forms cometh?
            public StringId SpewMarker;
            // Min/max time between spawning each infection form during spew. (defaults to 0.1 and 0.3)
            public Bounds<float> SpewFrequency; // seconds
            // Morphing inside this range causes a tank guy, outside this range causes a ranged fella
            public float StealthMorphDistanceThreshold;
            // Percentage of body health he has to be taken down in order to cause a morph
            public float StealthMorphDamageThreshold;
            // We want to stalk our target from outside this radius
            public float StalkRangeMin;
            // We want to stalk our target from inside this radius
            public float StalkRangeMax;
            // We will never be able to pick a firing position more than this far from our target
            public float StalkRangeHardMax;
            // While stalking, charge randomly with this probability per second (also will charge when on periphery, this is just
            // some spice)
            public float StalkChargeChance;
            // Morph to tank/stalker when someone gets this close to me as a ranged form
            public float RangedProximityDistance;
            // amount of damage necessary to trigger a turtle
            public float TurtleDamageThreshold;
            // when turtling, turtle for a random time with these bounds
            public Bounds<float> TurtleTime; // seconds
            // when I turtle I send out a stimulus to friends within this radius to also turtle
            public float TurtleDistance; // wus
            // when my target get within this range, abort turtling
            public float TurtleAbortDistance;
            // Follow the morph of any other form within this distance
            public float GroupMorphRange;
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterEquipmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            // The equipment item that is to be usable
            public CachedTag Equipment;
            public CharacterEquipmentDefBits Flags;
            // The relative chance of this equipment being dropped with respect to the other pieces of equipment specified in this
            // block
            public float RelativeDropChance;
            public List<CharacterEquipmentUsageBlock> EquipmentUse;
            
            [Flags]
            public enum CharacterEquipmentDefBits : uint
            {
                StopIfNoEnergy = 1 << 0
            }
            
            [TagStructure(Size = 0xC)]
            public class CharacterEquipmentUsageBlock : TagStructure
            {
                // When should we use this equipment?
                public CharacterEquipmentUsageWhenEnum UseWhen;
                // How should we use this equipment?
                public CharacterEquipmentUsageHowEnum UseHow;
                public float EasyNormal; // 0-1
                public float Legendary; // 0-1
                
                public enum CharacterEquipmentUsageWhenEnum : short
                {
                    Combat,
                    Cover,
                    Shield,
                    Health,
                    Uncover,
                    Berserk,
                    Investigate,
                    AntiVehicle
                }
                
                public enum CharacterEquipmentUsageHowEnum : short
                {
                    AttachToSelf,
                    ThrowAtEnemy,
                    ThrowAtFeet,
                    UseOnSelf,
                    StopUsingOnSelf
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterStimuliResponseBlock : TagStructure
        {
            public StringId StimulusName;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag OverrideCharacter;
        }
        
        [TagStructure(Size = 0x8)]
        public class CampaignMetagameBucketBlock : TagStructure
        {
            public CampaignMetagameBucketFlags Flags;
            public CampaignMetagameBucketTypeEnum Type;
            public CampaignMetagameBucketClassEnum Class;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short PointCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum CampaignMetagameBucketFlags : byte
            {
                OnlyCountsWithRiders = 1 << 0
            }
            
            public enum CampaignMetagameBucketTypeEnum : sbyte
            {
                Brute,
                Grunt,
                Jackel,
                Skirmisher,
                Marine,
                Spartan,
                Bugger,
                Hunter,
                FloodInfection,
                FloodCarrier,
                FloodCombat,
                FloodPure,
                Sentinel,
                Elite,
                Engineer,
                Mule,
                Turret,
                Mongoose,
                Warthog,
                Scorpion,
                Hornet,
                Pelican,
                Revenant,
                Seraph,
                Shade,
                Watchtower,
                Ghost,
                Chopper,
                Mauler,
                Wraith,
                Banshee,
                Phantom,
                Scarab,
                Guntower,
                TuningFork,
                Broadsword,
                Mammoth,
                Lich,
                Mantis,
                Wasp,
                Phaeton,
                Bishop,
                Knight,
                Pawn
            }
            
            public enum CampaignMetagameBucketClassEnum : sbyte
            {
                Infantry,
                Leader,
                Hero,
                Specialist,
                LightVehicle,
                HeavyVehicle,
                GiantVehicle,
                StandardVehicle
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class CharacterActivityObjectBlock : TagStructure
        {
            public StringId ActivityName;
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag Crate;
            public StringId CrateMarkerName;
            public StringId UnitMarkerName;
        }
        
        [TagStructure(Size = 0x1C)]
        public class CharacterPainScreenBlock : TagStructure
        {
            // The duration of the pain function
            // 0 defaults to 0.5
            public float PainScreenDuration; // seconds
            // The time it takes to fade out a damage region that is no longer the most recent damage region to be hit
            public float PainScreenRegionFadeOutDuration; // seconds|CCBBAA
            // The threshold weight below which the focus channel must fall before we can cross fade to another region.
            public float PainScreenRegionFadeOutWeightThreshold; // [0,1]
            // The tolerance angle between next and previous damage directions, below which we randomly vary the ping direction.
            public Angle PainScreenAngleTolerance; // degrees
            // The maximum random angle to vary the incoming ping direction by if it's too close to the previous ping.
            public Angle PainScreenAngleRandomness; // degrees
            // The duration of the defensive function
            // 0 defaults to 2.0
            public float DefensiveScreenDuration; // seconds
            // When receiving multiple pings, this is the min percentage of the defensive screen scrub value will fallback to.
            public float DefensiveScreenScrubFallbackFraction; // [0,1]
        }
        
        [TagStructure(Size = 0x138)]
        public class CharacterBishopBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag JunkCollectEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag JunkAttackEffect;
            public Bounds<float> JunkAttackPrepTimeMinMax;
            public Bounds<float> JunkAttackRechargeTimeMinMax;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ShieldEffect;
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag ShieldCrate;
            public Bounds<float> ShieldLifetimeMinMax;
            public Bounds<float> ShieldRechargeTimeMinMax;
            public float ShieldRangeMax;
            // max distance of shield from shieldee
            public float ShieldOffsetMax;
            public Bounds<float> RepairPrepTimeMinMax;
            public Bounds<float> RepairRechargeTimeMinMax;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag RepairBeamEffect;
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag RepairCrate;
            // Min/Max distance that bishop will try to maintain orbit in.
            public Bounds<float> OrbitDistance;
            // Distance at which bishop will stop orbiting and try to catch up to allies.
            public float MinFollowDistance;
            // How high/low bishop will float relative to allies being protected.
            public float VerticalOffset;
            // Controls how far bishop will hang back from combat.
            public float RealLineDistance;
            // Fraction of max throttle to use when catching up to allies.
            public float FollowThrottle;
            // Fraction of max throttle to use when orbiting allies.
            public float OrbitThrottle;
            // Fraction of shield level remaining required to trigger evasion.
            public float EvasionShieldLevelTrigger;
            // Fraction of body vitality remaining required to trigger evasion.
            public float EvasionBodyLevelTrigger;
            // Perceived danger required to trigger evasion.
            public float EvasionDangerThresholdTrigger;
            // Time Bishop waits upon arriving at resurrection target before activating resurrection beam.
            public float ResurrectionInitiationDelayTime;
            // Time from resurrection beam activation until actual resurrection is triggered.
            public float ResurrectionChargeUpTime;
            // If the resurrection target is not reached in this amount of time, the resurrection attempt is aborted.
            public float ResurrectionGiveUpTime;
            [TagField(ValidTags = new [] { "effe" })]
            // The effect created on the ground when resurrecting a dead ally.
            public CachedTag ResurrectionGroundEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // The effect created on the ground when scanning for a clear area before resurrection.
            public CachedTag ResAreaScanStartEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // The effect created on the ground when area scan succeeds.
            public CachedTag ResAreaScanSuccessEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // The effect created on the ground when area scan fails.
            public CachedTag ResAreaScanFailureEffect;
            [TagField(ValidTags = new [] { "bloc" })]
            // Created by bishop when shard spawning.  Runs spawning logic.
            public CachedTag ShardObject;
            [TagField(ValidTags = new [] { "effe" })]
            // The effect created on the ground when spawning shards (pawns).
            public CachedTag ShardSpawnGroundEffect;
            // Time it takes for bishop to place shards after arriving at destination.
            public float ShardCreationDelay;
            // Min/max time it takes for shards to spawn actors.
            public Bounds<float> SpawnDelay;
        }
        
        [TagStructure(Size = 0x44)]
        public class CharacterCombotronParentBlock : TagStructure
        {
            // for each socket, this is the time to wait after desocketing before socketing something else into it
            public Bounds<float> SocketCooldown;
            // use this value to set scaling for child objects / etc...
            public float ChildScale;
            // if a socketed child creates a shield for me, how big should it be?
            public float ShieldScale;
            // when bishop is attached and shielding, where should the shield go?
            public StringId ShieldLocation;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag EffectSocketOccupied;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag EffectSocketCooling;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag EffectSocketAvailable;
        }
        
        [TagStructure(Size = 0x58)]
        public class CharacterCombotronChildBlock : TagStructure
        {
            public RookFlags Flags;
            [TagField(ValidTags = new [] { "weap" })]
            // When socketed, this guy turns into a weapon of the type referenced here
            public CachedTag SocketWeapon;
            [TagField(ValidTags = new [] { "obje" })]
            // When socketed, this guy turns into a child of the type referenced here
            public CachedTag SocketChild;
            [TagField(ValidTags = new [] { "char" })]
            // Parent adopts this character definition when this guy is socketed
            public CachedTag SocketCharacterDefinition;
            // How much damage does this guy absorb before de-socketing
            public float DamageThresholdWhenSocketed;
            // After being de-socketed, wait this long before re-socketing
            public Bounds<float> SocketingCooldown;
            // at what distance does the client particleize and attach to socket
            public Bounds<float> SocketingAttachDistance;
            [TagField(ValidTags = new [] { "effe" })]
            // this effect plays while a child is socketing into a parent
            public CachedTag EffectSocketing;
            
            [Flags]
            public enum RookFlags : uint
            {
                ActiveWhileSocketed = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterHandleDismembermentBlock : TagStructure
        {
            public StringId HeadshotFreakoutAnimation;
            public StringId LimbDismemberedAnimation;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag BerserkPersistentMeleeEffect;
            [TagField(ValidTags = new [] { "weap" })]
            // If I lose my firing arm then I pull out a ...
            public CachedTag AlternateWeapon;
        }
        
        [TagStructure(Size = 0x90)]
        public class CharacterCoverFightBlock : TagStructure
        {
            // Character will prefer to use cover fighting between min/max distances
            public Bounds<float> CoverFightMinMax;
            // How much time do we want to spend before assessing other actions/spots.
            public Bounds<float> WantedCoverTime;
            // How long do we want to kneel.
            public Bounds<float> WantedKneelingTime;
            // How long do we want to stand in cover.
            public Bounds<float> WantedStandingTime;
            // How long do we wait before we realize that no cover spot can be found.
            public float RealizeNoCoverFound;
            // How long to disallow cover fight behavior when no cover is found.
            public Bounds<float> NoCoverFoundSuppress;
            // How much of the character needs to be visible before cover is broken.
            public float FullyExposedWidth;
            // How big is the area that I want to avoid when I am exposed.
            public float MoveExposedAvoidanceRadius;
            // How big is the area that I want to avoid when I move from cover to cover.
            public float MoveAvoidanceRadius;
            // How often to go into cover fighting when found good cover.
            public float CoverFightChanceWhenFoundCover;
            // How often to go into ball movement when moving from cover to cover.
            public float BallingChance;
            // How we rate distance vs side movement when choosing next cover spot.
            public float PreferDistanceOverFlankMovement;
            // Chance of exiting cover fight when too close.
            public float ChanceOfExitingCoverFightWhenTooClose;
            // Damage delta when to interrupt balling.
            public float BallingInterruptDamage;
            public StringId BallingInterruptAnimation;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag IntoBallTransitionEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag OutOfBallTransitionEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag BallMovingEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag BallInterruptedEffect;
        }
        
        [TagStructure(Size = 0x20)]
        public class CharacterEmergeBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag WallEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag FloorEffect;
        }
        
        [TagStructure(Size = 0x38)]
        public class DynamicTaskBlock : TagStructure
        {
            // Limits dynamic tasks that can be assigned to this AI.
            public AgentFilterFlags AgentFilterFlags1;
            // Controls which tasks can be generated by this AI.
            public TaskGenerationFlags TaskGenerationFlags1;
            // Shield vitality must be less than or equal to this level before AI will post Protect task.
            public float ProtectRequestShieldLevel;
            // Body vitality must be less than or equal to this level before AI will post Protect task.
            public float ProtectRequestBodyLevel;
            // Percent chance that actor will request to be resurrected on death.
            public float ResurrectionReqChance;
            // Priority level of Shield task generated by this character.
            public float ShieldTaskPriority;
            // Priority levels of Protect task as health drops.
            public Bounds<float> ProtectTaskPriorities;
            // Priority level of Resurrection task generated by this character.
            public float ResurrectionTaskPriority;
            // Priority of the shield task generated while being resurrected.
            public float ShieldDuringResurrectionTaskPriority;
            [TagField(ValidTags = new [] { "bloc" })]
            // Crate created to represent projected shield.
            public CachedTag ShieldCrateOverride;
            
            [Flags]
            public enum AgentFilterFlags : uint
            {
                Companion = 1 << 0,
                Protector = 1 << 1,
                Spawner = 1 << 2,
                Birther = 1 << 3
            }
            
            [Flags]
            public enum TaskGenerationFlags : uint
            {
                ProtectMe = 1 << 0,
                ResurrectOnDeath = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class CharacterAdvanceBlock : TagStructure
        {
            // Chance per second that actor will initiate an advance when able.
            public float InitiateChance;
            public AdvanceFlags AdvanceFlags1;
            // shield level that triggers charge if charge on shields down is set.
            public float ShieldTriggerVitality;
            // Max allowed range to target for advance to trigger.
            public float MaxRange;
            // Distance at which terminator (slow advance) behavior can kick in.
            public float TerminatorRange;
            // Throttle used while terminating.
            public StringId TerminatorThrottleStyle;
            // Throttle used for fast advance.
            public StringId FastChargeThrottleStyle;
            // Advance will abort if danger goes above this value.
            public float AbortDangerThreshold;
            // Cooldown time before advance can start again.
            public float DelayTimer;
            // Range at which melee charge behavior will trigger automatically during an advance.
            public float ChargeRange;
            // Shortest range for faster advance styles, including teleport and leap.
            public float MinimumFastAdvanceRange;
            // Chance that sword flick will play at the start of advance behavior.
            public float SwordFlickChance;
            public CharacterAdvanceTypeEnum AdvanceMethod;
            // How far from target to arrive
            public float TeleportDestinationDistance;
            // How far to appear to the side of teleport line for intermediate steps
            public float TeleportSidestepDistance;
            // How far to teleport forward for intermediate steps
            public float TeleportForwardDistance;
            // Time between end of sword flick and first teleport
            public float TeleportRunupTime;
            // Time spent at each teleport location
            public float TeleportPositionTime;
            // Time after attack before trying to get away
            public float TeleportCooldownTime;
            // How far the landing point can move between jumps
            public float TeleportTrackingDistance;
            // How far from the target do we stop tracking
            public float TeleportTrackingLockdownDist;
            [TagField(ValidTags = new [] { "effe" })]
            // Effect to play at teleport launch point
            public CachedTag TeleportLaunchFx;
            [TagField(ValidTags = new [] { "effe" })]
            // Effect to play at teleport destination point
            public CachedTag TeleportLandFx;
            
            [Flags]
            public enum AdvanceFlags : uint
            {
                ChargeOnShieldDown = 1 << 0
            }
            
            public enum CharacterAdvanceTypeEnum : int
            {
                Charge,
                Teleport,
                Leap
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterCoverEvasionBlock : TagStructure
        {
            // Danger level needed to trigger behavior.
            public float InitiateDangerThreshold;
            // Danger level that will cause behavior to be aborted.
            public float AbortDangerThreshold;
            // Seconds that must pass before behavior can trigger again.
            public float Cooldown;
            // Min time character must spend in cover before stepping out.
            public float CoverMinTime;
            // Min time that character must spend out of cover before stepping in.
            public float FightMinTime;
            // Danger threshold that causes character to step behind cover again.
            public float StepInDangerThreshold;
            // Chance per second that character will step into cover once danger threshold reached.
            public float StepInChance;
            // Danger must be below this value before character can step out of cover.
            public float StepOutDangerThreshold;
            // Chance per second that character will step out of cover.
            public float StepOutChance;
        }
        
        [TagStructure(Size = 0x14)]
        public class CharacterPackStalkBlock : TagStructure
        {
            // Minimum distance pack must move when search for new cover position.
            public float MinWanderDistance;
            // Distance target must be from pack's assigned area before stalk triggers.
            public float OutsideAreaBorder;
            // Random time each member will wait before following leader.
            public Bounds<float> PositionUpdateDelay;
            // Throttle used when slowing down behind cover.
            public float ThrottleInCover;
        }
        
        [TagStructure(Size = 0x28)]
        public class CharacterFightCircleBlock : TagStructure
        {
            // Time spent facing the player and strafing after reaching a position.
            public Bounds<float> StrafeTime;
            // Time after strafe that trigger is held down.
            public Bounds<float> ExtraFiringTime;
            // Time allowed for turning back onto target at FP, not always counted against firing time.
            public float TargetPatienceTime;
            // Angle used by outside of spring evaluator, including rejector.
            public Angle MaxAngleFromThreatAxis;
            // Angle within which the nearby evaluator considers the point fully preferred.
            public Angle NearbyInnerAngle;
            // Angle outside which the nearby evaluator considers the point fully avoided.
            public Angle NearbyOuterAngle;
            // Throttle used in slow strafe mode
            public StringId StrafeThrottleStyle;
            // Throttle used moving to next firing point
            public StringId MoveThrottleStyle;
        }
        
        [TagStructure(Size = 0x20)]
        public class CharacterHamstringChargeBlock : TagStructure
        {
            // Angle from player facing where charge will begin.
            public Angle FlankAngle;
            // Distance from player before dropping into charge.
            public float FlankDistance;
            // Close to this distance before picking target-relative flanking position.
            public float OuterEngageDistance;
            // Time after last hamstring or melee attack that behavior is banned.
            public float HamstringDelay;
            // Chance hamstring is initiated per second.
            public float InitiateChance;
            // Time to close from flank range to melee charge range.
            public float MaxRushTime;
            // Minimum time for terminal melee attack.
            public float MeleeAttackTimeMin;
            // Maximum time for terminal melee attack.
            public float MeleeAttackTimeMax;
        }
        
        [TagStructure(Size = 0x24)]
        public class CharacterForerunnerBlock : TagStructure
        {
            public ForerunnerFlags ForerunnerFlags1;
            // Chance lackeys are ordered to charge when shield depleted.
            public float OrderMinionChargeChance;
            // Radius within which lackeys can be ordered to charge.
            public float OrderMinionChargeRadius;
            // min time for lackey charge.
            public float MinionChargeMinTime;
            // max time for lackey charge.
            public float MinionChargeMaxTime;
            // Below min, no phase. as distance moves from low to high, chance moves from low to high probabilities
            public Bounds<float> PhaseToPositionDistanceBounds;
            // As distance moves from low to high, chance to phase moves from low to high values
            public Bounds<float> PhaseToPositionProbabilityRange;
            
            [Flags]
            public enum ForerunnerFlags : uint
            {
                ChargeOnCommand = 1 << 0,
                TeleportToCombatPos = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class CharacterGravityJumpBlock : TagStructure
        {
            // Look for destination point within this distance.
            public float RetreatRadius;
            // Time spent at top of leap.
            public float FloatTime;
            // Gravity multiplier during descent.
            public float DescendGravity;
            // Time spent before full gravity returns.
            public float SlowDescendTime;
            // How high to attempt to jump for shot.
            public float JumpTargetHeight;
            // works as a sort of damped spring to draw the character to its landing point. Bigger is faster.
            public float TargetAttractor;
            // Time before this character can gravjump again.
            public float Cooldown;
            // How close your enemy has to be to trigger the jump.
            public float TriggerDistance;
        }
    }
}
