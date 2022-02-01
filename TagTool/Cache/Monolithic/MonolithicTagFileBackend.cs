using System;
using System.IO;
using TagTool.IO;

namespace TagTool.Cache.Monolithic
{
    public class MonolithicTagFileBackend
    {
        public Guid SessionId;
        public MonolithicTagFileIndex TagFileIndex;
        private FileInfo IndexFile;
        private DirectoryInfo Directory;
        private DirectoryInfo BlobDirectory;
        public EndianFormat Format;

        public MonolithicTagFileBackend(FileInfo indexFile, EndianFormat format)
        {
            IndexFile = indexFile;
            Directory = IndexFile.Directory;
            BlobDirectory = new DirectoryInfo(Path.Combine(Directory.FullName, "blobs"));
            Format = format;

            using (var reader = new PersistChunkReader(IndexFile.OpenRead(), format))
                Load(reader);
        }

        private void Load(PersistChunkReader reader)
        {
            SessionId = new Guid(reader.ReadBytes(16));
            ReadChunks(reader);
        }

        public FileInfo GetTagPartitionFile(int index)
        {
            return new FileInfo(Path.Combine(BlobDirectory.FullName, $"tags_{index}"));
        }

        public FileInfo GetCachePartitionFile(int index)
        {
            return new FileInfo(Path.Combine(BlobDirectory.FullName, $"cache_{index}"));
        }

        public byte[] ExtractTagRaw(TagFileEntry entry)
        {
            if (!TagFileIndex.GetTagFilePartitionBlock(entry, out var partitionBlock))
                return null;

            if (partitionBlock.Size > int.MaxValue)
                throw new Exception("Partition block size too large");

            using (var stream = new EndianReader(GetTagPartitionFile(partitionBlock.PartitionIndex).OpenRead()))
            {
                stream.SeekTo(partitionBlock.Offset);
                return stream.ReadBytes((int)partitionBlock.Size);
            }
        }

        public byte[] ExtractTagRaw(int entryIndex)
        {
            return ExtractTagRaw(TagFileIndex.Index[entryIndex]);
        }

        private void ReadChunks(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    case "tgin":
                        ReadChunks(chunkReader);
                        break;
                    case "mtfi":
                        TagFileIndex = new MonolithicTagFileIndex(chunkReader);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
