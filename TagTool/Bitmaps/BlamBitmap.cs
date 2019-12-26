using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Resources;

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
        var def = definition.Texture.Definition.Bitmap;
        Height = def.Height;
        Width = def.Width;
        Depth = def.Depth;
        MipMapCount = def.MipmapCount - 1;
        Type = def.BitmapType;
        Flags = image.Flags;
        UpdateFormat(image.Format);
    }

    public BaseBitmap(BitmapTextureInterleavedInteropResource definition, int index, Bitmap.Image image)
    {
        
        if(index == 0)
        {
            var def = definition.Texture.Definition.Bitmap1;
            Height = def.Height;
            Width = def.Width;
            Depth = def.Depth;
            MipMapCount = def.MipmapCount - 1;
            Type = def.BitmapType;
            Flags = image.Flags;
            UpdateFormat(image.Format);
        }
        else
        {
            var def = definition.Texture.Definition.Bitmap2;
            Height = def.Height;
            Width = def.Width;
            Depth = def.Depth;
            MipMapCount = def.MipmapCount - 1;
            Type = def.BitmapType;
            Flags = image.Flags;
            UpdateFormat(image.Format);
        }
    }

    public BaseBitmap(BaseBitmap bitmap)
    {
        Height = bitmap.Height;
        Width = bitmap.Width;
        Depth = bitmap.Depth;
        MipMapCount = bitmap.MipMapCount;
        Type = bitmap.Type;
        Flags = bitmap.Flags;
        UpdateFormat(bitmap.Format);
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

    public XboxBitmap(XboxBitmap bitmap) : base(bitmap)
    {
        UpdateFormat(bitmap.Format);
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
        NearestHeight = BlockDimension * ((Height + (BlockDimension - 1)) / BlockDimension);
        NearestWidth = BlockDimension * ((Width + (BlockDimension - 1)) / BlockDimension);
    }

    public XboxBitmap ShallowCopy()
    {
        return (XboxBitmap)this.MemberwiseClone();
    }
    
}

public class XboxMipMap : XboxBitmap
{
    public XboxMipMap (XboxBitmap bitmap, int width, int height, int offset, byte[] data) : base(bitmap)
    {
        Height = height;
        Width = width;
        Offset = offset;
        Data = data;
        UpdateFormat(bitmap.Format);
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
        NearestHeight = BlockDimension * ((Height + (BlockDimension - 1)) / BlockDimension);
        NearestWidth = BlockDimension * ((Width + (BlockDimension - 1)) / BlockDimension);
        TilePitch = (int)(VirtualWidth * BlockDimension / CompressionFactor);
        Pitch = (int)(NearestWidth * BlockDimension / CompressionFactor);
        MultipleOfBlockDimension = Width % BlockDimension == 0 && Height % BlockDimension == 0;
        NotExact = Width != VirtualWidth || Height != VirtualHeight;
        InTile = Width <= MinimalBitmapSize / 2 && Height <= MinimalBitmapSize / 2;
    }
}