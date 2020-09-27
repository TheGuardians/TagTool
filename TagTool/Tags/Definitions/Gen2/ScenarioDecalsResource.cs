using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_decals_resource", Tag = "dec*", Size = 0x18)]
    public class ScenarioDecalsResource : TagStructure
    {
        public List<ScenarioDecalPaletteEntry> Palette;
        public List<ScenarioDecal> Decals;
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalPaletteEntry : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecal : TagStructure
        {
            public short DecalType;
            public sbyte Yaw127127;
            public sbyte Pitch127127;
            public RealPoint3d Position;
        }
    }
}

