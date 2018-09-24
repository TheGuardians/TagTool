using TagTool.Serialization;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_scale_animation_definition", Tag = "wscl", Size = 0x24)]
    public class GuiWidgetScaleAnimationDefinition : TagStructure
	{
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagStructure(Size = 0x24)]
        public class AnimationDefinitionBlock : TagStructure
		{
            public uint Frame;
            public AnchorValue Anchor;
            public short Unknown;
            public RealPoint2d CustomAnchor;
            public RealVector2d Scale;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;

            public enum AnchorValue : short
            {
                Custom,
                Center,
                TopCenter,
                BottomCenter,
                LeftCenter,
                RightCenter,
                TopLeft,
                TopRight,
                BottomRight,
                BottomLeft
            }
        }
    }
}