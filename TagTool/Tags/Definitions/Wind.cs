using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x74, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x80, MinVersion = CacheVersion.HaloOnline106708)]
    public class Wind
    {
        public TagFunction DirectionMapping = new TagFunction { Data = new byte[0] };
        public TagFunction SpeedMapping = new TagFunction { Data = new byte[0] };
        public TagFunction BendMapping = new TagFunction { Data = new byte[0] };
        public TagFunction OscillationMapping = new TagFunction { Data = new byte[0] };
        public TagFunction FrequencyMapping = new TagFunction { Data = new byte[0] };

        public float GustSize;
        public CachedTagInstance GustNoiseBitmap;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}