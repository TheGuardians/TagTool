using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_color_animation_definition", Tag = "wclr", Size = 0x24)]
    public class GuiWidgetColorAnimationDefinition : TagStructure
	{
        public GuiAnimationFlags AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagStructure(Size = 0x20)]
        public class AnimationDefinitionBlock : TagStructure
		{
            public uint Frame;
            public RealArgbColor Color;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}