using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1B4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1E4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1EC, MinVersion = CacheVersion.HaloOnline106708)]
    public class ScenarioLightmapBspData : TagStructure
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
        public CachedTag PrimaryMap;
        public CachedTag IntensityMap;
        public List<StaticPerVertexLighting> StaticPerVertexLightingBuffers;
        public List<ClusterStaticPerVertexLighting> ClusterStaticPerVertexLightingBuffers;
        public List<InstancedGeometryLighting> InstancedGeometry;
        public List<CompressedSH> InstancedSHCoefficients;
        public RenderGeometry Geometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<Airprobe> Airprobes;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownSHBlock2> Unknown64;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownSHBlock3> Unknown65;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown66;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown67;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown68;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [TagStructure(Size = 0x10)]
        public class StaticPerVertexLighting : TagStructure
		{
            public List<int> UnusedVertexBuffer;
            public int VertexBufferIndex;

            [TagField(Flags = Runtime)]
            public VertexBufferDefinition VertexBuffer;
        }

        [TagStructure(Size = 0x4)]
        public class ClusterStaticPerVertexLighting : TagStructure
		{
            public short LightmapBitmapImageIndex;
            public short StaticPerVertexLightingIndex;
        }

        [TagStructure(Size = 0x8)]
        public class InstancedGeometryLighting : TagStructure
		{
            public short Unknown;
            public short StaticPerVertexLightingIndex;
            public short UnknownSHCoefficientsIndex;
            public short Unknown2;
        }
    }

    [TagStructure(Size = 0x18)]
    public class ScenarioLightmapBspDataSection : TagStructure
	{
        public List<Header> Headers;
        public VertexList VertexLists;

        [TagStructure(Size = 0xC)]
        public class VertexList : TagStructure
		{
            public List<Datum> Vertex;

            [TagStructure(Size = 0x1)]
			public class Datum : TagStructure
			{
                public byte Value;
            }
        }

        [TagStructure(Size = 0x2C)]
        public class Header : TagStructure
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