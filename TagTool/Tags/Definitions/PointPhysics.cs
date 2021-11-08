using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "point_physics", Tag = "pphy", Size = 0x40)]
    public class PointPhysics : TagStructure
	{
        public PointPhysicsFlags Flags;
        public float RuntimeMassOverRadiusCubed;
        public float RuntimeInverseDensity;
        public float Padding0;

        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding1;

        public float Density; // g/mL
        public float AirFriction;
        public float WaterFriction;
        public float SurfaceFriction; // when hitting the ground or interior, percentage of velocity lost in one collision
        public float Elasticity; // 0.0 is inelastic collisions (no bounce) 1.0 is perfectly elastic (reflected velocity equals incoming velocity)

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding2;

        [Flags]
        public enum PointPhysicsFlags : int
        {
            None,
            Unused = 1 << 0,
            CollidesWithStructures = 1 << 1,
            CollidesWithWaterSurface = 1 << 2,
            UsesSimpleWind = 1 << 3, // the wind on this point won't have high-frequency variations
            UsesDampedWind = 1 << 4, // the wind on this point will be artificially slow
            NoGravity = 1 << 5 // the point is not affected by gravity
        }
    }
}
