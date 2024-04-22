using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_effect_collection", Tag = "sfx+", Size = 0xC)]
    public class SoundEffectCollection : TagStructure
    {
        public List<PlatformSoundPlaybackBlockStruct> SoundEffects;
        
        [TagStructure(Size = 0x60)]
        public class PlatformSoundPlaybackBlockStruct : TagStructure
        {
            public StringId Name;
            public PlatformSoundPlaybackStruct Playback;
            
            [TagStructure(Size = 0x5C)]
            public class PlatformSoundPlaybackStruct : TagStructure
            {
                public PlatformSoundEffectFlags Flags;
                [TagField(ValidTags = new [] { "srad" })]
                public CachedTag RadioEffect;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag StartEvent;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag EndEvent;
                // Must clear the effect without any transition - used for exiting levels, etc
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag ImmediateStopEvent;
                public List<PlatformSoundPlaybackLowpassBlockStruct> LowpassEffect;
                public List<PlatformSoundPlaybackComponentBlockStruct> SoundComponents;
                
                [Flags]
                public enum PlatformSoundEffectFlags : uint
                {
                    TurnOffInSplitscreen = 1 << 0,
                    OnlyTurnOnInFirstPerson = 1 << 1
                }
                
                [TagStructure(Size = 0x10)]
                public class PlatformSoundPlaybackLowpassBlockStruct : TagStructure
                {
                    public float Attack; // seconds
                    public float Release; // seconds
                    public GlobalSoundLowpassBlock Settings;
                    
                    [TagStructure(Size = 0x8)]
                    public class GlobalSoundLowpassBlock : TagStructure
                    {
                        public float CutoffFrequency; // Hz
                        public float OutputGain; // dB
                    }
                }
                
                [TagStructure(Size = 0x18)]
                public class PlatformSoundPlaybackComponentBlockStruct : TagStructure
                {
                    [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
                    public CachedTag Sound;
                    // additional attenuation to sound
                    public float Gain; // dB
                    public PlatformSoundPlaybackComponentFlags Flags;
                    
                    [Flags]
                    public enum PlatformSoundPlaybackComponentFlags : uint
                    {
                        DonTPlayAtStart = 1 << 0,
                        PlayOnStop = 1 << 1,
                        PlayAlternate = 1 << 2,
                        SyncWithOriginLoopingSound = 1 << 3
                    }
                }
            }
        }
    }
}
