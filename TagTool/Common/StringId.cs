using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    /// <summary>
    /// A constant ID representing a debug string.
    /// </summary>
    public struct StringId : IComparable<StringId>, IBlamType
	{
        /// <summary>
        /// A null stringID.
        /// </summary>
        public static readonly StringId Invalid = new StringId(0);

        /// <summary>
        /// Gets the value of the stringID as a 32-bit integer.
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// Constructs a new StringID from a 32-bit value.
        /// </summary>
        /// <param name="value">The 32-bit value of the stringID.</param>
        public StringId(uint value)
        {
            Value = value;
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

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 1)
            {
                error = $"{args.Count} arguments supplied; should be 1";
                return false;
            }
            result = cache.StringTable.GetStringId(args[0]);
            error = null;
            return true;
        }

        public int CompareTo(StringId other)
        {
            return (int)Value - (int)other.Value;
        }
    }
}
