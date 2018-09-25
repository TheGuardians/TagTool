using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Serialization
{
	/// <summary>
	/// Utility class for caching/accessing a <see cref="TagStructureInfo"/> or <see cref="TagFieldEnumerable"/>.
	/// </summary>
	public static class ReflectionCache
	{
		private static readonly Dictionary<CacheVersion, VersionedCache> VersionedCaches =
			new Dictionary<CacheVersion, VersionedCache> { };

		public static TagFieldEnumerable GetTagFieldEnumerable(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			var info = ReflectionCache.GetTagStructureInfo(type, version);
			var enumerator = ReflectionCache.GetTagFieldEnumerable(info);
			return enumerator;
		}

		public static TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info)
			=> ReflectionCache.VersionedCaches[info.Version].GetTagFieldEnumerable(info);

		public static TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version = CacheVersion.Unknown)
			=> ReflectionCache.VersionedCaches[version].GetTagStructureAttribute(type, version);

		public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
			=> ReflectionCache.VersionedCaches[version].GetTagStructureInfo(type, version);

		public static TagFieldAttribute GetTagFieldAttribute(FieldInfo field, CacheVersion version = CacheVersion.Unknown)
			=> ReflectionCache.VersionedCaches[version].GetTagFieldAttribute(field, version);

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
				if (!this.TagStructureInfos.TryGetValue(type, out TagStructureInfo info))
					lock (this.TagStructureInfos)
					{
						if (!this.TagStructureInfos.TryGetValue(type, out info))
							this.TagStructureInfos[type] = info = new TagStructureInfo(type, version);
					}
				return info;
			}

			public TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info)
			{
				if (!this.TagFieldEnumerables.TryGetValue(info.Types[0], out TagFieldEnumerable enumerator))
					lock (this.TagFieldEnumerables)
					{
						if (!this.TagFieldEnumerables.TryGetValue(info.Types[0], out enumerator))
							this.TagFieldEnumerables[info.Types[0]] = enumerator = new TagFieldEnumerable(info);
					}
				return enumerator;
			}

			public TagStructureAttribute GetTagStructureAttribute(Type type, CacheVersion version = CacheVersion.Unknown)
			{
				if (!this.TagStructureAttributes.TryGetValue(type, out TagStructureAttribute attribute))
					lock (this.TagStructureInfos)
					{
						if (!this.TagStructureAttributes.TryGetValue(type, out attribute))
							this.TagStructureAttributes[type] = attribute = GetStructureAttribute();
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

			public TagFieldAttribute GetTagFieldAttribute(FieldInfo field, CacheVersion version = CacheVersion.Unknown)
			{
				if (!this.TagFieldAttributes.TryGetValue(field, out TagFieldAttribute attribute))
					lock (this.TagFieldEnumerables)
					{
						if (!this.TagFieldAttributes.TryGetValue(field, out attribute))
							this.TagFieldAttributes[field] = attribute =
								field.GetCustomAttributes<TagFieldAttribute>(false).DefaultIfEmpty(TagFieldAttribute.Default).First();
					}
				return attribute;
			}

			public VersionedCache(CacheVersion version) => this.Version = version;
		}

		static ReflectionCache()
		{
			lock (ReflectionCache.VersionedCaches)
				foreach (var version in Enum.GetValues(typeof(CacheVersion)) as CacheVersion[])
					ReflectionCache.VersionedCaches[version] = new VersionedCache(version);
		}
	}
}