using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealRgbaColor : IEquatable<RealRgbaColor>, IBlamType
    {
        public float Alpha { get; set; }
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }

        public RealRgbaColor(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public bool Equals(RealRgbaColor other) =>
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue) &&
            (Alpha == other.Alpha);

        public override bool Equals(object obj) =>
            obj is RealRgbaColor ?
                Equals((RealRgbaColor)obj) :
            false;

        public static bool operator ==(RealRgbaColor a, RealRgbaColor b) =>
            a.Equals(b);

        public static bool operator !=(RealRgbaColor a, RealRgbaColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode()
               * 17 + Alpha.GetHashCode();

        public override string ToString() =>
            $"{{ Red: {Red}, Green: {Green}, Blue: {Blue}, Alpha: {Alpha} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"{args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!float.TryParse(args[0], out float r))
            {
                error = $"Unable to parse \"{args[0]}\" (r) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float g))
            {
                error = $"Unable to parse \"{args[1]}\" (g) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float b))
            {
                error = $"Unable to parse \"{args[2]}\" (b) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[3], out float a))
            {
                error = $"Unable to parse \"{args[3]}\" (a) as `float`.";
                return false;
            }
            else
            {
                result = new RealRgbaColor(r, g, b, a);
                error = null;
                return true;
            }
        }
    }
}
