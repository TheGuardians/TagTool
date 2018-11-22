using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace System
{
    public static class TypeExtensions
    {
        private static Dictionary<string, Tag> TypeGroupTags { get; } = new Dictionary<string, Tag>();

        public static Tag GetGroupTag(this Type type)
        {
            if (TypeGroupTags.ContainsKey(type.FullName))
                return TypeGroupTags[type.FullName];

            var attr = type.GetCustomAttributes(typeof(TagStructureAttribute), false);

            if (attr.Length < 1)
                throw new NotSupportedException(type.FullName);

            return TypeGroupTags[type.FullName] = new Tag((attr[0] as TagStructureAttribute).Tag);
        }
    }
}