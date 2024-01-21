using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders.ShaderMatching;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;

namespace TagTool.Shaders.ShaderGenerator
{
    public class PopulateRenderMethodConstants
    {
        public List<TextureConstant> SetupTextureConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions, GameCacheHaloOnlineBase Cache)
        {
            List<TextureConstant> textureConstants = new List<TextureConstant>();

            foreach (var samplerName in rmt2.TextureParameterNames)
            {
                string name = Cache.StringTable[(int)samplerName.Name.Value];

                TextureConstant textureConstant = new TextureConstant();

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable[(int)option.Name.Value] == name && option.Type == RenderMethodOption.ParameterBlock.OptionDataType.Bitmap)
                        {
                            textureConstant.Bitmap = option.DefaultSamplerBitmap;
                            textureConstant.SamplerAddressMode = new TextureConstant.PackedSamplerAddressMode
                            {
                                AddressU = (TextureConstant.SamplerAddressModeEnum)option.DefaultAddressMode,
                                AddressV = (TextureConstant.SamplerAddressModeEnum)option.DefaultAddressMode
                            };
                            textureConstant.FilterMode = (TextureConstant.SamplerFilterMode)option.DefaultFilterMode;
                            textureConstant.ExternTextureMode = TextureConstant.RenderMethodExternTextureMode.UseBitmapAsNormal;

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
                        textureConstant.TextureTransformConstantIndex = i;
                        break;
                    }
                }

                if (textureConstant.Bitmap == null)
                    new TagToolWarning($"Texture constant \"{name}\" has no default bitmap. This needs to be set or this shader can become corrupted ingame");

                textureConstants.Add(textureConstant);
            }

            return textureConstants;
        }

        public List<RealConstant> SetupRealConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions, GameCacheHaloOnlineBase Cache)
        {
            List<RealConstant> realConstants = new List<RealConstant>();

            foreach (var realConstantName in rmt2.RealParameterNames)
            {
                string name = Cache.StringTable.GetString(realConstantName.Name);

                RealConstant realConstant = new RealConstant();

                foreach (var rmop in renderMethodOptions)
                {
                    bool found = false;

                    foreach (var option in rmop.Parameters)
                    {
                        if (Cache.StringTable.GetString(option.Name) == name &&
                            option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Bool &&
                            option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Int)
                        {
                            if (option.Type == RenderMethodOption.ParameterBlock.OptionDataType.Bitmap)
                            {
                                realConstant.Arg0 = option.DefaultBitmapScale > 0 ? option.DefaultBitmapScale : 1.0f;
                                realConstant.Arg1 = option.DefaultBitmapScale > 0 ? option.DefaultBitmapScale : 1.0f;
                                realConstant.Arg2 = 0;
                                realConstant.Arg3 = 0;
                            }
                            else if (option.Type == RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor)
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

        public List<uint> SetupIntegerConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions, GameCacheHaloOnlineBase Cache)
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
                        if (Cache.StringTable.GetString(option.Name) == name && option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Int)
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

        public uint SetupBooleanConstants(RenderMethodTemplate rmt2, List<RenderMethodOption> renderMethodOptions, GameCacheHaloOnlineBase Cache)
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
                        if (Cache.StringTable.GetString(option.Name) == name && option.Type != RenderMethodOption.ParameterBlock.OptionDataType.Bool)
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

        public static readonly Dictionary<string, BlendModeValue> BlendModeBinding = new Dictionary<string, BlendModeValue>
        {
            ["opaque"] = BlendModeValue.Opaque,
            ["additive"] = BlendModeValue.Additive,
            ["multiply"] = BlendModeValue.Multiply,
            ["alpha_blend"] = BlendModeValue.AlphaBlend,
            ["double_multiply"] = BlendModeValue.DoubleMultiply,
            ["pre_multiplied_alpha"] = BlendModeValue.PreMultipliedAlpha,
            ["maximum"] = BlendModeValue.Maximum,
            ["multiply_add"] = BlendModeValue.MultiplyAdd,
            ["add_src_times_dstalpha"] = BlendModeValue.AddSrcTimesDstalpha,
            ["add_src_times_srcalpha"] = BlendModeValue.AddSrcTimesSrcalpha,
            ["inv_alpha_blend"] = BlendModeValue.InverseAlphaBlend,
        };

        public BlendModeValue GetAlphaBlendMode(ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor, RenderMethodDefinition rmdf, GameCacheHaloOnlineBase Cache)
        {
            for (int i = 0; i < rmdf.Categories.Count; i++)
            {
                if (Cache.StringTable.GetString(rmdf.Categories[i].Name) == "blend_mode")
                {
                    string blendMode = Cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[rmt2Descriptor.Options[i]].Name);

                    if (BlendModeBinding.TryGetValue(blendMode, out BlendModeValue alphaBlendMode))
                        return alphaBlendMode;

                    Console.WriteLine($"Could not parse blend mode \"{blendMode}\"");
                    return BlendModeValue.Opaque;
                }
            }

            return BlendModeValue.Opaque;
        }
    }
}
