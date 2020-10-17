using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_ai_resource", Tag = "ai**", Size = 0x98)]
    public class ScenarioAiResource : TagStructure
    {
        public List<StylePaletteBlock> StylePalette;
        public List<SquadGroupsBlock> SquadGroups;
        public List<SquadsBlock> Squads;
        public List<ZoneBlock> Zones;
        public List<CharacterPaletteBlock> CharacterPalette;
        public List<AiAnimationReferenceBlock> AiAnimationReferences;
        public List<AiScriptReferenceBlock> AiScriptReferences;
        public List<AiRecordingReferenceBlock> AiRecordingReferences;
        public List<AiConversationBlock> AiConversations;
        public List<CsScriptDataBlock> ScriptingData;
        public List<OrdersBlock> Orders;
        public List<TriggersBlock> Triggers;
        public List<ScenarioStructureBspReferenceBlock> BspPreferences;
        public List<ScenarioWeaponPaletteBlock> WeaponReferences;
        public List<ScenarioVehiclePaletteBlock> VehicleReferences;
        public List<ScenarioVehicleBlock> VehicleDatumReferences;
        public List<AiSceneBlock> MissionDialogueScenes;
        public List<FlockDefinitionBlock> Flocks;
        public List<ScenarioTriggerVolumeBlock> TriggerVolumeReferences;
        
        [TagStructure(Size = 0x8)]
        public class StylePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "styl" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x24)]
        public class SquadGroupsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Parent;
            public short InitialOrders;
        }
        
        [TagStructure(Size = 0x74)]
        public class SquadsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public TeamValue Team;
            public short Parent;
            public float SquadDelayTime; // seconds
            /// <summary>
            /// initial number of actors on normal difficulty
            /// </summary>
            public short NormalDiffCount;
            /// <summary>
            /// initial number of actors on insane difficulty (hard difficulty is midway between normal and insane)
            /// </summary>
            public short InsaneDiffCount;
            public MajorUpgradeValue MajorUpgrade;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// The following default values are used for spawned actors
            /// </summary>
            public short VehicleType;
            public short CharacterType;
            public short InitialZone;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public short InitialWeapon;
            public short InitialSecondaryWeapon;
            public GrenadeTypeValue GrenadeType;
            public short InitialOrder;
            public StringId VehicleVariant;
            public List<ActorStartingLocationsBlock> StartingLocations;
            [TagField(Length = 32)]
            public string PlacementScript;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Unused = 1 << 0,
                NeverSearch = 1 << 1,
                StartTimerImmediately = 1 << 2,
                NoTimerDelayForever = 1 << 3,
                MagicSightAfterTimer = 1 << 4,
                AutomaticMigration = 1 << 5,
                Deprecated = 1 << 6,
                RespawnEnabled = 1 << 7,
                Blind = 1 << 8,
                Deaf = 1 << 9,
                Braindead = 1 << 10,
                _3dFiringPositions = 1 << 11,
                InitiallyPlaced = 1 << 12,
                UnitsNotEnterableByPlayer = 1 << 13
            }
            
            public enum TeamValue : short
            {
                Default,
                Player,
                Human,
                Covenant,
                Flood,
                Sentinel,
                Heretic,
                Prophet,
                Unused8,
                Unused9,
                Unused10,
                Unused11,
                Unused12,
                Unused13,
                Unused14,
                Unused15
            }
            
            public enum MajorUpgradeValue : short
            {
                Normal,
                Few,
                Many,
                None,
                All
            }
            
            public enum GrenadeTypeValue : short
            {
                None,
                HumanGrenade,
                CovenantPlasma
            }
            
            [TagStructure(Size = 0x64)]
            public class ActorStartingLocationsBlock : TagStructure
            {
                public StringId Name;
                public RealPoint3d Position;
                public short ReferenceFrame;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealEulerAngles2d FacingYawPitch; // degrees
                public FlagsValue Flags;
                public short CharacterType;
                public short InitialWeapon;
                public short InitialSecondaryWeapon;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public short VehicleType;
                public SeatTypeValue SeatType;
                public GrenadeTypeValue GrenadeType;
                /// <summary>
                /// number of cretures in swarm if a swarm is spawned at this location
                /// </summary>
                public short SwarmCount;
                public StringId ActorVariantName;
                public StringId VehicleVariantName;
                /// <summary>
                /// before doing anything else, the actor will travel the given distance in its forward direction
                /// </summary>
                public float InitialMovementDistance;
                public short EmitterVehicle;
                public InitialMovementModeValue InitialMovementMode;
                [TagField(Length = 32)]
                public string PlacementScript;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyAsleep = 1 << 0,
                    InfectionFormExplode = 1 << 1,
                    NA = 1 << 2,
                    AlwaysPlace = 1 << 3,
                    InitiallyHidden = 1 << 4
                }
                
                public enum SeatTypeValue : short
                {
                    Default,
                    Passenger,
                    Gunner,
                    Driver,
                    SmallCargo,
                    LargeCargo,
                    NoDriver,
                    NoVehicle
                }
                
                public enum GrenadeTypeValue : short
                {
                    None,
                    HumanGrenade,
                    CovenantPlasma
                }
                
                public enum InitialMovementModeValue : short
                {
                    Default,
                    Climbing,
                    Flying
                }
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class ZoneBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public short ManualBsp;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<FiringPositionsBlock> FiringPositions;
            public List<AreasBlock> Areas;
            
            [Flags]
            public enum FlagsValue : uint
            {
                ManualBspIndex = 1 << 0
            }
            
            [TagStructure(Size = 0x20)]
            public class FiringPositionsBlock : TagStructure
            {
                /// <summary>
                /// Ctrl-N: Creates a new area and assigns it to the current selection of firing points.
                /// </summary>
                public RealPoint3d PositionLocal;
                public short ReferenceFrame;
                public FlagsValue Flags;
                public short Area;
                public short ClusterIndex;
                [TagField(Length = 0x4)]
                public byte[] Unknown;
                public RealEulerAngles2d Normal;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Open = 1 << 0,
                    Partial = 1 << 1,
                    Closed = 1 << 2,
                    Mobile = 1 << 3,
                    WallLean = 1 << 4,
                    Perch = 1 << 5,
                    GroundPoint = 1 << 6,
                    DynamicCoverPoint = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x88)]
            public class AreasBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public AreaFlagsValue AreaFlags;
                [TagField(Length = 0x14)]
                public byte[] Unknown;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short ManualReferenceFrame;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public List<FlightReferenceBlock> FlightHints;
                
                [Flags]
                public enum AreaFlagsValue : uint
                {
                    VehicleArea = 1 << 0,
                    Perch = 1 << 1,
                    ManualReferenceFrame = 1 << 2
                }
                
                [TagStructure(Size = 0x4)]
                public class FlightReferenceBlock : TagStructure
                {
                    public short FlightHintIndex;
                    public short PoitIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CharacterPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x34)]
        public class AiAnimationReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string AnimationName;
            /// <summary>
            /// leave this blank to use the unit's normal animation graph
            /// </summary>
            [TagField(ValidTags = new [] { "jmad" })]
            public CachedTag AnimationGraph;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiScriptReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string ScriptName;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiRecordingReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string RecordingName;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x68)]
        public class AiConversationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// distance the player must enter before the conversation can trigger
            /// </summary>
            public float TriggerDistance; // world units
            /// <summary>
            /// if the 'involves player' flag is set, when triggered the conversation's participant(s) will run to within this distance
            /// of the player
            /// </summary>
            public float RunToPlayerDist; // world units
            [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<AiConversationParticipantBlock> Participants;
            public List<AiConversationLineBlock> Lines;
            public List<GNullBlock> Unknown;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                /// <summary>
                /// this conversation will be aborted if any participant dies
                /// </summary>
                StopIfDeath = 1 << 0,
                /// <summary>
                /// an actor will abort this conversation if they are damaged
                /// </summary>
                StopIfDamaged = 1 << 1,
                /// <summary>
                /// an actor will abort this conversation if they see an enemy
                /// </summary>
                StopIfVisibleEnemy = 1 << 2,
                /// <summary>
                /// an actor will abort this conversation if they suspect an enemy
                /// </summary>
                StopIfAlertedToEnemy = 1 << 3,
                /// <summary>
                /// this conversation cannot take place unless the participants can _see_ a nearby player
                /// </summary>
                PlayerMustBeVisible = 1 << 4,
                /// <summary>
                /// participants stop doing whatever they were doing in order to perform this conversation
                /// </summary>
                StopOtherActions = 1 << 5,
                /// <summary>
                /// if this conversation fails initially, it will keep testing to see when it can play
                /// </summary>
                KeepTryingToPlay = 1 << 6,
                /// <summary>
                /// this conversation will not start until the player is looking at one of the participants
                /// </summary>
                PlayerMustBeLooking = 1 << 7
            }
            
            [TagStructure(Size = 0x54)]
            public class AiConversationParticipantBlock : TagStructure
            {
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                /// <summary>
                /// if a unit with this name exists, we try to pick them to start the conversation
                /// </summary>
                public short UseThisObject;
                /// <summary>
                /// once we pick a unit, we name it this
                /// </summary>
                public short SetNewName;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                [TagField(Length = 32)]
                public string EncounterName;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
            }
            
            [TagStructure(Size = 0x4C)]
            public class AiConversationLineBlock : TagStructure
            {
                public FlagsValue Flags;
                public short Participant;
                public AddresseeValue Addressee;
                /// <summary>
                /// this field is only used if the addressee type is 'participant'
                /// </summary>
                public short AddresseeParticipant;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float LineDelayTime;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant1;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant2;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant3;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant5;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant6;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    AddresseeLookAtSpeaker = 1 << 0,
                    EveryoneLookAtSpeaker = 1 << 1,
                    EveryoneLookAtAddressee = 1 << 2,
                    WaitAfterUntilToldToAdvance = 1 << 3,
                    WaitUntilSpeakerNearby = 1 << 4,
                    WaitUntilEveryoneNearby = 1 << 5
                }
                
                public enum AddresseeValue : short
                {
                    None,
                    Player,
                    Participant
                }
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
        }
        
        [TagStructure(Size = 0x80)]
        public class CsScriptDataBlock : TagStructure
        {
            public List<CsPointSetBlock> PointSets;
            [TagField(Length = 0x78, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x30)]
            public class CsPointSetBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public List<CsPointBlock> Points;
                public short BspIndex;
                public short ManualReferenceFrame;
                public FlagsValue Flags;
                
                [TagStructure(Size = 0x3C)]
                public class CsPointBlock : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                    public RealPoint3d Position;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int SurfaceIndex;
                    public RealEulerAngles2d FacingDirection;
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    ManualReferenceFrame = 1 << 0,
                    TurretDeployment = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x7C)]
        public class OrdersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Style;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public FlagsValue Flags;
            public ForceCombatStatusValue ForceCombatStatus;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string EntryScript;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public short FollowSquad;
            public float FollowRadius;
            public List<ZoneSetBlock> PrimaryAreaSet;
            public List<SecondaryZoneSetBlock> SecondaryAreaSet;
            public List<SecondarySetTriggerBlock> SecondarySetTrigger;
            public List<SpecialMovementBlock> SpecialMovement;
            public List<OrderEndingBlock> OrderEndings;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Locked = 1 << 0,
                AlwaysActive = 1 << 1,
                DebugOn = 1 << 2,
                StrictAreaDef = 1 << 3,
                FollowClosestPlayer = 1 << 4,
                FollowSquad = 1 << 5,
                ActiveCamo = 1 << 6,
                SuppressCombatUntilEngaged = 1 << 7,
                InhibitVehicleUse = 1 << 8
            }
            
            public enum ForceCombatStatusValue : short
            {
                None,
                Asleep,
                Idle,
                Alert,
                Combat
            }
            
            [TagStructure(Size = 0x8)]
            public class ZoneSetBlock : TagStructure
            {
                public AreaTypeValue AreaType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Zone;
                public short Area;
                
                public enum AreaTypeValue : short
                {
                    Deault,
                    Search,
                    Goal
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class SecondaryZoneSetBlock : TagStructure
            {
                public AreaTypeValue AreaType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Zone;
                public short Area;
                
                public enum AreaTypeValue : short
                {
                    Deault,
                    Search,
                    Goal
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class SecondarySetTriggerBlock : TagStructure
            {
                public CombinationRuleValue CombinationRule;
                /// <summary>
                /// when this ending is triggered, launch a dialogue event of the given type
                /// </summary>
                public DialogueTypeValue DialogueType;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRuleValue : short
                {
                    Or,
                    And
                }
                
                public enum DialogueTypeValue : short
                {
                    None,
                    Advance,
                    Charge,
                    FallBack,
                    Retreat,
                    Moveone,
                    Arrival,
                    EnterVehicle,
                    ExitVehicle,
                    FollowPlayer,
                    LeavePlayer,
                    Support
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class SpecialMovementBlock : TagStructure
            {
                public SpecialMovement1Value SpecialMovement1;
                
                [Flags]
                public enum SpecialMovement1Value : uint
                {
                    Jump = 1 << 0,
                    Climb = 1 << 1,
                    Vault = 1 << 2,
                    Mount = 1 << 3,
                    Hoist = 1 << 4,
                    WallJump = 1 << 5,
                    NA = 1 << 6
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class OrderEndingBlock : TagStructure
            {
                public short NextOrder;
                public CombinationRuleValue CombinationRule;
                public float DelayTime;
                /// <summary>
                /// when this ending is triggered, launch a dialogue event of the given type
                /// </summary>
                public DialogueTypeValue DialogueType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRuleValue : short
                {
                    Or,
                    And
                }
                
                public enum DialogueTypeValue : short
                {
                    None,
                    Advance,
                    Charge,
                    FallBack,
                    Retreat,
                    Moveone,
                    Arrival,
                    EnterVehicle,
                    ExitVehicle,
                    FollowPlayer,
                    LeavePlayer,
                    Support
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class TriggersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TriggerFlagsValue TriggerFlags;
            public CombinationRuleValue CombinationRule;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<OrderCompletionCondition> Conditions;
            
            [Flags]
            public enum TriggerFlagsValue : uint
            {
                LatchOnWhenTriggered = 1 << 0
            }
            
            public enum CombinationRuleValue : short
            {
                Or,
                And
            }
            
            [TagStructure(Size = 0x38)]
            public class OrderCompletionCondition : TagStructure
            {
                public RuleTypeValue RuleType;
                public short Squad;
                public short SquadGroup;
                public short A;
                public float X;
                public short TriggerVolume;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 32)]
                public string ExitConditionScript;
                public short Unknown;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public FlagsValue Flags;
                
                public enum RuleTypeValue : short
                {
                    AOrGreaterAlive,
                    AOrFewerAlive,
                    XOrGreaterStrength,
                    XOrLessStrength,
                    IfEnemySighted,
                    AfterATicks,
                    IfAlertedBySquadA,
                    ScriptRefTrue,
                    ScriptRefFalse,
                    IfPlayerInTriggerVolume,
                    IfAllPlayersInTriggerVolume,
                    CombatStatusAOrMore,
                    CombatStatusAOrLess,
                    Arrived,
                    InVehicle,
                    SightedPlayer,
                    AOrGreaterFighting,
                    AOrFewerFighting,
                    PlayerWithinXWorldUnits,
                    PlayerShotMoreThanXSecondsAgo,
                    GameSafeToSave
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Not = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioStructureBspReferenceBlock : TagStructure
        {
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag StructureBsp;
            [TagField(ValidTags = new [] { "ltmp" })]
            public CachedTag StructureLightmap;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float UnusedRadianceEstSearchDistance;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float UnusedLuminelsPerWorldUnit;
            public float UnusedOutputWhiteReference;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public short DefaultSky;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DefaultSkyEnabled = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioWeaponPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioVehiclePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioVehicleBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioObjectPermutationStructBlock PermutationData;
            public ScenarioUnitStructBlock UnitData;
            
            [TagStructure(Size = 0x30)]
            public class ScenarioObjectDatumStructBlock : TagStructure
            {
                public PlacementFlagsValue PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public TransformFlagsValue TransformFlags;
                public ushort ManualBspFlags;
                public ScenarioObjectIdStructBlock ObjectId;
                public BspPolicyValue BspPolicy;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short EditorFolder;
                
                [Flags]
                public enum PlacementFlagsValue : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8
                }
                
                [Flags]
                public enum TransformFlagsValue : ushort
                {
                    Mirrored = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStructBlock : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public TypeValue Type;
                    public SourceValue Source;
                    
                    public enum TypeValue : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Garbage,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        LightFixture,
                        SoundScenery,
                        Crate,
                        Creature
                    }
                    
                    public enum SourceValue : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy
                    }
                }
                
                public enum BspPolicyValue : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStructBlock : TagStructure
            {
                public StringId VariantName;
                public ActiveChangeColorsValue ActiveChangeColors;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ActiveChangeColorsValue : uint
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioUnitStructBlock : TagStructure
            {
                public float BodyVitality; // [0,1]
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Dead = 1 << 0,
                    Closed = 1 << 1,
                    NotEnterableByPlayer = 1 << 2
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class AiSceneBlock : TagStructure
        {
            public StringId Name;
            public FlagsValue Flags;
            public List<AiSceneTriggerBlock> TriggerConditions;
            public List<AiSceneRoleBlock> Roles;
            
            [Flags]
            public enum FlagsValue : uint
            {
                SceneCanPlayMultipleTimes = 1 << 0,
                EnableCombatDialogue = 1 << 1
            }
            
            [TagStructure(Size = 0xC)]
            public class AiSceneTriggerBlock : TagStructure
            {
                public CombinationRuleValue CombinationRule;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRuleValue : short
                {
                    Or,
                    And
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class AiSceneRoleBlock : TagStructure
            {
                public StringId Name;
                public GroupValue Group;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<AiSceneRoleVariantsBlock> RoleVariants;
                
                public enum GroupValue : short
                {
                    Group1,
                    Group2,
                    Group3
                }
                
                [TagStructure(Size = 0x4)]
                public class AiSceneRoleVariantsBlock : TagStructure
                {
                    public StringId VariantDesignation;
                }
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class FlockDefinitionBlock : TagStructure
        {
            public short Bsp;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short BoundingVolume;
            public FlagsValue Flags;
            /// <summary>
            /// distance from ecology boundary that creature begins to be repulsed
            /// </summary>
            public float EcologyMargin; // wus
            public List<FlockSourceBlock> Sources;
            public List<FlockSinkBlock> Sinks;
            /// <summary>
            /// How frequently boids are produced at one of the sources (limited by the max boid count)
            /// </summary>
            public float ProductionFrequency; // boids/sec
            public Bounds<float> Scale;
            [TagField(ValidTags = new [] { "crea" })]
            public CachedTag Creature;
            public Bounds<short> BoidCount;
            /// <summary>
            /// Recommended initial values (for a sentinel-sized unit): 
            /// 	neighborhood radius= 6.0 
            /// 	avoidance radius= 3 
            ///  forward scale=
            /// 0.5 
            ///  alignment scale= 0.5 
            ///  avoidance scale= 1.0 
            ///  leveling force scale= 0.1 
            ///  perception angle= 120 
            ///  average throttle=
            /// 0.75 
            ///  maximum throttle= 1.0 
            ///  position scale= 1.0 
            ///  position min radius= 3 
            ///  position max radius = 9
            /// </summary>
            /// <summary>
            /// Flocks with a neighborhood radius of 0 don't FLOCK, per se (in the creature-creature interaction kind of way), but they
            /// are much cheaper. The best thing is to have a non-zero radius for small flocks, and a zero radius for large flocks (or
            /// for 'flow-flocks', ones with sources and sinks that are intended to create a steady flow of something). Note also that
            /// for flocks with a 0 neighborhood radius, the parameters for avoidance, alignment, position and perception angle are
            /// ignored entirely.
            /// </summary>
            /// <summary>
            /// distance within which one boid is affected by another
            /// </summary>
            public float NeighborhoodRadius; // world units
            /// <summary>
            /// distance that a boid tries to maintain from another
            /// </summary>
            public float AvoidanceRadius; // world units
            /// <summary>
            /// weight given to boid's desire to fly straight ahead
            /// </summary>
            public float ForwardScale; // [0..1]
            /// <summary>
            /// weight given to boid's desire to align itself with neighboring boids
            /// </summary>
            public float AlignmentScale; // [0..1]
            /// <summary>
            /// weight given to boid's desire to avoid collisions with other boids, when within the avoidance radius
            /// </summary>
            public float AvoidanceScale; // [0..1]
            /// <summary>
            /// weight given to boids desire to fly level
            /// </summary>
            public float LevelingForceScale; // [0..1]
            /// <summary>
            /// weight given to boid's desire to fly towards its sinks
            /// </summary>
            public float SinkScale; // [0..1]
            /// <summary>
            /// angle-from-forward within which one boid can perceive and react to another
            /// </summary>
            public Angle PerceptionAngle; // degrees
            /// <summary>
            /// throttle at which boids will naturally fly
            /// </summary>
            public float AverageThrottle; // [0..1]
            /// <summary>
            /// maximum throttle applicable
            /// </summary>
            public float MaximumThrottle; // [0..1]
            /// <summary>
            /// weight given to boid's desire to be near flock center
            /// </summary>
            public float PositionScale; // [0..1]
            /// <summary>
            /// distance to flock center beyond which an attracting force is applied
            /// </summary>
            public float PositionMinRadius; // wus
            /// <summary>
            /// distance to flock center at which the maximum attracting force is applied
            /// </summary>
            public float PositionMaxRadius; // wus
            /// <summary>
            /// The threshold of accumulated weight over which movement occurs
            /// </summary>
            public float MovementWeightThreshold;
            /// <summary>
            /// distance within which boids will avoid a dangerous object (e.g. the player)
            /// </summary>
            public float DangerRadius; // wus
            /// <summary>
            /// weight given to boid's desire to avoid danger
            /// </summary>
            public float DangerScale;
            /// <summary>
            /// Recommended initial values: 
            /// 	random offset scale= 0.2 
            /// 	offset period bounds= 1, 3
            /// </summary>
            /// <summary>
            /// weight given to boid's random heading offset
            /// </summary>
            public float RandomOffsetScale; // [0..1]
            public Bounds<float> RandomOffsetPeriod; // seconds
            public StringId FlockName;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                HardBoundariesOnVolume = 1 << 0,
                FlockInitiallyStopped = 1 << 1
            }
            
            [TagStructure(Size = 0x1C)]
            public class FlockSourceBlock : TagStructure
            {
                public RealVector3d Position;
                public RealEulerAngles2d StartingYawPitch; // degrees
                public float Radius;
                /// <summary>
                /// probability of producing at this source
                /// </summary>
                public float Weight;
            }
            
            [TagStructure(Size = 0x10)]
            public class FlockSinkBlock : TagStructure
            {
                public RealVector3d Position;
                public float Radius;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioTriggerVolumeBlock : TagStructure
        {
            public StringId Name;
            public short ObjectName;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public StringId NodeName;
            [TagField(Length = 6)]
            public float[] Unknown1;
            public RealPoint3d Position;
            public RealPoint3d Extents;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short KillTriggerVolume;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
    }
}

