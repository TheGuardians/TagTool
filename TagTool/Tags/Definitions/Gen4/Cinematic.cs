using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cinematic", Tag = "cine", Size = 0xC8)]
    public class Cinematic : TagStructure
    {
        public CinematicPlaybackDataBlock CinematicPlayback;
        public ScenarioAndZoneSetStruct ScenarioAndZoneSet;
        public StringId Name;
        public CinematicChannelTypeEnum ChannelType;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public CinematicFlags Flags;
        // seconds
        public float EasingInTime;
        // seconds
        public float EasingOutTime;
        [TagField(ValidTags = new [] { "citr" })]
        public CachedTag TransitionSettings;
        [TagField(ValidTags = new [] { "bink" })]
        public CachedTag BinkMovie;
        [TagField(Length = 32)]
        public string BinkMovieOnDisc;
        public CinematicCustomScriptBlock Header;
        public List<CinematicSceneReferenceBlock> Scenes;
        public CinematicCustomScriptBlock Footer;
        public CinematicCustomScriptBlock EarlyExit;
        
        public enum CinematicChannelTypeEnum : short
        {
            Letterbox,
            Briefing,
            Perspective,
            Vignette,
            BinkBriefing,
            Bink
        }
        
        [Flags]
        public enum CinematicFlags : uint
        {
            Outro = 1 << 0,
            ExtraMemoryBink = 1 << 1,
            OpaqueBink = 1 << 2,
            DonTStretchBink = 1 << 3,
            DonTForceHologramRender = 1 << 4
        }
        
        [TagStructure(Size = 0x18)]
        public class CinematicPlaybackDataBlock : TagStructure
        {
            public uint Scenes;
            public uint ScenesExpanded;
            public List<CinematicShotPlaybackDataBlock> Shots;
            public int BspZoneFlags;
            
            [TagStructure(Size = 0x8)]
            public class CinematicShotPlaybackDataBlock : TagStructure
            {
                [TagField(Length = 2)]
                public GCinematicshotFlagArray[]  ShotFlags;
                
                [TagStructure(Size = 0x4)]
                public class GCinematicshotFlagArray : TagStructure
                {
                    public uint ShotFlagData;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioAndZoneSetStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag Scenario;
            public int ZoneSet;
        }
        
        [TagStructure(Size = 0x14)]
        public class CinematicCustomScriptBlock : TagStructure
        {
            public byte[] Script;
        }
        
        [TagStructure(Size = 0x20)]
        public class CinematicSceneReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "cisc" })]
            public CachedTag Scene;
            [TagField(ValidTags = new [] { "cisd" })]
            public CachedTag Data;
        }
    }
}
