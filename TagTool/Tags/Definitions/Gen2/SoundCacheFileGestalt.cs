using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0x58)]
    public class SoundCacheFileGestalt : TagStructure
    {
        public List<SoundGestaltPlaybackBlock> Playbacks;
        public List<SoundGestaltScaleBlock> Scales;
        public List<SoundGestaltImportNamesBlock> ImportNames;
        public List<SoundGestaltPitchRangeParametersBlock> PitchRangeParameters;
        public List<SoundGestaltPitchRangesBlock> PitchRanges;
        public List<SoundGestaltPermutationsBlock> Permutations;
        public List<SoundGestaltCustomPlaybackBlock> CustomPlaybacks;
        public List<SoundGestaltRuntimePermutationBitVectorBlock> RuntimePermutationFlags;
        public List<SoundPermutationChunkBlock> Chunks;
        public List<SoundGestaltPromotionsBlock> Promotions;
        public List<SoundGestaltExtraInfoBlock> ExtraInfos;
        
        [TagStructure(Size = 0x38)]
        public class SoundGestaltPlaybackBlock : TagStructure
        {
            public SoundPlaybackParametersStructBlock Unknown;
            
            [TagStructure(Size = 0x38)]
            public class SoundPlaybackParametersStructBlock : TagStructure
            {
                /// <summary>
                /// the distance below which this sound no longer gets louder
                /// </summary>
                public float MinimumDistance; // world units
                /// <summary>
                /// the distance beyond which this sound is no longer audible
                /// </summary>
                public float MaximumDistance; // world units
                /// <summary>
                /// fraction of requests to play this sound that will be ignored (0 means always play.)
                /// </summary>
                public float SkipFraction;
                public float MaximumBendPerSecond; // cents
                /// <summary>
                /// these settings control random variation of volume and pitch.
                ///  the second parameter gets clipped to the first.
                /// </summary>
                /// <summary>
                /// sound's random gain will start here
                /// </summary>
                public float GainBase; // dB
                /// <summary>
                /// sound's gain will be randomly modulated within this range
                /// </summary>
                public float GainVariance; // dB
                /// <summary>
                /// the sound's pitch will be modulated randomly within this range.
                /// </summary>
                public Bounds<short> RandomPitchBounds; // cents
                /// <summary>
                /// these settings allow sounds to be directional, fading as they turn away from the listener
                /// </summary>
                /// <summary>
                /// within the cone defined by this angle and the sound's direction, the sound plays with a gain of 1.0.
                /// </summary>
                public Angle InnerConeAngle; // degrees
                /// <summary>
                /// outside the cone defined by this angle and the sound's direction, the sound plays with a gain of OUTER CONE GAIN. (0
                /// means the sound does not attenuate with direction.)
                /// </summary>
                public Angle OuterConeAngle; // degrees
                /// <summary>
                /// the gain to use when the sound is directed away from the listener
                /// </summary>
                public float OuterConeGain; // dB
                /// <summary>
                /// NOTE: this will only apply when the sound is started via script
                /// azimuth:
                ///     0 => front
                ///     90 => left
                ///     180 => back
                ///   
                /// 270 => right
                /// 
                /// </summary>
                public FlagsValue Flags;
                public Angle Azimuth;
                public float PositionalGain; // dB
                public float FirstPersonGain; // dB
                
                [Flags]
                public enum FlagsValue : uint
                {
                    OverrideAzimuth = 1 << 0,
                    Override3dGain = 1 << 1,
                    OverrideSpeakerGain = 1 << 2
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundGestaltScaleBlock : TagStructure
        {
            public SoundScaleModifiersStructBlock Unknown;
            
            [TagStructure(Size = 0x14)]
            public class SoundScaleModifiersStructBlock : TagStructure
            {
                /// <summary>
                /// as the sound's input scale changes from zero to one, these modifiers move between the two values specified here. the
                /// sound will play using the current scale modifier multiplied by the values specified above. (0 values are ignored.)
                /// </summary>
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
        
        [TagStructure(Size = 0xA)]
        public class SoundGestaltPitchRangeParametersBlock : TagStructure
        {
            public short NaturalPitch; // cents
            /// <summary>
            /// the range of pitches that will be represented using this sample.
            /// </summary>
            public Bounds<short> BendBounds; // cents
            public Bounds<short> MaxGainPitchBounds; // cents
        }
        
        [TagStructure(Size = 0xC)]
        public class SoundGestaltPitchRangesBlock : TagStructure
        {
            public short Name;
            public short Parameters;
            public short EncodedPermutationData;
            public short FirstRuntimePermutationFlagIndex;
            public short FirstPermutation;
            public short PermutationCount;
        }
        
        [TagStructure(Size = 0x10)]
        public class SoundGestaltPermutationsBlock : TagStructure
        {
            public short Name;
            public short EncodedSkipFraction;
            public sbyte EncodedGain; // dB
            public sbyte PermutationInfoIndex;
            public short LanguageNeutralTime; // ms
            public int SampleSize;
            public short FirstChunk;
            public short ChunkCount;
        }
        
        [TagStructure(Size = 0x34)]
        public class SoundGestaltCustomPlaybackBlock : TagStructure
        {
            public SimplePlatformSoundPlaybackStructBlock PlaybackDefinition;
            
            [TagStructure(Size = 0x34)]
            public class SimplePlatformSoundPlaybackStructBlock : TagStructure
            {
                public List<PlatformSoundOverrideMixbinsBlock> Unknown;
                public FlagsValue Flags;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PlatformSoundFilterBlock> Filter;
                public List<PlatformSoundPitchLfoBlock> PitchLfo;
                public List<PlatformSoundFilterLfoBlock> FilterLfo;
                public List<SoundEffectPlaybackBlock> SoundEffect;
                
                [TagStructure(Size = 0x8)]
                public class PlatformSoundOverrideMixbinsBlock : TagStructure
                {
                    public MixbinValue Mixbin;
                    public float Gain; // dB
                    
                    public enum MixbinValue : int
                    {
                        FrontLeft,
                        FrontRight,
                        BackLeft,
                        BackRight,
                        Center,
                        LowFrequency,
                        Reverb,
                        _3dFrontLeft,
                        _3dFrontRight,
                        _3dBackLeft,
                        _3dBackRight,
                        DefaultLeftSpeakers,
                        DefaultRightSpeakers
                    }
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Use3dRadioHack = 1 << 0
                }
                
                [TagStructure(Size = 0x48)]
                public class PlatformSoundFilterBlock : TagStructure
                {
                    /// <summary>
                    /// DLS2 filtering:
                    ///     resonance gain range: [0, 22.5] dB
                    /// 
                    /// parametric EQ:
                    ///     gain range: [-64, 14] dB
                    /// 
                    /// for mono sounds:
                    ///    
                    /// the left filter controls the DLS 2 parameters
                    ///     the right filter controls the Parametric EQ parameters
                    /// 
                    /// for stereo
                    /// sounds:
                    ///     both left and right channels must have the same filter
                    ///     i.e., filter type both is invalid
                    /// </summary>
                    public FilterTypeValue FilterType;
                    public int FilterWidth; // [0,7]
                    /// <summary>
                    /// in Hz [0,8000]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock LeftFilterFrequency;
                    /// <summary>
                    /// in dB
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock1 LeftFilterGain;
                    /// <summary>
                    /// in Hz [0,8000]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock2 RightFilterFrequency;
                    /// <summary>
                    /// in dB
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock3 RightFilterGain;
                    
                    public enum FilterTypeValue : int
                    {
                        ParametricEq,
                        Dls2,
                        BothOnlyValidForMono
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock1 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock2 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock3 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x30)]
                public class PlatformSoundPitchLfoBlock : TagStructure
                {
                    /// <summary>
                    /// seconds
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock Delay;
                    /// <summary>
                    /// Hz[0,43.7]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock1 Frequency;
                    /// <summary>
                    /// octaves[-1,1]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock2 PitchModulation;
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock1 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock2 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x40)]
                public class PlatformSoundFilterLfoBlock : TagStructure
                {
                    /// <summary>
                    /// in seconds
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock Delay;
                    /// <summary>
                    /// in Hz[0,43.7]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock1 Frequency;
                    /// <summary>
                    /// octaves[-8,8]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock2 CutoffModulation;
                    /// <summary>
                    /// dB[-16,16]
                    /// </summary>
                    public SoundPlaybackParameterDefinitionBlock3 GainModulation;
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock1 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock2 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameterDefinitionBlock3 : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x28)]
                public class SoundEffectPlaybackBlock : TagStructure
                {
                    public SoundEffectStructDefinitionBlock SoundEffectStruct;
                    
                    [TagStructure(Size = 0x28)]
                    public class SoundEffectStructDefinitionBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "<fx>" })]
                        public CachedTag Unknown;
                        public List<SoundEffectComponentBlock> Components;
                        public List<SoundEffectOverridesBlock> Unknown1;
                        public byte[] Unknown2;
                        public List<PlatformSoundEffectCollectionBlock> Unknown3;
                        
                        [TagStructure(Size = 0x10)]
                        public class SoundEffectComponentBlock : TagStructure
                        {
                            [TagField(ValidTags = new [] { "lsnd","snd!" })]
                            public CachedTag Sound;
                            /// <summary>
                            /// additional attenuation to sound
                            /// </summary>
                            public float Gain; // dB
                            public FlagsValue Flags;
                            
                            [Flags]
                            public enum FlagsValue : uint
                            {
                                DonTPlayAtStart = 1 << 0,
                                PlayOnStop = 1 << 1,
                                Unknown = 1 << 2,
                                PlayAlternate = 1 << 3,
                                Unknown1 = 1 << 4,
                                SyncWithOriginLoopingSound = 1 << 5
                            }
                        }
                        
                        [TagStructure(Size = 0xC)]
                        public class SoundEffectOverridesBlock : TagStructure
                        {
                            public StringId Name;
                            public List<SoundEffectOverrideParametersBlock> Overrides;
                            
                            [TagStructure(Size = 0x20)]
                            public class SoundEffectOverrideParametersBlock : TagStructure
                            {
                                public StringId Name;
                                public StringId Input;
                                public StringId Range;
                                public float TimePeriod; //  seconds
                                public int IntegerValue;
                                public float RealValue;
                                public MappingFunctionBlock FunctionValue;
                                
                                [TagStructure(Size = 0x8)]
                                public class MappingFunctionBlock : TagStructure
                                {
                                    public List<ByteBlock> Data;
                                    
                                    [TagStructure(Size = 0x1)]
                                    public class ByteBlock : TagStructure
                                    {
                                        public sbyte Value;
                                    }
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class PlatformSoundEffectCollectionBlock : TagStructure
                        {
                            public List<PlatformSoundEffectBlock> SoundEffects;
                            public List<PlatformSoundEffectFunctionBlock> LowFrequencyInput;
                            public int SoundEffectOverrides;
                            
                            [TagStructure(Size = 0x1C)]
                            public class PlatformSoundEffectBlock : TagStructure
                            {
                                public List<PlatformSoundEffectFunctionBlock> FunctionInputs;
                                public List<PlatformSoundEffectConstantBlock> ConstantInputs;
                                public List<PlatformSoundEffectOverrideDescriptorBlock> TemplateOverrideDescriptors;
                                public int InputOverrides;
                                
                                [TagStructure(Size = 0x10)]
                                public class PlatformSoundEffectFunctionBlock : TagStructure
                                {
                                    public InputValue Input;
                                    public RangeValue Range;
                                    public MappingFunctionBlock Function;
                                    public float TimePeriod; //  seconds
                                    
                                    public enum InputValue : short
                                    {
                                        Zero,
                                        Time,
                                        Scale,
                                        Rolloff
                                    }
                                    
                                    public enum RangeValue : short
                                    {
                                        Zero,
                                        Time,
                                        Scale,
                                        Rolloff
                                    }
                                    
                                    [TagStructure(Size = 0x8)]
                                    public class MappingFunctionBlock : TagStructure
                                    {
                                        public List<ByteBlock> Data;
                                        
                                        [TagStructure(Size = 0x1)]
                                        public class ByteBlock : TagStructure
                                        {
                                            public sbyte Value;
                                        }
                                    }
                                }
                                
                                [TagStructure(Size = 0x4)]
                                public class PlatformSoundEffectConstantBlock : TagStructure
                                {
                                    public float ConstantValue;
                                }
                                
                                [TagStructure(Size = 0x1)]
                                public class PlatformSoundEffectOverrideDescriptorBlock : TagStructure
                                {
                                    public sbyte OverrideDescriptor;
                                }
                            }
                            
                            [TagStructure(Size = 0x10)]
                            public class PlatformSoundEffectFunctionBlock : TagStructure
                            {
                                public InputValue Input;
                                public RangeValue Range;
                                public MappingFunctionBlock Function;
                                public float TimePeriod; //  seconds
                                
                                public enum InputValue : short
                                {
                                    Zero,
                                    Time,
                                    Scale,
                                    Rolloff
                                }
                                
                                public enum RangeValue : short
                                {
                                    Zero,
                                    Time,
                                    Scale,
                                    Rolloff
                                }
                                
                                [TagStructure(Size = 0x8)]
                                public class MappingFunctionBlock : TagStructure
                                {
                                    public List<ByteBlock> Data;
                                    
                                    [TagStructure(Size = 0x1)]
                                    public class ByteBlock : TagStructure
                                    {
                                        public sbyte Value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class SoundGestaltRuntimePermutationBitVectorBlock : TagStructure
        {
            public sbyte Unknown;
        }
        
        [TagStructure(Size = 0xC)]
        public class SoundPermutationChunkBlock : TagStructure
        {
            public int FileOffset;
            public int Unknown;
            public int Unknown1;
        }
        
        [TagStructure(Size = 0x1C)]
        public class SoundGestaltPromotionsBlock : TagStructure
        {
            public SoundPromotionParametersStructBlock Unknown;
            
            [TagStructure(Size = 0x1C)]
            public class SoundPromotionParametersStructBlock : TagStructure
            {
                public List<SoundPromotionRuleBlock> PromotionRules;
                public List<SoundPromotionRuntimeTimerBlock> Unknown;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x10)]
                public class SoundPromotionRuleBlock : TagStructure
                {
                    public short PitchRange;
                    public short MaximumPlayingCount;
                    /// <summary>
                    /// time from when first permutation plays to when another sound from an equal or lower promotion can play
                    /// </summary>
                    public float SuppressionTime; // seconds
                    [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x4)]
                public class SoundPromotionRuntimeTimerBlock : TagStructure
                {
                    public int Unknown;
                }
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class SoundGestaltExtraInfoBlock : TagStructure
        {
            public List<SoundEncodedDialogueSectionBlock> EncodedPermutationSection;
            public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
            
            [TagStructure(Size = 0x10)]
            public class SoundEncodedDialogueSectionBlock : TagStructure
            {
                public byte[] EncodedData;
                public List<SoundPermutationDialogueInfoBlock> SoundDialogueInfo;
                
                [TagStructure(Size = 0x10)]
                public class SoundPermutationDialogueInfoBlock : TagStructure
                {
                    public int MouthDataOffset;
                    public int MouthDataLength;
                    public int LipsyncDataOffset;
                    public int LipsyncDataLength;
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class GlobalGeometryBlockInfoStructBlock : TagStructure
            {
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GlobalGeometryBlockResourceBlock> Resources;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short OwnerTagSectionOffset;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x10)]
                public class GlobalGeometryBlockResourceBlock : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short PrimaryLocator;
                    public short SecondaryLocator;
                    public int ResourceDataSize;
                    public int ResourceDataOffset;
                    
                    public enum TypeValue : sbyte
                    {
                        TagBlock,
                        TagData,
                        VertexBuffer
                    }
                }
            }
        }
    }
}

