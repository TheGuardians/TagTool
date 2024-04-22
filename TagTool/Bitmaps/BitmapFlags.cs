using System;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// Bitmap flags.
    /// </summary>
    [Flags]
    public enum BitmapFlags : ushort
    {
        None = 0,

        PowerOfTwoDimensions = 1 << 0,

        /// <summary>
        /// The texture is in a "compressed" format (DXT, DXN, etc.).
        /// Its width and height must be rounded up to multiples of 4 on load.
        /// </summary>
        Compressed = 1 << 1,

        SwapAxes = 1 << 2,
        Unknown3 = 1 << 3,
        Unknown4 = 1 << 4,
        Unknown5 = 1 << 5,
        Unknown6 = 1 << 6,

        PreferLowDetail = 1 << 7,

        Unknown8 = 1 << 8,
        Unknown9 = 1 << 9,
        Unknown10 = 1 << 10,
        Unknown11 = 1 << 11,
        Unknown12 = 1 << 12,
        Unknown13 = 1 << 13,
        Unknown14 = 1 << 14,
        Unknown15 = 1 << 15
    }
}
