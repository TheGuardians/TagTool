using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x34)]
    public class Script : TagStructure
	{
        [TagField(Length = 32)]
        public string ScriptName;
        public ScriptType Type;
        public ScriptValueType ReturnType;
        public uint RootExpressionHandle;
        public List<ScriptParameter> Parameters;
    }
}
