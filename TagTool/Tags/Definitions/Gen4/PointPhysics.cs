using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "point_physics", Tag = "pphy", Size = 0x40)]
    public class PointPhysics : TagStructure
    {
        public PointPhysicsDefinitionFlags Flags;
        public float RuntimeMassOverRadiusCubed;
        public float RuntimeInverseDensity;
        public int Ignore1;
        public int Ignore2;
        public int Ignore3;
        public int Ignore4;
        public int Ignore5;
        public float Density; // g/mL
        public float AirFriction;
        public float WaterFriction;
        // when hitting the ground or interior, percentage of velocity lost in one collision
        public float SurfaceFriction;
        // 0.0 is inelastic collisions (no bounce) 1.0 is perfectly elastic (reflected velocity equals incoming velocity)
        public float Elasticity;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [Flags]
        public enum PointPhysicsDefinitionFlags : uint
        {
            Unused = 1 << 0,
            CollidesWithStructures = 1 << 1,
            CollidesWithWaterSurface = 1 << 2,
            // the wind on this point won't have high-frequency variations
            UsesSimpleWind = 1 << 3,
            // the wind on this point will be artificially slow
            UsesDampedWind = 1 << 4,
            // the point is not affected by gravity
            NoGravity = 1 << 5
        }
    }
}
