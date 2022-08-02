using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Shaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TagTool.Tags;
using static System.Runtime.InteropServices.CharSet;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Commands.Common;
using TagTool.Geometry.BspCollisionGeometry;

namespace TagTool.Serialization
{
    /// <summary>
    /// Serializes classes into tag data by     /// </summary>
    public class TagSerializer
    {
        public const int DefaultBlockAlign = 4;

        public CacheVersion Version { get; }
        public EndianFormat Format { get; }
        public CachePlatform CachePlatform { get; }

        /// <summary>
        /// Constructs a tag serializer for a specific engine version.
        /// </summary>
        /// <param name="version">The engine version to target.</param>
        /// <param name="cachePlatform">Cache platform of the engine</param>
        /// <param name="format">EndianFormat, default to little endian</param>
        public TagSerializer(CacheVersion version, CachePlatform cachePlatform, EndianFormat format=EndianFormat.LittleEndian)
        {
            Version = version;
            Format = format;
            CachePlatform = cachePlatform;
        }

        /// <summary>
        /// Serializes a tag structure into a context.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStructure">The tag structure.</param>
        /// <param name="offset">An optional offset to begin serializing at.</param>
        public virtual void Serialize(ISerializationContext context, object tagStructure, uint? offset = null)
        {
            // Serialize the structure to a data block
            var info = TagStructure.GetTagStructureInfo(tagStructure.GetType(), Version, CachePlatform);
            context.BeginSerialize(info);
            var tagStream = new MemoryStream();
            var structBlock = context.CreateBlock();
            structBlock.Writer.Format = Format;
            SerializeStruct(context, tagStream, structBlock, info, tagStructure);

            // Finalize the block and write all of the tag data out
            var mainStructOffset = offset ?? structBlock.Finalize(tagStream);
            var data = tagStream.ToArray();
            context.EndSerialize(info, data, mainStructOffset);
        }

        /// <summary>
        /// Serializes a structure into a temporary memory block.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="info">Information about the tag structure type.</param>
        /// <param name="structure">The structure to serialize.</param>
        /// <exception cref="System.InvalidOperationException">Structure type must have TagStructureAttribute</exception>
        public void SerializeStruct(ISerializationContext context, MemoryStream tagStream, IDataBlock block, TagStructureInfo info, object structure)
        {
            var baseOffset = block.Stream.Position;

			foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(info))
				SerializeProperty(context, tagStream, block, structure, tagFieldInfo, baseOffset);

            // Honor the struct size if it's defined
            if (info.TotalSize > 0)
            { 
                block.Stream.Position = baseOffset + info.TotalSize;
                if (block.Stream.Position > block.Stream.Length)
                    block.Stream.SetLength(block.Stream.Position);
            }

            // Honor alignment
            if (info.Structure.Align > 0)
                block.SuggestAlignment(info.Structure.Align);
        }

        /// <summary>
        /// Serializes a property.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="instance">The object that the property belongs to.</param>
        /// <param name="tagFieldInfo">The field enumerator.</param>
        /// <param name="baseOffset">The base offset of the structure from the start of its block.</param>
        /// <exception cref="System.InvalidOperationException">Offset for property \ + property.Name + \ is outside of its structure</exception>
        private void SerializeProperty(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object instance, TagFieldInfo tagFieldInfo, long baseOffset)
        {
            if (tagFieldInfo.Attribute.Flags.HasFlag(Runtime))
                return;

            uint align = TagFieldInfo.GetFieldAlignment(tagFieldInfo.FieldType, tagFieldInfo.Attribute, Version, CachePlatform);
            if (align > 0)
            {
                int fieldOffset = (int)(block.Stream.Position - baseOffset);
                int padding = -fieldOffset & (int)(align - 1);
                if (padding > 0)
                {
                    var paddingBytes = new byte[padding];
                    block.Stream.Write(paddingBytes, 0, paddingBytes.Length);
                }
            }

            object objectValue = tagFieldInfo.GetValue(instance);

            // second condition is a hack to prevent exceptions when encountering cached tags
            if (objectValue != null)
            {
                if (objectValue.GetType() == tagFieldInfo.FieldType || tagFieldInfo.FieldType == typeof(CachedTag))
                {
                    SerializeValue(context, tagStream, block,
                                    objectValue, tagFieldInfo.Attribute, tagFieldInfo.FieldType);
                }
                else
                    throw new Exception($"TagFieldInfo.GetValue return type {objectValue.GetType()} is not the same as the FieldInfo Type {tagFieldInfo.FieldType}!");
            }
            else
            {
                SerializeValue(context, tagStream, block,
                                    objectValue, tagFieldInfo.Attribute, tagFieldInfo.FieldType);
            }
        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="val">The value.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        /// <param name="valueType">Type of the value.</param>
        public void SerializeValue(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object val, TagFieldAttribute valueInfo, Type valueType)
        {
            // Call the data block's PreSerialize callback to determine if the value should be mutated
            val = block.PreSerialize(valueInfo, val);
            if (val != null)
                valueType = val.GetType(); // TODO: Fix hax

            if (valueType.IsPrimitive)
                SerializePrimitiveValue(block.Writer, val, valueType);
            else
                SerializeComplexValue(context, tagStream, block, val, valueInfo, valueType);
        }

        /// <summary>
        /// Serializes a primitive value.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="val">The value.</param>
        /// <param name="valueType">Type of the value.</param>
        private void SerializePrimitiveValue(EndianWriter writer, object val, Type valueType)
        {
            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.Boolean:
                    writer.Write((bool)val);
                    break;
                case TypeCode.Byte:
                    writer.Write((byte)val);
                    break;
                case TypeCode.Double:
                    writer.Write((double)val);
                    break;
                case TypeCode.Int16:
                    writer.Write((short)val);
                    break;
                case TypeCode.Int32:
                    writer.Write((int)val);
                    break;
                case TypeCode.Int64:
                    writer.Write((long)val);
                    break;
                case TypeCode.SByte:
                    writer.Write((sbyte)val);
                    break;
                case TypeCode.Single:
                    writer.Write((float)val);
                    break;
                case TypeCode.UInt16:
                    writer.Write((ushort)val);
                    break;
                case TypeCode.UInt32:
                    writer.Write((uint)val);
                    break;
                case TypeCode.UInt64:
                    writer.Write((ulong)val);
                    break;
                default:
                    throw new ArgumentException("Unsupported type " + valueType.Name);
            }
        }

        /// <summary>
        /// Serializes a complex value.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="value">The value.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        /// <param name="valueType">Type of the value.</param>
        private void SerializeComplexValue(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object value, TagFieldAttribute valueInfo, Type valueType)
        {
            if (valueInfo != null && valueInfo.Flags.HasFlag(Pointer))
                SerializeIndirectValue(context, tagStream, block, value, valueType);
            else if (valueType.IsEnum)
                SerializeEnum(block.Writer, value, valueInfo, valueType);
            else if (valueType == typeof(string))
                SerializeString(block.Writer, (string)value, valueInfo);
            else if (valueType == typeof(Tag))
                SerializeTag(block, (Tag)value);
            else if (valueType.BaseType == typeof(CachedTag) || (valueType == typeof(CachedTag) && value == null))
                SerializeTagReference(context, block, (CachedTag)value, valueInfo);
            else if (valueType == typeof(CacheAddress))
                block.Writer.Write(((CacheAddress)value).Value);
            else if (valueType == typeof(byte[]))
            {
                if (valueInfo.Flags.HasFlag(Padding) || (value == null && valueInfo.Length > 0))
                    block.Writer.Write(new byte[valueInfo.Length]);
                else if (valueInfo.Length > 0)
                    block.Writer.Write((byte[])value);
                else
                    SerializeDataReference(tagStream, block, (byte[])value, valueInfo);
            }
            else if (valueType == typeof(TagData))
                SerializeTagData(context, tagStream, block, (TagData)value, valueInfo);
            else if (valueType == typeof(RealRgbColor))
                SerializeColor(block, (RealRgbColor)value);
            else if (valueType == typeof(RealArgbColor))
                SerializeColor(block, (RealArgbColor)value);
            else if (valueType == typeof(RealRgbaColor))
                SerializeColor(block, (RealRgbaColor)value);
            else if (valueType == typeof(ArgbColor))
                SerializeColor(block, (ArgbColor)value);
            else if (value is RealBoundingBox boundingBox)
            {
                SerializeRange(context, tagStream, block, boundingBox.XBounds);
                SerializeRange(context, tagStream, block, boundingBox.YBounds);
                SerializeRange(context, tagStream, block, boundingBox.ZBounds);
            }
            else if (valueType == typeof(RealEulerAngles2d))
                SerializeEulerAngles(block, (RealEulerAngles2d)value);
            else if (valueType == typeof(RealEulerAngles3d))
                SerializeEulerAngles(block, (RealEulerAngles3d)value);
            else if (valueType == typeof(Point2d))
                SerializePoint(block, (Point2d)value);
            else if (valueType == typeof(Rectangle2d))
                SerializeRectangle2d(block, (Rectangle2d)value);
            else if (valueType == typeof(RealRectangle3d))
                SerializeRealRectangle3d(block, (RealRectangle3d)value);
            else if (valueType == typeof(RealPoint2d))
                SerializePoint(block, (RealPoint2d)value);
            else if (valueType == typeof(RealPoint3d))
                SerializePoint(block, (RealPoint3d)value);
            else if (valueType == typeof(RealVector2d))
                SerializeVector(block, (RealVector2d)value);
            else if (valueType == typeof(RealVector3d))
                SerializeVector(block, (RealVector3d)value);
            else if (valueType == typeof(RealQuaternion))
                SerializeVector(block, (RealQuaternion)value);
            else if (valueType == typeof(RealPlane2d))
                SerializePlane(block, (RealPlane2d)value);
            else if (valueType == typeof(RealPlane3d))
                SerializePlane(block, (RealPlane3d)value);
            else if (valueType == typeof(RealMatrix4x3))
                SerializeMatrix(block, (RealMatrix4x3)value);
            else if (valueType == typeof(StringId))
                block.Writer.Write(((StringId)value).Value);
            else if (valueType == typeof(Angle))
                block.Writer.Write(((Angle)value).Radians);
            else if (valueType == typeof(DatumHandle))
                block.Writer.Write(((DatumHandle)value).Value);
            else if (valueType == typeof(VertexShaderReference))
                block.Writer.Write(0); // TODO: fix  (not used in halo online)
            else if (valueType == typeof(PixelShaderReference))
                block.Writer.Write(0); // TODO: fix  (not used in halo online)
            else if (valueType.IsArray)
                SerializeInlineArray(context, tagStream, block, (Array)value, valueInfo, valueType);
            else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                SerializeTagBlockAsList(context, tagStream, block, value, valueType, valueInfo);
            else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(TagBlock<>))
                SerializeTagBlock(context, tagStream, block, value, valueType, valueInfo);
            else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(D3DStructure<>))
                SerializeD3DStructure(context, tagStream, block, value, valueType);
            else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Bounds<>))
                SerializeRange(context, tagStream, block, value);
            else if (valueType == typeof(PlatformUnsignedValue))
                SerializePlatformUnsignedValue(block, (PlatformUnsignedValue)value);
            else if (valueType == typeof(PlatformSignedValue))
                SerializePlatformSignedValue(block, (PlatformSignedValue)value);
            else if (valueType == typeof(IndexBufferIndex))
                SerializeIndexBufferIndex(block, (IndexBufferIndex)value);
            else if (valueType == typeof(StructureSurfaceToTriangleMapping))
                SerializePlaneReference(block, (StructureSurfaceToTriangleMapping)value);
            else if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(FlagBits<>))
                SerializeFlagBits(block.Writer, (IFlagBits)value, valueInfo, valueType);
            else
            {
                if (value == null)
                    value = Activator.CreateInstance(valueType);

                SerializeStruct(context, tagStream, block, TagStructure.GetTagStructureInfo(valueType, Version, CachePlatform), value);
            }
        }

        private void SerializeFlagBits(EndianWriter writer, IFlagBits value, TagFieldAttribute valueInfo, Type valueType)
        {
            var enumType = valueType.GenericTypeArguments[0];
            uint exportedValue = VersionedEnum.ExportFlags(enumType, value, Version, CachePlatform);
            object castedValue = Convert.ChangeType(exportedValue, valueInfo.EnumType);
            SerializePrimitiveValue(writer, castedValue, valueInfo.EnumType);
        }

        private void SerializeEnum(EndianWriter writer, object value, TagFieldAttribute valueInfo, Type valueType)
        {
            var enumInfo = TagEnum.GetInfo(valueType, Version, CachePlatform);
            if (enumInfo.Attribute.IsVersioned)
                value = ConvertVersionedEnumValue(value, valueInfo, valueType, enumInfo);

            if(valueInfo.EnumType != null)
                value = Convert.ChangeType(value, valueInfo.EnumType);

            SerializePrimitiveValue(writer, value, value.GetType());
        }

        private object ConvertVersionedEnumValue(object value, TagFieldAttribute valueInfo, Type valueType, TagEnumInfo enumInfo)
        {
            try
            {
                int exportedValue = VersionedEnum.ExportValue(valueType, value, Version, CachePlatform);
                return Convert.ChangeType(exportedValue, valueInfo.EnumType ?? valueType.GetEnumUnderlyingType());
            }
            catch (ArgumentOutOfRangeException)
            {
                new TagToolWarning($"Enum value out of range {enumInfo.Type.FullName} = {value}");
                return value;
            }
        }

        /// <summary>
        /// Serializes a string.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="str">The string to serialize.</param>
        /// <param name="valueInfo">Information about the value.</param>
        private void SerializeString(EndianWriter writer, string str, TagFieldAttribute valueInfo)
        {
            if (valueInfo == null || valueInfo.Length == 0)
                throw new ArgumentException("Cannot serialize a string with no length set");

            var charSize = valueInfo.CharSet == Unicode ? 2 : 1;
            var byteCount = valueInfo.Length * charSize;
            var clampedLength = 0;

            if (str != null)
            {
                byte[] bytes = null;

                switch (valueInfo.CharSet)
                {
                    case Ansi:
                        bytes = Encoding.ASCII.GetBytes(str);
                        break;

                    case Unicode:
                        if (Format == EndianFormat.LittleEndian)
                            bytes = Encoding.Unicode.GetBytes(str);
                        else
                            bytes = Encoding.BigEndianUnicode.GetBytes(str);
                        break;

                    default:
                        throw new NotSupportedException(valueInfo.CharSet.ToString());
                }

                clampedLength = Math.Min(byteCount - charSize, bytes.Length);

                if (valueInfo.ForceNullTerminated && clampedLength == valueInfo.Length)
                    bytes[valueInfo.Length - 1] = 0;

                writer.Write(bytes, 0, clampedLength);
            }

            for (var i = clampedLength; i < byteCount; i++)
                writer.Write((byte)0);
        }

        private void SerializeTag(IDataBlock block, Tag tag)
        {
            block.Writer.Write(tag.Value);
        }

        /// <summary>
        /// Serializes a tag reference.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="block">The block to write to.</param>
        /// <param name="referencedTag">The referenced tag.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        private void SerializeTagReference(ISerializationContext context, IDataBlock block, CachedTag referencedTag, TagFieldAttribute valueInfo)
        {
            if (referencedTag != null && referencedTag.Group.IsNull())
                referencedTag = null;

            if (referencedTag != null && valueInfo != null && valueInfo.ValidTags != null)
            {
                bool invalid = true;

                foreach (string tag in valueInfo.ValidTags)
                    if (referencedTag.IsInGroup(tag))
                        invalid = false;

                if (invalid)
                    new TagToolWarning($"ERROR: Tag reference with invalid group found during serialization: {referencedTag.Name}.{referencedTag.Group.Tag}");
            }

            block.AddTagReference(referencedTag, valueInfo == null ? false : valueInfo.Flags.HasFlag(Short));

            if (valueInfo == null || !valueInfo.Flags.HasFlag(Short))
            {
                block.Writer.Write((referencedTag != null) ? referencedTag.Group.Tag.Value : -1);
                block.Writer.Write(0);
                block.Writer.Write(0);
            }
            
            block.Writer.Write((referencedTag != null) ? referencedTag.Index : -1);
        }

        /// <summary>
        /// Serializes a data reference composed of raw bytes.
        /// </summary>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="data">The data.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        public void SerializeDataReference(MemoryStream tagStream, IDataBlock block, byte[] data, TagFieldAttribute valueInfo)
        {
            var writer = block.Writer;
            uint offset = 0;
            uint size = 0;
            if (data != null && data.Length > 0)
            {
                // Ensure the block is aligned correctly
                var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.DataAlign : 0);
                StreamUtil.Align(tagStream, (int)align);

                // Write its data
                offset = (uint)tagStream.Position;
                size = (uint)data.Length;
                tagStream.Write(data, 0, data.Length);
                StreamUtil.Align(tagStream, DefaultBlockAlign);
            }

            // Write the reference data
            writer.Write(size);
            writer.Write(0);
            writer.Write(0);
            if (size > 0)
                block.WritePointer(offset, typeof(byte[]));
            else
                writer.Write(0);
            writer.Write(0);
        }

        public virtual void SerializeTagData(ISerializationContext context, MemoryStream tagStream, IDataBlock block, TagData tagData, TagFieldAttribute valueInfo)
        {
            var writer = block.Writer;
            uint offset = 0;
            uint size = 0;
            var data = tagData.Data;

            if (data != null && data.Length > 0)
            {
                // Ensure the block is aligned correctly
                var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.DataAlign : 0);
                StreamUtil.Align(tagStream, (int)align);

                // Write its data
                offset = (uint)tagStream.Position;
                size = (uint)data.Length;
                tagStream.Write(data, 0, data.Length);
                StreamUtil.Align(tagStream, DefaultBlockAlign);
            }

            // Write the reference data
            writer.Write(size);
            writer.Write(0);
            writer.Write(0);
            if (size > 0)
                block.WritePointer(offset, typeof(byte[]));
            else
                writer.Write(0);
            writer.Write(0);
        }

        /// <summary>
        /// Serializes an inline array.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="data">The array.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        /// <param name="valueType">The type of the value.</param>
        private void SerializeInlineArray(ISerializationContext context, MemoryStream tagStream, IDataBlock block, Array data, TagFieldAttribute valueInfo, Type valueType)
        {
            if (valueInfo == null || valueInfo.Length == 0)
                throw new ArgumentException("Cannot serialize an inline array with no count set");

            var elementType = valueType.GetElementType();

            if (data == null)
                data = Array.CreateInstance(elementType, valueInfo.Length);

            if (data == null || data.Length != valueInfo.Length)
                throw new ArgumentException("Array length does not match the fixed count of " + valueInfo.Length);

            // Serialize each element into the current block
            foreach (var element in data)
                SerializeValue(context, tagStream, block, element, null, elementType);
        }

        /// <summary>
        /// Serializes a tag block.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="list">The list of values in the tag block.</param>
        /// <param name="listType">Type of the list.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        public virtual void SerializeTagBlock(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object list, Type listType, TagFieldAttribute valueInfo)
        {
            var writer = block.Writer;
            var count = 0;
            if (list != null)
            {
                // Use reflection to get the number of elements in the list
                var countProperty = listType.GetProperty("Count");
                count = (int)countProperty.GetValue(list);
            }
            if (count == 0)
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                return;
            }

            var elementType = listType.GenericTypeArguments[0];
            
            // Serialize each value in the list to a data block
            var tagBlock = context.CreateBlock();
            var enumerableList = (System.Collections.IEnumerable)list;

            foreach (var val in enumerableList)
                SerializeValue(context, tagStream, tagBlock, val, null, elementType);

            // Ensure the block is aligned correctly
            var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.DataAlign : 0);
            StreamUtil.Align(tagStream, (int)align);

            // Finalize the block and write the tag block reference
            writer.Write(count);
            block.WritePointer(tagBlock.Finalize(tagStream), listType);
            writer.Write(0);
        }

        /// <summary>
        /// Serializes a tag block.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStream">The stream to write completed blocks of tag data to.</param>
        /// <param name="block">The temporary block to write incomplete tag data to.</param>
        /// <param name="list">The list of values in the tag block.</param>
        /// <param name="listType">Type of the list.</param>
        /// <param name="valueInfo">Information about the value. Can be <c>null</c>.</param>
        private void SerializeTagBlockAsList(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object list, Type listType, TagFieldAttribute valueInfo)
        {
            var writer = block.Writer;
            var count = 0;
            if (list != null)
            {
                // Use reflection to get the number of elements in the list
                var countProperty = listType.GetProperty("Count");
                count = (int)countProperty.GetValue(list);
            }
            if (count == 0)
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                return;
            }

            var elementType = listType.GenericTypeArguments[0];

            // Serialize each value in the list to a data block
            var tagBlock = context.CreateBlock();
            var enumerableList = (System.Collections.IEnumerable)list;

            foreach (var val in enumerableList)
                SerializeValue(context, tagStream, tagBlock, val, null, elementType);

            // Ensure the block is aligned correctly
            var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.DataAlign : 0);
            StreamUtil.Align(tagStream, (int)align);

            // Finalize the block and write the tag block reference
            writer.Write(count);
            block.WritePointer(tagBlock.Finalize(tagStream), listType);
            writer.Write(0);
        }

        private void SerializeIndirectValue(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object val, Type valueType)
        {
            var writer = block.Writer;
            if (val == null)
            {
                writer.Write(0);
                return;
            }

            // Serialize the value to a temporary block
            var valueBlock = context.CreateBlock();
            SerializeValue(context, tagStream, valueBlock, val, null, valueType);

            // Finalize the block and write the pointer
            block.WritePointer(valueBlock.Finalize(tagStream), valueType);
        }

        public virtual void SerializeD3DStructure(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object val, Type valueType)
        {
            var writer = block.Writer;

            if (val == null)
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                return;
            }

            // Serialize the value to a temporary block
            var valueBlock = context.CreateBlock();
            SerializeValue(context, tagStream, valueBlock, val, null, valueType);

            // Finalize the block and write the pointer
            block.WritePointer(valueBlock.Finalize(tagStream), valueType);
            writer.Write(0);
            writer.Write(0);
        }

        public virtual void SerializePlatformUnsignedValue(IDataBlock block, PlatformUnsignedValue value)
        {
            var platformType = CacheVersionDetection.GetPlatformType(CachePlatform);

            var writer = block.Writer;
            switch (platformType)
            {
                case PlatformType._32Bit:
                    writer.Write(value?.Get32BitValue() ?? 0);
                    break;

                case PlatformType._64Bit:
                    writer.Write(value?.Get64BitValue() ?? 0);
                    break;
            }
        }

        public virtual void SerializePlatformSignedValue(IDataBlock block, PlatformSignedValue value)
        {
            var platformType = CacheVersionDetection.GetPlatformType(CachePlatform);

            var writer = block.Writer;
            switch (platformType)
            {
                case PlatformType._32Bit:
                    writer.Write(value?.Get32BitValue() ?? 0);
                    break;

                case PlatformType._64Bit:
                    writer.Write(value?.Get64BitValue() ?? 0);
                    break;
            }
        }

        private void SerializeColor(IDataBlock block, RealRgbColor color)
        {
            block.Writer.Write(color.Red);
            block.Writer.Write(color.Green);
            block.Writer.Write(color.Blue);
        }

        private void SerializeColor(IDataBlock block, RealArgbColor color)
        {
            block.Writer.Write(color.Alpha);
            block.Writer.Write(color.Red);
            block.Writer.Write(color.Green);
            block.Writer.Write(color.Blue);
        }

        private void SerializeColor(IDataBlock block, RealRgbaColor color)
        {
            block.Writer.Write(color.Red);
            block.Writer.Write(color.Green);
            block.Writer.Write(color.Blue);
            block.Writer.Write(color.Alpha);
        }

        private void SerializeColor(IDataBlock block, ArgbColor color)
        {
            block.Writer.Write(color.GetValue());
        }

        private void SerializeEulerAngles(IDataBlock block, RealEulerAngles2d angles)
        {
            block.Writer.Write(angles.Yaw.Radians);
            block.Writer.Write(angles.Pitch.Radians);
        }

        private void SerializeEulerAngles(IDataBlock block, RealEulerAngles3d angles)
        {
            block.Writer.Write(angles.Yaw.Radians);
            block.Writer.Write(angles.Pitch.Radians);
            block.Writer.Write(angles.Roll.Radians);
        }

        private void SerializePoint(IDataBlock block, Point2d point)
        {
            block.Writer.Write(point.X);
            block.Writer.Write(point.Y);
        }

        private void SerializeRectangle2d(IDataBlock block, Rectangle2d rect)
        {
            block.Writer.Write(rect.Top);
            block.Writer.Write(rect.Left);
            block.Writer.Write(rect.Bottom);
            block.Writer.Write(rect.Right);
        }

        private void SerializeRealRectangle3d(IDataBlock block, RealRectangle3d rect)
        {
            block.Writer.Write(rect.X0);
            block.Writer.Write(rect.X1);
            block.Writer.Write(rect.Y0);
            block.Writer.Write(rect.Y1);
            block.Writer.Write(rect.Z0);
            block.Writer.Write(rect.Z1);
        }

        private void SerializePoint(IDataBlock block, RealPoint2d point)
        {
            block.Writer.Write(point.X);
            block.Writer.Write(point.Y);
        }

        private void SerializePoint(IDataBlock block, RealPoint3d point)
        {
            block.Writer.Write(point.X);
            block.Writer.Write(point.Y);
            block.Writer.Write(point.Z);
        }

        private void SerializeVector(IDataBlock block, RealVector2d vec)
        {
            block.Writer.Write(vec.I);
            block.Writer.Write(vec.J);
        }

        private void SerializeVector(IDataBlock block, RealVector3d vec)
        {
            block.Writer.Write(vec.I);
            block.Writer.Write(vec.J);
            block.Writer.Write(vec.K);
        }

        private void SerializeVector(IDataBlock block, RealQuaternion vec)
        {
            block.Writer.Write(vec.I);
            block.Writer.Write(vec.J);
            block.Writer.Write(vec.K);
            block.Writer.Write(vec.W);
        }
        
        private void SerializePlane(IDataBlock block, RealPlane2d plane)
        {
            block.Writer.Write(plane.I);
            block.Writer.Write(plane.J);
            block.Writer.Write(plane.D);
        }

        private void SerializePlane(IDataBlock block, RealPlane3d plane)
        {
            block.Writer.Write(plane.I);
            block.Writer.Write(plane.J);
            block.Writer.Write(plane.K);
            block.Writer.Write(plane.D);
        }

        private void SerializeMatrix(IDataBlock block, RealMatrix4x3 mat)
        {
            block.Writer.Write(mat.m11);
            block.Writer.Write(mat.m12);
            block.Writer.Write(mat.m13);
            block.Writer.Write(mat.m21);
            block.Writer.Write(mat.m22);
            block.Writer.Write(mat.m23);
            block.Writer.Write(mat.m31);
            block.Writer.Write(mat.m32);
            block.Writer.Write(mat.m33);
            block.Writer.Write(mat.m41);
            block.Writer.Write(mat.m42);
            block.Writer.Write(mat.m43);
        }

        private void SerializeRange(ISerializationContext context, MemoryStream tagStream, IDataBlock block, object val)
        {
            var type = val.GetType();
            var boundsType = type.GenericTypeArguments[0];
            var lower = type.GetProperty("Lower").GetValue(val);
            var upper = type.GetProperty("Upper").GetValue(val);
            SerializeValue(context, tagStream, block, lower, null, boundsType);
            SerializeValue(context, tagStream, block, upper, null, boundsType);
        }

        private void SerializeIndexBufferIndex(IDataBlock block, IndexBufferIndex val)
        {
            if (Version >= CacheVersion.HaloReach || Version == CacheVersion.HaloOnlineED)
            {
                block.Writer.Write(val.Value);
            }
            else
            {
                if (val > ushort.MaxValue)
                    new TagToolWarning("Downgrade from uint to ushort for index buffer index. Unexpected behavior.");

                block.Writer.Write((ushort)val.Value);
            }
        }

        private void SerializePlaneReference(IDataBlock block, StructureSurfaceToTriangleMapping val)
        {
            if (Version >= CacheVersion.HaloReach || Version == CacheVersion.HaloOnlineED)
            {
                uint value = ((uint)val.TriangleIndex << 12) | ((uint)val.ClusterIndex & 0xFFF);
                block.Writer.Write(value);
            }
            else
            {
                if (val.ClusterIndex > ushort.MaxValue || val.TriangleIndex > ushort.MaxValue)
                    new TagToolWarning("Downgrade from int to short for plane reference. Unexpected behavior.");

                block.Writer.Write((ushort)val.TriangleIndex);
                block.Writer.Write((ushort)val.ClusterIndex);
            }
        }
    }
}
