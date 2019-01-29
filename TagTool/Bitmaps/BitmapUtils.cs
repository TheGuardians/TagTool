using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags.Definitions;

namespace TagTool.Bitmaps
{
    public static class BitmapUtils
    {
        /// <summary>
        /// Get the virtual size of an Xbox 360 bitmap.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="minimalSize"></param>
        /// <returns></returns>
        public static int GetVirtualSize(int size, int minimalSize)
        {
            return (size % minimalSize == 0) ? size : size + (minimalSize - (size % minimalSize));
        }

        public static int GetXboxImageSize(XboxBitmap xboxBitmap)
        {
            // add special case for bitmaps not in virtual height/wdith

            int size = 0;
            int dataWidth = 0;
            int dataHeight = 0;
            if (xboxBitmap.NotExact)
            {
                dataWidth = xboxBitmap.VirtualWidth;
                dataHeight = xboxBitmap.VirtualHeight;
            }
            else if (!xboxBitmap.MultipleOfBlockDimension)
            {
                dataWidth = xboxBitmap.NearestWidth;
                dataHeight = xboxBitmap.NearestHeight;
            }
            else
            {
                dataWidth = xboxBitmap.Width;
                dataHeight = xboxBitmap.Height;
            }

            size = (int)(dataWidth * dataHeight / xboxBitmap.CompressionFactor);

            switch (xboxBitmap.Type)
            {
                case BitmapType.CubeMap:
                    size *= 6;
                    break;
                case BitmapType.Texture3D:
                case BitmapType.Array:
                    size *= xboxBitmap.Depth;
                    break;
            }
            return size;
        }

        public static int GetXboxImageSize(Bitmap.Image image)
        {
            return GetXboxImageSize(new XboxBitmap(image));
        }

        public static int GetImageSize(BaseBitmap bitmap)
        {
            return (int)(bitmap.NearestHeight * bitmap.NearestWidth / bitmap.CompressionFactor);
        }

        public static int GetXboxMipMapSize(XboxBitmap xboxBitmap)
        {
            var minDim = xboxBitmap.MinimalBitmapSize;
            var largestSide = Math.Max(xboxBitmap.Width, xboxBitmap.Height);
            if(largestSide / 2 <= minDim / 2)
            {
                return (int)(minDim * minDim / xboxBitmap.CompressionFactor);
            }

            return -1;
        }

        public static int GetXboxMipMapSize(Bitmap.Image image)
        {
            return GetXboxMipMapSize(new XboxBitmap(image));
        }

        public static int GetMipMapSize(BaseBitmap blamBitmap)
        {
            int pixelCount = 0;
            var mipMapCount = blamBitmap.MipMapCount;

            if (blamBitmap.MipMapCount > 0)
            {
                int previousHeight = blamBitmap.Height;
                int previousWidth = blamBitmap.Width;

                for (int i = 0; i < mipMapCount; i++)
                {
                    var mipMapHeight = previousHeight / 2;
                    var mipMapWidth = previousWidth / 2;

                    previousHeight /= 2;
                    previousWidth /= 2;

                    pixelCount += mipMapHeight * mipMapWidth;
                }
            }
            return (int)(pixelCount / blamBitmap.CompressionFactor);

        }

        public static int GetInterleavedOffset(int interleavedIndex, XboxBitmap bitmap)
        {
            if(interleavedIndex == 0)
                return 0;
            else{
                int offset = 0;

                return offset;
            }
        }

    }
}
