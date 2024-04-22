using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "hud_number", Tag = "hud#", Size = 0x64)]
    public class HudNumber : TagStructure
    {
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DigitsBitmap;
        public sbyte BitmapDigitWidth;
        public sbyte ScreenDigitWidth;
        public sbyte XOffset;
        public sbyte YOffset;
        public sbyte DecimalPointWidth;
        public sbyte ColonWidth;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x4C)]
        public byte[] Padding1;
    }
}

