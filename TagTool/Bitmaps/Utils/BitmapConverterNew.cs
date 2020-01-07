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
    public static class BitmapConverterNew
    {
        public static BaseBitmap ConvertGen3Bitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                return ConvertGen3InterleavedBitmap(cache, bitmap, imageIndex);
            else
                return ConvertGen3RegularBitmap(cache, bitmap, imageIndex);
        }

        private static BaseBitmap ConvertGen3InterleavedBitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            var resourceReference = bitmap.InterleavedResources[image.InterleavedTextureIndex1];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(resourceReference);

            if (resourceDefinition == null)
                return null;

            BitmapTextureInteropDefinition bitmapTextureInteropDef;

            if(image.InterleavedTextureIndex2 == 0)
                bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap1;
            else
                bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap2;

            return ConvertGen3BitmapInternal(bitmapTextureInteropDef, resourceDefinition.Texture.Definition.PrimaryResourceData.Data, resourceDefinition.Texture.Definition.SecondaryResourceData.Data, image);
        }

        private static BaseBitmap ConvertGen3RegularBitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            var resourceReference = bitmap.Resources[imageIndex];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);

            if (resourceDefinition == null)
                return null;

            BitmapTextureInteropDefinition bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap;

            return ConvertGen3BitmapInternal(bitmapTextureInteropDef, resourceDefinition.Texture.Definition.PrimaryResourceData.Data, resourceDefinition.Texture.Definition.SecondaryResourceData.Data, image);
        }


        private static BaseBitmap ConvertGen3BitmapInternal(BitmapTextureInteropDefinition bitmapTextureInteropDefinition, byte[] primaryData, byte[] secondaryData, Bitmap.Image image)
        {
            var xboxBitmap = new XboxBitmap(bitmapTextureInteropDefinition, image);
            var bitmapSize = BitmapUtils.GetXboxImageSize(xboxBitmap);

            byte[] imageData = new byte[bitmapSize];
            int offset;
            bool usePadding;

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
            {
                offset = image.DataOffset;
                usePadding = image.DataSize < 0 ? true : false;

            }
            else
            {
                offset = 0;
                usePadding = false;
            }

            if (bitmapTextureInteropDefinition.HighResInSecondaryResource == 1)
            {
                if (secondaryData == null)
                    return null;

                int copySize = bitmapSize;
                if (usePadding)
                    copySize = secondaryData.Length - offset;

                Array.Copy(secondaryData, offset, imageData, 0, copySize);

            }
            else
            {
                if (primaryData == null)
                    return null;

                int copySize = bitmapSize;
                if (usePadding)
                    copySize = primaryData.Length - offset;

                Array.Copy(primaryData, offset, imageData, 0, copySize);
            }

            List<XboxBitmap> xboxBitmaps = ParseImages(xboxBitmap, image, imageData, bitmapSize);
            bool flipImage;
            // rearrange cubemaps order
            if (xboxBitmap.Type == BitmapType.CubeMap)
            {
                XboxBitmap temp = xboxBitmaps[1];
                xboxBitmaps[1] = xboxBitmaps[2];
                xboxBitmaps[2] = temp;

            }

            List<BaseBitmap> finalBitmaps = new List<BaseBitmap>();
            foreach (var bitmap in xboxBitmaps)
            {
                // extract bitmap from padded image
                BaseBitmap finalBitmap = ExtractImage(bitmap);
                // convert to PC format
                flipImage = ConvertImage(finalBitmap);
                // flip data if required
                if (flipImage)
                    FlipImage(finalBitmap, image);
                // until I write code to move mipmaps at the end of the file, remove cubemap mipmaps
                if (xboxBitmap.Type == BitmapType.CubeMap)
                {
                    finalBitmap.MipMapCount = 0;
                }
                // generate mipmaps for uncompressed textures
                if (!finalBitmap.Flags.HasFlag(BitmapFlags.Compressed) && finalBitmap.MipMapCount > 0)
                    GenerateUncompressedMipMaps(finalBitmap);

                finalBitmaps.Add(finalBitmap);
            }
            // build and return the final bitmap
            return RebuildBitmap(finalBitmaps);
        }


        private static List<XboxBitmap> ParseImages(XboxBitmap xboxBitmap, Bitmap.Image image, byte[] imageData, int bitmapSize)
        {
            List<XboxBitmap> xboxBitmaps = new List<XboxBitmap>();
            switch (image.Type)
            {
                case BitmapType.Texture2D:
                    xboxBitmap.Data = imageData;
                    xboxBitmaps.Add(xboxBitmap);
                    if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                        xboxBitmap.Data = BitmapDecoder.ConvertToLinearTexture(xboxBitmap.Data, xboxBitmap.VirtualWidth, xboxBitmap.VirtualHeight, xboxBitmap.Format);
                    break;
                case BitmapType.Texture3D:
                case BitmapType.Array:
                    var count = xboxBitmap.Depth;
                    var size = bitmapSize / count;
                    for (int i = 0; i < count; i++)
                    {
                        byte[] data = new byte[size];
                        Array.Copy(imageData, i * size, data, 0, size);
                        XboxBitmap newXboxBitmap = xboxBitmap.ShallowCopy();

                        if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            data = BitmapDecoder.ConvertToLinearTexture(data, xboxBitmap.VirtualWidth, xboxBitmap.VirtualHeight, xboxBitmap.Format);

                        newXboxBitmap.Data = data;
                        xboxBitmaps.Add(newXboxBitmap);
                    }
                    break;
                case BitmapType.CubeMap:
                    count = 6;
                    size = bitmapSize / count;
                    for (int i = 0; i < count; i++)
                    {
                        byte[] data = new byte[size];
                        Array.Copy(imageData, i * size, data, 0, size);
                        XboxBitmap newXboxBitmap = xboxBitmap.ShallowCopy();

                        if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            data = BitmapDecoder.ConvertToLinearTexture(data, xboxBitmap.VirtualWidth, xboxBitmap.VirtualHeight, xboxBitmap.Format);

                        newXboxBitmap.Data = data;
                        xboxBitmaps.Add(newXboxBitmap);
                    }
                    break;
            }
            return xboxBitmaps;
        }

        private static BaseBitmap ExtractImage(XboxBitmap bitmap)
        {
            if (bitmap.NotExact)
            {
                int dataHeight;

                if (!bitmap.MultipleOfBlockDimension)
                {
                    dataHeight = bitmap.NearestHeight;
                }
                else
                {
                    dataHeight = bitmap.Height;
                }

                byte[] data = new byte[BitmapUtils.GetImageSize(bitmap)];
                int numberOfPass = dataHeight / bitmap.BlockDimension;
                for (int i = 0; i < numberOfPass; i++)
                {
                    Array.Copy(bitmap.Data, i * bitmap.TilePitch + bitmap.Offset, data, i * bitmap.Pitch, bitmap.Pitch);
                }
                bitmap.Data = data;
            }
            return bitmap;
        }

        private static bool ConvertImage(BaseBitmap bitmap)
        {
            BitmapFormat targetFormat = bitmap.Format;
            var data = bitmap.Data;
            bool DXTFlip = false;
            switch (bitmap.Format)
            {
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                    targetFormat = BitmapFormat.Y8;
                    bitmap.Flags &= ~BitmapFlags.Compressed;
                    break;

                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt5aAlpha:
                    targetFormat = BitmapFormat.A8;
                    bitmap.Flags &= ~BitmapFlags.Compressed;
                    break;

                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.AY8:
                    targetFormat = BitmapFormat.A8Y8; ;
                    bitmap.Flags &= ~BitmapFlags.Compressed;
                    break;

                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                    targetFormat = BitmapFormat.A8R8G8B8;
                    break;

                case BitmapFormat.A8Y8:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.X8R8G8B8:
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A32B32G32R32F:
                case BitmapFormat.V8U8:
                    break;

                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    if (bitmap.Height != bitmap.NearestHeight || bitmap.Width != bitmap.NearestWidth)
                    {
                        targetFormat = BitmapFormat.A8R8G8B8;
                        bitmap.Flags &= ~BitmapFlags.Compressed;
                        DXTFlip = true;
                    }
                    break;

                case BitmapFormat.Ctx1:
                    bitmap.UpdateFormat(BitmapFormat.Dxn);
                    data = BitmapDecoder.Ctx1ToDxn(data, bitmap.NearestWidth, bitmap.NearestHeight);
                    targetFormat = BitmapFormat.Dxn;
                    break;

                default:
                    throw new Exception($"Unsupported bitmap format {bitmap.Format}");
            }

            if (targetFormat != bitmap.Format)
            {
                data = BitmapDecoder.DecodeBitmap(data, bitmap.Format, bitmap.NearestWidth, bitmap.NearestHeight);
                data = BitmapDecoder.EncodeBitmap(data, targetFormat, bitmap.NearestWidth, bitmap.NearestHeight);

                bool reformat = false;

                if (bitmap.NearestHeight != bitmap.Height || bitmap.NearestWidth != bitmap.Width)
                    reformat = true;

                if (reformat)
                {
                    var compressionFactor = BitmapFormatUtils.GetCompressionFactor(targetFormat);
                    int fixedSize = (int)(bitmap.Width * bitmap.Height / compressionFactor);
                    int tilePitch = (int)(bitmap.NearestWidth / compressionFactor);
                    int pitch = (int)(bitmap.Width / compressionFactor);

                    byte[] fixedData = new byte[fixedSize];
                    int numberOfPass = bitmap.Height;   // encode does not give back block compressed data.
                    for (int i = 0; i < numberOfPass; i++)  // may need to compute an offset for special bitmaps
                    {
                        Array.Copy(data, i * tilePitch, fixedData, i * pitch, pitch);
                    }
                    data = fixedData;
                }

                bitmap.UpdateFormat(targetFormat);
                bitmap.Data = data;
            }

            bitmap.Data = data;

            if (DXTFlip)
                return false;

            return true;
        }

        private static void FlipImage(BaseBitmap bitmap, Bitmap.Image image)
        {
            switch (image.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    for (int j = 0; j < bitmap.Data.Length; j += 2)
                        Array.Reverse(bitmap.Data, j, 2);
                    break;

                case BitmapFormat.AY8:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Ctx1:
                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A4R4G4B4:
                    break;

                case BitmapFormat.A8R8G8B8:
                case BitmapFormat.X8R8G8B8:
                    for (int j = 0; j < bitmap.Data.Length; j += 4)
                        Array.Reverse(bitmap.Data, j, 4);
                    break;

                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A8Y8:
                case BitmapFormat.V8U8:
                    for (int j = 0; j < bitmap.Data.Length; j += 2)
                        Array.Reverse(bitmap.Data, j, 2);
                    break;

                default:
                    throw new Exception($"Unsupported format {image.Format} flipping");
            }

            if (bitmap.Format == BitmapFormat.Dxn)
                bitmap.Data = BitmapDecoder.SwapXYDxn(bitmap.Data, bitmap.Width, bitmap.Height);

        }

        private static BaseBitmap RebuildBitmap(List<BaseBitmap> bitmaps)
        {
            int totalSize = 0;
            foreach (var b in bitmaps)
                totalSize += b.Data.Length;

            byte[] totalData = new byte[totalSize];
            int currentPos = 0;

            for (int i = 0; i < bitmaps.Count; i++)
            {
                var bitmap = bitmaps[i];
                Array.Copy(bitmap.Data, 0, totalData, currentPos, bitmap.Data.Length);
                currentPos += bitmap.Data.Length;
            }
            var finalBitmap = bitmaps[0];
            finalBitmap.Data = totalData;

            if (finalBitmap.Flags.HasFlag(BitmapFlags.Compressed) && finalBitmap.MipMapCount > 0)
                GenerateCompressedMipMaps(finalBitmap);
            return finalBitmap;
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
