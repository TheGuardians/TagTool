using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_scenery", Tag = "ssce", Size = 0x10, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_scenery", Tag = "ssce", Size = 0x1C, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundScenery : GameObject
    {
        public Bounds<float> Distance;
        public Bounds<Angle> ConeAngle;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;
    }
}