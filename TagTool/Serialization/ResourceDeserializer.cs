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
    public class ResourceDeserializer : TagDeserializer
    {

        public ResourceDeserializer(CacheVersion version) : base(version) { }

        /// <summary>
        /// Deserializes a value which is pointed to by an address.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="valueType">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        public override object DeserializeD3DStructure(EndianReader reader, ISerializationContext context, Type valueType)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

            var result = Activator.CreateInstance(valueType);
            var elementType = valueType.GenericTypeArguments[0];

            // Read the pointer
            var startOffset = reader.BaseStream.Position;
            var address = new CacheAddress(reader.ReadUInt32());
            var definitionAddress = reader.ReadUInt32(); // unused
            var runtimeAddress = reader.ReadUInt32(); // unused
            

            var nextReader = resourceContext.GetReader(address.Type);

            // Seek to it and read the object

            nextReader.BaseStream.Position = address.Offset;

            var definition = DeserializeValue(nextReader, context, null, elementType);
            valueType.GetField("Definition").SetValue(result, definition);
            valueType.GetField("AddressType").SetValue(result, address.Type);

            reader.BaseStream.Position = startOffset + 0xC;
            return result;
        }

        public override object DeserializeTagBlock(EndianReader reader, ISerializationContext context, Type valueType)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

            var result = Activator.CreateInstance(valueType);
            var elementType = valueType.GenericTypeArguments[0];

            // Read count and offset
            var startOffset = reader.BaseStream.Position;

            var count = reader.ReadInt32();
            var pointer = new CacheAddress(reader.ReadUInt32());

            // Set block address type
            valueType.GetField("AddressType").SetValue(result, pointer.Type);

            if (count == 0)
            {
                // Null tag block
                reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0xC : 0x8);
                return result;
            }

            //
            // Read each value
            //

            var nextReader = resourceContext.GetReader(pointer.Type);
            nextReader.BaseStream.Position = pointer.Offset;

            var addMethod = valueType.GetMethod("Add");

            for (var i = 0; i < count; i++)
            {
                var element = DeserializeValue(nextReader, resourceContext, null, elementType);
                addMethod.Invoke(result, new[] { element });
            }

            reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0xC : 0x8);

            return result;
        }

        public override TagData DeserializeTagData(EndianReader reader, ISerializationContext context)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

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
                return new TagData();
            }

            var address = new CacheAddress(pointer);

            var nextReader = resourceContext.GetReader(address.Type);
            nextReader.BaseStream.Position = address.Offset;

            // Read the data
            var result = new byte[size];
            nextReader.Read(result, 0, size);
            reader.BaseStream.Position = startOffset + (Version > CacheVersion.Halo2Vista ? 0x14 : 0x8);

            // instantiate tagdata and return it
            var tagData = new TagData
            {
                Data = result,
                Address = address
            };

            return tagData;
        }
    }
}
