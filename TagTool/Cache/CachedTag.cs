using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{

    public abstract class CachedTag
    {
        public string Name;
        public int Index;
        public uint ID;
        public TagGroup Group;
     
        /// <summary>
        /// A function to map cache addresses to file offsets
        /// </summary>
        public AddressToOffsetOverrideDelegate AddressToOffsetOverride;
        public delegate uint AddressToOffsetOverrideDelegate(uint currentOffset, uint address);

        public abstract uint DefinitionOffset { get; }

        public CachedTag()
        {
            Index = -1;
            Name = null;
            Group = new TagGroup();
        }

        public CachedTag(int index, string name = null) : this(index, new TagGroup(), name) { }

        public CachedTag(int index, TagGroup group, string name = null)
        {
            Index = index;
            Group = group;
            if (name != null)
                Name = name;
        }

        public override string ToString()
        {
            if (Name == null)
                return $"0x{Index.ToString("X8")}.{Group.ToString()}";
            else
                return $"{Name}.{Group.ToString()}";
        }

        public bool IsInGroup(params Tag[] groupTags)
        {
            return Group.BelongsTo(groupTags);
        }
    }

}
