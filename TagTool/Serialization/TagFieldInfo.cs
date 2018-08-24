using System;
using System.Collections.Generic;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;

namespace TagTool.Serialization
{
	/// <summary>
	/// Class for pairing of <see cref="System.Reflection.FieldInfo"/> and <see cref="Serialization.TagFieldAttribute"/>.
	/// </summary>
	public class TagFieldInfo
	{
		/// <summary>
		/// Constructs a <see cref="TagFieldInfo"/> from a <see cref="System.Reflection.FieldInfo"/> and <see cref="Serialization.TagFieldAttribute"/>.
		/// </summary>
		/// <param name="offset"> The offset of the field within the tag-structure. </param>
		/// <param name="field"> The <see cref="System.Reflection.FieldInfo"/> for the field. </param>
		/// <param name="attribute"> The <see cref="Serialization.TagFieldAttribute"/> for the field. </param>
		public TagFieldInfo(FieldInfo field, TagFieldAttribute attribute)
		{
			this.FieldInfo = field;
			this.Type = this.FieldInfo.FieldType;
			this.TagFieldAttribute = attribute;
			this.Offset = TagFieldInfo.GetFieldSize(field.FieldType, this.TagFieldAttribute);
		}

		/// <summary>
		/// Gets the offset of the field in it's structure.
		/// </summary>
		public uint Offset { get; }

		/// <summary>
		/// Gets the type of the field.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Gets the <see cref="System.Reflection.FieldInfo"/> that was used in construction.
		/// </summary>
		public FieldInfo FieldInfo { get; }

		/// <summary>
		/// Gets the <see cref="Serialization.TagFieldAttribute"/> that was used in construction.
		/// </summary>
		public TagFieldAttribute TagFieldAttribute { get; }

		/// <summary>
		/// Gets the size of a tag-field.
		/// </summary>
		/// <param name="type">The <see cref="System.Type"/> of the field.</param>
		/// <param name="attr">The <see cref="Serialization.TagFieldAttribute"/> of the field.</param>
		/// <returns></returns>
		private static uint GetFieldSize(Type type, TagFieldAttribute attr)
		{
			if (type.IsEnum)
				type = type.GetEnumUnderlyingType();

			switch (Type.GetTypeCode(type))
			{
				#region Primitives
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
				case TypeCode.Object when type == typeof(List<>) && attr.Version <= CacheVersion.Halo2Vista:
					return 8;
				case TypeCode.Object when type == typeof(RealRgbColor):
				case TypeCode.Object when type == typeof(RealEulerAngles3d):
				case TypeCode.Object when type == typeof(RealPoint3d):
				case TypeCode.Object when type == typeof(RealVector3d):
				case TypeCode.Object when type == typeof(RealPlane2d):
				case TypeCode.Object when type == typeof(List<>) && attr.Version > CacheVersion.Halo2Vista:
					return 12;
				case TypeCode.Decimal:
				case TypeCode.Object when type == typeof(CachedTagInstance) && attr.Version > CacheVersion.Halo2Vista:
				case TypeCode.Object when type == typeof(RealArgbColor):
				case TypeCode.Object when type == typeof(RealQuaternion):
				case TypeCode.Object when type == typeof(RealPlane3d):
					return 16;
				#endregion

				case TypeCode.Object when type == typeof(Byte[]) && attr.Version > CacheVersion.Halo2Vista:
					return 20;

				case TypeCode.Object when type == typeof(RealMatrix4x3):
					return 48;

				case TypeCode.String:
				case TypeCode.Object when type.IsArray:
					return (uint)attr.Length;

				case TypeCode.Object when type == typeof(Bounds<>):
					return TagFieldInfo.GetFieldSize(type.GenericTypeArguments[0], attr) * 2;

				// Assume the field is a structure
				default:
					return new TagStructureInfo(type, attr.Version).TotalSize;
			}
		}
	}
}