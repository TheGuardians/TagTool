using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Bitmaps.DDS
{
    public class DDSFile
    {
        public DDSHeader Header;
        public byte[] BitmapData;

        public DDSFile() { }

        public DDSFile(DDSHeader header, byte[] data)
        {
            Header = header;
            BitmapData = data;
        }

        public void Write(EndianWriter writer)
        {
            Header.Write(writer);
            writer.WriteBlock(BitmapData);
        }
        public bool Read(EndianReader reader)
        {
            Header.Read(reader);
            var dataSize = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
            BitmapData = new byte[dataSize];
            reader.ReadBlock(BitmapData, 0, dataSize);
            return true;
        }
    }
}
