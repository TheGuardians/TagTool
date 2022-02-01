using System;
using System.IO;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class LruvCache
    {
        public DataArray<LruvBlock> Blocks;
        public int PageBits { get; }
        public int MaxPageCount { get; }
        public DatumHandle FirstBlockIndex { get; private set; }
        public DatumHandle LastBlockIndex { get; private set; }

        public LruvCache(PersistChunkReader reader, int maxPageCount, int pageBits)
        {
            var header = reader.Deserialize<LruvPersistHeader>();
            PageBits = pageBits;
            MaxPageCount = maxPageCount;
            FirstBlockIndex = header.FirstBlockIndex;
            LastBlockIndex = header.LastBlockIndex;
            Blocks = new DataArray<LruvBlock>(reader);

            var tag = reader.ReadTag();
            if (tag != "lvft")
                throw new Exception("invalid lruv footer signature");
        }

        public uint GetBlockAddress(DatumHandle blockIndex)
        {
            return (uint)Blocks.TryGetDatum(blockIndex).FirstPageIndex << PageBits;
        }

        public uint GetBlockSize(DatumHandle blockIndex)
        {
            return (uint)Blocks.TryGetDatum(blockIndex).PageCount << PageBits; 
        }


        [TagStructure(Size = 0x18)]
        public class LruvBlock :  TagStructure, IDatum
        {
            public ushort Identifier;
            public byte Flags;
            [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int PageCount;
            public int FirstPageIndex;
            public int NextBlockIndex;
            public int PreviousBlockIndex;
            public int LastUsedFrameIndex;

            ushort IDatum.Identifier { get => Identifier; set => Identifier = value; }
        }

        [TagStructure(Size = 0x3C)]
        public class LruvPersistHeader : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public int HoleAlgorithm;
            public int PageCount;
            public int Unknown1;
            public int FrameIndex;
            public DatumHandle FirstBlockIndex;
            public DatumHandle LastBlockIndex;
            public Tag Signature;
        }
    }
}
