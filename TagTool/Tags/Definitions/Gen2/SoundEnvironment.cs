using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x48)]
    public class SoundEnvironment : TagStructure
    {
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public short Priority; // when multiple listeners are in different sound environments in split screen, the combined environment will be the one with the highest priority.
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public float RoomIntensity; // dB
        public float RoomIntensityHf; // dB
        public float RoomRolloff0To10; // how quickly the room effect rolls off, from 0.0 to 10.0
        public float DecayTime1To20; // seconds
        public float DecayHfRatio1To2;
        public float ReflectionsIntensity; // dB[-100,10]
        public float ReflectionsDelay0To3; // seconds
        public float ReverbIntensity; // dB[-100,20]
        public float ReverbDelay0To1; // seconds
        public float Diffusion;
        public float Density;
        public float HfReference20To20000; // Hz
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding3;
    }
}

