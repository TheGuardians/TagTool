using TagTool.Tags;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x18)]
    public class ScriptExpression : TagStructure
	{
        public ushort Salt;
        public ushort Opcode;
        public ScriptValueType ValueType; // 30 max length
        public ScriptExpressionType ExpressionType; // 18 max length
        public uint NextExpressionHandle;
        public uint StringAddress;

        [TagField(Length = 4)]
        public byte[] Data;

        public short LineNumber;
        public short Unknown;
    }

    public enum ScriptExpressionType : short
    {
        Group = 8,
        Expression = 9,
        ScriptReference = 10,
        GlobalsReference = 13,
        ParameterReference = 29,
        Invalid = 0x3ABB // ushort 0xBABA
    }
}