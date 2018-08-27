using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Shaders;
using TagTool.Serialization;

namespace TagTool.HyperSerialization
{
	class HyperDeserializer
	{
		private Task DeserializeBranch(ISerializationContext context, long offset, Stream stream, CacheVersion cache_version, Type structure_type, TagStructureInfo structure_info, object root_parent, FieldInfo field_info, int? list_index)
		{
			var new_stream = new StreamMultiplxer(stream);
			new_stream.Position = offset;//todo get structure size

			//todo something smarter
			Task task = new Task(() => {
				Deserialize(context, new_stream, cache_version, structure_type, structure_info, root_parent, field_info, list_index);
			});
			task.Start();
			return task;
		}

		public object Deserialize(ISerializationContext context, Stream stream, CacheVersion cache_version, Type structure_type, TagStructureInfo structure_info, object root_parent, FieldInfo field_info, int? list_index)
		{
			var parent = Activator.CreateInstance(structure_type);

			var structureAttribute = structure_type.GetCustomAttributes<TagStructureAttribute>()
				.Where(attr => attr.MinVersion >= cache_version && attr.MaxVersion <= cache_version).FirstOrDefault();

			if (structureAttribute != null)
			{
			}

			var tasks = new List<Task>();
			using (EndianReader reader = new EndianReader(stream))
			{
				// Ensure that fields are in declaration order - GetFields does NOT guarantee this!
				var fields = structure_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken);
				foreach (var field in fields)
				{
					// Get some standard info about each field.
					var fieldAttribute = field.GetCustomAttributes<TagFieldAttribute>(true).FirstOrDefault();
					if (fieldAttribute != null)
					{
						if (fieldAttribute.MinVersion < cache_version) continue;
						if (fieldAttribute.MaxVersion > cache_version) continue;
					}

					var fieldType = field.FieldType;
					var genericType = field.FieldType.IsGenericType ? field.FieldType.GetGenericTypeDefinition() : null;
					var genericTypeArgument = field.FieldType.IsGenericType ? field.FieldType.GenericTypeArguments[0] : null;

					if (fieldType.IsPrimitive)
					{
						field.SetValue(parent, DeserializePrimitive(fieldType, reader));
					}

					else if (fieldType.IsDefined(typeof(BlamTypeAttribute)))
					{
						var value = Activator.CreateInstance(fieldType);
						switch (value)
						{
							case TypeCode.Object when fieldAttribute != null && fieldAttribute.Pointer:
								// Read the pointer
								var pointer = reader.ReadUInt32();
								if (pointer == 0)
									{ value = null; break; }// Null object

								// Seek to it and read the object
								var nextOffset = reader.BaseStream.Position;
								reader.BaseStream.Position = context.AddressToOffset((uint)nextOffset - 4, pointer);
								value = Deserialize(context, stream, cache_version, structure_type, structure_info, root_parent, field_info, list_index);
								reader.BaseStream.Position = nextOffset;
								break;

							case Object obj when fieldType.IsEnum:
								fieldType = field.FieldType.GetEnumUnderlyingType();
								field.SetValue(parent, DeserializePrimitive(fieldType, reader));
								break;

							case String stringVal:
								if (fieldAttribute == null || fieldAttribute.Length == 0)
									throw new ArgumentException("Cannot deserialize a string with no length set");
								else if (fieldAttribute.CharSet == CharSet.Ansi || fieldAttribute.CharSet == CharSet.Unicode)
									stringVal = reader.ReadNullTerminatedString(fieldAttribute.Length, fieldAttribute.CharSet);
								else
									throw new NotSupportedException($"{fieldAttribute.CharSet}");
								break;

							case Tag tag:
								tag = new Tag(reader.ReadInt32());
								break;

							case CachedTagInstance cachedTagInstance:
								if (fieldAttribute == null || !fieldAttribute.Short)
									reader.BaseStream.Position += (cache_version > CacheVersion.Halo2Vista ? 0xC : 0x4); // Skip the class name and zero bytes, it's not important
								cachedTagInstance = context.GetTagByIndex(reader.ReadInt32());
								if (cachedTagInstance != null && fieldAttribute != null && fieldAttribute.ValidTags != null)
									foreach (string tag in fieldAttribute.ValidTags)
										if (!cachedTagInstance.IsInGroup(tag))
											throw new Exception($"Invalid group for tag reference: {cachedTagInstance.Group.Tag}");
								break;

							case CacheAddress cacheAddress:
								cacheAddress = new CacheAddress(reader.ReadUInt32());
								break;

							case Byte[] byteArray:
								// Read size and pointer
								var startOffset = reader.BaseStream.Position;
								var size = reader.ReadInt32();
								if (cache_version > CacheVersion.Halo2Vista)
									reader.BaseStream.Position = startOffset + 0xC;
								pointer = reader.ReadUInt32();
								if (pointer == 0)
								{
									// Null data reference
									reader.BaseStream.Position = startOffset + (cache_version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
									value = new byte[0];
									break;
								}
								// Read the data
								var result = new byte[size];
								reader.BaseStream.Position = context.AddressToOffset((uint)(startOffset + (cache_version > CacheVersion.Halo2Vista ? 0xC : 0x4)), pointer);
								reader.Read(result, 0, size);
								reader.BaseStream.Position = startOffset + (cache_version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
								break;

							case RealRgbColor realRgbColor:
								realRgbColor = new RealRgbColor(
									reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
								break;

							case RealArgbColor realArgbColor:
								realArgbColor = new RealArgbColor(
									reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle());
								break;

							case ArgbColor argbColor:
								argbColor = new ArgbColor(
									reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
								break;

							case Point2d point2D:
								point2D = new Point2d(reader.ReadInt16(), reader.ReadInt16());
								break;

							case Rectangle2d rectangle2D:
								rectangle2D = new Rectangle2d(
									reader.ReadInt16(), reader.ReadInt16(),
									reader.ReadInt16(), reader.ReadInt16());
								break;

							case RealEulerAngles2d realEulerAngles2D:
								realEulerAngles2D = new RealEulerAngles2d(
									Angle.FromRadians(reader.ReadSingle()), Angle.FromRadians(reader.ReadSingle()));
								break;

							case RealEulerAngles3d realEulerAngles3D:
								realEulerAngles3D = new RealEulerAngles3d(
									Angle.FromRadians(reader.ReadSingle()),
									 Angle.FromRadians(reader.ReadSingle()),
									  Angle.FromRadians(reader.ReadSingle()));
								break;

							case RealPoint2d realPoint2D:
								realPoint2D = new RealPoint2d(
									reader.ReadSingle(), reader.ReadSingle());
								break;

							case RealPoint3d realPoint3D:
								realPoint3D.X = reader.ReadSingle();
								realPoint3D.Y = reader.ReadSingle();
								realPoint3D.Z = reader.ReadSingle();
								break;

							case RealVector2d realVector2D:
								realVector2D = new RealVector2d(
									reader.ReadSingle(), reader.ReadSingle());
								break;

							case RealVector3d realVector3D:
								realVector3D.I = reader.ReadSingle();
								realVector3D.J = reader.ReadSingle();
								realVector3D.K = reader.ReadSingle();
								break;

							case RealQuaternion realQuaternion:
								realQuaternion = new RealQuaternion(
									reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle());
								break;

							case RealPlane2d realPlane2D:
								realPlane2D.I = reader.ReadSingle();
								realPlane2D.J = reader.ReadSingle();
								realPlane2D.D = reader.ReadSingle();
								break;

							case RealPlane3d realPlane3D:
								realPlane3D = new RealPlane3d(
									reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle());
								break;

							case RealMatrix4x3 realMatrix4X3:
								realMatrix4X3 = new RealMatrix4x3(
									reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
									reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
								break;

							case StringId stringId:
								stringId = new StringId(reader.ReadUInt32(), cache_version);
								break;

							case Angle angle:
								angle = Angle.FromRadians(reader.ReadSingle());
								break;

							case Array array when fieldType.IsArray:
								if (fieldAttribute == null || fieldAttribute.Length == 0)
									throw new ArgumentException("Cannot deserialize an inline array with no count set");
								var elementCount = fieldAttribute.Length;
								var elementType = fieldType.GetElementType();
								array = Array.CreateInstance(elementType, elementCount);
								for (var i = 0; i < elementCount; i++)
								{
									var val = Deserialize(context, stream, cache_version, elementType, new TagStructureInfo(genericType), null, null, null);
									array.SetValue(val, i);
								}
								break;

							case Object bounds when genericType == typeof(Bounds<>):
								var boundsInfo = genericType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken).ToList();
								boundsInfo[0].SetValue(value, DeserializePrimitive(genericTypeArgument, reader));
								boundsInfo[1].SetValue(value, DeserializePrimitive(genericTypeArgument, reader));
								break;

							case VertexShaderReference vertexShaderReference:
								vertexShaderReference = DeserializeVertexShaderReference(context, reader);
								break;

							case PixelShaderReference pixelShaderReference:
								pixelShaderReference = DeserializePixelShaderReference(context, reader);
								break;

							default:
								throw new ArgumentException($"Unhandled Blam! Type: {fieldType}");
						}

						field.SetValue(parent, value);
					}

					else if (typeof(List<>).IsAssignableFrom(genericType))
					{
						var count = reader.ReadInt32();
						var pointer = reader.ReadUInt32();

						if (cache_version > CacheVersion.Halo2Vista)
							stream.Position += 4;

						var list = (List<dynamic>)Activator.CreateInstance(structure_type, new object[] { count });

						if (genericTypeArgument.IsPrimitive)
							list = (List<dynamic>)DeserializePrimitiveList(genericType, genericTypeArgument, reader, count);

						for (var i = 0; i < count; i++)
						{
							object value;

							if (genericTypeArgument.IsClass)
							{
								value = null;
								long offset = stream.Position + pointer + count;
								Task task = DeserializeBranch(context, offset, stream, cache_version, genericTypeArgument, new TagStructureInfo(genericTypeArgument), list, null, i);
								tasks.Add(task);
							}

							else
							{
								value = Deserialize(context, stream, cache_version, genericTypeArgument, new TagStructureInfo(genericTypeArgument), null, null, null);
								list[i] = value;
							}
						}

						field.SetValue(parent, list);
					}

					else if (fieldType.IsClass)
					{
						structure_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken);
						field.SetValue(parent, null);
						long offset = stream.Position;
						Task task = DeserializeBranch(context, offset, stream, cache_version, fieldType, new TagStructureInfo(genericType), parent, field_info, null);
						tasks.Add(task);
					}

					// assign back to root
					if (root_parent != null)
					{
						if (field_info != null)
						{
							field_info.SetValue(root_parent, parent);
						}

						else if (list_index != null)
						{
							((List<dynamic>)root_parent)[list_index ?? 0] = parent;
						}
						else throw new Exception("Invalid usage");
					}
				}
			}

			// End deserialize.
			Task.WaitAll(tasks.ToArray());
			return parent;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private object DeserializePrimitiveList(Type type, Type element_type, EndianReader reader, int count)
		{
			switch (Type.GetTypeCode(element_type))
			{
				case TypeCode.Boolean:
					var bools = new List<Boolean>(count);
					for (var r = 0; r < count; r++)
						bools[r] = reader.ReadBoolean();
					return bools;

				case TypeCode.SByte:
					var sbytes = new List<SByte>(count);
					for (var r = 0; r < count; r++)
						sbytes[r] = reader.ReadSByte();
					return sbytes;

				case TypeCode.Byte:
					var bytes = new List<Byte>(count);
					for (var r = 0; r < count; r++)
						bytes[r] = reader.ReadByte();
					return bytes;

				case TypeCode.Char:
					var chars = new List<Char>(count);
					for (var r = 0; r < count; r++)
						chars[r] = reader.ReadChar();
					return chars;

				case TypeCode.Int16:
					var int16s = new List<Int16>(count);
					for (var r = 0; r < count; r++)
						int16s[r] = reader.ReadInt16();
					return int16s;

				case TypeCode.UInt16:
					var uint16s = new List<UInt16>(count);
					for (var r = 0; r < count; r++)
						uint16s[r] = reader.ReadUInt16();
					return uint16s;

				case TypeCode.Single:
					var singles = new List<Single>(count);
					for (var r = 0; r < count; r++)
						singles[r] = reader.ReadSingle();
					return singles;

				case TypeCode.Int32:
					var int32s = new List<Int32>(count);
					for (var r = 0; r < count; r++)
						int32s[r] = reader.ReadInt32();
					return int32s;

				case TypeCode.UInt32:
					var uint32s = new List<UInt32>(count);
					for (var r = 0; r < count; r++)
						uint32s[r] = reader.ReadUInt32();
					return uint32s;

				case TypeCode.Double:
					var doubles = new List<Double>(count);
					for (var r = 0; r < count; r++)
						doubles[r] = reader.ReadDouble();
					return doubles;

				case TypeCode.Int64:
					var int64s = new List<Int64>(count);
					for (var r = 0; r < count; r++)
						int64s[r] = reader.ReadInt64();
					return int64s;

				case TypeCode.UInt64:
					var uint64s = new List<UInt64>(count);
					for (var r = 0; r < count; r++)
						uint64s[r] = reader.ReadUInt64();
					return uint64s;

				case TypeCode.Decimal:
					var decimals = new List<Decimal>(count);
					for (var r = 0; r < count; r++)
						decimals[r] = reader.ReadDecimal();
					return decimals;

				default:
					throw new ArgumentException($"Unhandled PrimitiveCollection Type: {type.Name} <{element_type.Name}>");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ValueType DeserializePrimitive(Type primitive_type, EndianReader reader)
		{
			switch (Type.GetTypeCode(primitive_type))
			{
				case TypeCode.Boolean:
					return reader.ReadBoolean();
				case TypeCode.SByte:
					return reader.ReadSByte();
				case TypeCode.Byte:
					return reader.ReadByte();
				case TypeCode.Char:
					return reader.ReadChar();
				case TypeCode.Int16:
					return reader.ReadInt16();
				case TypeCode.UInt16:
					return reader.ReadUInt16();
				case TypeCode.Single:
					return reader.ReadSingle();
				case TypeCode.Int32:
					return reader.ReadInt32();
				case TypeCode.UInt32:
					return reader.ReadUInt32();
				case TypeCode.Double:
					return reader.ReadDouble();
				case TypeCode.Int64:
					return reader.ReadInt64();
				case TypeCode.UInt64:
					return reader.ReadUInt64();
				case TypeCode.Decimal:
					return reader.ReadDecimal();
				default:
					throw new ArgumentException($"Unhandled Primitive Type: {Type.GetTypeCode(primitive_type)}");
			}
		}

		public PixelShaderReference DeserializePixelShaderReference(ISerializationContext context, EndianReader reader)
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

			PixelShaderHeader header = null;
			// header = (PixelShaderHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(PixelShaderHeader)));
			throw new NotImplementedException();

			if (header.ShaderDataAddress == 0)
				return null;

			var debugHeaderOffset = reader.Position;
			ShaderDebugHeader debugHeader = null;
			// debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(ShaderDebugHeader)));
			throw new NotImplementedException();

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

		public VertexShaderReference DeserializeVertexShaderReference(ISerializationContext context, EndianReader reader)
		{
			// This reference is a uint32 pointer, we'll be moving the stream position around. Right before returning
			// from this method 'reader.SeekTo(endPosition)' will bring us to where the serializer expects the next
			// bit of data to be.
			var endPosition = reader.BaseStream.Position + 0x04;

			var headerAddress = reader.ReadUInt32();

			if (headerAddress < 1)
				return null;

			var headerOffset = context.AddressToOffset((uint)(reader.BaseStream.Position - 4), headerAddress);
			reader.SeekTo(headerOffset + 0x4 * sizeof(uint));

			VertexShaderHeader header = null;
			// header = (VertexShaderHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(VertexShaderHeader)));
			throw new NotImplementedException();

			if (header.ShaderDataAddress == 0)
				return null;

			var debugHeaderOffset = reader.Position;
			ShaderDebugHeader debugHeader = null;
			// debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(ShaderDebugHeader)));
			throw new NotImplementedException();

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
