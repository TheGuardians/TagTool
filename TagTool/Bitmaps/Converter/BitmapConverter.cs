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
            int mipMapOffset = 0;

            var image = bitmapTag.Images[index];
            int handle = GetBitmapResourceHandle(bitmapTag, index, version);

            if (!ResourceEntryValid(cache, handle))
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

                //temp
                mipMapSize = bitmapSize;

                xboxBitmap.Offset = image.InterleavedTextureIndex2 * (int)(0.25 * xboxBitmap.MinimalBitmapSize * xboxBitmap.MinimalBitmapSize / xboxBitmap.CompressionFactor + 0.5 * xboxBitmap.TilePitch);
                mipMapOffset = bitmapSize;

                if (!xboxBitmap.InTile)
                {
                    xboxBitmap.Offset = 0;
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, 0);
                    mipMapData = cache.GetPrimaryResource(handle, mipMapSize, 0);
                }
                else
                {
                    imageData = cache.GetPrimaryResource(handle, bitmapSize, 0);
                    mipMapData = cache.GetPrimaryResource(handle, mipMapSize, mipMapOffset);
                }

                

                //imageData = mipMapData;
               
            }
            else
            {
                var resourceDef = GetResourceDefinition(cache, handle);
                xboxBitmap = new XboxBitmap(resourceDef, image);
                bitmapSize = BitmapUtils.GetXboxImageSize(xboxBitmap);

                if(HasSecondaryResource(cache, handle))
                {
                    imageData = cache.GetSecondaryResource(handle, bitmapSize, 0, true);
                    if (xboxBitmap.MipMapCount > 1)
                    {
                        if(HasPrimaryResource(cache, handle))
                        {
                            // dedicated resource for mipmaps
                            mipMapData = cache.GetPrimaryResource(handle, mipMapSize, mipMapOffset);
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
                        // mipmaps are the first in that kind of bitmap, then the full texture.
                        // compute offset in the bitmap, most likely will be one that is not exact.

                        imageData = cache.GetPrimaryResource(handle, bitmapSize, 0, true);
                        mipMapData = cache.GetPrimaryResource(handle, bitmapSize, 0, true);
                        mipMapOffset = 0;

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

            List<XboxBitmap> xboxBitmaps = new List<XboxBitmap>();
            List<XboxBitmap> xboxMipMaps = new List<XboxBitmap>();

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
                    for (int i = 0; i < count; i++)
                    {
                        byte[] data = new byte[size];
                        Array.Copy(mipMapData, i * size, data, 0, size);
                        XboxBitmap newXboxBitmap = xboxBitmap.ShallowCopy();

                        if ((image.XboxFlags.HasFlag(BitmapFlagsXbox.TiledTexture) && image.XboxFlags.HasFlag(BitmapFlagsXbox.Xbox360ByteOrder)))
                            data = BitmapDecoder.ConvertToLinearTexture(data, xboxBitmap.VirtualWidth, xboxBitmap.VirtualHeight, xboxBitmap.Format);

                        newXboxBitmap.Data = data;
                        xboxMipMaps.Add(newXboxBitmap);
                    }

                    // Fix data
                    if (image.InterleavedTextureIndex2 == 1 && xboxBitmap.BlockSize == 16 && xboxBitmap.Width == 64 && xboxBitmap.Type == BitmapType.CubeMap)
                    {
                        // Very special case where the 2 bitmap in the interleaved texture is split in half, the lower half is in the mipmap data.
                        // The following code copies the lower half of the bitmap where it should be in the original image.
                        // copies 128x64 upper half into original 128x128 image in lower half
                        int tileSize = (int)(128 * 128 / xboxBitmap.CompressionFactor);
                        int halfOffset = tileSize / 2;
                        for (int i = 0; i < 6; i++)
                        {
                            Array.Copy(xboxMipMaps[i].Data, 0, xboxBitmaps[i].Data, halfOffset, halfOffset);
                        }
                        Console.WriteLine("Big oof");
                    }

                    break;
            }

            List<BaseBitmap> finalBitmaps = new List<BaseBitmap>();
            foreach (var bitmap in xboxBitmaps)
            {
                BaseBitmap finalBitmap = ExtractImage(bitmap);
                ConvertImage(finalBitmap);
                FlipImage(finalBitmap, image);
                finalBitmaps.Add(finalBitmap);
                finalBitmap.MipMapCount = 0;
            }

            return RebuildBitmap(finalBitmaps);
        }

        public static int GetBitmapResourceHandle(Bitmap bitmap, int index, CacheVersion version)
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

        public static BaseBitmap ExtractImage(XboxBitmap bitmap)
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

        public static void ConvertImage(BaseBitmap bitmap)
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

        public static void FlipImage(BaseBitmap bitmap, Bitmap.Image image)
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
                    break;

                case BitmapFormat.A8R8G8B8:
                    for (int j = 0; j < bitmap.Data.Length; j += 4)
                        Array.Reverse(bitmap.Data, j, 4);
                    break;

                default:
                    throw new Exception($"Unsupported format {image.Format} flipping");
            }
        }

        public static BaseBitmap RebuildBitmap(List<BaseBitmap> bitmaps)
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

            bitmaps[0].Data = totalData;
            return bitmaps[0];
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
