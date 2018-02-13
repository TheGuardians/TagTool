using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.VertexShaders
{
    class DisassembleCommand : Command
	{
		private GameCacheContext CacheContext { get; }
		private CachedTagInstance Tag { get; }
		private VertexShader Definition { get; }

		public DisassembleCommand(GameCacheContext cacheContext, CachedTagInstance tag, VertexShader definition) :
			base(CommandFlags.Inherit,

				"Disassemble",
				"Disassembles a VertexShader at the specified index.",

				"Disassemble <index>",

				"<index> - index into the VertexShaders tagblock.")
		{
			CacheContext = cacheContext;
			Tag = tag;
			Definition = definition;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 1)
				return false;

			Console.WriteLine(ShaderCompiler.Disassemble(Definition.Shaders[int.Parse(args[0])].PcCompiledShader));

			return true;
		}
	}
}
