using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_rotation_animation_definition", Tag = "wrot", Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED)]
    public class GuiWidgetRotationAnimationDefinition : TagStructure
	{
        public GuiAnimationFlags AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown2;

        [TagStructure(Size = 0x20)]
        public class AnimationDefinitionBlock : TagStructure
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
