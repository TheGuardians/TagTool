using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.GUI;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_color_animation_definition", Tag = "wclr", Size = 0x24)]
    public class GuiWidgetColorAnimationDefinition : TagStructure
	{
        public WidgetComponentAnimationFlags Flags;
        public List<WidgetColorAnimationKeyframeBlock> Keyframes;
        public TagFunction DefaultFunction;

        [Flags]
        public enum WidgetComponentAnimationFlags : uint
        {
            LoopCyclic = 1 << 0,
            LoopReverse = 1 << 1
        }

        [TagStructure(Size = 0x20)]
        public class WidgetColorAnimationKeyframeBlock : TagStructure
        {
            public int TimeOffset; // milliseconds
            public RealArgbColor Color;
            public List<KeyframeTransitionFunctionBlock> CustomTransitionFxn;

            [TagStructure(Size = 0x14)]
            public class KeyframeTransitionFunctionBlock : TagStructure
            {
                public byte[] CustomFunction;
            }
        }
    }
}