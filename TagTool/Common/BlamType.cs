using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Common
{
	[BlamType]
	public interface IBlamType
	{
        // If we redo all the blam types (RealEulerAngle2d etc) definitions to be assignable directly
        // from an array of primitives, we can handle them in a much cleaner way.
        // void SetValues(ValueType[] values);

        bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error);
    }

	public class BlamTypeAttribute : Attribute
	{
		public BlamType BlamType = BlamType.TagStructure;
		public Type Type = null;
		public Type ElementType = null;
		public int ElementCount = 0;

		public BlamTypeAttribute(BlamType fieldType = BlamType.TagStructure, Type Type = null, Type ElementType = null, int ElementCount = 0)
		{
			BlamType = fieldType;
		}
	}

	public enum BlamType
	{
		// TagStructure
		TagStructure = TypeCode.Object,

		// SystemTypes
		Empty       = TypeCode.Empty,
		DBNull      = TypeCode.DBNull,
		Boolean     = TypeCode.Boolean,
		Char        = TypeCode.Char,
		SByte       = TypeCode.SByte,
		Byte        = TypeCode.Byte,
		Int16       = TypeCode.Int16,
		UInt16      = TypeCode.UInt16,
		Int32       = TypeCode.Int32,
		UInt32      = TypeCode.UInt32,
		Int64       = TypeCode.Int64,
		UInt64      = TypeCode.UInt64,
		Single      = TypeCode.Single,
		Double      = TypeCode.Double,
		Decimal     = TypeCode.Decimal,
		DateTime    = TypeCode.DateTime,
		String      = TypeCode.String,

		// Common Blam Types
		Pointer,
        DatumIndex,
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
