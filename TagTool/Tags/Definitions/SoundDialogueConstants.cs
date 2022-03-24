using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_dialogue_constants", Tag = "spk!", Size = 0x28, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundDialogueConstants : TagStructure
	{
        public float AlmostNever;
        public float Rarely;
        public float Somewhat;
        public float Often;

        [TagField(Length = 24, Flags = Padding)]
        public byte[] Pad0;
    }
}
