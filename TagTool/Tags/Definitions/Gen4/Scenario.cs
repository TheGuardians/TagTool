using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x98C)]
    public class Scenario : TagStructure
    {
        public List<ScenarioChildReferencesBlock> ChildScenarios;
        public ScenarioTypeEnum Type;
        public ScenarioFlags Flags;
        public ScenarioRuntimeTriggerVolumeFlags RuntimeTriggerVolumeFlags;
        public int CampaignId;
        public int MapId;
        // Used to associate external resources with this map - e.g. PDA camera setting block names.
        public StringId MapName;
        // Scenario-specific sound bank.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag ScenarioSoundBank;
        // Another scenario-specific sound bank. All will be loaded.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag ScenarioSoundBankNumber2;
        // Another scenario-specific sound bank. All will be loaded.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag ScenarioSoundBankNumber3;
        // Another scenario-specific sound bank. All will be loaded.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag ScenarioSoundBankNumber4;
        // This reverb will be used for inside areas when the listener is outside.
        public StringId InsideReverbName;
        public int InsideReverbHashId;
        public short SoundPermutationMissionId;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public int MinimumStructureBspImporterVersion;
        public Angle LocalNorth;
        // used to calculate aircraft altitude
        public float LocalSeaLevel; // wu
        public float AltitudeCap; // wu
        // forge coordinates are relative to this point
        public RealPoint3d SandboxOriginPoint;
        public float SandboxBudget;
        // when vehicle set is "map default," this vehicle set is used
        public StringId DefaultVehicleSet;
        [TagField(ValidTags = new [] { "gptd" })]
        public CachedTag GamePerformanceThrottles;
        public List<ScenarioStructureBspReferenceBlockStruct> StructureBsps;
        public List<ScenarioDesignReferenceBlock> StructureDesigns;
        [TagField(ValidTags = new [] { "stse" })]
        public CachedTag StructureSeams;
        [TagField(ValidTags = new [] { "stse" })]
        public CachedTag LocalStructureSeams;
        public List<ScenarioSkyReferenceBlock> Skies;
        public List<ScenarioZoneSetPvsBlock> ZoneSetPvs;
        public List<GameAudibilityBlock> ZoneSetAudibility;
        public List<ScenarioZoneSetBlock> ZoneSets;
        public List<ScenarioLightingZoneSetBlock> LightingZoneSets;
        public List<GNullBlock> PredictedResources;
        public List<ScenarioFunctionBlock> Functions;
        public byte[] EditorScenarioData;
        public List<EditorCommentBlock> Comments;
        public List<DontUseMeScenarioEnvironmentObjectBlock> UnusedScenarioEnvironmentObjects;
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
        public List<ScenarioTerminalBlock> Terminals;
        public List<ScenarioTerminalPaletteBlock> TerminalPalette;
        public List<ScenarioControlBlock> Controls;
        public List<ScenarioControlPaletteBlock> ControlPalette;
        public List<ScenarioDispenserBlock> Dispensers;
        public List<ScenarioDispenserPaletteBlock> DispenserPalette;
        public List<ScenarioSoundSceneryBlock> SoundScenery;
        public List<ScenarioSoundSceneryPaletteBlock> SoundSceneryPalette;
        public List<ScenarioGiantBlock> Giants;
        public List<ScenarioGiantPaletteBlock> GiantPalette;
        public List<ScenarioEffectSceneryBlock> EffectScenery;
        public List<ScenarioEffectSceneryPaletteBlock> EffectSceneryPalette;
        public List<ScenarioSpawnerBlock> Spawners;
        public List<ScenarioSpawnerPaletteBlock> SpawnerPalette;
        public List<BinkpaletteBlock> BinkPalette;
        public List<ScenarioattachedEffectsBlock> ScenarioAttachedEffects;
        public List<ScenarioattachedLensFlaresBlock> ScenarioAttachedLensFlares;
        public List<ScenarioattachedLightConesBlock> ScenarioAttachedLightCones;
        public List<MapVariantPaletteBlock> MapVariantPalettes;
        [TagField(ValidTags = new [] { "motl" })]
        public CachedTag MultiplayerObjectTypes;
        public MultiplayerMapSizeEnum MultiplayerMapSize;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // requisition for SvE, activated via an init.txt option for playtest balance
        public List<ScenarioRequisitionPaletteBlock> PlaytestReqPalette;
        public float PlayerRequisitionFrequency; // seconds
        public int InitialGameCurrency; // SpaceBucks
        public List<ScenarioSoftCeilingsBlock> SoftCeilings;
        public List<ScenarioProfilesBlock> PlayerStartingProfile;
        public List<ScenarioPlayersBlock> PlayerStartingLocations;
        public List<ScenarioTriggerVolumeStruct> TriggerVolumes;
        public List<ScenarioAcousticSectorBlockStruct> AcousticSectors;
        public List<ScenarioAcousticTransitionBlockStruct> AcousticTransitions;
        public List<ScenarioAtmosphereDumplingBlock> AtmosphereDumplings;
        public List<ScenarioWeatherDumplingBlock> WeatherDumplings;
        public List<RecordedAnimationBlock> RecordedAnimations;
        public List<ScenarioZoneSetSwitchTriggerVolumeBlock> ZoneSetSwitchTriggerVolumes;
        public List<ScenarioNamedLocationVolumeBlockStruct> NamedLocationVolumes;
        [TagField(ValidTags = new [] { "ssdf" })]
        public CachedTag SpawnSettings;
        public List<ScenarioDecalsBlock> Decals;
        public List<ScenarioDecalPaletteBlock> DecalPalette;
        // this is memory for the largest possible zoneset - default (0) is 2048
        public int LargestZonesetStaticDecalMemorySize; // kilobytes
        public byte[] StaticDecalMemoryData;
        // you should not need to do this -- this is for a mission with strange collision geo
        public float DecalDepthBiasOverride;
        public List<ScenarioDetailObjectCollectionPaletteBlock> DetailObjectCollectionPalette;
        public List<StylePaletteBlock> StylePallette;
        public List<SquadGroupsBlock> SquadGroups;
        public List<SquadsBlockStruct> Squads;
        public List<ZoneBlock> Zones;
        public List<SquadPatrolBlock> SquadPatrols;
        public List<AiCueBlockStruct> ActualCues;
        public List<AiFullCueBlockStruct> FullCues;
        public List<AiQuickCueBlockStruct> QuickCues;
        public List<AiSceneBlock> MissionScenes;
        public List<CharacterPaletteBlock> CharacterPalette;
        [TagField(ValidTags = new [] { "pfnd" })]
        public CachedTag AiPathfindingData;
        public List<UserHintBlock> AiUserHintData;
        public List<AiRecordingReferenceBlock> AiRecordingReferences;
        public HsScriptDataStruct ScriptData;
        public List<HsSourceReferenceBlock> ManualScriptFileReferences;
        [TagField(ValidTags = new [] { "hscn" })]
        public CachedTag CompiledGlobalScripts;
        public List<CsScriptDataBlock> ScriptingData;
        public List<ScenarioCutsceneFlagBlock> CutsceneFlags;
        public List<ScenarioCutsceneCameraPointBlock> CutsceneCameraPoints;
        public List<ScenarioCutsceneTitleStruct> CutsceneTitles;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag CustomObjectNames;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag ChapterTitleText;
        public List<ScenarioKillTriggerVolumesBlock> ScenarioKillTriggers;
        public List<ScenarioSafeZoneTriggerVolumesBlock> ScenarioSafeZoneTriggers;
        public List<TriggerVolumeMoppCodeBlock> ScenarioTriggerVolumesMoppCode;
        public List<ScenarioRequisitionTriggerVolumesBlock> ScenarioRequisitionTriggers;
        public List<ScenarioLocationNameTriggerVolumesBlock> ScenarioLocationNameTriggers;
        public List<ScenariounsafeSpawnZoneTriggerVolumesBlock> ScenarioUnsafeSpawnTriggerVolumes;
        public short ScenarioOrdnanceBoundsTriggerVolume;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<OrdersBlock> Orders;
        public List<TriggersBlock> Triggers;
        public List<ScenarioAcousticsPaletteBlockStruct> AcousticsPalette;
        public List<ScenarioAtmospherePaletteBlock> Atmosphere;
        public List<ScenarioCameraFxPaletteBlock> CameraFxPalette;
        public List<ScenarioWeatherPaletteBlock> WeatherPalette;
        public List<ScenarioClusterDataBlock> ScenarioClusterData;
        [TagField(Length = 32)]
        public ObjectSaltStorageArray[]  ObjectSalts;
        public List<ScenarioSpawnDataBlock> SpawnData;
        [TagField(ValidTags = new [] { "sfx+" })]
        public CachedTag SoundEffectCollection;
        public List<ScenarioCrateBlock> Crates;
        public List<ScenarioCratePaletteBlock> CratePalette;
        public List<FlockPaletteBlock> FlockPalette;
        public List<FlockInstanceBlock> Flocks;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag Subtitles;
        public List<SoundSubtitleBlock> Soundsubtitles;
        public List<ScenarioCreatureBlock> Creatures;
        public List<ScenarioCreaturePaletteBlock> CreaturePalette;
        public List<BigBattleCreaturePaletteBlock> BigBattleCreaturePalette;
        public List<GScenarioEditorFolderBlock> EditorFolders;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GameEngineStrings;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public List<AiScenarioMissionDialogueBlock> MissionDialogue;
        [TagField(ValidTags = new [] { "mmvo" })]
        public CachedTag Voiceover;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag Objectives;
        [TagField(ValidTags = new [] { "sirp" })]
        public CachedTag Interpolators;
        public List<HsReferencesBlock> SharedReferences;
        [TagField(ValidTags = new [] { "cfxs" })]
        public CachedTag CameraEffects;
        // ignores the falloff curves
        [TagField(ValidTags = new [] { "sefc" })]
        public CachedTag GlobalScreenEffect;
        [TagField(ValidTags = new [] { "ssao" })]
        public CachedTag GlobalSsao;
        // settings that apply to the entire scenario
        [TagField(ValidTags = new [] { "atgf" })]
        public CachedTag AtmosphereGlobals;
        [TagField(ValidTags = new [] { "sLdT" })]
        public CachedTag NewLightmaps;
        [TagField(ValidTags = new [] { "perf" })]
        public CachedTag PerformanceThrottles;
        public List<ObjectivesBlock> AiObjectives;
        public List<ScenarioDesignerZoneBlock> DesignerZones;
        public List<ScenarioZoneDebuggerBlockStruct> ZoneDebugger;
        public List<ScenarioDecoratorBlock> Decorators;
        public List<ScenarioCheapParticleSystemPaletteBlock> NeuticlePalette;
        public List<ScenarioCheapParticleSystemsBlock> Neuticles;
        public List<ScriptablelightRigBlock> ScriptableLightRigs;
        public List<ScenarioCinematicsBlock> Cinematics;
        public List<ScenarioCinematicLightingPaletteBlock> CinematicLightingPalette;
        public List<PlayerRepresentationBlock> OverridePlayerRepresentations;
        public List<CampaignMetagameScenarioBlock> CampaignMetagame;
        public List<SoftSurfacesDefinitionBlock> SoftSurfaces;
        public List<ScenarioCubemapBlock> Cubemaps;
        public List<ScenarioAirprobesBlock> Airprobes;
        public List<ScenarioBudgetReferencesBlock> BudgetReferences;
        public List<ModelReferencesBlock> ModelReferences;
        public List<ScenarioPerformancesBlockStruct> Thespian;
        public List<PuppetShowsBlock> Puppetshows;
        [TagField(ValidTags = new [] { "locs" })]
        public CachedTag LocationNameGlobals;
        public List<GarbageCollectionBlock> GarbageCollection;
        // appears for the player through the scenario
        [TagField(ValidTags = new [] { "cusc" })]
        public CachedTag HudScreenReference;
        [TagField(ValidTags = new [] { "sdzg" })]
        public CachedTag RequiredResources;
        [TagField(ValidTags = new [] { "vtgl" })]
        public CachedTag VariantGlobals;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag OrdnanceMapBitmap;
        public Bounds<float> OrdnanceMapDepthBounds;
        public OrdnanceFlags OrdnanceFlags1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        // if set, overrides that in progression globals
        [TagField(ValidTags = new [] { "scen" })]
        public CachedTag DropPod;
        public int OrdnanceDropCount;
        // zero means unlimited
        public int OrdnanceMaxActiveCount;
        public Bounds<float> TimeBetweenRandomDrops; // seconds
        [TagField(Length = 32)]
        public string InitialDropName;
        // from start of play til fanfare begins
        public float InitialDropDelay; // seconds
        public float InitialDropFanfareDuration; // seconds
        [TagField(Length = 32)]
        public string NormalDropName; // blank string will match all sets
        [TagField(Length = 32)]
        public string PlayerDropName;
        public float NavMarkerVisibilityProximity; // wu
        public float NavMarkerPremiumVisibilityProximity; // wu
        public List<ScenariorandomOrdnanceDropSetBlock> DropSets;
        [TagField(ValidTags = new [] { "scol" })]
        public CachedTag ScenarioOrdnanceList;
        public List<ScenarioUnitRecordingBlockStruct> UnitRecordings;
        // for non-mainmenu, we always use the first one
        public List<LoadscreenReferenceBlock> ExitLoadScreen;
        
        public enum ScenarioTypeEnum : short
        {
            Solo,
            Multiplayer,
            MainMenu,
            MultiplayerShared,
            SinglePlayerShared
        }
        
        [Flags]
        public enum ScenarioFlags : ushort
        {
            // always draw sky 0, even if no +sky polygons are visible
            AlwaysDrawSky = 1 << 0,
            // always leave pathfinding in, even for a multiplayer scenario
            DonTStripPathfinding = 1 << 1,
            SymmetricMultiplayerMap = 1 << 2,
            QuickLoading = 1 << 3,
            CharactersUsePreviousMissionWeapons = 1 << 4,
            LightmapsSmoothPalettesWithNeighbors = 1 << 5,
            SnapToWhiteAtStart = 1 << 6,
            OverrideGlobals = 1 << 7,
            BigVehicleUseCenterPointForLightSampling = 1 << 8,
            DonTUseCampaignSharing = 1 << 9,
            IgnoreSizeAndCanTShip = 1 << 10,
            AlwaysRunLightmapsPerBsp = 1 << 11,
            // so we can hide hud elements like the compass
            InSpace = 1 << 12,
            // so we can strip the elite from the global player representations
            Survival = 1 << 13,
            // so we can test the impact of variant stripping
            DoNotStripVariants = 1 << 14
        }
        
        [Flags]
        public enum ScenarioRuntimeTriggerVolumeFlags : uint
        {
            HasHardSafeVolume = 1 << 0,
            HasSoftSafeVolume = 1 << 1
        }
        
        public enum MultiplayerMapSizeEnum : sbyte
        {
            Small,
            Medium,
            Large
        }
        
        [Flags]
        public enum OrdnanceFlags : ushort
        {
            SuppressIncidentFanfareUi = 1 << 0
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioChildReferencesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag Tag;
        }
        
        [TagStructure(Size = 0x150)]
        public class ScenarioStructureBspReferenceBlockStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag StructureBsp;
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag LocalStructureBsp;
            [TagField(ValidTags = new [] { "smet" })]
            public CachedTag StructureMetadata;
            public ScenarioStructureSizeEnum SizeClass;
            public ScenarioStructureRefinementSizeEnum RefinementSizeClass;
            public float HackyAmbientMinLuminance;
            public float DirectDraftAmbientMinLuminance;
            // this is the most that we can sink a soft surface link snow in the structure_bsp via vertex painting.
            public float StructureVertexSink;
            public ScenarioStructureBspReferenceFlags Flags;
            public short DefaultSky;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BspSpecificCubemap;
            [TagField(ValidTags = new [] { "wind" })]
            public CachedTag Wind;
            [TagField(ValidTags = new [] { "aulp" })]
            public CachedTag AuthoredLightProbe;
            [TagField(ValidTags = new [] { "aulp" })]
            public CachedTag VehicleAuthoredLightProbe;
            // scale up or down the max number of shadows as set in the throttle tag per bsp
            public float MaxShadowCountScale;
            // 0.0 means allow fully dark in the shadows, higher values will brighten up the shadowed decorators
            public float DecoratorSunlightMinimum; // [0.0 to 1.0]
            public ScenariovolumetricLightShaftSettingsStruct VolumetricLightShaftSettings;
            public ScenariofloatingShadowSettingsStruct FloatingShadowSettings;
            public uint ClonedBspFlags;
            public ScenarioLightmapSettingStruct LightmapSetting;
            // 0==nogravity, 1==full, set the custom gravity scale flag to make this parameter active
            public float CustomGravityScale;
            
            public enum ScenarioStructureSizeEnum : int
            {
                _32x32,
                _64x64,
                _128x128,
                _256x25604Meg,
                _512x51215Meg,
                _768x76834Meg,
                _1024x10246Meg,
                _1280x128094Meg,
                _1536x1536135Meg,
                _1792x1792184meg
            }
            
            public enum ScenarioStructureRefinementSizeEnum : int
            {
                _40Meg,
                _10Meg,
                _20Meg,
                _60Meg
            }
            
            [Flags]
            public enum ScenarioStructureBspReferenceFlags : ushort
            {
                DefaultSkyEnabled = 1 << 0,
                PerVertexOnlyLightmap = 1 << 1,
                NeverLightmap = 1 << 2,
                GenerateFakeSmallLightmaps = 1 << 3,
                RayTraceAdjacentBspsOnSkyHits = 1 << 4,
                LightmapsUseConservativeSubcharts = 1 << 5,
                LightmapsReduceStretchHack = 1 << 6,
                LightmapsUseExtendedGathering = 1 << 7,
                LightmapsFinalGatherIgnoresBackfacingHits = 1 << 8,
                NotANormallyPlayableSpaceInAnMpMap = 1 << 9,
                SharedBsp = 1 << 10,
                DontUseExtraLightingBspsForCubemaps = 1 << 11,
                CustomGravityScale = 1 << 12,
                DisableStreamingSubregions = 1 << 13,
                DoNotDesaturateDecorators = 1 << 14,
                MakeAllShadowsBlob = 1 << 15
            }
            
            [TagStructure(Size = 0x24)]
            public class ScenariovolumetricLightShaftSettingsStruct : TagStructure
            {
                public ScenariovolumetricLightShaftSettingsFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d SourceDirection;
                public RealRgbColor ShaftColorTint;
                public float ExposureLevel;
                public float Decay;
                
                [Flags]
                public enum ScenariovolumetricLightShaftSettingsFlags : byte
                {
                    EnableVolumetricLightShaftsForThisBsp = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x68)]
            public class ScenariofloatingShadowSettingsStruct : TagStructure
            {
                public byte NumberOfCascades;
                public byte HasBeenInitialized;
                public FloatingshadowQualityDefinition Quality;
                public FloatingshadowBufferResolution Resolution;
                public float StaticShadowSharpening;
                [TagField(Length = 4)]
                public ScenariofloatingShadowCascadeSettingsArray[]  Frustums;
                
                public enum FloatingshadowQualityDefinition : sbyte
                {
                    _8Tap,
                    _12Tap,
                    _6Tap
                }
                
                public enum FloatingshadowBufferResolution : sbyte
                {
                    _512x512,
                    _800x800
                }
                
                [TagStructure(Size = 0x18)]
                public class ScenariofloatingShadowCascadeSettingsArray : TagStructure
                {
                    public float CascadeHalfWidth;
                    public float CascadeLength;
                    public float CascadeOffset;
                    public float Bias;
                    public float FilterWidth;
                    // if we want to slide the frustum up closer to the sun so that not as much of the frustum is below the ground
                    public float SunDirectionOffset;
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class ScenarioLightmapSettingStruct : TagStructure
            {
                public float LightmapResLowest;
                public float LightmapResSecondLow;
                public float LightmapResThirdLow;
                public float LightmapResMedium;
                public float LightmapResThirdHigh;
                public float LightmapResSecondHigh;
                public float LightmapResHighest;
                public ScenarioLightmapPerBspFlags LightmapFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float AnalyticalLightBounceModifier;
                public float NonAnalyticalLightBounceModifier;
                // neighbor bsp that occlude or contribute light (including bounce light)
                public uint ExtraLightingBspFlags;
                
                [Flags]
                public enum ScenarioLightmapPerBspFlags : byte
                {
                    AnalyticalBounceUsesPerBspSetting = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioDesignReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sddt" })]
            public CachedTag StructureDesign;
            [TagField(ValidTags = new [] { "sddt" })]
            public CachedTag LocalStructureDesign;
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioSkyReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag Sky;
            // mapping to the world unit
            public float CloudScale;
            // cloud movement speed
            public float CloudSpeed;
            // cloud movement direction, 0-360 degree
            public float CloudDirection;
            // red channel is used
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag CloudTexture;
            public short Name;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint ActiveOnBsps;
        }
        
        [TagStructure(Size = 0x2C)]
        public class ScenarioZoneSetPvsBlock : TagStructure
        {
            public uint StructureBspMask;
            public short Version;
            public ScenarioZoneSetPvsFlags Flags;
            public List<ScenarioZoneSetBspChecksumBlock> BspChecksums;
            public List<ScenarioZoneSetBspPvsBlock> StructureBspPvs;
            public List<StructurePortalDeviceMappingBlock> PortalDeviceMapping;
            
            [Flags]
            public enum ScenarioZoneSetPvsFlags : ushort
            {
                EmptyDebugPvs = 1 << 0
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioZoneSetBspChecksumBlock : TagStructure
            {
                public uint BspChecksum;
            }
            
            [TagStructure(Size = 0x24)]
            public class ScenarioZoneSetBspPvsBlock : TagStructure
            {
                public List<ScenarioZoneSetClusterPvsBlock> ClusterPvs;
                public List<ScenarioZoneSetClusterPvsBlock> ClusterPvsDoorsClosed;
                public List<ScenarioZoneSetBspSeamClusterMappingsBlock> BspClusterMapings;
                
                [TagStructure(Size = 0xC)]
                public class ScenarioZoneSetClusterPvsBlock : TagStructure
                {
                    public List<ScenarioZoneSetBspBitsBlock> ClusterPvsBitVectors;
                    
                    [TagStructure(Size = 0xC)]
                    public class ScenarioZoneSetBspBitsBlock : TagStructure
                    {
                        public List<ScenarioZoneSetClusterPvsBitVectorBlock> Bits;
                        
                        [TagStructure(Size = 0x4)]
                        public class ScenarioZoneSetClusterPvsBitVectorBlock : TagStructure
                        {
                            public uint Dword;
                        }
                    }
                }
                
                [TagStructure(Size = 0x24)]
                public class ScenarioZoneSetBspSeamClusterMappingsBlock : TagStructure
                {
                    public List<ScenarioZoneSetClusterReferenceBlock> RootClusters;
                    public List<ScenarioZoneSetClusterReferenceBlock> AttachedClusters;
                    public List<ScenarioZoneSetClusterReferenceBlock> ConnectedClusters;
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioZoneSetClusterReferenceBlock : TagStructure
                    {
                        public sbyte BspIndex;
                        public byte ClusterIndex;
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class StructurePortalDeviceMappingBlock : TagStructure
            {
                public List<StructureDevicePortalAssociationBlock> DevicePortalAssociations;
                public List<GamePortalToPortalMappingBlock> GamePortalToPortalMap;
                public List<OccludingPortalToPortalMappingBlock> OccludingPortalToPortalMap;
                
                [TagStructure(Size = 0xC)]
                public class StructureDevicePortalAssociationBlock : TagStructure
                {
                    public ScenarioObjectIdStruct DeviceId;
                    public short FirstGamePortalIndex;
                    public ushort GamePortalCount;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScenarioObjectIdStruct : TagStructure
                    {
                        public int UniqueId;
                        public short OriginBspIndex;
                        public ObjectTypeEnum Type;
                        public ObjectSourceEnum Source;
                        
                        public enum ObjectTypeEnum : sbyte
                        {
                            Biped,
                            Vehicle,
                            Weapon,
                            Equipment,
                            Terminal,
                            Projectile,
                            Scenery,
                            Machine,
                            Control,
                            Dispenser,
                            SoundScenery,
                            Crate,
                            Creature,
                            Giant,
                            EffectScenery,
                            Spawner
                        }
                        
                        public enum ObjectSourceEnum : sbyte
                        {
                            Structure,
                            Editor,
                            Dynamic,
                            Legacy,
                            Sky,
                            Parent
                        }
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class GamePortalToPortalMappingBlock : TagStructure
                {
                    public short PortalIndex;
                }
                
                [TagStructure(Size = 0x2)]
                public class OccludingPortalToPortalMappingBlock : TagStructure
                {
                    public short PortalIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x64)]
        public class GameAudibilityBlock : TagStructure
        {
            public int DoorPortalCount;
            public int RoomCount;
            public Bounds<float> RoomDistanceBounds;
            public List<DoorEncodedPasBlock> EncodedDoorPas;
            public List<RoomDoorPortalEncodedPasBlock> RoomDoorPortalEncodedPas;
            public List<AiDeafeningEncodedPasBlock> AiDeafeningPas;
            public List<EncodedRoomDistancesBlock> RoomDistances;
            public List<GamePortalToDoorOccluderBlock> GamePortalToDoorOccluderMapping;
            public List<BspClusterToRoomBounds> BspClusterToRoomBounds1;
            public List<BspClusterToRoomIndices> BspClusterToRoomIndices1;
            
            [TagStructure(Size = 0x4)]
            public class DoorEncodedPasBlock : TagStructure
            {
                public int EncodedData;
            }
            
            [TagStructure(Size = 0x4)]
            public class RoomDoorPortalEncodedPasBlock : TagStructure
            {
                public int EncodedData;
            }
            
            [TagStructure(Size = 0x4)]
            public class AiDeafeningEncodedPasBlock : TagStructure
            {
                public int EncodedData;
            }
            
            [TagStructure(Size = 0x1)]
            public class EncodedRoomDistancesBlock : TagStructure
            {
                public sbyte EncodedData;
            }
            
            [TagStructure(Size = 0x8)]
            public class GamePortalToDoorOccluderBlock : TagStructure
            {
                public int FirstDoorOccluderIndex;
                public int DoorOccluderCount;
            }
            
            [TagStructure(Size = 0x8)]
            public class BspClusterToRoomBounds : TagStructure
            {
                public int FirstRoomIndex;
                public int RoomIndexCount;
            }
            
            [TagStructure(Size = 0x2)]
            public class BspClusterToRoomIndices : TagStructure
            {
                public short RoomIndex;
            }
        }
        
        [TagStructure(Size = 0x1A0)]
        public class ScenarioZoneSetBlock : TagStructure
        {
            public StringId Name;
            [TagField(Length = 256)]
            public string NameString;
            public int PvsIndex;
            public ScenarioZoneSetFlags Flags;
            public uint BspZoneFlags;
            public uint StructureDesignZoneFlags;
            public uint RuntimeBspZoneFlags;
            public uint SruntimeTructureDesignZoneFlags;
            public uint RequiredDesignerZones;
            public ulong RuntimeDesignerZoneFlags;
            public uint CinematicZones;
            public int HintPreviousZoneSet;
            public int AudibilityIndex;
            public List<PlanarFogZoneSetVisibilityDefinitionBlock> PlanarFogVisibility;
            public List<ScenarioZoneSetBudgetOverrideBlock> BudgetOverrides;
            [TagField(ValidTags = new [] { "SDzs" })]
            public CachedTag StreamingReferenceTag;
            // Physics world will include this min point
            public RealPoint3d WorldBoundsMin;
            // Physics world will include this max point
            public RealPoint3d WorldBoundsMax;
            public List<ScenarioZoneSetLipsyncBlock> LipsyncSounds;
            // only for cinematics. If you try to use this for anything else without talking to me, i will stab you in the face
            [TagField(ValidTags = new [] { "sbnk" })]
            public CachedTag CinematicSoundbank;
            // linear color, must check override flag above to use
            public RealRgbColor SkyClearColor;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum ScenarioZoneSetFlags : uint
            {
                BeginLoadingNextLevel = 1 << 0,
                DebugPurposesOnly = 1 << 1,
                InteralZoneSet = 1 << 2,
                DisableSkyClearing = 1 << 3,
                OverrideSkyClearColor = 1 << 4
            }
            
            [TagStructure(Size = 0xC)]
            public class PlanarFogZoneSetVisibilityDefinitionBlock : TagStructure
            {
                public List<PlanarFogStructureVisibilityDefinitionBlock> StructureVisiblity;
                
                [TagStructure(Size = 0xC)]
                public class PlanarFogStructureVisibilityDefinitionBlock : TagStructure
                {
                    public List<PlanarFogClusterVisibilityDefinitionBlock> ClusterVisiblity;
                    
                    [TagStructure(Size = 0xC)]
                    public class PlanarFogClusterVisibilityDefinitionBlock : TagStructure
                    {
                        public List<PlanarFogReferenceDefinitionBlock> AttachedFogs;
                        
                        [TagStructure(Size = 0x4)]
                        public class PlanarFogReferenceDefinitionBlock : TagStructure
                        {
                            public short StructureDesignIndex;
                            public short FogIndex;
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioZoneSetBudgetOverrideBlock : TagStructure
            {
                public int EnvBitmap; // megs
                public int EnvObjectBitmap; // megs
                public int EnvGeometry; // megs
                public int EnvObjectGeometry; // megs
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioZoneSetLipsyncBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Dummy;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class ScenarioLightingZoneSetBlock : TagStructure
        {
            public StringId Name;
            public uint RenderedBspFlags;
            public uint ExtraBspFlags;
        }
        
        [TagStructure(Size = 0x0)]
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure(Size = 0x78)]
        public class ScenarioFunctionBlock : TagStructure
        {
            public ScenarioFunctionFlags Flags;
            [TagField(Length = 32)]
            public string Name;
            // this is the period for the above function (lower values make the function oscillate quickly, higher values make it
            // oscillate slowly)
            public float Period; // seconds
            // multiply this function by the above period
            public short ScalePeriodBy;
            public GlobalPeriodicFunctionsEnum Function;
            // multiply this function by the result of the above function
            public short ScaleFunctionBy;
            // the curve used for the wobble
            public GlobalPeriodicFunctionsEnum WobbleFunction;
            // the length of time it takes for the magnitude of this function to complete a wobble
            public float WobblePeriod; // seconds
            // the amount of random wobble in the magnitude
            public float WobbleMagnitude; // percent
            // if non-zero, all values above the square wave threshold are snapped to 1.0, and all values below it are snapped to
            // 0.0 to create a square wave.
            public float SquareWaveThreshold;
            // the number of discrete values to snap to (e.g., a step count of 5 would snap the function to 0.00,0.25,0.50,0.75 or
            // 1.00)
            public short StepCount;
            public GlobalTransitionFunctionsEnum MapTo;
            // the number of times this function should repeat (e.g., a sawtooth count of 5 would give the function a value of 1.0
            // at each of 0.25,0.50,0.75 as well as at 1.0
            public short SawtoothCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // multiply this function (from a weapon, vehicle, etc.) final result of all of the above math
            public short ScaleResultBy;
            // controls how the bounds, below, are used
            public FunctionBoundsModeEnum BoundsMode;
            public Bounds<float> Bounds;
            public float RuntimeInverseBoundsRange;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // if the specified function is off, so is this function
            public short TurnOffWith;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float RuntimeReciprocalSawtoothCount;
            public float RuntimeReciprocalBoundsRange;
            public float RuntimeReciprocalStepCount;
            public float RuntimeOneOverPeriod;
            
            [Flags]
            public enum ScenarioFunctionFlags : uint
            {
                // the level script will set this value; the other settings here will be ignored.
                Scripted = 1 << 0,
                // result of function is one minus actual result
                Invert = 1 << 1,
                Additive = 1 << 2,
                // function does not deactivate when at or below lower bound
                AlwaysActive = 1 << 3
            }
            
            public enum GlobalPeriodicFunctionsEnum : short
            {
                One,
                Zero,
                Cosine,
                Cosine1,
                DiagonalWave,
                DiagonalWave1,
                Slide,
                Slide1,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum GlobalTransitionFunctionsEnum : short
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
            
            public enum FunctionBoundsModeEnum : short
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
            public EditorCommentTypeEnum Type;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 256)]
            public string Comment;
            
            public enum EditorCommentTypeEnum : int
            {
                Generic
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class DontUseMeScenarioEnvironmentObjectBlock : TagStructure
        {
            public short Bsp;
            public short RuntimeObjectType;
            public int UniqueId;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Tag ObjectDefinitionTag;
            public int Object;
            [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioObjectNamesBlock : TagStructure
        {
            public StringId Name;
            public short ObjectType;
            public short ScenarioDatumIndex;
        }
        
        [TagStructure(Size = 0x180)]
        public class ScenarioSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioSceneryDatumStructV4 SceneryData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ScenarioSceneryDatumStructV4 : TagStructure
            {
                public PathfindingPolicyEnum PathfindingPolicy;
                public SceneryLightmapPolicyEnum LightmappingPolicy;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                public short HavokMoppIndex;
                public short AiSpawningSquad;
                
                public enum PathfindingPolicyEnum : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                public enum SceneryLightmapPolicyEnum : short
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
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x174)]
        public class ScenarioBipedBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioUnitStruct UnitData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioUnitStruct : TagStructure
            {
                public float BodyVitality; // [0,1]
                public ScenarioUnitDatumFlags Flags;
                
                [Flags]
                public enum ScenarioUnitDatumFlags : uint
                {
                    Dead = 1 << 0,
                    Opened = 1 << 1,
                    NotEnterableByPlayer = 1 << 2
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioBipedPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bipd" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x184)]
        public class ScenarioVehicleBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioUnitStruct UnitData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            public ScenarioVehicleDatumStruct VehicleData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioUnitStruct : TagStructure
            {
                public float BodyVitality; // [0,1]
                public ScenarioUnitDatumFlags Flags;
                
                [Flags]
                public enum ScenarioUnitDatumFlags : uint
                {
                    Dead = 1 << 0,
                    Opened = 1 << 1,
                    NotEnterableByPlayer = 1 << 2
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioVehicleDatumStruct : TagStructure
            {
                public PathfindingPolicyEnum PathfindingPolicy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                
                public enum PathfindingPolicyEnum : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioVehiclePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x158)]
        public class ScenarioEquipmentBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioEquipmentDatumStruct EquipmentData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioEquipmentDatumStruct : TagStructure
            {
                public ScenarioEquipmentFlags EquipmentFlags;
                
                [Flags]
                public enum ScenarioEquipmentFlags : uint
                {
                    Obsolete0 = 1 << 0,
                    Obsolete1 = 1 << 1,
                    DoesAccelerate = 1 << 2
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioEquipmentPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x174)]
        public class ScenarioWeaponBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioWeaponDatumStruct WeaponData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioWeaponDatumStruct : TagStructure
            {
                public short RoundsLeft;
                public short RoundsLoaded;
                public ScenarioWeaponDatumFlags Flags;
                
                [Flags]
                public enum ScenarioWeaponDatumFlags : uint
                {
                    InitiallyAtRest = 1 << 0,
                    Obsolete = 1 << 1,
                    DoesAccelerate = 1 << 2
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioWeaponPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x2C)]
        public class DeviceGroupBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float InitialValue; // [0,1]
            public DeviceGroupFlags Flags;
            public short EditorFolder;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum DeviceGroupFlags : uint
            {
                CanChangeOnlyOnce = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x188)]
        public class ScenarioMachineBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioDeviceStruct DeviceData;
            public ScenarioMachineStructV3 MachineData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStruct : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public ScenarioDeviceFlags Flags;
                
                [Flags]
                public enum ScenarioDeviceFlags : uint
                {
                    InitiallyOpen = 1 << 0,
                    InitiallyOff = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4,
                    ClosesWithoutPower = 1 << 5
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ScenarioMachineStructV3 : TagStructure
            {
                public ScenarioMachineFlags Flags;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                public ScenarioMachinePathfindingPolicyEnum PathfindingPolicy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ScenarioMachineFlags : uint
                {
                    DoesNotOperateAutomatically = 1 << 0,
                    OneSided = 1 << 1,
                    NeverAppearsLocked = 1 << 2,
                    OpenedByMeleeAttack = 1 << 3,
                    OneSidedForPlayer = 1 << 4,
                    DoesNotCloseAutomatically = 1 << 5,
                    IgnoresPlayer = 1 << 6,
                    IgnoresAi = 1 << 7
                }
                
                public enum ScenarioMachinePathfindingPolicyEnum : short
                {
                    Default,
                    Discs,
                    Sectors,
                    CutOut,
                    None
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioMachinePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mach" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0xC4)]
        public class ScenarioTerminalBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioDeviceStruct DeviceData;
            public ScenarioTerminalStruct TerminalData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStruct : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public ScenarioDeviceFlags Flags;
                
                [Flags]
                public enum ScenarioDeviceFlags : uint
                {
                    InitiallyOpen = 1 << 0,
                    InitiallyOff = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4,
                    ClosesWithoutPower = 1 << 5
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioTerminalStruct : TagStructure
            {
                public int PahPah;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioTerminalPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "term" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x180)]
        public class ScenarioControlBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioDeviceStruct DeviceData;
            public ScenarioControlStruct ControlData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStruct : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public ScenarioDeviceFlags Flags;
                
                [Flags]
                public enum ScenarioDeviceFlags : uint
                {
                    InitiallyOpen = 1 << 0,
                    InitiallyOff = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4,
                    ClosesWithoutPower = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class ScenarioControlStruct : TagStructure
            {
                public ScenarioControlFlags Flags;
                public short DonTTouchThis;
                // if this control is a health station, this sets the number of charges it contains.
                // Use 0 for infinite
                public short HealthStationCharges;
                public ScenarioControlCharacterTypes AllowedPlayers;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ScenarioControlFlags : uint
                {
                    UsableFromBothSides = 1 << 0
                }
                
                public enum ScenarioControlCharacterTypes : short
                {
                    Any,
                    Spartan,
                    Elite
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioControlPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ctrl" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x178)]
        public class ScenarioDispenserBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioDeviceStruct DeviceData;
            public ScenarioDispenserStruct DispenserData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStruct : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public ScenarioDeviceFlags Flags;
                
                [Flags]
                public enum ScenarioDeviceFlags : uint
                {
                    InitiallyOpen = 1 << 0,
                    InitiallyOff = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4,
                    ClosesWithoutPower = 1 << 5
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioDispenserStruct : TagStructure
            {
                public ScenarioDispenserFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ScenarioDispenserFlags : byte
                {
                    UsableFromFrontOnly = 1 << 0
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDispenserPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "dspn" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0xD4)]
        public class ScenarioSoundSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public SoundSceneryDatumStruct SoundScenery;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class SoundSceneryDatumStruct : TagStructure
            {
                public SoundVolumeTypeEnumeration VolumeType;
                public float Height;
                public Bounds<Angle> OverrideConeAngleBounds;
                public float OverrideOuterConeGain; // dB
                public SoundDistanceParametersStruct OverrideDistanceParameters;
                
                public enum SoundVolumeTypeEnumeration : int
                {
                    Sphere,
                    VerticalCylinder,
                    Pill
                }
                
                [TagStructure(Size = 0x20)]
                public class SoundDistanceParametersStruct : TagStructure
                {
                    // don't obstruct below this distance
                    public float DonTObstructDistance; // world units
                    // don't play below this distance
                    public float DonTPlayDistance; // world units
                    // start playing at full volume at this distance
                    public float AttackDistance; // world units
                    // start attenuating at this distance
                    public float MinimumDistance; // world units
                    // set attenuation to sustain db at this distance
                    public float SustainBeginDistance; // world units
                    // continue attenuating to silence at this distance
                    public float SustainEndDistance; // world units
                    // the distance beyond which this sound is no longer audible
                    public float MaximumDistance; // world units
                    // the amount of attenuation between sustain begin and end
                    public float SustainDb; // dB
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioSoundSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ssce" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0xD0)]
        public class ScenarioGiantBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioUnitStruct UnitData;
            public ScenarioGiantDatumStruct GiantData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioUnitStruct : TagStructure
            {
                public float BodyVitality; // [0,1]
                public ScenarioUnitDatumFlags Flags;
                
                [Flags]
                public enum ScenarioUnitDatumFlags : uint
                {
                    Dead = 1 << 0,
                    Opened = 1 << 1,
                    NotEnterableByPlayer = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioGiantDatumStruct : TagStructure
            {
                public PathfindingPolicyEnum PathfindingPolicy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                
                public enum PathfindingPolicyEnum : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioGiantPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "gint" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x158)]
        public class ScenarioEffectSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioEffectSceneryDatumStruct EffectSceneryData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioEffectSceneryDatumStruct : TagStructure
            {
                public float EffectSizeScale;
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioEffectSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "efsc" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0xC0)]
        public class ScenarioSpawnerBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioEntityStruct EntityData;
            public ScenarioSpawnerStruct SpawnerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioEntityStruct : TagStructure
            {
                public float EntityPlaceholder;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioSpawnerStruct : TagStructure
            {
                public float SpawnerPlaceholder;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioSpawnerPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "spnr" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x10)]
        public class BinkpaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bink" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioattachedEffectsBlock : TagStructure
        {
            public int CutsceneFlagIndex;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag EffectReference;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioattachedLensFlaresBlock : TagStructure
        {
            public int CutsceneFlagIndex;
            [TagField(ValidTags = new [] { "lens" })]
            public CachedTag LensFlareReference;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioattachedLightConesBlock : TagStructure
        {
            public int CutsceneFlagIndex;
            [TagField(ValidTags = new [] { "licn" })]
            public CachedTag LightConeReference;
            public RealArgbColor Color;
            public RealPoint2d Size;
            public float Intensity;
            [TagField(ValidTags = new [] { "crvs" })]
            public CachedTag IntensityCurve;
        }
        
        [TagStructure(Size = 0x14)]
        public class MapVariantPaletteBlock : TagStructure
        {
            public StringId Name;
            public MapVariantPaletteFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<MapVariantPaletteEntryBlock> Entries;
            
            [Flags]
            public enum MapVariantPaletteFlags : byte
            {
                // this palette is only visible in superforge, and objects within it are only editable in superforge
                Hidden = 1 << 0
            }
            
            [TagStructure(Size = 0x18)]
            public class MapVariantPaletteEntryBlock : TagStructure
            {
                public StringId Name;
                public List<MapVariantObjectVariantBlock> Variants;
                // if this is <= 0, these are 'unlimited' (up to a reasonable code-defined maximum)
                public int MaximumAllowed;
                public int PricePerInstance;
                
                [TagStructure(Size = 0x48)]
                public class MapVariantObjectVariantBlock : TagStructure
                {
                    public StringId DisplayName;
                    [TagField(ValidTags = new [] { "obje" })]
                    public CachedTag Object;
                    public StringId VariantName;
                    public MapvariantResourceManifest ResourceDependencies;
                    
                    [TagStructure(Size = 0x30)]
                    public class MapvariantResourceManifest : TagStructure
                    {
                        public List<MapvariantPaletteDependencyBlock> AttachedresourceOwners;
                        public List<MapvariantPaletteDependencyBlock> ToplevelResourceOwners;
                        public List<ResourcehandleBlock> Attachedresources;
                        public List<ResourcehandleBlock> Orphanedresources;
                        
                        [TagStructure(Size = 0x4)]
                        public class MapvariantPaletteDependencyBlock : TagStructure
                        {
                            public Tag Tag;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ResourcehandleBlock : TagStructure
                        {
                            public int Resourcehandle;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x6C)]
        public class ScenarioRequisitionPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "obje","vehi","scen","mach","capg" })]
            public CachedTag Name;
            [TagField(ValidTags = new [] { "obje","vehi","scen","mach","capg" })]
            public CachedTag SecondName;
            [TagField(ValidTags = new [] { "obje","vehi","scen","mach","capg" })]
            public CachedTag ThirdName;
            public StringId DisplayName;
            // controls which requisition submenu this object should appear in
            public RequisitionSubmenuGlobalEnum Submenu;
            // cant buy more if there are too many in play
            public int MaximumAllowed;
            public float PricePerInstance;
            public StringId ModelVariantName;
            public float BountyForDestruction;
            // 0=Bronze, 1=Silver, or 2=Gold
            public short MinFireteamTier;
            public byte AdditionalFragGrenades;
            public byte AdditionalPlasmaGrenades;
            public ScenarioRequisitionPalettePresence BuiltInPalettesForWhichItemIsEnabledByDefault;
            public RequisitionSpecialBuyEnum SpecialBuy;
            // 1..100 for ammoless weapons, 0 = default for all weapons
            public int StartingAmmo;
            // item will be unavailable until x seconds into the scenario
            public float WarmUpTime; // seconds
            public float PlayerPurchaseFrequency; // seconds
            public float TeamPurchaseFrequency; // seconds
            // price = original-price x increase-factor to-the n_times_bought
            public float PriceIncreaseFactor;
            public byte MaximumBuyCount; // per player
            public byte TotalMaximumBuyCount; // per team
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum RequisitionSubmenuGlobalEnum : int
            {
                Support
            }
            
            [Flags]
            public enum ScenarioRequisitionPalettePresence : uint
            {
                EmptyPalette = 1 << 0,
                FullPalette = 1 << 1,
                SpartanPalette = 1 << 2,
                ElitePalette = 1 << 3
            }
            
            public enum RequisitionSpecialBuyEnum : int
            {
                None,
                Airstrike,
                MacCannon,
                MagneticAmmo,
                LaserAmmo,
                ExplosiveAmmo,
                NormalAmmo,
                FriendlyAiLightInfantry,
                FriendlyAiHeavyInfantry,
                FriendlyAiLightVehicle,
                FriendlyAiHeavyVehicle,
                FriendlyAiFlyer
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class ScenarioSoftCeilingsBlock : TagStructure
        {
            public ScenarioSoftCeilingFlags Flags;
            public ScenarioSoftCeilingFlags RuntimeFlags;
            public StringId Name;
            public SoftCeilingTypeEnum Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum ScenarioSoftCeilingFlags : ushort
            {
                IgnoreBipeds = 1 << 0,
                IgnoreVehicles = 1 << 1,
                IgnoreCamera = 1 << 2,
                IgnoreHugeVehicles = 1 << 3
            }
            
            public enum SoftCeilingTypeEnum : short
            {
                Acceleration,
                SoftKill,
                SlipSurface
            }
        }
        
        [TagStructure(Size = 0x7C)]
        public class ScenarioProfilesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float StartingHealthDamage; // [0,1]
            public float StartingShieldDamage; // [0,1]
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag PrimaryWeapon;
            // -1 = weapon default
            public short PrimaryroundsLoaded;
            // -1 = weapon default
            public short PrimaryroundsTotal;
            // 0.0 = default, 1.0 = full
            public float PrimaryageRemaining;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag SecondaryWeapon;
            // -1 = weapon default
            public short SecondaryroundsLoaded;
            // -1 = weapon default
            public short SecondaryroundsTotal;
            // 0.0 = default, 1.0 = full
            public float SecondaryageRemaining;
            public sbyte StartingFragmentationGrenadeCount;
            public sbyte StartingPlasmaGrenadeCount;
            public sbyte StartingGrenade3Count;
            public sbyte StartingGrenade4Count;
            public sbyte StartingGrenade5Count;
            public sbyte StartingGrenade6Count;
            public sbyte StartingGrenade7Count;
            public sbyte StartingGrenade8Count;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag StartingEquipment;
            public StringId StartingTacticalPackage;
            public StringId StartingSupportUpgrade;
            public short EditorFolder;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioPlayersBlock : TagStructure
        {
            public RealPoint3d Position;
            public int PackedkeyOffaceref;
            public int NavmeshUidoffaceref;
            public Angle Facing; // degrees
            public Angle Pitch; // degrees
            public short InsertionPointIndex;
            public ScenarioPlayerFlags Flags;
            public short EditorFolder;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum ScenarioPlayerFlags : ushort
            {
                SurvivalMode = 1 << 0,
                SurvivalModeElite = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class ScenarioTriggerVolumeStruct : TagStructure
        {
            public StringId Name;
            public short ObjectName;
            public short RuntimeNodeIndex;
            public StringId NodeName;
            public TriggerVolumeTypeEnum Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealVector3d Forward;
            public RealVector3d Up;
            public RealPoint3d Position;
            public int PackedkeyOffaceref;
            public int NavmeshUidoffaceref;
            public RealPoint3d Extents;
            // this is only valid for sector type trigger volumes
            public float ZSink;
            public List<TriggerVolumePointBlock> SectorPoints;
            public List<TriggerVolumeRuntimeTrianglesBlock> RuntimeTriangles;
            public float RuntimeSectorBoundsX0;
            public float RuntimeSectorBoundsX1;
            public float RuntimeSectorBoundsY0;
            public float RuntimeSectorBoundsY1;
            public float RuntimeSectorBoundsZ0;
            public float RuntimeSectorBoundsZ1;
            public float C;
            public short KillTriggerVolume;
            public short EditorFolder;
            
            public enum TriggerVolumeTypeEnum : short
            {
                BoundingBox,
                Sector
            }
            
            [TagStructure(Size = 0x14)]
            public class TriggerVolumePointBlock : TagStructure
            {
                public RealPoint3d Position;
                public RealEulerAngles2d Normal;
            }
            
            [TagStructure(Size = 0x70)]
            public class TriggerVolumeRuntimeTrianglesBlock : TagStructure
            {
                public RealPlane3d Plane0;
                public RealPlane3d Plane1;
                public RealPlane3d Plane2;
                public RealPlane3d Plane3;
                public RealPlane3d Plane4;
                public RealPoint2d Vertex0;
                public RealPoint2d Vertex1;
                public RealPoint2d Vertex2;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioAcousticSectorBlockStruct : TagStructure
        {
            public List<AcousticSectorPointBlock> Points;
            public RealPlane3d TopPlane;
            public RealPlane3d BottomPlane;
            public AcousticpaletteFlags Flags;
            public float OcclusionValue;
            public short Acoustics;
            public short EditorFolder;
            public float Height;
            public float Sink;
            
            [Flags]
            public enum AcousticpaletteFlags : uint
            {
                OccludeIfAbove = 1 << 0,
                OccludeIfBelow = 1 << 1
            }
            
            [TagStructure(Size = 0xC)]
            public class AcousticSectorPointBlock : TagStructure
            {
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioAcousticTransitionBlockStruct : TagStructure
        {
            public RealPoint3d Center;
            public RealPoint3d Forward;
            public RealPoint3d Up;
            public float HalfWidth;
            public float HalfHeight;
            public float SamplePointOffset0;
            public float SamplePointOffset1;
            public short Sample0;
            public short Sample1;
            public ScenarioAcousticLocationDefinition Location0;
            public ScenarioAcousticLocationDefinition Location1;
            public short EditorFolder;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x4)]
            public class ScenarioAcousticLocationDefinition : TagStructure
            {
                public short SectorIndex;
                public ScenarioAcousticClusterReferenceDefinition ClusterReference;
                
                [TagStructure(Size = 0x2)]
                public class ScenarioAcousticClusterReferenceDefinition : TagStructure
                {
                    public sbyte BspIndex;
                    public byte ClusterIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class ScenarioAtmosphereDumplingBlock : TagStructure
        {
            public ScenarioDumplingStruct Dumpling;
            public short Atmosphere;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x38)]
            public class ScenarioDumplingStruct : TagStructure
            {
                public List<DumplingPointBlock> InnerPoints;
                public List<DumplingPointBlock> OuterPoints;
                public float Height;
                public float Sink;
                public float InnerValue;
                public float OuterValue;
                public RealPoint3d CenterPoint;
                public float TrivialCullRadiusSquared;
                
                [TagStructure(Size = 0x14)]
                public class DumplingPointBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public RealEulerAngles2d Normal;
                }
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class ScenarioWeatherDumplingBlock : TagStructure
        {
            public ScenarioDumplingStruct Dumpling;
            public short Weather;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x38)]
            public class ScenarioDumplingStruct : TagStructure
            {
                public List<DumplingPointBlock> InnerPoints;
                public List<DumplingPointBlock> OuterPoints;
                public float Height;
                public float Sink;
                public float InnerValue;
                public float OuterValue;
                public RealPoint3d CenterPoint;
                public float TrivialCullRadiusSquared;
                
                [TagStructure(Size = 0x14)]
                public class DumplingPointBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public RealEulerAngles2d Normal;
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
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
        
        [TagStructure(Size = 0x8)]
        public class ScenarioZoneSetSwitchTriggerVolumeBlock : TagStructure
        {
            public ScenarioZoneSetSwitchTriggerVolumeFlags Flags;
            public short BeginZoneSet;
            public short TriggerVolume;
            public short CommitZoneSet;
            
            [Flags]
            public enum ScenarioZoneSetSwitchTriggerVolumeFlags : ushort
            {
                TeleportVehicles = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class ScenarioNamedLocationVolumeBlockStruct : TagStructure
        {
            public List<NamedLocationVolumePointBlock> Points;
            public float Height;
            public float Sink;
            public StringId LocationName;
            
            [TagStructure(Size = 0x14)]
            public class NamedLocationVolumePointBlock : TagStructure
            {
                public RealPoint3d Position;
                public RealEulerAngles2d Normal;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class ScenarioDecalsBlock : TagStructure
        {
            public short DecalPaletteIndex;
            public DecalPlacementFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public ManualbspFlagsReferences ManualBspFlags;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float ScaleX;
            public float ScaleY;
            public float CullAngle;
            
            [Flags]
            public enum DecalPlacementFlags : byte
            {
                // force decal to be 2 triangle quad.  does not clip to geometry
                ForcePlanar = 1 << 0
            }
            
            [TagStructure(Size = 0x10)]
            public class ManualbspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;
                
                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sbsp" })]
                    public CachedTag StructureDesign;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioDecalPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "decs" })]
            public CachedTag Reference;
            public int MaxStaticBucketSize;
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioDetailObjectCollectionPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "dobc" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x10)]
        public class StylePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "styl" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x28)]
        public class SquadGroupsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Parent;
            public short InitialObjective;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short EditorFolder;
        }
        
        [TagStructure(Size = 0x6C)]
        public class SquadsBlockStruct : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public SquadFlags Flags;
            public AiTeamEnum Team;
            public short Parent;
            public short InitialZone;
            public short InitialObjective;
            public short InitialTask;
            public short EditorFolder;
            public List<SpawnFormationBlockStruct> SpawnFormations;
            public List<SpawnPointsBlockStruct> SpawnPoints;
            // Filter which squads in Firefight waves can be spawned into this squad
            public WavePlacementFilterEnum WavePlacementFilter;
            public StringId Template;
            public int SquadTemplateIndex;
            public SquadDefinitionInternalStruct Designer;
            public SquadDefinitionInternalStruct Templated;
            
            [Flags]
            public enum SquadFlags : uint
            {
                Unused = 1 << 0,
                Blind = 1 << 1,
                Deaf = 1 << 2,
                Braindead = 1 << 3,
                InitiallyPlaced = 1 << 4,
                UnitsNotEnterableByPlayer = 1 << 5,
                FireteamAbsorber = 1 << 6,
                SquadIsRuntime = 1 << 7,
                NoWaveSpawn = 1 << 8
            }
            
            public enum AiTeamEnum : short
            {
                Default,
                Player,
                Human,
                Covenant,
                Brute,
                Mule,
                Spare,
                CovenantPlayer,
                Forerunner
            }
            
            public enum WavePlacementFilterEnum : int
            {
                None,
                HeavyInfantry,
                BossInfantry,
                LightVehicle,
                HeavyVehicle,
                FlyingInfantry,
                FlyingVehicle,
                Bonus
            }
            
            [TagStructure(Size = 0x54)]
            public class SpawnFormationBlockStruct : TagStructure
            {
                public AiSpawnConditionsStruct PlaceOn;
                public StringId Name;
                public RealPoint3d Position;
                public int PackedkeyOffaceref;
                public int NavmeshUidoffaceref;
                public RealEulerAngles2d Facing; // degrees
                public float Roll;
                public StringId Formation;
                // before doing anything else, the actor will travel the given distance in its forward direction
                public float InitialMovementDistance;
                public ActorMovementModes InitialMovementMode;
                public short PlacementScriptIndex;
                public StringId PlacementScript;
                public StringId ActivityName;
                public StringId MovementSet;
                public short PointSet;
                public PatrolModeEnum PatrolMode;
                public List<PatrolPointBlock> Points;
                public SpawnFormationFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum ActorMovementModes : short
                {
                    Default,
                    Climbing,
                    Flying
                }
                
                public enum PatrolModeEnum : short
                {
                    PingPong,
                    Loop,
                    Random
                }
                
                [Flags]
                public enum SpawnFormationFlags : byte
                {
                    NoVerticalOffsetForFlying = 1 << 0
                }
                
                [TagStructure(Size = 0x4)]
                public class AiSpawnConditionsStruct : TagStructure
                {
                    public GlobalCampaignDifficultyEnum DifficultyFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum GlobalCampaignDifficultyEnum : ushort
                    {
                        Easy = 1 << 0,
                        Normal = 1 << 1,
                        Heroic = 1 << 2,
                        Legendary = 1 << 3
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class PatrolPointBlock : TagStructure
                {
                    public short Point;
                    public PatrolPointFlags Flags;
                    // how long the AI should pause at this point
                    public float Delay; // seconds
                    // the angle-from-forward that the AI can pick at this point
                    public float Angle; // degrees
                    public StringId ActivityName;
                    public GActivityEnum Activity;
                    public short ActivityVariant;
                    public StringId CommandScript;
                    public short CommandScriptIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PatrolPointFlags : ushort
                    {
                        SingleUse = 1 << 0
                    }
                    
                    public enum GActivityEnum : short
                    {
                        None,
                        Patrol,
                        Stand,
                        Crouch,
                        StandDrawn,
                        CrouchDrawn,
                        Combat,
                        Backup,
                        Guard,
                        GuardCrouch,
                        GuardWall,
                        Typing,
                        Kneel,
                        Gaze,
                        Poke,
                        Sniff,
                        Track,
                        Watch,
                        Examine,
                        Sleep,
                        AtEase,
                        Cower,
                        TaiChi,
                        Pee,
                        Doze,
                        Eat,
                        Medic,
                        Work,
                        Cheering,
                        Injured,
                        Captured
                    }
                }
            }
            
            [TagStructure(Size = 0x7C)]
            public class SpawnPointsBlockStruct : TagStructure
            {
                public AiSpawnConditionsStruct PlaceOn;
                public StringId Name;
                public short Cell;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealPoint3d Position;
                public int PackedkeyOffaceref;
                public int NavmeshUidoffaceref;
                public RealEulerAngles2d Facing; // degrees
                public float Roll;
                public StartingLocationFlags Flags;
                public short CharacterType;
                public short InitialWeapon;
                public short InitialSecondaryWeapon;
                public short InitialEquipment;
                public short VehicleType;
                public AiPlacementSeatPreferenceEnum SeatType;
                public GlobalAiGrenadeTypeEnum GrenadeType;
                // number of cretures in swarm if a swarm is spawned at this location
                public short SwarmCount;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId ActorVariantName;
                public StringId VehicleVariantName;
                public StringId VoiceDesignator;
                // before doing anything else, the actor will travel the given distance in its forward direction
                public float InitialMovementDistance;
                public ActorMovementModes InitialMovementMode;
                public short EmitterVehicle;
                public short GiantBody;
                public short BipedBody;
                public StringId PlacementScript;
                public short PlacementScriptIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId ActivityName;
                public StringId MovementSet;
                public short PointSet;
                public PatrolModeEnum PatrolMode;
                public List<PatrolPointBlock> Points;
                public short VehicleBody;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                
                [Flags]
                public enum StartingLocationFlags : ushort
                {
                    InfectionFormExplode = 1 << 0,
                    NA = 1 << 1,
                    AlwaysPlace = 1 << 2,
                    InitiallyHidden = 1 << 3,
                    VehicleDestroyedWhenNoDriver = 1 << 4,
                    VehicleOpen = 1 << 5,
                    ActorSurfaceEmerge = 1 << 6,
                    ActorSurfaceEmergeAuto = 1 << 7,
                    ActorSurfaceEmergeUpwards = 1 << 8
                }
                
                public enum AiPlacementSeatPreferenceEnum : short
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
                
                public enum GlobalAiGrenadeTypeEnum : short
                {
                    None,
                    HumanGrenade,
                    CovenantPlasma,
                    BruteClaymore,
                    Firebomb
                }
                
                public enum ActorMovementModes : short
                {
                    Default,
                    Climbing,
                    Flying
                }
                
                public enum PatrolModeEnum : short
                {
                    PingPong,
                    Loop,
                    Random
                }
                
                [TagStructure(Size = 0x4)]
                public class AiSpawnConditionsStruct : TagStructure
                {
                    public GlobalCampaignDifficultyEnum DifficultyFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum GlobalCampaignDifficultyEnum : ushort
                    {
                        Easy = 1 << 0,
                        Normal = 1 << 1,
                        Heroic = 1 << 2,
                        Legendary = 1 << 3
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class PatrolPointBlock : TagStructure
                {
                    public short Point;
                    public PatrolPointFlags Flags;
                    // how long the AI should pause at this point
                    public float Delay; // seconds
                    // the angle-from-forward that the AI can pick at this point
                    public float Angle; // degrees
                    public StringId ActivityName;
                    public GActivityEnum Activity;
                    public short ActivityVariant;
                    public StringId CommandScript;
                    public short CommandScriptIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PatrolPointFlags : ushort
                    {
                        SingleUse = 1 << 0
                    }
                    
                    public enum GActivityEnum : short
                    {
                        None,
                        Patrol,
                        Stand,
                        Crouch,
                        StandDrawn,
                        CrouchDrawn,
                        Combat,
                        Backup,
                        Guard,
                        GuardCrouch,
                        GuardWall,
                        Typing,
                        Kneel,
                        Gaze,
                        Poke,
                        Sniff,
                        Track,
                        Watch,
                        Examine,
                        Sleep,
                        AtEase,
                        Cower,
                        TaiChi,
                        Pee,
                        Doze,
                        Eat,
                        Medic,
                        Work,
                        Cheering,
                        Injured,
                        Captured
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class SquadDefinitionInternalStruct : TagStructure
            {
                public List<CellBlockStruct> Cells;
                
                [TagStructure(Size = 0x64)]
                public class CellBlockStruct : TagStructure
                {
                    public StringId Name;
                    public AiSpawnConditionsStruct PlaceOn;
                    // initial number of actors on normal difficulty
                    public short NormalDiffCount;
                    public MajorUpgradeEnum MajorUpgrade;
                    public List<CharacterPaletteChoiceBlockStruct> CharacterType;
                    public List<WeaponPaletteChoiceBlockStruct> InitialWeapon;
                    public List<WeaponPaletteChoiceBlockStruct> InitialSecondaryWeapon;
                    public List<EquipmentPaletteChoiceBlockStruct> InitialEquipment;
                    public GlobalAiGrenadeTypeEnum GrenadeType;
                    public short VehicleType;
                    public StringId VehicleVariant;
                    public StringId PlacementScript;
                    public short PlacementScriptIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId ActivityName;
                    public StringId MovementSet;
                    public short PointSet;
                    public PatrolModeEnum PatrolMode;
                    public List<PatrolPointBlock> Points;
                    
                    public enum MajorUpgradeEnum : short
                    {
                        Normal,
                        Few,
                        Many,
                        None,
                        All
                    }
                    
                    public enum GlobalAiGrenadeTypeEnum : short
                    {
                        None,
                        HumanGrenade,
                        CovenantPlasma,
                        BruteClaymore,
                        Firebomb
                    }
                    
                    public enum PatrolModeEnum : short
                    {
                        PingPong,
                        Loop,
                        Random
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class AiSpawnConditionsStruct : TagStructure
                    {
                        public GlobalCampaignDifficultyEnum DifficultyFlags;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum GlobalCampaignDifficultyEnum : ushort
                        {
                            Easy = 1 << 0,
                            Normal = 1 << 1,
                            Heroic = 1 << 2,
                            Legendary = 1 << 3
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class CharacterPaletteChoiceBlockStruct : TagStructure
                    {
                        public AiSpawnConditionsStruct PlaceOn;
                        public short CharacterType;
                        public short Chance;
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class WeaponPaletteChoiceBlockStruct : TagStructure
                    {
                        public AiSpawnConditionsStruct PlaceOn;
                        public short WeaponType;
                        public short Chance;
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class EquipmentPaletteChoiceBlockStruct : TagStructure
                    {
                        public AiSpawnConditionsStruct PlaceOn;
                        public short EquipmentType;
                        public short Chance;
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class PatrolPointBlock : TagStructure
                    {
                        public short Point;
                        public PatrolPointFlags Flags;
                        // how long the AI should pause at this point
                        public float Delay; // seconds
                        // the angle-from-forward that the AI can pick at this point
                        public float Angle; // degrees
                        public StringId ActivityName;
                        public GActivityEnum Activity;
                        public short ActivityVariant;
                        public StringId CommandScript;
                        public short CommandScriptIndex;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum PatrolPointFlags : ushort
                        {
                            SingleUse = 1 << 0
                        }
                        
                        public enum GActivityEnum : short
                        {
                            None,
                            Patrol,
                            Stand,
                            Crouch,
                            StandDrawn,
                            CrouchDrawn,
                            Combat,
                            Backup,
                            Guard,
                            GuardCrouch,
                            GuardWall,
                            Typing,
                            Kneel,
                            Gaze,
                            Poke,
                            Sniff,
                            Track,
                            Watch,
                            Examine,
                            Sleep,
                            AtEase,
                            Cower,
                            TaiChi,
                            Pee,
                            Doze,
                            Eat,
                            Medic,
                            Work,
                            Cheering,
                            Injured,
                            Captured
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class ZoneBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public ZoneFlags Flags;
            public short EditorFolderIndex;
            public List<FiringPositionsBlock> FiringPositions;
            public List<AreasBlockStruct> Areas;
            public NavMeshAttachmentsStruct NavMeshAttachments;
            public ManualbspFlagsReferences DisallowedAttachmentBsps;
            
            [Flags]
            public enum ZoneFlags : ushort
            {
                GiantsZone = 1 << 0
            }
            
            [TagStructure(Size = 0x30)]
            public class FiringPositionsBlock : TagStructure
            {
                public RealPoint3d Position;
                public int PackedkeyOffaceref;
                public int NavmeshUidoffaceref;
                public GFiringPositionFlags Flags;
                public GFiringPositionPostureFlags PostureFlags;
                public short Area;
                public short ClusterIndex;
                public short ClusterBsp;
                public sbyte BitsAndPad;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealEulerAngles2d Normal;
                public Angle Facing;
                public int LastabsoluteRejectionGameTime;
                
                [Flags]
                public enum GFiringPositionFlags : ushort
                {
                    Open = 1 << 0,
                    Partial = 1 << 1,
                    Closed = 1 << 2,
                    Mobile = 1 << 3,
                    WallLean = 1 << 4,
                    Perch = 1 << 5,
                    GroundPoint = 1 << 6,
                    DynamicCoverPoint = 1 << 7,
                    AutomaticallyGenerated = 1 << 8,
                    NavVolume = 1 << 9
                }
                
                [Flags]
                public enum GFiringPositionPostureFlags : ushort
                {
                    CornerLeft = 1 << 0,
                    CornerRight = 1 << 1,
                    Bunker = 1 << 2,
                    BunkerHigh = 1 << 3,
                    BunkerLow = 1 << 4
                }
            }
            
            [TagStructure(Size = 0xC0)]
            public class AreasBlockStruct : TagStructure
            {
                public int HkaivolumeVtable;
                public short Size;
                public short Count;
                [TagField(Length = 32)]
                public string Name;
                public AreaFlags AreaFlags1;
                public RealPoint3d RuntimeRelativeMeanPoint;
                public int PackedkeyOffaceref;
                public int NavmeshUidoffaceref;
                public float RuntimeStandardDeviation;
                public short RuntimeStartingIndex;
                public short RuntimeCount;
                public NavMeshAttachmentsStruct NavMeshAttachments;
                [TagField(Length = 8)]
                public AreaClusterOccupancyBitvectorArray[]  ClusterOccupancy;
                public List<FlightReferenceBlock> FlightHints;
                public List<AreaSectorPointBlock> Points;
                public GeneratePresetEnum Preset;
                public short RuntimecarverInversion;
                public GenerateFlags Flags;
                public float Extrusion;
                public float Sink;
                public Angle FiringPointOrientation;
                public Angle GridOrientation;
                public float NavVolumeCellSize;
                public float Spacing;
                public float AirborneSpacing;
                public float MinCoverLength;
                public float CoverSpacing;
                public float CoverOffsetDistance;
                public float TooCloseDistance;
                
                [Flags]
                public enum AreaFlags : uint
                {
                    VehicleArea = 1 << 0,
                    WallClimb = 1 << 1,
                    ManualReferenceFrame = 1 << 2,
                    TurretDeploymentArea = 1 << 3,
                    InvalidSectorDef = 1 << 4,
                    AirVolumeNavigation = 1 << 5,
                    GenerateWallClimbNavMesh = 1 << 6
                }
                
                public enum GeneratePresetEnum : short
                {
                    HighDensity,
                    MediumDensity,
                    LowDensity,
                    HighDensityVehicle,
                    MediumDensityVehicle,
                    Airborne,
                    Engineer,
                    Dogfight,
                    Dropship,
                    Orbital,
                    BishopHighDensity,
                    BishopMediumDensity,
                    BishopLowDensity
                }
                
                [Flags]
                public enum GenerateFlags : uint
                {
                    ExcludeCover = 1 << 0,
                    IgnoreExisting = 1 << 1,
                    GenerateRadial = 1 << 2,
                    DonTStagger = 1 << 3,
                    Airborne = 1 << 4,
                    AirborneStagger = 1 << 5,
                    ContinueCasting = 1 << 6
                }
                
                [TagStructure(Size = 0xC)]
                public class NavMeshAttachmentsStruct : TagStructure
                {
                    public List<NavMeshAttachmentBlock> Attachments;
                    
                    [TagStructure(Size = 0x4)]
                    public class NavMeshAttachmentBlock : TagStructure
                    {
                        public uint NavmeshUid;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class AreaClusterOccupancyBitvectorArray : TagStructure
                {
                    public int BitvectorData;
                }
                
                [TagStructure(Size = 0x8)]
                public class FlightReferenceBlock : TagStructure
                {
                    public short FlightHintIndex;
                    public short PointIndex;
                    public short StructureIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x1C)]
                public class AreaSectorPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class NavMeshAttachmentsStruct : TagStructure
            {
                public List<NavMeshAttachmentBlock> Attachments;
                
                [TagStructure(Size = 0x4)]
                public class NavMeshAttachmentBlock : TagStructure
                {
                    public uint NavmeshUid;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ManualbspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;
                
                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sbsp" })]
                    public CachedTag StructureDesign;
                }
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class SquadPatrolBlock : TagStructure
        {
            public StringId Name;
            public List<SquadPatrolMemberBlock> Squads;
            public List<SquadPatrolPointBlock> Points;
            public List<SquadPatrolTransitionBlock> Transitions;
            public short EditorFolder;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x4)]
            public class SquadPatrolMemberBlock : TagStructure
            {
                public short Squad;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x14)]
            public class SquadPatrolPointBlock : TagStructure
            {
                public short Objective;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // How long the AI should pause at this point before searching
                public float HoldTime; // seconds
                // How long the AI should search at this point before returning
                public float SearchTime; // seconds
                // How long the AI should pause at this point after searching before moving on
                public float PauseTime; // seconds
                // How long after being abandoned should this point be available again
                public float CooldownTime; // seconds
            }
            
            [TagStructure(Size = 0x10)]
            public class SquadPatrolTransitionBlock : TagStructure
            {
                public short Point1;
                public short Point2;
                public List<SquadPatrolWaypointBlock> Waypoints;
                
                [TagStructure(Size = 0x14)]
                public class SquadPatrolWaypointBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                }
            }
        }
        
        [TagStructure(Size = 0xA4)]
        public class AiCueBlockStruct : TagStructure
        {
            public StringId Name;
            public CueFlags Flags;
            public sbyte QuickCue;
            public short EditorFolder;
            public RealPoint3d Position;
            public int PackedkeyOffaceref;
            public int NavmeshUidoffaceref;
            public RealEulerAngles2d Facing; // degrees
            public float Roll;
            public CueDistributionStruct Distribution;
            public CuePayloadStruct Payload;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum CueFlags : byte
            {
                NotInitiallyPlaced = 1 << 0,
                PassiveStimulus = 1 << 1
            }
            
            [TagStructure(Size = 0x3C)]
            public class CueDistributionStruct : TagStructure
            {
                public List<TaskDistributionBlockStruct> Tasks;
                public CueStimulusDistributionStruct Distribution;
                
                [TagStructure(Size = 0x4)]
                public class TaskDistributionBlockStruct : TagStructure
                {
                    public short Objective;
                    public short Task;
                }
                
                [TagStructure(Size = 0x30)]
                public class CueStimulusDistributionStruct : TagStructure
                {
                    public List<RadialDistributionBlockStruct> Radius;
                    public List<ProbabilityDistributionBlockStruct> Probability;
                    public List<CharacterDistributionBlockStruct> Characters;
                    public List<WeaponDistributionBlockStruct> Weapons;
                    
                    [TagStructure(Size = 0x8)]
                    public class RadialDistributionBlockStruct : TagStructure
                    {
                        public float Radius;
                        public short TravelTime;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class ProbabilityDistributionBlockStruct : TagStructure
                    {
                        public float ChancePerSecond;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class CharacterDistributionBlockStruct : TagStructure
                    {
                        public short Character;
                        public DistributionCharacterFlags Flags;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum DistributionCharacterFlags : byte
                        {
                            DonTDistributeToChildren = 1 << 0
                        }
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class WeaponDistributionBlockStruct : TagStructure
                    {
                        public short Weapon;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class CuePayloadStruct : TagStructure
            {
                public List<FiringPointPayloadBlockStruct> FiringPoints;
                public List<ScriptPayloadBlockStruct> Script;
                public List<CombatSyncActionGroupPayloadBlockStruct> CombatSyncAction;
                public List<StimulusPayloadBlockStruct> Stimulus;
                public List<CombatCuePayloadBlockStruct> CombatCue;
                
                [TagStructure(Size = 0x4)]
                public class FiringPointPayloadBlockStruct : TagStructure
                {
                    public float Radius;
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptPayloadBlockStruct : TagStructure
                {
                    public StringId ScriptFunctionName;
                }
                
                [TagStructure(Size = 0x8)]
                public class CombatSyncActionGroupPayloadBlockStruct : TagStructure
                {
                    public StringId SyncActionGroupName;
                    // seconds
                    public float Cooldown;
                }
                
                [TagStructure(Size = 0x4)]
                public class StimulusPayloadBlockStruct : TagStructure
                {
                    public StringId StimulusType;
                }
                
                [TagStructure(Size = 0x34)]
                public class CombatCuePayloadBlockStruct : TagStructure
                {
                    public RealPoint3d Position;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public GFiringPositionFlags Flags;
                    public GFiringPositionPostureFlags PostureFlags;
                    public short Area;
                    public short ClusterIndex;
                    public short ClusterBsp;
                    public sbyte BitsAndPad;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealEulerAngles2d Normal;
                    public Angle Facing;
                    public int LastabsoluteRejectionGameTime;
                    public CombatCuePreferenceEnum Preference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum GFiringPositionFlags : ushort
                    {
                        Open = 1 << 0,
                        Partial = 1 << 1,
                        Closed = 1 << 2,
                        Mobile = 1 << 3,
                        WallLean = 1 << 4,
                        Perch = 1 << 5,
                        GroundPoint = 1 << 6,
                        DynamicCoverPoint = 1 << 7,
                        AutomaticallyGenerated = 1 << 8,
                        NavVolume = 1 << 9
                    }
                    
                    [Flags]
                    public enum GFiringPositionPostureFlags : ushort
                    {
                        CornerLeft = 1 << 0,
                        CornerRight = 1 << 1,
                        Bunker = 1 << 2,
                        BunkerHigh = 1 << 3,
                        BunkerLow = 1 << 4
                    }
                    
                    public enum CombatCuePreferenceEnum : short
                    {
                        Low,
                        High,
                        Total
                    }
                }
            }
        }
        
        [TagStructure(Size = 0xA4)]
        public class AiFullCueBlockStruct : TagStructure
        {
            public StringId Name;
            public CueFlags Flags;
            public sbyte QuickCue;
            public short EditorFolder;
            public RealPoint3d Position;
            public int PackedkeyOffaceref;
            public int NavmeshUidoffaceref;
            public RealEulerAngles2d Facing;
            public float Roll;
            public CueDistributionStruct Distribution;
            public CuePayloadStruct Payload;
            public int CueDefinitionIndex;
            
            [Flags]
            public enum CueFlags : byte
            {
                NotInitiallyPlaced = 1 << 0,
                PassiveStimulus = 1 << 1
            }
            
            [TagStructure(Size = 0x3C)]
            public class CueDistributionStruct : TagStructure
            {
                public List<TaskDistributionBlockStruct> Tasks;
                public CueStimulusDistributionStruct Distribution;
                
                [TagStructure(Size = 0x4)]
                public class TaskDistributionBlockStruct : TagStructure
                {
                    public short Objective;
                    public short Task;
                }
                
                [TagStructure(Size = 0x30)]
                public class CueStimulusDistributionStruct : TagStructure
                {
                    public List<RadialDistributionBlockStruct> Radius;
                    public List<ProbabilityDistributionBlockStruct> Probability;
                    public List<CharacterDistributionBlockStruct> Characters;
                    public List<WeaponDistributionBlockStruct> Weapons;
                    
                    [TagStructure(Size = 0x8)]
                    public class RadialDistributionBlockStruct : TagStructure
                    {
                        public float Radius;
                        public short TravelTime;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class ProbabilityDistributionBlockStruct : TagStructure
                    {
                        public float ChancePerSecond;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class CharacterDistributionBlockStruct : TagStructure
                    {
                        public short Character;
                        public DistributionCharacterFlags Flags;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum DistributionCharacterFlags : byte
                        {
                            DonTDistributeToChildren = 1 << 0
                        }
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class WeaponDistributionBlockStruct : TagStructure
                    {
                        public short Weapon;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class CuePayloadStruct : TagStructure
            {
                public List<FiringPointPayloadBlockStruct> FiringPoints;
                public List<ScriptPayloadBlockStruct> Script;
                public List<CombatSyncActionGroupPayloadBlockStruct> CombatSyncAction;
                public List<StimulusPayloadBlockStruct> Stimulus;
                public List<CombatCuePayloadBlockStruct> CombatCue;
                
                [TagStructure(Size = 0x4)]
                public class FiringPointPayloadBlockStruct : TagStructure
                {
                    public float Radius;
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptPayloadBlockStruct : TagStructure
                {
                    public StringId ScriptFunctionName;
                }
                
                [TagStructure(Size = 0x8)]
                public class CombatSyncActionGroupPayloadBlockStruct : TagStructure
                {
                    public StringId SyncActionGroupName;
                    // seconds
                    public float Cooldown;
                }
                
                [TagStructure(Size = 0x4)]
                public class StimulusPayloadBlockStruct : TagStructure
                {
                    public StringId StimulusType;
                }
                
                [TagStructure(Size = 0x34)]
                public class CombatCuePayloadBlockStruct : TagStructure
                {
                    public RealPoint3d Position;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public GFiringPositionFlags Flags;
                    public GFiringPositionPostureFlags PostureFlags;
                    public short Area;
                    public short ClusterIndex;
                    public short ClusterBsp;
                    public sbyte BitsAndPad;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealEulerAngles2d Normal;
                    public Angle Facing;
                    public int LastabsoluteRejectionGameTime;
                    public CombatCuePreferenceEnum Preference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum GFiringPositionFlags : ushort
                    {
                        Open = 1 << 0,
                        Partial = 1 << 1,
                        Closed = 1 << 2,
                        Mobile = 1 << 3,
                        WallLean = 1 << 4,
                        Perch = 1 << 5,
                        GroundPoint = 1 << 6,
                        DynamicCoverPoint = 1 << 7,
                        AutomaticallyGenerated = 1 << 8,
                        NavVolume = 1 << 9
                    }
                    
                    [Flags]
                    public enum GFiringPositionPostureFlags : ushort
                    {
                        CornerLeft = 1 << 0,
                        CornerRight = 1 << 1,
                        Bunker = 1 << 2,
                        BunkerHigh = 1 << 3,
                        BunkerLow = 1 << 4
                    }
                    
                    public enum CombatCuePreferenceEnum : short
                    {
                        Low,
                        High,
                        Total
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class AiQuickCueBlockStruct : TagStructure
        {
            public StringId Name;
            public QuickCueFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short EditorFolder;
            public RealPoint3d Position;
            public int PackedkeyOffaceref;
            public int NavmeshUidoffaceref;
            public RealEulerAngles2d Facing;
            public float Roll;
            public List<TaskDistributionBlockStruct> Tasks;
            public short Character;
            public short Weapon;
            public StringId Template;
            public int CueDefinitionIndex;
            
            [Flags]
            public enum QuickCueFlags : byte
            {
                DonTDistributeToChildren = 1 << 0
            }
            
            [TagStructure(Size = 0x4)]
            public class TaskDistributionBlockStruct : TagStructure
            {
                public short Objective;
                public short Task;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class AiSceneBlock : TagStructure
        {
            public StringId Name;
            public SceneFlags Flags;
            public List<AiSceneTriggerBlock> TriggerConditions;
            public List<AiSceneRoleBlock> Roles;
            
            [Flags]
            public enum SceneFlags : uint
            {
                SceneCanPlayMultipleTimes = 1 << 0,
                EnableCombatDialogue = 1 << 1
            }
            
            [TagStructure(Size = 0x10)]
            public class AiSceneTriggerBlock : TagStructure
            {
                public CombinationRulesEnum CombinationRule;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRulesEnum : short
                {
                    Or,
                    And
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerRefFlags TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerRefFlags : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class AiSceneRoleBlock : TagStructure
            {
                public StringId Name;
                public RoleGroupEnum Group;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<AiSceneRoleVariantsBlock> RoleVariants;
                
                public enum RoleGroupEnum : short
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
        
        [TagStructure(Size = 0x10)]
        public class CharacterPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x90)]
        public class UserHintBlock : TagStructure
        {
            public List<UserHintLineSegmentBlock> LineSegmentGeometry;
            public List<UserHintParallelogramBlock> ParallelogramGeometry;
            public List<UserHintJumpBlock> JumpHints;
            public List<UserHintClimbBlock> ClimbHints;
            public List<UserHintWellBlock> WellHints;
            public List<UserHintFlightBlock> FlightHints;
            public List<UserHintVolumeAvoidanceStruct> VolumeAvoidanceHints;
            public List<UserHintSplineBlock> SplineHints;
            public List<UserHintCookieCutterBlockStruct> CookieCutters;
            public List<UserHintNavmeshAreaBlockStruct> NavmeshAreas;
            public List<UserHintGiantBlock> GiantHints;
            public List<UserHintFloodBlock> FloodHints;
            
            [TagStructure(Size = 0x2C)]
            public class UserHintLineSegmentBlock : TagStructure
            {
                public UserHintGeometryFlags Flags;
                public RealPoint3d Point0;
                public int PackedkeyOffaceref0;
                public int NavmeshUidoffaceref0;
                public RealPoint3d Point1;
                public int PackedkeyOffaceref1;
                public int NavmeshUidoffaceref1;
                
                [Flags]
                public enum UserHintGeometryFlags : uint
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x58)]
            public class UserHintParallelogramBlock : TagStructure
            {
                public UserHintGeometryFlags Flags;
                public RealPoint3d Point0;
                public int PackedkeyOffaceref0;
                public int NavmeshUidoffaceref0;
                public RealPoint3d Point1;
                public int PackedkeyOffaceref1;
                public int NavmeshUidoffaceref1;
                public RealPoint3d Point2;
                public int PackedkeyOffaceref2;
                public int NavmeshUidoffaceref2;
                public RealPoint3d Point3;
                public int PackedkeyOffaceref3;
                public int NavmeshUidoffaceref3;
                public ParallelogramPointsInvalidFlags InvalidPoints;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum UserHintGeometryFlags : uint
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
                
                [Flags]
                public enum ParallelogramPointsInvalidFlags : ushort
                {
                    _1 = 1 << 0,
                    _2 = 1 << 1,
                    _3 = 1 << 2,
                    _4 = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class UserHintJumpBlock : TagStructure
            {
                public HintTypeEnum HintType;
                public short SquadGroupFilter;
                public List<HintVertexBlock> HintVertices;
                public int HintData0;
                public short HintData1;
                public byte HintData2;
                public byte Pad1;
                public UserHintGeometryFlags Flags;
                public short GeometryIndex;
                public GlobalAiJumpHeightEnum ForceJumpHeight;
                public JumpFlags ControlFlags;
                
                public enum HintTypeEnum : short
                {
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    TakeoffLink,
                    JumpMandatoryApproach
                }
                
                [Flags]
                public enum UserHintGeometryFlags : ushort
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
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
                public enum JumpFlags : ushort
                {
                    MagicLift = 1 << 0,
                    VehicleOnly = 1 << 1,
                    Railing = 1 << 2,
                    Vault = 1 << 3,
                    Down = 1 << 4,
                    Phase = 1 << 5,
                    StopAutodown = 1 << 6
                }
                
                [TagStructure(Size = 0xC)]
                public class HintVertexBlock : TagStructure
                {
                    public RealPoint3d Point;
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class UserHintClimbBlock : TagStructure
            {
                public HintTypeEnum HintType;
                public short SquadGroupFilter;
                public List<HintVertexBlock> HintVertices;
                public int HintData0;
                public short HintData1;
                public byte HintData2;
                public byte Pad1;
                public UserHintGeometryFlags Flags;
                public short GeometryIndex;
                public ForcedHoistHeightEnum ForceHoistHeight;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum HintTypeEnum : short
                {
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    TakeoffLink,
                    JumpMandatoryApproach
                }
                
                [Flags]
                public enum UserHintGeometryFlags : ushort
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
                
                public enum ForcedHoistHeightEnum : short
                {
                    None,
                    Step,
                    Crouch,
                    Stand
                }
                
                [TagStructure(Size = 0xC)]
                public class HintVertexBlock : TagStructure
                {
                    public RealPoint3d Point;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class UserHintWellBlock : TagStructure
            {
                public UserHintWellGeometryFlags Flags;
                public List<UserHintWellPointBlock> Points;
                
                [Flags]
                public enum UserHintWellGeometryFlags : uint
                {
                    Bidirectional = 1 << 0
                }
                
                [TagStructure(Size = 0x20)]
                public class UserHintWellPointBlock : TagStructure
                {
                    public UserHintWellPointTypeEnum Type;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                    
                    public enum UserHintWellPointTypeEnum : short
                    {
                        Jump,
                        Invalid,
                        Hoist
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UserHintFlightBlock : TagStructure
            {
                public List<UserHintFlightPointBlock> Points;
                
                [TagStructure(Size = 0xC)]
                public class UserHintFlightPointBlock : TagStructure
                {
                    public RealVector3d Point;
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class UserHintVolumeAvoidanceStruct : TagStructure
            {
                public UserHintAvoidanceVolumeEnum Type;
                public RealPoint3d Origin;
                public float Radius;
                // for pills
                public RealVector3d FacingVector;
                // for pills
                public float Height;
                public short Bsp;
                public short SplineCount;
                public short ZoneIndex;
                public short AreaIndex;
                
                public enum UserHintAvoidanceVolumeEnum : int
                {
                    Sphere,
                    Pill
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class UserHintSplineBlock : TagStructure
            {
                public StringId Name;
                public float Radius; // wus
                public float TimeBetweenPoints; // sec
                public List<UserHintSplineControlPointBlockStruct> ControlPoints;
                public short Bsp;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<UserHintSplineIntersectPointBlockStruct> VolumeIntersectPoints;
                
                [TagStructure(Size = 0x20)]
                public class UserHintSplineControlPointBlockStruct : TagStructure
                {
                    public UserHintSplineSegmentFlags Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public RealVector3d Tangent;
                    public float SegmentArcLength;
                    
                    [Flags]
                    public enum UserHintSplineSegmentFlags : ushort
                    {
                        NoAttachDetach = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSplineIntersectPointBlockStruct : TagStructure
                {
                    public short VolumeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public RealVector3d Tangent;
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class UserHintCookieCutterBlockStruct : TagStructure
            {
                public int HkaivolumeVtable;
                public short Size;
                public short Count;
                public List<UserHintSectorPointBlock> Points;
                public List<HintObjectIdBlock> PointsobjectIds;
                public float ZHeight;
                public float ZSink;
                public CookieCutterTypeEnum Type;
                public short Pad;
                public int RuntimeobjectTransformOverrideIndex;
                public sbyte Invalid;
                public sbyte Pad2;
                public sbyte Pad3;
                public sbyte Pad4;
                
                public enum CookieCutterTypeEnum : short
                {
                    CarveOut,
                    Preserve,
                    CarveAirVolume
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSectorPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                }
                
                [TagStructure(Size = 0x8)]
                public class HintObjectIdBlock : TagStructure
                {
                    public ScenarioObjectIdStruct ObjectId;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScenarioObjectIdStruct : TagStructure
                    {
                        public int UniqueId;
                        public short OriginBspIndex;
                        public ObjectTypeEnum Type;
                        public ObjectSourceEnum Source;
                        
                        public enum ObjectTypeEnum : sbyte
                        {
                            Biped,
                            Vehicle,
                            Weapon,
                            Equipment,
                            Terminal,
                            Projectile,
                            Scenery,
                            Machine,
                            Control,
                            Dispenser,
                            SoundScenery,
                            Crate,
                            Creature,
                            Giant,
                            EffectScenery,
                            Spawner
                        }
                        
                        public enum ObjectSourceEnum : sbyte
                        {
                            Structure,
                            Editor,
                            Dynamic,
                            Legacy,
                            Sky,
                            Parent
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x48)]
            public class UserHintNavmeshAreaBlockStruct : TagStructure
            {
                public int HkaivolumeVtable;
                public short Size;
                public short Count;
                public List<UserHintSectorPointBlock> Points;
                public float ZHeight;
                public float ZSink;
                public float StepHeight;
                public NavmeshAreaTypeEnum Type;
                public float Isvalid;
                public float MaxConvexBorderSimplifyArea;
                public float MaxBorderDistanceError;
                public float MaxConcaveBorderSimplifyArea;
                public float MaxWalkableSlope;
                public float CosineAngleMergeControl;
                public float HoleReplacementArea;
                public int PartitionSize;
                public float LoopShrinkFactor;
                
                public enum NavmeshAreaTypeEnum : int
                {
                    NavmeshLowRes,
                    Navmesh2,
                    Navmesh3,
                    Navmesh4,
                    Navmesh5,
                    Navmesh6,
                    Navmesh7,
                    Navmesh8,
                    NavmeshHighRes,
                    Custom
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSectorPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class UserHintGiantBlock : TagStructure
            {
                public List<UserHintGiantSectorBlock> GiantSectorHints;
                public List<UserHintGiantRailBlock> GiantRailHints;
                
                [TagStructure(Size = 0xC)]
                public class UserHintGiantSectorBlock : TagStructure
                {
                    public List<UserHintSectorPointBlock> Points;
                    
                    [TagStructure(Size = 0x1C)]
                    public class UserHintSectorPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public int PackedkeyOffaceref;
                        public int NavmeshUidoffaceref;
                        public RealEulerAngles2d Normal;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserHintGiantRailBlock : TagStructure
                {
                    public short GeometryIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UserHintFloodBlock : TagStructure
            {
                public List<UserHintFloodSectorBlock> FloodSectorHints;
                
                [TagStructure(Size = 0xC)]
                public class UserHintFloodSectorBlock : TagStructure
                {
                    public List<UserHintSectorPointBlock> Points;
                    
                    [TagStructure(Size = 0x1C)]
                    public class UserHintSectorPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public int PackedkeyOffaceref;
                        public int NavmeshUidoffaceref;
                        public RealEulerAngles2d Normal;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class AiRecordingReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string RecordingName;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class HsScriptDataStruct : TagStructure
        {
            public List<HsSourceReferenceBlock> SourceFileReferences;
            public List<HsSourceReferenceBlock> ExternalSourceReferences;
            [TagField(ValidTags = new [] { "hsdt" })]
            public CachedTag CompiledScript;
            
            [TagStructure(Size = 0x10)]
            public class HsSourceReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "hsc*" })]
                public CachedTag Reference;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class HsSourceReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "hsc*" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x90)]
        public class CsScriptDataBlock : TagStructure
        {
            public List<CsPointSetBlock> PointSets;
            public List<CsAnimationPointBlock> AnimationPoints;
            [TagField(Length = 0x78, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x3C)]
            public class CsPointSetBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public List<CsPointBlockStruct> Points;
                public short BspIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public PointSetFlags Flags;
                public PointSetTraversalFlags TraversalFlags;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [Flags]
                public enum PointSetFlags : uint
                {
                    ManualReferenceFrame = 1 << 0,
                    TurretDeployment = 1 << 1,
                    GiantSet = 1 << 2,
                    InvalidSectorRefs = 1 << 3
                }
                
                [Flags]
                public enum PointSetTraversalFlags : uint
                {
                    CurveTheTraversalPath = 1 << 0,
                    LoopWhenEndIsReached = 1 << 1
                }
                
                [TagStructure(Size = 0x40)]
                public class CsPointBlockStruct : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                    public StringId NameId;
                    public RealPoint3d Position;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d FacingDirection;
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class CsAnimationPointBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public int AnimatingObject;
                public int AnimatingObjectIndex;
                public StringId AnimationName;
                public int AnimationBoneToTrack;
                public RealPoint3d OffsetFromBone;
                public float AnimationTimeOffset;
                public float AssumedPlaybackRate;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioCutsceneFlagBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Name;
            public RealPoint3d Position;
            public RealEulerAngles3d Facing;
            public short EditorFolder;
            public short SourceBsp;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioCutsceneCameraPointBlock : TagStructure
        {
            public ScenarioCutsceneCameraFlags Flags;
            public ScenarioCutsceneCameraTypes Type;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
            public short ZoneSet;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum ScenarioCutsceneCameraFlags : ushort
            {
                EditAsRelative = 1 << 0
            }
            
            public enum ScenarioCutsceneCameraTypes : short
            {
                Normal,
                IgnoreTargetOrientation,
                Dolly,
                IgnoreTargetUpdates
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioCutsceneTitleStruct : TagStructure
        {
            public StringId Name;
            public Bounds<float> TextBoundsX;
            public Bounds<float> TextBoundsY;
            public TextJustificationEnum Justification;
            public TextVerticalJustificationEnum VerticalJustification;
            public GlobalFontIdEnum Font;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public ArgbColor TextColor;
            public ArgbColor ShadowColor;
            public float FadeInTimeSeconds;
            public float UpTimeSeconds;
            public float FadeOutTimeSeconds;
            public float LetterPrintTime; // seconds
            
            public enum TextJustificationEnum : short
            {
                Left,
                Right,
                Center
            }
            
            public enum TextVerticalJustificationEnum : short
            {
                Default,
                Top,
                Center,
                Bottom
            }
            
            public enum GlobalFontIdEnum : short
            {
                TerminalFont,
                Baksheesh15Font,
                Baksheesh16Font,
                Baksheesh17Font,
                Baksheesh18Font,
                Baksheesh20Font,
                Baksheesh21Font,
                Baksheesh22Font,
                Baksheesh28Font,
                Baksheesh36Font,
                Baksheesh38Font,
                BaksheeshBold16Font,
                BaksheeshBold20Font,
                BaksheeshBold21Font,
                BaksheeshBold23Font,
                BaksheeshBold24Font,
                BaksheeshThin36,
                BaksheeshThin42,
                ArameRegular16,
                ArameRegular18,
                ArameRegular23,
                ArameStencil16,
                ArameStencil18,
                ArameStencil23,
                ArameThin14,
                ArameThin16,
                ArameThin18,
                ArameThin23,
                ArameExtra01,
                ArameExtra02,
                ArameExtra03
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioKillTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
            public KillVolumeFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum KillVolumeFlags : byte
            {
                DonTKillImmediately = 1 << 0,
                OnlyKillPlayers = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioSafeZoneTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
            public KillVolumeFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum KillVolumeFlags : byte
            {
                DonTKillImmediately = 1 << 0,
                OnlyKillPlayers = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class TriggerVolumeMoppCodeBlock : TagStructure
        {
            public int TriggerVolumeChecksum;
            public List<MoppCodeDefinitionBlock> MoppCode;
            
            [TagStructure(Size = 0x40)]
            public class MoppCodeDefinitionBlock : TagStructure
            {
                public int FieldPointerSkip;
                public short Size;
                public short Count;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float VI;
                public float VJ;
                public float VK;
                public float VW;
                public int MDataPointer;
                public int IntMSize;
                public int IntMCapacityandFlags;
                public sbyte Int8MBuildtype;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public List<MoppCodeDataDefinitionBlock> MoppDataBlock;
                // they say it only matters for ps3
                public sbyte MoppBuildType;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x1)]
                public class MoppCodeDataDefinitionBlock : TagStructure
                {
                    public byte MoppData;
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioRequisitionTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
            public ScenarioRequisitionTriggerVolumeFlags Flags;
            
            [Flags]
            public enum ScenarioRequisitionTriggerVolumeFlags : ushort
            {
                DefenderCanBuy = 1 << 0,
                AttackerCanBuy = 1 << 1,
                CanBuyWeapons = 1 << 2,
                CanBuyEquipment = 1 << 3,
                CanBuyVehicles = 1 << 4,
                CanBuyCustomApps = 1 << 5
            }
        }
        
        [TagStructure(Size = 0x22)]
        public class ScenarioLocationNameTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
            [TagField(Length = 32)]
            public string Name;
        }
        
        [TagStructure(Size = 0x2)]
        public class ScenariounsafeSpawnZoneTriggerVolumesBlock : TagStructure
        {
            public short TriggerVolume;
        }
        
        [TagStructure(Size = 0x90)]
        public class OrdersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Style;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public OrderFlags Flags;
            public ForceCombatStatusEnum ForceCombatStatus;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string EntryScript;
            public short ScriptIndex;
            public short FollowSquad;
            public float FollowRadius;
            public List<AreaReferenceBlockStruct> PrimaryAreaSet;
            public List<AreaReferenceBlockStruct> SecondaryAreaSet;
            public List<SecondarySetTriggerBlock> SecondarySetTrigger;
            public List<SpecialMovementBlock> SpecialMovement;
            public List<OrderEndingBlock> OrderEndings;
            
            [Flags]
            public enum OrderFlags : uint
            {
                Locked = 1 << 0,
                AlwaysActive = 1 << 1,
                DebugOn = 1 << 2,
                StrictAreaDef = 1 << 3,
                FollowClosestPlayer = 1 << 4,
                FollowSquad = 1 << 5,
                SuppressActiveCamo = 1 << 6,
                SuppressCombatUntilEngaged = 1 << 7,
                InhibitVehicleUse = 1 << 8
            }
            
            public enum ForceCombatStatusEnum : short
            {
                None,
                Idle,
                Alert,
                Combat
            }
            
            [TagStructure(Size = 0x1C)]
            public class AreaReferenceBlockStruct : TagStructure
            {
                public ZoneSetTypeEnum AreaType;
                public ZoneSetFlags Flags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Zone;
                public short Area;
                public Angle Yaw;
                public int ConnectionFlags0;
                public int ConnectionFlags1;
                public int ConnectionFlags2;
                public int ConnectionFlags3;
                
                public enum ZoneSetTypeEnum : short
                {
                    Normal,
                    Search,
                    Core
                }
                
                [Flags]
                public enum ZoneSetFlags : byte
                {
                    Goal = 1 << 0,
                    DirectionValid = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class SecondarySetTriggerBlock : TagStructure
            {
                public CombinationRulesEnum CombinationRule;
                // when this ending is triggered, launch a dialogue event of the given type
                public OrderEndingDialogueEnum DialogueType;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRulesEnum : short
                {
                    Or,
                    And
                }
                
                public enum OrderEndingDialogueEnum : short
                {
                    None,
                    EnemyIsAdvancing,
                    EnemyIsCharging,
                    EnemyIsFallingBack,
                    Advance,
                    Charge,
                    FallBack,
                    MoveOn,
                    FollowPlayer,
                    ArrivingIntoCombat,
                    EndCombat,
                    Investigate,
                    SpreadOut,
                    HoldPosition,
                    FindCover,
                    CoveringFire
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerRefFlags TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerRefFlags : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class SpecialMovementBlock : TagStructure
            {
                public SpecialMovementFlags SpecialMovement1;
                
                [Flags]
                public enum SpecialMovementFlags : uint
                {
                    Jump = 1 << 0,
                    Climb = 1 << 1,
                    Vault = 1 << 2,
                    Mount = 1 << 3,
                    Hoist = 1 << 4,
                    WallJump = 1 << 5,
                    Takeoff = 1 << 6,
                    JumpMandatoryApproach = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class OrderEndingBlock : TagStructure
            {
                public short NextOrder;
                public CombinationRulesEnum CombinationRule;
                public float DelayTime;
                // when this ending is triggered, launch a dialogue event of the given type
                public OrderEndingDialogueEnum DialogueType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<TriggerReferences> Triggers;
                
                public enum CombinationRulesEnum : short
                {
                    Or,
                    And
                }
                
                public enum OrderEndingDialogueEnum : short
                {
                    None,
                    EnemyIsAdvancing,
                    EnemyIsCharging,
                    EnemyIsFallingBack,
                    Advance,
                    Charge,
                    FallBack,
                    MoveOn,
                    FollowPlayer,
                    ArrivingIntoCombat,
                    EndCombat,
                    Investigate,
                    SpreadOut,
                    HoldPosition,
                    FindCover,
                    CoveringFire
                }
                
                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerRefFlags TriggerFlags;
                    public short Trigger;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum TriggerRefFlags : uint
                    {
                        Not = 1 << 0
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class TriggersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TriggerFlags TriggerFlags1;
            public CombinationRulesEnum CombinationRule;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<OrderCompletionCondition> Conditions;
            
            [Flags]
            public enum TriggerFlags : uint
            {
                LatchOnWhenTriggered = 1 << 0
            }
            
            public enum CombinationRulesEnum : short
            {
                Or,
                And
            }
            
            [TagStructure(Size = 0x38)]
            public class OrderCompletionCondition : TagStructure
            {
                public ConditionTypeEnum RuleType;
                public short Squad;
                public short SquadGroup;
                public short A;
                public float X;
                public short TriggerVolume;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 32)]
                public string ExitConditionScript;
                public short ScriptIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public CompletionConditionFlags Flags;
                
                public enum ConditionTypeEnum : short
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
                public enum CompletionConditionFlags : uint
                {
                    Not = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x98)]
        public class ScenarioAcousticsPaletteBlockStruct : TagStructure
        {
            public StringId Name;
            public ScenarioAcousticsEnvironmentDefinition Reverb;
            public ScenarioAcousticsAmbienceDefinition Ambience;
            [TagField(ValidTags = new [] { "sbnk" })]
            public CachedTag SoundBankTag;
            [TagField(ValidTags = new [] { "sbnk" })]
            public CachedTag DvdOnlySoundBankTag;
            
            [TagStructure(Size = 0x1C)]
            public class ScenarioAcousticsEnvironmentDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "snde" })]
                public CachedTag SoundEnvironment;
                public SoundClassAcousticsStringDefinition Type;
                public float CutoffDistance;
                public float InterpolationTime; // seconds
                
                public enum SoundClassAcousticsStringDefinition : int
                {
                    Outside,
                    Inside
                }
            }
            
            [TagStructure(Size = 0x58)]
            public class ScenarioAcousticsAmbienceDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag BackgroundSound;
                // plays when rain is active, weather rate gets applied to scale.
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag WeatherSound;
                // plays when entering this area
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag EntrySound;
                // plays when leaving this area
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag ExitSound;
                public float CutoffDistance;
                public float InterpolationTime; // seconds
                public BackgroundSoundScaleFlags ScaleFlagsDepricated;
                public float InteriorScaleDepricated;
                public float PortalScaleDepricated;
                public float ExteriorScaleDepricated;
                
                [Flags]
                public enum BackgroundSoundScaleFlags : uint
                {
                    OverrideDefaultScale = 1 << 0,
                    UseAdjacentClusterAsPortalScale = 1 << 1,
                    UseAdjacentClusterAsExteriorScale = 1 << 2,
                    ScaleWithWeatherIntensity = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class ScenarioAtmospherePaletteBlock : TagStructure
        {
            public StringId Name;
            public ushort AtmosphereSettingIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "fogg" })]
            public CachedTag Atmosphere;
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioCameraFxPaletteBlock : TagStructure
        {
            public StringId Name;
            // if empty, uses default
            [TagField(ValidTags = new [] { "cfxs" })]
            public CachedTag ClusterCameraFxTag; // if empty, uses default
            public CameraFxPaletteFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the target exposure (ONLY USED WHEN FORCE EXPOSURE IS CHECKED)
            public float ForcedExposure; // stops
            // how bright you want the screen to be (ONLY USED WHEN FORCE AUTO EXPOSURE IS CHECKED)
            public float ForcedAutoExposureScreenBrightness; // [0.0001-1]
            public float ExposureMin; // stops
            public float ExposureMax; // stops
            public float InherentBloom;
            public float BloomIntensity;
            
            [Flags]
            public enum CameraFxPaletteFlags : byte
            {
                ForceExposure = 1 << 0,
                ForceAutoExposure = 1 << 1,
                OverrideExposureBounds = 1 << 2,
                OverrideInherentBloom = 1 << 3,
                OverrideBloomIntensity = 1 << 4
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioWeatherPaletteBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "rain" })]
            public CachedTag Rain;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioClusterDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag Bsp;
            public int BspChecksum;
            public List<ScenarioClusterPointsBlock> ClusterCentroids;
            public int DefaultAcousticPalette;
            public List<ScenarioClusterAcousticsBlockStruct> Acoustics;
            public List<ScenarioClusterAtmospherePropertiesBlock> AtmosphericProperties;
            public List<ScenarioClusterCameraFxPropertiesBlock> CameraFxProperties;
            public List<ScenarioClusterWeatherPropertiesBlock> WeatherProperties;
            
            [TagStructure(Size = 0xC)]
            public class ScenarioClusterPointsBlock : TagStructure
            {
                public RealPoint3d Centroid;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterAcousticsBlockStruct : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterAtmospherePropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterCameraFxPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterWeatherPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ObjectSaltStorageArray : TagStructure
        {
            public int Salt;
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioSpawnDataBlock : TagStructure
        {
            public float GameObjectResetHeight;
        }
        
        [TagStructure(Size = 0x17C)]
        public class ScenarioCrateBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            public ScenarioObjectPermutationStruct PermutationData;
            public ScenarioCrateDatumStruct CrateData;
            public ScenarioMultiplayerObjectStruct MultiplayerData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutationStruct : TagStructure
            {
                public StringId VariantName;
                public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ScenarioObjectActiveChangeColorFlags : byte
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioCrateDatumStruct : TagStructure
            {
                public PathfindingPolicyEnum PathfindingPolicy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PathfindingObjectIndexListBlock> PathfindingReferences;
                
                public enum PathfindingPolicyEnum : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexListBlock : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
            
            [TagStructure(Size = 0xB4)]
            public class ScenarioMultiplayerObjectStruct : TagStructure
            {
                [TagField(Length = 32)]
                public string MegaloLabel;
                [TagField(Length = 32)]
                public string MegaloLabel2;
                [TagField(Length = 32)]
                public string MegaloLabel3;
                [TagField(Length = 32)]
                public string MegaloLabel4;
                public GameEngineSymmetryPlacementFlags GameEngineSymmetricPlacement;
                public GlobalGameEngineTypeFlags GameEngineFlags;
                public GlobalMultiplayerTeamDesignatorEnum OwnerTeam;
                public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
                public sbyte QuotaMinimum;
                public sbyte QuotaMaximum; // <=0 for unlimited
                public MultiplayerObjectRemappingPolicy RemappingPolicy;
                public MultiplayerTeleporterChannel TeleporterChannel;
                public TeleporterPassabilityFlags TeleporterPassability;
                public sbyte SpawnOrder; // -1 for random
                public sbyte UserData2;
                public MultiplayerTeleporterChannel TraitZoneChannel;
                public float BoundaryWidthOrRadius;
                public float BoundaryBoxLength;
                public float BoundaryPositiveHeight;
                public float BoundaryNegativeHeight;
                public MultiplayerGoalAreaBoundaryShapeEnum BoundaryShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short SpawnTime; // seconds
                public short AbandonmentTime; // seconds
                public StringId LocationName;
                public ScenarioObjectParentStruct MapVariantParent;
                
                public enum GameEngineSymmetryPlacementFlags : sbyte
                {
                    Ignore,
                    Symmetric,
                    Asymmetric
                }
                
                [Flags]
                public enum GlobalGameEngineTypeFlags : byte
                {
                    None = 1 << 0,
                    Sandbox = 1 << 1,
                    Megalogamengine = 1 << 2,
                    Campaign = 1 << 3,
                    Survival = 1 << 4,
                    Firefight = 1 << 5
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [Flags]
                public enum MultiplayerObjectPlacementSpawnFlags : byte
                {
                    UniqueSpawn = 1 << 0,
                    NotInitiallyPlaced = 1 << 1,
                    HideUnlessMegaloRequired = 1 << 2,
                    IsShortcutObject = 1 << 3,
                    CanSpawnOnBipeds = 1 << 4,
                    SpawnerStartsInactive = 1 << 5
                }
                
                public enum MultiplayerObjectRemappingPolicy : sbyte
                {
                    NormalDefault,
                    DoNotReplace,
                    OnlyReplace
                }
                
                public enum MultiplayerTeleporterChannel : sbyte
                {
                    Alpha,
                    Bravo,
                    Charlie,
                    Delta,
                    Echo,
                    Foxtrot,
                    Golf,
                    Hotel,
                    India,
                    Juliet,
                    Kilo,
                    Lima,
                    Mike,
                    November,
                    Oscar,
                    Papa,
                    Quebec,
                    Romeo,
                    Sierra,
                    Tango,
                    Uniform,
                    Victor,
                    Whiskey,
                    Xray,
                    Yankee,
                    Zulu
                }
                
                [Flags]
                public enum TeleporterPassabilityFlags : byte
                {
                    DisallowPlayers = 1 << 0,
                    AllowLightLandVehicles = 1 << 1,
                    AllowHeavyLandVehicles = 1 << 2,
                    AllowFlyingVehicles = 1 << 3,
                    AllowProjectiles = 1 << 4
                }
                
                public enum MultiplayerGoalAreaBoundaryShapeEnum : sbyte
                {
                    Unused,
                    Sphere,
                    Cylinder,
                    Box
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioCratePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x10)]
        public class FlockPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "flck" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x54)]
        public class FlockInstanceBlock : TagStructure
        {
            public StringId FlockName;
            public short FlockDefinition;
            public short Bsp;
            public short BoundingVolume;
            public FlockFlags Flags;
            // distance from ecology boundary that creature begins to be repulsed
            public float EcologyMargin; // wus
            public List<FlockSourceBlock> Sources;
            public List<FlockDestinationBlock> Destinations;
            // How frequently boids are produced at one of the sources (limited by the max boid count)
            public Bounds<float> ProductionFrequencyBounds; // boids/sec
            public Bounds<float> Scale;
            // Distance from a source at which the creature scales to full size
            public float SourceScaleTo0Radius; // wus
            // Distance from a sink at which the creature begins to scale to zero
            public float SinkScaleTo0Radius; // wus
            // The number of seconds it takes to kill all units in the flock if it gets destroyed
            public float FlockDestroyDuration; // sec
            public short BoidCreature;
            public short BigBattleCreature;
            public Bounds<short> BoidCount;
            public short EnemyFlock;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float EnemyFlockMaxTargetDistance;
            
            [Flags]
            public enum FlockFlags : ushort
            {
                NotInitiallyCreated = 1 << 0,
                HardBoundariesOnVolume = 1 << 1,
                FlockInitiallyStopped = 1 << 2,
                FlockInitiallyPerched = 1 << 3,
                OneCreaturePerSource = 1 << 4,
                ScaredByAi = 1 << 5,
                CreaturesRespectKillVolumes = 1 << 6,
                BigBattleSquad = 1 << 7
            }
            
            [TagStructure(Size = 0x28)]
            public class FlockSourceBlock : TagStructure
            {
                public StringId Name;
                public FlockSourceFlags SourceFlags;
                public RealVector3d Position;
                public RealEulerAngles2d StartingYawPitch; // degrees
                public float Radius;
                // probability of producing at this source
                public float Weight;
                public sbyte BspIndex;
                public byte ClusterIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlockSourceFlags : uint
                {
                    GroundSource = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class FlockDestinationBlock : TagStructure
            {
                public StringId Name;
                public DestinationTypeEnum Type;
                public RealVector3d Position;
                public float Radius;
                // The farthest the boid will go inside our destination volume
                public float MaxDestinationVolumePenetration; // wu
                public short DestinationVolume;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum DestinationTypeEnum : int
                {
                    Sink,
                    Front,
                    Circle
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class SoundSubtitleBlock : TagStructure
        {
            public int Tagindex;
            public StringId Subtitlename;
        }
        
        [TagStructure(Size = 0xA0)]
        public class ScenarioCreatureBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStruct ObjectData;
            
            [TagStructure(Size = 0x9C)]
            public class ScenarioObjectDatumStruct : TagStructure
            {
                public ObjectLocationPlacementFlags PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public List<ScenarioObjectNodeOrientationsBlock> NodeOrientations;
                public float GravityOverride;
                public ObjectGravityFlags GravityFlags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScenarioObjectBspPlacementPolicyDefinition BspPolicy;
                public ScenarioobjectScriptFlags ScriptFlags;
                public List<ScriptlistBlock> ForceEnabledScripts;
                public List<ScriptlistBlock> DisabledScripts;
                public ManualbspFlagsReferences ManualBspFlags;
                public ObjectTransformFlags TransformFlags;
                public NavMeshCuttingOverrideEnum NavMeshCutting;
                public BooleanOverrideEnum NavMeshObstacle;
                public ObjectNavmeshFlags NavMeshFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId LightAirprobeName;
                public ScenarioObjectIdStruct ObjectId;
                public ChanneldefinitionFlags LightChannels;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public ScenarioObjectParentStruct ParentId;
                public uint CanAttachToBspFlags;
                // Multiplier applied to all phantoms' direction acceleration factors.  Used to scale man-cannon strength.
                public float DirectionalAccelerationMult;
                public List<CommandlinkBlock> CommandLinks;
                
                [Flags]
                public enum ObjectLocationPlacementFlags : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused0 = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8,
                    StoreOrientations = 1 << 9,
                    PvsBound = 1 << 10,
                    Startup = 1 << 11,
                    AttachPhysically = 1 << 12,
                    AttachWithScale = 1 << 13,
                    NoParentLighting = 1 << 14
                }
                
                [Flags]
                public enum ObjectGravityFlags : byte
                {
                    ApplyOverride = 1 << 0,
                    ApplyToChildrenAlso = 1 << 1
                }
                
                public enum ScenarioObjectBspPlacementPolicyDefinition : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
                
                [Flags]
                public enum ScenarioobjectScriptFlags : byte
                {
                    ScriptsDisabled = 1 << 0,
                    UseOverrideLists = 1 << 1,
                    ScriptsAlwaysRun = 1 << 2
                }
                
                [Flags]
                public enum ObjectTransformFlags : ushort
                {
                    Mirrored = 1 << 0
                }
                
                public enum NavMeshCuttingOverrideEnum : sbyte
                {
                    Default,
                    Cut,
                    NotCut
                }
                
                public enum BooleanOverrideEnum : sbyte
                {
                    Default,
                    Yes,
                    No
                }
                
                [Flags]
                public enum ObjectNavmeshFlags : byte
                {
                    ChildrenInheritNavmeshInteraction = 1 << 0,
                    NavmeshAlwaysLoaded = 1 << 1
                }
                
                [Flags]
                public enum ChanneldefinitionFlags : uint
                {
                    _0 = 1 << 0,
                    _1 = 1 << 1,
                    _2 = 1 << 2,
                    _3 = 1 << 3,
                    _4 = 1 << 4,
                    _5 = 1 << 5,
                    _6 = 1 << 6,
                    _7 = 1 << 7,
                    _8 = 1 << 8,
                    _9 = 1 << 9,
                    _10 = 1 << 10,
                    _11 = 1 << 11,
                    _12 = 1 << 12,
                    _13 = 1 << 13,
                    _14 = 1 << 14,
                    _15 = 1 << 15,
                    _16 = 1 << 16,
                    _17 = 1 << 17,
                    _18 = 1 << 18,
                    _19 = 1 << 19,
                    _20 = 1 << 20,
                    _21 = 1 << 21,
                    _22 = 1 << 22,
                    _23 = 1 << 23,
                    _24 = 1 << 24,
                    _25 = 1 << 25,
                    _26 = 1 << 26,
                    _27 = 1 << 27,
                    _28 = 1 << 28,
                    _29 = 1 << 29,
                    _30 = 1 << 30,
                    _31 = 1u << 31
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioObjectNodeOrientationsBlock : TagStructure
                {
                    public short NodeCount;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioObjectNodeOrientationsBitVectorBlock> BitVector;
                    public List<ScenarioObjectNodeOrientationsOrientationsBlock> Orientations;
                    
                    [TagStructure(Size = 0x1)]
                    public class ScenarioObjectNodeOrientationsBitVectorBlock : TagStructure
                    {
                        public byte Data;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class ScenarioObjectNodeOrientationsOrientationsBlock : TagStructure
                    {
                        public short Number;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ScriptlistBlock : TagStructure
                {
                    public StringId ScriptName;
                }
                
                [TagStructure(Size = 0x10)]
                public class ManualbspFlagsReferences : TagStructure
                {
                    public List<ScenariobspReferenceBlock> ReferencesBlock;
                    public int Flags;
                    
                    [TagStructure(Size = 0x10)]
                    public class ScenariobspReferenceBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sbsp" })]
                        public CachedTag StructureDesign;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ScenarioObjectParentStruct : TagStructure
                {
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // if an object with this name exists, we attach to it as a child
                    public short ParentObject;
                    public StringId ParentMarker;
                    public StringId ConnectionMarker;
                }
                
                [TagStructure(Size = 0x14)]
                public class CommandlinkBlock : TagStructure
                {
                    public InternalEventEnum Trigger;
                    public int Target;
                    public CommandEventEnum Command;
                    public float Delay;
                    public CommandlinkFlags Flags;
                    
                    public enum InternalEventEnum : int
                    {
                        OnBirth,
                        OnDeath,
                        OnInteract,
                        OnInitSpawnerShard,
                        OnInitKnightTaint
                    }
                    
                    public enum CommandEventEnum : int
                    {
                        Interact,
                        InitShardSpawn,
                        InitKnightTaint
                    }
                    
                    [Flags]
                    public enum CommandlinkFlags : uint
                    {
                        FireOnce = 1 << 0
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioCreaturePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "crea" })]
            public CachedTag Name;
        }
        
        [TagStructure(Size = 0x10)]
        public class BigBattleCreaturePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bbcr" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x110)]
        public class GScenarioEditorFolderBlock : TagStructure
        {
            public int ParentFolder;
            [TagField(Length = 256)]
            public string Name;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public long Offset;
        }
        
        [TagStructure(Size = 0x10)]
        public class AiScenarioMissionDialogueBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mdlg" })]
            public CachedTag MissionDialogue;
        }
        
        [TagStructure(Size = 0x10)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x24)]
        public class ObjectivesBlock : TagStructure
        {
            public StringId Name;
            public List<OpposingObjectiveBlock> OpposingObjectives;
            public ObjectiveFlags ObjectiveFlags1;
            public short ZoneIndex;
            public short FirstTaskIndex;
            public short EditorFolder;
            public List<TasksBlockStruct> Tasks;
            
            [Flags]
            public enum ObjectiveFlags : ushort
            {
                UseFrontAreaSelection = 1 << 0,
                UsePlayersAsFront = 1 << 1,
                InhibitVehicleEntry = 1 << 2
            }
            
            [TagStructure(Size = 0x4)]
            public class OpposingObjectiveBlock : TagStructure
            {
                public short Objective;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x84)]
            public class TasksBlockStruct : TagStructure
            {
                public TaskFlags Flags;
                public InhibitBehaviorFlags InhibitGroups;
                public GlobalCampaignDifficultyEnum InhibitOnDifficulty;
                public TaskMovementEnum Movement;
                public TaskFollowEnum Follow;
                public short FollowSquad;
                public float FollowRadius;
                // Don't follow at areas outside of this vertical margin
                public float FollowZClamp; // wus
                public TaskFollowPlayerFlags FollowPlayers;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float PlayerFrontRadius;
                // Exhaust this task after it has been active for this long
                public float MaximumDuration; // seconds
                // When a task exhausts, hold actors in the task for this long before releasing them
                public float ExhaustionDelay; // seconds
                public StringId EntryScript;
                public StringId CommandScript;
                // static script that is run when the task is exhausted
                public StringId ExhaustionScript;
                public short ScriptIndex;
                public short CommandScriptIndex;
                public short ExhaustionScriptIndex;
                public short SquadGroupFilter;
                // when someone enters this task for the first time, they play this type of dialogue
                public TaskDialogueEnum DialogueType;
                public TaskRuntimeFlags RuntimeFlags;
                // The number of guys under this task that should be allowed to fight the player at a time
                public short KungfuCount;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId Name;
                public short Priority;
                public short FirstChild;
                public short NextSibling;
                public short Parent;
                public List<ScriptFragmentBlock> ActivationScript;
                public short ScriptIndex1;
                // task will never want to suck in more then n guys over lifetime (soft ceiling only applied when limit exceeded
                public short LifetimeCount;
                public FilterFlags FilterFlags1;
                public FilterEnum Filter;
                public Bounds<short> Capacity;
                // task becomes inactive after the given number of casualties
                public short MaxBodyCount;
                public TaskAttitudeEnum Attitude;
                // task becomes inactive after the strength of the participants falls below the given level
                public float MinStrength; // [0,1]
                public List<AreaReferenceBlockStruct> Areas;
                public List<TaskDirectionBlockV2Struct> Direction;
                
                [Flags]
                public enum TaskFlags : ushort
                {
                    LatchOn = 1 << 0,
                    LatchOff = 1 << 1,
                    Gate = 1 << 2,
                    SingleUse = 1 << 3,
                    SuppressCombat = 1 << 4,
                    SuppressActiveCamo = 1 << 5,
                    Blind = 1 << 6,
                    Deaf = 1 << 7,
                    Braindead = 1 << 8,
                    MagicPlayerSight = 1 << 9,
                    Disable = 1 << 10,
                    IgnoreFronts = 1 << 11,
                    DonTGenerateFront = 1 << 12,
                    ReverseDirection = 1 << 13,
                    InvertFilterLogic = 1 << 14
                }
                
                [Flags]
                public enum InhibitBehaviorFlags : ushort
                {
                    Cover = 1 << 0,
                    Retreat = 1 << 1,
                    Vehicles = 1 << 2,
                    Grenades = 1 << 3,
                    Berserk = 1 << 4,
                    Equipment = 1 << 5,
                    ObjectInteraction = 1 << 6
                }
                
                [Flags]
                public enum GlobalCampaignDifficultyEnum : ushort
                {
                    Easy = 1 << 0,
                    Normal = 1 << 1,
                    Heroic = 1 << 2,
                    Legendary = 1 << 3
                }
                
                public enum TaskMovementEnum : short
                {
                    Run,
                    Walk,
                    Crouch
                }
                
                public enum TaskFollowEnum : short
                {
                    None,
                    Player,
                    Squad,
                    LeadPlayer,
                    PlayerFront
                }
                
                [Flags]
                public enum TaskFollowPlayerFlags : ushort
                {
                    Player0 = 1 << 0,
                    Player1 = 1 << 1,
                    Player2 = 1 << 2,
                    Player3 = 1 << 3
                }
                
                public enum TaskDialogueEnum : short
                {
                    None,
                    EnemyIsAdvancing,
                    EnemyIsCharging,
                    EnemyIsFallingBack,
                    Advance,
                    Charge,
                    FallBack,
                    MoveOn,
                    FollowPlayer,
                    ArrivingIntoCombat,
                    EndCombat,
                    Investigate,
                    SpreadOut,
                    HoldPosition,
                    FindCover,
                    CoveringFire
                }
                
                [Flags]
                public enum TaskRuntimeFlags : ushort
                {
                    AreaConnectivityValid = 1 << 0
                }
                
                [Flags]
                public enum FilterFlags : ushort
                {
                    Exclusive = 1 << 0
                }
                
                public enum FilterEnum : short
                {
                    None,
                    Fireteam,
                    Leader,
                    NoLeader,
                    Arbiter,
                    PlayerInMyVehicle,
                    InCombat,
                    SightedPlayer,
                    SightedEnemy,
                    TargetDisengaged,
                    Infantry,
                    HasAnEngineer,
                    Strength025,
                    Strength05,
                    Strength075,
                    Strength0251,
                    Strength051,
                    Strength0751,
                    HumanTeam,
                    CovenantTeam,
                    MuleTeam,
                    Elite,
                    Jackal,
                    Grunt,
                    Hunter,
                    Marine,
                    Brute,
                    Bugger,
                    Bishop,
                    Knight,
                    Pawn,
                    Rook,
                    Engineer,
                    Skirmisher,
                    Mule,
                    Spartan,
                    Sniper,
                    Rifle,
                    Vehicle,
                    Scorpion,
                    Ghost,
                    Warthog,
                    Wraith,
                    Phantom,
                    TuningFork,
                    Falcon,
                    Seraph,
                    Sabre,
                    Pelican,
                    Banshee,
                    Mongoose,
                    Revenant,
                    ShadeTurret
                }
                
                public enum TaskAttitudeEnum : short
                {
                    Normal,
                    Defensive,
                    Aggressive,
                    Playfighting,
                    Patrol
                }
                
                [TagStructure(Size = 0x108)]
                public class ScriptFragmentBlock : TagStructure
                {
                    public StringId ScriptName;
                    [TagField(Length = 256)]
                    public string ScriptSource;
                    public FragmentStateEnum CompileState;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum FragmentStateEnum : short
                    {
                        Edited,
                        Success,
                        Error
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class AreaReferenceBlockStruct : TagStructure
                {
                    public ZoneSetTypeEnum AreaType;
                    public ZoneSetFlags Flags;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short Zone;
                    public short Area;
                    public Angle Yaw;
                    public int ConnectionFlags0;
                    public int ConnectionFlags1;
                    public int ConnectionFlags2;
                    public int ConnectionFlags3;
                    
                    public enum ZoneSetTypeEnum : short
                    {
                        Normal,
                        Search,
                        Core
                    }
                    
                    [Flags]
                    public enum ZoneSetFlags : byte
                    {
                        Goal = 1 << 0,
                        DirectionValid = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class TaskDirectionBlockV2Struct : TagStructure
                {
                    public List<TaskDirectionPointBlock> Points;
                    
                    [TagStructure(Size = 0x14)]
                    public class TaskDirectionPointBlock : TagStructure
                    {
                        public RealPoint3d Point0;
                        public int PackedkeyOffaceref;
                        public int NavmeshUidoffaceref;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0xF4)]
        public class ScenarioDesignerZoneBlock : TagStructure
        {
            public StringId Name;
            public List<ScenariodesignerZoneTagReferenceBlock> References;
            public List<BipedBlockIndexFlagsBlockStruct> Biped;
            public List<VehicleBlockIndexFlagsBlockStruct> Vehicle;
            public List<WeaponBlockIndexFlagsBlockStruct> Weapon;
            public List<EquipmentBlockIndexFlagsBlockStruct> Equipment;
            public List<SceneryBlockIndexFlagsBlockStruct> Scenery;
            public List<MachineBlockIndexFlagsBlockStruct> Machine;
            public List<TerminalBlockIndexFlagsBlockStruct> Terminal;
            public List<ControlBlockIndexFlagsBlockStruct> Control;
            public List<DispenserBlockIndexFlagsBlockStruct> Dispenser;
            public List<SoundSceneryBlockIndexFlagsBlockStruct> SoundScenery;
            public List<CrateBlockIndexFlagsBlockStruct> Crate;
            public List<CreatureBlockIndexFlagsBlockStruct> Creature;
            public List<GiantBlockIndexFlagsBlockStruct> Giant;
            public List<EffectSceneryBlockIndexFlagsBlockStruct> EffectScenery;
            public List<CharacterBlockIndexFlagsBlockStruct> Character;
            public List<SpawnerBlockIndexFlagsBlockStruct> Spawner;
            public List<BudgetReferenceBlockIndexFlagsBlockStruct> BudgetReference;
            public List<BinkBlockIndexFlagsBlockStruct> Bink;
            public List<ScenariodesignerResourceDependenciesBlock> ResourceDependencies;
            
            [TagStructure(Size = 0x10)]
            public class ScenariodesignerZoneTagReferenceBlock : TagStructure
            {
                public CachedTag Tag;
            }
            
            [TagStructure(Size = 0x2)]
            public class BipedBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class VehicleBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class WeaponBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class EquipmentBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class SceneryBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class MachineBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class TerminalBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class ControlBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class DispenserBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class SoundSceneryBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class CrateBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class CreatureBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class GiantBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class EffectSceneryBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class CharacterBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class SpawnerBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class BudgetReferenceBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class BinkBlockIndexFlagsBlockStruct : TagStructure
            {
                public short PaletteIndex;
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenariodesignerResourceDependenciesBlock : TagStructure
            {
                public CachedTag Tag;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioZoneDebuggerBlockStruct : TagStructure
        {
            public uint ActiveDesignerZones;
        }
        
        [TagStructure(Size = 0x9C)]
        public class ScenarioDecoratorBlock : TagStructure
        {
            public DecoratorBrushStruct Brush;
            public int DecoratorCount;
            public int CurrentBspCount;
            public RealVector3d GlobalOffset;
            public RealVector3d GlobalX;
            public RealVector3d GlobalY;
            public RealVector3d GlobalZ;
            public List<DecoratorPalette> Palette;
            public List<DecoratorScenarioSetBlock> Sets;
            
            [TagStructure(Size = 0x4C)]
            public class DecoratorBrushStruct : TagStructure
            {
                public DecoratorLeftBrushTypeEnum LeftButtonBrush;
                public DecoratorRightBrushTypeEnum MiddleButtonBrush;
                public DecoratorLeftBrushTypeEnum ControlLeftButtonBrush;
                public DecoratorRightBrushTypeEnum ControlMiddleButtonBrush;
                public DecoratorLeftBrushTypeEnum AltLeftButtonBrush;
                public DecoratorRightBrushTypeEnum AltMiddleButtonBrush;
                public float OuterRadius;
                public float FeatherPercent;
                public DecoratorBrushReapplyFlags ReapplyFlags;
                public DecoratorBrushRenderFlags RenderFlags;
                public DecoratorBrushActionFlags ActionFlags;
                public DecoratorBrushShapeEnum BrushShape;
                public int CurrentPalette;
                public int CurrentSet;
                public int CurrentType;
                public float PaintRate; // [0 - 1]
                public RealRgbColor PaintColor;
                // drop height for drop to ground
                public float MoveDistance;
                // rotate brushes will snap to intervals of this
                public float AngleSnapInterval;
                // decorators will not draw beyond this distance from the camera
                public float EditorCullDistance;
                
                public enum DecoratorLeftBrushTypeEnum : int
                {
                    FillAdd,
                    AirbrushAdd,
                    AirbrushColor,
                    AirbrushErase,
                    DensitySmooth,
                    PrecisionPlace,
                    PrecisionDelete,
                    Scale,
                    ScaleAdditive,
                    ScaleSubtractive,
                    RotateRandom,
                    RotateNormal,
                    RotateLocal,
                    Eraser,
                    ReapplyTypeSettings,
                    RaiseToGround,
                    DropToGround,
                    Comb,
                    Thin
                }
                
                public enum DecoratorRightBrushTypeEnum : int
                {
                    FillAdd,
                    AirbrushAdd,
                    AirbrushColor,
                    AirbrushErase,
                    DensitySmooth,
                    PrecisionPlace,
                    PrecisionDelete,
                    Scale,
                    ScaleAdditive,
                    ScaleSubtractive,
                    RotateRandom,
                    RotateNormal,
                    RotateLocal,
                    Eraser,
                    ReapplyTypeSettings,
                    RaiseToGround,
                    DropToGround,
                    Comb,
                    Thin
                }
                
                [Flags]
                public enum DecoratorBrushReapplyFlags : byte
                {
                    ReapplyHover = 1 << 0,
                    ReapplyOrientation = 1 << 1,
                    ReapplyScale = 1 << 2,
                    ReapplyMotion = 1 << 3,
                    ReapplyColor = 1 << 4,
                    ReapplyGroundTint = 1 << 5,
                    ReapplyAllDecorators = 1 << 6
                }
                
                [Flags]
                public enum DecoratorBrushRenderFlags : byte
                {
                    RenderPreview = 1 << 0,
                    RenderInRadiusOnly = 1 << 1,
                    RenderSelectedOnly = 1 << 2,
                    DontRenderLines = 1 << 3
                }
                
                [Flags]
                public enum DecoratorBrushActionFlags : byte
                {
                    ClampScale = 1 << 0,
                    EnforceMinimumDistance = 1 << 1,
                    SelectAllDecoratorSets = 1 << 2,
                    UseGlobalUp = 1 << 3,
                    ConstrainToASingleChannel = 1 << 4
                }
                
                public enum DecoratorBrushShapeEnum : sbyte
                {
                    FlattenedSphere,
                    Spherical,
                    TallSphere,
                    FloatingSphere
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class DecoratorPalette : TagStructure
            {
                public StringId Name;
                public short DecoratorSet0;
                public ushort DecoratorWeight0;
                public short DecoratorSet1;
                public ushort DecoratorWeight1;
                public short DecoratorSet2;
                public ushort DecoratorWeight2;
                public short DecoratorSet3;
                public ushort DecoratorWeight3;
                public short DecoratorSet4;
                public ushort DecoratorWeight4;
                public short DecoratorSet5;
                public ushort DecoratorWeight5;
                public short DecoratorSet6;
                public ushort DecoratorWeight6;
                public short DecoratorSet7;
                public ushort DecoratorWeight7;
            }
            
            [TagStructure(Size = 0x1C)]
            public class DecoratorScenarioSetBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "dctr" })]
                public CachedTag DecoratorSet;
                public List<GlobalDecoratorPlacementBlock> Placements;
                
                [TagStructure(Size = 0x50)]
                public class GlobalDecoratorPlacementBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public byte TypeIndex;
                    public byte MotionScale;
                    public byte GroundTint;
                    public DecoratorPlacementFlags Flags;
                    public RealQuaternion Rotation;
                    public float Scale;
                    public RealPoint3d TintColor;
                    public RealPoint3d OriginalPoint;
                    public RealPoint3d OriginalNormal;
                    public int BspIndex;
                    public short ClusterIndex;
                    public short ClusterDecoratorSetIndex;
                    
                    [Flags]
                    public enum DecoratorPlacementFlags : byte
                    {
                        Unused = 1 << 0,
                        Unused2 = 1 << 1
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioCheapParticleSystemPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "cpem" })]
            public CachedTag Definition;
        }
        
        [TagStructure(Size = 0x1C)]
        public class ScenarioCheapParticleSystemsBlock : TagStructure
        {
            public short PaletteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScriptablelightRigBlock : TagStructure
        {
            public StringId String;
            [TagField(ValidTags = new [] { "lrig" })]
            public CachedTag LightRig;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioCinematicsBlock : TagStructure
        {
            public ScenarioCinematicsFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "cine" })]
            public CachedTag Name;
            
            [Flags]
            public enum ScenarioCinematicsFlags : byte
            {
                DebugOnly = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioCinematicLightingPaletteBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "nclt" })]
            public CachedTag CinematicLightingTag;
        }
        
        [TagStructure(Size = 0x80)]
        public class PlayerRepresentationBlock : TagStructure
        {
            public PlayerRepresentationFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag HudScreenReference;
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag FirstPersonHandsModel;
            public StringId FirstPersonMultiplayerHandsVariant;
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag FirstPersonBodyModel;
            public StringId FirstPersonMultiplayerBodyVariant;
            public List<FirstpersonpHiddenBodyRegionsBlock> HiddenFpbodyRegions;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag BinocularsZoomInSound;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag BinocularsZoomOutSounds;
            public int PlayerInformation;
            
            [Flags]
            public enum PlayerRepresentationFlags : byte
            {
                CanUseHealthPacks = 1 << 0
            }
            
            [TagStructure(Size = 0x8)]
            public class FirstpersonpHiddenBodyRegionsBlock : TagStructure
            {
                public StringId HiddenRegion;
                public FpBodyRegionFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FpBodyRegionFlags : byte
                {
                    VisibleInIcs = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CampaignMetagameScenarioBlock : TagStructure
        {
            public float ParScore;
            public List<CampaignMetagameScenarioBonusesBlock> TimeBonuses;
            
            [TagStructure(Size = 0x8)]
            public class CampaignMetagameScenarioBonusesBlock : TagStructure
            {
                // if you finish in under this time you get the following bonus
                public float Time;
                public float ScoreMultiplier;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class SoftSurfacesDefinitionBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // max - .2f
            public float ClassBiped;
            // max - .09f
            public float ClassDeadBiped;
            // max - .2f
            public float ClassCratesVehicles;
            // max - .04f
            public float ClassDebris;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioCubemapBlock : TagStructure
        {
            public StringId Name;
            public RealPoint3d CubemapPosition;
            public CubemapResolutionEnum CubemapResolution;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public ManualbspFlagsReferences ManualBspFlags;
            public List<CubemapReferencePointsBlock> ReferencePoints;
            
            public enum CubemapResolutionEnum : short
            {
                _16,
                _32,
                _64,
                _128,
                _256
            }
            
            [TagStructure(Size = 0x10)]
            public class ManualbspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;
                
                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sbsp" })]
                    public CachedTag StructureDesign;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class CubemapReferencePointsBlock : TagStructure
            {
                public RealPoint3d ReferencePoint;
                public int PointIndex;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioAirprobesBlock : TagStructure
        {
            public RealPoint3d AirprobePosition;
            public StringId AirprobeName;
            public int BspIndex;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioBudgetReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x10)]
        public class ModelReferencesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag ModelReference;
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioPerformancesBlockStruct : TagStructure
        {
            public StringId Name;
            // The name of a custom script used to drive the performance. If none is given, a default script is uses that goes
            // through the lines in sequence
            public StringId ScriptName;
            // The name of a script global that will be declared for this performance.
            public StringId GlobalName;
            public PerformanceFlags Flags;
            public short EditorFolder;
            public short PointSet;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ScenarioPerformanceActorBlockStruct> Actors;
            public List<ScenarioPerformanceLineBlockStruct> Lines;
            public RealPoint3d Position;
            public RealEulerAngles2d TemplateFacing;
            // The radius inside which actors have to be in order to start.
            public float ThespianRadius;
            // The radius inside which actors become attracted to the thespian origin.
            public float AttractionRadius;
            // The probability that an actor be attracted once inside the thespian radius, every second.
            public float AttractionProbabilityPerSecond; // [0,1]
            public StringId Template;
            public int TemplateIndex;
            public List<ScenarioPerformanceTaskBlockStruct> Tasks;
            
            [Flags]
            public enum PerformanceFlags : ushort
            {
                NotInitiallyPlaced = 1 << 0,
                AllowReplay = 1 << 1,
                InfiniteRadius = 1 << 2,
                ActorsInSearch = 1 << 3
            }
            
            [TagStructure(Size = 0x14)]
            public class ScenarioPerformanceActorBlockStruct : TagStructure
            {
                public ScenarioPerformanceActorFlags Flags;
                public StringId ActorName;
                public short ActorType;
                public short WeaponType;
                public short VehicleType;
                public short DebugSpawnPoint;
                public StringId VehicleSeatLabel;
                
                [Flags]
                public enum ScenarioPerformanceActorFlags : uint
                {
                    ActorIsCritical = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x64)]
            public class ScenarioPerformanceLineBlockStruct : TagStructure
            {
                public StringId Name;
                public short Actor;
                public ScenarioPerformanceLineFlags Flags;
                public short SleepMinimum; // ticks
                public short SleepMaximum; // ticks
                public ScenarioPerformanceLineProgressDefinition LineProgressType;
                public List<ScenarioPerformanceLineScriptFragmentBlock> ScriptFragments;
                public List<ScenarioPerformanceLinePointInteractionBlockStruct> PointInteraction;
                public List<ScenarioPerformanceLineAnimationBlockStruct> Animations;
                public List<ScenarioPerformanceLineSyncActionBlockStruct> SyncActions;
                public List<ScenarioPerformanceLineScenerySyncActionBlockStruct> ScenerySyncActions;
                public List<ScenarioPerformanceLineDialogBlockStruct> DialogLines;
                public List<ScenarioPerformanceLineSoundBlockStruct> Sounds;
                
                [Flags]
                public enum ScenarioPerformanceLineFlags : ushort
                {
                    Disable = 1 << 0
                }
                
                public enum ScenarioPerformanceLineProgressDefinition : int
                {
                    Immediate,
                    BlockUntilAllDone,
                    BlockUntilLineDone,
                    QueueBlocking,
                    QueueImmediate
                }
                
                [TagStructure(Size = 0x204)]
                public class ScenarioPerformanceLineScriptFragmentBlock : TagStructure
                {
                    public ScenarioPerformanceFragmentPlacementDefinition FragmentPlacement;
                    public ScenarioPerformanceFragmentType FragmentType;
                    // maximum 256 characters, type just branch condition (with brackets) in case of branches
                    [TagField(Length = 256)]
                    public string Fragment;
                    // the script to branch to (with any arguments to it). Used only if type is branch
                    [TagField(Length = 256)]
                    public string BranchTarget;
                    
                    public enum ScenarioPerformanceFragmentPlacementDefinition : short
                    {
                        PreLine,
                        PostLine
                    }
                    
                    public enum ScenarioPerformanceFragmentType : short
                    {
                        Default,
                        ConditionalSleep,
                        Branch
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ScenarioPerformanceLinePointInteractionBlockStruct : TagStructure
                {
                    public ScenarioPerformanceLinePointInteractionType InteractionType;
                    public short Point;
                    public short Actor;
                    public StringId ObjectName;
                    public StringId ThrottleStyle;
                    
                    [Flags]
                    public enum ScenarioPerformanceLinePointInteractionType : uint
                    {
                        FacePoint = 1 << 0,
                        AimAtPoint = 1 << 1,
                        LookAtPoint = 1 << 2,
                        ShootAtPoint = 1 << 3,
                        GoByPoint = 1 << 4,
                        GoToPoint = 1 << 5,
                        GoToAndAlign = 1 << 6,
                        GoToThespianCenter = 1 << 7,
                        TeleportToPoint = 1 << 8
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class ScenarioPerformanceLineAnimationBlockStruct : TagStructure
                {
                    public ScenarioPerformanceLineAnimationFlags Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Stance;
                    public StringId Animation;
                    public float Duration;
                    public float Probability;
                    public float ThrottleTransitionTime; // seconds
                    // The number of frames from the end of the animation to start transitioning out
                    public int TransitionFrameCount;
                    
                    [Flags]
                    public enum ScenarioPerformanceLineAnimationFlags : ushort
                    {
                        Loop = 1 << 0,
                        LoopUntilTaskTransition = 1 << 1,
                        DieOnAnimationCompletion = 1 << 2
                    }
                }
                
                [TagStructure(Size = 0x18)]
                public class ScenarioPerformanceLineSyncActionBlockStruct : TagStructure
                {
                    public StringId SyncActionName;
                    public float Probability;
                    public short AttachToPoint;
                    public ScenarioPerformanceLineSyncActionFlagType Flags;
                    public List<ScenarioPerformanceLineSyncActionActorBlock> Actors;
                    
                    [Flags]
                    public enum ScenarioPerformanceLineSyncActionFlagType : ushort
                    {
                        ShareInitiatorStance = 1 << 0,
                        InitiatorIsOrigin = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class ScenarioPerformanceLineSyncActionActorBlock : TagStructure
                    {
                        public short ActorType;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x20)]
                public class ScenarioPerformanceLineScenerySyncActionBlockStruct : TagStructure
                {
                    public StringId SceneryObjectName;
                    public StringId SyncActionName;
                    public StringId StanceName;
                    public float Probability;
                    public ScenarioPerformanceLineScenerySyncActionFlagType Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<ScenarioPerformanceLineSyncActionActorBlock> Actors;
                    
                    [Flags]
                    public enum ScenarioPerformanceLineScenerySyncActionFlagType : ushort
                    {
                        ShareInitiatorStance = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class ScenarioPerformanceLineSyncActionActorBlock : TagStructure
                    {
                        public short ActorType;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioPerformanceLineDialogBlockStruct : TagStructure
                {
                    public StringId Dialog;
                    public float Probability;
                }
                
                [TagStructure(Size = 0x18)]
                public class ScenarioPerformanceLineSoundBlockStruct : TagStructure
                {
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag SoundEffect;
                    public short AttachToPoint;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId AttachToObjectNamed;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioPerformanceTaskBlockStruct : TagStructure
            {
                public short Objective;
                public short Task;
            }
        }
        
        [TagStructure(Size = 0x118)]
        public class PuppetShowsBlock : TagStructure
        {
            public StringId Name;
            public StringId Designerzone;
            public ManualbspFlagsReferences ManualBspFlags;
            public PuppetShowFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Icspoint0;
            [TagField(Length = 32)]
            public string Icspoint1;
            [TagField(Length = 32)]
            public string Icspoint2;
            [TagField(Length = 32)]
            public string Icspoint3;
            public int LastactionId;
            public List<PuppetBlock> Puppets;
            public List<PuppetHeaderStruct> Puppetheaders;
            public List<PuppetActionHeaderStruct> Actions;
            public List<PuppetSubActionHeaderStruct> Subactions;
            public List<PuppetActionAnimationStruct> Animations;
            public List<PuppetActionPathStruct> Paths;
            public List<PuppetSubActionPointStruct> Points;
            public List<PuppetSubActionBranchStruct> Branches;
            public List<PuppetSubActionScriptStruct> Scripts;
            public List<CommentsBlock> Comments;
            
            [Flags]
            public enum PuppetShowFlags : byte
            {
                CoopIcs = 1 << 0,
                ImmediateIcs = 1 << 1
            }
            
            [TagStructure(Size = 0x10)]
            public class ManualbspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;
                
                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sbsp" })]
                    public CachedTag StructureDesign;
                }
            }
            
            [TagStructure(Size = 0x394)]
            public class PuppetBlock : TagStructure
            {
                public PuppetHeaderStruct Header;
                public List<PuppetActionBlock> Actions;
                public List<PuppetSubActionBlock> Subactions;
                public List<SubTracksBlock> Subtracks;
                [TagField(Length = 256)]
                public string Comment;
                [TagField(Length = 256)]
                public string Startscript;
                [TagField(Length = 256)]
                public string Endscript;
                public short Height;
                public PuppetEditorFlags Editorflags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum PuppetEditorFlags : byte
                {
                    Muted = 1 << 0,
                    Expanded = 1 << 1
                }
                
                [TagStructure(Size = 0x6C)]
                public class PuppetHeaderStruct : TagStructure
                {
                    public PuppetFlags Flags;
                    public PuppetIndexTypeEnum Indextype;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Name;
                    [TagField(ValidTags = new [] { "bipd","char","bloc","mach","scen","vehi" })]
                    public CachedTag Type;
                    public StringId Objectname;
                    public int Index;
                    public PuppetPathPointStruct Position;
                    public StringId StartscriptName;
                    public StringId EndscriptName;
                    public short Startscript;
                    public short Endscript;
                    public short Firstaction;
                    public short Actioncount;
                    public short FirstsubAction;
                    public short SubactionCount;
                    [TagField(ValidTags = new [] { "jmad" })]
                    public CachedTag Additionalanimations;
                    
                    [Flags]
                    public enum PuppetFlags : byte
                    {
                        Optional = 1 << 0,
                        Player = 1 << 1,
                        ForcePosition = 1 << 2,
                        CreateIfMissing = 1 << 3,
                        DestroyWhenFinished = 1 << 4,
                        AbortOnDamage = 1 << 5,
                        AbortOnAlert = 1 << 6,
                        AbortShow = 1 << 7
                    }
                    
                    public enum PuppetIndexTypeEnum : sbyte
                    {
                        Name,
                        Ai,
                        Global,
                        Puppet,
                        PointSet,
                        Flag
                    }
                    
                    [TagStructure(Size = 0x28)]
                    public class PuppetPathPointStruct : TagStructure
                    {
                        public PuppetPathPointTypeEnum Type;
                        public PuppetPathPointFlags Flags;
                        public PuppetIndexTypeEnum Indextype;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public StringId Objectname;
                        public int Index;
                        public StringId Marker;
                        public RealPoint3d Pos;
                        public RealEulerAngles3d Rot;
                        
                        public enum PuppetPathPointTypeEnum : sbyte
                        {
                            Current,
                            Object,
                            PointSet,
                            Flag,
                            Custom
                        }
                        
                        [Flags]
                        public enum PuppetPathPointFlags : byte
                        {
                            HasRotation = 1 << 0,
                            DonTStop = 1 << 1
                        }
                    }
                }
                
                [TagStructure(Size = 0x298)]
                public class PuppetActionBlock : TagStructure
                {
                    public PuppetActionHeaderStruct Header;
                    public PuppetActionAnimationStruct Animation;
                    public PuppetActionPathStruct Path;
                    [TagField(Length = 256)]
                    public string Comment;
                    [TagField(Length = 256)]
                    public string Animcondition;
                    public uint Color;
                    public PuppetEditorFlags Editorflags;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int Startframe;
                    public int Endframe;
                    public int Blendframe;
                    public int Startpixel;
                    public int Endpixel;
                    public int Blendpixel;
                    public int BlendinDrag;
                    public int Lengthdrag;
                    
                    [Flags]
                    public enum PuppetEditorFlags : byte
                    {
                        Muted = 1 << 0,
                        Expanded = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x18)]
                    public class PuppetActionHeaderStruct : TagStructure
                    {
                        public StringId Name;
                        public int Id;
                        public int Blendin;
                        public int Length;
                        public int Comment;
                        public PuppetActionBlendTypeEnum Blendtype;
                        public PuppetActionTypeEnum Type;
                        public short Dataindex;
                        
                        public enum PuppetActionBlendTypeEnum : sbyte
                        {
                            Linear,
                            Smooth
                        }
                        
                        public enum PuppetActionTypeEnum : sbyte
                        {
                            Animation,
                            Path
                        }
                    }
                    
                    [TagStructure(Size = 0x48)]
                    public class PuppetActionAnimationStruct : TagStructure
                    {
                        public StringId Name;
                        public PuppetPathPointStruct Position;
                        public PuppetAnimationFlags Flags;
                        public AnimPositionTypeEnum Postype;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public int Startframe;
                        public int Endframe;
                        public int Repeatcount;
                        public float Scale;
                        public StringId ConditionscriptName;
                        public int Condition;
                        
                        [Flags]
                        public enum PuppetAnimationFlags : byte
                        {
                            Looping = 1 << 0,
                            Reverse = 1 << 1,
                            ResetObjectPosition = 1 << 2,
                            Ics = 1 << 3,
                            IcsResetCamera = 1 << 4,
                            Additional = 1 << 5
                        }
                        
                        public enum AnimPositionTypeEnum : sbyte
                        {
                            Teleport,
                            Attach,
                            Free
                        }
                        
                        [TagStructure(Size = 0x28)]
                        public class PuppetPathPointStruct : TagStructure
                        {
                            public PuppetPathPointTypeEnum Type;
                            public PuppetPathPointFlags Flags;
                            public PuppetIndexTypeEnum Indextype;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public StringId Objectname;
                            public int Index;
                            public StringId Marker;
                            public RealPoint3d Pos;
                            public RealEulerAngles3d Rot;
                            
                            public enum PuppetPathPointTypeEnum : sbyte
                            {
                                Current,
                                Object,
                                PointSet,
                                Flag,
                                Custom
                            }
                            
                            [Flags]
                            public enum PuppetPathPointFlags : byte
                            {
                                HasRotation = 1 << 0,
                                DonTStop = 1 << 1
                            }
                            
                            public enum PuppetIndexTypeEnum : sbyte
                            {
                                Name,
                                Ai,
                                Global,
                                Puppet,
                                PointSet,
                                Flag
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class PuppetActionPathStruct : TagStructure
                    {
                        public float Throttle;
                        public List<PuppetPathPointStruct> Points;
                        
                        [TagStructure(Size = 0x28)]
                        public class PuppetPathPointStruct : TagStructure
                        {
                            public PuppetPathPointTypeEnum Type;
                            public PuppetPathPointFlags Flags;
                            public PuppetIndexTypeEnum Indextype;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public StringId Objectname;
                            public int Index;
                            public StringId Marker;
                            public RealPoint3d Pos;
                            public RealEulerAngles3d Rot;
                            
                            public enum PuppetPathPointTypeEnum : sbyte
                            {
                                Current,
                                Object,
                                PointSet,
                                Flag,
                                Custom
                            }
                            
                            [Flags]
                            public enum PuppetPathPointFlags : byte
                            {
                                HasRotation = 1 << 0,
                                DonTStop = 1 << 1
                            }
                            
                            public enum PuppetIndexTypeEnum : sbyte
                            {
                                Name,
                                Ai,
                                Global,
                                Puppet,
                                PointSet,
                                Flag
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x4A8)]
                public class PuppetSubActionBlock : TagStructure
                {
                    public PuppetSubActionHeaderStruct Header;
                    public PuppetSubActionPointStruct Point;
                    public PuppetSubActionBranchStruct Branch;
                    public PuppetSubActionScriptStruct Script;
                    [TagField(Length = 256)]
                    public string Comment;
                    [TagField(Length = 256)]
                    public string Startcondition;
                    [TagField(Length = 256)]
                    public string Endcondition;
                    [TagField(Length = 256)]
                    public string Scripttext;
                    public List<PuppetScriptTextBlock> Branchconditions;
                    public uint Color;
                    public byte Subtrack;
                    public PuppetEditorFlags Editorflags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int Startframe;
                    public int Endframe;
                    public int Startpixel;
                    public int Endpixel;
                    public int SubtrackDrag;
                    public int StartoffsetDrag;
                    public int EndoffsetDrag;
                    
                    [Flags]
                    public enum PuppetEditorFlags : byte
                    {
                        Muted = 1 << 0,
                        Expanded = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x28)]
                    public class PuppetSubActionHeaderStruct : TagStructure
                    {
                        public PuppetSubActionTypeEnum Type;
                        public SubActionTimeTypeEnum Starttype;
                        public SubActionTimeTypeEnum Endtype;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public int Startaction;
                        public int Startoffset;
                        public StringId StartconditionScriptName;
                        public StringId EndconditionScriptName;
                        public short Startcondition;
                        public short Endcondition;
                        public int Endaction;
                        public int Endoffset;
                        public int Comment;
                        public short Dataindex;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        
                        public enum PuppetSubActionTypeEnum : sbyte
                        {
                            LookAt,
                            Face,
                            ShootAt,
                            Overlay,
                            Effect,
                            Sound,
                            LoopingSound,
                            Dialogue,
                            Branch,
                            Script
                        }
                        
                        public enum SubActionTimeTypeEnum : sbyte
                        {
                            OffsetFromStart,
                            OffsetFromEnd,
                            Condition
                        }
                    }
                    
                    [TagStructure(Size = 0x3C)]
                    public class PuppetSubActionPointStruct : TagStructure
                    {
                        public PuppetPathPointStruct Point;
                        [TagField(ValidTags = new [] { "effe","snd!","lsnd","mdlg" })]
                        public CachedTag Asset;
                        public SubActionPointFlags Flags;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum SubActionPointFlags : byte
                        {
                            LoopingEffect = 1 << 0,
                            AttachedEffect = 1 << 1
                        }
                        
                        [TagStructure(Size = 0x28)]
                        public class PuppetPathPointStruct : TagStructure
                        {
                            public PuppetPathPointTypeEnum Type;
                            public PuppetPathPointFlags Flags;
                            public PuppetIndexTypeEnum Indextype;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public StringId Objectname;
                            public int Index;
                            public StringId Marker;
                            public RealPoint3d Pos;
                            public RealEulerAngles3d Rot;
                            
                            public enum PuppetPathPointTypeEnum : sbyte
                            {
                                Current,
                                Object,
                                PointSet,
                                Flag,
                                Custom
                            }
                            
                            [Flags]
                            public enum PuppetPathPointFlags : byte
                            {
                                HasRotation = 1 << 0,
                                DonTStop = 1 << 1
                            }
                            
                            public enum PuppetIndexTypeEnum : sbyte
                            {
                                Name,
                                Ai,
                                Global,
                                Puppet,
                                PointSet,
                                Flag
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class PuppetSubActionBranchStruct : TagStructure
                    {
                        public List<PuppetSubActionBranchElementBlock> Elements;
                        
                        [TagStructure(Size = 0xC)]
                        public class PuppetSubActionBranchElementBlock : TagStructure
                        {
                            public int Targetaction;
                            public StringId ConditionscriptName;
                            public int Condition;
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class PuppetSubActionScriptStruct : TagStructure
                    {
                        public StringId Scriptname;
                        public int Script;
                    }
                    
                    [TagStructure(Size = 0x100)]
                    public class PuppetScriptTextBlock : TagStructure
                    {
                        [TagField(Length = 256)]
                        public string Scripttext;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class SubTracksBlock : TagStructure
                {
                    public StringId Name;
                }
            }
            
            [TagStructure(Size = 0x6C)]
            public class PuppetHeaderStruct : TagStructure
            {
                public PuppetFlags Flags;
                public PuppetIndexTypeEnum Indextype;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId Name;
                [TagField(ValidTags = new [] { "bipd","char","bloc","mach","scen","vehi" })]
                public CachedTag Type;
                public StringId Objectname;
                public int Index;
                public PuppetPathPointStruct Position;
                public StringId StartscriptName;
                public StringId EndscriptName;
                public short Startscript;
                public short Endscript;
                public short Firstaction;
                public short Actioncount;
                public short FirstsubAction;
                public short SubactionCount;
                [TagField(ValidTags = new [] { "jmad" })]
                public CachedTag Additionalanimations;
                
                [Flags]
                public enum PuppetFlags : byte
                {
                    Optional = 1 << 0,
                    Player = 1 << 1,
                    ForcePosition = 1 << 2,
                    CreateIfMissing = 1 << 3,
                    DestroyWhenFinished = 1 << 4,
                    AbortOnDamage = 1 << 5,
                    AbortOnAlert = 1 << 6,
                    AbortShow = 1 << 7
                }
                
                public enum PuppetIndexTypeEnum : sbyte
                {
                    Name,
                    Ai,
                    Global,
                    Puppet,
                    PointSet,
                    Flag
                }
                
                [TagStructure(Size = 0x28)]
                public class PuppetPathPointStruct : TagStructure
                {
                    public PuppetPathPointTypeEnum Type;
                    public PuppetPathPointFlags Flags;
                    public PuppetIndexTypeEnum Indextype;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Objectname;
                    public int Index;
                    public StringId Marker;
                    public RealPoint3d Pos;
                    public RealEulerAngles3d Rot;
                    
                    public enum PuppetPathPointTypeEnum : sbyte
                    {
                        Current,
                        Object,
                        PointSet,
                        Flag,
                        Custom
                    }
                    
                    [Flags]
                    public enum PuppetPathPointFlags : byte
                    {
                        HasRotation = 1 << 0,
                        DonTStop = 1 << 1
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class PuppetActionHeaderStruct : TagStructure
            {
                public StringId Name;
                public int Id;
                public int Blendin;
                public int Length;
                public int Comment;
                public PuppetActionBlendTypeEnum Blendtype;
                public PuppetActionTypeEnum Type;
                public short Dataindex;
                
                public enum PuppetActionBlendTypeEnum : sbyte
                {
                    Linear,
                    Smooth
                }
                
                public enum PuppetActionTypeEnum : sbyte
                {
                    Animation,
                    Path
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class PuppetSubActionHeaderStruct : TagStructure
            {
                public PuppetSubActionTypeEnum Type;
                public SubActionTimeTypeEnum Starttype;
                public SubActionTimeTypeEnum Endtype;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Startaction;
                public int Startoffset;
                public StringId StartconditionScriptName;
                public StringId EndconditionScriptName;
                public short Startcondition;
                public short Endcondition;
                public int Endaction;
                public int Endoffset;
                public int Comment;
                public short Dataindex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum PuppetSubActionTypeEnum : sbyte
                {
                    LookAt,
                    Face,
                    ShootAt,
                    Overlay,
                    Effect,
                    Sound,
                    LoopingSound,
                    Dialogue,
                    Branch,
                    Script
                }
                
                public enum SubActionTimeTypeEnum : sbyte
                {
                    OffsetFromStart,
                    OffsetFromEnd,
                    Condition
                }
            }
            
            [TagStructure(Size = 0x48)]
            public class PuppetActionAnimationStruct : TagStructure
            {
                public StringId Name;
                public PuppetPathPointStruct Position;
                public PuppetAnimationFlags Flags;
                public AnimPositionTypeEnum Postype;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Startframe;
                public int Endframe;
                public int Repeatcount;
                public float Scale;
                public StringId ConditionscriptName;
                public int Condition;
                
                [Flags]
                public enum PuppetAnimationFlags : byte
                {
                    Looping = 1 << 0,
                    Reverse = 1 << 1,
                    ResetObjectPosition = 1 << 2,
                    Ics = 1 << 3,
                    IcsResetCamera = 1 << 4,
                    Additional = 1 << 5
                }
                
                public enum AnimPositionTypeEnum : sbyte
                {
                    Teleport,
                    Attach,
                    Free
                }
                
                [TagStructure(Size = 0x28)]
                public class PuppetPathPointStruct : TagStructure
                {
                    public PuppetPathPointTypeEnum Type;
                    public PuppetPathPointFlags Flags;
                    public PuppetIndexTypeEnum Indextype;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Objectname;
                    public int Index;
                    public StringId Marker;
                    public RealPoint3d Pos;
                    public RealEulerAngles3d Rot;
                    
                    public enum PuppetPathPointTypeEnum : sbyte
                    {
                        Current,
                        Object,
                        PointSet,
                        Flag,
                        Custom
                    }
                    
                    [Flags]
                    public enum PuppetPathPointFlags : byte
                    {
                        HasRotation = 1 << 0,
                        DonTStop = 1 << 1
                    }
                    
                    public enum PuppetIndexTypeEnum : sbyte
                    {
                        Name,
                        Ai,
                        Global,
                        Puppet,
                        PointSet,
                        Flag
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class PuppetActionPathStruct : TagStructure
            {
                public float Throttle;
                public List<PuppetPathPointStruct> Points;
                
                [TagStructure(Size = 0x28)]
                public class PuppetPathPointStruct : TagStructure
                {
                    public PuppetPathPointTypeEnum Type;
                    public PuppetPathPointFlags Flags;
                    public PuppetIndexTypeEnum Indextype;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Objectname;
                    public int Index;
                    public StringId Marker;
                    public RealPoint3d Pos;
                    public RealEulerAngles3d Rot;
                    
                    public enum PuppetPathPointTypeEnum : sbyte
                    {
                        Current,
                        Object,
                        PointSet,
                        Flag,
                        Custom
                    }
                    
                    [Flags]
                    public enum PuppetPathPointFlags : byte
                    {
                        HasRotation = 1 << 0,
                        DonTStop = 1 << 1
                    }
                    
                    public enum PuppetIndexTypeEnum : sbyte
                    {
                        Name,
                        Ai,
                        Global,
                        Puppet,
                        PointSet,
                        Flag
                    }
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class PuppetSubActionPointStruct : TagStructure
            {
                public PuppetPathPointStruct Point;
                [TagField(ValidTags = new [] { "effe","snd!","lsnd","mdlg" })]
                public CachedTag Asset;
                public SubActionPointFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum SubActionPointFlags : byte
                {
                    LoopingEffect = 1 << 0,
                    AttachedEffect = 1 << 1
                }
                
                [TagStructure(Size = 0x28)]
                public class PuppetPathPointStruct : TagStructure
                {
                    public PuppetPathPointTypeEnum Type;
                    public PuppetPathPointFlags Flags;
                    public PuppetIndexTypeEnum Indextype;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId Objectname;
                    public int Index;
                    public StringId Marker;
                    public RealPoint3d Pos;
                    public RealEulerAngles3d Rot;
                    
                    public enum PuppetPathPointTypeEnum : sbyte
                    {
                        Current,
                        Object,
                        PointSet,
                        Flag,
                        Custom
                    }
                    
                    [Flags]
                    public enum PuppetPathPointFlags : byte
                    {
                        HasRotation = 1 << 0,
                        DonTStop = 1 << 1
                    }
                    
                    public enum PuppetIndexTypeEnum : sbyte
                    {
                        Name,
                        Ai,
                        Global,
                        Puppet,
                        PointSet,
                        Flag
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class PuppetSubActionBranchStruct : TagStructure
            {
                public List<PuppetSubActionBranchElementBlock> Elements;
                
                [TagStructure(Size = 0xC)]
                public class PuppetSubActionBranchElementBlock : TagStructure
                {
                    public int Targetaction;
                    public StringId ConditionscriptName;
                    public int Condition;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class PuppetSubActionScriptStruct : TagStructure
            {
                public StringId Scriptname;
                public int Script;
            }
            
            [TagStructure(Size = 0x1)]
            public class CommentsBlock : TagStructure
            {
                public sbyte Char;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class GarbageCollectionBlock : TagStructure
        {
            public float DroppedItem; // seconds
            public float DroppedItemByPlayer; // seconds
            public float DroppedItemInMultiplayer; // seconds
            public float BrokenConstraints; // seconds
            public float DeadUnit; // seconds
            public float DeadPlayer; // seconds
            public float DeadMpPlayer; // seconds
            public float DeadMpPlayerOverloaded; // seconds
            // above this number, overloaded mp time is used to garbage collect dead bodies
            public int MaxDeadBodyCount;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenariorandomOrdnanceDropSetBlock : TagStructure
        {
            public OrdnanceDropsetFlags DropSetFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            public int Count;
            [TagField(ValidTags = new [] { "scol" })]
            public CachedTag OrdnanceList;
            public List<ScenariorandomOrdnanceDropPointBlock> DropPointList;
            
            [Flags]
            public enum OrdnanceDropsetFlags : ushort
            {
                // will be used for initial drops
                InitialDrop = 1 << 0,
                // will be used for personal drops
                PlayerDrop = 1 << 1,
                // will be used for random drops
                RandomDrop = 1 << 2,
                // will be used for objective-based drops
                ObjectiveDrop = 1 << 3
            }
            
            [TagStructure(Size = 0x2)]
            public class ScenariorandomOrdnanceDropPointBlock : TagStructure
            {
                public short DropPoint;
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioUnitRecordingBlockStruct : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public int SamplingRate;
            public int NumSamples;
            public int UnitRecordingVersion;
            public byte[] UnitRecordingData;
        }
        
        [TagStructure(Size = 0x18)]
        public class LoadscreenReferenceBlock : TagStructure
        {
            // Only valid for main menu - otherwise always use first reference
            public int MapId;
            [TagField(ValidTags = new [] { "ldsc" })]
            public CachedTag LoadScreenReference;
            // Value between 0.0 and 1.0 determines which line to use for tint.
            // A negative value will choose a random tint from the palette.
            public float TintGradientLookupVCoordinate;
        }
    }
}
