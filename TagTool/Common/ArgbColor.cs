using System;

namespace TagTool.Common
{
    public struct ArgbColor : IEquatable<ArgbColor>, IBlamType
	{
        public uint Value;

        public byte Alpha
        {
            get => (byte)((Value >> 24) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[3] = value;
                Value = BitConverter.ToUInt32(allBytes, 0);
            }
        }

        public byte Red
        {
            get => (byte)((Value >> 16) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[2] = value;
                Value = BitConverter.ToUInt32(allBytes, 0);
            }
        }

        public byte Green
        {
            get => (byte)((Value >> 8) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[1] = value;
                Value = BitConverter.ToUInt32(allBytes, 0);
            }
        }

        public byte Blue
        {
            get => (byte)(Value & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[0] = value;
                Value = BitConverter.ToUInt32(allBytes, 0);
            }
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
            13 * 17 + Alpha.GetHashCode()
               * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode();

        public override string ToString() =>
            $"{{ Alpha: {Alpha}, Red: {Red}, Green: {Green}, Blue: {Blue} }}";
    }
}