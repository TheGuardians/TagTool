using TagTool.Tags;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Scripting
{
	[TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x34, MinVersion = CacheVersion.Halo3Retail)]
    public class HsScript : TagStructure
	{
        [TagField(Length = 32)]
        public string ScriptName;
        public HsScriptType Type;
        public HsType ReturnType;
        public DatumIndex RootExpressionHandle;
        public List<HsScriptParameter> Parameters;
    }
}
