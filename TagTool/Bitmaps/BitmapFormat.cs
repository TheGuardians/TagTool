using System;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// Bitmap data formats.
    /// </summary>
    public enum BitmapFormat : sbyte
    {
        A8,                 // 0x00, alpha
        Y8,                 // 0x01, intensity
        AY8,                // 0x02, combined alpha-intensity
        A8Y8,               // 0x03, separate alpha-intensity
        Unused4,            // 0x04
        Unused5,            // 0x05
        R5G6B5,             // 0x06, high-color
        Unused7,            // 0x07
        A1R5G5B5,           // 0x08, high-color with 1-bit alpha
        A4R4G4B4,           // 0x09, high-color with alpha
        X8R8G8B8,           // 0x0A, true-color
        A8R8G8B8,           // 0x0B, true-color with alpha
        UnusedC,            // 0x0C
        UnusedD,            // 0x0D
        Dxt1,               // 0x0E, compressed with color-key transparency ('DXT1')
        Dxt3,               // 0x0F, compressed with explicit alpha ('DXT3')
        Dxt5,               // 0x10, compressed with interpolated alpha ('DXT5')
        A4R4G4B4Font,       // 0x11, font format
        P8,                 // 0x12
        ARGBFP32,           // 0x13
        RGBFP32,            // 0x14
        RGBFP16,            // 0x15
        V8U8,               // 0x16, v8u8 signed 8-bit normals
        Unused17,           // 0x17
        A32B32G32R32F,      // 0x18, 32 bit float ABGR
        A16B16G16R16F,      // 0x19, 16 bit float ABGR
        Q8W8V8U8,           // 0x1A, 8 bit signed 4 channel
        A2R10G10B10,        // 0x1B, 30-bit color 2-bit alpha
        A16B16G16R16,       // 0x1C, 48-bit color 16-bit alpha
        V16U16,             // 0x1D, v16u16 signed 16-bit normals
        Unused1E,           // 0x1E
        Dxt5a,              // 0x1F
        Unused20,           // 0x20
        Dxn,                // 0x21, compressed normals: high quality ('ATI2')
        Ctx1,               // 0x22
        Dxt3aAlpha,         // 0x23
        Dxt3aMono,          // 0x24
        Dxt5aAlpha,         // 0x25
        Dxt5aMono,          // 0x26, Reach DXN
        DxnMonoAlpha,       // 0x27, Reach CTX1
        ReachDxt3aMono,     // 0x28, Reach Dxt3aMono
        ReachDxt3aAlpha,    // 0x29, Reach Dxt3aAlpha
        ReachDxt5aMono,     // 0x2A, Reach Dxt5aAlpha
        ReachDxt5aAlpha,    // 0x2B, Reach Dxt5aMono
        ReachDxnMonoAlpha   // 0x2C, Reach DxnMonoAlpha
    }

    public static class BitmapFormatUtils
    {
        public static int GetBitsPerPixel(BitmapFormat format)
        {
            int bitsPerPixel = 0;
            switch (format)
            {
                case BitmapFormat.A8:
                case BitmapFormat.AY8:
                case BitmapFormat.Y8:
                case BitmapFormat.P8:
                    bitsPerPixel = 8;
                    break;

                case BitmapFormat.A8Y8:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A1R5G5B5:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.A4R4G4B4Font:
                case BitmapFormat.V8U8:
                    bitsPerPixel = 16;
                    break;

                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.X8R8G8B8:
                case BitmapFormat.Q8W8V8U8:
                case BitmapFormat.A2R10G10B10:
                case BitmapFormat.V16U16:
                    bitsPerPixel = 32;
                    break;

                case BitmapFormat.RGBFP16:
                    bitsPerPixel = 48;
                    break;

                case BitmapFormat.A16B16G16R16:
                case BitmapFormat.A16B16G16R16F:
                    bitsPerPixel = 64;
                    break;

                case BitmapFormat.RGBFP32:
                    bitsPerPixel = 96;
                    break;

                case BitmapFormat.ARGBFP32:
                case BitmapFormat.A32B32G32R32F:
                    bitsPerPixel = 128;
                    break;

                default:
                    throw new Exception($"Unsupported uncompressed format {format}");
            }

            return bitsPerPixel;
        }

        public static int GetBlockSize(BitmapFormat format)
        {
            int blockSize = 0;
            switch (format)
            {
                case BitmapFormat.Dxt1:
                    blockSize = 8;
                    break;
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.ReachDxt3aAlpha:
                case BitmapFormat.ReachDxt3aMono:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.ReachDxt5aAlpha:
                case BitmapFormat.ReachDxt5aMono:
                case BitmapFormat.Dxn:
                case BitmapFormat.Ctx1:
                    blockSize = 16;
                    break;
                default:
                    throw new Exception($"Unsupported Compressed Format {format}");
            }
            return blockSize;
        }
    }
}
