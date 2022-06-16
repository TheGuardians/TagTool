using System;
using System.Collections.Generic;
using TagTool.Tags.Definitions;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;
using TagTool.Bitmaps;
using System.IO;
using TagTool.IO;
using TagTool.Commands.Common;
using System.IO.Compression;
using TagTool.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public Bitmap ConvertBitmap(BitmapGen2 gen2Bitmap)
        {
            Bitmap newBitmap = new Bitmap
            {
                Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource,
                SpriteSpacing = gen2Bitmap.SpriteSpacing,
                BumpMapHeight = gen2Bitmap.BumpHeight / 10.0f,
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
                    MipmapCount = 0, //no mipmaps for now
                    Flags = new BitmapFlags(),
                    Curve = BitmapImageCurve.xRGB, //default to this for now
                };

                if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.PowerOfTwoDimensions))
                    newImg.Flags |= BitmapFlags.PowerOfTwoDimensions;
                if (gen2Img.Flags.HasFlag(BitmapGen2.BitmapDataBlock.FlagsValue.Compressed))
                    newImg.Flags |= BitmapFlags.Compressed;

                //get raw bitmap data and create resource
                byte[] rawBitmapData = Gen2Cache.GetCacheRawData((uint)gen2Img.Lod0Pointer, gen2Img.Lod0Size);

                //h2v raw bitmap data is gz compressed
                if(Gen2Cache.Version == TagTool.Cache.CacheVersion.Halo2Vista)
                {
                    using (var stream = new MemoryStream(rawBitmapData))
                    using (var resultStream = new MemoryStream())
                    using (var zstream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        zstream.CopyTo(resultStream);
                        rawBitmapData = resultStream.ToArray();
                    }
                }

                //convert palettized formats to A8R8B8G8
                if (gen2Img.Format == BitmapGen2.BitmapDataBlock.FormatValue.P8 ||
                    gen2Img.Format == BitmapGen2.BitmapDataBlock.FormatValue.P8Bump)
                    rawBitmapData = ConvertP8BitmapData(rawBitmapData);

                //set pixel data size after decompression and modification
                newImg.PixelDataSize = rawBitmapData.Length;

                BaseBitmap bitmapbase = new BaseBitmap(newImg);
                bitmapbase.Data = rawBitmapData;
                var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(bitmapbase);
                var resourceReference = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
                newBitmap.HardwareTextures.Add(resourceReference);

                newBitmap.Images.Add(newImg);
            }

            return newBitmap;
        }

        private byte[] ConvertP8BitmapData(byte[] data)
        {
            //convert p8 palletized data to A8R8B8G8
            byte[] outputdata = new byte[(data.Length - 1024) * 4];
            using(var dataStream = new MemoryStream(data))
            using(var outStream = new MemoryStream(outputdata))
            using (var reader = new EndianReader(dataStream))
            using(var writer = new EndianWriter(outStream))
            {
                List<ArgbColor> palette = new List<ArgbColor>();
                //256 ARGB colors form the palette
                for (var i = 0; i < 256; i++)
                    palette.Add(reader.ReadArgbColor());
                while(reader.BaseStream.Position < reader.BaseStream.Length)
                    writer.Write(palette[reader.ReadByte()].GetValue());
            }
            return outputdata;
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
            if (format == BitmapGen2.BitmapDataBlock.FormatValue.P8Bump ||
                format == BitmapGen2.BitmapDataBlock.FormatValue.P8)
                return BitmapFormat.A8R8G8B8;
            else if (Enum.TryParse(format.ToString(), true, out result))
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
