using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x4)]
    public class ImportName : TagStructure
	{
        public StringId Name;
    }
}