using System;
using System.Collections.Generic;
using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public  abstract class HeaderChunk
    {
        public int ChunkSize;
        public uint Name;


        abstract public void WriteChunk(EndianWriter writer);

        abstract public void ReadChunk(EndianReader reader);
    }

    public class RIFFChunk : HeaderChunk
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

        public RIFFChunk()
        {
            Name = 0x52494646;                      //RIFF
            WAVE = 0x57415645;                      //WAVE
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
                reader.Format = EndianFormat.BigEndian;
                if (reader.ReadUInt32() == WAVE)
                    Size = tempSize;
                else
                    Size = -1;
            }
            else
                Size = -1;
            reader.Format = EndianFormat.LittleEndian;
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
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_CENTER | ChannelMask.XMA_SPEAKER_LFE));
                    Streams.Add(new XMAStream(sampleRate, 2, ChannelMask.XMA_SPEAKER_LEFT_BACK | ChannelMask.XMA_SPEAKER_RIGHT_BACK));
                    ChunkSize += 0x3C;
                    NumberOfStreams = 3;
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

    class XMA2FMTChunk : HeaderChunk
    {
        public int StructureSize;

        public byte Version = 4;
        public byte NumberOfStreams;
        public byte Reserved = 0;
        public byte LoopCount = 0;
        public int LoopBegin = 0;
        public int LoopEnd;
        public int SampleRate = 44100;
        public int EncodeOptions = 0;
        public int PsuedoBytesPerSec = 0;
        public int BlockSizeInBytes = 0x800;
        public int SamplesEncoded;
        public int SamplesInSource;
        public int BlockCount = 0;

        List<XMA2Stream> Streams;

        public XMA2FMTChunk(int channels, int sampleRate, int sampleCount)
        {
            Name = 0x584d4132;
            ChunkSize = 0x28;
            LoopEnd = 0;
            SamplesEncoded = sampleCount;
            SamplesInSource = sampleCount;

            Streams = new List<XMA2Stream>();

            // Figure out proper streams for 3.1 and 5.1 encoding.
            switch (channels)
            {
                case 1:
                    Streams.Add(new XMA2Stream(1, ChannelMask.XMA_SPEAKER_CENTER));
                    ChunkSize += 0x4;
                    NumberOfStreams = 1;
                    break;

                case 2:
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    ChunkSize += 0x4;
                    NumberOfStreams = 1;
                    break;

                case 4:
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_LEFT_BACK | ChannelMask.XMA_SPEAKER_RIGHT_BACK));
                    ChunkSize += 0x8;
                    NumberOfStreams = 2;
                    break;

                case 6:
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_LEFT | ChannelMask.XMA_SPEAKER_RIGHT));
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_LEFT_BACK | ChannelMask.XMA_SPEAKER_RIGHT_BACK));
                    Streams.Add(new XMA2Stream(2, ChannelMask.XMA_SPEAKER_CENTER | ChannelMask.XMA_SPEAKER_LFE));
                    ChunkSize += 0xC;
                    NumberOfStreams = 3;
                    break;

                default:
                    break;
            }

            StructureSize = ChunkSize;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(StructureSize);
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Version);
            writer.Write(NumberOfStreams);
            writer.Write(Reserved);
            writer.Write(LoopCount);
            writer.Write(LoopBegin);
            writer.Write(LoopEnd);
            writer.Write(SampleRate);
            writer.Write(EncodeOptions);
            writer.Write(PsuedoBytesPerSec);
            writer.Write(BlockSizeInBytes);
            writer.Write(SamplesEncoded);
            writer.Write(SamplesInSource);
            writer.Write(BlockCount);

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

    public class WAVFMTChunk : HeaderChunk
    {
        public int SubchunkSize;
        public short PCMLinearQuantization = 1;
        public short Channels;
        public int SampleRate;
        public int ByteRate;
        public short BlockAlign;
        public short BitsPerSample;

        public WAVFMTChunk(int channels, int sampleRate, int PCMType = 0x10)
        {
            Name = 0x666D7420;
            SubchunkSize = 0x10;
            Channels = (short)channels;
            SampleRate = sampleRate;
            ByteRate = SampleRate * Channels * PCMType / 8;
            BlockAlign = (short)(Channels * PCMType / 8);
            BitsPerSample = (short)PCMType;
            ChunkSize = 0x18;
        }

        public WAVFMTChunk()
        {
            Name = 0x666D7420;
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
            writer.Write(BitsPerSample);
        }

        override public void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.BigEndian;

            if(reader.ReadUInt32() == Name)
            {
                reader.Format = EndianFormat.LittleEndian;
                SubchunkSize = reader.ReadInt32();
                var endOffset = reader.Position + SubchunkSize;
                PCMLinearQuantization = reader.ReadInt16();
                Channels = reader.ReadInt16();
                SampleRate = reader.ReadInt32();
                ByteRate = reader.ReadInt32();
                BlockAlign = reader.ReadInt16();
                BitsPerSample = reader.ReadInt16();
                reader.SeekTo(endOffset);
            }
            reader.Format = EndianFormat.LittleEndian;

        }

    }

    public class ADPCMWAVFMTChunk : HeaderChunk
    {
        public int SubchunkSize;
        public short FormatCode;
        public short Channels;
        public int SampleRate;
        public int ByteRate;
        public short BlockAlign;
        public short BitsPerSample;
        public short ByteExtraData;
        public ushort ExtraData;

        public ADPCMWAVFMTChunk(int channels, int sampleRate, bool isXboxFormat)
        {
            Name = 0x666D7420;
            SubchunkSize = 0x14;
            FormatCode = (short)(isXboxFormat ? 0x0069 : 0x0011);
            Channels = (short)channels;
            SampleRate = sampleRate;
            ByteRate = SampleRate * Channels / 2;
            BlockAlign = (short)(Channels * 36);        // can change if IMA
            BitsPerSample = 4;
            ByteExtraData = 2;
            ExtraData = 0x0040; // can change if IMA

            ChunkSize = SubchunkSize + 4;
        }

        public ADPCMWAVFMTChunk()
        {
            Name = 0x666D7420;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(SubchunkSize);
            writer.Write(FormatCode);
            writer.Write(Channels);
            writer.Write(SampleRate);
            writer.Write(ByteRate);
            writer.Write(BlockAlign);
            writer.Write(BitsPerSample);
            writer.Write(ByteExtraData);
            writer.Write(ExtraData);
        }

        override public void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.BigEndian;

            if (reader.ReadUInt32() == Name)
            {
                reader.Format = EndianFormat.LittleEndian;
                SubchunkSize = reader.ReadInt32();
                var endOffset = reader.Position + SubchunkSize;
                FormatCode = reader.ReadInt16();
                Channels = reader.ReadInt16();
                SampleRate = reader.ReadInt32();
                ByteRate = reader.ReadInt32();
                BlockAlign = reader.ReadInt16();
                BitsPerSample = reader.ReadInt16();
                ByteExtraData = reader.ReadInt16();
                ExtraData = reader.ReadUInt16();
                reader.SeekTo(endOffset);
            }
            reader.Format = EndianFormat.LittleEndian;

        }

    }

    public class DataChunk : HeaderChunk
    {
        public byte[] Data;
        public int DataLength;


        public DataChunk(byte[] data)
        {
            Name = 0x64617461;
            Data = data;
            DataLength = data.Length;
            ChunkSize = 8;
        }

        public DataChunk()
        {
            Name = 0x64617461;
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
            SampleRate = 44100;     // XMA only accept this, convert to th right sample rate after conversion to wav
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

    class XMA2Stream
    {
        byte Channels;
        byte Reserved = 0;
        ChannelMask ChannelMask;

        public XMA2Stream(int channels, ChannelMask mask)
        {
            Channels = (byte)channels;
            ChannelMask = mask;
        }

        public void Write(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Channels);
            writer.Write(Reserved);
            writer.Write((short)ChannelMask);
        }

        public byte[] Read()
        {
            return null;
        }
    }

    class FSB4ArchiveHeader : HeaderChunk
    {
        public int NumberOfFiles = 1;
        public int DirectoryLength = 0x50;
        public int DataLength;
        public int ExtendedVersion = 0x40000;
        public int Flags;
        public int Unused1 = 0;
        public int Unused2 = 0;
        public int Hash1 = 0;
        public int Hash2 = 0;
        public int Hash3 = 0;
        public int Hash4 = 0;

        public FSB4ArchiveHeader(int dataLength, int flags=0x20)
        {
            Name = 0x46534234;
            ChunkSize = 0x30;
            DataLength = dataLength;
            Flags = flags;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.BigEndian;
            writer.Write(Name);
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(NumberOfFiles);
            writer.Write(DirectoryLength);
            writer.Write(DataLength);
            writer.Write(ExtendedVersion);
            writer.Write(Flags);
            writer.Write(Unused1);
            writer.Write(Unused2);
            writer.Write(Hash1);
            writer.Write(Hash2);
            writer.Write(Hash3);
            writer.Write(Hash4);

        }

        override public void ReadChunk(EndianReader reader)
        {

        }
    }

    class FSB4FileHeader : HeaderChunk
    {
        public short Length = 0x50;
        public byte[] FileName = new byte[30];
        public int SampleCount;
        public int CompressedFileLength;
        public int LoopBegin;
        public int LoopEnd;
        public int Mode;
        public int SampleRate;
        public short Volume;
        public short Pan;
        public short Pri;
        public short ChannelCount;
        public float MinDistance;
        public float MaxDistance;
        public int VariableFrequency;
        public short VariableVolume;
        public short VariablePan;

        public FSB4FileHeader(int length, int sampleCount, int sampleRate, int loopBegin, int loopEnd, int channelCount)
        {
            ChunkSize = 0x50;
            SampleCount = sampleCount;
            CompressedFileLength = length;
            LoopBegin = loopBegin;
            LoopEnd = loopEnd;
            Mode = 0x240;
            SampleRate = sampleRate;
            Volume = 0xFF;
            Pan = 0x80;
            Pri = 0x80;
            ChannelCount = (short)channelCount;
            MinDistance = 1.0f;
            MaxDistance = 10000.0f;
            VariableFrequency = 0x50;
            VariableVolume = 0;
            VariablePan = 0;
        }

        override public void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(Length);
            writer.WriteBlock(FileName);
            writer.Write(SampleCount);
            writer.Write(CompressedFileLength);
            writer.Write(LoopBegin);
            writer.Write(LoopEnd);
            writer.Write(Mode);
            writer.Write(SampleRate);
            writer.Write(Volume);
            writer.Write(Pan);
            writer.Write(Pri);
            writer.Write(ChannelCount);
            writer.Write(MinDistance);
            writer.Write(MaxDistance);
            writer.Write(VariableFrequency);
            writer.Write(VariableVolume);
            writer.Write(VariablePan);

        }

        override public void ReadChunk(EndianReader reader)
        {

        }
    }

    [Flags]
    public enum ChannelMask : short
    {
        None = 0x00,
        XMA_SPEAKER_LEFT = 0x01,
        XMA_SPEAKER_RIGHT = 0x02,
        XMA_SPEAKER_CENTER = 0x04,
        XMA_SPEAKER_LFE = 0x08,
        XMA_SPEAKER_LEFT_SURROUND = 0x10,
        XMA_SPEAKER_RIGHT_SURROUND = 0x20,
        XMA_SPEAKER_LEFT_BACK = 0x40,
        XMA_SPEAKER_RIGHT_BACK = 0x80
    }

}

