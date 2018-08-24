using System;
using System.Collections.Generic;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Serialization
{
	/// <summary>
	/// Combines a <see cref="MemberInfo"/>.MetadataToken and (<see cref="int"/>)<see cref="CacheVersion"/> into
	/// a unified value which can be used as a key into a <see cref="Dictionary{TKey, TValue}"/>.
	/// </summary>
	public struct MemberInfoVersionKey : IEquatable<MemberInfoVersionKey>
	{
		/// <summary>
		/// Contains the MetaDataToken of the <see cref="MemberInfo"/> that was used in construction.
		/// </summary>
		private readonly int _memberInfo;

		/// <summary>
		/// Contains the <see cref="CacheVersion"/> that was used in construction, cast to an <see cref="int"/>.
		/// </summary>
		private readonly int _version;

		/// <summary>
		/// Constructs a new <see cref="MemberInfoVersionKey"/> from a <see cref="MemberInfo"/> and <see cref="CacheVersion"/>.
		/// </summary>
		/// <param name="memberInfo"> The <see cref="MemberInfo"/> used as the first half of the key. </param>
		/// <param name="version"> The <see cref="CacheVersion"/> used as the second half of the key. </param>
		public MemberInfoVersionKey(MemberInfo memberInfo, CacheVersion version)
		{
			this._memberInfo = memberInfo.MetadataToken;
			this._version = (int)version;
		}

		/// <summary>
		/// Returns a hash-code generated from the <see cref="MemberInfo"/> 
		/// and <see cref="CacheVersion"/> used in construction.
		/// </summary>
		public override int GetHashCode() =>
			(17 * 31 + this._memberInfo) * 31 + this._version;

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