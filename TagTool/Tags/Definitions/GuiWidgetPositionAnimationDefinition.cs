using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_position_animation_definition", Tag = "wpos", Size = 0x24)]
    public class GuiWidgetPositionAnimationDefinition : TagStructure
	{
        public GuiAnimationFlags AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagStructure(Size = 0x1C)]
        public class AnimationDefinitionBlock : TagStructure
		{
            public uint Frame;
            public float XPosition;
            public float YPosition;
            public float ZPosition;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}
