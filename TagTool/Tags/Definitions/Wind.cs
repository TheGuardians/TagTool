using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x84, MinVersion = CacheVersion.HaloOnline106708)]
    public class Wind : TagStructure
	{
        public TagFunction DirectionMapping = new TagFunction { Data = new byte[0] };
        public TagFunction SpeedMapping = new TagFunction { Data = new byte[0] };
        public TagFunction BendMapping = new TagFunction { Data = new byte[0] };
        public TagFunction OscillationMapping = new TagFunction { Data = new byte[0] };
        public TagFunction FrequencyMapping = new TagFunction { Data = new byte[0] };

        public float GustSize;
        public CachedTag GustNoiseBitmap;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}