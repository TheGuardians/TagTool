using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class PartitionHeapConfig
    {
        public int MaxPageCount;
        public int PageSizeBits;
    }
    public class PartitionHeap
    {
        public PartitionHeapConfig Config;
        public List<PartitionedHeapEntry> Entries;
        public List<LruvCache> Partitions;

        public PartitionHeap(PersistChunkReader reader, PartitionHeapConfig config)
        {
            Config = config;
            ReadChunks(reader);
        }

        void ReadChunks(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    
                    case "mtag":
                    case "disk":
                    case "heap":
                        ReadChunks(chunkReader);
                        break;
                    case "hpls": // partition list
                        ReadHeapEntries(chunkReader);
                        break;
                    case "ptls": // heap entries
                        ReadPartitionList(chunkReader);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReadPartitionList(PersistChunkReader reader)
        {
            Partitions = new List<LruvCache>();
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                if (chunk.Header.Signature == "part")
                {
                    var index = chunkReader.ReadInt32();
                    var partition = new LruvCache(chunkReader, Config.MaxPageCount, Config.PageSizeBits);
                    Partitions.Add(partition);
                }
            }
        }

        private void ReadHeapEntries(PersistChunkReader reader)
        {
            var header = reader.Deserialize<PartitionedHeapEntryListHeader>();
            Entries = reader.Deserialize<PartitionedHeapEntry>(header.EntryCount).ToList();
        }
        
        public PartitionBlock GetPartitionBlock(int entryIndex)
        {
            var heapEntry = Entries[entryIndex];
            var partition = Partitions[heapEntry.PartitionIndex];
            var offset = partition.GetBlockAddress(heapEntry.BlockIndex);
            var size = partition.GetBlockSize(heapEntry.BlockIndex);
            return new PartitionBlock() { PartitionIndex = heapEntry.PartitionIndex, Offset = offset, Size = size };
        }

        [TagStructure(Size = 0x8)]
        public class PartitionedHeapEntryListHeader : TagStructure
        {
            public int EntryCount;
            public int Unknown1;
        }

        [TagStructure(Size = 0x8)]
        public class PartitionedHeapEntry : TagStructure
        {
            public uint PackedInfo;
            public DatumHandle BlockIndex;
            public int PartitionIndex => (int)((PackedInfo << 2) >> 2);
        }
    }
}
