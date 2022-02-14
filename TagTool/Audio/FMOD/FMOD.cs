using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TagTool.Audio
{
    public unsafe static class FMOD
    {
        private static object GlobalMutex = new object();

        public class FMODSound : IDisposable
        {
            public FMODSystem SystemFMOD;
            public IntPtr Handle;

            public FMODSound(FMODSystem system, IntPtr handle)
            {
                SystemFMOD = system;
                Handle = handle;
                SystemFMOD.AddNativeHandle(handle, FMODHandleType.Sound);
            }

            public FMOD_RESULT GetLength(out uint length, FMOD_TIMEUNIT lengthtype)
            {
                return FMOD_Sound_GetLength(Handle, out length, lengthtype);
            }

            public FMOD_RESULT GetFormat(out FMOD_SOUND_TYPE type, out FMOD_SOUND_FORMAT format, out int channels, out int bits)
            {
                return FMOD_Sound_GetFormat(Handle, out type, out format, out channels, out bits);
            }

            public FMOD_RESULT GetDefaults(out float frequency, out int priority)
            {
                return FMOD_Sound_GetDefaults(Handle, out frequency, out priority);
            }

            public FMOD_RESULT GetSubSound(int index, out FMODSound subsound)
            {
                subsound = null;
                FMOD_RESULT result = FMOD_Sound_GetSubSound(Handle, index, out IntPtr handle);
                if (result == FMOD_RESULT.OK)
                    subsound = new FMODSound(SystemFMOD, handle);
                return result;
            }

            public FMOD_RESULT SeekData(uint pcm)
            {
                return FMOD_Sound_SeekData(Handle, pcm);
            }

            public FMOD_RESULT ReadData(byte[] buffer, uint lenbytes, out uint read)
            {
                return FMOD_Sound_ReadData(Handle, buffer, lenbytes, out read);
            }

            public FMOD_RESULT GetName(out string name)
            {
                fixed (byte* ptr = new byte[128])
                {
                    name = null;
                    FMOD_RESULT result = FMOD_Sound_GetName(Handle, ptr, 128);
                    if (result == FMOD_RESULT.OK)
                        name = System.Text.Encoding.UTF8.GetString(ptr, 128).TrimEnd('\0');
                    return result;
                }
            }

            private FMOD_RESULT Release()
            {
                if (Handle == IntPtr.Zero)
                    return FMOD_RESULT.OK;

                SystemFMOD.RemoveNativeHandle(Handle);
                var result = FMOD_Sound_Release(Handle);
                Handle = IntPtr.Zero;
                return result;
            }

            public void Dispose()
            {
                Release();
            }
        }

        public class FMODSystem : IDisposable
        {
            public IntPtr Handle;
            private Dictionary<IntPtr, TrackedHandle> TrackedHandles;
            private bool disposedValue;

            public FMODSystem(IntPtr handle)
            {
                Handle = handle;
                TrackedHandles = new Dictionary<IntPtr, TrackedHandle>();
            }

            ~FMODSystem()
            {
                Dispose(false);
            }

            public static FMOD_RESULT Create(out FMODSystem system)
            {
                lock (GlobalMutex)
                {
                    system = null;
                    FMOD_RESULT result = FMOD_System_Create(out IntPtr handle);
                    if (result == FMOD_RESULT.OK)
                        system = new FMODSystem(handle);
                    return result;
                }
            }

            public FMOD_RESULT Init(int channels, FMOD_INITFLAGS flags)
            {
                return FMOD_System_Init(Handle, channels, flags, IntPtr.Zero);
            }

            public FMOD_RESULT CreateSound(string name, FMOD_MODE mode, out FMODSound sound)
            {
                sound = null;
                FMOD_RESULT result = FMOD_System_CreateSound(Handle, name, mode, IntPtr.Zero, out IntPtr handle);
                if (result == FMOD_RESULT.OK)
                    sound = new FMODSound(this, handle);
                return result;
            }

            public FMOD_RESULT CreateStream(string name, FMOD_MODE mode, out FMODSound sound)
            {
                sound = null;
                FMOD_RESULT result = FMOD_System_CreateStream(Handle, name, mode, IntPtr.Zero, out IntPtr handle);
                if (result == FMOD_RESULT.OK)
                    sound = new FMODSound(this, handle);
                return result;
            }

            public FMOD_RESULT PlaySound(FMODSound sound)
            {
                return FMOD_System_PlaySound(Handle, sound.Handle, IntPtr.Zero, false, out IntPtr channel);
            }


            #region Dispose Stuff

            protected virtual void Dispose(bool disposing)
            {
                lock (GlobalMutex)
                {
                    // release tracked handles
                    if (!disposedValue)
                    {
                        foreach (var trackedHandle in TrackedHandles.Values)
                        {
                            switch (trackedHandle.Type)
                            {
                                case FMODHandleType.Sound:
                                    FMOD_Sound_Release(trackedHandle.Handle);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        FMOD_System_Release(Handle);
                        disposedValue = true;
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion

            #region Handle Tracking

            struct TrackedHandle
            {
                public FMODHandleType Type;
                public IntPtr Handle;

                public TrackedHandle(FMODHandleType type, IntPtr handle)
                {
                    Type = type;
                    Handle = handle;
                }
            }

            public void AddNativeHandle(IntPtr ptr, FMODHandleType type)
            {
                lock (TrackedHandles)
                    TrackedHandles.Add(ptr, new TrackedHandle(type, ptr));
            }

            public void RemoveNativeHandle(IntPtr ptr)
            {
                lock (TrackedHandles)
                    TrackedHandles.Remove(ptr);
            }
            #endregion
        }

        #region typedef

        public enum FMODHandleType
        {
            System,
            Sound
        }

        public enum FMOD_RESULT : int
        {
            OK,
            ERR_BADCOMMAND,
            ERR_CHANNEL_ALLOC,
            ERR_CHANNEL_STOLEN,
            ERR_DMA,
            ERR_DSP_CONNECTION,
            ERR_DSP_DONTPROCESS,
            ERR_DSP_FORMAT,
            ERR_DSP_INUSE,
            ERR_DSP_NOTFOUND,
            ERR_DSP_RESERVED,
            ERR_DSP_SILENCE,
            ERR_DSP_TYPE,
            ERR_FILE_BAD,
            ERR_FILE_COULDNOTSEEK,
            ERR_FILE_DISKEJECTED,
            ERR_FILE_EOF,
            ERR_FILE_ENDOFDATA,
            ERR_FILE_NOTFOUND,
            ERR_FORMAT,
            ERR_HEADER_MISMATCH,
            ERR_HTTP,
            ERR_HTTP_ACCESS,
            ERR_HTTP_PROXY_AUTH,
            ERR_HTTP_SERVER_ERROR,
            ERR_HTTP_TIMEOUT,
            ERR_INITIALIZATION,
            ERR_INITIALIZED,
            ERR_INTERNAL,
            ERR_INVALID_FLOAT,
            ERR_INVALID_HANDLE,
            ERR_INVALID_PARAM,
            ERR_INVALID_POSITION,
            ERR_INVALID_SPEAKER,
            ERR_INVALID_SYNCPOINT,
            ERR_INVALID_THREAD,
            ERR_INVALID_VECTOR,
            ERR_MAXAUDIBLE,
            ERR_MEMORY,
            ERR_MEMORY_CANTPOINT,
            ERR_NEEDS3D,
            ERR_NEEDSHARDWARE,
            ERR_NET_CONNECT,
            ERR_NET_SOCKET_ERROR,
            ERR_NET_URL,
            ERR_NET_WOULD_BLOCK,
            ERR_NOTREADY,
            ERR_OUTPUT_ALLOCATED,
            ERR_OUTPUT_CREATEBUFFER,
            ERR_OUTPUT_DRIVERCALL,
            ERR_OUTPUT_FORMAT,
            ERR_OUTPUT_INIT,
            ERR_OUTPUT_NODRIVERS,
            ERR_PLUGIN,
            ERR_PLUGIN_MISSING,
            ERR_PLUGIN_RESOURCE,
            ERR_PLUGIN_VERSION,
            ERR_RECORD,
            ERR_REVERB_CHANNELGROUP,
            ERR_REVERB_INSTANCE,
            ERR_SUBSOUNDS,
            ERR_SUBSOUND_ALLOCATED,
            ERR_SUBSOUND_CANTMOVE,
            ERR_TAGNOTFOUND,
            ERR_TOOMANYCHANNELS,
            ERR_TRUNCATED,
            ERR_UNIMPLEMENTED,
            ERR_UNINITIALIZED,
            ERR_UNSUPPORTED,
            ERR_VERSION,
            ERR_EVENT_ALREADY_LOADED,
            ERR_EVENT_LIVEUPDATE_BUSY,
            ERR_EVENT_LIVEUPDATE_MISMATCH,
            ERR_EVENT_LIVEUPDATE_TIMEOUT,
            ERR_EVENT_NOTFOUND,
            ERR_STUDIO_UNINITIALIZED,
            ERR_STUDIO_NOT_LOADED,
            ERR_INVALID_STRING,
            ERR_ALREADY_LOCKED,
            ERR_NOT_LOCKED,
            ERR_RECORD_DISCONNECTED,
            ERR_TOOMANYSAMPLES,
        }

        [Flags]
        public enum FMOD_MODE : uint
        {
            DEFAULT = 0x00000000,
            LOOP_OFF = 0x00000001,
            LOOP_NORMAL = 0x00000002,
            LOOP_BIDI = 0x00000004,
            _2D = 0x00000008,
            _3D = 0x00000010,
            CREATESTREAM = 0x00000080,
            CREATESAMPLE = 0x00000100,
            CREATECOMPRESSEDSAMPLE = 0x00000200,
            OPENUSER = 0x00000400,
            OPENMEMORY = 0x00000800,
            OPENMEMORY_POINT = 0x10000000,
            OPENRAW = 0x00001000,
            OPENONLY = 0x00002000,
            ACCURATETIME = 0x00004000,
            MPEGSEARCH = 0x00008000,
            NONBLOCKING = 0x00010000,
            UNIQUE = 0x00020000,
            _3D_HEADRELATIVE = 0x00040000,
            _3D_WORLDRELATIVE = 0x00080000,
            _3D_INVERSEROLLOFF = 0x00100000,
            _3D_LINEARROLLOFF = 0x00200000,
            _3D_LINEARSQUAREROLLOFF = 0x00400000,
            _3D_INVERSETAPEREDROLLOFF = 0x00800000,
            _3D_CUSTOMROLLOFF = 0x04000000,
            _3D_IGNOREGEOMETRY = 0x40000000,
            IGNORETAGS = 0x02000000,
            LOWMEM = 0x08000000,
            VIRTUAL_PLAYFROMSTART = 0x80000000,
        }

        [Flags]
        public enum FMOD_INITFLAGS
        {
            NORMAL = 0x00000000,
            STREAM_FROM_UPDATE = 0x00000001,
            MIX_FROM_UPDATE = 0x00000002,
            _3D_RIGHTHANDED = 0x00000004,
            CHANNEL_LOWPASS = 0x00000100,
            CHANNEL_DISTANCEFILTER = 0x00000200,
            PROFILE_ENABLE = 0x00010000,
            VOL0_BECOMES_VIRTUAL = 0x00020000,
            GEOMETRY_USECLOSEST = 0x00040000,
            PREFER_DOLBY_DOWNMIX = 0x00080000,
            THREAD_UNSAFE = 0x00100000,
            PROFILE_METER_ALL = 0x00200000,
            MEMORY_TRACKING = 0x00400000,
        }

        [Flags]
        public enum FMOD_TIMEUNIT
        {
            MS = 0x00000001,
            PCM = 0x00000002,
            PCMBYTES = 0x00000004,
            RAWBYTES = 0x00000008,
            PCMFRACTION = 0x00000010,
            MODORDER = 0x00000100,
            MODROW = 0x00000200,
            MODPATTERN = 0x00000400,
        }

        public enum FMOD_SOUND_TYPE : int
        {
            UNKNOWN,
            AIFF,
            ASF,
            DLS,
            FLAC,
            FSB,
            IT,
            MIDI,
            MOD,
            MPEG,
            OGGVORBIS,
            PLAYLIST,
            RAW,
            S3M,
            USER,
            WAV,
            XM,
            XMA,
            AUDIOQUEUE,
            AT9,
            VORBIS,
            MEDIA_FOUNDATION,
            MEDIACODEC,
            FADPCM,
            OPUS,

            MAX,
        }

        public enum FMOD_SOUND_FORMAT : int
        {
            NONE,
            PCM8,
            PCM16,
            PCM24,
            PCM32,
            PCMFLOAT,
            BITSTREAM,

            MAX,
        }

        public enum FMOD_CHANNELORDER : int
        {
            DEFAULT,
            WAVEFORMAT,
            PROTOOLS,
            ALLMONO,
            ALLSTEREO,
            ALSA,

            MAX,
        }

        #endregion

        #region Imports

#if _x64
        public const string DllPath = "fmod64.dll";
#else
        public const string DllPath = "fmod.dll";
#endif

        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_Create(out IntPtr system);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_Release(IntPtr system);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_Init(IntPtr system, int maxchannels, FMOD_INITFLAGS flags, IntPtr extradriverdata);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_CreateSound(IntPtr system, [MarshalAs(UnmanagedType.LPStr)] string name, FMOD_MODE mode, IntPtr exinfo, out IntPtr sound);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_CreateStream(IntPtr system, [MarshalAs(UnmanagedType.LPStr)] string name, FMOD_MODE mode, IntPtr exinfo, out IntPtr sound);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_System_PlaySound(IntPtr system, IntPtr sound, IntPtr channelgroup, bool paused, out IntPtr channel);

        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_GetSubSound(IntPtr sound, int index, out IntPtr subsound);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_GetNumSubSounds(IntPtr sound, out int numsubsounds);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_GetLength(IntPtr sound, out uint length, FMOD_TIMEUNIT lengthtype);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_GetFormat(IntPtr sound, out FMOD_SOUND_TYPE type, out FMOD_SOUND_FORMAT format, out int channels, out int bits);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_SeekData(IntPtr sound, uint pcm);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_ReadData(IntPtr sound, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] buffer, uint lenbytes, out uint read);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_GetDefaults(IntPtr sound, out float frequency, out int priority);
        [DllImport(DllPath)]
        private static extern unsafe FMOD_RESULT FMOD_Sound_GetName(IntPtr sound, byte* name, int namelen);
        [DllImport(DllPath)]
        private static extern FMOD_RESULT FMOD_Sound_Release(IntPtr sound);
        #endregion
    }
}
