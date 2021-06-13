using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_classes", Tag = "sncl", Size = 0xC)]
    public class SoundClassesDefinition : TagStructure
    {
        public List<SoundClassBlockStruct> SoundClasses;
        
        [TagStructure(Size = 0xD4)]
        public class SoundClassBlockStruct : TagStructure
        {
            // maximum number of sounds playing per individual sound tag
            public short MaxSoundsPerTag116;
            // maximum number of sounds per individual sound tag playing on an object
            public short MaxSoundsPerObjectPerTag116;
            // maximum number of sounds playing of this class. zero means ignore.
            public short MaxSoundsPerClass016;
            // maximum number of sounds of this class playing on an object. zero means ignore.
            public short MaxSoundsPerObjectPerClass016;
            // replaces other instances after this many milliseconds
            public int PreemptionTime; // ms
            public SoundClassInternalFlags InternalFlags;
            public SoundClassExternalFlags Flags;
            // higher means more important
            public short Priority;
            public SoundClassCacheMissModeDefinition CacheMissMode;
            public SoundClassAcousticsStringDefinition BindToAcoustics;
            public SoundClassSuppressSpatializationStringDefintion SuppressSpatialization;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // how much reverb applies to this sound class
            public float AirReverbGain; // dB
            // how much goes to direct path (dry)
            public float AirDirectPathGain; // dB
            public float AirBaseObstruction;
            public float AirBaseOcclusion;
            // how much reverb applies to this sound class
            public float UnderwaterReverbGain; // dB
            // how much goes to direct path (dry)
            public float UnderwaterDirectPathGain; // dB
            public float UnderwaterBaseObstruction;
            public float UnderwaterBaseOcclusion;
            public float OverrideSpeakerGain; // dB
            public SoundDistanceParametersStruct DistanceParameters;
            public Bounds<float> GainBounds; // dB~
            // sets the lowpass wet mix when an equiment is active
            public float EquipmentLowpass; // wetmix
            // sets the lowpass wet mix when an environment forced lowpass is active
            public float EnvironmentForcedLowpass; // wetmix
            // sets the lowpass wet mix when a lowpass effect is active
            public float EffectLowpass; // wetmix
            public float CutsceneDucking; // dB
            public float CutsceneDuckingFadeInTime; // seconds
            // how long this lasts after the cutscene ends
            public float CutsceneDuckingSustainTime; // seconds
            public float CutsceneDuckingFadeOutTime; // seconds
            public float ScriptedDialogDucking; // dB
            public float ScriptedDialogDuckingFadeInTime; // seconds
            // how long this lasts after the scripted dialog ends
            public float ScriptedDialogDuckingSustainTime; // seconds
            public float ScriptedDialogDuckingFadeOutTime; // seconds
            public float EquipmentChannelDucking; // dB
            public float EquipmentChannelDuckingFadeInTime; // seconds
            // how long this lasts after the equipment is turned off
            public float EquipmentChannelDuckingSustainTime; // seconds
            public float EquipmentChannelDuckingFadeOutTime; // seconds
            public float BetweenRoundsDucking; // dB
            public float BetweenRoundsDuckingFadeInTime; // seconds
            // how long this lasts after we get back in the game
            public float BetweenRoundsDuckingSustainTime; // seconds
            public float BetweenRoundsDuckingFadeOutTime; // seconds
            public float DopplerFactor;
            public SoundClassStereoPlaybackDefinition StereoPlaybackType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float TransmissionMultiplier;
            // default is 0.5 seconds
            public float TransmissionInterpolationTime; // seconds
            public int XmaCompressionLevel;
            // When send (mono) to lfe is set, this is how much additional gain to apply
            public float SendToLfeGain; // dB
            // setting this forces sounds of this class to be delayed while the facial animation resource loads.
            public int MinimumFacialAnimationDelay; // msecs
            // setting this allows sounds of this class to be delayed while the facial animation resource loads.
            public int MaximumFacialAnimationDelay; // msecs
            // setting this makes sounds blends in facial animation (will cut off at maximum facial animation delay).
            public int MaximumFacialAnimationBlend; // msecs
            
            [Flags]
            public enum SoundClassInternalFlags : ushort
            {
                Valid = 1 << 0,
                IsSpeech = 1 << 1,
                Scripted = 1 << 2,
                StopsWithObject = 1 << 3,
                ValidXmaCompressionLevel = 1 << 4,
                ValidDopplerFactor = 1 << 5,
                ValidObstructionFactor = 1 << 6,
                Multilingual = 1 << 7,
                DonTStripLanguages = 1 << 8,
                ValidUnderwaterPropagation = 1 << 9,
                ValidSuppressSpatialization = 1 << 10
            }
            
            [Flags]
            public enum SoundClassExternalFlags : ushort
            {
                PlaysDuringPause = 1 << 0,
                BypassDefaultDspEffects = 1 << 1,
                NoObjectObstruction = 1 << 2,
                UseCenterSpeakerUnspatialized = 1 << 3,
                Send = 1 << 4,
                Deterministic = 1 << 5,
                UseHugeTransmission = 1 << 6,
                AlwaysUseSpeakers = 1 << 7,
                DonTStripFromMainMenu = 1 << 8,
                IgnoreStereoHeadroom = 1 << 9,
                LoopFadeOutIsLinear = 1 << 10,
                StopWhenObjectDies = 1 << 11,
                DonTFadeOnGameOver = 1 << 12,
                DonTPromotePriorityByProximity = 1 << 13
            }
            
            public enum SoundClassCacheMissModeDefinition : sbyte
            {
                Discard,
                Postpone
            }
            
            [Flags]
            public enum SoundClassAcousticsStringDefinition : byte
            {
                Outside = 1 << 0,
                Inside = 1 << 1
            }
            
            [Flags]
            public enum SoundClassSuppressSpatializationStringDefintion : byte
            {
                FirstPerson = 1 << 0,
                ThirdPerson = 1 << 1
            }
            
            public enum SoundClassStereoPlaybackDefinition : sbyte
            {
                FirstPerson,
                Ambient
            }
            
            [TagStructure(Size = 0x20)]
            public class SoundDistanceParametersStruct : TagStructure
            {
                // don't obstruct below this distance
                public float DonTObstructDistance; // world units
                // don't play below this distance
                public float DonTPlayDistance; // world units
                // start playing at full volume at this distance
                public float AttackDistance; // world units
                // start attenuating at this distance
                public float MinimumDistance; // world units
                // set attenuation to sustain db at this distance
                public float SustainBeginDistance; // world units
                // continue attenuating to silence at this distance
                public float SustainEndDistance; // world units
                // the distance beyond which this sound is no longer audible
                public float MaximumDistance; // world units
                // the amount of attenuation between sustain begin and end
                public float SustainDb; // dB
            }
        }
    }
}
