using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Layouts
{
    /// <summary>
    /// Holds information about the layout of tag data.
    /// </summary>
    public class TagLayout
    {
        private readonly List<TagLayoutField> _fields = new List<TagLayoutField>();

        /// <summary>
        /// Creates a tag layout with a name and a size in bytes.
        /// It will not have a group tag associated with it.
        /// </summary>
        /// <param name="name">The name of the layout.</param>
        /// <param name="size">The size of the layout in bytes.</param>
        public TagLayout(string name, uint size)
            : this(name, size, new Tag(0))
        {
        }

        /// <summary>
        /// Creates a tag layout with a name, a size in bytes, and a group tag.
        /// </summary>
        /// <param name="name">The name of the layout.</param>
        /// <param name="size">The size of the layout.</param>
        /// <param name="groupTag">The group tag.</param>
        public TagLayout(string name, uint size, Tag groupTag)
        {
            Name = name;
            Size = size;
            GroupTag = groupTag;
            Fields = _fields.AsReadOnly();
        }

        /// <summary>
        /// Gets the name of the layout.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The size of the tag data structure in bytes.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// The layout's group tag (or 0 for none).
        /// </summary>
        public Tag GroupTag { get; set; }

        /// <summary>
        /// Gets a list of fields in the layout.
        /// </summary>
        public IReadOnlyList<TagLayoutField> Fields { get; private set; }

        /// <summary>
        /// Adds a field to the layout.
        /// </summary>
        /// <param name="field">The field to add.</param>
        public void Add(TagLayoutField field)
        {
            _fields.Add(field);
        }

        /// <summary>
        /// Adds a range of fields to the layout.
        /// </summary>
        /// <param name="fields">The fields to add.</param>
        public void AddRange(IEnumerable<TagLayoutField> fields)
        {
            foreach (var field in fields)
                Add(field);
        }

        /// <summary>
        /// Visits each field in the tag layout.
        /// </summary>
        /// <param name="visitor">The visitor to use.</param>
        public void Accept(ITagLayoutFieldVisitor visitor)
        {
            foreach (var field in Fields)
                field.Accept(visitor);
        }
    }
}
