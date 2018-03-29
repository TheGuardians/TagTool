using TagTool.Common;
using System;

namespace TagTool.Cache
{
    /// <summary>
    /// Base class for an object which converts stringID values to and from string list indices.
    /// </summary>
    public abstract class StringIdResolver
    {
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

        /// <summary>
        /// Converts a stringID value to a string list index.
        /// </summary>
        /// <param name="stringId">The stringID.</param>
        /// <returns>The string list index, or -1 if none.</returns>
        public int StringIDToIndex(StringId stringId)
        {
            var setMin = GetMinSetStringIndex();
            var setMax = GetMaxSetStringIndex();
            var setOffsets = GetSetOffsets();

            var set = stringId.Set;
            var index = stringId.Index;

            if (set == 0 && (index < setMin || index > setMax))
            {
                // Value does not go into a set, so the index is the same as the ID
                return index;
            }

            if (set < 0 || set >= setOffsets.Length)
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
        /// <returns>The stringID value, or <see cref="StringId.Invalid"/> if none.</returns>
        public StringId IndexToStringID(int index)
        {
            if (index < 0)
                return StringId.Invalid;

            var setMin = GetMinSetStringIndex();
            var setMax = GetMaxSetStringIndex();
            var setOffsets = GetSetOffsets();

            // If the value is outside of a set, just return it
            if (index < setMin || index > setMax)
                return new StringId(0, index);

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
            return new StringId(set, idIndex);
        }
    }
}
