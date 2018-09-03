using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Serialization;

namespace TagTool.Commands.RenderMethods
{
    class ListArgumentsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethod Definition { get; }

        public ListArgumentsCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderMethod definition)
            : base(true,

                 "ListArguments",
                 "Lists the arguments of the render_method.",

                 "ListArguments",

                 "Lists the arguments of the render_method.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = CacheContext.OpenTagCacheRead())
                    template = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                for (var i = 0; i < template.VectorArguments.Count; i++)
                {
                    Console.WriteLine("");

                    var argumentName = CacheContext.GetString(template.VectorArguments[i].Name);
                    var argumentValue = new RealQuaternion(property.Arguments[i].Values);

                    Console.WriteLine(string.Format("{0}:", argumentName));

                    if (argumentName.EndsWith("_map"))
                    {
                        Console.WriteLine(string.Format("\tX Scale: {0}", argumentValue.I));
                        Console.WriteLine(string.Format("\tY Scale: {0}", argumentValue.J));
                        Console.WriteLine(string.Format("\tX Offset: {0}", argumentValue.K));
                        Console.WriteLine(string.Format("\tY Offset: {0}", argumentValue.W));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("\tX: {0}", argumentValue.I));
                        Console.WriteLine(string.Format("\tY: {0}", argumentValue.J));
                        Console.WriteLine(string.Format("\tZ: {0}", argumentValue.K));
                        Console.WriteLine(string.Format("\tW: {0}", argumentValue.W));
                    }
                }
            }

            return true;
        }
    }
}
