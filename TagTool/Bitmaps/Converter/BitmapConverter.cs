using System;
using System.Collections.Generic;
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
                mipMapSize = BitmapUtils.GetXboxMipMapSize(xboxBitmap);

                //temporary until full bitmap size is computed
                mipMapSize = bitmapSize;

                xboxBitmap.Offset = 0;
                //xboxBitmap.Offset = image.InterleavedTextureIndex2 * (int)(0.25 * xboxBitmap.MinimalBitmapSize * xboxBitmap.MinimalBitmapSize / xboxBitmap.CompressionFactor + 0.5 * xboxBitmap.TilePitch);

                if (!xboxBitmap.InTile)
                {
                    var offset = image.InterleavedTextureIndex2 * (int)( xboxBitmap.VirtualHeight * xboxBitmap.VirtualWidth / xboxBitmap.CompressionFactor);
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, offset, true);
                    if(xboxBitmap.MipMapCount > 1)
                    {
                        mipMapData = cache.GetPrimaryResource(handle, mipMapSize, 0);
                    }
                    else
                    {
                        mipMapData = null;
                    }
                }
                else
                {
                    imageData = cache.GetPrimaryResource(handle, bitmapSize, 0);
                    if(xboxBitmap.MipMapCount > 1)
                    {
                        mipMapData = cache.GetPrimaryResource(handle, mipMapSize, bitmapSize);
                    }
                    else
                    {
                        mipMapData = null;
                    }
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
                mipMapSize = BitmapUtils.GetXboxMipMapSize(xboxBitmap);

                if (HasSecondaryResource(cache, handle))
                {
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, 0, true);
                    /*
                    imageData = cache.GetPrimaryResource(handle, mipMapSize, 114688, true);
                    xboxBitmap.MipMapCount = 0;
                    xboxBitmap.Width = 128;
                    xboxBitmap.Height = 128;
                    xboxBitmap.UpdateFormat(xboxBitmap.Format);
                    */
                    if (xboxBitmap.MipMapCount > 1)
                    {
                        if(HasPrimaryResource(cache, handle))
                        {
                            // dedicated resource for mipmaps
                            mipMapData = cache.GetPrimaryResource(handle, mipMapSize);
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
                    if(xboxBitmap.MipMapCount > 1)
                    {
                        imageData = cache.GetPrimaryResource(handle, bitmapSize, 0, true);
                        mipMapData = cache.GetPrimaryResource(handle, mipMapSize, 0, true);

                        // Formula seems quite complex, small hack to make it work
                        if(xboxBitmap.BlockDimension == 4)
                        {
                            xboxBitmap.Offset = xboxBitmap.TilePitch * xboxBitmap.BlockDimension;
                        }
                        else
                        {
                            xboxBitmap.Offset = (int)(xboxBitmap.Width * 4 / xboxBitmap.CompressionFactor);
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
            List<BaseBitmap> finalBitmaps = new List<BaseBitmap>();
            foreach (var bitmap in xboxBitmaps)
            {
                BaseBitmap finalBitmap = ExtractImage(bitmap);
                ConvertImage(finalBitmap);
                FlipImage(finalBitmap, image);
                finalBitmaps.Add(finalBitmap);
            }

            //
            // Convert mipmaps
            //

            List<XboxMipMap> xboxMipMaps = new List<XboxMipMap>();
            List<BaseBitmap> finalMipMaps = new List<BaseBitmap>();
            if(xboxBitmap.MipMapCount > 1)
            {
                xboxMipMaps = ParseMipMaps(xboxBitmap, image, mipMapData, mipMapSize);
                foreach(var mipmap in xboxMipMaps)
                {
                    BaseBitmap finalMipMap = ExtractImage(mipmap);
                    ConvertImage(finalMipMap);
                    FlipImage(finalMipMap, image);
                    finalMipMaps.Add(finalMipMap); 
                }
            }
            else
            {
                finalMipMaps = null;
            }
            return RebuildBitmap(finalBitmaps, finalMipMaps);
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

        private static List<XboxMipMap> ParseMipMaps(XboxBitmap xboxBitmap, Bitmap.Image image, byte[] mipMapData, int mipMapSize)
        {
            List<XboxMipMap> mipMaps = new List<XboxMipMap>();
            var imageCount = 0;
            switch (image.Type)
            {
                case BitmapType.Texture2D:
                    imageCount = 1;
                    break;
                case BitmapType.Texture3D:
                case BitmapType.Array:
                    imageCount = xboxBitmap.Depth;
                    break;
                case BitmapType.CubeMap:
                    imageCount = 6;
                    break;
            }

            var singleSize = mipMapSize / imageCount;

            for(int i = 0; i < imageCount; i++)
            {
                byte[] data = new byte[singleSize];
                Array.Copy(mipMapData, (i * singleSize), data, 0, singleSize);

                // split mip maps into sub images for conversion

                var prevWidth = xboxBitmap.Width;
                var prevHeight = xboxBitmap.Height;
                var mipMapCount = xboxBitmap.MipMapCount - 1;
                var mipMapDataOffset = 0;

                while (mipMapCount != 0)
                {
                    var currentMipWidth = BitmapUtils.NextNearestSize(prevWidth, xboxBitmap.BlockDimension);
                    var currentMipHeight = BitmapUtils.NextNearestSize(prevHeight, xboxBitmap.BlockDimension);
                    var minVirtualSize = xboxBitmap.MinimalBitmapSize;

                    // mips are contained in a single image, stored in reverse order. 
                    if (currentMipWidth < minVirtualSize / 4 && currentMipHeight < minVirtualSize / 4)
                    {
                        int size = (int)(minVirtualSize * minVirtualSize / xboxBitmap.CompressionFactor);
                        byte[] sharedData = new byte[size];
                        Array.Copy(mipMapData, mipMapDataOffset, sharedData, 0, size);

                        if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            sharedData = BitmapDecoder.ConvertToLinearTexture(sharedData, minVirtualSize, minVirtualSize, xboxBitmap.Format);

                        int sharedMipMapCount = mipMapCount;
                        int curOffset = (int)(minVirtualSize / 8 * xboxBitmap.BlockDimension / xboxBitmap.CompressionFactor);
                        

                        // create xboxMipMaps for each mipmap in the shared data
                        for (int j = 0; j < sharedMipMapCount; j++)
                        {
                            int offset = curOffset;

                            if (curOffset < xboxBitmap.BlockSize)
                            {
                                curOffset = (int)(minVirtualSize / 2 * xboxBitmap.BlockDimension / xboxBitmap.CompressionFactor);
                                offset = curOffset;
                                curOffset = 2 * offset;
                            }
                            else
                            {
                                curOffset -= offset / 2;
                            }

                            mipMaps.Add(new XboxMipMap(xboxBitmap, currentMipWidth, currentMipHeight, offset, sharedData));
                            prevWidth = BitmapUtils.NextNearestSize(currentMipWidth, xboxBitmap.BlockDimension);
                            prevHeight = BitmapUtils.NextNearestSize(currentMipHeight, xboxBitmap.BlockDimension);
                        }
                        break;
                    }
                    else
                    {
                        var curVirtualWidth = BitmapUtils.GetVirtualSize(currentMipWidth, xboxBitmap.MinimalBitmapSize);
                        var curVirtualHeight = BitmapUtils.GetVirtualSize(currentMipHeight, xboxBitmap.MinimalBitmapSize);

                        int size = (int)(curVirtualWidth * curVirtualHeight / xboxBitmap.CompressionFactor);
                        byte[] singleData = new byte[size];
                        Array.Copy(mipMapData, mipMapDataOffset, singleData, 0, size);

                        if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            singleData = BitmapDecoder.ConvertToLinearTexture(singleData, curVirtualWidth, curVirtualHeight, xboxBitmap.Format);

                        mipMaps.Add(new XboxMipMap(xboxBitmap, currentMipWidth, currentMipHeight, 0, singleData));
                        mipMapDataOffset += size;
                        prevWidth = currentMipWidth;
                        prevHeight = currentMipHeight;
                        mipMapCount--;
                    }
                }
            }
            return mipMaps;
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
                case BitmapFormat.A16B16G16R16F:
                case BitmapFormat.A32B32G32R32F:
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                case BitmapFormat.V8U8:
                    break;

                case BitmapFormat.Ctx1:
                    bitmap.UpdateFormat(BitmapFormat.Dxn);
                    data = BitmapDecoder.Ctx1ToDxn(data, bitmap.Width, bitmap.Height);
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
        }

        private static void FlipImage(BaseBitmap bitmap, Bitmap.Image image)
        {
            switch (image.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                    for (int j = 0; j < bitmap.Data.Length; j += 2)
                        Array.Reverse(bitmap.Data, j, 2);
                    break;

                case BitmapFormat.Dxn:
                    for (int j = 0; j < bitmap.Data.Length; j += 2)
                        Array.Reverse(bitmap.Data, j, 2);
                    BitmapDecoder.SwapXYDxn(bitmap.Data, bitmap.NearestWidth, bitmap.NearestHeight);
                    break;

                case BitmapFormat.AY8:
                case BitmapFormat.Y8:
                case BitmapFormat.A8:
                case BitmapFormat.Dxt5aMono:
                case BitmapFormat.Dxt3aMono:
                case BitmapFormat.Dxt3aAlpha:
                case BitmapFormat.Dxt5aAlpha:
                case BitmapFormat.Ctx1:
                case BitmapFormat.A8Y8:
                case BitmapFormat.DxnMonoAlpha:
                case BitmapFormat.Dxt5a:
                case BitmapFormat.R5G6B5:
                case BitmapFormat.A4R4G4B4:
                case BitmapFormat.V8U8:
                case BitmapFormat.A16B16G16R16F:
                    break;

                case BitmapFormat.A8R8G8B8:
                    for (int j = 0; j < bitmap.Data.Length; j += 4)
                        Array.Reverse(bitmap.Data, j, 4);
                    break;

                default:
                    throw new Exception($"Unsupported format {image.Format} flipping");
            }
        }

        private static BaseBitmap RebuildBitmap(List<BaseBitmap> bitmaps, List<BaseBitmap> mipMaps)
        {
            int totalSize = 0;
            if ( mipMaps != null && mipMaps.Count > 0){
                int mipCount = mipMaps.Count / bitmaps.Count;   // Each image always has the same number of mipmaps (applies to cubemaps and arrays/texture3D)

                

                for (int i = 0; i < bitmaps.Count; i++)
                {
                    totalSize += bitmaps[i].Data.Length;
                    for (int j = 0; j < mipCount; j++)
                    {
                        totalSize += mipMaps[i * mipCount + j].Data.Length;
                    }
                }

                byte[] totalData = new byte[totalSize];
                int currentPos = 0;

                for (int i = 0; i < bitmaps.Count; i++)
                {
                    var bitmap = bitmaps[i];

                    Array.Copy(bitmap.Data, 0, totalData, currentPos, bitmap.Data.Length);
                    currentPos += bitmap.Data.Length;

                    for (int j = 0; j < mipCount; j++)
                    {
                        var mipMap = mipMaps[i * mipCount + j];
                        Array.Copy(mipMap.Data, 0, totalData, currentPos, mipMap.Data.Length);
                        currentPos += mipMap.Data.Length;
                    }
                }
                bitmaps[0].Data = totalData;
                bitmaps[0].MipMapCount = mipCount;
            }
            else
            {
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
                bitmaps[0].Data = totalData;
            }

            return bitmaps[0];
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
    }
}
