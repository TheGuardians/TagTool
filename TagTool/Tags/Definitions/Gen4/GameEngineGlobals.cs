using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_engine_globals", Tag = "gegl", Size = 0x2C)]
    public class GameEngineGlobals : TagStructure
    {
        [TagField(ValidTags = new [] { "wezr" })]
        public CachedTag GameEngineSettings;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GameEngineText;
        public List<GameEngineEventBlockStruct> GameEngineEventResponseList;
        
        [TagStructure(Size = 0x58)]
        public class GameEngineEventBlockStruct : TagStructure
        {
            public StringId Name;
            public GameEngineEventAudienceEnum Audience;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // This string can use a bunch of neat tokens for substitution of runtime data (e.g. player names).  See an engineer
            // for more info.
            public StringId DisplayString;
            public GameEngineEventInputEnum RequiredField;
            public GameEngineEventInputEnum ExcludedAudience;
            public GameEngineEventSplitscreenSuppressionEnum SplitscreenSuppression;
            public GameEngineEventFlags Flags;
            // This string can use a bunch of neat tokens for substitution of runtime data (e.g. player names).  See an engineer
            // for more info.
            public StringId PrimaryString;
            public int PrimaryStringDuration; // seconds
            // After we commit to playing this sound, wait this long to actually play it.  Used to control announcer cadence.
            public float SoundDelay;
            public SoundResponseDefinitionStruct DefaultSound;
            // which family does this response live in for announcer-spew-suppression?  e.g. all multi-kills should use the same
            // string_id here.
            public StringId CategoryForPrioritization;
            // higher numbers mean more important
            public int SoundPriority;
            // If a sound from this event has been queued for more than this long, it can't be suppressed anymore.  Higher values
            // cause a shallower announcer queue for these sounds.
            public float PrioritySuppressionAgeMaxSeconds; // seconds
            // e.g. flag taken and flag dropped should use the same id here, while ball taken and ball dropped should use another
            // shared id.
            public StringId CategoryForPairCulling;
            // 0 or 1, used to recognized matched pairs.  If both a 0 and a 1 are in the queue at the same time, and neither has
            // started playing, both are removed.
            public int PairId;
            // Used to keep a sound in the queue so later sounds can priority-suppress or pair-suppress it
            public float DelayBeforeConsideringSoundSeconds; // seconds
            public List<SoundResponseDefinitionBlock> SoundPermutations;
            
            public enum GameEngineEventAudienceEnum : sbyte
            {
                CausePlayer,
                CauseTeam,
                EffectPlayer,
                EffectTeam,
                All
            }
            
            public enum GameEngineEventInputEnum : sbyte
            {
                None,
                CausePlayer,
                CauseTeam,
                EffectPlayer,
                EffectTeam
            }
            
            public enum GameEngineEventSplitscreenSuppressionEnum : sbyte
            {
                None,
                SuppressAudio,
                SuppressAudioIfOverlapping,
                SuppressText,
                SuppressAudioAndText
            }
            
            [Flags]
            public enum GameEngineEventFlags : byte
            {
                // used for respawn ticks and final tick in halo 3
                AlwaysPlaySound = 1 << 0,
                // so you can make multi kill sounds never be suppressed in MP
                AlwaysPlaySoundInMp = 1 << 1
            }
            
            [TagStructure(Size = 0x18)]
            public class SoundResponseDefinitionStruct : TagStructure
            {
                public GameEngineSoundResponseFlags SoundFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Sound;
                // Ignored for the default sound - used for sound permutation block entries only.
                public float Probability;
                
                [Flags]
                public enum GameEngineSoundResponseFlags : byte
                {
                    AnnouncerSound = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class SoundResponseDefinitionBlock : TagStructure
            {
                public SoundResponseDefinitionStruct SoundResponseDefinitionStruct;
            }
        }
    }
}
