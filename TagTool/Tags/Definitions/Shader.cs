using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "shader", Tag = "rmsh", Size = 0x4)]
    public class Shader : RenderMethod
    {
        public StringId Material;
    }
}