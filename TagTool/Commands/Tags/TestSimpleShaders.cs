using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Direct3D.Enums;
using TagTool.Direct3D.Functions;
using CSharpImageLibrary;
using System.Linq;

namespace TagTool.Commands.Tags
{
	class TestSimpleShadersCommand : Command
	{
		public HaloOnlineCacheContext CacheContext { get; }

		public TestSimpleShadersCommand(HaloOnlineCacheContext cacheContext)
			: base(true,

				  "SimplifyShaders",
				  "Sets the name of a tag file in the current cache.",

				  "NameTag <tag> <name> [csv path]",

				  "<tag>  - A valid tag index, tag name, or * for the last tag in the current cache. \n" +
				  "\n" +
				  "<name> - The name of the tag. Should be a concise name that resembles the format \n" +
				  "         of existing tag names.\n")
		{
			CacheContext = cacheContext;
		}

		public override object Execute(List<string> args)
		{
			using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
			{
				// build our default pixl which outputs albedo_color.
				var tagContext = new TagSerializationContext(tagsStream, CacheContext, CacheContext.TagCache.Index[0x1894]);
				var pixl = CacheContext.Deserializer.Deserialize<PixelShader>(tagContext);
				pixl.Shaders[0].PCShaderBytecode = GetShader();
				CacheContext.Serialize(tagContext, pixl);

				// extract our default rmsh
				byte[] rmsh_data = CacheContext.TagCache.ExtractTagRaw(tagsStream, CacheContext.TagCache.Index[0x1AE7]);

				// foreach rmsh tag:
				var groups = new List<Tag> { };
				groups.Add(new Tag("rmtr"));
				foreach (var tag in CacheContext.TagCache.Index.FindAllInGroups(groups))
				{
					tagContext = new TagSerializationContext(tagsStream, CacheContext, tag);
					var rmtr = CacheContext.Deserializer.Deserialize<ShaderTerrain>(tagContext);

					if (rmtr.ShaderProperties.Count > 0 && 
						rmtr.ShaderProperties[0].ShaderMaps.Count > 0 &&
						!Omits.Contains(tag.Index))
					{
						// get average color of the first bitm in the shader.
						float[] color;
						var extractor = new BitmapDdsExtractor(CacheContext);
						tagContext = new TagSerializationContext(tagsStream, CacheContext, rmtr.ShaderProperties[0].ShaderMaps[0].Bitmap);
						var bitmap = CacheContext.Deserializer.Deserialize<Bitmap>(tagContext);
						using (var outStream = new MemoryStream())
						{
							extractor.ExtractDds(bitmap, 0, outStream);
							color = GetAverageColor(outStream);
						}

						// import our default rmsh over the rmsh tag
						CacheContext.TagCache.SetTagDataRaw(tagsStream, tag, rmsh_data);

						// deserialize our NEW rmsh (at the same index as before
						tagContext = new TagSerializationContext(tagsStream, CacheContext, tag);
						rmtr = CacheContext.Deserializer.Deserialize<ShaderTerrain>(tagContext);

                        // setup or color from above into the albedo_color arguments of our new rmsh
                        rmtr.ShaderProperties[0].Arguments[2].Values = color;

						// serialize our rmsh
						CacheContext.Serialize(tagContext, rmtr);
						Console.WriteLine($"Updated 0x{tag.Index.ToString("X4")}.rmsh");
					}
				}
			}

			return true;
		}

		public float[] GetAverageColor(MemoryStream stream)
		{
			var random = new Random(DateTime.Now.Millisecond);
			byte[] pixls;
			var fcolor = new float[4];
			
			try
			{
				var engineImage = new ImageEngineImage(stream);
				var wpfBitmap = engineImage.GetWPFBitmap();
				pixls = new byte[wpfBitmap.PixelWidth * wpfBitmap.PixelHeight * 4];
				wpfBitmap.CopyPixels(pixls, wpfBitmap.PixelWidth * 4, 0);

				// get an average color of the pixels... 
				uint r = 0, g = 0, b = 0, a = 0;
				for (var p = 0; p < pixls.Length; p += 4)
				{
					r += pixls[p + 0];
					g += pixls[p + 1];
					b += pixls[p + 2];
					a += pixls[p + 3];
				}
				fcolor[0] = (r / (pixls.Length / 4)) / 255f;
				fcolor[1] = (g / (pixls.Length / 4)) / 255f;
				fcolor[2] = (b / (pixls.Length / 4)) / 255f;
				fcolor[3] = (a / (pixls.Length / 4)) / 255f;
				Console.WriteLine($"Average: {fcolor[0]}, {fcolor[1]}, {fcolor[2]}, {fcolor[3]}");
			}
			catch(Exception e) { // if above fails for some reason, use a random color, with 1 alpha...
				pixls = new byte[4];
				random.NextBytes(pixls);
				fcolor[0] = pixls[0] / 255f;
				fcolor[1] = pixls[1] / 255f;
				fcolor[2] = pixls[2] / 255f;
				fcolor[3] = 1.0f;
				Console.WriteLine(e.Message);
				Console.WriteLine($"Random: {fcolor[0]}, {fcolor[1]}, {fcolor[2]}, {fcolor[3]}");
			}

			return fcolor;
		}

		public byte[] GetShader()
		{
			var hlsl =
		"uniform float4 albedo_color : register(c58);" +
		"struct PS_OUTPUT																		" +
		"{																						" +
		"	float4 COLOR0 : COLOR0;																" +
		"   float4 COLOR1 : COLOR1;																" +
		"   float4 COLOR2 : COLOR2;																" +
		"};																						" +
		"																						" +
		"PS_OUTPUT main()																		" +
		"{																						" +
		"	PS_OUTPUT Out;																		" +
		$"	Out.COLOR0 = albedo_color;" +
		$"	Out.COLOR1 = albedo_color;" +
		"	Out.COLOR2 = float4(0, 0, 0, 0);													" +
		"	return Out;																			" +
		"}";

			new Compile(hlsl, "main", "ps_3_0", CompileConstants.PARTIAL_PRECISION, out string errors, out byte[] data);

			if (!string.IsNullOrEmpty(errors))
				Console.WriteLine(errors);

			return data;
		}

		public static List<int> Omits = new List<int>
		{

		};
	}
}
