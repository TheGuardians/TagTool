using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.ShaderGenerator;
using TagTool.ShaderGenerator.Types;
using System.Linq;

namespace TagTool.Commands.Shaders
{
    class GenerateCommand<T> : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private T Definition { get; }
        public static bool IsVertexShader => typeof(T) == typeof(GlobalVertexShader) || typeof(T) == typeof(VertexShader);
        public static bool IsPixelShader => typeof(T) == typeof(GlobalPixelShader) || typeof(T) == typeof(PixelShader);

        public GenerateCommand(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
            base(CommandFlags.Inherit,

                "Generate",
                "Compiles HLSL source file from scratch :D",
                "Generate <index> <shader_type> <parameters...>",
                "Compiles HLSL source file from scratch :D")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

		/// <summary>
		/// Generates a shader from shader args. ex: 0_1_0_0_0_2_3_0
		/// </summary>
		/// <param name="args"> 0, 1, 0, 0, 0, 2, 3, 0 </param>
		/// <returns></returns>
        private ShaderGeneratorResult shader_template_gen(List<string> args)
        {
            int arg_pos = 0;

            ShaderTemplateShaderGenerator generator = null;
            try
            {
				// gets enum values for each shader arg (args are the numbers in the tagname)
                var albedo = (ShaderTemplateShaderGenerator.Albedo)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var bump_mapping = (ShaderTemplateShaderGenerator.Bump_Mapping)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var alpha_test = (ShaderTemplateShaderGenerator.Alpha_Test)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var specular_mask = (ShaderTemplateShaderGenerator.Specular_Mask)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var material_model = (ShaderTemplateShaderGenerator.Material_Model)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var environment_mapping = (ShaderTemplateShaderGenerator.Environment_Mapping)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var self_illumination = (ShaderTemplateShaderGenerator.Self_Illumination)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var blend_mode = (ShaderTemplateShaderGenerator.Blend_Mode)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var parallax = (ShaderTemplateShaderGenerator.Parallax)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var misc = (ShaderTemplateShaderGenerator.Misc)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var distortion = (ShaderTemplateShaderGenerator.Distortion)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var soft_fade = (ShaderTemplateShaderGenerator.Soft_Fade)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);

				// constructs a generator using the enum values that were gotten above.
                generator = new ShaderTemplateShaderGenerator(
                    CacheContext,
                    albedo,
                    bump_mapping,
                    alpha_test,
                    specular_mask,
                    material_model,
                    environment_mapping,
                    self_illumination,
                    blend_mode,
                    parallax,
                    misc,
                    distortion,
                    soft_fade
                );

            } catch
            {
                Console.WriteLine("Invalid arguments");
            }

			// generates HLSL and a List<Parameters> using the generator
            return generator?.Generate();
        }

        private ShaderGeneratorResult decals_template_gen(List<string> args)
        {
            int arg_pos = 0;

            DecalTemplateShaderGenerator generator = null;
            try
            {
                var albedo = (DecalTemplateShaderGenerator.Albedo)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var blend_mode = (DecalTemplateShaderGenerator.Blend_Mode)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var render_pass = (DecalTemplateShaderGenerator.Render_Pass)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var specular = (DecalTemplateShaderGenerator.Specular)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var bump_mapping = (DecalTemplateShaderGenerator.Bump_Mapping)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var tinting = (DecalTemplateShaderGenerator.Tinting)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);

                // constructs a generator using the enum values that were gotten above.
                generator = new DecalTemplateShaderGenerator(
                    CacheContext,
                    albedo,
                    blend_mode,
                    render_pass,
                    specular,
                    bump_mapping,
                    tinting
                );

            }
            catch
            {
                Console.WriteLine("Invalid arguments");
            }

            // generates HLSL and a List<Parameters> using the generator
            return generator?.Generate();
        }

        private ShaderGeneratorResult terrain_template_gen(List<string> args)
        {
            int arg_pos = 0;

            TerrainTemplateShaderGenerator generator = null;
            try
            {
                var blending = (TerrainTemplateShaderGenerator.Blending)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var environment_map = (TerrainTemplateShaderGenerator.Environment_Map)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var material_0 = (TerrainTemplateShaderGenerator.Material_0)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var material_1 = (TerrainTemplateShaderGenerator.Material_1)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var material_2 = (TerrainTemplateShaderGenerator.Material_2)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);
                var material_3 = (TerrainTemplateShaderGenerator.Material_3)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]);

                // constructs a generator using the enum values that were gotten above.
                generator = new TerrainTemplateShaderGenerator(
                    CacheContext,
                    blending,
                    environment_map,
                    material_0,
                    material_1,
                    material_2,
                    material_3
                );

            }
            catch
            {
                Console.WriteLine("Invalid arguments");
            }

            // generates HLSL and a List<Parameters> using the generator
            return generator?.Generate();
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

            int index;
            string type;
            try
            {
                index = int.Parse(args[0]);
                type = args[1].ToLower();
            } catch
            {
                Console.WriteLine("Invalid index and type combination");
                return false;
            }
            
			// runs the appropriate shader-generator for the template type.
            ShaderGeneratorResult shader_gen_result;
            switch(type)
            {
                case "shader_templates":
                case "shader_template":
                    shader_gen_result = shader_template_gen(args.Skip(2).ToList());
                    break;
                case "decal_templates":
                case "decal_template":
                    shader_gen_result = decals_template_gen(args.Skip(2).ToList());
                    break;
                case "terrain_templates":
                case "terrain_template":
                    shader_gen_result = terrain_template_gen(args.Skip(2).ToList());
                    break;
                case "beam_templates":
                case "beam_template":
                case "contrail_templates":
                case "contrail_template":
                case "cortana_templates":
                case "cortana_template":
                case "foliage_templates":
                case "foliage_template":
                case "halogram_templates":
                case "halogram_template":
                case "light_volume_templates":
                case "light_volume_template":
                case "particle_templates":
                case "particle_template":
                case "water_templates":
                case "water_template":
                default:
                    Console.WriteLine($"{type} is not implemented");
                    return false;
            }

            if (shader_gen_result == null) return false;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                var shader_data_block = new PixelShaderBlock
                {
                    PCShaderBytecode = shader_gen_result.ByteCode
                };

                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    shader_data_block.PCParameters = shader_gen_result.Parameters;

                    _definition.Shaders[index] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    shader_data_block.PCParameters = shader_gen_result.Parameters;

                    _definition.Shaders[index] = shader_data_block;
                }
            }
            else throw new NotImplementedException();

            //if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            //{

            //    var shader_data_block = new VertexShaderBlock
            //    {
            //        PCShaderBytecode = bytecode
            //    };

            //    if (typeof(T) == typeof(VertexShader))
            //    {
            //        var _definition = Definition as VertexShader;
            //        var existing_block = _definition.Shaders[index];
            //        shader_data_block.PCParameters = existing_block.PCParameters;

            //        _definition.Shaders[index] = shader_data_block;
            //    }

            //    if (typeof(T) == typeof(GlobalVertexShader))
            //    {
            //        var _definition = Definition as GlobalVertexShader;
            //        var existing_block = _definition.Shaders[index];
            //        shader_data_block.PCParameters = existing_block.PCParameters;


            //        _definition.Shaders[index] = shader_data_block;
            //    }
            //}

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
                            ParameterName = CacheContext.GetStringId(split[0]),
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