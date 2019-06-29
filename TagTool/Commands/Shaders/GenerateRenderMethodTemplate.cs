using TagTool.Cache;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HaloShaderGeneratorLib;
using HaloShaderGeneratorLib.Enums;

namespace TagTool.Commands.Shaders
{
    public class GenerateRenderMethodTemplate : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderMethodTemplate Definition { get; }

        public GenerateRenderMethodTemplate(GameCacheContext cacheContext, CachedTagInstance tag, RenderMethodTemplate definition) :
            base(true,

                "Generate",
                "Compiles HLSL source file from scratch :D",
                "Generate <shader_type> <parameters...>",
                "Compiles HLSL source file from scratch :D")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
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

        public override object Execute(List<string> args)
        {
            if (args.Count <= 0)
                return false;

            if(args.Count < 2)
            {
                Console.WriteLine("Invalid number of args");
                return false;
            }

            string shader_type;
            try
            {
                shader_type = args[0].ToLower();
            } catch
            {
                Console.WriteLine("Invalid index, type, and drawmode combination");
                return false;
            }

            Int32[] shader_args;
			try { shader_args = Array.ConvertAll(args.Skip(1).ToArray(), Int32.Parse); }
			catch { Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return false; }

			// runs the appropriate shader-generator for the template type.
            switch(shader_type)
            {
                case "shader_templates":
                case "shader_template":
                    {
                        ResetRMT2(Definition);

                        if (HaloShaderGenerator.IsShaderSuppored(ShaderType.Shader, ShaderStage.Default))
                        {
                            var result = HaloShaderGenerator.GenerateShader(
                                ShaderStage.Albedo,
                                Albedo.Default,
                                Bump_Mapping.Off,
                                Alpha_Test.None,
                                Specular_Mask.No_Specular_Mask,
                                Material_Model.None,
                                Environment_Mapping.None,
                                Self_Illumination.Off,
                                Blend_Mode.Opaque,
                                Parallax.Off,
                                Misc.First_Person_Always,
                                Distortion.Off,
                                Soft_fade.Off
                            );

                            Console.WriteLine($"Generated Shader : {result?.Bytecode?.Length ?? 0} bytes");

                            Definition.DrawModeBitmask |= RenderMethodTemplate.ShaderModeBitmask.Albedo;
                        }
                    }
                    break;
                case "beam_templates":
                case "beam_template":
                    {
                        ResetRMT2(Definition);

                        if (HaloShaderGenerator.IsShaderSuppored(ShaderType.Beam, ShaderStage.Default))
                        {
                            var result = HaloShaderGenerator.GenerateShader(
                                ShaderStage.Albedo,
                                Albedo.Two_Change_Color,
                                Bump_Mapping.Off,
                                Alpha_Test.None,
                                Specular_Mask.No_Specular_Mask,
                                Material_Model.None,
                                Environment_Mapping.None,
                                Self_Illumination.Off,
                                Blend_Mode.Opaque,
                                Parallax.Off,
                                Misc.First_Person_Always,
                                Distortion.Off,
                                Soft_fade.Off
                            );

                            Console.WriteLine($"Generated Shader : {result?.Bytecode?.Length ?? 0} bytes");

                            Definition.DrawModeBitmask |= RenderMethodTemplate.ShaderModeBitmask.Default;
                        }

                        //TODO: Extract shader parameters

                        //foreach (var param in result_default.Parameters)
                        //{
                        //    var param_name = CacheContext.GetString(param.ParameterName);

                        //    var mapping = GlobalUniformMappings.GetMapping(param_name, param.RegisterType, RenderMethodTemplate.ShaderMode.Default);

                        //    if (mapping != null)
                        //    {
                        //        Console.WriteLine($"SUCCESS: Found parameter {param_name} register_index:{param.RegisterIndex} argument_index:{mapping.ArgumentIndex}");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("WARNING: Missing parameter " + param_name);
                        //    }
                        //}
                    }
                    break;
				case "contrail_templates":
                case "contrail_template":
				case "cortana_templates":
				case "cortana_template":
				case "custom_templates":
				case "custom_template":
				case "decal_templates":
                case "decal_template":
                case "foliage_templates":
                case "foliage_template":
                case "halogram_templates":
                case "halogram_template":
                case "light_volume_templates":
                case "light_volume_template":
				case "particle_templates":
				case "particle_template":
				case "screen_templates":
				case "screen_template":
                case "terrain_templates":
                case "terrain_template":
                case "water_templates":
                case "water_template":
                    Console.WriteLine($"{shader_type} is not implemented");
                    return false;
                default:
                    Console.WriteLine($"Unknown template {shader_type}");
                    return false;
            }

            return true;
        }

        public List<ShaderParameter> GetParamInfo(string assembly)
        {
            var parameters = new List<ShaderParameter> { };

            using (StringReader reader = new StringReader(assembly))
            {
                if (string.IsNullOrEmpty(assembly))
                    return null;

                string line;

                while (!(line = reader.ReadLine()).Contains("//   -"))
                    continue;

                while (!string.IsNullOrEmpty((line = reader.ReadLine())))
                {
                    line = (line.Replace("//   ", "").Replace("//", "").Replace(";", ""));

                    while (line.Contains("  "))
                        line = line.Replace("  ", " ");

                    if (!string.IsNullOrEmpty(line))
                    {
                        var split = line.Split(' ');
                        parameters.Add(new ShaderParameter
                        {
                            ParameterName = (CacheContext as HaloOnlineCacheContext).GetStringId(split[0]),
                            RegisterType = (ShaderParameter.RType)Enum.Parse(typeof(ShaderParameter.RType), split[1][0].ToString()),
                            RegisterIndex = byte.Parse(split[1].Substring(1)),
                            RegisterCount = byte.Parse(split[2])
                        });
                    }
                }
            }

            return parameters;
        }
    }
}