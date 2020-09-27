using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_ai_resource", Tag = "ai**", Size = 0xE4)]
    public class ScenarioAiResource : TagStructure
    {
        public List<StylePaletteEntry> StylePalette;
        public List<SquadGroupDefinition> SquadGroups;
        public List<SquadDefinition> Squads;
        public List<ZoneDefinition> Zones;
        public List<CharacterPaletteEntry> CharacterPalette;
        public List<AiAnimationReferenceDefinition> AiAnimationReferences;
        public List<AiScriptReferenceDefinition> AiScriptReferences;
        public List<AiRecordingReferenceDefinition> AiRecordingReferences;
        public List<AiConversation> AiConversations;
        public List<CsScriptData> ScriptingData;
        public List<OrdersDefinition> Orders;
        public List<TriggerDefinition> Triggers;
        public List<ScenarioStructureBspReference> BspPreferences;
        public List<ScenarioObjectPaletteEntry> WeaponReferences;
        public List<ScenarioObjectPaletteEntry> VehicleReferences;
        public List<ScenarioVehicle> VehicleDatumReferences;
        public List<AiScene> MissionDialogueScenes;
        public List<FlockDefinition> Flocks;
        public List<ScenarioTriggerVolume> TriggerVolumeReferences;
        
        [TagStructure(Size = 0x10)]
        public class StylePaletteEntry : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x24)]
        public class SquadGroupDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Parent;
            public short InitialOrders;
        }
        
        [TagStructure(Size = 0x78)]
        public class SquadDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public TeamValue Team;
            public short Parent;
            public float SquadDelayTime; // seconds
            public short NormalDiffCount; // initial number of actors on normal difficulty
            public short InsaneDiffCount; // initial number of actors on insane difficulty (hard difficulty is midway between normal and insane)
            public MajorUpgradeValue MajorUpgrade;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            /// <summary>
            /// Actor defaults
            /// </summary>
            /// <remarks>
            /// The following default values are used for spawned actors
            /// </remarks>
            public short VehicleType;
            public short CharacterType;
            public short InitialZone;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public short InitialWeapon;
            public short InitialSecondaryWeapon;
            public GrenadeTypeValue GrenadeType;
            public short InitialOrder;
            public StringId VehicleVariant;
            public List<ActorStartingLocationDefinition> StartingLocations;
            [TagField(Length = 32)]
            public string PlacementScript;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown2;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            
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
            public class ActorStartingLocationDefinition : TagStructure
            {
                public StringId Name;
                public RealPoint3d Position;
                public short ReferenceFrame;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public RealEulerAngles2d FacingYawPitch; // degrees
                public FlagsValue Flags;
                public short CharacterType;
                public short InitialWeapon;
                public short InitialSecondaryWeapon;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                public short VehicleType;
                public SeatTypeValue SeatType;
                public GrenadeTypeValue GrenadeType;
                public short SwarmCount; // number of cretures in swarm if a swarm is spawned at this location
                public StringId ActorVariantName;
                public StringId VehicleVariantName;
                public float InitialMovementDistance; // before doing anything else, the actor will travel the given distance in its forward direction
                public short EmitterVehicle;
                public InitialMovementModeValue InitialMovementMode;
                [TagField(Length = 32)]
                public string PlacementScript;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                
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
        
        [TagStructure(Size = 0x40)]
        public class ZoneDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public short ManualBsp;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<FiringPositionDefinition> FiringPositions;
            public List<AreaDefinition> Areas;
            
            [Flags]
            public enum FlagsValue : uint
            {
                ManualBspIndex = 1 << 0
            }
            
            [TagStructure(Size = 0x20)]
            public class FiringPositionDefinition : TagStructure
            {
                /// <summary>
                /// CONTROLS~
                /// </summary>
                /// <remarks>
                /// Ctrl-N: Creates a new area and assigns it to the current selection of firing points.
                /// </remarks>
                public RealPoint3d PositionLocal;
                public short ReferenceFrame;
                public FlagsValue Flags;
                public short Area;
                public short ClusterIndex;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
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
            
            [TagStructure(Size = 0x8C)]
            public class AreaDefinition : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public AreaFlagsValue AreaFlags;
                [TagField(Flags = Padding, Length = 20)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                [TagField(Flags = Padding, Length = 64)]
                public byte[] Padding1;
                public short ManualReferenceFrame;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                public List<FlightHintReference> FlightHints;
                
                [Flags]
                public enum AreaFlagsValue : uint
                {
                    VehicleArea = 1 << 0,
                    Perch = 1 << 1,
                    ManualReferenceFrame = 1 << 2
                }
                
                [TagStructure(Size = 0x4)]
                public class FlightHintReference : TagStructure
                {
                    public short FlightHintIndex;
                    public short PoitIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CharacterPaletteEntry : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x3C)]
        public class AiAnimationReferenceDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string AnimationName;
            public CachedTag AnimationGraph; // leave this blank to use the unit's normal animation graph
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiScriptReferenceDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string ScriptName;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiRecordingReferenceDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string RecordingName;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x74)]
        public class AiConversation : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float TriggerDistance; // world units
            public float RunToPlayerDist; // world units
            [TagField(Flags = Padding, Length = 36)]
            public byte[] Padding2;
            public List<AiConversationParticipant> Participants;
            public List<AiConversationLine> Lines;
            public List<GNullBlock> Unknown1;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                StopIfDeath = 1 << 0,
                StopIfDamaged = 1 << 1,
                StopIfVisibleEnemy = 1 << 2,
                StopIfAlertedToEnemy = 1 << 3,
                PlayerMustBeVisible = 1 << 4,
                StopOtherActions = 1 << 5,
                KeepTryingToPlay = 1 << 6,
                PlayerMustBeLooking = 1 << 7
            }
            
            [TagStructure(Size = 0x54)]
            public class AiConversationParticipant : TagStructure
            {
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding1;
                public short UseThisObject; // if a unit with this name exists, we try to pick them to start the conversation
                public short SetNewName; // once we pick a unit, we name it this
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding3;
                [TagField(Length = 32)]
                public string EncounterName;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding4;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding5;
            }
            
            [TagStructure(Size = 0x7C)]
            public class AiConversationLine : TagStructure
            {
                public FlagsValue Flags;
                public short Participant;
                public AddresseeValue Addressee;
                public short AddresseeParticipant; // this field is only used if the addressee type is 'participant'
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public float LineDelayTime;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding2;
                public CachedTag Variant1;
                public CachedTag Variant2;
                public CachedTag Variant3;
                public CachedTag Variant4;
                public CachedTag Variant5;
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
        
        [TagStructure(Size = 0x84)]
        public class CsScriptData : TagStructure
        {
            public List<CsPointSet> PointSets;
            [TagField(Flags = Padding, Length = 120)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x34)]
            public class CsPointSet : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public List<CsPoint> Points;
                public short BspIndex;
                public short ManualReferenceFrame;
                public FlagsValue Flags;
                
                [TagStructure(Size = 0x3C)]
                public class CsPoint : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                    public RealPoint3d Position;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
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
        
        [TagStructure(Size = 0x90)]
        public class OrdersDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Style;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public FlagsValue Flags;
            public ForceCombatStatusValue ForceCombatStatus;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Length = 32)]
            public string EntryScript;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown2;
            public short FollowSquad;
            public float FollowRadius;
            public List<OrdersAreaReferenceDefinition> PrimaryAreaSet;
            public List<OrdersAreaReferenceDefinition> SecondaryAreaSet;
            public List<SecondarySetTriggers> SecondarySetTrigger;
            public List<SpecialMovementDefinition> SpecialMovement;
            public List<OrderEndingDefinition> OrderEndings;
            
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
            public class OrdersAreaReferenceDefinition : TagStructure
            {
                public AreaTypeValue AreaType;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public short Zone;
                public short Area;
                
                public enum AreaTypeValue : short
                {
                    Deault,
                    Search,
                    Goal
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class SecondarySetTriggers : TagStructure
            {
                public CombinationRuleValue CombinationRule;
                public DialogueTypeValue DialogueType; // when this ending is triggered, launch a dialogue event of the given type
                public List<OrderTriggerReference> Triggers;
                
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
                public class OrderTriggerReference : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class SpecialMovementDefinition : TagStructure
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
            
            [TagStructure(Size = 0x18)]
            public class OrderEndingDefinition : TagStructure
            {
                public short NextOrder;
                public CombinationRuleValue CombinationRule;
                public float DelayTime;
                public DialogueTypeValue DialogueType; // when this ending is triggered, launch a dialogue event of the given type
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public List<OrderTriggerReference> Triggers;
                
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
                public class OrderTriggerReference : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class TriggerDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TriggerFlagsValue TriggerFlags;
            public CombinationRuleValue CombinationRule;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<OrderCompletionConditionDefinition> Conditions;
            
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
            public class OrderCompletionConditionDefinition : TagStructure
            {
                public RuleTypeValue RuleType;
                public short Squad;
                public short SquadGroup;
                public short A;
                public float X;
                public short TriggerVolume;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                [TagField(Length = 32)]
                public string ExitConditionScript;
                public short Unknown1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
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
        
        [TagStructure(Size = 0x54)]
        public class ScenarioStructureBspReference : TagStructure
        {
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public CachedTag StructureBsp;
            public CachedTag StructureLightmap;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float UnusedRadianceEstSearchDistance;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public float UnusedLuminelsPerWorldUnit;
            public float UnusedOutputWhiteReference;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding4;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding5;
            public short DefaultSky;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding6;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DefaultSkyEnabled = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioObjectPaletteEntry : TagStructure
        {
            public CachedTag Name;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioVehicle : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            public ScenarioUnitDatum UnitData;
            
            [TagStructure(Size = 0x30)]
            public class ScenarioObjectDatum : TagStructure
            {
                public PlacementFlagsValue PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public TransformFlagsValue TransformFlags;
                public ushort[] ManualBspFlags;
                public ObjectIdentifier ObjectId;
                public BspPolicyValue BspPolicy;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                public short EditorFolder;
                
                [Flags]
                public enum PlacementFlagsValue : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused = 1 << 1,
                    Unused0 = 1 << 2,
                    Unused1 = 1 << 3,
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
                public class ObjectIdentifier : TagStructure
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
            public class ScenarioObjectPermutation : TagStructure
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
            public class ScenarioUnitDatum : TagStructure
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
        
        [TagStructure(Size = 0x20)]
        public class AiScene : TagStructure
        {
            public StringId Name;
            public FlagsValue Flags;
            public List<AiSceneTrigger> TriggerConditions;
            public List<AiSceneRole> Roles;
            
            [Flags]
            public enum FlagsValue : uint
            {
                SceneCanPlayMultipleTimes = 1 << 0,
                EnableCombatDialogue = 1 << 1
            }
            
            [TagStructure(Size = 0x10)]
            public class AiSceneTrigger : TagStructure
            {
                public CombinationRuleValue CombinationRule;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public List<OrderTriggerReference> Triggers;
                
                public enum CombinationRuleValue : short
                {
                    Or,
                    And
                }
                
                [TagStructure(Size = 0x8)]
                public class OrderTriggerReference : TagStructure
                {
                    public TriggerFlagsValue TriggerFlags;
                    public short Trigger;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum TriggerFlagsValue : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class AiSceneRole : TagStructure
            {
                public StringId Name;
                public GroupValue Group;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public List<AiSceneRoleVariant> RoleVariants;
                
                public enum GroupValue : short
                {
                    Group1,
                    Group2,
                    Group3
                }
                
                [TagStructure(Size = 0x4)]
                public class AiSceneRoleVariant : TagStructure
                {
                    public StringId VariantDesignation;
                }
            }
        }
        
        [TagStructure(Size = 0x94)]
        public class FlockDefinition : TagStructure
        {
            public short Bsp;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short BoundingVolume;
            public FlagsValue Flags;
            public float EcologyMargin; // wus
            public List<FlockSource> Sources;
            public List<FlockSink> Sinks;
            public float ProductionFrequency; // boids/sec
            public Bounds<float> Scale;
            public CachedTag Creature;
            public Bounds<short> BoidCount;
            /// <summary>
            /// Flock parameters
            /// </summary>
            /// <remarks>
            /// Recommended initial values (for a sentinel-sized unit): 
            /// 	neighborhood radius= 6.0 
            /// 	avoidance radius= 3 
            ///  forward scale= 0.5 
            ///  alignment scale= 0.5 
            ///  avoidance scale= 1.0 
            ///  leveling force scale= 0.1 
            ///  perception angle= 120 
            ///  average throttle= 0.75 
            ///  maximum throttle= 1.0 
            ///  position scale= 1.0 
            ///  position min radius= 3 
            ///  position max radius = 9
            /// </remarks>
            /// <summary>
            /// Let me give you a piece of free advice.
            /// </summary>
            /// <remarks>
            /// Flocks with a neighborhood radius of 0 don't FLOCK, per se (in the creature-creature interaction kind of way), but they are much cheaper. The best thing is to have a non-zero radius for small flocks, and a zero radius for large flocks (or for 'flow-flocks', ones with sources and sinks that are intended to create a steady flow of something). Note also that for flocks with a 0 neighborhood radius, the parameters for avoidance, alignment, position and perception angle are ignored entirely.
            /// </remarks>
            public float NeighborhoodRadius; // world units
            public float AvoidanceRadius; // world units
            public float ForwardScale; // [0..1]
            public float AlignmentScale; // [0..1]
            public float AvoidanceScale; // [0..1]
            public float LevelingForceScale; // [0..1]
            public float SinkScale; // [0..1]
            public Angle PerceptionAngle; // degrees
            public float AverageThrottle; // [0..1]
            public float MaximumThrottle; // [0..1]
            public float PositionScale; // [0..1]
            public float PositionMinRadius; // wus
            public float PositionMaxRadius; // wus
            public float MovementWeightThreshold; // The threshold of accumulated weight over which movement occurs
            public float DangerRadius; // wus
            public float DangerScale; // weight given to boid's desire to avoid danger
            /// <summary>
            /// Perlin noise parameters
            /// </summary>
            /// <remarks>
            /// Recommended initial values: 
            /// 	random offset scale= 0.2 
            /// 	offset period bounds= 1, 3
            /// </remarks>
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
            public class FlockSource : TagStructure
            {
                public RealVector3d Position;
                public RealEulerAngles2d StartingYawPitch; // degrees
                public float Radius;
                public float Weight; // probability of producing at this source
            }
            
            [TagStructure(Size = 0x10)]
            public class FlockSink : TagStructure
            {
                public RealVector3d Position;
                public float Radius;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioTriggerVolume : TagStructure
        {
            public StringId Name;
            public short ObjectName;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public StringId NodeName;
            public float Unknown3;
            [TagField(Length = 6)]
            public float EmptyString;
            public RealPoint3d Position;
            public RealPoint3d Extents;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public short KillTriggerVolume;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
        }
    }
}

