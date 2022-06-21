using System;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class SingleTagFileReader
    {
        public SingleTagFileHeader Header;
        public PersistChunkReader Reader;

        public SingleTagFileReader(PersistChunkReader reader)
        {
            Header = reader.Deserialize<SingleTagFileHeader>();
            if (!Header.Valid)
                throw new Exception("Invalid single tag file header");

            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tag!")
                throw new Exception("Invalid single tag file chunk signature");

            Reader = new PersistChunkReader(chunk.Stream, reader.Format);
        }

        public TagLayout ReadLayout(EndianFormat endianness)
        {
            var chunk = Reader.ReadNextChunk();
            if (chunk.Header.Signature != "blay")
                throw new Exception("Invalid tag layout chunk signature");

            var chunkReader = new PersistChunkReader(chunk.Stream, endianness);

            var persistentId = chunkReader.ReadBytes(0x14);
            var layoutVersion = chunkReader.ReadUInt32();
            var layoutHeader = chunkReader.Deserialize<TagPersistLayoutHeader>();

            var layoutChunk = chunkReader.ReadNextChunk();
            if (layoutChunk.Header.Signature != "tgly")
                throw new Exception("Invalid tag layout chunk siganture");

            var layoutReader = new TagPersistLayout(layoutHeader, new PersistChunkReader(layoutChunk.Stream, chunkReader.Format));
            var layout = new TagLayout(layoutReader);
            return layout;
        }

        public byte[] ReadAndFixupData(uint tagId, TagLayout layout, ISingleTagFilePersistContext fixupContext, out uint mainStructOffset)
        {
            var dataReader = new SingleTagFileDataReader(tagId, layout, fixupContext);
            return dataReader.ReadAndFixupData(Reader, out mainStructOffset);
        }

        [TagStructure(Size = 0x40)]
        public class SingleTagFileHeader : TagStructure
        {
            [TagField(Length = 0x24)]
            public byte[] Unknown1;
            public int TagFileVersion;
            public int Unknown2;
            public int Unknown3;
            public Tag GroupTag;
            public int GroupVersion;
            public int Checksum;
            public Tag FooterSignature;

            public bool Valid => FooterSignature == "BLAM";
        }
    }
}
