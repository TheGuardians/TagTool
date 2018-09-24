using TagTool.Serialization;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "point_physics", Tag = "pphy", Size = 0x40)]
    public class PointPhysics : TagStructure
	{
        public FlagBits Flags;

        public float RuntimeMassOverRadiusCubed;
        public float RuntimeInverseDensity;

        [TagField(Padding = true, Length = 20)]
        public byte[] Unused1;

        public float Density;
        public float AirFriction;
        public float WaterFriction;
        public float SurfaceFriction;
        public float Elasticity;

        [TagField(Padding = true, Length = 12)]
        public byte[] Unused2;

        [Flags]
        public enum FlagBits : int
        {
            None,
            Unused = 1 << 0,
            CollidesWithStructures = 1 << 1,
            CollidesWithWaterSurface = 1 << 2,
            UsesSimpleWind = 1 << 3,
            UsesDampedWind = 1 << 4,
            NoGravity = 1 << 5
        }
    }
}