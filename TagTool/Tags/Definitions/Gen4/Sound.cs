using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x48)]
    public class Sound : TagStructure
    {
        public SoundEventDefinitionFlags Flags;
        public SoundImportFlags ImportFlags;
        public SoundXsyncFlags XsyncFlags;
        // Sound event name.
        public uint EventName;
        // Optional sound event name for player only.
        public uint PlayerEventName;
        // Fallback sound event if the others don't play - should be an a guaranteed bank.
        public uint FallbackEventName;
        public float MaxRadius;
        // Max duration of this event. Enter manually for now - will auto-fill later.
        public float MaxDuration;
        // Time the event will not retrigger for (global)
        public float DonTPlayTime;
        public int HiddenRuntimeInfoIndex;
        // Reference to the sound bank tag this event is in. Can be empty.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag SoundBank;
        public List<SoundlipSyncInfoBlockStruct> LipsyncInfo;
        public int DeterministicFlagIndex;
        
        [Flags]
        public enum SoundEventDefinitionFlags : uint
        {
            // disable occlusion/obstruction for this sound
            NeverObstruct = 1 << 0,
            UseHugeSoundTransmission = 1 << 1,
            LinkCountToOwnerUnit = 1 << 2,
            DonTUseLipsyncData = 1 << 3,
            InstantSoundPropagation = 1 << 4,
            OptionalPlayerSoundEvent = 1 << 5,
            FallbackSoundEvent = 1 << 6,
            HasLipsyncData = 1 << 7,
            IsDeterministic = 1 << 8,
            IsExternalSource = 1 << 9,
            OverrideExternalSourceEvent = 1 << 10,
            UseDonTPlayTime = 1 << 11,
            DelayStartIfSoundBankNotLoaded = 1 << 12,
            UseFallbackOnlyForDvd = 1 << 13,
            HasSubtitle = 1 << 14,
            // use this if you're getting wacky spatialization (such as scorpion cannon)
            IgnoreNodeTransform = 1 << 15,
            RadioComboVoice = 1 << 16,
            CinematicAllowTailsToPlayOut = 1 << 17
        }
        
        [Flags]
        public enum SoundImportFlags : uint
        {
            DuplicateDirectoryName = 1 << 0,
            CutToBlockSize = 1 << 1,
            UseMarkers = 1 << 2,
            UseLayerMarkers = 1 << 3
        }
        
        [Flags]
        public enum SoundXsyncFlags : uint
        {
            ProcessedLanguageTimes = 1 << 0,
            OptimizedFacialAnimation = 1 << 1
        }
        
        [TagStructure(Size = 0x1C)]
        public class SoundlipSyncInfoBlockStruct : TagStructure
        {
            public int NumberOfUsableFacialAnimations;
            public List<DeterministicspeechEventBlockStruct> DeterministicEventInfo;
            public List<FacialAnimationLanguageBlockStruct> FacialAnimationResourceLanguages;
            
            [TagStructure(Size = 0x10)]
            public class DeterministicspeechEventBlockStruct : TagStructure
            {
                public SpeechEventInfoFlags Flags;
                public uint VoiceFilePath;
                public float EventDuration;
                // Percent this file will be skipped when picked. 0 is always, 0.99 is almost never
                public float SkipFraction;
                
                [Flags]
                public enum SpeechEventInfoFlags : uint
                {
                    CampaignOnly = 1 << 0,
                    ExcludeFromCertainMissions = 1 << 1,
                    ExcludeFromM10 = 1 << 2,
                    ExcludeFromM20 = 1 << 3,
                    ExcludeFromM30 = 1 << 4,
                    ExcludeFromM40 = 1 << 5,
                    ExcludeFromM60 = 1 << 6,
                    ExcludeFromM70 = 1 << 7,
                    ExcludeFromM80 = 1 << 8,
                    ExcludeFromM90 = 1 << 9
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class FacialAnimationLanguageBlockStruct : TagStructure
            {
                public TagResourceReference FacialAnimationResource;
                public SoundLanguageEnum Language;
                
                public enum SoundLanguageEnum : int
                {
                    English,
                    Japanese,
                    German,
                    French,
                    Spanish,
                    MexicanSpanish,
                    Italian,
                    Korean,
                    ChineseTraditional,
                    ChineseSimplified,
                    Portuguese,
                    Polish,
                    Russian,
                    Danish,
                    Finnish,
                    Dutch,
                    Norwegian
                }
            }
        }
    }
}
