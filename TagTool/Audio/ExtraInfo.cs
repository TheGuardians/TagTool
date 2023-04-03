using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.ReleaseBuild)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.TagsBuild)]
    public class ExtraInfo : TagStructure
	{
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<LanguagePermutation> LanguagePermutations;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<EncodedPermutationSection> EncodedPermutationSections;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown1;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown2;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown3;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown4;

        [TagField(MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.ReleaseBuild)]
        public TagResourceReference FacialAnimationResource;

        [TagField(MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.TagsBuild)]
        public List<FacialAnimationLanguageBlockStruct> FacialAnimationResources;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public GlobalGeometryBlockInfoStruct GeometryBlockInfo;


        [TagStructure(Size = 0xC)]
        public class LanguagePermutation : TagStructure
		{
            public List<RawInfoBlock> RawInfo;

            [TagStructure(Size = 0x7C, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x4C, MinVersion = CacheVersion.HaloReach)]
            public class RawInfoBlock : TagStructure
			{
                public StringId SkipFractionName;
                public byte[] Samples;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public byte[] MouthData;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public byte[] LipsyncData;
                public List<SoundPermutationMarkerBlock> Markers;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<SoundPermutationMarkerBlock> LayerMarkers;
                public List<SeekTableBlock> SeekTable;
                public short Compression;
                public byte Language;
                [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public uint SampleCount;
                public uint ResourceSampleOffset;
                public uint ResourceSampleSize;
                [TagField(Gen = CacheGeneration.HaloOnline)]
                public uint Unknown20;
                [TagField(Gen = CacheGeneration.HaloOnline)]
                public uint Unknown21;
                [TagField(Gen = CacheGeneration.HaloOnline)]
                public uint Unknown22;
                [TagField(Gen = CacheGeneration.HaloOnline)]
                public uint Unknown23;
                [TagField(Gen = CacheGeneration.HaloOnline)]
                public int Unknown24;

                [TagStructure(Size = 0xC)]
                public class SoundPermutationMarkerBlock : TagStructure
                {
                    public int MarkerId;
                    public StringId Name;
                    public int SampleOffset;
                }

                [TagStructure(Size = 0x18)]
                public class SeekTableBlock : TagStructure
				{
                    public uint BlockRelativeSampleStart;
                    public uint BlockRelativeSampleEnd;
                    public uint StartingSampleIndex;
                    public uint EndingSampleIndex;
                    public uint StartingOffset;
                    public uint EndingOffset;
                }
            }
        }

        [TagStructure(Size = 0x2C)]
        public class EncodedPermutationSection : TagStructure
		{
            public byte[] EncodedData; // legacy CE-H2 data, apparently can still be used
            public List<PermutationDialogueInfo> SoundDialogueInfo; // ditto
            public List<SoundPermutationDialogueInfoNewBlock> SoundDialogueInfoNew;
			
            [TagStructure(Size = 0x10)]
            public class PermutationDialogueInfo : TagStructure
			{
                public uint MouthDataOffset;
                public uint MouthDataLength;
                public uint LipsyncDataOffset;
                public uint LipsyncDataLength;
            }

            [TagStructure(Size = 0xC, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x3C, Platform = CachePlatform.MCC)]
            public class SoundPermutationDialogueInfoNewBlock : TagStructure
			{
                [TagField(Platform = CachePlatform.MCC)]
                public LanguageIndicesStruct LanguageIndices;

                public List<FacialAnimationBlock> FacialAnimation;

                [TagStructure(Size = 0x28)]
                public class FacialAnimationBlock : TagStructure
				{
                    public float StartTime;
                    public float EndTime;
                    public float BlendIn;
                    public float BlendOut;
                    [TagField(Flags = TagFieldFlags.Padding, Length = 0xC)]
                    public byte[] Pad = new byte[0xC]; // possibly offset & size, unused
                    public List<FacialAnimationCurve> FacialAnimationCurves;

                    [TagStructure(Size = 0x8)]
                    public class FacialAnimationCurve : TagStructure
					{
                        public short AnimationStartTime;
                        public FacialAnimationTrack FirstTrack;
                        public FacialAnimationTrack SecondTrack;
                        public FacialAnimationTrack ThirdTrack;
                        public sbyte FirstPoseWeight;
                        public sbyte SecondPoseWeight;
                        public sbyte ThirdPoseWeight;

                        public enum FacialAnimationTrack : byte
                        {
                            Silence,
                            Eat,
                            Earth,
                            If,
                            Ox,
                            Oat,
                            Wet,
                            Size,
                            Church,
                            Fave,
                            Though,
                            Told,
                            Bump,
                            New,
                            Roar,
                            Cage,
                            Eyebrow_Raise,
                            Blink,
                            Orientation_Head_Pitch,
                            Orientation_Head_Roll,
                            Orientation_Head_Yaw,
                            Emphasis_Head_Pitch,
                            Emphasis_Head_Roll,
                            Emphasis_Head_Yaw,
                            Gaze_Eye_Pitch,
                            Gaze_Eye_Yaw,
                            happy,
                            sad,
                            angry,
                            disgusted,
                            scared,
                            surprised,
                            pain,
                            shout
                        }
                    }
                }

                [TagStructure(Size = 0x30, Platform = CachePlatform.MCC)]
                public class LanguageIndicesStruct : TagStructure
                {
                    public int EnglishIndex;
                    public int JapaneseIndex;
                    public int GermanIndex;
                    public int FrenchIndex;
                    public int SpanishIndex;
                    public int MexicanSpanishIndex;
                    public int ItalianIndex;
                    public int KoreanIndex;
                    public int ChinesetraditionalIndex;
                    public int ChinesesimplifiedIndex;
                    public int PortugueseIndex;
                    public int PolishIndex;
                }
            }
        }

        [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public class GlobalGeometryBlockInfoStruct : TagStructure
        {
            public int BlockOffset;
            public int BlockSize;
            public int SectionDataSize;
            public int ResourceDataSize;
            public List<GlobalGeometryBlockResourceBlock> Resources;

            public uint Unknown1;

            public short OwnerTagSectionOffset;

            public ushort Unknown2;
            public uint Unknown3;

            [TagStructure(Size = 0x10)]
            public class GlobalGeometryBlockResourceBlock : TagStructure
            {
                public TypeValue Type;

                public sbyte Unknown1;
                public ushort Unknown2;

                public short PrimaryLocator;
                public short SecondaryLocator;
                public int ResourceDataSize;
                public int ResourceDataOffset;

                public enum TypeValue : sbyte
                {
                    TagBlock,
                    TagData,
                    VertexBuffer
                }
            }
        }
    }

    [TagStructure(Size = 0xC)]
    public class FacialAnimationLanguageBlockStruct : TagStructure
    {
        public TagResourceReference FacialAnimationResource;
        public SoundLanguageEnum Language;

        public enum SoundLanguageEnum : int
        {
            English,
            Japanese,
            German,
            French,
            Spanish,
            MexicanSpanish,
            Italian,
            Korean,
            ChineseTraditional,
            ChineseSimplified,
            Portuguese,
            Polish,
            Russian,
            Danish,
            Finnish,
            Dutch,
            Norwegian
        }
    }
}