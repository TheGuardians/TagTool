using TagTool.Cache;
using TagTool.Commands;
using TagTool.Legacy.Base;
using TagTool.Legacy.Halo3Beta;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    public class ListBitmapsCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public ListBitmapsCommand(GameCacheContext cacheContext, CacheFile blamCache)
            : base(CommandFlags.None,

                  "ListBitmaps",
                  "",

                  "ListBitmaps <Blam Tag>",
                  "")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            CacheFile.IndexItem item = null;

            Console.WriteLine("Verifying blam shader tag...");

            var shaderName = args[0];

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.ParentClass == "rm") && tag.Filename == shaderName)
                {
                    item = tag;
                    break;
                }
            }

            if (item == null)
            {
                Console.WriteLine("Blam shader tag does not exist: " + shaderName);
                return false;
            }

            var blamShader = new shader(BlamCache, item.Offset);

            var templateItem = BlamCache.IndexItems.Find(i =>
                i.ID == blamShader.Properties[0].TemplateTagID);

            var template = new render_method_template(BlamCache, templateItem.Offset);

            for (var i = 0; i < template.UsageBlocks.Count; i++)
            {
                var entry = template.UsageBlocks[i].Usage;

                var bitmItem = BlamCache.IndexItems.Find(j =>
                j.ID == blamShader.Properties[0].ShaderMaps[i].BitmapTagID);
                Console.WriteLine(string.Format("{0:D2} {2}\t {1}", i, bitmItem, entry));
            }

            return true;
        }
    }
}