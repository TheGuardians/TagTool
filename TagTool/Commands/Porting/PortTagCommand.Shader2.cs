using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
	partial class PortTagCommand
	{
		public RenderMethod ConvertRenderMethod_2(Stream cacheStream, RenderMethod rm)
		{
			for (var sp = 0; sp < rm.ShaderProperties.Count; sp++)
			{
				var rmt2_tag = rm.ShaderProperties[sp].Template;
				var context = new TagSerializationContext(cacheStream, CacheContext, rmt2_tag);
				var rmt2_new = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(context);
				var rmt2_old = OldRenderMethodTemplates[rmt2_tag.Index];

				// Fixup Arguments ordering.
				var new_args = new List<RenderMethod.ShaderProperty.Argument> { };
				for (var arg = 0; arg < rmt2_new.Arguments.Count; arg++)
				{
					var old_arg = rmt2_old.Arguments.FindIndex(a => a.Name == rmt2_new.Arguments[arg].Name);
					new_args.Add(rm.ShaderProperties[sp].Arguments[old_arg]);
				}
				rm.ShaderProperties[sp].Arguments = new_args;

				// Fixup ShaderMaps ordering
				var new_maps = new List<RenderMethod.ShaderProperty.ShaderMap> { };
				for (var map = 0; map < rmt2_new.ShaderMaps.Count; map++)
				{
					var old_map = rmt2_old.ShaderMaps.FindIndex(m => m.Name == rmt2_new.ShaderMaps[map].Name);
					new_maps.Add(rm.ShaderProperties[sp].ShaderMaps[old_map]);
				}
				rm.ShaderProperties[sp].ShaderMaps = new_maps;
			}

			return rm;
		}

		public RenderMethodTemplate ConvertRenderMethodTemplate_2(Stream cacheStream, RenderMethodTemplate rmt2, int index)
		{
			// Due to the way PortTag works, rmt2 gets ported before rm__; 
			// Since fields in rm__ need to align with the fields in rmt2, we need to hold onto
			// the pre-converted data, so we can use it later in rm__ porting.
			OldRenderMethodTemplates.Add(index, rmt2);

			string tag_name = CacheContext.TagNames[index];
			PixelShader pixl = NewPixelShaders[tag_name];

			//
			//	rmt2 = GenerateRenderMethodTemplate(pixl, tag_name);
			//

			return rmt2;
		}

		public PixelShader ConvertPixelShader_2(Stream cacheStream, PixelShader pixl, int index)
		{
			string tag_name = CacheContext.TagNames[index];

			//
			//	pixl = GeneratePixlShader(tag_name);
			//

			NewPixelShaders.Add(tag_name, pixl);
			return pixl;
		}
	}
}
