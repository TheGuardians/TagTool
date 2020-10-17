using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_screen", Tag = "rmss", Size = 0x8)]
    public class ShaderScreen : RenderMethod
    {
        public StringId Material;
        public byte Layer;
        public byte SortingOrder;
        public byte Flags;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x1)]
        public byte[] Padding;
    }
}
