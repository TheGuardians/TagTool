using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.ShaderDecompiler;
using TagTool.Tags.Definitions;

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

			Console.WriteLine(hlsl);

			File.WriteAllBytes(tagName.Replace('\\', '_'), shaderData);

			return true;
		}
	}
}