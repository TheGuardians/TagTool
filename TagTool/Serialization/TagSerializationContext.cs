using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Serialization
{
    /// <summary>
    /// A serialization context for serializing and deserializing tags.
    /// </summary>
    public class TagSerializationContext : ISerializationContext
    {
        private const int DefaultBlockAlign = 4;

        private Stream Stream { get; }
        private HaloOnlineCacheContext Context { get; }
        private CachedTagData Data { get; set; }

        /// <summary>
        /// Creates a tag serialization context which serializes data into a tag.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="context">The game cache context.</param>
        /// <param name="tag">The tag to overwrite.</param>
        public TagSerializationContext(Stream stream, HaloOnlineCacheContext context, CachedTagInstance tag)
        {
            Stream = stream;
            Context = context;
            Tag = tag;
        }

        /// <summary>
        /// Gets the tag that the context is operating on.
        /// </summary>
        public CachedTagInstance Tag { get; }

        public void BeginSerialize(TagStructureInfo info)
        {
            Data = new CachedTagData
            {
                Group = new TagGroup
                (
                    tag: info.GroupTag,
                    parentTag: info.ParentGroupTag,
                    grandparentTag: info.GrandparentGroupTag,
                    name: (info.Structure.Name != null) ? Context.GetStringId(info.Structure.Name) : StringId.Invalid
                ),
            };
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Data.MainStructOffset = mainStructOffset;
            Data.Data = data;
            Context.TagCache.SetTagData(Stream, Tag, Data);
            Data = null;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var data = Context.TagCache.ExtractTagRaw(Stream, Tag);
            var reader = new EndianReader(new MemoryStream(data));
            reader.BaseStream.Position = Tag.DefinitionOffset;
            return reader;
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            return Tag.PointerToOffset(address);
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            return (index >= 0 && index < Context.TagCache.Index.Count) ? Context.TagCache.Index[index] : null;
        }

        public IDataBlock CreateBlock()
        {
            return new TagDataBlock(this);
        }

        private class TagDataBlock : IDataBlock
        {
            private readonly TagSerializationContext _context;
            private readonly List<CachedTagData.PointerFixup> _fixups = new List<CachedTagData.PointerFixup>();
            private readonly List<uint> _resourceOffsets = new List<uint>();
            private uint _align = DefaultBlockAlign;

            public TagDataBlock(TagSerializationContext context)
            {
                _context = context;
                Stream = new MemoryStream();
                Writer = new BinaryWriter(Stream);
            }

            public MemoryStream Stream { get; private set; }

            public BinaryWriter Writer { get; private set; }

            public void WritePointer(uint targetOffset, Type type)
            {
                // Add a data fixup for the pointer
                var fixup = MakeFixup(targetOffset);
                _fixups.Add(fixup);

                // Add a resource fixup if this is a resource reference
                if (type == typeof(PageableResource))
                    _resourceOffsets.Add(fixup.WriteOffset);

                // Write the address
                Writer.Write(_context.Tag.OffsetToPointer(targetOffset));
            }

            public object PreSerialize(TagFieldAttribute info, object obj)
            {
                if (obj == null)
                    return null;

                // Get the object type and make sure it's supported
                var type = obj.GetType();
                if (type == typeof(TagData) ||
                    (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(D3DPointer<>)))
                    throw new InvalidOperationException(type + " cannot be serialized as tag data");

                // HACK: If the object is a ResourceReference, fix the Owner property
                if (obj is PageableResource resource)
                    resource.Resource.ParentTag = _context.Tag;

                if (type == typeof(CachedTagInstance))
                {
                    // Object is a tag reference - add it as a dependency
                    if (obj is CachedTagInstance referencedTag && referencedTag != _context.Tag)
                        _context.Data.Dependencies.Add(referencedTag.Index);
                }
                return obj;
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

                // Adjust fixups and add them to the tag
                _context.Data.PointerFixups.AddRange(_fixups.Select(f => FinalizeFixup(f, dataOffset)));
                _context.Data.ResourcePointerOffsets.AddRange(_resourceOffsets.Select(o => o + dataOffset));

                // Free the block data
                Writer.Close();
                Stream = null;
                Writer = null;
                return dataOffset;
            }

            private CachedTagData.PointerFixup MakeFixup(uint targetOffset)
            {
                return new CachedTagData.PointerFixup
                {
                    TargetOffset = targetOffset,
                    WriteOffset = (uint)Stream.Position
                };
            }

            private static CachedTagData.PointerFixup FinalizeFixup(CachedTagData.PointerFixup fixup, uint dataOffset)
            {
                return new CachedTagData.PointerFixup
                {
                    TargetOffset = fixup.TargetOffset,
                    WriteOffset = dataOffset + fixup.WriteOffset
                };
            }
        }
    }
}
