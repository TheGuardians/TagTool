using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TagTool.Common
{
    /// <summary>
    /// A lookup table which can be used to find a tag's equivalents in different game versions.
    /// </summary>
    public class TagCacheMap
    {
        private readonly Dictionary<string, VersionMap> _versionMaps = new Dictionary<string, VersionMap>();
        private int _nextGlobalTagIndex = 0;

        /// <summary>
        /// Connects a tag index to a tag in another version.
        /// </summary>
        /// <param name="version1">The first version.</param>
        /// <param name="index1">The tag index in the first version.</param>
        /// <param name="version2">The second version.</param>
        /// <param name="index2">The tag index in the second version.</param>
        public void Add(string version1, int index1, string version2, int index2)
        {
            if (!_versionMaps.TryGetValue(version1, out VersionMap map1))
            {
                map1 = new VersionMap();
                _versionMaps[version1] = map1;
            }
            if (!_versionMaps.TryGetValue(version2, out VersionMap map2))
            {
                map2 = new VersionMap();
                _versionMaps[version2] = map2;
            }

            // Check if the first index is in the map for the first version.
            // If it is, then we'll get a "global index" which can be used to look it up in other versions.
            // If it isn't, then we need to make a new global index for it.
            var globalIndex = map1.GetGlobalTagIndex(index1);
            if (globalIndex < 0)
            {
                globalIndex = _nextGlobalTagIndex;
                _nextGlobalTagIndex++;
                map1.Add(globalIndex, index1);
            }

            // Connect the global index to the second index in the second version
            map2.Add(globalIndex, index2);
        }

        /// <summary>
        /// Translates a tag index between two versions.
        /// </summary>
        /// <param name="version1">The version of the index to translate.</param>
        /// <param name="index1">The tag index.</param>
        /// <param name="version2">The version to get the equivalent tag index in.</param>
        /// <returns>The equivalent tag index if found, or -1 otherwise.</returns>
        public int Translate(string version1, int index1, string version2)
        {
            if (!_versionMaps.TryGetValue(version1, out VersionMap map1))
                return -1;
            if (!_versionMaps.TryGetValue(version2, out VersionMap map2))
                return -1;

            // Get the global index from the first map, then look up that index in the second one
            var globalIndex = map1.GetGlobalTagIndex(index1);
            if (globalIndex < 0)
                return -1;
            return map2.GetVersionedTagIndex(globalIndex);
        }

        /// <summary>
        /// Writes the map out to a CSV.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteCsv(TextWriter writer)
        {
            // Write a list of versions being represented
            writer.WriteLine(string.Join(",", _versionMaps.Keys));
            
            // Now write out each tag
            for (var i = 0; i < _nextGlobalTagIndex; i++)
            {
                var globalIndex = i;
                writer.WriteLine(string.Join(",", _versionMaps.Keys.Select(v => _versionMaps[v].GetVersionedTagIndex(globalIndex).ToString("X4"))));
            }
        }

        /// <summary>
        /// Parses a map from a CSV.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The map that was read.</returns>
        public static TagCacheMap ParseCsv(TextReader reader)
        {
            var result = new TagCacheMap();
            
            // Read the timestamp list and resolve each one
            var cacheFileLine = reader.ReadLine();
            if (cacheFileLine == null)
                return result;

            var cacheFileNames = cacheFileLine.Split(',');

            // Read each line and store the tag indices in the result map
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Parse each tag index as a hex number
                var tags = line.Split(',');
                var tagIndices = tags.Select(t =>
                {
                    if (int.TryParse(t, NumberStyles.HexNumber, null, out int r))
                        return r;
                    return -1;
                }).ToArray();

                // Now connect all of them to the first tag
                for (var i = 1; i < tagIndices.Length; i++)
                {
                    if (tagIndices[i] >= 0)
                        result.Add(cacheFileNames[0], tagIndices[0], cacheFileNames[i], tagIndices[i]);
                }
            }
            return result;
        }

        private class VersionMap
        {
            private readonly List<int> _localIndices = new List<int>();
            private readonly Dictionary<int, int> _globalIndices = new Dictionary<int, int>();

            /// <summary>
            /// Converts a tag to a global tag index which can be used to look up the tag in another version.
            /// </summary>
            /// <param name="tag">The index of the tag for this version.</param>
            /// <returns>An index which can be passed to <see cref="GetVersionedTagIndex"/> for any version, or -1 if the tag was not found.</returns>
            public int GetGlobalTagIndex(int tag)
            {
                if (_globalIndices.TryGetValue(tag, out int result))
                    return result;
                return -1;
            }

            /// <summary>
            /// Converts a global tag index to a tag index for this version.
            /// </summary>
            /// <param name="globalIndex">The global tag index returned by <see cref="GetGlobalTagIndex"/> for this version.</param>
            /// <returns>The tag's index in this version, or -1 if not found.</returns>
            public int GetVersionedTagIndex(int globalIndex)
            {
                if (globalIndex < 0 || globalIndex >= _localIndices.Count)
                    return -1;
                return _localIndices[globalIndex];
            }

            /// <summary>
            /// Adds a mapping between a global tag index and a versioned tag index.
            /// </summary>
            /// <param name="globalIndex">The global tag index.</param>
            /// <param name="versionedIndex">The tag's index in this version.</param>
            public void Add(int globalIndex, int versionedIndex)
            {
                _globalIndices[versionedIndex] = globalIndex;
                while (globalIndex >= _localIndices.Count)
                    _localIndices.Add(-1);
                _localIndices[globalIndex] = versionedIndex;
            }
        }
    }
}
