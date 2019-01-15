using System;

namespace TagTool.Common
{
    public struct ArgbColor : IEquatable<ArgbColor>, IBlamType
    {
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(ArgbColor other) =>
            (Alpha == other.Alpha) &&
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue);

        public override bool Equals(object obj) =>
            obj is ArgbColor ?
                Equals((ArgbColor)obj) :
            false;

        public static bool operator ==(ArgbColor a, ArgbColor b) =>
            a.Equals(b);

        public static bool operator !=(ArgbColor a, ArgbColor b) =>
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