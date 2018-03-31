using TagTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TagTool.Cache
{
    /// <summary>
    /// A file containing a cache of all of the stringID strings.
    /// </summary>
    public class StringIdCache
    {
        /// <summary>
        /// Gets the strings in the file.
        /// Note that strings can be <c>null</c>.
        /// </summary>
        public List<string> Strings { get; private set; }

        /// <summary>
        /// Gets the stringID resolver that the cache is using.
        /// </summary>
        public StringIdResolver Resolver { get; private set; }

        /// <summary>
        /// Loads a stringID cache from a string_ids.dat file.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="resolver">The stringID resolver to use.</param>
        public StringIdCache(Stream stream, StringIdResolver resolver)
        {
            Resolver = resolver;

            if (stream.Length != 0)
                Load(stream);
            else
                Strings = new List<string>();
        }

        /// <summary>
        /// Checks to see if a string is cached.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Contains(string str) => Strings.Contains(str);

        /// <summary>
        /// Adds a string to the cache.
        /// </summary>
        /// <param name="str">The string to add.</param>
        /// <returns>The stringID corresponding to the string that was added.</returns>
        public StringId AddString(string str, CacheVersion version = CacheVersion.Halo3Retail)
        {
            var strIndex = Strings.Count;
            Strings.Add(str);
            return GetStringId(strIndex, version);
        }

        /// <summary>
        /// Gets the string corresponding to a stringID.
        /// </summary>
        /// <param name="id">The stringID.</param>
        /// <returns>The string corresponding to the stringID, or <c>null</c> if not found.</returns>
        public string GetString(StringId id)
        {
            var strIndex = Resolver.StringIDToIndex(id);

            if (strIndex < 0 || strIndex >= Strings.Count)
                return null;

            return Strings[strIndex];
        }

        /// <summary>
        /// Gets the stringID corresponding to a string list index.
        /// </summary>
        /// <param name="index">The string list index to convert.</param>
        /// <returns>The corresponding stringID.</returns>
        public StringId GetStringId(int index)
        {
            if (index < 0 || index >= Strings.Count)
                return StringId.Invalid;

            return Resolver.IndexToStringID(index);
        }

        /// <summary>
        /// Gets the stringID corresponding to a string list index from the cache version.
        /// </summary>
        /// <param name="index">The string list index to convert.</param>
        /// <param name="version">The version of the returned StringID.</param>
        /// <returns>The corresponding stringID.</returns>
        public StringId GetStringId(int index, CacheVersion version)
        {
            if (index < 0 || index >= Strings.Count)
                return StringId.Invalid;

            return Resolver.IndexToStringID(index, version);
        }

        /// <summary>
        /// Gets the stringID corresponding to a string in the list.
        /// </summary>
        /// <param name="value">The string to search for.</param>
        /// <param name="version">The version of the returned StringID.</param>
        /// <returns>The corresponding stringID, or <see cref="StringId.Invalid"/> if not found.</returns>
        public StringId GetStringId(string value, CacheVersion version = CacheVersion.Halo3Retail)
        {
            return GetStringId(Strings.IndexOf(value), version);
        }

        /// <summary>
        /// Gets the <see cref="StringId"/> corresponding to a string in a specific set in the list.
        /// </summary>
        /// <param name="set">The set containing the string.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name = "version" > The version of the returned StringID.</param>
        /// <returns>The corresponding string_id, or <see cref="StringId.Invalid"/> if not found.</returns>
        public StringId GetStringId(int set, string value, CacheVersion version = CacheVersion.Halo3Retail)
        {
            var setOffsets = Resolver.GetSetOffsets();

            if (set < 0 || set >= setOffsets.Length)
                throw new IndexOutOfRangeException($"string_id set {set}");

            var setStrings = new Dictionary<string, StringId>();

            for (var i = 1; i < Strings.Count; i++)
            {
                var stringId = GetStringId(i, version);
                var stringValue = GetString(stringId);

                if (stringValue == null)
                    continue;

                if (set == stringId.Set)
                    setStrings[stringValue] = stringId;
            }

            if (setStrings.ContainsKey(value))
                return setStrings[value];

            return StringId.Invalid;
        }

        /// <summary>
        /// Saves the string data back to the file.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Save(Stream stream)
        {
            var writer = new BinaryWriter(stream);

            // Write the string count and then skip over the offset table, because it will be filled in last
            writer.Write(Strings.Count);
            writer.BaseStream.Position += 4 + Strings.Count * 4; // 4 byte data size + 4 bytes per string offset
            
            // Write string data and keep track of offsets
            var stringOffsets = new int[Strings.Count];
            var dataOffset = (int)writer.BaseStream.Position;
            var currentOffset = 0;
            for (var i = 0; i < Strings.Count; i++)
            {
                var str = Strings[i];
                if (str == null)
                {
                    // Null string - set offset to -1
                    stringOffsets[i] = -1;
                    continue;
                }

                // Write the string as null-terminated ASCII
                stringOffsets[i] = currentOffset;
                var data = Encoding.ASCII.GetBytes(str);
                writer.Write(data, 0, data.Length);
                writer.Write((byte)0);
                currentOffset += data.Length + 1;
            }

            // Now go back and write the string offsets
            writer.BaseStream.Position = 0x4;
            writer.Write(currentOffset); // Data size
            foreach (var offset in stringOffsets)
                writer.Write(offset);
            writer.BaseStream.SetLength(dataOffset + currentOffset);
        }

        /// <summary>
        /// Loads the cache from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        private void Load(Stream stream)
        {
            var reader = new BinaryReader(stream);

            // Read the header
            var stringCount = reader.ReadInt32();  // int32 string count
            var dataSize = reader.ReadInt32();     // int32 string data size

            // Read the string offsets into a list of (index, offset) pairs, and then sort by offset
            // This lets us know the length of each string without scanning for a null terminator
            var stringOffsets = new List<Tuple<int, int>>(stringCount);
            for (var i = 0; i < stringCount; i++)
            {
                var offset = reader.ReadInt32();
                if (offset >= 0 && offset < dataSize)
                    stringOffsets.Add(Tuple.Create(i, offset));
            }
            stringOffsets.Sort((x, y) => x.Item2 - y.Item2);

            // Seek to each offset and read each string
            var dataOffset = reader.BaseStream.Position;
            var strings = new string[stringCount];
            for (var i = 0; i < stringOffsets.Count; i++)
            {
                var index = stringOffsets[i].Item1;
                var offset = stringOffsets[i].Item2;
                var nextOffset = (i < stringOffsets.Count - 1) ? stringOffsets[i + 1].Item2 : dataSize;
                var length = Math.Max(0, nextOffset - offset - 1); // Subtract 1 for null terminator
                reader.BaseStream.Position = dataOffset + offset;
                strings[index] = Encoding.ASCII.GetString(reader.ReadBytes(length));
            }
            Strings = strings.ToList();
        }
    }
}
