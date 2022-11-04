using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Bitmaps;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Bitmaps.Utils;
using TagTool.BlamFile;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using TagTool.Bitmaps.DDS;
using TagTool.IO;
using System.Runtime.Remoting.Messaging;

namespace TagTool.Commands.Bitmaps
{
    internal class ReencodeBitmapCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bitmap Bitmap { get; }

        public ReencodeBitmapCommand(GameCache cache, CachedTag tag, Bitmap bitmap)
            : base(false,

                  "ReencodeBitmap",
                  "Re-encodes the current bitmap as the specified format.",

                  "ReencodeBitmap <format> [image index]",
                  $"Valid format types are: {string.Join(", ", Enum.GetNames(typeof(BitmapFormat)))}\n")
        {
            Cache = cache;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            // parse args
            if (!Enum.TryParse<BitmapFormat>(args[0], out BitmapFormat destFormat))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\" is not a valid bitmap format");
            int imageIndex = 0;
            if (args.Count > 1 && (!int.TryParse(args[1], out imageIndex) || imageIndex >= Bitmap.Images.Count))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\" is not a valid image index");

            // decode current bitmap resource

            var resourceReference = Bitmap.HardwareTextures[imageIndex];
            var resourceDefinition = Cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);

            byte[] primaryData = resourceDefinition?.Texture.Definition.PrimaryResourceData.Data;
            byte[] secondaryData = resourceDefinition?.Texture.Definition.SecondaryResourceData.Data;

            BitmapFormat currentFormat = Bitmap.Images[imageIndex].Format;
            int mipCount = Bitmap.Images[imageIndex].MipmapCount;

            byte[] bitmapData;
            using (var result = new MemoryStream())
            {
                for (int mipLevel = 0; mipLevel < mipCount; mipLevel++)
                {
                    for (int layerIndex = 0; layerIndex < Bitmap.Images[imageIndex].Depth; layerIndex++)
                    {
                        var pixelDataOffset = BitmapUtilsPC.GetTextureOffset(Bitmap.Images[imageIndex], mipLevel);
                        var pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(Bitmap.Images[imageIndex], mipLevel);

                        byte[] pixelData = new byte[pixelDataSize];
                        if (mipLevel == 0 && resourceDefinition.Texture.Definition.Bitmap.HighResInSecondaryResource > 0 || primaryData == null)
                        {
                            Array.Copy(secondaryData, pixelDataOffset, pixelData, 0, pixelData.Length);
                        }
                        else
                        {
                            if (secondaryData != null)
                                pixelDataOffset -= secondaryData.Length;
                            Array.Copy(primaryData, pixelDataOffset, pixelData, 0, pixelDataSize);
                        }

                        result.Write(pixelData, 0, pixelData.Length);
                    }
                }

                bitmapData = result.ToArray();
            }

            byte[] rawData = BitmapDecoder.DecodeBitmap(bitmapData, Bitmap.Images[imageIndex].Format, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height);
            //rawData = BitmapUtils.TrimAlignedBitmap(Bitmap.Images[imageIndex].Format, destFormat, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height, rawData);

            // re-encode the bitmap to the specified format

            BaseBitmap resultBitmap = new BaseBitmap(Bitmap.Images[imageIndex]);
            rawData = BitmapDecoder.EncodeBitmap(rawData, destFormat, resultBitmap.Width, resultBitmap.Height);

            //if (BitmapUtils.RequiresDecompression(destFormat, (uint)resultBitmap.Width, (uint)resultBitmap.Height))
            //{
            //    rawData = BitmapUtils.ConvertNonMultipleBlockSizeBitmap(rawData, (uint)resultBitmap.Width, (uint)resultBitmap.Height, destFormat);
            //}

            // update image data

            resultBitmap.UpdateFormat(destFormat);

            if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                resultBitmap.Flags &= ~BitmapFlags.Compressed;
            else
                resultBitmap.Flags |= BitmapFlags.Compressed;

            resultBitmap.Data = rawData;

            // remove lowest mips from dxn
            if (destFormat == BitmapFormat.Dxn)
            {
                if (!Direct3D.D3D9x.D3D.IsPowerOfTwo(resultBitmap.Width) || 
                    !Direct3D.D3D9x.D3D.IsPowerOfTwo(resultBitmap.Height))
                {
                    BitmapConverter.GenerateCompressedMipMaps(resultBitmap);
                }
                else
                {
                    int dataSize = SquishLib.Compressor.GetStorageRequirements(SquishLib.SquishFlags.kDxn, resultBitmap.Width, resultBitmap.Height);

                    int mipMapCount = resultBitmap.MipMapCount;
                    if (mipMapCount > 0)
                    {
                        var width = resultBitmap.Width;
                        var height = resultBitmap.Height;

                        for (mipMapCount = 0; mipMapCount < resultBitmap.MipMapCount; mipMapCount++)
                        {
                            width /= 2;
                            height /= 2;

                            if (width < 4 || height < 4)
                                break;

                            dataSize += SquishLib.Compressor.GetStorageRequirements(SquishLib.SquishFlags.kDxn, width, height);
                        }
                    }

                    resultBitmap.MipMapCount = mipMapCount;
                    byte[] raw = new byte[dataSize];
                    Array.Copy(resultBitmap.Data, raw, dataSize);
                    resultBitmap.Data = raw;
                }
            }

            BitmapUtils.SetBitmapImageData(resultBitmap, Bitmap.Images[imageIndex]);

            // write to new resource

            var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(resultBitmap);
            var newResourceReference = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);

            Bitmap.HardwareTextures.Clear();
            Bitmap.HardwareTextures.Add(newResourceReference);
            Bitmap.InterleavedHardwareTextures = new List<TagResourceReference>();

            return true;
        }
    }
}
