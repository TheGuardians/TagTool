using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "lightmap_model_globals", Tag = "LMMg", Size = 0xC)]
    public class LightmapModelGlobals : TagStructure
    {
        public List<LightmapModelReferenceBlock> LightmappedModels;
        
        [TagStructure(Size = 0x10)]
        public class LightmapModelReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag ModelReference;
        }
    }
}
