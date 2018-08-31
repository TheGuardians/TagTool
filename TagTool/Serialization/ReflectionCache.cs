#define TEST_REFLECTION_CACHE

using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Serialization
{
	/// <summary>
	/// Utility class for caching/accessing a <see cref="TagStructureInfo"/> or <see cref="TagFieldEnumerator"/>.
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
		private static readonly Dictionary<MemberInfoVersionKey, TagStructureInfo> tagStructureInfos =
			new Dictionary<MemberInfoVersionKey, TagStructureInfo> { };

		/// <summary>
		/// <see cref="Dictionary{TKey, TValue}"/> using <see cref="MemberInfoVersionKey"/> as keys,
		/// and <see cref="TagFieldEnumerator"/> as values.
		/// </summary>
		private static readonly Dictionary<MemberInfoVersionKey, TagFieldEnumerator> tagFieldEnumerators =
			new Dictionary<MemberInfoVersionKey, TagFieldEnumerator> { };

		/// <summary>
		/// Finds a cached <see cref="TagStructureInfo"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
		{

#if TEST_REFLECTION_CACHE
			if (!ReflectionCache.IsEnabled)
				return new TagStructureInfo(type, version);

			var key = new MemberInfoVersionKey(type, version);
			if (!ReflectionCache.tagStructureInfos.TryGetValue(key, out TagStructureInfo info))
				ReflectionCache.tagStructureInfos[key] = info = new TagStructureInfo(type, version);
			return info;
#else
			return new TagStructureInfo(type, version);
#endif
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerator"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="info">The <see cref="TagStructureInfo"/> used in lookup/creation.</param>
		public static TagFieldEnumerator GetTagFieldEnumerator(TagStructureInfo info)
		{
#if TEST_REFLECTION_CACHE
			if (!ReflectionCache.IsEnabled)
				return new TagFieldEnumerator(info);
			
			var key = new MemberInfoVersionKey(info.Types[0], info.Version);
			if (!ReflectionCache.tagFieldEnumerators.TryGetValue(key, out TagFieldEnumerator enumerator))
				ReflectionCache.tagFieldEnumerators[key] = enumerator = new TagFieldEnumerator(info);

			if (enumerator.IsFree)
				return enumerator;
			else
				return new TagFieldEnumerator(info);
#else
			return new TagFieldEnumerator(info);
#endif
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerator"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagFieldEnumerator GetTagFieldEnumerator(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			var info = ReflectionCache.GetTagStructureInfo(type, version);
			var enumerator = ReflectionCache.GetTagFieldEnumerator(info);
			return enumerator;
		}
	}
}