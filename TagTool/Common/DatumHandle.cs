using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct DatumHandle : IEquatable<DatumHandle>, IComparable<DatumHandle>, IBlamType
    {
        public static DatumHandle None { get; } = new DatumHandle(uint.MaxValue);

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

        public DatumHandle(uint value)
        {
            Value = value;
        }

        public DatumHandle(ushort salt, ushort index) :
            this(0)
        {
            Salt = salt;
            Index = index;
        }

        public bool Equals(DatumHandle other) =>
            Value.Equals(other.Value);

        public override bool Equals(object obj) =>
            obj is DatumHandle datumIndex ?
                Equals(datumIndex) :
                false;

        public static bool operator ==(DatumHandle lhs, DatumHandle rhs) =>
            lhs.Equals(rhs);

        public static bool operator !=(DatumHandle lhs, DatumHandle rhs) =>
            !(lhs == rhs);

        public int CompareTo(DatumHandle other) =>
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
                error = $"ERROR: {args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!ushort.TryParse(args[0], out ushort salt))
            {
                error = $"ERROR: Unable to parse \"{args[0]}\" (salt) as `ushort`.";
                return false;
            }
            else if (!ushort.TryParse(args[1], out ushort index))
            {
                error = $"ERROR: Unable to parse \"{args[1]}\" (index) as `ushort`.";
                return false;
            }
            else
            {
                result = new DatumHandle(salt, index);
                error = null;
                return true;
            }
        }
    }
}