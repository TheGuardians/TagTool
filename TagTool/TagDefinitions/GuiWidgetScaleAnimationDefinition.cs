using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gui_widget_scale_animation_definition", Tag = "wscl", Size = 0x24)]
    public class GuiWidgetScaleAnimationDefinition
    {
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public byte[] Data;

        [TagStructure(Size = 0x24)]
        public class AnimationDefinitionBlock
        {
            public uint Frame;
            public AnchorValue Anchor;
            public short Unknown;
            public float CustomAnchorX;
            public float CustomAnchorY;
            public float XScale;
            public float YScale;
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
                BottomLeft,
            }
        }
    }
}
