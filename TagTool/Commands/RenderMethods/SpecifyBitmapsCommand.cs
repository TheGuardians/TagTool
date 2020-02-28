using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class SpecifyBitmapsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public SpecifyBitmapsCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "SpecifyBitmaps",
                 "Allows the bitmaps of the render_method to be respecified.",

                 "SpecifyBitmaps",

                 "Allows the bitmaps of the render_method to be respecified.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;
            
            var shaderMaps = new Dictionary<StringId, CachedTag>();

            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = Cache.OpenCacheRead())
                    template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                for (var i = 0; i < template.TextureParameterNames.Count; i++)
                {
                    var mapTemplate = template.TextureParameterNames[i];

                    Console.Write(string.Format("Please enter the {0} index: ", Cache.StringTable.GetString(mapTemplate.Name)));

                    if (!Cache.TryGetCachedTag(Console.ReadLine(), out var shaderMap))
                    {
                        Console.WriteLine($"ERROR: Invalid bitmap name, setting to null.");
                        shaderMaps[mapTemplate.Name] = null;
                    }

                    property.TextureConstants[i].Bitmap = shaderMaps[mapTemplate.Name];
                }
            }

            foreach (var import in Definition.ImportData)
                if (shaderMaps.ContainsKey(import.Name))
                    import.Bitmap = shaderMaps[import.Name];

            using (var cacheStream = Cache.OpenCacheReadWrite())
                Cache.Serialize(cacheStream, Tag, Definition);

            Console.WriteLine("Done!");

            return true;
        }
    }
}
