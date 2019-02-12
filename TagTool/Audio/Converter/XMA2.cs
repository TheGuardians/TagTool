using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class XMA2File : SoundFile
    {
        public int RealHeaderSize;
        public int TotalSize;
        RIFFChunk RIFF;
        XMA2FMTChunk FMT;
        DataChunk Data;

        public XMA2File(byte[] data, int channels, int sampleRate, int sampleCount)
        {
            InitXMA2File(data, channels, sampleRate, sampleCount);
        }

        public XMA2File(BlamSound blamSound)
        {
            InitXMA2File(blamSound.Data, Encoding.GetChannelCount(blamSound.Encoding), blamSound.SampleRate.GetSampleRateHz(), (int)blamSound.SampleCount);
        }

        private void InitXMA2File(byte[] data, int channels, int sampleRate, int sampleCount)
        {
            // More like min header size in this case
            HeaderSize = 0x3C;

            Data = new DataChunk(data);
            FMT = new XMA2FMTChunk(channels, sampleRate, sampleCount);
            RIFF = new RIFFChunk(data.Length, Data.ChunkSize + FMT.ChunkSize);

            RealHeaderSize = Data.ChunkSize + FMT.ChunkSize + RIFF.ChunkSize;
            TotalSize = RealHeaderSize + Data.GetDataLength();

            // Not a solid verification but whatever
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
