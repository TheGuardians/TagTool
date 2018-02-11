using TagTool.Common;
using TagTool.Serialization;
using System.Runtime.InteropServices;

namespace TagTool.Cache
{
    [TagStructure(Size = 0xE090, MinVersion = CacheVersion.HaloOnline106708)]
    public class MapVariant
    {
        public ulong ID; // not sure about this

        [TagField(Length = 16, CharSet = CharSet.Unicode)]
        public string Name;

        [TagField(Length = 128)]
        public string Description;

        [TagField(Length = 16)]
        public string Author;

        public uint Type;
        public uint Unknown1; // 0
        public uint Unknown2; // 1
        public uint Unknown3; // 0
        public uint Size; // 0xE1F0 for maps, 0x3BC for variants
        public uint Unknown4; // 0
        public ulong Timestamp;
        public uint Unknown6; // 0
        public uint Unknown7; // -1
        public uint Unknown8; // 0x140 for maps, -1 for variants,
        public uint Unknown9; // 0 for maps, 4 for variants
        public uint Unknown10; // -1;
        public uint Unknown11; // 0;
        public uint Unknown12; // 0;
        public uint Unknown13; // 0;
        public ushort UnknownF8;
        public ushort ScnrPlacementsCount;
        public ushort UsedPlacementsCount;
        public ushort BudgetEntryCount;
        public uint MapId;
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public uint ContentType;
        public float MaxBudget;
        public float CurrentBudget;
        public uint Unknown128;
        public uint Unknown12C;

        [TagField(Length = 640)]
        public Placement[] Placements;

        [TagField(Length = 16)]
        public ushort[] ScnrIndices = new ushort[16];

        [TagField(Length = 256)]
        public BudgetEntry[] BudgetEntries = new BudgetEntry[256];

        [TagField(Padding = true, Length = 320)]
        public byte[] Unused = new byte[320];

        [TagStructure(Size = 0x54)]
        public class Placement
        {
            public ushort PlacementFlags;
            public ushort Unknown02;
            public uint ObjectIndex;
            public uint EditorObjectIndex;
            public uint BudgetIndex;
            public RealPoint3d Position;
            public RealVector3d Right;
            public RealVector3d Up;
            public uint Unknown34;
            public uint Unknown38;
            public ushort EngineFlags;
            public byte ObjectFlags;
            public byte TeamAffilation;
            public byte SharedStorage;
            public byte RespawnTime;
            public byte ObjectType;
            public byte ZoneShape;
            public float ZoneRadiusWidth;
            public float ZoneDepth;
            public float ZoneTop;
            public float ZoneBottom;
        }

        [TagStructure(Size = 0xC)]
        public class BudgetEntry
        {
            public int TagIndex;
            public byte RuntimeMin;
            public byte RuntimeMax;
            public byte CountOnMap;
            public byte DesignTimeMax;
            public float Cost;
        }
    }
}