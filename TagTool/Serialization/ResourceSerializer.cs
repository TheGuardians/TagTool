using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;
using TagTool.Tags;
using static TagTool.Tags.Resources.BitmapTextureInterleavedInteropResource;
using TagTool.Tags.Resources;
using static TagTool.Tags.Resources.BitmapTextureInteropResource;

namespace TagTool.Serialization
{
    public class ResourceSerializer : TagSerializer
    {
        public const int DefaultResourceAlign = 0;

        public ResourceSerializer(CacheVersion version) : base(version) {  }

        /// <summary>
        /// Serializes a tag structure into a context.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="tagStructure">The tag structure.</param>
        /// <param name="offset">An optional offset to begin serializing at.</param>
        public override void Serialize(ISerializationContext context, object tagStructure, uint? offset = null)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

            // Serialize the structure to a data block
            var info = TagStructure.GetTagStructureInfo(tagStructure.GetType(), Version);
            context.BeginSerialize(info);
            var tagStream = new MemoryStream();
            var structBlock = (ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock)context.CreateBlock();
            structBlock.BlockType = resourceContext.InitialAddressType;
            structBlock.Writer.Format = Format;
            SerializeStruct(context, tagStream, structBlock, info, tagStructure);

            // Finalize the block and write all of the tag data out
            var mainStructOffset = offset ?? structBlock.Finalize(tagStream);

            var data = tagStream.ToArray();
            context.EndSerialize(info, data, mainStructOffset);
            var mainOffset = resourceContext.MainStructOffset.Offset;

            // move over the remaining fixups to the context
            foreach (var fixup in structBlock.ResourceFixups)
            {
                fixup.BlockOffset += (uint)mainOffset;
                resourceContext.ResourceFixups.Add(fixup);
            }

            foreach (var d3dfixup in structBlock.D3DFixups)
            {
                d3dfixup.Address = new CacheAddress(d3dfixup.Address.Type, (int)(d3dfixup.Address.Offset + mainOffset));
                resourceContext.D3DFixups.Add(d3dfixup);
            }
        }

        public override void SerializeTagData(ISerializationContext context, MemoryStream tagStream, IDataBlock block, TagData tagData, TagFieldAttribute valueInfo)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            if (block.GetType() != typeof(ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock))
                throw new Exception($"Invalid block type given resource deserialization");

            var resourceBlock = block as ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock;
            var resourceContext = context as ResourceDefinitionSerializationContext;

            var writer = block.Writer;

            if (tagData == null || tagData.Data == null || tagData.Data.Length == 0)
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                return;
            }

            CacheAddressType addressType = tagData.AddressType;

            int dataOffset = 0; // offset in the data stream to the start of the data
            uint blockOffset = (uint)writer.BaseStream.Position + 0xC; // offset to the address pointing to the above relative to the current block.
            uint size = 0;

            // stream where the byte[] in the data should be written to:
            var dataStream = (MemoryStream)resourceContext.GetWriter(addressType).BaseStream;

            var data = tagData.Data;
            if (data != null && data.Length > 0)
            {
                // Ensure the block is aligned correctly
                var align = Math.Max(DefaultResourceAlign, (valueInfo != null) ? valueInfo.Align : 0);
                StreamUtil.Align(dataStream, (int)align);

                // Write its data
                dataOffset = (int)dataStream.Position;
                size = (uint)data.Length;
                dataStream.Write(data, 0, data.Length);
                StreamUtil.Align(dataStream, DefaultResourceAlign);
            }

            var dataAddress = new CacheAddress(addressType, dataOffset);

            var dataFixup = new TagResourceGen3.ResourceFixup
            {
                BlockOffset = blockOffset,
                Address = dataAddress
            };
            resourceBlock.ResourceFixups.Add(dataFixup);

            // this fixup will need to be adjusted when we move the block

            // Write the reference data
            writer.Write(size);
            writer.Write(0);
            writer.Write(0);
            writer.Write(dataAddress.Value);
            writer.Write(0);
        }

        public override void SerializeTagBlock(CacheVersion version, ISerializationContext context, MemoryStream tagStream, IDataBlock block, object list, Type listType, TagFieldAttribute valueInfo)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            if (block.GetType() != typeof(ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock))
                throw new Exception($"Invalid block type given resource deserialization");

            var resourceBlock = block as ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock;
            var resourceContext = context as ResourceDefinitionSerializationContext;

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

            CacheAddressType addressType = (CacheAddressType)listType.GetField("AddressType").GetValue(list);

            // Serialize each value in the list to a data block
            var resourceBlock2 = (ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock)resourceContext.CreateBlock();
            resourceBlock2.BlockType = addressType;
            var addressTypeStream = (MemoryStream)resourceContext.GetWriter(addressType).BaseStream;

            var enumerableList = (System.Collections.IEnumerable)list;

            foreach (var val in enumerableList)
                SerializeValue(version, resourceContext, tagStream, resourceBlock2, val, null, elementType);

            // Ensure the block is aligned correctly
            var align = 0x10;
            StreamUtil.Align(resourceBlock2.Stream, align);

            // Finalize the block and write the tag block reference using a cache address
            var offset = resourceBlock2.Finalize(addressTypeStream);    // offset of the data in the tagblock on the actual stream
            //var blockOffset = addressTypeStream.Position;               // no need to fix that particular fixup later
            var address = new CacheAddress(addressType, (int)offset);

            var resourceFixup = new TagResourceGen3.ResourceFixup
            {
                Address = address,
                BlockOffset = (uint)writer.BaseStream.Position + 0x4
            };

            foreach (var fixup in resourceBlock2.ResourceFixups)
            {
                fixup.BlockOffset += offset;
                resourceContext.ResourceFixups.Add(fixup);
            }
                
            foreach (var d3dfixup in resourceBlock2.D3DFixups)
            {
                d3dfixup.Address = new CacheAddress(d3dfixup.Address.Type, (int)(d3dfixup.Address.Offset + offset));
                resourceContext.D3DFixups.Add(d3dfixup);
            }
                
            resourceBlock.ResourceFixups.Add(resourceFixup);

            writer.Write(count);
            writer.Write(address.Value);    // write address as 0, we use the fixups
            writer.Write(0);
        }

        public override void SerializeD3DStructure(CacheVersion version, ISerializationContext context, MemoryStream tagStream, IDataBlock block, object val, Type valueType)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            if (block.GetType() != typeof(ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock))
                throw new Exception($"Invalid block type given resource deserialization");

            var resourceBlock = block as ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock;
            var resourceContext = context as ResourceDefinitionSerializationContext;

            var writer = block.Writer;

            if (val == null)
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                return;
            }

            var addressType = (CacheAddressType)valueType.GetField("AddressType").GetValue(val);
            var nextStream = (MemoryStream)resourceContext.GetWriter(addressType).BaseStream;
            var genericType = valueType.GenericTypeArguments[0];
            var def = valueType.GetField("Definition").GetValue(val);
            // Serialize the value to a temporary block
            var resourceBlock2 = (ResourceDefinitionSerializationContext.ResourceDefinitionDataBlock)context.CreateBlock();
            resourceBlock2.BlockType = addressType;
            SerializeValue(version, context, tagStream, resourceBlock2, def, null, genericType);

            // Finalize the block and write the pointer

            var offset = (int)resourceBlock2.Finalize(nextStream);


            // find where in the stream the d3dstructure pointer will be written
            var blockOffset = (int)writer.BaseStream.Position;
            var address = new CacheAddress(addressType, blockOffset);


            int structureTypeIndex;

            if (genericType == typeof(BitmapDefinition))
                structureTypeIndex = 2;
            else if (genericType == typeof(BitmapInterleavedDefinition))
                structureTypeIndex = 3;
            else if (genericType == typeof(VertexBufferDefinition))
                structureTypeIndex = 0;
            else if (genericType == typeof(IndexBufferDefinition))
                structureTypeIndex = 1;
            else
                throw new Exception();

            var d3dFixup = new TagResourceGen3.D3DFixup
            {
                ResourceStructureTypeIndex = structureTypeIndex,
                Address = address
            };

            var resourceFixup = new TagResourceGen3.ResourceFixup
            {
                Address = new CacheAddress(addressType, offset),
                BlockOffset = (uint)blockOffset
            };

            foreach (var fixup in resourceBlock2.ResourceFixups)
            {
                fixup.BlockOffset += (uint)offset;
                resourceContext.ResourceFixups.Add(fixup);
            }
                
            resourceBlock.ResourceFixups.Add(resourceFixup);
            resourceBlock.D3DFixups.Add(d3dFixup);

            writer.Write(address.Value);
            writer.Write(0);
            writer.Write(0);
        }
    }
}
