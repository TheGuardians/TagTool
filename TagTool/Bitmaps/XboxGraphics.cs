using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.D3D9x;
using TagTool.Direct3D.Xbox360;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// C# implementation of xgraphics
    /// </summary>
    public static class XboxGraphics
    {
        public class XGTEXTURE_DESC
        {
            public D3D9xTypes.D3DRESOURCETYPE ResourceType;
            public uint Width;
            public uint  Height;
            public uint Depth;
            public int D3DFormat;
            public uint RowPitch;
            public uint SlicePitch;
            public uint BitsPerPixel;
            public uint WidthInBlocks;
            public uint HeightInBlocks;
            public uint DepthInBlocks;
            public uint BytesPerBlock;
            public int ExpBias;
            public int Flags;
            public int MultiSampleType;
        }

        public static D3D9xGPU.GPUTEXTUREFORMAT  XGGetGpuFormat(int d3dFormat)
        {
            return (D3D9xGPU.GPUTEXTUREFORMAT)((d3dFormat & D3D9xTypes.D3DFORMAT_TEXTUREFORMAT_MASK) >> D3D9xTypes.D3DFORMAT_TEXTUREFORMAT_SHIFT);
        }

        public static bool XGIsTiledFormat(int d3dFormat)
        {
            return (d3dFormat & D3D9xTypes.D3DFORMAT_TILED_MASK) != 0;
        }

        public static bool XGIsCompressedFormat(int d3dFormat)
        {
            switch (XGGetGpuFormat(d3dFormat))
            {
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT1_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT2_3:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT2_3_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT4_5:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT4_5_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXN:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT3A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT5A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_CTX1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT3A_AS_1_1_1_1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_Y1_Cr_Y0_Cb_REP:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_Cr_Y1_Cb_Y0_REP:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8_8:
                    return true;
            }
            return false;
        }

        public static byte[] XGTextureFormatBitsPerPixel = new byte[]
        {
            1,   // GPUTEXTUREFORMAT_1_REVERSE
            1,   // GPUTEXTUREFORMAT_1
            8,   // GPUTEXTUREFORMAT_8
            16,  // GPUTEXTUREFORMAT_1_5_5_5
            16,  // GPUTEXTUREFORMAT_5_6_5
            16,  // GPUTEXTUREFORMAT_6_5_5
            32,  // GPUTEXTUREFORMAT_8_8_8_8
            32,  // GPUTEXTUREFORMAT_2_10_10_10
            8,   // GPUTEXTUREFORMAT_8_A
            8,   // GPUTEXTUREFORMAT_8_B
            16,  // GPUTEXTUREFORMAT_8_8
            16,  // GPUTEXTUREFORMAT_Cr_Y1_Cb_Y0_REP
            16,  // GPUTEXTUREFORMAT_Y1_Cr_Y0_Cb_REP
            32,  // GPUTEXTUREFORMAT_16_16_EDRAM
            32,  // GPUTEXTUREFORMAT_8_8_8_8_A
            16,  // GPUTEXTUREFORMAT_4_4_4_4
            32,  // GPUTEXTUREFORMAT_10_11_11
            32,  // GPUTEXTUREFORMAT_11_11_10
            4,   // GPUTEXTUREFORMAT_DXT1
            8,   // GPUTEXTUREFORMAT_DXT2_3
            8,   // GPUTEXTUREFORMAT_DXT4_5
            64,  // GPUTEXTUREFORMAT_16_16_16_16_EDRAM
            32,  // GPUTEXTUREFORMAT_24_8
            32,  // GPUTEXTUREFORMAT_24_8_FLOAT
            16,  // GPUTEXTUREFORMAT_16
            32,  // GPUTEXTUREFORMAT_16_16
            64,  // GPUTEXTUREFORMAT_16_16_16_16
            16,  // GPUTEXTUREFORMAT_16_EXPAND
            32,  // GPUTEXTUREFORMAT_16_16_EXPAND
            64,  // GPUTEXTUREFORMAT_16_16_16_16_EXPAND
            16,  // GPUTEXTUREFORMAT_16_FLOAT
            32,  // GPUTEXTUREFORMAT_16_16_FLOAT
            64,  // GPUTEXTUREFORMAT_16_16_16_16_FLOAT
            32,  // GPUTEXTUREFORMAT_32
            64,  // GPUTEXTUREFORMAT_32_32
            128, // GPUTEXTUREFORMAT_32_32_32_32
            32,  // GPUTEXTUREFORMAT_32_FLOAT
            64,  // GPUTEXTUREFORMAT_32_32_FLOAT
            128, // GPUTEXTUREFORMAT_32_32_32_32_FLOAT
            8,   // GPUTEXTUREFORMAT_32_AS_8
            16,  // GPUTEXTUREFORMAT_32_AS_8_8
            16,  // GPUTEXTUREFORMAT_16_MPEG
            32,  // GPUTEXTUREFORMAT_16_16_MPEG
            8,   // GPUTEXTUREFORMAT_8_INTERLACED
            8,   // GPUTEXTUREFORMAT_32_AS_8_INTERLACED
            16,  // GPUTEXTUREFORMAT_32_AS_8_8_INTERLACED
            16,  // GPUTEXTUREFORMAT_16_INTERLACED
            16,  // GPUTEXTUREFORMAT_16_MPEG_INTERLACED
            32,  // GPUTEXTUREFORMAT_16_16_MPEG_INTERLACED
            8,   // GPUTEXTUREFORMAT_DXN
            32,  // GPUTEXTUREFORMAT_8_8_8_8_AS_16_16_16_16
            4,   // GPUTEXTUREFORMAT_DXT1_AS_16_16_16_16
            8,   // GPUTEXTUREFORMAT_DXT2_3_AS_16_16_16_16
            8,   // GPUTEXTUREFORMAT_DXT4_5_AS_16_16_16_16
            32,  // GPUTEXTUREFORMAT_2_10_10_10_AS_16_16_16_16
            32,  // GPUTEXTUREFORMAT_10_11_11_AS_16_16_16_16
            32,  // GPUTEXTUREFORMAT_11_11_10_AS_16_16_16_16
            96,  // GPUTEXTUREFORMAT_32_32_32_FLOAT
            4,   // GPUTEXTUREFORMAT_DXT3A
            4,   // GPUTEXTUREFORMAT_DXT5A
            4,   // GPUTEXTUREFORMAT_CTX1
            4,   // GPUTEXTUREFORMAT_DXT3A_AS_1_1_1_1
            32,  // GPUTEXTUREFORMAT_8_8_8_8_GAMMA_EDRAM
            32,  // GPUTEXTUREFORMAT_2_10_10_10_FLOAT_EDRAM
        };

        public static uint XGBitsPerPixelFromGpuFormat(D3D9xGPU.GPUTEXTUREFORMAT format)
        {
            return XGTextureFormatBitsPerPixel[(int)format];
        }

        public static uint XGBytesPerPixelFromFormat(int d3dFormat)
        {
            D3D9xGPU.GPUTEXTUREFORMAT format = XGGetGpuFormat(d3dFormat);
            var bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
            return bitsPerPixel >> 3;
        }

        /// <summary>
        /// Get the dimension of the compression block, if the format is not compressed, return 1x1 blocks
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <returns></returns>
        public static uint XGGetBlockDimensions(D3D9xGPU.GPUTEXTUREFORMAT format, ref uint pWidth, ref uint pHeight)
        {
            uint result;

            switch (format)
            {
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT2_3:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT4_5:

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXN:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT1_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT2_3_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT4_5_AS_16_16_16_16:
                
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT3A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT5A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_CTX1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_DXT3A_AS_1_1_1_1:
                    result = 4;
                    pWidth = result;
                    pHeight = result;
                    break;

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8:
                    result = 1;
                    pWidth = 4;
                    pHeight = 1;
                    break;

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_1_REVERSE:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_1:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_1_5_5_5:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_5_6_5:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_6_5_5:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_8_8_8:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_2_10_10_10:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_B:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_8:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_Cr_Y1_Cb_Y0_REP:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_Y1_Cr_Y0_Cb_REP:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8_8:
                    pWidth = 2;
                    pHeight = 1;
                    result = 1;
                    break;

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_EDRAM:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_8_8_8_A:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_4_4_4_4:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_10_11_11:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_11_11_10:

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_16_16_EDRAM:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_24_8:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_24_8_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_EXPAND:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_EXPAND:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_16_16_EXPAND:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_16_16_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_32:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_32_32_32:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_32_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_32_32_32_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_MPEG:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_MPEG:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_8_8_8_GAMMA_EDRAM:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_2_10_10_10_FLOAT_EDRAM:

                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_8_8_8_8_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_2_10_10_10_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_10_11_11_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_11_11_10_AS_16_16_16_16:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_32_32_FLOAT:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_32_AS_8_8_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_MPEG_INTERLACED:
                case D3D9xGPU.GPUTEXTUREFORMAT.GPUTEXTUREFORMAT_16_16_MPEG_INTERLACED:
                    pWidth = 1;
                    pHeight = 1;
                    result = 1;
                    break;

                default:
                    result = 1;
                    pWidth = 1;
                    pHeight = 1;
                    break;
            }
            return result;
        }


        public static uint XGSetTextureHeader(int width, int height, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, 1, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.unknown2, 0, 0, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetCubeTextureHeader(int edgeLength, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
            ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(edgeLength, edgeLength, 6, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.unknown2, 0, 0, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_CUBETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetArrayTextureHeader(int width, int height, int arraySize, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, arraySize, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.unknown2, 0, 0, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_ARRAYTEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetVolumeTextureHeader(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
           ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, depth, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.unknown2, 0, 0, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_VOLUMETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetTextureHeaderEx(int width, int height, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, int flags, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, 1, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~flags & 1), (flags >> 1) & 1, expBias, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetCubeTextureHeaderEx(int edgeLength, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, int flags, int baseOffset, int mipOffset,
            ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(edgeLength, edgeLength, 6, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~flags & 1), (flags >> 1) & 1, expBias, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_CUBETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetArrayTextureHeaderEx(int width, int height, int arraySize, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, int flags, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, arraySize, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~flags & 1), (flags >> 1) & 1, expBias, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_ARRAYTEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetVolumeTextureHeaderEx(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, int flags, int baseOffset, int mipOffset,
           ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, depth, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~flags & 1), (flags >> 1) & 1, expBias, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_VOLUMETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }


        private static uint SetBaseTextureHeader(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, D3D9xTypes.D3DMIPPACKINGTYPE mipPackingType, int flags_unknown, 
            int expBias, int initialBaseOffset, int initialMipOffset, uint pitch, D3D9xTypes.D3DRESOURCETYPE resourceType, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            uint blockHeight = 0;
            uint blockWidth = 0;
            uint nBlocksPitch = 0;
            uint baseSize = 0;
            uint mipSize = 0;

            XGGetBlockDimensions(XGGetGpuFormat(format), ref blockWidth, ref blockHeight);

            if(levels != 0)
            {
                if(levels == 1 || (width == 1 && height == 1 && depth == 1 && flags_unknown > 0) || (width == 3 && height == 3 && depth == 3 && flags_unknown > 0))
                {
                    if (levels == 1 && resourceType == D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE)
                    {
                        nBlocksPitch = pitch / blockHeight;
                    }
                }
            }
            
            Direct3D.D3D9x.D3D.SetTextureHeader(resourceType, width, height, depth, levels, usage, format, mipPackingType, flags_unknown,
                expBias, nBlocksPitch, ref pTexture, ref baseSize, ref mipSize);

            pTexture.Unknown14 = 0xFFFF0000;
            pTexture.Unknown18 = 0xFFFF0000;
            pTexture.Flags = D3DTexture9Flags.Unknown1 | D3DTexture9Flags.Unknown2;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_CPU_CACHED_MEMORY))
                pTexture.Flags |= D3DTexture9Flags.CPU_CACHED_MEMORY;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_RUNCOMMANDBUFFER_TIMESTAMP))
                pTexture.Flags |= D3DTexture9Flags.RUNCOMMANDBUFFER_TIMESTAMP;

            pTexture.BaseOffset = UnknownAlign(initialBaseOffset, pTexture.BaseOffset);
            pBaseSize = baseSize;
            pMipSize = mipSize;

            if (levels > 0)
            {
                if(initialMipOffset == -1)
                {
                    pBaseSize = AlignToPage(baseSize);
                    pTexture.MipOffset = UnknownAlign((int)(initialBaseOffset + pBaseSize), pTexture.MipOffset);
                }
                else
                    pTexture.MipOffset = UnknownAlign(initialMipOffset, pTexture.MipOffset);
            }
            else
                pTexture.MipOffset &=  0xFFF;

            
            return pBaseSize + pMipSize;
        }

        private static uint AlignToPage(uint offset)
        {
            return (offset + 0xFFF) & 0xFFFFF000;
        }

        private static int UnknownAlign(int offset1, int offset2)
        {
            return offset1 ^ (offset1 ^ offset2) & 0xFFF;
        }
    }
}
