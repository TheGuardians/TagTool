using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
    public class ExtraInfo : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public int Unknown5;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public int Unknown6;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public DatumHandle BlockOffset;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public int BlockSize;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public uint SectionDataSize;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public uint ResourceDataSize;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<TagResourceGen2> Resources;

        [TagField(Flags = Short, MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTag Original;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short OwnerTagSectionOffset;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte RuntimeLinked;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte RuntimeLoaded;

        [TagField(Flags = Short, MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTag Runtime;


        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<LanguagePermutation> LanguagePermutations;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
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
        public class LanguagePermutation : TagStructure
		{
            public List<RawInfoBlock> RawInfo;

            [TagStructure(Size = 0x7C)]
            public class RawInfoBlock : TagStructure
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
                public uint ResourceSampleSize;
                public uint ResourceSampleOffset;
                public uint SampleCount;
                public uint Unknown20;
                public uint Unknown21;
                public uint Unknown22;
                public uint Unknown23;
                public int Unknown24;

                [TagStructure(Size = 0x18)]
                public class Unknown : TagStructure
				{
                    public int Unknown2;
                    public short SoundDialogInfoSize;
                    public short Unknown1;
                    public int Unknown3;
                    public uint Unknown4;
                    public int Unknown5;
                    public int Unknown6;
                }
            }
        }

        [TagStructure(Size = 0x2C)]
        public class EncodedPermutationSection : TagStructure
		{
            public byte[] EncodedData;
            public List<SoundDialogueInfoBlock> SoundDialogueInfo;
            public List<UnknownBlock> Unknown;

            [TagStructure(Size = 0x10)]
            public class SoundDialogueInfoBlock : TagStructure
			{
                public uint MouthDataOffset;
                public uint MouthDataLength;
                public uint LipsyncDataOffset;
                public uint LipsyncDataLength;
            }

            [TagStructure(Size = 0xC)]
            public class UnknownBlock : TagStructure
			{
                public List<UnknownBlock2> Unknown;

                [TagStructure(Size = 0x28)]
                public class FacialAnimationPermutations : TagStructure
				{
                    public float StartTime;
                    public float EndTime;
                    public float BlendIn;
                    public float BlendOut;
                    public List<UnknownBlock2_1> Unknown5;
                    public List<FacialAnimationCurve> FacialAnimationCurves;

                    [TagStructure(Size = 0x8)]
                    public class UnknownBlock2_1 : TagStructure
					{
                        public uint Unknown1;
                        public uint Unknown2;
                    }

                    [TagStructure(Size = 0x8)]
                    public class FacialAnimationCurve : TagStructure
					{
                        public short AnimationStartTime;
                        public sbyte Unknown2;
                        public sbyte Unknown3;
                        public sbyte Unknown4;
                        public sbyte Unknown5;
                        public sbyte Unknown6;
                        public sbyte Unknown7;
                    }
                }
            }
        }
    }
}