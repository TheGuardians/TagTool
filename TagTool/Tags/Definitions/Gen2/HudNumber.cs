using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "hud_number", Tag = "hud#", Size = 0x64)]
    public class HudNumber : TagStructure
    {
        public CachedTag DigitsBitmap;
        public sbyte BitmapDigitWidth;
        public sbyte ScreenDigitWidth;
        public sbyte XOffset;
        public sbyte YOffset;
        public sbyte DecimalPointWidth;
        public sbyte ColonWidth;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 76)]
        public byte[] Padding2;
    }
}

