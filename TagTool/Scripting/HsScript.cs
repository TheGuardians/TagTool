using TagTool.Tags;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Scripting
{
	[TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x34, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    public class HsScript : TagStructure
	{
        [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
        public string ScriptName;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId ScriptNameReach;
        public HsScriptType Type;
        public HsType ReturnType;
        public DatumHandle RootExpressionHandle;
        public List<HsScriptParameter> Parameters;
    }
}
