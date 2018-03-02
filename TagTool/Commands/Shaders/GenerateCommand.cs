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

            ShaderGenerator.ShaderGenerator.ShaderGeneratorParameters shader_generator_params;
            ShaderGenerator.ShaderGenerator.ShaderType type;
            int index;

            try
            {
                type = ShaderGenerator.ShaderGenerator.ShaderType.ShaderTemplate;
                index = int.Parse(args[0]);
                switch (args[1].ToLower())
                {
                    case "shader_template":
                        type = ShaderGenerator.ShaderGenerator.ShaderType.ShaderTemplate;
                        break;
                    case "decals_template":
                        type = ShaderGenerator.ShaderGenerator.ShaderType.DecalsTemplate;
                        break;
                }

                int arg_pos = 2;
                shader_generator_params = new ShaderGenerator.ShaderGenerator.ShaderGeneratorParameters
                {
                    albedo = (ShaderGenerator.ShaderGenerator.Albedo)Int32.Parse(args[arg_pos++]),
                    bump_mapping = (ShaderGenerator.ShaderGenerator.Bump_Mapping)Int32.Parse(args[arg_pos++]),
                    alpha_test = (ShaderGenerator.ShaderGenerator.Alpha_Test)Int32.Parse(args[arg_pos++]),
                    specular_mask = (ShaderGenerator.ShaderGenerator.Specular_Mask)Int32.Parse(args[arg_pos++]),
                    material_model = (ShaderGenerator.ShaderGenerator.Material_Model)Int32.Parse(args[arg_pos++]),
                    environment_mapping = (ShaderGenerator.ShaderGenerator.Environment_Mapping)Int32.Parse(args[arg_pos++]),
                    self_illumination = (ShaderGenerator.ShaderGenerator.Self_Illumination)Int32.Parse(args[arg_pos++]),
                    blend_mode = (ShaderGenerator.ShaderGenerator.Blend_Mode)Int32.Parse(args[arg_pos++]),
                    parallax = (ShaderGenerator.ShaderGenerator.Parallax)Int32.Parse(args[arg_pos++]),
                    misc = (ShaderGenerator.ShaderGenerator.Misc)Int32.Parse(args[arg_pos++]),
                    //distortion = (ShaderGenerator.ShaderGenerator.Distortion)Int32.Parse(args[arg_pos++]),
                    //soft_fade = (ShaderGenerator.ShaderGenerator.Soft_Fade)Int32.Parse(args[arg_pos++])
                };

            } catch {
                return false;
            }

            var result = ShaderGenerator.ShaderGenerator.GenerateSource(type, shader_generator_params, CacheContext);

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                var shader_data_block = new PixelShaderBlock
                {
                    PCShaderBytecode = result.ByteCode
                };

                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    shader_data_block.PCParameters = result.Parameters;

                    _definition.Shaders[index] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    var existing_block = _definition.Shaders[index];
                    //shader_data_block.PCParameters = existing_block.PCParameters;
                    shader_data_block.PCParameters = result.Parameters;

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