using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.TagDefinitions;
using TagTool.Serialization;
using TagTool.Commands;

namespace TagTool.Commands.RenderModels
{
    class SpecifyShadersCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public SpecifyShadersCommand(GameCacheContext cacheContext, CachedTagInstance tag, RenderModel definition)
            : base(CommandFlags.Inherit,

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

                var newRenderMethod = Console.ReadLine();

                material.RenderMethod = ArgumentParser.ParseTagSpecifier(CacheContext, newRenderMethod);

                if (material.RenderMethod == null && newRenderMethod != null)
                {
                    Console.WriteLine($"Using default shader 0x101F.");
                    material.RenderMethod = CacheContext.GetTag(0x101F);
                }
            }

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var context = new TagSerializationContext(cacheStream, CacheContext, Tag);
                CacheContext.Serializer.Serialize(context, Definition);
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
