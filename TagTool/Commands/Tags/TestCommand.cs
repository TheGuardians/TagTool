using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;
using System.Linq;
using System.IO.Compression;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();

            var mapFiles = new List<FileInfo>() { new FileInfo(@"D:\halo online test\maps\tags.dat")};
            //mapFilesFolder.GetFiles("*.map"); // 

            //
            // Insert what test command you want below
            //


            var file = new FileInfo(@"D:\halo\maps\halo3\guardian.map");

            var cache = GameCache.Open(file);

            using(var stream = cache.OpenCacheRead())
            {
                var bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle", "bitm");
                var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);
                
                var resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[0]);

                var bitmapResource = resource.Texture.Definition.Bitmap;
                var primaryData = resource.Texture.Definition.PrimaryResourceData.Data;
                var secondaryData = resource.Texture.Definition.SecondaryResourceData.Data;
                byte[] data = null;
                if (bitmapResource.HighResInSecondaryResource == 1)
                {
                    data = secondaryData;
                }
                else
                {
                    data = primaryData;
                }

                var format = XboxGraphics.XGGetGpuFormat(bitmapResource.D3DFormat);
                uint blockWidth = 0;
                uint blockHeight = 0;

                XboxGraphics.XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);
                
                var bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(format);
                uint rowPitch = (uint)(((blockWidth * blockHeight * bitsPerPixel) >> 3) * bitmapResource.Width);


                byte[] result = XboxGraphics.XGUntileTextureLevel((uint)bitmapResource.Width, (uint)bitmapResource.Height, 0, format, XboxGraphics.XGTILE.NONE, rowPitch, null, data, null);

            }



            /*
            foreach (var mapFile in mapFiles)
            {
                var cache = GameCache.Open(mapFile);

                using(var stream = cache.OpenCacheRead())
                {
                    foreach(var tag in cache.TagCache.NonNull())
                    {
                        if (tag.IsInGroup("bitm"))
                        {
                            var bitmap = cache.Deserialize<Bitmap>(stream, tag);
                            foreach(var resource in bitmap.Resources)
                            {
                                var bitmapResource = cache.ResourceCache.GetBitmapTextureInteropResource(resource);
                                if(bitmapResource != null)
                                {
                                    Console.WriteLine($"{XboxGraphics.XGGetGpuFormat(bitmapResource.Texture.Definition.Bitmap.D3DFormat)}");
                                }
                            }
                        }
                    }
                }
            }
            */







            return true;
        }

        public void PrintStringID(int set, int index, int tableIndex, string str, StringIDType type)
        {
            Console.WriteLine($"{tableIndex:X8}, {set:X2}, {index:X4}, {type.ToString()}, {str}");
        }

        public void PrintEnum(int index, string str)
        {
            Console.WriteLine($"string_id_{str} = 0x{index:X4},");
        }
    }



}

