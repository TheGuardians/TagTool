using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

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
                "Generate <index>",
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

            ShaderGenerator.ShaderGenerator.ShaderType type = ShaderGenerator.ShaderGenerator.ShaderType.ShaderTemplate;
            int index;
            if (args.Count > 1)
            {
                index = int.Parse(args[1]);
                switch(args[0].ToLower())
                {
                    case "shader_template":
                        type = ShaderGenerator.ShaderGenerator.ShaderType.ShaderTemplate;
                        break;
                    case "decals_template":
                        type = ShaderGenerator.ShaderGenerator.ShaderType.DecalsTemplate;
                        break;
                }
            }
            else index = int.Parse(args[0]);



            byte[] bytecode = ShaderGenerator.ShaderGenerator.GenerateSource(type,
                new ShaderGenerator.ShaderGenerator.Parameters
                {
                    albedo = ShaderGenerator.ShaderGenerator.Albedo.Constant_Color
                });

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
                    shader_data_block.PCParameters = existing_block.PCParameters;

                    _definition.Shaders[index] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    var existing_block = _definition.Shaders[index];
                    shader_data_block.PCParameters = existing_block.PCParameters;

                    _definition.Shaders[index] = shader_data_block;
                }
            }

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {

                var shader_data_block = new VertexShaderBlock
                {
                    PCShaderBytecode = bytecode
                };

                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    var existing_block = _definition.Shaders[index];
                    shader_data_block.PCParameters = existing_block.PCParameters;

                    _definition.Shaders[index] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
                    var existing_block = _definition.Shaders[index];
                    shader_data_block.PCParameters = existing_block.PCParameters;


                    _definition.Shaders[index] = shader_data_block;
                }
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