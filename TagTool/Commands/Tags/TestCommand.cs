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

            //var cache = GameCache.Open(new FileInfo(@"D:\halo\maps\halo3\100_citadel.map"));

            foreach (var mapFile in mapFiles)
            {
                var cache = GameCache.Open(mapFile);
                int total = 0;
                int failed = 0;
                using(var stream = cache.OpenCacheRead())
                {
                    foreach(var tag in cache.TagCache.NonNull())
                    {
                        if (tag.IsInGroup("rm  "))
                        {
                            total++;
                            var renderMethod = cache.Deserialize<RenderMethod>(stream, tag);
                            var templateTag = renderMethod.ShaderProperties[0].Template;

                            string indexString = templateTag.Name.Split('\\').ToList().Last();
                            var indices = indexString.Split('_').ToList();
                            indices.RemoveAt(0);
                            List<short> methodIndices = new List<short>();
                            foreach(var strIndex in indices)
                            {
                                methodIndices.Add(Convert.ToInt16(strIndex));
                            }


                            

                            if(methodIndices.Count != renderMethod.RenderMethodDefinitionOptionIndices.Count)
                            {
                                Console.WriteLine($"WARNING: Failed to match render method option count in {tag.Name}");
                                failed++;
                            }
                            else
                            {
                                for (int i = 0; i < methodIndices.Count; i++)
                                {
                                    if( methodIndices[i] != renderMethod.RenderMethodDefinitionOptionIndices[i].OptionIndex)
                                    {
                                        string actualName = "";
                                        for(int j = 0; j < methodIndices.Count; j++)
                                        {
                                            actualName += $"_{renderMethod.RenderMethodDefinitionOptionIndices[i].OptionIndex}";
                                        }
                                        Console.WriteLine($"Failed to match render method option indices in {tag.Name}");
                                        Console.WriteLine($"Current: {indexString}");
                                        Console.WriteLine($"Real   : {actualName}");
                                        failed++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Out of {total} render methods, {failed} didn't have the right rmt2 name");
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

