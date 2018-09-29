using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;

namespace TagTool.Tags
{
	/// <summary>
	/// Class for pairing of <see cref="System.Reflection.FieldInfo"/> and <see cref="TagFieldAttribute"/>.
	/// </summary>
	public class TagFieldInfo
	{
		/// <summary>
		/// Constructs a <see cref="TagFieldInfo"/> from a <see cref="System.Reflection.FieldInfo"/> and <see cref="TagFieldAttribute"/>.
		/// </summary>
		/// <param name="field">The <see cref="System.Reflection.FieldInfo"/> for the field.</param>
		/// <param name="attribute">The <see cref="TagFieldAttribute"/> for the field.</param>
		/// <param name="offset">The offset (in bytes) of the field in it's structure.</param>
		/// <param name="size">The size of the field (in bytes).</param>
		public TagFieldInfo(FieldInfo field, TagFieldAttribute attribute, uint offset, uint size)
		{
			FieldInfo = field;
			Size = size;
			Offset = offset;
			Attribute = attribute;
			SetValue = TagFieldInfo.CreateSetter(this);
			GetValue = TagFieldInfo.CreateGetter(this);
		}

		/// <summary>
		/// Gets the <see cref="System.Reflection.FieldInfo"/> that was used in construction.
		/// </summary>
		public FieldInfo FieldInfo { get; }

		// Expose the FieldInfo's properties more directly.
		public MemberTypes MemberType => FieldInfo.MemberType;
		public string Name => FieldInfo.Name;
		public Type DeclaringType => FieldInfo.DeclaringType;
		public Type ReflectedType => FieldInfo.ReflectedType;
		public int MetadataToken => FieldInfo.MetadataToken;
		public Module Module => FieldInfo.Module;
		public Type FieldType => FieldInfo.FieldType;

		/// <summary>
		/// Gets the field size (in bytes) that was used in construction.
		/// </summary>
		public uint Size { get; }

		/// <summary>
		/// Gets the field offset (in bytes) that was used in construction.
		/// </summary>
		public uint Offset { get; }

		/// <summary>
		/// Gets the <see cref="TagFieldAttribute"/> that was used in construction.
		/// </summary>
		public TagFieldAttribute Attribute { get; }

		/// <summary>
		/// Encapsulates a method for SETTING this field's value on it's owner.
		/// Usage: 'tagFieldInfo.SetValue(object owner, object value);'
		/// </summary>
		public readonly ValueSetter SetValue;

		/// <summary>
		/// Encapsulates a method for GETTING this field's value on it's owner.
		/// Usage: 'var value = tagFieldInfo.GetValue(object owner);'
		/// </summary>
		public readonly ValueGetter GetValue;

		/// <summary>
		/// A <see cref="Delegate"/> for SETTING the value of a field on it's owner.
		/// </summary>
		/// <param name="owner">The <see cref="object"/> that owns the field.</param>
		/// <param name="value">The <see cref="object"/> to SET the value of the field to.</param>
		public delegate void ValueSetter(object owner, object value);

		/// <summary>
		/// A <see cref="Delegate"/> for GETTING the value of a field on it's owner.
		/// </summary>
		/// <param name="owner">The <see cref="object"/> that owns the field.</param>
		/// <returns>The value of the field on it's owner.</returns>
		public delegate object ValueGetter(object owner);

		private static ValueSetter CreateSetter(TagFieldInfo tagFieldInfo)
		{
			var ownerType = tagFieldInfo.DeclaringType;
			var valueType = tagFieldInfo.FieldType;

			if (ownerType.IsGenericTypeDefinition)
				ownerType = ownerType.MakeGenericType(valueType);

			// Parameter "target", the object on which to set the field `field`.
			var ownerParam = Expression.Parameter(typeof(object));

			// Parameter "value" the value to be set in the `field` on "target".
			var valueParam = Expression.Parameter(typeof(object));

			// Unbox structs to their type, or cast a class to it's type.
			var castTartgetExp = ownerType.IsValueType ?
				Expression.Unbox(ownerParam, ownerType) : Expression.Convert(ownerParam, ownerType);

			// Cast the value to its correct type.
			var castValueExp = Expression.Convert(valueParam, valueType);

			// Access the field
			var fieldExp = Expression.Field(castTartgetExp, tagFieldInfo.FieldInfo);

			// Assign the "value" to the `field`.
			var assignExp = Expression.Assign(fieldExp, castValueExp);


			// Compile the whole thing and return.
			var setter = Expression.Lambda<ValueSetter>(assignExp, ownerParam, valueParam).Compile();
			return setter;
		}

		static public ValueGetter CreateGetter(TagFieldInfo tagFieldInfo)
		{
			var ownerType = tagFieldInfo.DeclaringType;

			// Parameter "owner", the object on which to get the field value from.
			ParameterExpression ownerParam = Expression.Parameter(typeof(object));

			// Unbox structs to their type, or cast a class to it's type.
			Expression castTartgetExp = ownerType.IsValueType ?
				Expression.Unbox(ownerParam, ownerType) : Expression.Convert(ownerParam, ownerType);

			// Access the field
			MemberExpression fieldExp = Expression.Field(castTartgetExp, tagFieldInfo.FieldInfo);

			// Convert field to object Type
			UnaryExpression boxedExp = Expression.Convert(fieldExp, typeof(object));

			var getter = Expression.Lambda<ValueGetter>(boxedExp, ownerParam).Compile();
			return getter;
		}

		/// <summary>
		/// Gets the size of a tag-field.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the field.</param>
		/// <param name="attr">The <see cref="TagFieldAttribute"/> of the field.</param>
		/// <returns></returns>
		public static uint GetFieldSize(Type type, TagFieldAttribute attr)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
					return 0x01;

				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.UInt16:
					return 0x02;

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
					return 0x04;

				case TypeCode.Double:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Object when type == typeof(CachedTagInstance) && attr.Version != CacheVersion.Unknown && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista):
				case TypeCode.Object when type == typeof(Byte[]) && attr.Version != CacheVersion.Unknown && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista):
				case TypeCode.Object when type == typeof(Rectangle2d):
				case TypeCode.Object when type == typeof(RealEulerAngles2d):
				case TypeCode.Object when type == typeof(RealPoint2d):
				case TypeCode.Object when type == typeof(RealVector2d):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && attr.Version != CacheVersion.Unknown && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TagBlock<>) && attr.Version != CacheVersion.Unknown && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista):
					return 0x08;

				case TypeCode.Object when type == typeof(RealRgbColor):
				case TypeCode.Object when type == typeof(RealEulerAngles3d):
				case TypeCode.Object when type == typeof(RealPoint3d):
				case TypeCode.Object when type == typeof(RealVector3d):
				case TypeCode.Object when type == typeof(RealPlane2d):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo3Retail, CacheVersion.Unknown):
				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TagBlock<>) && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo3Retail, CacheVersion.Unknown):
					return 0x0C;

				case TypeCode.Decimal:
				case TypeCode.Object when type == typeof(CachedTagInstance) && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo3Retail, CacheVersion.Unknown):
				case TypeCode.Object when type == typeof(RealArgbColor):
				case TypeCode.Object when type == typeof(RealQuaternion):
				case TypeCode.Object when type == typeof(RealPlane3d):
					return 0x10;

				case TypeCode.Object when type == typeof(Byte[]) && CacheVersionDetection.IsBetween(attr.Version, CacheVersion.Halo3Retail, CacheVersion.Unknown):
					return 0x14;

				case TypeCode.Object when type == typeof(RealMatrix4x3):
					return 0x30;

				case TypeCode.String:
				case TypeCode.Object when type.IsArray:
					return (uint)attr.Length;

				case TypeCode.Object when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Bounds<>):
					return TagFieldInfo.GetFieldSize(type.GenericTypeArguments[0], attr) * 2;

				case TypeCode.Object when type.IsEnum:
					return TagFieldInfo.GetFieldSize(type.GetEnumUnderlyingType(), attr);

				// Assume the field is a structure
				default:
					return TagStructure.GetTagStructureInfo(type, attr.Version).TotalSize;
			}
		}
	}
}