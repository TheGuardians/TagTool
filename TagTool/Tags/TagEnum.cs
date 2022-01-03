using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Tags
{
    // TODO: merge into TagStructure, just keeping it separate for now while we trial this

    public class TagEnum
    {
        private static Dictionary<(CacheVersion, CachePlatform), VersionedCache> VersionedCaches
            = new Dictionary<(CacheVersion, CachePlatform), VersionedCache>();

        static TagEnum()
        {
            foreach (var platform in Enum.GetValues(typeof(CachePlatform)) as CachePlatform[])
                foreach (var version in Enum.GetValues(typeof(CacheVersion)) as CacheVersion[])
                    VersionedCaches[(version, platform)] = new VersionedCache(version, platform);
        }

        public static TagEnumInfo GetInfo(Type enumType, CacheVersion version, CachePlatform platform)
        {
            return VersionedCaches[(version, platform)].GetInfo(enumType);
        }

        public static TagEnumMemberEnumerable GetMemberEnumerable(TagEnumInfo info)
        {
            return VersionedCaches[(info.CacheVersion, info.CachePlatform)].GetMemberEnumerable(info);
        }

        public static TagEnumMemberEnumerable GetMemberEnumerable(Type enumType, CacheVersion version, CachePlatform platform)
        {
            var info = GetInfo(enumType, version, platform);
            return VersionedCaches[(version, platform)].GetMemberEnumerable(info);
        }

        public static bool AttributeInCacheVersion(TagEnumMemberAttribute attr, CacheVersion compare)
        {
            if (attr.Version != CacheVersion.Unknown)
                if (attr.Version != compare)
                    return false;

            if (attr.Gen != CacheGeneration.Unknown)
                if (!CacheVersionDetection.IsInGen(attr.Gen, compare))
                    return false;

            if (attr.MinVersion != CacheVersion.Unknown || attr.MaxVersion != CacheVersion.Unknown)
                if (!CacheVersionDetection.IsBetween(compare, attr.MinVersion, attr.MaxVersion))
                    return false;

            return true;
        }

        public static bool AttributeInPlatform(TagEnumMemberAttribute attr, CachePlatform compare)
        {
            return CacheVersionDetection.ComparePlatform(attr.Platform, compare);
        }

        class VersionedCache
        {
            public CacheVersion CacheVersion;
            public CachePlatform CachePlatform;

            public Dictionary<Type, TagEnumInfo> Infos = new Dictionary<Type, TagEnumInfo>();
            public Dictionary<FieldInfo, TagEnumMemberInfo> MemberInfos = new Dictionary<FieldInfo, TagEnumMemberInfo>();
            public Dictionary<TagEnumInfo, TagEnumMemberEnumerable> MemberEnumerables = new Dictionary<TagEnumInfo, TagEnumMemberEnumerable>();

            public VersionedCache(CacheVersion cacheVersion, CachePlatform cachePlatform)
            {
                CacheVersion = cacheVersion;
                CachePlatform = cachePlatform;
            }

            public TagEnumInfo GetInfo(Type enumType)
            {
                lock (Infos)
                {
                    if (!Infos.TryGetValue(enumType, out TagEnumInfo info))
                        Infos.Add(enumType, info = new TagEnumInfo(enumType, CacheVersion, CachePlatform));
                    return info;
                }
            }

            public TagEnumMemberEnumerable GetMemberEnumerable(TagEnumInfo info)
            {
                lock (MemberEnumerables)
                {
                    if (!MemberEnumerables.TryGetValue(info, out TagEnumMemberEnumerable enumerable))
                        MemberEnumerables.Add(info, enumerable = new TagEnumMemberEnumerable(info));
                    return enumerable;
                }
            }
        }
    }

    public class TagEnumInfo
    {
        public Type Type;
        public Type UnderlyingType;
        public CacheVersion CacheVersion;
        public CachePlatform CachePlatform;
        public TagEnumAttribute Attribute;
        public bool IsFlags;

        public TagEnumInfo(Type type, CacheVersion cacheVersion, CachePlatform cachePlatform)
        {
            Type = type;
            CacheVersion = cacheVersion;
            CachePlatform = cachePlatform;
            UnderlyingType = type.GetEnumUnderlyingType();
            Attribute = type.GetCustomAttribute<TagEnumAttribute>() ?? TagEnumAttribute.Default;
            IsFlags = type.GetCustomAttribute<FlagsAttribute>() != null;

            if (Attribute.IsVersioned)
            {
                var typeCode = Type.GetTypeCode(type.GetEnumUnderlyingType());

                if (typeCode != TypeCode.SByte && typeCode != TypeCode.Int16 && typeCode != TypeCode.Int32)
                    throw new NotSupportedException("Versioned enums must have a signed underlying type.");

                if (IsFlags)
                    throw new NotSupportedException("C# [Flags] Enum cannot be versioned, use FlagBits<Enum> instead.");
            }
        }
    }

    public class TagEnumMemberEnumerable : IEnumerable<TagEnumMemberInfo>
    {
        public TagEnumInfo Info;
        public List<TagEnumMemberInfo> Members = new List<TagEnumMemberInfo>();

        public TagEnumMemberEnumerable(TagEnumInfo info)
        {
            Info = info;
            Build();
        }

        public IEnumerator<TagEnumMemberInfo> GetEnumerator() => Members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Members.GetEnumerator();

        private void Build()
        {
            int memberIndex = 0;
            foreach (var fieldInfo in Info.Type.GetFields(BindingFlags.Static | BindingFlags.Public).OrderBy(x => x.MetadataToken))
            {
                var attr = fieldInfo.GetCustomAttribute<TagEnumMemberAttribute>() ?? TagEnumMemberAttribute.Default;

                if (TagEnum.AttributeInCacheVersion(attr, Info.CacheVersion) &&
                    TagEnum.AttributeInPlatform(attr, Info.CachePlatform))
                {
                    var value = fieldInfo.GetValue(null);
                    if (Info.Attribute.IsVersioned)
                    {
                        if (memberIndex.Equals(value))
                            throw new NotSupportedException("Versioned enums must be sequential.");
                    }

                    Members.Add(new TagEnumMemberInfo(fieldInfo, attr, value));
                }

                memberIndex++;
            }
        }
    }

    public class TagEnumMemberInfo
    {
        public FieldInfo FieldInfo;
        public TagEnumMemberAttribute Attribute;
        public object Value;

        public TagEnumMemberInfo(FieldInfo fieldInfo, TagEnumMemberAttribute attr, object value)
        {
            FieldInfo = fieldInfo;
            Attribute = attr;
            Value = value;
        }

        public string Name => FieldInfo.Name;
    }

    public class TagEnumAttribute : Attribute
    {
        public static readonly TagEnumAttribute Default = new TagEnumAttribute();
        public bool IsVersioned { get; set; } = false;
    }

    public class TagEnumMemberAttribute : Attribute
    {
        public static readonly TagEnumMemberAttribute Default = new TagEnumMemberAttribute();

        /// <summary>
        /// The minimum cache version the tag field is present in.
        /// </summary>
        public CacheVersion MinVersion { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The maximum cache version the tag field is present in.
        /// </summary>
        public CacheVersion MaxVersion { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The set of versions the tag field is present in. (Can be combined with MinVersion/MaxVersion)
        /// </summary>
        public CacheVersion Version { get; set; } = CacheVersion.Unknown;

        /// <summary>
        /// The game generation of the tag field.
        /// </summary>
        public CacheGeneration Gen { get; set; } = CacheGeneration.Unknown;

        /// <summary>
        /// The platforms that the tag field are available on.
        /// </summary>
        public CachePlatform Platform { get; set; } = CachePlatform.All;
    }
}
