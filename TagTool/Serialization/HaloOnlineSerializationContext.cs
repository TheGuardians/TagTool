using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;
using System.Collections;

namespace TagTool.Serialization
{
    /// <summary>
    /// A serialization context for serializing and deserializing tags.
    /// </summary>
    public class HaloOnlineSerializationContext : ISerializationContext
    {
        private const int DefaultBlockAlign = 4;

        private Stream Stream { get; }
        protected GameCacheContextHaloOnline Context { get; }
        private CachedTagData Data { get; set; }

        /// <summary>
        /// Gets the tag that the context is operating on.
        /// </summary>
        public CachedTagHaloOnline Tag { get; }

        /// <summary>
        /// Creates a tag serialization context which serializes data into a tag.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="context">The game cache context.</param>
        /// <param name="tag">The tag to overwrite.</param>
        public HaloOnlineSerializationContext(Stream stream, GameCacheContextHaloOnline context, CachedTagHaloOnline tag)
        {
            Stream = stream;
            Context = context;
            Tag = tag;
        }

        public void BeginSerialize(TagStructureInfo info)
        {
            Data = new CachedTagData
            {
                Group = new TagGroup
                (
                    tag: info.GroupTag,
                    parentTag: info.ParentGroupTag,
                    grandparentTag: info.GrandparentGroupTag,
                    name: (info.Structure.Name != null) ? Context.StringTable.GetStringId(info.Structure.Name) : StringId.Invalid
                ),
            };
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Data.MainStructOffset = mainStructOffset;
            Data.Data = data;
            Context.TagCacheGenHO.SetTagData(Stream, Tag, Data);
            Data = null;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var data = Context.TagCacheGenHO.ExtractTagRaw(Stream, Tag);
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

        public virtual CachedTag GetTagByIndex(int index)
        {
            return Context.TagCacheGenHO.GetTagByIndex(index);
        }

        public virtual CachedTag GetTagByName(TagGroup group, string name)
        {
            return Context.TagCache.GetTagByName(name, group.Tag);
        }

        public IDataBlock CreateBlock()
        {
            return new TagDataBlock(this);
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class TagDataBlock : IDataBlock
        {
            private readonly HaloOnlineSerializationContext _context;
            private readonly List<CachedTagData.PointerFixup> _fixups = new List<CachedTagData.PointerFixup>();
            private readonly List<uint> _resourceOffsets = new List<uint>();
            private readonly List<uint> _tagReferenceOffsets = new List<uint>();
            private uint _align = DefaultBlockAlign;

            public TagDataBlock(HaloOnlineSerializationContext context)
            {
                _context = context;
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
            }

            public MemoryStream Stream { get; private set; }

            public EndianWriter Writer { get; private set; }

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
                    (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TagStructureReference<>)))
                    throw new InvalidOperationException(type + " cannot be serialized as tag data");

                // HACK: If the object is a ResourceReference, fix the Owner property
                if (obj is PageableResource resource)
                    resource.Resource.ParentTag = _context.Tag;

                if (type == typeof(CachedTag))
                {
                    // Object is a tag reference - add it as a dependency
                    if (obj is CachedTag referencedTag && referencedTag != _context.Tag)
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
                _context.Data.TagReferenceOffsets.AddRange(_tagReferenceOffsets.Select(o => o + dataOffset));

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

            // add position of tag index from tag references in definition
            public void AddTagReference(CachedTag referencedTag, bool isShort)
            {
                if(isShort)
                    _tagReferenceOffsets.Add((uint)Stream.Position);
                else
                    _tagReferenceOffsets.Add((uint)Stream.Position + 0xC);
            }
        }
    }
}
