using System;

namespace TagTool.Common
{
    public struct RealArgbColor : IEquatable<RealArgbColor>, IBlamType
	{
        public float Alpha { get; set; }
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealArgbColor(float alpha, float red, float green, float blue)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(RealArgbColor other) =>
            (Alpha == other.Alpha) &&
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue);

        public override bool Equals(object obj) =>
            obj is RealArgbColor ?
                Equals((RealArgbColor)obj) :
            false;

        public static bool operator ==(RealArgbColor a, RealArgbColor b) =>
            a.Equals(b);

        public static bool operator !=(RealArgbColor a, RealArgbColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Alpha.GetHashCode()
               * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode();

        public override string ToString() =>
            $"{{ Alpha: {Alpha}, Red: {Red}, Green: {Green}, Blue: {Blue} }}";
    }
}
