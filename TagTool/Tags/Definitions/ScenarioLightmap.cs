using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

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

        public List<CachedTag> LightmapDataReferences;
        public List<CachedTag> Unknown2;
        public List<Airprobe> Airprobes;
        public List<SceneryLightProbe> SceneryLightProbes;
        public List<MachineLightProbes> MachineLightProbes;
        // block is always empty, format not known
        public List<int> Unknown5;
    }
}