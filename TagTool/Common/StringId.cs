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

        private uint _value;

        private byte IndexBits;
        private byte SetBits;
        private byte LengthBits;

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
        /// Constructs a new StringID from a set and an index from specified version.
        /// </summary>
        /// <param name="set">The set the stringID belongs to.</param>
        /// <param name="index">The index of the stringID within the set.</param>
        /// <param name="version">The version of the cache the stringID is from.</param>
        public StringId(int set, int index, CacheVersion version)
            : this(0, set, index, version)
        {
        }

        /// <summary>
        /// Constructs a new StringID from a length, a set, and an index based on Halo 3 format.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="set">The set the stringID belongs to.</param>
        /// <param name="index">The index of the stringID within the set.</param>
        public StringId(int length, int set, int index)
        {
            IndexBits = 16;
            SetBits = 8;
            LengthBits = 8;

            var shiftedLength = ((length & 0xFF) << 24);
            var shiftedSet = ((set & 0xFF) << 16);
            var shiftedIndex = (index & 0xFFFF);
            _value = (uint)(shiftedLength | shiftedSet | shiftedIndex);
        }

        /// <summary>
        /// Constructs a new StringID from a length, a set, and an index from specified version.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="set">The set the stringID belongs to.</param>
        /// <param name="index">The index of the stringID within the set.</param>
        /// <param name="version">The version of the cache the stringID is from.</param>
        public StringId(int length, int set, int index, CacheVersion version)
        {
            switch (version)
            {
                case CacheVersion.Halo3ODST:
                case CacheVersion.Halo3Retail:
                case CacheVersion.HaloOnline106708:
                    IndexBits = 16;
                    SetBits = 8;
                    LengthBits = 8;
                    break;

                case CacheVersion.HaloReach:
                    IndexBits = 17;
                    SetBits = 8;
                    LengthBits = 7;
                    break;

                default:
                    IndexBits = 16;
                    SetBits = 8;
                    LengthBits = 8;
                    break;
            }
            var indexMask = (0x1 << IndexBits) - 1;
            var setMask = (0x1 << SetBits) - 1;
            var lengthMask = (0x1 << LengthBits) - 1;

            var shiftedLength = ((length & lengthMask) << (IndexBits+SetBits) );
            var shiftedSet = ((set & setMask) << IndexBits);
            var shiftedIndex = (index & indexMask);
            _value = (uint)(shiftedLength | shiftedSet | shiftedIndex);
        }

        /// <summary>
        /// Constructs a new StringID from a 32-bit value.
        /// </summary>
        /// <param name="value">The 32-bit value of the stringID.</param>
        public StringId(uint value)
        {
            IndexBits = 16;
            SetBits = 8;
            LengthBits = 8;
            _value = value;
        }

        /// <summary>
        /// Constructs a new StringID from a 32-bit value from specified version.
        /// </summary>
        /// <param name="value">The 32-bit value of the string id.</param>
        /// <param name="version">The cache version of the string id.</param>
        public StringId(uint value, CacheVersion version)
        {
            switch (version)
            {
                case CacheVersion.Halo3ODST:
                case CacheVersion.Halo3Retail:
                case CacheVersion.HaloOnline106708:
                    IndexBits = 16;
                    SetBits = 8;
                    LengthBits = 8;
                    break;

                case CacheVersion.HaloReach:
                    IndexBits = 17;
                    SetBits = 8;
                    LengthBits = 7;
                    break;

                default:
                    IndexBits = 16;
                    SetBits = 8;
                    LengthBits = 8;
                    break;
            }

            _value = value;
        }

		/// <summary>
		/// Gets the value of the stringID as a 32-bit integer.
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
		/// </summary>
		public uint Value
        {
            get { return _value; }
			set { _value = value; }
        }

        /// <summary>
        /// Gets the length component of the stringID.
        /// </summary>
        public int Length
        {
            get {
                var lengthMask = (0x1 << LengthBits) - 1;
                return (int)((_value >> (IndexBits + SetBits)) & lengthMask);
            }
        }

        /// <summary>
        /// Gets the set component of the stringID.
        /// </summary>
        public int Set
        {
            get {
                var setMask = (0x1 << SetBits) - 1;
                return (int)((_value >> IndexBits) & setMask);
            }
        }

        /// <summary>
        /// Gets the index component of the stringID.
        /// Note that this is not a direct index into the string list and must be translated first!
        /// </summary>
        public int Index
        {
            get {
                var indexMask = (0x1 << IndexBits) - 1;
                return (int)(_value & indexMask);
            }
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

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 1)
            {
                error = $"{args.Count} arguments supplied; should be 1";
                return false;
            }
            result = cacheContext.GetStringId(args[0]);
            error = null;
            return true;
        }

        public int CompareTo(StringId other)
        {
            return (int)Value - (int)other.Value;
        }
    }
}
