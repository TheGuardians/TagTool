using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_classes", Tag = "sncl", Size = 0xC)]
    public class SoundClasses : TagStructure
    {
        public List<SoundClassDefinition> Classes;
        
        [TagStructure(Size = 0x5C)]
        public class SoundClassDefinition : TagStructure
        {
            public short MaxSoundsPerTag116; // maximum number of sounds playing per individual sound tag
            public short MaxSoundsPerObject116; // maximum number of sounds of this type playing on an object
            public int PreemptionTime; // ms
            public InternalFlagsValue InternalFlags;
            public FlagsValue Flags;
            public short Priority;
            public CacheMissModeValue CacheMissMode;
            public float ReverbGain; // dB
            public float OverrideSpeakerGain; // dB
            public Bounds<float> DistanceBounds;
            public Bounds<float> GainBounds; // dB
            public float CutsceneDucking; // dB
            public float CutsceneDuckingFadeInTime; // seconds
            public float CutsceneDuckingSustainTime; // seconds
            public float CutsceneDuckingFadeOutTime; // seconds
            public float ScriptedDialogDucking; // dB
            public float ScriptedDialogDuckingFadeInTime; // seconds
            public float ScriptedDialogDuckingSustainTime; // seconds
            public float ScriptedDialogDuckingFadeOutTime; // seconds
            public float DopplerFactor;
            public StereoPlaybackTypeValue StereoPlaybackType;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding1;
            public float TransmissionMultiplier;
            public float ObstructionMaxBend;
            public float OcclusionMaxBend;
            
            [Flags]
            public enum InternalFlagsValue : ushort
            {
                Valid = 1 << 0,
                IsSpeech = 1 << 1,
                Scripted = 1 << 2,
                StopsWithObject = 1 << 3,
                Unused = 1 << 4,
                ValidDopplerFactor = 1 << 5,
                ValidObstructionFactor = 1 << 6,
                Multilingual = 1 << 7
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                PlaysDuringPause = 1 << 0,
                DryStereoMix = 1 << 1,
                NoObjectObstruction = 1 << 2,
                UseCenterSpeakerUnspatialized = 1 << 3,
                SendMonoToLfe = 1 << 4,
                Deterministic = 1 << 5,
                UseHugeTransmission = 1 << 6,
                AlwaysUseSpeakers = 1 << 7,
                DonTStripFromMainMenu = 1 << 8,
                IgnoreStereoHeadroom = 1 << 9,
                LoopFadeOutIsLinear = 1 << 10,
                StopWhenObjectDies = 1 << 11,
                AllowCacheFileEditing = 1 << 12
            }
            
            public enum CacheMissModeValue : short
            {
                Discard,
                Postpone
            }
            
            public enum StereoPlaybackTypeValue : sbyte
            {
                FirstPerson,
                Ambient
            }
        }
    }
}

