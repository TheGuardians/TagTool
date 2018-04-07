using System;
using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x44)]
    public class PlaybackParameter
    {
        public FieldDisableFlagsValue FieldDisableFlags;
        public float DistanceA;
        public float DistanceB;
        public float DistanceC;
        public float DistanceD;
        public float SkipFraction;
        public float MaximumBendPerSecond;
        public float GainBase;
        public float GainVariance;
        public Bounds<short> RandomPitchBounds;
        public Bounds<Angle> ConeAngleBounds;
        public float OuterConeGain;
        public FlagsValue Flags;
        public Angle Azimuth;
        public float PositionalGain;
        public float FirstPersonGain;

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