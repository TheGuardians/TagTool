using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multilingual_unicode_string_list", Tag = "unic", Size = 0x34)]
    public class MultilingualUnicodeStringList : TagStructure
    {
        public List<MultilingualUnicodeStringReferenceBlock> StringReferences;
        public byte[] StringDataUtf8;
        [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x28)]
        public class MultilingualUnicodeStringReferenceBlock : TagStructure
        {
            public StringId StringId;
            public int EnglishOffset;
            public int JapaneseOffset;
            public int GermanOffset;
            public int FrenchOffset;
            public int SpanishOffset;
            public int ItalianOffset;
            public int KoreanOffset;
            public int ChineseOffset;
            public int PortugueseOffset;
        }
    }
}

