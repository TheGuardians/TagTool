using TagTool.Common;
using System;
using TagTool.Tags.Definitions;

namespace TagTool.Cache
{
    /// <summary>
    /// Base class for an object which converts stringID values to and from string list indices.
    /// </summary>
    public abstract class StringIdResolver
    {
        public int LengthBits;
        public int SetBits;
        public int IndexBits;

        /// <summary>
        /// Gets the index of the first string which belongs to a set.
        /// </summary>
        public abstract int GetMinSetStringIndex();

        /// <summary>
        /// Gets the index of the last string which belongs to a set.
        /// </summary>
        public abstract int GetMaxSetStringIndex();

        /// <summary>
        /// Gets the beginning offset for each set.
        /// </summary>
        public abstract int[] GetSetOffsets();

        public virtual int GetSet(StringId stringId)
        {
            var setMask = (0x1 << SetBits) - 1;
            return (int)((stringId.Value >> IndexBits) & setMask);
        }

        public virtual int GetIndex(StringId stringId)
        {
            var indexMask = (0x1 << IndexBits) - 1;
            return (int)((stringId.Value >> 0) & indexMask);
        }

        public virtual int GetLength(StringId stringId)
        {
            var lengthMask = (0x1 << LengthBits) - 1;
            return (int)((stringId.Value >> (IndexBits + SetBits)) & lengthMask);
        }

        public virtual StringId MakeStringId(int length, int set, int index)
        {
            var shiftedLength = (length & CreateMask(LengthBits)) << (IndexBits + SetBits);
            var shiftedSet = (set & CreateMask(SetBits)) << IndexBits;
            var shiftedIndex = index & CreateMask(IndexBits);
            return new StringId((uint)(shiftedLength | shiftedSet | shiftedIndex));
        }

        private StringId MakeStringId(int set, int index)
        {
            return MakeStringId(0, set, index);
        }

        private static uint CreateMask(int size)
        {
            return (1u << size) - 1;
        }

        /// <summary>
        /// Converts a stringID value to a string list index.
        /// </summary>
        /// <param name="stringId">The stringID.</param>
        /// <returns>The string list index, or -1 if none.</returns>
        public int StringIDToIndex(StringId stringId)
        {
            if (stringId.Value == uint.MaxValue)
                return 0;

            var setMin = GetMinSetStringIndex();
            var setMax = GetMaxSetStringIndex();
            var setOffsets = GetSetOffsets();

            var set = GetSet(stringId);
            var index = GetIndex(stringId);

            if (SetBits == 0 || (set == 0 && (index < setMin || index > setMax)))
            {
                // Value does not go into a set, so the index is the same as the ID
                return index;
            }

            if (set < 0 || setOffsets == null || set >= setOffsets.Length)
                throw new IndexOutOfRangeException($"string_id set {set}");

            // Convert the index part of the ID into a string index based on its set
            if (set == 0)
                index -= setMin;

            return index + setOffsets[set];
        }

        /// <summary>
        /// Converts a string list index to a stringID value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="version">The version of the stringID. Halo 3 By default.</param>
        /// <returns>The stringID value, or <see cref="StringId.Invalid"/> if none.</returns>
        public StringId IndexToStringID(int index, CacheVersion version = CacheVersion.Halo3Retail)
        {
            if (index < 0)
                return StringId.Invalid;

            var setMin = GetMinSetStringIndex();
            var setMax = GetMaxSetStringIndex();
            var setOffsets = GetSetOffsets();

            // If the value is outside of a set, just return it
            if (SetBits == 0 || (index < setMin || index > setMax))
                return MakeStringId(0, index);

            // Find the set which the index is closest to
            var set = 0;
            var minDistance = int.MaxValue;
            for (var i = 0; i < setOffsets.Length; i++)
            {
                if (index < setOffsets[i])
                    continue;
                var distance = index - setOffsets[i];
                if (distance >= minDistance)
                    continue;
                set = i;
                minDistance = distance;
            }

            // Compute the index within the set
            var idIndex = index - setOffsets[set];
            if (set == 0)
                idIndex += setMin;
            return MakeStringId(set, idIndex);
        }
    }
}
