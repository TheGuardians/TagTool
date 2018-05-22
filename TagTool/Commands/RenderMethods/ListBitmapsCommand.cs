using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Serialization;
using TagTool.Commands;

namespace TagTool.Commands.RenderMethods
{
    class ListBitmapsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethod Definition { get; }

        public ListBitmapsCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderMethod definition)
            : base(true,

                 "ListBitmaps",
                 "Lists the bitmaps used by the render_method.",

                 "ListBitmaps",

                 "Lists the bitmaps used by the render_method.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = CacheContext.OpenTagCacheRead())
                {
                    var context = new TagSerializationContext(cacheStream, CacheContext, property.Template);
                    template = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(context);
                }

                for (var i = 0; i < template.ShaderMaps.Count; i++)
                {
                    var mapTemplate = template.ShaderMaps[i];

                    Console.WriteLine($"Bitmap {i} ({CacheContext.GetString(mapTemplate.Name)}): {property.ShaderMaps[i].Bitmap.Group.Tag} 0x{property.ShaderMaps[i].Bitmap.Index:X4}");
                }
            }

            return true;
        }
    }
}
