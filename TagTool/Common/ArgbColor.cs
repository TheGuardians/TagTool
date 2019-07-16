using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct ArgbColor : IEquatable<ArgbColor>, IBlamType
    {
        private int Value;

        public byte Alpha
        {
            get => (byte)((Value >> 24) & byte.MaxValue);
            set => Value = (Value & ~(byte.MaxValue << 24)) | (value << 24);
        }

        public byte Red
        {
            get => (byte)((Value >> 16) & byte.MaxValue);
            set => Value = (Value & ~(byte.MaxValue << 16)) | (value << 16);
        }

        public byte Green
        {
            get => (byte)((Value >> 8) & byte.MaxValue);
            set => Value = (Value & ~(byte.MaxValue << 8)) | (value << 8);
        }

        public byte Blue
        {
            get => (byte)(Value & byte.MaxValue);
            set => Value = (Value & ~byte.MaxValue) | value;
        }

        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            Value = 0;
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
            Value.GetHashCode();

        public override string ToString() =>
            $"{{ Alpha: {Alpha}, Red: {Red}, Green: {Green}, Blue: {Blue} }}";

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"{args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!byte.TryParse(args[0], out byte a))
            {
                error = $"Unable to parse \"{args[0]}\" (a) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[1], out byte r))
            {
                error = $"Unable to parse \"{args[1]}\" (r) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[2], out byte g))
            {
                error = $"Unable to parse \"{args[2]}\" (g) as `byte`.";
                return false;
            }
            else if (!byte.TryParse(args[3], out byte b))
            {
                error = $"Unable to parse \"{args[3]}\" (b) as `byte`.";
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