using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace System
{
    public static class TypeExtensions
    {
        private static Dictionary<string, Tag> TypeGroupTags { get; } = new Dictionary<string, Tag>();

        public static Tag GetGroupTag(this Type type, CacheVersion version = CacheVersion.Unknown)
        {
            if (TypeGroupTags.ContainsKey(type.FullName))
                return TypeGroupTags[type.FullName];

            var attr = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                .Cast<TagStructureAttribute>()
                .FirstOrDefault(a => version == CacheVersion.Unknown || CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

            if (attr == null)
                throw new NotSupportedException(type.FullName);

            return TypeGroupTags[type.FullName] = new Tag(attr.Tag);
        }

        private static Dictionary<string, uint> TypeSizes { get; } = new Dictionary<string, uint>();

        public static uint GetSize(this Type type, CacheVersion version = CacheVersion.Unknown)
        {
            if (TypeSizes.ContainsKey(type.FullName))
                return TypeSizes[type.FullName];

            var attr = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                .Cast<TagStructureAttribute>()
                .FirstOrDefault(a => version == CacheVersion.Unknown || CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

            if (attr == null)
                throw new NotSupportedException(type.FullName);

            return TypeSizes[type.FullName] = attr.Size;
        }
    }
}