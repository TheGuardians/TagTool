using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "material_physics", Tag = "mpdt", Size = 0x14)]
    public class MaterialPhysics : TagStructure
    {
        /// <summary>
        /// the following fields modify the way a vehicle drives over terrain of this material type.
        /// </summary>
        /// <summary>
        /// fraction of original velocity parallel to the ground after one tick
        /// </summary>
        public float GroundFrictionScale;
        /// <summary>
        /// cosine of angle at which friction falls off
        /// </summary>
        public float GroundFrictionNormalK1Scale;
        /// <summary>
        /// cosine of angle at which friction is zero
        /// </summary>
        public float GroundFrictionNormalK0Scale;
        /// <summary>
        /// depth a point mass rests in the ground
        /// </summary>
        public float GroundDepthScale;
        /// <summary>
        /// fraction of original velocity perpendicular to the ground after one tick
        /// </summary>
        public float GroundDampFractionScale;
    }
}

