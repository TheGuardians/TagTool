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

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Reach");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();

            var mapFiles = mapFilesFolder.GetFiles("*.map");

            //
            // Insert what test command you want below
            //

            var newStringTable = new StringTableHaloOnline(CacheVersion.HaloOnline106708, null);

            for(int i = 0; i < newStringTable.Count; i++)
            {
                var cacheString = Cache.StringTable[i];
                var newString = newStringTable[i];
                if (newString != cacheString)
                {
                    var test = 1;
                }
            }


            return true;
            if (Cache.StringTable != null)
            {
                using (var fileStream = new FileInfo($"stringids\\ms23.txt").CreateText())
                {
                    var resolver = Cache.StringTable.Resolver;
                    for (int i = 0; i < Cache.StringTable.Count; i++)
                    {
                        var stringId = Cache.StringTable.GetStringId(i);
                        var set = resolver.GetSet(stringId);
                        var index = resolver.GetIndex(stringId);
                        fileStream.WriteLine($"{i:X8}, {set:X2}, {index:X4}, {Cache.StringTable[i]}");

                    }
                }
            }

            return true;
            foreach(var mapFile in mapFiles)
            {
                var cache = GameCache.Open(mapFile);
                if(cache.StringTable != null)
                {
                    using (var fileStream = new FileInfo($"stringids\\reach\\{mapFile.Name}.txt").CreateText())
                    {
                        var resolver = cache.StringTable.Resolver;
                        for (int i = 0; i < cache.StringTable.Count; i++)
                        {
                            var stringId = cache.StringTable.GetStringId(i);
                            var set = resolver.GetSet(stringId);
                            var index = resolver.GetIndex(stringId);
                            fileStream.WriteLine($"{i:X8}, {set:X2}, {index:X4}, {cache.StringTable[i]}");

                        }
                    }
                }
            }

            /*
            using(var fileStream = new FileInfo("ms23set0.txt").CreateText())
            {
                var resolver = Cache.StringTable.Resolver;
                for (int i = 0; i < Cache.StringTable.Count; i++)
                {
                    var stringId = Cache.StringTable.GetStringId(i);
                    var set = resolver.GetSet(stringId);
                    var index = resolver.GetIndex(stringId);
                    fileStream.WriteLine($"{i:X8}, {set:X2}, {index:X4}, {Cache.StringTable[i]}");

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

