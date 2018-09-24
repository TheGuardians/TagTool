using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class WAVFile : SoundFile
    {
        public int TotalSize;
        RIFFChunk RIFF;
        WAVFMTChunk FMT;
        DataChunk Data;

        public WAVFile(byte[] data, int channels, int sampleRate)
        {
            HeaderSize = 0x2C;

            Data = new DataChunk(data);
            FMT = new WAVFMTChunk(channels, sampleRate);
            RIFF = new RIFFChunk(data.Length, Data.ChunkSize + FMT.ChunkSize);

            if (Data.ChunkSize + FMT.ChunkSize + RIFF.ChunkSize == HeaderSize)
                isValid = true;
                TotalSize = HeaderSize + Data.GetDataLength();
        }


        override public void Write(EndianWriter writer)
        {
            if (isValid)
            {
                RIFF.WriteChunk(writer);
                FMT.WriteChunk(writer);
                Data.WriteChunk(writer);
            }
        }

        override public void Read(EndianReader reader)
        {
            return;
        }


    }
}
