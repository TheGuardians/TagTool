using TagTool.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TagTool.Serialization
{
    /// <summary>
    /// Allows easy enumeration over a tag structure's elements and filtering by version.
    /// </summary>
    public class TagFieldEnumerator
    {
        private static readonly TagFieldAttribute DefaultFieldAttribute = new TagFieldAttribute();

        private readonly List<TagFieldInfo> _tagFieldInfos = new List<TagFieldInfo>();
        private int _nextIndex;
		
        /// <summary>
        /// Constructs an enumerator over a tag structure.
        /// </summary>
        /// <param name="info">The info for the structure. Only fields which match the version used to create the info will be enumerated over.</param>
        public TagFieldEnumerator(TagStructureInfo info)
        {
            this.Info = info;
            this.Build();
        }

        /// <summary>
        /// Gets the info that was used to construct the enumerator.
        /// </summary>
        public TagStructureInfo Info { get; private set; }

        /// <summary>
        /// Gets information about the current field.
        /// </summary>
        public FieldInfo Field { get; private set; }

        /// <summary>
        /// Gets the current property's <see cref="TagFieldAttribute"/>.
        /// </summary>
        public TagFieldAttribute Attribute { get; private set; }
        
        /// <summary>
        /// Moves to the next tag field in the structure.
        /// This must be called before accessing any of the other properties.
        /// </summary>
        /// <returns><c>true</c> if the enumerator moved to a new element, or <c>false</c> if the end of the structure has been reached.</returns>
        public bool Next()
        {
            if (this._tagFieldInfos == null || this._nextIndex >= this._tagFieldInfos.Count)
                return false;

            this.Field = this._tagFieldInfos[this._nextIndex].FieldInfo;
			this.Attribute = this._tagFieldInfos[this._nextIndex].TagFieldAttribute;

			this._nextIndex++;

            return true;
        }

		/// <summary>
		/// Resets the <see cref="TagFieldEnumerator"/> position, <see cref="Field"/>, and <see cref="Attribute"/>.
		/// </summary>
		public void Reset()
		{
			this._nextIndex = 0;
			this.Field = null;
			this.Attribute = null;
		}

		/// <summary>
		/// Builds the <see cref="TagFieldInfo"/> <see cref="List{T}"/> to be enumerated.
		/// </summary>
		private void Build()
        {
            // Build the field list. Scan through the type's inheritance
            // hierarchy and add any fields belonging to tag structures.
            foreach (var type in Info.Types.Reverse<Type>())
            {
                // Ensure that fields are in declaration order - GetFields does NOT guarantee this!
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken))
                {
                    var attribute = field.GetCustomAttributes(typeof(TagFieldAttribute), false).FirstOrDefault() as TagFieldAttribute ?? DefaultFieldAttribute;
					var tagFieldInfo = new TagFieldInfo(field, attribute);

                    if (attribute.Gen3Only)
                    {
                        if (Info.Version == CacheVersion.Halo3Retail || Info.Version == CacheVersion.Halo3ODST || Info.Version == CacheVersion.HaloReach)
                            _tagFieldInfos.Add(tagFieldInfo);
                        continue;
                    }

                    if (attribute.HaloOnlineOnly)
                    {
                        if (CacheVersionDetection.IsBetween(Info.Version, CacheVersion.HaloOnline106708, CacheVersion.HaloOnline700123))
                            _tagFieldInfos.Add(tagFieldInfo);
                        continue;
                    }

                    if (attribute.Version != CacheVersion.Unknown)
                    {
                        if (Info.Version == attribute.Version)
                            _tagFieldInfos.Add(tagFieldInfo);
                        continue;
                    }

                    if (CacheVersionDetection.IsBetween(Info.Version, attribute.MinVersion, attribute.MaxVersion))
                        _tagFieldInfos.Add(tagFieldInfo);
                }
            }
        }

		/// <summary>
		/// Finds a <see cref="TagFieldInfo"/> based on a <see cref="FieldInfo"/> <see cref="Predicate{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="FieldInfo"/> <see cref="Predicate{T}"/> to query.</param>
		/// <returns></returns>
		public FieldInfo Find(Predicate<FieldInfo> match) =>
			this._tagFieldInfos.Find(f => match.Invoke(f.FieldInfo)).FieldInfo;
	}
}