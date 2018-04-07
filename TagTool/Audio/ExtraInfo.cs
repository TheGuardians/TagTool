using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
    public class ExtraInfo
    {
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<LanguagePermutation> LanguagePermutations;

        public List<EncodedPermutationSection> EncodedPermutationSections;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown1;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;

        [TagStructure(Size = 0xC)]
        public class LanguagePermutation
        {
            public List<RawInfoBlock> RawInfo;

            [TagStructure(Size = 0x7C)]
            public class RawInfoBlock
            {
                public StringId SkipFractionName;
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
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
                public uint Unknown16;
                public uint Unknown17;
                public uint Unknown18;
                public List<Unknown> UnknownList;
                public short Compression;
                public byte Language;
                public byte Unknown19;
                public uint SampleCount;
                public uint ResourceSampleOffset;
                public uint ResourceSampleSize;
                public uint Unknown20;
                public uint Unknown21;
                public uint Unknown22;
                public uint Unknown23;
                public int Unknown24;

                [TagStructure(Size = 0x18)]
                public class Unknown
                {
                    public uint Unknown1;
                    public uint Unknown2;
                    public uint Unknown3;
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;
                }
            }
        }

        [TagStructure(Size = 0x2C)]
        public class EncodedPermutationSection
        {
            public byte[] EncodedData;
            public List<SoundDialogueInfoBlock> SoundDialogueInfo;
            public List<UnknownBlock> Unknown;

            [TagStructure(Size = 0x10)]
            public class SoundDialogueInfoBlock
            {
                public uint MouthDataOffset;
                public uint MouthDataLength;
                public uint LipsyncDataOffset;
                public uint LipsynceDataLength;
            }

            [TagStructure(Size = 0xC)]
            public class UnknownBlock
            {
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown1;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown3;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public List<UnknownBlock2> Unknown;

                [TagStructure(Size = 0x28)]
                public class UnknownBlock2
                {
                    public uint Unknown;
                    public uint Unknown2;
                    public uint Unknown3;
                    public uint Unknown4;
                    public List<UnknownBlock> Unknown5;
                    public List<UnknownBlock2_2> Unknown6;

                    [TagStructure(Size = 0x8)]
                    public class UnknownBlock
                    {
                        public uint Unknown;
                        public uint Unknown2;
                    }

                    [TagStructure(Size = 0x8)]
                    public class UnknownBlock2_2
                    {
                        public short Unknown;
                        public short Unknown2;
                        public short Unknown3;
                        public short Unknown4;
                    }
                }
            }
        }
    }
}