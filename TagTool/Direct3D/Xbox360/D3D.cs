using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.Xbox360;

namespace TagTool.Direct3D.D3D9x
{
    // TODO: fill the values using the resource definition and the bitmap tag definition.
    public enum D3DFormatXbox : byte
    {
        D3DFMT_A8 = 0x2,
        D3DFMT_R5G6B5 = 0x44,
        D3DFMT_A4R4G4B4 = 0x4F,
        D3DFMT_A8Y8 = 0x4A,
        D3DFMT_DXT1 = 0x52,
        D3DFMT_DXT3 = 0x53,
        D3DFMT_DXT5 = 0x54,
        D3DFMT_A16R16G16B16F = 0x5D,
        D3DFMT_LIN_DXN = 0x71,
        D3DFMT_LIN_DXT1 = 0x73,
        D3DFMT_LIN_DXT3 = 0x74,
        D3DFMT_LIN_DXT5 = 0x75,
        D3DFMT_DXT3A = 0x7A,
        D3DFMT_DXT5A = 0x7B,
        D3DFMT_CTX1 = 0x7C,
        D3DFMT_A8R8G8B8 = 0x86,
        D3DFMT_LIN_A8R8G8B8 = 0xB2
    }

    public static class D3D9xGPU
    {
        public enum GPUSIGN : int
        {
            GPUSIGN_UNSIGNED = 0,
            GPUSIGN_SIGNED = 1,
            GPUSIGN_BIAS = 2,
            GPUSIGN_GAMMA = 3,
        }

        public static int GPUSIGN_ALL_UNSIGNED()
        {
            return (int)GPUSIGN.GPUSIGN_UNSIGNED | (int)GPUSIGN.GPUSIGN_UNSIGNED << 2 | (int)GPUSIGN.GPUSIGN_UNSIGNED << 4 | (int)GPUSIGN.GPUSIGN_UNSIGNED << 6;
        }

        public static int GPUSIGN_ALL_SIGNED() {
            return (int)GPUSIGN.GPUSIGN_SIGNED | (int)GPUSIGN.GPUSIGN_SIGNED << 2 | (int)GPUSIGN.GPUSIGN_SIGNED << 4 | (int)GPUSIGN.GPUSIGN_SIGNED << 6;
        }

        public enum GPUENDIAN : int
        {
            GPUENDIAN_NONE = 0,
            GPUENDIAN_8IN16 = 1,
            GPUENDIAN_8IN32 = 2,
            GPUENDIAN_16IN32 = 3
        }

        public enum GPUENDIAN128 : int
        {
            GPUENDIAN128_NONE = 0,
            GPUENDIAN128_8IN16 = 1,
            GPUENDIAN128_8IN32 = 2,
            GPUENDIAN128_16IN32 = 3,
            GPUENDIAN128_8IN64 = 4,
            GPUENDIAN128_8IN128 = 5,
        }

        public enum GPUSWIZZLE
        {
            GPUSWIZZLE_X = 0,
            GPUSWIZZLE_Y = 1,
            GPUSWIZZLE_Z = 2,
            GPUSWIZZLE_W = 3,
            GPUSWIZZLE_0 = 4,
            GPUSWIZZLE_1 = 5,
            GPUSWIZZLE_KEEP = 7
        }

        public static int GPUSWIZZLE_ARGB() => ((int)GPUSWIZZLE.GPUSWIZZLE_Z | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_W << 9);
        public static int GPUSWIZZLE_ORGB() => ((int)GPUSWIZZLE.GPUSWIZZLE_Z | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_ABGR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_Z << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_W << 9);
        public static int GPUSWIZZLE_OBGR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_Z << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_OOGR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_OZGR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_0 << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_RZZZ() => ((int)GPUSWIZZLE.GPUSWIZZLE_0 | (int)GPUSWIZZLE.GPUSWIZZLE_0 << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_0 << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 9);
        public static int GPUSWIZZLE_OOOR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_ORRR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_X << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_1 << 9);
        public static int GPUSWIZZLE_GRRR() => ((int)GPUSWIZZLE.GPUSWIZZLE_X | (int)GPUSWIZZLE.GPUSWIZZLE_X << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 9);
        public static int GPUSWIZZLE_RGBA() => ((int)GPUSWIZZLE.GPUSWIZZLE_W | (int)GPUSWIZZLE.GPUSWIZZLE_Z << 3 | (int)GPUSWIZZLE.GPUSWIZZLE_Y << 6 | (int)GPUSWIZZLE.GPUSWIZZLE_X << 9);


        public enum GPUNUMFORMAT
        {
            GPUNUMFORMAT_FRACTION = 0,
            GPUNUMFORMAT_INTEGER = 1
        }

        public enum GPUTEXTUREFORMAT
        {
            GPUTEXTUREFORMAT_1_REVERSE = 0,
            GPUTEXTUREFORMAT_1 = 1,
            GPUTEXTUREFORMAT_8 = 2,
            GPUTEXTUREFORMAT_1_5_5_5 = 3,
            GPUTEXTUREFORMAT_5_6_5 = 4,
            GPUTEXTUREFORMAT_6_5_5 = 5,
            GPUTEXTUREFORMAT_8_8_8_8 = 6,
            GPUTEXTUREFORMAT_2_10_10_10 = 7,
            GPUTEXTUREFORMAT_8_A = 8,
            GPUTEXTUREFORMAT_8_B = 9,
            GPUTEXTUREFORMAT_8_8 = 10,
            GPUTEXTUREFORMAT_Cr_Y1_Cb_Y0_REP = 11,
            GPUTEXTUREFORMAT_Y1_Cr_Y0_Cb_REP = 12,
            GPUTEXTUREFORMAT_16_16_EDRAM = 13, // EDRAM render target only
            GPUTEXTUREFORMAT_8_8_8_8_A = 14,
            GPUTEXTUREFORMAT_4_4_4_4 = 15,
            GPUTEXTUREFORMAT_10_11_11 = 16,
            GPUTEXTUREFORMAT_11_11_10 = 17,
            GPUTEXTUREFORMAT_DXT1 = 18,
            GPUTEXTUREFORMAT_DXT2_3 = 19,
            GPUTEXTUREFORMAT_DXT4_5 = 20,
            GPUTEXTUREFORMAT_16_16_16_16_EDRAM = 21, // EDRAM render target only
            GPUTEXTUREFORMAT_24_8 = 22,
            GPUTEXTUREFORMAT_24_8_FLOAT = 23,
            GPUTEXTUREFORMAT_16 = 24,
            GPUTEXTUREFORMAT_16_16 = 25,
            GPUTEXTUREFORMAT_16_16_16_16 = 26,
            GPUTEXTUREFORMAT_16_EXPAND = 27,
            GPUTEXTUREFORMAT_16_16_EXPAND = 28,
            GPUTEXTUREFORMAT_16_16_16_16_EXPAND = 29,
            GPUTEXTUREFORMAT_16_FLOAT = 30,
            GPUTEXTUREFORMAT_16_16_FLOAT = 31,
            GPUTEXTUREFORMAT_16_16_16_16_FLOAT = 32,
            GPUTEXTUREFORMAT_32 = 33,
            GPUTEXTUREFORMAT_32_32 = 34,
            GPUTEXTUREFORMAT_32_32_32_32 = 35,
            GPUTEXTUREFORMAT_32_FLOAT = 36,
            GPUTEXTUREFORMAT_32_32_FLOAT = 37,
            GPUTEXTUREFORMAT_32_32_32_32_FLOAT = 38,
            GPUTEXTUREFORMAT_32_AS_8 = 39,
            GPUTEXTUREFORMAT_32_AS_8_8 = 40,
            GPUTEXTUREFORMAT_16_MPEG = 41,
            GPUTEXTUREFORMAT_16_16_MPEG = 42,
            GPUTEXTUREFORMAT_8_INTERLACED = 43,
            GPUTEXTUREFORMAT_32_AS_8_INTERLACED = 44,
            GPUTEXTUREFORMAT_32_AS_8_8_INTERLACED = 45,
            GPUTEXTUREFORMAT_16_INTERLACED = 46,
            GPUTEXTUREFORMAT_16_MPEG_INTERLACED = 47,
            GPUTEXTUREFORMAT_16_16_MPEG_INTERLACED = 48,
            GPUTEXTUREFORMAT_DXN = 49,
            GPUTEXTUREFORMAT_8_8_8_8_AS_16_16_16_16 = 50,
            GPUTEXTUREFORMAT_DXT1_AS_16_16_16_16 = 51,
            GPUTEXTUREFORMAT_DXT2_3_AS_16_16_16_16 = 52,
            GPUTEXTUREFORMAT_DXT4_5_AS_16_16_16_16 = 53,
            GPUTEXTUREFORMAT_2_10_10_10_AS_16_16_16_16 = 54,
            GPUTEXTUREFORMAT_10_11_11_AS_16_16_16_16 = 55,
            GPUTEXTUREFORMAT_11_11_10_AS_16_16_16_16 = 56,
            GPUTEXTUREFORMAT_32_32_32_FLOAT = 57,
            GPUTEXTUREFORMAT_DXT3A = 58,
            GPUTEXTUREFORMAT_DXT5A = 59,
            GPUTEXTUREFORMAT_CTX1 = 60,
            GPUTEXTUREFORMAT_DXT3A_AS_1_1_1_1 = 61,
            GPUTEXTUREFORMAT_8_8_8_8_GAMMA_EDRAM = 62, // EDRAM render target only
            GPUTEXTUREFORMAT_2_10_10_10_FLOAT_EDRAM = 63, // EDRAM render target only
        }
    }

    public static class D3D9xTypes
    {
        public static int D3DFORMAT_TEXTUREFORMAT_SHIFT = 0;     // GPUTEXTUREFORMAT
        public static int D3DFORMAT_ENDIAN_SHIFT = 6;     // GPUENDIAN
        public static int D3DFORMAT_TILED_SHIFT = 8;      // BOOL
        public static int D3DFORMAT_SIGNX_SHIFT = 9;       // GPUSIGN
        public static int D3DFORMAT_SIGNY_SHIFT = 11;      // GPUSIGN
        public static int D3DFORMAT_SIGNZ_SHIFT = 13;     // GPUSIGN
        public static int D3DFORMAT_SIGNW_SHIFT = 15;     // GPUSIGN
        public static int D3DFORMAT_NUMFORMAT_SHIFT = 17;      // GPUNUMFORMAT
        public static int D3DFORMAT_SWIZZLEX_SHIFT = 18;    // GPUSWIZZLE
        public static int D3DFORMAT_SWIZZLEY_SHIFT = 21;      // GPUSWIZZLE
        public static int D3DFORMAT_SWIZZLEZ_SHIFT = 24;      // GPUSWIZZLE
        public static int D3DFORMAT_SWIZZLEW_SHIFT = 27;     // GPUSWIZZLE
        public static int D3DFORMAT_SWIZZLE_SHIFT = D3DFORMAT_SWIZZLEX_SHIFT;

        public static int D3DFORMAT_TEXTUREFORMAT_MASK = 0x0000003F;
        public static int D3DFORMAT_ENDIAN_MASK = 0x000000C0;
        public static int D3DFORMAT_TILED_MASK = 0x00000100;
        public static int D3DFORMAT_SIGNX_MASK = 0x00000600;
        public static int D3DFORMAT_SIGNY_MASK = 0x00001800;
        public static int D3DFORMAT_SIGNZ_MASK = 0x00006000;
        public static int D3DFORMAT_SIGNW_MASK = 0x00018000;
        public static int D3DFORMAT_NUMFORMAT_MASK = 0x00020000;
        public static int D3DFORMAT_SWIZZLEX_MASK = 0x001C0000;
        public static int D3DFORMAT_SWIZZLEY_MASK = 0x00E00000;
        public static int D3DFORMAT_SWIZZLEZ_MASK = 0x07000000;
        public static int D3DFORMAT_SWIZZLEW_MASK = 0x38000000;
        public static int D3DFORMAT_SWIZZLE_MASK = (D3DFORMAT_SWIZZLEX_MASK | D3DFORMAT_SWIZZLEY_MASK | D3DFORMAT_SWIZZLEZ_MASK | D3DFORMAT_SWIZZLEW_MASK);

        public static int D3DINDEXFORMAT_ENDIAN_SHIFT = 0;    // GPUENDIAN
        public static int D3DINDEXFORMAT_32BITS_SHIFT = 2; // BOOL

        public static int D3DINDEXFORMAT_ENDIAN_MASK = 0x00000003;
        public static int D3DINDEXFORMAT_32BITS_MASK = 0x00000004;


        public static int MAKED3DFMT(int TextureFormat, int Endian, int Tiled, int TextureSign, int NumFormat, int Swizzle)
        {
            return ((TextureFormat) << D3DFORMAT_TEXTUREFORMAT_SHIFT | (Endian) << D3DFORMAT_ENDIAN_SHIFT | (Tiled) << D3DFORMAT_TILED_SHIFT |
             (TextureSign) << D3DFORMAT_SIGNX_SHIFT | (NumFormat) << D3DFORMAT_NUMFORMAT_SHIFT | (Swizzle) << D3DFORMAT_SWIZZLEX_SHIFT);
        }

        public static int MAKED3DFMT2(int TextureFormat, int Endian, int Tiled, int TextureSignX, int TextureSignY, int TextureSignZ, int TextureSignW, int NumFormat, int SwizzleX, int SwizzleY, int SwizzleZ, int SwizzleW)
        {
            return ((TextureFormat) << D3DFORMAT_TEXTUREFORMAT_SHIFT | (Endian) << D3DFORMAT_ENDIAN_SHIFT | (Tiled) << D3DFORMAT_TILED_SHIFT | (TextureSignX) << D3DFORMAT_SIGNX_SHIFT | 
             (TextureSignY) << D3DFORMAT_SIGNY_SHIFT | (TextureSignZ) << D3DFORMAT_SIGNZ_SHIFT | (TextureSignW) << D3DFORMAT_SIGNW_SHIFT | 
             (NumFormat) << D3DFORMAT_NUMFORMAT_SHIFT | (SwizzleX) << D3DFORMAT_SWIZZLEX_SHIFT | (SwizzleY) << D3DFORMAT_SWIZZLEY_SHIFT | 
             (SwizzleZ) << D3DFORMAT_SWIZZLEZ_SHIFT | (SwizzleW) << D3DFORMAT_SWIZZLEW_SHIFT);
        }

        public static int MAKELINFMT(int D3dFmt) => (D3dFmt) & ~D3DFORMAT_TILED_MASK;

        public static int MAKESRGBFMT(int D3dFmt)
        {
            return (((D3dFmt) & ~(D3DFORMAT_SIGNX_MASK | D3DFORMAT_SIGNY_MASK | D3DFORMAT_SIGNZ_MASK)) |
            (((int)D3D9xGPU.GPUSIGN.GPUSIGN_GAMMA << D3DFORMAT_SIGNX_SHIFT) |
              ((int)D3D9xGPU.GPUSIGN.GPUSIGN_GAMMA << D3DFORMAT_SIGNY_SHIFT) |
              ((int)D3D9xGPU.GPUSIGN.GPUSIGN_GAMMA << D3DFORMAT_SIGNZ_SHIFT)));
        }

        public static int MAKELEFMT(int D3dFmt) => (((D3dFmt) & ~D3DFORMAT_ENDIAN_MASK) | ((int)D3D9xGPU.GPUENDIAN.GPUENDIAN_NONE << D3DFORMAT_ENDIAN_SHIFT));

        public static int MAKEINDEXFMT(int Is32Bits, int Endian) => ((Is32Bits) << D3DINDEXFORMAT_32BITS_SHIFT | (Endian) << D3DINDEXFORMAT_ENDIAN_SHIFT);

        [Flags]
        public enum D3DUSAGE : int
        {
            D3DUSAGE_CPU_CACHED_MEMORY              = 0x00000004, // Xbox 360 only
            D3DUSAGE_RUNCOMMANDBUFFER_TIMESTAMP     = 0x00000200, // Xbox 360 only
            D3DUSAGE_RENDERTARGET                   = 0x00000001,
            D3DUSAGE_DEPTHSTENCIL                   = 0x00000002,
            D3DUSAGE_DMAP                           = 0x00004000,
            D3DUSAGE_QUERY_LEGACYBUMPMAP            = 0x00008000,
            D3DUSAGE_QUERY_SRGBREAD                 = 0x00010000,
            D3DUSAGE_QUERY_FILTER                   = 0x00020000,
            D3DUSAGE_QUERY_SRGBWRITE                = 0x00040000,
            D3DUSAGE_QUERY_POSTPIXELSHADER_BLENDING = 0x00080000,
            D3DUSAGE_QUERY_VERTEXTEXTURE            = 0x00100000,
            D3DUSAGE_WRITEONLY                      = 0x00000008,
            D3DUSAGE_SOFTWAREPROCESSING             = 0x00000010,
            D3DUSAGE_DONOTCLIP                      = 0x00000020,
            D3DUSAGE_POINTS                         = 0x00000040,
            D3DUSAGE_RTPATCHES                      = 0x00000080,
            D3DUSAGE_NPATCHES                       = 0x00000100
        }

        public enum D3DRESOURCETYPE : int
        {
            D3DRTYPE_NONE = 0,
            D3DRTYPE_VERTEXBUFFER = 1,
            D3DRTYPE_INDEXBUFFER = 2,
            D3DRTYPE_TEXTURE = 3,
            D3DRTYPE_SURFACE = 4,
            D3DRTYPE_VERTEXDECLARATION = 5,
            D3DRTYPE_VERTEXSHADER = 6,
            D3DRTYPE_PIXELSHADER = 7,
            D3DRTYPE_CONSTANTBUFFER = 8,
            D3DRTYPE_COMMANDBUFFER = 9,

            D3DRTYPE_VOLUME = 16,
            D3DRTYPE_VOLUMETEXTURE = 17,
            D3DRTYPE_CUBETEXTURE = 18,
            D3DRTYPE_ARRAYTEXTURE = 19,
            D3DRTYPE_LINETEXTURE = 20,
        }

        public enum D3DMIPPACKINGTYPE
        {
            None = 0,
            NonPacked = 1,
            Packed = 2
        }

    }


    public static class D3D
    {
        public static int GetMaxMipLevels(int width, int height, int depth, int hasBorder)
        {
            return 0;
        }

        public static void FindTextureSize(int width, int height, int depth, int levels, D3D9xGPU.GPUTEXTUREFORMAT format, int someType, int unknown,
            bool mipsPacked, int hasBorder, int expBias, uint nBlocksPitch, ref int unknown2, ref int outWidth, ref int outHeight)
        {
            
        }

        public static D3D9xGPU.GPUTEXTUREFORMAT GpuFormat(int d3dFormat)
        {
            return (D3D9xGPU.GPUTEXTUREFORMAT)((d3dFormat & D3D9xTypes.D3DFORMAT_TEXTUREFORMAT_MASK) >> D3D9xTypes.D3DFORMAT_TEXTUREFORMAT_SHIFT);
        }

        public static void SetTextureHeader(D3D9xTypes.D3DRESOURCETYPE resourceType, int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, 
            D3D9xTypes.D3DMIPPACKINGTYPE mipPackingType, int hasBorder, int expBias, uint nBlocksPitch, ref D3DTexture9 pTexture, ref uint baseSize, ref uint mipmapSize)
        {
            var someType = 0;

            switch (resourceType)
            {
                case D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_ARRAYTEXTURE:
                case D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE:
                    someType = 1;
                    break;
                case D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_CUBETEXTURE:
                    someType = 3;
                    depth = 1;
                    break;
                case D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_VOLUMETEXTURE:
                    someType = 2;
                    break;
                case D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_LINETEXTURE:
                    someType = 0;
                    mipPackingType = D3D9xTypes.D3DMIPPACKINGTYPE.None;
                    break;
                default:
                    someType = 0;
                    depth = 1;
                    break;
            }

            if (levels == 0)
                levels = GetMaxMipLevels(width, height, depth, hasBorder);

            bool mipsPacked = false;
            if (mipPackingType == D3D9xTypes.D3DMIPPACKINGTYPE.Packed)
                mipsPacked = true;

            int tempUnknown2 = 0;
            int tempWidth = 0;
            int tempHeight = 0;
            FindTextureSize(width, height, depth, levels, GpuFormat(format), someType, (format >> 8) & 0xFF, mipsPacked,
                hasBorder, expBias, nBlocksPitch, ref tempUnknown2, ref tempWidth, ref tempHeight);

            pTexture.Unknown4 = 1;
            pTexture.Unknown8 = 0;
            pTexture.Unknown14 = 0xFFFF0000;
            pTexture.Unknown18 = 0xFFFF0000;

            pTexture.Flags = D3DTexture9Flags.Unknown1 | D3DTexture9Flags.Unknown2 | D3DTexture9Flags.Unknown3;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_CPU_CACHED_MEMORY))
                pTexture.Flags |= D3DTexture9Flags.CPU_CACHED_MEMORY;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_RUNCOMMANDBUFFER_TIMESTAMP))
                pTexture.Flags |= D3DTexture9Flags.RUNCOMMANDBUFFER_TIMESTAMP;



        }
    }










}
