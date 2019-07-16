using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Commands.Editing;
using TagTool.Common;

namespace TagTool.Cache
{
    public struct CacheAddress : IComparable<CacheAddress>, IEquatable<CacheAddress>, IBlamType
    {
        private const int TypeShift = 29;
        private const uint OffsetMask = (uint.MaxValue >> (32 - TypeShift));

		/// <summary>
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
		/// </summary>
		public uint Value { get; set; }

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
                var cacheAddressType = SetFieldCommand.ParseArgs(cacheContext, typeof(CacheAddressType), null, args.Take(1).ToList());
                if (!(cacheAddressType is CacheAddressType))
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
                    result = new CacheAddress((cacheAddressType as CacheAddressType?).Value, offset);
                    error = null;
                    return true;
                }
            }
        }
    }
}