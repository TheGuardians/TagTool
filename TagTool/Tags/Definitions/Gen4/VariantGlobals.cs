using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "variant_globals", Tag = "vtgl", Size = 0x24)]
    public class VariantGlobals : TagStructure
    {
        public List<SingleVariantBlock> MapVariants;
        public List<SingleVariantBlock> GameVariants;
        public List<FallbackHopperFileBlock> FallbackHoppers;
        
        [TagStructure(Size = 0x128)]
        public class SingleVariantBlock : TagStructure
        {
            public byte[] Metadata;
            public byte[] FullData;
            [TagField(Length = 256)]
            public string Filename;
        }
        
        [TagStructure(Size = 0x114)]
        public class FallbackHopperFileBlock : TagStructure
        {
            public byte[] FileData;
            [TagField(Length = 256)]
            public string Filename;
        }
    }
}
