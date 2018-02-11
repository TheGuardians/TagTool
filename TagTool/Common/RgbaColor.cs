using System;

namespace TagTool.Common
{
    public struct RgbaColor : IEquatable<RgbaColor>
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        public RgbaColor(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public bool Equals(RgbaColor other) =>
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue) &&
            (Alpha == other.Alpha);

        public override bool Equals(object obj) =>
            obj is RgbaColor ?
                Equals((RgbaColor)obj) :
            false;

        public static bool operator ==(RgbaColor a, RgbaColor b) =>
            a.Equals(b);

        public static bool operator !=(RgbaColor a, RgbaColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode()
               * 17 + Alpha.GetHashCode();

        public override string ToString() =>
            $"{{ Red: {Red}, Green: {Green}, Blue: {Blue}, Alpha: {Alpha} }}";
    }
}
