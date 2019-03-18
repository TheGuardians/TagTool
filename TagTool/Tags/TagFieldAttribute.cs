using System;
using System.Runtime.InteropServices;
using TagTool.Cache;

namespace TagTool.Tags
{
    /// <summary>
    /// Attribute for automatically-serializable values in a tag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TagFieldAttribute : Attribute
    {
		public static readonly TagFieldAttribute Default = new TagFieldAttribute();

        /// <summary>
        /// The flags of the field.
        /// </summary>
        public TagFieldFlags Flags { get; set; } = TagFieldFlags.None;

        /// <summary>
        /// The minimum cache version the tag field is present in.
        /// </summary>
        public CacheVersion MinVersion { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The maximum cache version the tag field is present in.
        /// </summary>
        public CacheVersion MaxVersion { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The tag field version.
        /// </summary>
        public CacheVersion Version { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The name of the field to upgrade to (if any).
        /// </summary>
        public string Upgrade { get; set; } = string.Empty;

        /// <summary>
        /// The name of the field to downgrade to (if any).
        /// </summary>
        public string Downgrade { get; set; } = string.Empty;

        /// <summary>
        /// If the value is an inline array, determines the number of elements in the array.
        /// If the value is a string, determines the maximum number of characters in the string (including the null terminator).
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The power of two to align the field's data to.
        /// Only applicable to fields which contain pointers.
        /// </summary>
        public uint Align { get; set; } = 0;

        /// <summary>
        /// If the field is a string, determines the character set of the string.
        /// </summary>
        public CharSet CharSet { get; set; } = CharSet.Ansi;

        /// <summary>
        /// If the field is a tag reference, an array of valid group tags, or any if null.
        /// </summary>
        public string[] ValidTags { get; set; } = null;

        /// <summary>
        /// A data-orientage description of the usage format of the field.
        /// (i.e., world units, [0,1], degrees, etc...)
        /// </summary>
        public string Format { get; set; } = "";
    }

    [Flags]
    public enum TagFieldFlags : int
    {
        None,
        Label = 1 << 0,
        Short = 1 << 1,
        Padding = 1 << 2,
        Pointer = 1 << 3,
        Runtime = 1 << 4,
        Fraction = 1 << 5,
        Resource = 1 << 6,
    }
}