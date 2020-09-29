using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_effect_collection", Tag = "sfx+", Size = 0xC)]
    public class SoundEffectCollection : TagStructure
    {
        public List<PlatformSoundPlaybackNamedDefinition> SoundEffects;
        
        [TagStructure(Size = 0x4C)]
        public class PlatformSoundPlaybackNamedDefinition : TagStructure
        {
            public StringId Name;
            public PlatformSoundPlaybackDefinition Playback;
            
            [TagStructure(Size = 0x48)]
            public class PlatformSoundPlaybackDefinition : TagStructure
            {
                public List<PlatformSoundMixbin> Unknown1;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding1;
                public List<PlatformSoundSourceFilter> Filter;
                public List<PlatformSoundSourcePitchLfo> PitchLfo;
                public List<PlatformSoundSourceFilterLfo> FilterLfo;
                public List<SoundEffectDefinition> SoundEffect;
                
                [TagStructure(Size = 0x8)]
                public class PlatformSoundMixbin : TagStructure
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
                public class PlatformSoundSourceFilter : TagStructure
                {
                    /// <summary>
                    /// parameter description
                    /// </summary>
                    /// <remarks>
                    /// DLS2 filtering:
                    ///     resonance gain range: [0, 22.5] dB
                    /// 
                    /// parametric EQ:
                    ///     gain range: [-64, 14] dB
                    /// 
                    /// for mono sounds:
                    ///     the left filter controls the DLS 2 parameters
                    ///     the right filter controls the Parametric EQ parameters
                    /// 
                    /// for stereo sounds:
                    ///     both left and right channels must have the same filter
                    ///     i.e., filter type both is invalid
                    /// </remarks>
                    public FilterTypeValue FilterType;
                    public int FilterWidth; // [0,7]
                    /// <summary>
                    /// left filter frequency
                    /// </summary>
                    /// <remarks>
                    /// in Hz [0,8000]
                    /// </remarks>
                    public SoundPlaybackParameter LeftFilterFrequency;
                    /// <summary>
                    /// left filter gain
                    /// </summary>
                    /// <remarks>
                    /// in dB
                    /// </remarks>
                    public SoundPlaybackParameter LeftFilterGain;
                    /// <summary>
                    /// right filter frequency
                    /// </summary>
                    /// <remarks>
                    /// in Hz [0,8000]
                    /// </remarks>
                    public SoundPlaybackParameter RightFilterFrequency;
                    /// <summary>
                    /// right filter gain
                    /// </summary>
                    /// <remarks>
                    /// in dB
                    /// </remarks>
                    public SoundPlaybackParameter RightFilterGain;
                    
                    public enum FilterTypeValue : int
                    {
                        ParametricEq,
                        Dls2,
                        BothOnlyValidForMono
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameter : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x30)]
                public class PlatformSoundSourcePitchLfo : TagStructure
                {
                    /// <summary>
                    /// delay
                    /// </summary>
                    /// <remarks>
                    /// seconds
                    /// </remarks>
                    public SoundPlaybackParameter Delay;
                    /// <summary>
                    /// frequency
                    /// </summary>
                    /// <remarks>
                    /// Hz[0,43.7]
                    /// </remarks>
                    public SoundPlaybackParameter Frequency;
                    /// <summary>
                    /// pitch modulation
                    /// </summary>
                    /// <remarks>
                    /// octaves[-1,1]
                    /// </remarks>
                    public SoundPlaybackParameter PitchModulation;
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameter : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x40)]
                public class PlatformSoundSourceFilterLfo : TagStructure
                {
                    /// <summary>
                    /// delay
                    /// </summary>
                    /// <remarks>
                    /// in seconds
                    /// </remarks>
                    public SoundPlaybackParameter Delay;
                    /// <summary>
                    /// frequency
                    /// </summary>
                    /// <remarks>
                    /// in Hz[0,43.7]
                    /// </remarks>
                    public SoundPlaybackParameter Frequency;
                    /// <summary>
                    /// cutoff modulation
                    /// </summary>
                    /// <remarks>
                    /// octaves[-8,8]
                    /// </remarks>
                    public SoundPlaybackParameter CutoffModulation;
                    /// <summary>
                    /// gain modulation
                    /// </summary>
                    /// <remarks>
                    /// dB[-16,16]
                    /// </remarks>
                    public SoundPlaybackParameter GainModulation;
                    
                    [TagStructure(Size = 0x10)]
                    public class SoundPlaybackParameter : TagStructure
                    {
                        public Bounds<float> ScaleBounds;
                        public Bounds<float> RandomBaseAndVariance;
                    }
                }
                
                [TagStructure(Size = 0x48)]
                public class SoundEffectDefinition : TagStructure
                {
                    public SoundEffectDefinition SoundEffectStruct;
                }
            }
        }
    }
}

