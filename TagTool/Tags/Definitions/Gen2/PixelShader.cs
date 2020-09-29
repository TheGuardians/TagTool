using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "pixel_shader", Tag = "pixl", Size = 0x18)]
    public class PixelShader : TagStructure
    {
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public byte[] CompiledShader;
    }
}

