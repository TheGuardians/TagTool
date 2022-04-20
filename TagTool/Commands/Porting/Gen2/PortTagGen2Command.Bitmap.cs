using System;
using System.Collections.Generic;
using TagTool.Tags.Definitions;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;
using TagTool.Bitmaps;
using System.IO;
using TagTool.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public Bitmap ConvertBitmap(BitmapGen2 gen2Bitmap)
        {
            Bitmap newBitmap = new Bitmap
            {
                Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource,
                SpriteSpacing = gen2Bitmap.SpriteSpacing, //this seems to be a default value
                BumpMapHeight = gen2Bitmap.BumpHeight,
                FadeFactor = gen2Bitmap.DetailFadeFactor,
                Sequences = new List<Bitmap.Sequence>(),
                Images = new List<Bitmap.Image>(),
                HardwareTextures = new List<TagTool.Tags.TagResourceReference>()
            };

            //convert sequences
            foreach(var gen2seq in gen2Bitmap.Sequences)
            {
                Bitmap.Sequence newSeq = new Bitmap.Sequence
                {
                    Name = gen2seq.Name,
                    FirstBitmapIndex = gen2seq.FirstBitmapIndex,
                    BitmapCount = gen2seq.BitmapCount,
                    Sprites = new List<Bitmap.Sequence.Sprite>()
                };
                foreach(var gen2spr in gen2seq.Sprites)
                {
                    newSeq.Sprites.Add(new Bitmap.Sequence.Sprite
                    {
                        BitmapIndex = gen2spr.BitmapIndex,
                        Left = gen2spr.Left,
                        Right = gen2spr.Right,
                        Top = gen2spr.Top,
                        Bottom = gen2spr.Bottom,
                        RegistrationPoint = gen2spr.RegistrationPoint
                    });
                }
                newBitmap.Sequences.Add(newSeq);
            }

            //convert images
            foreach(var gen2Img in gen2Bitmap.Bitmaps)
            {
                Bitmap.Image newImg = new Bitmap.Image
                {
                    Signature = gen2Img.Signature,
                    Width = gen2Img.Width,
                    Height = gen2Img.Height,
                    Depth = gen2Img.Depth,
                    Type = ConvertBitmapType(gen2Img.Type),
                    Format = ConvertBitmapFormat(gen2Img.Format),
                    RegistrationPoint = gen2Img.RegistrationPoint,
                    MipmapCount = (sbyte)gen2Img.MipmapCount,
                    Flags = new BitmapFlags(),
                    Curve = BitmapImageCurve.xRGB, //default to this for now
                    PixelDataSize = (int)gen2Img.Lod0Size
                };
                if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.PowerOfTwoDimensions))
                    newImg.Flags |= BitmapFlags.PowerOfTwoDimensions;
                if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Compressed))
                    newImg.Flags |= BitmapFlags.Compressed;
                if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Linear))
                    newImg.Curve = BitmapImageCurve.Linear;

                //get raw bitmap data and create resource
                byte[] rawBitmapData = Gen2Cache.GetCacheRawData(gen2Img.Lod0Pointer, (int)gen2Img.Lod0Size);
                BaseBitmap bitmapbase = new BaseBitmap(newImg);
                bitmapbase.Data = rawBitmapData;
                var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(bitmapbase);
                var resourceReference = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
                newBitmap.HardwareTextures.Add(resourceReference);

                newBitmap.Images.Add(newImg);
            }

            return newBitmap;
        }

        private BitmapType ConvertBitmapType(BitmapGen2.BitmapDataBlock.TypeValue type)
        {
            switch (type)
            {
                case BitmapGen2.BitmapDataBlock.TypeValue._2dTexture:
                    return BitmapType.Texture2D;
                case BitmapGen2.BitmapDataBlock.TypeValue._3dTexture:
                    return BitmapType.Texture3D;
                case BitmapGen2.BitmapDataBlock.TypeValue.CubeMap:
                    return BitmapType.CubeMap;
                default:
                    return BitmapType.Texture2D;
            }
        }

        private BitmapFormat ConvertBitmapFormat(BitmapGen2.BitmapDataBlock.FormatValue format)
        {
            BitmapFormat result;
            if(Enum.TryParse(format.ToString().ToUpper(), out result) ||
                Enum.TryParse(format.ToString(), out result))
                return result;
            else
            {
                new TagToolWarning($"Failed to find bitmap format matching {format}");
                return BitmapFormat.A8R8G8B8;
            }
            /*
            switch (format)
            {
                case BitmapGen2.BitmapDataBlock.FormatValue.A8y8:
                    return BitmapFormat.A8Y8;
                case BitmapGen2.BitmapDataBlock.FormatValue.A8:
                    return BitmapFormat.A8;
                case BitmapGen2.BitmapDataBlock.FormatValue.A1r5g5b5:
                    return BitmapFormat.A1R5G5B5;
                case BitmapGen2.BitmapDataBlock.FormatValue.A4r4g4b4:
                    return BitmapFormat.A4R4G4B4;
                case BitmapGen2.BitmapDataBlock.FormatValue.A8r8g8b8:
                    return BitmapFormat.A8R8G8B8;
                case BitmapGen2.BitmapDataBlock.FormatValue.Argbfp32:
                    return BitmapFormat.ARGBFP32;
                case BitmapGen2.BitmapDataBlock.FormatValue.Ay8:
                    return BitmapFormat.AY8;
                case BitmapGen2.BitmapDataBlock.FormatValue.Dxt1:
                    return BitmapFormat.Dxt1;
                case BitmapGen2.BitmapDataBlock.FormatValue.Dxt3:
                    return BitmapFormat.Dxt3;
                case BitmapGen2.BitmapDataBlock.FormatValue.
            }
            */
        }
    }
}
