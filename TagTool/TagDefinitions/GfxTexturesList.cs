using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gfx_textures_list", Tag = "gfxt", Size = 0x10)]
    public class GfxTexturesList
    {
        public List<Texture> Textures;
        public uint Unknown;

        [TagStructure(Size = 0x110)]
        public class Texture
        {
            [TagField(Length = 256)] public string FileName;
            public CachedTagInstance Bitmap;
        }
    }
}