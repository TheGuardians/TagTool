using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x3E0)]
    public class Scenario : TagStructure
    {
        [TagField(ValidTags = new [] { "sbsp" })]
        public CachedTag DoNotUse;
        public List<ScenarioSkyReferenceBlock> Skies;
        public TypeValue Type;
        public FlagsValue Flags;
        public List<ScenarioChildScenarioBlock> ChildScenarios;
        public Angle LocalNorth;
        public List<PredictedResourceBlock> PredictedResources;
        public List<ScenarioFunctionBlock> Functions;
        public byte[] EditorScenarioData;
        public List<EditorCommentBlock> Comments;
        public List<DontUseMeScenarioEnvironmentObjectBlock> Unknown;
        public List<ScenarioObjectNamesBlock> ObjectNames;
        public List<ScenarioSceneryBlock> Scenery;
        public List<ScenarioSceneryPaletteBlock> SceneryPalette;
        public List<ScenarioBipedBlock> Bipeds;
        public List<ScenarioBipedPaletteBlock> BipedPalette;
        public List<ScenarioVehicleBlock> Vehicles;
        public List<ScenarioVehiclePaletteBlock> VehiclePalette;
        public List<ScenarioEquipmentBlock> Equipment;
        public List<ScenarioEquipmentPaletteBlock> EquipmentPalette;
        public List<ScenarioWeaponBlock> Weapons;
        public List<ScenarioWeaponPaletteBlock> WeaponPalette;
        public List<DeviceGroupBlock> DeviceGroups;
        public List<ScenarioMachineBlock> Machines;
        public List<ScenarioMachinePaletteBlock> MachinePalette;
        public List<ScenarioControlBlock> Controls;
        public List<ScenarioControlPaletteBlock> ControlPalette;
        public List<ScenarioLightFixtureBlock> LightFixtures;
        public List<ScenarioLightFixturePaletteBlock> LightFixturesPalette;
        public List<ScenarioSoundSceneryBlock> SoundScenery;
        public List<ScenarioSoundSceneryPaletteBlock> SoundSceneryPalette;
        public List<ScenarioLightBlock> LightVolumes;
        public List<ScenarioLightPaletteBlock> LightVolumesPalette;
        public List<ScenarioProfilesBlock> PlayerStartingProfile;
        public List<ScenarioPlayersBlock> PlayerStartingLocations;
        public List<ScenarioTriggerVolumeBlock> KillTriggerVolumes;
        public List<RecordedAnimationBlock> RecordedAnimations;
        public List<ScenarioNetpointsBlock> NetgameFlags;
        public List<ScenarioNetgameEquipmentBlock> NetgameEquipment;
        public List<ScenarioStartingEquipmentBlock> StartingEquipment;
        public List<ScenarioBspSwitchTriggerVolumeBlock> BspSwitchTriggerVolumes;
        public List<ScenarioDecalsBlock> Decals;
        public List<ScenarioDecalPaletteBlock> DecalsPalette;
        public List<ScenarioDetailObjectCollectionPaletteBlock> DetailObjectCollectionPalette;
        public List<StylePaletteBlock> StylePalette;
        public List<SquadGroupsBlock> SquadGroups;
        public List<SquadsBlock> Squads;
        public List<ZoneBlock> Zones;
        public List<AiSceneBlock> MissionScenes;
        public List<CharacterPaletteBlock> CharacterPalette;
        public List<PathfindingDataBlock> AiPathfindingData;
        public List<AiAnimationReferenceBlock> AiAnimationReferences;
        public List<AiScriptReferenceBlock> AiScriptReferences;
        public List<AiRecordingReferenceBlock> AiRecordingReferences;
        public List<AiConversationBlock> AiConversations;
        public byte[] ScriptSyntaxData;
        public byte[] ScriptStringData;
        public List<HsScriptsBlock> Scripts;
        public List<HsGlobalsBlock> Globals;
        public List<HsReferencesBlock> References;
        public List<HsSourceFilesBlock> SourceFiles;
        public List<CsScriptDataBlock> ScriptingData;
        public List<ScenarioCutsceneFlagBlock> CutsceneFlags;
        public List<ScenarioCutsceneCameraPointBlock> CutsceneCameraPoints;
        public List<ScenarioCutsceneTitleBlock> CutsceneTitles;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag CustomObjectNames;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag ChapterTitleText;
        [TagField(ValidTags = new [] { "hmt " })]
        public CachedTag HudMessages;
        public List<ScenarioStructureBspReferenceBlock> StructureBsps;
        public List<ScenarioResourcesBlock> ScenarioResources;
        public List<OldUnusedStrucurePhysicsBlock> ScenarioResources1;
        public List<HsUnitSeatBlock> HsUnitSeats;
        public List<ScenarioKillTriggerVolumesBlock> ScenarioKillTriggers;
        public List<SyntaxDatumBlock> HsSyntaxDatums;
        public List<OrdersBlock> Orders;
        public List<TriggersBlock> Triggers;
        public List<StructureBspBackgroundSoundPaletteBlock> BackgroundSoundPalette;
        public List<StructureBspSoundEnvironmentPaletteBlock> SoundEnvironmentPalette;
        public List<StructureBspWeatherPaletteBlock> WeatherPalette;
        public List<GNullBlock> Unknown1;
        public List<GNullBlock1> Unknown2;
        public List<GNullBlock2> Unknown3;
        public List<GNullBlock3> Unknown4;
        public List<GNullBlock4> Unknown5;
        public List<ScenarioClusterDataBlock> ScenarioClusterData;
        [TagField(Length = 32)]
        public int[] Unknown6;
        public List<ScenarioSpawnDataBlock> SpawnData;
        [TagField(ValidTags = new [] { "sfx+" })]
        public CachedTag SoundEffectCollection;
        public List<ScenarioCrateBlock> Crates;
        public List<ScenarioCratePaletteBlock> CratesPalette;
        [TagField(ValidTags = new [] { "gldf" })]
        public CachedTag GlobalLighting;
        /// <summary>
        /// Editing Fog palette data will not behave as expected with split scenarios.
        /// </summary>
        public List<ScenarioAtmosphericFogPalette> AtmosphericFogPalette;
        public List<ScenarioPlanarFogPalette> PlanarFogPalette;
        public List<FlockDefinitionBlock> Flocks;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag Subtitles;
        public List<DecoratorPlacementDefinitionBlock> Decorators;
        public List<ScenarioCreatureBlock> Creatures;
        public List<ScenarioCreaturePaletteBlock> CreaturesPalette;
        public List<ScenarioDecoratorSetPaletteEntryBlock> DecoratorsPalette;
        public List<ScenarioBspSwitchTransitionVolumeBlock> BspTransitionVolumes;
        public List<ScenarioStructureBspSphericalHarmonicLightingBlock> StructureBspLighting;
        public List<GScenarioEditorFolderBlock> EditorFolders;
        public List<ScenarioLevelDataBlock> LevelData;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag TerritoryLocationNames;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<AiScenarioMissionDialogueBlock> MissionDialogue;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag Objectives;
        public List<ScenarioInterpolatorBlock> Interpolators;
        public List<HsReferencesBlock1> SharedReferences;
        public List<ScenarioScreenEffectReferenceBlock> ScreenEffectReferences;
        public List<ScenarioSimulationDefinitionTableBlock> SimulationDefinitionTable;
        
        [TagStructure(Size = 0x8)]
        public class ScenarioSkyReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sky " })]
            public CachedTag Sky;
        }
        
        public enum TypeValue : short
        {
            SinglePlayer,
            Multiplayer,
            MainMenu,
            MultiplayerShared,
            SinglePlayerShared
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            /// <summary>
            /// Sorts Cortana in front of other transparent geometry.
            /// </summary>
            CortanaHack = 1 << 0,
            /// <summary>
            /// Always draws sky 0, even if no +sky polygons are visible.
            /// </summary>
            AlwaysDrawSky = 1 << 1,
            /// <summary>
            /// Always leaves pathfinding in, even for multiplayer scenario.
            /// </summary>
            DonTStripPathfinding = 1 << 2,
            SymmetricMultiplayerMap = 1 << 3,
            QuickLoadingCinematicOnlyScenario = 1 << 4,
            CharactersUsePreviousMissionWeapons = 1 << 5,
            LightmapsSmoothPalettesWithNeighbors = 1 << 6,
            SnapToWhiteAtStart = 1 << 7
        }
        
        [TagStructure(Size = 0x18)]
        public class ScenarioChildScenarioBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag ChildScenario;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound,
                RenderModelGeometry,
                ClusterGeometry,
                ClusterInstancedGeometry,
                LightmapGeometryObjectBuckets,
                LightmapGeometryInstanceBuckets,
                LightmapClusterBitmaps,
                LightmapInstanceBitmaps
            }
        }
        
        [TagStructure(Size = 0x78)]
        public class ScenarioFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 32)]
            public string Name;
            /// <summary>
            /// Period for above function (lower values make function oscillate quickly; higher values make it oscillate slowly).
            /// </summary>
            public float Period; // Seconds
            /// <summary>
            /// Multiply this function by above period
            /// </summary>
            public short ScalePeriodBy;
            public FunctionValue Function;
            /// <summary>
            /// Multiply this function by result of above function.
            /// </summary>
            public short ScaleFunctionBy;
            /// <summary>
            /// Curve used for wobble.
            /// </summary>
            public WobbleFunctionValue WobbleFunction;
            /// <summary>
            /// Time it takes for magnitude of this function to complete a wobble.
            /// </summary>
            public float WobblePeriod; // Seconds
            /// <summary>
            /// Amount of random wobble in the magnitude.
            /// </summary>
            public float WobbleMagnitude; // Percent
            /// <summary>
            /// If non-zero, all values above square wave threshold are snapped to 1.0, and all values below it are snapped to 0.0 to
            /// create a square wave.
            /// </summary>
            public float SquareWaveThreshold;
            /// <summary>
            /// Number of discrete values to snap to (e.g., step count of 5 snaps function to 0.00, 0.25, 0.50,0.75, or 1.00).
            /// </summary>
            public short StepCount;
            public MapToValue MapTo;
            /// <summary>
            /// Number of times this function should repeat (e.g., sawtooth count of 5 gives function value of 1.0 at each of 0.25, 0.50,
            /// and 0.75, as well as at 1.0).
            /// </summary>
            public short SawtoothCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// Multiply this function (e.g., from a weapon, vehicle) final result of all of the above math.
            /// </summary>
            public short ScaleResultBy;
            /// <summary>
            /// Controls how bounds, below, are used.
            /// </summary>
            public BoundsModeValue BoundsMode;
            public Bounds<float> Bounds;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            /// <summary>
            /// If specified function is off, so is this function.
            /// </summary>
            public short TurnOffWith;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// Level script will set this value; other settings here will be ignored.
                /// </summary>
                Scripted = 1 << 0,
                /// <summary>
                /// Result of function is 1 minus actual result.
                /// </summary>
                Invert = 1 << 1,
                Additive = 1 << 2,
                /// <summary>
                /// Function does not deactivate when at or below lower bound.
                /// </summary>
                AlwaysActive = 1 << 3
            }
            
            public enum FunctionValue : short
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
            
            public enum MapToValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine,
                One,
                Zero
            }
            
            public enum BoundsModeValue : short
            {
                Clip,
                ClipAndNormalize,
                ScaleToFit
            }
        }
        
        [TagStructure(Size = 0x130)]
        public class EditorCommentBlock : TagStructure
        {
            public RealPoint3d Position;
            public TypeValue Type;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 256)]
            public string Comment;
            
            public enum TypeValue : int
            {
                Generic
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class DontUseMeScenarioEnvironmentObjectBlock : TagStructure
        {
            public short Bsp;
            public short Unknown;
            public int UniqueId;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Tag ObjectDefinitionTag;
            public int Object;
            [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectNamesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown;
            public short Unknown1;
        }
        
        [TagStructure(Size = 0x5C)]
        public class ScenarioSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioObjectPermutationStructBlock PermutationData;
            public ScenarioSceneryDatumStructV4Block SceneryData;
            
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
            
            [TagStructure(Size = 0x10)]
            public class ScenarioSceneryDatumStructV4Block : TagStructure
            {
                public PathfindingPolicyValue PathfindingPolicy;
                public LightmappingPolicyValue LightmappingPolicy;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ValidMultiplayerGamesValue ValidMultiplayerGames;
                
                public enum PathfindingPolicyValue : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                public enum LightmappingPolicyValue : short
                {
                    TagDefault,
                    Dynamic,
                    PerVertex
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
                
                [Flags]
                public enum ValidMultiplayerGamesValue : ushort
                {
                    CaptureTheFlag = 1 << 0,
                    Slayer = 1 << 1,
                    Oddball = 1 << 2,
                    KingOfTheHill = 1 << 3,
                    Juggernaut = 1 << 4,
                    Territories = 1 << 5,
                    Assault = 1 << 6
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioBipedBlock : TagStructure
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
        
        [TagStructure(Size = 0x28)]
        public class ScenarioBipedPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bipd" })]
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
        
        [TagStructure(Size = 0x28)]
        public class ScenarioVehiclePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x38)]
        public class ScenarioEquipmentBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioEquipmentDatumStructBlock EquipmentData;
            
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
            
            [TagStructure(Size = 0x4)]
            public class ScenarioEquipmentDatumStructBlock : TagStructure
            {
                public EquipmentFlagsValue EquipmentFlags;
                
                [Flags]
                public enum EquipmentFlagsValue : uint
                {
                    InitiallyAtRestDoesNotFall = 1 << 0,
                    Obsolete = 1 << 1,
                    DoesAccelerateMovesDueToExplosions = 1 << 2
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioEquipmentPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioWeaponBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioObjectPermutationStructBlock PermutationData;
            public ScenarioWeaponDatumStructBlock WeaponData;
            
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
            public class ScenarioWeaponDatumStructBlock : TagStructure
            {
                public short RoundsLeft;
                public short RoundsLoaded;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyAtRestDoesNotFall = 1 << 0,
                    Obsolete = 1 << 1,
                    DoesAccelerateMovesDueToExplosions = 1 << 2
                }
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
        public class DeviceGroupBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float InitialValue; // [0,1]
            public FlagsValue Flags;
            
            [Flags]
            public enum FlagsValue : uint
            {
                CanChangeOnlyOnce = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class ScenarioMachineBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioDeviceStructBlock DeviceData;
            public ScenarioMachineStructV3Block MachineData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStructBlock : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyOpen10 = 1 << 0,
                    InitiallyOff00 = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class ScenarioMachineStructV3Block : TagStructure
            {
                public FlagsValue Flags;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    DoesNotOperateAutomatically = 1 << 0,
                    OneSided = 1 << 1,
                    NeverAppearsLocked = 1 << 2,
                    OpenedByMeleeAttack = 1 << 3,
                    OneSidedForPlayer = 1 << 4,
                    DoesNotCloseAutomatically = 1 << 5
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioMachinePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mach" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioControlBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioDeviceStructBlock DeviceData;
            public ScenarioControlStructBlock ControlData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStructBlock : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyOpen10 = 1 << 0,
                    InitiallyOff00 = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioControlStructBlock : TagStructure
            {
                public FlagsValue Flags;
                public short DonTTouchThis;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    UsableFromBothSides = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioControlPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ctrl" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioLightFixtureBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioDeviceStructBlock DeviceData;
            public ScenarioLightFixtureStructBlock LightFixtureData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStructBlock : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyOpen10 = 1 << 0,
                    InitiallyOff00 = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioLightFixtureStructBlock : TagStructure
            {
                public RealRgbColor Color;
                public float Intensity;
                public Angle FalloffAngle; // Degrees
                public Angle CutoffAngle; // Degrees
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioLightFixturePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "lifi" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x50)]
        public class ScenarioSoundSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public SoundSceneryDatumStructBlock SoundScenery;
            
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
            
            [TagStructure(Size = 0x1C)]
            public class SoundSceneryDatumStructBlock : TagStructure
            {
                public VolumeTypeValue VolumeType;
                public float Height;
                public Bounds<float> OverrideDistanceBounds;
                public Bounds<Angle> OverrideConeAngleBounds;
                public float OverrideOuterConeGain; // dB
                
                public enum VolumeTypeValue : int
                {
                    Sphere,
                    VerticalCylinder
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioSoundSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ssce" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x6C)]
        public class ScenarioLightBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioDeviceStructBlock DeviceData;
            public ScenarioLightStructBlock LightData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStructBlock : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyOpen10 = 1 << 0,
                    InitiallyOff00 = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class ScenarioLightStructBlock : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public LightmapTypeValue LightmapType;
                public LightmapFlagsValue LightmapFlags;
                public float LightmapHalfLife;
                public float LightmapLightScale;
                public RealPoint3d TargetPoint;
                public float Width; // World Units*
                public float HeightScale; // World Units*
                public Angle FieldOfView; // Degrees*
                public float FalloffDistance; // World Units*
                public float CutoffDistance; // World Units (from Far Plane)*
                
                public enum TypeValue : short
                {
                    Sphere,
                    Orthogonal,
                    Projective,
                    Pyramid
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    CustomGeometry = 1 << 0,
                    Unused = 1 << 1,
                    CinematicOnly = 1 << 2
                }
                
                public enum LightmapTypeValue : short
                {
                    UseLightTagSetting,
                    DynamicOnly,
                    DynamicWithLightmaps,
                    LightmapsOnly
                }
                
                [Flags]
                public enum LightmapFlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioLightPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioProfilesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float StartingHealthDamage; // [0,1]
            public float StartingShieldDamage; // [0,1]
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag PrimaryWeapon;
            public short RoundsLoaded;
            public short RoundsTotal;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag SecondaryWeapon;
            public short RoundsLoaded1;
            public short RoundsTotal1;
            public sbyte StartingFragmentationGrenadeCount;
            public sbyte StartingPlasmaGrenadeCount;
            public sbyte StartingUnknownGrenadeCount;
            public sbyte StartingUnknownGrenadeCount1;
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioPlayersBlock : TagStructure
        {
            public RealPoint3d Position;
            public Angle Facing; // Degrees
            public TeamDesignatorValue TeamDesignator;
            public short BspIndex;
            public GameType1Value GameType1;
            public GameType2Value GameType2;
            public GameType3Value GameType3;
            public GameType4Value GameType4;
            public SpawnType0Value SpawnType0;
            public SpawnType1Value SpawnType1;
            public SpawnType2Value SpawnType2;
            public SpawnType3Value SpawnType3;
            public StringId Unknown;
            public StringId Unknown1;
            public CampaignPlayerTypeValue CampaignPlayerType;
            [TagField(Length = 0x6, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum TeamDesignatorValue : short
            {
                RedAlpha,
                BlueBravo,
                YellowCharlie,
                GreenDelta,
                PurpleEcho,
                OrangeFoxtrot,
                BrownGolf,
                PinkHotel,
                Neutral
            }
            
            public enum GameType1Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType2Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType3Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType4Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum SpawnType0Value : short
            {
                Both,
                InitialSpawnOnly,
                RespawnOnly
            }
            
            public enum SpawnType1Value : short
            {
                Both,
                InitialSpawnOnly,
                RespawnOnly
            }
            
            public enum SpawnType2Value : short
            {
                Both,
                InitialSpawnOnly,
                RespawnOnly
            }
            
            public enum SpawnType3Value : short
            {
                Both,
                InitialSpawnOnly,
                RespawnOnly
            }
            
            public enum CampaignPlayerTypeValue : short
            {
                Masterchief,
                Dervish,
                ChiefMultiplayer,
                EliteMultiplayer
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
        
        [TagStructure(Size = 0x34)]
        public class RecordedAnimationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short LengthOfAnimation; // ticks
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public byte[] RecordedAnimationEventStream;
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioNetpointsBlock : TagStructure
        {
            public RealPoint3d Position;
            public Angle Facing; // Degrees
            public TypeValue Type;
            public TeamDesignatorValue TeamDesignator;
            public short Identifier;
            public FlagsValue Flags;
            public StringId Unknown;
            public StringId Unknown1;
            
            public enum TypeValue : short
            {
                CtfFlagSpawn,
                CtfFlagReturn,
                AssaultBombSpawn,
                AssaultBombReturn,
                OddballSpawn,
                Unused,
                RaceCheckpoint,
                TeleporterSrc,
                TeleporterDest,
                HeadhunterBin,
                TerritoriesFlag,
                KingHill0,
                KingHill1,
                KingHill2,
                KingHill3,
                KingHill4,
                KingHill5,
                KingHill6,
                KingHill7
            }
            
            public enum TeamDesignatorValue : short
            {
                RedAlpha,
                BlueBravo,
                YellowCharlie,
                GreenDelta,
                PurpleEcho,
                OrangeFoxtrot,
                BrownGolf,
                PinkHotel,
                Neutral
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                MultipleFlagBomb = 1 << 0,
                SingleFlagBomb = 1 << 1,
                NeutralFlagBomb = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class ScenarioNetgameEquipmentBlock : TagStructure
        {
            public FlagsValue Flags;
            public GameType1Value GameType1;
            public GameType2Value GameType2;
            public GameType3Value GameType3;
            public GameType4Value GameType4;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short SpawnTimeInSeconds0Default;
            public short RespawnOnEmptyTime; // seconds
            public RespawnTimerStartsValue RespawnTimerStarts;
            public ClassificationValue Classification;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public RealPoint3d Position;
            public ScenarioNetgameEquipmentOrientationStructBlock Orientation;
            [TagField(ValidTags = new [] { "itmc","vehc" })]
            public CachedTag ItemVehicleCollection;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Levitate = 1 << 0,
                DestroyExistingOnNewSpawn = 1 << 1
            }
            
            public enum GameType1Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType2Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType3Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType4Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum RespawnTimerStartsValue : short
            {
                OnPickUp,
                OnBodyDepletion
            }
            
            public enum ClassificationValue : sbyte
            {
                Weapon,
                PrimaryLightLand,
                SecondaryLightLand,
                PrimaryHeavyLand,
                PrimaryFlying,
                SecondaryHeavyLand,
                PrimaryTurret,
                SecondaryTurret,
                Grenade,
                Powerup
            }
            
            [TagStructure(Size = 0xC)]
            public class ScenarioNetgameEquipmentOrientationStructBlock : TagStructure
            {
                public RealEulerAngles3d Orientation;
            }
        }
        
        [TagStructure(Size = 0x9C)]
        public class ScenarioStartingEquipmentBlock : TagStructure
        {
            public FlagsValue Flags;
            public GameType1Value GameType1;
            public GameType2Value GameType2;
            public GameType3Value GameType3;
            public GameType4Value GameType4;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection1;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection2;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection3;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection4;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection5;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection6;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum FlagsValue : uint
            {
                NoGrenades = 1 << 0,
                PlasmaGrenades = 1 << 1
            }
            
            public enum GameType1Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType2Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType3Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
            
            public enum GameType4Value : short
            {
                None,
                CaptureTheFlag,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Headhunter,
                Juggernaut,
                Territories,
                Stub,
                Ignored3,
                Ignored4,
                AllGameTypes,
                AllExceptCtf,
                AllExceptCtfRace
            }
        }
        
        [TagStructure(Size = 0xE)]
        public class ScenarioBspSwitchTriggerVolumeBlock : TagStructure
        {
            public short TriggerVolume;
            public short Source;
            public short Destination;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalsBlock : TagStructure
        {
            public short DecalType;
            public sbyte Yaw127127;
            public sbyte Pitch127127;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioDecalPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "deca" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioDetailObjectCollectionPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "dobc" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
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
        
        [TagStructure(Size = 0x8)]
        public class CharacterPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x74)]
        public class PathfindingDataBlock : TagStructure
        {
            public List<SectorBlock> Sectors;
            public List<SectorLinkBlock> Links;
            public List<RefBlock> Refs;
            public List<SectorBsp2dNodesBlock> Bsp2dNodes;
            public List<SurfaceFlagsBlock> SurfaceFlags;
            public List<SectorVertexBlock> Vertices;
            public List<EnvironmentObjectRefs> ObjectRefs;
            public List<PathfindingHintsBlock> PathfindingHints;
            public List<InstancedGeometryReferenceBlock> InstancedGeometryRefs;
            public int StructureChecksum;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<UserHintBlock> UserPlacedHints;
            
            [TagStructure(Size = 0x8)]
            public class SectorBlock : TagStructure
            {
                public PathFindingSectorFlagsValue PathFindingSectorFlags;
                public short HintIndex;
                public int FirstLinkDoNotSetManually;
                
                [Flags]
                public enum PathFindingSectorFlagsValue : ushort
                {
                    SectorWalkable = 1 << 0,
                    SectorBreakable = 1 << 1,
                    SectorMobile = 1 << 2,
                    SectorBspSource = 1 << 3,
                    Floor = 1 << 4,
                    Ceiling = 1 << 5,
                    WallNorth = 1 << 6,
                    WallSouth = 1 << 7,
                    WallEast = 1 << 8,
                    WallWest = 1 << 9,
                    Crouchable = 1 << 10,
                    Aligned = 1 << 11,
                    SectorStep = 1 << 12,
                    SectorInterior = 1 << 13
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class SectorLinkBlock : TagStructure
            {
                public short Vertex1;
                public short Vertex2;
                public LinkFlagsValue LinkFlags;
                public short HintIndex;
                public short ForwardLink;
                public short ReverseLink;
                public short LeftSector;
                public short RightSector;
                
                [Flags]
                public enum LinkFlagsValue : ushort
                {
                    SectorLinkFromCollisionEdge = 1 << 0,
                    SectorIntersectionLink = 1 << 1,
                    SectorLinkBsp2dCreationError = 1 << 2,
                    SectorLinkTopologyError = 1 << 3,
                    SectorLinkChainError = 1 << 4,
                    SectorLinkBothSectorsWalkable = 1 << 5,
                    SectorLinkMagicHangingLink = 1 << 6,
                    SectorLinkThreshold = 1 << 7,
                    SectorLinkCrouchable = 1 << 8,
                    SectorLinkWallBase = 1 << 9,
                    SectorLinkLedge = 1 << 10,
                    SectorLinkLeanable = 1 << 11,
                    SectorLinkStartCorner = 1 << 12,
                    SectorLinkEndCorner = 1 << 13
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class RefBlock : TagStructure
            {
                public int NodeRefOrSectorRef;
            }
            
            [TagStructure(Size = 0x14)]
            public class SectorBsp2dNodesBlock : TagStructure
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }
            
            [TagStructure(Size = 0x4)]
            public class SurfaceFlagsBlock : TagStructure
            {
                public int Flags;
            }
            
            [TagStructure(Size = 0xC)]
            public class SectorVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
            
            [TagStructure(Size = 0x1C)]
            public class EnvironmentObjectRefs : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int FirstSector;
                public int LastSector;
                public List<EnvironmentObjectBspRefs> Bsps;
                public List<EnvironmentObjectNodes> Nodes;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Mobile = 1 << 0
                }
                
                [TagStructure(Size = 0x10)]
                public class EnvironmentObjectBspRefs : TagStructure
                {
                    public int BspReference;
                    public int FirstSector;
                    public int LastSector;
                    public short NodeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x4)]
                public class EnvironmentObjectNodes : TagStructure
                {
                    public short ReferenceFrameIndex;
                    public sbyte ProjectionAxis;
                    public ProjectionSignValue ProjectionSign;
                    
                    [Flags]
                    public enum ProjectionSignValue : byte
                    {
                        ProjectionSign = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class PathfindingHintsBlock : TagStructure
            {
                public HintTypeValue HintType;
                public short NextHintIndex;
                public short HintData0;
                public short HintData1;
                public short HintData2;
                public short HintData3;
                public short HintData4;
                public short HintData5;
                public short HintData6;
                public short HintData7;
                
                public enum HintTypeValue : short
                {
                    IntersectionLink,
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    BreakableFloor
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class InstancedGeometryReferenceBlock : TagStructure
            {
                public short PathfindingObjectIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x48)]
            public class UserHintBlock : TagStructure
            {
                public List<UserHintPointBlock> PointGeometry;
                public List<UserHintRayBlock> RayGeometry;
                public List<UserHintLineSegmentBlock> LineSegmentGeometry;
                public List<UserHintParallelogramBlock> ParallelogramGeometry;
                public List<UserHintPolygonBlock> PolygonGeometry;
                public List<UserHintJumpBlock> JumpHints;
                public List<UserHintClimbBlock> ClimbHints;
                public List<UserHintWellBlock> WellHints;
                public List<UserHintFlightBlock> FlightHints;
                
                [TagStructure(Size = 0x10)]
                public class UserHintPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintRayBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealVector3d Vector;
                }
                
                [TagStructure(Size = 0x24)]
                public class UserHintLineSegmentBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class UserHintParallelogramBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    public RealPoint3d Point2;
                    public short ReferenceFrame2;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding2;
                    public RealPoint3d Point3;
                    public short ReferenceFrame3;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding3;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class UserHintPolygonBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintPointBlock> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class UserHintPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public short ReferenceFrame;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class UserHintJumpBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public short GeometryIndex;
                    public ForceJumpHeightValue ForceJumpHeight;
                    public ControlFlagsValue ControlFlags;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    public enum ForceJumpHeightValue : short
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
                    public enum ControlFlagsValue : ushort
                    {
                        MagicLift = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserHintClimbBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public short GeometryIndex;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class UserHintWellBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintWellPointBlock> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class UserHintWellPointBlock : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealVector3d Point;
                        public short ReferenceFrame;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        public int SectorIndex;
                        public RealEulerAngles2d Normal;
                        
                        public enum TypeValue : short
                        {
                            Jump,
                            Climb,
                            Hoist
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class UserHintFlightBlock : TagStructure
                {
                    public List<UserHintFlightPointBlock> Points;
                    
                    [TagStructure(Size = 0xC)]
                    public class UserHintFlightPointBlock : TagStructure
                    {
                        public RealVector3d Point;
                    }
                }
            }
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
        
        [TagStructure(Size = 0x28)]
        public class HsScriptsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public ScriptTypeValue ScriptType;
            public ReturnTypeValue ReturnType;
            public int RootExpressionIndex;
            
            public enum ScriptTypeValue : short
            {
                Startup,
                Dormant,
                Continuous,
                Static,
                Stub,
                CommandScript
            }
            
            public enum ReturnTypeValue : short
            {
                Unparsed,
                SpecialForm,
                FunctionName,
                Passthrough,
                Void,
                Boolean,
                Real,
                Short,
                Long,
                String,
                Script,
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                StartingProfile,
                Conversation,
                StructureBsp,
                Navpoint,
                PointReference,
                Style,
                HudMessage,
                ObjectList,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                GameDifficulty,
                Team,
                ActorType,
                HudCorner,
                ModelState,
                NetworkEvent,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class HsGlobalsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int InitializationExpressionIndex;
            
            public enum TypeValue : short
            {
                Unparsed,
                SpecialForm,
                FunctionName,
                Passthrough,
                Void,
                Boolean,
                Real,
                Short,
                Long,
                String,
                Script,
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                StartingProfile,
                Conversation,
                StructureBsp,
                Navpoint,
                PointReference,
                Style,
                HudMessage,
                ObjectList,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                GameDifficulty,
                Team,
                ActorType,
                HudCorner,
                ModelState,
                NetworkEvent,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x28)]
        public class HsSourceFilesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public byte[] Source;
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
        
        [TagStructure(Size = 0x38)]
        public class ScenarioCutsceneFlagBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioCutsceneCameraPointBlock : TagStructure
        {
            public FlagsValue Flags;
            public TypeValue Type;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
            public Angle Unused;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                EditAsRelative = 1 << 0
            }
            
            public enum TypeValue : short
            {
                Normal,
                IgnoreTargetOrientation,
                Dolly,
                IgnoreTargetUpdates
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioCutsceneTitleBlock : TagStructure
        {
            public StringId Name;
            public Rectangle2d TextBoundsOnScreen;
            public JustificationValue Justification;
            public FontValue Font;
            public ArgbColor TextColor;
            public ArgbColor ShadowColor;
            public float FadeInTimeSeconds;
            public float UpTimeSeconds;
            public float FadeOutTimeSeconds;
            
            public enum JustificationValue : short
            {
                Left,
                Right,
                Center,
                CustomTextEntry
            }
            
            public enum FontValue : short
            {
                TerminalFont,
                BodyTextFont,
                TitleFont,
                SuperLargeFont,
                LargeBodyTextFont,
                SplitScreenHudMessageFont,
                FullScreenHudMessageFont,
                EnglishBodyTextFont,
                HudNumberFont,
                SubtitleFont,
                MainMenuFont,
                TextChatFont
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
        
        [TagStructure(Size = 0x18)]
        public class ScenarioResourcesBlock : TagStructure
        {
            public List<ScenarioResourceReferenceBlock> References;
            public List<ScenarioHsSourceReferenceBlock> ScriptSource;
            public List<ScenarioAiResourceReferenceBlock> AiResources;
            
            [TagStructure(Size = 0x8)]
            public class ScenarioResourceReferenceBlock : TagStructure
            {
                public CachedTag Reference;
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioHsSourceReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "hsc*" })]
                public CachedTag Reference;
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioAiResourceReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "ai**" })]
                public CachedTag Reference;
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class OldUnusedStrucurePhysicsBlock : TagStructure
        {
            public byte[] MoppCode;
            public List<OldUnusedObjectIdentifiersBlock> EvironmentObjectIdentifiers;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d MoppBoundsMin;
            public RealPoint3d MoppBoundsMax;
            
            [TagStructure(Size = 0x8)]
            public class OldUnusedObjectIdentifiersBlock : TagStructure
            {
                public ScenarioObjectIdStructBlock ObjectId;
                
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
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class HsUnitSeatBlock : TagStructure
        {
            public int Unknown;
            public int Unknown1;
        }
        
        [TagStructure(Size = 0x2)]
        public class ScenarioKillTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
        }
        
        [TagStructure(Size = 0x14)]
        public class SyntaxDatumBlock : TagStructure
        {
            public short DatumHeader;
            public short ScriptIndexFunctionIndexConstantTypeUnion;
            public short Type;
            public short Flags;
            public int NextNodeIndex;
            public int Data;
            public int SourceOffset;
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
        
        [TagStructure(Size = 0x64)]
        public class StructureBspBackgroundSoundPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag BackgroundSound;
            /// <summary>
            /// Play only when player is inside cluster.
            /// </summary>
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag InsideClusterSound;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class StructureBspSoundEnvironmentPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "snde" })]
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x88)]
        public class StructureBspWeatherPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "weat" })]
            public CachedTag WeatherSystem;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "wind" })]
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure()]
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure()]
        public class GNullBlock1 : TagStructure
        {
        }
        
        [TagStructure()]
        public class GNullBlock2 : TagStructure
        {
        }
        
        [TagStructure()]
        public class GNullBlock3 : TagStructure
        {
        }
        
        [TagStructure()]
        public class GNullBlock4 : TagStructure
        {
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioClusterDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag Bsp;
            public List<ScenarioClusterBackgroundSoundsBlock> BackgroundSounds;
            public List<ScenarioClusterSoundEnvironmentsBlock> SoundEnvironments;
            public int BspChecksum;
            public List<ScenarioClusterPointsBlock> ClusterCentroids;
            public List<ScenarioClusterWeatherPropertiesBlock> WeatherProperties;
            public List<ScenarioClusterAtmosphericFogPropertiesBlock> AtmosphericFogProperties;
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterBackgroundSoundsBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterSoundEnvironmentsBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0xC)]
            public class ScenarioClusterPointsBlock : TagStructure
            {
                public RealPoint3d Centroid;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterWeatherPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterAtmosphericFogPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioSpawnDataBlock : TagStructure
        {
            /// <summary>
            /// Non-0 values here overload what appears in multiplayer_globals.
            /// </summary>
            public float DynamicSpawnLowerHeight;
            public float DynamicSpawnUpperHeight;
            public float GameObjectResetHeight;
            [TagField(Length = 0x3C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<DynamicSpawnZoneOverloadBlock> DynamicSpawnOverloads;
            public List<StaticSpawnZoneBlock> StaticRespawnZones;
            public List<StaticSpawnZoneBlock1> StaticInitialSpawnZones;
            
            [TagStructure(Size = 0x10)]
            public class DynamicSpawnZoneOverloadBlock : TagStructure
            {
                public OverloadTypeValue OverloadType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float InnerRadius;
                public float OuterRadius;
                public float Weight;
                
                public enum OverloadTypeValue : short
                {
                    Enemy,
                    Friend,
                    EnemyVehicle,
                    FriendlyVehicle,
                    EmptyVehicle,
                    OddballInclusion,
                    OddballExclusion,
                    HillInclusion,
                    HillExclusion,
                    LastRaceFlag,
                    DeadAlly,
                    ControlledTerritory
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class StaticSpawnZoneBlock : TagStructure
            {
                /// <summary>
                /// Lower and upper heights can be left at 0, in which case they use defaults.  Leaving relevant teams empty means all teams;
                /// leaving all games empty means all games.
                /// </summary>
                public StaticSpawnZoneDataStructBlock Data;
                public RealPoint3d Position;
                public float LowerHeight;
                public float UpperHeight;
                public float InnerRadius;
                public float OuterRadius;
                public float Weight;
                
                [TagStructure(Size = 0x10)]
                public class StaticSpawnZoneDataStructBlock : TagStructure
                {
                    public StringId Name;
                    public RelevantTeamValue RelevantTeam;
                    public RelevantGamesValue RelevantGames;
                    public FlagsValue Flags;
                    
                    [Flags]
                    public enum RelevantTeamValue : uint
                    {
                        RedAlpha = 1 << 0,
                        BlueBravo = 1 << 1,
                        YellowCharlie = 1 << 2,
                        GreenDelta = 1 << 3,
                        PurpleEcho = 1 << 4,
                        OrangeFoxtrot = 1 << 5,
                        BrownGolf = 1 << 6,
                        PinkHotel = 1 << 7,
                        Neutral = 1 << 8
                    }
                    
                    [Flags]
                    public enum RelevantGamesValue : uint
                    {
                        Slayer = 1 << 0,
                        Oddball = 1 << 1,
                        KingOfTheHill = 1 << 2,
                        CaptureTheFlag = 1 << 3,
                        Race = 1 << 4,
                        Headhunter = 1 << 5,
                        Juggernaut = 1 << 6,
                        Territories = 1 << 7
                    }
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        DisabledIfFlagHome = 1 << 0,
                        DisabledIfFlagAway = 1 << 1,
                        DisabledIfBombHome = 1 << 2,
                        DisabledIfBombAway = 1 << 3
                    }
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class StaticSpawnZoneBlock1 : TagStructure
            {
                /// <summary>
                /// Lower and upper heights can be left at 0, in which case they use defaults.  Leaving relevant teams empty means all teams;
                /// leaving all games empty means all games.
                /// </summary>
                public StaticSpawnZoneDataStructBlock Data;
                public RealPoint3d Position;
                public float LowerHeight;
                public float UpperHeight;
                public float InnerRadius;
                public float OuterRadius;
                public float Weight;
                
                [TagStructure(Size = 0x10)]
                public class StaticSpawnZoneDataStructBlock : TagStructure
                {
                    public StringId Name;
                    public RelevantTeamValue RelevantTeam;
                    public RelevantGamesValue RelevantGames;
                    public FlagsValue Flags;
                    
                    [Flags]
                    public enum RelevantTeamValue : uint
                    {
                        RedAlpha = 1 << 0,
                        BlueBravo = 1 << 1,
                        YellowCharlie = 1 << 2,
                        GreenDelta = 1 << 3,
                        PurpleEcho = 1 << 4,
                        OrangeFoxtrot = 1 << 5,
                        BrownGolf = 1 << 6,
                        PinkHotel = 1 << 7,
                        Neutral = 1 << 8
                    }
                    
                    [Flags]
                    public enum RelevantGamesValue : uint
                    {
                        Slayer = 1 << 0,
                        Oddball = 1 << 1,
                        KingOfTheHill = 1 << 2,
                        CaptureTheFlag = 1 << 3,
                        Race = 1 << 4,
                        Headhunter = 1 << 5,
                        Juggernaut = 1 << 6,
                        Territories = 1 << 7
                    }
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        DisabledIfFlagHome = 1 << 0,
                        DisabledIfFlagAway = 1 << 1,
                        DisabledIfBombHome = 1 << 2,
                        DisabledIfBombAway = 1 << 3
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class ScenarioCrateBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioObjectPermutationStructBlock PermutationData;
            
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
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioCratePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0xF4)]
        public class ScenarioAtmosphericFogPalette : TagStructure
        {
            public StringId Name;
            public RealRgbColor Color;
            /// <summary>
            /// How far fog spreads into adjacent clusters: 0 defaults to 1.
            /// </summary>
            public float SpreadDistance; // World Units
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// Fog density clamps to this value.
            /// </summary>
            public float MaximumDensity; // [0,1]
            /// <summary>
            /// Before this distance, there is no fog.
            /// </summary>
            public float StartDistance; // World Units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from viewer.
            /// </summary>
            public float OpaqueDistance; // World Units
            public RealRgbColor Color1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// Fog density clamps to this value.
            /// </summary>
            public float MaximumDensity1; // [0,1]
            /// <summary>
            /// Before this distance, there is no fog.
            /// </summary>
            public float StartDistance1; // World Units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from viewer.
            /// </summary>
            public float OpaqueDistance1; // World Units
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            /// <summary>
            /// Planar fog, if present, is interpolated toward these values.
            /// </summary>
            public RealRgbColor PlanarColor;
            public float PlanarMaxDensity; // [0,1]
            public float PlanarOverrideAmount; // [0,1]
            /// <summary>
            /// Don't ask.
            /// </summary>
            public float PlanarMinDistanceBias; // World Units
            [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public RealRgbColor PatchyColor;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public Bounds<float> PatchyDensity; // [0,1]
            public Bounds<float> PatchyDistance; // World Units
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(ValidTags = new [] { "fpch" })]
            public CachedTag PatchyFog;
            public List<ScenarioAtmosphericFogMixerBlock> Mixers;
            public float Amount; // [0,1]
            public float Threshold; // [0,1]
            public float Brightness; // [0,1]
            public float GammaPower;
            public CameraImmersionFlagsValue CameraImmersionFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            
            [TagStructure(Size = 0x10)]
            public class ScenarioAtmosphericFogMixerBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId AtmosphericFogSource; // From Scenario Atmospheric Fog Palette
                public StringId Interpolator; // From Scenario Interpolators
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                [TagField(Length = 0x2)]
                public byte[] Unknown1;
            }
            
            [Flags]
            public enum CameraImmersionFlagsValue : ushort
            {
                DisableAtmosphericFog = 1 << 0,
                DisableSecondaryFog = 1 << 1,
                DisablePlanarFog = 1 << 2,
                InvertPlanarFogPriorities = 1 << 3,
                DisableWater = 1 << 4
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioPlanarFogPalette : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "fog " })]
            public CachedTag PlanarFog;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x30)]
        public class DecoratorPlacementDefinitionBlock : TagStructure
        {
            public RealPoint3d GridOrigin;
            public int CellCountPerDimension;
            public List<DecoratorCacheBlockBlock> CacheBlocks;
            public List<DecoratorGroupBlock> Groups;
            public List<DecoratorCellCollectionBlock> Cells;
            public List<DecoratorProjectedDecalBlock> Decals;
            
            [TagStructure(Size = 0x34)]
            public class DecoratorCacheBlockBlock : TagStructure
            {
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<DecoratorCacheBlockDataBlock> CacheBlockData;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [TagStructure(Size = 0x24)]
                public class GlobalGeometryBlockInfoStructBlock : TagStructure
                {
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GlobalGeometryBlockResourceBlock> Resources;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short OwnerTagSectionOffset;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding2;
                    
                    [TagStructure(Size = 0x10)]
                    public class GlobalGeometryBlockResourceBlock : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public short PrimaryLocator;
                        public short SecondaryLocator;
                        public int ResourceDataSize;
                        public int ResourceDataOffset;
                        
                        public enum TypeValue : sbyte
                        {
                            TagBlock,
                            TagData,
                            VertexBuffer
                        }
                    }
                }
                
                [TagStructure(Size = 0x88)]
                public class DecoratorCacheBlockDataBlock : TagStructure
                {
                    public List<DecoratorPlacementBlock> Placements;
                    public List<DecalVerticesBlock> DecalVertices;
                    public List<IndicesBlock> DecalIndices;
                    public VertexBuffer DecalVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<SpriteVerticesBlock> SpriteVertices;
                    public List<IndicesBlock1> SpriteIndices;
                    public VertexBuffer SpriteVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x18)]
                    public class DecoratorPlacementBlock : TagStructure
                    {
                        public int InternalData1;
                        public int CompressedPosition;
                        public ArgbColor TintColor;
                        public ArgbColor LightmapColor;
                        public int CompressedLightDirection;
                        public int CompressedLight2Direction;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class DecalVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealPoint2d Texcoord0;
                        public RealPoint2d Texcoord1;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class SpriteVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealVector3d Offset;
                        public RealVector3d Axis;
                        public RealPoint2d Texcoord;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock1 : TagStructure
                    {
                        public short Index;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorGroupBlock : TagStructure
            {
                public sbyte DecoratorSet;
                public DecoratorTypeValue DecoratorType;
                public sbyte ShaderIndex;
                public sbyte CompressedRadius;
                public short Cluster;
                public short CacheBlock;
                public short DecoratorStartIndex;
                public short DecoratorCount;
                public short VertexStartOffset;
                public short VertexCount;
                public short IndexStartOffset;
                public short IndexCount;
                public int CompressedBoundingCenter;
                
                public enum DecoratorTypeValue : sbyte
                {
                    Model,
                    FloatingDecal,
                    ProjectedDecal,
                    ScreenFacingQuad,
                    AxisRotatingQuad,
                    CrossQuad
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorCellCollectionBlock : TagStructure
            {
                [TagField(Length = 8)]
                public short[] ChildIndex;
                public short CacheBlockIndex;
                public short GroupCount;
                public int GroupStartIndex;
            }
            
            [TagStructure(Size = 0x40)]
            public class DecoratorProjectedDecalBlock : TagStructure
            {
                public sbyte DecoratorSet;
                public sbyte DecoratorClass;
                public sbyte DecoratorPermutation;
                public sbyte SpriteIndex;
                public RealPoint3d Position;
                public RealVector3d Left;
                public RealVector3d Up;
                public RealVector3d Extents;
                public RealPoint3d PreviousPosition;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioCreatureBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            
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
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioCreaturePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "crea" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioDecoratorSetPaletteEntryBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "DECR" })]
            public CachedTag DecoratorSet;
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioBspSwitchTransitionVolumeBlock : TagStructure
        {
            public int BspIndexKey;
            public short TriggerVolume;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioStructureBspSphericalHarmonicLightingBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag Bsp;
            public List<ScenarioSphericalHarmonicLightingPoint> LightingPoints;
            
            [TagStructure(Size = 0xC)]
            public class ScenarioSphericalHarmonicLightingPoint : TagStructure
            {
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class GScenarioEditorFolderBlock : TagStructure
        {
            public int ParentFolder;
            [TagField(Length = 256)]
            public string Name;
        }
        
        [TagStructure(Size = 0x18)]
        public class ScenarioLevelDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag LevelDescription;
            public List<GlobalUiCampaignLevelBlock> CampaignLevelData;
            public List<GlobalUiMultiplayerLevelBlock> Multiplayer;
            
            [TagStructure(Size = 0xB50)]
            public class GlobalUiCampaignLevelBlock : TagStructure
            {
                public int CampaignId;
                public int MapId;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(Length = 0x240)]
                public byte[] Unknown;
                [TagField(Length = 0x900)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0xC64)]
            public class GlobalUiMultiplayerLevelBlock : TagStructure
            {
                public int MapId;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(Length = 0x240)]
                public byte[] Unknown;
                [TagField(Length = 0x900)]
                public byte[] Unknown1;
                [TagField(Length = 256)]
                public string Path;
                public int SortOrder;
                public FlagsValue Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public sbyte MaxTeamsNone;
                public sbyte MaxTeamsCtf;
                public sbyte MaxTeamsSlayer;
                public sbyte MaxTeamsOddball;
                public sbyte MaxTeamsKoth;
                public sbyte MaxTeamsRace;
                public sbyte MaxTeamsHeadhunter;
                public sbyte MaxTeamsJuggernaut;
                public sbyte MaxTeamsTerritories;
                public sbyte MaxTeamsAssault;
                public sbyte MaxTeamsStub10;
                public sbyte MaxTeamsStub11;
                public sbyte MaxTeamsStub12;
                public sbyte MaxTeamsStub13;
                public sbyte MaxTeamsStub14;
                public sbyte MaxTeamsStub15;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    Unlockable = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class AiScenarioMissionDialogueBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mdlg" })]
            public CachedTag MissionDialogue;
        }
        
        [TagStructure(Size = 0x18)]
        public class ScenarioInterpolatorBlock : TagStructure
        {
            public StringId Name;
            public StringId AcceleratorName; // Interpolator
            public StringId MultiplierName; // Interpolator
            public ScalarFunctionStructBlock Function;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            [TagField(Length = 0x2)]
            public byte[] Unknown1;
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class HsReferencesBlock1 : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioScreenEffectReferenceBlock : TagStructure
        {
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "egor" })]
            public CachedTag ScreenEffect;
            public StringId PrimaryInput; // Interpolator
            public StringId SecondaryInput; // Interpolator
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            [TagField(Length = 0x2)]
            public byte[] Unknown1;
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioSimulationDefinitionTableBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Unknown;
        }
    }
}

