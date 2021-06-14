using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "airstrike", Tag = "airs", Size = 0xC)]
    public class Airstrike : TagStructure
    {
        public List<AirstrikeBatteryBlock> Batteries;
        
        [TagStructure(Size = 0x48)]
        public class AirstrikeBatteryBlock : TagStructure
        {
            // each shot will be fired with a random offset in this radius in the x/y plane above the target location
            public float LaunchRadius; // wu
            // strike will be launched at this plane height above the target location
            public float LaunchZHeight; // wu
            // seconds to wait before launching the strike
            public float LaunchWarmup; // seconds
            // seconds to wait until the launch site marker is removed
            public float LaunchArrival; // seconds
            // seconds to wait before the next launch will be ready
            public float LaunchCooldown; // seconds
            // how long a launch should take to fire all rounds
            public float LaunchDuration; // seconds
            // number of rounds to fire per launch
            public int ShotsPerLaunch;
            [TagField(ValidTags = new [] { "effe" })]
            // the effect that will be created when the strike fires.
            public CachedTag FireEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // the effect that will be created when the strike fires and is indoors
            public CachedTag FireEffect1;
            public List<AirstrikeFireLocationBlock> FireOffsets;
            
            [TagStructure(Size = 0x8)]
            public class AirstrikeFireLocationBlock : TagStructure
            {
                public RealPoint2d Offset;
            }
        }
    }
}
