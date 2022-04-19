using System;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_screen", Tag = "rmss", Size = 0x8)]
    public class ShaderScreen : RenderMethod
    {
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public StringId Material;
        public GlobalScreenShaderRenderLayerEnum RenderLayer;
        public byte SortingOrder;
        public GlobalScreenShaderFlagsDefinition ScreenRenderFlags;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x1)]
        public byte[] Padding;


        public enum GlobalScreenShaderRenderLayerEnum : sbyte
        {
            PreUi,
            PostUi
        }

        [Flags]
        public enum GlobalScreenShaderFlagsDefinition : byte
        {
            ResolveScreen = 1 << 0
        }
    }
}
