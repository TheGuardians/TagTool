using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "location_name_globals_definition", Tag = "locs", Size = 0x1C)]
    public class LocationNameGlobalsDefinition : TagStructure
    {
        public List<LocationNameBlock> LocationNames;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag LocationNameStringList;
        
        [TagStructure(Size = 0x4)]
        public class LocationNameBlock : TagStructure
        {
            public StringId Name;
        }
    }
}
