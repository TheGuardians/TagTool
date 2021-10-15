using System;
using System.Linq;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Shaders.ShaderMatching;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class SetArgumentCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public SetArgumentCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "SetArgument",
                 "Sets the value(s) of the specified argument in the render_method.",

                 "SetArgument <Name> [Arg1 Arg2 Arg3 Arg4]\n"+
				 "SetArgument Booleans flag,flag,flag,flag...",

                 "Sets the value(s) of the specified argument in the render_method.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 5)
                return new TagToolError(CommandError.ArgCount);

            RenderMethodTemplate template = null;

            // set default value
            if (args.Count == 1)
            {
                string constantName = args[0];

                using (var stream = Cache.OpenCacheRead())
                {
                    template = Cache.Deserialize<RenderMethodTemplate>(stream, Definition.ShaderProperties[0].Template);
                    var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, Definition.BaseRenderMethod);

                    int index = -1;
                    for (int i = 0; i < template.RealParameterNames.Count; i++)
                        if (Cache.StringTable.GetString(template.RealParameterNames[i].Name) == constantName)
                        {
                            index = i;
                            break;
                        }

                    if (index == -1)
                        return new TagToolError(CommandError.ArgInvalid, $"The argument \"{args[0]}\" does not exist");

                    ShaderMatcherNew.Rmt2Descriptor.TryParse(Definition.ShaderProperties[0].Template.Name, out var rmt2Descriptor);

                    for (int methodIndex = 0; methodIndex < rmt2Descriptor.Options.Length; methodIndex++)
                    {
                        var optionTag = rmdf.Categories[methodIndex].ShaderOptions[rmt2Descriptor.Options[methodIndex]].Option;

                        if (optionTag != null)
                        {
                            var rmop = Cache.Deserialize<RenderMethodOption>(stream, optionTag);

                            foreach (var constantData in rmop.Parameters)
                                if (Cache.StringTable.GetString(constantData.Name) == constantName)
                                {
                                    // constant found, apply default value

                                    if (constantData.Type == RenderMethodOption.ParameterBlock.OptionDataType.Sampler)
                                    {
                                        Definition.ShaderProperties[0].RealConstants[index].Arg0 = constantData.DefaultBitmapScale;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg1 = constantData.DefaultBitmapScale;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg2 = 0.0f;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg3 = 0.0f;
                                    }
                                    else if (constantData.Type == RenderMethodOption.ParameterBlock.OptionDataType.Boolean)
                                    {
                                        Definition.ShaderProperties[0].RealConstants[index].Arg0 = constantData.DefaultIntBoolArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg1 = constantData.DefaultIntBoolArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg2 = constantData.DefaultIntBoolArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg3 = constantData.DefaultIntBoolArgument;
                                    }
                                    else if (constantData.DefaultColor.GetValue() > 0)
                                    {
                                        Definition.ShaderProperties[0].RealConstants[index].Arg0 = constantData.DefaultColor.Red / 255.0f;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg1 = constantData.DefaultColor.Green / 255.0f;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg2 = constantData.DefaultColor.Blue / 255.0f;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg3 = constantData.DefaultColor.Alpha / 255.0f;
                                    }
                                    else // float, float4
                                    {
                                        Definition.ShaderProperties[0].RealConstants[index].Arg0 = constantData.DefaultFloatArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg1 = constantData.DefaultFloatArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg2 = constantData.DefaultFloatArgument;
                                        Definition.ShaderProperties[0].RealConstants[index].Arg3 = constantData.DefaultFloatArgument;
                                    }

                                    string argumentValues = $"" +
                                        $"{Definition.ShaderProperties[0].RealConstants[index].Arg0}, " +
                                        $"{Definition.ShaderProperties[0].RealConstants[index].Arg1}, " +
                                        $"{Definition.ShaderProperties[0].RealConstants[index].Arg2}, " +
                                        $"{Definition.ShaderProperties[0].RealConstants[index].Arg3}";

                                    Console.WriteLine($"No values provided: \"{constantName}\" defaulted to \"{argumentValues}\"");
                                    return true;
                                }
                        }
                        else
                            return new TagToolError(CommandError.ArgCount);
                    }
                }

                return false;
            }

            var argumentName = args[0];
            var values = new List<float>();
            var properties = Definition.ShaderProperties[0];

            using (var cacheStream = Cache.OpenCacheRead())
                template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, properties.Template);


            if (argumentName.ToLower() == "booleans")
            {
                Console.WriteLine();
                var names = args[1].ToLower().Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
                var accumulator = 0;
                for (int i = 0; i < template.BooleanParameterNames.Count; i++)
                {
                    var nameStr = Cache.StringTable.GetString(template.BooleanParameterNames[i].Name);
                    var namesIdx = names.IndexOf(nameStr.ToLower());
                    
                    if (namesIdx > -1)
                    {
                        accumulator += 1 << i;
                        names.RemoveAt(namesIdx);
                        Console.Write("   [X] ");
                    }
                    else Console.Write("   [ ] ");
                    Console.WriteLine(nameStr);
                }

                if (names.Count > 0)
                {
                    Console.WriteLine();
                    new TagToolWarning($"Flag(s) not found: {String.Join(", ", names)}");
                }

                properties.BooleanConstants = (uint)accumulator;

                return true;
            }


            while (args.Count > 1)
            {
                if (!float.TryParse(args[1], out var value))
                    return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\"");

                values.Add(value);
                args.RemoveAt(1);
            }

            var argumentIndex = -1;

            for (var i = 0; i < template.RealParameterNames.Count; i++)
            {
                if (Cache.StringTable.GetString(template.RealParameterNames[i].Name) == argumentName)
                {
                    argumentIndex = i;
                    break;
                }
            }

            if (argumentIndex < 0 || argumentIndex >= properties.RealConstants.Count)
                return new TagToolError(CommandError.ArgInvalid, $"Invalid argument name: {argumentName}");

            var argument = properties.RealConstants[argumentIndex];

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