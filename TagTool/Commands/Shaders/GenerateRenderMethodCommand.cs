using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders.ShaderMatching;

namespace TagTool.Commands.Shaders
{
    class GenerateRenderMethodCommand : Command
    {
        GameCache Cache;

        public GenerateRenderMethodCommand(GameCache cache) :
            base(true,

                "GenerateRenderMethod",
                "Generates a render method tag from the specified template",

                "GenerateRenderMethod <tag name> <rmt2 name>",

                "Generates a render method tag from the specified template\n" +
                "The rm\'s parameters will use default values as specified in rmop")
        {
            Cache = cache;
        }

        static readonly Dictionary<string, string> ShaderTypeGroups = new Dictionary<string, string>
        {
            ["shader"] = "rmsh",
            ["decal"] = "rmd ",
            ["halogram"] = "rmhg",
            ["water"] = "rmw ",
            ["black"] = "rmbk",
            ["terrain"] = "rmtr",
            ["custom"] = "rmcs",
            ["foliage"] = "rmfl",
            ["screen"] = "rmss",
            ["cortana"] = "rmct",
            ["zonly"] = "rmzo",
        };

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            if (!Cache.TagCache.TryGetTag($"{args[1]}.rmt2", out var rmt2Tag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find \"{args[1]}.rmt2\"");

            // easier to get the type, and cleaner to check if ms30
            ShaderMatcherNew.Rmt2Descriptor.TryParse(rmt2Tag.Name, out var rmt2Descriptor);

            // check if tag already exists, or allocate new one
            string rmGroup = ShaderTypeGroups[rmt2Descriptor.Type];
            if (!Cache.TagCache.TryGetTag($"{args[0]}.{rmGroup}", out var rmTag))
                rmTag = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(rmGroup), args[0]);

            string prefix = rmt2Descriptor.IsMs30 ? "ms30\\" : "";
            string rmdfName = prefix + "shaders\\" + rmt2Descriptor.Type;
            if (!Cache.TagCache.TryGetTag($"{rmdfName}.rmdf", out var rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find \"{rmdfName}.rmdf\"");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                // store rmop definitions for quick lookup
                List<RenderMethodOption> renderMethodOptions = new List<RenderMethodOption>();
                for (int i = 0; i < rmt2Descriptor.Options.Length; i++)
                {
                    var rmopTag = rmdf.Categories[i].ShaderOptions[rmt2Descriptor.Options[i]].Option;
                    if (rmopTag != null)
                        renderMethodOptions.Add(Cache.Deserialize<RenderMethodOption>(stream, rmopTag));
                }

                // create definition
                object definition = Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(rmGroup));
                // make changes as RenderMethod so the code can be reused for each rm type
                var rmDefinition = definition as RenderMethod;

                rmDefinition.BaseRenderMethod = rmdfTag;

                // initialize lists
                rmDefinition.RenderMethodDefinitionOptionIndices = new List<RenderMethod.RenderMethodDefinitionOptionIndex>();
                rmDefinition.ShaderProperties = new List<RenderMethod.ShaderProperty>();

                foreach (var option in rmt2Descriptor.Options)
                    rmDefinition.RenderMethodDefinitionOptionIndices.Add(new RenderMethod.RenderMethodDefinitionOptionIndex { OptionIndex = option });
                rmDefinition.SortingLayer = TagTool.Shaders.SortingLayerValue.Normal;
                rmDefinition.Unknown2 = -1;

                // setup shader property
                RenderMethod.ShaderProperty shaderProperty = new RenderMethod.ShaderProperty
                {
                    Template = rmt2Tag,
                    // setup constants
                    TextureConstants = SetupTextureConstants(rmt2, renderMethodOptions),
                    RealConstants = SetupRealConstants(rmt2, renderMethodOptions),
                    IntegerConstants = SetupIntegerConstants(rmt2, renderMethodOptions),
                    BooleanConstants = SetupBooleanConstants(rmt2, renderMethodOptions),
                    // get alpha blend mode
                    AlphaBlendMode = GetAlphaBlendMode(rmt2Descriptor, rmdf),
                    // TODO
                    QueryableProperties = new short[] { -1, -1, -1, -1, -1, -1, -1, -1 }
                };

                rmDefinition.ShaderProperties.Add(shaderProperty);

                Cache.Serialize(stream, rmTag, definition);
                (Cache as GameCacheHaloOnlineBase).SaveTagNames();
            }

            Console.WriteLine($"Generated {rmGroup} tag: {rmTag.Name}.{rmTag.Group}");
            return true;
        }

        List<RenderMethod.ShaderProperty.TextureConstant> SetupTextureConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions)
        {
            List<RenderMethod.ShaderProperty.TextureConstant> textureConstants = new List<RenderMethod.ShaderProperty.TextureConstant>();

            foreach (var samplerName in rmt2.TextureParameterNames)
            {
                string name = Cache.StringTable.GetString(samplerName.Name);

                RenderMethod.ShaderProperty.TextureConstant textureConstant = new RenderMethod.ShaderProperty.TextureConstant();

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable.GetString(option.Name) == name && option.Type == RenderMethodOption.ParameterBlock.OptionDataType.Sampler)
                        {
                            textureConstant.Bitmap = option.DefaultSamplerBitmap;
                            textureConstant.SamplerAddressMode = new RenderMethod.ShaderProperty.TextureConstant.PackedSamplerAddressMode
                            {
                                AddressU = (RenderMethod.ShaderProperty.TextureConstant.SamplerAddressModeEnum)option.DefaultAddressMode,
                                AddressV = (RenderMethod.ShaderProperty.TextureConstant.SamplerAddressModeEnum)option.DefaultAddressMode
                            };
                            textureConstant.FilterMode = (RenderMethod.ShaderProperty.TextureConstant.SamplerFilterMode)option.DefaultFilterMode;
                            textureConstant.ExternMode = (sbyte)option.RenderMethodExtern;

                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                for (sbyte i = 0; i < rmt2.RealParameterNames.Count; i++)
                {
                    string realName = Cache.StringTable.GetString(rmt2.RealParameterNames[i].Name);
                    if (name == realName)
                    {
                        textureConstant.XFormArgumentIndex = i;
                        break;
                    }
                }

                if (textureConstant.Bitmap == null)
                    new TagToolWarning("Texture constant has no default bitmap. This needs to be set or this shader can become corrupted ingame");

                textureConstants.Add(textureConstant);
            }

            return textureConstants;
        }

        List<RenderMethod.ShaderProperty.RealConstant> SetupRealConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions)
        {
            List<RenderMethod.ShaderProperty.RealConstant> realConstants = new List<RenderMethod.ShaderProperty.RealConstant>();

            foreach (var realConstantName in rmt2.RealParameterNames)
            {
                string name = Cache.StringTable.GetString(realConstantName.Name);

                RenderMethod.ShaderProperty.RealConstant realConstant = new RenderMethod.ShaderProperty.RealConstant();

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable.GetString(option.Name) == name &&
                            option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Boolean &&
                            option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Integer)
                        {
                            if (option.Type == RenderMethodOption.ParameterBlock.OptionDataType.Sampler)
                            {
                                realConstant.Arg0 = option.DefaultBitmapScale > 0 ? option.DefaultBitmapScale : 1.0f;
                                realConstant.Arg1 = option.DefaultBitmapScale > 0 ? option.DefaultBitmapScale : 1.0f;
                                realConstant.Arg2 = 0;
                                realConstant.Arg3 = 0;
                            }
                            else if (option.Type == RenderMethodOption.ParameterBlock.OptionDataType.IntegerColor)
                            {
                                realConstant.Arg0 = (float)option.DefaultColor.Red / 255;
                                realConstant.Arg1 = (float)option.DefaultColor.Green / 255;
                                realConstant.Arg2 = (float)option.DefaultColor.Blue / 255;
                                realConstant.Arg3 = (float)option.DefaultColor.Alpha / 255;
                            }
                            else
                            {
                                realConstant.Arg0 = option.DefaultFloatArgument;
                                realConstant.Arg1 = option.DefaultFloatArgument;
                                realConstant.Arg2 = option.DefaultFloatArgument;
                                realConstant.Arg3 = option.DefaultFloatArgument;
                            }

                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                realConstants.Add(realConstant);
            }

            return realConstants;
        }

        List<uint> SetupIntegerConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions)
        {
            List<uint> integerConstants = new List<uint>();

            foreach (var integerConstantName in rmt2.IntegerParameterNames)
            {
                string name = Cache.StringTable.GetString(integerConstantName.Name);

                uint integerConstant = 0;

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable.GetString(option.Name) == name && option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Integer)
                        {
                            integerConstant = option.DefaultIntBoolArgument;
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                integerConstants.Add(integerConstant);
            }

            return integerConstants;
        }

        uint SetupBooleanConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions)
        {
            uint booleanConstants = 0;

            for (int i = 0; i < rmt2.BooleanParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(rmt2.BooleanParameterNames[i].Name);

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable.GetString(option.Name) == name && option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Boolean)
                        {
                            booleanConstants |= option.DefaultIntBoolArgument >> i;
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }
            }

            return booleanConstants;
        }

        RenderMethod.ShaderProperty.AlphaBlendModeValue GetAlphaBlendMode(ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor, RenderMethodDefinition rmdf)
        {
            for (int i = 0; i < rmdf.Categories.Count; i++)
            {
                if (Cache.StringTable.GetString(rmdf.Categories[i].Name) == "blend_mode")
                {
                    string blendMode = Cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[rmt2Descriptor.Options[i]].Name);

                    if (Enum.TryParse<RenderMethod.ShaderProperty.AlphaBlendModeValue>(blendMode, out var alphaBlendMode))
                        return alphaBlendMode;

                    Console.WriteLine($"Could not parse blend mode \"{blendMode}\"");
                    return RenderMethod.ShaderProperty.AlphaBlendModeValue.opaque;
                }
            }

            return RenderMethod.ShaderProperty.AlphaBlendModeValue.opaque;
        }
    }
}
