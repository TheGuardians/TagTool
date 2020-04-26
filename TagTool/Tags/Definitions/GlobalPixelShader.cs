using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_pixel_shader", Tag = "glps", Size = 0x1C)]
    public class GlobalPixelShader : TagStructure
	{
        public List<EntryPointBlock> EntryPoints;
        public uint Unknown2;
        public List<PixelShaderBlock> Shaders;

        [TagStructure(Size = 0x10)]
        public class EntryPointBlock : TagStructure
		{
            public List<OptionBlock> Option;
            public int ShaderIndex; // this is used if there is no option block

            [TagStructure(Size = 0x10)]
            public class OptionBlock : TagStructure
			{
                public short RenderMethodOptionIndex;

                [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
                public byte[] Unused_02;

                public List<int> OptionMethodShaderIndices; // the value is the shader index
            }
        }
    }
}
