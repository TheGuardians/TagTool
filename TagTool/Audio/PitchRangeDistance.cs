using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class PitchRangeDistance : TagStructure
	{
        public int DontPlayDistance;
        public int AttackDistance;
        public int MinimumDistance;
        public int MaximumDistance;
    }
}