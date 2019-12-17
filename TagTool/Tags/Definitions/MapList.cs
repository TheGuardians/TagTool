using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "map_list", Tag = "mlst", Size = 0x2C)]
    public class MapList : TagStructure
    {
        public List<MapImage> MapImages;
        public CachedTagInstance DefaultMapImage;
        public CachedTagInstance DefaultLoadingScreen;

        [TagStructure(Size = 0x20)]
        public class MapImage : TagStructure
        {
            public int MapId;
            public CachedTagInstance Bitmap;
            public List<CachedTagInstance> LoadingScreenBitmaps;
        }
    }
}
