using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Bitmaps.DDS;

namespace TagTool.Bitmaps
{
    public static class BitmapDdsFormatDetection
    {
        public static void SetUpHeaderForFormat(BitmapFormat format, DDSHeader header)
        {
            if (!ExtractionDefinitions.TryGetValue(format, out BitmapFormatDefinition definition))
                throw new InvalidOperationException("Invalid bitmap format: " + format);
            header.PixelFormat.RGBBitCount = definition.BitsPerPixel;
            header.PixelFormat.RBitMask = definition.RBitMask;
            header.PixelFormat.GBitMask = definition.GBitMask;
            header.PixelFormat.BBitMask = definition.BBitMask;
            header.PixelFormat.ABitMask = definition.ABitMask;
            header.PixelFormat.FourCC = definition.FourCc;
        }

        public static BitmapFormat DetectFormat(DDSHeader header)
        {
            InitInjectionDefinitions();
            var definition = new BitmapFormatDefinition
            {
                BitsPerPixel = header.PixelFormat.RGBBitCount,
                RBitMask = header.PixelFormat.RBitMask,
                GBitMask = header.PixelFormat.GBitMask,
                BBitMask = header.PixelFormat.BBitMask,
                ABitMask = header.PixelFormat.ABitMask,
                FourCc = header.PixelFormat.FourCC
            };
            if (!_injectionDefinitions.TryGetValue(definition, out BitmapFormat result))
                throw new InvalidOperationException("Unsupported pixel format");
            return result;
        }

        public static BitmapType DetectType(DDSHeader dds)
        {
            if ((dds.Caps2 & DDSSurfaceInfoFlags.CubeMap) != 0)
                return BitmapType.CubeMap;
            if ((dds.Caps2 & DDSSurfaceInfoFlags.Volume) != 0)
                return BitmapType.Texture3D;
            return BitmapType.Texture2D;
        }

        private static void InitInjectionDefinitions()
        {
            if (_injectionDefinitions == null)
                _injectionDefinitions = ExtractionDefinitions.Where(d => d.Key != BitmapFormat.A4R4G4B4Font && d.Key != BitmapFormat.AY8).ToDictionary(p => p.Value, p => p.Key);
        }

        private class BitmapFormatDefinition
        {
            public DDSFormatType FormatType { get; set; }

            public int BitsPerPixel { get; set; }

            public uint RBitMask { get; set; }

            public uint GBitMask { get; set; }

            public uint BBitMask { get; set; }

            public uint ABitMask { get; set; }

            public uint FourCc { get; set; }

            public DXGIFormat D3D10Format { get; set; }

            public override bool Equals(object obj)
            {
                var other = obj as BitmapFormatDefinition;
                if (other == null)
                    return false;
                return (FormatType == other.FormatType && BitsPerPixel == other.BitsPerPixel &&
                        RBitMask == other.RBitMask && GBitMask == other.GBitMask && BBitMask == other.BBitMask &&
                        ABitMask == other.ABitMask && FourCc == other.FourCc && D3D10Format == other.D3D10Format);
            }

            public override int GetHashCode()
            {
                var result = 17;
                result = result * 13 + FormatType.GetHashCode();
                result = result * 13 + BitsPerPixel.GetHashCode();
                result = result * 13 + RBitMask.GetHashCode();
                result = result * 13 + GBitMask.GetHashCode();
                result = result * 13 + BBitMask.GetHashCode();
                result = result * 13 + ABitMask.GetHashCode();
                result = result * 13 + FourCc.GetHashCode();
                result = result * 13 + D3D10Format.GetHashCode();
                return result;
            }
        }

        private static readonly Dictionary<BitmapFormat, BitmapFormatDefinition> ExtractionDefinitions = new Dictionary
            <BitmapFormat, BitmapFormatDefinition>
        {
            { BitmapFormat.A8,            new BitmapFormatDefinition { FormatType = DDSFormatType.Alpha, BitsPerPixel = 8, ABitMask = 0xFF } },
            { BitmapFormat.Y8,            new BitmapFormatDefinition { FormatType = DDSFormatType.Luminance, BitsPerPixel = 8, RBitMask = 0xFF } },
            { BitmapFormat.AY8,           new BitmapFormatDefinition { FormatType = DDSFormatType.Alpha, BitsPerPixel = 8, ABitMask = 0xFF } },
            { BitmapFormat.A8Y8,          new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 16, ABitMask = 0xFF00, RBitMask = 0x00FF } },
            { BitmapFormat.R5G6B5,        new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 16, RBitMask = 0xF800, GBitMask = 0x07E0, BBitMask = 0x001F } },
            { BitmapFormat.A1R5G5B5,      new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 16, ABitMask = 0x8000, RBitMask = 0x7C00, GBitMask = 0x03E0, BBitMask = 0x001F } },
            { BitmapFormat.A4R4G4B4,      new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 16, ABitMask = 0xF000, RBitMask = 0x0F00, GBitMask = 0x00F0, BBitMask = 0x000F } },
            { BitmapFormat.X8R8G8B8,      new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 32, RBitMask = 0x00FF0000, GBitMask = 0x0000FF00, BBitMask = 0x000000FF } },
            { BitmapFormat.A8R8G8B8,      new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 32, ABitMask = 0xFF000000, RBitMask = 0x00FF0000, GBitMask = 0x0000FF00, BBitMask = 0x000000FF } },
            { BitmapFormat.Dxt1,          new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = DDSFourCC.FromString("DXT1") } },
            { BitmapFormat.Dxt3,          new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = DDSFourCC.FromString("DXT3") } },
            { BitmapFormat.Dxt5,          new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = DDSFourCC.FromString("DXT5") } },
            { BitmapFormat.A4R4G4B4Font,  new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 16, ABitMask = 0xF000, RBitMask = 0x0F00, GBitMask = 0x00F0, BBitMask = 0x000F } },
            { BitmapFormat.Dxn,           new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = DDSFourCC.FromString("ATI2") } },
            { BitmapFormat.A16B16G16R16F, new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = 0x00000071} },
            { BitmapFormat.A32B32G32R32F, new BitmapFormatDefinition { FormatType = DDSFormatType.Other, FourCc = 0x00000074} },
            { BitmapFormat.V8U8,          new BitmapFormatDefinition { FormatType = DDSFormatType.Luminance, BitsPerPixel=16 } },
            { BitmapFormat.A2R10G10B10,   new BitmapFormatDefinition { FormatType = DDSFormatType.RGB, BitsPerPixel = 32, ABitMask = 0xC0000000, RBitMask = 0x3FF00000, GBitMask = 0x000FFC00, BBitMask = 0x000003FF } }
        };

        private static Dictionary<BitmapFormatDefinition, BitmapFormat> _injectionDefinitions;
    }
}
