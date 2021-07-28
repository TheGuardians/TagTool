using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_sprite_animation_definition", Tag = "wspr", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_sprite_animation_definition", Tag = "wspr", Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED)]
    public class GuiWidgetSpriteAnimationDefinition : TagStructure
	{
        public GuiAnimationFlags AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown2;

        [TagStructure(Size = 0x14)]
        public class AnimationDefinitionBlock : TagStructure
		{
            public uint Frame;
            public short SpriteIndex;
            public short SpriteIndex2;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}