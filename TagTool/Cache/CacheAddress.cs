using System;

namespace TagTool.Cache
{
    public struct CacheAddress : IComparable<CacheAddress>, IEquatable<CacheAddress>
    {
        private const int TypeShift = 29;
        private const uint OffsetMask = (uint.MaxValue >> (32 - TypeShift));

        public uint Value { get; }

        public CacheAddressType Type => (CacheAddressType)(Value >> TypeShift);
        
        public int Offset => (int)(Value & OffsetMask);

        public static explicit operator int(CacheAddress address) => address.Offset;

        public CacheAddress(uint value) { Value = value; }

        public CacheAddress(CacheAddressType type, int offset) : this((((uint)type) << TypeShift) | ((uint)offset & OffsetMask)) { }

        public bool Equals(CacheAddress other) => Value == other.Value;

        public override bool Equals(object obj) => obj is CacheAddress ? Equals((CacheAddress)obj) : false;

        public static bool operator ==(CacheAddress a, CacheAddress b) => a.Equals(b);

        public static bool operator !=(CacheAddress a, CacheAddress b) => !a.Equals(b);

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(CacheAddress other) => Value.CompareTo(other.Value);

        public override string ToString() => $"{{ Type: {Type}, Offset: {Offset} }}";
    }
}