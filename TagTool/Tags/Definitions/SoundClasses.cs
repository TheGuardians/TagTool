using TagTool.Cache;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_classes", Tag = "sncl", Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundClasses : TagStructure
	{
        public List<Class> Classes;

        [TagStructure(Size = 0x98, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC0, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.HaloOnline106708)]
        public class Class : TagStructure
		{
            public short MaxSoundsPerTag;
            public short MaxSoundsPerObject;
            public int PreemptionTime;
            public InternalFlagBits InternalFlags;
            public ClassFlagBits Flags;
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

            [Flags]
            public enum InternalFlagBits : ushort
            {
                None,
                ClassIsValid = 1 << 0,
                ClassIsSpeech = 1 << 1,
                ClassIsScripted = 1 << 2,
                MutesWithObject = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Multilingual = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15
            }

            [Flags]
            public enum ClassFlagBits : ushort
            {
                None,
                CanPlayDuringPause = 1 << 0,
                DryStereoMix = 1 << 1,
                PlaysThroughObjects = 1 << 2,
                IsCenterUnspatialized = 1 << 3,
                MonoLfe = 1 << 4,
                Deterministic = 1 << 5,
                UseHugeTransmission = 1 << 6,
                AlwaysUseSpeakers = 1 << 7,
                IsAvailableOnMainmenu = 1 << 8,
                IgnoreStereoHeadroom = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                ClassPlaysOnMainmenu = 1 << 14,
                Bit15 = 1 << 15,
            }

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