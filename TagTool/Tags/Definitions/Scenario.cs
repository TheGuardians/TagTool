using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Audio;
using TagTool.Tags.Definitions.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x7B8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x834, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x824, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x834, MinVersion = CacheVersion.HaloOnline498295)]
    public class Scenario : TagStructure
	{
        [TagField(Length = 1, Flags = Padding, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public byte MapTypePadding;

        [TagField(Length = 1, Flags = Padding, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.Original)]
        public byte MapTypePaddingReach;

        public ScenarioMapType MapType;

        [TagField(Length = 1, Flags = Padding, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public byte MapTypePaddingMCC;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public ScenarioMapSubType MapSubType;

        public ScenarioFlags Flags;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ScenarioRuntimeTriggerVolumeFlags RuntimeTriggerVolumeFlags;

        public int CampaignId;
        public int MapId;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId MapName;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short SoundPermutationMissionId;
        [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int MinimumStructureBspImporterVersion;

        public Angle LocalNorth;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LocalSeaLevel; // wu
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AltitudeCap; // wu
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealPoint3d SandboxOriginPoint;  // forge coordinates are relative to this point

        public float SandboxBudget;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId DefaultVehicleSet;
        [TagField(ValidTags = new[] { "gptd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GamePerformanceThrottles;

        public List<StructureBspBlock> StructureBsps;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioDesignReferenceBlock> StructureDesigns;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public CachedTag ScenarioPDA;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public List<PdaDefinitionsBlock> PdaDefinitions;

        [TagField(ValidTags = new[] { "stse" })]
        public CachedTag StructureSeams;
        [TagField(ValidTags = new[] { "stse" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag LocalStructureSeams;

        public List<SkyReference> SkyReferences;
        public List<ZoneSetPvsBlock> ZoneSetPvs;
        public List<ZoneSetAudibilityBlock> ZoneSetAudibility;
        public List<ZoneSet> ZoneSets;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LightingZoneSetBlock> LightingZoneSets;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<BspAtlasBlock> BspAtlas;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CampaignPlayer> CampaignPlayers;

        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;

        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;

        public uint Unknown15;
        public uint Unknown16;
        public uint Unknown17;

        public byte[] EditorScenarioData;

        public List<Comment> Comments;
        public List<ObjectName> ObjectNames;
        public List<SceneryInstance> Scenery;
        public List<ScenarioPaletteEntry> SceneryPalette;
        public List<BipedInstance> Bipeds;
        public List<ScenarioPaletteEntry> BipedPalette;
        public List<VehicleInstance> Vehicles;
        public List<ScenarioPaletteEntry> VehiclePalette;
        public List<EquipmentInstance> Equipment;
        public List<ScenarioPaletteEntry> EquipmentPalette;
        public List<WeaponInstance> Weapons;
        public List<ScenarioPaletteEntry> WeaponPalette;
        public List<DeviceGroup> DeviceGroups;
        public List<MachineInstance> Machines;
        public List<ScenarioPaletteEntry> MachinePalette;
        public List<TerminalInstance> Terminals;
        public List<ScenarioPaletteEntry> TerminalPalette;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<AlternateRealityDeviceInstance> AlternateRealityDevices;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ScenarioPaletteEntry> AlternateRealityDevicePalette;

        public List<ControlInstance> Controls;
        public List<ScenarioPaletteEntry> ControlPalette;
        public List<SoundSceneryInstance> SoundScenery;
        public List<ScenarioPaletteEntry> SoundSceneryPalette;
        public List<GiantInstance> Giants;
        public List<ScenarioPaletteEntry> GiantPalette;
        public List<EffectSceneryInstance> EffectScenery;
        public List<ScenarioPaletteEntry> EffectSceneryPalette;
        public List<LightVolumeInstance> LightVolumes;
        public List<ScenarioPaletteEntry> LightVolumePalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxVehicles;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxWeapons;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxEquipment;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxScenery;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxTeleporters;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxGoalObjects;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SandboxObject> SandboxSpawning;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<MapVariantPaletteBlock> MapVariantPalettes;

        [TagField(ValidTags = new[] { "motl" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag MultiplayerObjectTypes;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown200;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown201;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown202;

        public List<SoftCeiling> SoftCeilings;
        public List<PlayerStartingProfileBlock> PlayerStartingProfile;
        public List<PlayerStartingLocation> PlayerStartingLocations;
        public List<TriggerVolume> TriggerVolumes;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioAcousticSectorBlockStruct> AcousticSectors;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> AcousticTransitions;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> AtmosphereDumplings;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> WeatherDumplings;

        public List<RecordedAnimation> RecordedAnimations;
        public List<ZoneSetSwitchTriggerVolume> ZonesetSwitchTriggerVolumes;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioNamedLocationVolumeBlock> NamedLocationVolumes;

        [TagField(ValidTags = new[] { "ssdf" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag SpawnSettings;

        public List<PlayerSpawnInfluencerBlock> EnemyForbidInfluence;
        public List<PlayerSpawnInfluencerBlock> EnemyBiasInfluence;
        public List<PlayerSpawnInfluencerBlock> AllyBiasInfluence;
        public List<PlayerSpawnInfluencerBlock> SelectedAllyBiasInfluence;
        public List<PlayerSpawnInfluencerBlock> DeadTeammateInfluence;

        public List<WeaponSpawnInfluenceBlock> WeaponSpawnInfluencers;
        public List<VehicleSpawnInfluenceBlock> VehicleSpawnInfluencers;
        public List<ProjectileSpawnInfluenceBlock> ProjectileSpawnInfluencers;
        public List<EquipmentSpawnInfluenceBlock> EquipmentSpawnInfluencers;

        public List<NetgameGoalInfluencerBlock> KothHillInfluencer;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> OddballInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> CtfFlagAwayInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> TerritoriesAllyInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> TerritoriesEnemyInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> InfectionSafeZoneHumanInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> InfectionSafeZoneZombieInfluencer;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NetgameGoalInfluencerBlock> VipInfluencer;

        public List<Decal> Decals;
        public List<TagReferenceBlock> DecalPalette;
        public List<TagReferenceBlock> DetailObjectCollectionPalette;
        public List<TagReferenceBlock> StylePalette;
        public List<SquadGroup> SquadGroups;
        public List<Squad> Squads;
        public List<Zone> Zones;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<SquadPatrol> SquadPatrols;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> ActualCues; // TODO
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> FullCues; // TODO
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GNullBlock> QuickCues; // TODO

        public List<MissionScene> MissionScenes;
        public List<TagReferenceBlock> CharacterPalette;
        public List<PathfindingDataBlock> AiPathfindingData;
        public List<UserHintBlock> AiUserHintData;
        public List<AiRecordingReferenceBlock> AiRecordingReferences;

        public byte[] ScriptStrings;

        public List<HsScript> Scripts;
        public List<HsGlobal> Globals;
        public List<TagReferenceBlock> ScriptSourceFileReferences;
        public List<TagReferenceBlock> ScriptExternalFileReferences;
        public List<ScriptingDatum> ScriptingData;
        public List<CutsceneFlag> CutsceneFlags;
        public List<CutsceneCameraPoint> CutsceneCameraPoints;
        public List<CutsceneTitle> CutsceneTitles;
        public CachedTag CustomObjectNameStrings;
        public CachedTag ChapterTitleStrings;

        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag Unknown156;

        public List<ScenarioResource> ScenarioResources;
        public List<UnitSeatsMappingBlock> UnitSeatsMapping;
        public List<ScenarioKillTrigger> ScenarioKillTriggers;
        public List<ScenarioSafeTrigger> ScenarioSafeTriggers;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TriggerVolumeMoppCodeBlock> ScenarioTriggerVolumesMoppCode;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown210;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown211;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown212;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown213;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown214;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown215;

        public List<HsSyntaxNode> ScriptExpressions;
        public List<OrdersBlock> Orders;
        public List<TriggersBlock> Triggers;

        public List<ScenarioStructureBsp.AcousticsPaletteBlock> AcousticsPalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<AcousticsAmbiencePaletteBlock> OldBackgroundSoundPalette;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<AcousticsEnvironmentPaletteBlock> SoundEnvironmentPalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<WeatherPaletteBlock> WeatherPalette;

        public List<ScenarioStructureBsp.StructureBspAtmospherePaletteBlock> Atmosphere;
        public List<ScenarioStructureBsp.StructureBspCameraFxPaletteBlock> CameraFx;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<WeatherPaletteBlock> WeatherPaletteReach;

        //[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        //public List<GNullBlock> Unused;
        //[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        //public List<GNullBlock> Unused2;
        //[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        //public List<GNullBlock> Unused3;
        [TagField(Flags = Padding, Length = 36, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding3UnusedBlocks;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ScenarioClusterDatum> ScenarioClusterData;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.Scenario.ScenarioClusterDataBlock> ScenarioClusterDataReach; // ugh

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ScenarioAcousticVolumeBlock> AcousticSpaces;
        
        [TagField(Length = 32)]
        public int[] ObjectSalts = new int[32];

        public List<SpawnDatum> SpawnData;
        public CachedTag SoundEffectsCollection;
        public List<CrateInstance> Crates;
        public List<ScenarioPaletteEntry> CratePalette;
        public List<TagReferenceBlock> FlockPalette;
        public List<Flock> Flocks;
        public CachedTag SubtitleStrings;
        public List<CreatureInstance> Creatures;
        public List<ScenarioPaletteEntry> CreaturePalette;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioPaletteEntry> BigBattleCreaturePalette;

        public List<EditorFolder> EditorFolders;
        public CachedTag TerritoryLocationNameStrings;

        [TagField(Length = 0x8, Flags = Padding)]
        public byte[] Padding2;

        public List<TagReferenceBlock> MissionDialogue;
        public CachedTag ObjectiveStrings;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag InterpolatorsReach;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<Interpolator> Interpolators;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown127;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown128;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown129;

        public uint Unknown130;
        public uint Unknown131;
        public uint Unknown132;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        public List<SimulationDefinitionTableBlock> SimulationDefinitionTable;

        public CachedTag DefaultCameraFx;
        public CachedTag DefaultScreenFx;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag Unknown133;

        public CachedTag SkyParameters;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag AtmosphereGlobals;

        public CachedTag GlobalLighting;
        public CachedTag Lightmap;
        public CachedTag PerformanceThrottles;
        public List<ReferenceFrame> ObjectReferenceFrames;
        public List<AiObjective> AiObjectives;
        public List<DesignerZoneSet> DesignerZoneSets;
        public List<ScenarioZoneDebuggerBlock> ZoneDebugger;
        public List<ScenarioDecoratorBlock> Decorators;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagReferenceBlock> NeuticlePalette;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioCheapParticleSystemsBlock> Neuticles;

        public List<CinematicsBlock> Cinematics;
        public List<CinematicLightingBlock> CinematicLighting;
        public List<PlayerRepresentationBlock> OverridePlayerRepresentations;
        public List<ScenarioMetagameBlock> ScenarioMetagame;
        public List<SoftSurfaceBlock> SoftSurfaces;
        public List<ScenarioCubemapBlock> Cubemaps;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> CortanaEffects;

        public List<LightmapAirprobe> LightmapAirprobes;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

        [TagField(Platform = CachePlatform.MCC)]
        public List<NullBlock> ScavengerHuntObjects;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag MissionVisionModeEffect;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag MissionVisionModeTheaterEffect;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]     
        public CachedTag MissionVisionMode;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<TagReferenceBlock> Unknown155;

        [Flags]
        public enum BspFlags : int
        {
            None = 0,
            Bsp0 = 1 << 0,
            Bsp1 = 1 << 1,
            Bsp2 = 1 << 2,
            Bsp3 = 1 << 3,
            Bsp4 = 1 << 4,
            Bsp5 = 1 << 5,
            Bsp6 = 1 << 6,
            Bsp7 = 1 << 7,
            Bsp8 = 1 << 8,
            Bsp9 = 1 << 9,
            Bsp10 = 1 << 10,
            Bsp11 = 1 << 11,
            Bsp12 = 1 << 12,
            Bsp13 = 1 << 13,
            Bsp14 = 1 << 14,
            Bsp15 = 1 << 15,
            Bsp16 = 1 << 16,
            Bsp17 = 1 << 17,
            Bsp18 = 1 << 18,
            Bsp19 = 1 << 19,
            Bsp20 = 1 << 20,
            Bsp21 = 1 << 21,
            Bsp22 = 1 << 22,
            Bsp23 = 1 << 23,
            Bsp24 = 1 << 24,
            Bsp25 = 1 << 25,
            Bsp26 = 1 << 26,
            Bsp27 = 1 << 27,
            Bsp28 = 1 << 28,
            Bsp29 = 1 << 29,
            Bsp30 = 1 << 30,
            Bsp31 = 1 << 31
        }

        [Flags]
        public enum BspShortFlags : ushort
        {
            None = 0,
            Bsp0 = 1 << 0,
            Bsp1 = 1 << 1,
            Bsp2 = 1 << 2,
            Bsp3 = 1 << 3,
            Bsp4 = 1 << 4,
            Bsp5 = 1 << 5,
            Bsp6 = 1 << 6,
            Bsp7 = 1 << 7,
            Bsp8 = 1 << 8,
            Bsp9 = 1 << 9,
            Bsp10 = 1 << 10,
            Bsp11 = 1 << 11,
            Bsp12 = 1 << 12,
            Bsp13 = 1 << 13,
            Bsp14 = 1 << 14,
            Bsp15 = 1 << 15
        }

        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xAC, MinVersion = CacheVersion.HaloReach)]
        public class StructureBspBlock : TagStructure
		{
            public CachedTag StructureBsp;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag LocalStructureBsp;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Design;
            public CachedTag Lighting;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag LocalLighting;
            public ScenarioStructureSizeEnum SizeClass;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float HackyAmbientMinLuminance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DirectDraftAmbientMinLuminance;

            public float StructureVertexSink;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown4;

            public ushort Flags;
            public short DefaultSkyIndex;
            public ushort InstanceFadeStartPixels;
            public ushort InstanceFadeEndPixels;
            public CachedTag Cubemap;
            public CachedTag Wind;
            public uint ClosnedBspFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ScenarioLightmapSettingStruct LightmapSettings;
            [TagField(MinVersion = CacheVersion.HaloReach)]
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
            [TagField(ValidTags = new[] { "sddt" })]
            public CachedTag StructureDesign;
            [TagField(ValidTags = new[] { "sddt" })]
            public CachedTag LocalStructureDesign;
        }

        [TagStructure(Size = 0xC0)]
        public class PdaDefinitionsBlock : TagStructure
        {
            public StringId Name;
            public List<ScenarioPdaRenderModelBlock> RenderModels;
            public List<PdaRenderModelColorBlock> PdaRenderModelColors;
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag PdaValidMovementMap; // white is valid
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag PdaHeightMap;
            public float PdaHeightMapMin; // black
            public float PdaHeightMapMax; // white
            public float FalloffRadius;
            public RealPoint2d InitialPosition;
            public RealPoint3d MinimumWorldPosition;
            public RealPoint3d MaximumWorldPosition;
            public RealPoint3d MinimumCameraOffset; // wu
            public RealPoint3d MinimumCameraFocalOffset; // wu
            public RealPoint3d MaximumCameraOffset; // wu
            public RealPoint3d MaximumCameraFocalOffset; // wu
            public float InitialZoom; // [0,1]
            public float ZoomSpeed; // wu per tick
            public float MovementSpeed;
            public RealEulerAngles2d InitialRotation; // degrees
            public RealEulerAngles2d MinimumRotation; // degrees
            public RealEulerAngles2d MaximumRotation; // degrees
            public float RotationSpeed; // degrees per tick

            [TagStructure(Size = 0x1C)]
            public class ScenarioPdaRenderModelBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "mode" })]
                public CachedTag RenderModel;
                public short ModelColorIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] pad;
                public float ScaleX00Means10;
                public float ScaleY00Means10;
            }

            [TagStructure(Size = 0x10)]
            public class PdaRenderModelColorBlock : TagStructure
            {
                public RealRgbColor Color;
                public float Intensity;
            }
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class SkyReference : TagStructure
		{
            public CachedTag SkyObject;

            // mapping to the world unit
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CloudScale;
            // cloud movement speed
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CloudSpeed;
            // cloud movement direction, 0-360 degree
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CloudDirection;
            // red channel is used
            [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag CloudTexture;

            [TagField(Flags = Label)]
            public short NameIndex;
            public BspShortFlags ActiveBsps;
        }

        [TagStructure(Size = 0x2C)]
        public class ZoneSetPvsBlock : TagStructure
		{
            public BspFlags StructureBspMask;
            public short Version;
            public ZoneSetPvsFlags Flags;
            public List<BspChecksum> BspChecksums;
            public List<BspPvsBlock> StructureBspPvs;
            public List<PortalDeviceMappingBlock> PortaldeviceMapping;

            [Flags]
            public enum ZoneSetPvsFlags : ushort
            {
                EmptyDebugPvs = 1 << 0
            }

            [TagStructure(Size = 0x4)]
            public class BspChecksum : TagStructure
			{
                public uint Checksum;
            }

            [TagStructure(Size = 0x54, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
            public class BspPvsBlock : TagStructure
			{
                public List<ClusterPvsBlock> ClusterPvs;
                public List<ClusterPvsBlock> ClusterPvsDoorsClosed;
                public List<SkyIndicesBlock> AttachedSkyIndices;
                public List<SkyIndicesBlock> VisibleSkyIndices;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public List<BitVectorDword> MutipleSkiesVisibleBitvector;
                public List<BitVectorDword> ClusterAudioBitvector;
                public List<BspSeamClusterMapping> ClusterMappings;

                [TagStructure(Size = 0xC)]
                public class ClusterPvsBlock : TagStructure
				{
                    public List<CluserPvsBitVectorBlock> ClusterPvsBitVectors;

                    [TagStructure(Size = 0xC)]
                    public class CluserPvsBitVectorBlock : TagStructure
					{
                        public List<BitVectorDword> Bits;
                    }
                }

                [TagStructure(Size = 0x1)]
                public class SkyIndicesBlock : TagStructure
				{
                    public sbyte SkyIndex;
                }

                [TagStructure(Size = 0xC, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
                public class BspSeamClusterMapping : TagStructure
				{
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public List<ClusterReference> RootClusters;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public List<ClusterReference> AttachedClusters;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public List<ClusterReference> ConnectedClusters;

                    [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                    public List<ClusterReference> Clusters;

                    [TagStructure(Size = 0x1, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0x2, MinVersion = CacheVersion.HaloReach)]
                    public class ClusterReference : TagStructure
					{
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public sbyte BspIndex;
                        public sbyte ClusterIndex;
                    }
                }
            }

            [TagStructure(Size = 0x18, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
            public class PortalDeviceMappingBlock : TagStructure
			{
                public List<DevicePortalAssociation> DevicePortalAssociations;
                public List<GamePortalToPortalMapping> GamePortalToPortalMap;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<OccludingPortalToPortalMapping> OccludingPortalToPortalMap;

                [TagStructure(Size = 0xC)]
                public class DevicePortalAssociation : TagStructure
				{
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ScenarioObjectType ObjectType;
                    public ObjectSource Source;
                    public short FirstGamePortalIndex;
                    public ushort GamePortalCount;
                }

                [TagStructure(Size = 0x2)]
                public class GamePortalToPortalMapping : TagStructure
				{
                    public short PortalIndex;
                }

                [TagStructure(Size = 0x2)]
                public class OccludingPortalToPortalMapping : TagStructure
                {
                    public short PortalIndex;
                }
            }

            [TagStructure(Size = 0x4)]
            public class BitVectorDword : TagStructure
            {
                public DwordBits Bits;

                [Flags]
                public enum DwordBits : int
                {
                    None = 0,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15,
                    Bit16 = 1 << 16,
                    Bit17 = 1 << 17,
                    Bit18 = 1 << 18,
                    Bit19 = 1 << 19,
                    Bit20 = 1 << 20,
                    Bit21 = 1 << 21,
                    Bit22 = 1 << 22,
                    Bit23 = 1 << 23,
                    Bit24 = 1 << 24,
                    Bit25 = 1 << 25,
                    Bit26 = 1 << 26,
                    Bit27 = 1 << 27,
                    Bit28 = 1 << 28,
                    Bit29 = 1 << 29,
                    Bit30 = 1 << 30,
                    Bit31 = 1 << 31
                }
            }
        }

        public enum ObjectSource : sbyte
        {
            Structure,
            Editor,
            Dynamic,
            Legacy,
            Sky,
            Parent
        }

        [TagStructure(Size = 0x64)]
        public class ZoneSetAudibilityBlock : TagStructure
		{
            public int DoorPortalCount;
            public int RoomCount;
            public Bounds<float> RoomDistanceBounds;
            public List<EncodedDoorPa> EncodedDoorPas;
            public List<RoomDoorPortalEncodedPa> RoomDoorPortalEncodedPas;
            public List<AiDeafeningPa> AiDeafeningPas;
            public List<RoomDistance> RoomDistances;
            public List<GamePortalToDoorOccluderMapping> GamePortalToDoorOccluderMappings;
            public List<BspClusterToRoomBoundsMapping> BspClusterToRoomBoundsMappings;
            public List<BspClusterToRoomIndex> BspClusterToRoomIndices;

            [TagStructure(Size = 0x4)]
            public class EncodedDoorPa : TagStructure
			{
                public int EncodedData;
            }

            [TagStructure(Size = 0x4)]
            public class RoomDoorPortalEncodedPa : TagStructure
			{
                public int EncodedData;
            }

            [TagStructure(Size = 0x4)]
            public class AiDeafeningPa : TagStructure
			{
                public int EncodedData;
            }

            [TagStructure(Size = 0x1)]
            public class RoomDistance : TagStructure
			{
                public sbyte EncodedData;
            }

            [TagStructure(Size = 0x8)]
            public class GamePortalToDoorOccluderMapping : TagStructure
			{
                public int FirstDoorOccluderIndex;
                public int DoorOccluderCount;
            }

            [TagStructure(Size = 0x8)]
            public class BspClusterToRoomBoundsMapping : TagStructure
			{
                public int FirstRoomIndex;
                public int RoomIndexCount;
            }

            [TagStructure(Size = 0x2)]
            public class BspClusterToRoomIndex : TagStructure
			{
                public short RoomIndex;
            }
        }

        [Flags]
        public enum ZoneFlags : int
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15,
            Bit16 = 1 << 16,
            Bit17 = 1 << 17,
            Bit18 = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20,
            Bit21 = 1 << 21,
            Bit22 = 1 << 22,
            Bit23 = 1 << 23,
            Bit24 = 1 << 24,
            Bit25 = 1 << 25,
            Bit26 = 1 << 26,
            Bit27 = 1 << 27,
            Bit28 = 1 << 28,
            Bit29 = 1 << 29,
            Bit30 = 1 << 30,
            Bit31 = 1 << 31
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x13C, MinVersion = CacheVersion.HaloReach)]
        public class ZoneSet : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            [TagField(Length = 256, MinVersion = CacheVersion.HaloReach)]
            public string NameString;
            public int PvsIndex;
            public ZoneSetFlags Flags;
            public BspFlags Bsps;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ZoneFlags StructureDesignZones;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public BspFlags RuntimeBsps;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ZoneFlags RuntimeStructureDesignZones;

            public ZoneFlags RequiredDesignerZones;
            public ZoneFlags ForbiddenDesignerZones;
            public ZoneFlags CinematicZones;
            public int HintPreviousZoneSet;
            public int AudibilityIndex;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<PlanarFogZoneSetVisibilityBlock> PlanarFogVisibility;

            [Flags]
            public enum ZoneSetFlags : uint
            {
                None = 0,
                BeginLoadingNextLevel = 1 << 0,
                DebugPurposesOnly = 1 << 1,
                InteralZoneSet = 1 << 2
            }

            [TagStructure(Size = 0xC)]
            public class PlanarFogZoneSetVisibilityBlock : TagStructure
            {
                public List<PlanarFogStructureVisibilityBlock> StructureVisiblity;

                [TagStructure(Size = 0xC)]
                public class PlanarFogStructureVisibilityBlock : TagStructure
                {
                    public List<PlanarFogClusterVisibilityBlock> ClusterVisiblity;

                    [TagStructure(Size = 0xC)]
                    public class PlanarFogClusterVisibilityBlock : TagStructure
                    {
                        public List<PlanarFogReferenceBlock> AttachedFogs;

                        [TagStructure(Size = 0x4)]
                        public class PlanarFogReferenceBlock : TagStructure
                        {
                            public short StructureDesignIndex;
                            public short FogIndex;
                        }
                    }
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class LightingZoneSetBlock : TagStructure
        {
            public StringId Name;
            public uint RenderedBspFlags;
            public uint ExtraBspFlags;
        }

        [TagStructure(Size = 0xC)]
        public class BspAtlasBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public BspFlags Bsp;
            public BspFlags ConnectedBsps;
        }

        [TagStructure(Size = 0x4)]
        public class CampaignPlayer : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId PlayerRepresentationName;
        }

        [TagStructure(Size = 0x130)]
        public class Comment : TagStructure
		{
            public RealPoint3d Position;

            public TypeValue Type;

            [TagField(Flags = Label, Length = 32)]
            public string Name;

            [TagField(Length = 256)]
            public string Text;

            public enum TypeValue : int
            {
                Generic
            }
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class ObjectName : TagStructure
		{
            [TagField(Flags = Label, Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
            public string Name;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId NameReach;
            public GameObjectType ObjectType;
            public short PlacementIndex;
        }

        [Flags]
        public enum ObjectPlacementFlags : int
        {
            None = 0,
            NotAutomatically = 1 << 0,
            NotOnEasy = 1 << 1,
            NotOnNormal = 1 << 2,
            NotOnHard = 1 << 3,
            LockTypeToEnvObject = 1 << 4,
            LockTransformToEnvObject = 1 << 5,
            NeverPlaced = 1 << 6,
            LockNameToEnvObject = 1 << 7,
            CreateAtRest = 1 << 8,
            StoreOrientations = 1 << 9,
            Startup = 1 << 10,
            AttachPhysically = 1 << 11,
            AttachWithScale = 1 << 12,
            NoParentLighting = 1 << 13
        }

        [TagStructure(Size = 0x1C)]
        public class ObjectNodeOrientation : TagStructure
		{
            public short NodeCount;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
            public List<BitVector> BitVectors;
            public List<Orientation> Orientations;

            [TagStructure(Size = 0x1)]
            public class BitVector : TagStructure
			{
                public byte Data;
            }

            [TagStructure(Size = 0x2)]
            public class Orientation : TagStructure
			{
                public short Number;
            }
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
        public class ScenarioInstance : TagStructure
		{
            public short PaletteIndex;

            [TagField(Flags = Label)]
            public short NameIndex;

            public ObjectPlacementFlags PlacementFlags; // int
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            public float Scale;
            public List<ObjectNodeOrientation> NodeOrientations;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public ObjectTransformFlags TransformFlags;

            [TagField(Length = 0x3, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding1;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public BspPolicyValue BspPolicyReach;

            public ushort ManualBspFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ObjectTransformFlags TransformFlagsReach;

            public StringId LightAirprobeName;

            // object id
            public DatumHandle UniqueHandle;
            public short OriginBspIndex;
            public ScenarioObjectType ObjectType;
            public SourceValue Source; // sbyte

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public BspPolicyValue BspPolicy; // sbyte
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte EditingBoundToBsp;

            public short EditorFolder;

            [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding3;

            public ScenarioObjectParentStruct ParentId;
            public ushort CanAttachToBspFlags;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding4;


            public enum SourceValue : sbyte
            {
                Structure,
                Editor,
                Dynamic,
                Legacy,
                Sky,
                Parent
            }

            public enum BspPolicyValue : sbyte
            {
                Default,
                AlwaysPlaced,
                ManualBspIndex,
            }

            [Flags]
            public enum ObjectTransformFlags : ushort
            {
                Mirrored = 1 << 0
            }
        }

        public interface IMultiplayerInstance
        {
            MultiplayerObjectProperties Multiplayer { get; set; }
        }

        [TagStructure(Size = 0x34, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
        public class MultiplayerObjectProperties : TagStructure
        {
            [TagField(Length = 32, MinVersion = CacheVersion.HaloReach)]
            public string MegaloLabel;

            public GameEngineSymmetry Symmetry;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public GameEngineSubTypeFlags EngineFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public GameEngineFlagsReach EngineFlagsReach;

            [TagField(EnumType = typeof(short), MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.HaloReach)]
            public MultiplayerTeamDesignator Team;

            [TagField(Length = 2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] PaddingReach;

            public sbyte SpawnOrder;
            public sbyte QuotaMinimum;
            public sbyte QuotaMaximum;
            public MultiplayerObjectPlacementSpawnFlags SpawnFlags;
            public short SpawnTime;
            public short AbandonTime;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MultiplayerObjectRemappingPolicy RemappingPolicy;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BoundaryWidthRadiusReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BoundaryBoxLengthReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BoundaryPositiveHeightReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BoundaryNegativeHeightReach;

            public MultiplayerObjectBoundaryShape Shape;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectRemappingPolicy RemappingPolicyReach;

            public sbyte TeleporterChannel;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TeleporterPassabilityFlags TeleporterPassability;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId LocationName;

            [TagField(Length = 1, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Pad0;

            public ScenarioObjectParentStruct MapVariantParent;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float BoundaryWidthRadius;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float BoundaryBoxLength;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float BoundaryPositiveHeight;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float BoundaryNegativeHeight;

            public float RespawnWeight;
        }

        [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
        public class PermutationInstance : ScenarioInstance
        {
            public StringId Variant;
            public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
            public ArgbColor PrimaryColor;
            public ArgbColor SecondaryColor;
            public ArgbColor TertiaryColor;
            public ArgbColor QuaternaryColor;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public ArgbColor QuinaryColor;

            [Flags]
            public enum ScenarioObjectActiveChangeColorFlags : uint
            {
                Primary = 1 << 0,
                Secondary = 1 << 1,
                Tertiary = 1 << 2,
                Quaternary = 1 << 3
            }
        }

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class ScenarioPaletteEntry : TagStructure
		{
            public CachedTag Object;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown5;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown6;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown7;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown8;
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReach)]
        public class SceneryInstance : PermutationInstance, IMultiplayerInstance
        {
            public PathfindingPolicyValue PathfindingPolicy;
            public LightmappingPolicyValue LightmappingPolicy;
            public List<PathfindingReference> PathfindingReferences;
            public short HavokMoppIndex;
            public short AiSpawningSquad;
            public MultiplayerObjectProperties Multiplayer;

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }
        }

        [TagStructure(Size = 0x8)]
        public class BipedInstance : PermutationInstance
        {
            public float BodyVitalityFraction; // [0,1]
            public ScenarioUnitDatumFlags Flags;

            [Flags]
            public enum ScenarioUnitDatumFlags : uint
            {
                Dead = 1 << 0,
                Closed = 1 << 1,
                NotEnterableByPlayer = 1 << 2
            }
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class VehicleInstance : PermutationInstance, IMultiplayerInstance
        {
            public float BodyVitalityFraction; // [0,1]
            public ScenarioUnitDatumFlags Flags;
            public MultiplayerObjectProperties Multiplayer;

            [Flags]
            public enum ScenarioUnitDatumFlags : uint
            {
                Dead = 1 << 0,
                Closed = 1 << 1,
                NotEnterableByPlayer = 1 << 2
            }

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }
        }

        public enum PathfindingPolicyValue : short
        {
            TagDefault,
            Dynamic,
            CutOut,
            Standard,
            None,
        }

        public enum LightmappingPolicyValue : short
        {
            TagDefault,
            Dynamic,
            PerVertex,
        }

        [TagStructure(Size = 0x4)]
        public class PathfindingReference : TagStructure
        {
            public short BspIndex;
            public short PathfindingObjectIndex;
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloReach)]
        public class EquipmentInstance : ScenarioInstance, IMultiplayerInstance
        {
            public ScenarioEquipmentFlagsDefinition EquipmentFlags;
            public MultiplayerObjectProperties Multiplayer;

            [Flags]
            public enum ScenarioEquipmentFlagsDefinition : uint
            {
                OBSOLETE0 = 1 << 0,
                OBSOLETE1 = 1 << 1,
                DoesAcceleratemovesDueToExplosions = 1 << 2
            }

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class WeaponInstance : PermutationInstance, IMultiplayerInstance
        {
            public short RoundsLeft;
            public short RoundsLoaded;
            public ScenarioWeaponDatumFlags WeaponFlags;
            public MultiplayerObjectProperties Multiplayer;

            [Flags]
            public enum ScenarioWeaponDatumFlags : uint
            {
                InitiallyAtRestdoesntFall = 1 << 0,
                Obsolete = 1 << 1,
                DoesAcceleratemovesDueToExplosions = 1 << 2
            }

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }
        }

        [Flags]
        public enum DeviceGroupFlags : int
        {
            None = 0,
            OnlyUseOnce = 1 << 0
        }

        [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3ODST)]
        public class DeviceGroup : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string Name;
            public float InitialValue;
            public DeviceGroupFlags Flags;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short EditorFolderIndex;
            [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.Halo3ODST)]
            public byte[] Pad0;
        }

        [Flags]
        public enum ScenarioDeviceFlags : int
        {
            None = 0,
            InitiallyOpen = 1 << 0,
            InitiallyOff = 1 << 1,
            CanOnlyChangeOnce = 1 << 2,
            PositionReversed = 1 << 3,
            NotUsableFromAnySide = 1 << 4,
            ClosesWithoutPower = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloReach)]
        public class MachineInstance : ScenarioInstance
        {
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId Variant;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ScenarioObjectActiveChangeColorFlags ActiveChangeColors;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor PrimaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor SecondaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor TertiaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor QuaternaryColor;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public ArgbColor QuinaryColor;

            [Flags]
            public enum ScenarioObjectActiveChangeColorFlags : uint
            {
                Primary = 1 << 0,
                Secondary = 1 << 1,
                Tertiary = 1 << 2,
                Quaternary = 1 << 3
            }

            public short PowerGroup;
            public short PositionGroup;
            public ScenarioDeviceFlags DeviceFlags;
            public ScenarioMachineFlags MachineFlags;
            public List<PathfindingReference> PathfindingReferences;
            public PathfindingPolicyValue PathfindingPolicy;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Pad0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectProperties Multiplayer;

            [Flags]
            public enum ScenarioMachineFlags : int
            {
                None = 0,
                DoesNotOperateAutomatically = 1 << 0,
                OneSided = 1 << 1,
                NeverAppearsLocked = 1 << 2,
                OpenedByMeleeAttack = 1 << 3,
                OneSidedForPlayer = 1 << 4,
                DoesNotCloseAutomatically = 1 << 5,
                IgnoresPlayer = 1 << 6,
                IgnoresAi = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15
            }

            [TagStructure(Size = 0x4)]
            public class PathfindingReference : TagStructure
			{
                public short BspIndex;
                public short PathfindingObjectIndex;
            }

            public enum PathfindingPolicyValue : short
            {
                TagDefault,
                CutOut,
                Sectors,
                Discs,
                None,
            }
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
        public class TerminalInstance : ScenarioInstance
        {
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId Variant;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public byte ActiveChangeColors;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown7;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown8;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown9;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor PrimaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor SecondaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor TertiaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor QuaternaryColor;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown10;

            public short PowerGroup;
            public short PositionGroup;
            public uint DeviceFlags;
            public uint MachineFlags; // PahPah?
        }

        [TagStructure(Size = 0x4C)]
        public class AlternateRealityDeviceInstance : PermutationInstance
        {
            public short PowerGroup;
            public short PositionGroup;
            public uint DeviceFlags;
            [TagField(Length = 32)]
            public string TapScriptName;
            [TagField(Length = 32)]
            public string HoldScriptName;
            public short TapScriptIndex;
            public short HoldScriptIndex;
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x84, MinVersion = CacheVersion.HaloReach)]
        public class ControlInstance : ScenarioInstance
        {
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId Variant;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public byte ActiveChangeColors;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown7;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown8;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown9;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor PrimaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor SecondaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor TertiaryColor;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ArgbColor QuaternaryColor;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown10;

            public short PowerGroup;
            public short PositionGroup;
            public ScenarioDeviceFlags DeviceFlags;
            public ScenarioControlFlags ControlFlags;
            public short Unknown11;
            public short Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ScenarioControlCharacterTypes AllowedPlayers;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectProperties Multiplayer;

            [Flags]
            public enum ScenarioControlFlags : int
            {
                None = 0,
                UsableFromBothSides = 1 << 0,
                Bit1 = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7
            }

            public enum ScenarioControlCharacterTypes : short
            {
                Any,
                Spartan,
                Elite
            }
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloReach)]
        public class SoundSceneryInstance : ScenarioInstance
        {
            public int VolumeType;
            public float Height;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> OverrideDistance;
            public Bounds<Angle> OverrideConeAngle;
            public float OverrideOuterConeGain;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public SoundDistanceParameters DistanceParameters;
        }

        [TagStructure(Size = 0x18)]
        public class GiantInstance : PermutationInstance
        {
            public float BodyVitalityPercentage;
            public uint Flags;
            public short Unknown11;
            public short Unknown12;
            public List<PathfindingReference> PathfindingReferences;

            [TagStructure(Size = 0x4)]
            public class PathfindingReference : TagStructure
			{
                public short BspIndex;
                public short PathfindingObjectIndex;
            }
        }

        [TagStructure(Size = 0x0, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
        public class EffectSceneryInstance : ScenarioInstance
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectProperties Multiplayer;
        }

        [TagStructure(Size = 0x38)]
        public class LightVolumeInstance : ScenarioInstance
        {
            public short PowerGroup;
            public short PositionGroup;
            public uint DeviceFlags;
            public TypeValue2 Type2;
            public ushort Flags;
            public LightmapTypeValue LightmapType;
            public ushort LightmapFlags;
            public float LightmapHalfLife;
            public float LightmapLightScale;
            public float X;
            public float Y;
            public float Z;
            public float Width;
            public float HeightScale;
            public Angle FieldOfView;
            public float FalloffDistance;
            public float CutoffDistance;

            public enum TypeValue2 : short
            {
                Sphere,
                Projective,
            }

            public enum LightmapTypeValue : short
            {
                UseLightTagSetting,
                DynamicOnly,
                DynamicWithLightmaps,
                LightmapsOnly,
            }
        }

        [TagStructure(Size = 0x30)]
        public class SandboxObject : TagStructure
		{
            public CachedTag Object;
            [TagField(Flags = Label)]
            public StringId Name;
            public int MaxAllowed;
            public float Cost;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }

        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
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

                [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
                [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo4)]
                public class MapVariantObjectVariantBlock : TagStructure
                {
                    public StringId DisplayName;
                    [TagField(ValidTags = new[] { "obje" })]
                    public CachedTag Object;
                    public StringId VariantName;

                    [TagField(MinVersion = CacheVersion.Halo4)]
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

        [Flags]
        public enum SoftCeilingFlags : ushort
        {
            None = 0,
            IgnoreBipeds = 1 << 0,
            IgnoreVehicles = 1 << 1,
            IgnoreCamera = 1 << 2,
            IgnoreHugeVehicles = 1 << 3
        }

        public enum SoftCeilingType : short
        {
            Acceleration,
            SoftKill,
            SlipSurface
        }

        [TagStructure(Size = 0xC)]
        public class SoftCeiling : TagStructure
		{
            public SoftCeilingFlags Flags;
            public SoftCeilingFlags RuntimeFlags;
            [TagField(Flags = Label)]
            public StringId Name;
            public SoftCeilingType Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x58, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x68, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x58, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public class PlayerStartingProfileBlock : TagStructure
        {
            [TagField(Flags = Label, Length = 32)]
            public string Name;
            public float StartingHealthDamage;
            public float StartingShieldDamage;
            public CachedTag PrimaryWeapon;
            public short PrimaryRoundsLoaded;
            public short PrimaryRoundsTotal;
            public CachedTag SecondaryWeapon;
            public short SecondaryRoundsLoaded;
            public short SecondaryRoundsTotal;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;
            public byte StartingFragGrenadeCount;
            public byte StartingPlasmaGrenadeCount;
            public byte StartingSpikeGrenadeCount;
            public byte StartingFirebombGrenadeCount;
            [TagField(ValidTags = new[] { "eqip" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag StartingEquipment;
            [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagField(Platform = CachePlatform.MCC)]
            public short EditorFolder;
            [TagField(Flags = TagFieldFlags.Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagField(Flags = TagFieldFlags.Padding, Length = 2, Platform = CachePlatform.MCC)]
            public byte[] Padding;
        }

        public enum PlayerUnitTypeValue : short
        {
            MasterChief,
            Dervish,
            ChiefMultiplayer,
            EliteMultiplayer,
            EliteCoop,
            Monitor
        }

        [Flags]
        public enum PlayerStartingLocationFlags : ushort
        {
            None = 0,
            SurvivalMode = 1 << 0,
            SurvivalModeElite = 1 << 1
        }

        public enum TriggerVolumeType : short
        {
            BoundingBox,
            Sector
        }

        [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST)]
        public class PlayerStartingLocation : TagStructure
		{
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
            public short InsertionPointIndex;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public PlayerUnitTypeValue UnitType;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public PlayerStartingLocationFlags Flags;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short EditorFolderIndex;

            [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x7C, MinVersion = CacheVersion.Halo3ODST)]
        public class TriggerVolume : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            public short ObjectName;
            public short RuntimeNodeIndex;
            public StringId NodeName;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public TriggerVolumeType Type;

            [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused;

            public RealVector3d Forward;
            public RealVector3d Up;
            public RealPoint3d Position;
            public RealPoint3d Extents;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float ZSink; // this is only valid for sector type trigger volumes

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<SectorPoint> SectorPoints;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<RuntimeTriangle> RuntimeTriangles;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> RuntimeSectorXBounds;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> RuntimeSectorYBounds;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> RuntimeSectorZBounds;

            public float C;

            public short KillVolume;
            public short EditorFolderIndex;

            [TagStructure(Size = 0x14)]
            public class SectorPoint : TagStructure
			{
                public RealPoint3d Position;
                public RealEulerAngles2d Normal;
            }

            [TagStructure(Size = 0x50, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
            public class RuntimeTriangle : TagStructure
			{
                public RealPlane3d Plane0;
                public RealPlane3d Plane1;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public RealPlane3d Plane2;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public RealPlane3d Plane3;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public RealPlane3d Plane4;

                public RealPoint2d Vertex0;
                public RealPoint2d Vertex1;
                public RealPoint2d Vertex2;

                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
                public byte[] Padding1;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsX0;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsX1;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsY0;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsY1;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsZ0;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float BoundsZ1;
            }
        }

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo4)]
        public class ScenarioAcousticSectorBlockStruct : TagStructure
        {
            public List<AcousticSectorPointBlock> Points;
            public RealPlane3d TopPlane;
            public RealPlane3d BottomPlane;

            [TagField(MinVersion = CacheVersion.Halo4)]
            public AcousticpaletteFlags Flags;
            [TagField(MinVersion = CacheVersion.Halo4)]
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

        [TagStructure(Size = 0x40)]
        public class RecordedAnimation : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Unused1;
            public short LengthOfAnimation;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unused3;
            public byte[] RecordedAnimationEventStream;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST)]
        public class ZoneSetSwitchTriggerVolume : TagStructure
		{
            public FlagBits Flags;
            public short BeginZoneSet;
            public short TriggerVolume;
            public short CommitZoneSet;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown2;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown3;

            [Flags]
            public enum FlagBits : ushort
            {
                None,
                TeleportVehicles = 1 << 0
            }
        }

        [TagStructure(Size = 0x18)]
        public class ScenarioNamedLocationVolumeBlock : TagStructure
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

        [TagStructure(Size = 0x14)]
        public class PlayerSpawnInfluencerBlock : TagStructure
		{
            public float OverrideFullWeightRadius; // wu
            public float OverrideFalloffRadius; // wu
            public float OverrideUpperHeight; // wu
            public float OverrideLowerHeight; // wu
            public float OverrideWeight;
        }

        [TagStructure(Size = 0x20)]
        public class WeaponSpawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "weap" })]
            public CachedTag Weapon;
            public float FullWeightRange; // wu
            public float FallOffRange; // wu
            public float FallOffConeRadius; // wu
            public float Weight;
        }

        [TagStructure(Size = 0x20)]
        public class VehicleSpawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "vehi" })]
            public CachedTag Vehicle;
            public float PillRadius; // wu
            public float LeadTime; // seconds
            public float MinimumVelocity; // wu/sec
            public float Weight;
        }

        [TagStructure(Size = 0x1C)]
        public class ProjectileSpawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "proj" })]
            public CachedTag Projectile;
            public float LeadTime; // seconds
            public float CollisionCylinderRadius; // wu
            public float Weight;
        }

        [TagStructure(Size = 0x14)]
        public class EquipmentSpawnInfluenceBlock : TagStructure
        {
            public CachedTag Equipment;
            public float Weight;
        }

        [TagStructure(Size = 0x14)]
        public class NetgameGoalInfluencerBlock : TagStructure
        {
            public float OverrideFullWeightRadius; // wu
            public float OverrideFallOffRadius; // wu
            public float OverrideUpperCylinderHeight; // wu
            public float OverrideLowerCylinderHeight; // wu
            public float OverrideWeight;
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
        public class Decal : TagStructure
		{
            public short DecalPaletteIndex;
            public FlagBits Flags;
            public byte Unknown1;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Scale;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> ScaleReach;

            [Flags]
            public enum FlagBits : byte
            {
                None,
                ForcePlaner = 1 << 0,
                ProjectUVs = 1 << 1
            }
        }

        [TagStructure(Size = 0x28)]
        public class SquadGroup : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string Name = "";

            public short ParentIndex = -1;

            [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
            public byte[] Unused1 = new byte[2];

            public short InitialObjective = -1;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused2 = new byte[2];

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short EditorFolderIndex = -1;
        }

        [Flags]
        public enum SquadFlags : int
        {
            None = 0,
            Bit0 = 1 << 0,
            Blind = 1 << 1,
            Deaf = 1 << 2,
            Braindead = 1 << 3,
            InitiallyPlaced = 1 << 4,
            UnitsNotEnterableByPlayer = 1 << 5,
            FireteamAbsorber = 1 << 6,
            SquadIsRuntime = 1 << 7,
            NoWaveSpawn = 1 << 8,
            SquadIsMusketeer = 1 << 9
        }

        [Flags]
        public enum SquadDifficultyFlags : ushort
        {
            None = 0,
            Easy = 1 << 0,
            Normal = 1 << 1,
            Heroic = 1 << 2,
            Legendary = 1 << 3
        }

        public enum SquadMovementMode : short
        {
            Default,
            Climbing,
            Flying
        }

        public enum SquadPatrolMode : short
        {
            PingPong,
            Loop,
            Random
        }

        public enum SquadActivity : short
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

        [Flags]
        public enum SquadPointFlags : ushort
        {
            None = 0,
            SingleUse = 1 << 0
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class SquadPoint : TagStructure
		{
            public short PointIndex;
            public SquadPointFlags Flags;
            public float Delay;
            public float AngleDegrees;
            public StringId ActivityName;
            public SquadActivity Activity;
            public short ActivityVariant;

            [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
            public string CommandScriptName;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId CommandScriptNameReach;

            public short CommandScriptIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
        }

        public enum SquadSeatType : short
        {
            Default,
            Passenger,
            Gunner,
            Driver,
            OutOfVehicle,
            VehicleOnly = 6,
            Passenger2
        }

        public enum SquadGrenadeType : short
        {
            None,
            Frag,
            Plasma,
            Spike,
            Fire
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST)]
        public class AiConditions : TagStructure
        {
            public SquadDifficultyFlags DifficultyFlags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;

            [TagField(Length = 2, MinVersion = CacheVersion.Halo3ODST)]
            public short[] RoundRange = new short[2];

            [TagField(Length = 2, MinVersion = CacheVersion.Halo3ODST)]
            public short[] SetRange = new short[2];
        }

        [TagStructure(Size = 0x1C)]
        public class AiPoint3d : TagStructure
        {
            public RealPoint3d Position;
            public short ReferenceFrame;
            public short BspIndex;
            public RealEulerAngles3d Facing;
        }

        [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x68, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloReach11883)]
        public class Squad : TagStructure
		{
            /// <summary>
            /// The name of the squad.
            /// </summary>
            [TagField(Flags = Label, Length = 32)]
            public string Name;

            /// <summary>
            /// The flags of the squad.
            /// </summary>
            public SquadFlags Flags;

            /// <summary>
            /// The team the squad is on.
            /// </summary>
            public GameTeam Team;

            /// <summary>
            /// The index of the parent group of the squad.
            /// </summary>
            public short ParentSquadGroupIndex;

            /// <summary>
            /// The initial zone index the squad is placed on.
            /// </summary>
            public short InitialZoneIndex;

            [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
            public byte[] Unused1 = new byte[2];
            
            public short ObjectiveIndex;

            public short ObjectiveRoleIndex;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short EditorFolderIndexNew = -1;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<Fireteam> Fireteams;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<SpawnFormation> SpawnFormations;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<SpawnPoint> SpawnPoints;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public short EditorFolderIndexOld;

            [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
            public byte[] Unused2 = new byte[2];

            // Filter which squads in Firefight waves can be spawned into this squad
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public WavePlacementFilterEnum WavePlacementFilter;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId ModuleId;

            [TagField(Flags = Short, MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag SquadTemplate;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<Fireteam> DesignerFireteams;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<Fireteam> TemplatedFireteams;

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

            [TagStructure(Size = 0x60, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x84, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReach)]
            public class Fireteam : TagStructure
            {
                [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST)]
                public StringId Name;

                public AiConditions SpawnConditions;

                public short SpawnCount;
                public short MajorUpgrade;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short CharacterTypeIndex;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<CharacterTypeBlock> CharacterType;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short InitialPrimaryWeaponIndex;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<ItemTypeBlock> InitialPrimaryWeapon;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short InitialSecondaryWeaponIndex;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<ItemTypeBlock> InitialSecondaryWeapon;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<ItemTypeBlock> InitialEquipment;

                public SquadGrenadeType GrenadeType;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short InitialEquipmentIndex;

                public short VehicleTypeIndex;
                public StringId VehicleVariant;

                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string CommandScriptName;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId CommandScriptNameReach;

                public short CommandScriptIndex;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused1 = new byte[2];

                public StringId ActivityName;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId MovementSet;

                [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
                public byte[] Unused2 = new byte[2];

                public short PointSetIndex;
                public SquadPatrolMode PatrolMode;

                [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
                public byte[] Unused3 = new byte[2];

                public List<SquadPoint> PatrolPoints;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public List<SpawnPoint> StartingLocations;

                [TagStructure(Size = 0x10)]
                public class CharacterTypeBlock : TagStructure
                {
                    public AiConditions SpawnConditions;
                    public short CharacterTypeIndex;
                    public short Chance;
                }

                [TagStructure(Size = 0x10)]
                public class ItemTypeBlock : TagStructure
                {
                    public AiConditions SpawnConditions;
                    public short ItemTypeIndex;
                    public short Probability;
                }
            }

            [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
            public class SpawnFormation : TagStructure
			{
                public AiConditions SpawnConditions;
                [TagField(Flags = Label)]
                public StringId Name;

                public AiPoint3d Point;

                public StringId FormationType;
                public uint InitialMovementDistance;
                public SquadMovementMode InitialMovementMode;
                public short PlacementScriptIndex;

                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string PlacementScriptName;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId PlacementScriptNameReach;
                public StringId InitialState;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId MovementSet;
                public short PointSetIndex;
                public SquadPatrolMode PatrolMode;
                public List<SquadPoint> PatrolPoints;
            }

            public enum SpawnPointFlags : ushort
            {
                None = 0,
                InfectionFormExplode = 1 << 0,
                Nothing = 1 << 2,
                AlwaysPlace = 1 << 3,
                InitiallyHidden = 1 << 4,
                VehicleDestroyedWhenNoDriver = 1 << 5,
                VehicleOpen = 1 << 6,
                ActorSurfaceEmerge = 1 << 7,
                ActorSurfaceEmergeAuto = 1 << 8,
                ActorSurfaceEmergeUpwards = 1 << 9
            }

            [TagStructure(Size = 0x88, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x90, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x7C, MinVersion = CacheVersion.HaloReach)]
            public class SpawnPoint : TagStructure
			{
                public AiConditions Condition;

                [TagField(Flags = Label)]
                public StringId Name;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short FireteamIndex;

                [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
                public byte[] Unused1 = new byte[2];

                public AiPoint3d Point;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short Unknown5;

                public SpawnPointFlags Flags;
                public short CharacterTypeIndex;
                public short InitialPrimaryWeaponIndex;
                public short InitialSecondaryWeaponIndex;

                [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
                public byte[] Unused2 = new byte[2];

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short InitialEquipmentIndexNew;

                public short VehicleTypeIndex;
                public SquadSeatType SeatType;
                public SquadGrenadeType InitialGrenades;
                public short SwarmCount;

                [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
                public byte[] Unused3 = new byte[2];

                public StringId ActorVariant;
                public StringId VehicleVariant;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId VoiceDesignator;

                public float InitialMovementDistance;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SquadMovementMode InitialMovementModeNew;

                public short EmitterVehicleIndex;
                public short EmitterGiantIndex;
                public short EmitterBipedIndex;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public SquadMovementMode InitialMovementModeOld;

                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string PlacementScriptName;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId PlacementScriptNameReach;

                public short PlacementScriptIndex;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short InitialEquipmentIndexOld;

                [TagField(Flags = TagFieldFlags.Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
                public byte[] Unused4 = new byte[2];

                public StringId ActivityName;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId MovementSet;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public int Unknown; // ???

                public short PointSetIndex;
                public SquadPatrolMode PatrolMode;

                public List<SquadPoint> PatrolPoints;
            }
        }

        [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
        public class Zone : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string Name;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public ZoneFlagsOld FlagsOld;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ZoneFlagsNew FlagsNew;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public short ManualBspIndex;

            public ushort BspFlags;

            public List<FiringPosition> FiringPositions;
            public List<Area> Areas;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown;

            [Flags]
            public enum ZoneFlagsOld : int
            {
                None,
                UsesManualBspIndex = 1 << 0
            }

            [Flags]
            public enum ZoneFlagsNew : ushort
            {
                None,
                GiantsZone = 1 << 0
            }

            [TagStructure(Size = 0x28)]
            public class FiringPosition : TagStructure
			{
                public RealPoint3d Position;
                public short ReferenceFrame;
                public short BspIndex;
                public FlagsValue Flags;
                public PostureFlagsValue PostureFlags;
                public short AreaIndex;
                public short ClusterIndex;
                public short SectorBspIndex;
                public short SectorIndex;
                public RealEulerAngles2d Normal;
                public Angle Yaw;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None = 0,
                    Open = 1 << 0,
                    Partial = 1 << 1,
                    Closed = 1 << 2,
                    Mobile = 1 << 3,
                    WallLean = 1 << 4,
                    Perch = 1 << 5,
                    GroundPoint = 1 << 6,
                    DynamicCoverPoint = 1 << 7,
                    AutomaticallyGenerated = 1 << 8,
                    NavVolume = 1 << 9,
                    CenterBunkering = 1 << 10
                }

                [Flags]
                public enum PostureFlagsValue : ushort
                {
                    None = 0,
                    CornerLeft = 1 << 0,
                    CornerRight = 1 << 1,
                    Bunker = 1 << 2,
                    BunkerHigh = 1 << 3,
                    BunkerLow = 1 << 4
                }
            }

            [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0xA8, MinVersion = CacheVersion.Halo3ODST)]
            public class Area : TagStructure
			{
                [TagField(Flags = Label, Length = 32)]
                public string Name;
                public AreaFlags Flags;
                public RealPoint3d RuntimeRelativeMeanPoint;
                public short RuntimeRelativeReferenceFrame;
                public short RuntimeRelativeBspIndex;
                public float RuntimeStandardDeviation;
                public short FiringPositionStartIndex;
                public short FiringPositionCount;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short ManualReferenceFrameNew;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short BSPIndex;

                [TagField(Length = 8)]
                public int[] ClusterOccupancyBitVector = new int[8];

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short ManualReferenceFrameOld;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short BSPIndexOld;

                public List<FlightHint> FlightHints;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<Point> Points;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short GenerationPreset = 1;

                [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3ODST)]
                public byte[] Unused = new byte[2];

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public AreaGenerationFlags GenerationFlags = AreaGenerationFlags.IgnoreExisting;

                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown16 = 0.5f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown17 = 0.5f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown18 = 0.0f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown19 = 0.0f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown20 = 1.0f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown21 = 1.0f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown22 = 0.2f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown23 = 0.7f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown24 = 0.25f;
                [TagField(MinVersion = CacheVersion.Halo3ODST)] public float Unknown25 = 0.5f;

                [Flags]
                public enum AreaFlags : int
                {
                    None = 0,
                    VehicleArea = 1 << 0,
                    Perch = 1 << 1,
                    ManualReferenceFrame = 1 << 2,
                    TurretDeploymentArea = 1 << 3,
                    InvalidSectorDef = 1 << 4
                }

                [Flags]
                public enum AreaGenerationFlags : int
                {
                    None = 0,
                    ExcludeCover = 1 << 0,
                    IgnoreExisting = 1 << 1,
                    GenerateRadial = 1 << 2,
                    DoNotStagger = 1 << 3,
                    Airborne = 1 << 4,
                    AirborneStagger = 1 << 5,
                    ContinueCasting = 1 << 6
                }

                [TagStructure(Size = 0x8)]
                public class FlightHint : TagStructure
				{
                    public short FlightHintIndex;
                    public short PointIndex;
                    public short BSPIndex;
                    public short Unknown2;
                }

                [TagStructure(Size = 0x18)]
                public class Point : TagStructure
				{
                    public RealPoint3d Position;
                    public short ReferenceFrame = -1;
                    public short BspIndex;
                    public RealEulerAngles2d Facing;
                }
            }
        }

        [TagStructure(Size = 0x2C)]
        public class SquadPatrol : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public List<Squad> Squads;
            public List<Point> Points;
            public List<Transition> Transitions;
            public short EditorFolderIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;

            [TagStructure(Size = 0x4)]
            public class Squad : TagStructure
			{
                public short SquadIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;
            }

            [TagStructure(Size = 0x14)]
            public class Point : TagStructure
			{
                public short ObjectiveIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;
                public float HoldTime;
                public float SearchTime;
                public float PauseTime;
                public float CooldownTime;
            }

            [TagStructure(Size = 0x10)]
            public class Transition : TagStructure
			{
                public short Point1Index;
                public short Point2Index;
                public List<Waypoint> Waypoints;

                [TagStructure(Size = 0x14)]
                public class Waypoint : TagStructure
				{
                    public RealPoint3d Position;
                    public short ManualReferenceFrame;
                    public short BspIndex;
                    public int SurfaceIndex;
                }
            }
        }

        [TagStructure(Size = 0x20)]
        public class MissionScene : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public FlagBits Flags;
            public List<TriggerCondition> TriggerConditions;
            public List<Role> Roles;

            [Flags]
            public enum FlagBits : int
            {
                None,
                SceneCanPlayMultipleTimes = 1 << 0,
                EnableCombatDialogue = 1 << 1
            }

            [TagStructure(Size = 0x4)]
            public class TriggerCondition : TagStructure
			{
                public RuleValue CombinationRule;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;

                public enum RuleValue : short
                {
                    Or,
                    And
                }
            }

            [TagStructure(Size = 0x14)]
            public class Role : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public GroupValue Group;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;
                public List<Variant> Variants;

                public enum GroupValue : short
                {
                    Group1,
                    Group2,
                    Group3
                }

                [TagStructure(Size = 0x4)]
                public class Variant : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId Name;
                }
            }
        }

        [TagStructure(Size = 0x94)]
        public class PathfindingDataBlock : TagStructure
        {
            public List<SectorBlock> Sectors;
            public List<SectorLinkBlock> Links;
            public List<RefBlock> Bsp2dRefs;
            public List<SectorBsp2dNodesBlock> Bsp2dNodes;
            public List<SectorVertexBlock> Vertices;
            public List<EnvironmentObjectRefs> ObjectRefs;
            public List<PathfindingHintsBlock> PathfindingHints;
            public List<InstancedGeometryReferenceBlock> InstancedGeometryRefs;
            public int StructureChecksum;
            public List<GiantPathfindingDataBlock> GiantPathfindingData;
            public List<PfSeamBlock> Seams;
            public List<PfJumpSeamBlock> JumpSeams;
            public List<PfDoorBlock> Doors;

            [TagStructure(Size = 0x8)]
            public class SectorBlock : TagStructure
            {
                public SectorFlags PathFindingSectorFlags;
                public short HintIndex;
                public int FirstLinkDoNotSetManually;

                [Flags]
                public enum SectorFlags : ushort
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
                    SectorInterior = 1 << 13,
                    SectorRail = 1 << 14,
                    SectorUser = 1 << 15
                }
            }

            [TagStructure(Size = 0x10)]
            public class SectorLinkBlock : TagStructure
            {
                public short Vertex1;
                public short Vertex2;
                public SectorLinkFlags LinkFlags;
                public short HintIndex;
                public short ForwardLink;
                public short ReverseLink;
                public short LeftSector;
                public short RightSector;

                [Flags]
                public enum SectorLinkFlags : ushort
                {
                    SectorLinkFromCollisionEdge = 1 << 0,
                    SectorIntersectionLink = 1 << 1,
                    SectorLinkBsp2dCreationError = 1 << 2,
                    SectorLinkTopologyError = 1 << 3,
                    SectorLinkChainError = 1 << 4,
                    SectorLinkThreshold = 1 << 5,
                    SectorLinkCrouchable = 1 << 6,
                    SectorLinkWallBase = 1 << 7,
                    SectorLinkLedge = 1 << 8,
                    SectorLinkLeanable = 1 << 9,
                    SectorLinkStartCorner = 1 << 10,
                    SectorLinkEndCorner = 1 << 11,
                    SectorLinkSeam = 1 << 12
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

            [TagStructure(Size = 0xC)]
            public class SectorVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }

            [TagStructure(Size = 0x18)]
            public class EnvironmentObjectRefs : TagStructure
            {
                public EnvironmentObjectFlags Flags;
                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] OGJM;
                public List<EnvironmentObjectBspRefs> Bsps;
                public ScenarioObjectIdStruct ObjectId;

                [Flags]
                public enum EnvironmentObjectFlags : ushort
                {
                    Mobile = 1 << 0
                }

                [TagStructure(Size = 0x18)]
                public class EnvironmentObjectBspRefs : TagStructure
                {
                    public int BspReference;
                    public short NodeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] NWLLGJU;
                    public List<RefBlock> Bsp2dRefs;
                    public int VertexOffset;

                    [TagStructure(Size = 0x4)]
                    public class RefBlock : TagStructure
                    {
                        public int NodeRefOrSectorRef;
                    }
                }

                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnumDefinition Type;
                    public ObjectSourceEnumDefinition Source;

                    public enum ObjectTypeEnumDefinition : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        ArgDevice,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery
                    }

                    public enum ObjectSourceEnumDefinition : sbyte
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

            [TagStructure(Size = 0x14)]
            public class PathfindingHintsBlock : TagStructure
            {
                public HintTypeEnum HintType;
                public short NextHintIndex;
                public int HintData0;
                public int HintData1;
                public int HintData2;
                public int HintData3;

                public enum HintTypeEnum : short
                {
                    IntersectionLink,
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    BreakableFloor,
                    RailLink,
                    SeamLink,
                    DoorLink
                }
            }

            [TagStructure(Size = 0x4)]
            public class InstancedGeometryReferenceBlock : TagStructure
            {
                public short PathfindingObjectIndex;
                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] RWU;
            }

            [TagStructure(Size = 0x4)]
            public class GiantPathfindingDataBlock : TagStructure
            {
                public int Bsp2dRoot;
            }

            [TagStructure(Size = 0xC)]
            public class PfSeamBlock : TagStructure
            {
                public List<PfSeamLinkBlock> LinkMappings;

                [TagStructure(Size = 0x4)]
                public class PfSeamLinkBlock : TagStructure
                {
                    public int LinkIndex;
                }
            }

            [TagStructure(Size = 0x14)]
            public class PfJumpSeamBlock : TagStructure
            {
                public short UserJumpIndex;

                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] Padding0;

                public float RailLength;
                public List<PfJumpIndexBlock> JumpHints;

                [TagStructure(Size = 0x4)]
                public class PfJumpIndexBlock : TagStructure
                {
                    public short JumpIndex;
                    [TagField(Length = 0x2, Flags = Padding)]
                    public byte[] Padding0;
                }
            }

            [TagStructure(Size = 0x4)]
            public class PfDoorBlock : TagStructure
            {
                public short ScenarioMachineIndex;

                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] Padding0;
            }
        }

        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x84, MinVersion = CacheVersion.HaloReach)]
        public class UserHintBlock : TagStructure
		{
            public List<LineSegment> LineSegments;
            public List<Parallelogram> Parallelograms;
            public List<JumpHint> JumpHints;
            public List<ClimbHint> ClimbHints;
            public List<WellHint> WellHints;
            public List<FlightHint> FlightHints;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<TriggerVolume> CookieCutters;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<UnknownBlock8> Unknown8;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<UnknownBlock9> Unknown9;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<GNullBlock> Unknown10;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<GNullBlock> Unknown11;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<GNullBlock> Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<GNullBlock> Unknown13;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<GNullBlock> Unknown14;


            [Flags]
            public enum UserHintShortFlags : short
            {
                None,
                Bidirectional = 1 << 0,
                Closed = 1 << 1
            }

            [Flags]
            public enum UserHintLongFlags : int
            {
                None,
                Bidirectional = 1 << 0,
                Closed = 1 << 1
            }

            [TagStructure(Size = 0x24)]
            public class LineSegment : TagStructure
			{
                public UserHintLongFlags Flags;

                public RealPoint3d Point0;
                public short ReferenceUnknown0;
                public short ReferenceFrame0;

                public RealPoint3d Point1;
                public short ReferenceUnknown1;
                public short ReferenceFrame1;
            }

            [TagStructure(Size = 0x48)]
            public class Parallelogram : TagStructure
			{
                public UserHintLongFlags Flags;

                public RealPoint3d Point0;
                public short ReferenceUnknown0;
                public short ReferenceFrame0;

                public RealPoint3d Point1;
                public short ReferenceUnknown1;
                public short ReferenceFrame1;

                public RealPoint3d Point2;
                public short ReferenceUnknown2;
                public short ReferenceFrame2;

                public RealPoint3d Point3;
                public short ReferenceUnknown3;
                public short ReferenceFrame3;

                public short Unknown1;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;
            }

            [TagStructure(Size = 0x8)]
            public class JumpHint : TagStructure
			{
                public UserHintShortFlags Flags;
                public short ParallelogramIndex;
                public GlobalAiJumpHeight ForceJumpHeight;
                public ControlFlagsValue ControlFlags;

                [Flags]
                public enum ControlFlagsValue : ushort
                {
                    None,
                    MagicLift = 1 << 0
                }
            }

            [TagStructure(Size = 0x8)]
            public class ClimbHint : TagStructure
            {
                public UserHintShortFlags Flags;
                public short LineSegmentIndex;
                public short Unknown1;
                public short Unknown2;
            }

            [TagStructure(Size = 0x10, Align = 0x8)]
            public class WellHint : TagStructure
			{
                public FlagsValue Flags;
                public List<WellPoint> Points;

                [Flags]
                public enum FlagsValue : int
                {
                    None = 0,
                    Bidirectional = 1 << 0
                }

                public enum WellPointType : short
                {
                    Jump,
                    Climb,
                    Hoist
                }

                [TagStructure(Size = 0x1C)]
                public class WellPoint : TagStructure
				{
                    public WellPointType Type;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused1 = new byte[2];

                    public RealPoint3d Position;

                    public short ReferenceFrame;
                    public short SectorIndex;

                    public RealVector2d Normal;
                }
            }

            [TagStructure(Size = 0xC)]
            public class FlightHint : TagStructure
			{
                public List<FlightPoint> FlightPoints;

                [TagStructure(Size = 0xC)]
                public class FlightPoint : TagStructure
				{
                    public RealPoint3d Point;
                }
            }

            [TagStructure(MaxVersion = CacheVersion.Halo3Retail, Size = 0xC)]
            [TagStructure(MinVersion = CacheVersion.Halo3ODST, Size = 0x18)]
            public class UnknownBlock8 : TagStructure
			{
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown2;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown3;

                public List<UnknownBlock> Unknown4;

                [TagStructure(MaxVersion = CacheVersion.Halo3Retail, Size = 0xC)]
                [TagStructure(MinVersion = CacheVersion.Halo3ODST, Size = 0x28)]
                public class UnknownBlock : TagStructure
				{
                    [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                    public List<UnknownBlock2> Unknown;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown2;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown3;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown4;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown5;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown6;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown7;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown8;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown9;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown10;
                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public int Unknown11;

                    [TagStructure(Size = 0x18)]
                    public class UnknownBlock2 : TagStructure
					{
                        public float Unknown;
                        public float Unknown2;
                        public float Unknown3;
                        public short Unknown4;
                        public short Unknown5;
                        public Angle Unknown6;
                        public Angle Unknown7;
                    }
                }
            }

            [TagStructure(MaxVersion = CacheVersion.Halo3Retail, Size = 0x2)]
            [TagStructure(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Size = 0x4)]
            [TagStructure(MinVersion = CacheVersion.HaloOnlineED, Size = 0xC)]
            public class UnknownBlock9 : TagStructure
			{
                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
                public short UnknownH3;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown1;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public int Unknown2;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public int Unknown3;
            }
        }

        [TagStructure(Size = 0x28)]
        public class AiRecordingReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string RecordingName;

            [TagField(Length = 0x8, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x84)]
        public class ScriptingDatum : TagStructure
		{
            public List<PointSet> PointSets;

            [TagField(Flags = Padding, Length = 120)]
            public byte[] Unused;

            [Flags]
            public enum PointSetFlags : int
            {
                None,
                ManualReferenceFrame = 1 << 0,
                TurretDeployment = 1 << 1,
                GiantSet = 1 << 2,
                InvalidSectorRefs = 1 << 3
            }

            [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST)]
            public class PointSet : TagStructure
			{
                [TagField(Flags = Label, Length = 32)]
                public string Name;

                public List<Point> Points;

                public short BspIndex;
                public short ManualReferenceFrame;

                public PointSetFlags Flags;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short EditorFolderIndex;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short Unknown;

                [TagStructure(Size = 0x3C)]
                public class Point : TagStructure
				{
                    [TagField(Flags = Label, Length = 32)]
                    public string Name;
                    public RealPoint3d Position;
                    public short ReferenceFrame;
                    public short BspIndex;
                    public short ZoneIndex;
                    public short SurfaceIndex;
                    public RealEulerAngles2d FacingDirection;
                }
            }
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public class CutsceneFlag : TagStructure
		{
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unused;
            [TagField(Flags = Label)]
            public StringId Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
            [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            public short EditorFolderIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            public short SourceBspIndex;
        }

        public enum CutsceneCameraPointType : short
        {
            Normal,
            IgnoreTargetOrientation,
            Dolly,
            IgnoreTargetUpdates
        }

        [Flags]
        public enum CutsceneCameraPointFlags : ushort
        {
            None = 0,
            Bit0 = 1 << 0,
            PrematchCameraHack = 1 << 1,
            PodiumCameraHack = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x40)]
        public class CutsceneCameraPoint : TagStructure
		{
            public CutsceneCameraPointFlags Flags;
            public CutsceneCameraPointType Type;
            [TagField(Flags = Label, Length = 32)]
            public string Name;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unused;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
        }

        public enum CutsceneTitleHorizontalJustification : short
        {
            Left,
            Right,
            Center
        }

        public enum CutsceneTitleVerticalJustification : short
        {
            Bottom,
            Top,
            Middle,
            Bottom2,
            Top2
        }

        public enum CutsceneTitleFont : short
        {
            TerminalFont,
            SubtitleFont
        }

        [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class CutsceneTitle : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Rectangle2d TextBounds;

            // TODO: RealRectangle2d
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TextBoundsReachX0;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TextBoundsReachX1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TextBoundsReachY0;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TextBoundsReachY1;

            public CutsceneTitleHorizontalJustification HorizontalJustification;
            public CutsceneTitleVerticalJustification VerticalJustification;
            public CutsceneTitleFont Font;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
            public ArgbColor TextColor;
            public ArgbColor ShadowColor;
            public float FadeInTime;
            public float Uptime;
            public float FadeOutTime;
        }

        [TagStructure(Size = 0x28)]
        public class ScenarioResource : TagStructure
		{
            public int Unknown;
            public List<TagReferenceBlock> ScriptSource;
            public List<TagReferenceBlock> AiResources;
            public List<Reference> References;

            [TagStructure(Size = 0x130, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x16C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x150, MinVersion = CacheVersion.HaloReach)]
            public class Reference : TagStructure
			{
                public CachedTag SceneryResource;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public List<TagReferenceBlock> OtherScenery;
                public CachedTag BipedsResource;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public List<TagReferenceBlock> OtherBipeds;
                public CachedTag VehiclesResource;
                public CachedTag EquipmentResource;
                public CachedTag WeaponsResource;
                public CachedTag SoundSceneryResource;
                public CachedTag LightsResource;
                public CachedTag DevicesResource;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public List<TagReferenceBlock> OtherDevices;
                public CachedTag EffectSceneryResource;
                public CachedTag DecalsResource;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public List<TagReferenceBlock> OtherDecals;
                public CachedTag CinematicsResource;
                public CachedTag TriggerVolumesResource;
                public CachedTag ClusterDataResource;
                public CachedTag CommentsResource;
                public CachedTag CreatureResource;
                public CachedTag StructureLightingResource;
                public CachedTag DecoratorsResource;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public List<TagReferenceBlock> OtherDecorators;
                public CachedTag SkyReferencesResource;
                public CachedTag CubemapResource;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public CachedTag PerformancesResource;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public CachedTag DumplingsResource;
            }
        }

        [Flags]
        public enum UnitSeatFlags : int
        {
            None = 0,
            Seat0 = 1 << 0,
            Seat1 = 1 << 1,
            Seat2 = 1 << 2,
            Seat3 = 1 << 3,
            Seat4 = 1 << 4,
            Seat5 = 1 << 5,
            Seat6 = 1 << 6,
            Seat7 = 1 << 7,
            Seat8 = 1 << 8,
            Seat9 = 1 << 9,
            Seat10 = 1 << 10,
            Seat11 = 1 << 11,
            Seat12 = 1 << 12,
            Seat13 = 1 << 13,
            Seat14 = 1 << 14,
            Seat15 = 1 << 15,
            Seat16 = 1 << 16,
            Seat17 = 1 << 17,
            Seat18 = 1 << 18,
            Seat19 = 1 << 19,
            Seat20 = 1 << 20,
            Seat21 = 1 << 21,
            Seat22 = 1 << 22,
            Seat23 = 1 << 23,
            Seat24 = 1 << 24,
            Seat25 = 1 << 25,
            Seat26 = 1 << 26,
            Seat27 = 1 << 27,
            Seat28 = 1 << 28,
            Seat29 = 1 << 29,
            Seat30 = 1 << 30,
            Seat31 = 1 << 31
        }

        [TagStructure(Size = 0xC)]
        public class UnitSeatsMappingBlock : TagStructure
		{
            [TagField(Flags = Short)]
            public CachedTag Unit;
            public UnitSeatFlags Seats1;
            public UnitSeatFlags Seats2;
        }

        [TagStructure(Size = 0x2, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class ScenarioKillTrigger : TagStructure
		{
            public short TriggerVolume;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public KillVolumeFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding;
        }

        [TagStructure(Size = 0x2, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class ScenarioSafeTrigger : TagStructure
		{
            public short TriggerVolume;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public KillVolumeFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding;
        }

        [Flags]
        public enum KillVolumeFlags : byte
        {
            DontKillImmediately = 1 << 0,
            OnlyKillPlayers = 1 << 1
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

        [TagStructure(Size = 0x9C)]
        public class OrdersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            public short Style;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding0;

            public OrderFlags Flags;
            public ForceCombatStatusEnum ForceCombatStatus;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding1;

            [TagField(Length = 32)]
            public string EntryScript;

            public short ScriptIndex;
            public short FollowSquad;
            public float FollowRadius;
            public List<ZoneSetBlockStruct> PrimaryAreaSet;
            public List<ZoneSetBlockStruct> SecondaryAreaSet;
            public List<SecondarySetTriggerBlock> SecondarySetTrigger;
            public List<SpecialMovementBlock> SpecialMovement;
            public List<OrderEndingBlock> OrderEndings;
            public List<PureformDistributionBlock> PureformDistribution;

            [Flags]
            public enum OrderFlags : uint
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

            public enum ForceCombatStatusEnum : short
            {
                None,
                Idle,
                Alert,
                Combat
            }

            [TagStructure(Size = 0x10)]
            public class ZoneSetBlockStruct : TagStructure
            {
                public ZoneSetTypeEnum AreaType;
                public ZoneSetFlags Flags;
                public OrderAreaReferenceCharacterFlags CharacterFlags;
                public short Zone;
                public short Area;
                public Angle Yaw;
                public int ConnectionFlags;

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

                [Flags]
                public enum OrderAreaReferenceCharacterFlags : byte
                {
                    PureformRanged = 1 << 0,
                    PureformTank = 1 << 1,
                    PureformStealth = 1 << 2
                }
            }

            [TagStructure(Size = 0x10)]
            public class SecondarySetTriggerBlock : TagStructure
            {
                public CombinationRulesEnum CombinationRule;
                public OrderEndingDialogueEnum DialogueType; // when this ending is triggered, launch a dialogue event of the given type
                public List<TriggerReferences> Triggers;

                public enum CombinationRulesEnum : short
                {
                    Or,
                    And
                }

                public enum OrderEndingDialogueEnum : short
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
                    Support,
                    Flank
                }

                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerRefFlags TriggerFlags;
                    public short Trigger;

                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding0;

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
                    NA = 1 << 6,
                    Rail = 1 << 7,
                    Seam = 1 << 8,
                    Door = 1 << 9
                }
            }

            [TagStructure(Size = 0x18)]
            public class OrderEndingBlock : TagStructure
            {
                public short NextOrder;
                public CombinationRulesEnum CombinationRule;
                public float DelayTime;
                public OrderEndingDialogueEnum DialogueType; // when this ending is triggered, launch a dialogue event of the given type

                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding0;

                public List<TriggerReferences> Triggers;

                public enum CombinationRulesEnum : short
                {
                    Or,
                    And
                }

                public enum OrderEndingDialogueEnum : short
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
                    Support,
                    Flank
                }

                [TagStructure(Size = 0x8)]
                public class TriggerReferences : TagStructure
                {
                    public TriggerRefFlags TriggerFlags;
                    public short Trigger;

                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding0;

                    [Flags]
                    public enum TriggerRefFlags : uint
                    {
                        Not = 1 << 0
                    }
                }
            }

            [TagStructure(Size = 0x8)]
            public class PureformDistributionBlock : TagStructure
            {
                public short NumRanged;
                public short NumTank;
                public short NumStealth;

                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding0;
            }
        }

        [TagStructure(Size = 0x34)]
        public class TriggersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TriggerFlags Flags;
            public CombinationRulesEnum CombinationRule;

            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding0;

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
                public byte[] Padding1;

                [TagField(Length = 32)]
                public string ExitConditionScript;
                public short ScriptIndex;

                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;

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

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
        public class StructureBspAtmospherePaletteBlock : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            public short AtmosphereSettingIndex;
            [TagField(Flags = Padding, Length = 0x2)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag AtmosphereFog;
        }

        [TagStructure(Size = 0x74)]
        public class AcousticsAmbiencePaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag BackgroundSound;
            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag InsideClusterSound; // play this only when the player is inside the cluster
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] EB;
            public float CutoffDistance;
            public BackgroundSoundScaleFlagsDefinition ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] LU;

            [Flags]
            public enum BackgroundSoundScaleFlagsDefinition : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }

        [TagStructure(Size = 0x50)]
        public class AcousticsEnvironmentPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new[] { "snde" })]
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x18, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x78, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class WeatherPaletteBlock : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 32, Flags = Label)]
            public string Name;

            [TagField(MinVersion = CacheVersion.HaloReach, Flags = Label)]
            public StringId ReachName;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 0x24, Flags = Padding)]
            public byte[] Padding0;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public RealVector3d WindDirection;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float WindMagnitude;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 0x2, Flags = Padding)]
            public byte[] Padding1;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short RuntimeWindGlobalScenarioFunctionIndex;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 32)]
            public string WindScaleFunction;

            [TagField(MinVersion = CacheVersion.HaloReach, ValidTags = new[] { "rain" })]
            public CachedTag Rain;
        }

        [TagStructure(Size = 0x68, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x74, MinVersion = CacheVersion.HaloOnline498295)]
        public class ScenarioClusterDatum : TagStructure
		{
            public CachedTag Bsp;
            public List<BackgroundSoundEnvironment> BackgroundSoundEnvironments;
            public List<UnknownBlock> Unknown;
            public List<UnknownBlock2> Unknown2;
            public int BspChecksum;
            public List<ClusterCentroid> ClusterCentroids;
            public List<WeatherProperty> WeatherProperties;
            public List<FogBlock> Fog;
            public List<CameraEffect> CameraEffects;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public List<UnknownBlock4> Unknown4;

            [TagStructure(Size = 0x4)]
            public class BackgroundSoundEnvironment : TagStructure
			{
                public short BackgroundSoundEnvironmentIndex;
                public short Unknown;
            }

            [TagStructure(Size = 0x4)]
            public class UnknownBlock : TagStructure
			{
                public short Unknown;
                public short Unknown2;
            }

            [TagStructure(Size = 0x4)]
            public class UnknownBlock2 : TagStructure
			{
                public short Unknown;
                public short Unknown2;
            }

            [TagStructure(Size = 0xC)]
            public class ClusterCentroid : TagStructure
			{
                public RealPoint3d Centroid;
            }

            [TagStructure(Size = 0x4)]
            public class WeatherProperty : TagStructure
			{
                public short Unknown;
                [TagField(Flags = TagFieldFlags.Padding)]
                public short Unused;
            }

            [TagStructure(Size = 0x4)]
            public class FogBlock : TagStructure
			{
                public short FogIndex;
                public short Unknown;
            }

            [TagStructure(Size = 0x4)]
            public class CameraEffect : TagStructure
			{
                public short CameraEffectIndex;
                public short Unknown;
            }

            [TagStructure(Size = 0x4)]
            public class UnknownBlock4 : TagStructure
			{
                public short Unknown;
                public short Unknown2;
            }
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x80, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public class ScenarioAcousticVolumeBlock : TagStructure
        {
            public TriggerVolume Volume;
            public short Acoustics;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class SpawnDatum : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> DynamicSpawnHeightBounds;

            public float GameObjectResetHeight;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown5;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown6;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown7;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown8;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown9;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown10;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown11;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown12;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown13;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown14;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown15;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<DynamicSpawnOverload> DynamicSpawnOverloads;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<SpawnZone> StaticRespawnZones;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<SpawnZone> StaticInitialSpawnZones;

            [TagStructure(Size = 0x10)]
            public class DynamicSpawnOverload : TagStructure
			{
                public short OverloadType;
                public short Unknown;
                public float InnerRadius;
                public float OuterRadius;
                public float Weight;
            }

            [Flags]
            public enum RelevantTeamFlags : int
            {
                None = 0,
                Red = 1 << 0,
                Blue = 1 << 1,
                Green = 1 << 2,
                Orange = 1 << 3,
                Purple = 1 << 4,
                Yellow = 1 << 5,
                Brown = 1 << 6,
                Pink = 1 << 7,
                Neutral = 1 << 8
            }

            [TagStructure(Size = 0x30)]
            public class SpawnZone : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public RelevantTeamFlags RelevantTeams;
                public uint RelevantGames;
                public uint Flags;
                public RealPoint3d Position;
                public Bounds<float> HeightBounds;
                public Bounds<float> RadiusBounds;
                public float Weight;
            }
        }

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x68, MinVersion = CacheVersion.HaloReach)]
        public class CrateInstance : PermutationInstance, IMultiplayerInstance
        {
            public PathfindingPolicyValue PathfindingPolicy;
            public LightmappingPolicyValue LightmappingPolicy;
            public List<PathfindingReference> PathfindingReferences;
            public MultiplayerObjectProperties Multiplayer;

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }

            [TagStructure]
            public class UnknownBlock2 : TagStructure
			{
                public uint Unknown;
            }
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
        public class Flock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public short FlockPaletteIndex;
            public short BspIndex;
            public short BoundingTriggerVolume;
            public FlockFlags Flags;
            public float EcologyMargin;
            public List<Source> Sources;
            public List<Sink> Sinks;
            public Bounds<float> ProductionFrequencyBounds;
            public Bounds<float> ScaleBounds;
            // Distance from a source at which the creature scales to full size
            public float SourceScaleto0;
            // Distance from a sink at which the creature begins to scale to zero
            public float SinkScaleto0;
            // The number of seconds it takes to kill all units in the flock if it gets destroyed
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FlockDestroyDuration; // sec
            public short CreaturePaletteIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short BigBattleCreaturePaletteIndex;
            public Bounds<short> BoidCountBounds;
            public short EnemyFlockIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding;
            [TagField(MinVersion = CacheVersion.HaloReach)]
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

            [TagStructure(Size = 0x24)]
            public class Source : TagStructure
			{
                public int Unknown;
                public RealPoint3d Position;
                public RealEulerAngles2d Starting;
                public float Radius;
                public float Weight;
                public sbyte Unknown2;
                public sbyte Unknown3;
                public sbyte Unknown4;
                public sbyte Unknown5;
            }

            [TagStructure(Size = 0x10)]
            public class Sink : TagStructure
			{
                public RealPoint3d Position;
                public float Radius;
            }
        }

        [TagStructure(Size = 0x0)]
        public class CreatureInstance : ScenarioInstance
        {
        }

        [TagStructure(Size = 0x104)]
        public class EditorFolder : TagStructure
		{
            public int ParentFolder;

            [TagField(Flags = Label, Length = 256)]
            public string Name;
        }

        [TagStructure(Size = 0x24)]
        public class Interpolator : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public StringId AcceleratorName;
            public StringId MultiplierName;
            public TagFunction Function;
            public short AcceleratorInterpolatorIndex;
            public short MultiplierInterpolatorIndex;
        }

        [TagStructure(Size = 0x4)]
        public class SimulationDefinitionTableBlock : TagStructure
		{
            [TagField(Flags = Short)]
            public CachedTag Tag;
        }

        [TagStructure(Size = 0x10)]
        public class ReferenceFrame : TagStructure
		{
            public DatumHandle ObjectHandle;
            public short OriginBspIndex;
            public ScenarioObjectType ObjectType;
            public ScenarioInstance.SourceValue Source;
            public short NodeIndex;
            public short ProjectionAxis;
            public AiReferenceFrameFlags Flags;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding0;

            [Flags]
            public enum AiReferenceFrameFlags : ushort
            {
                ProjectionSign = 1 << 0
            }
        }

        [Flags]
        public enum AiObjectiveFlags : ushort
        {
            None = 0,
            UseFrontAreaSelection = 1 << 0,
            UsePlayersAsFront = 1 << 1,
            InhibitVehicleEntry = 1 << 2
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3ODST)]
        public class AiObjective : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<OpposingObjective> OpposingObjectives;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public AiObjectiveFlags Flags;

            public short Zone;

            public short FirstTaskIndex;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short EditorFolderIndex = -1;

            public List<Task> Tasks;

            [TagStructure(Size = 0x4)]
            public class OpposingObjective : TagStructure
			{
                public short ObjectiveIndex;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused;
            }

            [Flags]
            public enum TaskFlags : ushort
            {
                None = 0,
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
                DontGenerateFront = 1 << 12,
                ReverseDirection = 1 << 13,
                InvertFilterLogic = 1 << 14
            }

            [TagStructure(Size = 0xCC, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0xE8, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x84, MaxVersion = CacheVersion.HaloReach11883)]
            public class Task : TagStructure
			{
                public TaskFlags Flags;

                public TaskInhibitGroups InhibitGroups;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public uint Unknown1;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public uint Unknown2;

                public SquadDifficultyFlags InhibitOnDifficulty;

                public MovementValue Movement;
                public FollowValue Follow;
                public short FollowSquadIndex;
                public float FollowRadius;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float FollowZClamp;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public FollowPlayerFlags FollowPlayers;

                [TagField(Length = 2, MinVersion = CacheVersion.Halo3ODST)]
                public byte[] Unused = new byte[2];

                /// <summary>
                /// Exhaust this task after it has been active for this long.
                /// </summary>
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float MaximumDuration;

                /// <summary>
                /// When a task exhausts, hold actors in the task for this long before releasing them.
                /// </summary>
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float ExhaustionDelay;

                [TagField(MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
                public uint Unknown23;

                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string EntryScriptName;
                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string CommandScriptName;
                [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                public string ExhaustionScriptName;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId EntryScriptNameReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId CommandScriptNameReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId ExhaustionScriptNameReach;

                public short EntryScriptIndex;
                public short CommandScriptIndex;
                public short ExhaustionScriptIndex;

                public short SquadGroupFilter;

                /// <summary>
                /// When someone enters this task for the first time, they play this type of dialogue.
                /// </summary>
                public DialogueTypeValue DialogueType;

                public RuntimeFlagsValue RuntimeFlags;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public List<PureformDistributionBlock> PureformDistribution;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short Unknown20; // kungfu count lol?
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short Unknown21;

                public StringId Name;

                public short HierarchyLevelFrom100;

                public short PreviousRole;
                public short NextRole;
                public short ParentRole;

                public List<ActivationScriptBlock> ActivationScript;
                public short ScriptIndex;

                /// <summary>
                /// Task will never want to suck in more then N guys over lifetime (soft ceiling only applied when limit exceeded.
                /// </summary>
                public short LifetimeCount;

                public FilterFlagsValue FilterFlags;
                public TaskFilter Filter;

                public Bounds<short> Capacity;
                
                /// <summary>
                /// Task becomes inactive after the given number of casualties.
                /// </summary>
                public short MaxBodyCount;

                public AttitudeValue Attitude;
                
                /// <summary>
                /// Task becomes inactive after the strength of the participants falls below the given level.
                /// </summary>
                [TagField(Format = "[0,1]")]
                public float MinStrength;

                public List<Area> Areas;
                public List<DirectionBlock> Direction;

                [Flags]
                public enum TaskInhibitGroups : ushort
                {
                    None = 0,
                    Cover = 1 << 0,
                    Retreat = 1 << 1,
                    VehiclesAll = 1 << 2,
                    Grenades = 1 << 3,
                    Berserk = 1 << 4,
                    Equipment = 1 << 5,
                    ObjectInteraction = 1 << 6,
                    Turrets = 1 << 7,
                    VehiclesNonTurrets = 1 << 8
                }

                public enum MovementValue : short
                {
                    Run,
                    Walk,
                    Crouch
                }

                public enum FollowValue : short
                {
                    None,
                    Player,
                    Squad
                }

                [Flags]
                public enum FollowPlayerFlags : ushort
                {
                    None,
                    Player0 = 1 << 0,
                    Player1 = 1 << 1,
                    Player2 = 1 << 2,
                    Player3 = 1 << 3
                }

                [TagStructure(Size = 0x1C)]
                public class FollowFiringPointQueryBlock : TagStructure
				{
                    public ShapeTypeValue ShapeType;
                    public AnchorRelationshipValue AnchorRelationship;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];

                    public float RelationshipOffset;
                    public float Scale;

                    /// <summary>
                    /// Don't include firing points outside of this vertical margin.
                    /// </summary>
                    [TagField(Format = "World Units")]  
                    public float ZThreshold;

                    public RealEulerAngles3d Angles;

                    public enum ShapeTypeValue : sbyte
                    {
                        Circle,
                        Triangle,
                        Square,
                        Bar
                    }

                    public enum AnchorRelationshipValue : sbyte
                    {
                        Center,
                        Front,
                        Back,
                        Left,
                        Right
                    }
                }

                public enum DialogueTypeValue : short
                {
                    None,
                    EnemyIsAdvancing,
                    EnemyIsCharging,
                    EnemyIsFallingBack,
                    Advance,
                    Charge,
                    FallBack,
                    MoveOnMoveone,
                    FollowPlayer,
                    ArrivingIntoCombat,
                    EndCombat,
                    Investigate,
                    SpreadOut,
                    HoldPositionHold,
                    FindCover,
                    CoveringFire
                }

                [Flags]
                public enum RuntimeFlagsValue : ushort
                {
                    None,
                    AreaConnectivityValid = 1 << 0
                }

                [TagStructure(Size = 0x8)]
                public class PureformDistributionBlock : TagStructure
				{
                    public short Unknown1;
                    public short Unknown2;
                    public short Unknown3;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];
                }

                [TagStructure(Size = 0x124, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x108, MinVersion = CacheVersion.HaloReach)]
                public class ActivationScriptBlock : TagStructure
				{
                    [TagField(Flags = Label, Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
                    public string ScriptName;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public StringId ScriptNameReach;

                    [TagField(Length = 256)]
                    public string ScriptSource;

                    public CompileStateValue CompileState;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];

                    public enum CompileStateValue : short
                    {
                        Edited,
                        Success,
                        Error
                    }
                }

                [Flags]
                public enum FilterFlagsValue : ushort
                {
                    None,
                    Exclusive = 1 << 0
                }

                [TagStructure(Size = 0x2)]
                public class TaskFilter : TagStructure
                {
                    [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                    public Halo3RetailValue Halo3Retail;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public Halo3OdstValue Halo3Odst;

                    public enum Halo3RetailValue : short
                    {
                        None,
                        Leader,
                        NoLeader,
                        Arbiter,
                        Player,
                        SightedPlayer,
                        SightedEnemy,
                        Infantry,
                        StrengthGreaterThanOneQuarter,
                        StrengthGreaterThanOneHalf,
                        StrengthGreaterThanThreeQuarters,
                        StrengthLessThanOneQuarter,
                        StrengthLessThanOneHalf,
                        StrengthLessThanThreeQuarters,
                        HumanTeam,
                        CovenantTeam,
                        FloodTeam,
                        SentinelTeam,
                        ProphetTeam,
                        GuiltyTeam,
                        Elite,
                        Jackal,
                        Grunt,
                        Hunter,
                        Marine,
                        FloodCombat,
                        FloodCarrier,
                        Sentinel,
                        Brute,
                        Prophet,
                        Bugger,
                        FloodPureform,
                        Guardian,
                        Sniper,
                        Vehicle,
                        Scorpion,
                        Ghost,
                        Warthog,
                        Spectre,
                        Wraith,
                        Phantom,
                        Pelican,
                        Banshee,
                        Hornet,
                        BruteChopper,
                        Mauler,
                        Mongoose
                    }

                    public enum Halo3OdstValue : short
                    {
                        None,
                        Leader,
                        NoLeader,
                        Arbiter,
                        Player,
                        InCombat,
                        SightedPlayer,
                        SightedEnemy,
                        Disengaged,
                        Infantry,
                        HasAnEngineer,
                        StrengthGreaterThanOneQuarter,
                        StrengthGreaterThanOneHalf,
                        StrengthGreaterThanThreeQuarters,
                        StrengthLessThanOneQuarter,
                        StrengthLessThanOneHalf,
                        StrengthLessThanThreeQuarters,
                        HumanTeam,
                        CovenantTeam,
                        FloodTeam,
                        SentinelTeam,
                        ProphetTeam,
                        GuiltyTeam,
                        Elite,
                        Jackal,
                        Grunt,
                        Hunter,
                        Marine,
                        FloodCombat,
                        FloodCarrier,
                        Sentinel,
                        Brute,
                        Prophet,
                        Bugger,
                        FloodPureform,
                        Guardian,
                        Engineer,
                        Sniper,
                        Rifle,
                        Vehicle,
                        Scorpion,
                        Ghost,
                        Warthog,
                        Spectre,
                        Wraith,
                        Phantom,
                        Pelican,
                        Banshee,
                        Hornet,
                        BruteChopper,
                        Mauler,
                        Mongoose
                    }
                }

                public enum AttitudeValue : short
                {
                    Normal,
                    Defensive,
                    Aggressive,
                    Playfighting,
                    Patrol,
                    ChcknShitRecon,
                    SpreadOut
                }

                public enum AreaType : short
                {
                    Normal,
                    Search,
                    Core
                }

                [Flags]
                public enum AreaFlags : byte
                {
                    None,
                    Goal = 1 << 0,
                    DirectionValid = 1 << 1
                }

                [TagStructure(Size = 0xA, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
                public class Area : TagStructure
				{
                    public AreaType Type;

                    [TagField(Flags = Padding, Length = 1, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                    public byte[] Unused = new byte[1];

                    public AreaFlags Flags;

                    [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                    [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
                    public byte CharacterFlags;

                    [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                    public short Unknown3;

                    public short ZoneIndex;
                    public short AreaIndex;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public Angle Yaw;

                    [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                    public int ConnectivityBitVector;

                    [TagField(Length = 4, MinVersion = CacheVersion.HaloReach)]
                    public int[] ConnectivityBitVectorReach;
                }

                [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST)]
                public class DirectionBlock : TagStructure
				{
                    [TagField(Length = 2, MaxVersion = CacheVersion.Halo3Retail)]
                    public Point[] Points_H3 = new Point[2];

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public List<Point> Points;

                    [TagStructure(Size = 0x10)]
                    public class Point : TagStructure
					{
                        public RealPoint3d Position;
                        public short ManualReferenceFrame;
                        public short BspIndex;
                    }
                }
            }
        }

        [TagStructure(Size = 0x2)]
		public class ObjectReference : TagStructure
		{
            public short PaletteIndex;
        }

        [TagStructure(Size = 0xBC, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC8, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xBC, MinVersion = CacheVersion.HaloOnlineED)]
        public class DesignerZoneSet : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public ScenarioDesignerZoneFlagsDefinition Flags;
            public List<ObjectReference> Bipeds;
            public List<ObjectReference> Vehicles;
            public List<ObjectReference> Weapons;
            public List<ObjectReference> Equipment;
            public List<ObjectReference> Scenery;
            public List<ObjectReference> Machines;
            public List<ObjectReference> Terminals;
            public List<ObjectReference> Controls;
            public List<ObjectReference> Unknown2;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public List<ObjectReference> Unknown3;
            public List<ObjectReference> Crates;
            public List<ObjectReference> Creatures;
            public List<ObjectReference> Giants;
            public List<ObjectReference> Unknown4;
            public List<ObjectReference> Characters;
            public List<ObjectReference> BudgetReference;

            [Flags]
            public enum ScenarioDesignerZoneFlagsDefinition : uint
            {
            }
        }

        [TagStructure(Size = 0x4)]
        public class ScenarioZoneDebuggerBlock : TagStructure
		{
            public uint ActiveDesignerZones;
        }

        [TagStructure(Size = 0x84)]
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

            [TagStructure(Size = 0x34)]
            public class DecoratorBrushStruct : TagStructure
            {
                public int EditingThisBsp;
                public DecoratorLeftBrushTypeEnumDefinition LeftButtonBrush;
                public DecoratorRightBrushTypeEnumDefinition RightButtonBrush;
                public float OuterRadius;
                public float FeatherPercent;
                public DecoratorBrushReapplyFlagsDefinition ReapplyFlags;
                public DecoratorBrushRenderFlagsDefinition RenderFlags;
                public DecoratorBrushActionFlagsDefinition ActionFlags;
                public DecoratorBrushShapeEnumDefinition BrushShape;
                public int CurrentPalette;
                public int CurrentSet;
                public float PaintRate; // [0 - 1]
                public RealRgbColor PaintColor;
                public float MoveDistance; // drop height for drop to ground

                /* KEYS */

                public enum DecoratorLeftBrushTypeEnumDefinition : int
                {
                    AirbrushAdd,
                    AirbrushColor,
                    AirbrushErase,
                    DensitySmooth,
                    PrecisionPlace,
                    PrecisionDelete,
                    Scale,
                    ScaleAdditive,
                    RotateRandom,
                    RotateNormal,
                    RotateLocal,
                    Eraser,
                    ReapplyTypeSettings,
                    DropToGroundUseReapplyFlags,
                    Comb,
                    Thin
                }

                public enum DecoratorRightBrushTypeEnumDefinition : int
                {
                    AirbrushAdd,
                    AirbrushColor,
                    AirbrushErase,
                    DensitySmooth,
                    PrecisionPlace,
                    PrecisionDelete,
                    Scale,
                    ScaleAdditive,
                    RotateRandom,
                    RotateNormal,
                    RotateLocal,
                    Eraser,
                    ReapplyTypeSettings,
                    DropToGroundUseReapplyFlags,
                    Comb,
                    Thin
                }

                [Flags]
                public enum DecoratorBrushReapplyFlagsDefinition : byte
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
                public enum DecoratorBrushRenderFlagsDefinition : byte
                {
                    RenderPreview = 1 << 0,
                    RenderInRadiusOnly = 1 << 1,
                    RenderSelectedOnly = 1 << 2,
                    DontRenderLines = 1 << 3
                }

                [Flags]
                public enum DecoratorBrushActionFlagsDefinition : byte
                {
                    ClampScale = 1 << 0,
                    EnforceMinimumDistance = 1 << 1,
                    SelectAllDecoratorSets = 1 << 2
                }

                public enum DecoratorBrushShapeEnumDefinition : sbyte
                {
                    FlattenedSphere,
                    Spherical,
                    TallSphere
                }
            }

            [TagStructure(Size = 0x24)]
            public class DecoratorPalette : TagStructure
            {
                public StringId Name;
                public short DecoratorSet0;
                public short DecoratorWeight0;
                public short DecoratorSet1;
                public short DecoratorWeight1;
                public short DecoratorSet2;
                public short DecoratorWeight2;
                public short DecoratorSet3;
                public short DecoratorWeight3;
                public short DecoratorSet4;
                public short DecoratorWeight4;
                public short DecoratorSet5;
                public short DecoratorWeight5;
                public short DecoratorSet6;
                public short DecoratorWeight6;
                public short DecoratorSet7;
                public short DecoratorWeight7;
            }

            [TagStructure(Size = 0x1C)]
            public class DecoratorScenarioSetBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "dctr" })]
                public CachedTag DecoratorSet;
                public List<GlobalDecoratorPlacementBlock> Placements;

                [TagStructure(Size = 0x50)]
                public class GlobalDecoratorPlacementBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte TypeIndex;
                    public sbyte MotionScale;
                    public sbyte GroundTint;
                    public DecoratorPlacementFlagsDefinition Flags;
                    public RealQuaternion Rotation;
                    public float Scale;
                    public RealPoint3d TintColor;
                    public RealPoint3d OriginalPoint;
                    public RealPoint3d OriginalNormal;
                    public sbyte EditorBoundToBsp;
                    public sbyte RuntimeBspIndex;
                    public short ClusterIndex;
                    public short ClusterDecoratorSetIndex;
                    public sbyte BlockX;
                    public sbyte BlockY;

                    [Flags]
                    public enum DecoratorPlacementFlagsDefinition : byte
                    {
                        Unused = 1 << 0,
                        Unused2 = 1 << 1
                    }
                }
            }
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class CinematicsBlock : TagStructure
        {
            [TagField(Flags = Label, MinVersion = CacheVersion.HaloReach)]
            public StringId Name;
            public CachedTag Cinematic;
        }

        [TagStructure(Size = 0x14)]
        public class CinematicLightingBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public CachedTag CinematicLight;
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloReach)]
        public class PlayerRepresentationBlock : TagStructure
        {
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public StringId Name;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ReachPlayerRepFlags Flags;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public PlayerModelChoiceEnum ModelChoice;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public PlayerRepresentationClassEnum Class;

            [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.Halo3ODST)]
            public byte[] pad;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag HUDReference;

            [TagField(ValidTags = new[] { "mode" })]
            public CachedTag FirstPersonHands;
            [TagField(ValidTags = new[] { "mode" })]
            public CachedTag FirstPersonBody;
            [TagField(ValidTags = new[] { "unit" })]
            public CachedTag ThirdPersonUnit;

            public StringId ThirdPersonVariant;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag BinocularsZoomInSound;
            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag BinocularsZoomOutSound;

            [TagField(ValidTags = new[] { "udlg" }, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Voice;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int PlayerInformationIndex;

            public enum PlayerModelChoiceEnum : sbyte
            {
                Spartan,
                Elite
            }

            public enum PlayerRepresentationClassEnum : sbyte
            {
                Campaign,
                Multiplayer,
                Editor
            }

            [Flags]
            public enum ReachPlayerRepFlags : sbyte
            {
                CanUseHealthPacks = 1 << 0,
            }
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class ScenarioMetagameBlock : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<TimeMultiplier> TimeMultipliers;
            public float ParScore;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<TimeMultiplier> TimeMultipliersReach;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<SurvivalBlock> Survival;

            [TagStructure(Size = 0x8)]
            public class TimeMultiplier : TagStructure
			{
                public float Time;
                public float Multiplier;
            }

            [TagStructure(Size = 0x8)]
            public class SurvivalBlock : TagStructure
			{
                public short InsertionIndex;
                public short Unknown;
                public float ParScore;
            }
        }

        [TagStructure(Size = 0x18)]
        public class SoftSurfaceBlock : TagStructure
		{
            [TagField(Length = 0x4, Flags = Padding)]
            public byte[] Padding0;

            public float ClassBiped; // max - .2f
            public float ClassDeadBiped; // max - .09f
            public float ClassCratesVehicles; // max - .2f
            public float ClassDebris; // max - .04f

            [TagField(Length = 0x4, Flags = Padding)]
            public byte[] Padding1;
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.Halo4)]
        public class ScenarioCubemapBlock : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId Name;

            public RealPoint3d CubemapPosition;
            public CubemapResolutionEnum CubemapResolution;

            [TagField(Length = 0x2, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ManualBspFlagsReach;

            [TagField(MinVersion = CacheVersion.Halo4)]
            public ManualBspFlagsReferences ManualBspFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
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
            public class ManualBspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;

                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new[] { "sbsp" })]
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
        public class LightmapAirprobe : TagStructure
		{
            public RealPoint3d Position;
            public StringId Name;
            public short ManualBspFlags;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x0)]
        public class GNullBlock : TagStructure
        {
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
    }

    public enum ScenarioMapType : sbyte
    {
        SinglePlayer,
        Multiplayer,
        MainMenu
    }

    public enum ScenarioMapSubType : sbyte
    {
        None,
        Hub,
        Level,
        Scene,
        Cinematic
    }

    [Flags]
    public enum ScenarioFlags : ushort
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        CharactersUsePreviousMissionWeapons = 1 << 5,
    }

    [Flags]
    public enum ScenarioRuntimeTriggerVolumeFlags : uint
    {
        HasHardSafeVolume = 1 << 0,
        HasSoftSafeVolume = 1 << 1
    }

    [TagStructure(Size = 0xC)]
    public class ScenarioObjectParentStruct : TagStructure
    {
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // if an object with this name exists, we attach to it as a child
        public short NameIndex = -1;
        public StringId ParentMarker;
        public StringId ConnectionMarker;
    }
}