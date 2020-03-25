using TagTool.Cache;
using TagTool.Cache.HaloOnline;
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
    public class RuntimeSerializationContext : ISerializationContext
    {
        private const int DefaultBlockAlign = 4;

        private Stream Stream { get; }
        protected GameCacheHaloOnlineBase Context { get; }
        private CachedTagData Data { get; set; }

        /// <summary>
        /// Gets the tag that the context is operating on.
        /// </summary>
        public CachedTagHaloOnline Tag { get; }
        public long TagOffset { get; set; }
        public uint AlignedHeaderSize { get; set; }

        /// <summary>
        /// Creates a tag serialization context which serializes data into a tag.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="context">The game cache context.</param>
        /// <param name="tag">The tag to overwrite.</param>
        public RuntimeSerializationContext(Stream stream, GameCacheHaloOnlineBase context, CachedTagHaloOnline tag, long tagoffset)
        {
            Stream = stream;
            Context = context;
            Tag = tag;
            TagOffset = tagoffset;
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
            // Get header size
            var headerSize = CachedTagHaloOnline.CalculateHeaderSize(Data);
            AlignedHeaderSize = (uint)((headerSize + 0xF) & ~0xF);
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Data.MainStructOffset = mainStructOffset;
            Data.Data = data;

            //this is an inlined version of Context.TagCacheGenHO.SetTagData(Stream, Tag, Data);
            if (Tag == null)
                throw new ArgumentNullException(nameof(Tag));
            else if (Data == null)
                throw new ArgumentNullException(nameof(Data));
            else if (Data.Group == TagGroup.None)
                throw new ArgumentException("Cannot assign a tag to a null tag group");
            else if (Data.Data == null)
                throw new ArgumentException("The tag data buffer is null");

            var alignedLength = (Data.Data.Length + 0xF) & ~0xF;

            // Write in the new data
            var writer = new EndianWriter(Stream, EndianFormat.LittleEndian);
            Stream.Position = TagOffset + AlignedHeaderSize;
            Stream.Write(Data.Data, 0, Data.Data.Length);
            StreamUtil.Fill(Stream, 0, alignedLength - Data.Data.Length);

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
            return Context.TagCacheGenHO.GetTag(index);
        }

        public virtual CachedTag GetTagByName(TagGroup group, string name)
        {
            return Context.TagCache.GetTag(name, group.Tag);
        }

        public IDataBlock CreateBlock()
        {
            return new TagDataBlock(this, TagOffset, AlignedHeaderSize);
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class TagDataBlock : IDataBlock
        {
            private uint _align = DefaultBlockAlign;
            private long TagOffset;
            private uint HeaderSize;
            
            public TagDataBlock(RuntimeSerializationContext context, long offset, uint headersize)
            {
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
                TagOffset = offset;
                HeaderSize = headersize;
            }

            public MemoryStream Stream { get; private set; }

            public EndianWriter Writer { get; private set; }

            public void WritePointer(uint targetOffset, Type type)
            {
                // Write the address
                Writer.Write(TagOffset + HeaderSize + targetOffset);
            }

            public object PreSerialize(TagFieldAttribute info, object obj)
            {
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

                // Free the block data
                Writer.Close();
                Stream = null;
                Writer = null;
                return dataOffset;
            }

            // add position of tag index from tag references in definition
            public void AddTagReference(CachedTag referencedTag, bool isShort)
            {
            }
        }
    }
}
