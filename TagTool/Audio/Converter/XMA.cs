using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class XMAFile : SoundFile
    {
        public int RealHeaderSize;
        public int TotalSize;
        RIFFChunk RIFF;
        XMAFMTChunk FMT;
        DataChunk Data;

        public XMAFile(byte[] data, int channels, int sampleRate)
        {
            // More like min header size in this case
            HeaderSize = 0x3C;

            Data = new DataChunk(data);
            FMT = new XMAFMTChunk(channels, sampleRate);
            RIFF = new RIFFChunk(data.Length, Data.ChunkSize + FMT.ChunkSize);

            RealHeaderSize = Data.ChunkSize + FMT.ChunkSize + RIFF.ChunkSize;
            TotalSize = RealHeaderSize + Data.GetDataLength();

            // Not so solid verification but whatever
            if (RealHeaderSize >= HeaderSize)
                isValid = true;
        }


        override public void Write(EndianWriter writer)
        {
            if (isValid)
            {
                RIFF.WriteChunk(writer);
                FMT.WriteChunk(writer);
                Data.WriteChunk(writer);
                // Could add a seek chunk if it was ever needed
            }
        }

        override public void Read(EndianReader reader)
        {
            return;
        }


    }
}
