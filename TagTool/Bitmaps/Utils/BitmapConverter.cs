using Assimp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Extensions;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps.Utils
{
    public static class BitmapConverter
    {
        public static BaseBitmap ConvertGen3Bitmap(GameCache cache, Bitmap bitmap, int imageIndex, string tagName, bool forDDS = false)
        {
            var image = bitmap.Images[imageIndex];

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360UseInterleavedTextures))
            {
                BitmapTextureInterleavedInteropResource resource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedHardwareTextures[image.InterleavedInterop]);
                if (resource == null)
                    return null;

                BitmapTextureInteropDefinition definition;
                BitmapTextureInteropDefinition otherDefinition;
                int pairIndex = 0;

                if (image.InterleavedTextureIndex > 0)
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
                return ConvertGen3Bitmap(resource.Texture.Definition.PrimaryResourceData.Data, 
                    resource.Texture.Definition.SecondaryResourceData.Data, 
                    definition, 
                    bitmap, 
                    imageIndex, 
                    true, 
                    pairIndex, 
                    otherDefinition, 
                    forDDS, 
                    cache.Version,
                cache.Platform,
                    tagName);
            }
            else
            {
                BitmapTextureInteropResource resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.HardwareTextures[imageIndex]);
                if (resource == null)
                    return null;

                return ConvertGen3Bitmap(resource.Texture.Definition.PrimaryResourceData.Data, 
                    resource.Texture.Definition.SecondaryResourceData.Data, 
                    resource.Texture.Definition.Bitmap, 
                    bitmap, 
                    imageIndex, 
                    false, 
                    0, 
                    null, 
                    forDDS, 
                    cache.Version, 
                    cache.Platform,
                    tagName);
            }
        }

        private static BaseBitmap ConvertGen3Bitmap(byte[] primaryData, 
            byte[] secondaryData, 
            BitmapTextureInteropDefinition definition, 
            Bitmap bitmap, 
            int imageIndex, 
            bool isPaired, 
            int pairIndex, 
            BitmapTextureInteropDefinition otherDefinition, 
            bool forDDS, 
            CacheVersion version, 
            CachePlatform cachePlatform,
            string tagName)
        {
            if (primaryData == null && secondaryData == null)
                return null;

            var result = new MemoryStream();
            int mipLevelCount = definition.MipmapCount;
            int layerCount = definition.BitmapType == BitmapType.CubeMap ? 6 : definition.Depth;
            bool swapCubemapFaces = definition.BitmapType == BitmapType.CubeMap && cachePlatform != CachePlatform.MCC;

            if (definition.BitmapType == BitmapType.Array && mipLevelCount > 1)
                mipLevelCount = 1;

            foreach (var (layerIndex, mipLevel) in GetBitmapSurfacesEnumerable(layerCount, mipLevelCount, forDDS))
            {
                int sourceLayerIndex = layerIndex;

                if (swapCubemapFaces) // swap cubemap faces
                {
                    if (layerIndex == 1)
                        sourceLayerIndex = 2;
                    else if (layerIndex == 2)
                        sourceLayerIndex = 1;
                }

                if (cachePlatform == CachePlatform.MCC)
                    ConvertGen3BitmapDataMCC(result, primaryData, secondaryData, definition, bitmap, imageIndex, mipLevel, sourceLayerIndex, isPaired, pairIndex, otherDefinition, version);
                else
                    ConvertGen3BitmapData(result, primaryData, secondaryData, definition, bitmap, imageIndex, mipLevel, sourceLayerIndex, isPaired, pairIndex, otherDefinition, version);
            }

            BaseBitmap resultBitmap = new BaseBitmap(bitmap.Images[imageIndex]);
            resultBitmap.Data = result.ToArray();
            resultBitmap.MipMapCount = mipLevelCount;
            PostProcessResultBitmap(bitmap, imageIndex, cachePlatform, tagName, resultBitmap, version);

            return resultBitmap;
        }

        private static IEnumerable<(int layerIndex, int mipLevel)> GetBitmapSurfacesEnumerable(int layerCount, int mipLevelCount, bool forDDS)
        {
            if (!forDDS)
            {
                // order for d3d9, all faces first, then mipmaps
                for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                    for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                        yield return (layerIndex, mipLevel);
            }
            else
            {
                for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                    for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                        yield return (layerIndex, mipLevel);
            }
        }

        private static void PostProcessResultBitmap(Bitmap bitmap, int imageIndex, CachePlatform cachePlatform, string tagName, BaseBitmap resultBitmap, CacheVersion version)
        {
            // fix enum from reach
            if (version >= CacheVersion.HaloReach)
            {
                if (resultBitmap.Format >= (BitmapFormat)38)
                    resultBitmap.Format -= 5;

                // cubemap compatibility - this is required for h3 shaders to look correct when using reach dynamic cubemaps
                if (bitmap.Images[imageIndex].ExponentBias == 2)
                {
                    byte[] rawData = BitmapDecoder.DecodeBitmap(resultBitmap.Data, resultBitmap.Format, bitmap.Images[imageIndex].Width, bitmap.Images[imageIndex].Height);

                    const float oneDiv255 = 1.0f / 255.0f;
                    float expBias = (float)Math.Pow(2.0f, bitmap.Images[imageIndex].ExponentBias); // 4.0f
                    for (int i = 0; i < rawData.Length; i += 4)
                    {
                        var vector = VectorExtensions.InitializeVector(new float[] { rawData[i], rawData[i + 1], rawData[i + 2], rawData[i + 3] });

                        vector *= oneDiv255; // 0-1 range
                        // need more math here. not sure if it can be prebaked. this should do for now.
                        vector *= vector;
                        vector *= 255.0f; // 0-255 range

                        rawData[i + 0] = (byte)vector[0];
                        rawData[i + 1] = (byte)vector[1];
                        rawData[i + 2] = (byte)vector[2];
                        //rawData[i + 3] = (byte)(biasedAlpha * 255.0f); // no need to touch alpha.
                    }

                    resultBitmap.Data = BitmapDecoder.EncodeBitmap(rawData, resultBitmap.Format, bitmap.Images[imageIndex].Width, bitmap.Images[imageIndex].Height);
                }
            }

            if (cachePlatform == CachePlatform.MCC)
            {
                if (resultBitmap.Format == BitmapFormat.V8U8 ||
                    resultBitmap.Format == BitmapFormat.V16U16)
                {
                    resultBitmap.UpdateFormat(BitmapFormat.Dxn);
                }
            }
            // fix dxt5 bumpmaps (h3 wraith bump)
            else if (bitmap.Usage == Bitmap.BitmapUsageGlobalEnum.BumpMapfromHeightMap &&
                resultBitmap.Format == BitmapFormat.Dxt5 &&
                tagName != @"levels\multi\zanzibar\bitmaps\palm_frond_a_bump") // this tag is actually an alpha test bitmap, ignore it
            {
                // convert to raw RGBA
                byte[] rawData = BitmapDecoder.DecodeBitmap(resultBitmap.Data, resultBitmap.Format, bitmap.Images[imageIndex].Width, bitmap.Images[imageIndex].Height);

                // (0<->1) to (-1<->1)
                for (int i = 0; i < rawData.Length; i += 4)
                {
                    rawData[i + 0] = (byte)((((rawData[i + 0] / 255.0f) + 1.007874015748031f) / 2.007874015748031f) * 255.0f);
                    rawData[i + 1] = (byte)((((rawData[i + 1] / 255.0f) + 1.007874015748031f) / 2.007874015748031f) * 255.0f);
                    rawData[i + 2] = (byte)((((rawData[i + 2] / 255.0f) + 1.007874015748031f) / 2.007874015748031f) * 255.0f);
                    rawData[i + 3] = 0xFF;
                }

                // encode as DXN. unfortunately mips have artifacts, maybe this can be fixed?
                resultBitmap.Data = EncodeDXN(rawData, bitmap.Images[imageIndex].Width, bitmap.Images[imageIndex].Height, out resultBitmap.MipMapCount, true);
                resultBitmap.UpdateFormat(BitmapFormat.Dxn);
                resultBitmap.Flags |= BitmapFlags.Compressed;
            }
            else
            {
                // fix slope_water bitmap conversion
                if (resultBitmap.Format == BitmapFormat.V8U8)
                {
                    resultBitmap.MipMapCount = 1;
                    resultBitmap.Curve = BitmapImageCurve.Unknown;
                }

                var newFormat = BitmapUtils.GetEquivalentBitmapFormat(resultBitmap.Format);

                if (resultBitmap.Format == BitmapFormat.Ctx1 && bitmap.Images[imageIndex].Type == BitmapType.Array)
                    newFormat = BitmapFormat.A8R8G8B8;

                resultBitmap.UpdateFormat(newFormat);

                if (BitmapUtils.RequiresDecompression(resultBitmap.Format, (uint)resultBitmap.Width, (uint)resultBitmap.Height))
                {
                    resultBitmap.Format = BitmapFormat.A8R8G8B8;
                }

                if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                    resultBitmap.Flags &= ~BitmapFlags.Compressed;
                else
                    resultBitmap.Flags |= BitmapFlags.Compressed;
            }

            // truncate DXN to level 4x4
            if (resultBitmap.Format == BitmapFormat.Dxn) // wouldn't be required if d3d9 supported non power of two DXN and with mips less than 4x4
            {
                if (resultBitmap.Type == BitmapType.Array || resultBitmap.Type == BitmapType.Texture3D)
                {
                    resultBitmap.MipMapCount = 1;
                }
                else
                {
                    // Avoid having nvcompress touch the image data if we don't need to
                    if (!Direct3D.D3D9x.D3D.IsPowerOfTwo(resultBitmap.Width) ||
                        !Direct3D.D3D9x.D3D.IsPowerOfTwo(resultBitmap.Height))
                    {
                        GenerateCompressedMipMaps(resultBitmap);
                    }
                    else
                    {
                        // Remove lowest DXN mipmaps to prevent issues with D3D memory allocation.

                        TrimLowestMipmaps(resultBitmap);
                    }
                }

            }


            if (resultBitmap.Type == BitmapType.Array) // for HO, arrays use the index of Texture3D
                resultBitmap.Type = BitmapType.Texture3D;
        }

        public static void TrimLowestMipmaps(BaseBitmap resultBitmap)
        {
            int dataSize = 0;
            int mipMapCount;
            int width = resultBitmap.Width;
            int height = resultBitmap.Height;
            for (mipMapCount = 0; mipMapCount < resultBitmap.MipMapCount; mipMapCount++)
            {
                if (width < 4 || height < 4)
                    break;

                dataSize += BitmapUtils.RoundSize(width, 4) * BitmapUtils.RoundSize(height, 4);
                width /= 2;
                height /= 2;
            }

            resultBitmap.MipMapCount = mipMapCount;
            byte[] raw = new byte[dataSize];
            Array.Copy(resultBitmap.Data, raw, dataSize);
            resultBitmap.Data = raw;
        }

        private unsafe static void ConvertGen3BitmapDataMCC(Stream resultStream, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, int layerIndex, bool isPaired, int pairIndex, BitmapTextureInteropDefinition otherDefinition, CacheVersion version)
        {
            var pixelDataOffset = BitmapUtilsPC.GetTextureOffset(bitmap.Images[imageIndex], level);
            var pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(bitmap.Images[imageIndex], level);
            int width = BitmapUtilsPC.GetMipmapWidth(bitmap.Images[imageIndex], level);
            int height = BitmapUtilsPC.GetMipmapHeight(bitmap.Images[imageIndex], level);

            byte[] pixelData = new byte[pixelDataSize];
            if (level == 0 && definition.HighResInSecondaryResource > 0 || primaryData == null)
            {
                Array.Copy(secondaryData, pixelDataOffset, pixelData, 0, pixelData.Length);
            }
            else
            {
                if (secondaryData != null)
                    pixelDataOffset -= secondaryData.Length;
                Array.Copy(primaryData, pixelDataOffset, pixelData, 0, pixelDataSize);
            }
            
            if(bitmap.Images[imageIndex].Format == BitmapFormat.Dxn)
            {
                // convert bc5_snorm to ati2n_unorm
                byte[] rgba = BitmapDecoder.DecodeDxnSigned(pixelData, width, height, true);
                pixelData = EncodeDXN(rgba, width, height, out var _);
            }
            else if (bitmap.Images[imageIndex].Format == BitmapFormat.V8U8)
            {
                // convert R8G8_SNORM to ati2n_unorm
                var rgba = BitmapDecoder.DecodeV8U8(pixelData, width, height, true);
                pixelData = EncodeDXN(rgba, width, height, out var _);
            }
            else if (bitmap.Images[imageIndex].Format == BitmapFormat.V16U16)
            {
                // convert R16G16_SNORM to ati2n_unorm
                var rgba = BitmapDecoder.DecodeV16U16(pixelData, width, height, true);
                pixelData = EncodeDXN(rgba, width, height, out var _);
            }
            else if (bitmap.Images[imageIndex].Format == BitmapFormat.A2R10G10B10)
            {
                // convert DXGI_FORMAT_R10G10B10A2_UNORM to A2R10G10B10
                fixed(byte *ptr = pixelData)
                {
                    for (int i = 0; i < width * height; i++)
                    {
                        uint* pixel = (uint*)&ptr[i*4];
                        uint R = (*pixel & 0x3ff00000) >> 20;
                        uint G = (*pixel & 0x000ffc00);
                        uint B = (*pixel & 0x000003ff) << 20;
                        uint A = (*pixel & 0xC0000000);
                        *pixel = B | G | R | A;
                    }
                }
            }

            resultStream.Write(pixelData, 0, pixelData.Length);
        }

        private static void ConvertGen3BitmapData(Stream resultStream, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, int layerIndex, bool isPaired, int pairIndex, BitmapTextureInteropDefinition otherDefinition, CacheVersion version)
        {
            byte[] finalData = XboxBitmapUtils.GetXboxBitmapLevelData(primaryData, secondaryData, definition, level, layerIndex, isPaired, pairIndex, otherDefinition);

            uint actualWidth = Math.Max(1, (uint)definition.Width >> level);
            uint actualHeight = Math.Max(1, (uint)definition.Height >> level);

            bool requireDecompression = BitmapUtils.RequiresDecompression(BitmapUtils.GetEquivalentBitmapFormat(bitmap.Images[imageIndex].Format), (uint)definition.Width, (uint)definition.Height);
            finalData = BitmapUtils.ConvertXboxFormats(finalData, actualWidth, actualHeight, bitmap.Images[imageIndex].Format, bitmap.Images[imageIndex].Type, requireDecompression, version);

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
                case BitmapFormat.Y16:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.V8U8:
                case BitmapFormat.V16U16:
                case BitmapFormat.X8R8G8B8:
                    GenerateUncompressedMipMaps(bitmap);
                    break;

                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A32B32G32R32F:
                    bitmap.MipMapCount = 1;
                    break;
                default:
                    throw new Exception($"Unsupported format for mipmap generation {bitmap.Format}");

            }
        }

        public static byte[] EncodeDXN(byte[] rgba, int width, int height, out int mipCount, bool generateMips = false, bool resize = false)
        {
            string tempBitmap = $@"Temp\{Guid.NewGuid().ToString()}.dds";

            if (!Directory.Exists(@"Temp"))
                Directory.CreateDirectory(@"Temp");

            try
            {
                var ddsFile = new DDSFile(new DDSHeader((uint)width, (uint)height, 1, 1, BitmapFormat.A8R8G8B8, BitmapType.Texture2D), rgba);
                using (var writer = new EndianWriter(File.Create(tempBitmap)))
                    ddsFile.Write(writer);

                ProcessStartInfo info = new ProcessStartInfo($@"{Program.TagToolDirectory}\Tools\nvcompress.exe")
                {
                    Arguments = $"-bc5 -normal -tonormal {(generateMips ? "" : "-nomips")} {(!resize ? "" : "-resize")} {tempBitmap} {tempBitmap}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false
                };
                Process nvcompress = Process.Start(info);
                nvcompress.WaitForExit();

                using (var reader = new EndianReader(File.OpenRead(tempBitmap)))
                {
                    ddsFile = new DDSFile();
                    ddsFile.Read(reader);
                    mipCount = ddsFile.Header.MipMapCount;
                    return ddsFile.BitmapData;
                }
            }
            finally
            {
                if(File.Exists(tempBitmap))
                    File.Delete(tempBitmap);
            }
        }

        public static void GenerateCompressedMipMaps(BaseBitmap bitmap)
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
                    args += "-bc5 -resize -normal -tonormal";
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
                    bitmap.MipMapCount = 1;
                    if (File.Exists(tempBitmap))
                        File.Delete(tempBitmap);
                    return;
            }

            args += $"{tempBitmap} {tempBitmap}";

            ProcessStartInfo info = new ProcessStartInfo($@"{Program.TagToolDirectory}\Tools\nvcompress.exe")
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

            using (var ddsStream = File.OpenRead(tempBitmap))
            {
                header.Read(new EndianReader(ddsStream));
                var dataSize = (int)(ddsStream.Length - ddsStream.Position);

                bitmap.Width = header.Width;
                bitmap.Height = header.Height;
                bitmap.Data = new byte[dataSize];
                bitmap.MipMapCount = Math.Max(1, header.MipMapCount - 1);
                ddsStream.Read(bitmap.Data, 0, dataSize);

                // Remove lowest DXN mipmaps to prevent issues with D3D memory allocation.
                if (bitmap.Format == BitmapFormat.Dxn)
                    TrimLowestMipmaps(bitmap);
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
                case BitmapFormat.V16U16:
                    channelCount = 2;
                    break;
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.Y16:
                    channelCount = 1;
                    break;
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.X8R8G8B8:
                    channelCount = 4;
                    break;
                default:
                    bitmap.MipMapCount = 1;
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
