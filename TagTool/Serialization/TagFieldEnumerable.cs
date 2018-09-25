using TagTool.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace TagTool.Serialization
{
	/// <summary>
	/// Allows easy enumeration over a tag structure's elements and filtering by version.
	/// </summary>
	public class TagFieldEnumerable : IEnumerable<TagFieldInfo>
	{
		/// <summary>
		/// Collection of <see cref="Serialization.TagFieldInfo"/> for a <see cref="TagStructureInfo"/> in a given
		/// <see cref="CacheVersion"/>.
		/// </summary>
		private readonly List<TagFieldInfo> TagFieldInfos = new List<TagFieldInfo> { };

		/// <summary>
		/// Constructs aa <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/> over a tag structure for a <see cref="TagStructureInfo"/> in a given
		/// <see cref="CacheVersion"/>.
		/// </summary>
		/// <param name="info">The info for the structure. Only fields which match the version used to create the info will be enumerated over.</param>
		public TagFieldEnumerable(TagStructureInfo info)
		{
			Info = info;
			Build();
		}

		/// <summary>
		/// Gets the info that was used to construct the enumerator.
		/// </summary>
		public TagStructureInfo Info { get; private set; }

		/// <summary>
		/// Gets information about the current field.
		/// </summary>
		public TagFieldInfo TagFieldInfo { get; private set; }

		/// <summary>
		/// Gets the count of <see cref="TagFieldInfos"/>.
		/// </summary>
		public int Count => TagFieldInfos.Count;

		/// <summary>
		/// Gets an <see cref="IEnumerator{T}"/> over the <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/>.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<TagFieldInfo> GetEnumerator() => TagFieldInfos.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => TagFieldInfos.GetEnumerator();

		/// <summary>
		/// An indexer into the <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/>.
		/// </summary>
		/// <param name="index">The index into the <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/>.</param>
		/// <returns>The <see cref="Serialization.TagFieldInfo"/> at the specified index in the 
		/// <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/>.</returns>
		public TagFieldInfo this[int index] => TagFieldInfos[index];

		/// <summary>
		/// Builds the <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/> to be enumerated.
		/// </summary>
		private void Build()
		{
			uint offset = 0;

			// Build the field list. Scan through the type's inheritance
			// hierarchy and add any fields belonging to tag structures.
			foreach (var type in Info.Types.Reverse<Type>())
			{
				// Ensure that fields are in declaration order - GetFields does NOT guarantee 
				foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken))
				{
					var attribute = ReflectionCache.GetTagFieldAttribute(field);

					if (attribute.Gen3Only)
					{
						if (Info.Version == CacheVersion.Halo3Retail || Info.Version == CacheVersion.Halo3ODST || Info.Version == CacheVersion.HaloReach)
							CreateTagFieldInfo(field, attribute, ref offset);
						continue;
					}

					if (attribute.HaloOnlineOnly)
					{
						if (CacheVersionDetection.IsBetween(Info.Version, CacheVersion.HaloOnline106708, CacheVersion.HaloOnline700123))
							CreateTagFieldInfo(field, attribute, ref offset);
						continue;
					}

					if (attribute.Version != CacheVersion.Unknown)
					{
						if (Info.Version == attribute.Version)
							CreateTagFieldInfo(field, attribute, ref offset);
						continue;
					}

					if (CacheVersionDetection.IsBetween(Info.Version, attribute.MinVersion, attribute.MaxVersion))
						CreateTagFieldInfo(field, attribute, ref offset);
				}
			}
		}

		/// <summary>
		/// Creates and adds a <see cref="Serialization.TagFieldInfo"/> to the <see cref="Serialization.TagFieldInfo"/> <see cref="List{T}"/>.
		/// </summary>
		/// <param name="field">The <see cref="FieldInfo"/> to create the <see cref="Serialization.TagFieldInfo"/> from.</param>
		/// <param name="attribute">The <see cref="TagFieldAttribute"/> for the <see cref="Serialization.TagFieldInfo"/>.</param>
		/// <param name="offset">The offset (in bytes) of the field. Gets updated to reflect the new offset following field.</param>
		private void CreateTagFieldInfo(FieldInfo field, TagFieldAttribute attribute, ref uint offset)
		{
			var fieldSize = TagFieldInfo.GetFieldSize(field.FieldType, attribute);
			var tagFieldInfo = new TagFieldInfo(field, attribute, offset, fieldSize);
			TagFieldInfos.Add(tagFieldInfo);
			offset += fieldSize;
		}

		/// <summary>
		/// Finds a <see cref="Serialization.TagFieldInfo"/> based on a <see cref="FieldInfo"/> <see cref="Predicate{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="FieldInfo"/> <see cref="Predicate{T}"/> to query.</param>
		/// <returns></returns>
		public FieldInfo Find(Predicate<FieldInfo> match) =>
			TagFieldInfos.Find(f => match.Invoke(f.FieldInfo))?.FieldInfo ?? null;
	}
}