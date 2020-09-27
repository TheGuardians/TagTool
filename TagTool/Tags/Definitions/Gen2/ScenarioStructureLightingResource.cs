using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_lighting_resource", Tag = "sslt", Size = 0xC)]
    public class ScenarioStructureLightingResource : TagStructure
    {
        public List<ScenarioStructureBspSphericalHarmonicLighting> StructureLighting;
        
        [TagStructure(Size = 0x1C)]
        public class ScenarioStructureBspSphericalHarmonicLighting : TagStructure
        {
            public CachedTag Bsp;
            public List<RealPoint3d> LightingPoints;
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Position;
            }
        }
    }
}

