using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct ArgbColor : IEquatable<ArgbColor>, IBlamType
    {
        public byte Alpha;
        public byte Red;
        public byte Green;
        public byte Blue;

        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public ArgbColor(uint value)
        {
            Alpha = (byte)((value >> 24) & 0xFF);
            Red = (byte)((value >> 16) & 0xFF);
            Green = (byte)((value >> 8) & 0xFF);
            Blue = (byte)((value >> 0) & 0xFF);
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
            GetValue().GetHashCode();

        public override string ToString() =>
            $"{{ Alpha: {Alpha}, Red: {Red}, Green: {Green}, Blue: {Blue} }}";

        public uint GetValue()
        {
            return (uint)((Alpha << 24) + (Red << 16) + (Green << 8) + Blue);
        }

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"ERROR: {args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!byte.TryParse(args[0], out byte a))
            {
                error = $"ERROR: Unable to parse \"{args[0]}\" (a) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[1], out byte r))
            {
                error = $"ERROR: Unable to parse \"{args[1]}\" (r) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[2], out byte g))
            {
                error = $"ERROR: Unable to parse \"{args[2]}\" (g) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[3], out byte b))
            {
                error = $"ERROR: Unable to parse \"{args[3]}\" (b) as `byte`.";
                return false;
            }
            else
            {
                result = new ArgbColor(a, r, g, b);
                error = null;
                return true;
            }

        }
    }
}