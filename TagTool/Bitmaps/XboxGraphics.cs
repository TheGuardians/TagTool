using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.D3D9x;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// C# implementation of xgraphics
    /// </summary>
    public static class XboxGraphics
    {
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

    }
}
