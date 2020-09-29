using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "point_physics", Tag = "pphy", Size = 0x40)]
    public class PointPhysics : TagStructure
    {
        public FlagsValue Flags;
        [TagField(Flags = Padding, Length = 28)]
        public byte[] Padding1;
        public float Density; // g/mL
        public float AirFriction;
        public float WaterFriction;
        public float SurfaceFriction; // when hitting the ground or interior, percentage of velocity lost in one collision
        public float Elasticity; // 0.0 is inelastic collisions (no bounce) 1.0 is perfectly elastic (reflected velocity equals incoming velocity)
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding2;
        /// <summary>
        /// Densities (g/mL)
        /// </summary>
        /// <remarks>
        /// air        0.0011 (g/mL)
        /// snow       0.128
        /// cork       0.24
        /// cedar      0.43
        /// oak        0.866
        /// ice        0.897
        /// water      1.0
        /// soil       1.1
        /// cotton     1.491
        /// dry earth  1.52
        /// sand       1.7
        /// granite    2.4
        /// glass      2.5
        /// iron       7.65
        /// steel      7.77
        /// lead       11.37
        /// uranium    18.74
        /// gold       19.3
        /// 
        /// </remarks>
        
        [Flags]
        public enum FlagsValue : uint
        {
            Unused = 1 << 0,
            CollidesWithStructures = 1 << 1,
            CollidesWithWaterSurface = 1 << 2,
            UsesSimpleWind = 1 << 3,
            UsesDampedWind = 1 << 4,
            NoGravity = 1 << 5
        }
    }
}

