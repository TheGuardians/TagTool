using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_effect_collection", Tag = "sfx+", Size = 0x8)]
    public class SoundEffectCollection : TagStructure
    {
        public List<PlatformSoundPlaybackBlock> SoundEffects;
        
        [TagStructure(Size = 0x38)]
        public class PlatformSoundPlaybackBlock : TagStructure
        {
            public StringId Name;
            public PlatformSoundPlaybackStructBlock Playback;
            
            [TagStructure(Size = 0x34)]
            public class PlatformSoundPlaybackStructBlock : TagStructure
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
    }
}

