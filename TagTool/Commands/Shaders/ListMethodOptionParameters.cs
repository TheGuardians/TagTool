using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using TagTool.Shaders;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Commands.Shaders
{
    public class ListMethodOptionParameters : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethodDefinition Definition { get; }

        public ListMethodOptionParameters(GameCacheContext cacheContext, CachedTagInstance tag, RenderMethodDefinition definition) :
            base(true,

                "ListMethodOptionParameters",
                "ListMethodOptionParameters",
                "ListMethodOptionParameters",
                "ListMethodOptionParameters")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            //foreach(var method in Definition.Methods)
            //{
            //    var method_name = CacheContext.GetString(method.Type);

            //    foreach (var shader_option in method.ShaderOptions)
            //    {
            //        var option_name =CacheContext.GetString(shader_option.Type);
            //        var options_ref = shader_option.Option;

            //        if (options_ref == null) continue;

            //        RenderMethodOption options;
            //        using (var stream = CacheContext.OpenTagCacheRead())
            //        {
            //            var context = new TagSerializationContext(stream, CacheContext, options_ref);
            //            options = CacheContext.Deserializer.Deserialize(context, TagDefinition.Find(options_ref.Group.Tag)) as RenderMethodOption;
            //        }

            //        foreach(var option in options.Options)
            //        {
            //            var name = CacheContext.GetString(option.Name);
            //            var type = (int)option.Type;

            //            Console.WriteLine($"{method_name}_{option_name} {name} {type}");
            //        }


            //    }
            //}

            return true;
        }
    }
}
