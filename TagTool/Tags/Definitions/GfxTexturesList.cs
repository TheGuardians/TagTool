using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gfx_textures_list", Tag = "gfxt", Size = 0x10)]
    public class GfxTexturesList : TagStructure
	{
        public List<Texture> Textures;
        public uint Unknown;

        [TagStructure(Size = 0x110)]
        public class Texture : TagStructure
		{
            [TagField(Length = 252)] public string FileName;
            public int MapId;
            public CachedTagInstance Bitmap;
        }
    }
}