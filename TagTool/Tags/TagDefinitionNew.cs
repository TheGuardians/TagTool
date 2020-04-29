using System;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags
{
    public abstract class TagDefinitionsNew
    {
        abstract public Dictionary<TagGroupNew, Type> Types { get;}

        public bool TagDefinitionExists(TagGroupNew group)
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

        public Type GetTagDefinitionType(TagGroupNew group)
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

        public TagGroupNew GetTagDefinitionGroupTag(Type type)
        {
            foreach(var key in Types.Keys)
                if (Types[key] == type)
                    return key;
            return null;
        }

        public TagGroupNew GetTagGroupFromTag(Tag tag)
        {
            foreach (var group in Types.Keys)
                if (group.Tag == tag)
                    return group;
            return null;
        }
    }
}
