using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "unicode_string_list", Tag = "ustr", Size = 0xC)]
    public class UnicodeStringList : TagStructure
    {
        public List<StringReference> StringReferences;
        
        [TagStructure(Size = 0x14)]
        public class StringReference : TagStructure
        {
            public byte[] String;
        }
    }
}

