using Microsoft.DirectX.Direct3D;
using TagTool.Cache;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using D3DDevice = Microsoft.DirectX.Direct3D.Device;

namespace Sentinel.Render
{
    public class RenderTexture
    {
        public TagTool.Tags.Definitions.Bitmap Bitmap { get; }
        public Texture Texture { get; }

        public RenderTexture(D3DDevice device, HaloOnlineCacheContext cacheContext, TagTool.Tags.Definitions.Bitmap bitmapDefinition, int imageIndex = 0)
        {
            Bitmap = bitmapDefinition;

            var extractor = new TagTool.Bitmaps.BitmapDdsExtractor(cacheContext);
            var transparent = false;

            Bitmap bitmap = null;

            using (var ddsStream = new MemoryStream())
            {
                extractor.ExtractDds(bitmapDefinition, 0, ddsStream);
                ddsStream.Position = 0;

                // Create a DevIL image "name" (which is actually a number)
                DevIL.ilGenImages(1, out int img_name);
                DevIL.ilBindImage(img_name);

                var ddsData = ddsStream.ToArray();

                // Load the DDS file into the bound DevIL image
                DevIL.ilLoadL(DevIL.IL_DDS, ddsData, ddsData.Length);

                // Set a few size variables that will simplify later code

                int width = DevIL.ilGetInteger(DevIL.IL_IMAGE_WIDTH);
                int height = DevIL.ilGetInteger(DevIL.IL_IMAGE_HEIGHT);
                var rect = new System.Drawing.Rectangle(0, 0, width, height);

                // Convert the DevIL image to a pixel byte array to copy into Bitmap
                DevIL.ilConvertImage(DevIL.IL_BGRA, DevIL.IL_UNSIGNED_BYTE);

                // Create a Bitmap to copy the image into, and prepare it to get data
                bitmap = new Bitmap(width, height);
                var pixelData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Copy the pixel byte array from the DevIL image to the Bitmap
                DevIL.ilCopyPixels(0, 0, 0,
                  DevIL.ilGetInteger(DevIL.IL_IMAGE_WIDTH),
                  DevIL.ilGetInteger(DevIL.IL_IMAGE_HEIGHT),
                  1, DevIL.IL_BGRA, DevIL.IL_UNSIGNED_BYTE,
                  pixelData.Scan0);

                // Clean up and return Bitmap
                DevIL.ilDeleteImages(1, ref img_name);
                bitmap.UnlockBits(pixelData);

                var format = bitmapDefinition.Images[imageIndex].Format;

                if (!transparent)
                    transparent =
                        //format == BitmapFormat.Dxn ||
                        format == TagTool.Bitmaps.BitmapFormat.DxnMonoAlpha ||
                        format == TagTool.Bitmaps.BitmapFormat.A8 ||
                        format == TagTool.Bitmaps.BitmapFormat.AY8 ||
                        format == TagTool.Bitmaps.BitmapFormat.A8Y8 ||
                        format == TagTool.Bitmaps.BitmapFormat.A8R8G8B8 ||
                        format == TagTool.Bitmaps.BitmapFormat.A16B16G16R16 ||
                        format == TagTool.Bitmaps.BitmapFormat.A16B16G16R16F ||
                        format == TagTool.Bitmaps.BitmapFormat.A1R5G5B5 ||
                        format == TagTool.Bitmaps.BitmapFormat.A2R10G10B10 ||
                        format == TagTool.Bitmaps.BitmapFormat.A32B32G32R32F ||
                        format == TagTool.Bitmaps.BitmapFormat.A4R4G4B4 ||
                        format == TagTool.Bitmaps.BitmapFormat.A4R4G4B4Font;
            }

            Texture = new Texture(device, bitmap.Width, bitmap.Height, 1, Usage.AutoGenerateMipMap, Format.A8R8G8B8, Pool.Managed);

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var textureData = Texture.LockRectangle(0, LockFlags.None, out var pitch);

            unsafe
            {
                int* texturePointer = (int*)textureData.InternalData;

                for (var i = 0; i < bitmap.Height; i++)
                {
                    var bitmapLinePointer = (int*)bitmapData.Scan0 + i * (bitmapData.Stride / sizeof(int));
                    var textureLinePointer = texturePointer + i * (pitch / sizeof(int));
                    var length = bitmap.Width;

                    while (--length >= 0)
                        *textureLinePointer++ = *bitmapLinePointer++;
                }
            }

            bitmap.UnlockBits(bitmapData);
            Texture.UnlockRectangle(0);
            Texture.GenerateMipSubLevels();
            Texture.AutoGenerateFilterType = TextureFilter.Linear;
        }
    }
}