using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_performance_throttle", Tag = "gptd", Size = 0xC)]
    public class GamePerformanceThrottle : TagStructure
    {
        public List<GamePerformanceThrottleEntriesBlock> Entries;
        
        [TagStructure(Size = 0x20)]
        public class GamePerformanceThrottleEntriesBlock : TagStructure
        {
            public GamePerformanceThrottleFilterStruct Filter;
            public GamePerformanceThrottleProfileStruct Profile;
            
            [TagStructure(Size = 0x4)]
            public class GamePerformanceThrottleFilterStruct : TagStructure
            {
                public int MinimumPlayerCount;
            }
            
            [TagStructure(Size = 0x1C)]
            public class GamePerformanceThrottleProfileStruct : TagStructure
            {
                public int MaximumHavokProxyCount;
                public int MaximumRagdollCount;
                public int MaximumImpactCount;
                public int VehicleSuspensionUpdateFrequency;
                public int ActorLodAiActorsToUpdateFullyEachFrame;
                public int ActorLodNumberOfFramesToTickLodedAi;
                public int ActorLodNumberOfConcurrentLodActorsToTick;
            }
        }
    }
}
