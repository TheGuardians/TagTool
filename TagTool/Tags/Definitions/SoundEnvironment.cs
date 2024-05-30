using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x48, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
	[TagStructure(Name = "sound_environment", Tag = "snde", Size = 0x38, MinVersion = CacheVersion.HaloReach)]
	public class SoundEnvironment : TagStructure
	{
		[TagField(Length = 4, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
		public byte[] Padding0;

		[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
		public short Priority;

		[TagField(Length = 2, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
		public byte[] Padding1;

		public float RoomIntensity; // (decibels) intensity of the room effect
		public float RoomIntensityHighFrequency; // (decibels) intensity of the room effect above HighFrequencyReference
		public float RoomRolloff; // [0-10] how quickly the room effect rolls off, from 0.0 to 10.0
		public float DecayTime; // [1-20] seconds
		public float DecayHighFrequencyRatio; // [1-2]
		public float ReflectionsIntensity; // dB[-100,10]
		public float ReflectionsDelay; // [0-3] seconds
		public float ReverbIntensity; // dB[-100,20]
		public float ReverbDelay; // [0-1] seconds
		public float Diffusion;
		public float Density;
		public float HighFrequencyReference; // [20-20000] Hz

		[TagField(Length = 16, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
		public byte[] Padding2;

		[TagField(MinVersion = CacheVersion.HaloReach)]
		public float LowPassCutoffFrequency;

		[TagField(MinVersion = CacheVersion.HaloReach)]
		public float LowPassOutputGain;
	}
}
