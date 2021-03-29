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
        public short Unknown1; // public ChannelTypeEnum ChannelType; // based of h4 defs, though unconfirmed as these fields aren't used in h3 or odst
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] UnusedPadding;
        public int Unknown2; //public CinematicFlags Flags;
        public int Unknown3; // public float EasingInTime;
        public int Unknown4; // public float EasingOutTime;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public int Unknown8;

        // I can't confirm the types for these 3 as they don't seem to be used in retail at all
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;

        public int Unknown12;
        public CachedTag BinkMovie;

        // Scripts are in ASCIIZ format, they will probably need conversion to work in HO
        public byte[] ImportScriptHeader;
        public List<TagReferenceBlock> CinematicScenes;
        public byte[] ImportScriptFooter;
        public byte[] ImportScriptSkip;

        [TagStructure(Size = 0x4)]
		public class SceneIndex : TagStructure
		{
            public uint ShotFlags;
        }

        [Flags]
        public enum CinematicFlags : int
        {
            None = 0,
            Outro = 1 << 0,
            ExtraMemoryBink = 1 << 1,
            OpaqueBink = 1 << 2,
            DontStretchBink = 1 << 3,
            DontForceHologramRender = 1 << 4
        }

        public enum ChannelTypeEnum : short
        {
            Letterbox,
            Briefing,
            Perspective,
            Vignette,
            BinkBriefing, // binks are probably h4 only - need to come back to this
            BinkFullscreen
        }
    }
}