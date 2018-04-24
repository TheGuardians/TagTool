using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.ShaderDecompiler;
using TagTool.Tags.Definitions;
using TagTool.Direct3D.Functions;

namespace TagTool.Commands.Porting
{
	class TestShaderDecompilerCommand : Command
	{
		private CacheFile BlamCache { get; }
		private GameCacheContext CacheContext { get; }

		public TestShaderDecompilerCommand(GameCacheContext cacheContext, CacheFile blamCache) : base(
			CommandFlags.None,

			"DecompileShader",
			"Test command for xbox360 shader decompilation.",

			"DecompileShader <shader_index> <tag_name>",

			"shader_index - index into the Shaders block of the shader you wish to decompile.\n" +
			"tag_name - the name of the tag which contains the shader you wish to decompile.")
		{
			CacheContext = cacheContext;
			BlamCache = blamCache;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 2)
				return false;

			int.TryParse(args[0], out int shaderIndex);
			var tagName = args[1];

			PixelShader pixl = new PixelShader();
			foreach (var tag in BlamCache.IndexItems)
			{
				if (tag.ClassCode != "pixl" || tag.Filename != tagName)
					continue;

				var blamContext = new CacheSerializationContext(BlamCache, tag);
				pixl = BlamCache.Deserializer.Deserialize<PixelShader>(blamContext);
				break;
			}

			if (pixl.Equals(new PixelShader()))
			{
				Console.WriteLine($"Unable to locate tag: {tagName}\n " +
					$"Please check your spelling and verify the tag exists.");
				return false;
			}

			var shaderData = pixl.Shaders[shaderIndex].XboxShaderReference.ShaderData;

			var hlsl = Decompiler.Decompile(shaderData);

			/*
			* Uncomment the code below to test compiling the decompilation into dx9 bytecode,
			* and then printing the disassembly of the dx9 bytecode.
			*/

			new Compile(hlsl, "main", "ps_3_0", out string errors, out shaderData);
			new PrintError(hlsl, errors, out bool isError);
			//**********************************************************************************

			if (!isError)
				File.WriteAllBytes("test", shaderData);

			return true;
		}
	}
}