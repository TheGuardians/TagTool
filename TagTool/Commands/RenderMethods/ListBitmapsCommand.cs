using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class ListBitmapsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public ListBitmapsCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "ListBitmaps",
                 "Lists the bitmaps used by the render_method.",

                 "ListBitmaps",

                 "Lists the bitmaps used by the render_method.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = Cache.OpenCacheRead())
                    template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                for (var i = 0; i < template.TextureParameterNames.Count; i++)
                {
                    var mapTemplate = template.TextureParameterNames[i];

                    Console.WriteLine($"Bitmap {i} ({Cache.StringTable.GetString(mapTemplate.Name)}): {property.TextureConstants[i].Bitmap.Group.Tag} 0x{property.TextureConstants[i].Bitmap.Index:X4}");
                }
            }

            return true;
        }
    }
}
