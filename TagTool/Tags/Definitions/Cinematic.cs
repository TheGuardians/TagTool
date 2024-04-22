using System;
using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cinematic", Tag = "cine", Size = 0xB0 , MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "cinematic", Tag = "cine", Size = 0xB4, MinVersion = CacheVersion.Halo3ODST)]
    public class Cinematic : TagStructure
	{
        // both these fields reference an index in the scenes block, eg bit 0 = scene 0
        public uint ScenesFlags;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint ScenesExpandedFlags;
        public List<SceneIndex> Shots;

        public CachedTag Scenario;
        public int Zoneset;

        public StringId Name;
        public CinematicChannelTypeEnum ChannelType;
        [TagField(Length = 2, Flags = Padding)]
        public byte[] PADDING;
        public CinematicFlags Flags;
        public float EasingInTime;
        public float EasingOutTime;
        public RealRgbColor FadeInColor;
        public int FadeInTime;
        public RealRgbColor FadeOutColor;
        public int FadeOutTime;
        public CachedTag BinkMovie;
        public byte[] ImportScriptHeader;
        public List<TagReferenceBlock> CinematicScenes;
        public byte[] ImportScriptFooter;
        public byte[] ImportScriptSkip;

        [TagStructure(Size = 0x4)]
		public class SceneIndex : TagStructure
		{
            public uint ShotFlags;
        }

        public enum CinematicChannelTypeEnum : short
        {
            Letterbox,
            Briefing,
            Perspective,
            BinkBriefing,
            CortanaEffect
        }

        [Flags]
        public enum CinematicFlags : uint
        {
            Outro = 1 << 0
        }
    }
}