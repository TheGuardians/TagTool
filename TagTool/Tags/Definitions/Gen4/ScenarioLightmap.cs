using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x10)]
    public class ScenarioLightmap : TagStructure
    {
        public int JobGuid;
        public List<ScenarioLightmapBspDataReferenceBlock> LightmapBspReferences;
        
        [TagStructure(Size = 0x20)]
        public class ScenarioLightmapBspDataReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "Lbsp" })]
            public CachedTag LightmapBspDataReference;
            [TagField(ValidTags = new [] { "wetn" })]
            public CachedTag WetnessBspReference;
        }
    }
}
