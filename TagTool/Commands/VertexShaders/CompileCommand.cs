using BlamCore.Cache;
using BlamCore.Geometry;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using static BlamCore.TagDefinitions.VertexShader.ShaderBlock.ParameterBlock;

namespace TagTool.Commands.VertexShaders
{
    class CompileCommand : Command
	{
		private GameCacheContext CacheContext { get; }
		private CachedTagInstance Tag { get; }
		private VertexShader Definition { get; }

		public CompileCommand(GameCacheContext cacheContext, CachedTagInstance tag, VertexShader definition) :
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

            var vtshblck = new VertexShader.ShaderBlock
            {
                ByteCode = bytecode,
                Parameters = GetParamInfo(disassembly),
                Unknown8 = 1
            };

            Definition.Shaders[int.Parse(args[0])] = vtshblck;

			using (var stream = CacheContext.OpenTagCacheReadWrite())
			{
				var context = new TagSerializationContext(stream, CacheContext, Tag);
				CacheContext.Serializer.Serialize(context, Definition);
			}

			return true;
		}

		public List<VertexShader.ShaderBlock.ParameterBlock> GetParamInfo(string assembly)
		{
			var parameters = new List<VertexShader.ShaderBlock.ParameterBlock> { };

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
                        var param = new VertexShader.ShaderBlock.ParameterBlock
                        {
                            ParameterName = CacheContext.GetStringId(split[0]),
                            RegisterType = (RType)Enum.Parse(typeof(RType), split[1][0].ToString()),
                            RegisterIndex = byte.Parse(split[1].Substring(1)),
                            RegisterCount = byte.Parse(split[2])
                        };
                        parameters.Add(param);
					}
				}
			}
			return parameters;
		}
	}
}