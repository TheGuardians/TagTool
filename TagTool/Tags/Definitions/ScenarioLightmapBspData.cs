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
        public ushort Flags;
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

        public List<InstancedGeometryLightProbe> InstancedGeometryLightProbes;

        public RenderGeometry Geometry;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Unknown1Block> Unknown8;

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
            public short Coefficient; // half - shader constant

            [TagField(Flags = Runtime)]
            public VertexBufferDefinition VertexBuffer;
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class ClusterStaticPerVertexLighting : TagStructure
        {
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint AnalyticalLightIndex;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
        public class InstancedGeometryLighting : TagStructure
        {
            public short LightmapBitmapsImageIndex;
            public short StaticPerVertexLightingIndex;
            public short InstancedGeometryLightProbesIndex;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint AnalyticalLightIndex;
        }

        [TagStructure(Size = 0xC)]
        public class Unknown1Block : TagStructure
        {
            public List<Unknown1SubBlock> Data;

            [TagStructure(Size = 0x4)]
            public class Unknown1SubBlock : TagStructure
            {
                public int Unknown;
            }
        }

        [TagStructure(Size = 0x0)]
        public class NullBlock : TagStructure
        {
        }
    }

    [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    public class InstancedGeometryLightProbe : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public HalfRGBLightProbe LightProbe;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DualVmfLightProbe VmfLightProbe;
    }

    [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    public class Airprobe : TagStructure
    {
        public RealPoint3d Position;
        public StringId Name;
        public uint Flags;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public HalfRGBLightProbe LightProbe;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DualVmfLightProbe VmfLightProbe;
    }

    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    public class SceneryLightProbe : TagStructure
    {
        public ObjectIdentifier ObjectId;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public HalfRGBLightProbe LightProbe;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DualVmfLightProbe VmfLightProbe;
    }

    [TagStructure(Size = 0x2C)]
    public class MachineLightProbes : TagStructure
    {
        public ObjectIdentifier ObjectId;
        public RealRectangle3d Bounds;
        public List<MachineLightProbe> LightProbes;

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
        public class MachineLightProbe : TagStructure
        {
            public RealPoint3d Position;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public HalfRGBLightProbe LightProbe;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public DualVmfLightProbe VmfLightProbe;
        }
    }

    [TagStructure(Size = 0x24)]
    public class DualVmfLightProbe : TagStructure
    {
        [TagField(Length = 16)]
        public short[] VmfTerms;
        public uint AnalyticalLightIndex;
    }
}
