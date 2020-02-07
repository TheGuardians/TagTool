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

            var map = new MapFile();
            using(var stream = new FileInfo(@"D:\halo\maps\haloxbox\bloodgulch.map").OpenRead())
            using(var reader = new EndianReader(stream))
            using(var compressedStream = new MemoryStream())
            using(var uncompressedStream = new MemoryStream())
                
            {
                map.Read(reader);
                var compressedSize = stream.Length - 0x800;
                var uncompressedSize = map.Header.FileLength - 0x800;
                stream.Position = 0x800;
                StreamUtil.Copy(stream, compressedStream, compressedSize);
                using (var decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress, true))
                {
                    var test = new byte[uncompressedSize];
                    decompressionStream.Read(test, 0, uncompressedSize);
                    decompressionStream.CopyTo(uncompressedStream);
                    compressedStream.Flush();
                    compressedStream.Close();
                }

            }

            


            //var cache = GameCache.Open(new FileInfo(@"D:\halo\maps\haloxbox\bloodgulch.map"));
            Console.WriteLine("Success!");

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

