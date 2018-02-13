using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x2C)]
    [TagStructure(Name = "gui_widget_texture_coordinate_animation_definition", Tag = "wtuv", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    public class GuiWidgetTextureCoordinateAnimationDefinition
    {
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public byte[] Data;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;

        [TagStructure(Size = 0x18)]
        public class AnimationDefinitionBlock
        {
            public uint Frame;
            public float CoordinateX;
            public float CoordinateY;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}
