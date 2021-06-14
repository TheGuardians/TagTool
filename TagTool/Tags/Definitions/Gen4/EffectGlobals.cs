using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "effect_globals", Tag = "effg", Size = 0x40)]
    public class EffectGlobals : TagStructure
    {
        public List<EffectComponentHoldbacksBlock> Holdbacks;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag GruntBirthdayEffect;
        public List<EffectGlobalSpawnEffectsBlock> Multiplayer;
        public List<EffectGlobalSpawnEffectsBlock> Survival;
        public List<EffectGlobalSpawnEffectsBlock> Campaign;
        
        [TagStructure(Size = 0x14)]
        public class EffectComponentHoldbacksBlock : TagStructure
        {
            public EffectHoldbackTypeEnum HoldbackType;
            // from code
            public uint OverallBudget;
            public List<EffectComponentHoldbackBlock> Priorities;
            
            public enum EffectHoldbackTypeEnum : int
            {
                TypeEffect,
                TypeEvent,
                TypeLocation,
                TypeLightprobe,
                TypeEffectMessage,
                TracerSystem,
                TracerLocation,
                TracerSpawned,
                TracerStateless,
                TracerSpawnedProfileRow,
                TracerStatelessProfileRow,
                TypeDecalSystem,
                TypeDecal,
                TypeDecalVertex,
                TypeDecalIndex,
                TypeLightVolumeSystem,
                TypeLightVolumeLocation,
                TypeLightVolume,
                TypeLightVolumeProfileRow,
                TypeParticleSystem,
                TypeParticleLocation,
                TypeParticleEmitter,
                TypeCpuParticle,
                TypeGpuParticleRow,
                TypeParticleQueue,
                TracerQueue
            }
            
            [TagStructure(Size = 0x10)]
            public class EffectComponentHoldbackBlock : TagStructure
            {
                public GlobalEffectPriorityEnum PriorityType;
                public uint AbsoluteCount;
                public float RelativePercentage; // / 100
                public uint HowManyAvailableAtThisPriority;
                
                public enum GlobalEffectPriorityEnum : int
                {
                    Low,
                    Normal,
                    AboveNormal,
                    High,
                    VeryHigh,
                    Essential
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class EffectGlobalSpawnEffectsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag BipedSpawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag VehicleSpawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag WeaponSpawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag FirstPersonPlayerSpawnEffect;
        }
    }
}
