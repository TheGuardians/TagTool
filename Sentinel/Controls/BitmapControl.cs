using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagTool.Bitmaps;
using TagTool.Cache;
using System.IO;

namespace Sentinel.Controls
{
    public partial class BitmapControl : UserControl
    {
        public GameCache Cache { get; }
        public TagTool.Tags.Definitions.Bitmap Bitmap { get; }

        public BitmapControl()
        {
            InitializeComponent();
        }

        public BitmapControl(GameCache cache, TagTool.Tags.Definitions.Bitmap bitmap) :
            this()
        {
            Cache = cache;
            Bitmap = bitmap;

            Bitmap result = null;
            var transparent = false;

            // Create a DevIL image "name" (which is actually a number)
            int img_name;
            try { DevIL.ilGenImages(1, out img_name); } catch { return; }

            DevIL.ilBindImage(img_name);

            var ddsData = BitmapExtractor.ExtractBitmapToDDSArray(cache, Bitmap, 0);

            // Load the DDS file into the bound DevIL image
            DevIL.ilLoadL(DevIL.IL_DDS, ddsData, ddsData.Length);

            // Set a few size variables that will simplify later code

            int width = DevIL.ilGetInteger(DevIL.IL_IMAGE_WIDTH);
            int height = DevIL.ilGetInteger(DevIL.IL_IMAGE_HEIGHT);
            var rect = new System.Drawing.Rectangle(0, 0, width, height);

            pictureBox1.Size = new Size(Math.Min(512, width), Math.Min(512, height));
            //imagesComboBox.Width = pictureBox1.Width;

            // Convert the DevIL image to a pixel byte array to copy into Bitmap
            DevIL.ilConvertImage(DevIL.IL_BGRA, DevIL.IL_UNSIGNED_BYTE);

            // Create a Bitmap to copy the image into, and prepare it to get data
            result = new Bitmap(width, height);
            var pixelData = result.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Copy the pixel byte array from the DevIL image to the Bitmap
            DevIL.ilCopyPixels(0, 0, 0,
              DevIL.ilGetInteger(DevIL.IL_IMAGE_WIDTH),
              DevIL.ilGetInteger(DevIL.IL_IMAGE_HEIGHT),
              1, DevIL.IL_BGRA, DevIL.IL_UNSIGNED_BYTE,
              pixelData.Scan0);

            // Clean up and return Bitmap
            DevIL.ilDeleteImages(1, ref img_name);
            result.UnlockBits(pixelData);

            var format = Bitmap.Images[0].Format;

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

            pictureBox1.Image = result;
        }
    }
}
