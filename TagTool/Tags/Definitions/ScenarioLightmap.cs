using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x58, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x4C, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    public class ScenarioLightmap : TagStructure
	{
        public uint Unknown;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public List<ScenarioLightmapBspData> Lightmaps;

        public List<DataReferenceBlock> LightmapDataReferences;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CachedTag> Unknown2;

        public List<Airprobe> Airprobes;
        public List<SceneryLightProbe> SceneryLightProbes;
        public List<MachineLightProbes> MachineLightProbes;

        // block is always empty, format not known
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<int> Unknown5;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown3Reach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown4Reach;

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
        public class DataReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "Lbsp" })]
            public CachedTag LightmapBspData;
            [TagField(ValidTags = new[] { "wetn" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag WetnessBspData;
        }
    }
}
