using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Shaders.ShaderFunctions;

namespace TagTool.Commands.RenderMethods
{
    class ListFunctionsCommand : Command
    {
        private GameCache Cache { get; }
        private RenderMethod Definition { get; }

        public ListFunctionsCommand(GameCache cache, RenderMethod definition)
            : base(true,

                 "ListFunctions",
                 "Lists any animated parameters in this render method.",

                 "ListFunctions",
                 "Lists any animated parameters in this render method.")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (Definition.ShaderProperties.Count == 0 || 
                Definition.ShaderProperties[0].Functions.Count == 0 ||
                Definition.ShaderProperties[0].EntryPoints.Count == 0 ||
                Definition.ShaderProperties[0].Passes.Count == 0 ||
                Definition.ShaderProperties[0].RoutingInfo.Count == 0 ||
                Definition.ShaderProperties[0].Template == null)
                return true;

            var properties = Definition.ShaderProperties[0];

            using (var stream = Cache.OpenCacheRead())
            {
                var template = Cache.Deserialize<RenderMethodTemplate>(stream, properties.Template);

                var animatedParameters = ShaderFunctionHelper.GetAnimatedParameters(Cache, Definition, template);

                Console.WriteLine("TEXTURE FUNCTIONS:");
                foreach (var param in animatedParameters)
                    if (param.Type == ShaderFunctionHelper.ParameterType.Texture)
                        Console.WriteLine($"{param.Name}: {param.FunctionType}, function block {param.FunctionIndex}");

                Console.WriteLine("REAL CONSTANT FUNCTIONS:");
                foreach (var param in animatedParameters)
                    if (param.Type == ShaderFunctionHelper.ParameterType.Real)
                        Console.WriteLine($"{param.Name}: {param.FunctionType}, function block {param.FunctionIndex}");
            }

            return true;
        }
    }
}
