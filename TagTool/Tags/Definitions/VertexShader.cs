using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vertex_shader", Tag = "vtsh", Size = 0x20, MinVersion = CacheVersion.Halo3Retail)]
	public class VertexShader
	{
		public uint Unknown;
		public List<DrawModeList> DrawModeLists;
		public uint Unknown3;
		public List<ShaderData> Shaders;

		[TagStructure(Size = 0xC)]
		public class DrawModeList
		{
			public List<ShaderDrawMode> DrawModes;
		}
	}
}