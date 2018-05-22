using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Util;
using TagTool.Direct3D.Functions;

namespace TagTool.Commands.Shaders
{
    class CompileCommand<T> : Command
	{
		private HaloOnlineCacheContext CacheContext { get; }
		private CachedTagInstance Tag { get; }
		private T Definition { get; }
        public static bool IsVertexShader => typeof(T) == typeof(GlobalVertexShader) || typeof(T) == typeof(VertexShader);
        public static bool IsPixelShader => typeof(T) == typeof(GlobalPixelShader) || typeof(T) == typeof(PixelShader);

        public CompileCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, T definition) :
			base(true,

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
			if (args.Count < 2)
				return false;



            bool use_assembly_compiler = args.Count >= 2 && args[0].ToLower() == "asm";
            string file = args[args.Count - 1];
            string shader_code = File.ReadAllText(file);

            if (use_assembly_compiler && args.Count < 3)
                return false;

            int index = use_assembly_compiler ? int.Parse(args[1]) : int.Parse(args[0]);

            if (!use_assembly_compiler)
            {
                using (var reader = new StringReader(shader_code))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        var str = line.Trim();
                        if (str == "ps_3_0" || str == "vs_3_0")
                        {
                            use_assembly_compiler = true;
                            break;
                        }
                    }
                }
            }

            byte[] bytecode = null;
            if (use_assembly_compiler)
            {
                bytecode = DirectX.AssemblePCShader(shader_code);
            }
            else
            {
                var profile = IsVertexShader ? "vs_3_0" : "ps_3_0";
                new Compile(shader_code, "main", profile, 0, out string errors, out bytecode);

				new PrintError(shader_code, errors, out bool isError);
                if (isError)
                    return true;
            }

			new Disassemble(bytecode, out string disassembly);

            Console.WriteLine(disassembly);


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