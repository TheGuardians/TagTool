using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0x84)]
    public class SoundCacheFileGestalt : TagStructure
    {
        public List<SoundDefinitionPlaybackParameters> Playbacks;
        public List<SoundDefinitionScaleModifiers> Scales;
        public List<StringId> ImportNames;
        public List<CacheFileSoundPitchRangeParameters> PitchRangeParameters;
        public List<CacheFileSoundPitchRange> PitchRanges;
        public List<CacheFileSoundPermutation> Permutations;
        public List<PlatformSoundPlaybackDefinition> CustomPlaybacks;
        public List<RuntimePermutationFlagsBlock> RuntimePermutationFlags;
        public List<SoundPermutationChunk> Chunks;
        public List<SoundPromotionParameters> Promotions;
        public List<CacheFileSoundDefinitionExtraInfo> ExtraInfos;
        
        [TagStructure(Size = 0x38)]
        public class SoundDefinitionPlaybackParameters : TagStructure
        {
            public SoundDefinitionPlaybackParameters Unknown1;
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundDefinitionScaleModifiers : TagStructure
        {
            public SoundDefinitionScaleModifiers Unknown1;
        }
        
        [TagStructure(Size = 0x4)]
        public class StringId : TagStructure
        {
            public StringId ImportName;
        }
        
        [TagStructure(Size = 0xA)]
        public class CacheFileSoundPitchRangeParameters : TagStructure
        {
            public short NaturalPitch; // cents
            public Bounds<short> BendBounds; // cents
            public Bounds<short> MaxGainPitchBounds; // cents
        }
        
        [TagStructure(Size = 0xC)]
        public class CacheFileSoundPitchRange : TagStructure
        {
            public short Name;
            public short Parameters;
            public short EncodedPermutationData;
            public short FirstRuntimePermutationFlagIndex;
            public short FirstPermutation;
            public short PermutationCount;
        }
        
        [TagStructure(Size = 0x10)]
        public class CacheFileSoundPermutation : TagStructure
        {
            public short Name;
            public short EncodedSkipFraction;
            public sbyte EncodedGain; // dB
            public sbyte PermutationInfoIndex;
            public short LanguageNeutralTime; // ms
            public int SampleSize;
            public short FirstChunk;
            public short ChunkCount;
        }
        
        [TagStructure(Size = 0x48)]
        public class PlatformSoundPlaybackDefinition : TagStructure
        {
            public PlatformSoundPlaybackDefinition PlaybackDefinition;
        }
        
        [TagStructure(Size = 0x1)]
        public class RuntimePermutationFlagsBlock : TagStructure
        {
            public sbyte Unknown1;
        }
        
        [TagStructure(Size = 0xC)]
        public class SoundPermutationChunk : TagStructure
        {
            public int FileOffset;
            public int Unknown1;
            public int Unknown2;
        }
        
        [TagStructure(Size = 0x24)]
        public class SoundPromotionParameters : TagStructure
        {
            public SoundPromotionParameters Unknown1;
        }
        
        [TagStructure(Size = 0x34)]
        public class CacheFileSoundDefinitionExtraInfo : TagStructure
        {
            public List<SoundEncodedDialogueSection> EncodedPermutationSection;
            public GeometryBlockInfoStruct GeometryBlockInfo;
            
            [TagStructure(Size = 0x20)]
            public class SoundEncodedDialogueSection : TagStructure
            {
                public byte[] EncodedData;
                public List<SoundPermutationDialogueInfo> SoundDialogueInfo;
                
                [TagStructure(Size = 0x10)]
                public class SoundPermutationDialogueInfo : TagStructure
                {
                    public int MouthDataOffset;
                    public int MouthDataLength;
                    public int LipsyncDataOffset;
                    public int LipsyncDataLength;
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class GeometryBlockInfoStruct : TagStructure
            {
                /// <summary>
                /// BLOCK INFO
                /// </summary>
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GeometryBlockResource> Resources;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public short OwnerTagSectionOffset;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                
                [TagStructure(Size = 0x10)]
                public class GeometryBlockResource : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Flags = Padding, Length = 3)]
                    public byte[] Padding1;
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
    }
}

