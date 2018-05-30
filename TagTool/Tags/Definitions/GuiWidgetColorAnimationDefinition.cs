using TagTool.Serialization;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_color_animation_definition", Tag = "wclr", Size = 0x24)]
    public class GuiWidgetColorAnimationDefinition
    {
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagStructure(Size = 0x20)]
        public class AnimationDefinitionBlock
        {
            public uint Frame;
            public RealArgbColor Color;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}