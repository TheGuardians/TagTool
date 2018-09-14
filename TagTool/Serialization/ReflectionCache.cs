using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Serialization
{
	/// <summary>
	/// Utility class for caching/accessing a <see cref="TagStructureInfo"/> or <see cref="TagFieldEnumerable"/>.
	/// </summary>
	public static class ReflectionCache
	{
		/// <summary>
		/// Disable this when working from multiple threads. Re-enable it when mult-threaded work is complete.
		/// </summary>
		public static bool IsEnabled = true;

		/// <summary>
		/// <see cref="Dictionary{TKey, TValue}"/> using <see cref="MemberInfoVersionKey"/> as keys,
		/// and <see cref="TagStructureInfo"/> as values.
		/// </summary>
		private static readonly Dictionary<MemberInfoVersionKey, TagStructureInfo> TagStructureInfos =
			new Dictionary<MemberInfoVersionKey, TagStructureInfo> { };

		/// <summary>
		/// <see cref="Dictionary{TKey, TValue}"/> using <see cref="MemberInfoVersionKey"/> as keys,
		/// and <see cref="TagFieldEnumerable"/> as values.
		/// </summary>
		private static readonly Dictionary<MemberInfoVersionKey, TagFieldEnumerable> TagFieldEnumerables =
			new Dictionary<MemberInfoVersionKey, TagFieldEnumerable> { };

		/// <summary>
		/// Finds a cached <see cref="TagStructureInfo"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			if (ReflectionCache.IsEnabled)
			{
				var key = new MemberInfoVersionKey(type, version);
				if (!ReflectionCache.TagStructureInfos.TryGetValue(key, out TagStructureInfo info))
					lock (ReflectionCache.TagStructureInfos)
					{
						if (!ReflectionCache.TagStructureInfos.TryGetValue(key, out info))
							ReflectionCache.TagStructureInfos[key] = info = new TagStructureInfo(type, version);
					}

				return info;
			}

			return new TagStructureInfo(type, version);
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerable"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="info">The <see cref="TagStructureInfo"/> used in lookup/creation.</param>
		public static TagFieldEnumerable GetTagFieldEnumerable(TagStructureInfo info)
		{
			if (ReflectionCache.IsEnabled)
			{
				var key = new MemberInfoVersionKey(info.Types[0], info.Version);

				if (!ReflectionCache.TagFieldEnumerables.TryGetValue(key, out TagFieldEnumerable enumerator))
					lock (ReflectionCache.TagFieldEnumerables)
					{
						if (!ReflectionCache.TagFieldEnumerables.TryGetValue(key, out enumerator))
							ReflectionCache.TagFieldEnumerables[key] = enumerator = new TagFieldEnumerable(info);
					}

				return enumerator;
			}

			return new TagFieldEnumerable(info);
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerable"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagFieldEnumerable GetTagFieldEnumerable(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			var info = ReflectionCache.GetTagStructureInfo(type, version);
			var enumerator = ReflectionCache.GetTagFieldEnumerable(info);
			return enumerator;
		}
	}
}