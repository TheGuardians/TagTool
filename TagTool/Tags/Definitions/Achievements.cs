using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "achievements", Tag = "achi", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "achievements", Tag = "achi", Size = 0x18, MaxVersion = CacheVersion.HaloOnline106708)]
    public class Achievements : TagStructure
    {
        public List<AchievementInformationBlock> AchievementInformation;

        [TagField(Flags = TagFieldFlags.Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x18)]
        public class AchievementInformationBlock : TagStructure
        {
            public int Unknown;
            public int Unknown2;
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId LevelName;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
        }
    }
}
