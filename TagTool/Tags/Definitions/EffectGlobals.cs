using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect_globals", Tag = "effg", Size = 0xC)]
    public class EffectGlobals : TagStructure
	{
        public List<HoldbackBlock> Holdbacks;

        [TagStructure(Size = 0x14)]
        public class HoldbackBlock : TagStructure
		{
            public EffectHoldbackTypeEnum HoldbackType;

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

            public uint OverallBudget;
            public List<PriorityBlock> PriorityLevels;

            [TagStructure(Size = 0x10)]
            public class PriorityBlock : TagStructure
			{
                public GlobalEffectPriorityEnum Type;
                public uint AbsoluteCount;
                public uint RelativePercentage;
                public uint CountAvailableAtThisPriority;

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
    }
}
