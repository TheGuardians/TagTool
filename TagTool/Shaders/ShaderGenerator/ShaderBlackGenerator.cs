using System;
using System.Collections.Generic;
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
    partial class ShaderGenerator
    {
        public static RenderMethodDefinition GenerateRenderMethodDefinitionShaderBlack(GameCache cache, IShaderGenerator generator)
        {
            RenderMethodDefinition rmdf = new RenderMethodDefinition();

            rmdf.RenderMethodOptions = cache.TagCache.GetTag(@"shaders\shader_options\global_shader_options", "rmop");
            /*
            rmdf.GlobalPixelShader = glpsTag;
            rmdf.GlobalVertexShader = glvsTag;
            rmdf.DrawModes = new List<RenderMethodDefinition.DrawMode>();
            rmdf.Vertices = new List<RenderMethodDefinition.VertexBlock>();
            

            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                if (HaloShaderGenerator.Black.Globals.IsShaderStageSupported(entryPoint))
                {
                    rmdf.DrawModes.Add(new RenderMethodDefinition.DrawMode { Mode = (int)entryPoint });
                }
            }

            foreach (VertexType vertexType in Enum.GetValues(typeof(VertexType)))
            {
                if (HaloShaderGenerator.Black.Globals.IsVertexTypeSupported(vertexType))
                {
                    rmdf.Vertices.Add(new RenderMethodDefinition.VertexBlock { VertexType = (short)vertexType });
                }
            }
            */
            // hackfix for methods

            rmdf.Methods = new List<RenderMethodDefinition.Method>();

            var blacknessStringid = cache.StringTable.GetStringId("blackness(no_options)");
            if (blacknessStringid == StringId.Invalid)
            {
                blacknessStringid = cache.StringTable.AddString("blackness(no_options)");
            }
            rmdf.Methods.Add(new RenderMethodDefinition.Method { Type = blacknessStringid, VertexShaderMethodMacroName = StringId.Invalid, PixelShaderMethodMacroName = StringId.Invalid });

            return rmdf;
        }

        public static GlobalPixelShader GenerateGlobalPixelShaderShaderBlack(GameCache cache)
        {
            return new GlobalPixelShader();
        }

    }

    /*
    public class RenderMethodTemplateShaderBlackGenerator : RenderMethodTemplateGenerator
    {
        public RenderMethodTemplateShaderBlackGenerator(GameCache cache, RenderMethodDefinition renderMethodDefinition) : base(cache, renderMethodDefinition) { }
        

        public RenderMethodTemplate Generate()
        {
            var rmt2 = new RenderMethodTemplate();

            rmt2.PixelShader = pixelShaderTag;
            rmt2.VertexShader = vertexShaderTag;
            // iterate on rmdf to find all entry points
            //rmt2.ValidEntryPoints |= EntryPointBitMask.Active_Camo;

            rmt2.RealParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.IntegerParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.BooleanParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.TextureParameterNames = new List<RenderMethodTemplate.ShaderArgument>();
            rmt2.Parameters = new List<RenderMethodTemplate.ParameterMapping>();
            rmt2.ParameterTables = new List<RenderMethodTemplate.ParameterTable>();
            rmt2.EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();



            foreach (EntryPoint mode in Enum.GetValues(typeof(EntryPoint)))
            {

                var rmt2Drawmode = new RenderMethodTemplate.PackedInteger_10_6();
                rmt2.EntryPoints.Add(rmt2Drawmode);

                if (!rmt2.ValidEntryPoints.HasFlag((EntryPointBitMask)(1 << (int)mode)))
                    continue;

                rmt2Drawmode.Offset = (ushort)rmt2.ParameterTables.Count();
                rmt2Drawmode.Count = 1;


                var registerOffsets = new RenderMethodTemplate.ParameterTable();
                for (int i = 0; i < registerOffsets.Values.Length; i++)
                    registerOffsets.Values[i] = new RenderMethodTemplate.PackedInteger_10_6();
                rmt2.ParameterTables.Add(registerOffsets);

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
            }
            return null;
        }
    }
 */
}
