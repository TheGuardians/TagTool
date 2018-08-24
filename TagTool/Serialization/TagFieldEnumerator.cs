using TagTool.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Common;
using TagTool.Shaders;

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

            this.Field = this._tagFieldInfos[this._nextIndex].Field;
			this.Attribute = this._tagFieldInfos[this._nextIndex].Attribute;

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
			uint offset = 0;
            // Build the field list. Scan through the type's inheritance
            // hierarchy and add any fields belonging to tag structures.
            foreach (var type in Info.Types.Reverse<Type>())
            {
                // Ensure that fields are in declaration order - GetFields does NOT guarantee this!
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken))
                {
                    var attribute = field.GetCustomAttributes(typeof(TagFieldAttribute), false).FirstOrDefault() as TagFieldAttribute ?? DefaultFieldAttribute;
					var fieldSize = TagFieldEnumerator.GetFieldSize(field.FieldType, attribute);
					var tagFieldInfo = new TagFieldInfo(field, attribute, offset, fieldSize);

                    if (attribute.Gen3Only)
                    {
						if (Info.Version == CacheVersion.Halo3Retail || Info.Version == CacheVersion.Halo3ODST || Info.Version == CacheVersion.HaloReach)
						{
							_tagFieldInfos.Add(tagFieldInfo);
							offset++;
						}
                        continue;
                    }

                    if (attribute.HaloOnlineOnly)
                    {
						if (CacheVersionDetection.IsBetween(Info.Version, CacheVersion.HaloOnline106708, CacheVersion.HaloOnline700123))
						{
							_tagFieldInfos.Add(tagFieldInfo);
							offset++;
						}
                        continue;
                    }

                    if (attribute.Version != CacheVersion.Unknown)
                    {
						if (Info.Version == attribute.Version)
						{
							_tagFieldInfos.Add(tagFieldInfo);
							offset++;
						}
                        continue;
                    }

					if (CacheVersionDetection.IsBetween(Info.Version, attribute.MinVersion, attribute.MaxVersion))
					{
						_tagFieldInfos.Add(tagFieldInfo);
						offset++;
					}
                }
            }
        }

		/// <summary>
		/// Gets the size of a tag-field.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the field.</param>
		/// <param name="attr">The <see cref="TagFieldAttribute"/> of the field.</param>
		/// <returns></returns>
		private static uint GetFieldSize(Type type, TagFieldAttribute attr)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
					return 1;
				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.UInt16:
					return 2;
				case TypeCode.Single:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Object when attr != null && attr.Pointer:
				case TypeCode.Object when type == typeof(Tag):
				case TypeCode.Object when type == typeof(CacheAddress):
				case TypeCode.Object when type == typeof(CachedTagInstance) && attr.Short:
				case TypeCode.Object when type == typeof(ArgbColor):
				case TypeCode.Object when type == typeof(Point2d):
				case TypeCode.Object when type == typeof(StringId):
				case TypeCode.Object when type == typeof(Angle):
				case TypeCode.Object when type == typeof(VertexShaderReference):
				case TypeCode.Object when type == typeof(PixelShaderReference):
					return 4;
				case TypeCode.Double:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Object when type == typeof(CachedTagInstance) && attr.Version <= CacheVersion.Halo2Vista:
				case TypeCode.Object when type == typeof(Byte[]) && attr.Version <= CacheVersion.Halo2Vista:
				case TypeCode.Object when type == typeof(Rectangle2d):
				case TypeCode.Object when type == typeof(RealEulerAngles2d):
				case TypeCode.Object when type == typeof(RealPoint2d):
				case TypeCode.Object when type == typeof(RealVector2d):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && attr.Version <= CacheVersion.Halo2Vista:
					return 8;
				case TypeCode.Object when type == typeof(RealRgbColor):
				case TypeCode.Object when type == typeof(RealEulerAngles3d):
				case TypeCode.Object when type == typeof(RealPoint3d):
				case TypeCode.Object when type == typeof(RealVector3d):
				case TypeCode.Object when type == typeof(RealPlane2d):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && attr.Version > CacheVersion.Halo2Vista:
					return 12;
				case TypeCode.Decimal:
				case TypeCode.Object when type == typeof(CachedTagInstance) && attr.Version > CacheVersion.Halo2Vista:
				case TypeCode.Object when type == typeof(RealArgbColor):
				case TypeCode.Object when type == typeof(RealQuaternion):
				case TypeCode.Object when type == typeof(RealPlane3d):
					return 16;

				case TypeCode.Object when type == typeof(Byte[]) && attr.Version > CacheVersion.Halo2Vista:
					return 20;

				case TypeCode.Object when type == typeof(RealMatrix4x3):
					return 48;

				case TypeCode.String:
				case TypeCode.Object when type.IsArray:
					return (uint)attr.Length;

				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Bounds<>):
					return TagFieldEnumerator.GetFieldSize(type.GenericTypeArguments[0], attr) * 2;

				case TypeCode.Object when type.IsEnum:
					return TagFieldEnumerator.GetFieldSize(type.GetEnumUnderlyingType(), attr);

				// Assume the field is a structure
				default:
					return ReflectionCache.GetTagStructureInfo(type, attr.Version).TotalSize;
			}
		}

		/// <summary>
		/// Finds a <see cref="TagFieldInfo"/> based on a <see cref="FieldInfo"/> <see cref="Predicate{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="FieldInfo"/> <see cref="Predicate{T}"/> to query.</param>
		/// <returns></returns>
		public FieldInfo Find(Predicate<FieldInfo> match) =>
			this._tagFieldInfos.Find(f => match.Invoke(f.Field)).Field;
	}
}