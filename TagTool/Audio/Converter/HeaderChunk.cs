using System;
using System.Collections.Generic;
using TagTool.IO;

namespace TagTool.Audio.Converter
{
    abstract class HeaderChunk
    {
        public int ChunkSize;
        public uint Name;


        abstract public void WriteChunk(EndianWriter writer);

        abstract public void ReadChunk(EndianReader reader);
    }

    class RIFFChunk: HeaderChunk
    {
        public uint WAVE;
        public int Size;

        /// <summary>
        /// Create a RIFF chunk. headerSize EXCLUDING RIFF size
        /// </summary>
        /// <param name="dataSize"></param>
        /// <param name="headerSize"></param>
        public RIFFChunk(int dataSize, int headerSize)
        {
            Name = 0x52494646;                      //RIFF
            Size = dataSize + headerSize + 4;       //Get the size of the file from this point on
            WAVE = 0x57415645;                      //WAVE
            ChunkSize = 0xC;                        //Size of the whole chunk
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(Size);
            writer.Format = EndianFormat.BigEndian;
            writer.Write(WAVE);
        }
        
        override public void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.BigEndian;
            if( reader.ReadUInt32() == Name)
            {
                reader.Format = EndianFormat.LittleEndian;
                var tempSize = reader.ReadInt32();

                if(reader.ReadUInt32() == WAVE)
                    Size = tempSize;
                else
                    Size = -1;
            }
            else
                Size = -1;
        }
    }

    class XMAFMTChunk : HeaderChunk
    {
        public int StructureSize;
        public short Format = 0x165;
        public short BitsPerSample = 0x10;
        public short EncodeOptions = 0;
        public short LargestSkip = 0;
        public short NumberOfStreams;
        public byte Loops = 0;
        public byte EncoderVersion = 3;
        List<XMAStream> Streams;


        public XMAFMTChunk(int channels, int sampleRate)
        {
            Name = 0x666D7420;
            ChunkSize = 0x14;
            Streams = new List<XMAStream>();

            // Figure out proper streams for 3.1 and 5.1 encoding.
            switch (channels)
            {
                case 1:
                    Streams.Add(new XMAStream(sampleRate, 1, ChannelMask.XMA_SPEAKER_CENTER));
                    ChunkSize += 0x14;
                    NumberOfStreams = 1;
                    break;
                case 2:
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    ChunkSize += 0x14;
                    NumberOfStreams = 1;
                    break;
                case 4:
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT_BACK | ChannelMask.XMA_SPEAKER_RIGHT_BACK));
                    ChunkSize += 0x28;
                    NumberOfStreams = 2;
                    break;
                case 6:
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT_BACK | ChannelMask.XMA_SPEAKER_RIGHT_BACK));
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_CENTER | ChannelMask.XMA_SPEAKER_LFE));
                    
                    NumberOfStreams = 3;
                    ChunkSize += 0x3C;
                    break;
                default:
                    break;
            }

            StructureSize = ChunkSize - 8;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(StructureSize);
            writer.Write(Format);
            writer.Write(BitsPerSample);
            writer.Write(EncodeOptions);
            writer.Write(LargestSkip);
            writer.Write(NumberOfStreams);
            writer.Write(Loops);
            writer.Write(EncoderVersion);

            for (int i = 0; i < NumberOfStreams; i++)
            {
                Streams[i].Write(writer);
            }
        }

        override public void ReadChunk(EndianReader reader)
        {
            return;
        }
        
    }

    class WAVFMTChunk : HeaderChunk
    {
        int SubchunkSize;
        short PCMLinearQuantization = 1;
        short Channels;
        int SampleRate;
        int ByteRate;
        short BlockAlign;
        short BitsPerSecond;

        public WAVFMTChunk(int channels, int sampleRate, int PCMType = 0x10)
        {
            Name = 0x666D7420;
            SubchunkSize = PCMType;
            Channels = (short)channels;
            SampleRate = sampleRate;
            ByteRate = SampleRate * Channels * 2;
            BlockAlign = (short)(Channels * 2);
            BitsPerSecond = 0x10;
            ChunkSize = 0x18;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(SubchunkSize);
            writer.Write(PCMLinearQuantization);
            writer.Write(Channels);
            writer.Write(SampleRate);
            writer.Write(ByteRate);
            writer.Write(BlockAlign);
            writer.Write(BitsPerSecond);
        }

        override public void ReadChunk(EndianReader reader)
        {
            return;
        }

    }

    class DataChunk : HeaderChunk
    {
        byte[] Data;
        int DataLength;


        public DataChunk(byte[] data)
        {
            Name = 0x64617461;
            Data = data;
            DataLength = data.Length;
            ChunkSize = 8;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(DataLength);
            writer.Write(Data);
        }

        override public void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.BigEndian;
            if (reader.ReadUInt32() == Name)
            {
                reader.Format = EndianFormat.LittleEndian;
                DataLength = reader.ReadInt32();
                Data = reader.ReadBytes(DataLength);
            }
            else
            {
                DataLength = 1;
                Data = null;
            }
                
        }

        public int GetDataLength()
        {
            return DataLength;
        }
    }

    [Flags]
    public enum ChannelMask : short
    {
        None =                          0x00,
        XMA_SPEAKER_LEFT =              0x01,
        XMA_SPEAKER_RIGHT =             0x02,
        XMA_SPEAKER_CENTER =            0x04,
        XMA_SPEAKER_LFE =               0x08,
        XMA_SPEAKER_LEFT_SURROUND =     0x10,
        XMA_SPEAKER_RIGHT_SURROUND =    0x20,
        XMA_SPEAKER_LEFT_BACK =         0x40,
        XMA_SPEAKER_RIGHT_BACK =        0x80
    }

    class XMAStream
    {
        public int BytesPerSecond = 0;
        public int SampleRate;
        public int LoopStart = 0;
        public int LoopEnd = 0;
        public byte SubframeLoopData = 0;
        public byte Channels;
        public ChannelMask Mask;

        public XMAStream(int sampleRate, int channels, ChannelMask mask)
        {
            SampleRate = sampleRate;
            Channels = (byte)channels;
            Mask = mask;
        }

        public void Write(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(BytesPerSecond);
            writer.Write(SampleRate);
            writer.Write(LoopStart);
            writer.Write(LoopEnd);
            writer.Write(SubframeLoopData);
            writer.Write(Channels);
            writer.Write((short)Mask);
        }

        public byte[] Read()
        {
            return null;
        }


    }

    
}
