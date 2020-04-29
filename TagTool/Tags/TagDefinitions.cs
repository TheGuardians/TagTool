using System;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags
{
    public abstract class TagDefinitions
    {
        abstract public Dictionary<TagGroup, Type> Types { get;}

        public bool TagDefinitionExists(TagGroup group)
        {
            return Types.ContainsKey(group);
        }

        public bool TagDefinitionExists(Tag tag)
        {
            foreach (var group in Types.Keys)
                if (group.Tag == tag)
                    return true;
            return false;
        }

        public Type GetTagDefinitionType(TagGroup group)
        {
            if (Types.ContainsKey(group))
                return Types[group];
            else
                return null;
        }

        public Type GetTagDefinitionType(Tag tag)
        {
            foreach(var group in Types.Keys)
                if (group.Tag == tag)
                    return Types[group];
            return null;
        }

        public TagGroup GetTagDefinitionGroupTag(Type type)
        {
            foreach(var key in Types.Keys)
                if (Types[key] == type)
                    return key;
            return null;
        }

        public TagGroup GetTagGroupFromTag(Tag tag)
        {
            foreach (var group in Types.Keys)
                if (group.Tag == tag)
                    return group;
            return null;
        }
    }
}
