using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Shaders;
using TagTool.Serialization;
using System.Collections;
using System.Threading;

namespace TagTool.HyperSerialization
{
	partial class HyperSerializer
	{
		private static volatile int ThreadCounter;

		private Task DeserializeBranch(long offset, Stream stream, Type structure_type, TagStructureInfo structure_info, object root_parent, FieldInfo field_info, int? list_index)
		{
			var new_stream = stream; // new StreamMultiplxer(stream);
			new_stream.Position = offset;//todo get structure size

			Task task = new Task(() => {
				Deserialize(new_stream, structure_type, structure_info, root_parent, field_info, list_index);
			});

			Interlocked.Increment(ref HyperSerializer.ThreadCounter);
			task.Start();
			Interlocked.Decrement(ref HyperSerializer.ThreadCounter);

			//todo something smarter
			if (ThreadCounter >= Environment.ProcessorCount)
				task.Wait();
			return task;
		}

		public object Deserialize(Stream stream, Type structure_type, TagStructureInfo structure_info, object root_parent, FieldInfo field_info, int? list_index)
		{
			var parent = Activator.CreateInstance(structure_type);

			var structureAttribute = structure_type.GetCustomAttributes<TagStructureAttribute>()
				.Where(attr => attr.MinVersion >= this.CacheContext.Version 
				&& attr.MaxVersion <= this.CacheContext.Version).FirstOrDefault();

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
						if (fieldAttribute.MinVersion < this.CacheContext.Version) continue;
						if (fieldAttribute.MaxVersion > this.CacheContext.Version) continue;
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
							case Object unused when fieldAttribute != null && fieldAttribute.Pointer:
								// Read the pointer
								var pointer = reader.ReadUInt32();
								if (pointer == 0)
									{ value = null; break; } // Null object
								
								// Seek to it and read the object
								var nextOffset = reader.BaseStream.Position;
								reader.BaseStream.Position = this.AddressToOffset((uint)nextOffset - 4, pointer);
								value = Deserialize(stream, structure_type, structure_info, root_parent, field_info, list_index);
								reader.BaseStream.Position = nextOffset;
								break;

							case Enum unused:
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
									reader.BaseStream.Position += (this.CacheContext.Version > CacheVersion.Halo2Vista ? 0xC : 0x4); // Skip the class name and zero bytes, it's not important
								cachedTagInstance = this.GetTagByIndex(reader.ReadInt32());
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
								if (this.CacheContext.Version > CacheVersion.Halo2Vista)
									reader.BaseStream.Position = startOffset + 0xC;
								pointer = reader.ReadUInt32();
								if (pointer == 0)
								{
									// Null data reference
									reader.BaseStream.Position = startOffset + (this.CacheContext.Version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
									value = new byte[0];
									break;
								}
								// Read the data
								byteArray = new byte[size];
								reader.BaseStream.Position = this.AddressToOffset((uint)(startOffset + (this.CacheContext.Version > CacheVersion.Halo2Vista ? 0xC : 0x4)), pointer);
								reader.Read(byteArray, 0, size);
								reader.BaseStream.Position = startOffset + (this.CacheContext.Version > CacheVersion.Halo2Vista ? 0x14 : 0x8);
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
								stringId = new StringId(reader.ReadUInt32(), this.CacheContext.Version);
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
									var val = Deserialize(stream, elementType, new TagStructureInfo(genericType), null, null, null);
									array.SetValue(val, i);
								}
								break;

							case IBounds bounds:
								bounds._Lower = DeserializePrimitive(bounds._Lower.GetType(), reader);
								bounds._Upper = DeserializePrimitive(bounds._Upper.GetType(), reader);
								break;

							case VertexShaderReference vertexShaderReference:
								vertexShaderReference = DeserializeVertexShaderReference(reader);
								break;

							case PixelShaderReference pixelShaderReference:
								pixelShaderReference = DeserializePixelShaderReference(reader);
								break;

							default:
								throw new ArgumentException($"Unhandled Blam! Type: {fieldType}");
						}

						field.SetValue(parent, value);
					}

					else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
					{
						var count = reader.ReadInt32();
						var pointer = reader.ReadUInt32();

						if (this.CacheContext.Version > CacheVersion.Halo2Vista)
							stream.Position += 4;

						var list = (IList)Activator.CreateInstance(structure_type, new object[] { count });

						if (genericTypeArgument.IsPrimitive)
							list = DeserializePrimitiveList(genericType, genericTypeArgument, reader, count);

						for (var i = 0; i < count; i++)
						{
							object value;

							if (genericTypeArgument.IsClass)
							{
								value = null;
								long offset = stream.Position + pointer + count;
								Task task = DeserializeBranch(offset, stream, genericTypeArgument, new TagStructureInfo(genericTypeArgument), list, null, i);
								tasks.Add(task);
							}

							else
							{
								value = Deserialize(stream, genericTypeArgument, new TagStructureInfo(genericTypeArgument), null, null, null);
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
						Task task = DeserializeBranch(offset, stream, fieldType, new TagStructureInfo(genericType), parent, field_info, null);
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
							((IList)root_parent)[list_index ?? 0] = parent;
						}
						else throw new Exception("Invalid usage");
					}
				}
			}

			// End deserialize.
			Task.WaitAll(tasks.ToArray());
			return parent;
		}

		private unsafe IList DeserializePrimitiveList(Type type, Type element_type, EndianReader reader, int count)
		{
			switch (Type.GetTypeCode(element_type))
			{
				case TypeCode.Boolean:
					{
						var size = sizeof(SByte) * count;
						var src = reader.ReadBytes(size);
						var dst = new SByte[count];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;
					}

				case TypeCode.SByte:
					{
						var size = sizeof(SByte) * count;
						var src = reader.ReadBytes(size);
						var dst = new SByte[count];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;
					}

				case TypeCode.Byte:
					{
						var size = sizeof(SByte) * count;
						var src = reader.ReadBytes(size);
						var dst = new SByte[count];
						fixed (void* pBytes = src, pValue = dst)
							Buffer.MemoryCopy(pBytes, pValue, size, size);
						return dst;

					}

				case TypeCode.Char:
					{
						// let's make sum things
						var size = sizeof(Int32) * count;
						var source = reader.ReadBytes(size);
						var dest = new Int32[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int16:
					{
						// let's make sum things
						var size = sizeof(Int16) * count;
						var source = reader.ReadBytes(size);
						var dest = new Int16[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt16:
					{
						// let's make sum things
						var size = sizeof(UInt16) * count;
						var source = reader.ReadBytes(size);
						var dest = new UInt16[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Single:
					{
						// let's make sum things
						var size = sizeof(Single) * count;
						var source = reader.ReadBytes(size);
						var dest = new Single[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int32:
					{
						// let's make sum things
						var size = sizeof(Int32) * count;
						var source = reader.ReadBytes(size);
						var dest = new Int32[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt32:
					{
						// let's make sum things
						var size = sizeof(UInt32) * count;
						var source = reader.ReadBytes(size);
						var dest = new UInt32[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Double:
					{
						// let's make sum things
						var size = sizeof(Double) * count;
						var source = reader.ReadBytes(size);
						var dest = new Double[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Int64:
					{
						// let's make sum things
						var size = sizeof(Int64) * count;
						var source = reader.ReadBytes(size);
						var dest = new Int64[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.UInt64:
					{
						// let's make sum things
						var size = sizeof(UInt64) * count;
						var source = reader.ReadBytes(size);
						var dest = new UInt64[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				case TypeCode.Decimal:
					{
						// let's make sum things
						var size = sizeof(Decimal) * count;
						var source = reader.ReadBytes(size);
						var dest = new Decimal[count];

						// Reverse the source byte[] order when BigEndian
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(source);

						// Copy memory from our source byte[] to our dest UInt64[]
						fixed (void* pBytes = source, pValue = dest)
							Buffer.MemoryCopy(pBytes, pValue, size, size);

						// Reverse the dest UInt64[] to get it back in order
						if (reader.Format == EndianFormat.BigEndian)
							Array.Reverse(dest);

						return dest;
					}

				default:
					throw new ArgumentException($"Unhandled PrimitiveCollection Type: {type.Name} <{element_type.Name}>");
			}
		}

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

		public PixelShaderReference DeserializePixelShaderReference(EndianReader reader)
		{
			throw new NotImplementedException();
		}

		public VertexShaderReference DeserializeVertexShaderReference(EndianReader reader)
		{
			throw new NotImplementedException();
		}
	}
}
