using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0xE4, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x170, MinVersion = CacheVersion.HaloOnline106708)]
    public class CameraFxSettings : TagStructure
	{
        #region exposure
        public FlagsValue Flags;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused1 = new byte[2];

        public float OverexposureAmount;
        public float OverexposureUnknown;
        public float OverexposureUnknown2;
        public float BrightnessAmount;
        public float BrightnessUnknown;
        public float BrightnessUnknown2;
        public float BrightnessUnknown3;
        #endregion

        #region auto-exposure sensitivity
        public FlagsValue Flags2;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused2 = new byte[2];

        public float Unknown3;
        #endregion

        #region exposure compensation
        public FlagsValue Flags3;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused3 = new byte[2];

        public float Unknown5;
        #endregion

        #region bloom point
        public FlagsValue Flags4;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused4 = new byte[2];

        public float Base;
        public float Min;
        public float Max;
        #endregion

        #region inherent bloom
        public FlagsValue Flags5;
        public short Unknown7;
        public float Base2;
        public float Min2;
        public float Max2;
        #endregion

        public FlagsValue Flags6;
        public short Unknown8;
        public float Base3;
        public float Min3;
        public float Max3;

        public FlagsValue Flags7;
        public short Unknown9;
        public float Red;
        public float Green;
        public float Blue;

        public FlagsValue Flags8;
        public short Unknown10;
        public float Red2;
        public float Green2;
        public float Blue2;

        public FlagsValue Flags9;
        public short Unknown11;
        public float Red3;
        public float Green3;
        public float Blue3;

        public FlagsValue Flags10;
        public short Unknown12;
        public float Unknown13;
        public float Unknown14;
        public float Unknown15;

        public FlagsValue Flags11;
        public short Unknown16;
        public float Unknown17;
        public float Unknown18;
        public float Unknown19;

        public FlagsValue Flags12;
        public short Unknown20;
        public float Unknown21;
        public float Unknown22;
        public float Unknown23;
        public short Unknown24_1;
        public short Unknown24_2;

        public FlagsValue Flags13;
        public short Unknown25;
        public float Base4;
        public float Min4;
        public float Max4;

        public FlagsValue Flags14;
        public short Unknown26;
        public float Base5;
        public float Min5;
        public float Max5;

        //
        // SSAO
        //

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public FlagsValue Flags15;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown27;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown28;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown29;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown30;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public FlagsValue Flags16;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown31;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown32;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock> Unknown33;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock2> Unknown34;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock3> Unknown35;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock4> Unknown36;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock5> Unknown37;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock6> Unknown38;

        //
        // Godrays
        //

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public FlagsValue Flags17;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown39;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown40;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown41;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float ColorR;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float ColorG;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float ColorB;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown42;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown43;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown44;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown45;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown46;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            Disable = 1 << 0
        }

        [TagStructure(Size = 0x58)]
        public class UnknownBlock : TagStructure
		{
            public float Unknown;
            public int Unknown2;
            public TagFunction Unknown3 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown4 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown5 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown6 = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0xC)]
        public class UnknownBlock2 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock3 : TagStructure
		{
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock4 : TagStructure
		{
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
        }

        [TagStructure(Size = 0x94)]
        public class UnknownBlock5 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float Unknown19;
            public float Unknown20;
            public float Unknown21;
            public float Unknown22;
            public float Unknown23;
            public float Unknown24;
            public float Unknown25;
            public float Unknown26;
            public float Unknown27;
            public float Unknown28;
            public float Unknown29;
            public float Unknown30;
            public float Unknown31;
            public float Unknown32;
            public float Unknown33;
            public float Unknown34;
            public float Unknown35;
            public float Unknown36;
            public float Unknown37;
        }

        [TagStructure(Size = 0x28)]
        public class UnknownBlock6 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
        }
    }
}