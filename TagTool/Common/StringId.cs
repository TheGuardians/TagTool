using System;

namespace TagTool.Common
{
    /// <summary>
    /// A constant ID representing a debug string.
    /// </summary>
    public struct StringId : IComparable<StringId>
    {
        /// <summary>
        /// A null stringID.
        /// </summary>
        public static readonly StringId Invalid = new StringId(0);

        private readonly uint _value;

        /// <summary>
        /// Constructs a new StringID from a set and an index.
        /// </summary>
        /// <param name="set">The set the stringID belongs to.</param>
        /// <param name="index">The index of the stringID within the set.</param>
        public StringId(int set, int index)
            : this(0, set, index)
        {
        }

        /// <summary>
        /// Constructs a new StringID from a length, a set, and an index.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="set">The set the stringID belongs to.</param>
        /// <param name="index">The index of the stringID within the set.</param>
        public StringId(int length, int set, int index)
        {
            var shiftedLength = ((length & 0xFF) << 24);
            var shiftedSet = ((set & 0xFF) << 16);
            var shiftedIndex = (index & 0xFFFF);
            _value = (uint)(shiftedLength | shiftedSet | shiftedIndex);
        }

        /// <summary>
        /// Constructs a new StringID from a 32-bit value.
        /// </summary>
        /// <param name="value">The 32-bit value of the stringID.</param>
        public StringId(uint value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value of the stringID as a 32-bit integer.
        /// </summary>
        public uint Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the length component of the stringID.
        /// </summary>
        public int Length
        {
            get { return (int)((_value >> 24) & 0xFF); }
        }

        /// <summary>
        /// Gets the set component of the stringID.
        /// </summary>
        public int Set
        {
            get { return (int)((_value >> 16) & 0xFF); }
        }

        /// <summary>
        /// Gets the index component of the stringID.
        /// Note that this is not a direct index into the string list and must be translated first!
        /// </summary>
        public int Index
        {
            get { return (int)(_value & 0xFFFF); }
        }

        public override bool Equals(object obj)
        {
            return (obj is StringId) && (this == (StringId)obj);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }

        public static bool operator==(StringId x, StringId y)
        {
            return (x.Value == y.Value);
        }

        public static bool operator!=(StringId x, StringId y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return "0x" + Value.ToString("X8");
        }

        public int CompareTo(StringId other)
        {
            return (int)Value - (int)other.Value;
        }
    }
}
