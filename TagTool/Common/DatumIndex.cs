using System;

namespace TagTool.Common
{
    public struct DatumIndex : IEquatable<DatumIndex>, IComparable<DatumIndex>, IBlamType
    {
        public uint Value;

        public ushort Salt
        {
            get => (ushort)((Value >> 16) & ushort.MaxValue);
            set
            {
                Value &= ~(ushort.MaxValue << 16);
                Value |= (uint)(value << 16);
            }
        }

        public ushort Index
        {
            get => (ushort)(Value & ushort.MaxValue);
            set
            {
                Value &= ~(uint)ushort.MaxValue;
                Value |= value;
            }
        }

        public DatumIndex(uint value)
        {
            Value = value;
        }

        public DatumIndex(ushort salt, ushort index) :
            this(0)
        {
            Salt = salt;
            Index = index;
        }

        public bool Equals(DatumIndex other) =>
            Value.Equals(other.Value);

        public override bool Equals(object obj) =>
            obj is DatumIndex datumIndex ?
                Equals(datumIndex) :
                false;

        public int CompareTo(DatumIndex other) =>
            Index.CompareTo(other.Index);

        public override int GetHashCode() =>
            Value.GetHashCode();

        public override string ToString() =>
            $"{{ Salt: 0x{Salt:X4}, Index: {Index} }}";
    }
}