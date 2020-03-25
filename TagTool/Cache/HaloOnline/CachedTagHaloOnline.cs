using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.HaloOnline
{
    public class CachedTagHaloOnline : CachedTag
    {
        public CachedTagHaloOnline() : base() { }

        public CachedTagHaloOnline(int index, string name = null) : base(index, name) { }

        public CachedTagHaloOnline(int index, TagGroup group, string name = null) : base(index, group, name) { }

        public override uint DefinitionOffset => Offset;

        // Magic constant (NOT a build-specific memory address) used for pointers in tag data
        private const uint FixupPointerBase = 0x40000000;

        // Size of a tag header with no dependencies or offsets
        private const uint TagHeaderSize = 0x24;

        private List<uint> _pointerOffsets = new List<uint>();
        private List<uint> _resourceOffsets = new List<uint>();
        private List<uint> _tagReferenceOffsets = new List<uint>();


        /// <summary>
        /// Gets the offset of the tag's header, or -1 if the tag is not in a file.
        /// </summary>
        public long HeaderOffset { get; internal set; } = -1;

        /// <summary>
        /// Gets the offset of the tag's main structure from the start of its header.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// Gets the total size of the tag (including both its header and data), or 0 if the tag is not in a file.
        /// </summary>
        public uint TotalSize { get; internal set; }

        /// <summary>
        /// Gets the checksum (adler32?) of the tag's data. Ignored if checksum verification is patched out.
        /// </summary>
        public uint Checksum { get; private set; }

        /// <summary>
        /// Gets the indices of tags that this tag depends on.
        /// </summary>
        public ReadOnlySet<int> Dependencies { get; private set; } = new ReadOnlySet<int>(new HashSet<int>());

        /// <summary>
        /// Gets a list of offsets to each pointer in the tag, relative to the beginning of the tag's header.
        /// </summary>
        /// <remarks>
        /// This previously used offsets relative to the beginning of the tag's data.
        /// This was changed in order to speed up loading and be more closer to the engine.
        /// </remarks>
        public IReadOnlyList<uint> PointerOffsets => _pointerOffsets;

        /// <summary>
        /// Gets a list of offsets to each resource pointer in the tag, relative to the beginning of the tag's header.
        /// </summary>
        /// <remarks>
        /// See the remarks for <see cref="PointerOffsets"/>.
        /// </remarks>
        public IReadOnlyList<uint> ResourcePointerOffsets => _resourceOffsets;

        /// <summary>
        /// Gets a list of offsets to each tag reference in the tag, relative to the beginning of the tag's header.
        /// </summary>
        public IReadOnlyList<uint> TagReferenceOffsets => _tagReferenceOffsets;

        /// <summary>
        /// Converts a pointer to an offset relative to the tag's header.
        /// </summary>
        /// <param name="pointer">The pointer to convert.</param>
        /// <returns>The offset.</returns>
        public uint PointerToOffset(uint pointer) => pointer - FixupPointerBase;

        /// <summary>
        /// Converts an offset relative to the tag's header to a pointer.
        /// </summary>
        /// <param name="offset">The offset to convert.</param>
        /// <returns>The pointer.</returns>
        public uint OffsetToPointer(uint offset) => offset + FixupPointerBase;

        /// <summary>
        /// Reads the header for the tag instance from a stream.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        internal void ReadHeader(BinaryReader reader)
        {
            Checksum = reader.ReadUInt32();                        // 0x00 uint32 checksum
            TotalSize = reader.ReadUInt32();                       // 0x04 uint32 total size
            var numDependencies = reader.ReadInt16();              // 0x08 int16  dependencies count
            var numDataFixups = reader.ReadInt16();                // 0x0A int16  data fixup count
            var numResourceFixups = reader.ReadInt16();            // 0x0C int16  resource fixup count
            var numTagReferenceFixups = reader.ReadInt16();        // 0x0E int16  tag reference fixup count(was padding)
            Offset = reader.ReadUInt32();                // 0x10 uint32 main struct offset
            var groupTag = new Tag(reader.ReadInt32());            // 0x14 int32  group tag
            var parentGroupTag = new Tag(reader.ReadInt32());      // 0x18 int32  parent group tag
            var grandparentGroupTag = new Tag(reader.ReadInt32()); // 0x1C int32  grandparent group tag
            var groupName = new StringId(reader.ReadUInt32());     // 0x20 uint32 group name stringid
            Group = new TagGroup(groupTag, parentGroupTag, grandparentGroupTag, groupName);

            // Read dependencies
            var dependencies = new HashSet<int>();
            for (var j = 0; j < numDependencies; j++)
                dependencies.Add(reader.ReadInt32());
            Dependencies = new ReadOnlySet<int>(dependencies);

            // Read offsets
            _pointerOffsets = new List<uint>(numDataFixups);
            for (var j = 0; j < numDataFixups; j++)
                _pointerOffsets.Add(PointerToOffset(reader.ReadUInt32()));
            _resourceOffsets = new List<uint>(numResourceFixups);
            for (var j = 0; j < numResourceFixups; j++)
                _resourceOffsets.Add(PointerToOffset(reader.ReadUInt32()));
            _tagReferenceOffsets = new List<uint>(numTagReferenceFixups);
            for (var i = 0; i < numTagReferenceFixups; i++)
                _tagReferenceOffsets.Add(PointerToOffset(reader.ReadUInt32()));
        }

        /// <summary>
        /// Writes the header for the tag instance to a stream.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        internal void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Checksum);
            writer.Write((uint)TotalSize);
            writer.Write((short)Dependencies.Count);
            writer.Write((short)PointerOffsets.Count);
            writer.Write((short)ResourcePointerOffsets.Count);
            writer.Write((short)TagReferenceOffsets.Count);
            writer.Write(DefinitionOffset);
            writer.Write(Group.Tag.Value);
            writer.Write(Group.ParentTag.Value);
            writer.Write(Group.GrandparentTag.Value);
            writer.Write(Group.Name.Value);

            // Write dependencies
            foreach (var dependency in Dependencies)
                writer.Write(dependency);

            // Write offsets
            foreach (var offset in _pointerOffsets.Concat(_resourceOffsets).Concat(_tagReferenceOffsets))
                writer.Write(OffsetToPointer(offset));
        }

        /// <summary>
        /// Calculates the header size that would be needed for a given block of tag data.
        /// </summary>
        /// <param name="data">The descriptor to use.</param>
        /// <returns>The size of the tag's header.</returns>
        internal static uint CalculateHeaderSize(CachedTagData data) =>
            (uint)(TagHeaderSize +
                data.Dependencies.Count * 4 +
                data.PointerFixups.Count * 4 +
                data.ResourcePointerOffsets.Count * 4 +
                data.TagReferenceOffsets.Count * 4);

        public uint CalculateHeaderSize()
        {
            var size = (uint)(TagHeaderSize + Dependencies.Count * 4 + _pointerOffsets.Count * 4 + _resourceOffsets.Count * 4 + _tagReferenceOffsets.Count * 4);
            return (uint)((size + 0xF) & ~0xF);  // align to 0x10
        }

        /// <summary>
        /// Updates the tag instance's state from a block of tag data.
        /// </summary>
        /// <param name="data">The tag data.</param>
        /// <param name="dataOffset">The offset of the tag data relative to the tag instance's header.</param>
        internal void Update(CachedTagData data, uint dataOffset)
        {
            Group = data.Group;
            Offset = data.MainStructOffset + dataOffset;
            Dependencies = new ReadOnlySet<int>(new HashSet<int>(data.Dependencies));
            _pointerOffsets = data.PointerFixups.Select(fixup => fixup.WriteOffset + dataOffset).ToList();
            _resourceOffsets = data.ResourcePointerOffsets.Select(offset => offset + dataOffset).ToList();
            _tagReferenceOffsets = data.TagReferenceOffsets.Select(offset => offset + dataOffset).ToList();
        }

        public void AddResourceOffset(uint offset)
        {
            _resourceOffsets.Add(offset);
        }
    }
}
