using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagDepdendencyIndex
    {
        public Dictionary<uint, List<uint>> Depndencies = new Dictionary<uint, List<uint>>();

        public TagDepdendencyIndex(PersistChunkReader reader)
        {
            ReadChunks(reader);
        }

        private void ReadChunks(PersistChunkReader reader)
        {
            foreach(var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch(chunk.Header.Signature.ToString())
                {
                    case "tree":
                        ReadChunks(chunkReader);
                        break;
                    case "expl":
                        break;
                    case "opti":
                        ReadCompactDepdendencyIndex(chunkReader);
                        break;
                }
            }
        }

        private void ReadCompactDepdendencyIndex(PersistChunkReader reader)
        {
            var headerSignature = reader.ReadTag();
            if (headerSignature != "<cdi")
                throw new Exception("Invalid compact dependency header signature");

            var header = reader.Deserialize<CompactTagDependencyIndexHeader>();

            if(header.Count > 0)
            {
                for(int i = 0; i < header.Count; i++)
                    ReadCompactDependencyChain(reader);
            }

            if (reader.ReadTag() != "cdi>")
                throw new Exception("Invalid compact dependency footer signature");
        }

        private void ReadCompactDependencyChain(PersistChunkReader reader)
        {
            if (reader.ReadTag() != "<cdc")
                throw new Exception("Invalid tag dependency chain header");

            var chainHeader = reader.Deserialize<TagDepndencyChainHeader>();

            var dependencies = new List<uint>(chainHeader.DependencyCount);
            for (int i = 0; i < chainHeader.DependencyCount; i++)
                dependencies.Add(reader.ReadUInt32());

            Depndencies.Add(chainHeader.TagId, dependencies);

            if (reader.ReadTag() != "cdc>")
                throw new Exception("Invalid tag dependency chain footer");
        }

        [TagStructure(Size = 0x8)]
        public class CompactTagDependencyIndexHeader : TagStructure
        {
            public int Count;
            public int Unknown2;
        }

        [TagStructure(Size = 0x8)]
        public class TagDepndencyChainHeader : TagStructure
        {
            public uint TagId;
            public int DependencyCount;
        }
    }
}
