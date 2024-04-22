using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x48)]
    public class SoundEnvironment : TagStructure
    {
        [TagField(Length = 0x4)]
        public byte[] Padding;
        /// <summary>
        /// when multiple listeners are in different sound environments in split screen, the combined environment will be the one
        /// with the highest priority.
        /// </summary>
        public short Priority;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        /// <summary>
        /// intensity of the room effect
        /// </summary>
        public float RoomIntensity;
        /// <summary>
        /// intensity of the room effect above the reference high frequency
        /// </summary>
        public float RoomIntensityHf;
        /// <summary>
        /// how quickly the room effect rolls off, from 0.0 to 10.0
        /// </summary>
        public float RoomRolloff0To10;
        public float DecayTime1To20; // seconds
        public float DecayHfRatio1To2;
        public float ReflectionsIntensity;
        public float ReflectionsDelay0To3; // seconds
        public float ReverbIntensity;
        public float ReverbDelay0To1; // seconds
        public float Diffusion;
        public float Density;
        /// <summary>
        /// for hf values, what frequency defines hf, from 20 to 20,000
        /// </summary>
        public float HfReference20To20000; // Hz
        [TagField(Length = 0x10)]
        public byte[] Padding2;
    }
}

