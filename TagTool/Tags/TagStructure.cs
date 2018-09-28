using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Tags
{
    public class TagStructure
    {
        private static readonly Dictionary<CacheVersion, VersionedCache> VersionedCaches =
            new Dictionary<CacheVersion, VersionedCache> { };

        public static TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version = CacheVersion.Unknown) =>
            VersionedCaches[version].GetTagStructureAttribute(type, version);

        public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown) =>
            VersionedCaches[version].GetTagStructureInfo(type, version);

        public static TagFieldEnumerable GetTagFieldEnumerable(Type type, CacheVersion version = CacheVersion.Unknown) =>
            GetTagFieldEnumerable(GetTagStructureInfo(type, version));

        public static TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info) =>
            VersionedCaches[info.Version].GetTagFieldEnumerable(info);

        public static TagFieldAttribute GetTagFieldAttribute(Type type, FieldInfo field, CacheVersion version = CacheVersion.Unknown) =>
            VersionedCaches[version].GetTagFieldAttribute(type, field, version);

        static TagStructure()
        {
            lock (VersionedCaches)
                foreach (var version in Enum.GetValues(typeof(CacheVersion)) as CacheVersion[])
                    VersionedCaches[version] = new VersionedCache(version);
        }

        public TagStructureAttribute GetTagStructureAttribute(CacheVersion version = CacheVersion.Unknown) =>
            GetTagStructureAttribute(GetType(), version);

        public TagStructureInfo GetTagStructureInfo(CacheVersion version = CacheVersion.Unknown) =>
            GetTagStructureInfo(GetType(), version);

        public TagFieldEnumerable GetTagFieldEnumerable(CacheVersion version = CacheVersion.Unknown) =>
            GetTagFieldEnumerable(GetType(), version);

        public TagFieldAttribute GetTagFieldAttribute(FieldInfo fieldInfo, CacheVersion version = CacheVersion.Unknown) =>
            GetTagFieldAttribute(GetType(), fieldInfo, version);

        protected virtual void PreConvert(CacheVersion from, CacheVersion to)
        {
            throw new NotImplementedException();
        }

        protected virtual void MainConvert(CacheVersion from, CacheVersion to)
        {
            throw new NotImplementedException();
        }

        protected virtual void PostConvert(CacheVersion from, CacheVersion to)
        {
            throw new NotImplementedException();
        }

        public void Convert(CacheVersion from, CacheVersion to)
        {
            PreConvert(from, to);
            MainConvert(from, to);
            PostConvert(from, to);
        }

        private class VersionedCache
        {
            private readonly CacheVersion Version;

            private readonly Dictionary<Type, TagStructureAttribute> TagStructureAttributes =
                new Dictionary<Type, TagStructureAttribute> { };

            private readonly Dictionary<Type, TagStructureInfo> TagStructureInfos =
                new Dictionary<Type, TagStructureInfo> { };

            private readonly Dictionary<Type, TagFieldEnumerable> TagFieldEnumerables =
                new Dictionary<Type, TagFieldEnumerable> { };

            private readonly Dictionary<FieldInfo, TagFieldAttribute> TagFieldAttributes =
                new Dictionary<FieldInfo, TagFieldAttribute> { };

            public TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
            {
                if (!TagStructureInfos.TryGetValue(type, out TagStructureInfo info))
                    lock (TagStructureInfos)
                    {
                        if (!TagStructureInfos.TryGetValue(type, out info))
                            TagStructureInfos[type] = info = new TagStructureInfo(type, version);
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

            public TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version = CacheVersion.Unknown)
            {
                if (!TagStructureAttributes.TryGetValue(type, out TagStructureAttribute attribute))
                    lock (TagStructureAttributes)
                    {
                        if (!TagStructureAttributes.TryGetValue(type, out attribute))
                            TagStructureAttributes[type] = attribute = GetStructureAttribute();
                    }
                return attribute;

                TagStructureAttribute GetStructureAttribute()
                {
                    // First match against any TagStructureAttributes that have version restrictions
                    var attrib = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                        .Cast<TagStructureAttribute>()
                        .Where(a => a.MinVersion != CacheVersion.Unknown || a.MaxVersion != CacheVersion.Unknown)
                        .FirstOrDefault(a => CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

                    // If nothing was found, find the first attribute without any version restrictions
                    return attrib ?? type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                        .Cast<TagStructureAttribute>()
                        .FirstOrDefault(a => a.MinVersion == CacheVersion.Unknown && a.MaxVersion == CacheVersion.Unknown);
                }
            }

            public TagFieldAttribute GetTagFieldAttribute(Type type, FieldInfo field, CacheVersion version = CacheVersion.Unknown)
            {
                if (field.DeclaringType != type && !type.IsSubclassOf(field.DeclaringType))
                    throw new ArgumentException(nameof(field), new TypeAccessException(type.FullName));

                if (!TagFieldAttributes.TryGetValue(field, out TagFieldAttribute attribute))
                    lock (TagFieldAttributes)
                    {
                        if (!TagFieldAttributes.TryGetValue(field, out attribute))
                            TagFieldAttributes[field] = attribute =
                                field.GetCustomAttributes<TagFieldAttribute>(false).DefaultIfEmpty(TagFieldAttribute.Default).First();
                    }
                return attribute;
            }

            public VersionedCache(CacheVersion version) => Version = version;
        }
    }
}