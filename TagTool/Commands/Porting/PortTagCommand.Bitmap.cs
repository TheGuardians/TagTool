using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Bitmap ConvertBitmap(Bitmap bitmap)
        {
            bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
            bitmap.UnknownB4 = 0;

            //
            // For each bitmaps, apply conversion process and create a new list of resources that will replace the one from H3.
            //

            var resourceList = new List<Bitmap.BitmapResource>();
            for (int i = 0; i < bitmap.Images.Count(); i++)
            {
                try
                {
                    var resource = ConvertBlamBitmap(bitmap, i);
                    Bitmap.BitmapResource bitmapResource = new Bitmap.BitmapResource
                    {
                        Resource = resource
                    };
                    resourceList.Add(bitmapResource);
                }
                catch
                {
                    Console.WriteLine("Failed to port bitmap");
                    return null;
                }
            }

            bitmap.Resources = resourceList;
            bitmap.InterleavedResources = null;

            return bitmap;
        }

        public static DdsHeader CreateDdsHeader(Bitmap.Image image)
        {
            var info = image;
            var result = new DdsHeader
            {
                Width = (uint)info.Width,
                Height = (uint)info.Height,
                MipMapCount = (uint)info.MipmapCount
            };
            BitmapDdsFormatDetection.SetUpHeaderForFormat(info.Format, result);
            switch (info.Type)
            {
                case BitmapType.CubeMap:
                    result.SurfaceComplexityFlags = DdsSurfaceComplexityFlags.Complex;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.CubeMap | DdsSurfaceInfoFlags.CubeMapNegativeX |
                                              DdsSurfaceInfoFlags.CubeMapNegativeY | DdsSurfaceInfoFlags.CubeMapNegativeZ |
                                              DdsSurfaceInfoFlags.CubeMapPositiveX | DdsSurfaceInfoFlags.CubeMapPositiveY |
                                              DdsSurfaceInfoFlags.CubeMapPositiveZ;
                    break;
                case BitmapType.Texture3D:
                    result.Depth = (uint)info.Depth;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.Volume;
                    break;
            }
            return result;
        }

        public static DdsHeader CreateDdsHeader(BlamBitmap image, bool noMips = false)
        {
            var info = image;
            var result = new DdsHeader
            {
                Width = (uint)info.Width,
                Height = (uint)info.Height,
                MipMapCount = noMips ? 0 : (uint)image.MipMapCount
            };
            BitmapDdsFormatDetection.SetUpHeaderForFormat(info.Format, result);
            switch (info.Type)
            {
                case BitmapType.CubeMap:
                    result.SurfaceComplexityFlags = DdsSurfaceComplexityFlags.Complex;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.CubeMap | DdsSurfaceInfoFlags.CubeMapNegativeX |
                                              DdsSurfaceInfoFlags.CubeMapNegativeY | DdsSurfaceInfoFlags.CubeMapNegativeZ |
                                              DdsSurfaceInfoFlags.CubeMapPositiveX | DdsSurfaceInfoFlags.CubeMapPositiveY |
                                              DdsSurfaceInfoFlags.CubeMapPositiveZ;
                    break;
                case BitmapType.Texture3D:
                    result.Depth = (uint)info.Depth;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.Volume;
                    break;
            }
            return result;
        }

        private PageableResource ConvertBlamBitmap(Bitmap bitmap, int imageIndex)
        {

            BlamBitmap blamBitmap = new BlamBitmap(bitmap.Images[imageIndex], 0, 0);

            byte[] raw = new byte[0];
            var rawSize = blamBitmap.Type == BitmapType.CubeMap ? blamBitmap.RawSize * 6 : blamBitmap.RawSize;
            
            try
            {
                if (blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                {
                    //First or second image in an interleaved bitmap
                    var interleavedIndex = blamBitmap.Image.InterleavedTextureIndex2;

                    int rawID = bitmap.InterleavedResources[blamBitmap.Image.InterleavedTextureIndex1].ZoneAssetHandle;
                    byte[] totalRaw = BlamCache.GetRawFromID(rawID, (interleavedIndex + 1) * rawSize);

                    if (totalRaw == null)
                        throw new Exception("Raw not found");

                    raw = new byte[rawSize];

                    if (blamBitmap.VirtualHeight != blamBitmap.Height || blamBitmap.VirtualWidth != blamBitmap.Width)
                    {
                        Array.Copy(totalRaw, (int)(interleavedIndex * (blamBitmap.Width * blamBitmap.MinimalBitmapSize * 2 / blamBitmap.CompressionFactor)), raw, 0, rawSize);
                    }
                    else
                    {
                        Array.Copy(totalRaw, interleavedIndex * rawSize, raw, 0, rawSize);
                    }
                }
                else
                {
                    int rawID = bitmap.Resources[imageIndex].ZoneAssetHandle;
                    raw = BlamCache.GetRawFromID(rawID, rawSize);
                    if (raw == null)
                        throw new Exception("Raw not found");
                }
            }
            catch
            {
                //2 different type of crashes, missing raw or out of bounds when trying to get the raw
                throw new Exception("Raw not found or bad offset");
            }
            
            if (blamBitmap.Type == BitmapType.Texture2D)
            {
                raw = ConvertBlamBitmapData(raw, blamBitmap);
            }
            else if (blamBitmap.Type == BitmapType.CubeMap)
            {
                raw = ConvertBlamCubeData(raw, blamBitmap);
            }
            else if (blamBitmap.Type == BitmapType.Array)
            {
                raw = ConvertBlamArrayData(raw, blamBitmap);
            }
            else
            {
                throw new Exception("Unknown Bitmap Type");
            }

            //Set tag data to match blamBitmap
            blamBitmap.RawSize = raw.Length;
            SetTagData(blamBitmap, bitmap.Images[imageIndex]);

            var resource = new PageableResource
            {
                Page = new RawPage(),
                Resource = new TagResource
                {
                    ResourceFixups = new List<TagResource.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                    Type = TagResourceType.Bitmap,
                    Unknown2 = 1
                }
            };

            using (var dataStream = new MemoryStream(raw))
            {
                var bitmapResource = new Bitmap.BitmapResource
                {
                    Resource = resource,
                    Unknown4 = 0
                };
                var resourceContext = new ResourceSerializationContext(resource);

                // Create new definition
                var resourceDefinition = new BitmapTextureInteropResource
                {
                    Texture = new D3DPointer<BitmapTextureInteropResource.BitmapDefinition>
                    {
                        Definition = new BitmapTextureInteropResource.BitmapDefinition
                        {
                            Data = new TagData(),
                            UnknownData = new TagData(),
                        }
                    }
                };

                var texture = resourceDefinition.Texture.Definition;
                var imageData = blamBitmap;

                // Set resource definition;

                var dataSize = (int)(dataStream.Length);
                texture.Data = new TagData(dataSize, new CacheAddress(CacheAddressType.Resource, 0));
                texture.Width = (short)imageData.Width;
                texture.Height = (short)imageData.Height;
                texture.Depth = (sbyte)imageData.Depth;
                texture.MipmapCount = (sbyte)(imageData.MipMapCount + 1);
                texture.Type = imageData.Image.Type;
                texture.D3DFormatUnused = GetUnusedFormat(blamBitmap.Format);
                texture.Format = imageData.Format;
                texture.Flags = bitmap.Images[imageIndex].Flags;


                //
                // Serialize the new resource definition
                //

                CacheContext.AddResource(resource, ResourceLocation.ResourcesB, dataStream);
                CacheContext.Serializer.Serialize(resourceContext, resourceDefinition);
            }

            return resource;
        }

        private byte[] CompressBitmap(BlamBitmap blamBitmap, BitmapFormat format, byte[] image, bool noMips)
        {
            string tempBitmap = @"Temp\bitmap.dds";

            if (!File.Exists(@"Temp"))
                Directory.CreateDirectory(@"Temp");

            if (File.Exists(tempBitmap))
                File.Delete(tempBitmap);

            //Write input image, assuming format is A8R8G8B8
            using (var outStream = File.Open(tempBitmap, FileMode.Create, FileAccess.Write))
            {
                var header = CreateDdsHeader(blamBitmap,true);
                header.WriteTo(outStream);
                outStream.Write(image, 0, image.Length);

            }

            string args = " ";

            if (format == BitmapFormat.Dxn)
                args += "-normal ";

            if (noMips)
                args += "-nomips ";

            args += "-resize ";
            args += "-fast ";

            switch (format)
            {
                case BitmapFormat.Dxn:
                    args += "-bc5 ";
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
                    return null;
            }

            args += @"Temp\bitmap.dds " + @"Temp\bitmap.dds";

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
                var dds = DdsHeader.Read(ddsStream);
                var dataSize = (int)(ddsStream.Length - ddsStream.Position);


                blamBitmap.Type = BitmapDdsFormatDetection.DetectType(dds);
                blamBitmap.Format = BitmapDdsFormatDetection.DetectFormat(dds);
                blamBitmap.Height = (int)dds.Height;
                blamBitmap.Width = (int)dds.Width;
                blamBitmap.MipMapCount = (int)dds.MipMapCount-1;
                blamBitmap.RawSize = dataSize;

                if(format == BitmapFormat.Dxn)
                {
                    if (!noMips)
                    {
                        var width = blamBitmap.Width;
                        var height = blamBitmap.Height;
                        dataSize = width*height;
                        blamBitmap.MipMapCount = 0;
                        while ((width >= 8) && (height >= 8))
                        {
                            width /= 2;
                            height /= 2;
                            dataSize += width * height;
                            blamBitmap.MipMapCount++;
                        }
                    }
                }

                //Read the raw from the dds container
                byte[] raw = new byte[dataSize];
                ddsStream.Read(raw, 0, dataSize);
                result = raw;

            }

            if (File.Exists(tempBitmap))
                File.Delete(tempBitmap);
           
            return result;
        }

        private byte[] ConvertBlamBitmapData(byte[] raw, BlamBitmap blamBitmap)
        {
            byte[] result = new byte[0];

            BitmapFormat targetFormat = BitmapFormat.Dxt1;
            bool compress = false;
            bool noMips = false;
            bool genMips = false;

            //Convert original bitmap to usable format
            switch (blamBitmap.Format)
            {
                //Converted to Y8
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, BlamCache.Version);
                    blamBitmap.Format = BitmapFormat.Y8;
                    blamBitmap.Image.Format = BitmapFormat.Y8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);
                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.CompressionFactor = 1;
                    compress = false;
                    noMips = false;
                    genMips = true;
                    break;

                //Converted to A8
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt5aAlpha:
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, BlamCache.Version);
                    blamBitmap.Format = BitmapFormat.A8;
                    blamBitmap.Image.Format = BitmapFormat.A8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);

                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.CompressionFactor = 1;

                    compress = false;
                    noMips = false;
                    genMips = true;
                    break;
                //Converted to A8Y8
                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, BlamCache.Version);
                    blamBitmap.Format = BitmapFormat.A8Y8;
                    blamBitmap.Image.Format = BitmapFormat.A8Y8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);

                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.CompressionFactor = 0.5;
                    compress = false;
                    noMips = false;
                    genMips = true;
                    break;

                //Convert to A8R8G8B8 for ease
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.R5G6B5:
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, CacheVersion.Halo3Retail);
                    blamBitmap.Image.Format = BitmapFormat.A8R8G8B8;
                    blamBitmap.Format = BitmapFormat.A8R8G8B8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Image.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);

                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.CompressionFactor = 0.25;
                    
                    compress = false;
                    genMips = true;
                    break;

                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A32B32G32R32F:
                    if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                        raw = DxtDecoder.ConvertToLinearTexture(raw, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Image.Format);
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.MipMapCount = 0;

                    //requires conversion
                    if (blamBitmap.Convert)
                    {
                        result = new byte[(int)(blamBitmap.Width * blamBitmap.Height / blamBitmap.CompressionFactor)];
                        for (int j = 0; j < blamBitmap.Height; j++)
                        {
                            Array.Copy(raw, (int)(j * blamBitmap.VirtualWidth / blamBitmap.CompressionFactor), result, (int)(j * blamBitmap.Width / blamBitmap.CompressionFactor), (int)(blamBitmap.Width / blamBitmap.CompressionFactor));
                        }
                        raw = result;
                    }
                    blamBitmap.RawSize = raw.Length;

                    compress = false;
                    genMips = false;
                    blamBitmap.Convert = false;
                    blamBitmap.Reformat = false;
                    break;

                //Generate mipmaps if required, keep format
                case BitmapFormat.AY8:
                case BitmapFormat.A8Y8:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.A8R8G8B8:
                    if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                        raw = DxtDecoder.ConvertToLinearTexture(raw, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Image.Format);

                    if(blamBitmap.Format == BitmapFormat.A8R8G8B8)
                        for (int j = 0; j < raw.Length; j += 4)
                            Array.Reverse(raw, j, 4);

                    blamBitmap.RawSize = raw.Length;
                    compress = false;
                    noMips = true;
                    genMips = true;
                    break;


                //Decompress, compress with mipmaps.
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    targetFormat = blamBitmap.Format;
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, CacheVersion.Halo3Retail);
                    blamBitmap.Image.Format = BitmapFormat.A8R8G8B8;
                    blamBitmap.Format = BitmapFormat.A8R8G8B8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Image.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);
                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = raw.Length;
                    blamBitmap.CompressionFactor = 0.25;

                    if (blamBitmap.MipMapCount == 0)
                        noMips = true;

                    compress = true;
                    break;

                case BitmapFormat.Ctx1:
                    targetFormat = BitmapFormat.Dxn;
                    raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, CacheVersion.Halo3Retail);
                    blamBitmap.Image.Format = BitmapFormat.A8R8G8B8;
                    blamBitmap.Format = BitmapFormat.A8R8G8B8;
                    raw = DxtDecoder.EncodeBitmap(raw, blamBitmap.Image.Format, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight);
                    blamBitmap.BlockSize = 1;
                    blamBitmap.RawSize = Math.Max(blamBitmap.PixelCount * 4, 4);
                    blamBitmap.CompressionFactor = 0.25;

                    if (blamBitmap.MipMapCount == 0)
                        noMips = true;

                    blamBitmap.MipMapCount = 0;
                    compress = true;
                    break;

            }

            //Remove flags for conversion
            blamBitmap.Image.XboxFlags &= ~BitmapFlagsXbox.Xbox360ByteOrder;
            blamBitmap.Image.XboxFlags &= ~BitmapFlagsXbox.TiledTexture;

            //Flip byte ordering for regular-sized bitmaps
            if (!blamBitmap.Reformat && !blamBitmap.Convert)
            {
                if (blamBitmap.Format == BitmapFormat.A8 || blamBitmap.Format == BitmapFormat.A8Y8 || blamBitmap.Format == BitmapFormat.Y8 || blamBitmap.Format == BitmapFormat.A8R8G8B8)
                {
                    //No conversion
                }
                else
                    for (int j = 0; j < raw.Length; j += 2)
                        Array.Reverse(raw, j, 2);
            }

            //Rearrange pixels for odd-sized bitmaps
            if (blamBitmap.Reformat || blamBitmap.Convert)
            {

                var prevFormat = blamBitmap.Format;

                raw = DxtDecoder.DecodeBitmap(raw, blamBitmap.Image, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, BlamCache.Version);
                blamBitmap.Format = BitmapFormat.A8R8G8B8;
                blamBitmap.Image.Format = BitmapFormat.A8R8G8B8;
                blamBitmap.BlockSize = 1;
                blamBitmap.RawSize = raw.Length;
                blamBitmap.CompressionFactor = 0.25;

                if (blamBitmap.Convert)
                {
                    result = new byte[(blamBitmap.Width * blamBitmap.Height * 4)];
                    for (int j = 0; j < blamBitmap.Height; j++)
                    {
                        Array.Copy(raw, j * blamBitmap.VirtualWidth * 4, result, j * blamBitmap.Width * 4, blamBitmap.Width * 4);
                    }
                    raw = result;
                }

                blamBitmap.VirtualHeight = blamBitmap.Height;
                blamBitmap.VirtualWidth = blamBitmap.Width;


                if (!blamBitmap.Reformat && compress == true)
                {
                    compress = true;
                    genMips = false;
                } 
                else
                {
                    compress = false;

                    if (prevFormat == BitmapFormat.A8 || prevFormat == BitmapFormat.Y8 || prevFormat == BitmapFormat.A8Y8)
                    {
                        for (int j = 0; j < raw.Length; j += 4)
                            Array.Reverse(raw, j, 4);

                        raw = DxtDecoder.EncodeBitmap(raw, prevFormat, blamBitmap.Width, blamBitmap.Height);
                        blamBitmap.RawSize = raw.Length;
                        blamBitmap.Format = prevFormat;
                        blamBitmap.Image.Format = prevFormat;

                    }
                }
            }

            //Generate mipmaps for uncompressed bitmaps
            if (genMips)
            {
                if (blamBitmap.MipMapCount != 0)
                {
                    MipMapGenerator gen = new MipMapGenerator();
                    gen.GenerateMipMap(blamBitmap.Height, blamBitmap.Width, raw, blamBitmap.MipMapCount, blamBitmap.Format);
                    raw = gen.CombineImage(raw);
                    blamBitmap.RawSize = raw.Length;
                }
            }

            //Compress bitmap if required
            if (compress)
            {
                raw = CompressBitmap(blamBitmap, targetFormat, raw, noMips);
            }

            return raw;
        }

        private byte[] ConvertBlamArrayData(byte[] raw, BlamBitmap blamBitmap)
        {
            //Shortened code for DXT5 only

            int layerSize = blamBitmap.Width * blamBitmap.Height;
            int virtualLayerSize = blamBitmap.VirtualHeight * blamBitmap.VirtualWidth;

            byte[] result = new byte[blamBitmap.Depth * layerSize];

            for (int i = 0; i < blamBitmap.Depth; i++)
            {
                byte[] layerRaw = new byte[virtualLayerSize];
                Array.Copy(raw, virtualLayerSize * i, layerRaw, 0, virtualLayerSize);

                //Apply linear fix to each layer of the array
                if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                    layerRaw = DxtDecoder.ConvertToLinearTexture(layerRaw, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Format);

                //Reverse each block of 2 bytes
                for (int j = 0; j < layerRaw.Length; j += 2)
                    Array.Reverse(layerRaw, j, 2);


                byte[] tempResult = new byte[layerSize];

                if (blamBitmap.Convert)
                {
                    int yBlocks = blamBitmap.Height / blamBitmap.BlockSize;
                    for (int j = 0; j < yBlocks; j++)
                    {
                        Array.Copy(layerRaw, j * blamBitmap.BlockSize * blamBitmap.VirtualWidth, tempResult, j * blamBitmap.BlockSize * blamBitmap.Width, blamBitmap.BlockSize * blamBitmap.Width);
                    }
                }
                else
                {
                    tempResult = layerRaw;
                }

                //Copy layer data into the resulting array
                Array.Copy(layerRaw, 0, result, i * layerSize, layerSize);
            }
            blamBitmap.Type = BitmapType.Texture3D;
            blamBitmap.Image.Type = BitmapType.Texture3D;
            return result;
        }

        private byte[] ConvertBlamCubeData(byte[] raw, BlamBitmap blamBitmap)
        {

            blamBitmap.MipMapCount = 0;

            byte[] result;
            var realSize = (int)(blamBitmap.Width * blamBitmap.Height / blamBitmap.CompressionFactor);
            if (blamBitmap.Convert)
            {
                result = new byte[6 * realSize];
            }
            else
            {
                result = new byte[6 * blamBitmap.RawSize];
            }

            bool specialSize = blamBitmap.Height >= blamBitmap.VirtualHeight / 2 || blamBitmap.Width >= blamBitmap.VirtualWidth / 2;

            for (int i = 0; i < 6; i++)
            {

                if (!blamBitmap.Convert)
                {
                    byte[] buffer = new byte[blamBitmap.RawSize];
                    Array.Copy(raw, i * blamBitmap.RawSize, buffer, 0, blamBitmap.RawSize);

                    if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                        buffer = DxtDecoder.ConvertToLinearTexture(buffer, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Format);

                    if (blamBitmap.Format != BitmapFormat.A8R8G8B8)
                    {
                        for (int j = 0; j < buffer.Length; j += 2)
                            Array.Reverse(buffer, j, 2);
                    }
                    else
                    {
                        for (int j = 0; j < buffer.Length; j += 4)
                            Array.Reverse(buffer, j, 4);
                    }


                    Array.Copy(buffer, 0, result, i * blamBitmap.RawSize, blamBitmap.RawSize);
                }
                else
                {
                    var gap = (int)(blamBitmap.Width * blamBitmap.Height / blamBitmap.CompressionFactor) * 4 + (int)((blamBitmap.MinimalBitmapSize * blamBitmap.BlockSize / 2) / blamBitmap.CompressionFactor);



                    byte[] buffer = new byte[blamBitmap.RawSize];
                    byte[] tempBuffer = new byte[blamBitmap.RawSize];

                    if (specialSize)
                    {
                        Array.Copy(raw, i * blamBitmap.RawSize, tempBuffer, 0, blamBitmap.RawSize);

                        if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            buffer = DxtDecoder.ConvertToLinearTexture(tempBuffer, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Format);
                    }
                    else
                    {
                        Array.Copy(raw, i / 2 * blamBitmap.RawSize, tempBuffer, 0, blamBitmap.RawSize);

                        if ((blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && blamBitmap.Image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            tempBuffer = DxtDecoder.ConvertToLinearTexture(tempBuffer, blamBitmap.VirtualWidth, blamBitmap.VirtualHeight, blamBitmap.Format);

                        Array.Copy(tempBuffer, (i % 2) * (gap), buffer, 0, blamBitmap.RawSize - (i % 2) * (gap));
                    }



                    if (blamBitmap.Format != BitmapFormat.A8R8G8B8)
                    {
                        for (int j = 0; j < buffer.Length; j += 2)
                            Array.Reverse(buffer, j, 2);
                    }
                    else
                    {
                        for (int j = 0; j < buffer.Length; j += 4)
                            Array.Reverse(buffer, j, 4);
                    }


                    tempBuffer = new byte[realSize];
                    var yBlocks = blamBitmap.Height / blamBitmap.BlockSize;
                    for (int j = 0; j < yBlocks; j++)
                    {
                        Array.Copy(buffer, (int)(j * blamBitmap.BlockSize * blamBitmap.VirtualWidth / blamBitmap.CompressionFactor), tempBuffer, (int)(j * blamBitmap.BlockSize * blamBitmap.Width / blamBitmap.CompressionFactor), (int)(blamBitmap.BlockSize * blamBitmap.Width / blamBitmap.CompressionFactor));
                    }


                    Array.Copy(tempBuffer, 0, result, i * realSize, realSize);
                }
            }
            return result;


        }

        private void SetTagData(BlamBitmap blamBitmap, Bitmap.Image image)
        {
            image.Width = (short) blamBitmap.Width;
            image.Height = (short) blamBitmap.Height;
            image.Depth = (sbyte) blamBitmap.Depth;
            image.Format = blamBitmap.Format;
            image.Type = blamBitmap.Type;
            image.MipmapCount = (sbyte)blamBitmap.MipMapCount;
            image.DataSize = blamBitmap.RawSize;
            image.XboxFlags = BitmapFlagsXbox.None;

            switch (blamBitmap.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    image.Flags = BitmapFlags.Compressed;
                    break;
                default:
                    image.Flags = BitmapFlags.None;
                    break;
            }

            if ((image.Width & (image.Width - 1)) == 0 && (image.Height & (image.Height - 1)) == 0)
                image.Flags |= BitmapFlags.PowerOfTwoDimensions;

            if (image.Format == BitmapFormat.Dxn)
                image.Flags |= BitmapFlags.Unknown3;
 
        }

        private int GetUnusedFormat(BitmapFormat format)
        {
            int result = 0;
            switch (format)
            {
                case BitmapFormat.A8:
                    result = 0x0000001C;
                    break;

                case BitmapFormat.Y8:
                    result = 0x00000032;
                    break;

                case BitmapFormat.A8Y8:
                    result = 0x00000033;
                    break;

                case BitmapFormat.A8R8G8B8:
                    result = 0x00000015;
                    break;

                case BitmapFormat.A16B16G16R16F:
                    result = 0x00000071;
                    break;

                case BitmapFormat.A32B32G32R32F:
                    result = 0x00000074;
                    break;

                case BitmapFormat.Dxt1:
                    result = (int)DdsFourCc.FromString("DXT1");
                    break;

                case BitmapFormat.Dxt3:
                    result = (int)DdsFourCc.FromString("DXT3");
                    break;

                case BitmapFormat.Dxt5:
                    result = (int)DdsFourCc.FromString("DXT5");
                    break;

                case BitmapFormat.Dxn:
                    result = (int)DdsFourCc.FromString("ATI2");
                    break;

            }

            return result;
        }
    }
}