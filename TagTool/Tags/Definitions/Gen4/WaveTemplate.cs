using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "wave_template", Tag = "wave", Size = 0xC)]
    public class WaveTemplate : TagStructure
    {
        public List<WaveSquadSpecificationStructStruct> SquadSpecifications;
        
        [TagStructure(Size = 0x1C)]
        public class WaveSquadSpecificationStructStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "sqtm" })]
            public CachedTag SquadTemplate;
            public AiSpawnConditionsStruct SpawnConditions;
            // The relative weight given to this squad spawning
            public short Weight;
            // Spawn AT LEAST this number of squads. Value of 0 means "no minimum"
            public sbyte MinSpawn;
            // Spawn NO MORE THAN this number of squads. Value of 0 means "no maximum"
            public sbyte MaxSpawn;
            // Filter where this squad specification can spawn by matching this value with the values in squad definitions in the
            // scenario
            public WavePlacementFilterEnum PlacementFilter;
            
            public enum WavePlacementFilterEnum : int
            {
                None,
                HeavyInfantry,
                BossInfantry,
                LightVehicle,
                HeavyVehicle,
                FlyingInfantry,
                FlyingVehicle,
                Bonus
            }
            
            [TagStructure(Size = 0x4)]
            public class AiSpawnConditionsStruct : TagStructure
            {
                public GlobalCampaignDifficultyEnum DifficultyFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GlobalCampaignDifficultyEnum : ushort
                {
                    Easy = 1 << 0,
                    Normal = 1 << 1,
                    Heroic = 1 << 2,
                    Legendary = 1 << 3
                }
            }
        }
    }
}
