using System;

namespace TagTool.Common
{
    public struct RgbColor : IEquatable<RgbColor>
    {
        public uint Value;
        
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

        public RgbColor(byte red, byte green, byte blue)
        {
            Value = 0;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(RgbColor other) =>
            (Red == other.Red) &&
            (Green == other.Green) &&
            (Blue == other.Blue);

        public override bool Equals(object obj) =>
            obj is RgbColor ?
                Equals((RgbColor)obj) :
            false;

        public static bool operator ==(RgbColor a, RgbColor b) =>
            a.Equals(b);

        public static bool operator !=(RgbColor a, RgbColor b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Red.GetHashCode()
               * 17 + Green.GetHashCode()
               * 17 + Blue.GetHashCode();

        public override string ToString() =>
            $"{{ Red: {Red}, Green: {Green}, Blue: {Blue} }}";
    }
}