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
		/// <see cref="Dictionary{TKey, TValue}"/> using <see cref="MemberInfoVersionKey"/> as keys,
		/// and <see cref="TagStructureInfo"/> as values.
		/// </summary>
		private static readonly Dictionary<MemberInfoVersionKey, TagStructureInfo> _tagStructureInfos =
			new Dictionary<MemberInfoVersionKey, TagStructureInfo> { };

		/// <summary>
		/// <see cref="Dictionary{TKey, TValue}"/> using <see cref="MemberInfoVersionKey"/> as keys,
		/// and <see cref="TagFieldEnumerator"/> as values.
		/// </summary>
		private static readonly Dictionary<MemberInfoVersionKey, TagFieldEnumerator> _tagFieldEnumerators =
			new Dictionary<MemberInfoVersionKey, TagFieldEnumerator> { };

		/// <summary>
		/// Finds a cached <see cref="TagStructureInfo"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			var key = new MemberInfoVersionKey(type, version);
			if (!ReflectionCache._tagStructureInfos.TryGetValue(key, out TagStructureInfo info))
				ReflectionCache._tagStructureInfos[key] = info = new TagStructureInfo(type, version);
			return info;
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerator"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="info">The <see cref="TagStructureInfo"/> used in lookup/creation.</param>
		public static TagFieldEnumerator GetTagFieldEnumerator(TagStructureInfo info)
		{
			var key = new MemberInfoVersionKey(info.Types[0], info.Version);
			if (!ReflectionCache._tagFieldEnumerators.TryGetValue(key, out TagFieldEnumerator enumerator))
				ReflectionCache._tagFieldEnumerators[key] = enumerator = new TagFieldEnumerator(info);
			enumerator.Reset();
			return enumerator;
		}

		/// <summary>
		/// Finds a cached <see cref="TagFieldEnumerator"/> or creates a new one (and caches it for later use).
		/// </summary>
		/// <param name="type">The <see cref="Type"/> used in lookup/creation.</param>
		/// <param name="version">The <see cref="CacheVersion"/> used in lookup/creation. Defaults to <see cref="CacheVersion.Unknown"/></param>
		public static TagFieldEnumerator GetTagFieldEnumerator(Type type, CacheVersion version = CacheVersion.Unknown) =>
			ReflectionCache.GetTagFieldEnumerator(ReflectionCache.GetTagStructureInfo(type, version));
	}
}