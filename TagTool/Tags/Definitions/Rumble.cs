using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "rumble", Tag = "rmbl", Size = 0x30)]
    public class Rumble : TagStructure
    {
        public DamageResponseRumbleFrequencyStruct LowFrequencyRumble;
        public DamageResponseRumbleFrequencyStruct HighFrequencyRumble;

        [TagStructure(Size = 0x18)]
        public class DamageResponseRumbleFrequencyStruct : TagStructure
        {
            public float Duration; // seconds
            public TagFunction DirtyRumble = new TagFunction { Data = new byte[0] };
        }
    }
}
