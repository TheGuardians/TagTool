using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x18)]
    public class HsSyntaxNode : TagStructure
	{
        public ushort Identifier;
        public ushort Opcode;
        public HsType ValueType;
        public HsSyntaxNodeFlags Flags;
        public DatumIndex NextExpressionHandle;
        public uint StringAddress;

        [TagField(Length = 4)]
        public byte[] Data;

        public short LineNumber;
        public short Unknown;
    }

    [Flags]
    public enum HsSyntaxNodeFlags : ushort
    {
        Invalid = 0xBABA,

        Primitive = 1 << 0,
        ScriptIndex = 1 << 1,
        GlobalIndex = 1 << 2,
        DoNotGC = 1 << 3,
        ParameterIndex = 1 << 4,

        Group = DoNotGC,
        Expression = Primitive | DoNotGC,
        ScriptReference = ScriptIndex | DoNotGC,
        GlobalsReference = Primitive | GlobalIndex | DoNotGC,
        ParameterReference = Primitive | ParameterIndex | DoNotGC
    }
}