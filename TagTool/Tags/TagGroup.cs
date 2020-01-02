using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;

namespace TagTool.Tags
{
    /// <summary>
    /// Describes the type of data in a tag.
    /// </summary>
    [TagStructure(Size = 0x10)]
    public class TagGroup : TagStructure, IEquatable<TagGroup>
    {
        /// <summary>
        /// Gets the group's tag. Can be -1.
        /// </summary>
        public Tag Tag;

        /// <summary>
        /// Gets the parent group's tag. Can be -1.
        /// </summary>
        public Tag ParentTag;

        /// <summary>
        /// Gets the grandparent group's tag. Can be -1.
        /// </summary>
        public Tag GrandparentTag;

        /// <summary>
        /// Gets the group's name stringID.
        /// </summary>
        public StringId Name;

        /// <summary>
        /// Represents a "null" tag group.
        /// </summary>
        public static readonly TagGroup None = new TagGroup(new Tag(-1), new Tag(-1), new Tag(-1), StringId.Invalid);

        /// <summary>
        /// Constructs an empty tag group description.
        /// </summary>
        public TagGroup()
            : this(Tag.Null, Tag.Null, Tag.Null, StringId.Invalid)
        {
        }

        /// <summary>
        /// Constructs a new tag group description.
        /// </summary>
        /// <param name="tag">The group's tag.</param>
        /// <param name="parentTag">The parent group's tag. Can be -1.</param>
        /// <param name="grandparentTag">The grandparent group's tag. Can be -1.</param>
        /// <param name="name">The group's name stringID.</param>
        public TagGroup(Tag tag, Tag parentTag, Tag grandparentTag, StringId name)
        {
            Tag = tag;
            ParentTag = parentTag;
            GrandparentTag = grandparentTag;
            Name = name;
        }

        /// <summary>
        /// Determines whether this group is a subgroup of another group.
        /// </summary>
        /// <param name="groups">The group to check.</param>
        /// <returns><c>true</c> if this group is a subgroup of the other group.</returns>
        public bool BelongsTo(params TagGroup[] groups)
        {
            return BelongsTo(groups.Select(group => group.Tag).ToArray());
        }

        /// <summary>
        /// Determines whether this group is a subgroup of another group.
        /// </summary>
        /// <param name="groupTags">The group tag to check, as a 4-character string.</param>
        /// <returns><c>true</c> if this group is a subgroup of the group tag.</returns>
        public bool BelongsTo(params string[] groupTags)
        {
            return BelongsTo(groupTags.Select(groupTag => new Tag(groupTag)).ToArray());
        }

        /// <summary>
        /// Determines whether this group is a subgroup of another group.
        /// </summary>
        /// <param name="groupTags">The group tag to check.</param>
        /// <returns><c>true</c> if this group is a subgroup of the group tag.</returns>
        public bool BelongsTo(params Tag[] groupTags)
        {
            foreach (var groupTag in groupTags)
                if (Tag.Equals(groupTag) || ParentTag.Equals(groupTag) || GrandparentTag.Equals(groupTag))
                    return true;

            return false;
        }

        public bool Equals(TagGroup other)
        {
            return Tag == other.Tag && ParentTag == other.ParentTag && GrandparentTag == other.GrandparentTag &&
                   Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is TagGroup && Equals((TagGroup)obj);
        }

        public static bool operator ==(TagGroup lhs, TagGroup rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TagGroup lhs, TagGroup rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            var result = 13;
            result = result * 17 + Tag.GetHashCode();
            result = result * 17 + ParentTag.GetHashCode();
            result = result * 17 + GrandparentTag.GetHashCode();
            result = result * 17 + Name.GetHashCode();
            return result;
        }

        public override string ToString() => Tag.ToString();
    }
}
