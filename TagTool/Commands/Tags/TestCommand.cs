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
                var tag = Cache.TagCache.GetTagByName(@"objects\weapons\rifle\assault_rifle\assault_rifle_v6\material_11\base_map", "bitm");
                var bitmap = Cache.Deserialize<Bitmap>(stream, tag);
                var resourceDef = Cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[0]);


            }

            return true;
        }
    }
}

