using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "multilingual_unicode_string_list", Tag = "unic", Size = 0x70)]
    public class MultilingualUnicodeStringList : TagStructure
    {
        public List<MultilingualUnicodeStringReferenceBlock> StringReferences;
        public List<StringSubstitutionPairBlock> SubstitutionPairs;
        public byte[] StringDataUtf8;
        [TagField(Length = 17)]
        public MultilingualUnicodeStringListLanguagePackOffsets[]  LanguagePackOffsets;
        
        [TagStructure(Size = 0x48)]
        public class MultilingualUnicodeStringReferenceBlock : TagStructure
        {
            public StringId StringId;
            public int EnglishOffset;
            public int JapaneseOffset;
            public int GermanOffset;
            public int FrenchOffset;
            public int SpanishOffset;
            public int MexicanSpanishOffset;
            public int ItalianOffset;
            public int KoreanOffset;
            public int TraditionalChineseOffset;
            public int SimplifiedChineseOffset;
            public int PortugueseOffset;
            public int PolishOffset;
            public int RussianOffset;
            public int DanishOffset;
            public int FinnishOffset;
            public int DutchOffset;
            public int NorwegianOffset;
        }
        
        [TagStructure(Size = 0xC)]
        public class StringSubstitutionPairBlock : TagStructure
        {
            public StringId FirstStringId;
            public StringId SecondStringId;
            public int AssociatedValue;
        }
        
        [TagStructure(Size = 0x4)]
        public class MultilingualUnicodeStringListLanguagePackOffsets : TagStructure
        {
            public short StartIndex;
            public short StringCount;
        }
    }
}
