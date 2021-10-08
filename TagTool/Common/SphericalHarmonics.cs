using System;
using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Common
{
    /// <summary>
    /// SH coefficients are defined using float arrays. The length depends on the order
    /// </summary>
    public static class SphericalHarmonics
    {
        public static readonly int Order0Count = 1;
        public static readonly int Order1Count = 4;
        public static readonly int Order2Count = 9;
        public static readonly int Order3Count = 16;
        public static readonly int Order4Count = 25;
        public static readonly int Order5Count = 36;

        public static int GetCoefficientCount(int order)
        {
            if (order == 0)
                return 1;
            else
                return order * order;
        }

        
    }

    /// <summary>
    /// Precomputed radiance transfer (PRT) types using spherical harmonics as basis function.
    /// </summary>
    public enum PrtSHType : byte
    {
        None,
        /// <summary>
        /// SH order 0, 1 coefficent per vertex
        /// </summary>
        Ambient,
        /// <summary>
        /// SH order 1, 4 coefficients per vertex
        /// </summary>
        Linear,
        /// <summary>
        /// SH order 2, 9 coefficients per vertex
        /// </summary>
        Quadratic
    }

    [TagStructure(Size = 0x48)]
    public class HalfRGBLightProbe : TagStructure
    {
        [TagField(Length = 3)]
        public short[] DominantLightDirection = new short[3];

        public short Padding1;

        [TagField(Length = 3)]
        public short[] DominantLightIntensity = new short[3];

        public short Padding2;

        [TagField(Length = 9)]
        public short[] SHRed = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHGreen = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHBlue = new short[SphericalHarmonics.Order2Count];

        public short Padding3;
    }

    [TagStructure(Size = 0x18)]
    public class LuminanceScale : TagStructure
    {
        public float Scale;
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
    }
}
