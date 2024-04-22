using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "silent_assist_globals", Tag = "slag", Size = 0xC)]
    public class SilentAssistGlobals : TagStructure
    {
        public List<SilentassistLevelBlock> Levels;
        
        [TagStructure(Size = 0x28)]
        public class SilentassistLevelBlock : TagStructure
        {
            public float GrenadeDamage;
            public float GrenadeRadius;
            // multiplier for attributed-damage during assist calculation
            public float AssistAwardBias;
            public float WeaponSpread;
            // increases autoaim for headshot weapons
            public float HeadshotAimAssist;
            public float MeleeCone;
            // unimplemented
            public float AimAssist;
            public GSilentassistFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int KillsToDecreaseLevel;
            public int DeathsToIncreaseLevel;
            
            [Flags]
            public enum GSilentassistFlags : byte
            {
                KillsMustBeConsecutive = 1 << 0,
                DeathsMustBeConsecutive = 1 << 1
            }
        }
    }
}
