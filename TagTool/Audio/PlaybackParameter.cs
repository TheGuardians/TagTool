using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x44,  MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
    public class PlaybackParameter : TagStructure
	{
        /// <summary>
        /// the distance below which this sound no longer gets louder
        /// </summary>
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MininumDistance; // world units

        /// <summary>
        /// the distance beyond which this sound is no longer audible
        /// </summary>
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumDistance; // world units

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public FieldDisableFlagsValue FieldDisableFlags;


        // TODO: reverse code to figure out if that's actually true
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SkipFractionReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaximumBendPerSecondReach;

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public SoundDistanceParameters DistanceParameters;


        /// <summary>
        /// fraction of requests to play this sound that will be ignored (0 means always play.)
        /// </summary>
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float SkipFraction;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float MaximumBendPerSecond;  // cents


        /// <summary>
        /// these settings control random variation of volume and pitch.
        ///  the second parameter gets clipped to the first.
        /// </summary>

        /// <summary>
        /// sound's random gain will start here
        /// </summary>
        public float GainBase;  // dB

        /// <summary>
        /// sound's gain will be randomly modulated within this range
        /// </summary>
        public float GainVariance; // dB

        /// <summary>
        /// the sound's pitch will be modulated randomly within this range.
        /// </summary>
        public Bounds<short> RandomPitchBounds;

        /// <summary>
        /// these settings allow sounds to be directional, fading as they turn away from the listener
        /// </summary>
        /// <summary>
        /// within the cone defined by this angle and the sound's direction, the sound plays with a gain of 1.0.
        /// </summary>
        public Angle InnerConeAngle; // degrees

        /// <summary>
        /// outside the cone defined by this angle and the sound's direction, the sound plays with a gain of OUTER CONE GAIN. (0
        /// means the sound does not attenuate with direction.)
        /// </summary>
        public Angle OuterConeAngle; // degrees

        /// <summary>
        /// the gain to use when the sound is directed away from the listener
        /// </summary>
        public float OuterConeGain;

        /// <summary>
        /// NOTE: this will only apply when the sound is started via script
        /// azimuth:
        ///     0 => front
        ///     90 => left
        ///     180 => back
        ///   
        /// 270 => right
        /// 
        /// </summary>
        public FlagsValue Flags;
        public Angle Azimuth;
        public float PositionalGain;    // dB
        public float FirstPersonGain;    // dB

        [Flags]
        public enum FieldDisableFlagsValue : int
        {
            None = 0,
            DistanceA = 1 << 0,
            DistanceB = 1 << 1,
            DistanceC = 1 << 2,
            DistanceD = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15,
            Bit16 = 1 << 16,
            Bit17 = 1 << 17,
            Bit18 = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20,
            Bit21 = 1 << 21,
            Bit22 = 1 << 22,
            Bit23 = 1 << 23,
            Bit24 = 1 << 24,
            Bit25 = 1 << 25,
            Bit26 = 1 << 26,
            Bit27 = 1 << 27,
            Bit28 = 1 << 28,
            Bit29 = 1 << 29,
            Bit30 = 1 << 30,
            Bit31 = 1 << 31
        }

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            OverrideAzimuth = 1 << 0,
            Override3dGain = 1 << 1,
            OverrideSpeakerGain = 1 << 2,
        }
    }
}