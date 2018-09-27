using TagTool.Common;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x4)]
    public class ImportName : TagStructure
	{
        public StringId Name;
    }
}