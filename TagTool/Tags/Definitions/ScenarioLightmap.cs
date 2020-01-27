using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x58, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x4C, MinVersion = CacheVersion.Halo3ODST)]
    public class ScenarioLightmap : TagStructure
	{
        public uint Unknown;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public List<ScenarioLightmapBspData> Lightmaps;

        public List<CachedTag> LightmapDataReferences;
        public List<TagReferenceBlock> Unknown2;
        public List<Airprobe> Airprobes;
        public List<UnknownSHBlock2> Unknown3;
        public List<UnknownSHBlock3> Unknown4;
        // block is always empty, format not known
        public List<int> Unknown5;
    }
}