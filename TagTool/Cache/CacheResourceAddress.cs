using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Commands.Editing;
using TagTool.Common;

namespace TagTool.Cache
{
    public struct CacheResourceAddress : IComparable<CacheResourceAddress>, IEquatable<CacheResourceAddress>, IBlamType
    {
        private const int TypeShift = 29;
        private const uint OffsetMask = (uint.MaxValue >> (32 - TypeShift));

		/// <summary>
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
		/// </summary>
		public uint Value { get; set; }

        public CacheResourceAddressType Type => (CacheResourceAddressType)(Value >> TypeShift);
        
        public int Offset => (int)(Value & OffsetMask);

        public static explicit operator int(CacheResourceAddress address) => address.Offset;

        public CacheResourceAddress(uint value) { Value = value; }

        public CacheResourceAddress(CacheResourceAddressType type, int offset) : this((((uint)type) << TypeShift) | ((uint)offset & OffsetMask)) { }

        public bool Equals(CacheResourceAddress other) => Value == other.Value;

        public override bool Equals(object obj) => obj is CacheResourceAddress ? Equals((CacheResourceAddress)obj) : false;

        public static bool operator ==(CacheResourceAddress a, CacheResourceAddress b) => a.Equals(b);

        public static bool operator !=(CacheResourceAddress a, CacheResourceAddress b) => !a.Equals(b);

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(CacheResourceAddress other) => Value.CompareTo(other.Value);

        public override string ToString() => $"{{ Type: {Type}, Offset: {Offset} }}";

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            
            if (args.Count != 2)
            {
                error = $"{args.Count} arguments supplied; should be 2";
                return false;
            }
            else
            {
                var cacheAddressType = SetFieldCommand.ParseArgs(cacheContext, typeof(CacheResourceAddressType), null, args.Take(1).ToList());
                if (!(cacheAddressType is CacheResourceAddressType))
                {
                    error = $"Failed to parse `{args[0]}` as `CacheAddressType`";
                    return false;
                }
                else if (!int.TryParse(args[1], out int offset))
                {
                    error = $"Failed to parse `{args[1]}` as `int` (offset).";
                    return false;
                }
                else
                {
                    result = new CacheResourceAddress((cacheAddressType as CacheResourceAddressType?).Value, offset);
                    error = null;
                    return true;
                }
            }
        }
    }
}