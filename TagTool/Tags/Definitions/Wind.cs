using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x84, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x78, MinVersion = CacheVersion.HaloReach)]
    public class Wind : TagStructure
	{
        public TagFunction Direction = new TagFunction { Data = new byte[0] };
        public TagFunction Speed = new TagFunction { Data = new byte[0] };
        public TagFunction Bend = new TagFunction { Data = new byte[0] };
        public TagFunction Oscillation = new TagFunction { Data = new byte[0] };
        public TagFunction Frequency = new TagFunction { Data = new byte[0] };

        public float GustSize; // world units
        [TagField(ValidTags = new[] { "bitm" })] public CachedTag GustNoiseBitmap;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] PadHO;
    }
}