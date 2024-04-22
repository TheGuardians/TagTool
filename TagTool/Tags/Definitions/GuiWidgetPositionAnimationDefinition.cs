using System;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_position_animation_definition", Tag = "wpos", Size = 0x24)]
    public class GuiWidgetPositionAnimationDefinition : TagStructure
    {
        public WidgetComponentAnimationFlags Flags;
        public List<WidgetPositionAnimationKeyframeBlock> Keyframes;
        public TagFunction Function;

        [TagStructure(Size = 0x1C)]
        public class WidgetPositionAnimationKeyframeBlock : TagStructure
        {
            public int TimeOffset; // milliseconds
            public RealPoint3d Position;
            public List<TagFunction> CustomTransitionFxn;
        }

        [Flags]
        public enum WidgetComponentAnimationFlags : uint
        {
            LoopCyclic = 1 << 0,
            LoopReverse = 1 << 1
        }
    }
}
