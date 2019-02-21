using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Serialization
{
    /// <summary>
    /// A serialization context for serializing and deserializing resource definition structures.
    /// </summary>
    public class ResourceSerializationContext : ISerializationContext
    {
        private const int DefaultBlockAlign = 0x0;

        private GameCacheContext CacheContext { get; }
        private PageableResource Resource { get; }
        private List<TagResourceGen3.ResourceFixup> ResourceFixups { get; } = new List<TagResourceGen3.ResourceFixup>();
        private List<TagResourceGen3.ResourceDefinitionFixup> ResourceDefinitionFixups { get; } = new List<TagResourceGen3.ResourceDefinitionFixup>();

        public ResourceSerializationContext(GameCacheContext cacheContext, PageableResource resource)
        {
            CacheContext = cacheContext;
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

        public CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            throw new InvalidOperationException("Resource definitions cannot contain tag references");
        }

        public IDataBlock CreateBlock()
        {
            return new ResourceDataBlock(this);
        }

        private Dictionary<(int, CacheAddress), IList> ResourceBlocks = new Dictionary<(int, CacheAddress), IList>();

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            foreach (var key in ResourceBlocks.Keys)
                if (key.Item1 == count && key.Item2 == address)
                    throw new InvalidOperationException();

            ResourceBlocks[(count, address)] = block;
        }

        private class ResourceDataBlock : IDataBlock
        {
            private readonly ResourceSerializationContext _context;
            private readonly List<TagResourceGen3.ResourceFixup> _fixups = new List<TagResourceGen3.ResourceFixup>();
            private readonly List<TagResourceGen3.ResourceDefinitionFixup> _tagStructureFixups = new List<TagResourceGen3.ResourceDefinitionFixup>();
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
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TagStructureReference<>))
                {
                    // Add a D3D fixup for D3DPointers based on the type of object being pointed to
                    var definitionType = GetTagStructureDefinitionType(type.GenericTypeArguments[0]);
                    _tagStructureFixups.Add(MakeTagStructureFixup((uint)Stream.Position, definitionType));
                }
                return obj;
            }

            private static int GetTagStructureDefinitionType(Type type)
            {
                if (type == typeof(VertexBufferDefinition))
                    return 0;
                if (type == typeof(IndexBufferDefinition))
                    return 1;
                if (type == typeof(BitmapTextureInteropResource.BitmapDefinition))
                    return 2;
                // TODO: interleaved textures
                throw new InvalidOperationException("Invalid tag structure type: " + type);
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
                _context.ResourceDefinitionFixups.AddRange(_tagStructureFixups.Select(f => FinalizeD3DFixup(f, dataOffset)));

                // Free the block data
                Writer.Close();
                Stream = null;
                Writer = null;
                return dataOffset;
            }

            private TagResourceGen3.ResourceFixup MakeDefinitionFixup(CacheAddress address)
            {
                return new TagResourceGen3.ResourceFixup
                {
                    BlockOffset = (uint)Stream.Position,
                    Address = address
                };
            }

            private TagResourceGen3.ResourceDefinitionFixup MakeTagStructureFixup(uint offset, int typeIndex)
            {
                return new TagResourceGen3.ResourceDefinitionFixup
                {
                    Address = new CacheAddress(CacheAddressType.Definition, (int)offset),
                    ResourceStructureTypeIndex = typeIndex
                };
            }
            
            private static TagResourceGen3.ResourceFixup FinalizeDefinitionFixup(TagResourceGen3.ResourceFixup fixup, uint dataOffset)
            {
                return new TagResourceGen3.ResourceFixup
                {
                    BlockOffset = dataOffset + fixup.BlockOffset,
                    Address = fixup.Address
                };
            }

            private static TagResourceGen3.ResourceDefinitionFixup FinalizeD3DFixup(TagResourceGen3.ResourceDefinitionFixup fixup, uint dataOffset)
            {
                return new TagResourceGen3.ResourceDefinitionFixup
                {
                    Address = new CacheAddress(fixup.Address.Type, fixup.Address.Offset + (int)dataOffset),
                    ResourceStructureTypeIndex = fixup.ResourceStructureTypeIndex
                };
            }
        }
    }
}
