using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.Cache;

namespace TagTool.Geometry
{
    public static class IndexBufferConverter
    {
        public static void ConvertIndexBuffer(CacheVersion inVersion, CachePlatform inPlatform, CacheVersion outVersion, CachePlatform outPlatform, IndexBufferDefinition indexBuffer)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(indexBuffer.Data.Data))
            {
                var inIndexStream = new IndexBufferStream(inputStream, CacheVersionDetection.IsLittleEndian(inVersion, inPlatform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian);
                var outIndexStream = new IndexBufferStream(outputStream, CacheVersionDetection.IsLittleEndian(outVersion, outPlatform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian);
                var indexCount = indexBuffer.Data.Data.Length / 2;
                for (var j = 0; j < indexCount; j++)
                    outIndexStream.WriteIndex(inIndexStream.ReadIndex());

                indexBuffer.Data.Data = outputStream.ToArray();
            }
        }

        public static IndexBufferDefinition CreateIndexBuffer(int count)
        {
            var newIndexBuffer = new IndexBufferDefinition
            {
                Format = IndexBufferFormat.TriangleStrip,
                Data = new TagData()
            };

            using (var stream = new MemoryStream())
            using (var writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                for (var j = 0; j < count; j++)
                    writer.Write((short)j);
                newIndexBuffer.Data.Data = stream.ToArray();
            }
            return newIndexBuffer;
        }

    }
}
