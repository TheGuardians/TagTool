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
            try
            {
                index = Int32.Parse(args[0]);
                type = args[1].ToLower();
            } catch
            {
                Console.WriteLine("Invalid index and type combination");
                return false;
            }

			Int32[] shader_args;
			try { shader_args = Array.ConvertAll(args.ToArray(), Int32.Parse); }
			catch { Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return false; }

			// runs the appropriate shader-generator for the template type.
			ShaderGeneratorResult shader_gen_result;
            switch(type)
            {
                case "beam_templates":
                case "beam_template":
					shader_gen_result = new BeamTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
					break;
				case "contrail_templates":
                case "contrail_template":
					shader_gen_result = new ContrailTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
					break;
                case "cortana_templates":
                case "cortana_template":
					shader_gen_result = new ContrailTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
					break;
				case "decal_templates":
                case "decal_template":
					shader_gen_result = new DecalTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
                    break;
                case "foliage_templates":
                case "foliage_template":
					goto default;
                case "halogram_templates":
                case "halogram_template":
					goto default;
                case "light_volume_templates":
                case "light_volume_template":
					goto default;
				case "particle_templates":
                case "particle_template":
					goto default;
				case "shader_templates":
                case "shader_template":
					shader_gen_result = new ShaderTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
                    break;
                case "terrain_templates":
                case "terrain_template":
					shader_gen_result = new TerrainTemplateShaderGenerator(CacheContext, shader_args)?.Generate();
                    break;
                case "water_templates":
                case "water_template":
					goto default;
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