using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TagTool.Bitmaps;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;
using TagTool.Cache;
using TagTool.Bitmaps.DDS;

namespace TagTool.Commands.Gen2.Bitmaps
{
    public static class BitmapConverterGen2
    {
        public static BaseBitmap ExtractBitmap(GameCacheGen2 cache, BitmapGen2 gen2Bitmap, int imageIndex)
        {
            BitmapGen2.BitmapDataBlock gen2Img = gen2Bitmap.Bitmaps[imageIndex];
            byte[] rawBitmapData = ConvertBitmapData(cache, gen2Bitmap, gen2Img);
            Bitmap.Image newImg = ConvertBitmapImage(cache, gen2Img, rawBitmapData);
            return new BaseBitmap(newImg, rawBitmapData);
        }
        public static DDSFile ExtractBitmapDDS(GameCacheGen2 cache, BitmapGen2 bitmap, int imageIndex)
        {
            var baseBitmap = ExtractBitmap(cache, bitmap, imageIndex);
            if (baseBitmap == null)
                return null;

            return new DDSFile(baseBitmap);
        }

        public static byte[] ConvertBitmapData(GameCacheGen2 cache, BitmapGen2 gen2Bitmap, BitmapGen2.BitmapDataBlock gen2Img)
        {
            //get raw bitmap data and create resource
            byte[] rawBitmapData = cache.GetCacheRawData((uint)gen2Img.Lod0Pointer, gen2Img.Lod0Size);

            //h2v raw bitmap data is gz compressed
            if (cache.Version == TagTool.Cache.CacheVersion.Halo2Vista)
            {
                using (var stream = new MemoryStream(rawBitmapData))
                using (var resultStream = new MemoryStream())
                using (var zstream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    zstream.CopyTo(resultStream);
                    rawBitmapData = resultStream.ToArray();
                }
            }

            //deswizzle
            if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Swizzled))
            {
                rawBitmapData = Swizzle(rawBitmapData, gen2Img.Width, gen2Img.Height, gen2Img.Depth,
                    CalculateBytesPerPixel(gen2Img), true);
            }

            //handle p8_bump format using bump palette
            if (gen2Img.Format == BitmapGen2.BitmapDataBlock.FormatValue.P8Bump)
            {
                var outStream = new MemoryStream();
                var outWriter = new EndianWriter(outStream);
                foreach (var pix in rawBitmapData)
                    outWriter.Write(bumpPalette[pix]);
                rawBitmapData = outStream.ToArray();
            }           
            else if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Palettized))
                //convert palettized formats to A8R8B8G8
                rawBitmapData = ConvertP8BitmapData(rawBitmapData);

            //normalize X8R8G8B8 bumpmaps
            if (gen2Img.Format == BitmapGen2.BitmapDataBlock.FormatValue.X8r8g8b8 &&
                gen2Bitmap.Usage == BitmapGen2.UsageValue.HeightMap)
                rawBitmapData = NormalizeX8R8G8B8HeightMap(rawBitmapData);

            return rawBitmapData;
        }

        public static Bitmap.Image ConvertBitmapImage(GameCacheGen2 cache, BitmapGen2.BitmapDataBlock gen2Img, byte[] rawBitmapData)
        {
            Bitmap.Image newImg = new Bitmap.Image
            {
                Signature = gen2Img.Signature,
                Width = gen2Img.Width,
                Height = gen2Img.Height,
                Depth = gen2Img.Depth,
                Type = ConvertBitmapType(gen2Img.Type),
                Format = ConvertBitmapFormat(gen2Img.Format),
                RegistrationPoint = gen2Img.RegistrationPoint,
                MipmapCount = (sbyte)gen2Img.MipmapCount,
                Flags = new BitmapFlags(),
                Curve = BitmapImageCurve.xRGB, //default to this for now
            };

            if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.PowerOfTwoDimensions))
                newImg.Flags |= BitmapFlags.PowerOfTwoDimensions;
            if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Compressed))
                newImg.Flags |= BitmapFlags.Compressed;
            if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Palettized))
                newImg.Format = BitmapFormat.A8R8G8B8;

            //set pixel data size after decompression and modification
            newImg.PixelDataSize = rawBitmapData.Length;

            return newImg;
        }

        private static byte[] ConvertP8BitmapData(byte[] data)
        {
            //convert p8 palletized data to A8R8B8G8
            byte[] outputdata = new byte[(data.Length - 1024) * 4];
            using (var dataStream = new MemoryStream(data))
            using (var outStream = new MemoryStream(outputdata))
            using (var reader = new EndianReader(dataStream))
            using (var writer = new EndianWriter(outStream))
            {
                List<ArgbColor> palette = new List<ArgbColor>();
                //256 ARGB colors form the palette
                for (var i = 0; i < 256; i++)
                    palette.Add(reader.ReadArgbColor());
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                    writer.Write(palette[reader.ReadByte()].GetValue());
            }
            return outputdata;
        }

        private static byte[] NormalizeX8R8G8B8HeightMap(byte[] data)
        {
            using (var dataStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            using (var reader = new EndianReader(dataStream))
            using (var writer = new EndianWriter(outStream))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ArgbColor color = reader.ReadArgbColor();
                    Vector3 vec = new Vector3(color.Red, color.Green, color.Blue);
                    vec = Vector3.Normalize(vec);
                    color = new ArgbColor
                    {
                        Alpha = color.Alpha,
                        Red = (byte)(vec.X * 255.0f),
                        Green = (byte)(vec.Y * 255.0f),
                        Blue = (byte)(vec.Z * 255.0f),
                    };
                    writer.Write(color.GetValue());
                }
                return outStream.ToArray();
            }
        }

        private static int CalculateBytesPerPixel(BitmapGen2.BitmapDataBlock gen2Img)
        {
            int bitsperpixel = 16;
            //handle p8 and p8bump deswizzle
            switch (gen2Img.Format)
            {
                case BitmapGen2.BitmapDataBlock.FormatValue.P8:
                case BitmapGen2.BitmapDataBlock.FormatValue.P8Bump:
                    bitsperpixel = 8;
                    break;
                default:
                    bitsperpixel = BitmapFormatUtils.GetBitsPerPixel(ConvertBitmapFormat(gen2Img.Format));
                    break;
            }
            return bitsperpixel / 8;
        }

        private static BitmapType ConvertBitmapType(BitmapGen2.BitmapDataBlock.TypeValue type)
        {
            switch (type)
            {
                case BitmapGen2.BitmapDataBlock.TypeValue._2dTexture:
                    return BitmapType.Texture2D;
                case BitmapGen2.BitmapDataBlock.TypeValue._3dTexture:
                    return BitmapType.Texture3D;
                case BitmapGen2.BitmapDataBlock.TypeValue.CubeMap:
                    return BitmapType.CubeMap;
                default:
                    return BitmapType.Texture2D;
            }
        }

        private static BitmapFormat ConvertBitmapFormat(BitmapGen2.BitmapDataBlock.FormatValue format)
        {
            BitmapFormat result;
            if (format == BitmapGen2.BitmapDataBlock.FormatValue.P8Bump)
                return BitmapFormat.A8R8G8B8;
            else if (format == BitmapGen2.BitmapDataBlock.FormatValue.P8)
                return BitmapFormat.A8;
            else if (Enum.TryParse(format.ToString(), true, out result))
                return result;
            else
            {
                new TagToolError(CommandError.None, $"Failed to find bitmap format matching {format}");
                return BitmapFormat.A8R8G8B8;
            }
        }

        /* http://www.h2maps.net/Tools/Xbox/Mutation/Mutation/DDS/Swizzle.cs */

        private class MaskSet
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Z;

            public MaskSet(int w, int h, int d)
            {
                var bit = 1;
                var index = 1;

                while (bit < w || bit < h || bit < d)
                {
                    if (bit < w)
                    {
                        X |= index;
                        index <<= 1;
                    }

                    if (bit < h)
                    {
                        Y |= index;
                        index <<= 1;
                    }

                    if (bit < d)
                    {
                        Z |= index;
                        index <<= 1;
                    }

                    bit <<= 1;
                }
            }
        }

        public static byte[] Swizzle(byte[] data, int width, int height, int depth, int bpp) => Swizzle(data, width, height, depth, bpp, true);

        public static byte[] Swizzle(byte[] data, int width, int height, int depth, int bpp, bool deswizzle)
        {
            int a, b;
            var output = new byte[data.Length];

            var masks = new MaskSet(width, height, depth);
            for (var y = 0; y < height * depth; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (deswizzle)
                    {
                        a = ((y * width) + x) * bpp;
                        b = Swizzle(x, y, depth, masks) * bpp;
                    }
                    else
                    {
                        b = ((y * width) + x) * bpp;
                        a = Swizzle(x, y, depth, masks) * bpp;
                    }

                    if (a < output.Length && b < data.Length)
                    {
                        for (var i = 0; i < bpp; i++)
                            output[a + i] = data[b + i];
                    }
                    else
                        return null;
                }
            }

            return output;
        }

        private static int Swizzle(int x, int y, int z, MaskSet masks) => SwizzleAxis(x, masks.X) | SwizzleAxis(y, masks.Y) | (z == -1 ? 0 : SwizzleAxis(z, masks.Z));

        private static int SwizzleAxis(int val, int mask)
        {
            var bit = 1;
            var result = 0;

            while (bit <= mask)
            {
                var tmp = mask & bit;

                if (tmp != 0)
                    result |= val & bit;
                else
                    val <<= 1;

                bit <<= 1;
            }

            return result;
        }

        static uint[] bumpPalette = new uint[]{0xFF7E7EFF, 0xFF7F7EFF, 0xFF807EFF, 0xFF817EFF, 0xFF7E7FFF, 0xFF7F7FFF,
            0xFF807FFF, 0xFF817FFF, 0xFF7E80FF, 0xFF7F80FF, 0xFF8080FF, 0xFF8180FF, 0xFF7E81FF, 0xFF7F81FF, 0xFF8081FF,
            0xFF8181FF, 0xFF827FFF, 0xFF7F83FF, 0xFF7F7DFF, 0xFF8381FF, 0xFF7C81FF, 0xFF827CFF, 0xFF8184FF, 0xFF7C7DFF,
            0xFF857FFF, 0xFF7D84FF, 0xFF807AFF, 0xFF8484FF, 0xFF7A80FF, 0xFF857CFF, 0xFF7F87FF, 0xFF7C7AFF, 0xFF8882FF,
            0xFF7984FF, 0xFF8378FF, 0xFF8488FF, 0xFF777CFF, 0xFF897DFF, 0xFF7B89FF, 0xFF7D76FF, 0xFF8986FF, 0xFF7582FF,
            0xFF8777FF, 0xFF818CFF, 0xFF7778FF, 0xFF8D80FF, 0xFF7789FF, 0xFF8173FF, 0xFF888BFF, 0xFF727EFF, 0xFF8C78FF,
            0xFF7C8EFF, 0xFF7973FF, 0xFF8E85FF, 0xFF7186FF, 0xFE8771FF, 0xFE8590FF, 0xFE7178FF, 0xFE917CFF, 0xFE768EFF,
            0xFE7E6EFF, 0xFE8E8CFF, 0xFE6D81FF, 0xFE8E72FF, 0xFE7F93FF, 0xFE7371FF, 0xFE9483FF, 0xFE6F8CFF, 0xFE856BFF,
            0xFE8B93FF, 0xFE6B79FF, 0xFE9477FF, 0xFD7795FF, 0xFD786AFF, 0xFD958BFF, 0xFD6986FF, 0xFD8D6CFF, 0xFD8498FF,
            0xFD6C71FF, 0xFD997EFF, 0xFD6F93FF, 0xFD8066FF, 0xFD9293FF, 0xFD657EFF, 0xFD966FFF, 0xFC7B9BFF, 0xFC7168FF,
            0xFC9B87FF, 0xFC678DFF, 0xFC8A65FF, 0xFC8B9BFF, 0xFC6573FF, 0xFC9D77FF, 0xFC719BFF, 0xFC7962FF, 0xFC9A92FF,
            0xFB6084FF, 0xFB9567FF, 0xFB81A1FF, 0xFB6969FF, 0xFBA181FF, 0xFB6696FF, 0xFB845EFF, 0xFB949CFF, 0xFB5E78FF,
            0xFB9F6EFF, 0xFA75A2FF, 0xFA715FFF, 0xFAA28EFF, 0xFA5D8DFF, 0xFA915FFF, 0xFA8AA4FF, 0xFA606CFF, 0xFAA679FF,
            0xF9689FFF, 0xF97D59FF, 0xF99D9BFF, 0xF95880FF, 0xF99E65FF, 0xF97CA9FF, 0xF9675FFF, 0xF8A987FF, 0xF85C97FF,
            0xF88B57FF, 0xF894A6FF, 0xF85771FF, 0xF8A86FFF, 0xF86DA8FF, 0xF77356FF, 0xF7A796FF, 0xF7548AFF, 0xF79A5BFF,
            0xF786AEFF, 0xF75C62FF, 0xF7AF7EFF, 0xF65EA2FF, 0xF68250FF, 0xF69FA5FF, 0xF6507AFF, 0xF6A864FF, 0xF675B0FF,
            0xF56755FF, 0xF5B08FFF, 0xF55295FF, 0xF59451FF, 0xF592B0FF, 0xF45268FF, 0xF4B272FF, 0xF464ACFF, 0xF4774CFF,
            0xF4AAA1FF, 0xF44A85FF, 0xF3A558FF, 0xF380B7FF, 0xF35B57FF, 0xF3B785FF, 0xF354A2FF, 0xF28A49FF, 0xF29EB0FF,
            0xF24971FF, 0xF2B365FF, 0xF26CB6FF, 0xF16A4AFF, 0xF1B599FF, 0xF14892FF, 0xF19E4CFF, 0xF08DBBFF, 0xF04F5DFF,
            0xF0BC78FF, 0xF059AFFF, 0xF07D42FF, 0xEFACACFF, 0xEF427DFF, 0xEFB058FF, 0xEF78BFFF, 0xEE5C4CFF, 0xEEBF8EFF,
            0xEE48A0FF, 0xEE9442FF, 0xED9CBBFF, 0xED4367FF, 0xEDBE69FF, 0xED61BBFF, 0xED6F3FFF, 0xECB9A4FF, 0xEC3D8CFF,
            0xECAA4AFF, 0xEB86C4FF, 0xEB4D51FF, 0xEBC580FF, 0xEB4DAFFF, 0xEA863AFF, 0xEAABB8FF, 0xEA3A74FF, 0xEABC5AFF,
            0xE96DC5FF, 0xE95F40FF, 0xE9C499FF, 0xE93D9CFF, 0xE89F3EFF, 0xE896C6FF, 0xE8405BFF, 0xE7C970FF, 0xE755BDFF,
            0xE77635FF, 0xE7BAB1FF, 0xE63483FF, 0xE6B64AFF, 0xE67DCDFF, 0xE54E45FF, 0xE5CD8AFF, 0xE540ADFF, 0xE49133FF,
            0xE4A7C4FF, 0xE43468FF, 0xE3C85EFF, 0xE361CAFF, 0xE36534FF, 0xE3C8A5FF, 0xE23195FF, 0xE2AC3BFF, 0xE28ED1FF,
            0xE13F4EFF, 0xE1D379FF, 0xE148BDFF, 0xE0802CFF, 0xE0B9BEFF, 0xE02C79FF, 0xDFC34CFF, 0xDF71D4FF, 0xDF5238FF,
            0xDED396FF, 0xDE33A8FF, 0xDD9E2FFF, 0xDDA1D1FF, 0xDD315BFF, 0xDCD466FF, 0xDC54CCFF, 0xDC6D29FF, 0xDBC9B3FF,
            0xDB278CFF, 0xDBBA3BFF, 0xDA84DAFF, 0xDA4040FF, 0xD9DB84FF, 0xD93ABBFF, 0xD98C25FF, 0xD8B5CBFF, 0xD8266CFF,
            0xD8D052FF, 0xD764D9FF, 0xD7592BFF, 0xD6D7A4FF, 0xD627A0FF, 0xD6AC2CFF, 0xFF808000};
    }
}
