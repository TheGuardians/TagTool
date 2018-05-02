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
    class GenerateShader<T> : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private T Definition { get; }
        public static bool IsVertexShader => typeof(T) == typeof(GlobalVertexShader) || typeof(T) == typeof(VertexShader);
        public static bool IsPixelShader => typeof(T) == typeof(GlobalPixelShader) || typeof(T) == typeof(PixelShader);

        public GenerateShader(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
            base(CommandFlags.Inherit,

                "Generate",
                "Compiles HLSL source file from scratch :D",
                "Generate <index> <shader_type> <drawmode> <parameters...>",
                "Compiles HLSL source file from scratch :D")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
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

            Int32 index;
            string type;
            string drawmode_str;
            try
            {
                index = Int32.Parse(args[0]);
                type = args[1].ToLower();
                drawmode_str = args[2].ToLower();
            } catch
            {
                Console.WriteLine("Invalid index, type, and drawmode combination");
                return false;
            }

            var drawmode = TemplateShaderGenerator.Drawmode.Default;
            {
                bool found_drawmode = false;
                var drawmode_enums = Enum.GetValues(typeof(TemplateShaderGenerator.Drawmode)).Cast<TemplateShaderGenerator.Drawmode>();
                foreach (var drawmode_enum in drawmode_enums)
                {
                    var enum_name = Enum.GetName(typeof(TemplateShaderGenerator.Drawmode), drawmode_enum).ToLower();
                    if (drawmode_str == enum_name)
                    {
                        drawmode = drawmode_enum;
                        found_drawmode = true;
                        break;
                    }
                }

                if (!found_drawmode)
                {
                    //try
                    //{
                    //    drawmode = (TemplateShaderGenerator.Drawmode)Int32.Parse(drawmode_str);
                    //}
                    //catch
                    {
                        Console.WriteLine("Invalid shader arguments! (could not parse to drawmode)");
                        return false;
                    }
                }
            }

            Int32[] shader_args;
			try { shader_args = Array.ConvertAll(args.Skip(3).ToArray(), Int32.Parse); }
			catch { Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return false; }

			// runs the appropriate shader-generator for the template type.
			var shader_gen_result = new ShaderGeneratorResult();
            switch(type)
            {
                case "beam_templates":
                case "beam_template":
					shader_gen_result = new BeamTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "contrail_templates":
                case "contrail_template": // we already have all contrail_templates from H3 MP
					shader_gen_result = new ContrailTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "cortana_templates":
				case "cortana_template": // we already have all cortana_templates from H3 MP
					shader_gen_result = new CortanaTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "custom_templates":
				case "custom_template":
					shader_gen_result = new CustomTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "decal_templates":
                case "decal_template":
					shader_gen_result = new DecalTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                    break;
                case "foliage_templates":
                case "foliage_template": // we already have all foliage_templates from H3 MP
					shader_gen_result = new FoliageTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
                case "halogram_templates":
                case "halogram_template":
					shader_gen_result = new HalogramTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
                case "light_volume_templates":
                case "light_volume_template": // we already have all light_volume_templates from H3 MP
					shader_gen_result = new LightVolumeTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "particle_templates":
				case "particle_template":
					shader_gen_result = new ParticleTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "screen_templates":
				case "screen_template":
					shader_gen_result = new ScreenTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
				case "shader_templates":
                case "shader_template":
					shader_gen_result = new ShaderTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                    break;
                case "terrain_templates":
                case "terrain_template":
					shader_gen_result = new TerrainTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                    break;
                case "water_templates":
                case "water_template":
					shader_gen_result = new WaterTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
					break;
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