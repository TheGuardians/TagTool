using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Audio.Converter
{
    public class FSB4File : SoundFile
    {
        FSB4ArchiveHeader ArchiveHeader;
        FSB4FileHeader File;
        byte[] Data;

        public FSB4File(byte[] data, int channels, int sampleRate, int sampleCount)
        {
            InitFSB4File(data, channels, sampleRate, sampleCount);
        }

        public FSB4File(BlamSound blamSound)
        {
            InitFSB4File(blamSound.Data, Encoding.GetChannelCount(blamSound.Encoding), blamSound.SampleRate.GetSampleRateHz(), (int)blamSound.SampleCount);
        }

        private void InitFSB4File(byte[] data, int channels, int sampleRate, int sampleCount)
        {
            ArchiveHeader = new FSB4ArchiveHeader(data.Length);
            File = new FSB4FileHeader(data.Length, sampleCount, sampleRate, 0, sampleCount - 1, channels);
            Data = data;
        }

        override public void Write(EndianWriter writer)
        {
            ArchiveHeader.WriteChunk(writer);
            File.WriteChunk(writer);
            writer.WriteBlock(Data);
        }

        override public void Read(EndianReader reader)
        {
            return;
        }


        //0x01  |  FSOUND_LOOP_OFF  |  Unknown
        //0x02  |  FSOUND_LOOP_NORMAL  |  Unknown
        //#define FSOUND_LOOP_BIDI     0x00000004  /* For bidirectional looping samples.  (no effect if in hardware). */
        //#define FSOUND_8BITS         0x00000008  /* For 8 bit samples. */
        //#define FSOUND_16BITS        0x00000010  /* For 16 bit samples. */
        //#define FSOUND_MONO          0x00000020  /* For mono samples. */
        //#define FSOUND_STEREO        0x00000040  /* For stereo samples. */
        //#define FSOUND_UNSIGNED      0x00000080  /* For user created source data containing unsigned samples. */
        //#define FSOUND_SIGNED        0x00000100  /* For user created source data containing signed data. */
        //#define FSOUND_DELTA         0x00000200  /* For user created source data stored as delta values. */
        //#define FSOUND_IT214         0x00000400  /* For user created source data stored using IT214 compression. */
        //#define FSOUND_IT215         0x00000800  /* For user created source data stored using IT215 compression. */
        //#define FSOUND_HW3D          0x00001000  /* Attempts to make samples use 3d hardware acceleration. (if the card supports it) */
        //#define FSOUND_2D            0x00002000  /* Tells software (not hardware) based sample not to be included in 3d processing. */
        //#define FSOUND_STREAMABLE    0x00004000  /* For a streamimg sound where you feed the data to it. */
        //#define FSOUND_LOADMEMORY    0x00008000  /* "name" will be interpreted as a pointer to data for streaming and samples. */
        //#define FSOUND_LOADRAW       0x00010000  /* Will ignore file format and treat as raw pcm. */
        //#define FSOUND_MPEGACCURATE  0x00020000  /* For FSOUND_Stream_Open - for accurate FSOUND_Stream_GetLengthMs/FSOUND_Stream_SetTime.  WARNING, see FSOUND_Stream_Open for inital opening time performance issues. */
        //#define FSOUND_FORCEMONO     0x00040000  /* For forcing stereo streams and samples to be mono - needed if using FSOUND_HW3D and stereo data - incurs a small speed hit for streams */
        //#define FSOUND_HW2D          0x00080000  /* 2D hardware sounds.  allows hardware specific effects */
        //#define FSOUND_ENABLEFX      0x00100000  /* Allows DX8 FX to be played back on a sound.  Requires DirectX 8 - Note these sounds cannot be played more than once, be 8 bit, be less than a certain size, or have a changing frequency */
        //#define FSOUND_MPEGHALFRATE  0x00200000  /* For FMODCE only - decodes mpeg streams using a lower quality decode, but faster execution */
        //#define FSOUND_IMAADPCM      0x00400000  /* Contents are stored compressed as IMA ADPCM */
        //#define FSOUND_VAG           0x00800000  /* For PS2 only - Contents are compressed as Sony VAG format */
        //#define FSOUND_NONBLOCKING   0x01000000  /* For FSOUND_Stream_Open/FMUSIC_LoadSong - Causes stream or music to open in the background and not block the foreground app.  See FSOUND_Stream_GetOpenState or FMUSIC_GetOpenState to determine when it IS ready. */
        //#define FSOUND_GCADPCM       0x02000000  /* For Gamecube only - Contents are compressed as Gamecube DSP-ADPCM format */
        //#define FSOUND_MULTICHANNEL  0x04000000  /* For PS2 and Gamecube only - Contents are interleaved into a multi-channel (more than stereo) format */
        //#define FSOUND_USECORE0      0x08000000  /* For PS2 only - Sample/Stream is forced to use hardware voices 00-23 */
        //#define FSOUND_USECORE1      0x10000000  /* For PS2 only - Sample/Stream is forced to use hardware voices 24-47 */
        //#define FSOUND_LOADMEMORYIOP 0x20000000  /* For PS2 only - "name" will be interpreted as a pointer to data for streaming and samples.  The address provided will be an IOP address */
        //#define FSOUND_IGNORETAGS    0x40000000  /* Skips id3v2 etc tag checks when opening a stream, to reduce seek/read overhead when opening files (helps with CD performance) */
        //#define FSOUND_STREAM_NET    0x80000000  /* Specifies an internet stream */

        //#define FSOUND_NORMAL       (FSOUND_16BITS | FSOUND_SIGNED | FSOUND_MONO)      

    }
}
