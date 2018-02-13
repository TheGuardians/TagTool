using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

/*  
 * The BlamBitmap class is used to create a general format that is then used for conversion.  It contains a list of mipmap which are only referenced by their size, offset and a boolean for 
 * wether or not they are encased in a 32x32 or 128x128 image. They share the same properties as the blambitmap they come from. If a format change is done before the bitmap/mipmaps have been
 * extracted, then the offsets need to be updated to match the new format.
 * 
 */

/// <summary>
/// Class for general bitmap formatting
/// </summary>
public class BlamBitmap
{
    public int VirtualHeight;
    public int VirtualWidth;
    public int Height;
    public int Width;
    public int Depth;
    public int MipMapCount;
    public BitmapFormat Format;
    public int BlockSize;
    public double CompressionFactor;
    public int PixelCount;
    public BitmapType Type;

    public int RawSize;
    public int MipMapRawSize;

    public int MinimalBitmapSize;
    public int MinimalRawSize;

    public bool Reformat;
    public bool Convert;
    public List<BlamMipMap> MipMaps;

    private byte[] Raw;

    public int InterleavedOffset;

    public int ImageOffset;

    public Bitmap.Image Image;

    /// <summary>
    /// Constructor for a BlamBitmap using a Bitmap.Image from a tag definition.
    /// </summary>
    public BlamBitmap(Bitmap.Image image, int interleavedOffset, int imageOffset)
    {
        Width = image.Width;
        Height = image.Height;
        Depth = image.Depth;
        MipMapCount = image.MipmapCount;
        Format = image.Format;
        Image = image;
        ImageOffset = imageOffset;
        InterleavedOffset = interleavedOffset;
        Type = image.Type;

        //Minimal Bitmap Size
        switch (image.Format)
        {
            case BitmapFormat.A8:
            case BitmapFormat.Y8:
            case BitmapFormat.AY8:
            case BitmapFormat.A8Y8:
            case BitmapFormat.A8R8G8B8:
            case BitmapFormat.A4R4G4B4:
            case BitmapFormat.R5G6B5:
            case BitmapFormat.A16B16G16R16F:
            case BitmapFormat.A32B32G32R32F:
                MinimalBitmapSize = 32;
                break;

            default:
                MinimalBitmapSize = 128;
                break;
        }

        VirtualWidth = (Width % MinimalBitmapSize == 0) ? Width : Width + (MinimalBitmapSize - (Width % MinimalBitmapSize));
        VirtualHeight = (Height % MinimalBitmapSize == 0) ? Height : Height + (MinimalBitmapSize - (Height % MinimalBitmapSize));

        //Compression factor
        switch (image.Format)
        {
            case BitmapFormat.Ctx1:
            case BitmapFormat.Dxt1:
            case BitmapFormat.Dxt3aMono:
            case BitmapFormat.Dxt3aAlpha:
            case BitmapFormat.Dxt5a:
            case BitmapFormat.Dxt5aMono:
            case BitmapFormat.Dxt5aAlpha:
                CompressionFactor = 2;
                break;
            case BitmapFormat.A8:
            case BitmapFormat.Y8:
            case BitmapFormat.AY8:
            case BitmapFormat.Dxt3:
            case BitmapFormat.Dxt5:
            case BitmapFormat.Dxn:
            case BitmapFormat.DxnMonoAlpha:
            case BitmapFormat.A4R4G4B4Font:
                CompressionFactor = 1;
                break;
            case BitmapFormat.A4R4G4B4:
            case BitmapFormat.A1R5G5B5:
            case BitmapFormat.A8Y8:
            case BitmapFormat.V8U8:
            case BitmapFormat.R5G6B5:
                CompressionFactor = 0.5;
                break;
            case BitmapFormat.A8R8G8B8:
            case BitmapFormat.X8R8G8B8:
                CompressionFactor = 0.25;
                break;
            case BitmapFormat.A16B16G16R16F:
                CompressionFactor = 0.125;
                break;
            case BitmapFormat.A32B32G32R32F:
                CompressionFactor = 0.0625;
                break;
            default:
                CompressionFactor = 1.0;
                break;
        }

        //Block size
        switch (image.Format)
        {
            case BitmapFormat.Dxt5aMono:
            case BitmapFormat.Dxt5aAlpha:
            case BitmapFormat.Dxt1:
            case BitmapFormat.Ctx1:
            case BitmapFormat.Dxt5a:
            case BitmapFormat.Dxt3aAlpha:
            case BitmapFormat.Dxt3aMono:
                BlockSize = 4;
                break;
            case BitmapFormat.Dxt3:
            case BitmapFormat.Dxt5:
            case BitmapFormat.Dxn:
            case BitmapFormat.DxnMonoAlpha:
                BlockSize = 4;
                break;
            case BitmapFormat.AY8:
            case BitmapFormat.Y8:
                BlockSize = 1;
                break;
            case BitmapFormat.A8Y8:
                BlockSize = 1;
                break;
            case BitmapFormat.A8R8G8B8:
                BlockSize = 1;
                break;
            case BitmapFormat.A16B16G16R16F:
            case BitmapFormat.A32B32G32R32F:
                BlockSize = 1;
                break;
            default:
                BlockSize = 1;
                break;
        }

        //Pixel Count
        PixelCount = VirtualHeight * VirtualWidth;

        //Main bitmap raw size
        RawSize = (int)(PixelCount / CompressionFactor)*Depth;

        Reformat = Width % BlockSize != 0 || Height % BlockSize != 0;

        Convert = Width != VirtualWidth || Height != VirtualHeight;

        if (Reformat)
            MipMapCount = 0;

        //Mipmap raw size and list (for resource extraction)
        MipMaps = new List<BlamMipMap>();

        if (MipMapCount > 0)
        {
            for(int i = 0; i < MipMapCount; i++)
            {
                int previousHeight;
                int previousWidth;

                if (i == 0)
                {
                    previousHeight = Height;
                    previousWidth = Width;
                }
                else
                {
                    previousHeight = MipMaps.Last().Height;
                    previousWidth = MipMaps.Last().Width;
                }

                BlamMipMap mipmap = new BlamMipMap
                {
                    Height = Math.Max(1, ((previousHeight + 3) / 4)),
                    Width = Math.Max(1, ((previousWidth + 3) / 4)),
                };
                
                int boundingSize = Math.Max(mipmap.Width, mipmap.Height);

                /*
                * Each mipmap is contained in the bounding box formed by the largest side. This holds for all formats.
                * If the mipmap bounding size is less than half the minimal image size (128 or 32), then we must have a pointer 
                * to beginning of the next image. They are arranged in a staircase like fashion. The largest mipmap occupies the
                * the upper left corner, then the next one is stacked the the right, touching the upper border. At one point,
                * the images are not large enough for the compression method, say dxt compression with images less than 4x4 pixels.
                * It is not known how they are stored yet. A convenient solution would be to use the minimal image size for the last mipmaps.
                */

                if (boundingSize >= MinimalBitmapSize / 2) //Division by 2 is true for power of two bitmaps, need verification for other cases.
                {
                    //Mipmap is big enough to have its own image
                    mipmap.ImageOffset = 0;
                }
                else
                {
                    mipmap.ImageOffset = 0;
                    mipmap.Encased = true;
                }



                MipMaps.Add(mipmap);
            }

            for(int j = 0; j < MipMapCount; j++)
            {
                var current = MipMaps[j];
                var h = current.Height;
                var w = current.Width;

                if (h < MinimalBitmapSize && w < MinimalBitmapSize)
                {
                    MipMapRawSize += (int)(MinimalBitmapSize*MinimalBitmapSize / CompressionFactor);
                    break;
                }
                else
                {
                    if (h < MinimalBitmapSize)
                        h = MinimalBitmapSize;
                    if (w < MinimalBitmapSize)
                        w = MinimalBitmapSize;
                    MipMapRawSize += (int)(h * w / CompressionFactor);
                }
            }
        }
        else
            MinimalRawSize = -1;
            
    }

    /// <summary>
    /// Set raw resource.
    /// </summary>
    public void SetRaw(byte[] raw)
    {
        Raw = raw;
    }

}/// <summary>
/// Class for lightmaps
/// </summary>
public class BlamLightmap
{
    public List<BlamBitmap> Maps;
}/// <summary>
/// Class for cubemaps
/// </summary>
public class BlamCubemap
{
    public List<BlamBitmap> Sides;
}/// <summary>
/// Class for mipmaps
/// </summary>
public class BlamMipMap
{
    public int Width;
    public int Height;
    public bool Encased;
    public int ImageOffset;
}
