using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "pixel_shader", Tag = "pixl", Size = 0xC)]
    public class PixelShader : TagStructure
    {
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public byte[] CompiledShader;
    }
}

