using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x1C)]
    public class ShaderDebugHeader : TagStructure
	{
        public uint Magic;
        public uint StructureSize;
        public uint ShaderDataSize;
        public uint UpdbPointerOffset;
        public uint Unknown;
        public uint UnknownOffset;
        public uint CodeHeaderOffset;
    }
}
