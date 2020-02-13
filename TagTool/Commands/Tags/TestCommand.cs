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
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\color_black", "bitm");
                var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);
                
                var resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[0]);

                var bitmapResource = resource.Texture.Definition.Bitmap;
                var primaryData = resource.Texture.Definition.PrimaryResourceData.Data;
                var secondaryData = resource.Texture.Definition.SecondaryResourceData.Data;

                var filename = bitmapTag.Name.Split('\\').Last();

                generateXboxFiles(filename, primaryData, secondaryData, bitmapResource);

                return true;
                /*
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
                var point = new XboxGraphics.XGPOINT();

                var test = XboxGraphics.GetMipTailLevelOffsetCoords((uint)bitmapResource.Width, (uint)bitmapResource.Height, (uint)bitmapResource.Depth, 2 , format, true, false, point);
                byte[] result = XboxGraphics.XGUntileTextureLevel((uint)bitmapResource.Width, (uint)bitmapResource.Height, 0, format, XboxGraphics.XGTILE.NONE, rowPitch, point, data, null);
                */
            }

            return true;
        }

        public void generateXboxFiles(string filename, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition)
        {
            using(var stream = new FileStream($"XboxBitmaps/{filename}.primary", FileMode.Create))
            {
                stream.Write(primaryData, 0, primaryData.Length);
            }

            using (var stream = new FileStream($"XboxBitmaps/{filename}.secondary", FileMode.Create))
            {
                stream.Write(secondaryData, 0, secondaryData.Length);
            }

            var serializer = new TagSerializer(CacheVersion.HaloOnline106708, EndianFormat.LittleEndian);

            using (var stream = new FileStream($"XboxBitmaps/{filename}.bitmapdefinition", FileMode.Create))
            using(var writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                var context = new DataSerializationContext(writer);
                serializer.Serialize(context, definition);
            }
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

