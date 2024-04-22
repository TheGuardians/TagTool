using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "SpawnSettings", Tag = "ssdf", Size = 0x13C)]
    public class SpawnSettingsDefinition : TagStructure
    {
        // Absolute floor.  Used to put a few-frame delay between death and instaspawn.  Will not override longer minimum
        // times.
        public float MinimumSpawnTime; // seconds
        // spawns with a score lower than this will never be used
        public float MinAcceptableSpawnScore;
        // Controls how large of a random value gets added to each spawn point
        public float MaximumRandomSpawnBias; // 0 disables
        // Example -
        // On a map with 140 respawn points, a value of 0.1 here
        // will choose randomly between up to the best 14 points.
        // Number of points considered is also limited by the following parameters and flags.
        // If next two parameters are 0, only points with the exact same weight as the highest
        // scoring respawn point will be considered.  If 'Min absolute weight range' is 25 then
        // the best 'X' points within 25 points of the highest scoring point will be considered.
        public float NumSpawnPointsToUse; // 0 uses only best point
        // Example -
        // Value of 0.25 will consider all points with weight within 25% of the highest scoring point.
        // Note - At run-time, the larger of '% best weight to use' and 'Min absolute weight range' will be used.
        // For instance if '% best weight' is 0.1 and 'Min absolute weight' is 5.0' a max weight
        // of 150 will use 15, but a max weight of 10 will fall back on the min weight of 5
        public float BestWeightToUse; // 0 disables
        // Example -
        // Value of 50 will consider all points within 50 of the highest scoring point.
        // Note - At run-time, the larger of '% best weight to use' and 'Min absolute weight range' will be used.
        public float MinAbsoluteWeightRange; // 0 disables
        public SpawnSettingsFlags Flags;
        public List<InfluencerspawnSettingsBlock> SpawnSettings;
        public float EfFullWeightRadius; // wu
        public float EfFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> EfFalloffFunction;
        public float EfUpperHeight; // wu
        public float EfLowerHeight; // wu
        public float EfWeight;
        public float EbFullWeightRadius; // wu
        public float EbFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> EbFalloffFunction;
        public float EbUpperHeight; // wu
        public float EbLowerHeight; // wu
        public float EbWeight;
        public float AbFullWeightRadius; // wu
        public float AbFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> AbFalloffFunction;
        public float AbUpperHeight; // wu
        public float AbLowerHeight; // wu
        public float AbWeight;
        public float SabFullWeightRadius; // wu
        public float SabFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> SabFalloffFunction;
        public float SabUpperHeight; // wu
        public float SabLowerHeight; // wu
        public float SabWeight;
        public float DtFullWeightRadius; // wu
        public float DtFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> DtFalloffFunction;
        public float DtUpperHeight; // wu
        public float DtLowerHeight; // wu
        public float DtWeight;
        public float DeadTeammateInfluenceDuration; // seconds
        public float DropPodFullWeightRadius; // wu
        public float DropPodFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> DropPodFalloffFunction;
        public float DropPodUpperHeight; // wu
        public float DropPodLowerHeight; // wu
        public float DropPodWeight;
        public float AutoTurretFullWeightRadius; // wu
        public float AutoTurretFallOffRadius; // wu
        // Multiplier applied to weight (domain is full weight radius to fall-off radius, range should be 0 to 1).
        public List<SpawnInfluenceWeightFalloffFunctionBlock> AutoTurretFalloffFunction;
        public float AutoTurretUpperHeight; // wu
        public float AutoTurretLowerHeight; // wu
        public float AutoTurretWeight;
        public List<WeaponspawnInfluenceBlock> WeaponInfluencers;
        public List<VehiclespawnInfluenceBlock> VehicleInfluencers;
        public List<ProjectilespawnInfluenceBlock> ProjectileInfluencers;
        public List<EquipmentspawnInfluenceBlock> EquipmentInfluencers;
        
        [Flags]
        public enum SpawnSettingsFlags : uint
        {
            // If checked, negative weighted respawn points are treated the same as ones with positive weight.
            // If not checked, negative respawn points aren't grouped with positive weighted respawn points.
            AllowNegativeWeightsInRandomization = 1 << 0
        }
        
        [TagStructure(Size = 0xC)]
        public class InfluencerspawnSettingsBlock : TagStructure
        {
            public InfluencerSpawnSettingsFlags Flags;
            public float MinimumInfluence; // Only used if 'Pin' flag is set
            public float MaximumInfluence; // Only used if 'Pin' flag is set
            
            [Flags]
            public enum InfluencerSpawnSettingsFlags : uint
            {
                OnlyUseLargestInfluence = 1 << 0,
                PinInfluenceToMinimumAndMaximum = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class SpawnInfluenceWeightFalloffFunctionBlock : TagStructure
        {
            public ScalarFunctionNamedStruct Function;
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class WeaponspawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            public float FullWeightRange; // wu
            public float FallOffRange; // wu
            public float FallOffConeRadius; // wu
            public float Weight;
        }
        
        [TagStructure(Size = 0x28)]
        public class VehiclespawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Vehicle;
            public float BoxWidth; // wu
            // How far influencer will extend below unit's origin.  Will usually be negative.  (If min and max are both 0, unit
            // radius is used)
            public float BoxMinHeight; // wu
            // How far influencer will extend above unit's origin.  Will usually be positive.  (If min and max are both 0, unit
            // radius is used)
            public float BoxMaxHeight; // wu
            public float LeadTime; // seconds
            public float MinimumVelocity; // wu/sec
            public float Weight;
        }
        
        [TagStructure(Size = 0x1C)]
        public class ProjectilespawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
            public float LeadTime; // seconds
            public float CollisionCylinderRadius; // wu
            public float Weight;
        }
        
        [TagStructure(Size = 0x14)]
        public class EquipmentspawnInfluenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Equipment;
            public float Weight;
        }
    }
}
