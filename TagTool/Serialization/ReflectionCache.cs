using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;

namespace TagTool.Serialization
{
	/// <summary>
	/// TODO: Document this. Currently only used by <see cref="TagFieldEnumerator(TagStructureInfo)"/> and
	/// <see cref="TagStructureInfo.GetStructureAttribute(Type, CacheVersion)"/>
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

		public static TagStructureInfo GetTagStructureInfo(Type type, CacheVersion version = CacheVersion.Unknown)
		{
			var key = new MemberInfoVersionKey(type, version);
			if (!ReflectionCache._tagStructureInfos.TryGetValue(key, out TagStructureInfo info))
				ReflectionCache._tagStructureInfos[key] = info = new TagStructureInfo(type, version);
			return info;
		}

		public static TagFieldEnumerator GetTagFieldEnumerator(TagStructureInfo info)
		{
			var key = new MemberInfoVersionKey(info.Types[0], info.Version);
			if (!ReflectionCache._tagFieldEnumerators.TryGetValue(key, out TagFieldEnumerator enumerator))
				ReflectionCache._tagFieldEnumerators[key] = enumerator = new TagFieldEnumerator(info);
			enumerator.Reset();
			return enumerator;
		}

		public static TagFieldEnumerator GetTagFieldEnumerator(Type type, CacheVersion version = CacheVersion.Unknown) =>
			ReflectionCache.GetTagFieldEnumerator(ReflectionCache.GetTagStructureInfo(type, version));
	}
}