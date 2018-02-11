using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.TagResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Serialization
{
    /// <summary>
    /// A serialization context for serializing and deserializing resource definition structures.
    /// </summary>
    public class ResourceSerializationContext : ISerializationContext
    {
        private const int DefaultBlockAlign = 0x0;

        private PageableResource Resource { get; }
        private List<TagResource.ResourceFixup> ResourceFixups { get; } = new List<TagResource.ResourceFixup>();
        private List<TagResource.ResourceDefinitionFixup> ResourceDefinitionFixups { get; } = new List<TagResource.ResourceDefinitionFixup>();

        public ResourceSerializationContext(PageableResource resource)
        {
            Resource = resource;
        }

        public void BeginSerialize(TagStructureInfo info)
        {
            ResourceFixups.Clear();
            ResourceDefinitionFixups.Clear();
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Resource.Resource.ResourceFixups.Clear();
            Resource.Resource.ResourceDefinitionFixups.Clear();
            foreach(var fixup in ResourceFixups)
            {
                if(fixup.Address.Type != CacheAddressType.Memory)
                {
                    Resource.Resource.ResourceFixups.Add(fixup);
                }

            }
            Resource.Resource.ResourceDefinitionFixups.AddRange(ResourceDefinitionFixups);
            Resource.Resource.DefinitionData = data;
            Resource.Resource.DefinitionAddress = new CacheAddress(CacheAddressType.Definition, (int)mainStructOffset);
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            if (Resource.Resource.DefinitionAddress.Value == 0 || Resource.Resource.DefinitionAddress.Type != CacheAddressType.Definition)
                throw new InvalidOperationException("Invalid resource definition address");

            // Create a stream with a copy of the resource definition data
            var stream = new MemoryStream(Resource.Resource.DefinitionData.Length);
            stream.Write(Resource.Resource.DefinitionData, 0, Resource.Resource.DefinitionData.Length);
            
            // Apply fixups
            var writer = new BinaryWriter(stream);
            foreach (var fixup in Resource.Resource.ResourceFixups)
            {
                stream.Position = fixup.BlockOffset;
                writer.Write(fixup.Address.Value);
            }
            stream.Position = Resource.Resource.DefinitionAddress.Offset;
            return new EndianReader(stream);
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            var resourceAddress = new CacheAddress(address);
            if (resourceAddress.Type != CacheAddressType.Definition)
                throw new InvalidOperationException("Cannot dereference a resource address of type " + resourceAddress.Type);
            return (uint)resourceAddress.Offset;
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            throw new InvalidOperationException("Resource definitions cannot contain tag references");
        }

        public IDataBlock CreateBlock()
        {
            return new ResourceDataBlock(this);
        }

        private class ResourceDataBlock : IDataBlock
        {
            private readonly ResourceSerializationContext _context;
            private readonly List<TagResource.ResourceFixup> _fixups = new List<TagResource.ResourceFixup>();
            private readonly List<TagResource.ResourceDefinitionFixup> _d3dFixups = new List<TagResource.ResourceDefinitionFixup>();
            private uint _align = DefaultBlockAlign;

            public ResourceDataBlock(ResourceSerializationContext context)
            {
                _context = context;
                Stream = new MemoryStream();
                Writer = new BinaryWriter(Stream);
            }

            public MemoryStream Stream { get; private set; }

            public BinaryWriter Writer { get; private set; }

            public void WritePointer(uint targetOffset, Type type)
            {
                // Add a fixup for the pointer
                _fixups.Add(MakeDefinitionFixup(new CacheAddress(CacheAddressType.Definition, (int)targetOffset)));

                // Just write a zero (this is how it's done officially...)
                Writer.Write(0);
            }

            public object PreSerialize(TagFieldAttribute info, object obj)
            {
                if (obj == null)
                    return null;

                // When serializing a resource address, just add a fixup for it and serialize a null pointer
                if (obj is CacheAddress)
                {
                    _fixups.Add(MakeDefinitionFixup((CacheAddress)obj));
                    return 0U;
                }

                var type = obj.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(D3DPointer<>))
                {
                    // Add a D3D fixup for D3DPointers based on the type of object being pointed to
                    var d3dType = GetD3DObjectType(type.GenericTypeArguments[0]);
                    _d3dFixups.Add(MakeD3DFixup((uint)Stream.Position, d3dType));
                }
                return obj;
            }

            private static int GetD3DObjectType(Type type)
            {
                if (type == typeof(VertexBufferDefinition))
                    return 0;
                if (type == typeof(IndexBufferDefinition))
                    return 1;
                if (type == typeof(BitmapTextureInteropResource.BitmapDefinition))
                    return 2;
                // TODO: interleaved textures
                throw new InvalidOperationException("Invalid D3D object type: " + type);
            }

            public void SuggestAlignment(uint align)
            {
                _align = Math.Max(_align, align);
            }

            public uint Finalize(Stream outStream)
            {
                // Write the data out, aligning the offset and size
                StreamUtil.Align(outStream, (int)_align);
                var dataOffset = (uint)outStream.Position;
                outStream.Write(Stream.GetBuffer(), 0, (int)Stream.Length);
                StreamUtil.Align(outStream, DefaultBlockAlign);

                // Adjust fixups and add them to the resource
                _context.ResourceFixups.AddRange(_fixups.Select(f => FinalizeDefinitionFixup(f, dataOffset)));
                _context.ResourceDefinitionFixups.AddRange(_d3dFixups.Select(f => FinalizeD3DFixup(f, dataOffset)));

                // Free the block data
                Writer.Close();
                Stream = null;
                Writer = null;
                return dataOffset;
            }

            private TagResource.ResourceFixup MakeDefinitionFixup(CacheAddress address)
            {
                return new TagResource.ResourceFixup
                {
                    BlockOffset = (uint)Stream.Position,
                    Address = address
                };
            }

            private TagResource.ResourceDefinitionFixup MakeD3DFixup(uint offset, int typeIndex)
            {
                return new TagResource.ResourceDefinitionFixup
                {
                    Address = new CacheAddress(CacheAddressType.Definition, (int)offset),
                    ResourceStructureTypeIndex = typeIndex
                };
            }
            
            private static TagResource.ResourceFixup FinalizeDefinitionFixup(TagResource.ResourceFixup fixup, uint dataOffset)
            {
                return new TagResource.ResourceFixup
                {
                    BlockOffset = dataOffset + fixup.BlockOffset,
                    Address = fixup.Address
                };
            }

            private static TagResource.ResourceDefinitionFixup FinalizeD3DFixup(TagResource.ResourceDefinitionFixup fixup, uint dataOffset)
            {
                return new TagResource.ResourceDefinitionFixup
                {
                    Address = new CacheAddress(fixup.Address.Type, fixup.Address.Offset + (int)dataOffset),
                    ResourceStructureTypeIndex = fixup.ResourceStructureTypeIndex
                };
            }
        }
    }
}
