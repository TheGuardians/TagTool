using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1B4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1E4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1EC, MinVersion = CacheVersion.HaloOnline106708)]
    public class ScenarioLightmapBspData
    {
        public short Unknown;
        public short BspIndex;
        public int StructureChecksum;
        public float Shadows;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public float Midtones;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public float Highlights;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public uint Unknown16;
        public float TopDownWhites;
        public uint Unknown17;
        public uint Unknown18;
        public uint Unknown19;
        public uint Unknown20;
        public uint Unknown21;
        public float TopDownBlacks;
        public uint Unknown22;
        public uint Unknown23;
        public uint Unknown24;
        public uint Unknown25;
        public uint Unknown26;
        public uint Unknown27;
        public uint Unknown28;
        public uint Unknown29;
        public uint Unknown30;
        public uint Unknown31;
        public uint Unknown32;
        public uint Unknown33;
        public uint Unknown34;
        public uint Unknown35;
        public uint Unknown36;
        public uint Unknown37;
        public uint Unknown38;
        public uint Unknown39;
        public uint Unknown40;
        public uint Unknown41;
        public uint Unknown42;
        public uint Unknown43;
        public uint Unknown44;
        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;
        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;
        public CachedTagInstance PrimaryMap;
        public CachedTagInstance IntensityMap;
        public List<InstancedMesh> InstancedMeshes;
        public List<UnknownBlock> Unknown51;
        public List<InstancedGeometryBlock> InstancedGeometry;
        public List<UnknownBBlock> UnknownB;
        public RenderGeometry Geometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ScenarioLightmap.Airprobe> Airprobes;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ScenarioLightmap.UnknownBlock2> Unknown64;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ScenarioLightmap.UnknownBlock3> Unknown65;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown66;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown67;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown68;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [TagStructure(Size = 0x10)]
        public class InstancedMesh
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public int UnknownIndex;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock
        {
            public short Unknown;
            public short Unknown2;
        }

        [TagStructure(Size = 0x8)]
        public class InstancedGeometryBlock
        {
            public short Unknown;
            public short InstancedMeshIndex;
            public short UnknownBIndex;
            public short Unknown2;
        }

        [TagStructure(Size = 0x48)]
        public class UnknownBBlock
        {
            public short Unknown1;
            public short Unknown2;
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public short Unknown10;
            public short Unknown11;
            public short Unknown12;
            public short Unknown13;
            public short Unknown14;
            public short Unknown15;
            public short Unknown16;
            public short Unknown17;
            public short Unknown18;
            public short Unknown19;
            public short Unknown20;
            public short Unknown21;
            public short Unknown22;
            public short Unknown23;
            public short Unknown24;
            public short Unknown25;
            public short Unknown26;
            public short Unknown27;
            public short Unknown28;
            public short Unknown29;
            public short Unknown30;
            public short Unknown31;
            public short Unknown32;
            public short Unknown33;
            public short Unknown34;
            public short Unknown35;
            public short Unknown36;
        }
    }

    [TagStructure(Size = 0x18)]
    public class ScenarioLightmapBspDataSection
    {
        public List<Header> Headers;
        public VertexList VertexLists;

        [TagStructure(Size = 0xC)]
        public class VertexList
        {
            public List<Datum> Vertex;

            [TagStructure(Size = 0x1)]
			public /*was_struct*/ class Datum
            {
                public byte Value;
            }
        }

        [TagStructure(Size = 0x2C)]
        public class Header
        {
            public short Unknown00;
            public short Unknown01;
            public short Unknown02;
            public short Unknown03;
            public sbyte Unknown04;
            public sbyte Unknown05;
            public sbyte Unknown06;
            public sbyte Unknown07;
            public sbyte Unknown08;
            public sbyte Unknown09;
            public sbyte Unknown10;
            public sbyte Unknown11;
            public RealPoint3d Position;
            public int Unknown15;
            public int Unknown16;
            public int BytesCount;
            public int BytesCount2;
        }
    }
}