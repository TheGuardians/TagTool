using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "new_cinematic_lighting", Tag = "nclt", Size = 0x1C, MinVersion = CacheVersion.Halo3Retail)]
    public class NewCinematicLighting : TagStructure
	{
        public List<CinematicShLightBlock> ShLights;
        public List<CinematicDynamicLightBlock> DynamicLights;
        public float EnvironmentalLightingScale;

        [TagStructure(Size = 0x20)]
        public class CinematicShLightBlock : TagStructure
        {
            public CinematicShLightFlags Flags;
            public float Direction; // degrees
            public float FrontBack; // degrees
            public float Intensity;
            public RealRgbColor Color;
            public float Diffusion;

            [Flags]
            public enum CinematicShLightFlags : uint
            {
                None,
                DebugThisLight = 1 << 0
            }
        }

        [TagStructure(Size = 0x20)]
        public class CinematicDynamicLightBlock : TagStructure
        {
            public CinematicDynamicLightFlags Flags;
            public float Direction;
            public float FrontBack;
            public float Distance; // world units
            public CachedTag Light;

            [Flags]
            public enum CinematicDynamicLightFlags : uint
            {
                None,
                DebugThisLight = 1 << 0,
                FollowObject = 1 << 1,
                PositionAtMarker = 1 << 2
            }
        }
    }
}