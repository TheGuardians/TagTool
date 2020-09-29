using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "material_physics", Tag = "mpdt", Size = 0x14)]
    public class MaterialPhysics : TagStructure
    {
        /// <summary>
        /// vehicle terrain parameters
        /// </summary>
        /// <remarks>
        /// the following fields modify the way a vehicle drives over terrain of this material type.
        /// </remarks>
        public float GroundFrictionScale; // fraction of original velocity parallel to the ground after one tick
        public float GroundFrictionNormalK1Scale; // cosine of angle at which friction falls off
        public float GroundFrictionNormalK0Scale; // cosine of angle at which friction is zero
        public float GroundDepthScale; // depth a point mass rests in the ground
        public float GroundDampFractionScale; // fraction of original velocity perpendicular to the ground after one tick
    }
}

