using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class WAVFile : SoundFile
    {
        public int TotalSize;
        public RIFFChunk RIFF;
        public WAVFMTChunk FMT;
        public DataChunk Data;

        public WAVFile(byte[] data, int channels, int sampleRate)
        {
            InitWAVFile(data, channels, sampleRate);
        }

        public WAVFile(BlamSound blamSound)
        {
            InitWAVFile(blamSound.Data, Encoding.GetChannelCount(blamSound.Encoding), blamSound.SampleRate.GetSampleRateHz());
        }

        public WAVFile(EndianReader reader)
        {
            RIFF = new RIFFChunk();
            FMT = new WAVFMTChunk();
            Data = new DataChunk();
            Read(reader);
        }

        private void InitWAVFile(byte[] data, int channels, int sampleRate)
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
            RIFF.ReadChunk(reader);
            FMT.ReadChunk(reader);
            Data.ReadChunk(reader);
            TotalSize = RIFF.ChunkSize + FMT.ChunkSize + Data.ChunkSize;
        }


    }
}
