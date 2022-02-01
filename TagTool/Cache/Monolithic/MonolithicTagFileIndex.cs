namespace TagTool.Cache.Monolithic
{
    public class MonolithicTagFileIndex
    {
        public TagFileIndex Index;
        public PartitionHeap TagHeap;
        public PartitionHeap CacheHeap;
        public WideDataArray<TagFileBlock> TagFileBlocks;

        public MonolithicTagFileIndex(PersistChunkReader reader)
        {
            ReadChunks(reader);
        }

        private void ReadChunks(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    case "indx": // tag file index
                        Index = new TagFileIndex(chunkReader);
                        break;
                    case "tags": // tag heap
                        TagHeap = new PartitionHeap(chunkReader, new PartitionHeapConfig() { MaxPageCount = 4192256, PageSizeBits = 9 });
                        break;
                    case "cash": // cache heap
                        CacheHeap = new PartitionHeap(chunkReader, new PartitionHeapConfig() { MaxPageCount = 4192256, PageSizeBits = 9 });
                        break;
                    case "blok": // tag file blocks
                        TagFileBlocks = new WideDataArray<TagFileBlock>(chunkReader);
                        break;
                    case "id#6": // build identifier
                        break;
                    case "mtag":
                        ReadChunks(chunkReader);
                        break;
                }
            }
        }

        public bool GetTagFilePartitionBlock(int entryIndex, out PartitionBlock block)
        {
            return GetTagFilePartitionBlock(Index[entryIndex], out block);
        }

        public bool GetCacheFilePartitionBlock(int entryIndex, out PartitionBlock block)
        {
            return GetCacheFilePartitionBlock(Index[entryIndex], out block);
        }

        public bool GetTagFilePartitionBlock(TagFileEntry entry, out PartitionBlock block)
        {
            block = null;

            var tagFileBlock = TagFileBlocks.TryGetDatum(entry.WideBlockIndex);

            if (tagFileBlock == null)
                return false;

            if (tagFileBlock.TagHeapEntryIndex == -1)
                return false;

            block = TagHeap.GetPartitionBlock(tagFileBlock.TagHeapEntryIndex);
            return true;
        }

        public bool GetCacheFilePartitionBlock(TagFileEntry entry, out PartitionBlock block)
        {
            block = null;

            var tagFileBlock = TagFileBlocks.TryGetDatum(entry.WideBlockIndex);

            if (tagFileBlock == null)
                return false;

            if (tagFileBlock.CacheHeapEntryIndex == -1)
                return false;

            block = CacheHeap.GetPartitionBlock(tagFileBlock.CacheHeapEntryIndex);
            return true;
        }
    }
}
