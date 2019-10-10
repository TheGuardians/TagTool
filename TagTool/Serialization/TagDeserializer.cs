using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Shaders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;
using BindingFlags = System.Reflection.BindingFlags;
using System.IO;

namespace TagTool.Serialization
{
    /// <summary>
    /// Deserializes tag data into objects by     
	/// </summary>
    public class TagDeserializer
    {
        public CacheVersion Version { get; }

        /// <summary>
        /// Constructs a tag deserializer for a specific engine version.
        /// </summary>
        /// <param name="version">The engine version to target.</param>
        public TagDeserializer(CacheVersion version)
        {
            Version = version;
        }

        /// <summary>
        /// Deserializes tag data into an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the tag data as.</typeparam>
        /// <param name="context">The serialization context to use.</param>
        /// <returns>The object that was read.</returns>
        public T Deserialize<T>(ISerializationContext context)
        {
            var result = Deserialize(context, typeof(T));
            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Deserializes tag data into an object.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="structureType">The type of object to deserialize the tag data as.</param>
        /// <returns>The object that was read.</returns>
        public object Deserialize(ISerializationContext context, Type structureType)
        {
			var info = TagStructure.GetTagStructureInfo(structureType, Version);
			var reader = context.BeginDeserialize(info);
            var result = DeserializeStruct(reader, context, info);
            context.EndDeserialize(info, result);
            return result;
        }

        /// <summary>
        /// Deserializes a structure.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="info">Information about the structure to deserialize.</param>
        /// <returns>The deserialized structure.</returns>
        /// <exception cref="System.InvalidOperationException">Target type must have TagStructureAttribute</exception>
        public object DeserializeStruct(EndianReader reader, ISerializationContext context, TagStructureInfo info)
        {
            var baseOffset = reader.BaseStream.Position;
            var instance = Activator.CreateInstance(info.Types[0]);

			foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(info.Types[0], info.Version))
				DeserializeProperty(reader, context, instance, tagFieldInfo, baseOffset);

			if (info.TotalSize > 0)
					reader.BaseStream.Position = baseOffset + info.TotalSize;

            return instance;
        }

        /// <summary>
        /// Deserializes a property of a structure.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="instance">The instance to store the property to.</param>
        /// <param name="tagFieldInfo">The active element enumerator.</param>
        /// <param name="baseOffset">The offset of the start of the structure.</param>
        /// <exception cref="System.InvalidOperationException">Offset for property is outside of its structure</exception>
        public void DeserializeProperty(EndianReader reader, ISerializationContext context, object instance, TagFieldInfo tagFieldInfo, long baseOffset)
        {
            var attr = tagFieldInfo.Attribute;

            if (attr.Flags.HasFlag(Runtime))
                return;

            if (tagFieldInfo.FieldType.IsArray && attr.Flags.HasFlag(Relative))
            {
                var type = instance.GetType();
                var field = type.GetField(
                    attr.Field,
                    BindingFlags.Instance | BindingFlags.Public);

                var attr2 = TagStructure.GetTagFieldAttribute(type, field);
                if(CacheVersionDetection.AttributeInCacheVersion(attr2, Version))
                {
                    attr.Length = (int)Convert.ChangeType(field.GetValue(instance), typeof(int));
                }
                else throw new InvalidOperationException(attr2.Field);
            }

            if (attr.Flags.HasFlag(Padding))
            {
#if DEBUG
                foreach (var b in reader.ReadBytes(attr.Length))
                {
                    if (b != 0)
                    {
                        Console.WriteLine($"WARNING: non-zero padding found in {tagFieldInfo.FieldInfo.DeclaringType.FullName}.{tagFieldInfo.FieldInfo.Name}");
                        break;
                    }
                }
#else
                reader.BaseStream.Position += attr.Length;
#endif
            }
            else if (CacheVersionDetection.AttributeInCacheVersion(attr, Version))
            {
                var value = DeserializeValue(reader, context, attr, tagFieldInfo.FieldType);
                tagFieldInfo.SetValue(instance, value);
            }
        }

        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueInfo">The value information. Can be <c>null</c>.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        public object DeserializeValue(EndianReader reader, ISerializationContext context, TagFieldAttribute valueInfo, Type valueType)
        {
            if (valueType.IsPrimitive)
                return DeserializePrimitiveValue(reader, valueType);
            return DeserializeComplexValue(reader, context, valueInfo, valueType);
        }

        /// <summary>
        /// Deserializes a primitive value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        /// <exception cref="System.ArgumentException">Unsupported type</exception>
        public object DeserializePrimitiveValue(EndianReader reader, Type valueType)
        {
            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.Single:
                    return reader.ReadSingle();
                case TypeCode.Byte:
                    return reader.ReadByte();
                case TypeCode.Int16:
                    return reader.ReadInt16();
                case TypeCode.Int32:
                    return reader.ReadInt32();
                case TypeCode.Int64:
                    return reader.ReadInt64();
                case TypeCode.SByte:
                    return reader.ReadSByte();
                case TypeCode.UInt16:
                    return reader.ReadUInt16();
                case TypeCode.UInt32:
                    return reader.ReadUInt32();
                case TypeCode.UInt64:
                    return reader.ReadUInt64();
                case TypeCode.Boolean:
                    return reader.ReadBoolean();
                case TypeCode.Double:
                    return reader.ReadDouble();
                default:
                    throw new ArgumentException("Unsupported type " + valueType.Name);
            }
        }

        /// <summary>
        /// Deserializes a complex value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueInfo">The value information. Can be <c>null</c>.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        public object DeserializeComplexValue(EndianReader reader, ISerializationContext context, TagFieldAttribute valueInfo, Type valueType)
        {
            // Indirect objects
            // TODO: Remove ResourceReference hax, the Indirect flag wasn't available when I generated the tag structures
            if (valueInfo != null && valueInfo.Flags.HasFlag(Pointer))
                return DeserializeIndirectValue(reader, context, valueType);

            var compression = TagFieldCompression.None;

            if (valueInfo != null && valueInfo.Compression != TagFieldCompression.None)
                compression = valueInfo.Compression;

            // enum = Enum type
            if (valueType.IsEnum)
                return DeserializePrimitiveValue(reader, valueType.GetEnumUnderlyingType());

            // string = ASCII string
            if (valueType == typeof(string))
                return DeserializeString(reader, valueInfo);

            if (valueType == typeof(Tag))
                return new Tag(reader.ReadInt32());

            // TagInstance = Tag reference
            if (valueType == typeof(CachedTagInstance))
                return DeserializeTagReference(reader, context, valueInfo);

            // ResourceAddress = Resource address
            if (valueType == typeof(CacheAddress))
                return new CacheAddress(reader.ReadUInt32());

            // Byte array = Data reference
            // TODO: Allow other types to be in data references, since sometimes they can point to a structure
            if (valueType == typeof(byte[]) && valueInfo.Length == 0)
                return DeserializeDataReference(reader, context);

            // Color types
            if (valueType == typeof(RealRgbColor))
                return new RealRgbColor(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            else if (valueType == typeof(RealArgbColor))
                return new RealArgbColor(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            else if (valueType == typeof(ArgbColor))
                return new ArgbColor(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());

            if (valueType == typeof(Point2d))
                return new Point2d(reader.ReadInt16(), reader.ReadInt16());
            if (valueType == typeof(Rectangle2d))
                return new Rectangle2d(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());

            if (valueType == typeof(RealBoundingBox))
                return new RealBoundingBox(
                    reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle());

            if (valueType == typeof(RealEulerAngles2d))
            {
                var i = Angle.FromRadians(reader.ReadSingle(compression));
                var j = Angle.FromRadians(reader.ReadSingle(compression));
                return new RealEulerAngles2d(i, j);
            }
            else if (valueType == typeof(RealEulerAngles3d))
            {
                var i = Angle.FromRadians(reader.ReadSingle(compression));
                var j = Angle.FromRadians(reader.ReadSingle(compression));
                var k = Angle.FromRadians(reader.ReadSingle(compression));
                return new RealEulerAngles3d(i, j, k);
            }

            if (valueType == typeof(RealPoint2d))
                return new RealPoint2d(reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealPoint3d))
                return new RealPoint3d(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealVector2d))
                return new RealVector2d(reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealVector3d))
                return new RealVector3d(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealQuaternion))
                return new RealQuaternion(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealPlane2d))
                return new RealPlane2d(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealPlane3d))
                return new RealPlane3d(reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));
            if (valueType == typeof(RealMatrix4x3))
                return new RealMatrix4x3(
                    reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression),
                    reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression),
                    reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression),
                    reader.ReadSingle(compression), reader.ReadSingle(compression), reader.ReadSingle(compression));

            // StringID
            if (valueType == typeof(StringId))
                return new StringId(reader.ReadUInt32(), Version);

            // Angle (radians)
            if (valueType == typeof(Angle))
                return Angle.FromRadians(reader.ReadSingle(compression));

            if (valueType == typeof(DatumIndex))
                return new DatumIndex(reader.ReadUInt32());

            // Non-byte array = Inline array
            // TODO: Define more clearly in general what constitutes a data reference and what doesn't
            if (valueType.IsArray)
                return DeserializeInlineArray(reader, context, valueInfo, valueType);

            // List = Tag block
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                return DeserializeTagBlock(reader, context, valueType);

            // Ranges
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Bounds<>))
                return DeserializeRange(reader, context, valueType);

			if (valueType == typeof(VertexShaderReference))
				return DeserializeVertexShaderReference(reader, context);

			if (valueType == typeof(PixelShaderReference))
                return DeserializePixelShaderReference(reader, context);

            // Assume the value is a structure
            return DeserializeStruct(reader, context, TagStructure.GetTagStructureInfo(valueType, Version));
        }

        /// <summary>
        /// Deserializes a tag block (list of values).
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized tag block.</returns>
        public object DeserializeTagBlock(EndianReader reader, ISerializationContext context, Type valueType)
        {
            var result = Activator.CreateInstance(valueType);
            var elementType = valueType.GenericTypeArguments[0];

            TagStructureAttribute structure;

            try
            {
                structure = TagStructure.GetTagStructureInfo(elementType, Version).Structure;
            }
            catch
            {
                structure = null;
            }

            // Read count and offset
            var startOffset = reader.BaseStream.Position;
            var count = reader.ReadInt32();
            
            var pointer = new CacheAddress(reader.ReadUInt32());
            if (count == 0 || pointer.Value == 0)
            {
                // Null tag block
                reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0xC : 0x8);
                return result;
            }

            //
            // Read each value
            //

            var addMethod = valueType.GetMethod("Add");

            reader.BaseStream.Position = context.AddressToOffset((uint)startOffset + 4, pointer.Value);

            for (var i = 0; i < count; i++)
            {
                var element = DeserializeValue(reader, context, null, elementType);
                addMethod.Invoke(result, new[] { element });
            }

            reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0xC : 0x8);

            return result;
        }

        /// <summary>
        /// Deserializes a value which is pointed to by an address.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        public object DeserializeIndirectValue(EndianReader reader, ISerializationContext context, Type valueType)
        {
            // Read the pointer
            var pointer = reader.ReadUInt32();
            if (pointer == 0)
                return null; // Null object

            // Seek to it and read the object
            var nextOffset = reader.BaseStream.Position;
            reader.BaseStream.Position = context.AddressToOffset((uint)nextOffset - 4, pointer);
            var result = DeserializeValue(reader, context, null, valueType);
            reader.BaseStream.Position = nextOffset;
            return result;
        }

        /// <summary>
        /// Deserializes a tag reference.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueInfo">The value information. Can be <c>null</c>.</param>
        /// <returns>The deserialized tag reference.</returns>
        public CachedTagInstance DeserializeTagReference(EndianReader reader, ISerializationContext context, TagFieldAttribute valueInfo)
        {
            if (valueInfo == null || !valueInfo.Flags.HasFlag(Short))
                reader.BaseStream.Position += (Version > CacheVersion.Halo2Vista ? 0xC : 0x4); // Skip the class name and zero bytes, it's not important
            
            var result = context.GetTagByIndex(reader.ReadInt32());

            if (result != null && valueInfo != null && valueInfo.ValidTags != null)
                foreach (string tag in valueInfo.ValidTags)
                    if (!result.IsInGroup(tag))
                        throw new Exception($"Invalid group for tag reference: {result.Group.Tag}");

            return result;
        }

        /// <summary>
        /// Deserializes a data reference.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <returns>The deserialized data reference.</returns>
        public byte[] DeserializeDataReference(EndianReader reader, ISerializationContext context)
        {
            // Read size and pointer
            var startOffset = reader.BaseStream.Position;
            var size = reader.ReadInt32();
            if (Version > CacheVersion.Halo2Vista)
                reader.BaseStream.Position = startOffset + 0xC;
            var pointer = reader.ReadUInt32();
            if (pointer == 0)
            {
                // Null data reference
                reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
                return new byte[0];
            }

            // Read the data
            var result = new byte[size];
            reader.BaseStream.Position = context.AddressToOffset((uint)(startOffset + (Version > CacheVersion.Halo2Vista ? 0xC : 0x4)), pointer);
            reader.Read(result, 0, size);
            reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
            return result;
        }

        /// <summary>
        /// Deserializes an inline array.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueInfo">The value information. Can be <c>null</c>.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized array.</returns>
        public Array DeserializeInlineArray(EndianReader reader, ISerializationContext context, TagFieldAttribute valueInfo, Type valueType)
        {
            // Create the array and read the elements in order
            var elementCount = valueInfo.Length;
            var elementType = valueType.GetElementType();
            var result = Array.CreateInstance(elementType, elementCount);
            for (var i = 0; i < elementCount; i++)
                result.SetValue(DeserializeValue(reader, context, null, elementType), i);
            return result;
        }

        /// <summary>
        /// Deserializes a null-terminated ASCII string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="valueInfo">The value information.</param>
        /// <returns>The deserialized string.</returns>
        public string DeserializeString(EndianReader reader, TagFieldAttribute valueInfo)
        {
            if (valueInfo == null || valueInfo.Length == 0)
                throw new ArgumentException("Cannot deserialize a string with no length set");

            switch (valueInfo.CharSet)
            {
                case CharSet.Ansi:
                case CharSet.Unicode:
                    return reader.ReadNullTerminatedString(valueInfo.Length, valueInfo.CharSet);
                default:
                    throw new NotSupportedException($"{valueInfo.CharSet}");
            }
        }

        /// <summary>
        /// Deserializes a <see cref="Bounds{T}"/> value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="rangeType">The range's type.</param>
        /// <returns>The deserialized range.</returns>
        public object DeserializeRange(EndianReader reader, ISerializationContext context, Type rangeType)
        {
            var boundsType = rangeType.GenericTypeArguments[0];
            var min = DeserializeValue(reader, context, null, boundsType);
            var max = DeserializeValue(reader, context, null, boundsType);
            return Activator.CreateInstance(rangeType, min, max);
        }

		public PixelShaderReference DeserializePixelShaderReference(EndianReader reader, ISerializationContext context)
		{
			// This reference is a uint32 pointer, we'll be moving the stream position around. Right before returning
			// from this method 'reader.SeekTo(endPosition)' will bring us to where the serializer expects the next
			// bit of data to be.
			var endPosition = reader.BaseStream.Position + 0x04;

			var headerAddress = reader.ReadUInt32();

			if (headerAddress < 1)
				return null;

			var headerOffset = context.AddressToOffset((uint)(reader.BaseStream.Position - 4), headerAddress);
			reader.SeekTo(headerOffset);

			var header = (PixelShaderHeader)DeserializeStruct(reader, context, TagStructure.GetTagStructureInfo(typeof(PixelShaderHeader)));

			if (header.ShaderDataAddress == 0)
				return null;

			var debugHeaderOffset = reader.Position;
			var debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, TagStructure.GetTagStructureInfo(typeof(ShaderDebugHeader)));

			if ((debugHeader.Magic >> 16) != 0x102A)
				return null;

			if (debugHeader.StructureSize == 0)
				return null;

			reader.SeekTo(debugHeaderOffset);
			var debugData = reader.ReadBytes((int)debugHeader.StructureSize);

			var updbName = "";

			if (debugHeader.UpdbPointerOffset != 0)
			{
				reader.SeekTo(debugHeaderOffset + (long)debugHeader.UpdbPointerOffset);
				var updbNameLength = reader.ReadUInt64();

				if (updbNameLength > 0)
					updbName = new string(reader.ReadChars((int)updbNameLength));
			}

			var totalSize = debugHeader.ShaderDataSize;
			var constantSize = 0U;
			var codeSize = totalSize;

			if (debugHeader.CodeHeaderOffset != 0)
			{
				reader.SeekTo(debugHeaderOffset + debugHeader.CodeHeaderOffset);
				constantSize = reader.ReadUInt32();
				codeSize = reader.ReadUInt32();
            }

            var constant_block_offset = context.AddressToOffset(headerOffset + 0x10, header.ShaderDataAddress);
            reader.SeekTo(constant_block_offset);
            var constantData = reader.ReadBytes((int)constantSize);

            var shader_data_block_offset = constant_block_offset + constantSize;
            reader.SeekTo(shader_data_block_offset);
            var shaderData = reader.ReadBytes((int)codeSize);

            reader.SeekTo(endPosition);

            var info = new XboxShaderInfo
            {
                DataAddress = shader_data_block_offset,
                DebugInfoOffset = (uint)debugHeaderOffset,
                DebugInfoSize = debugHeader.StructureSize,
                DatabasePath = updbName,
                DataSize = totalSize,
                ConstantDataSize = constantSize,
                CodeDataSize = codeSize
            };

            return new PixelShaderReference
            {
                Info = info,
                UpdbName = updbName,
                Header = header,
                DebugHeader = debugHeader,
                DebugData = debugData,
                ShaderData = shaderData,
                ConstantData = constantData
            };
        }

		public VertexShaderReference DeserializeVertexShaderReference(EndianReader reader, ISerializationContext context)
		{
			// This reference is a uint32 pointer, we'll be moving the stream position around. Right before returning
			// from this method 'reader.SeekTo(endPosition)' will bring us to where the serializer expects the next
			// bit of data to be.
			var endPosition = reader.BaseStream.Position + 0x04;

			var headerAddress = reader.ReadUInt32();

			if (headerAddress < 1)
				return null;

			var headerOffset = context.AddressToOffset((uint)(reader.BaseStream.Position - 4), headerAddress);
			reader.SeekTo(headerOffset);

			var header = (VertexShaderHeader)DeserializeStruct(reader, context, TagStructure.GetTagStructureInfo(typeof(VertexShaderHeader)));

			if (header.ShaderDataAddress == 0)
				return null;

			var debugHeaderOffset = reader.Position;
			var debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, TagStructure.GetTagStructureInfo(typeof(ShaderDebugHeader)));

			if ((debugHeader.Magic >> 16) != 0x102A)
				return null;

			if (debugHeader.StructureSize == 0)
				return null;

			reader.SeekTo(debugHeaderOffset);
			var debugData = reader.ReadBytes((int)debugHeader.StructureSize);

			var updbName = "";

			if (debugHeader.UpdbPointerOffset != 0)
			{
				reader.SeekTo(debugHeaderOffset + (long)debugHeader.UpdbPointerOffset);
				var updbNameLength = reader.ReadUInt64();

				if (updbNameLength > 0)
					updbName = new string(reader.ReadChars((int)updbNameLength));
			}

			var totalSize = debugHeader.ShaderDataSize;
			var constantSize = 0U;
			var codeSize = totalSize;

			if (debugHeader.CodeHeaderOffset != 0)
			{
				reader.SeekTo(debugHeaderOffset + debugHeader.CodeHeaderOffset);
				constantSize = reader.ReadUInt32();
				codeSize = reader.ReadUInt32();
            }

            var constant_block_offset = context.AddressToOffset(headerOffset + 0x10, header.ShaderDataAddress);
            reader.SeekTo(constant_block_offset);
            var constantData = reader.ReadBytes((int)constantSize);

            var shader_data_block_offset = constant_block_offset + constantSize;
            reader.SeekTo(shader_data_block_offset);
            var shaderData = reader.ReadBytes((int)codeSize);

            reader.SeekTo(endPosition);

            var info = new XboxShaderInfo
            {
                DataAddress = shader_data_block_offset,
                DebugInfoOffset = (uint)debugHeaderOffset,
                DebugInfoSize = debugHeader.StructureSize,
                DatabasePath = updbName,
                DataSize = totalSize,
                ConstantDataSize = constantSize,
                CodeDataSize = codeSize
            };

            return new VertexShaderReference
            {
                Info = info,
                UpdbName = updbName,
                Header = header,
                DebugHeader = debugHeader,
                DebugData = debugData,
                ShaderData = shaderData,
                ConstantData = constantData
            };
        }
	}
}
