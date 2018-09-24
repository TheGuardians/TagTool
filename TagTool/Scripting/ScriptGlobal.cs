using TagTool.Serialization;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x28)]
    public class ScriptGlobal : TagStructure
	{
        [TagField(Length = 32)]
        public string Name;
        public ScriptValueType Type;
        public short Unknown;
        public uint InitializationExpressionHandle;
    }
}
