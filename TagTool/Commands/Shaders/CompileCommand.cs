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
    class CompileCommand<T> : Command
	{
		private GameCacheContext CacheContext { get; }
		private CachedTagInstance Tag { get; }
		private T Definition { get; }

		public CompileCommand(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
			base(CommandFlags.Inherit,

				"Compile",
				"Compiles HLSL source file against shader profile vs_3_0",

				"Compile <index> <file.hlsl>",

				"Compiles <file.hlsl> against shader profile vs_3_0" +
				"into shader bytecode, generates a VertexShaderBlock element\n" +
				"and writes it over <index>")
		{
			CacheContext = cacheContext;
			Tag = tag;
			Definition = definition;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 2)
				return false;


            string ps = File.ReadAllText(args[1]);

            var bytecode = ShaderCompiler.Compile(ps, "main", "vs_3_0", out string errors);

			if (ShaderCompiler.PrintError(errors))
				return true;

			var disassembly = ShaderCompiler.Disassemble(bytecode);



            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                var shader_data_block = new PixelShaderBlock
                {
                    PCShaderBytecode = bytecode,
                    PCParameters = GetParamInfo(disassembly),
                    //Unknown3 = CacheContext.GetStringId("default") // No evidence to suggest this is actually a stringid
                };

                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    _definition.Shaders[int.Parse(args[0])] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    _definition.Shaders[int.Parse(args[0])] = shader_data_block;
                }
            }

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {
                var shader_data_block = new VertexShaderBlock
                {
                    PCShaderBytecode = bytecode,
                    PCParameters = GetParamInfo(disassembly),
                    //Unknown3 = CacheContext.GetStringId("default") // No evidence to suggest this is actually a stringid
                };

                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    _definition.Shaders[int.Parse(args[0])] = shader_data_block;
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
                    _definition.Shaders[int.Parse(args[0])] = shader_data_block;
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