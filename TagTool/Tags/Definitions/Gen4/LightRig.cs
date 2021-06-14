using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "light_rig", Tag = "lrig", Size = 0x40)]
    public class LightRig : TagStructure
    {
        public float EnvironmentVmfLobeWeight;
        public RealRgbColor RigFillColor;
        public float RigFillScale;
        public List<DirectionallightRigBlock> DirectionalLightRigs;
        public RealRgbColor RigVmfLobeColor;
        public float RigVmfLobeIntensity;
        public float RigVmfLobeTheta;
        public float RigVmfLobePhi;
        public float RigVmfLobeScale;
        public float RigVmfLobeVsFillScale;
        
        [TagStructure(Size = 0x20)]
        public class DirectionallightRigBlock : TagStructure
        {
            public LightrigLocation Location;
            public float Theta;
            public float Phi;
            public float DistanceFromLocation;
            [TagField(ValidTags = new [] { "ligh" })]
            public CachedTag MidnightLight;
            
            [Flags]
            public enum LightrigLocation : uint
            {
                FollowObject = 1 << 0,
                PositionAtMarker = 1 << 1
            }
        }
    }
}
