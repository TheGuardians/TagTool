using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_radio_settings", Tag = "srad", Size = 0x24)]
    public class SoundRadioSettings : TagStructure
    {
        public RadioMixSettingDefinition Mix;
        public GlobalSoundLookUpTableDistortionBlock DistortionSettings;
        public GlobalSoundEqualizerBlock EqualizerSettings;
        
        public enum RadioMixSettingDefinition : int
        {
            // full unspatialized radio effect
            Full,
            // mixes in the unspatialized radio effect with distance
            _3d,
            // essentially turns off the radio effect!
            None
        }
        
        [TagStructure(Size = 0xC)]
        public class GlobalSoundLookUpTableDistortionBlock : TagStructure
        {
            public int TableSize;
            public float NoiseAmount;
            public LookUpTableFlags Flags;
            
            [Flags]
            public enum LookUpTableFlags : uint
            {
                Interpolate = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class GlobalSoundEqualizerBlock : TagStructure
        {
            public float InputGain; // dB
            public float OutputGain; // dB
            public List<SoundEqualizerBandSettingsBlockStruct> BandSettings;
            
            [TagStructure(Size = 0x10)]
            public class SoundEqualizerBandSettingsBlockStruct : TagStructure
            {
                public EqualizerBandType Type;
                public float Frequency; // Hz
                public float Gain; // dB
                public float Q;
                
                public enum EqualizerBandType : int
                {
                    BandPass,
                    LowShelf,
                    HighShelf,
                    LowPass,
                    HighPass
                }
            }
        }
    }
}
