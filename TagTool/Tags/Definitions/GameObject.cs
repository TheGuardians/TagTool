using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "object", Tag = "obje", Size = 0xF8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "object", Tag = "obje", Size = 0x104, Version = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "object", Tag = "obje", Size = 0x120, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "object", Tag = "obje", Size = 0x178, MinVersion = CacheVersion.HaloReach)]
    public class GameObject : TagStructure
	{
        public GameObjectType16 ObjectType;

        [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.HaloReach)]
        public byte[] pad = new byte[2];

        public ObjectDefinitionFlags ObjectFlags;

        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public float AccelerationScale; // [0,+inf] marine 1.0, grunt 1.4, elite 0.9, hunter 0.5, etc.

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float VerticalAccelerationScale;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AngularAccelerationScale;

        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        public WaterDensityType WaterDensity;
        public int RuntimeFlags;
        public float DynamicLightSphereRadius; // sphere to use for dynamic lights and shadows. only used if not 0
        public RealPoint3d DynamicLightSphereOffset; // only used if radius not 0

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId GenericHUDText;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag GenericNameList;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag GenericServiceTagList;

        public StringId DefaultModelVariant;

        [TagField(ValidTags = new[] { "hlmt" })]
        public CachedTag Model;

        [TagField(ValidTags = new[] { "bloc" }, MaxVersion = CacheVersion.HaloOnline416097)]
        [TagField(ValidTags = new[] { "bloc" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag CrateObject;

        [TagField(ValidTags = new[] { "cddf" })]    // only set this tag if you want to override the default collision damage values in globals.globals
        public CachedTag CollisionDamage;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag BrittleCollisionDamage;

        public List<EarlyMoverProperty> EarlyMoverProperties;

        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag CreationEffect;

        [TagField(ValidTags = new[] { "foot" })]
        public CachedTag MaterialEffects;

        [TagField(ValidTags = new[] { "arms" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag ArmorSounds;

        [TagField(ValidTags = new[] { "snd!", "scmb" })]
        public CachedTag MeleeImpactSound; // sound made when I am meleed. Overrides the sweetener sound of my material.

        public List<AiProperty> AiProperties;
        public List<Function> Functions;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ObjectRuntimeInterpolatorFunctionsBlock> RuntimeInterpolatorFunctions;

        public short HudTextMessageIndex;

        public ObjectDefinitionSecondaryFlags SecondaryFlags;

        public List<Attachment> Attachments;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<WaterPhysicsHullSurface> HullSurfaces;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<JetwashBlock> Jetwash;

        public List<TagReferenceBlock> Widgets;

        public List<ChangeColor> ChangeColors;

        [TagField(Gen = CacheGeneration.Third, Platform = CachePlatform.Original)]
        public List<PredictedResource> PredictedResources;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NodeMap> NodeMaps;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<MultiplayerObjectBlock> MultiplayerObject;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public CachedTag SimulationInterpolation;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(Platform = CachePlatform.MCC)]
        public List<TagReferenceBlock> RevivingEquipment;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<SpawnEffectsBlock> SpawnEffects;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<PathfindingSphere> PathfindingSpheres;

        [TagField(Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        public CachedTag SimulationInterpolationODSTMCC;

        public enum LightmapShadowModeValue : short
        {
            Default,
            Never,
            Always,
            Blur
        }

        public enum MultipleAirprobeModeValue : sbyte
        {
            Default,
            Always
        }

        public enum SweetenerSizeValue : sbyte
        {
            Default,
            Small,
            Medium,
            Large
        }

        public enum WaterDensityType : sbyte
        {
            Default,
            SuperFloater,
            Floater,
            Neutral,
            Sinker,
            SuperSinker,
            None
        }

        [TagStructure(Size = 0x4)]
        public class ObjectRuntimeInterpolatorFunctionsBlock : TagStructure
        {
            public int RuntimeInterpolatorToObjectFunctionMapping;
        }

        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class SpawnEffectsBlock : TagStructure
        {
            public TagReference MultiplayerSpawnEffect;
            public TagReference SurvivalSpawnEffect;
            public TagReference CampaignSpawnEffect;
        }

        [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class EarlyMoverProperty : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId NodeName;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int RuntimeNodeIndex;

            public Bounds<float> XBounds;
            public Bounds<float> YBounds;
            public Bounds<float> ZBounds;
            public RealEulerAngles3d Angles;
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        public class AiProperty : TagStructure
		{
            public AiPropertiesFlags AiFlags;
            public StringId AiTypeName;

            [TagField(Length = 0x4, Flags = Padding, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId InteractionName;

            public AiSizeEnum AiSize;
            public GlobalAiJumpHeight LeapJumpSpeed;

            [Flags]
            public enum AiPropertiesFlags : uint
            {
                None = 0,
                DestroyableCover = 1 << 0,
                PathfindingIgnoreWhenDead = 1 << 1,
                DynamicCover = 1 << 2,
                NonFlightBlocking = 1 << 3,
                DynamicCoverFromCentre = 1 << 4,
                HasCornerMarkers = 1 << 5,
                Inspectable = 1 << 6,
                IdleWhenFlying = 1 << 7
            }

            public enum AiSizeEnum : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }

        }

        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
        public class Function : TagStructure
		{
            public ObjectFunctionFlags Flags;
            public StringId ImportName;
            public StringId ExportName;
            public StringId TurnOffWith; // if the specified function is off, so is this function

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId RangedInterpolationName;

            public float MinimumValue; // function must exceed this value (after mapping) to be active. 0 means do nothing
            public TagFunction DefaultFunction = new TagFunction { Data = new byte[0] };
            public StringId ScaleBy;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<FunctionInterpolation> FunctionInterpolations;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int RuntimeInterpolatorIndex;

            [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
            public class FunctionInterpolation : TagStructure
            {
                public int InterpolationMode;
                public float LinearTravelTime;
                public float Acceleration;
                public float SpringK;
                public float SpringC;
                public float Fraction;
            }

            [Flags]
            public enum ObjectFunctionFlags : int
            {
                None = 0,
                Invert = 1 << 0, // result of function is one minus actual result
                MappingDoesNotControlsActive = 1 << 1, // the curve mapping can make the function active/inactive
                AlwaysActive = 1 << 2, // function does not deactivate when at or below lower bound
                RandomTimeOffset = 1 << 3, // function offsets periodic function input by random value between 0 and 1
                AlwaysExportsValue = 1 << 4, // when this function is deactivated, it still exports its value
                TurnOffWithUsesMagnitude = 1 << 5 // the function will be turned off if the value of the turns_off_with function is 0
            }
        }

        [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
        public class Attachment : TagStructure
		{
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public AtlasFlagsValue AtlasFlags;

            [TagField(Flags = Label, ValidTags = new[] { "ligh", "gldf", "ltvl", "effe", "lsnd", "lens", "cpem" })]
            public CachedTag Type;

            public StringId Marker;
            public ChangeColorValue ChangeColor;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;

            public StringId PrimaryScale;
            public StringId SecondaryScale;

            [Flags]
            public enum AtlasFlagsValue : int
            {
                None = 0,
                GameplayVisionMode = 1 << 0,
                TheaterVisionMode = 1 << 1
            }

            public enum ChangeColorValue : short
            {
                None,
                Primary,
                Secondary,
                Tertiary,
                Quaternary
            }

        }
        
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail)]
        public class ChangeColor : TagStructure
		{
            public List<InitialPermutation> InitialPermutations;
            public List<ChangeColorFunction> Functions;

            [TagStructure(Size = 0x20)]
            public class InitialPermutation : TagStructure
			{
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                [TagField(Flags = Label)]
                public StringId VariantName; // if empty, may be used by any model variant
            }

            [TagStructure(Size = 0x28, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x24, Platform = CachePlatform.MCC)]
            public class ChangeColorFunction : TagStructure
            {
                [TagField(Flags = TagFieldFlags.Padding, Length = 4, Platform = CachePlatform.Original)]
                public byte[] Unused = new byte[4];
                public GlobalRgbInterpolationFlags ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;

                [Flags]
                public enum GlobalRgbInterpolationFlags : int
                {
                    None = 0,
                    BlendInHsv = 1 << 0, // blends colors in hsv rather than rgb space
                    MoreColors = 1 << 1 // blends colors through more hues (goes the long way around the color wheel)
                }
            }
        }

        [TagStructure(Size = 0x8)]
        public class PredictedResource : TagStructure
		{
            public short Type;
            public short ResourceIndex;
            [TagField(Flags = Short)]
            public CachedTag TagIndex;
        }

        [TagStructure(Size = 0x1)]
        public class NodeMap : TagStructure
		{
            public sbyte TargetNode;
        }

        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xBC, MinVersion = CacheVersion.HaloReach)]
        public class MultiplayerObjectBlock : TagStructure
		{
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public GameEngineFlagsReach ReachEngineFlags;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public GameEngineSubTypeFlags EngineFlags;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MultiplayerObjectType Type;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectTypeReach TypeReach;

            public TeleporterPassabilityFlags TeleporterPassability;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MultiplayerObjectFlags Flags;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MultiplayerObjectBoundaryShape BoundaryShape;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MultiplayerObjectSpawnTimerType SpawnTimerType;

            [TagField(Flags = Padding, Length = 1, MinVersion = CacheVersion.HaloReach)]
            public byte[] pad = new byte[1];

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short DefaultSpawnTime; // seconds
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short DefaultAbandonTime;

            public float BoundaryWidthRadius;
            public float BoundaryBoxLength;
            public float BoundaryPositiveHeight;
            public float BoundaryNegativeHeight;

            [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectBoundaryShape ReachBoundaryShape;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectSpawnTimerType SpawnTimerTypeReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short SpawnTimeReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short AbandonTimeReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MultiplayerObjectFlagsReach FlagsReach;

            //only the first of these exists in Reach
            public float StandardRespawnZoneWeight;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float FlagAwayRespawnZoneWeight;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float FlagAtHomeRespawnZoneWeight;

            public StringId BoundaryCenterMarker;
            public StringId SpawnedObjectMarkerName;

            [TagField(ValidTags = new[] { "obje" })]
            public CachedTag SpawnedObject;

            public StringId NyiBoundaryMaterial;

            [TagField(Length = (int)MultiplayerObjectBoundaryShape.Count)]
            public BoundaryShader[] BoundaryShaders = new BoundaryShader[(int)MultiplayerObjectBoundaryShape.Count];

            [Flags]
            public enum MultiplayerObjectFlags : ushort
            {
                None,
                OnlyVisibleInEditor = 1 << 0,
                ValidInitialPlayerSpawn = 1 << 1,
                FixedBoundaryOrientation = 1 << 2,
                CandyMonitorShouldIgnore = 1 << 3,
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

            [Flags]
            public enum MultiplayerObjectFlagsReach : ushort
            {
                None,
                OnlyVisibleInEditor = 1 << 0,
                PhasedPhysicsInForge = 1 << 1,
                ValidInitialPlayerSpawn = 1 << 2,
                FixedBoundaryOrientation = 1 << 3,
                CandyMonitorShouldIgnore = 1 << 4,
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


            [TagStructure(Size = 0x20)]
            public class BoundaryShader : TagStructure
            {
                [TagField(ValidTags = new[] { "rm  " })]
                public CachedTag StandardShader;
                [TagField(ValidTags = new[] { "rm  " })]
                public CachedTag OpaqueShader;
            }
        }

        [Flags]
        public enum PathfindingSphereFlags : ushort
        {
            None = 0,
            RemainsWhenOpen = 1 << 0,
            VehicleOnly = 1 << 1,
            WithSectors = 1 << 2
        }

        [TagStructure(Size = 0x14)]
        public class PathfindingSphere : TagStructure
        {
            public short Node;
            public PathfindingSphereFlags Flags;
            public RealPoint3d Center;
            public float Radius;
        }
    }

    public enum GameObjectTypeHalo2
    {
        None = -1,
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

    public enum GameObjectTypeHalo3Retail
    {
        None = -1,
        Biped,
        Vehicle,
        Weapon,
        Equipment,
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

    public enum GameObjectTypeHalo3ODST
    {
        None = -1,
        Biped,
        Vehicle,
        Weapon,
        Equipment,
        AlternateRealityDevice,
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

    public enum GameObjectTypeHaloOnline
    {
        None = -1,
        Biped,
        Vehicle,
        Weapon,
        Armor,
        Equipment,
        AlternateRealityDevice,
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

    public enum GameObjectTypeHaloReach
    {
        None = -1,
        Biped,
        Vehicle,
        Weapon,
        Equipment,
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

    [TagStructure(Size = 0x1)]
    public class GameObjectType8 : TagStructure
    {
        [TagField(EnumType = typeof(sbyte), MaxVersion = CacheVersion.Halo2Vista, Platform = CachePlatform.Original)]
        public GameObjectTypeHalo2 Halo2;

        [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public GameObjectTypeHalo3Retail Halo3Retail;

        [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
        public GameObjectTypeHalo3ODST Halo3ODST;

        [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public GameObjectTypeHaloOnline HaloOnline;

        [TagField(EnumType = typeof(sbyte), MinVersion = CacheVersion.HaloReach)]
        public GameObjectTypeHalo3Retail HaloReach;

        public Enum GetValue(CacheVersion version)
        {
            if (version <= CacheVersion.Halo2Vista)
                return Halo2;
            else if (version <= CacheVersion.Halo3Retail)
                return Halo3Retail;
            else if (version <= CacheVersion.HaloOnline449175)
                return Halo3ODST;
            else if (version <= CacheVersion.HaloOnline700123)
                return HaloOnline;
            else if (version <= CacheVersion.HaloReach)
                return HaloReach;
            else
                throw new FormatException(version.ToString());
        }

        public void SetValue(CacheVersion version, Enum value)
        {
            if (version <= CacheVersion.Halo2Vista)
                Halo2 = value.ConvertLexical<GameObjectTypeHalo2>();
            else if (version <= CacheVersion.Halo3Retail)
                Halo3Retail = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else if (version <= CacheVersion.HaloOnline449175)
                Halo3ODST = value.ConvertLexical<GameObjectTypeHalo3ODST>();
            else if (version <= CacheVersion.HaloOnline700123)
                HaloOnline = value.ConvertLexical<GameObjectTypeHaloOnline>();
            else if (version <= CacheVersion.HaloReach)
                HaloReach = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else
                throw new FormatException(version.ToString());
        }
    }

    [TagStructure(Size = 0x2)]
    public class GameObjectType16 : TagStructure
    {
        [TagField(EnumType = typeof(short), MaxVersion = CacheVersion.Halo2Vista, Platform = CachePlatform.Original)]
        public GameObjectTypeHalo2 Halo2;

        [TagField(EnumType = typeof(short), MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public GameObjectTypeHalo3Retail Halo3Retail;

        [TagField(EnumType = typeof(short), MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
        public GameObjectTypeHalo3ODST Halo3ODST;

        [TagField(EnumType = typeof(short), MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public GameObjectTypeHaloOnline HaloOnline;

        [TagField(EnumType = typeof(short), MinVersion = CacheVersion.HaloReach)]
        public GameObjectTypeHalo3Retail HaloReach;

        public Enum GetValue(CacheVersion version)
        {
            if (version <= CacheVersion.Halo2Vista)
                return Halo2;
            else if (version <= CacheVersion.Halo3Retail)
                return Halo3Retail;
            else if (version <= CacheVersion.HaloOnline449175)
                return Halo3ODST;
            else if (version <= CacheVersion.HaloOnline700123)
                return HaloOnline;
            else if (version <= CacheVersion.HaloReach)
                return HaloReach;
            else
                throw new FormatException(version.ToString());
        }

        public void SetValue(CacheVersion version, Enum value)
        {
            if (version <= CacheVersion.Halo2Vista)
                Halo2 = value.ConvertLexical<GameObjectTypeHalo2>();
            else if (version <= CacheVersion.Halo3Retail)
                Halo3Retail = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else if (version <= CacheVersion.HaloOnline449175)
                Halo3ODST = value.ConvertLexical<GameObjectTypeHalo3ODST>();
            else if (version <= CacheVersion.HaloOnline700123)
                HaloOnline = value.ConvertLexical<GameObjectTypeHaloOnline>();
            else if (version <= CacheVersion.HaloReach)
                HaloReach = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else
                throw new FormatException(version.ToString());
        }
    }

    [TagStructure(Size = 0x4)]
    public class GameObjectType32 : TagStructure
    {
        [TagField(EnumType = typeof(int), MaxVersion = CacheVersion.Halo2Vista, Platform = CachePlatform.Original)]
        public GameObjectTypeHalo2 Halo2;

        [TagField(EnumType = typeof(int), MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public GameObjectTypeHalo3Retail Halo3Retail;

        [TagField(EnumType = typeof(int), MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
        public GameObjectTypeHalo3ODST Halo3ODST;

        [TagField(EnumType = typeof(int), MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public GameObjectTypeHaloOnline HaloOnline;

        [TagField(EnumType = typeof(int), MinVersion = CacheVersion.HaloReach)]
        public GameObjectTypeHalo3Retail HaloReach;

        public Enum GetValue(CacheVersion version)
        {
            if (version <= CacheVersion.Halo2Vista)
                return Halo2;
            else if (version <= CacheVersion.Halo3Retail)
                return Halo3Retail;
            else if (version <= CacheVersion.HaloOnline449175)
                return Halo3ODST;
            else if (version <= CacheVersion.HaloOnline700123)
                return HaloOnline;
            else if (version <= CacheVersion.HaloReach)
                return HaloReach;
            else
                throw new FormatException(version.ToString());
        }

        public void SetValue(CacheVersion version, Enum value)
        {
            if (version <= CacheVersion.Halo2Vista)
                Halo2 = value.ConvertLexical<GameObjectTypeHalo2>();
            else if (version <= CacheVersion.Halo3Retail)
                Halo3Retail = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else if (version <= CacheVersion.HaloOnline449175)
                Halo3ODST = value.ConvertLexical<GameObjectTypeHalo3ODST>();
            else if (version <= CacheVersion.HaloOnline700123)
                HaloOnline = value.ConvertLexical<GameObjectTypeHaloOnline>();
            else if (version <= CacheVersion.HaloReach)
                HaloReach = value.ConvertLexical<GameObjectTypeHalo3Retail>();
            else
                throw new FormatException(version.ToString());
        }
    }


    [Flags]
    public enum ObjectTypeFlagsHalo2 : ushort
    {
        None,
        Biped = 1 << 0,
        Vehicle = 1 << 1,
        Weapon = 1 << 2,
        Equipment = 1 << 3,
        Garbage = 1 << 4,
        Projectile = 1 << 5,
        Scenery = 1 << 6,
        Machine = 1 << 7,
        Control = 1 << 8,
        LightFixture = 1 << 9,
        SoundScenery = 1 << 10,
        Crate = 1 << 11,
        Creature = 1 << 12
    }

    [Flags]
    public enum ObjectTypeFlagsHalo3Retail : ushort
    {
        None,
        Biped = 1 << 0,
        Vehicle = 1 << 1,
        Weapon = 1 << 2,
        Equipment = 1 << 3,
        Terminal = 1 << 4,
        Projectile = 1 << 5,
        Scenery = 1 << 6,
        Machine = 1 << 7,
        Control = 1 << 8,
        SoundScenery = 1 << 9,
        Crate = 1 << 10,
        Creature = 1 << 11,
        Giant = 1 << 12,
        EffectScenery = 1 << 13
    }

    [Flags]
    public enum ObjectTypeFlagsHalo3ODST : ushort
    {
        None,
        Biped = 1 << 0,
        Vehicle = 1 << 1,
        Weapon = 1 << 2,
        Equipment = 1 << 3,
        AlternateRealityDevice = 1 << 4,
        Terminal = 1 << 5,
        Projectile = 1 << 6,
        Scenery = 1 << 7,
        Machine = 1 << 8,
        Control = 1 << 9,
        SoundScenery = 1 << 10,
        Crate = 1 << 11,
        Creature = 1 << 12,
        Giant = 1 << 13,
        EffectScenery = 1 << 14
    }

    [Flags]
    public enum ObjectTypeFlagsHaloOnline : ushort
    {
        None,
        Biped = 1 << 0,
        Vehicle = 1 << 1,
        Weapon = 1 << 2,
        Armor = 1 << 3,
        Equipment = 1 << 4,
        AlternateRealityDevice = 1 << 5,
        Terminal = 1 << 6,
        Projectile = 1 << 7,
        Scenery = 1 << 8,
        Machine = 1 << 9,
        Control = 1 << 10,
        SoundScenery = 1 << 11,
        Crate = 1 << 12,
        Creature = 1 << 13,
        Giant = 1 << 14,
        EffectScenery = 1 << 15
    }

    [TagStructure(Size = 0x2)]
    public class ObjectTypeFlags : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public ObjectTypeFlagsHalo2 Halo2;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public ObjectTypeFlagsHalo3Retail Halo3Retail;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
        public ObjectTypeFlagsHalo3ODST Halo3ODST;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public ObjectTypeFlagsHaloOnline HaloOnline;
    }

    [Flags]
    public enum GameObjectFlagsReach : int
    {
        None = 0,
        DoesNotCastShadow = 1 << 0,
        SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
        PreservesInitialDamageOwner = 1 << 2,
        NotAPathfindingObstacle = 1 << 3,
        ExtensionOfParent = 1 << 4,
        DoesNotCauseCollisionDamage = 1 << 5,
        EarlyMover = 1 << 6,
        EarlyMoverLocalizedPhysics = 1 << 7,
        UseStaticMassiveLightmapSample = 1 << 8,
        ObjectScalesAttachments = 1 << 9,
        InheritsPlayersAppearance = 1 << 10,
        NonPhysicalInMapEditor = 1 << 11,
        AttachToClustersByDynamicSphere = 1 << 12,
        EffectsDoNotSpawnObjectsInMultiplayer = 1 << 13,
        DoesNotCollideWithCamera = 1 << 14,
        DamageNotBlockedByObstructions = 1 << 15
    }

    [TagStructure(Size = 0x2, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
    public class ObjectDefinitionFlags : VersionedFlags
    {
        [TagField(MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        public ObjectFlags Flags;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public ObjectFlagsMCC FlagsMCC;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GameObjectFlagsReach FlagsReach;
    }

    [Flags]
    public enum ObjectFlagsMCC : ushort
    {
        DoesNotCastShadow = 1 << 0,
        SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
        PreservesInitialDamageOwner = 1 << 2,
        NotAPathfindingObstacle = 1 << 3,
        // object passes all function values to parent and uses parent's markers
        ExtensionOfParent = 1 << 4,
        DoesNotCauseCollisionDamage = 1 << 5,
        EarlyMover = 1 << 6,
        EarlyMoverLocalizedPhysics = 1 << 7,
        ObjectScalesAttachments = 1 << 8,
        NonPhysicalInMapEditor = 1 << 9,
        // use this for the mac gun on spacestation
        AttachToClustersByDynamicSphere = 1 << 10,
        EffectsCreatedByThisObjectDoNotSpawnObjectsInMultiplayer = 1 << 11,
        // specificly the flying observer camera
        DoesNotCollideWithCamera = 1 << 12,
        // AOE damage being applied to this object does not test for obstrutions.
        DamageNotBlockedByObstructions = 1 << 13
    }

    [Flags]
    public enum ObjectFlags : ushort
    {
        None = 0,
        DoesNotCastShadow = 1 << 0,
        SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
        PreservesInitialDamageOwner = 1 << 2,
        NotAPathfindingObstacle = 1 << 3,
        ExtensionOfParent = 1 << 4, // object passes all function values to parent and uses parent's markers
        DoesNotCauseCollisionDamage = 1 << 5,
        EarlyMover = 1 << 6,
        EarlyMoverLocalizedPhysics = 1 << 7,
        UseStaticMassiveLightmapSample = 1 << 8,
        ObjectScalesAttachments = 1 << 9,
        InheritsPlayersAppearance = 1 << 10,
        NonPhysicalInMapEditor = 1 << 11, // formerly DeadBipedsCantLocalize, which was probably incorrect
        AttachToClustersByDynamicSphere = 1 << 12, // use this for the mac gun on spacestation
        EffectsDoNotSpawnObjectsInMultiplayer = 1 << 13,
        DoesNotCollideWithCamera = 1 << 14, // specifically the flying observer camera
        DamageNotBlockedByObstructions = 1 << 15 // AOE damage being applied to this object does not test for obstrutions.
    }

    public enum GlobalAiJumpHeight : short
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
    public enum ObjectDefinitionSecondaryFlags : ushort
    {
        None = 0,
        DoesNotAffectProjectileAiming = 1 << 0
    }

    [TagStructure(Size = 0x18)]
    public class WaterPhysicsHullSurface : TagStructure
    {
        public WaterPhysicsHullSurfaceDefinitionFlags Flags;
        [TagField(Length = 0x2, Flags = Padding)]
        public byte[] pad0;
        public StringId MarkerName;
        public float Radius;
        public List<WaterPhysicsMaterialOverride> Drag;

        [Flags]
        public enum WaterPhysicsHullSurfaceDefinitionFlags : ushort
        {
            WorksOnLand = 1 << 0, // drives on an extruded version of everything physical in your level
            EffectsOnly = 1 << 1
        }

        [TagStructure(Size = 0x3C)]
        public class WaterPhysicsMaterialOverride : TagStructure
        {
            public StringId Material;
            public WaterPhysicsDragPropertiesStruct Drag;

            [TagStructure(Size = 0x38)]
            public class WaterPhysicsDragPropertiesStruct : TagStructure
            {
                public PhysicsForceFunctionStruct Pressure;
                public PhysicsForceFunctionStruct Suction;
                public float LinearDamping;
                public float AngularDamping;

                [TagStructure(Size = 0x18)]
                public class PhysicsForceFunctionStruct : TagStructure
                {
                    public TagFunction VelocityToPressure;
                    public float MaxVelocity; // wu/s
                }
            }
        }
    }

    [TagStructure(Size = 0x24)]
    public class JetwashBlock : TagStructure
    {
        public StringId MarkerName;
        public float Radius;
        public int MaximumTraces; // traces per second
        public float MaximumEmissionLength; // world units
        public Bounds<Angle> TraceYawAngle; // degrees
        public Bounds<Angle> TracePitchAngle; // degrees
        public float ParticleOffset; // world units
    }
}
