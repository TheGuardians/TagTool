using System;

namespace TagTool.HyperSerialization
{
	class BlamTypeAttribute : Attribute
	{
		public BlamType BlamType = BlamType.Unknown;

		public BlamTypeAttribute(BlamType fieldType = BlamType.Unknown)
		{
			this.BlamType = fieldType;
		}
	}

	enum BlamType
	{
		Unknown = -1,

		// Generic Blam-structure (needs to be TypeCode.Object so we can check if a type has the `BlamTypeAttribute`
		// and then if it doesn't just use the TypeCode._.
		// `field.FieldType.GetCustomAttributes<BlamTypeAttribute>(true).First();`
		Structure = TypeCode.Object,

		// SystemTypes
		Empty = TypeCode.Empty,
		DBNull = TypeCode.DBNull,
		Boolean = TypeCode.Boolean,
		Char = TypeCode.Char,
		SByte = TypeCode.SByte,
		Byte = TypeCode.Byte,
		Int16 = TypeCode.Int16,
		UInt16 = TypeCode.UInt16,
		Int32 = TypeCode.Int32,
		UInt32 = TypeCode.UInt32,
		Int64 = TypeCode.Int64,
		UInt64 = TypeCode.UInt64,
		Single = TypeCode.Single,
		Double = TypeCode.Double,
		Decimal = TypeCode.Decimal,
		DateTime = TypeCode.DateTime,
		String = TypeCode.String,

		// Common Blam Types
		Pointer,
		Tag,
		CacheAddress,
		CachedTagInstance,
		ArgbColor,
		Point2d,
		StringId,
		Angle,
		VertexShaderReference,
		PixelShaderReference,
		ByteArray,
		Rectangle2d,
		RealEulerAngles2d,
		RealPoint2d,
		RealVector2d,
		RealRgbColor,
		RealEulerAngles3d,
		RealPoint3d,
		RealVector3d,
		RealPlane2d,
		RealArgbColor,
		RealQuaternion,
		RealPlane3d,
		RealMatrix4x3,
	}
}
