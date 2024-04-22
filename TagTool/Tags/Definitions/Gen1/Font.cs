using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "font", Tag = "font", Size = 0x9C)]
    public class Font : TagStructure
    {
        public int Flags;
        public short AscendingHeight;
        public short DecendingHeight;
        public short LeadingHeight;
        public short LeadinWidth;
        [TagField(Length = 0x24)]
        public byte[] Padding;
        public List<FontCharacterTables> CharacterTables;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag Bold;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag Italic;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag Condense;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag Underline;
        public List<Character> Characters;
        public byte[] Pixels;
        
        [TagStructure(Size = 0xC)]
        public class FontCharacterTables : TagStructure
        {
            public List<FontCharacterTable> CharacterTable;
            
            [TagStructure(Size = 0x2)]
            public class FontCharacterTable : TagStructure
            {
                public short CharacterIndex;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class Character : TagStructure
        {
            public short Character1;
            public short CharacterWidth;
            public short BitmapWidth;
            public short BitmapHeight;
            public short BitmapOriginX;
            public short BitmapOriginY;
            public short HardwareCharacterIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public int PixelsOffset;
        }
    }
}

