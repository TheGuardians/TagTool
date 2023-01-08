using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps
{
    public static class XboxBitmapUtils
    {
        public static int GetXboxImageSize(XboxBitmap xboxBitmap)
        {
            // add special case for bitmaps not in virtual height/wdith

            int size;
            int dataWidth;
            int dataHeight;
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
            if (xboxBitmap.MipMapCount <= 1)
                return 0;
            else
            {
                var totalSize = 0;
                var mipMapCount = xboxBitmap.MipMapCount - 1;

                var curWidth = xboxBitmap.Width;
                var curHeight = xboxBitmap.Height;

                while (mipMapCount != 0)
                {
                    var nextMipWidth = curWidth / 2;
                    var nextMipHeight = curHeight / 2;
                    totalSize += BitmapUtils.GetSingleMipMapSize(nextMipWidth, nextMipHeight, xboxBitmap.MinimalBitmapSize, xboxBitmap.CompressionFactor);
                    mipMapCount--;
                }

                switch (xboxBitmap.Type)
                {
                    case BitmapType.CubeMap:
                        totalSize *= 6;
                        break;
                    case BitmapType.Texture3D:
                    case BitmapType.Array:
                        totalSize *= xboxBitmap.Depth;
                        break;
                }

                return totalSize;
            }
        }

        public static int GetXboxMipMapSize(Bitmap.Image image)
        {
            return GetXboxMipMapSize(new XboxBitmap(image));
        }


        public static uint GetXboxBitmapD3DTextureType(BitmapTextureInteropDefinition bitmapResource)
        {
            switch (bitmapResource.BitmapType)
            {
                case BitmapType.Texture2D:
                case BitmapType.Array:
                    return 1;
                case BitmapType.Texture3D:
                    return 2;
                case BitmapType.CubeMap:
                    return 3;
                default:
                    throw new NotSupportedException();
            }
        }

        public static uint GetXboxInterleavedBitmapOffset(BitmapTextureInteropDefinition bitmap1, BitmapTextureInteropDefinition bitmap2, int arrayIndex, int level, int currentBitmapIndex, bool hasHighResData = false)
        {
            /*
             * Block size, bits per pixel, tiling and formats are the same, also texture must be square.
             */

            uint offset = 0;

            var format = XboxGraphics.XGGetGpuFormat(bitmap1.D3DFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(format);
            bool isTiled = Direct3D.D3D9x.D3D.IsTiled(bitmap1.D3DFormat);
            XboxGraphics.XGGetBlockDimensions(format, out uint blockWidth, out blockWidth);
            BitmapTextureInteropDefinition currentBitmap = currentBitmapIndex == 0 ? bitmap1 : bitmap2;
            BitmapTextureInteropDefinition otherBitmap = currentBitmapIndex == 0 ? bitmap2 : bitmap1;

            uint dimension = (uint)currentBitmap.Width;
            uint depth = (uint)currentBitmap.Depth;
            uint levelDimension = dimension;
            uint levelDepth = depth;
            uint tileSize = (blockWidth * blockWidth * 32 * 32 * bitsPerPixel) >> 3;

            uint arrayStride = 1;
            if (currentBitmap.BitmapType == BitmapType.CubeMap || currentBitmap.BitmapType == BitmapType.Array)
            {
                int arrayFactor = currentBitmap.BitmapType == BitmapType.Array ? 2 : 0;
                uint actualDepth = (uint)(currentBitmap.BitmapType == BitmapType.CubeMap ? 6 : currentBitmap.Depth);
                arrayStride = Direct3D.D3D9x.D3D.NextMultipleOf(actualDepth, 1u << arrayFactor);
            }

            // assume power of two for now

            bool useInterleavedOffset;

            if (currentBitmap.Width == otherBitmap.Width)
                useInterleavedOffset = currentBitmapIndex != 0;
            else
                useInterleavedOffset = currentBitmap.Width < otherBitmap.Width;

            if (useInterleavedOffset)
            {
                if ((levelDimension >> level) <= 64)
                {
                    offset += tileSize / 2;
                }
            }

            if (bitmap1.Width > bitmap2.Width)
            {
                if (useInterleavedOffset)
                {
                    // compute used data from the other image
                    if (currentBitmap.HighResInSecondaryResource > 0)
                    {
                        if (level == 0)
                            offset += (uint)(arrayStride * otherBitmap.Width * otherBitmap.Width * bitsPerPixel) / 8;
                    }

                    if (currentBitmap.Width >> level <= 64)
                    {
                        // compute first level that can fit mips
                        int lowerBound = currentBitmap.Width > 16 ? 64 : 16;
                        int targetLevel = 0;
                        uint tempWidth = (uint)bitmap1.Width;
                        do
                        {
                            targetLevel++;
                            tempWidth >>= 1;
                            if (tempWidth < 1) tempWidth = 1;
                        }
                        while (tempWidth > lowerBound && targetLevel <= bitmap1.MipmapCount);

                        if (targetLevel > 0)
                            offset += GetXboxBitmapLevelOffset(bitmap1, 0, targetLevel, bitmap1.HighResInSecondaryResource > 0);
                    }
                    else
                    {
                        if (bitmap2.Width >> level <= 64)
                        {
                            // compute first level that can fit mips
                            int targetLevel = 0;
                            uint tempWidth = (uint)bitmap1.Width;
                            do
                            {
                                targetLevel++;
                                tempWidth >>= 1;
                                if (tempWidth < 1) tempWidth = 1;
                            }
                            while (tempWidth > (bitmap2.Width >> level) && targetLevel <= bitmap1.MipmapCount);

                            if (targetLevel > 0)
                                offset += GetXboxBitmapLevelOffset(bitmap1, 0, targetLevel, bitmap1.HighResInSecondaryResource > 0);
                        }
                    }
                }
                else
                {
                    offset = 0;
                }

            }
            else if (bitmap1.Width == bitmap2.Width)
            {
                if (bitmap1.Width > 64 && level == 0 && useInterleavedOffset)
                    offset += arrayStride * tileSize;

                if (bitmap1.Width > 128)
                    throw new Exception("FIX ME");
            }
            else
            {
                if (useInterleavedOffset)
                {
                    // compute used data from the other image
                    if (currentBitmap.HighResInSecondaryResource > 0)
                    {
                        if (level == 0)
                            offset += (uint)(arrayStride * otherBitmap.Width * otherBitmap.Width * bitsPerPixel) / 8;
                    }

                    if (currentBitmap.Width >> level <= 64)
                    {
                        // compute first level that can fit mips
                        int lowerBound = currentBitmap.Width > 16 ? 64 : 16;
                        int targetLevel = 0;
                        uint tempWidth = (uint)bitmap2.Width;
                        do
                        {
                            targetLevel++;
                            tempWidth >>= 1;
                            if (tempWidth < 1) tempWidth = 1;
                        }
                        while (tempWidth > lowerBound && targetLevel <= bitmap2.MipmapCount);

                        if (targetLevel > 0)
                            offset += GetXboxBitmapLevelOffset(bitmap2, 0, targetLevel, bitmap2.HighResInSecondaryResource > 0);
                    }
                    else
                    {
                        if (bitmap1.Width >> level <= 64)
                        {
                            // compute first level that can fit mips
                            int targetLevel = 0;
                            uint tempWidth = (uint)bitmap2.Width;
                            do
                            {
                                targetLevel++;
                                tempWidth >>= 1;
                                if (tempWidth < 1) tempWidth = 1;
                            }
                            while (tempWidth > (bitmap1.Width >> level) && targetLevel <= bitmap2.MipmapCount);

                            if (targetLevel > 0)
                                offset += GetXboxBitmapLevelOffset(bitmap2, 0, targetLevel, bitmap2.HighResInSecondaryResource > 0);
                        }
                    }
                }
                else
                {
                    offset = 0;
                }
            }

            offset += GetXboxBitmapLevelOffset(currentBitmap, arrayIndex, level, hasHighResData);

            return offset;
        }

        public static uint GetXboxBitmapLevelOffset(BitmapTextureInteropDefinition bitmapResource, int ArrayIndex, int Level, bool hasHighResData = false)
        {
            uint blockWidth, blockHeight;
            uint layerSize;
            uint offset = 0;
            uint levelSizeBytes;

            uint unknownType = GetXboxBitmapD3DTextureType(bitmapResource);
            var format = XboxGraphics.XGGetGpuFormat(bitmapResource.D3DFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(format);
            bool isTiled = Direct3D.D3D9x.D3D.IsTiled(bitmapResource.D3DFormat);
            XboxGraphics.XGGetBlockDimensions(format, out blockWidth, out blockHeight);

            int levels = bitmapResource.MipmapCount == 0 ? Direct3D.D3D9x.D3D.GetMaxMipLevels(bitmapResource.Width, bitmapResource.Height, bitmapResource.Depth, 0) : bitmapResource.MipmapCount;

            bool isPacked = levels > 1;

            uint width = (uint)bitmapResource.Width;
            uint height = (uint)bitmapResource.Height;
            uint depth = (uint)bitmapResource.Depth;

            uint levelWidth = width;
            uint levelHeight = height;
            uint levelDepth = depth;

            if (Level > 0 || (isPacked && (levelWidth <= 16 || levelHeight <= 16)))
            {
                uint arrayStride = 1;
                if (bitmapResource.BitmapType == BitmapType.CubeMap || bitmapResource.BitmapType == BitmapType.Array)
                {
                    int arrayFactor = bitmapResource.BitmapType == BitmapType.Array ? 2 : 0;
                    uint actualDepth = (uint)(bitmapResource.BitmapType == BitmapType.CubeMap ? 6 : bitmapResource.Depth);
                    arrayStride = Direct3D.D3D9x.D3D.NextMultipleOf(actualDepth, 1u << arrayFactor);
                }

                uint alignedWidth = 0;
                uint alignedHeight = 0;
                uint alignedDepth = 0;

                for (int i = 0; i < Level; i++)
                {
                    levelWidth = width >> i;
                    levelHeight = height >> i;

                    if (levelWidth < 1) levelWidth = 1;
                    if (levelHeight < 1) levelHeight = 1;

                    alignedDepth = levelDepth;
                    alignedWidth = levelWidth;
                    alignedHeight = levelHeight;

                    Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, format, unknownType, isTiled);

                    // if not first mip level, align to next power of two
                    if (i > 0)
                    {
                        if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedWidth))
                        {
                            alignedWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedWidth);
                            if (alignedWidth < 0) alignedWidth = 0;
                            alignedWidth = 1u << (int)alignedWidth;
                        }

                        if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedHeight))
                        {
                            alignedHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedHeight);
                            if (alignedHeight < 0) alignedHeight = 0;
                            alignedHeight = 1u << (int)alignedHeight;
                        }
                    }

                    layerSize = (bitsPerPixel * alignedWidth * alignedHeight) / 8;

                    // if the bitmap uses the high resolution buffer, the first level is stored there so no need to add it to the running offset
                    if (hasHighResData && i == 0)
                    {
                        if (Level == 1)
                        {
                            levelWidth = width >> 1;
                            levelHeight = height >> 1;

                            if (levelWidth < 1) levelWidth = 1;
                            if (levelHeight < 1) levelHeight = 1;

                            alignedDepth = levelDepth;
                            alignedWidth = levelWidth;
                            alignedHeight = levelHeight;

                            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, format, unknownType, isTiled);
                        }
                        continue;
                    }


                    if ((levelWidth <= 16 || levelHeight <= 16) && isPacked)
                        break;
                    else
                    {
                        if (unknownType == 2)
                            levelSizeBytes = Direct3D.D3D9x.D3D.NextMultipleOf(alignedDepth * layerSize, 0x1000);
                        else
                            levelSizeBytes = alignedDepth * Direct3D.D3D9x.D3D.NextMultipleOf(layerSize, 0x1000);

                        offset += arrayStride * levelSizeBytes;
                    }
                }
                // when array index is > 0, we need to add an offset into the right array layer, outside of the loop since the loop computes the offset for all layers
                if (ArrayIndex > 0)
                {
                    alignedDepth = depth;
                    alignedWidth = width >> Level;
                    alignedHeight = height >> Level;

                    if (alignedWidth < 1) alignedWidth = 1;
                    if (alignedHeight < 1) alignedHeight = 1;

                    Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, format, unknownType, isTiled);

                    // if not first mip level, align to next power of two
                    if (Level > 0)
                    {
                        if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedWidth))
                        {
                            alignedWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedWidth);
                            if (alignedWidth < 0) alignedWidth = 0;
                            alignedWidth = 1u << (int)alignedWidth;
                        }

                        if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedHeight))
                        {
                            alignedHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedHeight);
                            if (alignedHeight < 0) alignedHeight = 0;
                            alignedHeight = 1u << (int)alignedHeight;
                        }
                    }

                    uint size = alignedWidth * alignedHeight * bitsPerPixel / 8;
                    uint nextLevelSize = Direct3D.D3D9x.D3D.NextMultipleOf(size, 0x1000);
                    offset += (uint)(ArrayIndex * nextLevelSize);
                }
            }
            else
            {
                levelWidth = width;
                levelHeight = height;
                levelDepth = depth;

                Direct3D.D3D9x.D3D.AlignTextureDimensions(
                    ref levelWidth, ref levelHeight, ref levelDepth, bitsPerPixel, format, unknownType, isTiled);

                if (!isTiled
                  && !(bitmapResource.BitmapType == BitmapType.Array)
                  && !(isPacked)
                  && unknownType == 1
                  && bitmapResource.MipmapCount < 1)
                {

                    levelHeight = Direct3D.D3D9x.D3D.NextMultipleOf(height, blockHeight);
                }

                layerSize = (bitsPerPixel * levelWidth * levelHeight) >> 3;

                levelSizeBytes = Direct3D.D3D9x.D3D.NextMultipleOf(layerSize, 0x1000);
                offset += levelSizeBytes * (uint)ArrayIndex;
            }

            return offset;
        }

        public static byte[] GetXboxBitmapLevelData(byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, int level, int layerIndex, bool isPaired, int pairIndex, BitmapTextureInteropDefinition otherDefinition)
        {
            byte[] data;
            uint levelOffset;

            var d3dFormat = definition.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            uint blockWidth;
            uint blockHeight;

            uint alignedWidth = (uint)definition.Width >> level;
            uint alignedHeight = (uint)definition.Height >> level;

            if (alignedWidth < 1) alignedWidth = 1;
            if (alignedHeight < 1) alignedHeight = 1;

            uint alignedDepth = definition.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);

            XboxGraphics.XGGetBlockDimensions(gpuFormat, out blockWidth, out blockHeight);
            XboxGraphics.XGPOINT point = new XboxGraphics.XGPOINT();
            if (definition.MipmapCount > 1)
                XboxGraphics.GetMipTailLevelOffsetCoords((uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)level, gpuFormat, false, false, point);

            var textureType = GetXboxBitmapD3DTextureType(definition);
            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, textureType, isTiled);

            if (level > 0)
            {
                // align to next power of two
                if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedWidth))
                {
                    alignedWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedWidth);
                    if (alignedWidth < 0) alignedWidth = 0;
                    alignedWidth = 1u << (int)alignedWidth;
                }

                if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedHeight))
                {
                    alignedHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedHeight);
                    if (alignedHeight < 0) alignedHeight = 0;
                    alignedHeight = 1u << (int)alignedHeight;
                }
            }

            // hacks when the point is outside of the first aligned texture, compute how many tiles you need and extract them (non-square only)
            if (point.X >= 32)
                alignedWidth *= (uint)(1 + point.X / 32);
            if (point.Y >= 32)
                alignedHeight *= (uint)(1 + point.Y / 32);

            uint texelPitch = blockWidth * blockHeight * bitsPerPixel / 8;
            uint size = alignedWidth * alignedHeight * bitsPerPixel / 8;

            // documentation says that each packed mip level should be aligned to 4KB, required to make untiling work smoothly
            size = (uint)((size + 0xFFF) & ~0xFFF);

            int tileOffset = 0;

            if (!isPaired)
            {
                bool useHighResBuffer = definition.HighResInSecondaryResource > 0;
                if ((level == 0 && useHighResBuffer) || primaryData == null)
                {
                    levelOffset = GetXboxBitmapLevelOffset(definition, layerIndex, level);
                    uint alignedSecondaryLength = (uint)((secondaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedSecondaryLength];
                    Array.Copy(secondaryData, 0, data, 0, secondaryData.Length);
                }
                else
                {
                    levelOffset = GetXboxBitmapLevelOffset(definition, layerIndex, level, useHighResBuffer);
                    uint alignedPrimaryLength = (uint)((primaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedPrimaryLength];
                    Array.Copy(primaryData, 0, data, 0, primaryData.Length);
                }
            }
            else
            {
                bool useHighResBuffer = definition.HighResInSecondaryResource > 0;
                var bitmap1 = pairIndex == 0 ? definition : otherDefinition;
                var bitmap2 = pairIndex == 0 ? otherDefinition : definition;

                if (level == 0 && useHighResBuffer)
                {
                    levelOffset = GetXboxInterleavedBitmapOffset(bitmap1, bitmap2, layerIndex, level, pairIndex);
                    uint alignedSecondaryLength = (uint)((secondaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedSecondaryLength];
                    Array.Copy(secondaryData, 0, data, 0, secondaryData.Length);
                }
                else
                {
                    levelOffset = GetXboxInterleavedBitmapOffset(bitmap1, bitmap2, layerIndex, level, pairIndex, useHighResBuffer);
                    uint alignedPrimaryLength = (uint)((primaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedPrimaryLength];
                    Array.Copy(primaryData, 0, data, 0, primaryData.Length);
                }
            }

            tileOffset += (int)levelOffset;

            byte[] tempResult = new byte[size];

            // check if data has enough memory for the requested, size, sometimes it does not (truncated to save memory)
            uint copySize = size;
            if (size + tileOffset >= data.Length)
                copySize = (uint)(data.Length - tileOffset);

            Array.Copy(data, tileOffset, tempResult, 0, copySize);
            data = tempResult;

            uint nBlockWidth;
            uint nBlockHeight;
            if (isTiled)
            {
                //
                // Untile texture
                //

                byte[] result = new byte[size];

                nBlockWidth = alignedWidth / blockWidth;
                nBlockHeight = alignedHeight / blockHeight;
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        int destinationIndex = (int)(i * nBlockWidth + j);  // offset in terms block
                        int destinationOffset = (int)(destinationIndex * texelPitch);
                        uint tiledIndex = XboxGraphics.XGAddress2DTiledOffset((uint)j, (uint)i, nBlockWidth, texelPitch); // returns offset in terms of block
                        uint tiledOffset = tiledIndex * texelPitch;
                        Array.Copy(data, tiledOffset, result, destinationOffset, texelPitch);
                    }
                }
                data = result;
            }

            // find level size aligned to block size

            int levelWidth = definition.Width >> level;
            int levelHeight = definition.Height >> level;

            if (levelWidth < 1)
                levelWidth = 1;
            if (levelHeight < 1)
                levelHeight = 1;

            if (levelWidth % blockWidth != 0)
                levelWidth = (int)(levelWidth + blockWidth - levelWidth % blockWidth);

            if (levelHeight % blockHeight != 0)
                levelHeight = (int)(levelHeight + blockHeight - levelHeight % blockHeight);

            byte[] finalData = new byte[levelWidth * levelHeight * bitsPerPixel >> 3];

            nBlockWidth = (uint)(levelWidth / blockWidth);
            nBlockHeight = (uint)(levelHeight / blockHeight);

            uint sliceBlockWidth = alignedWidth / blockWidth;

            // skip these loops if the bitmap is already the proper format
            if (point.X != 0 || point.Y != 0 || finalData.Length != data.Length)
            {
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        uint offset = (uint)(((i + point.Y) * sliceBlockWidth) + j + point.X) * texelPitch;
                        uint destOffset = (uint)((i * nBlockWidth) + j) * texelPitch;
                        Array.Copy(data, offset, finalData, destOffset, texelPitch);
                    }
                }
            }
            else
            {
                Array.Copy(data, 0, finalData, 0, data.Length);
            }

            XboxGraphics.XGEndianSwapSurface(d3dFormat, finalData);
            return finalData;
        }
    }
}
