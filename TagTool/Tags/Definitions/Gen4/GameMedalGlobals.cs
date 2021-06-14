using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_medal_globals", Tag = "gmeg", Size = 0x18)]
    public class GameMedalGlobals : TagStructure
    {
        public List<GamemedalTiers> Tiers;
        public List<GameMedalBlock> Medals;
        
        [TagStructure(Size = 0xC)]
        public class GamemedalTiers : TagStructure
        {
            public StringId Name;
            public StringId Description;
            public short SequenceIndex;
            public short PointValue;
        }
        
        [TagStructure(Size = 0x14)]
        public class GameMedalBlock : TagStructure
        {
            public StringId Name;
            public StringId Description;
            public short SequenceIndex;
            public MedalClassEnum MedalClass;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // The number of ordnance points that are awarded to the player when they earn this medal.
            public int OrdnancePoints;
            // The ordnance multiplier to add to players ordnance multiplier when they earn this medal.
            public byte OrdnanceMultiplier;
            // the tier that this medal belongs to
            public byte TierIndex;
            // if greater than zero, this point value will be awarded to players instead of the tier's point value
            public short OverridePointValue;
            
            public enum MedalClassEnum : sbyte
            {
                Special,
                RoleSpree,
                Spree,
                Multikill,
                Objectives,
                Circumstance,
                Finesse
            }
        }
    }
}
