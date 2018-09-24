using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    public class GuiWidgetTextureCoordinateAnimationDefinition : TagStructure
	{
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public TagFunction Function;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;

        [TagStructure(Size = 0x18)]
        public class AnimationDefinitionBlock : TagStructure
		{
            public uint Frame;
            public RealPoint2d Coordinate;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}
