using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct ArgbColor : IEquatable<ArgbColor>
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

    }
}