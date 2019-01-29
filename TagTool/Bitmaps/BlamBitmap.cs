using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Resources;

/*  
 * The BlamBitmap class is used to create a general format that is used for conversion.  It contains a list of mipmap which are only referenced by their size, offset and a boolean for 
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

    public Bitmap.Image Image;

    /// <summary>
    /// Constructor for a BlamBitmap using a Bitmap.Image from a tag definition.
    /// </summary>
    public BlamBitmap(Bitmap.Image image)
    {
        Width = image.Width;
        Height = image.Height;
        Depth = image.Depth;
        MipMapCount = image.MipmapCount;
        Format = image.Format;
        Image = image;
        Type = image.Type;

        MinimalBitmapSize = BitmapFormatUtils.GetMinimalVirtualSize(Format);

        VirtualWidth = BitmapUtils.GetVirtualSize(Width, MinimalBitmapSize);
        VirtualHeight = BitmapUtils.GetVirtualSize(Height, MinimalBitmapSize);

        CompressionFactor = BitmapFormatUtils.GetCompressionFactor(Format);

        BlockSize = BitmapFormatUtils.GetBlockDimension(Format);

        PixelCount = VirtualHeight * VirtualWidth;

        Reformat = Width % BlockSize != 0 || Height % BlockSize != 0;

        Convert = Width != VirtualWidth || Height != VirtualHeight;

        RawSize = BitmapUtils.GetXboxImageSize(image);
    }
 
}
public class BaseBitmap
{
    public int Height;
    public int Width;
    public int Depth;
    public int MipMapCount;
    public BitmapFormat Format;
    public int BlockSize;
    public int BlockDimension;
    public double CompressionFactor;
    public BitmapType Type;
    public BitmapFlags Flags;
    public byte[] Data;

    public int NearestHeight;
    public int NearestWidth;

    public BaseBitmap(Bitmap.Image image)
    {
        Height = image.Height;
        Width = image.Width;
        Depth = image.Depth;
        MipMapCount = image.MipmapCount;
        Type = image.Type;
        Flags = image.Flags;
        UpdateFormat(image.Format);
    }

    public BaseBitmap(BitmapTextureInteropResource definition, Bitmap.Image image)
    {
        var def = definition.Texture.Definition;
        Height = def.Height;
        Width = def.Width;
        Depth = def.Depth;
        MipMapCount = def.MipmapCount;
        Type = def.Type;
        Flags = image.Flags;
        UpdateFormat(image.Format);
    }

    public BaseBitmap(BitmapTextureInterleavedInteropResource definition, int index, Bitmap.Image image)
    {
        var def = definition.Texture.Definition;
        if(index == 0)
        {
            Height = def.Height1;
            Width = def.Width1;
            Depth = def.Depth1;
            MipMapCount = def.MipmapCount1;
            Type = def.Type1;
            Flags = image.Flags;
            UpdateFormat(image.Format);
        }
        else
        {
            Height = def.Height2;
            Width = def.Width2;
            Depth = def.Depth2;
            MipMapCount = def.MipmapCount2;
            Type = def.Type2;
            Flags = image.Flags;
            UpdateFormat(image.Format);
        }
    }

    public void UpdateFormat(BitmapFormat format)
    {
        Format = format;
        BlockSize = BitmapFormatUtils.GetBlockSize(Format);
        BlockDimension = BitmapFormatUtils.GetBlockDimension(Format);
        CompressionFactor = BitmapFormatUtils.GetCompressionFactor(Format);
        NearestHeight = BlockDimension * ((Height + (BlockDimension - 1)) / BlockDimension);
        NearestWidth = BlockDimension * ((Width + (BlockDimension - 1)) / BlockDimension);
    }
}

public class XboxBitmap : BaseBitmap
{
    public int VirtualHeight;
    public int VirtualWidth;
    public int TilePitch;
    public int Pitch;
    public int MinimalBitmapSize;
    public int Offset;
    public bool MultipleOfBlockDimension;
    public bool InTile;
    public bool NotExact;

    public XboxBitmap(Bitmap.Image image) : base(image)
    {
        UpdateFormat(image.Format);
        MultipleOfBlockDimension = Width % BlockDimension == 0 && Height % BlockDimension == 0;
        NotExact = Width != VirtualWidth || Height != VirtualHeight;
        InTile = Width <= MinimalBitmapSize / 2 && Height <= MinimalBitmapSize / 2;
        Offset = 0;
    }

    public XboxBitmap(BitmapTextureInteropResource definition, Bitmap.Image image) : base(definition, image)
    {
        UpdateFormat(image.Format);
        MultipleOfBlockDimension = Width % BlockDimension == 0 && Height % BlockDimension == 0;
        NotExact = Width != VirtualWidth || Height != VirtualHeight;
        InTile = Width <= MinimalBitmapSize / 2 && Height <= MinimalBitmapSize / 2;
        Offset = 0;

    }

    public XboxBitmap(BitmapTextureInterleavedInteropResource definition, int index, Bitmap.Image image) : base(definition, index, image)
    {
        UpdateFormat(image.Format);
        MultipleOfBlockDimension = Width % BlockDimension == 0 && Height % BlockDimension == 0;
        NotExact = Width != VirtualWidth || Height != VirtualHeight;
        InTile = Width <= MinimalBitmapSize / 2 && Height <= MinimalBitmapSize / 2;
        Offset = 0;
    }

    public new void UpdateFormat(BitmapFormat format)
    {
        Format = format;
        BlockSize = BitmapFormatUtils.GetBlockSize(Format);
        BlockDimension = BitmapFormatUtils.GetBlockDimension(Format);
        CompressionFactor = BitmapFormatUtils.GetCompressionFactor(Format);
        MinimalBitmapSize = BitmapFormatUtils.GetMinimalVirtualSize(Format);
        VirtualWidth = BitmapUtils.GetVirtualSize(Width, MinimalBitmapSize);
        VirtualHeight = BitmapUtils.GetVirtualSize(Height, MinimalBitmapSize);
        TilePitch = (int)(VirtualWidth * BlockDimension / CompressionFactor);
        Pitch = (int)(NearestWidth * BlockDimension / CompressionFactor);
    }

    public XboxBitmap ShallowCopy()
    {
        return (XboxBitmap)this.MemberwiseClone();
    }
    
}
