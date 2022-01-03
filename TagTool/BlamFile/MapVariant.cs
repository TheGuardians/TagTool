using System;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Common;

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
        public MultiplayerTeamDesignator Team;
        public byte SharedStorage;
        public byte SpawnTime;
        public MultiplayerObjectType ObjectType;
        public MultiplayerObjectBoundary Shape;
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

    public enum MultiplayerObjectFlags : sbyte
    {
        Unknown = (1 << 0),
        PlacedAtStart = (1 << 1),
        Symmetric = (1 << 2),
        Asymmetric = (1 << 3)
    }

    [TagStructure(Size = 0x11)]
    public class MultiplayerObjectBoundary
    {
        [TagField(EnumType = typeof(sbyte))]
        public MultiplayerObjectBoundaryShape Type;
        public float WidthRadius;
        public float BoxLength;
        public float PositiveHeight;
        public float NegativeHeight;
    }
}