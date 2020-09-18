using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1B4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x1E4, MinVersion = CacheVersion.Halo3ODST)]
    public class ScenarioLightmapBspData : TagStructure
	{
        public short Unknown;
        public short BspIndex;
        public int StructureChecksum;

        /// <summary>
        /// When sampling from the lightmap coefficient map, the resuling rgb SH coefficients are multiplied by this luminance scale.
        /// </summary>
        [TagField(Length = 9)]
        public LuminanceScale[] CoefficientsMapScale;

        public CachedTag LightmapSHCoefficientsBitmap;
        public CachedTag LightmapDominantLightDirectionBitmap;
        public List<StaticPerVertexLighting> StaticPerVertexLightingBuffers;
        public List<ClusterStaticPerVertexLighting> ClusterStaticPerVertexLightingBuffers;
        public List<InstancedGeometryLighting> InstancedGeometry;
        public List<HalfRGBLightProbe> InstancedGeometryLightProbes;
        public RenderGeometry Geometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<Airprobe> Airprobes;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<SceneryLightProbe> SceneryLightProbes;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<MachineLightProbes> MachineLightProbes;

        /// <summary>
        /// Actually unused in all games. Probably intended for another object type.
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<int> Unused;

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
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
        }

        [TagStructure(Size = 0x8)]
        public class InstancedGeometryLighting : TagStructure
		{
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
            public short InstancedGeometryLightProbesIndex;
            public short Padding;
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