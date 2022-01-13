using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED)]
    public class GuiWidgetTextureCoordinateAnimationDefinition : TagStructure
    {
        public WidgetComponentAnimationFlags Flags;
        public List<WidgetTextureCoordinateAnimationKeyframeBlock> Keyframes;
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

        [TagStructure(Size = 0x18)]
        public class WidgetTextureCoordinateAnimationKeyframeBlock : TagStructure
        {
            public int TimeOffset; // milliseconds
            public RealVector2d UVs;
            public List<KeyframeTransitionFunctionBlock> CustomTransitionFxn;

            [TagStructure(Size = 0x14)]
            public class KeyframeTransitionFunctionBlock : TagStructure
            {
                public byte[] CustomFunction;
            }
        }
    }
}
