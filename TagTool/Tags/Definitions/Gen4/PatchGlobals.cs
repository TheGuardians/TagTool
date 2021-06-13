using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "patch_globals", Tag = "patg", Size = 0x484)]
    public class PatchGlobals : TagStructure
    {
        public LanguagePackDefinition LanguagePack1;
        public LanguagePackDefinition LanguagePack2;
        public LanguagePackDefinition LanguagePack3;
        public LanguagePackDefinition LanguagePack4;
        public LanguagePackDefinition LanguagePack5;
        public LanguagePackDefinition LanguagePack6;
        public LanguagePackDefinition LanguagePack7;
        public LanguagePackDefinition LanguagePack8;
        public LanguagePackDefinition LanguagePack9;
        public LanguagePackDefinition LanguagePack10;
        public LanguagePackDefinition LanguagePack11;
        public LanguagePackDefinition LanguagePack12;
        public LanguagePackDefinition LanguagePack13;
        public LanguagePackDefinition LanguagePack14;
        public LanguagePackDefinition LanguagePack15;
        public LanguagePackDefinition LanguagePack16;
        public LanguagePackDefinition LanguagePack17;
        
        [TagStructure(Size = 0x44)]
        public class LanguagePackDefinition : TagStructure
        {
            public int StringReferencePointer;
            public int StringDataPointer;
            public int NumberOfStrings;
            public int StringDataSize;
            public int StringReferenceCacheOffset;
            public int StringDataCacheOffset;
            [TagField(Length = 20)]
            public DataHashDefinition[]  StringReferenceChecksum;
            [TagField(Length = 20)]
            public DataHashDefinition[]  StringDataChecksum;
            public int DataLoadedBoolean;
            
            [TagStructure(Size = 0x1)]
            public class DataHashDefinition : TagStructure
            {
                public byte HashByte;
            }
        }
    }
}
