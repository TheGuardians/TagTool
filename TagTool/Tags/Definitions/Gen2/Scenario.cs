using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x5C4)]
    public class Scenario : TagStructure
    {
        public CachedTag DoNotUse;
        public List<TagReference> Skies;
        public TypeValue Type;
        public FlagsValue Flags;
        public List<ScenarioChildScenarioReference> ChildScenarios;
        public Angle LocalNorth;
        public List<PredictedResource> PredictedResources;
        public List<ScenarioFunction> Functions;
        public byte[] EditorScenarioData;
        public List<EditorCommentDefinition> Comments;
        public List<ScenarioEnvironmentObject> Unknown1;
        public List<ScenarioObjectName> ObjectNames;
        public List<ScenarioScenery> Scenery;
        public List<ScenarioObjectPaletteEntry> SceneryPalette;
        public List<ScenarioBiped> Bipeds;
        public List<ScenarioObjectPaletteEntry> BipedPalette;
        public List<ScenarioVehicle> Vehicles;
        public List<ScenarioObjectPaletteEntry> VehiclePalette;
        public List<ScenarioEquipment> Equipment;
        public List<ScenarioObjectPaletteEntry> EquipmentPalette;
        public List<ScenarioWeapon> Weapons;
        public List<ScenarioObjectPaletteEntry> WeaponPalette;
        public List<ScenarioDeviceGroup> DeviceGroups;
        public List<ScenarioMachine> Machines;
        public List<ScenarioObjectPaletteEntry> MachinePalette;
        public List<ScenarioControl> Controls;
        public List<ScenarioObjectPaletteEntry> ControlPalette;
        public List<ScenarioLightFixture> LightFixtures;
        public List<ScenarioObjectPaletteEntry> LightFixturesPalette;
        public List<ScenarioSoundScenery> SoundScenery;
        public List<ScenarioObjectPaletteEntry> SoundSceneryPalette;
        public List<ScenarioLight> LightVolumes;
        public List<ScenarioObjectPaletteEntry> LightVolumesPalette;
        public List<ScenarioStartingProfile> PlayerStartingProfile;
        public List<ScenarioPlayer> PlayerStartingLocations;
        public List<ScenarioTriggerVolume> KillTriggerVolumes;
        public List<RecordedAnimationDefinition> RecordedAnimations;
        public List<ScenarioNetpoint> NetgameFlags;
        public List<ScenarioNetgameEquipment> NetgameEquipment;
        public List<ScenarioStartingEquipment> StartingEquipment;
        public List<ScenarioBspSwitchTriggerVolume> BspSwitchTriggerVolumes;
        public List<ScenarioDecal> Decals;
        public List<ScenarioDecalPaletteEntry> DecalsPalette;
        public List<ScenarioDetailObjectCollectionPaletteEntry> DetailObjectCollectionPalette;
        public List<StylePaletteEntry> StylePalette;
        public List<SquadGroupDefinition> SquadGroups;
        public List<SquadDefinition> Squads;
        public List<ZoneDefinition> Zones;
        public List<AiScene> MissionScenes;
        public List<CharacterPaletteEntry> CharacterPalette;
        public List<PathfindingData> AiPathfindingData;
        public List<AiAnimationReferenceDefinition> AiAnimationReferences;
        public List<AiScriptReferenceDefinition> AiScriptReferences;
        public List<AiRecordingReferenceDefinition> AiRecordingReferences;
        public List<AiConversation> AiConversations;
        public byte[] ScriptSyntaxData;
        public byte[] ScriptStringData;
        public List<HsScript> Scripts;
        public List<HsGlobalInternal> Globals;
        public List<HsTagReference> References;
        public List<HsSourceFile> SourceFiles;
        public List<CsScriptData> ScriptingData;
        public List<ScenarioCutsceneFlag> CutsceneFlags;
        public List<ScenarioCutsceneCameraPoint> CutsceneCameraPoints;
        public List<ScenarioCutsceneTitle> CutsceneTitles;
        public CachedTag CustomObjectNames;
        public CachedTag ChapterTitleText;
        public CachedTag HudMessages;
        public List<ScenarioStructureBspReference> StructureBsps;
        public List<ScenarioResourcesDefinition> ScenarioResources;
        public List<HereButForTheGraceOfGodGoThisPoorSoul> ScenarioResources1;
        public List<HsUnitSeatMapping> HsUnitSeats;
        public List<ScenarioKillTriggerVolume> ScenarioKillTriggers;
        public List<HsSyntaxNode> HsSyntaxDatums;
        public List<OrdersDefinition> Orders;
        public List<TriggerDefinition> Triggers;
        public List<StructureBackgroundSoundPaletteEntry> BackgroundSoundPalette;
        public List<StructureSoundEnvironmentPaletteEntry> SoundEnvironmentPalette;
        public List<StructureWeatherPaletteEntry> WeatherPalette;
        public List<GNullBlock> Unknown2;
        public List<GNullBlock> Unknown3;
        public List<GNullBlock> Unknown4;
        public List<GNullBlock> Unknown5;
        public List<GNullBlock> Unknown6;
        public List<ScenarioClusterDataBlock> ScenarioClusterData;
        public int Unknown7;
        [TagField(Length = 32)]
        public int ObjectSalts;
        public List<ScenarioSpawnData> SpawnData;
        public CachedTag SoundEffectCollection;
        public List<ScenarioCrate> Crates;
        public List<ScenarioObjectPaletteEntry> CratesPalette;
        /// <summary>
        /// Global Lighting Override
        /// </summary>
        public CachedTag GlobalLighting;
        /// <summary>
        /// WARNING
        /// </summary>
        /// <remarks>
        /// Editing Fog palette data will not behave as expected with split scenarios.
        /// </remarks>
        public List<ScenarioAtmosphericFogPaletteEntry> AtmosphericFogPalette;
        public List<ScenarioPlanarFogPaletteEntry> PlanarFogPalette;
        public List<FlockDefinition> Flocks;
        public CachedTag Subtitles;
        public List<DecoratorPlacementDefinition> Decorators;
        public List<ScenarioCreature> Creatures;
        public List<ScenarioObjectPaletteEntry> CreaturesPalette;
        public List<ScenarioDecoratorPaletteEntry> DecoratorsPalette;
        public List<ScenarioBspSwitchTransitionVolume> BspTransitionVolumes;
        public List<ScenarioStructureBspSphericalHarmonicLighting> StructureBspLighting;
        public List<ScenarioEditorFolder> EditorFolders;
        public List<ScenarioLevelData> LevelData;
        public CachedTag TerritoryLocationNames;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding1;
        public List<AiScenarioMissionDialogue> MissionDialogue;
        public CachedTag Objectives;
        public List<ScenarioInterpolator> Interpolators;
        public List<HsTagReference> SharedReferences;
        public List<ScenarioScreenEffectReference> ScreenEffectReferences;
        public List<ScenarioSimulationDefinitionTableElement> SimulationDefinitionTable;
        
        [TagStructure(Size = 0x10)]
        public class TagReference : TagStructure
        {
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
            CortanaHack = 1 << 0,
            AlwaysDrawSky = 1 << 1,
            DonTStripPathfinding = 1 << 2,
            SymmetricMultiplayerMap = 1 << 3,
            QuickLoadingCinematicOnlyScenario = 1 << 4,
            CharactersUsePreviousMissionWeapons = 1 << 5,
            LightmapsSmoothPalettesWithNeighbors = 1 << 6,
            SnapToWhiteAtStart = 1 << 7
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioChildScenarioReference : TagStructure
        {
            public CachedTag ChildScenario;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResource : TagStructure
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
        public class ScenarioFunction : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 32)]
            public string Name;
            public float Period; // Seconds
            public short ScalePeriodBy; // Multiply this function by above period
            public FunctionValue Function;
            public short ScaleFunctionBy; // Multiply this function by result of above function.
            public WobbleFunctionValue WobbleFunction; // Curve used for wobble.
            public float WobblePeriod; // Seconds
            public float WobbleMagnitude; // Percent
            public float SquareWaveThreshold; // If non-zero, all values above square wave threshold are snapped to 1.0, and all values below it are snapped to 0.0 to create a square wave.
            public short StepCount; // Number of discrete values to snap to (e.g., step count of 5 snaps function to 0.00, 0.25, 0.50,0.75, or 1.00).
            public MapToValue MapTo;
            public short SawtoothCount; // Number of times this function should repeat (e.g., sawtooth count of 5 gives function value of 1.0 at each of 0.25, 0.50, and 0.75, as well as at 1.0).
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short ScaleResultBy; // Multiply this function (e.g., from a weapon, vehicle) final result of all of the above math.
            public BoundsModeValue BoundsMode; // Controls how bounds, below, are used.
            public Bounds<float> Bounds;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            public short TurnOffWith; // If specified function is off, so is this function.
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding5;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Scripted = 1 << 0,
                Invert = 1 << 1,
                Additive = 1 << 2,
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
        public class EditorCommentDefinition : TagStructure
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
        public class ScenarioEnvironmentObject : TagStructure
        {
            public short Bsp;
            public short Unknown2;
            public int UniqueId;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public Tag ObjectDefinitionTag;
            public int Object;
            [TagField(Flags = Padding, Length = 44)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectName : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown1;
            public short Unknown2;
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioScenery : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            public ScenarioSceneryDatum SceneryData;
            
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
            
            [TagStructure(Size = 0x14)]
            public class ScenarioSceneryDatum : TagStructure
            {
                public PathfindingPolicyValue PathfindingPolicy;
                public LightmappingPolicyValue LightmappingPolicy;
                public List<PathfindingObjectIndexList> PathfindingReferences;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
                public class PathfindingObjectIndexList : TagStructure
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
        
        [TagStructure(Size = 0x30)]
        public class ScenarioObjectPaletteEntry : TagStructure
        {
            public CachedTag Name;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioBiped : TagStructure
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
        
        [TagStructure(Size = 0x38)]
        public class ScenarioEquipment : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioEquipmentDatum EquipmentData;
            
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
            
            [TagStructure(Size = 0x4)]
            public class ScenarioEquipmentDatum : TagStructure
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
        
        [TagStructure(Size = 0x54)]
        public class ScenarioWeapon : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            public ScenarioWeaponDatum WeaponData;
            
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
            public class ScenarioWeaponDatum : TagStructure
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
        public class ScenarioDeviceGroup : TagStructure
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
        
        [TagStructure(Size = 0x4C)]
        public class ScenarioMachine : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioMachineDatum MachineData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            
            [TagStructure(Size = 0x10)]
            public class ScenarioMachineDatum : TagStructure
            {
                public FlagsValue Flags;
                public List<PathfindingObjectIndexList> PathfindingReferences;
                
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
                public class PathfindingObjectIndexList : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioControl : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioControlDatum ControlData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            public class ScenarioControlDatum : TagStructure
            {
                public FlagsValue Flags;
                public short DonTTouchThis;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    UsableFromBothSides = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioLightFixture : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioLightFixtureDatum LightFixtureData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            public class ScenarioLightFixtureDatum : TagStructure
            {
                public RealRgbColor Color;
                public float Intensity;
                public Angle FalloffAngle; // Degrees
                public Angle CutoffAngle; // Degrees
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class ScenarioSoundScenery : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioSoundSceneryDatum SoundScenery;
            
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
            
            [TagStructure(Size = 0x1C)]
            public class ScenarioSoundSceneryDatum : TagStructure
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
        
        [TagStructure(Size = 0x6C)]
        public class ScenarioLight : TagStructure
        {
            /// <summary>
            /// ~Controls
            /// </summary>
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioLightDatum LightData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            public class ScenarioLightDatum : TagStructure
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
        
        [TagStructure(Size = 0x54)]
        public class ScenarioStartingProfile : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float StartingHealthDamage; // [0,1]
            public float StartingShieldDamage; // [0,1]
            public CachedTag PrimaryWeapon;
            public short RoundsLoaded;
            public short RoundsTotal;
            public CachedTag SecondaryWeapon;
            public short RoundsLoaded1;
            public short RoundsTotal2;
            public sbyte StartingFragmentationGrenadeCount;
            public sbyte StartingPlasmaGrenadeCount;
            public sbyte StartingUnknownGrenadeCount;
            public sbyte StartingUnknownGrenadeCount3;
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioPlayer : TagStructure
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
            public StringId Unknown2;
            public StringId Unknown3;
            public CampaignPlayerTypeValue CampaignPlayerType;
            [TagField(Flags = Padding, Length = 6)]
            public byte[] Padding1;
            
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
        
        [TagStructure(Size = 0x40)]
        public class RecordedAnimationDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            public short LengthOfAnimation; // ticks
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public byte[] RecordedAnimationEventStream;
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioNetpoint : TagStructure
        {
            public RealPoint3d Position;
            public Angle Facing; // Degrees
            public TypeValue Type;
            public TeamDesignatorValue TeamDesignator;
            public short Identifier;
            public FlagsValue Flags;
            public StringId Unknown2;
            public StringId Unknown3;
            
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
        
        [TagStructure(Size = 0x98)]
        public class ScenarioNetgameEquipment : TagStructure
        {
            public FlagsValue Flags;
            public GameType1Value GameType1;
            public GameType2Value GameType2;
            public GameType3Value GameType3;
            public GameType4Value GameType4;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short SpawnTimeInSeconds0Default;
            public short RespawnOnEmptyTime; // seconds
            public RespawnTimerStartsValue RespawnTimerStarts;
            public ClassificationValue Classification;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding3;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
            public CachedTag ItemVehicleCollection;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding4;
            
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
            public class RealEulerAngles3d : TagStructure
            {
                public RealEulerAngles3d Orientation;
            }
        }
        
        [TagStructure(Size = 0xCC)]
        public class ScenarioStartingEquipment : TagStructure
        {
            public FlagsValue Flags;
            public GameType1Value GameType1;
            public GameType2Value GameType2;
            public GameType3Value GameType3;
            public GameType4Value GameType4;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding1;
            public CachedTag ItemCollection1;
            public CachedTag ItemCollection2;
            public CachedTag ItemCollection3;
            public CachedTag ItemCollection4;
            public CachedTag ItemCollection5;
            public CachedTag ItemCollection6;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding2;
            
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
        public class ScenarioBspSwitchTriggerVolume : TagStructure
        {
            public short TriggerVolume;
            public short Source;
            public short Destination;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecal : TagStructure
        {
            public short DecalType;
            public sbyte Yaw127127;
            public sbyte Pitch127127;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalPaletteEntry : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioDetailObjectCollectionPaletteEntry : TagStructure
        {
            public CachedTag Name;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
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
        
        [TagStructure(Size = 0x10)]
        public class CharacterPaletteEntry : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x9C)]
        public class PathfindingData : TagStructure
        {
            public List<Sector> Sectors;
            public List<SectorLink> Links;
            public List<Bsp2dRef> Refs;
            public List<LargeBsp2dNode> Bsp2dNodes;
            public List<LongSurfaceFlags> SurfaceFlags;
            public List<SectorVertex> Vertices;
            public List<EnvironmentObjectReference> ObjectRefs;
            public List<PathfindingHintData> PathfindingHints;
            public List<InstancedGeometryReference> InstancedGeometryRefs;
            public int StructureChecksum;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
            public List<UserHintData> UserPlacedHints;
            
            [TagStructure(Size = 0x8)]
            public class Sector : TagStructure
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
            public class SectorLink : TagStructure
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
            public class Bsp2dRef : TagStructure
            {
                public int NodeRefOrSectorRef;
            }
            
            [TagStructure(Size = 0x14)]
            public class LargeBsp2dNode : TagStructure
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }
            
            [TagStructure(Size = 0x4)]
            public class LongSurfaceFlags : TagStructure
            {
                public int Flags;
            }
            
            [TagStructure(Size = 0xC)]
            public class SectorVertex : TagStructure
            {
                public RealPoint3d Point;
            }
            
            [TagStructure(Size = 0x24)]
            public class EnvironmentObjectReference : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public int FirstSector;
                public int LastSector;
                public List<EnvironmentObjectBspReference> Bsps;
                public List<EnvironmentObjectNodeReference> Nodes;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Mobile = 1 << 0
                }
                
                [TagStructure(Size = 0x10)]
                public class EnvironmentObjectBspReference : TagStructure
                {
                    public int BspReference;
                    public int FirstSector;
                    public int LastSector;
                    public short NodeIndex;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                }
                
                [TagStructure(Size = 0x4)]
                public class EnvironmentObjectNodeReference : TagStructure
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
            public class PathfindingHintData : TagStructure
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
            public class InstancedGeometryReference : TagStructure
            {
                public short PathfindingObjectIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x6C)]
            public class UserHintData : TagStructure
            {
                public List<UserHintPoint> PointGeometry;
                public List<UserHintRay> RayGeometry;
                public List<UserHintLineSegment> LineSegmentGeometry;
                public List<UserHintParallelogram> ParallelogramGeometry;
                public List<UserHintPolygon> PolygonGeometry;
                public List<UserHintJump> JumpHints;
                public List<UserHintClimb> ClimbHints;
                public List<UserHintWell> WellHints;
                public List<UserFlightHint> FlightHints;
                
                [TagStructure(Size = 0x10)]
                public class UserHintPoint : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintRay : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealVector3d Vector;
                }
                
                [TagStructure(Size = 0x24)]
                public class UserHintLineSegment : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class UserHintParallelogram : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    public RealPoint3d Point2;
                    public short ReferenceFrame2;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding3;
                    public RealPoint3d Point3;
                    public short ReferenceFrame3;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding4;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class UserHintPolygon : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintPoint> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class UserHintPoint : TagStructure
                    {
                        public RealPoint3d Point;
                        public short ReferenceFrame;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding1;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class UserHintJump : TagStructure
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
                public class UserHintClimb : TagStructure
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
                
                [TagStructure(Size = 0x10)]
                public class UserHintWell : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintWellPoint> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class UserHintWellPoint : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding1;
                        public RealVector3d Point;
                        public short ReferenceFrame;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding2;
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
                
                [TagStructure(Size = 0xC)]
                public class UserFlightHint : TagStructure
                {
                    public List<UserHintFlightPoint> Points;
                    
                    [TagStructure(Size = 0xC)]
                    public class UserHintFlightPoint : TagStructure
                    {
                        public RealVector3d Point;
                    }
                }
            }
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
        
        [TagStructure(Size = 0x28)]
        public class HsScript : TagStructure
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
        public class HsGlobalInternal : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x10)]
        public class HsTagReference : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x34)]
        public class HsSourceFile : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public byte[] Source;
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
        
        [TagStructure(Size = 0x38)]
        public class ScenarioCutsceneFlag : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioCutsceneCameraPoint : TagStructure
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
        public class ScenarioCutsceneTitle : TagStructure
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
        
        [TagStructure(Size = 0x24)]
        public class ScenarioResourcesDefinition : TagStructure
        {
            public List<ScenarioResourceReference> References;
            public List<HsSourceReference> ScriptSource;
            public List<AiResourceReference> AiResources;
            
            [TagStructure(Size = 0x10)]
            public class ScenarioResourceReference : TagStructure
            {
                public CachedTag Reference;
            }
            
            [TagStructure(Size = 0x10)]
            public class HsSourceReference : TagStructure
            {
                public CachedTag Reference;
            }
            
            [TagStructure(Size = 0x10)]
            public class AiResourceReference : TagStructure
            {
                public CachedTag Reference;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class HereButForTheGraceOfGodGoThisPoorSoul : TagStructure
        {
            public byte[] MoppCode;
            public List<ObjectIdentifier> EvironmentObjectIdentifiers;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public RealPoint3d MoppBoundsMin;
            public RealPoint3d MoppBoundsMax;
            
            [TagStructure(Size = 0x8)]
            public class ObjectIdentifier : TagStructure
            {
                public ObjectIdentifier ObjectId;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class HsUnitSeatMapping : TagStructure
        {
            public int Unknown1;
            public int Unknown2;
        }
        
        [TagStructure(Size = 0x2)]
        public class ScenarioKillTriggerVolume : TagStructure
        {
            public short TriggerVolume;
        }
        
        [TagStructure(Size = 0x14)]
        public class HsSyntaxNode : TagStructure
        {
            public short DatumHeader;
            public short ScriptIndexFunctionIndexConstantTypeUnion;
            public short Type;
            public short Flags;
            public int NextNodeIndex;
            public int Data;
            public int SourceOffset;
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
        
        [TagStructure(Size = 0x74)]
        public class StructureBackgroundSoundPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag BackgroundSound;
            public CachedTag InsideClusterSound; // Play only when player is inside cluster.
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding1;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class StructureSoundEnvironmentPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x98)]
        public class StructureWeatherPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag WeatherSystem;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding3;
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure()]
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure(Size = 0x50)]
        public class ScenarioClusterDataBlock : TagStructure
        {
            public CachedTag Bsp;
            public List<ScenarioClusterProperty> BackgroundSounds;
            public List<ScenarioClusterProperty> SoundEnvironments;
            public int BspChecksum;
            public List<RealPoint3d> ClusterCentroids;
            public List<ScenarioClusterProperty> WeatherProperties;
            public List<ScenarioClusterProperty> AtmosphericFogProperties;
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterProperty : TagStructure
            {
                public short Type;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Centroid;
            }
        }
        
        [TagStructure(Size = 0x6C)]
        public class ScenarioSpawnData : TagStructure
        {
            /// <summary>
            /// Dynamic Spawn
            /// </summary>
            /// <remarks>
            /// Non-0 values here overload what appears in multiplayer_globals.
            /// </remarks>
            public float DynamicSpawnLowerHeight;
            public float DynamicSpawnUpperHeight;
            public float GameObjectResetHeight;
            [TagField(Flags = Padding, Length = 60)]
            public byte[] Padding1;
            public List<DynamicSpawnZoneOverload> DynamicSpawnOverloads;
            public List<StaticSpawnZone> StaticRespawnZones;
            public List<StaticSpawnZone> StaticInitialSpawnZones;
            
            [TagStructure(Size = 0x10)]
            public class DynamicSpawnZoneOverload : TagStructure
            {
                public OverloadTypeValue OverloadType;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            public class StaticSpawnZone : TagStructure
            {
                /// <summary>
                /// Static Spawn Zones
                /// </summary>
                /// <remarks>
                /// Lower and upper heights can be left at 0, in which case they use defaults.  Leaving relevant teams empty means all teams; leaving all games empty means all games.
                /// </remarks>
                public StaticSpawnZoneData Data;
                public RealPoint3d Position;
                public float LowerHeight;
                public float UpperHeight;
                public float InnerRadius;
                public float OuterRadius;
                public float Weight;
                
                [TagStructure(Size = 0x10)]
                public class StaticSpawnZoneData : TagStructure
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
        public class ScenarioCrate : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            
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
        }
        
        [TagStructure(Size = 0x100)]
        public class ScenarioAtmosphericFogPaletteEntry : TagStructure
        {
            public StringId Name;
            /// <summary>
            /// ATMOSPHERIC FOG
            /// </summary>
            public RealRgbColor Color;
            public float SpreadDistance; // World Units
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float MaximumDensity; // [0,1]
            public float StartDistance; // World Units
            public float OpaqueDistance; // World Units
            /// <summary>
            /// SECONDARY FOG
            /// </summary>
            public RealRgbColor Color1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float MaximumDensity2; // [0,1]
            public float StartDistance3; // World Units
            public float OpaqueDistance4; // World Units
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            /// <summary>
            /// PLANAR FOG OVERRIDE
            /// </summary>
            /// <remarks>
            /// Planar fog, if present, is interpolated toward these values.
            /// </remarks>
            public RealRgbColor PlanarColor;
            public float PlanarMaxDensity; // [0,1]
            public float PlanarOverrideAmount; // [0,1]
            public float PlanarMinDistanceBias; // World Units
            [TagField(Flags = Padding, Length = 44)]
            public byte[] Padding4;
            /// <summary>
            /// PATCHY FOG
            /// </summary>
            public RealRgbColor PatchyColor;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding5;
            public Bounds<float> PatchyDensity; // [0,1]
            public Bounds<float> PatchyDistance; // World Units
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding6;
            public CachedTag PatchyFog;
            public List<ScenarioAtmosphericFogMixer> Mixers;
            /// <summary>
            /// BLOOM OVERRIDE
            /// </summary>
            public float Amount; // [0,1]
            public float Threshold; // [0,1]
            public float Brightness; // [0,1]
            public float GammaPower;
            /// <summary>
            /// CAMERA IMMERSION OVERRIDE
            /// </summary>
            public CameraImmersionFlagsValue CameraImmersionFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding7;
            
            [TagStructure(Size = 0x10)]
            public class ScenarioAtmosphericFogMixer : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public StringId AtmosphericFogSource; // From Scenario Atmospheric Fog Palette
                public StringId Interpolator; // From Scenario Interpolators
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown2;
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
        
        [TagStructure(Size = 0x18)]
        public class ScenarioPlanarFogPaletteEntry : TagStructure
        {
            public StringId Name;
            public CachedTag PlanarFog;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
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
        
        [TagStructure(Size = 0x40)]
        public class DecoratorPlacementDefinition : TagStructure
        {
            public RealPoint3d GridOrigin;
            public int CellCountPerDimension;
            public List<DecoratorCacheBlock> CacheBlocks;
            public List<DecoratorGroup> Groups;
            public List<DecoratorCellCollection> Cells;
            public List<DecoratorProjectedDecal> Decals;
            
            [TagStructure(Size = 0x3C)]
            public class DecoratorCacheBlock : TagStructure
            {
                public GeometryBlockInfoStruct GeometryBlockInfo;
                public List<DecoratorCacheBlockData> CacheBlockData;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x28)]
                public class GeometryBlockInfoStruct : TagStructure
                {
                    /// <summary>
                    /// BLOCK INFO
                    /// </summary>
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GeometryBlockResource> Resources;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding1;
                    public short OwnerTagSectionOffset;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding3;
                    
                    [TagStructure(Size = 0x10)]
                    public class GeometryBlockResource : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Flags = Padding, Length = 3)]
                        public byte[] Padding1;
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
                
                [TagStructure(Size = 0x9C)]
                public class DecoratorCacheBlockData : TagStructure
                {
                    public List<DecoratorPlacement> Placements;
                    public List<RasterizerVertexDecoratorDecal> DecalVertices;
                    public List<Word> DecalIndices;
                    public VertexBuffer DecalVertexBuffer;
                    [TagField(Flags = Padding, Length = 16)]
                    public byte[] Padding1;
                    public List<RasterizerVertexDecoratorSprite> SpriteVertices;
                    public List<Word> SpriteIndices;
                    public VertexBuffer SpriteVertexBuffer;
                    [TagField(Flags = Padding, Length = 16)]
                    public byte[] Padding2;
                    
                    [TagStructure(Size = 0x18)]
                    public class DecoratorPlacement : TagStructure
                    {
                        public int InternalData1;
                        public int CompressedPosition;
                        public ArgbColor TintColor;
                        public ArgbColor LightmapColor;
                        public int CompressedLightDirection;
                        public int CompressedLight2Direction;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class RasterizerVertexDecoratorDecal : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealPoint2d Texcoord0;
                        public RealPoint2d Texcoord1;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class Word : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class RasterizerVertexDecoratorSprite : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealVector3d Offset;
                        public RealVector3d Axis;
                        public RealPoint2d Texcoord;
                        public ArgbColor Color;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorGroup : TagStructure
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
            public class DecoratorCellCollection : TagStructure
            {
                public short ChildIndex;
                [TagField(Length = 8)]
                public short ChildIndices;
                public short CacheBlockIndex;
                public short GroupCount;
                public int GroupStartIndex;
            }
            
            [TagStructure(Size = 0x40)]
            public class DecoratorProjectedDecal : TagStructure
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
        public class ScenarioCreature : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            
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
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecoratorPaletteEntry : TagStructure
        {
            public CachedTag DecoratorSet;
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioBspSwitchTransitionVolume : TagStructure
        {
            public int BspIndexKey;
            public short TriggerVolume;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x1C)]
        public class ScenarioStructureBspSphericalHarmonicLighting : TagStructure
        {
            public CachedTag Bsp;
            public List<RealPoint3d> LightingPoints;
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class ScenarioEditorFolder : TagStructure
        {
            public int ParentFolder;
            [TagField(Length = 256)]
            public string Name;
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioLevelData : TagStructure
        {
            public CachedTag LevelDescription;
            public List<CampaignUiLevelDefinition> CampaignLevelData;
            public List<MultiplayerUiLevelDefinition> Multiplayer;
            
            [TagStructure(Size = 0xB58)]
            public class CampaignUiLevelDefinition : TagStructure
            {
                public int CampaignId;
                public int MapId;
                public CachedTag Bitmap;
                [TagField(Flags = Padding, Length = 576)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2304)]
                public byte[] Unknown2;
            }
            
            [TagStructure(Size = 0xC6C)]
            public class MultiplayerUiLevelDefinition : TagStructure
            {
                public int MapId;
                public CachedTag Bitmap;
                [TagField(Flags = Padding, Length = 576)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2304)]
                public byte[] Unknown2;
                [TagField(Length = 256)]
                public string Path;
                public int SortOrder;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
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
        
        [TagStructure(Size = 0x10)]
        public class AiScenarioMissionDialogue : TagStructure
        {
            public CachedTag MissionDialogue;
        }
        
        [TagStructure(Size = 0x1C)]
        public class ScenarioInterpolator : TagStructure
        {
            public StringId Name;
            public StringId AcceleratorName; // Interpolator
            public StringId MultiplierName; // Interpolator
            public FunctionDefinition Function;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown2;
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public FunctionDefinition Function;
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class ScenarioScreenEffectReference : TagStructure
        {
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public CachedTag ScreenEffect;
            public StringId PrimaryInput; // Interpolator
            public StringId SecondaryInput; // Interpolator
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown2;
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioSimulationDefinitionTableElement : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
        }
    }
}

