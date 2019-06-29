using TagTool.Cache;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HaloShaderGeneratorLib;
using HaloShaderGeneratorLib.Enums;
using System.Reflection;

namespace TagTool.Commands.Shaders
{
    public class GenerateShader<T> : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private T Definition { get; }
        public static bool IsVertexShader => typeof(T) == typeof(GlobalVertexShader) || typeof(T) == typeof(VertexShader);
        public static bool IsPixelShader => typeof(T) == typeof(GlobalPixelShader) || typeof(T) == typeof(PixelShader);

        public GenerateShader(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
            base(true,

                "Generate",
                "Compiles HLSL source file from scratch :D",
                "Generate <index> <shader_type> <drawmode> <parameters...>",
                "Compiles HLSL source file from scratch :D")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public object[] CreateArguments(MethodInfo method, ShaderStage stage, Int32[] template)
        {
            var _params = method.GetParameters();
            object[] input_params = new object[_params.Length];

            for(int i=0;i<_params.Length;i++)
            {
                if (i == 0) input_params[0] = stage;
                else
                {
                    var template_index = i - 1;
                    if(template_index < template.Length)
                    {
                        var _enum = Enum.ToObject(_params[i].ParameterType, template[template_index]);
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

            var shader_stage = ShaderStage.Default;
            foreach(var _shader_stage in Enum.GetValues(typeof(ShaderStage)).Cast<ShaderStage>())
            {
                if(drawmode_str == _shader_stage.ToString().ToLower())
                {
                    shader_stage = _shader_stage;
                }
            }

            Int32[] shader_args;
            try { shader_args = Array.ConvertAll(args.Skip(3).ToArray(), Int32.Parse); }
            catch { Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return false; }
            

            // runs the appropriate shader-generator for the template type.
            byte[] bytecode = null;
            switch(type)
            {
                case "shader_templates":
                case "shader_template":


                    if (HaloShaderGenerator.IsShaderSuppored(ShaderType.Shader, ShaderStage.Albedo))
                    {
                        var GenerateShader = typeof(HaloShaderGenerator).GetMethod("GenerateShader");
                        var GenerateShaderArgs = CreateArguments(GenerateShader, shader_stage, shader_args);
                        bytecode = GenerateShader.Invoke(null, GenerateShaderArgs) as byte[];

                        Console.WriteLine(bytecode?.Length ?? -1);
                    }

                    break;
                case "beam_templates":
                case "beam_template":
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
				default:
                    Console.WriteLine($"{type} is not implemented");
                    return false;
            }

            if (bytecode == null) return false;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                var shader_data_block = new PixelShaderBlock
                {
                    PCShaderBytecode = bytecode
                };

                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    //TODO: Set parameters
                    //shader_data_block.PCParameters = shader_gen_result.Parameters;

                    _definition.Shaders[index] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    //TODO: Set parameters
                    //shader_data_block.PCParameters = shader_gen_result.Parameters;

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