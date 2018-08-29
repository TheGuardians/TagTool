using System;

namespace TagTool.Common
{
    public struct RealRgbColor : IEquatable<RealRgbColor>, IBlamType
	{
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealRgbColor(float red, float green, float blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(RealRgbColor other) =>
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue);

        public override bool Equals(object obj) =>
            obj is RealRgbColor ?
                Equals((RealRgbColor)obj) :
            false;

        public static bool operator ==(RealRgbColor a, RealRgbColor b) =>
            a.Equals(b);

        public static bool operator !=(RealRgbColor a, RealRgbColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode();

        public override string ToString() =>
            $"{{ Red: {Red}, Green: {Green}, Blue: {Blue} }}";
    }
}
