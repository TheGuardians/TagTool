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

            //
            // Insert what test command you want below
            //

            using(var stream = Cache.TagCache.OpenTagCacheRead())
            {
                foreach(var tag in Cache.TagCache.TagTable)
                {
                    if(tag.Group.Tag == "sbsp")
                    {
                        var sbsp = Cache.Deserialize<ScenarioStructureBsp>(stream, tag);

                        foreach(var cluster in sbsp.Clusters)
                        {
                            foreach(var decoratorGrid in cluster.DecoratorGrids)
                            {
                                if (decoratorGrid.HaloOnlineInfo.VertexBufferIndex != decoratorGrid.HaloOnlineInfo.PaletteIndex)
                                    Console.WriteLine("Weird");
                            }
                        }
                    }
                    
                }

            }
            



            return true;
        }
    }
}

