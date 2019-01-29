using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Shaders;
using HaloShaderGenerator;
using TagTool.Serialization;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {

        private void ConvertShaderCortana(ShaderCortana shaderCortana, Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            var render_method_option_indices = shaderCortana.RenderMethodDefinitionOptionIndices.Select(c => (int)c.OptionIndex).ToList();

            //CachedTagInstance newCortanaShaderInstance = CacheContext.TagCache.AllocateTag(TagGroup.Instances[groupTag]);
            //var ho_cortana_shader = (ShaderCortana)Activator.CreateInstance(typeof(ShaderCortana));

            var rmdf_instance = shaderCortana.BaseRenderMethod;
            var rmdf = CacheContext.Deserialize<RenderMethodDefinition>(new TagSerializationContext(cacheStream, CacheContext, rmdf_instance));

            //var shader_instance = CacheContext.GetTag<Shader>(@"shaders\invalid");
            //var shader = CacheContext.Deserialize<Shader>(new TagSerializationContext(cacheStream, CacheContext, shader_instance));

            //ho_cortana_shader.ImportData = shader.ImportData;
            //ho_cortana_shader.ShaderProperties = shader.ShaderProperties;

            var shader_properties = shaderCortana.ShaderProperties[0];
            shader_properties.ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>();
            shader_properties.Arguments = new List<RenderMethod.ShaderProperty.Argument>();
            shader_properties.Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>();
            shader_properties.DrawModes = new List<RenderMethodTemplate.DrawMode>();
            shader_properties.Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
            shader_properties.ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
            shader_properties.AnimationProperties = new List<RenderMethod.AnimationPropertiesBlock>();

            List<RenderMethodOption.OptionBlock> templateOptions = new List<RenderMethodOption.OptionBlock>();

            for (int i = 0; i < rmdf.Methods.Count; i++)
            {
                var method = rmdf.Methods[i];
                int selected_option_index = render_method_option_indices.Count > i ? render_method_option_indices[i] : 0;
                var selected_option = method.ShaderOptions[selected_option_index];

                var rmop_instance = selected_option.Option;
                if (rmop_instance != null)
                {
                    var rmop = CacheContext.Deserialize<RenderMethodOption>(new TagSerializationContext(cacheStream, CacheContext, rmop_instance));

                    templateOptions.AddRange(rmop.Options);
                }
            }

            RenderMethodTemplate rmt2 = null;
            if (shader_properties.Template == null)
            {
                GenerateCortanaRMT2Tag(
                    render_method_option_indices,
                    cacheStream,
                    resourceStreams,
                    out CachedTagInstance rmt2Instance,
                    out RenderMethodTemplate newRMT2);

                shader_properties.Template = rmt2Instance;
                rmt2 = newRMT2;
            }
            else
            {
                rmt2 = CacheContext.Deserialize<RenderMethodTemplate>(new TagSerializationContext(cacheStream, CacheContext, shader_properties.Template));
            }
            //shader_properties.DrawModes = rmt2.DrawModes;

            var shaderFunctions = new List<RenderMethod.AnimationPropertiesBlock>();
            var shaderVectorArguments = new RenderMethod.ShaderProperty.Argument[rmt2.VectorArguments.Count];

            var shaderSamplerArguments = new RenderMethod.ShaderProperty.ShaderMap[rmt2.SamplerArguments.Count];
            for (int rmt2SamplerIndex = 0; rmt2SamplerIndex < rmt2.SamplerArguments.Count; rmt2SamplerIndex++)
            {
                var rmt2SamplerArgument = rmt2.SamplerArguments[rmt2SamplerIndex];
                var name = rmt2SamplerArgument.Name;
                var name_str = CacheContext.GetString(name);
                var shaderSamplerArgument = new RenderMethod.ShaderProperty.ShaderMap();
                {
                    foreach (var importData in shaderCortana.ImportData)
                    {
                        if (importData.Type != RenderMethodOption.OptionBlock.OptionDataType.Sampler) continue;
                        if (importData.Name.Index != name.Index) continue;

                        if (importData.Bitmap != null)
                        {
                            shaderSamplerArgument.Bitmap = importData.Bitmap;
                            goto datafound;
                        }
                    }

                    foreach (var deafult_option in templateOptions)
                    {
                        if (deafult_option.Type != RenderMethodOption.OptionBlock.OptionDataType.Sampler) continue;
                        if (deafult_option.Name.Index != name.Index) continue;

                        shaderSamplerArgument.Bitmap = deafult_option.Bitmap;

                        goto datafound;
                    }

                    shaderSamplerArguments[rmt2SamplerIndex] = shaderSamplerArgument;

                    datafound:
                    if (shaderSamplerArgument.Bitmap == null)
                    {
                        Console.WriteLine($"WARNING: RMCT Conversion couldn't find a shader map for {name_str}");
                        shaderSamplerArgument.Bitmap = CacheContext.GetTag<Bitmap>(@"shaders\default_bitmaps\bitmaps\gray_50_percent");
                    }
                    shaderSamplerArguments[rmt2SamplerIndex] = shaderSamplerArgument;
                }

                {
                    int xform_index = GetExistingXFormArgumentIndex(name, rmt2.VectorArguments);
                    if (xform_index == -1)
                    {
                        Console.WriteLine($"WARNING: RMCT Conversion couldn't find a shader xform argument for {name_str}. Defaulting to 0");
                        xform_index = 0;
                    }
                    else
                    {
                        var shaderVectorArgument = ProcessArgument(rmt2SamplerArgument, shaderFunctions, templateOptions, shaderCortana);
                        shaderVectorArguments[xform_index] = shaderVectorArgument;
                    }
                    shaderSamplerArgument.XFormArgumentIndex = (sbyte)xform_index;
                }
            }
            shader_properties.ShaderMaps = shaderSamplerArguments.ToList();

            for (int rmt2ArgumentIndex = 0; rmt2ArgumentIndex < rmt2.VectorArguments.Count; rmt2ArgumentIndex++)
            {
                if (shaderVectorArguments[rmt2ArgumentIndex] != null) continue;
                var vectorArgument = rmt2.VectorArguments[rmt2ArgumentIndex];

                var shaderArgument = ProcessArgument(vectorArgument, shaderFunctions, templateOptions, shaderCortana);
                shaderVectorArguments[rmt2ArgumentIndex] = shaderArgument;
            }
            shader_properties.Arguments = shaderVectorArguments.ToList();
            shader_properties.AnimationProperties = shaderFunctions;

            if (shaderCortana.Material.Index == 0)
            {
                if (CacheContext.StringIdCache.Contains("default_material"))
                {
                    shaderCortana.Material = CacheContext.StringIdCache.GetStringId("default_material");
                }
            }

            //shader_cortana.Material = shader.Material;

            //ho_cortana_shader.BaseRenderMethod = shader.BaseRenderMethod;
            //newCortanaShaderInstance.Name = blamTag.Name;
            //CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newCortanaShaderInstance), ho_cortana_shader);
            //CacheContext.SaveTagNames();

        }

        private int GetExistingXFormArgumentIndex(StringId name, List<RenderMethodTemplate.ShaderArgument> vectorArguments)
        {
            int xform_index = -1;
            foreach (var rmt2VectorArgument in vectorArguments)
            {
                //NOTE: Shared name between Argumenst and Texture
                if (rmt2VectorArgument.Name.Index == name.Index)
                {
                    xform_index = vectorArguments.IndexOf(rmt2VectorArgument);
                    break;
                }
            }
            return xform_index;
        }

        private RenderMethod.ShaderProperty.Argument ProcessArgument(
            RenderMethodTemplate.ShaderArgument vectorArgument,
            List<RenderMethod.AnimationPropertiesBlock> shaderFunctions,
            List<RenderMethodOption.OptionBlock> templateOptions,
            ShaderCortana shaderCortana)
        {
            RenderMethod.ShaderProperty.Argument shaderArgument = new RenderMethod.ShaderProperty.Argument();

            var name = vectorArgument.Name;
            var nameStr = CacheContext.GetString(name);

            foreach (var importData in shaderCortana.ImportData)
            {
                if (importData.Name.Index != name.Index) continue;

                var argument_data = importData.AnimationProperties.Count > 0 ? importData.AnimationProperties[0].Function.Data : null;
                if (argument_data != null)
                {
                    var unknown0A = BitConverter.ToUInt16(argument_data, 0);
                    var unknown0B = BitConverter.ToUInt16(argument_data, 2);

                    var unknown1 = BitConverter.ToUInt32(argument_data, 20);
                    var unknown2 = BitConverter.ToUInt32(argument_data, 24);
                    var unknown3 = BitConverter.ToUInt32(argument_data, 28);

                    switch (importData.Type)
                    {
                        case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                            shaderArgument.Arg0 = BitConverter.ToSingle(argument_data, 4);
                            shaderArgument.Arg1 = BitConverter.ToSingle(argument_data, 8);
                            shaderArgument.Arg2 = BitConverter.ToSingle(argument_data, 12);
                            shaderArgument.Arg3 = BitConverter.ToSingle(argument_data, 16);
                            break;
                        case RenderMethodOption.OptionBlock.OptionDataType.Float4:
                            shaderArgument.Arg0 = BitConverter.ToSingle(argument_data, 4);
                            shaderArgument.Arg1 = BitConverter.ToSingle(argument_data, 8);
                            shaderArgument.Arg2 = BitConverter.ToSingle(argument_data, 12);
                            shaderArgument.Arg3 = BitConverter.ToSingle(argument_data, 16);
                            break;
                        case RenderMethodOption.OptionBlock.OptionDataType.Float:
                            shaderArgument.Arg0 = BitConverter.ToSingle(argument_data, 4);
                            shaderArgument.Arg1 = BitConverter.ToSingle(argument_data, 4);
                            shaderArgument.Arg2 = BitConverter.ToSingle(argument_data, 4);
                            shaderArgument.Arg3 = BitConverter.ToSingle(argument_data, 4);
                            break;
                        case RenderMethodOption.OptionBlock.OptionDataType.IntegerColor:
                            {
                                var iblue = argument_data[4];
                                var igreen = argument_data[5];
                                var ired = argument_data[6];
                                var iunusedAlpha = argument_data[7];

                                var blue = (float)iblue / 255.0f;
                                var green = (float)igreen / 255.0f;
                                var red = (float)ired / 255.0f;
                                var unusedAlpha = (float)iunusedAlpha / 255.0f;

                                var ialpha = argument_data[16];
                                var alpha = (float)ialpha / 255.0f;

                                shaderArgument.Arg0 = red;
                                shaderArgument.Arg1 = green;
                                shaderArgument.Arg2 = blue;
                                shaderArgument.Arg3 = alpha;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    // default arguments
                    switch (importData.Type)
                    {
                        case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                            shaderArgument.Arg0 = 1.0f;
                            shaderArgument.Arg1 = 1.0f;
                            shaderArgument.Arg2 = 0.0f;
                            shaderArgument.Arg3 = 0.0f;
                            break;
                    }
                }

                for (int functionIndex = 1; functionIndex < importData.AnimationProperties.Count; functionIndex++)
                {
                    shaderFunctions.Add(importData.AnimationProperties[functionIndex]);
                }

                goto datafound;
            }

            foreach (var deafult_option in templateOptions)
            {
                if (deafult_option.Name.Index != name.Index) continue;

                //TODO: Figure these bad boys out, I think its all just defaults but we should just
                // throw a warning if they're not part of the RMDF
                // (Don't throw warnings if we're using a custom shader RMDF

                goto datafound;
            }

            //TODO: Maybe we can do better than this, ie. custom shaders
            Console.WriteLine($"WARNING: RMCT Conversion couldn't find a argument for {nameStr}");
            datafound:

            return shaderArgument;
        }

        int GetArgumentIndex(string name, List<RenderMethodTemplate.ShaderArgument> args)
        {
            int index = -1;
            for (int i = 0; i < args.Count; i++)
            {
                var varg = args[i];
                if (name == CacheContext.GetString(varg.Name))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void GenerateCortanaRMT2Tag(List<int> options, Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, out CachedTagInstance rmt2Instance, out RenderMethodTemplate rmt2)
        {
            string template_name = $@"shaders\cortana_templates\_{string.Join("_", options)}";
            rmt2 = new RenderMethodTemplate();
            var rmt2_group = TagGroup.Instances[new TagStructureInfo(typeof(RenderMethodTemplate)).GroupTag];
            rmt2Instance = CacheContext.TagCache.AllocateTag(rmt2_group);
            var pixl = new PixelShader();
            var pixlGroup = TagGroup.Instances[new TagStructureInfo(typeof(PixelShader)).GroupTag];
            CachedTagInstance newPIXLInstance = CacheContext.TagCache.AllocateTag(pixlGroup);
            var vtsh = new VertexShader();
            var vtshGroup = TagGroup.Instances[new TagStructureInfo(typeof(VertexShader)).GroupTag];
            CachedTagInstance newVTSHInstance = CacheContext.TagCache.AllocateTag(vtshGroup);

            rmt2.PixelShader = newPIXLInstance;
            rmt2.VertexShader = newVTSHInstance;

            rmt2.DrawModeBitmask |= RenderMethodTemplate.ShaderModeBitmask.Active_Camo;

            rmt2.VectorArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.SamplerArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.ArgumentMappings = new List<RenderMethodTemplate.ArgumentMapping>();
            rmt2.RegisterOffsets = new List<RenderMethodTemplate.DrawModeRegisterOffsetBlock>();

            pixl.Shaders = new List<PixelShaderBlock>();
            pixl.DrawModes = new List<ShaderDrawMode>();
            rmt2.DrawModes = new List<RenderMethodTemplate.DrawMode>();
            foreach (RenderMethodTemplate.ShaderMode mode in Enum.GetValues(typeof(RenderMethodTemplate.ShaderMode)))
            {
                var pixelShaderDrawmode = new ShaderDrawMode();
                pixl.DrawModes.Add(pixelShaderDrawmode);
                var rmt2Drawmode = new RenderMethodTemplate.DrawMode();
                rmt2.DrawModes.Add(rmt2Drawmode);

                if (!HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(
                    HaloShaderGenerator.Enums.ShaderType.Cortana,
                    (HaloShaderGenerator.Enums.ShaderStage)(int)mode
                    )) continue;

                rmt2Drawmode.Offset = (ushort)rmt2.RegisterOffsets.Count();
                rmt2Drawmode.Count = 1;


                var shader_gen_result = HaloShaderGenerator.HaloShaderGenerator.GenerateShaderCortana(HaloShaderGenerator.Enums.ShaderStage.Active_Camo);

                var pixelShaderBlock = GeneratePixelShaderBlock(CacheContext, shader_gen_result);
                pixelShaderDrawmode.Count = 1;
                pixelShaderDrawmode.Offset = (byte)pixl.Shaders.Count;
                pixl.Shaders.Add(pixelShaderBlock);

                var registerOffsets = new RenderMethodTemplate.DrawModeRegisterOffsetBlock();
                rmt2.RegisterOffsets.Add(registerOffsets);

                registerOffsets.RenderMethodExternArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                var srcRenderMethodExternArguments = shader_gen_result.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.RenderMethodExtern_Arguments);
                foreach (var src_arg in srcRenderMethodExternArguments)
                {
                    var argument_mapping = new RenderMethodTemplate.ArgumentMapping
                    {
                        RegisterIndex = (ushort)src_arg.Register
                    };

                    foreach (var _enum in Enum.GetValues(typeof(RenderMethodTemplate.RenderMethodExtern)))
                    {
                        if (_enum.ToString().ToLower() == src_arg.Name)
                        {
                            argument_mapping.ArgumentIndex = (byte)_enum;
                            break;
                        }
                    }

                    rmt2.ArgumentMappings.Add(argument_mapping);
                    registerOffsets.RenderMethodExternArguments_Count++;
                }

                registerOffsets.SamplerArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                var srcSamplerArguments = shader_gen_result.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.TextureSampler_Arguments);
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping
                    {
                        RegisterIndex = (ushort)samplerRegister.Register,
                        ArgumentIndex = (byte)registerOffsets.SamplerArguments_Count++
                    };

                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = CacheContext.GetStringId(samplerRegister.Name)
                    };
                    rmt2.SamplerArguments.Add(shaderArgument);

                }

                registerOffsets.VectorArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                // add xform args
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    int index = GetArgumentIndex(samplerRegister.Name, rmt2.VectorArguments);
                    HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister xformRegister = null;
                    foreach (var register in shader_gen_result.Registers)
                    {
                        if (register.Name == $"{samplerRegister.Name}_xform")
                        {
                            xformRegister = register;
                            break;
                        }
                    }
                    if (xformRegister == null) continue;

                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping
                    {
                        RegisterIndex = (ushort)xformRegister.Register,

                        ArgumentIndex = (byte)(index != -1 ? index : rmt2.VectorArguments.Count)
                    };
                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = CacheContext.GetStringId(samplerRegister.Name)
                    };
                    rmt2.VectorArguments.Add(shaderArgument);

                    registerOffsets.VectorArguments_Count++;
                }

                var srcVectorArguments = shader_gen_result.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.Vector_Arguments);
                foreach (var vectorRegister in srcVectorArguments)
                {
                    if (vectorRegister.IsXFormArgument) continue; // we've already added these
                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping
                    {
                        RegisterIndex = (ushort)vectorRegister.Register,
                        ArgumentIndex = (byte)rmt2.VectorArguments.Count
                    };
                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = CacheContext.GetStringId(vectorRegister.Name)
                    };
                    rmt2.VectorArguments.Add(shaderArgument);

                    registerOffsets.VectorArguments_Count++;
                }
            }

            newPIXLInstance.Name = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newPIXLInstance), pixl);
            newVTSHInstance.Name = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newVTSHInstance), vtsh);
            rmt2Instance.Name = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, rmt2Instance), rmt2);

            CacheContext.SaveTagNames();
        }

        public static PixelShaderBlock GeneratePixelShaderBlock(HaloOnlineCacheContext cacheContext, ShaderGeneratorResult shader_gen_result)
        {
            var pixelShaderBlock = new PixelShaderBlock
            {
                PCShaderBytecode = shader_gen_result.Bytecode,
                PCParameters = new List<ShaderParameter>()
            };

            foreach (var register in shader_gen_result.Registers)
            {
                ShaderParameter shaderParameter = new ShaderParameter
                {
                    RegisterIndex = (ushort)register.Register,
                    RegisterCount = (byte)register.Size
                };

                switch (register.registerType)
                {
                    case HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.RegisterType.Boolean:
                        shaderParameter.RegisterType = ShaderParameter.RType.Boolean;
                        break;
                    case HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.RegisterType.Integer:
                        shaderParameter.RegisterType = ShaderParameter.RType.Integer;
                        break;
                    case HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.RegisterType.Sampler:
                        shaderParameter.RegisterType = ShaderParameter.RType.Sampler;
                        break;
                    case HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.RegisterType.Vector:
                        shaderParameter.RegisterType = ShaderParameter.RType.Vector;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                shaderParameter.ParameterName = cacheContext.GetStringId(register.Name);

                pixelShaderBlock.PCParameters.Add(shaderParameter);
            }

            return pixelShaderBlock;
        }
    }
}