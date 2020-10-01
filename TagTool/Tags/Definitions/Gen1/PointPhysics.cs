using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "point_physics", Tag = "pphy", Size = 0x40)]
    public class PointPhysics : TagStructure
    {
        public FlagsValue Flags;
        [TagField(Length = 0x1C)]
        public byte[] Padding;
        public float Density; // g/mL
        public float AirFriction;
        public float WaterFriction;
        /// <summary>
        /// when hitting the ground or interior, percentage of velocity lost in one collision
        /// </summary>
        public float SurfaceFriction;
        /// <summary>
        /// 0.0 is inelastic collisions (no bounce) 1.0 is perfectly elastic (reflected velocity equals incoming velocity)
        /// </summary>
        public float Elasticity;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// air        0.0011 (g/mL)
        /// snow       0.128
        /// cork       0.24
        /// cedar      0.43
        /// oak        0.866
        /// ice        0.897
        /// water     
        /// 1.0
        /// soil       1.1
        /// cotton     1.491
        /// dry earth  1.52
        /// sand       1.7
        /// granite    2.4
        /// glass      2.5
        /// iron       7.65
        /// steel   
        /// 7.77
        /// lead       11.37
        /// uranium    18.74
        /// gold       19.3
        /// 
        /// </summary>
        
        public enum FlagsValue : uint
        {
            FlamethrowerParticleCollision,
            CollidesWithStructures,
            CollidesWithWaterSurface,
            /// <summary>
            /// the wind on this point won't have high-frequency variations
            /// </summary>
            UsesSimpleWind,
            /// <summary>
            /// the wind on this point will be artificially slow
            /// </summary>
            UsesDampedWind,
            /// <summary>
            /// the point is not affected by gravity
            /// </summary>
            NoGravity
        }
    }
}

