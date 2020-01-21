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
using TagTool.Strings;

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

            /*
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();

            var mapFiles = mapFilesFolder.GetFiles("*.map");

            //
            // Insert what test command you want below
            //

            var mapFile = new FileInfo(@"D:\Halo\Maps\Halo3\guardian.map");
            var mapName = mapFile.Name;
            Console.WriteLine($"{mapName}...");

            var cache = GameCache.Open(mapFile);
            */

            var resolver = Cache.StringTable.Resolver;
            for(int i = 0; i < Cache.StringTable.Count; i++)
            {
                var stringId = Cache.StringTable.GetStringId(i);
                var set = resolver.GetSet(stringId);
                var index = resolver.GetIndex(stringId);

                if (set == 1)
                    PrintEnum(index, Cache.StringTable[i]);//PrintStringID(set, index, i, Cache.StringTable[i], (StringIDType)set);
            }




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

