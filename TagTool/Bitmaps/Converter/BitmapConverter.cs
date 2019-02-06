using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps.Converter
{
    public static class BitmapConverter
    {
        /// <summary>
        /// Returns a byte[] containing the converted image data from the cache.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="bitmapTag"></param>
        /// <param name="index"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static BaseBitmap ConvertGen3Bitmap(CacheFile cache, Bitmap bitmapTag, int index, CacheVersion version)
        {
            if (cache.ResourceLayoutTable == null || cache.ResourceGestalt == null)
                cache.LoadResourceTags();

            byte[] imageData = null;
            byte[] mipMapData = null;
            XboxBitmap xboxBitmap = null;
            int bitmapSize = 0;
            int mipMapSize = 0;

            var image = bitmapTag.Images[index];
            int handle = GetBitmapResourceHandle(bitmapTag, index, version);

            if (!ResourceEntryValid(cache, handle) || (!HasPrimaryResource(cache, handle) && !HasSecondaryResource(cache, handle)))
            {
                Console.WriteLine($"Invalid resource entry at {handle}. No data to convert.");
                return null;
            }
                

            // interleaved means two images are inside a single resource along with the mipmaps.
            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
            {
                var resourceDef = GetInterleavedResourceDefinition(cache, handle);
                xboxBitmap = new XboxBitmap(resourceDef, image.InterleavedTextureIndex2, image);
                bitmapSize = BitmapUtils.GetXboxImageSize(xboxBitmap);
                mipMapSize = 0;

                xboxBitmap.Offset = 0;

                if (!xboxBitmap.InTile)
                {
                    var offset = image.InterleavedTextureIndex2 * (int)( xboxBitmap.VirtualHeight * xboxBitmap.VirtualWidth / xboxBitmap.CompressionFactor);
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, offset, true);
                    mipMapData = null;
                }
                else
                {
                    if(xboxBitmap.Type == BitmapType.CubeMap && image.Flags.HasFlag(BitmapFlags.Compressed) && xboxBitmap.Width <= 16)
                    {
                        xboxBitmap.Offset = (int)(16 * 4 / xboxBitmap.CompressionFactor);   // account for the mipmaps
                    }
                    imageData = cache.GetPrimaryResource(handle, bitmapSize, 0, true);
                    mipMapData = null;
                }

                if (image.InterleavedTextureIndex2 == 1 && xboxBitmap.InTile)
                {
                    byte[] totalData = null;
                    var tileSize = (int)(xboxBitmap.MinimalBitmapSize * xboxBitmap.MinimalBitmapSize / xboxBitmap.CompressionFactor);
                    var subCount = 0;
                    switch (xboxBitmap.Type)
                    {
                        case BitmapType.Texture2D:
                            subCount = 1;
                            break;
                        case BitmapType.Texture3D:
                        case BitmapType.Array:
                            subCount = xboxBitmap.Depth;
                            break;
                        case BitmapType.CubeMap:
                            subCount = 6;
                            break;
                    }
                    if (mipMapData != null)
                    {
                        totalData = new byte[bitmapSize + mipMapSize];
                        Array.Copy(imageData, 0, totalData, 0, bitmapSize);
                        Array.Copy(mipMapData, 0, totalData, bitmapSize, mipMapSize);

                    }
                    else
                    {
                         totalData = imageData;
                    }

                    for (int i = 0; i < subCount; i++)
                    {
                        // make sure to copy the right amount of data
                        var copySize = tileSize;
                        if (copySize > totalData.Length - ((tileSize * i) + tileSize / 2))
                            copySize = totalData.Length - ((tileSize * i) + tileSize / 2);
                        Array.Copy(totalData, (tileSize * i) + tileSize / 2, imageData, (tileSize * i), copySize);
                    }

                }
            }
            else
            {
                var resourceDef = GetResourceDefinition(cache, handle);
                xboxBitmap = new XboxBitmap(resourceDef, image);
                bitmapSize = BitmapUtils.GetXboxImageSize(xboxBitmap);

                if (HasSecondaryResource(cache, handle))
                {
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, 0, true);
                    
                    if (xboxBitmap.MipMapCount > 0)
                    {
                        if(HasPrimaryResource(cache, handle))
                        {
                            // dedicated resource for mipmaps
                            mipMapData = cache.GetPrimaryResource(handle, mipMapSize, 0, true);
                        }
                        else
                        {
                            throw new Exception($"Unsupported layout. Compute bitmap offset for weird bitmap.");
                        }
                    }
                    else
                        mipMapData = null;
                }
                else
                {
                    // Bitmap doesn't have a secondary resource means either no mipmaps or everything is packed in the primary resource.
                    if(xboxBitmap.MipMapCount > 0)
                    {
                        imageData = cache.GetPrimaryResource(handle, 2*bitmapSize, 0, true);
                        mipMapData = cache.GetPrimaryResource(handle, mipMapSize, 0, true);

                        // Formula seems quite complex, small hack to make it work
                        if(xboxBitmap.BlockDimension == 4)
                        {
                            if(xboxBitmap.Width >= xboxBitmap.Height)
                            {
                                xboxBitmap.Offset = 4 * (int)(BitmapUtils.RoundSize(xboxBitmap.Height, 4) * xboxBitmap.VirtualWidth / xboxBitmap.CompressionFactor);
                            }
                            else
                            {
                                xboxBitmap.Offset = 4 * (int)(BitmapUtils.RoundSize(xboxBitmap.Width / 2, 4) * xboxBitmap.BlockDimension / xboxBitmap.CompressionFactor);
                            }
                            
                        }
                        else
                        {
                            xboxBitmap.Offset = (int)(xboxBitmap.Width * 4 / xboxBitmap.CompressionFactor);
                            Console.WriteLine("WEIRD BITMAP");
                        }
                    }
                    else
                    {
                        imageData = cache.GetPrimaryResource(handle, bitmapSize, 0, true);
                        mipMapData = null;
                    }
                    
                }
            }

            //
            // Convert main bitmap
            //
            
            List<XboxBitmap> xboxBitmaps = ParseImages(xboxBitmap, image, imageData, bitmapSize);

            // rearrange cubemaps order
            if(xboxBitmap.Type == BitmapType.CubeMap)
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
                ConvertImage(finalBitmap);
                // flip data if required
                FlipImage(finalBitmap, image);
                // until I write code to move mipmaps at the end of the file, remove cubemap mipmaps
                if (xboxBitmap.Type == BitmapType.CubeMap)
                {
                    finalBitmap.MipMapCount = 0;
                }
                // generate mipmaps for uncompressed textures
                if (!finalBitmap.Flags.HasFlag(BitmapFlags.Compressed) && finalBitmap.MipMapCount > 0 )
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
                var dataWidth = 0;
                var dataHeight = 0;

                if (!bitmap.MultipleOfBlockDimension)
                {
                    dataWidth = bitmap.NearestWidth;
                    dataHeight = bitmap.NearestHeight;
                }
                else
                {
                    dataWidth = bitmap.Width;
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
     
        private static void ConvertImage(BaseBitmap bitmap)
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
                    if(bitmap.Height != bitmap.NearestHeight || bitmap.Width != bitmap.NearestWidth)
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
                if (DXTFlip)
                {
                    for (int j = 0; j < bitmap.Data.Length; j += 2)
                        Array.Reverse(bitmap.Data, j, 2);
                }

                bitmap.UpdateFormat(targetFormat);
                bitmap.Data = data;
            }

            bitmap.Data = data;
        }

        private static void FlipImage(BaseBitmap bitmap, Bitmap.Image image)
        {
            switch (image.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    if(!(bitmap.Height != bitmap.NearestHeight || bitmap.Width != bitmap.NearestWidth)) // this means it has been flipped
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

        private static int GetBitmapResourceHandle(Bitmap bitmap, int index, CacheVersion version)
        {
            var image = bitmap.Images[index];
            int handle = 0;
            int resourceIndex = 0;

            List<Bitmap.BitmapResource> resources = null;

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
            {
                resourceIndex = image.InterleavedTextureIndex1;
                switch (version)
                {
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        resources = bitmap.InterleavedResourcesOld;
                        break;
                    case CacheVersion.HaloReach:
                        resources = bitmap.InterleavedResourcesNew;
                        break;
                }
            }
            else
            {
                resourceIndex = index;
                resources = bitmap.Resources;
            }

            switch (version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    handle = resources[resourceIndex].ZoneAssetHandleOld;
                    break;
                case CacheVersion.HaloReach:
                    handle = resources[resourceIndex].ZoneAssetHandleNew;
                    break;

                default:
                    throw new Exception($"Unsupported cache version {version}");
            }

            return handle;
        }

        private static BitmapTextureInteropResource GetResourceDefinition(CacheFile cache, int handle)
        {
            var resourceEntry = cache.ResourceGestalt.TagResources[handle & ushort.MaxValue];
            var definitionData = cache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();
            BitmapTextureInteropResource definition;
            using (var definitionStream = new MemoryStream(definitionData, true))
            using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
            using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
            {
                foreach (var fixup in resourceEntry.ResourceFixups)
                {
                    var newFixup = new TagResourceGen3.ResourceFixup
                    {
                        BlockOffset = (uint)fixup.BlockOffset,
                        Address = new CacheAddress(CacheAddressType.Definition, fixup.Offset)
                    };

                    definitionStream.Position = newFixup.BlockOffset;
                    definitionWriter.Write(newFixup.Address.Value);
                }

                var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);
                definitionStream.Position = resourceEntry.DefinitionAddress.Offset;
                definition = cache.Deserializer.Deserialize<BitmapTextureInteropResource>(dataContext);
            }
            return definition;
        }

        private static BitmapTextureInterleavedInteropResource GetInterleavedResourceDefinition(CacheFile cache, int handle)
        {
            var resourceEntry = cache.ResourceGestalt.TagResources[handle & ushort.MaxValue];
            var definitionData = cache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();
            BitmapTextureInterleavedInteropResource definition;
            using (var definitionStream = new MemoryStream(definitionData, true))
            using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
            using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
            {
                foreach (var fixup in resourceEntry.ResourceFixups)
                {
                    var newFixup = new TagResourceGen3.ResourceFixup
                    {
                        BlockOffset = (uint)fixup.BlockOffset,
                        Address = new CacheAddress(CacheAddressType.Definition, fixup.Offset)
                    };

                    definitionStream.Position = newFixup.BlockOffset;
                    definitionWriter.Write(newFixup.Address.Value);
                }

                var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);
                definitionStream.Position = resourceEntry.DefinitionAddress.Offset;
                definition = cache.Deserializer.Deserialize<BitmapTextureInterleavedInteropResource>(dataContext);
            }
            return definition;
        }

        private static bool ResourceEntryValid(CacheFile cache, int handle)
        {
            var resourceEntry = cache.ResourceGestalt.TagResources[handle & ushort.MaxValue];
            if (resourceEntry.ParentTag == null || resourceEntry.FixupInformationLength == 0 || resourceEntry.SegmentIndex == -1)
                return false;
            else
                return true;
        }

        private static bool HasPrimaryResource(CacheFile cache, int handle)
        {
            var resourceEntry = cache.ResourceGestalt.TagResources[handle & ushort.MaxValue];
            var segmentIndex = resourceEntry.SegmentIndex;
            var segment = cache.ResourceLayoutTable.Segments[segmentIndex];
            return segment.RequiredPageIndex != -1;
        }

        private static bool HasSecondaryResource(CacheFile cache, int handle)
        {
            var resourceEntry = cache.ResourceGestalt.TagResources[handle & ushort.MaxValue];
            var segmentIndex = resourceEntry.SegmentIndex;
            var segment = cache.ResourceLayoutTable.Segments[segmentIndex];
            return segment.OptionalPageIndex != -1;
        }

        public static void GenerateMipMaps(BaseBitmap bitmap)
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

        public static void GenerateUncompressedMipMaps(BaseBitmap bitmap)
        {
            int channelCount = 0;
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
