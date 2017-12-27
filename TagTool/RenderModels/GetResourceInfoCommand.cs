using System;
using System.Collections.Generic;
using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.TagDefinitions;

namespace TagTool.RenderModels
{
    class GetResourceInfoCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public GetResourceInfoCommand(GameCacheContext cacheContext, CachedTagInstance tag, RenderModel definition)
            : base(CommandFlags.None,

                  "GetResourceInfo",
                  "Gets information about the render_model's resource.",

                  "GetResourceInfo",

                  "Gets information about the render_model's resource.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            Console.WriteLine();
            Console.WriteLine($"[Location: {Definition.Geometry.Resource.GetLocation()}, Index: {Definition.Geometry.Resource.Page.Index}, Compressed Size: {Definition.Geometry.Resource.Page.CompressedBlockSize}]");

            return true;
        }
    }
}
