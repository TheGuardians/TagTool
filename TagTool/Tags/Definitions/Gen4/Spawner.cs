using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "spawner", Tag = "spnr", Size = 0xC)]
    public class Spawner : Entity
    {
        public SpawnerFlags SpawnerFlags1;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public int PostSpawnCooldown;
        // Priority of task to activate this spawner.
        public float ActivationTaskPriority;
        
        [Flags]
        public enum SpawnerFlags : byte
        {
            CooldownWaitsForObjectDeath = 1 << 0
        }
    }
}
