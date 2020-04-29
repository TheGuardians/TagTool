using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderModels
{
    class SpecifyShadersCommand : Command
    {
        private GameCache CacheContext { get; }
        private CachedTag Tag { get; }
        private RenderModel Definition { get; }

        public SpecifyShadersCommand(GameCache cacheContext, CachedTag tag, RenderModel definition)
            : base(true,

                  "SpecifyShaders",
                  "Allows the shaders of a render_model to be respecified.",

                  "SpecifyShaders",

                  "Allows the shaders of a render_model to be respecified.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            foreach (var material in Definition.Materials)
            {
                if (material.RenderMethod != null)
                    Console.Write("Please enter the replacement {0:X8} index: ", material.RenderMethod.Index);
                else
                    Console.Write("Please enter the replace material #{0} index: ", Definition.Materials.IndexOf(material));

                if (!CacheContext.TagCache.TryGetTag(Console.ReadLine(), out material.RenderMethod))
                {
                    Console.WriteLine($"ERROR: Invalid shader specified. Using default shader.");
                    material.RenderMethod = CacheContext.TagCache.GetTag<Shader>(@"shaders\default");
                }
            }

            using (var cacheStream = CacheContext.OpenCacheReadWrite())
                CacheContext.Serialize(cacheStream, Tag, Definition);

            Console.WriteLine("Done!");

            return true;
        }
    }
}
