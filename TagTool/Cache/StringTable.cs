using System;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Cache
{
    public abstract class StringTable : List<string>
    {
        public CacheVersion Version;
        public StringIdResolver Resolver;

        public abstract StringId AddString(string newString);

        // override if required
        public virtual string GetString(StringId id)
        {
            var index = Resolver.StringIDToIndex(id);
            if (index > 0 && index < Count)
                return this[index];
            else
                return "invalid";
        }

        public virtual StringId GetStringId(string str)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == str)
                {
                    return Resolver.IndexToStringID(i, Version);
                }
            }
            return StringId.Invalid;
        }

        public virtual StringId GetStringId(int index)
        {
            if (index < 0 || index >= this.Count)
                return StringId.Invalid;

            return Resolver.IndexToStringID(index);
        }
    }

}
