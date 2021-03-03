using System;
using System.Collections.Generic;
using TagTool.Cache;

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

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"{args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!float.TryParse(args[0], out float a))
            {
                error = $"Unable to parse \"{args[0]}\" (a) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float r))
            {
                error = $"Unable to parse \"{args[1]}\" (r) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float g))
            {
                error = $"Unable to parse \"{args[2]}\" (g) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[3], out float b))
            {
                error = $"Unable to parse \"{args[3]}\" (b) as `float`.";
                return false;
            }
            else
            {
                result = new RealArgbColor(a, r, g, b);
                error = null;
                return true;
            }
        }
    }
}
