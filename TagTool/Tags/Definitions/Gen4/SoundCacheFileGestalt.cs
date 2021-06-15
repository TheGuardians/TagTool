using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xDC)]
    public class SoundCacheFileGestalt : TagStructure
    {
        public List<SoundGestaltCodecBlock> Codecs;
        public List<SoundGestaltPlaybackBlock> Playbacks;
        public List<SoundGestaltScaleBlock> Scales;
        public List<SoundGestaltImportNamesBlock> ImportNames;
        public List<SoundDistanceParametersStruct> PitchRangeDistances;
        public List<SoundGestaltPitchRangeParametersBlock> PitchRangeParameters;
        public List<SoundGestaltPitchRangesBlock> PitchRanges;
        public List<SoundGestaltPermutationsBlock> Permutations;
        public List<SoundGestaltPermutationLanguagesBlockStruct> PermutationLanguages;
        public List<SoundGestaltCustomPlaybackBlock> CustomPlaybacks;
        public List<SoundLanguageInfoBlock> LanguageDurations;
        public List<SoundGestaltRuntimePermutationBitVectorBlock> RuntimePermutationFlags;
        public byte[] NaiveSampleData;
        public int NoOneListensToMe;
        public int ButNowIMUsedToIt;
        public List<SoundPermutationChunkBlock> Chunks;
        public List<SoundGestaltPromotionsBlock> Promotions;
        public List<SoundGestaltFacialAnimationBlock> FacialAnimations;
        public List<SoundGestaltLayerMarkersBlock> LayerMarkers;
        
        [TagStructure(Size = 0x3)]
        public class SoundGestaltCodecBlock : TagStructure
        {
            public SoundSampleRateEnum SampleRate;
            public SoundEncodingEnum Encoding;
            public SoundCompressionEnum Compression;
            
            public enum SoundSampleRateEnum : sbyte
            {
                _22khz,
                _44khz,
                _32khz
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
        }
        
        [TagStructure(Size = 0x54)]
        public class SoundGestaltPlaybackBlock : TagStructure
        {
            public SoundPlaybackParametersStruct Playback;
            
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
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundGestaltScaleBlock : TagStructure
        {
            public SoundScaleModifiersStruct Scale;
            
            [TagStructure(Size = 0x14)]
            public class SoundScaleModifiersStruct : TagStructure
            {
                public Bounds<float> GainModifier; // dB
                public Bounds<short> PitchModifier; // cents
                public Bounds<float> SkipFractionModifier;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class SoundGestaltImportNamesBlock : TagStructure
        {
            public StringId ImportName;
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
        
        [TagStructure(Size = 0x10)]
        public class SoundGestaltPitchRangeParametersBlock : TagStructure
        {
            public short NaturalPitch; // cents
            public short Pad;
            // the range of pitches that will be represented using this sample.
            public Bounds<short> BendBounds; // cents
            public Bounds<short> MaxGainPitchBounds; // cents
            public Bounds<short> PlaybackPitchBounds; // cents
        }
        
        [TagStructure(Size = 0xC)]
        public class SoundGestaltPitchRangesBlock : TagStructure
        {
            public short Name;
            public short Parameters;
            public short EncodedPermutationData;
            public short FirstRuntimePermutationFlagIndex;
            public int EncodedFirstPermutationAndCounts;
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundGestaltPermutationsBlock : TagStructure
        {
            public short Name;
            public short EncodedSkipFraction;
            public int UncompressedSampleCount;
            public int FirstChunkIndex;
            public short ChunkCount;
            public sbyte EncodedGain; // dB
            public sbyte PermutationInfoIndex;
            public ushort FirstLayerMarkerIndex;
            public ushort LayerMarkerCount;
        }
        
        [TagStructure(Size = 0xD0)]
        public class SoundGestaltPermutationLanguagesBlockStruct : TagStructure
        {
            public int PermutationIndex;
            public int EnglishUncompressedSampleCount;
            public int EnglishFirstChunkIndex;
            public int EnglishChunkCount;
            public int JapaneseUncompressedSampleCount;
            public int JapaneseFirstChunkIndex;
            public int JapaneseChunkCount;
            public int GermanUncompressedSampleCount;
            public int GermanFirstChunkIndex;
            public int GermanChunkCount;
            public int FrenchUncompressedSampleCount;
            public int FrenchFirstChunkIndex;
            public int FrenchChunkCount;
            public int SpanishUncompressedSampleCount;
            public int SpanishFirstChunkIndex;
            public int SpanishChunkCount;
            public int MexicanSpanishUncompressedSampleCount;
            public int MexicanSpanishFirstChunkIndex;
            public int MexicanSpanishChunkCount;
            public int ItalianUncompressedSampleCount;
            public int ItalianFirstChunkIndex;
            public int ItalianChunkCount;
            public int KoreanUncompressedSampleCount;
            public int KoreanFirstChunkIndex;
            public int KoreanChunkCount;
            public int TraditionalChineseUncompressedSampleCount;
            public int TraditionalChineseFirstChunkIndex;
            public int TraditionalChineseChunkCount;
            public int SimplifiedChineseUncompressedSampleCount;
            public int SimplifiedChineseFirstChunkIndex;
            public int SimplifiedChineseChunkCount;
            public int PortugueseUncompressedSampleCount;
            public int PortugueseFirstChunkIndex;
            public int PortugueseChunkCount;
            public int PolishUncompressedSampleCount;
            public int PolishFirstChunkIndex;
            public int PolishChunkCount;
            public int RussianUncompressedSampleCount;
            public int RussianFirstChunkIndex;
            public int RussianChunkCount;
            public int DanishUncompressedSampleCount;
            public int DanishFirstChunkIndex;
            public int DanishChunkCount;
            public int FinnishUncompressedSampleCount;
            public int FinnishFirstChunkIndex;
            public int FinnishChunkCount;
            public int DutchUncompressedSampleCount;
            public int DutchFirstChunkIndex;
            public int DutchChunkCount;
            public int NorwegianUncompressedSampleCount;
            public int NorwegianFirstChunkIndex;
            public int NorwegianChunkCount;
        }
        
        [TagStructure(Size = 0x5C)]
        public class SoundGestaltCustomPlaybackBlock : TagStructure
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
        
        [TagStructure(Size = 0x1)]
        public class SoundGestaltRuntimePermutationBitVectorBlock : TagStructure
        {
            public sbyte RuntimePermutationBitVector;
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundPermutationChunkBlock : TagStructure
        {
            public int FileOffset;
            public int EncodedSizeAndFlags;
            public int CacheIndex;
            public int Xma2SourceBufferSampleStart;
            public int Xma2SourceBufferSampleEnd;
        }
        
        [TagStructure(Size = 0x24)]
        public class SoundGestaltPromotionsBlock : TagStructure
        {
            public SoundPromotionParametersStruct RuntimePromotionStorage;
            
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
        }
        
        [TagStructure(Size = 0x8)]
        public class SoundGestaltFacialAnimationBlock : TagStructure
        {
            public TagResourceReference FacialAnimationResource;
        }
        
        [TagStructure(Size = 0x4)]
        public class SoundGestaltLayerMarkersBlock : TagStructure
        {
            public int SampleOffset;
        }
    }
}
