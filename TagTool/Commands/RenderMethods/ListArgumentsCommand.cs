using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class ListArgumentsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public ListArgumentsCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "ListArguments",
                 "Lists the arguments of the render_method.",

                 "ListArguments",

                 "Lists the arguments of the render_method.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template = null;

                using (var cacheStream = Cache.OpenCacheRead())
                    template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

				Console.WriteLine("");

				for (var i = 0; i < template.RealParameterNames.Count; i++)
                {
                    var argumentName = Cache.StringTable.GetString(template.RealParameterNames[i].Name);
                    var argumentValue = new RealQuaternion(property.RealConstants[i].Values);

                    Console.WriteLine(string.Format("{0} {1} {2} {3} {4}", argumentName, argumentValue.I, argumentValue.J, argumentValue.K, argumentValue.W));
                }
            }

            return true;
        }
    }
}
