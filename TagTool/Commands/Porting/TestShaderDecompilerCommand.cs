using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.ShaderDecompiler;
using TagTool.Tags.Definitions;
using TagTool.Direct3D.Functions;
using TagTool.Direct3D.Enums;

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

			"DecompileShader <shader_index> <pixl_tag_name>",

			"shader_index - index into the Shaders block of the shader you wish to decompile.\n" +
			"pixl_tag_name - the name of the pixl tag which contains the shader you wish to decompile.")
		{
			CacheContext = cacheContext;
			BlamCache = blamCache;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 1 && args.Count != 2)
				return false;

			int.TryParse(args[0], out int shaderIndex);

			PixelShader pixl = new PixelShader();
			foreach (var tag in BlamCache.IndexItems)
			{
				if (tag.ClassCode != "pixl" || (args.Count == 2 && tag.Filename != args[1]))
					continue;

				var blamContext = new CacheSerializationContext(BlamCache, tag);
				pixl = BlamCache.Deserializer.Deserialize<PixelShader>(blamContext);

				if (pixl.Shaders != null && pixl.Shaders.Count > 0)
				{
					foreach (var shader in pixl.Shaders)
					{
						var debugData = shader.XboxShaderReference.DebugData;
						var constantData = shader.XboxShaderReference.ConstantData;
						var shaderData = shader.XboxShaderReference.ShaderData;

						var hlsl = Decompiler.Decompile(debugData, constantData, shaderData);

						var flags1 = CompileConstants.ENABLE_BACKWARDS_COMPATIBILITY;

						new Compile(hlsl, "main", "ps_3_0", flags1, out string errors, out byte[] pcShaderData);
						new PrintError(hlsl, errors, out bool isError);

						if (isError)
						{
							Console.ReadLine();
						}
						else
						{
							Console.WriteLine("Shader successfully recompiled for DX9!");
							Console.WriteLine("(further processing needed to very they are 1:1 with the original)");

							// *** Check if shader is 1:1 with original shader:
							// Some things to mention: shaders that are logically 1:1 may not always contain
							// the exact same instructions. Ex. `mul r0, c0.xy, c1.yx` is the logically the
							// same as `mul r0, c1.yx, c0.xy`; however the instructions are not literally
							// identicle. Automated checks are only guaranteed if they determine the shader
							// is 1:1, however if they determine they aren't 1:1, manual checking needs to be 
							// done to confirm. Automated checks should only be used to minimize amount of
							// manual checking needed.
							//
							// A modded x360 could be loaded with a custom game/app/program which can automate
							// logical 1:1 checks by comparing shader execution outputs.
							//
							// Automated 1:1 filtering steps on PC with manual interjection:
							// 1) Compile shader with x360 as target, with UPDB generation.
							// 2) Check if the 12-byte UPDB hash/hint at the end of both x360 versions of the shader are ==
							// 3) If the UPDB hash/hint are not equal:
							//		a) Disassemble both versions of the shader using xsd.exe and compare line-by-line.
							//		b) Highlight the first line of text that is different.
							//		c) Analyze manually to determine whether they are logically 1:1
						}
					}
				}
			}
			return true;
		}
	}
}