using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TagTool.Tags.Definitions
{
    /// <summary>
    /// Contains a list of localized strings.
    /// </summary>
    [TagStructure(Name = "multilingual_unicode_string_list", Tag = "unic", Size = 0x50)]
    public class MultilingualUnicodeStringList
    {
        /// <summary>
        /// The strings in the list.
        /// </summary>
        public List<LocalizedString> Strings;

        /// <summary>
        /// The data block containing every string.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Array of ushorts, comes in pair, in each block of 2 is made of an index and string count. Only used by H3 and ODST
        /// </summary>
        [TagField(Length = 24)]
        public ushort[] OffsetCounts;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        /// <summary>
        /// Gets the value of a string in a given language.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="language">The language.</param>
        /// <returns>The value of the string, or <c>null</c> if the string is not available.</returns>
        public string GetString(LocalizedString str, GameLanguage language)
        {
            var offset = str.Offsets[(int)language];
            if (offset < 0 || offset >= Data.Length)
                return null; // String not available

            var length = GetStringLength(offset);
            return Encoding.UTF8.GetString(Data, offset, length);
        }

        /// <summary>
        /// Sets the value of a string for a given language.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="language">The language.</param>
        /// <param name="newValue">The new value. Can be <c>null</c>.</param>
        public void SetString(LocalizedString str, GameLanguage language, string newValue)
        {
            // Replace the string
            var offset = str.Offsets[(int)language];
            if (offset < 0 || offset >= Data.Length)
                offset = Data.Length; // Add the string at the end
            var oldLength = GetStringLength(offset);
            var bytes = (newValue != null) ? Encoding.UTF8.GetBytes(newValue) : new byte[0];
            if (bytes.Length > 0 && offset == Data.Length)
            {
                // If it's a new string, null-terminate it
                var nullTerminated = new byte[bytes.Length + 1];
                Buffer.BlockCopy(bytes, 0, nullTerminated, 0, bytes.Length);
                bytes = nullTerminated;
            }
            Data = ArrayUtil.Replace(Data, offset, oldLength, bytes);
            
            // Update string offsets
            str.Offsets[(int)language] = (bytes.Length > 0) ? offset : -1;
            var sizeDelta = bytes.Length - oldLength;
            foreach (var adjustStr in Strings)
            {
                for (var i = 0; i < adjustStr.Offsets.Length; i++)
                {
                    if (adjustStr.Offsets[i] > offset)
                        adjustStr.Offsets[i] += sizeDelta;
                }
            }
        }

        private int GetStringLength(int offset)
        {
            var endOffset = offset;
            while (endOffset < Data.Length && Data[endOffset] != 0)
                endOffset++;
            return endOffset - offset;
        }
    }

    /// <summary>
    /// A localized string.
    /// </summary>
    [TagStructure(Size = 0x54)]
    public class LocalizedString
    {
        /// <summary>
        /// The string's stringID.
        /// </summary>
        public StringId StringID;

        /// <summary>
        /// The stringID's string value. Can be empty.
        /// </summary>
        [TagField(Length = 0x20)]
        public string StringIDStr;

        /// <summary>
        /// The array of offsets for each language.
        /// If an offset is -1, then the string is not available.
        /// There must be 12 of these (one offset per language).
        /// </summary>
        [TagField(Length = 12)]
        public int[] Offsets;

        public LocalizedString()
        {
            StringID = StringId.Invalid;
            StringIDStr = "";
            Offsets = new int[12];
            for (var i = 0; i < Offsets.Length; i++)
                Offsets[i] = -1;
        }
    }
}