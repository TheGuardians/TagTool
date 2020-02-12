using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.D3D9;
using TagTool.Direct3D.D3D9x;
using TagTool.Direct3D.Xbox360;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// C# implementation of xgraphics
    /// </summary>
    public static class XboxGraphics
    {
        [Flags]
        public enum XGTILE : int
        {
            NONE = 0x0,
            XGTILE_NONPACKED = 0x1,
            XGTILE_BORDER = 0x2
        }

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
            public XGTILE Flags;
            public int MultiSampleType;
        }

        public class XGPOINT
        {
            public int X;
            public int Y;
            public int Z;
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
            return SetBaseTextureHeader(width, height, 1, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.Packed, 0, 0, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetCubeTextureHeader(int edgeLength, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
            ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(edgeLength, edgeLength, 6, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.Packed, 0, 0, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_CUBETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetArrayTextureHeader(int width, int height, int arraySize, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, arraySize, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.Packed, 0, 0, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_ARRAYTEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetVolumeTextureHeader(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, int pool, int baseOffset, int mipOffset,
           ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, depth, levels, usage, format, D3D9xTypes.D3DMIPPACKINGTYPE.Packed, 0, 0, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_VOLUMETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetTextureHeaderEx(int width, int height, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, XGTILE flags, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, 1, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~(int)flags & 1), ((int)flags >> 1) & 1, expBias, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetCubeTextureHeaderEx(int edgeLength, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, XGTILE flags, int baseOffset, int mipOffset,
            ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(edgeLength, edgeLength, 6, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~(int)flags & 1), ((int)flags >> 1) & 1, expBias, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_CUBETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetArrayTextureHeaderEx(int width, int height, int arraySize, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, XGTILE flags, int baseOffset, int mipOffset,
            uint pitch, ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, arraySize, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~(int)flags & 1), ((int)flags >> 1) & 1, expBias, baseOffset, mipOffset, pitch, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_ARRAYTEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        public static uint XGSetVolumeTextureHeaderEx(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, int expBias, XGTILE flags, int baseOffset, int mipOffset,
           ref D3DTexture9 pTexture, ref uint pBaseSize, ref uint pMipSize)
        {
            return SetBaseTextureHeader(width, height, depth, levels, usage, format, (D3D9xTypes.D3DMIPPACKINGTYPE)(~(int)flags & 1), ((int)flags >> 1) & 1, expBias, baseOffset, mipOffset, 0, D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_VOLUMETEXTURE, ref pTexture, ref pBaseSize, ref pMipSize);
        }

        private static uint SetBaseTextureHeader(int width, int height, int depth, int levels, D3D9xTypes.D3DUSAGE usage, int format, D3D9xTypes.D3DMIPPACKINGTYPE mipPackingType, int hasBorder, 
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
                if(levels == 1 || (width == 1 && height == 1 && depth == 1 && hasBorder > 0) || (width == 3 && height == 3 && depth == 3 && hasBorder > 0))
                {
                    if (levels == 1 && resourceType == D3D9xTypes.D3DRESOURCETYPE.D3DRTYPE_TEXTURE)
                    {
                        nBlocksPitch = pitch / blockHeight;
                    }
                }
            }

            pTexture = new D3DTexture9();

            Direct3D.D3D9x.D3D.SetTextureHeader(resourceType, width, height, depth, levels, usage, format, mipPackingType, hasBorder,
                expBias, nBlocksPitch, ref pTexture, ref baseSize, ref mipSize);

            pTexture.Unknown14 = 0xFFFF0000;
            pTexture.Unknown18 = 0xFFFF0000;
            pTexture.Flags = D3DTexture9Flags.Unknown1 | D3DTexture9Flags.Unknown2;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_CPU_CACHED_MEMORY))
                pTexture.Flags |= D3DTexture9Flags.CPU_CACHED_MEMORY;
            if (usage.HasFlag(D3D9xTypes.D3DUSAGE.D3DUSAGE_RUNCOMMANDBUFFER_TIMESTAMP))
                pTexture.Flags |= D3DTexture9Flags.RUNCOMMANDBUFFER_TIMESTAMP;

            pTexture.Unknown20 = UnknownAlign(initialBaseOffset, pTexture.Unknown20);
            pBaseSize = baseSize;
            pMipSize = mipSize;

            if (levels > 0)
            {
                if(initialMipOffset == -1)
                {
                    pBaseSize = AlignToPage(baseSize);
                    pTexture.Unknown30 = UnknownAlign((int)(initialBaseOffset + pBaseSize), pTexture.Unknown30);
                }
                else
                    pTexture.Unknown30 = UnknownAlign(initialMipOffset, pTexture.Unknown30);
            }
            else
                pTexture.Unknown30 &=  0xFFF;

            
            return pBaseSize + pMipSize;
        }

        /// <summary>
        /// This takes the current width and height and checks wether  the minimal dimension with border is <= 16.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hasBorder"></param>
        /// <returns>0 if min dimension less or equal to 16, else returns the mipmap level at which the min width would be less or equal to 16</returns>
        private static uint GetMipLevelRequiresOffset(uint width, uint height, uint hasBorder)
        {
            uint logWidth = hasBorder + Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - 2 * hasBorder - 1));
            uint logHeight = hasBorder + Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - 2 * hasBorder - 1));
            uint minLogDim = logWidth;

            if (logWidth >= logHeight)
                minLogDim = logHeight;

            int result = (int)minLogDim - 4;

            if (result <= 0)
                result = 0;

            return (uint)result;
        }

        private static uint sub_D55E38(uint level, uint width, uint height, uint depth, uint slicePitch, uint size, D3D9xGPU.GPUTEXTUREFORMAT format, ref uint offsetX, ref uint offsetY, ref uint offsetZ)
        {
            offsetX = 0;
            offsetY = 0;
            offsetZ = 0;
            uint blockWidth = 0;
            uint blockHeight = 0;

            uint bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
            XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);

            uint logWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - 1));
            uint logHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - 1));
            uint logDepth = Direct3D.D3D9x.D3D.Log2Ceiling((int)(depth - 1));

            uint logSize = logWidth <= logHeight ? logWidth : logHeight;

            uint packedMipBase = logSize > 4 ? (logSize - 4) : 0;
            uint packedMip = level - packedMipBase;


            uint nextPowerTwoWidth = (uint)1 << (int)logWidth;
            uint nextPowerTwoHeight = (uint)1 << (int)logHeight;

            if (level < 3)
            {
                if(logHeight < logWidth)
                    offsetY = (uint)16 >> (int)level;
                else
                    offsetX = (uint)16 >> (int)level;

                offsetZ = 0;
            }
            else
            {
                uint offset;

                if (logWidth > logHeight)
                {
                    offset = (uint)((1 << (int)(logWidth - packedMipBase)) >> (int)(packedMip - 2));
                    offsetX = offset;
                    offsetY = 0;
                }
                else
                {
                    offset = (uint)((1 << (int)(logHeight - packedMipBase)) >> (int)(packedMip - 2));
                    offsetY = offset;
                    offsetX = 0;
                }

                if (offset >= 4)
                    offsetZ = 0;
                else
                {
                    uint depthOffset = logDepth - level;
                    if (depthOffset <= 1)
                        depthOffset = 1;
                    offsetZ = 4 * depthOffset;
                }
            }

            uint xPixelOffset = offsetX;
            uint yPixelOffest = offsetY;

            offsetX /= blockWidth;
            offsetY /= blockHeight;

            return size * offsetZ + slicePitch * yPixelOffest + (xPixelOffset * bitsPerPixel * blockWidth >> 3);
        }

        public static uint GetMipTailLevelOffsetCoords(uint width, uint height, uint depth, uint level, D3D9xGPU.GPUTEXTUREFORMAT format, bool isTiled, bool hasBorder, XGPOINT point)
        {
            uint border = (uint)(hasBorder ? 1 : 0);
            uint mipLevelRequiresOffset = GetMipLevelRequiresOffset(width, height, border);

            if (level >= mipLevelRequiresOffset) // happens when the requested level bitmap dimensions are <= 16
            {
                uint logWidth = border + Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - 2 * border - 1));
                uint logHeight = border + Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - 2 * border - 1));
                uint logDepth = 0;
                if (depth > 1)
                    logDepth = border + Direct3D.D3D9x.D3D.Log2Ceiling((int)(depth - 2 * border - 1));

                width = (uint)1 << (int)(logWidth - mipLevelRequiresOffset);
                height = (uint)1 << (int)(logHeight - mipLevelRequiresOffset);
                if (logDepth - mipLevelRequiresOffset <= 0)
                    depth = 1;
                else
                    depth = (uint)1 << (int)(logDepth - mipLevelRequiresOffset);

                uint bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
                uint tempWidth = width;
                uint tempHeight = height;
                uint tempDepth = depth;
                uint xOffset = 0;
                uint yOffset = 0;
                uint zOffset = 0;

                Direct3D.D3D9x.D3D.AlignTextureDimensions(ref tempWidth, ref tempHeight, ref tempDepth, bitsPerPixel, format, 1, isTiled);
                // previous size maybe?
                uint size = (tempHeight * tempWidth * bitsPerPixel) >> 3;
                if(depth <= 1)
                    size = AlignToPage(size);   // probably not required on PC

                uint result = sub_D55E38(level - mipLevelRequiresOffset, width, height, depth, bitsPerPixel * width >> 3, size, format, ref xOffset, ref yOffset, ref zOffset);
                point.X = (int)xOffset;
                point.Y = (int)yOffset;
                point.Z = (int)zOffset;
                return result;
            }
            else
            {
                point.X = 0;
                point.Y = 0;
                point.Z = 0;
                return 0;
            }
            
        }

        public static byte[] XGUntileSurface(uint rowPitch, XGPOINT point, byte[] source, uint width, uint height, D3DBOX box, uint texelPitch)
        {
            return UntileSurface(width, height, rowPitch, point, source, texelPitch, box);
        }

        public static byte[] XGUntileVolume(uint rowPitch, uint slicePitch, XGPOINT point, byte[] source, uint width, uint height, uint depth, D3DBOX box, uint texelPitch)
        {
            return UntileVolume(box, width, height, rowPitch, slicePitch, point, source, depth, texelPitch);
        } 

        public static byte[] XGUntileTextureLevel(uint width, uint height, uint level, D3D9xGPU.GPUTEXTUREFORMAT format, XGTILE flags, uint rowPitch, XGPOINT point, byte[] source, D3DBOX box)
        {
            uint width_as_blocks;
            uint height_as_blocks;
            uint texelPitch;

            uint blockWidth = 0;
            uint blockHeight = 0;


            XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);
            int blockLogWidth = (int)Direct3D.D3D9x.D3D.Log2Floor((int)blockWidth);
            int blockLogHeight = (int)Direct3D.D3D9x.D3D.Log2Floor((int)blockHeight);
            var bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
            texelPitch = (bitsPerPixel << (blockLogWidth + blockLogHeight)) >> 3; // also bytes per block


            int borderSize = flags.HasFlag(XGTILE.XGTILE_BORDER) ? 2 : 0;
            int hasBorder = flags.HasFlag(XGTILE.XGTILE_BORDER) ? 1 : 0;

            if (level > 0)
            {
                int nextPowerOfTwoWidth = 1 << (hasBorder - (int)Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - borderSize - 1))) >> (int)level;
                int nextPowerOfTwoHeight = 1 << (hasBorder - (int)Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - borderSize - 1))) >> (int)level;
   
                if (nextPowerOfTwoWidth <= 1)
                    nextPowerOfTwoWidth = 1;
                if (nextPowerOfTwoHeight <= 1)
                    nextPowerOfTwoHeight = 1;

                width_as_blocks = (uint)(nextPowerOfTwoWidth + blockWidth - 1) >> blockLogWidth;
                height_as_blocks = (uint)(nextPowerOfTwoHeight + blockHeight - 1) >> blockLogHeight;
            }
            else
            {
                width_as_blocks = (width + blockWidth - 1) >> blockLogWidth;
                height_as_blocks = (height + blockHeight - 1) >> blockLogHeight;
            }

            // update point to be in terms of the block width and height
            if (point != null)
            {
                point.X >>= blockLogWidth;
                point.Y >>= blockLogWidth;
            }
            else
            {
                point = new XGPOINT();
                point.X = 0;
                point.Y = 0;
            }

            // update box bounds to be in terms of the block width and height
            if (box != null)
            {
                box.Left >>= blockLogWidth;
                box.Right = (box.Right + blockWidth - 1) >> blockLogWidth;
                box.Top >>= blockLogHeight;
                box.Bottom = (box.Bottom + blockHeight - 1) >> blockLogHeight;
            }
            else
            {
                box = new D3DBOX();
                box.Left = 0;
                box.Top = 0;

                var tempWidth = (width - borderSize) >> (int)level;
                if (tempWidth <= 1)
                    tempWidth = 1;
                box.Right = (uint)(tempWidth + blockWidth - 1) >> blockLogWidth;

                var tempHeight = (height - borderSize) >> (int)level;
                if (tempHeight <= 1)
                    tempHeight = 1;
                box.Bottom = (uint)(tempHeight + blockHeight - 1) >> blockLogHeight;


            }

            if (!flags.HasFlag(XGTILE.XGTILE_NONPACKED))
            {
                XGPOINT offset = new XGPOINT();
                // need to understand the return value and modify the byte[]
                var offsetInByteArray = GetMipTailLevelOffsetCoords(width, height, 1, level, format, true, flags.HasFlag(XGTILE.XGTILE_BORDER), offset);

                box.Top += (uint)offset.Y;
                box.Bottom += (uint)offset.Y;
                box.Left += (uint)offset.X;
                box.Right += (uint)offset.X;
            }

            return UntileSurface(width_as_blocks, height_as_blocks, rowPitch, point, source, texelPitch, box);
        }

        public static byte[] XGUntileVolumeTextureLevel(uint width, uint height, uint depth, uint level, D3D9xGPU.GPUTEXTUREFORMAT format, XGTILE flags, uint rowPitch, uint slicePitch, XGPOINT point, byte[] source, D3DBOX box)
        {
            uint width_as_blocks;
            uint height_as_blocks;
            uint texelPitch;

            uint blockWidth = 0;
            uint blockHeight = 0;


            XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);
            int blockLogWidth = (int)Direct3D.D3D9x.D3D.Log2Floor((int)blockWidth);
            int blockLogHeight = (int)Direct3D.D3D9x.D3D.Log2Floor((int)blockHeight);
            var bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
            texelPitch = (bitsPerPixel << (blockLogWidth + blockLogHeight)) >> 3; // also bytes per block


            int borderSize = flags.HasFlag(XGTILE.XGTILE_BORDER) ? 2 : 0;
            int hasBorder = flags.HasFlag(XGTILE.XGTILE_BORDER) ? 1 : 0;

            if (level > 0)
            {
                /*
                 * Textures with border are a special case. For example if there is a border around a 254 x 254 bitmap, the actual base map is 256x256. The next mipmap would be
                 * 127x127 but with the border it is 129x129 so it requires the next power of two size to store. This is what the following code is doing. 
                 * 
                 * I am not sure what happens with level = 0 and border texture, I assume the border has already been handled somewhere else
                 * 
                 * We can clean it up later to separate the mipmap from power of two code. The final >> is for the mipmap size
                 */


                // code assumes that if the left shift value is negative, the sign is ignored
                int nextPowerOfTwoWidth = 1 << (hasBorder - (int)Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - borderSize - 1))) >> (int)level;
                int nextPowerOfTwoHeight = 1 << (hasBorder - (int)Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - borderSize - 1))) >> (int)level;
                int nextPowerOfTwoDepth = 1 << (hasBorder - (int)Direct3D.D3D9x.D3D.Log2Ceiling((int)(depth - borderSize - 1))) >> (int)level;
                
                if (nextPowerOfTwoDepth <= 1)
                    nextPowerOfTwoDepth = 1;
                if (nextPowerOfTwoWidth <= 1)
                    nextPowerOfTwoWidth = 1;
                if (nextPowerOfTwoHeight <= 1)
                    nextPowerOfTwoHeight = 1;

                width_as_blocks = (uint)(nextPowerOfTwoWidth + blockWidth - 1) >> blockLogWidth;
                height_as_blocks = (uint)(nextPowerOfTwoHeight + blockHeight - 1) >> blockLogHeight;
                depth = (uint)nextPowerOfTwoDepth;
            }
            else
            {
                width_as_blocks = (width + blockWidth - 1) >> blockLogWidth;
                height_as_blocks = (height + blockHeight - 1) >> blockLogHeight;
            }

            // update point to be in terms of the block width and height
            if(point != null)
            {
                point.X >>= blockLogWidth;
                point.Y >>= blockLogWidth;
            }
            else
            {
                point = new XGPOINT();
                point.X = 0;
                point.Y = 0;
            }
            // update box bounds to be in terms of the block width and height
            if (box != null)
            {
                box.Left >>= blockLogWidth;
                box.Right = (box.Right + blockWidth - 1) >> blockLogWidth;
                box.Top >>= blockLogHeight;
                box.Bottom = (box.Bottom + blockHeight - 1) >> blockLogHeight;
            }
            else
            {
                box = new D3DBOX();
                box.Left = 0;
                box.Top = 0;
                box.Front = 0;

                var tempWidth = (width - borderSize) >> (int)level;
                if (tempWidth <= 1)
                    tempWidth = 1;
                box.Right = (uint)(tempWidth + blockWidth - 1) >> blockLogWidth;

                var tempHeight = (height - borderSize) >> (int)level;
                if (tempHeight <= 1)
                    tempHeight = 1;
                box.Bottom = (uint)(tempHeight + blockHeight - 1) >> blockLogHeight;

                box.Back = (uint)(borderSize + (depth - borderSize) >> (int)level);

            }

            if (!flags.HasFlag(XGTILE.XGTILE_NONPACKED))
            {
                XGPOINT offset = new XGPOINT();
                // need to understand the return value and modify the byte[]
                var offsetInByteArray = GetMipTailLevelOffsetCoords(width, height, depth, level, format, true, flags.HasFlag(XGTILE.XGTILE_BORDER), offset);

                box.Top += (uint)offset.Y;
                box.Bottom += (uint)offset.Y;
                box.Left += (uint)offset.X;
                box.Right += (uint)offset.X;
                box.Back += (uint)offset.Z;
                box.Front += (uint)offset.Z;
            }

            return UntileVolume(box, width_as_blocks, height_as_blocks, rowPitch, slicePitch, point, source, depth, texelPitch);
        }

        private static byte[] UntileVolume(D3DBOX box, uint width, uint height, uint rowPitch, uint slicePitch, XGPOINT point, byte[] source, uint depth, uint texelPitch)
        {
            uint boxWidth = box.Right - box.Left;
            uint boxHeight = box.Bottom - box.Top;
            uint boxDepth = box.Back - box.Front;

            uint heightRounded = (height + 31) & ~31u; // round to multiple of 32
            uint widthRounded = (width + 31) & ~31u; // round to multiple of 32
            uint depthRounded = (depth + 3) & ~3u; // round to multiple of 4

            uint totalSize = AlignToPage(widthRounded * heightRounded * depthRounded);
            byte[] result = new byte[totalSize];

            uint v15 = 1u << (int)((texelPitch >> 4) - (texelPitch >> 2) + 3);
            uint v16 = (texelPitch >> 2) + (texelPitch >> 1 >> (int)(texelPitch >> 2));
            uint v17 = (~(v15 - 1) & (box.Left + v15)) - box.Left;
            uint v18 = (~(v15 - 1) & (box.Left + boxWidth)) - box.Left;

            uint sliceOffset = slicePitch * (uint)point.Z;

            for (uint z = 0; z < boxDepth; z++)
            {
                uint v19 = z + box.Front;
                uint v20 = (v19 & 3) << (int)(v16 + 6);
                v19 >>= 2;

                for (uint y = 0; y < boxHeight; y++)
                {
                    uint v23 = y + box.Top;
                    uint v59 = 4 * (v23 & 6);
                    uint v57 = (uint)((byte)v19 + (byte)(v23 >> 3)) & 1;
                    uint v58 = 16 * (v23 & 1);
                    uint v25 = (widthRounded >> 5) * ((v19 * (heightRounded >> 4)) + (v23 >> 4));
                    uint v29 = sliceOffset + rowPitch * (y + (uint)point.Y);

                    {

                        uint v61 = v25 + (box.Left >> 5);
                        uint v32 = box.Left;
                        uint v27 = v59 + (v32 & 7u);
                        uint v33 = v27 << (int)(v16 + 6);
                        uint v34 = v58 + v20 + ((v33 >> 6) & 0xFu) + 2 * (((v33 >> 6) & ~0xFu) + 2 * ((v61 << (int)(v16 + 6)) & 0xFFFFFFFu));
                        uint v35 = v57 + 2 * ((2 * v57 + (byte)(box.Left >> 3)) & 3);
                        uint v36 = (((v34 >> 6) & 7) + 8 * (v35 & 1));

                        uint v17a = v17;
                        if (v17a > boxWidth) v17a = boxWidth;

                        uint sourceOffset = 8 * ((v34 & ~0x1FFu) + 4 * ((v35 & ~1u) + 8 * v36)) + (v34 & 0x3F);
                        uint destinationOffset = v29 + ((uint)point.X << (int)v16);
                        uint blockSize = v17a << (int)v16;

                        Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);
                    }

                    var x = v17;
                    while (x < v18)
                    {
                        uint v61 = v25 + (box.Left >> 5);
                        uint v32 = x + box.Left;
                        uint v27 = v59 + (v32 & 7);
                        uint v33 = v27 << (int)(v16 + 6);
                        uint v34 = v58 + v20 + ((v33 >> 6) & 0xF) + 2 * (((v33 >> 6) & ~0xFu) + 2 * ((v61 << (int)(v16 + 6)) & 0xFFFFFFF));
                        uint v35 = v57 + 2 * ((2 * v57 + (v32 >> 3)) & 3);
                        uint v36 = ((v34 >> 6) & 7) + 8 * (v35 & 1);

                        uint sourceOffset = 8 * ((v34 & ~0x1FFu) + 4 * ((v35 & ~1u) + 8 * v36)) + (v34 & 0x3F);
                        uint destinationOffset = v29 + ((x + (uint)point.X) << (int)v16);
                        uint blockSize = v15 << (int)v16;

                        Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);

                        x += v15;
                    }

                    if (x < boxWidth)
                    {
                        uint v61 = v25 + (box.Left >> 5);
                        uint v32 = x + box.Left;
                        uint v27 = v59 + (v32 & 7);
                        uint v33 = v27 << (int)(v16 + 6);
                        uint v34 = v58 + v20 + ((v33 >> 6) & 0xF) + 2 * (((v33 >> 6) & ~0xFu) + 2 * ((v61 << (int)(v16 + 6)) & 0xFFFFFFF));
                        uint v35 = v57 + 2 * ((2 * v57 + (v32 >> 3)) & 3);
                        uint v36 = ((v34 >> 6) & 7) + 8 * (v35 & 1);

                        uint sourceOffset = 8 * ((v34 & ~0x1FFu) + 4 * ((v35 & ~1u) + 8 * v36)) + (v34 & 0x3F);
                        uint destinationOffset = v29 + ((x + (uint)point.X) << (int)v16);
                        uint blockSize = (boxWidth - x) << (int)v16;

                        Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);
                    }
                }

                sliceOffset += slicePitch;
            }

            return result;
        }

        public static uint XGLog2LE16(uint texelPitch)
        {
            return (texelPitch >> 2) + ((texelPitch >> 1) >> (int)(texelPitch >> 2));
        }

        /// <summary>
        /// Translates the x and y offset of a pixel to the tiled memory offset in blocks
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="texelPitch"></param>
        /// <returns></returns>
        public static uint XGAddress2DTiledOffset(uint x, uint y, uint width, uint texelPitch)
        {
            uint alignedWidth; // width aligned to 32 (blocks)
            uint logBpp;       // log of bytes per pixel/texel
            uint macro;
            uint micro;
            uint offset;

            alignedWidth = (uint)((width + 31) & ~31);
            logBpp = XGLog2LE16(texelPitch);
            macro = ((x >> 5) + (y >> 5) * (alignedWidth >> 5)) << (int)(logBpp + 7);
            micro = (((x & 7) + ((y & 6) << 2)) << (int)logBpp);
            offset = (uint)(macro + ((micro & ~15) << 1) + (micro & 15) + ((y & 8) << (3 + (int)logBpp)) + ((y & 1) << 4));

            return (uint)((((offset & ~511) << 3) + ((offset & 448) << 2) + (offset & 63) +
                    ((y & 16) << 7) + (((((y & 8) >> 2) + (x >> 3)) & 3) << 6)) >> (int)logBpp);
        }

        private static uint XGAddress2DTiledX(uint offset, uint width, uint texelPitch)
        {
            uint alignedWidth = (uint)((width + 31) & ~31);

            uint logBpp = XGLog2LE16(texelPitch);
            uint offsetB = offset << (int)logBpp;
            uint offsetT = (uint)((offsetB & ~4095) >> 3) + ((offsetB & 1792) >> 2) + (offsetB & 63);
            uint offsetM = offsetT >> (7 + (int)logBpp);

            uint macroX = ((offsetM % (alignedWidth >> 5)) << 2);
            uint tile = ((((offsetT >> (5 + (int)logBpp)) & 2) + (offsetB >> 6)) & 3);
            uint macro = (macroX + tile) << 3;
            uint micro = (uint)(((((offsetT >> 1) & ~15) + (offsetT & 15)) & ((texelPitch << 3) - 1)) >> (int)logBpp);

            return macro + micro;
        }

        private static uint XGAddress2DTiledY(uint offset, uint width, uint texelPitch)
        {
            uint alignedWidth = (uint)((width + 31) & ~31);

            uint logBpp = XGLog2LE16(texelPitch);
            uint offsetB = offset << (int)logBpp;
            uint offsetT = (uint)((offsetB & ~4095) >> 3) + ((offsetB & 1792) >> 2) + (offsetB & 63);
            uint offsetM = offsetT >> (7 + (int)logBpp);

            uint macroY = ((offsetM / (alignedWidth >> 5)) << 2);
            uint tile = ((offsetT >> (6 + (int)logBpp)) & 1) + (((offsetB & 2048) >> 10));
            uint macro = (macroY + tile) << 3;
            uint micro = (uint)((((offsetT & (((texelPitch << 6) - 1) & ~31)) + ((offsetT & 15) << 1)) >> (3 + (int)logBpp)) & ~1);

            return macro + micro + ((offsetT & 16) >> 4);
        }

        /// <summary>
        /// Untile surface. The input dimensions must be in terms of blocks.
        /// </summary>
        /// <param name="width">Width of the surface in blocks</param>
        /// <param name="height">Height of the surface in blocks</param>
        /// <param name="rowPitch">Size in bytes of a row of pixels in the destination image</param>
        /// <param name="point">Offset in the surface</param>
        /// <param name="source">Source data</param>
        /// <param name="texelPitch">Size in bytes of a block</param>
        /// <param name="rect">Image rectangle to untile</param>
        /// <returns></returns>
        private static byte[] UntileSurface(uint width, uint height, uint rowPitch, XGPOINT point, byte[] source, uint texelPitch, D3DBOX rect)
        {
            uint nBlocksWidth = rect.Right - rect.Left;
            uint nBlocksHeight = rect.Bottom - rect.Top;

            uint alignedWidth = (width + 31) & ~31u;
            uint alignedHeight = (height + 31) & ~31u;

            uint totalSize = AlignToPage(alignedWidth * alignedHeight); // may not be necessary on PC

            byte[] result = new byte[totalSize];

            uint v12 = 16 / texelPitch;
            uint logBpp = XGLog2LE16(texelPitch); // log bytes per pixel
            uint v14 = (~(v12 - 1u) & (rect.Left + v12)) - rect.Left; //  v12 - (rect.Left) % v12
            uint v42 = (~(v12 - 1u) & (rect.Left + nBlocksWidth)) - rect.Left; // nBlocksWidth - (rect.Left + nBlocksWidth) % v12


            //int x = XGAddress2DTiledX(offset, xChunks, texPitch);
            //int y = XGAddress2DTiledY(offset, xChunks, texPitch);
            //int sourceIndex = ((i * xChunks) * texPitch) + (j * texPitch);
            //int destinationIndex = ((y * xChunks) * texPitch) + (x * texPitch);

            for (uint yBlockIndex = 0; yBlockIndex < nBlocksHeight; yBlockIndex++)
            {
                uint v38 = alignedWidth >> 5;
                uint _y = yBlockIndex + rect.Top;
                uint v47 = v38 * (_y >> 5);
                uint v44 = (_y >> 4) & 1;
                uint yBlockOffset = (uint)point.Y;
                uint v17 = (_y >> 3) & 1;
                uint v46 = 16 * (_y & 1);
                uint v45 = 2 * v17;
                uint v19 = rect.Left;
                uint v18 = 4 * (_y & 6);
                uint v52 = v17 << (int)(logBpp + 6);
                uint heightOffset = rowPitch * (yBlockIndex + yBlockOffset);

                {
                    uint v30 = rect.Left;
                    uint v31 = (v44 + 2 * ((v45 + (byte)(v19 >> 3)) & 3));
                    uint micro = (v18 + (v30 & 7)) << (int)(logBpp + 6);
                    uint v32 = v46 + v52 + ((micro >> 6) & 0xF) + 2 * (((micro >> 6) & ~0xFu) + (((v47 + (v19 >> 5)) << (int)(logBpp + 6)) & 0x1FFFFFFF));
                    uint v28 = ((v32 >> 6) & 7) + 8 * (v31 & 1);

                    var v37a = v14;
                    if (v37a > nBlocksWidth)
                        v37a = nBlocksWidth;

                    uint sourceOffset = 8 * ((v32 & ~0x1FFu) + 4 * ((v31 & ~1u) + 8 * v28)) + (v32 & 0x3F);
                    uint destinationOffset = heightOffset + ((uint)point.X << (int)logBpp);
                    uint blockSize = v37a;

                    Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);
                }

                uint x = v14;
                uint v48 = v14;

                while (x < v42)
                {
                    uint v30 = x + rect.Left;
                    uint v31 = v44 + 2 * ((v45 + (byte)(v30 >> 3)) & 3);
                    uint v25 = (v18 + (v30 & 7)) << (int)(logBpp + 6);
                    uint v32 = v46 + v52 + ((v25 >> 6) & 0xF) + 2 * (((v25 >> 6) & ~0xFu) + (((v47 + (v30 >> 5)) << (int)(logBpp + 6)) & 0x1FFFFFFF));
                    uint v28 = ((v32 >> 6) & 7) + 8 * (v31 & 1);

                    uint sourceOffset = 8 * ((v32 & ~0x1FFu) + 4 * ((v31 & ~1u) + 8 * v28)) + (v32 & 0x3F);
                    uint destinationOffset = heightOffset + ((v48 + (uint)point.X) << (int)logBpp);
                    uint blockSize = v12 << (int)logBpp;

                    Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);

                    x += v12;
                }

                if (x < nBlocksWidth)
                {
                    uint v30 = x + rect.Left;
                    uint v31 = v44 + 2 * ((v45 + (byte)(v30 >> 3)) & 3);
                    uint v25 = (v18 + (v30 & 7)) << (int)(logBpp + 6);
                    uint v32 = v46 + v52 + ((v25 >> 6) & 0xF) + 2 * (((v25 >> 6) & ~0xFu) + (((v47 + (v30 >> 5)) << (int)(logBpp + 6)) & 0x1FFFFFFF));
                    uint v28 = ((v32 >> 6) & 7) + 8 * (v31 & 1);

                    uint sourceOffset = 8 * ((v32 & ~0x1FFu) + 4 * ((v31 & ~1u) + 8 * v28)) + (v32 & 0x3F);
                    uint destinationOffset = heightOffset + ((v48 + (uint)point.X) << (int)logBpp);
                    uint blockSize = (nBlocksWidth - v48) << (int)logBpp;

                    Array.Copy(source, sourceOffset, result, destinationOffset, blockSize);
                }
            }

            return result;
        }

        private static uint AlignToPage(uint offset)
        {
            return (offset + 0xFFF) & 0xFFFFF000;
        }

        private static int UnknownAlign(int offset1, int offset2)
        {
            return offset1 ^ (offset1 ^ offset2) & 0xFFF;
        }

        public static uint GetUntiledLevelOffset(uint width, uint height, uint depth, uint level, D3D9xGPU.GPUTEXTUREFORMAT format)
        {
            /*
             * Returns the offset of the level in the untiled texture
             */
            uint blockWidth = 0;
            uint blockHeight = 0;

            XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);

            uint mipLevelRequiresOffset = GetMipLevelRequiresOffset(width, height, 0);

            if (level >= mipLevelRequiresOffset) // happens when the requested level bitmap dimensions are <= 16
            {
                uint result = 0;
                uint blockOffsetX = 0;
                uint blockOffsetY = 0;
                uint logWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)(width - 1));
                uint logHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)(height - 1));
                // get level width and height
                uint logLevelWidth = logWidth - level;
                uint logLevelHeight = logHeight - level;
                uint nextPo2Width = (uint)1 << (int)(logLevelWidth);
                uint nextPo2Height = (uint)1 << (int)(logLevelHeight);

                uint bitsPerPixel = XGBitsPerPixelFromGpuFormat(format);
                uint texelPitch = (blockWidth * blockHeight * bitsPerPixel) >> 3;
                uint slicePitch = 32 * texelPitch;
                // convert to block sizes

                nextPo2Height /= blockHeight;
                nextPo2Width /= blockWidth;

                // a tile is 32x32 (blocks)

                if(logLevelWidth == logLevelHeight)
                {
                    // square
                    switch (logLevelWidth)
                    {
                        case 4:
                            blockOffsetX = 16 / blockWidth;
                            blockOffsetY = 0;
                            break;
                        case 3:
                            blockOffsetX = 8 / blockWidth;
                            blockOffsetY = 0;
                            break;
                        case 2:
                            blockOffsetX = 4 / blockWidth;
                            blockOffsetY = 0;
                            break;
                        case 1:
                            blockOffsetX = 0;
                            blockOffsetY = 8 / blockHeight;
                            break;
                        case 0:
                            blockOffsetX = 0;
                            blockOffsetY = 4 / blockHeight;
                            break;
                    }

                }
                else if (logLevelWidth < logLevelHeight)
                {
                    // vertical
                }
                else
                {
                    // horizontal
                }

                result = slicePitch * blockOffsetY + texelPitch * blockOffsetX;

                return result;
            }
            else
            {
                return 0;
            }
        }
    }

}
