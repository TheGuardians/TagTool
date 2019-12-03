using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    public class CacheIndexItem
    {
        public readonly Tag GroupTag;

        public readonly string GroupName;

        public readonly Tag ParentGroupTag;

        public readonly Tag GrandparentGroupTag;

        public CacheIndexItem(int classIndex, int id, int offset, int index, TagClass tagClass, string groupName)
        {
            ClassIndex = classIndex;
            ID = id;
            Offset = offset;
            Index = index;

            GroupTag = (ClassIndex == -1 || tagClass.ClassCode == null) ? Tag.Null : new Tag(tagClass.ClassCode.ToCharArray());
            GroupName = groupName;
            ParentGroupTag = (ClassIndex == -1 || tagClass.Parent == null) ? Tag.Null : new Tag(tagClass.Parent.ToCharArray());
            GrandparentGroupTag = (ClassIndex == -1 || tagClass.Parent2 == null) ? Tag.Null : new Tag(tagClass.Parent2.ToCharArray());
        }

        /// <summary>
        /// Determines whether the tag belongs to a tag group.
        /// </summary>
        /// <param name="groupTag">The group tag.</param>
        /// <returns><c>true</c> if the tag belongs to the group.</returns>
        public bool IsInGroup(Tag groupTag)
        {
            return GroupTag == groupTag || ParentGroupTag == groupTag || GrandparentGroupTag == groupTag;
        }

        /// <summary>
        /// Determines whether the tag belongs to a tag group.
        /// </summary>
        /// <param name="groupTag">A 4-character string representing the group tag, e.g. "scnr".</param>
        /// <returns><c>true</c> if the tag belongs to the group.</returns>
        public bool IsInGroup(string groupTag) => IsInGroup(new Tag(groupTag));

        /// <summary>
        /// Determines whether the tag belongs to a tag group.
        /// </summary>
        /// <param name="group">The tag group.</param>
        /// <returns><c>true</c> if the tag belongs to the group.</returns>
        public bool IsInGroup(TagGroup group) => IsInGroup(group.Tag);

        public bool IsInGroup<T>() => IsInGroup(typeof(T).GetGroupTag());

        private string GetGen2GroupName(Tag groupTag)
        {
            if (!Tags.TagDefinition.Types.ContainsKey(groupTag))
            {
                Console.WriteLine($"WARNING: Tag definition not found for group tag '{groupTag}'");
                return "<unknown>";
            }

            var type = Tags.TagDefinition.Types[groupTag];
            var structure = TagStructure.GetTagStructureAttribute(type);

            return structure.Name;
        }

        public string Name;
        public int ID;
        public int Offset;
        public int ClassIndex;
        public int Size;
        public int Index;
        public int Magic;
        public bool External = false;

        public override string ToString()
        {
            return "[" + GroupTag + "] " + Name;
        }
    }

    public class CacheIndexTable : List<CacheIndexItem>
    {
        public List<TagClass> ClassList;

        public CacheIndexItem GetItemByID(int ID)
        {
            if (ID == -1)
                return null;
            return this[ID & 0xFFFF];
        }

        public CacheIndexItem this[Tag groupTag, string tagName]
        {
            get
            {
                foreach (var blamTag in this)
                {
                    if ((blamTag.GroupTag == groupTag.ToString()) && (blamTag.Name == tagName))
                    {
                        return blamTag;
                    }
                }

                throw new KeyNotFoundException($"[{groupTag}] {tagName}");
            }
        }
    }

    public class TagClass
    {
        public string ClassCode;
        public string Parent;
        public string Parent2;
        public int StringID;

        public override string ToString()
        {
            return ClassCode;
        }
    }
}
