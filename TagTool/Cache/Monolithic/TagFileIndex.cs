using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagFileEntry
    {
        public uint Id;
        public Tag GroupTag;
        public string Name;
        public WideDatumHandle WideBlockIndex;
    }

    public class TagFileIndex : IReadOnlyList<TagFileEntry>
    {
        public List<TagFileEntry> Files;
        public TagFileIndex(PersistChunkReader reader)
        {
            var header = reader.Deserialize<TagFileIndexHeaderStruct>();
            var compressedEntries = reader.Deserialize<TagFileIndexEntryStruct>(header.TagFileCount).ToList();
            var nameBuffer = new StringBuffer(reader.ReadBytes(header.NameBufferSize));

            Files = new List<TagFileEntry>(header.TagFileCount);
            foreach (var compressedEntry in compressedEntries)
            {
                var entry = new TagFileEntry();
                entry.Id = compressedEntry.Id;
                entry.GroupTag = compressedEntry.GroupTag;
                entry.Name = nameBuffer.GetString(compressedEntry.NameOffset);
                entry.WideBlockIndex = new WideDatumHandle(compressedEntry.WideBlockIndex);
                Files.Add(entry);
            }
        }

        // IReadOnlyList<T> Interface
        public TagFileEntry this[int index] => Files[index];
        public int Count => Files.Count;
        public IEnumerator<TagFileEntry> GetEnumerator() => Files.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        [TagStructure(Size = 0x3C)]
        public class TagFileIndexHeaderStruct : TagStructure
        {
            public uint Size;
            public int Version;
            public int Unknown1;
            public int Unknown2;
            [TagField(Length = 16)]
            public byte[] Guid;
            public int TagFileCount;
            public uint CreatorNameOffset;
            public uint EntriesSize;
            public uint EntriesAddress;
            public int Unknown3;
            public int NameBufferSize;
            public int NameBufferAddress;
        };


        [TagStructure(Size = 0x1C)]
        public class TagFileIndexEntryStruct : TagStructure
        {
            public Tag GroupTag;
            public int Unknown1;
            public int Unknown2;
            public long WideBlockIndex;
            public uint Id;
            public int NameOffset;
        }
    }
}
