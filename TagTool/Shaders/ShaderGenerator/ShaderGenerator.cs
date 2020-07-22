using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloShaderGenerator;
using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Globals;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders.ShaderGenerator
{
    public abstract class RenderMethodTemplateGenerator
    {
        protected GameCache Cache;
        protected RenderMethodDefinition Definition;

        public RenderMethodTemplateGenerator(GameCache cache, RenderMethodDefinition renderMethodDefinition)
        {
            Definition = renderMethodDefinition;
            Cache = cache;
        }
    }

    public partial class ShaderGenerator
    {
        private static StringId AddString(GameCache cache, string str)
        {
            if (str == "")
                return StringId.Invalid;
            var stringId = cache.StringTable.GetStringId(str);
            if (stringId == StringId.Invalid)
                stringId = cache.StringTable.AddString(str);
            return stringId;
        }

        private static List<ShaderParameter> GenerateShaderParametersFromGenerator(GameCache cache, ShaderGeneratorResult result)
        {
            var parameters = new List<ShaderParameter>();
            foreach (var register in result.Registers)
            {
                ShaderParameter shaderParameter = new ShaderParameter
                {
                    RegisterIndex = (ushort)register.Register,
                    RegisterCount = (byte)register.Size
                };

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

                shaderParameter.ParameterName = AddString(cache, register.Name);

                parameters.Add(shaderParameter);
            }
            return parameters;
        }

        private static PixelShaderBlock GeneratePixelShaderBlock(GameCache cache, ShaderGeneratorResult result)
        {
            if (result == null)
                return null;
            var pixelShaderBlock = new PixelShaderBlock
            {
                PCShaderBytecode = result.Bytecode,
                PCParameters = GenerateShaderParametersFromGenerator(cache, result),
                Unknown8 = 1
            };

            return pixelShaderBlock;
        }

        private static VertexShaderBlock GenerateVertexShaderBlock(GameCache cache, ShaderGeneratorResult result)
        {
            if (result == null)
                return null;
            var vertexShaderBlock = new VertexShaderBlock
            {
                PCShaderBytecode = result.Bytecode,
                PCParameters = GenerateShaderParametersFromGenerator(cache, result)
            };

            return vertexShaderBlock;
        }

        public static GlobalVertexShader GenerateSharedVertexShader(GameCache cache, IShaderGenerator generator)
        {
            var glvs = new GlobalVertexShader { VertexTypes = new List<GlobalVertexShader.VertexTypeShaders>(), Shaders = new List<VertexShaderBlock>() };

            foreach (VertexType vertex in Enum.GetValues(typeof(VertexType)))
            {
                var vertexBlock = new GlobalVertexShader.VertexTypeShaders { DrawModes = new List<GlobalVertexShader.VertexTypeShaders.DrawMode>() };
                glvs.VertexTypes.Add(vertexBlock);
                if (generator.IsVertexFormatSupported(vertex))
                {
                    foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
                    {
                        var entryBlock = new GlobalVertexShader.VertexTypeShaders.DrawMode { ShaderIndex = -1 };
                        
                        vertexBlock.DrawModes.Add(entryBlock);
                        if (generator.IsEntryPointSupported(entryPoint) && generator.IsVertexShaderShared(entryPoint))
                        {
                            entryBlock.ShaderIndex = glvs.Shaders.Count;
                            var result = generator.GenerateSharedVertexShader(vertex, entryPoint);
                            glvs.Shaders.Add(GenerateVertexShaderBlock(cache, result));
                        }
                    }
                }
            }

            return glvs;
        }

        public static VertexShader GenerateVertexShader(GameCache cache, IShaderGenerator generator)
        {
            var vtsh = new VertexShader { SupportedVertexTypes = new List<VertexShader.ShaderVertexBlock>(), Shaders = new List<VertexShaderBlock>() };

            foreach(VertexType vertex in Enum.GetValues(typeof(VertexType)))
            {
                var vertexBlock = new VertexShader.ShaderVertexBlock { EntryPointShaders = new List<ShaderEntryPointBlock>() };
                vtsh.SupportedVertexTypes.Add(vertexBlock);
                if (generator.IsVertexFormatSupported(vertex))
                {
                    foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
                    {
                        var entryBlock = new ShaderEntryPointBlock();
                        vertexBlock.EntryPointShaders.Add(entryBlock);
                        if (generator.IsEntryPointSupported(entryPoint) && !generator.IsVertexShaderShared(entryPoint))
                        {
                            entryBlock.Count = 1;
                            entryBlock.Offset = (byte)vtsh.Shaders.Count;
                            var result = generator.GenerateVertexShader(vertex, entryPoint);
                            vtsh.Shaders.Add(GenerateVertexShaderBlock(cache, result));
                        }
                    }
                }
            }

            if (vtsh.Shaders.Count == 0)
                vtsh.SupportedVertexTypes = null;

            return vtsh;
        }

        public static GlobalPixelShader GenerateSharedPixelShader(GameCache cache, IShaderGenerator generator)
        {
            var glps = new GlobalPixelShader { EntryPoints = new List<GlobalPixelShader.EntryPointBlock>(), Shaders = new List<PixelShaderBlock>() };
            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                var entryPointBlock = new GlobalPixelShader.EntryPointBlock {ShaderIndex = -1 };
                glps.EntryPoints.Add(entryPointBlock);
                if (generator.IsEntryPointSupported(entryPoint) && generator.IsPixelShaderShared(entryPoint))
                {
                    for (int i = 0; i < generator.GetMethodCount(); i++)
                    {
                        if(generator.IsMethodSharedInEntryPoint(entryPoint, i))
                        {
                            entryPointBlock.Option = new List<GlobalPixelShader.EntryPointBlock.OptionBlock>();
                            var optionBlock = new GlobalPixelShader.EntryPointBlock.OptionBlock { RenderMethodOptionIndex = (short)i, OptionMethodShaderIndices = new List<int>() };
                            entryPointBlock.Option.Add(optionBlock);
                            for (int option = 0; i < generator.GetMethodOptionCount(i); option++)
                            {
                                optionBlock.OptionMethodShaderIndices.Add(glps.Shaders.Count);
                                var result = generator.GenerateSharedPixelShader(entryPoint, i, option);
                                glps.Shaders.Add(GeneratePixelShaderBlock(cache, result));
                            }
                        }
                    }
                }
            }
            return glps;
        }

        public static PixelShader GeneratePixelShader(GameCache cache, IShaderGenerator generator)
        {
            var pixl = new PixelShader {EntryPointShaders = new List<ShaderEntryPointBlock>(), Shaders = new List<PixelShaderBlock>() };

            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                var entryBlock = new ShaderEntryPointBlock();
                pixl.EntryPointShaders.Add(entryBlock);
                if(generator.IsEntryPointSupported(entryPoint) && !generator.IsPixelShaderShared(entryPoint))
                {
                    entryBlock.Count = 1;
                    entryBlock.Offset = (byte)pixl.Shaders.Count;
                    var result = generator.GeneratePixelShader(entryPoint);
                    pixl.Shaders.Add(GeneratePixelShaderBlock(cache, result));
                }
            }
            return pixl;
        }

        public static RenderMethodDefinition GenerateRenderMethodDefinition(GameCache cache, Stream cacheStream, IShaderGenerator generator, string shaderType, out GlobalPixelShader glps, out GlobalVertexShader glvs)
        {
            var rmdf = new RenderMethodDefinition();

            glps = GenerateSharedPixelShader(cache, generator);
            glvs = GenerateSharedVertexShader(cache, generator);

            var glpsTag = cache.TagCache.AllocateTag<GlobalPixelShader>($"shaders\\{shaderType.ToLower()}_shared_pixel_shaders");
            cache.Serialize(cacheStream, glpsTag, glps);
            rmdf.GlobalPixelShader = glpsTag;

            var glvsTag = cache.TagCache.AllocateTag<GlobalVertexShader>($"shaders\\{shaderType.ToLower()}_shared_vertex_shaders");
            cache.Serialize(cacheStream, glvsTag, glvs);
            rmdf.GlobalVertexShader = glvsTag;
            

            rmdf.DrawModes = new List<RenderMethodDefinition.DrawMode>();
            rmdf.Vertices = new List<RenderMethodDefinition.VertexBlock>();

            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
                if (generator.IsEntryPointSupported(entryPoint))
                    rmdf.DrawModes.Add(new RenderMethodDefinition.DrawMode { Mode = (int)entryPoint });

            foreach (VertexType vertexType in Enum.GetValues(typeof(VertexType)))
                if (generator.IsVertexFormatSupported(vertexType))
                    rmdf.Vertices.Add(new RenderMethodDefinition.VertexBlock { VertexType = (short)vertexType });

            // hackfix for methods
            if (shaderType == "black")
            {
                rmdf.RenderMethodOptions = cache.TagCache.GetTag(@"shaders\shader_options\global_shader_options", "rmop");

                rmdf.Methods = new List<RenderMethodDefinition.Method>();

                var blacknessStringid = cache.StringTable.GetStringId("blackness(no_options)");
                if (blacknessStringid == StringId.Invalid)
                {
                    blacknessStringid = cache.StringTable.AddString("blackness(no_options)");
                }
                rmdf.Methods.Add(new RenderMethodDefinition.Method { Type = blacknessStringid, VertexShaderMethodMacroName = StringId.Invalid, PixelShaderMethodMacroName = StringId.Invalid });
            }


            return rmdf;
        }

        private static int GetArgumentIndex(GameCache cache, string name, List<RenderMethodTemplate.ShaderArgument> args)
        {
            int index = -1;
            for (int i = 0; i < args.Count; i++)
            {
                var varg = args[i];
                if (name == cache.StringTable.GetString(varg.Name))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private static List<RenderMethodTemplate.ParameterMapping> MapParameters(GameCache cache, ParameterUsage usage, ShaderParameters parameters, Dictionary<string, int> shaderRegisterMapping, List<RenderMethodTemplate.ShaderArgument> parameterNames)
        {
            var result = new List<RenderMethodTemplate.ParameterMapping>();
            List<HaloShaderGenerator.Globals.ShaderParameter> shaderParameters;
            switch (usage)
            {
                case ParameterUsage.PS_Real:
                    shaderParameters = parameters.GetRealPixelParameters();
                    break;

                case ParameterUsage.VS_Real:
                    shaderParameters = parameters.GetRealVertexParameters();
                    break;

                case ParameterUsage.PS_Integer:
                    shaderParameters = parameters.GetIntegerPixelParameters();
                    break;

                case ParameterUsage.VS_Integer:
                    shaderParameters = parameters.GetIntegerVertexParameters();
                    break;

                case ParameterUsage.PS_Boolean:
                    shaderParameters = parameters.GetBooleanPixelParameters();
                    break;

                case ParameterUsage.VS_Boolean:
                    shaderParameters = parameters.GetBooleanVertexParameters();
                    break;

                case ParameterUsage.Texture:
                    shaderParameters = parameters.GetSamplerPixelParameters();
                    break;
                default:
                    shaderParameters = new List<HaloShaderGenerator.Globals.ShaderParameter>();
                    break;
            }
            foreach (var parameter in shaderParameters)
            {
                var argumentMapping = new RenderMethodTemplate.ParameterMapping();
                var registerName = parameter.RegisterName;
                if (shaderRegisterMapping.ContainsKey(registerName))
                {
                    argumentMapping.RegisterIndex = (ushort)shaderRegisterMapping[registerName];
                    argumentMapping.ArgumentIndex = (byte)GetArgumentIndex(cache, parameter.ParameterName, parameterNames);
                }
                else
                {
                    // Console.WriteLine($"Failed to find {usage} register parameter for {parameter.ParameterName}");
                    continue; // skip parameter, not present in shader
                }
                result.Add(argumentMapping);
            }
            return result;
        }

        private static List<RenderMethodTemplate.ParameterMapping> MapExternParameters(ParameterUsage usage, ShaderParameters parameters, ShaderParameters globalParameters, Dictionary<string, int> shaderRegisterMapping)
        {
            var result = new List<RenderMethodTemplate.ParameterMapping>();
            List<HaloShaderGenerator.Globals.ShaderParameter> shaderParameters;
            switch (usage)
            {
                case ParameterUsage.PS_RealExtern:
                    shaderParameters = parameters.GetRealExternPixelParameters();
                    shaderParameters.AddRange(globalParameters.GetRealExternPixelParameters());
                    break;
                case ParameterUsage.PS_IntegerExtern:
                    shaderParameters = parameters.GetIntegerExternPixelParameters();
                    shaderParameters.AddRange(globalParameters.GetIntegerExternPixelParameters());
                    break;
                case ParameterUsage.TextureExtern:
                    shaderParameters = parameters.GetSamplerExternPixelParameters();
                    shaderParameters.AddRange(globalParameters.GetSamplerExternPixelParameters());
                    break;

                case ParameterUsage.VS_RealExtern:
                    shaderParameters = parameters.GetRealExternVertexParameters();
                    shaderParameters.AddRange(globalParameters.GetRealExternVertexParameters());
                    break;
                case ParameterUsage.VS_IntegerExtern:
                    shaderParameters = parameters.GetIntegerExternVertexParameters();
                    shaderParameters.AddRange(globalParameters.GetIntegerExternVertexParameters());
                    break;

                default:
                    shaderParameters = new List<HaloShaderGenerator.Globals.ShaderParameter>();
                    break;
            }
            foreach (var parameter in shaderParameters)
            {
                var argumentMapping = new RenderMethodTemplate.ParameterMapping();
                var registerName = parameter.RegisterName;
                if (shaderRegisterMapping.ContainsKey(registerName))
                {
                    argumentMapping.RegisterIndex = (ushort)shaderRegisterMapping[registerName];
                    argumentMapping.ArgumentIndex = (byte)parameter.RenderMethodExtern; // use the enum integer value
                }
                else
                {
                    // Console.WriteLine($"Failed to find {usage} register parameter for {parameter.ParameterName}");
                    continue; // skip parameter, not present in shader
                }
                result.Add(argumentMapping);
            }
            return result;
        }

        private static void AddMapping(ParameterUsage usage, RenderMethodTemplate rmt2, RenderMethodTemplate.ParameterTable table, List<RenderMethodTemplate.ParameterMapping> mappings)
        {
            if(mappings.Count > 0)
            {
                table[usage] = new RenderMethodTemplate.PackedInteger_10_6
                {
                    Offset = (ushort)rmt2.Parameters.Count,
                    Count = (ushort)mappings.Count
                };
                rmt2.Parameters.AddRange(mappings);
            }
        }

        public static RenderMethodTemplate GenerateRenderMethodTemplate(GameCache cache, Stream cacheStream, RenderMethodDefinition rmdf, GlobalPixelShader glps, GlobalVertexShader glvs, IShaderGenerator generator, string shaderName)
        {

            var rmt2 = new RenderMethodTemplate();

            var pixl = GeneratePixelShader(cache, generator);
            var vtsh = GenerateVertexShader(cache, generator);


            if (!cache.TagCache.TryGetTag(shaderName + ".pixl", out var pixlTag))
                pixlTag = cache.TagCache.AllocateTag<PixelShader>(shaderName);
            cache.Serialize(cacheStream, pixlTag, pixl);
            rmt2.PixelShader = pixlTag;

            if (!cache.TagCache.TryGetTag(shaderName + ".vtsh", out var vtshTag))
                vtshTag = cache.TagCache.AllocateTag<VertexShader>(shaderName);
            cache.Serialize(cacheStream, vtshTag, vtsh);
            rmt2.VertexShader = vtshTag;


            foreach (ShaderStage mode in Enum.GetValues(typeof(ShaderStage)))
                if (generator.IsEntryPointSupported(mode))
                    rmt2.ValidEntryPoints |= (EntryPointBitMask)(1 << (int)mode);

            #region build parameter names block using the generator

            rmt2.RealParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.TextureParameterNames = new List<RenderMethodTemplate.ShaderArgument>();

            var pixelShaderParameters = generator.GetPixelShaderParameters();
            var vertexShaderParameters = generator.GetVertexShaderParameters();
            var globalShaderParameters = generator.GetGlobalParameters();

            
            foreach (var realParam in pixelShaderParameters.GetRealPixelParameters())
                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, realParam.ParameterName) });

            foreach (var realParam in vertexShaderParameters.GetRealVertexParameters())
                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, realParam.ParameterName) });

            foreach (var boolParam in pixelShaderParameters.GetBooleanPixelParameters())
                rmt2.BooleanParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, boolParam.ParameterName) });

            foreach (var boolParam in vertexShaderParameters.GetBooleanVertexParameters())
                rmt2.BooleanParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, boolParam.ParameterName) });

            foreach (var intParam in pixelShaderParameters.GetIntegerPixelParameters())
                rmt2.IntegerParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, intParam.ParameterName) });

            foreach (var intParam in vertexShaderParameters.GetIntegerVertexParameters())
                rmt2.IntegerParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, intParam.ParameterName) });

            foreach (var samplerParam in pixelShaderParameters.GetSamplerPixelParameters())
                rmt2.TextureParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, samplerParam.ParameterName) });

            #endregion

            rmt2.Parameters = new List<RenderMethodTemplate.ParameterMapping>();
            rmt2.ParameterTables = new List<RenderMethodTemplate.ParameterTable>();
            rmt2.EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();

            foreach (ShaderStage mode in Enum.GetValues(typeof(ShaderStage)))
            {
                var rmt2Drawmode = new RenderMethodTemplate.PackedInteger_10_6();

                if(generator.IsEntryPointSupported(mode))
                {
                    while (rmt2.EntryPoints.Count < (int)mode) // makeup count, this is to prevent all entry points being added
                        rmt2.EntryPoints.Add(new RenderMethodTemplate.PackedInteger_10_6());

                    rmt2.EntryPoints.Add(rmt2Drawmode);
                    rmt2Drawmode.Offset = (ushort)rmt2.ParameterTables.Count();
                    rmt2Drawmode.Count = 1;


                    var registerOffsets = new RenderMethodTemplate.ParameterTable();

                    for (int i = 0; i < registerOffsets.Values.Length; i++)
                        registerOffsets.Values[i] = new RenderMethodTemplate.PackedInteger_10_6();

                    rmt2.ParameterTables.Add(registerOffsets);

                    // find pixel shader and vertex shader block loaded by this entry point

                    PixelShaderBlock pixelShader;
                    VertexShaderBlock vertexShader;


                    if (generator.IsVertexShaderShared(mode))
                        vertexShader = glvs.Shaders[glvs.VertexTypes[rmdf.Vertices[0].VertexType].DrawModes[(int)mode].ShaderIndex];
                    else
                        vertexShader = vtsh.Shaders[vtsh.SupportedVertexTypes[rmdf.Vertices[0].VertexType].EntryPointShaders[(int)mode].Offset];

                    if (generator.IsPixelShaderShared(mode))
                    {
                        if(glps.EntryPoints[(int)mode].ShaderIndex == -1)
                        {
                            // assumes shared pixel shader are only used for a single method, otherwise unknown procedure to obtain one or more pixel shader block
                            var optionValue = generator.GetMethodOptionValue(glps.EntryPoints[(int)mode].Option[0].RenderMethodOptionIndex);
                            pixelShader = glps.Shaders[glps.EntryPoints[(int)mode].Option[0].OptionMethodShaderIndices[optionValue]];
                        }
                        else
                            pixelShader = glps.Shaders[glps.EntryPoints[(int)mode].ShaderIndex];
                    }
                    else
                        pixelShader = pixl.Shaders[pixl.EntryPointShaders[(int)mode].Offset];

                    

                    // build dictionary register name to register index, speeds lookup time

                    Dictionary<string, int> pixelShaderRegisterMapping = new Dictionary<string, int>();
                    Dictionary<string, int> vertexShaderRegisterMapping = new Dictionary<string, int>();

                    foreach(var reg in pixelShader.PCParameters)
                        pixelShaderRegisterMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;

                    foreach (var reg in vertexShader.PCParameters)
                        vertexShaderRegisterMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;

                    // build parameter table and registers available for this entry point, order to be determiend

                    List<RenderMethodTemplate.ParameterMapping> mappings;
                    ParameterUsage currentUsage;

                    currentUsage = ParameterUsage.TextureExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderRegisterMapping);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.Texture;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderRegisterMapping, rmt2.TextureParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    // ps

                    currentUsage = ParameterUsage.PS_Real;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderRegisterMapping, rmt2.RealParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.PS_Integer;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderRegisterMapping, rmt2.IntegerParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.PS_Boolean;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderRegisterMapping, rmt2.BooleanParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    // vs

                    currentUsage = ParameterUsage.VS_Real;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderRegisterMapping, rmt2.RealParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.VS_Integer;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderRegisterMapping, rmt2.IntegerParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.VS_Boolean;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderRegisterMapping, rmt2.BooleanParameterNames);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    // extern

                    currentUsage = ParameterUsage.PS_RealExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderRegisterMapping);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.PS_IntegerExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderRegisterMapping);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    // vs extern

                    
                    currentUsage = ParameterUsage.VS_RealExtern;
                    mappings = MapExternParameters(currentUsage, vertexShaderParameters, globalShaderParameters, vertexShaderRegisterMapping);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);

                    currentUsage = ParameterUsage.VS_IntegerExtern;
                    mappings = MapExternParameters(currentUsage, vertexShaderParameters, globalShaderParameters, vertexShaderRegisterMapping);
                    AddMapping(currentUsage, rmt2, registerOffsets, mappings);
                    
                    
                }

            }
            
            return rmt2;
        }
    }

    
}
