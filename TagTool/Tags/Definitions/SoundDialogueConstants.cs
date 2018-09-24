using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_dialogue_constants", Tag = "spk!", Size = 0x28, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_dialogue_constants", Tag = "spk!", Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundDialogueConstants : TagStructure
	{
        public float AlmostNever;
        public float Rarely;
        public float Somewhat;
        public float Often;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}