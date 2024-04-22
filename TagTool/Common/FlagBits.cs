using System;
using System.Collections.Generic;

namespace TagTool.Common
{
    public struct FlagBits<T> : IFlagBits where T : struct, Enum
    {
        private ulong _value;

        private FlagBits(ulong value)
        {
            _value = value;
        }

        public FlagBits(T bit)
        {
            _value = MakeMask(bit);
        }

        public FlagBits(params T[] bits)
        {
            _value = 0;
            foreach (var val in bits)
                _value |= MakeMask(val);
        }

        public FlagBits(IEnumerable<T> bits)
        {
            _value = 0;
            foreach (var val in bits)
                _value |= MakeMask(val);
        }

        public bool Test(T bit)
        {
            return (_value & MakeMask(bit)) != 0;
        }

        public bool Test(FlagBits<T> bits)
        {
            return (this & bits)._value != 0;
        }

        public bool Test(IEnumerable<T> bits)
        {
            foreach (var bit in bits)
            {
                if (!Test((T)bit))
                    return false;
            }
            return true;
        }

        public void Set(T bit, bool value)
        {
            if (value)
                _value |= MakeMask(bit);
            else
                _value &= ~MakeMask(bit);
        }

        public void Set(IEnumerable<T> bits, bool value)
        {
            foreach (var bit in bits)
                Set(bit, value);
        }

        public IEnumerable<T> GetBits()
        {
            foreach (T bit in Enum.GetValues(typeof(T)))
            {
                if (Test(bit))
                    yield return bit;
            }
        }


        public static FlagBits<T> operator |(FlagBits<T> lhs, FlagBits<T> rhs) => new FlagBits<T>(lhs._value | rhs._value);

        public static FlagBits<T> operator &(FlagBits<T> lhs, FlagBits<T> rhs) => new FlagBits<T>(lhs._value & rhs._value);

        public static FlagBits<T> operator ~(FlagBits<T> lhs) => new FlagBits<T>(~lhs._value);

        public static bool operator ==(FlagBits<T> lhs, FlagBits<T> rhs) => lhs._value == rhs._value;

        public static bool operator !=(FlagBits<T> lhs, FlagBits<T> rhs) => lhs._value != rhs._value;

        public override bool Equals(object obj) => obj is FlagBits<T> bits && _value == bits._value;

        public override int GetHashCode() => ((int)_value) ^ (int)(_value >> 32);

        public override string ToString() => $"{string.Join(", ", GetBits())}";


        private static ulong MakeMask(T bit)
        {
            int bitIndex = Convert.ToInt32(bit);
            if (bitIndex < 0 || bitIndex >= 64)
                throw new ArgumentOutOfRangeException(nameof(bit));

            return (1UL << (bitIndex & 63));
        }

        bool IFlagBits.TestBit(Enum bit) => Test((T)bit);
        void IFlagBits.SetBit(Enum bit, bool value) => Set((T)bit, value);
    }

    public interface IFlagBits
    {
        bool TestBit(Enum bit);
        void SetBit(Enum bit, bool value);
    }
}
