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
        private void ConvertShader(RenderMethod shader, Stream cacheStream)
        {
            //var rmdf_instance = shaderCortana.BaseRenderMethod;
            //var rmdf = CacheContext.Deserialize<RenderMethodDefinition>(new TagSerializationContext(cacheStream, CacheContext, rmdf_instance));

            var shader_properties = shader.ShaderProperties[0];
            shader_properties.ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>();
            shader_properties.Arguments = new List<RenderMethod.ShaderProperty.Argument>();
            shader_properties.Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>();
            //shader_properties.DrawModes = new List<RenderMethodTemplate.DrawMode>();
            shader_properties.DrawmodeFunctionOffsets = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
            shader_properties.FunctionMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
            shader_properties.Functions = new List<RenderMethod.FunctionBlock>();

            RenderMethodTemplate rmt2 = null;
            PixelShader pixl = null;
            List<RenderMethodOption.OptionBlock> templateOptions;

            if (shader_properties.Template == null)
            {
                RMT2Generator.GenerateRenderMethodTemplate(
                    shader,
                    CacheContext,
                    cacheStream,
                    out CachedTagInstance rmt2Instance,
                    out CachedTagInstance pixlInstance,
                    out CachedTagInstance vtshInstance,
                    out RenderMethodTemplate _rmt2,
                    out PixelShader _pixl,
                    out VertexShader _vtsh,
                    out List<RenderMethodOption.OptionBlock> _templateOptions);

                shader_properties.Template = rmt2Instance;
                rmt2 = _rmt2;
                pixl = _pixl;
                templateOptions = _templateOptions;
            }
            else
            {
                rmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, shader_properties.Template);
                pixl = CacheContext.Deserialize<PixelShader>(cacheStream, rmt2.PixelShader);

                RMT2Generator.CreateOptionsLists(
                    shader,
                    CacheContext,
                    cacheStream,
                    out List<int> _templateOptionsIndices,
                    out List<RenderMethodOption.OptionBlock> _templateOptions);
                templateOptions = _templateOptions;
            }
            shader_properties.DrawModes = rmt2.DrawModes;

            var shaderFunctions = new List<RenderMethod.FunctionBlock>();
            var shaderVectorArguments = new RenderMethod.ShaderProperty.Argument[rmt2.VectorArguments.Count];

            var shaderSamplerArguments = new RenderMethod.ShaderProperty.ShaderMap[rmt2.SamplerArguments.Count];
            for (int rmt2SamplerIndex = 0; rmt2SamplerIndex < rmt2.SamplerArguments.Count; rmt2SamplerIndex++)
            {
                var rmt2SamplerArgument = rmt2.SamplerArguments[rmt2SamplerIndex];
                var name = rmt2SamplerArgument.Name;
                var name_str = CacheContext.GetString(name);
                var shaderSamplerArgument = new RenderMethod.ShaderProperty.ShaderMap();
                {
                    foreach (var importData in shader.ImportData)
                    {
                        if (importData.Type != RenderMethodOption.OptionBlock.OptionDataType.Sampler) continue;
                        if (importData.Name.Index != name.Index) continue;

                        if (importData.Unknown4 != 0)
                        {
                            shaderSamplerArgument.BitmapFlags = (byte)importData.Unknown5;
                        }


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
                        var shaderVectorArgument = ProcessArgument(rmt2SamplerArgument, shaderFunctions, templateOptions, shader);
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

                var shaderArgument = ProcessArgument(vectorArgument, shaderFunctions, templateOptions, shader);
                shaderVectorArguments[rmt2ArgumentIndex] = shaderArgument;
            }
            shader_properties.Arguments = shaderVectorArguments.ToList();
            shader_properties.Functions = shaderFunctions;

            shader_properties.DrawmodeFunctionOffsets.Add(new RenderMethod.ShaderProperty.UnknownBlock3());

            switch(shader)
            {
                case ShaderCortana shaderCortana:
                    if (shaderCortana.Material.Index == 0)
                    {
                        if (CacheContext.StringIdCache.Contains("default_material"))
                        {
                            shaderCortana.Material = CacheContext.StringIdCache.GetStringId("default_material");
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
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
            List<RenderMethod.FunctionBlock> shaderFunctions,
            List<RenderMethodOption.OptionBlock> templateOptions,
            RenderMethod shader)
        {
            RenderMethod.ShaderProperty.Argument shaderArgument = new RenderMethod.ShaderProperty.Argument();

            var name = vectorArgument.Name;
            var nameStr = CacheContext.GetString(name);

            foreach (var importData in shader.ImportData)
            {
                if (importData.Name.Index != name.Index) continue;

                var argument_data = importData.Functions.Count > 0 ? importData.Functions[0].Function.Data : null;
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
                        case RenderMethodOption.OptionBlock.OptionDataType.IntegerColor:
                            shaderArgument.Arg0 = 1.0f;
                            shaderArgument.Arg1 = 1.0f;
                            shaderArgument.Arg2 = 1.0f;
                            shaderArgument.Arg3 = 1.0f;
                            break;
                        case RenderMethodOption.OptionBlock.OptionDataType.Float:
                        case RenderMethodOption.OptionBlock.OptionDataType.Float4:
                            shaderArgument.Arg0 = 0.0f;
                            shaderArgument.Arg1 = 0.0f;
                            shaderArgument.Arg2 = 0.0f;
                            shaderArgument.Arg3 = 0.0f;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                for (int functionIndex = 1; functionIndex < importData.Functions.Count; functionIndex++)
                {
                    shaderFunctions.Add(importData.Functions[functionIndex]);
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






    }
}