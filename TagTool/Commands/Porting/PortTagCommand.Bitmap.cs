using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Converter;
using TagTool.Bitmaps.Utils;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Bitmap ConvertBitmap(CachedTag blamTag, Bitmap bitmap, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
            bitmap.UnknownB4 = 0;

            if (BlamCache.Version == CacheVersion.HaloReach)
            {
                bitmap.TightBoundsOld = bitmap.TightBoundsNew;
                
                foreach(var image in bitmap.Images)
                {
                    // For all formats above #38 (reach DXN, CTX1, DXT3a_mono, DXT3a_alpha, DXT5a_mono, DXT5a_alpha, DXN_mono_alpha), subtract 5 to match with H3/ODST/HO enum
                    if (image.Format >= (BitmapFormat)38)
                        image.Format = image.Format - 5;
                }
            }

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
            }

            bitmap.Resources = resourceList;
            bitmap.InterleavedResources = null;

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
            var image = bitmap.Images[imageIndex];
            BaseBitmap baseBitmap = BitmapConverterNew.ConvertGen3Bitmap(BlamCache, bitmap, imageIndex);
            BitmapUtils.SetBitmapImageData(baseBitmap, bitmap.Images[imageIndex]);
            var bitmapResourceDefinition = BitmapUtils.CreateEmptyBitmapTextureInteropResource(baseBitmap);
            var resourceReference = CacheContext.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
            return resourceReference;
        }
    }
}