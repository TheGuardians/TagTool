using TagTool.Serialization;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x24)]
    public class ScriptParameter
    {
        [TagField(Length = 32)]
        public string Name;
        public ScriptValueType Type;
        public short Unknown;
    }
}
