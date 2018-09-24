using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class RuntimePermutationFlag : TagStructure
	{
        public sbyte Unknown;
    }
}