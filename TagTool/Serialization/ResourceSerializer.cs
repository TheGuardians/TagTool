using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;
using TagTool.Tags;

namespace TagTool.Serialization
{
    public class ResourceSerializer : TagSerializer
    {
        public ResourceSerializer(CacheVersion version) : base(version) {  }

        public override void SerializeTagData(ISerializationContext context, MemoryStream tagStream, IDataBlock block, TagData tagData, TagFieldAttribute valueInfo)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

            CacheAddressType addressType = tagData.Address.Type;

            var nextStream = (MemoryStream)resourceContext.GetWriter(addressType).BaseStream;


            var writer = block.Writer;
            uint offset = 0;
            uint size = 0;
            var data = tagData.Data;

            if (data != null && data.Length > 0)
            {
                // Ensure the block is aligned correctly
                var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.Align : 0);
                StreamUtil.Align(nextStream, (int)align);

                // Write its data
                offset = (uint)nextStream.Position;
                size = (uint)data.Length;
                nextStream.Write(data, 0, data.Length);
                StreamUtil.Align(nextStream, DefaultBlockAlign);
            }

            resourceContext.ResourceFixups.Add(new TagResourceGen3.ResourceFixup
            {
                BlockOffset = (uint)writer.BaseStream.Position + 0xC,
                Address = new CacheAddress(addressType, (int)offset)
            });

            // Write the reference data
            writer.Write(size);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
        }

        public override void SerializeTagBlock(CacheVersion version, ISerializationContext context, MemoryStream tagStream, IDataBlock block, object list, Type listType, TagFieldAttribute valueInfo)
        {
            if (context.GetType() != typeof(ResourceDefinitionSerializationContext))
                throw new Exception($"Invalid context type given resource deserialization");

            var resourceContext = context as ResourceDefinitionSerializationContext;

            CacheAddressType addressType = (CacheAddressType)listType.GetField("AddressType").GetValue(list);

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
            TagStructureAttribute structure;

            try
            {
                structure = TagStructure.GetTagStructureInfo(elementType, Version).Structure;
            }
            catch
            {
                structure = null;
            }

            // Serialize each value in the list to a data block
            var tagBlock = resourceContext.CreateBlock();

            var addressTypeStream = (MemoryStream)resourceContext.GetWriter(addressType).BaseStream;

            var enumerableList = (System.Collections.IEnumerable)list;

            foreach (var val in enumerableList)
                SerializeValue(version, resourceContext, addressTypeStream, tagBlock, val, null, elementType);

            // Ensure the block is aligned correctly
            var align = Math.Max(DefaultBlockAlign, (valueInfo != null) ? valueInfo.Align : 0);
            StreamUtil.Align(addressTypeStream, (int)align);

            // Finalize the block and write the tag block reference using a cache address
            var offset = tagBlock.Finalize(addressTypeStream);
            var address = new CacheAddress(addressType, (int)offset);
            resourceContext.ResourceFixups.Add(new TagResourceGen3.ResourceFixup
            {
                Address = address,
                BlockOffset = (uint)writer.BaseStream.Position + 0x4
            });

            writer.Write(count);
            writer.Write(0);    // write address as 0, we use the fixups
            writer.Write(0);
        }
    }
}
