using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x28)]
    public class PixelShaderHeader : TagStructure
	{
		public uint Unknown0;
		public uint Unknown4;
		public uint Unknown8;
		public uint UnknownC;
		public uint ShaderDataAddress;
		public uint Unknown14;
		public uint Unknown18;
		public uint Unknown1C;
		public uint Unknown20;
		public uint Unknown24;
	}
}
