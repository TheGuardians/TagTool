using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.BlamFile.Reach
{
    // TODO: consolidate with Blf class, just keeping things simple for now

    public class BlfChunk
    {
        public Tag Tag;
        public int Version;
        public byte[] Data;

        public int MajorVerson => (int)(Version >> 16);
    }

    public class BlfReader
    {
        public static IEnumerable<BlfChunk> ReadChunks(Stream stream)
        {
            var reader = new EndianReader(stream, EndianFormat.BigEndian);
            while (!reader.EOF)
            {
                var tag = reader.ReadTag();
                var size = reader.ReadInt32();
                var version = reader.ReadInt32();

                var chunk = new BlfChunk();
                chunk.Tag = tag;
                chunk.Version = version;
                chunk.Data = reader.ReadBytes(size - 0xC);
                yield return chunk;

                if (chunk.Tag == "_eof")
                    break;
            }
        }
    }
}
