using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "hud_message_text", Tag = "hmt ", Size = 0x80)]
    public class HudMessageText : TagStructure
    {
        public byte[] TextData;
        public List<HudStateMessageElement> MessageElements;
        public List<HudStateMessageDefinition> Messages;
        [TagField(Flags = Padding, Length = 84)]
        public byte[] Padding1;
        
        [TagStructure(Size = 0x2)]
        public class HudStateMessageElement : TagStructure
        {
            public sbyte Type;
            public sbyte Data;
        }
        
        [TagStructure(Size = 0x40)]
        public class HudStateMessageDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short StartIndexIntoTextBlob;
            public short StartIndexOfMessageBlock;
            public sbyte PanelCount;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding2;
        }
    }
}

