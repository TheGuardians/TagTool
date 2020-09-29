using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_lighting_resource", Tag = "sslt", Size = 0x8)]
    public class ScenarioStructureLightingResource : TagStructure
    {
        public List<ScenarioStructureBspSphericalHarmonicLightingBlock> StructureLighting;
        
        [TagStructure(Size = 0x10)]
        public class ScenarioStructureBspSphericalHarmonicLightingBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag Bsp;
            public List<ScenarioSphericalHarmonicLightingPoint> LightingPoints;
            
            [TagStructure(Size = 0xC)]
            public class ScenarioSphericalHarmonicLightingPoint : TagStructure
            {
                public RealPoint3d Position;
            }
        }
    }
}

