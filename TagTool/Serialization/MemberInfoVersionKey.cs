using System;
using System.Collections.Generic;
using System.Reflection;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Serialization
{
	/// <summary>
	/// Combines a <see cref="MemberInfo"/>.MetadataToken and (<see cref="int"/>)<see cref="CacheVersion"/> into
	/// a unified value which can be used as a key into a <see cref="Dictionary{TKey, TValue}"/>.
	/// </summary>
	public struct MemberInfoVersionKey : IEquatable<MemberInfoVersionKey>
	{
		private readonly MemberInfo _memberInfo;

		private readonly int _metadataToken;

		/// <summary>
		/// Contains the <see cref="CacheVersion"/> that was used in construction, cast to an <see cref="int"/>.
		/// </summary>
		private readonly int _version;

		public MemberInfoVersionKey(MemberInfo memberInfo, CacheVersion version)
		{
			this._memberInfo = memberInfo;
			this._metadataToken = memberInfo.MetadataToken;
			this._version = (int)version;
		}

		/// <summary>
		/// Returns a hash-code generated from the <see cref="MemberInfo"/> 
		/// and <see cref="CacheVersion"/> used in construction.
		/// </summary>
		public override int GetHashCode() =>
			(17 * 31 + this._metadataToken) * 31 + this._version;

		/// <summary>
		/// <c>true</c> if the <see cref="MemberInfo"/> and <see cref="CacheVersion"/> values of 
		/// the <see cref="MemberInfoVersionKey"/> and 'other' <see cref="MemberInfoVersionKey"/> are the same. <c>false</c> if the values
		/// are different.
		/// </summary>
		/// <param name="other"> The 'other' <see cref="MemberInfoVersionKey"/> to compare against. </param>
		public bool Equals(MemberInfoVersionKey other) =>
			this._memberInfo == other._memberInfo && this._version == other._version;

		/// <summary>
		/// <c>true</c> if the <see cref="MemberInfo"/> and <see cref="CacheVersion"/> values of 
		/// the <see cref="MemberInfoVersionKey"/> and 'other' <see cref="object"/> are the same. <c>false</c> if the values
		/// are different, or the 'other' <see cref="object"/> can't be cast to <see cref="MemberInfoVersionKey"/>.
		/// </summary>
		/// <param name="other"> The 'other' <see cref="object"/> to compare against. </param>
		public override bool Equals(object other) =>
			other is MemberInfoVersionKey ? Equals((MemberInfoVersionKey)other) : false;
	}
}