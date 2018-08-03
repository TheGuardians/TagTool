using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    public class GuiWidgetRotationAnimationDefinition
    {
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;

        [TagStructure(Size = 0x20)]
        public class AnimationDefinitionBlock
        {
            public uint Frame;
            public AnchorValue Anchor;
            public short Unknown;
            public float CustomAnchorX;
            public float CustomAnchorY;
            public float RotationAmount;
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
