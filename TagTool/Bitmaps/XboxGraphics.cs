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


        private static uint GetMipTailLevelOffsetCoords(uint width, uint height, uint depth, uint level, D3D9xGPU.GPUTEXTUREFORMAT format, bool isTiled, bool hasBorder, int SHOULDBE3DOFFSETS)
        {
            return 0;
        }


        public static byte[] XGUntileVolume(uint rowPitch, uint slicePitch, XGPOINT point, byte[] source, uint width, uint height, uint depth, D3DBOX box, uint texelPitch)
        {
            return UntileVolume(box, width, height, rowPitch, slicePitch, point, source, depth, texelPitch);
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
            texelPitch = (bitsPerPixel << (blockLogWidth + blockLogHeight)) >> 8; // also bytes per block


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
                var offsetInByteArray = GetMipTailLevelOffsetCoords(width, height, depth, level, format, true, flags.HasFlag(XGTILE.XGTILE_BORDER), 0);
                // update box using the returned 3d offsets TODO
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
