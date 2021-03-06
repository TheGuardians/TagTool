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
        /// <summary>
        /// the apparent pitch when these samples are played at their recorded pitch.
        /// </summary>
        public short NaturalPitch;  // cents

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public short FirstDeterministicFlagIndex;

        /// <summary>
        /// the range of pitches that will be represented using this sample.
        /// </summary>
        public Bounds<short> BendBounds;    // cents

        /// <summary>
        /// the range of pitches that map to full gain.
        /// </summary>
        public Bounds<short> MaxGainPitchBounds;    // cents

        /// <summary>
        /// the actual pitch will be clamped to this
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public Bounds<short> PlaybackBendBounds;
    }
}