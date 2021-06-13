using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "effect_global_force", Tag = "egfd", Size = 0x10)]
    public class EffectGlobalForce : TagStructure
    {
        public GlobalforceFlags Flags;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // positive pushes out, negative pulls in
        public float ForceStrength;
        // doesn't work on cylinders, due to shader constant constraints
        public float SphereFalloffBegin; // wus
        // doesn't work on cylinders, due to shader constant constraints
        public float SphereFalloffEnd; // wus
        
        [Flags]
        public enum GlobalforceFlags : byte
        {
            IsInfinitelyLongCylinder = 1 << 0
        }
    }
}
