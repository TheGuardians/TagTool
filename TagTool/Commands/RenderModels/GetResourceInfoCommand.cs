using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderModels
{
    class GetResourceInfoCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public GetResourceInfoCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderModel definition)
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

            var resource = Definition.Geometry.Resource;

            if (resource == null || resource.Page.Index < 0 || !resource.GetLocation(out var location))
                return true;

            Console.WriteLine();
            Console.WriteLine($"[Location: {location}, Index: {resource.Page.Index}, Compressed Size: {resource.Page.CompressedBlockSize}]");

            return true;
        }
    }
}