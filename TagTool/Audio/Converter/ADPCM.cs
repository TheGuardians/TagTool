using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class XboxADPCM : SoundFile
    {
        public int TotalSize;
        public RIFFChunk RIFF;
        public ADPCMWAVFMTChunk FMT;
        public DataChunk Data;

        public XboxADPCM(byte[] data, int channels, int sampleRate)
        {
            InitFile(data, channels, sampleRate);
        }

        public XboxADPCM(BlamSound blamSound)
        {
            InitFile(blamSound.Data, Encoding.GetChannelCount(blamSound.Encoding), blamSound.SampleRate.GetSampleRateHz());
        }

        public XboxADPCM(EndianReader reader)
        {
            RIFF = new RIFFChunk();
            FMT = new ADPCMWAVFMTChunk();
            Data = new DataChunk();
            Read(reader);
        }

        private void InitFile(byte[] data, int channels, int sampleRate)
        {
            HeaderSize = 0x2C;

            Data = new DataChunk(data);
            FMT = new ADPCMWAVFMTChunk(channels, sampleRate, true);
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

    public class IMAADPCM : SoundFile
    {
        public int TotalSize;
        public RIFFChunk RIFF;
        public ADPCMWAVFMTChunk FMT;
        public DataChunk Data;

        public IMAADPCM(byte[] data, int channels, int sampleRate)
        {
            InitFile(data, channels, sampleRate);
        }

        public IMAADPCM(BlamSound blamSound)
        {
            InitFile(blamSound.Data, Encoding.GetChannelCount(blamSound.Encoding), blamSound.SampleRate.GetSampleRateHz());
        }

        public IMAADPCM(EndianReader reader)
        {
            RIFF = new RIFFChunk();
            FMT = new ADPCMWAVFMTChunk();
            Data = new DataChunk();
            Read(reader);
        }

        private void InitFile(byte[] data, int channels, int sampleRate)
        {
            HeaderSize = 0x2C;

            Data = new DataChunk(data);
            FMT = new ADPCMWAVFMTChunk(channels, sampleRate, false);
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
