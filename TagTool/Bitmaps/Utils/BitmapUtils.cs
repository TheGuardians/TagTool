using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Direct3D.D3D9;
using TagTool.Direct3D.Xbox360;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

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
                    totalSize += GetSingleMipMapSize(nextMipWidth, nextMipHeight, xboxBitmap.MinimalBitmapSize, xboxBitmap.CompressionFactor);
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

        public static int NextNearestSize(int curSize, int minSize)
        {
            return minSize * ((curSize/2 + (minSize - 1)) / minSize);
        }
        
        public static int RoundSize(int size, int blockDimension)
        {
            return blockDimension * ((size + (blockDimension - 1)) / blockDimension);
        }

        public static int GetSingleMipMapSize(int width, int height, int minimalSize, double compressionFactor)
        {
            int virtualWidth = GetVirtualSize(width, minimalSize);
            int virtualHeight = GetVirtualSize(height, minimalSize);
            int size = (int)(virtualHeight * virtualWidth / compressionFactor); ;
            return size;
        }

        public static bool IsPowerOfTwo(int size)
        {
            return (size != 0) && ((size & (size - 1)) == 0);
        }

        public static BitmapTextureInteropDefinition CreateBitmapTextureInteropDefinition(BaseBitmap bitmap)
        {
            var result = new BitmapTextureInteropDefinition
            {
                Width = (short)bitmap.Width,
                Height = (short)bitmap.Height,
                Depth = (byte)bitmap.Depth,
                MipmapCount = (byte)(bitmap.MipMapCount + 1),
                HighResInSecondaryResource = 0,
            };

            result.BitmapType = bitmap.Type;
            result.Format = bitmap.Format;

            switch (result.Format)
            {
                case BitmapFormat.Dxt1:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT1;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxt3:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT3;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxt5:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT5;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxn:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_ATI2;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                default:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_UNKNOWN;
                    break;
            }

            result.Curve = bitmap.Curve;

            if (IsPowerOfTwo(bitmap.Width) && IsPowerOfTwo(bitmap.Height))
                result.Flags |= BitmapFlags.PowerOfTwoDimensions;

            return result;
        }

        public static BitmapTextureInteropDefinition CreateBitmapTextureInteropDefinition(DDSHeader header)
        {
            var result = new BitmapTextureInteropDefinition
            {
                Width = (short)header.Width,
                Height = (short)header.Height,
                Depth = (byte)header.Depth,
                MipmapCount = (byte)header.MipMapCount,
                HighResInSecondaryResource = 0,
            };

            result.BitmapType = BitmapDdsFormatDetection.DetectType(header);
            result.Format = BitmapDdsFormatDetection.DetectFormat(header);

            switch (result.Format)
            {
                case BitmapFormat.Dxt1:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT1;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxt3:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT3;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxt5:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_DXT5;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                case BitmapFormat.Dxn:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_ATI2;
                    result.Flags |= BitmapFlags.Compressed;
                    break;
                default:
                    result.D3DFormat = (int)D3DFormat.D3DFMT_UNKNOWN;
                    break;
            }

            result.Curve = BitmapImageCurve.xRGB; // find a way to properly determine that

            if (header.PixelFormat.Flags.HasFlag(DDSPixelFormatFlags.Compressed))
                result.Flags |= BitmapFlags.Compressed;

            if (IsPowerOfTwo(header.Width) && IsPowerOfTwo(header.Height))
                result.Flags |= BitmapFlags.PowerOfTwoDimensions;

            return result;
        }

        public static Bitmap.Image CreateBitmapImageFromResourceDefinition(BitmapTextureInteropDefinition definition)
        {
            var result = new Bitmap.Image()
            {
                Signature = "mtib",
                Width = definition.Width,
                Height = definition.Height,
                Depth = (sbyte)definition.Depth,
                Format = definition.Format,
                Type = definition.BitmapType,
                MipmapCount = (sbyte)(definition.MipmapCount != 0 ? definition.MipmapCount - 1 : 0),
                Flags = definition.Flags,
                Curve = definition.Curve
            };
            return result;
        }

        public static BitmapTextureInteropResource CreateEmptyBitmapTextureInteropResource()
        {
            return new BitmapTextureInteropResource
            {
                Texture = new D3DStructure<BitmapTextureInteropResource.BitmapDefinition>
                {
                    Definition = new BitmapTextureInteropResource.BitmapDefinition
                    {
                        PrimaryResourceData = new TagData(),
                        SecondaryResourceData = new TagData(),
                        Bitmap = new BitmapTextureInteropDefinition()
                    },
                    AddressType = CacheAddressType.Definition
                }
            };
        }

        public static BitmapTextureInteropResource CreateBitmapTextureInteropResource(BaseBitmap bitmap)
        {
            var result = CreateEmptyBitmapTextureInteropResource();
            result.Texture.Definition.PrimaryResourceData.Data = bitmap.Data;
            result.Texture.Definition.Bitmap = CreateBitmapTextureInteropDefinition(bitmap);
            return result;
        }

        public static void SetBitmapImageData(BaseBitmap bitmap, Bitmap.Image image)
        {
            image.Width = (short)bitmap.Width;
            image.Height = (short)bitmap.Height;
            image.Depth = (sbyte)bitmap.Depth;
            image.Format = bitmap.Format;
            image.Type = bitmap.Type;
            image.MipmapCount = (sbyte)bitmap.MipMapCount;
            image.DataSize = bitmap.Data.Length;
            image.XboxFlags = BitmapFlagsXbox.None;
            image.Flags = bitmap.Flags;
            image.Curve = bitmap.Curve;

            if (image.Format == BitmapFormat.Dxn)
                image.Flags |= BitmapFlags.Unknown3;

        }

        public static RealArgbColor DecodeBitmapPixelXbox(byte[] bitmapData, Bitmap.Image image, D3DTexture9 pTexture, int layerIndex, int mipmapIndex)
        {
            RealArgbColor result = new RealArgbColor(1,0,0,0);

            if( image.Type != BitmapType.Texture3D && (
                (image.Type == BitmapType.Texture2D && layerIndex == 1) || 
                (image.Type == BitmapType.CubeMap && layerIndex >= 0 && layerIndex < 6)  ||
                (image.Type == BitmapType.Array && layerIndex >= 0 && layerIndex < image.Depth)) && 
                (pTexture.GetBaseOffset() > 0 || pTexture.GetMipOffset() > 0))
            {
                uint blockWidth = 0;
                uint blockHeight = 0;
                int dataOffset = 0;
                int layerOffset = 0;

                // verify the mipmap level index 
                var minMipLevel = pTexture.GetMinMipLevel();
                var maxMipLevel = pTexture.GetMaxMipLevel();

                // if mipmapIndex is too low
                if (mipmapIndex < minMipLevel)
                    mipmapIndex = minMipLevel;
                // if mipmapIndex is too high
                if (mipmapIndex > maxMipLevel)
                    mipmapIndex = maxMipLevel;

                var currentHeight = image.Height >> mipmapIndex;
                var currentWidth = image.Width >> mipmapIndex;

                if (currentWidth < 1)
                    currentWidth = 1;

                if (currentHeight < 1)
                    currentHeight = 1;


                //XGGetTextureDesc
                XboxGraphics.XGTEXTURE_DESC textureDesc = new XboxGraphics.XGTEXTURE_DESC();

                
                XboxGraphics.XGGetBlockDimensions(XboxGraphics.XGGetGpuFormat(textureDesc.D3DFormat), out blockWidth, out blockHeight);

                layerOffset = 0; // Unknown XG function
                

                if (mipmapIndex > 0 && pTexture.GetMipOffset() > 0)
                    dataOffset = pTexture.GetMipOffset() + layerOffset;
                else
                    dataOffset = pTexture.GetBaseOffset() + layerOffset;

                for(int h = 0; h < currentHeight; h++)
                {
                    for(int w = 0; w < currentWidth; w++)
                    {

                    }
                }
            }


            return result;
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

        private static uint GetInterleavedPackedOffset(BitmapTextureInteropDefinition bitmap, bool hasHighResData = false)
        {
            uint layerSize;
            uint offset = 0;
            uint levelSizeBytes;
            uint alignedDimension;
            uint alignedDepth;
            uint levelDimension;

            uint unknownType = GetXboxBitmapD3DTextureType(bitmap);
            var format = XboxGraphics.XGGetGpuFormat(bitmap.D3DFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(format);
            bool isTiled = Direct3D.D3D9x.D3D.IsTiled(bitmap.D3DFormat);
            XboxGraphics.XGGetBlockDimensions(format, out uint blockWidth, out blockWidth);

            uint dimension = (uint)bitmap.Width;
            uint depth = bitmap.Depth;
            
            uint levelDepth = depth;

            uint arrayStride = 1;
            if (bitmap.BitmapType == BitmapType.CubeMap || bitmap.BitmapType == BitmapType.Array)
            {
                int arrayFactor = bitmap.BitmapType == BitmapType.Array ? 2 : 0;
                uint actualDepth = (uint)(bitmap.BitmapType == BitmapType.CubeMap ? 6 : bitmap.Depth);
                arrayStride = Direct3D.D3D9x.D3D.NextMultipleOf(actualDepth, 1u << arrayFactor);
            }

            for (int i = 0; i < bitmap.MipmapCount; i++)
            {
                levelDimension = dimension >> i;

                if (levelDimension < 1) levelDimension = 1;
                alignedDimension = levelDimension;
                alignedDepth = levelDepth;

                Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedDimension, ref alignedDimension, ref alignedDepth, bitsPerPixel, format, unknownType, isTiled);

                if (i > 0)
                {
                    if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedDimension))
                    {
                        alignedDimension = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedDimension);
                        if (alignedDimension < 0) alignedDimension = 0;
                        alignedDimension = 1u << (int)alignedDimension;
                    }
                }

                layerSize = (bitsPerPixel * alignedDimension * alignedDimension) / 8;

                if (hasHighResData && i == 0)
                    continue;

                if (levelDimension <= 8 * blockWidth)   // if less than or equal to 64x64, the levels are now mixed together
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
            return offset;
        }

        public static uint GetXboxInterleavedBitmapOffset(BitmapTextureInteropDefinition bitmap1, BitmapTextureInteropDefinition bitmap2, int arrayIndex, int level, int currentBitmapIndex, bool hasHighResData = false)
        {
            /*
             * Block size, bits per pixel, tiling and formats are the same, also texture must be square.
             */

            uint offset = 0;

            uint unknownType = GetXboxBitmapD3DTextureType(bitmap1);
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

            // assume power of two for now

            uint currentWidth = (uint)currentBitmap.Width;
            uint otherWidth = (uint)otherBitmap.Width;
            bool useInterleavedOffset;
            if (currentBitmap.Width == otherBitmap.Width)
                useInterleavedOffset = currentBitmapIndex == 0;
            else
                useInterleavedOffset = currentBitmap.Width < otherBitmap.Width;
            if (useInterleavedOffset)
            {
                if (levelDimension <= 16 * blockWidth)
                {
                    offset += tileSize / 2;
                }
            }

            //
            // Compute the offset from the start of the array to the desired level OR the first packed level
            //
            /*
            if (currentWidth1 == currentWidth2)
            {
                if (currentWidth1 <= 64)
                {
                    // both are 64x64, packed together, no extra offset required
                    offset = 0;
                }
                else
                {
                    // must be 128x128 (up to a a power of two align), specs of interleaved bitmaps
                    if (useInterleavedOffset)
                        offset += tileSize;
                }
            }
            else */
            if (otherWidth >= currentWidth)
            {
                // find level to get currentWidth1 -> currentWidth2
                int targetLevel = 0;
                uint tempWidth = otherWidth;
                do
                {
                    targetLevel++;
                    tempWidth >>= 1;
                    if (tempWidth < 1) tempWidth = 1;
                }
                while (tempWidth != currentWidth && targetLevel <= otherBitmap.MipmapCount);

                if (targetLevel > 0)
                    offset += GetXboxBitmapLevelOffset(otherBitmap, 0, targetLevel, otherBitmap.HighResInSecondaryResource > 0);
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
                    if(i > 0)
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
                        continue;

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
                    // temp  hack if the loop above doesn't run
                    if (alignedHeight == 0 && alignedWidth == 0 && alignedDepth == 0)
                    {
                        alignedDepth = depth;
                        alignedWidth = width;
                        alignedHeight = height;
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

        private static uint AlignToPage(uint offset)
        {
            return offset + 0xFFFu & ~0xFFFu;
        }

        /// <summary>
        /// When converting xbox bitmap formats (and other rare formats), get the standard format that it can be converted it without loss
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static BitmapFormat GetEquivalentBitmapFormat(BitmapFormat format)
        {
            switch (format)
            {
                case BitmapFormat.Ctx1:
                    return BitmapFormat.Dxn;

                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.AY8:
                    return BitmapFormat.A8Y8;

                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt3aAlpha:
                    return BitmapFormat.A8;

                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                    return BitmapFormat.Y8;

                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                    return BitmapFormat.A8R8G8B8;

                default:
                    return format;
            }
        }

        public static byte[] ConvertXboxFormats(byte[] data, uint width, uint height, BitmapFormat format)
        {
            BitmapFormat destinationFormat = GetEquivalentBitmapFormat(format);

            if(format == BitmapFormat.Dxn)
            {
                // flip x and y channels
                data = BitmapDecoder.SwapXYDxn(data, (int)width, (int)height);
            }


            if (destinationFormat == format)
                return data;

            if(format == BitmapFormat.Ctx1)
            {
                data = BitmapDecoder.Ctx1ToDxn(data, (int)width, (int)height);
            }
            else
            {
                //
                // BitmapDecoder needs to be updated to use the aligned height and width. look at DXN mono alpha or CTX1 to DXN for example
                //
                byte[] uncompressedData = BitmapDecoder.DecodeBitmap(data, format, (int)width, (int)height);

                uncompressedData = TrimAlignedBitmap(format, destinationFormat, (int)width, (int)height, uncompressedData);

                data = BitmapDecoder.EncodeBitmap(uncompressedData, destinationFormat, (int)width, (int)height);
            }
            return data;
        }

        private static byte[] TrimAlignedBitmap(BitmapFormat originalFormat, BitmapFormat destinationFormat, int width, int height, byte[] data)
        {
            byte[] result = new byte[width * height * 4];
            uint blockSize;
            switch (originalFormat)
            {
                case BitmapFormat.Ctx1:
                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                    blockSize = 4;
                    break;
                case BitmapFormat.AY8:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                default:
                    blockSize = 1;
                    break;
            }

            if (blockSize == 1)
                return data;

            uint alignedWidth = Direct3D.D3D9x.D3D.NextMultipleOf((uint)width, blockSize);
            uint alignedHeight = Direct3D.D3D9x.D3D.NextMultipleOf((uint)height, blockSize);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    uint offset = (uint)((j* alignedWidth) + i) * 4;
                    uint destOffset = (uint)((j * width) + i) * 4;
                    Array.Copy(data, offset, result, destOffset, 4);
                }
            }

            return result;
        }

        public static bool IsCompressedFormat(BitmapFormat format)
        {
            switch (format)
            {
                case BitmapFormat.A8:
                case BitmapFormat.Y8:
                case BitmapFormat.AY8:
                case BitmapFormat.A8Y8:
                case BitmapFormat.Unused4:
                case BitmapFormat.Unused5:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.R6G5B5:
                case BitmapFormat.A1R5G5B5:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.X8R8G8B8:
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.UnusedC:
                case BitmapFormat.UnusedD:
                case BitmapFormat.A4R4G4B4Font:
                case BitmapFormat.P8:
                case BitmapFormat.ARGBFP32:
                case BitmapFormat.RGBFP32:
                case BitmapFormat.RGBFP16:
                case BitmapFormat.V8U8:
                case BitmapFormat.G8B8:
                case BitmapFormat.A32B32G32R32F:
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.Q8W8V8U8:
                case BitmapFormat.A2R10G10B10:
                case BitmapFormat.A16B16G16R16:
                case BitmapFormat.V16U16:
                    return false;
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Unused1E:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.Unused20:
                case BitmapFormat.Dxn:
                case BitmapFormat.Ctx1:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.ReachDxt3aMono:
                case BitmapFormat.ReachDxt3aAlpha:
                case BitmapFormat.ReachDxt5aMono:
                case BitmapFormat.ReachDxt5aAlpha:
                case BitmapFormat.ReachDxnMonoAlpha:
                    return true;
            }
            return false;
        }

        public static void FixBitmapFlags(Bitmap.Image image)
        {

            image.Flags &= ~BitmapFlags.Compressed;
        }
    }
}
