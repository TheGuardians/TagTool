using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Bitmaps.Utils;

namespace TagTool.Porting
{
    partial class PortingContext
    {
        private Bitmap ConvertBitmap(CachedTag blamTag, Bitmap bitmap, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
            bitmap.UnknownB4 = 0;

            //
            // For each bitmaps, apply conversion process and create a new list of resources that will replace the one from H3.
            //

            var tagName = blamTag.Name;
            var resourceList = new List<TagResourceReference>();
            for (int i = 0; i < bitmap.Images.Count(); i++)
            {
                var resource = ConvertBitmap(bitmap, resourceStreams, i, tagName);
                if (resource == null)
                    return null;
                resourceList.Add(resource);

                //increment of Mipmapcount in BlamBitmap unnecessary for Reach
                if (BlamCache.Version >= CacheVersion.HaloReach && bitmap.Images[i].MipmapCount > 0)
                    bitmap.Images[i].MipmapCount--;
            }

            bitmap.HardwareTextures = resourceList;
            bitmap.InterleavedHardwareTextures = null;

            //fixup for HO expecting 6 sequences in sensor_blips bitmap
            if (tagName == "ui\\chud\\bitmaps\\sensor_blips")
            {
                bitmap.Sequences.Add(bitmap.Sequences[3]);
                bitmap.Sequences.Add(bitmap.Sequences[3]);
            }

            return bitmap;
        }
        
        private TagResourceReference ConvertBitmap(Bitmap bitmap, Dictionary<ResourceLocation, Stream> resourceStreams, int imageIndex, string tagName)
        {
            BaseBitmap baseBitmap = BitmapConverter.ConvertGen3Bitmap(BlamCache, bitmap, imageIndex, tagName);

            if (baseBitmap == null)
                return null;

            BitmapUtils.SetBitmapImageData(baseBitmap, bitmap.Images[imageIndex]);
            var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(baseBitmap);
            var resourceReference = CacheContext.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
            return resourceReference;
        }
    }
}