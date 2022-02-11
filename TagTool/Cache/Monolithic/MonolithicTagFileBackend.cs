using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Cache.Monolithic
{
    public class MonolithicTagFileBackend
    {
        public Guid SessionId;
        public MonolithicTagFileIndex TagFileIndex;
        public TagDepdendencyIndex TagDependencyIndex;
        public Dictionary<Tag, TagLayout> TagLayouts;
        private FileInfo IndexFile;
        private DirectoryInfo Directory;
        private DirectoryInfo BlobDirectory;
        public EndianFormat Format;

        [Flags]
        public enum LoadFlags
        {
            TagIndex = 1 << 0,
            TagDependencyIndex = 1 << 1,
            TagLayouts = 1 << 2,
        }

        public MonolithicTagFileBackend(FileInfo indexFile, EndianFormat format, LoadFlags loadFlags)
        {
            IndexFile = indexFile;
            Directory = IndexFile.Directory;
            BlobDirectory = new DirectoryInfo(Path.Combine(Directory.FullName, "blobs"));
            Format = format;

            using (var reader = new PersistChunkReader(IndexFile.OpenRead(), format))
                Load(reader, loadFlags);
        }

        public void LoadAdditional(LoadFlags loadFlags)
        {
            using (var reader = new PersistChunkReader(IndexFile.OpenRead(), Format))
                Load(reader, loadFlags);
        }

        private void Load(PersistChunkReader reader, LoadFlags loadFlags)
        {
            SessionId = new Guid(reader.ReadBytes(16));
            ReadChunks(reader, loadFlags);
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

        private void ReadChunks(PersistChunkReader reader, LoadFlags loadFlags)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    case "tgin":
                        ReadChunks(chunkReader, loadFlags);
                        break;
                    case "mtfi" when loadFlags.HasFlag(LoadFlags.TagIndex):
                        TagFileIndex = new MonolithicTagFileIndex(chunkReader);
                        break;
                    case "mtdp" when loadFlags.HasFlag(LoadFlags.TagDependencyIndex):
                        TagDependencyIndex = new TagDepdendencyIndex(chunkReader);
                        break;
                    case "mreg" when loadFlags.HasFlag(LoadFlags.TagLayouts):
                        TagLayouts = ReadTagGroupLayouts(chunkReader);
                        break;
                    default:
                        break;
                }
            }
        }

        private Dictionary<Tag, TagLayout> ReadTagGroupLayouts(PersistChunkReader rootReader)
        {
            var result = new Dictionary<Tag, TagLayout>();
            foreach (var chunk in rootReader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, rootReader.Format);
                var layoutReader = new TagPersistLayout(chunkReader);
                var layout = new TagLayout(layoutReader);
                result.Add(chunk.Header.Signature, layout);
            }
            return result;
        }
    }
}
