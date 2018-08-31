using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;
using TagTool.Shaders;
using HaloShaderGenerator;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {

        private void ConvertShaderCortana(ShaderCortana shader_cortana, Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            var render_method_option_indices = shader_cortana.RenderMethodDefinitionOptionIndices.Select(c => (int)c.OptionIndex).ToList();

            //CachedTagInstance newCortanaShaderInstance = CacheContext.TagCache.AllocateTag(TagGroup.Instances[groupTag]);
            //var ho_cortana_shader = (ShaderCortana)Activator.CreateInstance(typeof(ShaderCortana));

            var rmdf_instance = shader_cortana.BaseRenderMethod;
            var rmdf = CacheContext.Deserialize<RenderMethodDefinition>(new TagSerializationContext(cacheStream, CacheContext, rmdf_instance));

            //var shader_instance = CacheContext.GetTag<Shader>(@"shaders\invalid");
            //var shader = CacheContext.Deserialize<Shader>(new TagSerializationContext(cacheStream, CacheContext, shader_instance));

            //ho_cortana_shader.ImportData = shader.ImportData;
            //ho_cortana_shader.ShaderProperties = shader.ShaderProperties;

            var shader_properties = shader_cortana.ShaderProperties[0];
            shader_properties.ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>();
            shader_properties.Arguments = new List<RenderMethod.ShaderProperty.Argument>();
            shader_properties.Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>();
            shader_properties.DrawModes = new List<RenderMethodTemplate.DrawMode>();
            shader_properties.Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
            shader_properties.ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
            shader_properties.Functions = new List<RenderMethod.FunctionBlock>();

            List<RenderMethodOption.OptionBlock> template_options = new List<RenderMethodOption.OptionBlock>();

            for (int i = 0; i < rmdf.Methods.Count; i++)
            {
                var method = rmdf.Methods[i];
                int selected_option_index = render_method_option_indices.Count > i ? render_method_option_indices[i] : 0;
                var selected_option = method.ShaderOptions[selected_option_index];

                var rmop_instance = selected_option.Option;
                if (rmop_instance != null)
                {
                    var rmop = CacheContext.Deserialize<RenderMethodOption>(new TagSerializationContext(cacheStream, CacheContext, rmop_instance));

                    template_options.AddRange(rmop.Options);
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

            var shader_arguments = new RenderMethod.ShaderProperty.Argument[rmt2.VectorArguments.Count];
            var shader_maps = new RenderMethod.ShaderProperty.ShaderMap[rmt2.SamplerArguments.Count];
            for (int i = 0; i < rmt2.SamplerArguments.Count; i++)
            {
                var shadermap = rmt2.SamplerArguments[i];
                var name = shadermap.Name;
                var name_str = CacheContext.GetString(name);

                var shader_map = new RenderMethod.ShaderProperty.ShaderMap();

                // Default Argument
                var shader_map_arg = new RenderMethod.ShaderProperty.Argument();
                shader_map_arg.Values = new float[4];
                shader_map_arg.Values[0] = 1.0f;
                shader_map_arg.Values[1] = 1.0f;
                shader_map_arg.Values[2] = 0.0f;
                shader_map_arg.Values[3] = 0.0f;

                int xform_index = -1;
                foreach (var arg in rmt2.VectorArguments)
                {
                    //NOTE: Shared name between Argumenst and Texture
                    if (arg.Name.Index == name.Index)
                    {
                        xform_index = rmt2.VectorArguments.IndexOf(arg);
                        break;
                    }
                }
                if (xform_index == -1)
                {
                    Console.WriteLine($"Waring: RMCT Conversion couldn't find a shader xform argument for {name_str}. Defaulting to 0");
                    shader_map.XFormArgumentIndex = 0;
                }
                shader_map.XFormArgumentIndex = (sbyte)xform_index;
                shader_arguments[xform_index] = shader_map_arg;

                //TODO: The rest of the shader_map information

                foreach (var importData in shader_cortana.ImportData)
                {
                    if (importData.Type != RenderMethodOption.OptionBlock.OptionDataType.Sampler) continue;
                    if (importData.Name.Index != name.Index) continue;

                    shader_map.Bitmap = importData.Bitmap;

                    goto datafound;
                }

                foreach (var deafult_option in template_options)
                {
                    if (deafult_option.Type != RenderMethodOption.OptionBlock.OptionDataType.Sampler) continue;
                    if (deafult_option.Name.Index != name.Index) continue;

                    shader_map.Bitmap = deafult_option.Bitmap;

                    goto datafound;
                }

                //TODO: Maybe we can do better than this, ie. custom shaders

                //throw new Exception($"Import data not found for {name_str}");
                Console.WriteLine($"Waring: RMCT Conversion couldn't find a shader map for {name_str}");
                datafound:
                shader_maps[i] = shader_map;
            }
            shader_properties.ShaderMaps = shader_maps.ToList();

            var shader_functions = new List<RenderMethod.FunctionBlock>();
            for (int rmt2ArgumentIndex = 0; rmt2ArgumentIndex < rmt2.VectorArguments.Count; rmt2ArgumentIndex++)
            {
                if (shader_arguments[rmt2ArgumentIndex] != null) continue;
                var argument = rmt2.VectorArguments[rmt2ArgumentIndex];
                var name = argument.Name;
                var name_str = CacheContext.GetString(name);

                RenderMethod.ShaderProperty.Argument shader_argument = new RenderMethod.ShaderProperty.Argument();

                float arg0 = 0.0f;
                float arg1 = 0.0f;
                float arg2 = 0.0f;
                float arg3 = 0.0f;

                foreach (var importData in shader_cortana.ImportData)
                {
                    if (importData.Name.Index != name.Index) continue;

                    var argument_data = importData.Functions.Count > 0 ? importData.Functions[0].Function.Data : null;
                    if (argument_data != null)
                    {
                        switch (importData.Type)
                        {
                            case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                                arg0 = 1.0f;
                                arg1 = 1.0f;
                                arg2 = 0.0f;
                                arg3 = 0.0f;
                                break;
                            case RenderMethodOption.OptionBlock.OptionDataType.Float:
                            case RenderMethodOption.OptionBlock.OptionDataType.Float4:
                            case RenderMethodOption.OptionBlock.OptionDataType.IntegerColor:
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        var unknown0A = BitConverter.ToUInt16(argument_data, 0);
                        var unknown0B = BitConverter.ToUInt16(argument_data, 2);

                        switch (unknown0B)
                        {
                            case 0: // float

                                arg0 = BitConverter.ToSingle(argument_data, 4);
                                arg1 = BitConverter.ToSingle(argument_data, 8);
                                arg2 = BitConverter.ToSingle(argument_data, 12);
                                arg3 = BitConverter.ToSingle(argument_data, 16);

                                break;

                            case 1: // integer or packed byte or something like that

                                var Iargument1 = BitConverter.ToUInt32(argument_data, 4);
                                var Iargument2 = BitConverter.ToUInt32(argument_data, 8);
                                var Iargument3 = BitConverter.ToUInt32(argument_data, 12);
                                var Iargument4 = BitConverter.ToUInt32(argument_data, 16);

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

                                arg0 = red;
                                arg1 = green;
                                arg2 = blue;
                                arg3 = alpha;

                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        var unknown1 = BitConverter.ToUInt32(argument_data, 20);
                        var unknown2 = BitConverter.ToUInt32(argument_data, 24);
                        var unknown3 = BitConverter.ToUInt32(argument_data, 28);

                        Console.WriteLine();
                    }

                    for (int functionIndex = 1; functionIndex < importData.Functions.Count; functionIndex++)
                    {
                        shader_functions.Add(importData.Functions[functionIndex]);
                    }

                    goto datafound;
                }

                foreach (var deafult_option in template_options)
                {
                    if (deafult_option.Name.Index != name.Index) continue;

                    //TODO: Figure these bad boys out, I think its all just defaults but we should just
                    // throw a warning if they're not part of the RMDF
                    // (Don't throw warnings if we're using a custom shader RMDF

                    goto datafound;
                }

                Console.WriteLine($"Waring: RMCT Conversion couldn't find a argument for {name_str}");
                datafound:

                //TODO: Maybe we can do better than this, ie. custom shaders

                shader_argument.Values = new float[4];
                shader_argument.Values[0] = arg0;
                shader_argument.Values[1] = arg1;
                shader_argument.Values[2] = arg2;
                shader_argument.Values[3] = arg3;

                shader_arguments[rmt2ArgumentIndex] = shader_argument;
            }
            shader_properties.Arguments = shader_arguments.ToList();
            //shader_properties.Functions = shader_functions;

            if (shader_cortana.Material.Index == 0)
            {
                if (CacheContext.StringIdCache.Contains("default_material"))
                {
                    shader_cortana.Material = CacheContext.StringIdCache.GetStringId("default_material");
                }
            }

            //shader_cortana.Material = shader.Material;

            //ho_cortana_shader.BaseRenderMethod = shader.BaseRenderMethod;
            //CacheContext.TagNames[newCortanaShaderInstance.Index] = blamTag.Name;
            //CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newCortanaShaderInstance), ho_cortana_shader);
            //CacheContext.SaveTagNames();

        }

        private static void ResetRMT2(RenderMethodTemplate Definition)
        {
            Definition.DrawModeBitmask = 0;
            Definition.DrawModes = new List<RenderMethodTemplate.DrawMode>();

            Definition.ArgumentMappings = new List<RenderMethodTemplate.ArgumentMapping>();
            Definition.RegisterOffsets = new List<RenderMethodTemplate.DrawModeRegisterOffsetBlock>();

            Definition.VectorArguments = new List<RenderMethodTemplate.ShaderArgument>();
            Definition.IntegerArguments = new List<RenderMethodTemplate.ShaderArgument>();
            Definition.BooleanArguments = new List<RenderMethodTemplate.ShaderArgument>();
            Definition.SamplerArguments = new List<RenderMethodTemplate.ShaderArgument>();
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
                    //var argument_mapping = new RenderMethodTemplate.ArgumentMapping();
                    //argument_mapping.RegisterIndex = (ushort)src_arg.Register;

                    //foreach (var _enum in Enum.GetValues(typeof(RenderMethodTemplate.RenderMethodExtern)))
                    //{
                    //    if(_enum.ToString().ToLower() == src_arg.Name)
                    //    {
                    //        argument_mapping.ArgumentIndex = (byte)_enum;
                    //        break;
                    //    }
                    //}

                    //argument_mappings.Add(argument_mapping);

                    //register_offsets.RenderMethodExternArguments_Count++;
                }

                registerOffsets.SamplerArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                var srcSamplerArguments = shader_gen_result.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.TextureSampler_Arguments);
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping();
                    argumentMapping.RegisterIndex = (ushort)samplerRegister.Register;
                    argumentMapping.ArgumentIndex = (byte)registerOffsets.SamplerArguments_Count++;

                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument();
                    shaderArgument.Name = CacheContext.GetStringId(samplerRegister.Name);
                    rmt2.SamplerArguments.Add(shaderArgument);

                }

                registerOffsets.VectorArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                // add xform args
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    int index = GetArgumentIndex(samplerRegister.Name, rmt2.VectorArguments);
                    HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister xformRegister = null;
                    foreach(var register in shader_gen_result.Registers)
                    {
                        if(register.Name == $"{samplerRegister.Name}_xform")
                        {
                            xformRegister = register;
                            break;
                        }
                    }
                    if (xformRegister == null) continue;

                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping();
                    argumentMapping.RegisterIndex = (ushort)xformRegister.Register;

                    argumentMapping.ArgumentIndex = (byte)(index != -1 ? index : rmt2.VectorArguments.Count);
                    registerOffsets.VectorArguments_Count++;
                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument();
                    shaderArgument.Name = CacheContext.GetStringId(samplerRegister.Name);
                    rmt2.VectorArguments.Add(shaderArgument);
                }

                var srcVectorArguments = shader_gen_result.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.Vector_Arguments);
                foreach (var src_arg in srcVectorArguments)
                {
                    if (src_arg.IsXFormArgument) continue; // we've already added these
                    var argument_mapping = new RenderMethodTemplate.ArgumentMapping();
                    argument_mapping.RegisterIndex = (ushort)src_arg.Register;
                    argument_mapping.ArgumentIndex = (byte)registerOffsets.VectorArguments_Count++;

                    rmt2.ArgumentMappings.Add(argument_mapping);
                }
            }




            {
                //TODO Hookup HaloShaderGenerator here
                var shader_instance = CacheContext.GetTag<Shader>(@"shaders\invalid");
                var shader = CacheContext.Deserialize<Shader>(new TagSerializationContext(cacheStream, CacheContext, shader_instance));

                var new_rmt2_instance = shader.ShaderProperties[0].Template;
                var new_rmt2 = CacheContext.Deserialize<RenderMethodTemplate>(new TagSerializationContext(cacheStream, CacheContext, new_rmt2_instance));

                //rmt2.DrawModes = new_rmt2.DrawModes;
                //rmt2.RegisterOffsets = new_rmt2.RegisterOffsets;
                //rmt2.ArgumentMappings = new_rmt2.ArgumentMappings;
                //rmt2.VectorArguments = new_rmt2.VectorArguments;
                //rmt2.IntegerArguments = new_rmt2.IntegerArguments;
                //rmt2.BooleanArguments = new_rmt2.BooleanArguments;
                //rmt2.SamplerArguments = new_rmt2.SamplerArguments;



                //rmt2 = new_rmt2;

                //rmt2.PixelShader = newPIXLInstance;


                //// update existing PIXL shader

                //CachedTagInstance current_pixl_instance = rmt2.PixelShader;
                //var current_pixl = CacheContext.Deserialize<PixelShader>(new TagSerializationContext(cacheStream, CacheContext, current_pixl_instance));

                //var shader_offset = current_pixl.DrawModes[12].Offset;



                //current_pixl.Shaders[shader_offset].PCShaderBytecode = shader_gen_result.Bytecode;

                //CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, current_pixl_instance), current_pixl);
            }



            CacheContext.TagNames[newPIXLInstance.Index] = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newPIXLInstance), pixl);
            CacheContext.TagNames[newVTSHInstance.Index] = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, newVTSHInstance), vtsh);
            CacheContext.TagNames[rmt2Instance.Index] = template_name;
            CacheContext.Serialize(new TagSerializationContext(cacheStream, CacheContext, rmt2Instance), rmt2);

            CacheContext.SaveTagNames();
        }

        public static PixelShaderBlock GeneratePixelShaderBlock(HaloOnlineCacheContext cacheContext, ShaderGeneratorResult shader_gen_result)
        {
            var pixelShaderBlock = new PixelShaderBlock();

            pixelShaderBlock.PCShaderBytecode = shader_gen_result.Bytecode;
            pixelShaderBlock.PCParameters = new List<ShaderParameter>();

            foreach (var register in shader_gen_result.Registers)
            {
                ShaderParameter shaderParameter = new ShaderParameter();
                shaderParameter.RegisterIndex = (ushort)register.Register;
                shaderParameter.RegisterCount = (byte)register.Size;

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