using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "device_terminal", Tag = "term", Size = 0x48)]
    public class DeviceTerminal : Device
    {
        public int BahBah;
        public StringId ActionString;
        // text pulled from strings tag above
        public StringId Name;
        [TagField(ValidTags = new [] { "sndo" })]
        public CachedTag ActivationSound;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag Strings;
        public List<TerminalPageBlock> Pages;
        
        [TagStructure(Size = 0x8)]
        public class TerminalPageBlock : TagStructure
        {
            public short BitmapSequenceIndex;
            public short BitmapSpriteIndex;
            public StringId Text;
        }
    }
}
