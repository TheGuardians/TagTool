using System;
using System.Collections.Generic;
using TagTool.Cache;

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

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 3)
            {
                error = $"ERROR: {args.Count} arguments supplied; should be 3";
                return false;
            }
            else if (!float.TryParse(args[0], out float r))
            {
                error = $"ERROR: Unable to parse \"{args[0]}\" (r) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float g))
            {
                error = $"ERROR: Unable to parse \"{args[1]}\" (g) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float b))
            {
                error = $"ERROR: Unable to parse \"{args[2]}\" (b) as `float`.";
                return false;
            }
            else
            {
                result = new RealRgbColor(r, g, b);
                error = null;
                return true;
            }
        }
    }
}
