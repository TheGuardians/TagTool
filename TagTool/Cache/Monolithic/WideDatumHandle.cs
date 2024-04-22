using System;
using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public struct WideDatumHandle : IEquatable<WideDatumHandle>, IComparable<WideDatumHandle>
    {
        public static WideDatumHandle None = new WideDatumHandle(-1L);

        public DatumHandle PartitionIndex;
        public DatumHandle DatumIndex;

        public WideDatumHandle(DatumHandle partitionIndex, DatumHandle datumIndex)
        {
            PartitionIndex = partitionIndex;
            DatumIndex = datumIndex;
        }

        public WideDatumHandle(long value)
        {
            PartitionIndex = new DatumHandle((uint)((ulong)value >> 32));
            DatumIndex = new DatumHandle((uint)(ulong)value);
        }

        public static bool operator ==(WideDatumHandle a, WideDatumHandle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(WideDatumHandle a, WideDatumHandle b)
        {
            return !(a == b);
        }
        public int CompareTo(WideDatumHandle other)
        {
            int diff = PartitionIndex.CompareTo(other.PartitionIndex);
            if (diff == 0)
                diff = DatumIndex.CompareTo(other.DatumIndex);
            return diff;
        }

        public bool Equals(WideDatumHandle other)
        {
            return PartitionIndex == other.PartitionIndex && DatumIndex == other.DatumIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is WideDatumHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 17 * PartitionIndex.GetHashCode() * 31 + DatumIndex.GetHashCode();
            }
        }

        public override string ToString()
            => $"{{ Partition: {PartitionIndex}, Datum: {DatumIndex} }}";
    }
}
