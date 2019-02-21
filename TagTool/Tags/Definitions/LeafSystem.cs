using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "leaf_system", Tag = "lswd", Size = 0x58)]
    public class LeafSystem : TagStructure
	{
        public FlagBits Flags;
        public StringId MarkerAttachName;
        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTagInstance BitmapSpritePlate;
        public Bounds<float> EmissionRate;
        public float Lifetime;
        public Bounds<float> WindBounds;
        public float WindScale;
        public float TimeScale;
        public Bounds<float> FadeDistance;
        public float EmissionSphrereRadius;
        public float MovementCylinderRadius;
        public float FadeInTime;
        public float FadeOutTime;
        public List<LeafType> LeafTypes;

        [Flags]
        public enum FlagBits : int
        {
            None,
            CollidesStructure = 1 << 0,
            CollidesObjects = 1 << 1,
            CollidesWater = 1 << 2,
            AffectedByExplosions = 1 << 3
        }

        [TagStructure(Size = 0x3C)]
        public class LeafType : TagStructure
		{
            public short BitmapSpriteIndex;
            [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Unused;
            public float Frequency;
            public float Mass;
            public Bounds<float> GeometryScale;
            public float Flitteriness;
            public float FlitterinessSwingArmLength;
            public float FlitterinessScale;
            public float FlitterinessSpeed;
            public float FlitterinessLeavesPhase;
            public float TumbleScale;
            public float RotationScale;
            public float StartingVelocity;
            public Bounds<float> AirFrictionBounds;
        }
    }
}