using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_classes", Tag = "sncl", Size = 0xC, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_classes", Tag = "sncl", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundClasses : TagStructure
	{
        public List<Class> Classes;

        [TagField(Flags = TagFieldFlags.Padding, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x98, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC0, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.HaloOnline106708)]
        public class Class : TagStructure
		{
            public short MaxSoundsPerTag;
            public short MaxSoundsPerObject;
            public int PreemptionTime;
            public ushort InternalFlags;
            public ushort Flags;
            public short Priority;
            public CacheMissModeValue CacheMissMode;
            public sbyte Unknown;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown2;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown5;

            public float ReverbGain;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public float DistanceBoundsMin;
            public float DistanceBoundsMax;
            public float GainBoundsMin;
            public float GainBoundsMax;
            public float CutsceneDucking;
            public float CutsceneDuckingFadeInTime;
            public float CutsceneDuckingSustain;
            public float CutsceneDuckingFadeOutTime;
            public float ScriptedDialogDucking;
            public float ScriptedDialogDuckingFadeIn;


            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;

            //
            // ODST unique part
            //

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown23;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown24;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown25;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown26;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown27;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown28;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown29;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown30;


            public float UnknownDoplerFactor;
            public StereoPlaybackTypeValue StereoPlaybackType;
            public sbyte Unknown31;
            public sbyte Unknown32;
            public sbyte Unknown33;
            public float TransmissionMultiplier;
            public float ObstructionMaxBend;
            public float OcclusionMaxBend;
            public int Unknown34;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown35;

            public enum CacheMissModeValue : sbyte
            {
                Discard,
                Postpone,
            }

            public enum StereoPlaybackTypeValue : sbyte
            {
                FirstPerson,
                Ambient,
            }
        }
    }
}