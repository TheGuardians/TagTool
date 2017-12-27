using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.Geometry;
using BlamCore.TagDefinitions;
using System;
using System.Collections.Generic;

namespace TagTool.PixelShaders
{
    class DisassembleCommand : Command
	{
		private GameCacheContext CacheContext { get; }
		private CachedTagInstance Tag { get; }
		private PixelShader Definition { get; }

		public DisassembleCommand(GameCacheContext cacheContext, CachedTagInstance tag, PixelShader definition) :
			base(CommandFlags.Inherit,

				"Disassemble",
				"Disassembles a PixelShader at the specified index.",

				"Disassemble <index>",

				"<index> - index into the PixelShaders tagblock.")
		{
			CacheContext = cacheContext;
			Tag = tag;
			Definition = definition;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 1)
				return false;

			Console.WriteLine(ShaderCompiler.Disassemble(Definition.Shaders[int.Parse(args[0])].ByteCode));

			return true;
		}
	}
}
