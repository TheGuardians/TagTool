using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct DatumIndex : IEquatable<DatumIndex>, IComparable<DatumIndex>, IBlamType
    {
        public static DatumIndex None { get; } = new DatumIndex(uint.MaxValue);

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

        public static bool operator ==(DatumIndex lhs, DatumIndex rhs) =>
            lhs.Equals(rhs);

        public static bool operator !=(DatumIndex lhs, DatumIndex rhs) =>
            !(lhs == rhs);

        public int CompareTo(DatumIndex other) =>
            Index.CompareTo(other.Index);

        public override int GetHashCode() =>
            Value.GetHashCode();

        public override string ToString() =>
            $"{{ Salt: 0x{Salt:X4}, Index: {Index} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;

            if (args.Count != 2)
            {
                error = $"{args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!ushort.TryParse(args[0], out ushort salt))
            {
                error = $"Unable to parse \"{args[0]}\" (salt) as `ushort`.";
                return false;
            }
            else if (!ushort.TryParse(args[1], out ushort index))
            {
                error = $"Unable to parse \"{args[1]}\" (index) as `ushort`.";
                return false;
            }
            else
            {
                result = new DatumIndex(salt, index);
                error = null;
                return true;
            }
        }
    }
}