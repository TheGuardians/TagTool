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

        private ShaderGeneratorResult shader_template_gen(List<string> args)
        {
            int arg_pos = 0;

            ShaderTemplateShaderGenerator generator = null;
            try
            {
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
            
            ShaderGeneratorResult shader_gen_result;
            switch(type)
            {
                case "shader_template":
                    shader_gen_result = shader_template_gen(args.Skip(2).ToList());
                    break;
                //case "decals_template":
                //    break;
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