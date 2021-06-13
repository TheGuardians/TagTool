using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x48)]
    public class SoundEnvironment : TagStructure
    {
        public GlobalSoundReverbBlock ReverbSettings;
        public GlobalSoundLowpassBlock LowpassSettings;
        
        [TagStructure(Size = 0x40)]
        public class GlobalSoundReverbBlock : TagStructure
        {
            // intensity of the room effect
            public float RoomIntensity; // dB
            // intensity of the room effect above the reference high frequency
            public float RoomIntensityHf; // dB
            // how quickly the room effect rolls off, from 0.0 to 10.0
            public float RoomRolloff;
            public float DecayTime; // seconds
            public float DecayHfRatio;
            public float ReflectionsIntensity; // dB[-100,10]
            public float ReflectionsDelay; // seconds
            public float ReverbIntensity; // dB[-100,20]
            public float ReverbDelay; // seconds
            public float Diffusion;
            public float Density;
            // for hf values, what frequency defines hf, from 20 to 20,000
            public float HfReference; // Hz
            // Name of the environment from WWise. This will be a DSP effect with Environmental Effect checked.
            public StringId EnvironmentName;
            // 0 to 1 with 0 being none, and 1 being full.
            public float DryMixValue;
            // 0 to 1 with 0 being none, and 1 being full.
            public float WetMixValue;
            // 0 to 1 with 0 being none, and 1 being full.
            public float PlayerWetMixValue;
        }
        
        [TagStructure(Size = 0x8)]
        public class GlobalSoundLowpassBlock : TagStructure
        {
            public float CutoffFrequency; // Hz
            public float OutputGain; // dB
        }
    }
}
