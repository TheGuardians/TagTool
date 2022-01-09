using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders.ShaderFunctions;

namespace TagTool.Commands.RenderMethods
{
    class AddFunctionCommand : Command
    {
        private GameCache Cache { get; }
        private RenderMethod Definition { get; }

        public AddFunctionCommand(GameCache cache, RenderMethod definition)
            : base(true,

                 "AddFunction",
                 "Binds a function to animate the specified parameter in this render method.",

                 "AddFunction <parameter> <type> <function index>",
                 "Binds a function to animate the specified parameter in this render method.\n" +
                 "Parameter: Name of the parameter to bind the function to.\n" +
                 "Type: Type of parameter - Real or Texture (Int and Bool are not supported).\n" +
                 "Function Index: Index of the functions block that you want the parameter to use.\n"/* +
                 "Function Types: " + string.Join(", ", Enum.GetNames(typeof(RenderMethod.ShaderFunction.FunctionType)))*/)
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 3)
                return new TagToolError(CommandError.ArgCount);

            if (Definition.ShaderProperties.Count == 0 ||
                Definition.ShaderProperties[0].Template == null)
                return new TagToolError(CommandError.CustomError, "Invalid shader properties.");

            string parameterName = args[0];

            ShaderFunctionHelper.ParameterType parameterType;
            if (!Enum.TryParse(args[1], out parameterType))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\"");
            if (parameterType == ShaderFunctionHelper.ParameterType.Bool || parameterType == ShaderFunctionHelper.ParameterType.Int)
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\"");

            int functionIndex;
            if (!int.TryParse(args[2], out functionIndex))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[2]}\"");

            var properties = Definition.ShaderProperties[0];

            if (functionIndex >= properties.Functions.Count)
                new TagToolWarning($"Function index \"{functionIndex}\" is invalid.");

            using (var stream = Cache.OpenCacheRead())
            {
                var template = Cache.Deserialize<RenderMethodTemplate>(stream, properties.Template);

                // Get parameter index

                int parameterIndex = -1;
                if (parameterType == ShaderFunctionHelper.ParameterType.Real)
                {
                    for (int i = 0; i < template.RealParameterNames.Count; i++)
                    {
                        if (Cache.StringTable.GetString(template.RealParameterNames[i].Name) == parameterName)
                        {
                            parameterIndex = i;
                            break;
                        }
                    }
                }
                else if (parameterType == ShaderFunctionHelper.ParameterType.Texture)
                {
                    for (int i = 0; i < template.TextureParameterNames.Count; i++)
                    {
                        if (Cache.StringTable.GetString(template.TextureParameterNames[i].Name) == parameterName)
                        {
                            parameterIndex = i;
                            break;
                        }
                    }
                }

                if (parameterIndex == -1)
                    return new TagToolError(CommandError.ArgInvalid, $"\"{parameterName}\"");

                // Get existing + new animated parameters

                var animatedParameters = ShaderFunctionHelper.GetAnimatedParameters(Cache, Definition, template);

                var functionType = RenderMethod.RenderMethodAnimatedParameterBlock.FunctionType.Value;
                if (functionIndex < properties.Functions.Count)
                    functionType = properties.Functions[functionIndex].Type;

                if (ShaderFunctionHelper.AnimatedParameterExists(animatedParameters, parameterName, parameterType, functionType))
                    new TagToolWarning("The specified parameter is already being animated.");

                ShaderFunctionHelper.AnimatedParameter newParameter = new ShaderFunctionHelper.AnimatedParameter
                {
                    FunctionIndex = functionIndex,
                    Name = parameterName,
                    FunctionType = RenderMethod.RenderMethodAnimatedParameterBlock.FunctionType.Value,
                    Type = parameterType
                };

                // Rebuild into render method

                animatedParameters.Add(newParameter);

                if (!ShaderFunctionHelper.BuildAnimatedParameters(Cache, Definition, template, animatedParameters))
                    return new TagToolError(CommandError.CustomError, "Failed to build animated parameters.");
            }

            return true;
        }

    }
}
