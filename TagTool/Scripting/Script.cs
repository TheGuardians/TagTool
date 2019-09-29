using TagTool.Tags;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Scripting
{
	[TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x34, MinVersion = CacheVersion.Halo3Retail)]
    public class Script : TagStructure
	{
        [TagField(Length = 32)]
        public string ScriptName;
        public ScriptType Type;
        public ScriptValueType ReturnType;
        public DatumIndex RootExpressionHandle;
        public List<ScriptParameter> Parameters;
    }
}
