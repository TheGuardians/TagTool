using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Audio
{
    /// <summary>
    /// File header of FMOD FSB format. 48 bytes long. Aligned on 32 bytes. One file per permutation.
    /// </summary>
    public class FMOD_FSB_HEADER            
    {
        /// <summary>
        /// FSB4 ID
        /// </summary>
        uint ID = 0x46534234;
        
        /// <summary>
        /// Number of samples in the file. Usually 1.
        /// </summary>
        int NumSamples;

        /// <summary>
        /// Size of the FMOD_FSB_SAMPLE_HEADER. Usually 0x50 or 0x8.
        /// </summary>
        int ShdrSample;

        /// <summary>
        /// Size in bytes of compressed sample data
        /// </summary>
        int DataSize;

        /// <summary>
        /// Extended FSB Version
        /// </summary>
        uint Version = 0x00004000;

        /// <summary>
        /// Mode flags that apply to all samples.
        /// </summary>
        FMOD_FSB_MODE Mode;

        /// <summary>
        /// MD5 Hash (unused)
        /// </summary>
        ulong Hash = 0x0;

        /// <summary>
        /// FMOD Unique GUID. 16 bytes long.
        /// </summary>
        byte[] GUID = new byte[16];

        [Flags]
        public enum FMOD_FSB_MODE : uint
        {
            None = 0,
            FMOD_FSB_SOURCE_FORMAT = 1 << 0,            /* all samples stored in their original compressed format */
            FMOD_FSB_SOURCE_BASICHEADERS = 1 << 1,      /* samples should use the basic header structure */
            FMOD_FSB_SOURCE_ENCRYPTED = 1 << 2,         /* all sample data is encrypted */
            FMOD_FSB_SOURCE_BIGENDIANPCM = 1 << 3,      /* pcm samples have been written out in big-endian format */
            FMOD_FSB_SOURCE_NOTINTERLEAVED = 1 << 4,    /* Sample data is not interleaved. */
            FMOD_FSB_SOURCE_MPEG_PADDED = 1 << 5,       /* Mpeg frames are now rounded up to the nearest 2 bytes for normal sounds, or 16 bytes for multichannel. */
            FMOD_FSB_SOURCE_MPEG_PADDED4 = 1 << 6,       /* Mpeg frames are now rounded up to the nearest 4 bytes for normal sounds, or 16 bytes for multichannel. */
        }

        public void WriteHeader()
        {

        }

        public void ReadHeader()
        {

        }

    }

    /// <summary>
    /// Sample header of FMOD FSB
    /// </summary>
    public class FMOD_FSB_SAMPLE_HEADER
    {
        /// <summary>
        /// FMOD_FSB_SAMPLE_HEADER size
        /// </summary>
        ushort Size = 0x50;

        /// <summary>
        /// Name of the original file in a char[] of length 30.
        /// </summary>
        char[] Name = new char[30];

        /// <summary>
        /// Number of audio samples in the file
        /// </summary>
        uint SampleCount;

        /// <summary>
        /// Length of the raw data in the file
        /// </summary>
        uint LengthCompressedBytes;

        /// <summary>
        /// Sample index of the loop start
        /// </summary>
        uint LoopStart;  

        /// <summary>
        /// Sample index of the loop end
        /// </summary>
        uint LoopEnd;

        /// <summary>
        /// FMOD_FSB_MODE flags
        /// </summary>
        FMOD_FSB_MODE Mode;

        /// <summary>
        /// Sample rate of the raw audio
        /// </summary>
        int SampleRate;

        ushort Volume;
        short Pan;
        ushort DefPri;
        ushort ChannelCount;
        float MinDistance;
        float MaxDistance;
        uint Size32Bits;
        ushort VarVol;
        short VarPan;

        [Flags]
        public enum FMOD_FSB_MODE : uint
        {
            None = 0,
            FSOUND_LOOP_OFF = 1 << 0,                                   /* For non looping samples. */
            FSOUND_LOOP_NORMAL = 1 << 1,                                /* For forward looping samples. */
            FSOUND_LOOP_BIDI = 1 << 2,                                  /* For bidirectional looping samples.  (no effect if in hardware). */
            FSOUND_8BITS = 1 << 3,                                      /* For 8 bit samples. */
            FSOUND_16BITS = 1 << 4,                                     /* For 16 bit samples. */
            FSOUND_MONO = 1 << 5,                                       /* For mono samples. */
            FSOUND_STEREO = 1 << 6,                                     /* For stereo samples. */
            FSOUND_UNSIGNED = 1 << 7,                                   /* For user created source data containing unsigned samples. */
            FSOUND_SIGNED = 1 << 8,                                     /* For user created source data containing signed data. */
            FSOUND_MPEG = 1 << 9,                                       /* For MPEG layer 2/3 data. */
            FSOUND_CHANNELMODE_ALLMONO = 1 << 10,                       /* Sample is a collection of mono channels. */
            FSOUND_CHANNELMODE_ALLSTEREO = 1 << 11,                     /* Sample is a collection of stereo channel pairs */
            FSOUND_HW3D = 1 << 12,                                      /* Attempts to make samples use 3d hardware acceleration. (if the card supports it) */
            FSOUND_2D = 1 << 13,                                        /* Tells software (not hardware) based sample not to be included in 3d processing. */
            FSOUND_SYNCPOINTS_NONAMES = 1 << 14,                        /* Specifies that syncpoints are present with no names */
            FSOUND_DUPLICATE = 1 << 15,                                 /* This subsound is a duplicate of the previous one i.e. it uses the same sample data but w/different mode bits */
            FSOUND_CHANNELMODE_PROTOOLS = 1 << 16,                      /* Sample is 6ch and uses L C R LS RS LFE standard. */
            FSOUND_MPEGACCURATE = 1 << 17,                              /* For FSOUND_Stream_Open - for accurate FSOUND_Stream_GetLengthMs/FSOUND_Stream_SetTime.  WARNING, see FSOUND_Stream_Open for inital opening time performance issues. */
            FSOUND_HW2D = 1 << 18,                                      /* 2D hardware sounds.  allows hardware specific effects */
            FSOUND_3D = 1 << 19,                                        /* 3D software sounds */
            FSOUND_32BITS = 1 << 20,                                    /* For 32 bit (float) samples. */
            FSOUND_IMAADPCM = 1 << 21,                                  /* Contents are stored compressed as IMA ADPCM */
            FSOUND_VAG = 1 << 22,                                       /* For PS2 only - Contents are compressed as Sony VAG format */
            FSOUND_XMA = 1 << 23,                                       /* For Xbox360 only - Contents are compressed as XMA format */
            FSOUND_GCADPCM = 1 <<24,                                    /* For Gamecube only - Contents are compressed as Gamecube DSP-ADPCM format */
            FSOUND_MULTICHANNEL = 1 <<25,                               /* For PS2 and Gamecube only - Contents are interleaved into a multi-channel (more than stereo) format */
            FSOUND_OGG = 1 << 26,                                       /* For vorbis encoded ogg data */
            FSOUND_MPEG_LAYER3 = 1 <<27,                                /* Data is in MP3 format. */
            FSOUND_MPEG_LAYER2 = 1 <<28,                                /* Data is in MP2 format. */
            FSOUND_IMAADPCMSTEREO = 1 <<29,                             /* Signify IMA ADPCM is actually stereo not two interleaved mono */
            FSOUND_IGNORETAGS = 1 << 30,                                /* Skips id3v2 etc tag checks when opening a stream, to reduce seek/read overhead when opening files (helps with CD performance) */
            FSOUND_SYNCPOINTS = (uint)1 << 31,                          /* Specifies that syncpoints are present */
        }
    }

    /// <summary>
    /// Basic Sample header of FMOD FSB
    /// </summary>
    public class FMOD_FSB_SAMPLE_HEADER_BASIC
    {
        uint LengthSamples;
        uint LengthCompressedBytes;

        public void WriteSampleHeader(EndianWriter writer)
        {
            writer.Write(LengthSamples);
            writer.Write(LengthCompressedBytes);
        }

        public void ReadSampleHeader(EndianReader reader)
        {
            LengthSamples = reader.ReadUInt32();
            LengthCompressedBytes = reader.ReadUInt32();
        }
    }
}
