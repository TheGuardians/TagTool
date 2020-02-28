using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Xbox360
{
    [Flags]
    public enum D3DTexture9Flags : int
    {
        Unknown1                    = 0x00000001,
        Unknown2                    = 0x00000002,
        Unknown3                    = 0x00100000,
        CPU_CACHED_MEMORY           = 0x00200000,
        RUNCOMMANDBUFFER_TIMESTAMP  = 0x00400000
    }

    public class D3DTexture9
    {
        public D3DTexture9Flags Flags;
        public int Unknown4;
        public int Unknown8;
        public int UnknownC;
        public int Unknown10;
        public uint Unknown14;
        public uint Unknown18;
        public int Unknown1C;
        public int Unknown20;
        public int Unknown24;
        public int Unknown28;
        public int Unknown2C;
        public int Unknown30;

        public int GetBaseOffset() => (int)(Unknown20 & 0xFFFFF000);

        public int GetMipOffset() => (int)(Unknown30 & 0xFFFFF000);

        public int GetMinMipLevel() => (Unknown2C >> 2) & 0xF;

        public int GetMaxMipLevel() => (Unknown2C >> 6) & 0xF;

    }
}
