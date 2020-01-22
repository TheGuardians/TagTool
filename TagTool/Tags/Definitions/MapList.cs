using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "map_list", Tag = "mlst", Size = 0x2C)]
    public class MapList : TagStructure
    {
        public List<MapImage> MapImages;
        public CachedTag DefaultMapImage;
        public CachedTag DefaultLoadingScreen;

        [TagStructure(Size = 0x20)]
        public class MapImage : TagStructure
        {
            public int MapId;
            public CachedTag Bitmap;
            public List<CachedTag> LoadingScreenBitmaps;
        }
    }
}
