using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x44, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
    public class PlaybackParameterStruct : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MininumDistanceGen2; // (world units) the distance below which this sound no longer gets louder

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumDistanceGen2; // (world units) the distance beyond which this sound is no longer audible

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public FieldDisableFlagsValue FieldDisableFlags; // "InternalFlags"


        // TODO: reverse code to figure out if that's actually true
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SkipFractionReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaximumBendPerSecondReach;

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public float DontPlayDistance;  // (wu) don't play below this distance
        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public float AttackDistance;    // (wu) start playing at full volume at this distance
        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public float MinimumDistance;   // (wu) the distance below which this sound no longer gets louder
        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public float MaximumDistance;   // (wu) the distance beyond which this sound is no longer audible

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DistanceE;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DistanceF;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DistanceG;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DistanceH;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float SkipFraction;  // fraction of requests to play this sound that will be ignored (0 means always play.)

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float MaximumBendPerSecond;  // (cents)

        // The following settings control random variation of volume and pitch. The second parameter gets clipped to the first.

        public float GainBase;  // (dB) sound's random gain will start here
        public float GainVariance; // (dB) sound's gain will be randomly modulated within this range
        public Bounds<short> RandomPitchBounds; // (cents) the sound's pitch will be modulated randomly within this range.
        // these settings allow sounds to be directional, fading as they turn away from the listener
        public Angle InnerConeAngle; // (degrees) within this cone the sound plays with a gain of 1.0.
        public Angle OuterConeAngle; // (degrees) outside the cone the sound plays with gain = OuterConeGain. (0 means the sound does not attenuate with direction.)
        public float OuterConeGain; // (dB) the gain to use when the sound is directed away from the listener
        public LocationOverrideFlagsValue LocationOverrideFlags;
        public Angle Azimuth;
        public float PositionalGain; // (dB)
        public float FirstPersonGain; // (dB)

        // NOTE: the following will only apply when the sound is started via script
        // azimuth values: 0 = front, 90 = left, 180 = back, 270 = right

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
        public enum LocationOverrideFlagsValue : int
        {
            None = 0,
            OverrideAzimuth = 1 << 0,
            Override3dGain = 1 << 1,
            OverrideSpeakerGain = 1 << 2,
        }
    }
}