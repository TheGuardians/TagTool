using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_decals_resource", Tag = "dec*", Size = 0x10)]
    public class ScenarioDecalsResource : TagStructure
    {
        public List<ScenarioDecalPaletteBlock> Palette;
        public List<ScenarioDecalsBlock> Decals;
        
        [TagStructure(Size = 0x8)]
        public class ScenarioDecalPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "deca" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalsBlock : TagStructure
        {
            public short DecalType;
            public sbyte Yaw127127;
            public sbyte Pitch127127;
            public RealPoint3d Position;
        }
    }
}

