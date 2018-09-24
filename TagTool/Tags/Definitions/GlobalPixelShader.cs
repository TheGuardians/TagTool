using TagTool.Serialization;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_pixel_shader", Tag = "glps", Size = 0x1C)]
    public class GlobalPixelShader : TagStructure
	{
        public List<DrawMode> DrawModes;
        public uint Unknown2;
        public List<PixelShaderBlock> Shaders;

        [TagStructure(Size = 0x10)]
        public class DrawMode : TagStructure
		{
            public List<UnknownBlock2> Unknown;
            public uint Unknown2;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2 : TagStructure
			{
                public uint Unknown;
                public List<UnknownBlock> Unknown2;

                [TagStructure(Size = 0x4)]
                public class UnknownBlock : TagStructure
				{
                    public uint Unknown;
                }
            }
        }
    }
}
