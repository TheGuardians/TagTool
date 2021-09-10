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

        public List<DataReferenceBlock> LightmapDataReferences;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CachedTag> Unknown2;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> AirprobesReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown1Reach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown2Reach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown3Reach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> Unknown4Reach;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<Airprobe> Airprobes;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<SceneryLightProbe> SceneryLightProbes;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<MachineLightProbes> MachineLightProbes;
        // block is always empty, format not known
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<int> Unknown5;

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
        public class DataReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "Lbsp" })]
            public CachedTag LightmapBspData;
            [TagField(ValidTags = new[] { "wetn" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag WetnessBspData;
        }

        [TagStructure(Size = 0x0)]
        public class NullBlock : TagStructure { }
    }
}
