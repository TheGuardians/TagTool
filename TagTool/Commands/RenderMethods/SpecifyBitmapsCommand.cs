using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Serialization;

namespace TagTool.Commands.RenderMethods
{
    class SpecifyBitmapsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethod Definition { get; }

        public SpecifyBitmapsCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderMethod definition)
            : base(true,

                 "SpecifyBitmaps",
                 "Allows the bitmaps of the render_method to be respecified.",

                 "SpecifyBitmaps",

                 "Allows the bitmaps of the render_method to be respecified.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;
            
            var shaderMaps = new Dictionary<StringId, CachedTagInstance>();

            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = CacheContext.OpenTagCacheRead())
                    template = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                for (var i = 0; i < template.ShaderMaps.Count; i++)
                {
                    var mapTemplate = template.ShaderMaps[i];

                    Console.Write(string.Format("Please enter the {0} index: ", CacheContext.GetString(mapTemplate.Name)));

                    if (!CacheContext.TryGetTag(Console.ReadLine(), out var shaderMap))
                    {
                        Console.WriteLine($"ERROR: Invalid bitmap name, setting to null.");
                        shaderMaps[mapTemplate.Name] = null;
                    }

                    property.ShaderMaps[i].Bitmap = shaderMaps[mapTemplate.Name];
                }
            }

            foreach (var import in Definition.ImportData)
                if (shaderMaps.ContainsKey(import.Name))
                    import.Bitmap = shaderMaps[import.Name];

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                CacheContext.Serialize(cacheStream, Tag, Definition);

            Console.WriteLine("Done!");

            return true;
        }
    }
}
