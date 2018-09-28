using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Shaders;

namespace TagTool.Serialization
{
    public static class HyperDeserializer
	{
		public static T Deserialize<T>(byte[] data, CachedTagInstance tag, GameCacheContext cacheContext)
		{
			var result = Deserialize(data, tag, cacheContext);
			return (T)Convert.ChangeType(result, typeof(T));
		}

		public static object Deserialize(byte[] data, CachedTagInstance tag, GameCacheContext cacheContext)
		{
			var isLittleEndian = CacheVersionDetection.IsLittleEndian(cacheContext.Version);
			var definitionOffset = (int)tag.DefinitionOffset;
			var endianBytes = new EndianBytes(data, isLittleEndian);
			var definition = TagDefinition.Find(tag.Group.Tag);
			var info = TagStructure.GetTagStructureInfo(definition, cacheContext.Version);
			var enumerator = TagStructure.GetTagFieldEnumerable(info);

			var value = MainDeserialize(definitionOffset, endianBytes, definition, cacheContext, enumerator);
			return value;
		}

		private static object MainDeserialize(int structOffset, EndianBytes endianBytes, Type structureType, GameCacheContext cacheContext, TagFieldEnumerable tagFieldEnumerable)
		{
			if (structOffset < 0 || structOffset >= endianBytes.Data.Length)
				throw new IndexOutOfRangeException();

			object value = null;
			TaskBag new_task = null;
			new_task = new TaskBag(() => {
				value = MainDeserializeInternal(new_task, ref structOffset, endianBytes, structureType, cacheContext, tagFieldEnumerable);
			});
			new_task.Start();
			new_task.Wait();
			new_task.WaitForChildren();
			return value;
		}

		/// <summary>
		/// This is where all the action takes place.
		/// </summary>
		private static object MainDeserializeInternal(TaskBag currentTask, ref int structOffset, EndianBytes endianBytes, Type structureType, GameCacheContext cacheContext, TagFieldEnumerable tagFieldEnumerable)
		{
			var parent = Activator.CreateInstance(structureType);

			// Ensure that fields are in declaration order - GetFields does NOT guarantee this!
			foreach (var tagFieldInfo in tagFieldEnumerable)
			{
				// Skip the field if the attribute tells us to.
				if (tagFieldInfo.Attribute != null)
				{
					// Dont deserialize fields that don't appear in this version's definition
					if (tagFieldInfo.Attribute.MinVersion != CacheVersion.Unknown &&
						tagFieldInfo.Attribute.MinVersion > cacheContext.Version)
						continue;
					if (tagFieldInfo.Attribute.MaxVersion != CacheVersion.Unknown &&
						tagFieldInfo.Attribute.MaxVersion < cacheContext.Version)
						continue;

					// Don't deserialize runtime-only fields.
					if (tagFieldInfo.Attribute.Runtime == true)
						continue;

					// Just move the offset forward for padding.
					if (tagFieldInfo.Attribute.Padding == true)
					{
						structOffset += tagFieldInfo.Attribute.Length;
						continue;
					}
				}

				// Get some standard info about each field.
				var tagField = tagFieldInfo;
				var tagFieldAttribute = tagFieldInfo.Attribute;
				var fieldType = tagField.FieldType;
				var genericType = tagField.FieldType.IsGenericType ? tagField.FieldType.GetGenericTypeDefinition() : null;
				var genericTypeArgument = tagField.FieldType.IsGenericType ? tagField.FieldType.GenericTypeArguments[0] : null;

				// Deserialize a primitive value.
				if (fieldType.IsPrimitive)
				{
					var value = DeserializePrimitive(fieldType, ref structOffset, endianBytes);
					tagField.SetValue(parent, value);
				}

				// Deserialize a Blam! type (Tag, CachedTagInstance, RealVector2d, RealQuaternion, etc...)
				else if (typeof(IBlamType).IsAssignableFrom(fieldType))
				{
					var value = Activator.CreateInstance(fieldType);
					switch (value)
					{
						// TODO: Order these cases based on how common they are.
						case Tag tag:
							tag = new Tag(endianBytes.ToInt32(ref structOffset));
							break;

						case CachedTagInstance cachedTagInstance:
							if (tagFieldAttribute == null || !tagFieldAttribute.Short)
								structOffset += (cacheContext.Version > CacheVersion.Halo2Vista ? 0xC : 0x4); // Skip the class name and zero bytes, it's not important

							var index = endianBytes.ToInt32(ref structOffset);
							cachedTagInstance = cacheContext.GetTag(index);

							if (cachedTagInstance != null && tagFieldAttribute != null && tagFieldAttribute.ValidTags != null)
								foreach (string tag in tagFieldAttribute.ValidTags)
									if (!cachedTagInstance.IsInGroup(tag))
										throw new Exception($"Invalid group for tag reference: {cachedTagInstance.Group.Tag}");
							value = cachedTagInstance;
							break;

						case CacheAddress cacheAddress:
							cacheAddress = new CacheAddress(endianBytes.ToUInt32(ref structOffset));
							break;

						case RealRgbColor realRgbColor:
							realRgbColor = new RealRgbColor(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case RealArgbColor realArgbColor:
							realArgbColor = new RealArgbColor(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case ArgbColor argbColor:
							argbColor = new ArgbColor(
								endianBytes.ToByte(ref structOffset), endianBytes.ToByte(ref structOffset),
								endianBytes.ToByte(ref structOffset), endianBytes.ToByte(ref structOffset));
							break;

						case Point2d point2D:
							point2D = new Point2d(endianBytes.ToInt16(ref structOffset), endianBytes.ToInt16(ref structOffset));
							break;

						case Rectangle2d rectangle2D:
							rectangle2D = new Rectangle2d(
								endianBytes.ToInt16(ref structOffset), endianBytes.ToInt16(ref structOffset),
								endianBytes.ToInt16(ref structOffset), endianBytes.ToInt16(ref structOffset));
							break;

						case RealEulerAngles2d realEulerAngles2D:
							realEulerAngles2D = new RealEulerAngles2d(
								Angle.FromRadians(endianBytes.ToSingle(ref structOffset)), Angle.FromRadians(endianBytes.ToSingle(ref structOffset)));
							break;

						case RealEulerAngles3d realEulerAngles3D:
							realEulerAngles3D = new RealEulerAngles3d(
								Angle.FromRadians(endianBytes.ToSingle(ref structOffset)),
								Angle.FromRadians(endianBytes.ToSingle(ref structOffset)),
								Angle.FromRadians(endianBytes.ToSingle(ref structOffset)));
							break;

						case RealPoint2d realPoint2D:
							realPoint2D = new RealPoint2d(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case RealPoint3d realPoint3D:
							realPoint3D = new RealPoint3d(
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset));
							break;

						case RealVector2d realVector2D:
							realVector2D = new RealVector2d(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case RealVector3d realVector3D:
							realVector3D = new RealVector3d(
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset));
							break;

						case RealQuaternion realQuaternion:
							realQuaternion = new RealQuaternion(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case RealPlane2d realPlane2D:
							realPlane2D = new RealPlane2d(
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset));
							break;

						case RealPlane3d realPlane3D:
							realPlane3D = new RealPlane3d(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case RealMatrix4x3 realMatrix4X3:
							realMatrix4X3 = new RealMatrix4x3(
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset),
								endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset), endianBytes.ToSingle(ref structOffset));
							break;

						case StringId stringId:
							stringId = new StringId(endianBytes.ToUInt32(ref structOffset), cacheContext.Version);
							break;

						case Angle angle:
							angle = Angle.FromRadians(endianBytes.ToSingle(ref structOffset));
							break;

						case IBounds bounds:
							(bounds as dynamic).Upper = DeserializePrimitive((bounds as dynamic).Upper.GetType(), ref structOffset, endianBytes);
							(bounds as dynamic).Lower = DeserializePrimitive((bounds as dynamic).Lower.GetType(), ref structOffset, endianBytes);
							break;

						default:
							throw new ArgumentException($"Unhandled Blam! Type: {fieldType}");
					}

					tagField.SetValue(parent, value);
				}

				// Deserialize an enum as it's underlying type.
				else if (fieldType.IsEnum)
				{
					var underlyingEnumType = tagField.FieldType.GetEnumUnderlyingType();
					var value = DeserializePrimitive(underlyingEnumType, ref structOffset, endianBytes);
					tagField.SetValue(parent, value);
				}

				// Deserialize a tagblock.
				else if (typeof(IList).IsAssignableFrom(fieldType) && genericTypeArgument != null)
				{
					var elementCount = endianBytes.ToInt32(ref structOffset);
					var pointer = endianBytes.ToPointer(ref structOffset);

					if (cacheContext.Version > CacheVersion.Halo2Vista)
						structOffset += 4;

					if (elementCount > 0 && pointer > 0)
					{
						var listType = typeof(List<>).MakeGenericType(genericTypeArgument);
						var list = (IList)Activator.CreateInstance(listType, new object[] { elementCount });

						if (genericTypeArgument.IsPrimitive)
						{
							list = DeserializePrimitiveArray(ref structOffset, endianBytes, genericTypeArgument, elementCount);
						}

						else if (genericTypeArgument.IsDefined(typeof(TagStructureAttribute)))
						{
							var value = Activator.CreateInstance(genericTypeArgument);
							var structEnumerator = TagStructure.GetTagFieldEnumerable(genericTypeArgument, cacheContext.Version);

							// TODO: add logic to decide when it's worth starting a task branch
							if (DateTime.Now.Millisecond < 0)
							{
								for (var i = 0; i < elementCount; i++)
								{
									list.Add(value);
									BranchDeserialize(currentTask, pointer, endianBytes, genericTypeArgument, structEnumerator.Info, list, null, i, cacheContext, structEnumerator);
								}
							}

							else
							{
								for (var i = 0; i < elementCount; i++)
								{
									value = MainDeserializeInternal(currentTask, ref pointer, endianBytes, genericTypeArgument, cacheContext, structEnumerator);
									list.Add(value);
								}
							}
						}

						tagField.SetValue(parent, list);
					}
				}

				// Deserialize a pointer.
				else if (tagFieldAttribute != null && tagFieldAttribute.Pointer)
				{
					// Read the pointer
					var pointer = endianBytes.ToPointer(ref structOffset);
					if (pointer > 0)
					{
						var structEnumerator = TagStructure.GetTagFieldEnumerable(fieldType, cacheContext.Version);
						var value = MainDeserializeInternal(currentTask, ref pointer, endianBytes, fieldType, cacheContext, structEnumerator);
						tagField.SetValue(parent, value);
					}
				}

				// Deserialize a TagStructure
				else if (fieldType.IsDefined(typeof(TagStructureAttribute)))
				{
					var structEnumerator = TagStructure.GetTagFieldEnumerable(fieldType, cacheContext.Version);
					var value = MainDeserializeInternal(currentTask, ref structOffset, endianBytes, fieldType, cacheContext, structEnumerator);
					tagField.SetValue(parent, value);
				}

				// Deserialize an array.
				else if (fieldType.IsArray)
				{
					// Byte array = Data reference
					// TODO: Allow other types to be in data references, since sometimes they can point to a structure
					if (fieldType == typeof(byte[]) && tagFieldAttribute.Length == 0)
					{
						// Read size and pointer
						var size = endianBytes.ToInt32(ref structOffset);

						if (cacheContext.Version > CacheVersion.Halo2Vista)
							structOffset += 8;

						var pointer = endianBytes.ToPointer(ref structOffset);

						// Read the data
						object result = null;
						if (size > 0 && pointer > 0)
							result = endianBytes.ReadBytes(ref pointer, size);

						tagField.SetValue(parent, result);
					}

					// Inline array
					else
					{
						if (tagFieldAttribute == null || tagFieldAttribute.Length == 0)
							throw new ArgumentException("Cannot deserialize an inline array with no count set");

						// Create the array and read the elements in order
						var elementCount = tagFieldAttribute.Length;
						var elementType = fieldType.GetElementType();

						if (elementType.IsPrimitive)
						{
							var value = DeserializePrimitiveArray(ref structOffset, endianBytes, elementType, elementCount);
							tagField.SetValue(parent, value);
						}
						else
						{
							var listType = typeof(List<>).MakeGenericType(genericTypeArgument);
							var list = (IList)Activator.CreateInstance(listType, new object[] { elementCount });

							var structEnumerator = TagStructure.GetTagFieldEnumerable(fieldType, cacheContext.Version);
							for (var i = 0; i < elementCount; i++)
							{
								var value = MainDeserializeInternal(currentTask, ref structOffset, endianBytes, elementType, cacheContext, structEnumerator);
								list.Add(value);
							}
							tagField.SetValue(parent, list);
						}
					}
				}

				// Deserialize a string.
				else if (fieldType == typeof(String))
				{
					string str;

					if (tagFieldAttribute.CharSet == CharSet.Ansi)
					{
						var bytes = endianBytes.ReadBytes(ref structOffset, tagFieldAttribute.Length);
						str = Encoding.UTF8.GetString(bytes);
					}
					else if (tagFieldAttribute.CharSet == CharSet.Unicode)
					{
						var bytes = endianBytes.ReadBytes(ref structOffset, tagFieldAttribute.Length * 2);
						str = Encoding.Unicode.GetString(bytes);
					}
					else
						throw new NotSupportedException($"{tagFieldAttribute.CharSet}");

					str = str.Substring(0, str.IndexOf('\0'));
				}

				else
					throw new NotImplementedException($"{fieldType}, {genericType}, {genericTypeArgument}");

				// Honor the value's size if it has one set
				if (tagFieldAttribute != null && tagFieldAttribute.Size > 0)
					structOffset += (int)tagFieldAttribute.Size;

				// TagFieldPrinter(Context.Context, field, parent);
			}

			return parent;
		}

		/// <summary>
		/// Branches a structure into a new <see cref="Task"/> for deserialization.
		/// </summary>
		private static TaskBag BranchDeserialize(TaskBag parent_task, int structOffset, EndianBytes endianBytes, Type structure_type, TagStructureInfo structure_info, object root_parent, FieldInfo field_info, int? list_index, GameCacheContext cacheContext, TagFieldEnumerable tagFieldEnumerable)
		{
			var size = structure_info.TotalSize;

			//todo something smarter
			TaskBag new_task = null;
			new_task = new TaskBag(() => {
				BranchDeserializeInternal(new_task, ref structOffset, endianBytes, structure_type, root_parent, field_info, list_index, cacheContext, tagFieldEnumerable);
			});

			new_task.Start();
			parent_task.ChildTasks.Add(new_task);
			return parent_task;
		}

		private static object BranchDeserializeInternal(TaskBag current_task, ref int structOffset, EndianBytes endianBytes, Type structure_type, object root_parent, FieldInfo field_info, int? list_index, GameCacheContext cacheContext, TagFieldEnumerable tagFieldEnumerable)
		{
			var parent = MainDeserializeInternal(current_task, ref structOffset, endianBytes, structure_type, cacheContext, tagFieldEnumerable);

			if (root_parent == null)
				throw new Exception("Invalid usage");

			if (field_info != null)
				field_info.SetValue(root_parent, parent);
			else if (list_index != null)
				(root_parent as dynamic)[list_index ?? 0] = parent as dynamic;
			else
				throw new Exception("Invalid usage");

			return parent;
		}

		/// <summary>
		/// Deserializes a primitive array.
		/// </summary>
		private static unsafe Array DeserializePrimitiveArray(ref int offset, EndianBytes reader, Type elementType, int elementCount)
		{
			// TODO: order these by how common they are
			switch (Type.GetTypeCode(elementType))
			{
				case TypeCode.Boolean:
					{
						var size = sizeof(SByte) * elementCount;
						var src = reader.ReadBytes(ref offset, size);
						var dst = new SByte[elementCount];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;
					}

				case TypeCode.SByte:
					{
						var size = sizeof(SByte) * elementCount;
						var src = reader.ReadBytes(ref offset, size);
						var dst = new SByte[elementCount];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;
					}

				case TypeCode.Byte:
					{
						var size = sizeof(SByte) * elementCount;
						var src = reader.ReadBytes(ref offset, size);
						var dst = new SByte[elementCount];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;

					}

				case TypeCode.Char:
					{
						// let's make sum things
						var size = sizeof(Int32) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Int32[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Char[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int16:
					{
						// let's make sum things
						var size = sizeof(Int16) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Int16[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Int16[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt16:
					{
						// let's make sum things
						var size = sizeof(UInt16) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new UInt16[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt16[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Single:
					{
						// let's make sum things
						var size = sizeof(Single) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Single[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Single[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int32:
					{
						// let's make sum things
						var size = sizeof(Int32) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Int32[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Int32[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt32:
					{
						// let's make sum things
						var size = sizeof(UInt32) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new UInt32[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt32[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Double:
					{
						// let's make sum things
						var size = sizeof(Double) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Double[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Double[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int64:
					{
						// let's make sum things
						var size = sizeof(Int64) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Int64[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Int64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt64:
					{
						// let's make sum things
						var size = sizeof(UInt64) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new UInt64[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Decimal:
					{
						// let's make sum things
						var size = sizeof(Decimal) * elementCount;
						var source = reader.ReadBytes(ref offset, size);
						var dest = new Decimal[elementCount];

						// Reverse the source byte[] order when BigEndian
						if (reader.NeedsEndianSwap)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest Decimal[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.NeedsEndianSwap)
							Array.Reverse(dest);

						return dest;
					}

				default:
					throw new ArgumentException($"Unhandled PrimitiveArray: {elementType.Name}[{elementCount}]");
			}
		}

		/// <summary>
		/// Deserializes a primitive value.
		/// </summary>
		private static ValueType DeserializePrimitive(Type primitive_type, ref int offset, EndianBytes reader)
		{
			// TODO: order these by how common they are.
			switch (Type.GetTypeCode(primitive_type))
			{
				case TypeCode.Byte:
					return reader.ToByte(ref offset);
				case TypeCode.Int16:
					return reader.ToInt16(ref offset);
				case TypeCode.UInt16:
					return reader.ToUInt16(ref offset);
				case TypeCode.UInt32:
					return reader.ToUInt32(ref offset);
				case TypeCode.Single:
					return reader.ToSingle(ref offset);
				case TypeCode.Int32:
					return reader.ToInt32(ref offset);
				case TypeCode.SByte:
					return reader.ToSByte(ref offset);
				case TypeCode.Double:
					return reader.ToDouble(ref offset);
				case TypeCode.Int64:
					return reader.ToInt64(ref offset);
				case TypeCode.UInt64:
					return reader.ToUInt64(ref offset);
				case TypeCode.Boolean:
					return reader.ToBoolean(ref offset);
				case TypeCode.Char:
					return reader.ToChar(ref offset);
				case TypeCode.Decimal:
					return reader.ToDecimal(ref offset);
				default:
					throw new ArgumentException($"Unhandled Primitive Type: {Type.GetTypeCode(primitive_type)}");
			}
		}

		/// <summary>
		/// TODO: set these up. Deserializes a Halo3/ODST PixelShaderReference.
		/// </summary>
		public static PixelShaderReference DeserializePixelShaderReference(EndianReader reader)
		{
			throw new NotImplementedException();

			//// This reference is a uint32 pointer, we'll be moving the stream position around. Right before returning
			//// from this method 'reader.SeekTo(endPosition)' will bring us to where the serializer expects the next
			//// bit of data to be.
			//var endPosition = reader.BaseStream.Position + 0x04;

			//var pointer = reader.ReadUInt32();

			//if (pointer < 1)
			//	return null;

			//var headerOffset = (int)Context.Tag.PointerToOffset(pointer);
			//reader.SeekTo(headerOffset);

			//PixelShaderHeader header = null;
			//// header = (PixelShaderHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(PixelShaderHeader)));
			//throw new NotImplementedException();

			//if (header.ShaderDataAddress == 0)
			//	return null;

			//var debugHeaderOffset = reader.Position;
			//ShaderDebugHeader debugHeader = null;
			//// debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(ShaderDebugHeader)));
			//throw new NotImplementedException();

			//if ((debugHeader.Magic >> 16) != 0x102A)
			//	return null;

			//if (debugHeader.StructureSize == 0)
			//	return null;

			//reader.SeekTo(debugHeaderOffset);
			//var debugData = reader.ReadBytes((int)debugHeader.StructureSize);

			//var updbName = "";

			//if (debugHeader.UpdbPointerOffset != 0)
			//{
			//	reader.SeekTo(debugHeaderOffset + (long)debugHeader.UpdbPointerOffset);
			//	var updbNameLength = reader.ReadUInt64();

			//	if (updbNameLength > 0)
			//		updbName = new string(reader.ReadChars((int)updbNameLength));
			//}

			//var totalSize = debugHeader.ShaderDataSize;
			//var constantSize = 0U;
			//var codeSize = totalSize;

			//if (debugHeader.CodeHeaderOffset != 0)
			//{
			//	reader.SeekTo(debugHeaderOffset + debugHeader.CodeHeaderOffset);
			//	constantSize = reader.ReadUInt32();
			//	codeSize = reader.ReadUInt32();
			//}

			//var constant_block_offset = (int)Context.Tag.PointerToOffset(header.ShaderDataAddress);
			//reader.SeekTo(constant_block_offset);
			//var constantData = reader.ReadBytes((int)constantSize);

			//var shader_data_block_offset = constant_block_offset + constantSize;
			//reader.SeekTo(shader_data_block_offset);
			//var shaderData = reader.ReadBytes((int)codeSize);

			//reader.SeekTo(endPosition);

			//var info = new XboxShaderInfo
			//{
			//	DataAddress = shader_data_block_offset,
			//	DebugInfoOffset = (uint)debugHeaderOffset,
			//	DebugInfoSize = debugHeader.StructureSize,
			//	DatabasePath = updbName,
			//	DataSize = totalSize,
			//	ConstantDataSize = constantSize,
			//	CodeDataSize = codeSize
			//};

			//return new PixelShaderReference
			//{
			//	Info = info,
			//	UpdbName = updbName,
			//	Header = header,
			//	DebugHeader = debugHeader,
			//	DebugData = debugData,
			//	ShaderData = shaderData,
			//	ConstantData = constantData
			//};
		}

		/// <summary>
		/// TODO: set these up. Deserializes a Halo3/ODST VertexShaderReference.
		/// </summary>
		public static VertexShaderReference DeserializeVertexShaderReference(EndianReader reader)
		{
			throw new NotImplementedException();

			//// This reference is a uint32 pointer, we'll be moving the stream position around. Right before returning
			//// from this method 'reader.SeekTo(endPosition)' will bring us to where the serializer expects the next
			//// bit of data to be.
			//var endPosition = reader.BaseStream.Position + 0x04;

			//var headerAddress = reader.ReadUInt32();

			//if (headerAddress < 1)
			//	return null;

			//var headerOffset = Context.AddressToOffset((uint)(reader.BaseStream.Position - 4), headerAddress);
			//reader.SeekTo(headerOffset + 0x4 * sizeof(uint));

			//VertexShaderHeader header = null;
			//// header = (VertexShaderHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(VertexShaderHeader)));
			//throw new NotImplementedException();

			//if (header.ShaderDataAddress == 0)
			//	return null;

			//var debugHeaderOffset = reader.Position;
			//ShaderDebugHeader debugHeader = null;
			//// debugHeader = (ShaderDebugHeader)DeserializeStruct(reader, context, ReflectionCache.GetTagStructureInfo(typeof(ShaderDebugHeader)));
			//throw new NotImplementedException();

			//if ((debugHeader.Magic >> 16) != 0x102A)
			//	return null;

			//if (debugHeader.StructureSize == 0)
			//	return null;

			//reader.SeekTo(debugHeaderOffset);
			//var debugData = reader.ReadBytes((int)debugHeader.StructureSize);

			//var updbName = "";

			//if (debugHeader.UpdbPointerOffset != 0)
			//{
			//	reader.SeekTo(debugHeaderOffset + (long)debugHeader.UpdbPointerOffset);
			//	var updbNameLength = reader.ReadUInt64();

			//	if (updbNameLength > 0)
			//		updbName = new string(reader.ReadChars((int)updbNameLength));
			//}

			//var totalSize = debugHeader.ShaderDataSize;
			//var constantSize = 0U;
			//var codeSize = totalSize;

			//if (debugHeader.CodeHeaderOffset != 0)
			//{
			//	reader.SeekTo(debugHeaderOffset + debugHeader.CodeHeaderOffset);
			//	constantSize = reader.ReadUInt32();
			//	codeSize = reader.ReadUInt32();
			//}

			//var constant_block_offset = Context.AddressToOffset(headerOffset + 0x10, header.ShaderDataAddress);
			//reader.SeekTo(constant_block_offset);
			//var constantData = reader.ReadBytes((int)constantSize);

			//var shader_data_block_offset = constant_block_offset + constantSize;
			//reader.SeekTo(shader_data_block_offset);
			//var shaderData = reader.ReadBytes((int)codeSize);

			//reader.SeekTo(endPosition);

			//var info = new XboxShaderInfo
			//{
			//	DataAddress = shader_data_block_offset,
			//	DebugInfoOffset = (uint)debugHeaderOffset,
			//	DebugInfoSize = debugHeader.StructureSize,
			//	DatabasePath = updbName,
			//	DataSize = totalSize,
			//	ConstantDataSize = constantSize,
			//	CodeDataSize = codeSize
			//};

			//return new VertexShaderReference
			//{
			//	Info = info,
			//	UpdbName = updbName,
			//	Header = header,
			//	DebugHeader = debugHeader,
			//	DebugData = debugData,
			//	ShaderData = shaderData,
			//	ConstantData = constantData
			//};
		}
	}
}