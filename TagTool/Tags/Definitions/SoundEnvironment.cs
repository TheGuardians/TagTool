using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x48, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x50, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundEnvironment : TagStructure
	{
        public uint Unknown1;
        public short Priority;
        public short Unknown2;
        public float RoomIntensity;
        public float RoomIntensityHighFrequency;
        public float RoomRolloff;
        public float DecayTime;
        public float DecayHighFrequencyRatio;
        public float ReflectionsIntensity;
        public float ReflectionsDelay;
        public float ReverbIntensity;
        public float ReverbDelay;
        public float Diffusion;
        public float Density;
        public float HighFrequencyRefrence;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}