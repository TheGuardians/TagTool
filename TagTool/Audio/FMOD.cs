using System;
using System.IO;
using System.Runtime.InteropServices;
using static TagTool.Audio.FMOD;

#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning disable CA1712 // suppress enum type prefix warning

namespace TagTool.Audio
{
    public static class FMOD
    {
        [Flags]
        public enum FMOD_MODE : uint
        {
            FMOD_DEFAULT = 0x00000000,
            FMOD_LOOP_OFF = 0x00000001,
            FMOD_LOOP_NORMAL = 0x00000002,
            FMOD_LOOP_BIDI = 0x00000004,
            FMOD_2D = 0x00000008,
            FMOD_3D = 0x00000010,
            FMOD_CREATESTREAM = 0x00000080,
            FMOD_CREATESAMPLE = 0x00000100,
            FMOD_CREATECOMPRESSEDSAMPLE = 0x00000200,
            FMOD_OPENUSER = 0x00000400,
            FMOD_OPENMEMORY = 0x00000800,
            FMOD_OPENMEMORY_POINT = 0x10000000,
            FMOD_OPENRAW = 0x00001000,
            FMOD_OPENONLY = 0x00002000,
            FMOD_ACCURATETIME = 0x00004000,
            FMOD_MPEGSEARCH = 0x00008000,
            FMOD_NONBLOCKING = 0x00010000,
            FMOD_UNIQUE = 0x00020000,
            FMOD_3D_HEADRELATIVE = 0x00040000,
            FMOD_3D_WORLDRELATIVE = 0x00080000,
            FMOD_3D_INVERSEROLLOFF = 0x00100000,
            FMOD_3D_LINEARROLLOFF = 0x00200000,
            FMOD_3D_LINEARSQUAREROLLOFF = 0x00400000,
            FMOD_3D_INVERSETAPEREDROLLOFF = 0x00800000,
            FMOD_3D_CUSTOMROLLOFF = 0x04000000,
            FMOD_3D_IGNOREGEOMETRY = 0x40000000,
            FMOD_IGNORETAGS = 0x02000000,
            FMOD_LOWMEM = 0x08000000,
            FMOD_VIRTUAL_PLAYFROMSTART = 0x80000000,
        }


        public enum FMOD_RESULT : int
        {
            FMOD_OK,
            FMOD_ERR_BADCOMMAND,
            FMOD_ERR_CHANNEL_ALLOC,
            FMOD_ERR_CHANNEL_STOLEN,
            FMOD_ERR_DMA,
            FMOD_ERR_DSP_CONNECTION,
            FMOD_ERR_DSP_DONTPROCESS,
            FMOD_ERR_DSP_FORMAT,
            FMOD_ERR_DSP_INUSE,
            FMOD_ERR_DSP_NOTFOUND,
            FMOD_ERR_DSP_RESERVED,
            FMOD_ERR_DSP_SILENCE,
            FMOD_ERR_DSP_TYPE,
            FMOD_ERR_FILE_BAD,
            FMOD_ERR_FILE_COULDNOTSEEK,
            FMOD_ERR_FILE_DISKEJECTED,
            FMOD_ERR_FILE_EOF,
            FMOD_ERR_FILE_ENDOFDATA,
            FMOD_ERR_FILE_NOTFOUND,
            FMOD_ERR_FORMAT,
            FMOD_ERR_HEADER_MISMATCH,
            FMOD_ERR_HTTP,
            FMOD_ERR_HTTP_ACCESS,
            FMOD_ERR_HTTP_PROXY_AUTH,
            FMOD_ERR_HTTP_SERVER_ERROR,
            FMOD_ERR_HTTP_TIMEOUT,
            FMOD_ERR_INITIALIZATION,
            FMOD_ERR_INITIALIZED,
            FMOD_ERR_INTERNAL,
            FMOD_ERR_INVALID_FLOAT,
            FMOD_ERR_INVALID_HANDLE,
            FMOD_ERR_INVALID_PARAM,
            FMOD_ERR_INVALID_POSITION,
            FMOD_ERR_INVALID_SPEAKER,
            FMOD_ERR_INVALID_SYNCPOINT,
            FMOD_ERR_INVALID_THREAD,
            FMOD_ERR_INVALID_VECTOR,
            FMOD_ERR_MAXAUDIBLE,
            FMOD_ERR_MEMORY,
            FMOD_ERR_MEMORY_CANTPOINT,
            FMOD_ERR_NEEDS3D,
            FMOD_ERR_NEEDSHARDWARE,
            FMOD_ERR_NET_CONNECT,
            FMOD_ERR_NET_SOCKET_ERROR,
            FMOD_ERR_NET_URL,
            FMOD_ERR_NET_WOULD_BLOCK,
            FMOD_ERR_NOTREADY,
            FMOD_ERR_OUTPUT_ALLOCATED,
            FMOD_ERR_OUTPUT_CREATEBUFFER,
            FMOD_ERR_OUTPUT_DRIVERCALL,
            FMOD_ERR_OUTPUT_FORMAT,
            FMOD_ERR_OUTPUT_INIT,
            FMOD_ERR_OUTPUT_NODRIVERS,
            FMOD_ERR_PLUGIN,
            FMOD_ERR_PLUGIN_MISSING,
            FMOD_ERR_PLUGIN_RESOURCE,
            FMOD_ERR_PLUGIN_VERSION,
            FMOD_ERR_RECORD,
            FMOD_ERR_REVERB_CHANNELGROUP,
            FMOD_ERR_REVERB_INSTANCE,
            FMOD_ERR_SUBSOUNDS,
            FMOD_ERR_SUBSOUND_ALLOCATED,
            FMOD_ERR_SUBSOUND_CANTMOVE,
            FMOD_ERR_TAGNOTFOUND,
            FMOD_ERR_TOOMANYCHANNELS,
            FMOD_ERR_TRUNCATED,
            FMOD_ERR_UNIMPLEMENTED,
            FMOD_ERR_UNINITIALIZED,
            FMOD_ERR_UNSUPPORTED,
            FMOD_ERR_VERSION,
            FMOD_ERR_EVENT_ALREADY_LOADED,
            FMOD_ERR_EVENT_LIVEUPDATE_BUSY,
            FMOD_ERR_EVENT_LIVEUPDATE_MISMATCH,
            FMOD_ERR_EVENT_LIVEUPDATE_TIMEOUT,
            FMOD_ERR_EVENT_NOTFOUND,
            FMOD_ERR_STUDIO_UNINITIALIZED,
            FMOD_ERR_STUDIO_NOT_LOADED,
            FMOD_ERR_INVALID_STRING,
            FMOD_ERR_ALREADY_LOCKED,
            FMOD_ERR_NOT_LOCKED,
            FMOD_ERR_RECORD_DISCONNECTED,
            FMOD_ERR_TOOMANYSAMPLES,

            FMOD_RESULT_FORCEINT = 65536
        }

        public enum FMOD_INITFLAGS
        {
            FMOD_INIT_NORMAL = 0x00000000,
            FMOD_INIT_STREAM_FROM_UPDATE = 0x00000001,
            FMOD_INIT_MIX_FROM_UPDATE = 0x00000002,
            FMOD_INIT_3D_RIGHTHANDED = 0x00000004,
            FMOD_INIT_CHANNEL_LOWPASS = 0x00000100,
            FMOD_INIT_CHANNEL_DISTANCEFILTER = 0x00000200,
            FMOD_INIT_PROFILE_ENABLE = 0x00010000,
            FMOD_INIT_VOL0_BECOMES_VIRTUAL = 0x00020000,
            FMOD_INIT_GEOMETRY_USECLOSEST = 0x00040000,
            FMOD_INIT_PREFER_DOLBY_DOWNMIX = 0x00080000,
            FMOD_INIT_THREAD_UNSAFE = 0x00100000,
            FMOD_INIT_PROFILE_METER_ALL = 0x00200000,
            FMOD_INIT_MEMORY_TRACKING = 0x00400000,
        }

        public enum FMOD_TIMEUNIT
        {
            FMOD_TIMEUNIT_MS = 0x00000001,
            FMOD_TIMEUNIT_PCM = 0x00000002,
            FMOD_TIMEUNIT_PCMBYTES = 0x00000004,
            FMOD_TIMEUNIT_RAWBYTES = 0x00000008,
            FMOD_TIMEUNIT_PCMFRACTION = 0x00000010,
            FMOD_TIMEUNIT_MODORDER = 0x00000100,
            FMOD_TIMEUNIT_MODROW = 0x00000200,
            FMOD_TIMEUNIT_MODPATTERN = 0x00000400,
        }

        public enum FMOD_SOUND_TYPE : int
        {
            FMOD_SOUND_TYPE_UNKNOWN,
            FMOD_SOUND_TYPE_AIFF,
            FMOD_SOUND_TYPE_ASF,
            FMOD_SOUND_TYPE_DLS,
            FMOD_SOUND_TYPE_FLAC,
            FMOD_SOUND_TYPE_FSB,
            FMOD_SOUND_TYPE_IT,
            FMOD_SOUND_TYPE_MIDI,
            FMOD_SOUND_TYPE_MOD,
            FMOD_SOUND_TYPE_MPEG,
            FMOD_SOUND_TYPE_OGGVORBIS,
            FMOD_SOUND_TYPE_PLAYLIST,
            FMOD_SOUND_TYPE_RAW,
            FMOD_SOUND_TYPE_S3M,
            FMOD_SOUND_TYPE_USER,
            FMOD_SOUND_TYPE_WAV,
            FMOD_SOUND_TYPE_XM,
            FMOD_SOUND_TYPE_XMA,
            FMOD_SOUND_TYPE_AUDIOQUEUE,
            FMOD_SOUND_TYPE_AT9,
            FMOD_SOUND_TYPE_VORBIS,
            FMOD_SOUND_TYPE_MEDIA_FOUNDATION,
            FMOD_SOUND_TYPE_MEDIACODEC,
            FMOD_SOUND_TYPE_FADPCM,
            FMOD_SOUND_TYPE_OPUS,

            FMOD_SOUND_TYPE_MAX,
            FMOD_SOUND_TYPE_FORCEINT = 65536
        }

        public enum FMOD_SOUND_FORMAT : int
        {
            FMOD_SOUND_FORMAT_NONE,
            FMOD_SOUND_FORMAT_PCM8,
            FMOD_SOUND_FORMAT_PCM16,
            FMOD_SOUND_FORMAT_PCM24,
            FMOD_SOUND_FORMAT_PCM32,
            FMOD_SOUND_FORMAT_PCMFLOAT,
            FMOD_SOUND_FORMAT_BITSTREAM,

            FMOD_SOUND_FORMAT_MAX,
            FMOD_SOUND_FORMAT_FORCEINT = 65536
        }

        public delegate FMOD_RESULT fn_FMOD_System_Create(out IntPtr system);
        public delegate FMOD_RESULT fn_FMOD_System_Init(IntPtr system, int maxchannels, FMOD_INITFLAGS flags, IntPtr extradriverdata);
        public delegate FMOD_RESULT fn_FMOD_System_Release(IntPtr system);
        public delegate FMOD_RESULT fn_FMOD_System_CreateSound(IntPtr system, [MarshalAs(UnmanagedType.LPStr)] string name, FMOD_MODE mode, IntPtr exinfo, out IntPtr sound);
        public delegate FMOD_RESULT fn_FMOD_System_CreateStream(IntPtr system, [MarshalAs(UnmanagedType.LPStr)] string name, FMOD_MODE mode, IntPtr exinfo, out IntPtr sound);
        public delegate FMOD_RESULT fn_FMOD_System_PlaySound(IntPtr system, IntPtr sound, IntPtr channelgroup, bool paused, out IntPtr channel);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetSubSound(IntPtr sound, int index, out IntPtr subsound);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetNumSubSounds(IntPtr sound, out int numsubsounds);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetLength(IntPtr sound, out uint length, FMOD_TIMEUNIT lengthtype);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetFormat(IntPtr sound, out FMOD_SOUND_TYPE type, out FMOD_SOUND_FORMAT format, out int channels, out int bits);
        public delegate FMOD_RESULT fn_FMOD_Sound_SeekData(IntPtr sound, uint pcm);
        public delegate FMOD_RESULT fn_FMOD_Sound_ReadData(IntPtr sound, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] buffer, uint lenbytes, out uint read);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetDefaults(IntPtr sound, out float frequency, out int priority);
        public delegate FMOD_RESULT fn_FMOD_Sound_GetName(IntPtr sound, out string name, out int namelen);
        public delegate FMOD_RESULT fn_FMOD_Sound_Release(IntPtr sound);

        public static fn_FMOD_System_Create FMOD_System_Create;
        public static fn_FMOD_System_Init FMOD_System_Init;
        public static fn_FMOD_System_Release FMOD_System_Release;
        public static fn_FMOD_System_CreateSound FMOD_System_CreateSound;
        public static fn_FMOD_System_CreateStream FMOD_System_CreateStream;
        public static fn_FMOD_System_PlaySound FMOD_System_PlaySound;
        public static fn_FMOD_Sound_GetSubSound FMOD_Sound_GetSubSound;
        public static fn_FMOD_Sound_GetNumSubSounds FMOD_Sound_GetNumSubSounds;
        public static fn_FMOD_Sound_SeekData FMOD_Sound_SeekData;
        public static fn_FMOD_Sound_ReadData FMOD_Sound_ReadData;
        public static fn_FMOD_Sound_GetLength FMOD_Sound_GetLength;
        public static fn_FMOD_Sound_GetFormat FMOD_Sound_GetFormat;
        public static fn_FMOD_Sound_GetDefaults FMOD_Sound_GetDefaults;
        public static fn_FMOD_Sound_Release FMOD_Sound_Release;

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        static FMOD()
        {
            string platformSuffix = IntPtr.Size == 8 ? "64" : "";
            var modulePath = new FileInfo($@"tools\fmod{platformSuffix}.dll");
            var module = LoadLibrary(modulePath.FullName);
            FMOD_System_Create = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_Create>(GetProcAddress(module, nameof(FMOD_System_Create)));
            FMOD_System_Init = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_Init>(GetProcAddress(module, nameof(FMOD_System_Init)));
            FMOD_System_Release = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_Release>(GetProcAddress(module, nameof(FMOD_System_Release)));
            FMOD_System_CreateSound = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_CreateSound>(GetProcAddress(module, nameof(FMOD_System_CreateSound)));
            FMOD_System_CreateStream = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_CreateStream>(GetProcAddress(module, nameof(FMOD_System_CreateStream)));
            FMOD_System_PlaySound = Marshal.GetDelegateForFunctionPointer<fn_FMOD_System_PlaySound>(GetProcAddress(module, nameof(FMOD_System_PlaySound)));
            FMOD_Sound_GetSubSound = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_GetSubSound>(GetProcAddress(module, nameof(FMOD_Sound_GetSubSound)));
            FMOD_Sound_GetNumSubSounds = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_GetNumSubSounds>(GetProcAddress(module, nameof(FMOD_Sound_GetNumSubSounds)));
            FMOD_Sound_SeekData = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_SeekData>(GetProcAddress(module, nameof(FMOD_Sound_SeekData)));
            FMOD_Sound_ReadData = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_ReadData>(GetProcAddress(module, nameof(FMOD_Sound_ReadData)));
            FMOD_Sound_GetLength = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_GetLength>(GetProcAddress(module, nameof(FMOD_Sound_GetLength)));
            FMOD_Sound_GetFormat = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_GetFormat>(GetProcAddress(module, nameof(FMOD_Sound_GetFormat)));
            FMOD_Sound_GetDefaults = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_GetDefaults>(GetProcAddress(module, nameof(FMOD_Sound_GetDefaults)));
            FMOD_Sound_Release = Marshal.GetDelegateForFunctionPointer<fn_FMOD_Sound_Release>(GetProcAddress(module, nameof(FMOD_Sound_Release)));
        }
    }

    public class FMODException : Exception
    {
        public FMOD_RESULT Result;

        public FMODException(FMOD_RESULT result, string message) : base(message) => Result = result;
        public FMODException(FMOD_RESULT result, string message, Exception innerException) : base(message, innerException) { }
    }
}

#pragma warning restore
