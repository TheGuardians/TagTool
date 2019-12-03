using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0xA, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xE, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class PitchRangeParameter : TagStructure
	{
        public short NaturalPitch;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public short Unknown;

        public Bounds<short> BendBounds;
        public Bounds<short> MaxGainPitchBounds;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public Bounds<short> UnknownBounds;
    }
}