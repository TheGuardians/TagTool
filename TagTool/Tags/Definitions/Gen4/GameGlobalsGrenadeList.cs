using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_globals_grenade_list", Tag = "gggl", Size = 0xC)]
    public class GameGlobalsGrenadeList : TagStructure
    {
        public List<GameGlobalsGrenadeBlock> Grenades;
        
        [TagStructure(Size = 0x78)]
        public class GameGlobalsGrenadeBlock : TagStructure
        {
            public short MaximumCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short InitialCount;
            public short InitialCount1;
            public short InitialCount2;
            public short GrenadierExtraCount;
            public short GrenadierExtraCount1;
            public short GrenadierExtraCount2;
            public float DropPercentage;
            public float DropPercentage1;
            public float DropPercentage2;
            public float ResourcefulScavengePercentage;
            public float ResourcefulScavengePercentage1;
            public float ResourcefulScavengePercentage2;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ThrowingEffect;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Equipment;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Equipment1;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile1;
        }
    }
}
