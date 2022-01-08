using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Audio;
using TagTool.Tags.Definitions.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.GameObject;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x7B8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x834, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x824, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x834, MinVersion = CacheVersion.HaloOnline498295)]
    public class Scenario : TagStructure
	{
        [TagField(Length = 1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public byte MapTypePadding;

        [TagField(Length = 1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        public byte MapTypePaddingReach;

        public ScenarioMapType MapType;

        [TagField(Length = 1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
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
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int MinimumStructureBspImporterVersion;

        public Angle LocalNorth;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LocalSeaLevel; // wu
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AltitudeCap; // wu
        // forge coordinates are relative to this point
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealPoint3d SandboxOriginPoint;

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
        public uint Unknown0;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public uint Unknown1;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;

        public CachedTag Unknown;

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

        public List<UnknownBlock> Unknown32;
        public List<UnknownBlock> Unknown33;
        public List<UnknownBlock> Unknown34;
        public List<UnknownBlock> Unknown35;
        public List<UnknownBlock> Unknown36;

        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;

        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;

        public uint Unknown51;
        public uint Unknown52;
        public uint Unknown53;

        public uint Unknown54;
        public uint Unknown55;
        public uint Unknown56;

        public uint Unknown57;
        public uint Unknown58;
        public uint Unknown59;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown60;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown61;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown62;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown63;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown64;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown65;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown66;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown67;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown68;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown69;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown70;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown71;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown72;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown73;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown74;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown75;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown76;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown77;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown78;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown79;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown80;

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

        public uint Unknown88;
        public uint Unknown89;
        public uint Unknown90;

        public List<AiPathfindingDatum> AiPathfindingData;

        public uint Unknown91;
        public uint Unknown92;
        public uint Unknown93;

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

        public uint Unknown97;
        public uint Unknown98;
        public uint Unknown99;

        public uint Unknown100;
        public uint Unknown101;
        public uint Unknown102;

        public List<ScenarioStructureBsp.BackgroundSoundEnvironmentPaletteBlock> BackgroundSoundEnvironmentPalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown103;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown104;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown105;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown106;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown107;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown108;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<UnknownBlock3> Unknown109;
        public List<StructureBspAtmospherePaletteBlock> Atmosphere;
        public List<ScenarioStructureBsp.CameraEffect> CameraFx;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioWeatherPaletteBlock> WeatherPalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown110;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown111;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown112;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown113;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown114;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown115;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown116;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown117;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown118;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ScenarioClusterDatum> ScenarioClusterData;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.Scenario.ScenarioClusterDataBlock> ScenarioClusterDataReach; // ugh

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown119;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown120;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown121;

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
        public uint Unknown125;
        public uint Unknown126;
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

        public List<UnknownBlock5> Unknown135;

        public uint Unknown136;
        public uint Unknown137;
        public uint Unknown138;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagReferenceBlock> NeuticlePalette;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioCheapParticleSystemsBlock> Neuticles;

        public List<CinematicsBlock> Cinematics;
        public List<CinematicLightingBlock> CinematicLighting;

        public uint Unknown139;
        public uint Unknown140;
        public uint Unknown141;

        public List<ScenarioMetagameBlock> ScenarioMetagame;
        public List<SoftSurfaceBlock> SoftSurfaces;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ScenarioCubemapBlock> Cubemaps;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<UnknownBlock7> Unknown143;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> CortanaEffects;

        public List<LightmapAirprobe> LightmapAirprobes;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

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
            public int Version;
            public List<BspChecksum> BspChecksums;
            public List<StructureBspPotentiallyVisibleSet> StructureBspPotentiallyVisibleSets;
            public List<PortalToDeviceMapping> PortalToDeviceMappings;

            [TagStructure(Size = 0x4)]
            public class BspChecksum : TagStructure
			{
                public uint Checksum;
            }

            [TagStructure(Size = 0x54, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
            public class StructureBspPotentiallyVisibleSet : TagStructure
			{
                public List<Cluster> Clusters;
                public List<Cluster> ClustersDoorsClosed;
                public List<Sky> ClusterSkies;
                public List<Sky> ClusterVisibleSkies;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public List<UnknownBlock> Unknown;
                public List<UnknownBlock> Unknown2;
                public List<BspSeamClusterMapping> ClusterMappings;

                [TagStructure(Size = 0xC)]
                public class Cluster : TagStructure
				{
                    public List<BitVector> BitVectors;

                    [TagStructure(Size = 0xC)]
                    public class BitVector : TagStructure
					{
                        public List<Bit> Bits;

                        [TagStructure(Size = 0x4)]
                        public class Bit : TagStructure
						{
                            public AllowFlags Allow;

                            [Flags]
                            public enum AllowFlags : int
                            {
                                None = 0,
                                Bit0 = 1 << 0,
                                Bit1 = 1 << 1,
                                Bit2 = 1 << 2,
                                Bit3 = 1 << 3,
                                Bit4 = 1 << 4,
                                Effects = 1 << 5,
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
                                FiringEffects = 1 << 16,
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
                }

                [TagStructure(Size = 0x1)]
                public class Sky : TagStructure
				{
                    public sbyte SkyIndex;
                }

                [TagStructure(Size = 0x4)]
                public class UnknownBlock : TagStructure
				{
                    public uint Unknown;
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

            [TagStructure(Size = 0x18)]
            public class PortalToDeviceMapping : TagStructure
			{
                public List<DevicePortalAssociation> DevicePortalAssociations;
                public List<GamePortalToPortalMapping> GamePortalToPortalMappings;

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
            public int UniqueClusterCount;
            public Bounds<float> ClusterDistanceBounds;
            public List<EncodedDoorPa> EncodedDoorPas;
            public List<RoomDoorPortalEncodedPa> ClusterDoorPortalEncodedPas;
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
        public enum ZoneSetFlags : int
        {
            None = 0,
            Set0 = 1 << 0,
            Set1 = 1 << 1,
            Set2 = 1 << 2,
            Set3 = 1 << 3,
            Set4 = 1 << 4,
            Set5 = 1 << 5,
            Set6 = 1 << 6,
            Set7 = 1 << 7,
            Set8 = 1 << 8,
            Set9 = 1 << 9,
            Set10 = 1 << 10,
            Set11 = 1 << 11,
            Set12 = 1 << 12,
            Set13 = 1 << 13,
            Set14 = 1 << 14,
            Set15 = 1 << 15,
            Set16 = 1 << 16,
            Set17 = 1 << 17,
            Set18 = 1 << 18,
            Set19 = 1 << 19,
            Set20 = 1 << 20,
            Set21 = 1 << 21,
            Set22 = 1 << 22,
            Set23 = 1 << 23,
            Set24 = 1 << 24,
            Set25 = 1 << 25,
            Set26 = 1 << 26,
            Set27 = 1 << 27,
            Set28 = 1 << 28,
            Set29 = 1 << 29,
            Set30 = 1 << 30,
            Set31 = 1 << 31
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x13C, MinVersion = CacheVersion.HaloReach)]
        public class ZoneSet : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            [TagField(Length = 256, MinVersion = CacheVersion.HaloReach)]
            public string NameString;
            public int PotentiallyVisibleSetIndex;

            // TODO fix for reach
            public int ImportLoadedBsps;
            public BspFlags LoadedBsps;
            public ZoneSetFlags LoadedDesignerZoneSets;
            public ZoneSetFlags UnloadedDesignerZoneSets;
            public ZoneSetFlags LoadedCinematicZoneSets;
            public int BspAtlasIndex;
            public int ScenarioBspAudibilityIndex;
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
            public short Unknown2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte Unknown21;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public BspPolicyValue BspPolicyReach;

            public ushort OldManualBspFlagsNowZoneSets;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ObjectTransformFlags TransformFlags;

            public StringId UniqueName;
            public DatumHandle UniqueHandle;
            public short OriginBspIndex;
            public ScenarioObjectType ObjectType;
            public SourceValue Source; // sbyte
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public BspPolicyValue BspPolicy; // sbyte
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown3;
            public short EditorFolderIndex;
            public short Unknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short Unknown5;
            public short ParentNameIndex;
            public StringId ChildName;
            public StringId Unknown6;
            public ushort AllowedZoneSets;
            public short Unknown61;

            public enum SourceValue : sbyte
            {
                Structure,
                Editor,
                Dynamic,
                Legacy,
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
            [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Unused;
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
            [TagField(Length = 1, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding1;
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
            public byte ActiveChangeColors;
            public sbyte Unknown8;
            public sbyte Unknown9;
            public sbyte Unknown10;
            public ArgbColor PrimaryColor;
            public ArgbColor SecondaryColor;
            public ArgbColor TertiaryColor;
            public ArgbColor QuaternaryColor;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint QuinaryColor;
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
            public float BodyVitalityPercentage;
            public uint Flags;
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class VehicleInstance : PermutationInstance, IMultiplayerInstance
        {
            public float BodyVitalityPercentage;
            public uint Flags;
            public MultiplayerObjectProperties Multiplayer;

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

        [TagStructure]
        public class PathfindingReference : TagStructure
        {
            public short BspIndex;
            public short PathfindingObjectIndex;
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloReach)]
        public class EquipmentInstance : ScenarioInstance, IMultiplayerInstance
        {
            public uint EquipmentFlags;
            public MultiplayerObjectProperties Multiplayer;

            MultiplayerObjectProperties IMultiplayerInstance.Multiplayer { get => Multiplayer; set => Multiplayer = value; }
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class WeaponInstance : PermutationInstance, IMultiplayerInstance
        {
            public short RoundsLeft;
            public short RoundsLoaded;
            public uint WeaponFlags;
            public MultiplayerObjectProperties Multiplayer;

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
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown;
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
            public ScenarioMachineFlags MachineFlags;
            public List<PathfindingReference> PathfindingReferences;
            public PathfindingPolicyValue PathfindingPolicy;
            public short Unknown11;

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

            [TagStructure]
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

                [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
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

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x58, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnlineED)]
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
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public uint Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public uint Unknown2;
            public byte StartingFragGrenadeCount;
            public byte StartingPlasmaGrenadeCount;
            public byte StartingSpikeGrenadeCount;
            public byte StartingFirebombGrenadeCount;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown3;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown4;
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

        public enum TriggerVolumeType : short
        {
            BoundingBox,
            Sector
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
            public float ZSink;

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

            [TagStructure(Size = 0x50)]
            public class RuntimeTriangle : TagStructure
			{
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;

                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;

                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;

                public uint Unknown13;
                public uint Unknown14;

                public uint Unknown15;
                public uint Unknown16;

                public uint Unknown17;
                public uint Unknown18;

                public uint Unknown19;
                public uint Unknown20;
            }
        }

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo4)]
        public class ScenarioAcousticSectorBlockStruct : TagStructure
        {
            public List<AcousticSectorPointBlock> Points;
            public RealPlane3d TopPlane;
            public RealPlane3d BottomPlane;
            [TagField(MinVersion = CacheVersion.Halo4)]
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
        public class UnknownBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
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
        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloReach)]
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

        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x84, MinVersion = CacheVersion.HaloReach)]
        public class AiPathfindingDatum : TagStructure
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

        [TagStructure(Size = 0x78)]
        public class UnknownBlock3 : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string Name;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;
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

        [TagStructure(Size = 0x14)]
        public class ScenarioWeatherPaletteBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;
            [TagField(ValidTags = new[] { "rain" })]
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
        public class CreatureInstance : PermutationInstance
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
            public byte[] Function;
            public short Unknown;
            public short Unknown2;
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
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
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
            [TagStructure(Size = 0x84, MaxVersion = CacheVersion.HaloReach)]
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

                [TagField(MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
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

                    [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Length = 1)]
                    [TagField(MinVersion = CacheVersion.HaloReach, Length = 4)]
                    public int[] ConnectivityBitVector;
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
            public short Index;
        }

        [TagStructure(Size = 0xBC, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC8, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xBC, MinVersion = CacheVersion.HaloOnlineED)]
        public class DesignerZoneSet : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public uint Unknown;
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
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock5 : TagStructure
		{
            public short Unknown;
            public short Unknown2;
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
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
        }

        [TagStructure(Size = 0x10)]
        public class UnknownBlock7 : TagStructure
		{
            public RealPoint3d Position;
            public short Unknown4;
            public short Unknown5;
        }

        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.Halo4)]
        public class ScenarioCubemapBlock : TagStructure
        {
            public StringId Name;
            public RealPoint3d CubemapPosition;
            public CubemapResolutionEnum CubemapResolution;

            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            [TagField(MinVersion = CacheVersion.Halo4)]
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
            public short Unknown5;
            public short Unknown6;
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