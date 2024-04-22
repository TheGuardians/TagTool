using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    [Flags]
    public enum DDSPixelFormatFlags
    {
        // The surface has alpha channel information in the pixel format.
        AlphaPixels = 0x1,

        // The pixel format contains alpha only information
        Alpha = 0x2,

        // The FourCC code is valid.
        FourCC = 0x4,

        // The surface is 4-bit color indexed.
        PaletteIndexed4 = 0x8,

        // The surface is indexed into a palette which stores indices into the destination surface's 8-bit palette.
        PaletteIndexedT08 = 0x10,

        // The surface is 8-bit color indexed.
        PaletteIndexed8 = 0x20,

        // The RGB data in the pixel format structure is valid.
        RGB = 0x40,

        RGBA = 0x41,
        
        // The surface will accept pixel data in the format specified and compress it during the write.
        Compressed = 0x80,

        // The surface will accept RGB data and translate it during the write to YUV data.  The format of the data to be written
        // will be contained in the pixel format structure.  The DDPF_RGB flag will be set.
        RGBToYUV = 0x100,

        // Pixel format is YUV - YUV data in pixel format struct is valid.
        YUV = 0x200,

        // Pixel format is a z buffer only surface.
        ZBuffer = 0x400,

        // The surface is 1-bit color indexed.
        PaletteIndexed1 = 0x800,

        // The surface is 2-bit color indexed.
        PaletteIndexed2 = 0x1000,

        // The surface contains Z information in the pixels.
        ZPixels = 0x2000,

        // The surface contains stencil information along with Z.
        StencilBuffer = 0x4000,

        // Premultiplied alpha format -- the color components have been premultiplied by the alpha component.
        PremultipliedAlpha = 0x8000,

        // Luminance data in the pixel format is valid. Use this flag for luminance-only or luminance+alpha surfaces,
        // the bit depth is then ddpf.dwLuminanceBitCount.
        Luminance = 0x20000,

        // Luminance data in the pixel format is valid. Use this flag when hanging luminance off bumpmap surfaces,
        // the bit mask for the luminance portion of the pixel is then ddpf.dwBumpLuminanceBitMask.
        BumpLuminance = 0x40000,

        // Bump map dUdV data in the pixel format is valid.
        BumpDUDV = 0x80000   

    }
}

