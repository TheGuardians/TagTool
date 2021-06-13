using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "script_container", Tag = "hscn", Size = 0x28)]
    public class ScriptContainer : TagStructure
    {
        public HsScriptDataStruct ScriptData;
        
        [TagStructure(Size = 0x28)]
        public class HsScriptDataStruct : TagStructure
        {
            public List<HsSourceReferenceBlock> SourceFileReferences;
            public List<HsSourceReferenceBlock> ExternalSourceReferences;
            [TagField(ValidTags = new [] { "hsdt" })]
            public CachedTag CompiledScript;
            
            [TagStructure(Size = 0x10)]
            public class HsSourceReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "hsc*" })]
                public CachedTag Reference;
            }
        }
    }
}
