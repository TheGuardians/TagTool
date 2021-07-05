using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_scenery", Tag = "ssce", Size = 0x10, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_scenery", Tag = "ssce", Size = 0x1C, MinVersion = CacheVersion.HaloOnlineED)]
    public class SoundScenery : GameObject
    {
        public Bounds<float> Distance;
        public Bounds<Angle> ConeAngle;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused2;
    }
}