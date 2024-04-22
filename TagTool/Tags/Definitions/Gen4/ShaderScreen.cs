using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "shader_screen", Tag = "rmss", Size = 0x8)]
    public class ShaderScreen : RenderMethod
    {
        public StringId MaterialName;
        public GlobalScreenShaderRenderLayerEnum RenderLayer;
        public sbyte SortOrder;
        public GlobalScreenShaderFlags RenderFlags;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        public enum GlobalScreenShaderRenderLayerEnum : sbyte
        {
            PreUi,
            PostUi,
            PreTransparents
        }
        
        [Flags]
        public enum GlobalScreenShaderFlags : byte
        {
            ResolveScreen = 1 << 0
        }
    }
}
