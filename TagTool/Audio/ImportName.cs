using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x4)]
    public class ImportName
    {
        public StringId Name;
    }
}