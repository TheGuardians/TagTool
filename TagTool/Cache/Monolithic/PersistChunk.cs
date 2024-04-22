using System.IO;
using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public class PersistChunkHeader
    {
        public Tag Signature;
        public int Version;
        public int Size;
    }

    public class PersistChunk
    {
        public PersistChunkHeader Header;
        public Stream Stream;

        public PersistChunk(PersistChunkHeader header, Stream stream)
        {
            Header = header;
            Stream = stream;
        }
    }
}
