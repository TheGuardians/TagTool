using System;

namespace TagTool.Common
{
    public struct RgbColor : IEquatable<RgbColor>
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public RgbColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(RgbColor other) =>
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue);

        public override bool Equals(object obj) =>
            obj is RgbColor ?
                Equals((RgbColor)obj) :
            false;

        public static bool operator ==(RgbColor a, RgbColor b) =>
            a.Equals(b);

        public static bool operator !=(RgbColor a, RgbColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode();

        public override string ToString() =>
            $"{{ Red: {Red}, Green: {Green}, Blue: {Blue} }}";
    }
}
