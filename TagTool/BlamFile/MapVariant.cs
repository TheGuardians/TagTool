using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.BlamFile
{
    [TagStructure(Size = 0xE090)]
    public class MapVariant
	{
        public ContentItemMetadata Metadata;
        public short Version;
        public short ScenarioObjectCount;
        public short PlacedObjectCount;
        public short PaletteItemCount;
        public int MapId;
        public RealRectangle3d WorldBounds;
        public GameEngineType GameType;
        public float MaximumBudget;
        public float SpentBudget;
        public bool RuntimeShowHelpers;
        public bool DirtyFlag;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public uint MapChecksum;

        [TagField(Length = 640)]
        public MapVariantPlacement[] Placements;

        [TagField(Length = 0x10)]
        public short[] ScenarioDatumIndices;

        [TagField(Length = 0x100)]
        public MapVariantPaletteItem[] Palette;

        [TagField(Length = 80)]
        public int[] SimulationEntities;
    }

    [Flags]
    public enum GameEngineFlags : ushort
    {
        Ctf = 1,
        Slayer = 2,
        Oddball = 4,
        Koth = 8,
        Sandbox = 16,
        Vip = 32,
        Juggernaut = 64,
        Territories = 128,
        Assault = 256,
        Infection = 512
    }

    public enum GameEngineType : int
    {
        None,
        Ctf,
        Slayer,
        Oddball,
        Koth,
        Sandbox,
        Vip,
        Juggernaut,
        Territories,
        Assault,
        Infection,
    }

    public enum MapVariantGameTeam : sbyte
    {
        Defender,
        Attacker,
        Team3,
        Team4,
        Team5,
        Team6,
        Team7,
        Team8,
        Neutral
    }

    [Flags]
    public enum PlacementFlags : short
    {
        Valid = (1 << 0),
        Touched = (1 << 1),
        UnknownBit2 = (1 << 2),
        FromScenario = (1 << 3),
        UnknownBit4 = (1 << 4),
        Deleted = (1 << 5),
        UnknownBit6 = (1 << 6),
        UnknownBit7 = (1 << 7),
        Attached = (1 << 8),
        AttachedFixed = (1 << 9)
    }

    [TagStructure(Size = 0x18)]
    public class MapVariantPlacementProperty
    {
        public GameEngineFlags EngineFlags;
        public MultiplayerObjectFlags MultiplayerFlags;
        public MapVariantGameTeam TeamAffiliation;
        public byte SharedStorage;
        public byte SpawnTime;
        public MultiplayerObjectType ObjectType;
        public MultiplayerObjectBoundaryShape Shape;
    }

    [TagStructure(Size = 0x54)]
    public class MapVariantPlacement
    {
        public PlacementFlags PlacementFlags;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public int ObjectIndex;
        public int EditorObjectIndex;
        public int PaletteIndex;
        public RealPoint3d Position;
        public RealVector3d Forward;
        public RealVector3d Up;
        public ObjectIdentifier ParentScenarioObject;
        public MapVariantPlacementProperty Properties;
    }

    [TagStructure(Size = 0xC)]
    public class MapVariantPaletteItem
    {
        public int TagIndex;
        public byte RuntimeMin;
        public byte RuntimeMax;
        public byte CountOnMap;
        public byte PlacedOnMapMax;
        public float Cost;
    }

    [TagStructure(Size = 0x8)]
    public class ObjectIdentifier
    {
        public DatumHandle UniqueID;
        public short BspIndex;
        public sbyte Type;
        public sbyte Source;
    }

    public enum MultiplayerObjectType : sbyte
    {
        Ordinary = 0x0,
        Weapon = 0x1,
        Grenade = 0x2,
        Projectile = 0x3,
        Powerup = 0x4,
        Equipment = 0x5,
        LightLandVehicle = 0x6,
        HeavyLandVehicle = 0x7,
        FlyingVehicle = 0x8,
        Teleporter2Way = 0x9,
        TeleporterSender = 0xA,
        TeleporterReceiver = 0xB,
        PlayerSpawnLocation = 0xC,
        PlayerRespawnZone = 0xD,
        HoldSpawnObjective = 0xE,
        CaptureSpawnObjective = 0xF,
        HoldDestinationObjective = 0x10,
        CaptureDestinationObjective = 0x11,
        HillObjective = 0x12,
        InfectionHavenObjective = 0x13,
        TerritoryObjective = 0x14,
        VIPBoundaryObjective = 0x15,
        VIPDestinationObjective = 0x16,
        JuggernautDestinationObjective = 0x17,
    };

    public enum MultiplayerObjectFlags : sbyte
    {
        Unknown = (1 << 0),
        PlacedAtStart = (1 << 1),
        Symmetric = (1 << 2),
        Asymmetric = (1 << 3)
    }

    public enum MultiplayerObjectShapeType : sbyte
    {
        None,
        Sphere,
        Cylinder,
        Box
    }

    [TagStructure(Size = 0x11)]
    public class MultiplayerObjectBoundaryShape
    {
        public MultiplayerObjectShapeType Type;
        public float Width;
        public float Length;
        public float Top;
        public float Bottom;
    }
}