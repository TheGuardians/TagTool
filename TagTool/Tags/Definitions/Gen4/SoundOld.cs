using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_old", Tag = "sndo", Size = 0xE0)]
    public class SoundOld : TagStructure
    {
        public SoundDefinitionFlags Flags;
        public SoundImportFlags ImportFlags;
        public SoundXsyncFlags XsyncFlags;
        public SoundClassEnum Class;
        public SoundSampleRateEnum SampleRate;
        public sbyte OverrideXmaCompression; // [1-100]
        public SoundImportTypeEnum ImportType;
        public SoundPlaybackParametersStruct Playback;
        public SoundScaleModifiersStruct Scale;
        public float SubPriority;
        public SoundEncodingEnum Encoding;
        public SoundCompressionEnum Compression;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public SoundPromotionParametersStruct Promotion;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // pitch ranges allow multiple samples to represent the same sound at different pitches
        public List<SoundPitchRangeBlock> PitchRanges;
        public List<SoundPlatformSoundPlaybackBlock> PlatformParameters;
        public TagResourceReference SoundDataResource;
        public List<SoundExtraInfoBlockStruct> ExtraInfo;
        public List<SoundLanguageInfoBlock> LanguageInfo;
        
        [Flags]
        public enum SoundDefinitionFlags : uint
        {
            FitToAdpcmBlocksize = 1 << 0,
            // always play as 3d sound, even in first person
            AlwaysSpatialize = 1 << 1,
            // disable occlusion/obstruction for this sound
            NeverObstruct = 1 << 2,
            InternalDonTTouch = 1 << 3,
            FacialAnimationDataStripped = 1 << 4,
            UseHugeSoundTransmission = 1 << 5,
            LinkCountToOwnerUnit = 1 << 6,
            PitchRangeIsLanguage = 1 << 7,
            DonTUseSoundClassSpeakerFlag = 1 << 8,
            DonTUseLipsyncData = 1 << 9,
            InstantSoundPropagation = 1 << 10,
            FakeSpatializationWithDistance = 1 << 11,
            // picks the first permutation randomly
            PlayPermutationsInOrder = 1 << 12
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
        
        public enum SoundClassEnum : sbyte
        {
            ProjectileImpact,
            ProjectileDetonation,
            ProjectileFlyby,
            ProjectileDetonationLod,
            WeaponFire,
            WeaponReady,
            WeaponReload,
            WeaponEmpty,
            WeaponCharge,
            WeaponOverheat,
            WeaponIdle,
            WeaponMelee,
            WeaponAnimation,
            ObjectImpacts,
            ParticleImpacts,
            WeaponFireLod,
            WaterTransitions,
            LowpassEffects,
            UnitFootsteps,
            UnitDialog,
            UnitAnimation,
            UnitUnused,
            VehicleCollision,
            VehicleEngine,
            VehicleAnimation,
            VehicleEngineLod,
            DeviceDoor,
            DeviceUnused0,
            DeviceMachinery,
            DeviceStationary,
            DeviceUnused1,
            DeviceUnused2,
            Music,
            AmbientNature,
            AmbientMachinery,
            AmbientStationary,
            HugeAss,
            ObjectLooping,
            CinematicMusic,
            UnknownUnused0,
            UnknownUnused1,
            AmbientFlock,
            NoPad,
            NoPadStationary,
            EquipmentEffect,
            MissionDialog,
            CinematicDialog,
            ScriptedCinematicFoley,
            GameEvent,
            Ui,
            Test,
            MultiplayerDialog,
            AmbientNatureDetails,
            AmbientMachineryDetails,
            InsideSurroundTail,
            OutsideSurroundTail,
            VehicleDetonation,
            AmbientDetonation,
            FirstPersonInside,
            FirstPersonOutside,
            FirstPersonAnywhere,
            SpaceProjectileDetonation,
            SpaceProjectileFlyby,
            SpaceVehicleEngine,
            SpaceWeaponFire,
            PlayerVoiceTeam,
            PlayerVoiceProxy,
            ProjectileImpactPostpone,
            UnitFootstepsPostpone,
            WeaponReadyThirdPerson,
            UiMusic
        }
        
        public enum SoundSampleRateEnum : sbyte
        {
            _22khz,
            _44khz,
            _32khz
        }
        
        public enum SoundImportTypeEnum : sbyte
        {
            Unknown,
            SingleShot,
            SingleLayer,
            MultiLayer
        }
        
        public enum SoundEncodingEnum : sbyte
        {
            Mono,
            Stereo,
            Quad,
            _51,
            Codec
        }
        
        public enum SoundCompressionEnum : sbyte
        {
            None,
            XboxAdpcm,
            ImaAdpcm,
            None1,
            Wma,
            None2,
            Xma,
            XmaV20
        }
        
        [TagStructure(Size = 0x54)]
        public class SoundPlaybackParametersStruct : TagStructure
        {
            public int InternalFlags;
            // fraction of requests to play this sound that will be ignored (0 means always play.)
            public float SkipFraction;
            public float MaximumBendPerSecond; // cents
            public SoundDistanceParametersStruct DistanceParameters;
            // sound's random gain will start here
            public float GainBase; // dB
            // sound's gain will be randomly modulated within this range
            public float GainVariance; // dB
            // the sound's pitch will be modulated randomly within this range.
            public Bounds<short> RandomPitchBounds; // cents
            // within the cone defined by this angle and the sound's direction, the sound plays with a gain of 1.0.
            public Angle InnerConeAngle; // degrees
            // outside the cone defined by this angle and the sound's direction, the sound plays with a gain of OUTER CONE GAIN.
            // (0 means the sound does not attenuate with direction.)
            public Angle OuterConeAngle; // degrees
            // the gain to use when the sound is directed away from the listener
            public float OuterConeGain; // dB
            public SoundOverrideLocationFlags Flags;
            public Angle Azimuth;
            public float PositionalGain; // dB
            public float FirstPersonGain; // dB
            
            [Flags]
            public enum SoundOverrideLocationFlags : uint
            {
                OverrideAzimuth = 1 << 0,
                Override3dGain = 1 << 1,
                OverrideSpeakerGain = 1 << 2
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
        
        [TagStructure(Size = 0x14)]
        public class SoundScaleModifiersStruct : TagStructure
        {
            public Bounds<float> GainModifier; // dB
            public Bounds<short> PitchModifier; // cents
            public Bounds<float> SkipFractionModifier;
        }
        
        [TagStructure(Size = 0x24)]
        public class SoundPromotionParametersStruct : TagStructure
        {
            public List<SoundPromotionRuleBlock> PromotionRules;
            public List<SoundPromotionRuntimeTimerBlock> RuntimeTimers;
            public int RuntimeActivePromotionIndex;
            public int RuntimeLastPromotionTime;
            public int RuntimeSuppressionTimeout;
            
            [TagStructure(Size = 0x10)]
            public class SoundPromotionRuleBlock : TagStructure
            {
                public short PitchRange;
                public short MaximumPlayingCount;
                // time from when first permutation plays to when another sound from an equal or lower promotion can play
                public float SuppressionTime; // seconds
                public int RuntimeRolloverTime;
                public int ImpulsePromotionTime;
            }
            
            [TagStructure(Size = 0x4)]
            public class SoundPromotionRuntimeTimerBlock : TagStructure
            {
                public int TimerStorage;
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class SoundPitchRangeBlock : TagStructure
        {
            // the name of the imported pitch range directory
            public StringId Name;
            // the apparent pitch when these samples are played at their recorded pitch.
            public short NaturalPitch; // cents
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the range of pitches that will be represented using this sample.
            public Bounds<short> BendBounds; // cents
            // the range of pitches that map to full gain.
            public Bounds<short> FullVolumeBounds; // cents
            // the actual pitch will be clamped to this
            public Bounds<short> PlaybackBendBounds; // cents
            public SoundDistanceParametersStruct DistanceParameters;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public sbyte RuntimeUsablePermutationCount;
            public SoundPitchRangeInternalXsyncFlags XsyncFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            // permutations represent equivalent variations of this sound.
            public List<SoundPermutationsBlock> Permutations;
            
            [Flags]
            public enum SoundPitchRangeInternalXsyncFlags : byte
            {
                ProcessedLanguagePermutations = 1 << 0
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
            
            [TagStructure(Size = 0x24)]
            public class SoundPermutationsBlock : TagStructure
            {
                // name of the file from which this sample was imported
                public StringId Name;
                // fraction of requests to play this permutation that are ignored (a different permutation is selected.)
                public float SkipFraction;
                // additional attenuation when played
                public float Gain; // dB
                public short RawInfo;
                public short PlayFractionType;
                // first and last mission ids this permutation can play in (zero values are ignored)
                public Bounds<short> MissionRange;
                public SoundPermutationExternalFlags PermutationFlags;
                public SoundPermutationFlags Flags;
                public List<SoundPermutationLanguagesBlockStruct> Languages;
                
                [Flags]
                public enum SoundPermutationExternalFlags : ushort
                {
                    DonTPlayInDvdBuild = 1 << 0
                }
                
                [Flags]
                public enum SoundPermutationFlags : ushort
                {
                }
                
                [TagStructure(Size = 0x10)]
                public class SoundPermutationLanguagesBlockStruct : TagStructure
                {
                    public int UncompressedSampleCount;
                    public List<SoundPermutationChunkBlock> Chunks;
                    
                    [TagStructure(Size = 0x14)]
                    public class SoundPermutationChunkBlock : TagStructure
                    {
                        public int FileOffset;
                        public int EncodedSizeAndFlags;
                        public int CacheIndex;
                        public int Xma2SourceBufferSampleStart;
                        public int Xma2SourceBufferSampleEnd;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class SoundPlatformSoundPlaybackBlock : TagStructure
        {
            public PlatformSoundPlaybackStruct PlaybackDefinition;
            
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
        
        [TagStructure(Size = 0x18)]
        public class SoundExtraInfoBlockStruct : TagStructure
        {
            public List<SoundDefinitionLanguagePermutationInfoBlock> LanguagePermutationInfo;
            public List<FacialAnimationLanguageBlockStruct> FacialAnimationResourceLanguages;
            
            [TagStructure(Size = 0xC)]
            public class SoundDefinitionLanguagePermutationInfoBlock : TagStructure
            {
                public List<SoundPermutationRawInfoBlock> RawInfoBlock;
                
                [TagStructure(Size = 0x4C)]
                public class SoundPermutationRawInfoBlock : TagStructure
                {
                    public StringId SkipFractionName;
                    // sampled sound data
                    public byte[] Samples;
                    public List<SoundPermutationMarkerBlock> Markers;
                    public List<SoundPermutationMarkerBlock> LayerMarkers;
                    public List<SoundXma2SeekTableBlock> Xma2SeekTable;
                    public SoundCompressionEnum Compression;
                    public SoundLanguageEnum Language;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int SampleCount;
                    public int ResourceSampleOffset;
                    public int ResourceSampleSize;
                    
                    public enum SoundCompressionEnum : short
                    {
                        None,
                        XboxAdpcm,
                        ImaAdpcm,
                        None1,
                        Wma,
                        None2,
                        Xma,
                        XmaV20
                    }
                    
                    public enum SoundLanguageEnum : sbyte
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
                    
                    [TagStructure(Size = 0xC)]
                    public class SoundPermutationMarkerBlock : TagStructure
                    {
                        public int MarkerId;
                        public StringId Name;
                        public int SampleOffset;
                    }
                    
                    [TagStructure(Size = 0x18)]
                    public class SoundXma2SeekTableBlock : TagStructure
                    {
                        public int BlockRelativeSampleStart;
                        public int BlockRelativeSampleEnd;
                        public int StartingSampleIndex;
                        public int EndingSampleIndex;
                        public int StartingXma2Offset;
                        public int EndingXma2Offset;
                    }
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
        
        [TagStructure(Size = 0x1C)]
        public class SoundLanguageInfoBlock : TagStructure
        {
            public SoundLanguageEnum Language;
            public List<SoundPermutationLanguageInfo> PermutationDurations;
            public List<SoundPitchRangeLanguageInfo> PitchRangeDurations;
            
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
            
            [TagStructure(Size = 0x2)]
            public class SoundPermutationLanguageInfo : TagStructure
            {
                public short DurationInHsTicks;
            }
            
            [TagStructure(Size = 0x4)]
            public class SoundPitchRangeLanguageInfo : TagStructure
            {
                public short FirstPermutationLanguageIndex;
                public short PermutationCount;
            }
        }
    }
}
