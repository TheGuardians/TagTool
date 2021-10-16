using System.Collections.Generic;
using System;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_scale_animation_definition", Tag = "wscl", Size = 0x24)]
    public class GuiWidgetScaleAnimationDefinition : TagStructure
    {
        public WidgetComponentAnimationFlags Flags;
        public List<WidgetScaleAnimationKeyframeBlock> Keyframes;
        public TagFunction DefaultFunction;

        [Flags]
        public enum WidgetComponentAnimationFlags : uint
        {
            LoopCyclic = 1 << 0,
            LoopReverse = 1 << 1
        }

        [TagStructure(Size = 0x24)]
        public class WidgetScaleAnimationKeyframeBlock : TagStructure
        {
            public int TimeOffset; // milliseconds
            public WidgetPositioning SpecialLocalOrigin; // if used, overrides field below
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Rotationpad0;
            public RealPoint2d LocalOrigin;
            public RealVector2d ScaleFactor;
            public List<KeyframeTransitionFunctionBlock> CustomTransitionFxn;

            public enum WidgetPositioning : short
            {
                UNUSED,
                Centered,
                TopEdge,
                BottomEdge,
                LeftEdge,
                RightEdge,
                TopleftCorner,
                ToprightCorner,
                BottomrightCorner,
                BottomleftCorner
            }

            [TagStructure(Size = 0x14)]
            public class KeyframeTransitionFunctionBlock : TagStructure
            {
                public byte[] CustomFunction;
            }
        }
    }
}