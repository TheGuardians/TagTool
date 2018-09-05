using HaloShaderGenerator;
using HaloShaderGenerator.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    static class RMT2Generator
    {
        #region GenerateRenderMethodTemplate Boilerplate

        public static void GenerateRenderMethodTemplate(
            RenderMethod renderMethod,
            HaloOnlineCacheContext cacheContext,
            Stream cacheStream,
            out CachedTagInstance rmt2Instance,
            out CachedTagInstance pixlInstance,
            out CachedTagInstance vtshInstance,
            out RenderMethodTemplate rmt2,
            out PixelShader pixl,
            out VertexShader vtsh,
            out List<RenderMethodOption.OptionBlock> templateOptions
            )
        {
            CreateOptionsLists(
                renderMethod,
                cacheContext,
                cacheStream,
                out List<int> templateOptionIndices,
                out templateOptions);

            ShaderType type;
            switch (renderMethod)
            {
                case ShaderCortana shaderCortana:
                    type = ShaderType.Cortana;
                    break;
                default:
                    throw new NotImplementedException();
            }

            GenerateRenderMethodTemplate(
                type,
                templateOptionIndices,
                cacheContext,
                cacheStream,
                out rmt2Instance,
                out pixlInstance,
                out vtshInstance,
                out rmt2,
                out pixl,
                out vtsh);
        }

        public static void GenerateRenderMethodTemplate(
            ShaderType shader_type,
            List<int> options,
            HaloOnlineCacheContext cacheContext,
            Stream cacheStream,
            out CachedTagInstance rmt2Instance,
            out CachedTagInstance pixlInstance,
            out CachedTagInstance vtshInstance,
            out RenderMethodTemplate rmt2,
            out PixelShader pixl,
            out VertexShader vtsh)
        {
            var templateName = CreateRenderMethodTemplateName(shader_type, options);
            rmt2 = new RenderMethodTemplate();
            pixl = new PixelShader();
            vtsh = new VertexShader();

            GenerateRenderMethodTemplateInternal(
                shader_type,
                options,
                cacheContext,
                cacheStream,
                rmt2,
                pixl,
                vtsh);

            rmt2Instance = cacheContext.TagCache.AllocateTag<RenderMethodTemplate>();
            pixlInstance = cacheContext.TagCache.AllocateTag<PixelShader>();
            vtshInstance = cacheContext.TagCache.AllocateTag<VertexShader>();

            rmt2.PixelShader = pixlInstance;
            rmt2.VertexShader = vtshInstance;

            cacheContext.Serialize(cacheStream, pixlInstance, pixl);
            cacheContext.Serialize(cacheStream, vtshInstance, vtsh);
            cacheContext.Serialize(cacheStream, rmt2Instance, rmt2);

            cacheContext.TagNames[pixlInstance.Index] = templateName;
            cacheContext.TagNames[vtshInstance.Index] = templateName;
            cacheContext.TagNames[rmt2Instance.Index] = templateName;

            cacheContext.SaveTagNames();
        }

        #endregion

        #region IsTemplateSupported

        public static bool IsTemplateSupported(string template_name)
        {
            switch (template_name)
            {
                case @"shaders\shader":
                    return IsTemplateSupported(ShaderType.Shader);
                case @"shaders\beam":
                    return IsTemplateSupported(ShaderType.Beam);
                case @"shaders\contrail":
                    return IsTemplateSupported(ShaderType.Contrail);
                case @"shaders\decal":
                    return IsTemplateSupported(ShaderType.Decal);
                case @"shaders\halogram":
                    return IsTemplateSupported(ShaderType.Halogram);
                case @"shaders\light_volume":
                    return IsTemplateSupported(ShaderType.LightVolume);
                case @"shaders\particle":
                    return IsTemplateSupported(ShaderType.Particle);
                case @"shaders\terrain":
                    return IsTemplateSupported(ShaderType.Terrain);
                case @"shaders\cortana":
                    return IsTemplateSupported(ShaderType.Cortana);
            }
            return false;
        }

        public static bool IsTemplateSupported(ShaderType shader_type)
        {
            foreach (ShaderStage stage in Enum.GetValues(typeof(ShaderStage)))
            {
                if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(shader_type, stage)) return true;
            }
            return false;
        }

        #endregion

        public static void CreateOptionsLists(
            RenderMethod shader,
            HaloOnlineCacheContext cacheContext,
            Stream cacheStream,
            out List<int> templateOptionsIndices,
            out List<RenderMethodOption.OptionBlock> templateOptions)
        {
            templateOptionsIndices = shader.RenderMethodDefinitionOptionIndices.Select(c => (int)c.OptionIndex).ToList();

            var rmdf_instance = shader.BaseRenderMethod;
            var rmdf = cacheContext.Deserialize<RenderMethodDefinition>(new TagSerializationContext(cacheStream, cacheContext, rmdf_instance));

            templateOptions = new List<RenderMethodOption.OptionBlock>();
            for (int i = 0; i < rmdf.Methods.Count; i++)
            {
                var method = rmdf.Methods[i];
                int selected_option_index = templateOptionsIndices.Count > i ? templateOptionsIndices[i] : 0;
                var selected_option = method.ShaderOptions[selected_option_index];

                var rmop_instance = selected_option.Option;
                if (rmop_instance != null)
                {
                    var rmop = cacheContext.Deserialize<RenderMethodOption>(new TagSerializationContext(cacheStream, cacheContext, rmop_instance));

                    templateOptions.AddRange(rmop.Options);
                }
            }
        }

        public static string CreateRenderMethodTemplateName(ShaderType shader_type, List<int> options)
        {
            string template_name = null;
            switch (shader_type)
            {
                case ShaderType.Shader:
                    template_name = "shader_templates";
                    break;
                case ShaderType.Cortana:
                    template_name = "cortana_templates";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return $@"shaders\{template_name}\_{string.Join("_", options)}";
        }

        private static ShaderStage? GetMaximumSupportedShaderStage(ShaderType shader_type)
        {
            ShaderStage? maximum_stage = null;
            foreach (ShaderStage stage in Enum.GetValues(typeof(ShaderStage)))
            {
                if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(shader_type, stage))
                {
                    maximum_stage = (ShaderStage)Math.Max((int?)stage ?? -1, (int)stage);
                }
            }
            return maximum_stage;
        }

        public static object[] CreateShaderGeneratorArguments(MethodInfo method, ShaderStage stage, IEnumerable<Int32> template)
        {
            Int32[] _template = template.ToArray();
            var _params = method.GetParameters();
            object[] input_params = new object[_params.Length];

            for (int i = 0; i < _params.Length; i++)
            {
                if (i == 0) input_params[0] = stage;
                else
                {
                    var template_index = i - 1;
                    if (template_index < _template.Length)
                    {
                        var _enum = Enum.ToObject(_params[i].ParameterType, _template[template_index]);
                        input_params[i] = _enum;
                    }
                    else
                    {
                        var _enum = Enum.ToObject(_params[i].ParameterType, 0);
                        input_params[i] = _enum;
                    }

                }
            }

            return input_params;
        }

        public static ShaderGeneratorResult InvokeHaloShaderGenerator(ShaderType shader_type, ShaderStage stage, IEnumerable<int> options)
        {
            MethodInfo method = null;
            switch (shader_type)
            {
                case ShaderType.Shader:
                    method = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShader");
                    break;
                case ShaderType.Cortana:
                    method = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShaderCortana");
                    break;
            }
            if (method == null) return null;

            var shaderArgs = CreateShaderGeneratorArguments(method, stage, options);

            var shaderGeneratorResult = method.Invoke(null, shaderArgs) as ShaderGeneratorResult;
            return shaderGeneratorResult;
        }

        private static void GenerateRenderMethodTemplateInternal(
            ShaderType shader_type,
            List<int> options,
            HaloOnlineCacheContext cacheContext,
            Stream cacheStream,
            RenderMethodTemplate rmt2,
            PixelShader pixl,
            VertexShader vtsh)
        {
            // reset rmt2
            rmt2.DrawModeBitmask = 0;
            rmt2.VectorArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.SamplerArguments = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.ArgumentMappings = new List<RenderMethodTemplate.ArgumentMapping>();
            rmt2.RegisterOffsets = new List<RenderMethodTemplate.DrawModeRegisterOffsetBlock>();
            rmt2.DrawModes = new List<RenderMethodTemplate.OffsetCount_16>();

            // reset pixl
            pixl.Shaders = new List<PixelShaderBlock>();
            pixl.DrawModes = new List<ShaderDrawMode>();

            var maximum_stages = (int)GetMaximumSupportedShaderStage(shader_type);
            foreach (ShaderStage stage in Enum.GetValues(typeof(ShaderStage)))
            {
                var pixelShaderDrawmode = new ShaderDrawMode();
                var rmt2Drawmode = new RenderMethodTemplate.OffsetCount_16();

                pixl.DrawModes.Add(pixelShaderDrawmode);
                rmt2.DrawModes.Add(rmt2Drawmode);

                // skip any unsupported stages
                if (!HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(shader_type, stage)) continue;
                if ((int)stage > maximum_stages) continue;

                rmt2.DrawModeBitmask |= (RenderMethodTemplate.ShaderModeBitmask)(1 << (int)stage);
                rmt2Drawmode.Offset = (ushort)rmt2.RegisterOffsets.Count();
                rmt2Drawmode.Count = 1;

                ShaderGeneratorResult shaderGenResult = InvokeHaloShaderGenerator(shader_type, stage, options);

                var registerOffsets = new RenderMethodTemplate.DrawModeRegisterOffsetBlock();
                registerOffsets.RenderMethodExternArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                var srcRenderMethodExternArguments = shaderGenResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.RenderMethodExtern_Arguments);

                foreach (var src_arg in srcRenderMethodExternArguments)
                {
                    var argument_mapping = new RenderMethodTemplate.ArgumentMapping();
                    argument_mapping.RegisterIndex = (ushort)src_arg.Register;

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
                var srcSamplerArguments = shaderGenResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.TextureSampler_Arguments);
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping();
                    argumentMapping.RegisterIndex = (ushort)samplerRegister.Register;
                    argumentMapping.ArgumentIndex = (byte)registerOffsets.SamplerArguments_Count++;

                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument();
                    shaderArgument.Name = cacheContext.GetStringId(samplerRegister.Name);
                    rmt2.SamplerArguments.Add(shaderArgument);

                }

                registerOffsets.VectorArguments_Offset = (ushort)rmt2.ArgumentMappings.Count;
                // add xform args
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    int index = GetArgumentIndex(cacheContext, samplerRegister.Name, rmt2.VectorArguments);
                    HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister xformRegister = null;
                    foreach (var register in shaderGenResult.Registers)
                    {
                        if (register.Name == $"{samplerRegister.Name}_xform")
                        {
                            xformRegister = register;
                            break;
                        }
                    }
                    if (xformRegister == null) continue;

                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping();
                    argumentMapping.RegisterIndex = (ushort)xformRegister.Register;

                    argumentMapping.ArgumentIndex = (byte)(index != -1 ? index : rmt2.VectorArguments.Count);
                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument();
                    shaderArgument.Name = cacheContext.GetStringId(samplerRegister.Name);
                    rmt2.VectorArguments.Add(shaderArgument);

                    registerOffsets.VectorArguments_Count++;
                }

                var srcVectorArguments = shaderGenResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.Vector_Arguments);
                foreach (var vectorRegister in srcVectorArguments)
                {
                    if (vectorRegister.IsXFormArgument) continue; // we've already added these
                    var argumentMapping = new RenderMethodTemplate.ArgumentMapping();
                    argumentMapping.RegisterIndex = (ushort)vectorRegister.Register;
                    argumentMapping.ArgumentIndex = (byte)rmt2.VectorArguments.Count;
                    rmt2.ArgumentMappings.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument();
                    shaderArgument.Name = cacheContext.GetStringId(vectorRegister.Name);
                    rmt2.VectorArguments.Add(shaderArgument);

                    registerOffsets.VectorArguments_Count++;
                }

                rmt2.RegisterOffsets.Add(registerOffsets);

                // add result to pixl tag
                pixelShaderDrawmode.Offset = (byte)pixl.Shaders.Count;
                pixelShaderDrawmode.Count = 1;
                var pixelShaderBlock = GeneratePixelShaderBlock(cacheContext, shaderGenResult);
                pixl.Shaders.Add(pixelShaderBlock);
            }
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
                    case ShaderGeneratorResult.ShaderRegister.RegisterType.Boolean:
                        shaderParameter.RegisterType = ShaderParameter.RType.Boolean;
                        break;
                    case ShaderGeneratorResult.ShaderRegister.RegisterType.Integer:
                        shaderParameter.RegisterType = ShaderParameter.RType.Integer;
                        break;
                    case ShaderGeneratorResult.ShaderRegister.RegisterType.Sampler:
                        shaderParameter.RegisterType = ShaderParameter.RType.Sampler;
                        break;
                    case ShaderGeneratorResult.ShaderRegister.RegisterType.Vector:
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

        private static int GetArgumentIndex(HaloOnlineCacheContext cacheContext, string name, List<RenderMethodTemplate.ShaderArgument> args)
        {
            int index = -1;
            for (int i = 0; i < args.Count; i++)
            {
                var varg = args[i];
                if (name == cacheContext.GetString(varg.Name))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
