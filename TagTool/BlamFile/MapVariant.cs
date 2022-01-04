using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.BlamFile
{
    [TagStructure(Size = 0xE090)]
    public class MapVariant : TagStructure
    {
        public ContentItemMetadata Metadata;
        public short Version;
        public short ScenarioObjectCount;
        public short VariantObjectCount;
        public short PlaceableQuotaCount;
        public int MapId = -1;
        public RealRectangle3d WorldBounds;
        public GameEngineSubType RuntimeEngineSubType = GameEngineSubType.All;
        public float MaximumBudget;
        public float SpentBudget;
        public bool RuntimeShowHelpers;
        public bool BuiltIn;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public uint MapChecksum;

        [TagField(Length = 640)]
        public VariantObjectDatum[] Objects;

        [TagField(Length = 14, MaxVersion = CacheVersion.Halo3Retail)]
        [TagField(Length = 16, MinVersion = CacheVersion.Halo3ODST)]
        public short[] ObjectTypeStartIndex;

        [TagField(Length = 0x100)]
        public VariantObjectQuota[] Quotas;

        [TagField(Length = 80)]
        public int[] SimulationEntities;
    }

    [TagStructure(Size = 0x54)]
    public class VariantObjectDatum : TagStructure
    {
        public VariantObjectPlacementFlags Flags;
        public short RuntimeRemovalTimer;
        public int RuntimeObjectIndex = -1;
        public int RuntimeEditorObjectIndex = -1;
        public int QuotaIndex = -1;
        public RealPoint3d Position;
        public RealVector3d Forward;
        public RealVector3d Up;
        public ObjectIdentifier ParentObject;
        public VariantMultiplayerProperties Properties;
    }

    [TagStructure(Size = 0x18)]
    public class VariantMultiplayerProperties : TagStructure
    {
        public GameEngineSubTypeFlags EngineFlags = GameEngineSubTypeFlags.All;
        public VariantPlacementFlags Flags;
        public MultiplayerTeamDesignator Team = MultiplayerTeamDesignator.Neutral;
        public byte SharedStorage; // spare clips, teleporter channel, spawn order
        public byte SpawnTime;
        public MultiplayerObjectType Type;
        public MultiplayerObjectBoundary Boundary = new MultiplayerObjectBoundary();
    }

    [TagStructure(Size = 0xC)]
    public class VariantObjectQuota : TagStructure
    {
        [TagField(Platform = CachePlatform.Original)]
        public int ObjectDefinitionIndex = -1;
        public byte MinimumCount;
        public byte MaximumCount;
        public byte PlacedOnMap;
        public byte MaxAllowed = 255;
        public float Cost = -1.0f;
    }

    [TagStructure(Size = 0x8)]
    public class ObjectIdentifier : TagStructure
    {
        public DatumHandle UniqueID = DatumHandle.None;
        public short BspIndex = -1;
        public sbyte Type = -1;
        public sbyte Source = -1;
    }

    [TagStructure(Size = 0x11)]
    public class MultiplayerObjectBoundary : TagStructure
    {
        [TagField(EnumType = typeof(sbyte))]
        public MultiplayerObjectBoundaryShape Type;
        public float WidthRadius;
        public float BoxLength;
        public float PositiveHeight;
        public float NegativeHeight;
    }

    [Flags]
    public enum VariantObjectPlacementFlags : ushort
    {
        OccupiedSlot = 1 << 0,            // not an empty slot
        Edited = 1 << 1,                  // set whenever the object has been edited in any way
        RuntimeIgnored = 1 << 2,          // hack for globally placed scenario objects
        ScenarioObject = 1 << 3,          // set for all scenario placements
        Unused4 = 1 << 4,                 // unused
        ScenarioObjectRemoved = 1 << 5,   // scenario object has been deleted
        RuntimeSandboxSuspended = 1 << 6, // object has been suspended by the sandbox engine
        RuntimeCandyMonitored = 1 << 7,   // object is being candy monitored
        SpawnsRelative = 1 << 8,          // position and axes are relative to the parent
        SpawnsAttached = 1 << 9           // object will be attached to the parent (node 0)
    }

    public enum VariantPlacementFlags : byte
    {
        UniqueSpawn = 1 << 0,
        NotInitiallyPlaced = 1 << 1,
        Symmetric = 1 << 2,
        Asymmetric = 1 << 3
    }
}
