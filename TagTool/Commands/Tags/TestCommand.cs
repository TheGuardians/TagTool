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

            var mapFiles = mapFilesFolder.GetFiles("*.map");

            //
            // Insert what test command you want below
            //

            foreach(var mapFile in mapFiles)
            {
                var mapName = mapFile.Name;
                var outFile = mapName + ".csv";
                outFile = Path.Combine(outDir.FullName, outFile);

                var map = new MapFile();
                using (var stream = mapFile.OpenRead())
                using (var reader = new EndianReader(stream))
                {
                    map.Read(reader);
                }

                Console.WriteLine(mapName);

                int fileOffset = map.Header.GetHeaderSize(map.Version);

                for(int i = 0; i < (int)CacheFileSectionType.Count; i++)
                {
                    var section = map.Header.Interop.Sections[i];
                    Console.WriteLine($"{(CacheFileSectionType)(i)}");
                    Console.WriteLine($"Virtual Address  {section.VirtualAddress:X}");
                    Console.WriteLine($"File Offset {fileOffset:X}");
                    Console.WriteLine($"Section Size  {section.Size:X}");
                    fileOffset += section.Size;
                }


                Console.WriteLine($"Tag section partitions:");
                int partitionOverallSize = 0;
                foreach(var partition in map.Header.Partitions)
                {
                    Console.WriteLine($"Size {partition.Size:X}");
                    Console.WriteLine($"Virtual Address {partition.VirtualAddress:X}"); 
                    Console.WriteLine($"Tag Section Offset {(partition.VirtualAddress - map.Header.BaseAddress):X}");
                    partitionOverallSize += partition.Size;
                }

                Console.WriteLine($"Overall partition size {partitionOverallSize:X}");

                /*
                using(var stream = new FileInfo(outFile).Create())
                {

                }
                */

                if(partitionOverallSize != map.Header.Interop.Sections[(int)CacheFileSectionType.TagSection].Size)
                {
                    throw new Exception("Tag section weirdness");
                }

                if (fileOffset != mapFile.Length)
                {
                    throw new Exception("File weirdness");
                }

            }
            



            return true;
        }
    }
}

