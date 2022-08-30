using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x4C, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x64, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x48, MinVersion = CacheVersion.HaloOnlineED)]
    public class SurvivalModeGlobals : TagStructure
    {
        public uint Unknown;
        [TagField(ValidTags = new [] { "unic" })] public CachedTag SurvivalModeStrings;
        [TagField(ValidTags = new [] { "snd!" })] public CachedTag CountdownSound;
        [TagField(ValidTags = new [] { "snd!" })] public CachedTag RespawnSound;

        public List<SurvivalEvent> SurvivalEvents;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public List<ArmorCustomization> ArmorCustomizations;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint UnknownHO;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint UnknownHO_1;

        [TagStructure(Size = 0x108, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x10C, MinVersion = CacheVersion.HaloOnlineED)]
        public class SurvivalEvent : TagStructure
		{
            public GameEngineEventFlagsDefinition Flags;
            public TypeValue Type;
            [TagField(Flags = Label)]
            public StringId Event;
            public AudienceValue Audience;
            public short DisplayPriority;
            public short SubPriority;
            public TeamValue Team;
            public StringId DisplayString;
            public StringId DisplayMedal; // This is only valid on Flavor Events and will not award a medal for Engine Events

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public uint UnknownHO;

            public float DisplayTime; // seconds
            public RequiredFieldValue RequiredField;
            public RequiredFieldValue ExcludedAudience;
            public GameEngineEventSplitscreenSuppressionEnumDefinition SplitscreenSuppression;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding0;

            public StringId PrimaryString;
            public int PrimaryStringDuration; // seconds
            public StringId PluralDisplayString;
            public float SoundDelayAnnouncerOnly;
            public GameEngineSoundResponseFlagsDefinition SoundFlags;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding1;

            public CachedTag EnglishSound;
            public CachedTag JapaneseSound;
            public CachedTag GermanSound;
            public CachedTag FrenchSound;
            public CachedTag SpanishSound;
            public CachedTag LatinAmericanSpanishSound;
            public CachedTag ItalianSound;
            public CachedTag KoreanSound;
            public CachedTag ChineseTraditionalSound;
            public CachedTag ChineseSimplifiedSound;
            public CachedTag PortugueseSound;
            public CachedTag PolishSound;
            public CachedTag ProbablyRussianSound;

            [Flags]
            public enum GameEngineEventFlagsDefinition : ushort
            {
                QuantityMessage = 1 << 0,
                SuppressText = 1 << 1
            }

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

            [Flags]
            public enum GameEngineSoundResponseFlagsDefinition : ushort
            {
                AnnouncerSound = 1 << 0
            }

            public enum GameEngineEventSplitscreenSuppressionEnumDefinition : short
            {
                None,
                SuppressAudio,
                SuppressAudioIfOverlapping,
                SuppressText,
                SuppressAudioAndText
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