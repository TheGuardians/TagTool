using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x4C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
    public class SurvivalModeGlobals : TagStructure
	{
        public uint Unknown;
        public CachedTagInstance InGameStrings;
        public CachedTagInstance TimerSound;
        public CachedTagInstance TimerSoundZero;
        public List<SurvivalEvent> SurvivalEvents;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public List<ArmorCustomization> ArmorCustomizations;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;

        [TagStructure(Size = 0x108, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x10C, MinVersion = CacheVersion.HaloOnline106708)]
        public class SurvivalEvent : TagStructure
		{
            public ushort Flags;
            public TypeValue Type;
            [TagField(Flags = Label)]
            public StringId Event;
            public AudienceValue Audience;
            public short Unknown;
            public short Unknown2;
            public TeamValue Team;
            public StringId DisplayString;
            public StringId DisplayMedal;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown3;
            public float DisplayDuration;
            public RequiredFieldValue RequiredField;
            public RequiredFieldValue ExcludedAudience;
            public RequiredFieldValue RequiredField2;
            public RequiredFieldValue ExcludedAudience2;
            public StringId PrimaryString;
            public int PrimaryStringDuration;
            public StringId PluralDisplayString;
            public float SoundDelayAnnouncerOnly;
            public ushort SoundFlags;
            public short Unknown4;
            public CachedTagInstance EnglishSound;
            public CachedTagInstance JapaneseSound;
            public CachedTagInstance GermanSound;
            public CachedTagInstance FrenchSound;
            public CachedTagInstance SpanishSound;
            public CachedTagInstance LatinAmericanSpanishSound;
            public CachedTagInstance ItalianSound;
            public CachedTagInstance KoreanSound;
            public CachedTagInstance ChineseTraditionalSound;
            public CachedTagInstance ChineseSimplifiedSound;
            public CachedTagInstance PortugueseSound;
            public CachedTagInstance PolishSound;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;

            public enum TypeValue : short
            {
                General,
                Flavor,
                Slayer,
                CaptureTheFlag,
                Oddball,
                Unused,
                KingOfTheHill,
                Vip,
                Juggernaut,
                Territories,
                Assault,
                Infection,
                Survival,
                Unknown
            }

            public enum AudienceValue : short
            {
                CausePlayer,
                CauseTeam,
                EffectPlayer,
                EffectTeam,
                All
            }

            public enum TeamValue : short
            {
                NonePlayerOnly,
                Cause,
                Effect,
                All
            }

            public enum RequiredFieldValue : short
            {
                None,
                CausePlayer,
                CauseTeam,
                EffectPlayer,
                EffectTeam
            }
        }

        [TagStructure(Size = 0x10)]
        public class ArmorCustomization : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId CharacterName;
            public List<Region> Regions;

            [TagStructure(Size = 0x10)]
            public class Region : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId RegionName;
                public List<Permutation> Permutations;

                [TagStructure(Size = 0x1C)]
                public class Permutation : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId Name;
                    public StringId Description;
                    public short Flags;
                    public short Unknown;
                    public StringId AchievementRequired;
                    public List<Variant> Variants;

                    [TagStructure(Size = 0x8)]
                    public class Variant : TagStructure
					{
                        [TagField(Flags = Label)]
                        public StringId Region;
                        public StringId Permutation;
                    }
                }
            }
        }
    }
}