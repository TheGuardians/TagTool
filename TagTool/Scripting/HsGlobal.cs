using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x28)]
    public class HsGlobal : TagStructure
	{
        [TagField(Length = 32)]
        public string Name;
        public HsType Type;
        public short Unknown;
        public DatumHandle InitializationExpressionHandle;
    }
}
