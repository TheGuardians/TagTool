using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                MipmapCount = (sbyte)Math.Max(1, bitmap.MipMapCount),
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

        public static BitmapTextureInteropDefinition CreateBitmapTextureInteropDefinition(DDSHeader header, BitmapImageCurve bitmapCurve)
        {
            var result = new BitmapTextureInteropDefinition
            {
                Width = (short)header.Width,
                Height = (short)header.Height,
                Depth = (byte)Math.Max(1, header.Depth),
                MipmapCount = (sbyte)Math.Max(1, header.MipMapCount),
                HighResInSecondaryResource = 0,
            };

            result.BitmapType = BitmapDdsFormatDetection.DetectType(header);
            result.Format = BitmapDdsFormatDetection.DetectFormat(header);
            result.D3DFormat = (int)header.PixelFormat.FourCC;

            result.Curve = bitmapCurve;

            if (IsCompressedFormat(result.Format))
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
                MipmapCount = (sbyte)Math.Max(0, definition.MipmapCount - 1),
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
            image.PixelDataSize = bitmap.Data.Length;
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
                    return Commands.Porting.PortingOptions.Current.HqNormalMapConversion ? 
                        BitmapFormat.Dxn : BitmapFormat.Dxt1;

                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.ReachDxnMonoAlpha:
                case BitmapFormat.AY8:
                    return BitmapFormat.A8Y8;

                case BitmapFormat.Dxt5a:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.ReachDxt3aAlpha:
                case BitmapFormat.ReachDxt5aAlpha:
                    return BitmapFormat.A8;

                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.ReachDxt3aMono:
                case BitmapFormat.ReachDxt5aMono:
                case BitmapFormat.Y16:
                    return BitmapFormat.Y8;
            
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.V8U8:
                case BitmapFormat.A8R8G8B8_reach:
                    return BitmapFormat.A8R8G8B8;

                default:
                    return format;
            }
        }
        public static bool RequiresDecompression(BitmapFormat format, uint width, uint height) => IsCompressedFormat(format) && (width % 4 != 0 || height % 4 != 0);

        public static byte[] ConvertXboxFormats(byte[] data, uint width, uint height, BitmapFormat format, BitmapType type, bool requireDecompression, CacheVersion version)
        {
            // TODO: clean this up


            // fix enum from reach
            if (version >= CacheVersion.HaloReach) 
            {
                if (format >= (BitmapFormat)36)
                    format -= 5;
            }

            BitmapFormat destinationFormat = GetEquivalentBitmapFormat(format);
            
            if (format == BitmapFormat.Dxn)
            {
                // flip x and y channels
                data = BitmapDecoder.SwapXYDxn(data, (int)width, (int)height);
            }

            if (destinationFormat == format && !requireDecompression)
                return data;


            

            /*if(format == BitmapFormat.Ctx1)
            {
                if (type == BitmapType.Array) //DXN array unsupported
                {
                    destinationFormat = BitmapFormat.A8R8G8B8;
                    requireDecompression = false;

                    data = ConvertNonMultipleBlockSizeBitmap(data, width, height, format);
                    data = BitmapDecoder.EncodeBitmap(data, destinationFormat, (int)width, (int)height);
                    format = destinationFormat;
                }
                else
                {
                    data = BitmapDecoder.Ctx1ToDxn(data, (int)width, (int)height);
                    format = BitmapFormat.Dxn;
                }
            }*/
            if (format == BitmapFormat.Ctx1 && type == BitmapType.Array) //DXN array unsupported
            {
                destinationFormat = BitmapFormat.A8R8G8B8;
                requireDecompression = false;

                data = ConvertNonMultipleBlockSizeBitmap(data, width, height, format);
                data = BitmapDecoder.EncodeBitmap(data, destinationFormat, (int)width, (int)height);
                format = destinationFormat;
            }
            else if(format != destinationFormat)
            {
                int blockDimension = BitmapFormatUtils.GetBlockDimension(format);
                int alignedWidth = BitmapUtils.RoundSize((int)width, blockDimension);
                int alignedHeight = BitmapUtils.RoundSize((int)height, blockDimension);


                byte[] uncompressedData;
                if (format == BitmapFormat.Ctx1 && (width % 4 != 0 || height % 4 != 0))
                {
                    uncompressedData = BitmapDecoder.DecodeBitmap(data, format, alignedWidth, alignedHeight);
                }
                else
                {
                    uncompressedData = BitmapDecoder.DecodeBitmap(data, format, alignedWidth, alignedHeight);
                    uncompressedData = TrimAlignedBitmap(format, destinationFormat, (int)width, (int)height, uncompressedData);
                }

                data = BitmapDecoder.EncodeBitmap(uncompressedData, destinationFormat, (int)width, (int)height);
                format = destinationFormat;
            }

            if (requireDecompression)
            {
                data = ConvertNonMultipleBlockSizeBitmap(data, width, height, format);
            }
                

            return data;
        }

        public static byte[] ConvertNonMultipleBlockSizeBitmap(byte[] data, uint width, uint height, BitmapFormat format)
        {
            // assume block size 4 because that's all we deal with in tag data

            if (!IsCompressedFormat(format))
                return data;

            uint alignedWidth = width % 4 != 0 ? width + 4 - width % 4 : width;
            uint alignedHeight = height % 4 != 0 ? height + 4 - height % 4 : height;

            byte[] uncompressedData = BitmapDecoder.DecodeBitmap(data, format, (int)alignedWidth, (int)alignedHeight);
            return TrimAlignedBitmap(format, BitmapFormat.A8R8G8B8, (int)width, (int)height, uncompressedData);
        }

        public static byte[] TrimAlignedBitmap(BitmapFormat originalFormat, BitmapFormat destinationFormat, int width, int height, byte[] data)
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
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                case BitmapFormat.ReachDxt3aAlpha:
                case BitmapFormat.ReachDxt3aMono:
                case BitmapFormat.ReachDxt5aAlpha:
                case BitmapFormat.ReachDxt5aMono:
                case BitmapFormat.ReachDxnMonoAlpha:
                    blockSize = 4;
                    break;
                case BitmapFormat.AY8:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.Y16:
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
                case BitmapFormat.A8R8G8B8_reach:
                case BitmapFormat.V16U16:
                case BitmapFormat.Y16:
                    return false;
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Unused1E:
                case BitmapFormat.Dxt5a:
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
