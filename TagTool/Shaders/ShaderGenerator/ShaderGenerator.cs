using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HaloShaderGenerator;
using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Globals;
using HaloShaderGenerator.Shared;
using HaloShaderGenerator.TemplateGenerator;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.RenderMethodDefinition;

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

        public static PixelShaderBlock GeneratePixelShaderBlock(GameCache cache, ShaderGeneratorResult result)
        {
            if (result == null)
                return null;
            var pixelShaderBlock = new PixelShaderBlock
            {
                PCShaderBytecode = result.Bytecode,
                PCConstantTable = new ShaderConstantTable
                {
                    Constants = GenerateShaderParametersFromGenerator(cache, result),
                    ShaderType = ShaderType.PixelShader
                }   
            };

            return pixelShaderBlock;
        }

        public static VertexShaderBlock GenerateVertexShaderBlock(GameCache cache, ShaderGeneratorResult result)
        {
            if (result == null)
                return null;
            var vertexShaderBlock = new VertexShaderBlock
            {
                PCShaderBytecode = result.Bytecode,
                PCConstantTable = new ShaderConstantTable()
                {
                    Constants = GenerateShaderParametersFromGenerator(cache, result),
                    ShaderType = ShaderType.VertexShader
                }
            };

            return vertexShaderBlock;
        }

        public static GlobalVertexShader GenerateSharedVertexShader(GameCache cache, IShaderGenerator generator)
        {
            var glvs = new GlobalVertexShader { VertexTypes = new List<GlobalVertexShader.VertexTypeShaders>(), Shaders = new List<VertexShaderBlock>() };

            foreach (VertexType vertex in Enum.GetValues(typeof(VertexType)))
            {
                var vertexBlock = new GlobalVertexShader.VertexTypeShaders { EntryPoints = new List<GlobalVertexShader.VertexTypeShaders.GlobalShaderEntryPointBlock>() };
                glvs.VertexTypes.Add(vertexBlock);
                if (generator.IsVertexFormatSupported(vertex))
                {
                    foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
                    {
                        var entryBlock = new GlobalVertexShader.VertexTypeShaders.GlobalShaderEntryPointBlock { ShaderIndex = -1 };
                        
                        vertexBlock.EntryPoints.Add(entryBlock);
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
            var vtsh = new VertexShader { EntryPoints = new List<VertexShader.VertexShaderEntryPoint>(), Shaders = new List<VertexShaderBlock>() };

            foreach(VertexType vertex in Enum.GetValues(typeof(VertexType)))
            {
                var vertexBlock = new VertexShader.VertexShaderEntryPoint { SupportedVertexTypes = new List<ShortOffsetCountBlock>() };
                vtsh.EntryPoints.Add(vertexBlock);
                if (generator.IsVertexFormatSupported(vertex))
                {
                    foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
                    {
                        var entryBlock = new ShortOffsetCountBlock();
                        vertexBlock.SupportedVertexTypes.Add(entryBlock);
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
                vtsh.EntryPoints = null;

            return vtsh;
        }

        public static GlobalPixelShader GenerateSharedPixelShader(GameCache cache, IShaderGenerator generator)
        {
            var glps = new GlobalPixelShader { EntryPoints = new List<GlobalPixelShader.EntryPointBlock>(), Shaders = new List<PixelShaderBlock>() };
            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                var entryPointBlock = new GlobalPixelShader.EntryPointBlock { DefaultCompiledShaderIndex = -1 };
                glps.EntryPoints.Add(entryPointBlock);

                if (generator.IsEntryPointSupported(entryPoint) && 
                    generator.IsPixelShaderShared(entryPoint))
                {
                    if (generator.IsSharedPixelShaderWithoutMethod(entryPoint))
                    {
                        entryPointBlock.DefaultCompiledShaderIndex = glps.Shaders.Count;
                        var result = generator.GenerateSharedPixelShader(entryPoint, 0, 0);
                        glps.Shaders.Add(GeneratePixelShaderBlock(cache, result));
                    }

                    else if (generator.IsSharedPixelShaderUsingMethods(entryPoint))
                    {
                        for (int i = 0; i < generator.GetMethodCount(); i++)
                        {
                            if (generator.IsMethodSharedInEntryPoint(entryPoint, i))
                            {
                                entryPointBlock.CategoryDependency = new List<GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock>();

                                var optionBlock = new GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock 
                                { 
                                    DefinitionCategoryIndex = (short)i, 
                                    OptionDependency = new List<GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock.GlobalShaderOptionDependency>()
                                };

                                entryPointBlock.CategoryDependency.Add(optionBlock);

                                for (int option = 0; option < generator.GetMethodOptionCount(i); option++)
                                {
                                    optionBlock.OptionDependency.Add(new GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock.GlobalShaderOptionDependency { CompiledShaderIndex = glps.Shaders.Count });
                                    var result = generator.GenerateSharedPixelShader(entryPoint, i, option);
                                    glps.Shaders.Add(GeneratePixelShaderBlock(cache, result));
                                }
                            }
                        }
                    }
                }
            }
            return glps;
        }

        public static PixelShader GeneratePixelShader(GameCache cache, IShaderGenerator generator)
        {
            var pixl = new PixelShader {EntryPointShaders = new List<ShortOffsetCountBlock>(), Shaders = new List<PixelShaderBlock>() };

            Dictionary<Task<ShaderGeneratorResult>, int> tasks = new Dictionary<Task<ShaderGeneratorResult>, int>(); // <task, entry point>


            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                var entryBlock = new ShortOffsetCountBlock();
                pixl.EntryPointShaders.Add(entryBlock);

                if (generator.IsEntryPointSupported(entryPoint) && !generator.IsPixelShaderShared(entryPoint))
                {
                    Task<ShaderGeneratorResult> generatorTask = Task.Run(() => { return generator.GeneratePixelShader(entryPoint); });
                    tasks.Add(generatorTask, (int)entryPoint);
                }
            }

            Task.WaitAll(tasks.Keys.ToArray());

            foreach (var task in tasks)
            {
                pixl.EntryPointShaders[task.Value].Count = 1;
                pixl.EntryPointShaders[task.Value].Offset = (byte)pixl.Shaders.Count;
                pixl.Shaders.Add(GeneratePixelShaderBlock(cache, task.Key.Result));
            }
            return pixl;
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

        private static List<RenderMethodTemplate.RoutingInfoBlock> MapParameters(GameCache cache, ParameterUsage usage, ShaderParameters parameters, Dictionary<string, int> shaderRegisterMapping, List<RenderMethodTemplate.ShaderArgument> parameterNames)
        {
            var result = new List<RenderMethodTemplate.RoutingInfoBlock>();
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
                    shaderParameters = parameters.GetSamplerParameters();
                    break;
                default:
                    shaderParameters = new List<HaloShaderGenerator.Globals.ShaderParameter>();
                    break;
            }
            foreach (var parameter in shaderParameters)
            {
                bool vertex = (usage == ParameterUsage.Texture || usage == ParameterUsage.TextureExtern) && parameter.Flags.HasFlag(ShaderParameterFlags.IsVertexShader);
                var argumentMapping = new RenderMethodTemplate.RoutingInfoBlock();
                var registerName = vertex ? parameter.RegisterName + "_VERTEX_" : parameter.RegisterName;
                if (shaderRegisterMapping.ContainsKey(registerName))
                {
                    argumentMapping.DestinationIndex = (ushort)shaderRegisterMapping[registerName];
                    argumentMapping.SourceIndex = (byte)GetArgumentIndex(cache, parameter.ParameterName, parameterNames);
                    argumentMapping.Flags = (byte)(vertex ? 1 : 0);
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

        private static List<RenderMethodTemplate.RoutingInfoBlock> MapExternParameters(ParameterUsage usage, ShaderParameters parameters, ShaderParameters globalParameters, Dictionary<string, int> shaderRegisterMapping)
        {
            var result = new List<RenderMethodTemplate.RoutingInfoBlock>();
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
                var argumentMapping = new RenderMethodTemplate.RoutingInfoBlock();
                var registerName = parameter.RegisterName;
                if (shaderRegisterMapping.ContainsKey(registerName))
                {
                    argumentMapping.DestinationIndex = (ushort)shaderRegisterMapping[registerName];
                    argumentMapping.SourceIndex = (byte)parameter.RenderMethodExtern; // use the enum integer value
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

        private static void AddMapping(ParameterUsage usage, RenderMethodTemplate rmt2, RenderMethodTemplate.PassBlock table, List<RenderMethodTemplate.RoutingInfoBlock> mappings)
        {
            if(mappings.Count > 0)
            {
                table[usage] = new RenderMethodTemplate.TagBlockIndex
                {
                    Offset = (ushort)rmt2.RoutingInfo.Count,
                    Count = (ushort)mappings.Count
                };
                rmt2.RoutingInfo.AddRange(mappings);
            }
        }

        public static RenderMethodTemplate GenerateRenderMethodTemplate(GameCache cache, Stream cacheStream, RenderMethodDefinition rmdf, GlobalPixelShader glps, GlobalVertexShader glvs, IShaderGenerator generator, string shaderName)
        {
            return GenerateRenderMethodTemplate(cache, cacheStream, rmdf, glps, glvs, generator, shaderName, out PixelShader pixl, out VertexShader vtsh);
        }
    
        public static RenderMethodTemplate GenerateRenderMethodTemplate(GameCache cache, Stream cacheStream, RenderMethodDefinition rmdf, GlobalPixelShader glps, GlobalVertexShader glvs, IShaderGenerator generator, string shaderName, out PixelShader pixl, out VertexShader vtsh)
        {
            var rmt2 = new RenderMethodTemplate();

            List<byte> options = new List<byte>();
            foreach (var option in shaderName.Split('\\')[2].Remove(0, 1).Split('_'))
                options.Add(byte.Parse(option));
            string type = shaderName.Split('\\')[1].Replace("_templates", "");

            //pixl = GeneratePixelShader(cache, generator);
            pixl = ShaderGeneratorNew.GeneratePixelShader(cache, 
                rmdf, 
                (HaloShaderGenerator.Globals.ShaderType)Enum.Parse(typeof(HaloShaderGenerator.Globals.ShaderType), type, true), 
                options.ToArray());

            vtsh = GenerateVertexShader(cache, generator);

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

            rmt2.RealParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.TextureParameterNames = new List<RenderMethodTemplate.ShaderArgument>();

            var pixelShaderParameters = generator.GetPixelShaderParameters();
            var vertexShaderParameters = generator.GetVertexShaderParameters();
            var globalShaderParameters = generator.GetGlobalParameters();

            List<string> realParameterNames = new List<string>();
            List<string> intParameterNames = new List<string>();
            List<string> boolParameterNames = new List<string>();
            List<string> textureParameterNames = new List<string>();

            foreach (var p in pixelShaderParameters.GetRealPixelParameters())
                if (!realParameterNames.Contains(p.ParameterName))
                    realParameterNames.Add(p.ParameterName);
            foreach (var p in vertexShaderParameters.GetRealVertexParameters())
                if (!realParameterNames.Contains(p.ParameterName))
                    realParameterNames.Add(p.ParameterName);

            foreach (var p in pixelShaderParameters.GetIntegerPixelParameters())
                if (!intParameterNames.Contains(p.ParameterName))
                    intParameterNames.Add(p.ParameterName);
            foreach (var p in vertexShaderParameters.GetIntegerVertexParameters())
                if (!intParameterNames.Contains(p.ParameterName))
                    intParameterNames.Add(p.ParameterName);

            foreach (var p in pixelShaderParameters.GetBooleanPixelParameters())
                if (!boolParameterNames.Contains(p.ParameterName))
                    boolParameterNames.Add(p.ParameterName);
            foreach (var p in vertexShaderParameters.GetBooleanVertexParameters())
                if (!boolParameterNames.Contains(p.ParameterName))
                    boolParameterNames.Add(p.ParameterName);

            foreach (var p in pixelShaderParameters.GetSamplerPixelParameters())
                if (!textureParameterNames.Contains(p.ParameterName))
                    textureParameterNames.Add(p.ParameterName);
            foreach (var p in vertexShaderParameters.GetSamplerVertexParameters())
                if (!textureParameterNames.Contains(p.ParameterName))
                    textureParameterNames.Add(p.ParameterName);

            foreach (var p in realParameterNames)
                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, p) });
            foreach (var p in intParameterNames)
                rmt2.IntegerParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, p) });
            foreach (var p in boolParameterNames)
                rmt2.BooleanParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, p) });
            foreach (var p in textureParameterNames)
                rmt2.TextureParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = AddString(cache, p) });

            rmt2.RoutingInfo = new List<RenderMethodTemplate.RoutingInfoBlock>();
            rmt2.Passes = new List<RenderMethodTemplate.PassBlock>();
            rmt2.EntryPoints = new List<RenderMethodTemplate.TagBlockIndex>();

            foreach (ShaderStage mode in Enum.GetValues(typeof(ShaderStage)))
            {
                var entryPoint = new RenderMethodTemplate.TagBlockIndex();

                if (generator.IsEntryPointSupported(mode))
                {
                    while (rmt2.EntryPoints.Count < (int)mode)
                        rmt2.EntryPoints.Add(new RenderMethodTemplate.TagBlockIndex());

                    entryPoint.Offset = (ushort)rmt2.Passes.Count();
                    entryPoint.Count = 1;
                    rmt2.EntryPoints.Add(entryPoint);

                    var parameterTable = new RenderMethodTemplate.PassBlock();

                    for (int i = 0; i < parameterTable.Values.Length; i++)
                        parameterTable.Values[i] = new RenderMethodTemplate.TagBlockIndex();

                    rmt2.Passes.Add(parameterTable);

                    // find pixel shader and vertex shader block loaded by this entry point

                    PixelShaderBlock pixelShader;
                    VertexShaderBlock vertexShader;

                    if (generator.IsVertexShaderShared(mode))
                        vertexShader = glvs.Shaders[glvs.VertexTypes[(ushort)rmdf.VertexTypes[0].VertexType].EntryPoints[(int)mode].ShaderIndex];
                    else
                        vertexShader = vtsh.Shaders[vtsh.EntryPoints[(ushort)rmdf.VertexTypes[0].VertexType].SupportedVertexTypes[(int)mode].Offset];

                    if (generator.IsPixelShaderShared(mode))
                    {
                        if (glps.EntryPoints[(int)mode].DefaultCompiledShaderIndex == -1)
                        {
                            // assumes shared pixel shader are only used for a single method, otherwise unknown procedure to obtain one or more pixel shader block
                            var optionValue = generator.GetMethodOptionValue(glps.EntryPoints[(int)mode].CategoryDependency[0].DefinitionCategoryIndex);
                            pixelShader = glps.Shaders[glps.EntryPoints[(int)mode].CategoryDependency[0].OptionDependency[optionValue].CompiledShaderIndex];
                        }
                        else
                            pixelShader = glps.Shaders[glps.EntryPoints[(int)mode].DefaultCompiledShaderIndex];
                    }
                    else
                        pixelShader = pixl.Shaders[pixl.EntryPointShaders[(int)mode].Offset];



                    // build dictionary register name to register index, speeds lookup time
                    // needs to be built for each usage type to avoid name conflicts

                    Dictionary<string, int> pixelShaderSamplerMapping = new Dictionary<string, int>();
                    Dictionary<string, int> pixelShaderVectorMapping = new Dictionary<string, int>();
                    Dictionary<string, int> pixelShaderIntegerMapping = new Dictionary<string, int>();
                    Dictionary<string, int> pixelShaderBooleanMapping = new Dictionary<string, int>();

                    foreach (var reg in pixelShader.PCConstantTable.Constants)
                    {
                        switch (reg.RegisterType)
                        {
                            case ShaderParameter.RType.Sampler:
                                pixelShaderSamplerMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                            case ShaderParameter.RType.Vector:
                                pixelShaderVectorMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                            case ShaderParameter.RType.Integer:
                                pixelShaderIntegerMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                            case ShaderParameter.RType.Boolean:
                                pixelShaderBooleanMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                        }
                    }

                    Dictionary<string, int> vertexShaderSamplerMapping = new Dictionary<string, int>();
                    Dictionary<string, int> vertexShaderVectorMapping = new Dictionary<string, int>();
                    Dictionary<string, int> vertexShaderIntegerMapping = new Dictionary<string, int>();
                    Dictionary<string, int> vertexShaderBooleanMapping = new Dictionary<string, int>();

                    foreach (var reg in vertexShader.PCConstantTable.Constants)
                    {
                        switch (reg.RegisterType)
                        {
                            case ShaderParameter.RType.Sampler:
                                //vertexShaderSamplerMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                pixelShaderSamplerMapping[cache.StringTable.GetString(reg.ParameterName) + "_VERTEX_"] = reg.RegisterIndex; // quick fix instead of rewriting all
                                break;
                            case ShaderParameter.RType.Vector:
                                vertexShaderVectorMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                            case ShaderParameter.RType.Integer:
                                vertexShaderIntegerMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                            case ShaderParameter.RType.Boolean:
                                vertexShaderBooleanMapping[cache.StringTable.GetString(reg.ParameterName)] = reg.RegisterIndex;
                                break;
                        }
                    }

                    // build parameter table and registers available for this entry point, order to be determined

                    List<RenderMethodTemplate.RoutingInfoBlock> mappings;
                    ParameterUsage currentUsage;

                    // sampler (ps)

                    currentUsage = ParameterUsage.TextureExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderSamplerMapping);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.Texture;
                    ShaderParameters newTextureParameters = new ShaderParameters();
                    newTextureParameters.Parameters.AddRange(pixelShaderParameters.GetSamplerPixelParameters());
                    newTextureParameters.Parameters.AddRange(vertexShaderParameters.GetSamplerVertexParameters());
                    mappings = MapParameters(cache, currentUsage, newTextureParameters, pixelShaderSamplerMapping, rmt2.TextureParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    // ps

                    currentUsage = ParameterUsage.PS_Real;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderVectorMapping, rmt2.RealParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.PS_Integer;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderIntegerMapping, rmt2.IntegerParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.PS_Boolean;
                    mappings = MapParameters(cache, currentUsage, pixelShaderParameters, pixelShaderBooleanMapping, rmt2.BooleanParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    // vs

                    currentUsage = ParameterUsage.VS_Real;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderVectorMapping, rmt2.RealParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.VS_Integer;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderIntegerMapping, rmt2.IntegerParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.VS_Boolean;
                    mappings = MapParameters(cache, currentUsage, vertexShaderParameters, vertexShaderBooleanMapping, rmt2.BooleanParameterNames);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    // ps extern

                    currentUsage = ParameterUsage.PS_RealExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderVectorMapping);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.PS_IntegerExtern;
                    mappings = MapExternParameters(currentUsage, pixelShaderParameters, globalShaderParameters, pixelShaderIntegerMapping);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    // vs extern

                    currentUsage = ParameterUsage.VS_RealExtern;
                    mappings = MapExternParameters(currentUsage, vertexShaderParameters, globalShaderParameters, vertexShaderVectorMapping);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);

                    currentUsage = ParameterUsage.VS_IntegerExtern;
                    mappings = MapExternParameters(currentUsage, vertexShaderParameters, globalShaderParameters, vertexShaderIntegerMapping);
                    AddMapping(currentUsage, rmt2, parameterTable, mappings);
                }
            }

            return rmt2;
        }

        public static IShaderGenerator GetGlobalShaderGenerator(string shaderType, bool applyFixes = false)
        {
            switch (shaderType)
            {
                case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(applyFixes);
                case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(applyFixes);
                case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(applyFixes);
                case "custom":          return new HaloShaderGenerator.Custom.CustomGenerator(applyFixes);
                case "decal":           return new HaloShaderGenerator.Decal.DecalGenerator(applyFixes);
                case "foliage":         return new HaloShaderGenerator.Foliage.FoliageGenerator(applyFixes);
                //case "glass":           return new HaloShaderGenerator.Glass.GlassGenerator(applyFixes);
                case "halogram":        return new HaloShaderGenerator.Halogram.HalogramGenerator(applyFixes);
                case "light_volume":    return new HaloShaderGenerator.LightVolume.LightVolumeGenerator(applyFixes);
                case "particle":        return new HaloShaderGenerator.Particle.ParticleGenerator(applyFixes);
                case "screen":          return new HaloShaderGenerator.Screen.ScreenGenerator(applyFixes);
                case "shader":          return new HaloShaderGenerator.Shader.ShaderGenerator(applyFixes);
                case "terrain":         return new HaloShaderGenerator.Terrain.TerrainGenerator(applyFixes);
                case "water":           return new HaloShaderGenerator.Water.WaterGenerator(applyFixes);
                case "zonly":           return new HaloShaderGenerator.ZOnly.ZOnlyGenerator(applyFixes);
            }

            Console.WriteLine($"ERROR: Could not retrieve shared shader generator for \"{shaderType}\"");
            return null;
        }
    }

    public partial class ShaderGeneratorNew
    {
        [Flags]
        private enum ParameterTypeFlags
        {
            Vector = 0,
            Integer = 1 << 0,
            Boolean = 1 << 1,
            Sampler = 1 << 2
        }

        private static StringId AddStringSafe(GameCache cache, string str)
        {
            var sTable = (StringTableHaloOnline)cache.StringTable;

            if (str == "")
                return StringId.Invalid;
            var stringId = sTable.GetStringId(str);
            if (stringId == StringId.Invalid)
                stringId = sTable.AddStringBlocking(str);
            return stringId;
        }

        public static bool AutoMacroIsParameter(string categoryName, HaloShaderGenerator.Globals.ShaderType shaderType)
        {
            switch (shaderType)
            {
                case HaloShaderGenerator.Globals.ShaderType.Water:
                    if (categoryName == "waveshape" || categoryName == "global_shape")
                        return true;
                    break;
                case HaloShaderGenerator.Globals.ShaderType.Particle:
                    switch (categoryName)
                    {
                        case "albedo":
                        case "blend_mode":
                        case "depth_fade":
                        case "lighting":
                        case "fog":
                        case "specialized_rendering":
                        case "frame_blend":
                        case "self_illumination":
                            return true;
                    }
                    break;
                // todo
            }

            return false;
        }

        private static List<OptionInfo> BuildOptionInfo(GameCache cache, RenderMethodDefinition rmdf, 
            byte[] options, HaloShaderGenerator.Globals.ShaderType shaderType, bool ps = true)
        {
            List<OptionInfo> optionInfo = new List<OptionInfo>();

            if (rmdf.Flags.HasFlag(RenderMethodDefinitionFlags.UseAutomaticMacros))
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (options[i] >= rmdf.Categories[i].ShaderOptions.Count)
                        continue;

                    string category = cache.StringTable.GetString(rmdf.Categories[i].Name);
                    string option = cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[options[i]].Name);

                    if (!AutoMacroIsParameter(category, shaderType) || ps)
                    {
                        optionInfo.Add(new OptionInfo
                        {
                            Category = category,
                            PsMacro = "category_" + category,
                            //VsMacro = "category_" + category,
                            VsMacro = "invalid",
                            Option = option,
                            PsMacroValue = "category_" + category + "_option_" + option,
                            //VsMacroValue = "category_" + category + "_option_" + option
                            VsMacroValue = "invalid"
                        });
                    }
                    // definitions
                    for (int j = 0; j < rmdf.Categories[i].ShaderOptions.Count; j++)
                    {
                        optionInfo.Add(new OptionInfo
                        {
                            Category = category,
                            PsMacro = "category_" + category + "_option_" + cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[j].Name),
                            //VsMacro = "category_" + category + "_option_" + cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[j].Name),
                            VsMacro = "invalid",
                            Option = cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[j].Name),
                            PsMacroValue = j.ToString(),
                            //VsMacroValue = j.ToString()
                            VsMacroValue = "invalid"
                        });
                    }
                }
            }
            else
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (options[i] >= rmdf.Categories[i].ShaderOptions.Count)
                        continue;

                    optionInfo.Add(new OptionInfo
                    {
                        Category = cache.StringTable.GetString(rmdf.Categories[i].Name),
                        PsMacro = cache.StringTable.GetString(rmdf.Categories[i].PixelFunction),
                        VsMacro = cache.StringTable.GetString(rmdf.Categories[i].VertexFunction),
                        Option = cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[options[i]].Name),
                        PsMacroValue = cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[options[i]].PixelFunction),
                        VsMacroValue = cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[options[i]].VertexFunction)
                    });
                }
            }

            return optionInfo;
        }

        private static ShaderConstantTable BuildConstantTable(GameCache cache, ShaderGeneratorResult result, ShaderType shaderType)
        {
            List<ShaderParameter> parameters = new List<ShaderParameter>();

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
                }

                shaderParameter.ParameterName = AddStringSafe(cache, register.Name);

                parameters.Add(shaderParameter);
            }

            return new ShaderConstantTable
            {
                Constants = parameters,
                ShaderType = shaderType
            };
        }

        private static int GetConstantIndex(GameCache cache, ShaderConstantTable constantTable, string parameterName, ShaderParameter.RType type)
        {
            for (int i = 0; i < constantTable.Constants.Count; i++)
            {
                string constantName = cache.StringTable.GetString(constantTable.Constants[i].ParameterName);

                if (type == ShaderParameter.RType.Vector)
                {
                    if (constantName.EndsWith("_xform"))
                        constantName = constantName.Remove(constantName.Length - 6, 6);
                    //else if (constantName.StartsWith("category_"))
                    //    constantName = constantName.Remove(0, 9);
                }

                if (type == constantTable.Constants[i].RegisterType && constantName == parameterName)
                    return i;
            }

            return -1;
        }

        private static ShaderParameter.RType ParameterTypeToRegisterType(RenderMethodOption.ParameterBlock.OptionDataType type)
        {
            switch (type)
            {
                case RenderMethodOption.ParameterBlock.OptionDataType.Bitmap:
                    return ShaderParameter.RType.Sampler;
                case RenderMethodOption.ParameterBlock.OptionDataType.Real:
                case RenderMethodOption.ParameterBlock.OptionDataType.Color:
                case RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor:
                    return ShaderParameter.RType.Vector;
                case RenderMethodOption.ParameterBlock.OptionDataType.Bool:
                    return ShaderParameter.RType.Boolean;
                case RenderMethodOption.ParameterBlock.OptionDataType.Int:
                    return ShaderParameter.RType.Integer;
            }
            return ShaderParameter.RType.Sampler;
        }

        private static List<RenderMethodTemplate.RoutingInfoBlock> GenerateRoutingInfo(GameCache cache,
            List<RenderMethodOption.ParameterBlock> rmopParameters,
            List<RenderMethodTemplate.ShaderArgument> rmt2ParameterList,
            ShaderConstantTable constantTable, 
            ShaderParameter.RType type,
            bool is_extern,
            bool vsSampler)
        {
            List<RenderMethodTemplate.RoutingInfoBlock> routingInfo = new List<RenderMethodTemplate.RoutingInfoBlock>();

            foreach (var parameter in rmopParameters.Where(x => (x.RenderMethodExtern == RenderMethodExtern.none) == !is_extern))
            {
                string parameterName = cache.StringTable.GetString(parameter.Name);

                int index = GetConstantIndex(cache, constantTable, parameterName, type);

                if (index != -1)
                {
                    int sourceIndex;
                    if (is_extern)
                    {
                        sourceIndex = (int)parameter.RenderMethodExtern;
                    }
                    else
                    {
                        sourceIndex = rmt2ParameterList.FindIndex(x => cache.StringTable.GetString(x.Name) == parameterName);

                        if (sourceIndex == -1)
                        {
                            throw new Exception("shits broke af");
                        }
                    }

                    routingInfo.Add(new RenderMethodTemplate.RoutingInfoBlock
                    {
                        DestinationIndex = constantTable.Constants[index].RegisterIndex,
                        SourceIndex = (byte)sourceIndex,
                        Flags = (byte)(vsSampler ? 1 : 0)
                    });
                }
                else if (ParameterTypeToRegisterType(parameter.Type) == type)
                {
                    //new TagToolWarning($"no binding for {constantTable.ShaderType} {(is_extern ? "extern " : "")}{type} \"{parameterName}\"");
                }
            }

            return routingInfo;
        }

        private static List<RenderMethodTemplate.RoutingInfoBlock> GenerateCategoryRoutingInfo(GameCache cache,
            RenderMethodDefinition rmdf,
            List<RenderMethodTemplate.ShaderArgument> rmt2ParameterList,
            ShaderConstantTable constantTable)
        {
            List<RenderMethodTemplate.RoutingInfoBlock> routingInfo = new List<RenderMethodTemplate.RoutingInfoBlock>();

            if (rmdf.Flags.HasFlag(RenderMethodDefinitionFlags.UseAutomaticMacros))
            {
                foreach (var category in rmdf.Categories)
                {
                    string categoryName = cache.StringTable.GetString(category.Name);

                    int index = GetConstantIndex(cache, constantTable, "category_" + categoryName, ShaderParameter.RType.Vector);

                    if (index != -1)
                    {
                        int sourceIndex = rmt2ParameterList.FindIndex(x => cache.StringTable.GetString(x.Name) == categoryName);

                        if (sourceIndex == -1)
                            throw new Exception($"\"categoryName\" category binding broken");

                        routingInfo.Add(new RenderMethodTemplate.RoutingInfoBlock
                        {
                            DestinationIndex = constantTable.Constants[index].RegisterIndex,
                            SourceIndex = (byte)sourceIndex,
                            Flags = 0
                        });
                    }
                }
            }

            return routingInfo;
        }

        private static void GetPassConstantTables(int entryPointIndex,
            List<byte> options,
            RenderMethodDefinition rmdf,
            PixelShader pixl,
            /*VertexShader vtsh,*/
            GlobalPixelShader glps,
            GlobalVertexShader glvs,
            out ShaderConstantTable psConstantTable, 
            out ShaderConstantTable vsConstantTable)
        {
            psConstantTable = new ShaderConstantTable { Constants = new List<ShaderParameter>(), ShaderType = ShaderType.PixelShader };

            if (entryPointIndex < glps.EntryPoints.Count && 
                (glps.EntryPoints[entryPointIndex].DefaultCompiledShaderIndex != -1 ||
                glps.EntryPoints[entryPointIndex].CategoryDependency.Count > 0))
            {
                if (glps.EntryPoints[entryPointIndex].CategoryDependency.Count > 0)
                {
                    int categoryIndex = glps.EntryPoints[entryPointIndex].CategoryDependency[0].DefinitionCategoryIndex;
                    if (categoryIndex >= 0 && categoryIndex < options.Count)
                    {
                        int shaderIndex = glps.EntryPoints[entryPointIndex].CategoryDependency[0].OptionDependency[options[categoryIndex]].CompiledShaderIndex;
                        psConstantTable = glps.Shaders[shaderIndex].PCConstantTable;
                    }
                }
                else
                {
                    psConstantTable = glps.Shaders[glps.EntryPoints[entryPointIndex].DefaultCompiledShaderIndex].PCConstantTable;
                }
            }
            else if (entryPointIndex < pixl.EntryPointShaders.Count)
            {
                if (pixl.EntryPointShaders[entryPointIndex].Count > 0)
                {
                    psConstantTable = pixl.Shaders[pixl.EntryPointShaders[entryPointIndex].Offset].PCConstantTable;
                }
            }

            int vertexIndex = (int)rmdf.VertexTypes[0].VertexType;
            int vsShaderIndex = glvs.VertexTypes[vertexIndex].EntryPoints[entryPointIndex].ShaderIndex;
            vsConstantTable = glvs.Shaders[vsShaderIndex].PCConstantTable;
        }

        private static void AccumRuntimeParameterType(GameCache cache, Dictionary<string, ParameterTypeFlags> parameterTypes, ShaderParameter constant)
        {
            string name = cache.StringTable.GetString(constant.ParameterName);

            if (parameterTypes.ContainsKey(name))
                parameterTypes[name] |= (ParameterTypeFlags)Enum.Parse(typeof(ParameterTypeFlags), constant.RegisterType.ToString());
            else
                parameterTypes[name] = (ParameterTypeFlags)Enum.Parse(typeof(ParameterTypeFlags), constant.RegisterType.ToString());
        }

        public static List<RenderMethodOption.ParameterBlock> GatherParameters(GameCache cache, Stream stream, RenderMethodDefinition rmdf, List<byte> options, bool includeGlobal = true)
        {
            List<RenderMethodOption.ParameterBlock> allRmopParameters = new List<RenderMethodOption.ParameterBlock>();

            if (includeGlobal)
            {
                if (rmdf.GlobalOptions != null)
                {
                    var globalRmop = cache.Deserialize<RenderMethodOption>(stream, rmdf.GlobalOptions);
                    allRmopParameters.AddRange(globalRmop.Parameters);
                }
            }

            for (int i = 0; i < rmdf.Categories.Count; i++)
            {
                if (rmdf.Categories[i].ShaderOptions.Count == 0)
                    continue;

                var option = rmdf.Categories[i].ShaderOptions[i < options.Count ? options[i] : 0];

                if (option.Option != null)
                {
                    var rmop = cache.Deserialize<RenderMethodOption>(stream, option.Option);

                    foreach (var parameter in rmop.Parameters)
                    {
                        if (allRmopParameters.Any(x => x.Name == parameter.Name)) // prevent duplicates
                            continue;

                        allRmopParameters.Add(parameter);
                    }
                }
            }

            return allRmopParameters;
        }

        /// <summary>
        /// Non async
        /// </summary>
        public static RenderMethodTemplate GenerateTemplateSafe(GameCache cache, 
            Stream stream, 
            RenderMethodDefinition rmdf, 
            string shaderName,
            out PixelShader pixl,
            out VertexShader vtsh)
        {
            var glps = cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            // get options in numeric array
            List<byte> options = new List<byte>();
            foreach (var option in shaderName.Split('\\')[2].Remove(0, 1).Split('_'))
                options.Add(byte.Parse(option));

            var allRmopParameters = GatherParameters(cache, stream, rmdf, options);

            var rmt2 = GenerateTemplate(cache, rmdf, glvs, glps, allRmopParameters, shaderName, out pixl, out vtsh);

            if (!cache.TagCache.TryGetTag(shaderName + ".pixl", out rmt2.PixelShader))
                rmt2.PixelShader = cache.TagCache.AllocateTag<PixelShader>(shaderName);
            if (!cache.TagCache.TryGetTag(shaderName + ".vtsh", out rmt2.VertexShader))
                rmt2.VertexShader = cache.TagCache.AllocateTag<VertexShader>(shaderName);

            cache.Serialize(stream, rmt2.PixelShader, pixl);
            cache.Serialize(stream, rmt2.VertexShader, vtsh);

            return rmt2;
        }

        /// <summary>
        /// Async compatible -- WARNING: no serialization is to occur within the scope of this function. pixl and vtsh must be serialized after this function.
        /// </summary>
        public static RenderMethodTemplate GenerateTemplate(GameCache cache,
            RenderMethodDefinition rmdf, 
            GlobalVertexShader glvs,
            GlobalPixelShader glps,
            List<RenderMethodOption.ParameterBlock> allRmopParameters,
            string shaderName, 
            out PixelShader pixl, 
            out VertexShader vtsh)
        {
            var rmt2 = new RenderMethodTemplate
            {
                RoutingInfo = new List<RenderMethodTemplate.RoutingInfoBlock>(),
                Passes = new List<RenderMethodTemplate.PassBlock>(),
                EntryPoints = new List<RenderMethodTemplate.TagBlockIndex>(),
                RealParameterNames = new List<RenderMethodTemplate.ShaderArgument>(),
                IntegerParameterNames = new List<RenderMethodTemplate.ShaderArgument>(),
                BooleanParameterNames = new List<RenderMethodTemplate.ShaderArgument>(),
                TextureParameterNames = new List<RenderMethodTemplate.ShaderArgument>(),
                OtherPlatforms = new List<RenderMethodTemplate.RenderMethodTemplatePlatformBlock>()
            };

            // get options in numeric array
            List<byte> options = new List<byte>();
            foreach (var option in shaderName.Split('\\')[2].Remove(0, 1).Split('_'))
                options.Add(byte.Parse(option));
            string type = shaderName.Split('\\')[1].Replace("_templates", "").Replace("_", "");

            // generate the shaders

            pixl = GeneratePixelShader(cache,
                rmdf,
                (HaloShaderGenerator.Globals.ShaderType)Enum.Parse(typeof(HaloShaderGenerator.Globals.ShaderType), type, true),
                options.ToArray());

            vtsh = new VertexShader();

            // accum parameters

            Dictionary<string, ParameterTypeFlags> parameterTypes = new Dictionary<string, ParameterTypeFlags>(); // <name, types>

            foreach (var shader in pixl.Shaders)
                foreach (var constant in shader.PCConstantTable.Constants)
                    AccumRuntimeParameterType(cache, parameterTypes, constant);
            //foreach (var shader in vtsh.Shaders)
            //    foreach (var constant in shader.PCConstantTable.Constants)
            //        AccumRuntimeParameterType(cache, parameterTypes, constant);
            foreach (var shader in glps.Shaders)
                foreach (var constant in shader.PCConstantTable.Constants)
                    AccumRuntimeParameterType(cache, parameterTypes, constant);
            foreach (var shader in glvs.Shaders)
                foreach (var constant in shader.PCConstantTable.Constants)
                    AccumRuntimeParameterType(cache, parameterTypes, constant);

            foreach (var parameter in allRmopParameters)
            {
                string name = cache.StringTable.GetString(parameter.Name);
                if (parameter.RenderMethodExtern == RenderMethodExtern.none)
                {
                    switch (parameter.Type)
                    {
                        case RenderMethodOption.ParameterBlock.OptionDataType.Real:
                        case RenderMethodOption.ParameterBlock.OptionDataType.Color:
                        case RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor:
                            rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            break;
                        case RenderMethodOption.ParameterBlock.OptionDataType.Int:
                            rmt2.IntegerParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            if (parameterTypes.ContainsKey(name) && parameterTypes[name].HasFlag(ParameterTypeFlags.Vector))
                                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            break;
                        case RenderMethodOption.ParameterBlock.OptionDataType.Bool:
                            rmt2.BooleanParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            if (parameterTypes.ContainsKey(name) && parameterTypes[name].HasFlag(ParameterTypeFlags.Vector))
                                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            break;
                        case RenderMethodOption.ParameterBlock.OptionDataType.Bitmap:
                            rmt2.TextureParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            if (parameterTypes.ContainsKey(name + "_xform")/* && parameterTypes[name + "_xform"].HasFlag(ParameterTypeFlags.Vector)*/)
                                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { Name = parameter.Name });
                            break;
                    }
                }
            }

            // hax for category, these really should be ordered in
            foreach (var categoryParameter in parameterTypes.Where(x => x.Key.StartsWith("category_")))
            {
                rmt2.RealParameterNames.Add(new RenderMethodTemplate.ShaderArgument { 
                    Name = cache.StringTable.GetStringId(categoryParameter.Key.Remove(0, 9)) 
                });
            }

            for (int i = 0; i < Enum.GetValues(typeof(EntryPoint)).Length; i++)
                rmt2.EntryPoints.Add(new RenderMethodTemplate.TagBlockIndex());

            foreach (var entryBlock in rmdf.EntryPoints)
            {
                int i = (int)entryBlock.EntryPoint;

                rmt2.EntryPoints[i].Offset = (ushort)rmt2.Passes.Count;
                rmt2.EntryPoints[i].Count = 1;

                GetPassConstantTables(i, options, rmdf, pixl, glps, glvs, 
                    out var psConstantTable, out var vsConstantTable);

                RenderMethodTemplate.PassBlock pass = new RenderMethodTemplate.PassBlock();

                for (int j = 0; j < (int)ParameterUsage.Count; j++) // init
                    pass.Values[j] = new RenderMethodTemplate.TagBlockIndex();

                // texture extern ps/vs //////////////////////////

                pass.Values[(int)ParameterUsage.TextureExtern].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    psConstantTable,
                    ShaderParameter.RType.Sampler,
                    true,
                    false));
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    vsConstantTable,
                    ShaderParameter.RType.Sampler,
                    true,
                    false));
                pass.Values[(int)ParameterUsage.TextureExtern].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.TextureExtern].Offset);

                // texture ps/vs ////////////////////////////

                pass.Values[(int)ParameterUsage.Texture].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.TextureParameterNames,
                    vsConstantTable,
                    ShaderParameter.RType.Sampler,
                    false,
                    true));
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.TextureParameterNames,
                    psConstantTable,
                    ShaderParameter.RType.Sampler,
                    false,
                    false));
                pass.Values[(int)ParameterUsage.Texture].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.Texture].Offset);

                // ps bindings ///////////////////////////////

                pass.Values[(int)ParameterUsage.PS_Real].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.RealParameterNames,
                    psConstantTable,
                    ShaderParameter.RType.Vector,
                    false,
                    false));
                rmt2.RoutingInfo.AddRange(GenerateCategoryRoutingInfo(cache,
                    rmdf,
                    rmt2.RealParameterNames,
                    psConstantTable));
                pass.Values[(int)ParameterUsage.PS_Real].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.PS_Real].Offset);
                pass.Values[(int)ParameterUsage.PS_Integer].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.IntegerParameterNames,
                    psConstantTable,
                    ShaderParameter.RType.Integer,
                    false,
                    false));
                pass.Values[(int)ParameterUsage.PS_Integer].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.PS_Integer].Offset);
                pass.Values[(int)ParameterUsage.PS_Boolean].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.BooleanParameterNames,
                    psConstantTable,
                    ShaderParameter.RType.Boolean,
                    false,
                    false));
                pass.Values[(int)ParameterUsage.PS_Boolean].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.PS_Boolean].Offset);

                // ps extern bindings /////////////////////////////////

                pass.Values[(int)ParameterUsage.PS_RealExtern].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    psConstantTable,
                    ShaderParameter.RType.Vector,
                    true,
                    false));
                pass.Values[(int)ParameterUsage.PS_RealExtern].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.PS_RealExtern].Offset);
                pass.Values[(int)ParameterUsage.PS_IntegerExtern].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    psConstantTable,
                    ShaderParameter.RType.Integer,
                    true,
                    false));
                pass.Values[(int)ParameterUsage.PS_IntegerExtern].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.PS_IntegerExtern].Offset);

                // vs bindings /////////////////////////////////

                pass.Values[(int)ParameterUsage.VS_Real].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.RealParameterNames,
                    vsConstantTable,
                    ShaderParameter.RType.Vector,
                    false,
                    false));
                rmt2.RoutingInfo.AddRange(GenerateCategoryRoutingInfo(cache,
                    rmdf,
                    rmt2.RealParameterNames,
                    vsConstantTable));
                pass.Values[(int)ParameterUsage.VS_Real].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.VS_Real].Offset);
                pass.Values[(int)ParameterUsage.VS_Integer].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.IntegerParameterNames,
                    vsConstantTable,
                    ShaderParameter.RType.Integer,
                    false,
                    false));
                pass.Values[(int)ParameterUsage.VS_Integer].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.VS_Integer].Offset);
                pass.Values[(int)ParameterUsage.VS_Boolean].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    rmt2.BooleanParameterNames,
                    vsConstantTable,
                    ShaderParameter.RType.Boolean,
                    false,
                    false));
                pass.Values[(int)ParameterUsage.VS_Boolean].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.VS_Boolean].Offset);

                // vs extern bindings
                pass.Values[(int)ParameterUsage.VS_RealExtern].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    vsConstantTable,
                    ShaderParameter.RType.Vector,
                    true,
                    false));
                pass.Values[(int)ParameterUsage.VS_RealExtern].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.VS_RealExtern].Offset);
                pass.Values[(int)ParameterUsage.VS_IntegerExtern].Offset = (ushort)rmt2.RoutingInfo.Count;
                rmt2.RoutingInfo.AddRange(GenerateRoutingInfo(cache,
                    allRmopParameters,
                    null,
                    vsConstantTable,
                    ShaderParameter.RType.Integer,
                    true,
                    false));
                pass.Values[(int)ParameterUsage.VS_IntegerExtern].Count = (ushort)(rmt2.RoutingInfo.Count - pass.Values[(int)ParameterUsage.VS_IntegerExtern].Offset);

                // clean
                for (int j = 0; j < (int)ParameterUsage.Count; j++)
                    if (pass.Values[j].Count == 0)
                        pass.Values[j].Offset = 0;

                rmt2.Passes.Add(pass);
                rmt2.ValidEntryPoints |= (EntryPointBitMask)(1 << i);
            }


            return rmt2;
        }

        public static PixelShader GeneratePixelShader(GameCache cache, RenderMethodDefinition rmdf, HaloShaderGenerator.Globals.ShaderType shaderType, byte[] options)
        {
            PixelShader pixl = new PixelShader { EntryPointShaders = new List<ShortOffsetCountBlock>(), Shaders = new List<PixelShaderBlock>() };

            Dictionary<Task<ShaderGeneratorResult>, int> tasks = new Dictionary<Task<ShaderGeneratorResult>, int>(); // <task, entry point>

            TemplateGenerator generator = new TemplateGenerator();
            List<OptionInfo> optionInfo = BuildOptionInfo(cache, rmdf, options, shaderType);

            for (int i = 0; i < 20; i++)
                pixl.EntryPointShaders.Add(new ShortOffsetCountBlock());

            foreach (var entry in rmdf.EntryPoints)
            {
                if (!entry.Passes[0].Flags.HasFlag(EntryPointBlock.PassBlock.PassFlags.HasSharedPixelShader))
                { 
                    ShaderStage entryPoint;

                    if (entry.EntryPoint == EntryPoint_32.Water_Tessellation)
                        entryPoint = ShaderStage.Water_Tessellation;
                    else
                        entryPoint = (ShaderStage)Enum.Parse(typeof(ShaderStage), entry.EntryPoint.ToString());

                    Task<ShaderGeneratorResult> generatorTask = Task.Run(() => { return generator.GeneratePixelShader(shaderType, entryPoint, optionInfo, true); });
                    tasks.Add(generatorTask, (int)entryPoint);
                }
            }

            Task.WaitAll(tasks.Keys.ToArray());

            foreach (var task in tasks)
            {
                pixl.EntryPointShaders[task.Value].Count = 1;
                pixl.EntryPointShaders[task.Value].Offset = (byte)pixl.Shaders.Count;

                var pixelShaderBlock = new PixelShaderBlock
                {
                    PCShaderBytecode = task.Key.Result.Bytecode,
                    PCConstantTable = BuildConstantTable(cache, task.Key.Result, ShaderType.PixelShader)
                };

                pixl.Shaders.Add(pixelShaderBlock);
            }
            return pixl;
        }

        private static List<byte> ApplyGlpsOptionHacks(List<byte> fakeOptions, GameCache cache, RenderMethodDefinition rmdf, HaloShaderGenerator.Globals.ShaderType shaderType)
        {
            // force foliage albedo to 'simple' for glps - we just want a basic sample
            if (shaderType == HaloShaderGenerator.Globals.ShaderType.Foliage)
            {
                for (int i = 0; i < rmdf.Categories.Count; i++)
                {
                    if (cache.StringTable.GetString(rmdf.Categories[i].Name) != "albedo")
                        continue;

                    for (int j = 0; j < rmdf.Categories[i].ShaderOptions.Count; j++)
                    {
                        if (cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[j].Name) != "simple")
                            continue;

                        fakeOptions[i] = (byte)j;
                        break;
                    }

                    break;
                }
            }

            return fakeOptions;
        }

        public static GlobalPixelShader GenerateSharedPixelShaders(GameCache cache, RenderMethodDefinition rmdf, HaloShaderGenerator.Globals.ShaderType shaderType)
        {
            GlobalPixelShader glps = new GlobalPixelShader { EntryPoints = new List<GlobalPixelShader.EntryPointBlock>(), Shaders = new List<PixelShaderBlock>() };
            for (int i = 0; i < Enum.GetValues(typeof(EntryPoint)).Length; i++)
                glps.EntryPoints.Add(new GlobalPixelShader.EntryPointBlock { DefaultCompiledShaderIndex = -1 });

            TemplateGenerator generator = new TemplateGenerator();

            foreach (var entryPoint in rmdf.EntryPoints)
            {
                if (entryPoint.Passes[0].Flags.HasFlag(EntryPointBlock.PassBlock.PassFlags.HasSharedPixelShader))
                {
                    ShaderStage entry = (ShaderStage)Enum.Parse(typeof(ShaderStage), entryPoint.EntryPoint.ToString());

                    if (entryPoint.Passes[0].CategoryDependencies.Count > 0)
                    {
                        glps.EntryPoints[(int)entry].CategoryDependency = new List<GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock>();

                        foreach (var dependency in entryPoint.Passes[0].CategoryDependencies)
                        {
                            var dependencyBlock = new GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock { DefinitionCategoryIndex = dependency.Category, 
                                OptionDependency = new List<GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock.GlobalShaderOptionDependency>() };

                            for (int i = 0; i < rmdf.Categories[dependency.Category].ShaderOptions.Count; i++)
                            {
                                List<byte> fakeOptions = Enumerable.Repeat((byte)0, rmdf.Categories.Count).ToList();
                                fakeOptions[dependency.Category] = (byte)i;

                                fakeOptions = ApplyGlpsOptionHacks(fakeOptions, cache, rmdf, shaderType);

                                List<OptionInfo> optionInfo = BuildOptionInfo(cache, rmdf, fakeOptions.ToArray(), shaderType);

                                ShaderGeneratorResult generatorResult = generator.GeneratePixelShader(shaderType, entry, optionInfo, true);

                                dependencyBlock.OptionDependency.Add(new GlobalPixelShader.EntryPointBlock.CategoryDependencyBlock.GlobalShaderOptionDependency { 
                                    CompiledShaderIndex = glps.Shaders.Count });

                                var pixelShaderBlock = new PixelShaderBlock
                                {
                                    PCShaderBytecode = generatorResult.Bytecode,
                                    PCConstantTable = BuildConstantTable(cache, generatorResult, ShaderType.PixelShader)
                                };

                                glps.Shaders.Add(pixelShaderBlock);
                            }

                            glps.EntryPoints[(int)entry].CategoryDependency.Add(dependencyBlock);
                        }
                    }
                    else
                    {
                        List<byte> fakeOptions = Enumerable.Repeat((byte)0, rmdf.Categories.Count).ToList();
                        List<OptionInfo> optionInfo = BuildOptionInfo(cache, rmdf, fakeOptions.ToArray(), shaderType);

                        ShaderGeneratorResult generatorResult = generator.GeneratePixelShader(shaderType, entry, optionInfo, true);

                        glps.EntryPoints[(int)entry].DefaultCompiledShaderIndex = glps.Shaders.Count;

                        var pixelShaderBlock = new PixelShaderBlock
                        {
                            PCShaderBytecode = generatorResult.Bytecode,
                            PCConstantTable = BuildConstantTable(cache, generatorResult, ShaderType.PixelShader)
                        };

                        glps.Shaders.Add(pixelShaderBlock);
                    }
                }
            }

            return glps;
        }

        public static GlobalVertexShader GenerateSharedVertexShaders(GameCache cache, RenderMethodDefinition rmdf, HaloShaderGenerator.Globals.ShaderType shaderType)
        {
            GlobalVertexShader glvs = new GlobalVertexShader { Shaders = new List<VertexShaderBlock>(), VertexTypes = new List<GlobalVertexShader.VertexTypeShaders>() };
            for (int i = 0; i < Enum.GetValues(typeof(VertexType)).Length; i++)
            {
                var vertexTypeBlock = new GlobalVertexShader.VertexTypeShaders { EntryPoints = new List<GlobalVertexShader.VertexTypeShaders.GlobalShaderEntryPointBlock>() };

                if (rmdf.VertexTypes.Any(x => x.VertexType == (VertexBlock.VertexTypeValue)i))
                    for (int j = 0; j < Enum.GetValues(typeof(EntryPoint)).Length; j++)
                        vertexTypeBlock.EntryPoints.Add(new GlobalVertexShader.VertexTypeShaders.GlobalShaderEntryPointBlock { ShaderIndex = -1 });
                glvs.VertexTypes.Add(vertexTypeBlock);
            }

            TemplateGenerator generator = new TemplateGenerator();

            foreach (var vertexTypeBlock in rmdf.VertexTypes)
            {
                VertexType vertexType = (VertexType)Enum.Parse(typeof(VertexType), vertexTypeBlock.VertexType.ToString());

                if (vertexType == VertexType.DualQuat)
                    continue; // unsupported

                foreach (var entryPoint in rmdf.EntryPoints)
                {
                    ShaderStage entry = (ShaderStage)Enum.Parse(typeof(ShaderStage), entryPoint.EntryPoint.ToString());

                    if (vertexTypeBlock.Dependencies.Count > 0)
                    {
                        // TODO: support vertex option dependencies (n/a in any stock gen3 halo)
                    }
                    else
                    {
                        List<byte> fakeOptions = Enumerable.Repeat((byte)0, rmdf.Categories.Count).ToList();
                        List<OptionInfo> optionInfo = BuildOptionInfo(cache, rmdf, fakeOptions.ToArray(), shaderType, false);

                        ShaderGeneratorResult generatorResult = generator.GenerateVertexShader(shaderType, entry, vertexType, optionInfo, true);

                        glvs.VertexTypes[(int)vertexType].EntryPoints[(int)entry].ShaderIndex = glvs.Shaders.Count;

                        var vertexShaderBlock = new VertexShaderBlock
                        {
                            PCShaderBytecode = generatorResult.Bytecode,
                            PCConstantTable = BuildConstantTable(cache, generatorResult, ShaderType.VertexShader)
                        };

                        glvs.Shaders.Add(vertexShaderBlock);
                    }
                }
            }

            return glvs;
        }

        public static bool VerifyRmt2Routing(GameCache cache, Stream stream, RenderMethodTemplate rmt2, RenderMethodDefinition rmdf, List<byte> options)
        {
            bool anyMissing = false;

            var allParameters = GatherParameters(cache, stream, rmdf, options);

            var pixl = cache.Deserialize<PixelShader>(stream, rmt2.PixelShader);

            foreach (var entry in rmdf.EntryPoints)
            {
                if (rmt2.EntryPoints[(int)entry.EntryPoint].Count > 0)
                {
                    int iEnd = rmt2.EntryPoints[(int)entry.EntryPoint].Count + rmt2.EntryPoints[(int)entry.EntryPoint].Offset;
                    for (int i = rmt2.EntryPoints[(int)entry.EntryPoint].Offset; i < iEnd; i++)
                    {
                        var pass = rmt2.Passes[i];

                        if (pass.Values[(int)ParameterUsage.PS_Real].Count > 0)
                        {
                            foreach (var constant in pixl.Shaders[pixl.EntryPointShaders[(int)entry.EntryPoint].Offset].PCConstantTable.Constants)
                            {
                                if (constant.RegisterType != ShaderParameter.RType.Vector)
                                    continue;

                                string constantName = cache.StringTable.GetString(constant.ParameterName);
                                bool found = false;

                                int jEnd = pass.Values[(int)ParameterUsage.PS_Real].Offset + pass.Values[(int)ParameterUsage.PS_Real].Count;
                                for (int j = pass.Values[(int)ParameterUsage.PS_Real].Offset; j < jEnd; j++)
                                {
                                    if (rmt2.RoutingInfo[j].DestinationIndex == constant.RegisterIndex)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    jEnd = pass.Values[(int)ParameterUsage.PS_RealExtern].Offset + pass.Values[(int)ParameterUsage.PS_RealExtern].Count;
                                    for (int j = pass.Values[(int)ParameterUsage.PS_RealExtern].Offset; j < jEnd; j++)
                                    {
                                        if (rmt2.RoutingInfo[j].DestinationIndex == constant.RegisterIndex)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }

                                if (!found)
                                {
                                    Console.WriteLine($"WARNING: {constantName} not bound in rmt2");
                                    anyMissing = true;
                                }
                            }
                        }
                    }
                }
            }

            return !anyMissing;
        }

        public static void GenerateExplicitShader(GameCache cache, Stream stream, string explicitShader, out PixelShader pixl, out VertexShader vtsh)
        {
            ExplicitGenerator generator = new ExplicitGenerator();
            
            HaloShaderGenerator.Globals.ExplicitShader eExplicitShader = generator.GetExplicitIndex(explicitShader);
            
            List<ShaderStage> supportedEntries = generator.ScrapeEntryPoints(eExplicitShader);
            List<VertexType> supportedVertices = generator.ScrapeVertexTypes(eExplicitShader);

            pixl = new PixelShader { EntryPointShaders = new List<ShortOffsetCountBlock>(), Shaders = new List<PixelShaderBlock>() };
            vtsh = new VertexShader { EntryPoints = new List<VertexShader.VertexShaderEntryPoint>(), Shaders = new List<VertexShaderBlock>() };

            for (int i = 0; i < Enum.GetValues(typeof(ShaderStage)).Length; i++)
            {
                pixl.EntryPointShaders.Add(new ShortOffsetCountBlock());
                vtsh.EntryPoints.Add(new VertexShader.VertexShaderEntryPoint { SupportedVertexTypes = new List<ShortOffsetCountBlock>() });
            }
            
            foreach (var entry in supportedEntries)
            {
                // pixel shader
                ShaderGeneratorResult pixelResult = generator.GeneratePixelShader(eExplicitShader, entry);

                pixl.EntryPointShaders[(int)entry].Count = 1;
                pixl.EntryPointShaders[(int)entry].Offset = (byte)pixl.Shaders.Count;

                var pixelShaderBlock = new PixelShaderBlock
                {
                    PCShaderBytecode = pixelResult.Bytecode,
                    PCConstantTable = BuildConstantTable(cache, pixelResult, ShaderType.PixelShader)
                };

                pixl.Shaders.Add(pixelShaderBlock);

                // vertex shaders
                foreach (var vertex in supportedVertices)
                {
                    for (int i = 0; vtsh.EntryPoints[(int)entry].SupportedVertexTypes.Count <= (int)vertex; i++)
                        vtsh.EntryPoints[(int)entry].SupportedVertexTypes.Add(new ShortOffsetCountBlock());

                    ShaderGeneratorResult vertexResult = generator.GenerateVertexShader(eExplicitShader, entry, vertex);

                    vtsh.EntryPoints[(int)entry].SupportedVertexTypes[(int)vertex].Count = 1;
                    vtsh.EntryPoints[(int)entry].SupportedVertexTypes[(int)vertex].Offset = (byte)vtsh.Shaders.Count;

                    var vertexShaderBlock = new VertexShaderBlock
                    {
                        PCShaderBytecode = vertexResult.Bytecode,
                        PCConstantTable = BuildConstantTable(cache, vertexResult, ShaderType.VertexShader)
                    };

                    vtsh.Shaders.Add(vertexShaderBlock);
                }
            }
        }
    }
}
