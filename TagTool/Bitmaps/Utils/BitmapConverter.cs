using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps.Utils
{
    public static class BitmapConverter
    {
        public static BaseBitmap ConvertGen3Bitmap(GameCache cache, Bitmap bitmap, int imageIndex, bool forDDS = false)
        {
            var image = bitmap.Images[imageIndex];

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
            {
                BitmapTextureInterleavedInteropResource resource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedResources[image.InterleavedTextureIndex1]);
                if (resource == null)
                    return null;

                BitmapTextureInteropDefinition definition;
                BitmapTextureInteropDefinition otherDefinition;
                int pairIndex = 0;

                if (image.InterleavedTextureIndex2 > 0)
                {
                    definition = resource.Texture.Definition.Bitmap2;
                    otherDefinition = resource.Texture.Definition.Bitmap1;
                    pairIndex = 1;
                }
                else
                {
                    definition = resource.Texture.Definition.Bitmap1;
                    otherDefinition = resource.Texture.Definition.Bitmap2;
                }
                return ConvertGen3Bitmap(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, definition, bitmap, imageIndex, true, pairIndex, otherDefinition, forDDS);
            }
            else
            {
                BitmapTextureInteropResource resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[imageIndex]);
                if (resource == null)
                    return null;

                return ConvertGen3Bitmap(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, resource.Texture.Definition.Bitmap, bitmap, imageIndex, false, 0, null, forDDS);
            }
        }

        private static BaseBitmap ConvertGen3Bitmap(byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, bool isPaired, int pairIndex, BitmapTextureInteropDefinition otherDefinition, bool forDDS)
        {
            if (primaryData == null && secondaryData == null)
                return null;

            using (var result = new MemoryStream())
            {
                int mipLevelCount = definition.MipmapCount;
                int layerCount = definition.BitmapType == BitmapType.CubeMap ? 6 : definition.Depth;

                if (definition.BitmapType == BitmapType.Array && mipLevelCount > 1)
                {
                    mipLevelCount = 1;
                    definition.MipmapCount = 1;
                }

                if (!forDDS)
                {
                    // order for d3d9, all faces first, then mipmaps
                    for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                    {
                        for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                        {
                            if (definition.BitmapType == BitmapType.CubeMap) // swap cubemap faces
                            {
                                if (layerIndex == 2)
                                    layerIndex = 3;
                                else if (layerIndex == 3)
                                    layerIndex = 2;
                            }

                            ConvertGen3BitmapData(result, primaryData, secondaryData, definition, bitmap, imageIndex, mipLevel, layerIndex, isPaired, pairIndex, otherDefinition);

                            if (definition.BitmapType == BitmapType.CubeMap)
                            {
                                if (layerIndex == 3)
                                    layerIndex = 2;
                                else if (layerIndex == 2)
                                    layerIndex = 3;
                            }
                        }
                    }
                }
                else
                {
                    for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                    {
                        if (definition.BitmapType == BitmapType.CubeMap) // swap cubemap faces
                        {
                            if (layerIndex == 2)
                                layerIndex = 3;
                            else if (layerIndex == 3)
                                layerIndex = 2;
                        }

                        for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                        {
                            ConvertGen3BitmapData(result, primaryData, secondaryData, definition, bitmap, imageIndex, mipLevel, layerIndex, isPaired, pairIndex, otherDefinition);
                        }
                        
                        if (definition.BitmapType == BitmapType.CubeMap)
                        {
                            if (layerIndex == 3)
                                layerIndex = 2;
                            else if (layerIndex == 2)
                                layerIndex = 3;
                        }
                    }
                }

                var resultData = result.ToArray();

                BaseBitmap resultBitmap = new BaseBitmap(bitmap.Images[imageIndex]);
                

                var newFormat = BitmapUtils.GetEquivalentBitmapFormat(bitmap.Images[imageIndex].Format);
                resultBitmap.UpdateFormat(newFormat);

                if(BitmapUtils.RequiresDecompression(resultBitmap.Format, (uint)resultBitmap.Width, (uint)resultBitmap.Height))
                {
                    resultBitmap.Format = BitmapFormat.A8R8G8B8;
                }

                if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                    resultBitmap.Flags &= ~BitmapFlags.Compressed;
                else
                    resultBitmap.Flags |= BitmapFlags.Compressed;

                //
                // Update resource definition/image, truncate DXN to level 4x4
                //

                resultBitmap.Data = resultData;
                
                if(resultBitmap.Format == BitmapFormat.Dxn) // wouldn't be required if d3d9 supported non power of two DXN and with mips less than 8x8
                {
                    GenerateCompressedMipMaps(resultBitmap);
                }

                
                if (resultBitmap.Type == BitmapType.Array) // for HO, arrays use the index of Texture3D
                    resultBitmap.Type = BitmapType.Texture3D;

                return resultBitmap;
            }
        }

        private static void ConvertGen3BitmapData(Stream resultStream, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, int layerIndex, bool isPaired, int pairIndex, BitmapTextureInteropDefinition otherDefinition)
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

            var textureType = BitmapUtils.GetXboxBitmapD3DTextureType(definition);
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
                    levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level);
                    uint alignedSecondaryLength = (uint)((secondaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedSecondaryLength];
                    Array.Copy(secondaryData, 0, data, 0, secondaryData.Length);
                }
                else
                {
                    levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level, useHighResBuffer);
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
                    levelOffset = BitmapUtils.GetXboxInterleavedBitmapOffset(bitmap1, bitmap2, layerIndex, level, pairIndex);
                    uint alignedSecondaryLength = (uint)((secondaryData.Length + 0x3FFF) & ~0x3FFF);
                    data = new byte[alignedSecondaryLength];
                    Array.Copy(secondaryData, 0, data, 0, secondaryData.Length);
                }
                else
                {
                    levelOffset = BitmapUtils.GetXboxInterleavedBitmapOffset(bitmap1, bitmap2, layerIndex, level, pairIndex, useHighResBuffer);
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

            uint actualWidth = (uint)definition.Width >> level;
            uint actualHeight = (uint)definition.Height >> level;

            if (actualWidth < 1)
                actualWidth = 1;
            if (actualHeight < 1)
                actualHeight = 1;
            bool requireDecompression = BitmapUtils.RequiresDecompression(BitmapUtils.GetEquivalentBitmapFormat(bitmap.Images[imageIndex].Format), (uint)definition.Width, (uint)definition.Height);
            finalData = BitmapUtils.ConvertXboxFormats(finalData, actualWidth, actualHeight, bitmap.Images[imageIndex].Format, requireDecompression);

            resultStream.Write(finalData, 0, finalData.Length);
        }

        private static void GenerateMipMaps(BaseBitmap bitmap)
        {
            switch (bitmap.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    GenerateCompressedMipMaps(bitmap);
                    break;
                case BitmapFormat.A8Y8:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.V8U8:
                case BitmapFormat.X8R8G8B8:
                    GenerateUncompressedMipMaps(bitmap);
                    break;

                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A32B32G32R32F:
                    bitmap.MipMapCount = 0;
                    break;
                default:
                    throw new Exception($"Unsupported format for mipmap generation {bitmap.Format}");

            }
        }

        private static void GenerateCompressedMipMaps(BaseBitmap bitmap)
        {
            string tempBitmap = $@"Temp\{Guid.NewGuid().ToString()}.dds";

            if (!Directory.Exists(@"Temp"))
                Directory.CreateDirectory(@"Temp");

            //Write input dds
            bitmap.MipMapCount = 0;
            var header = new DDSHeader(bitmap);


            using (var outStream = File.Open(tempBitmap, FileMode.Create, FileAccess.Write))
            {
                header.Write(new EndianWriter(outStream));
                var dataStream = new MemoryStream(bitmap.Data);
                StreamUtil.Copy(dataStream, outStream, bitmap.Data.Length);
            }

            string args = " ";

            switch (bitmap.Format)
            {
                case BitmapFormat.Dxn:
                    args += "-bc5 -resize -normal";
                    break;

                case BitmapFormat.Dxt1:
                    args += "-bc1 ";
                    break;
                case BitmapFormat.Dxt3:
                    args += "-bc2 ";
                    break;
                case BitmapFormat.Dxt5:
                    args += "-bc3 ";
                    break;

                default:
                    bitmap.MipMapCount = 0;
                    if (File.Exists(tempBitmap))
                        File.Delete(tempBitmap);
                    return;
            }

            args += $"{tempBitmap} {tempBitmap}";

            ProcessStartInfo info = new ProcessStartInfo(@"Tools\nvcompress.exe")
            {
                Arguments = args,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process nvcompress = Process.Start(info);
            nvcompress.WaitForExit();

            byte[] result;
            using (var ddsStream = File.OpenRead(tempBitmap))
            {
                header.Read(new EndianReader(ddsStream));
                var dataSize = (int)(ddsStream.Length - ddsStream.Position);

                int mipMapCount = header.MipMapCount - 1;

                bitmap.Width = header.Width;
                bitmap.Height = header.Height;

                // Remove lowest DXN mipmaps to prevent issues with D3D memory allocation.
                if (bitmap.Format == BitmapFormat.Dxn)
                {
                    dataSize = BitmapUtils.RoundSize(bitmap.Width, 4) * BitmapUtils.RoundSize(bitmap.Height, 4);
                    if (mipMapCount > 0)
                    {
                        if (bitmap.Format == BitmapFormat.Dxn)
                        {
                            var width = bitmap.Width;
                            var height = bitmap.Height;

                            dataSize = BitmapUtils.RoundSize(width, 4) * BitmapUtils.RoundSize(height, 4);

                            mipMapCount = 0;
                            while ((width >= 8) && (height >= 8))
                            {
                                width /= 2;
                                height /= 2;
                                dataSize += BitmapUtils.RoundSize(width, 4) * BitmapUtils.RoundSize(height, 4);
                                mipMapCount++;
                            }
                        }
                    }

                }
                bitmap.MipMapCount = mipMapCount;
                byte[] raw = new byte[dataSize];
                ddsStream.Read(raw, 0, dataSize);
                result = raw;
                bitmap.Data = result;
            }

            if (File.Exists(tempBitmap))
                File.Delete(tempBitmap);
        }

        private static void GenerateUncompressedMipMaps(BaseBitmap bitmap)
        {
            int channelCount;
            switch (bitmap.Format)
            {

                case BitmapFormat.A8Y8:
                case BitmapFormat.V8U8:
                    channelCount = 2;
                    break;
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                    channelCount = 1;
                    break;
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.X8R8G8B8:
                    channelCount = 4;
                    break;
                default:
                    bitmap.MipMapCount = 0;
                    return;

            }
            MipMapGenerator generator = new MipMapGenerator();
            generator.GenerateMipMap(bitmap.Height, bitmap.Width, bitmap.Data, channelCount);
            bitmap.MipMapCount = generator.MipMaps.Count;
            bitmap.Data = generator.CombineImage(bitmap.Data);
            return;
        }
    }
}
