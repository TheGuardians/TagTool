using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Tags
{
    public class TagStructure
    {
        private static readonly Dictionary<(CacheVersion version, CachePlatform platform), VersionedCache> VersionedCaches =
            new Dictionary<(CacheVersion version, CachePlatform platform), VersionedCache> { };

        public static TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version, CachePlatform cachePlatform) =>
            VersionedCaches[(version, cachePlatform)].GetTagStructureAttribute(type, version, cachePlatform);

        public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version, CachePlatform cachePlatform) =>
            VersionedCaches[(version, cachePlatform)].GetTagStructureInfo(type, version, cachePlatform);

        public static TagFieldEnumerable GetTagFieldEnumerable(Type type, CacheVersion version, CachePlatform cachePlatform) =>
            GetTagFieldEnumerable(GetTagStructureInfo(type, version, cachePlatform));

        public static TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info) =>
            VersionedCaches[(info.Version, info.CachePlatform)].GetTagFieldEnumerable(info);

        public static TagFieldAttribute GetTagFieldAttribute(Type type, FieldInfo field, CacheVersion version, CachePlatform cachePlatform) =>
            VersionedCaches[(version, cachePlatform)].GetTagFieldAttribute(type, field, version, cachePlatform);

        static TagStructure()
        {
            lock (VersionedCaches)
            {
                foreach (var platform in Enum.GetValues(typeof(CachePlatform)) as CachePlatform[])
                    foreach (var version in Enum.GetValues(typeof(CacheVersion)) as CacheVersion[])
                        VersionedCaches[(version, platform)] = new VersionedCache(version, platform);
            }      
        }

        public TagStructureAttribute GetTagStructureAttribute(CacheVersion version, CachePlatform cachePlatform) =>
            GetTagStructureAttribute(GetType(), version, cachePlatform);

        public TagStructureInfo GetTagStructureInfo(CacheVersion version, CachePlatform cachePlatform) =>
            GetTagStructureInfo(GetType(), version, cachePlatform);

        public TagFieldEnumerable GetTagFieldEnumerable(CacheVersion version, CachePlatform cachePlatform) =>
            GetTagFieldEnumerable(GetType(), version, cachePlatform);

        public TagFieldAttribute GetTagFieldAttribute(FieldInfo fieldInfo, CacheVersion version, CachePlatform cachePlatform) =>
            GetTagFieldAttribute(GetType(), fieldInfo, version, cachePlatform);

        public virtual void PreConvert(CacheVersion from, CacheVersion to)
        {
        }

        public virtual void PostConvert(CacheVersion from, CacheVersion to)
        {
        }

        private class VersionedCache
        {
            private readonly CacheVersion Version;
            private readonly CachePlatform Platform;

            private readonly Dictionary<Type, TagStructureAttribute> TagStructureAttributes =
                new Dictionary<Type, TagStructureAttribute> { };

            private readonly Dictionary<Type, TagStructureInfo> TagStructureInfos =
                new Dictionary<Type, TagStructureInfo> { };

            private readonly Dictionary<Type, TagFieldEnumerable> TagFieldEnumerables =
                new Dictionary<Type, TagFieldEnumerable> { };

            private readonly Dictionary<FieldInfo, TagFieldAttribute> TagFieldAttributes =
                new Dictionary<FieldInfo, TagFieldAttribute> { };

            public TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version, CachePlatform cachePlatform)
            {
                if (!TagStructureInfos.TryGetValue(type, out TagStructureInfo info))
                    lock (TagStructureInfos)
                    {
                        if (!TagStructureInfos.TryGetValue(type, out info))
                            TagStructureInfos[type] = info = new TagStructureInfo(type, version, cachePlatform);
                    }
                return info;
            }

            public TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info)
            {
                if (!TagFieldEnumerables.TryGetValue(info.Types[0], out TagFieldEnumerable enumerator))
                    lock (TagFieldEnumerables)
                    {
                        if (!TagFieldEnumerables.TryGetValue(info.Types[0], out enumerator))
                            TagFieldEnumerables[info.Types[0]] = enumerator = new TagFieldEnumerable(info);
                    }
                return enumerator;
            }

            public TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version, CachePlatform cachePlatform)
            {
                TagStructureAttribute GetStructureAttribute()
                {
                    // First match against any TagStructureAttributes that have version restrictions
                    var attrib = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                        .Cast<TagStructureAttribute>()
                        .Where(a => CacheVersionDetection.ComparePlatform(a.Platform, cachePlatform))
                        .FirstOrDefault(a => CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

                    if (attrib == null)
                        attrib = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                            .Cast<TagStructureAttribute>()
                            .Where(a => a.MinVersion != CacheVersion.Unknown || a.MaxVersion != CacheVersion.Unknown)
                            .FirstOrDefault(a => CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

                    // If nothing was found, find the first attribute without any version restrictions
                    return attrib ?? type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                        .Cast<TagStructureAttribute>()
                        .FirstOrDefault(a => a.MinVersion == CacheVersion.Unknown && a.MaxVersion == CacheVersion.Unknown && a.Platform == CachePlatform.All);
                }

                if (!TagStructureAttributes.TryGetValue(type, out TagStructureAttribute attribute))
                    lock (TagStructureAttributes)
                    {
                        if (!TagStructureAttributes.TryGetValue(type, out attribute))
                            TagStructureAttributes[type] = attribute = GetStructureAttribute();
                    }

                return attribute;
            }

            public TagFieldAttribute GetTagFieldAttribute(Type type, FieldInfo field, CacheVersion version, CachePlatform cachePlatform)
            {
                if (field.DeclaringType != type && !type.IsSubclassOf(field.DeclaringType))
                    throw new ArgumentException(nameof(field), new TypeAccessException(type.FullName));

                TagFieldAttribute GetFieldAttribute()
                {
                    // First match against any TagFieldAttributes that have version restrictions
                    var attrib = field.GetCustomAttributes(typeof(TagFieldAttribute), false)
                        .Cast<TagFieldAttribute>()
                        .Where(a => CacheVersionDetection.ComparePlatform(a.Platform, cachePlatform))
                        .FirstOrDefault(a => CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

                    if (attrib == null)
                        attrib = field.GetCustomAttributes(typeof(TagFieldAttribute), false)
                            .Cast<TagFieldAttribute>()
                            .Where(a => a.MinVersion != CacheVersion.Unknown || a.MaxVersion != CacheVersion.Unknown)
                            .FirstOrDefault(a => CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

                    // If nothing was found, return the default attribute (other code is necessary down the line to check the return of this function
                    return attrib ?? field.GetCustomAttributes<TagFieldAttribute>(false).DefaultIfEmpty(TagFieldAttribute.Default).First();
                }

                if (!TagFieldAttributes.TryGetValue(field, out TagFieldAttribute attribute))
                    lock (TagFieldAttributes)
                    {
                        if (!TagFieldAttributes.TryGetValue(field, out attribute))
                            TagFieldAttributes[field] = attribute = GetFieldAttribute();
                    }

                return attribute;
            }

            public VersionedCache(CacheVersion version, CachePlatform cachePlatform)
            {
                Version = version;
                Platform = cachePlatform;
            }
        }

        public static uint GetStructureSize(Type type, CacheVersion version, CachePlatform cachePlatform)
        {
            uint size = 0;

            var currentType = type;

            while (currentType != typeof(object))
            {
                var attribute = VersionedCaches[(version, cachePlatform)].GetTagStructureAttribute(currentType, version, cachePlatform);

                currentType = currentType.BaseType;

                if (attribute == null)
                    continue;
                    
                size += attribute.Size;
            }
            return size;
        }
    }
}