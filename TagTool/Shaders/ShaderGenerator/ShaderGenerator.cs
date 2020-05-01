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

                shaderParameter.ParameterName = cache.StringTable.GetStringId(register.Name);

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
                PCParameters = GenerateShaderParametersFromGenerator(cache, result)
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

        public static RenderMethodTemplate GenerateRenderMethodTemplate(GameCache cache, Stream cacheStream, RenderMethodDefinition rmdf, GlobalPixelShader glps, GlobalVertexShader glvs, IShaderGenerator generator, string shaderName)
        {

            var rmt2 = new RenderMethodTemplate();

            var pixl = GeneratePixelShader(cache, generator);
            var vtsh = GenerateVertexShader(cache, generator);


            var pixlTag = cache.TagCache.AllocateTag<PixelShader>(shaderName);
            cache.Serialize(cacheStream, pixlTag, pixl);
            rmt2.PixelShader = pixlTag;

            var vtshTag = cache.TagCache.AllocateTag<VertexShader>(shaderName);
            cache.Serialize(cacheStream, vtshTag, vtsh);
            rmt2.VertexShader = vtshTag;


            foreach (ShaderStage mode in Enum.GetValues(typeof(ShaderStage)))
                if (generator.IsEntryPointSupported(mode))
                    rmt2.ValidEntryPoints |= (EntryPointBitMask)(1 << (int)mode);


            rmt2.RealParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.TextureParameterNames = new List<RenderMethodTemplate.ShaderArgument>();

            rmt2.Parameters = new List<RenderMethodTemplate.ParameterMapping>();
            rmt2.ParameterTables = new List<RenderMethodTemplate.ParameterTable>();
            rmt2.EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();

            var pixelShaderResult = new ShaderGeneratorResult(null);
            
            foreach (ShaderStage mode in Enum.GetValues(typeof(ShaderStage)))
            {
                
                var rmt2Drawmode = new RenderMethodTemplate.PackedInteger_10_6();
                rmt2.EntryPoints.Add(rmt2Drawmode);

                if(generator.IsEntryPointSupported(mode))
                {
                    rmt2Drawmode.Offset = (ushort)rmt2.ParameterTables.Count();
                    rmt2Drawmode.Count = 1;


                    var registerOffsets = new RenderMethodTemplate.ParameterTable();
                    for (int i = 0; i < registerOffsets.Values.Length; i++)
                        registerOffsets.Values[i] = new RenderMethodTemplate.PackedInteger_10_6();
                    rmt2.ParameterTables.Add(registerOffsets);
                }
                /*
                registerOffsets[ParameterUsage.TextureExtern].Offset = (ushort)rmt2.Parameters.Count;
                var srcRenderMethodExternArguments = pixelShaderResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.RenderMethodExtern_Arguments);
                foreach (var src_arg in srcRenderMethodExternArguments)
                {
                    var argument_mapping = new RenderMethodTemplate.ParameterMapping
                    {
                        RegisterIndex = (ushort)src_arg.Register
                    };

                    foreach (var _enum in Enum.GetValues(typeof(RenderMethodExtern)))
                    {
                        if (_enum.ToString().ToLower() == src_arg.Name)
                        {
                            argument_mapping.ArgumentIndex = (byte)_enum;
                            break;
                        }
                    }

                    rmt2.Parameters.Add(argument_mapping);
                    registerOffsets[ParameterUsage.TextureExtern].Count++;
                }

                registerOffsets[ParameterUsage.Texture].Offset = (ushort)rmt2.Parameters.Count;
                var srcSamplerArguments = pixelShaderResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.TextureSampler_Arguments);
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    var argumentMapping = new RenderMethodTemplate.ParameterMapping
                    {
                        RegisterIndex = (ushort)samplerRegister.Register,
                        ArgumentIndex = (byte)registerOffsets[ParameterUsage.Texture].Count++
                    };

                    rmt2.Parameters.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = cache.StringTable.GetStringId(samplerRegister.Name)
                    };
                    rmt2.TextureParameterNames.Add(shaderArgument);

                }

                registerOffsets[ParameterUsage.PS_Real].Offset = (ushort)rmt2.Parameters.Count;
                // add xform args
                foreach (var samplerRegister in srcSamplerArguments)
                {
                    int index = GetArgumentIndex(cache, samplerRegister.Name, rmt2.RealParameterNames);
                    HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister xformRegister = null;
                    foreach (var register in pixelShaderResult.Registers)
                    {
                        if (register.Name == $"{samplerRegister.Name}_xform")
                        {
                            xformRegister = register;
                            break;
                        }
                    }
                    if (xformRegister == null) continue;

                    var argumentMapping = new RenderMethodTemplate.ParameterMapping
                    {
                        RegisterIndex = (ushort)xformRegister.Register,

                        ArgumentIndex = (byte)(index != -1 ? index : rmt2.RealParameterNames.Count)
                    };
                    rmt2.Parameters.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = cache.StringTable.GetStringId(samplerRegister.Name)
                    };
                    rmt2.RealParameterNames.Add(shaderArgument);

                    registerOffsets[ParameterUsage.PS_Real].Count++;
                }

                var srcVectorArguments = pixelShaderResult.Registers.Where(r => r.Scope == HaloShaderGenerator.ShaderGeneratorResult.ShaderRegister.ShaderRegisterScope.Vector_Arguments);
                foreach (var vectorRegister in srcVectorArguments)
                {
                    if (vectorRegister.IsXFormArgument) continue; // we've already added these
                    var argumentMapping = new RenderMethodTemplate.ParameterMapping
                    {
                        RegisterIndex = (ushort)vectorRegister.Register,
                        ArgumentIndex = (byte)rmt2.RealParameterNames.Count
                    };
                    rmt2.Parameters.Add(argumentMapping);

                    var shaderArgument = new RenderMethodTemplate.ShaderArgument
                    {
                        Name = cache.StringTable.GetStringId(vectorRegister.Name)
                    };
                    rmt2.RealParameterNames.Add(shaderArgument);

                    registerOffsets[ParameterUsage.PS_Real].Count++;
                }
                */
            }
            
            return rmt2;
        }
    }

    
}
