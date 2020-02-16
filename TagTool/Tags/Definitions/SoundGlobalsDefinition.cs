using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReachMCC0824)]
    public class SoundGlobalsDefinition : TagStructure
	{
        [TagField(ValidTags = new[] { "sncl" })]
        public CachedTag SoundClasses;

        [TagField(ValidTags = new[] { "sfx+" })]
        public CachedTag SoundEffects;

        [TagField(ValidTags = new[] { "snmm" }, MinVersion = CacheVersion.HaloReachMCC0824)]
        public CachedTag SoundMastering;

        [TagField(ValidTags = new[] { "snmx" })]
        public CachedTag SoundMix;

        [TagField(ValidTags = new[] { "spk!" })]
        public CachedTag SoundCombatDialogueConstants;

        [TagField(ValidTags = new[] { "sgp!" })]
        public CachedTag SoundGlobalPropagation;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public FireTeamSound FireTeamSounds;

        [TagField(ValidTags = new[] { "sus!" }, Gen = CacheGeneration.HaloOnline)]
        public CachedTag GfxUiSounds;

        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class FireTeamSound : TagStructure
        {
            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag Sound;
        }
    }
}