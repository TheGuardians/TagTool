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
using System.Threading.Tasks;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Dictionary<Bitmap, Task> BitmapConversionTasks = new Dictionary<Bitmap, Task>();

        class BitmapConversionResult
        {
            public Bitmap Definition;
            public List<BaseBitmap> BaseBitmaps;
        }

        public void WaitForPendingBitmapConversion()
        {
            try
            {
                Task.WaitAll(BitmapConversionTasks.Values.ToArray());
            }
            catch (AggregateException ex)
            {
                foreach (var inner in ex.InnerExceptions)
                    new TagToolError(CommandError.CustomError, inner.Message);
                throw (ex);
            }
        }

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

        private Bitmap ConvertBitmapAsync(CachedTag blamTag, Bitmap bitmap, Action<BitmapConversionResult> callback)
        {
            bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
            bitmap.UnknownB4 = 0;

            ConcurrencyLimiter.Wait();
            BitmapConversionTasks.Add(bitmap, Task.Run(() =>
            {
                try
                {
                    BitmapConversionResult result = new BitmapConversionResult();
                    result.Definition = bitmap;
                    result.BaseBitmaps = new List<BaseBitmap>();

                    for (int i = 0; i < bitmap.Images.Count; i++)
                    {
                        result.BaseBitmaps.Add(ConvertBitmapAsync(bitmap, i, blamTag.Name));
                    }

                    callback(result);
                }
                finally
                {
                    ConcurrencyLimiter.Release();
                }
            }));

            return bitmap;
        }

        private BaseBitmap ConvertBitmapAsync(Bitmap bitmap, int imageIndex, string tagName)
        {
            return BitmapConverter.ConvertGen3Bitmap(BlamCache, bitmap, imageIndex, tagName);
        }

        private Bitmap FinishConvertBitmap(BitmapConversionResult result, string tagName)
        {
            var task = BitmapConversionTasks[result.Definition];
            BitmapConversionTasks.Remove(result.Definition);

            if (!task.IsFaulted)
            {
                var bitmap = result.Definition;

                List<TagResourceReference> resources = new List<TagResourceReference>();
                for (int i = 0; i < result.BaseBitmaps.Count; i++)
                {
                    BitmapUtils.SetBitmapImageData(result.BaseBitmaps[i], bitmap.Images[i]);
                    var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(result.BaseBitmaps[i]);
                    var resourceReference = CacheContext.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
                    resources.Add(resourceReference);
                }
                bitmap.HardwareTextures = resources;

                //fixup for HO expecting 6 sequences in sensor_blips bitmap
                if (tagName == "ui\\chud\\bitmaps\\sensor_blips")
                {
                    bitmap.Sequences.Add(bitmap.Sequences[3]);
                    bitmap.Sequences.Add(bitmap.Sequences[3]);
                }

                return bitmap;
            }
            else
            {
                // rethrow the exception
                task.GetAwaiter().GetResult();
                return null;
            }
        }
    }
}