using System;
using static TagTool.Tags.Definitions.Bitmap;

namespace TagTool.Bitmaps.Utils
{
    public static class BitmapUtilsPC
    {
        public static int GetTextureOffset(Image bitmap, int mipmapIndex)
        {
            switch (bitmap.Type)
            {
                case BitmapType.Texture2D:
                    return GetTexture2DOffset(bitmap, 0, 0, mipmapIndex);
                case BitmapType.Texture3D:
                    return GetTexture3DOffset(bitmap, 0, 0, 0, mipmapIndex);
                case BitmapType.CubeMap:
                    return GetTextureCubemapOffset(bitmap, 0, 0, 0, mipmapIndex);
                case BitmapType.Array:
                    return GetTextureArrayOffset(bitmap, 0, 0, 0, mipmapIndex);
                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetTexture2DOffset(Image bitmap, int x, int y, int mipmapIndex)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int align = bitmap.Flags.HasFlag(BitmapFlags.Compressed) ? 4 : 1;
            int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);

            int offset = 0;
            for (int i = 0; i < mipmapIndex; i++)
            {
                offset += ((width + (align - 1)) & ~(align - 1)) * ((height + (align - 1)) & ~(align - 1));
                width = Math.Max(width >> 1, align);
                height = Math.Max(height >> 1, align);
            }

            return (offset + x + y * width) * bitsPerPixel / 8;
        }

        public static int GetTexture3DOffset(Image bitmap, int x, int y, int z, int mipmapIndex)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int depth = bitmap.Depth;
            int align = bitmap.Flags.HasFlag(BitmapFlags.Compressed) ? 4 : 1;
            int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);

            int offset = 0;
            for (int i = 0; i < mipmapIndex; ++i)
            {
                offset += width * height * depth;
                width = Math.Max(width >> 1, align);
                height = Math.Max(height >> 1, align);
                depth = Math.Max(depth >> 1, align);
            }
            return (bitsPerPixel * (offset + width * (y + z * height) + x) / 8);
        }

        public static int GetTextureCubemapOffset(Image bitmap, int x, int y, int z, int mipmapIndex)
        {
            int width = bitmap.Width;
            int align = bitmap.Flags.HasFlag(BitmapFlags.Compressed) ? 4 : 1;
            int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);

            int offset = 0;
            for (int i = 0; i < mipmapIndex; i++)
            {
                offset += 6 * width * width;
                width = Math.Max(width >> 1, align);
            }

            return (offset + x + y * width + z * width * width) * bitsPerPixel / 8;
        }

        public static int GetTextureArrayOffset(Image bitmap, int x, int y, int z, int mipmapIndex)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int align = bitmap.Flags.HasFlag(BitmapFlags.Compressed) ? 4 : 1;
            
            int offset = 0;
            for(int i = 0; i < z; i++)
            {
                for (int j = 0; j < mipmapIndex; j++)
                {
                    width = Math.Max(width >> 1, align);

                    int size;
                    if (bitmap.Flags.HasFlag(BitmapFlags.Compressed))
                    {
                        int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);
                        size = bitsPerPixel * (width >> 2) * (width >> 2);
                    }
                    else
                    {
                        int bytesPerBlock = GetBytesPerBlock(bitmap.Format);
                        height = Math.Max(height >> 1, align);
                        size = height * width * bytesPerBlock / 8;
                    }

                    offset += size;
                }
            }
 
            return offset;
        }

        public static int GetMipmapWidth(Image bitmap, int mipmapIndex)
        {
            int width = Math.Max(bitmap.Width >> mipmapIndex, 1);
            if (bitmap.Flags.HasFlag(BitmapFlags.Compressed))
                width = width + (4 - (width & 3) & 3);
            return width;
        }

        public static int GetMipmapHeight(Image bitmap, int mipmapIndex)
        {
            int height = Math.Max(bitmap.Height >> mipmapIndex, 1);
            if (bitmap.Flags.HasFlag(BitmapFlags.Compressed))
                height = height + (4 - (height & 3) & 3);
            return height;
        }

        public static int GetMipmapDepth(Image bitmap, int mipmapIndex)
        {
            return Math.Max(bitmap.Depth >> mipmapIndex, 1);
        }

        public static int GetMipmapPixelCount(Image bitmap, int mipmapIndex)
        {
            int width = GetMipmapWidth(bitmap, mipmapIndex);
            int height = GetMipmapHeight(bitmap, mipmapIndex);
            int depth = GetMipmapDepth(bitmap, mipmapIndex);

            int pixelCount = width * height * depth;
            if (bitmap.Type == BitmapType.CubeMap)
                pixelCount *= 6;

            return pixelCount;
        }

        public static int GetMipmapPixelDataSize(Image bitmap, int mipmapIndex)
        {
            int pixelCount = GetMipmapPixelCount(bitmap, mipmapIndex);
            int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);
            return pixelCount * bitsPerPixel / 8;
        }

        public static int GetPixelCount(Image bitmap)
        {
            int pixelCount = 0;
            for (int i = 0; i < bitmap.MipmapCount; i++)
                pixelCount += GetMipmapPixelCount(bitmap, i);
            return pixelCount;
        }

        public static int GetBytesPerBlock(BitmapFormat format)
        {
            switch (format)
            {
                case BitmapFormat.Unused5:
                case BitmapFormat.R6G5B5:
                case BitmapFormat.UnusedC:
                case BitmapFormat.UnusedD:
                case BitmapFormat.P8:
                    return 0;
                case BitmapFormat.A8:
                case BitmapFormat.Y8:
                case BitmapFormat.AY8:
                case BitmapFormat.Unused4:
                    return 1;
                case BitmapFormat.A8Y8:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A1R5G5B5:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.A4R4G4B4Font:
                case BitmapFormat.V8U8:
                case BitmapFormat.G8B8:
                    return 2;
                case BitmapFormat.X8R8G8B8:
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.Q8W8V8U8:
                case BitmapFormat.A2R10G10B10:
                case BitmapFormat.V16U16:
                    return 4;
                case BitmapFormat.RGBFP16:
                    return 6;
                case BitmapFormat.Dxt1:
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A8R8G8B8_reach:
                case BitmapFormat.Unused1E:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.Y16:
                case BitmapFormat.Ctx1:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.ReachDxt3aMono:
                case BitmapFormat.ReachDxt3aAlpha:
                case BitmapFormat.ReachDxt5aMono:
                case BitmapFormat.ReachDxt5aAlpha:
                case BitmapFormat.ReachDxnMonoAlpha:
                    return 8;
                case BitmapFormat.RGBFP32:
                    return 12;
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.ARGBFP32:
                case BitmapFormat.A32B32G32R32F:
                case BitmapFormat.Dxn:
                case BitmapFormat.DxnMonoAlpha:
                    return 16;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
