using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1B4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1E4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x15C, MinVersion = CacheVersion.HaloReach)]
    public class ScenarioLightmapBspData : TagStructure
	{
        public short Unknown;
        public short BspIndex;
        public int StructureChecksum;

        /// <summary>
        /// When sampling from the lightmap coefficient map, the resuling rgb SH coefficients are multiplied by this luminance scale.
        /// </summary>
        [TagField(Length = 9, MaxVersion = CacheVersion.HaloOnline700123)]
        public LuminanceScale[] CoefficientsMapScale;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag Unknown1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Brightness;

        public CachedTag LightmapSHCoefficientsBitmap;
        public CachedTag LightmapDominantLightDirectionBitmap;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown3;

        public List<StaticPerVertexLighting> StaticPerVertexLightingBuffers;
        public List<ClusterStaticPerVertexLighting> ClusterStaticPerVertexLightingBuffers;
        public List<InstancedGeometryLighting> InstancedGeometry;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HalfRGBLightProbe> InstancedGeometryLightProbes;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown6;

        public RenderGeometry Geometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<Airprobe> Airprobes;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SceneryLightProbe> SceneryLightProbes;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<MachineLightProbes> MachineLightProbes;
        /// <summary>
        /// Actually unused in all games. Probably intended for another object type.
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<int> Unused;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown8;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown9;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown11;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown12;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown13;


        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class StaticPerVertexLighting : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<int> UnusedVertexBuffer;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public int VertexBufferIndex;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short VertexBufferIndexReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ushort VertexBufferOffsetReach;

            [TagField(Flags = Runtime)]
            public VertexBufferDefinition VertexBuffer;
        }

        [TagStructure(Size = 0x4)]
        public class ClusterStaticPerVertexLighting : TagStructure
		{
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
        public class InstancedGeometryLighting : TagStructure
		{
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
            public short InstancedGeometryLightProbesIndex;
            public short Unknown1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown2;
        }

        [TagStructure(Size = 0x0)]
        public class NullBlock : TagStructure
        {
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