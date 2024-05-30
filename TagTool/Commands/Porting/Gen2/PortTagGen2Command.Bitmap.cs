using System;
using System.Collections.Generic;
using TagTool.Tags.Definitions;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;
using TagTool.Bitmaps;
using TagTool.Bitmaps.Utils;
using System.IO;
using TagTool.IO;
using TagTool.Commands.Common;
using System.IO.Compression;
using TagTool.Common;
using System.Numerics;
using TagTool.Commands.Gen2.Bitmaps;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public Bitmap ConvertBitmap(BitmapGen2 gen2Bitmap, string gen2TagName)
        {
            Bitmap newBitmap = new Bitmap
            {
                Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource,
                SpriteSpacing = gen2Bitmap.SpriteSpacing,
                BumpMapHeight = gen2Bitmap.BumpHeight,
                FadeFactor = gen2Bitmap.DetailFadeFactor,
                Sequences = new List<Bitmap.Sequence>(),
                Images = new List<Bitmap.Image>(),
                HardwareTextures = new List<TagTool.Tags.TagResourceReference>()
            };

            //convert sequences
            foreach (var gen2seq in gen2Bitmap.Sequences)
            {
                Bitmap.Sequence newSeq = new Bitmap.Sequence
                {
                    Name = gen2seq.Name,
                    FirstBitmapIndex = gen2seq.FirstBitmapIndex,
                    BitmapCount = gen2seq.BitmapCount,
                    Sprites = new List<Bitmap.Sequence.Sprite>()
                };
                foreach (var gen2spr in gen2seq.Sprites)
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
            foreach (var gen2Img in gen2Bitmap.Bitmaps)
            {
                byte[] rawBitmapData = BitmapConverterGen2.ConvertBitmapData(Gen2Cache, gen2Bitmap, gen2Img);
                Bitmap.Image newImg = BitmapConverterGen2.ConvertBitmapImage(Gen2Cache, gen2Img, rawBitmapData);
                BaseBitmap bitmapbase = new BaseBitmap(newImg);
                bitmapbase.Data = rawBitmapData;

                BitmapConverterGen2.PostprocessBitmap(bitmapbase, gen2Bitmap, newImg, gen2TagName);

                var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(bitmapbase);
                var resourceReference = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);
                newBitmap.HardwareTextures.Add(resourceReference);

                newBitmap.Images.Add(newImg);
            }

            //set scope mask data to make shit work
            if (gen2TagName.Contains("scope_masks"))
            {
                float newLeft = 1.325f;
                float newRight = 0.125f;
                float aspectRatio = (float)newBitmap.Images[0].Height / newBitmap.Images[0].Width;
                if (aspectRatio < 0.9f)
                {
                    float newModifier = aspectRatio * 0.18f;
                    newLeft -= newModifier;
                    newRight += (newModifier / 3);
                }
                newBitmap.SpriteSpacing = -80;
                newBitmap.Sequences[0].Sprites.Add(new Bitmap.Sequence.Sprite
                {
                    BitmapIndex = gen2Bitmap.Sequences[0].FirstBitmapIndex,
                    Left = newLeft,
                    Right = newRight,
                    Top = 1.325f,
                    Bottom = 0.1f,
                    RegistrationPoint = new RealPoint2d { X = 0, Y = 0 },
                });
            }
            return newBitmap;
        }
    }
}
