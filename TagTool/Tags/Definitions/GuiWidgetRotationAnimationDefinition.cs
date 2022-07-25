using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED)]
    public class GuiWidgetRotationAnimationDefinition : TagStructure
    {
        public WidgetComponentAnimationFlags Flags;
        public List<WidgetRotationAnimationKeyframeBlock> Keyframes;
        public TagFunction DefaultFunction;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown2;

        [Flags]
        public enum WidgetComponentAnimationFlags : uint
        {
            LoopCyclic = 1 << 0,
            LoopReverse = 1 << 1
        }

        [TagStructure(Size = 0x20)]
        public class WidgetRotationAnimationKeyframeBlock : TagStructure
        {
            public int TimeOffset; // milliseconds
            public WidgetPositioning SpecialLocalOrigin;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Rotationpad0;
            public RealPoint2d LocalOrigin;
            public float Angle; // degrees
            public List<TagFunction> CustomTransitionFxn;

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
        }
    }
}
