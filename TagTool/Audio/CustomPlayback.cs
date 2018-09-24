using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloOnline106708)]
    public class CustomPlayback : TagStructure
	{
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
        public FlagsValue Flags;
        public uint Unknown4;
        public uint Unknown5;
        public List<FilterBlock> Filter;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown15;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown16;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown17;

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            Use3dRadioHack = 1 << 0,
        }

        [TagStructure(Size = 0x48)]
        public class FilterBlock : TagStructure
		{
            public int FilterType;
            public int FilterWidth;

            public Bounds<float> ScaleBounds1;
            public float RandomBase1;
            public float RandomVariance1;

            public Bounds<float> ScaleBounds2;
            public float RandomBase2;
            public float RandomVariance2;

            public Bounds<float> ScaleBounds3;
            public float RandomBase3;
            public float RandomVariance3;

            public Bounds<float> ScaleBounds4;
            public float RandomBase4;
            public float RandomVariance4;
        }
    }
}