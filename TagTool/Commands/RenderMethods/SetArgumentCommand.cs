using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Serialization;

namespace TagTool.Commands.RenderMethods
{
    class SetArgumentCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethod Definition { get; }

        public SetArgumentCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderMethod definition)
            : base(true,

                 "SetArgument",
                 "Sets the value(s) of the specified argument in the render_method.",

                 "SetArgument <Name> [Arg1 Arg2 Arg3 Arg4]",

                 "Sets the value(s) of the specified argument in the render_method.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2 || args.Count > 5)
                return false;

            var argumentName = args[0];
            var values = new List<float>();

            while (args.Count > 1)
            {
                if (!float.TryParse(args[1], out var value))
                    throw new FormatException(args[1]);

                values.Add(value);
                args.RemoveAt(1);
            }

            RenderMethodTemplate template = null;
            var properties = Definition.ShaderProperties[0];
            
            using (var cacheStream = CacheContext.OpenTagCacheRead())
                template = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, properties.Template);

            var argumentIndex = -1;

            for (var i = 0; i < template.Arguments.Count; i++)
            {
                if (CacheContext.GetString(template.Arguments[i].Name) == argumentName)
                {
                    argumentIndex = i;
                    break;
                }
            }

            if (argumentIndex < 0 || argumentIndex >= properties.Arguments.Count)
                throw new KeyNotFoundException($"Invalid argument name: {argumentName}");

            var argument = properties.Arguments[argumentIndex];

            for (var i = 0; i < argument.Values.Length; i++)
            {
                if (i < values.Count)
                    argument.Values[i] = values[i];
                else
                    argument.Values[i] = 0.0f;
            }

            var argumentValue = new RealQuaternion(argument.Values);

            Console.WriteLine();
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

            return true;
        }
    }
}
