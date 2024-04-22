using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "hud_message_text", Tag = "hmt ", Size = 0x80)]
    public class HudMessageText : TagStructure
    {
        public byte[] TextData;
        public List<HudMessageElementsBlock> MessageElements;
        public List<HudMessagesBlock> Messages;
        [TagField(Length = 0x54)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x2)]
        public class HudMessageElementsBlock : TagStructure
        {
            public sbyte Type;
            public sbyte Data;
        }
        
        [TagStructure(Size = 0x40)]
        public class HudMessagesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short StartIndexIntoTextBlob;
            public short StartIndexOfMessageBlock;
            public sbyte PanelCount;
            [TagField(Length = 0x3)]
            public byte[] Padding;
            [TagField(Length = 0x18)]
            public byte[] Padding1;
        }
    }
}

