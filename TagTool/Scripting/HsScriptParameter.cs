using TagTool.Tags;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x24)]
    public class HsScriptParameter : TagStructure
	{
        [TagField(Length = 32)]
        public string Name;
        public HsType Type;
        public short Unknown;
    }
}
