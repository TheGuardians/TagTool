using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "leaf_system", Tag = "lswd", Size = 0x58)]
    public class LeafSystem : TagStructure
    {
        public LeafFlags Flags;
        public StringId MarkerAttachName;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BitmapSpritePlate;
        // seconds
        public Bounds<float> EmissionRate;
        public float Lifetime;
        // default 0,0
        public Bounds<float> WindMinMax;
        public float WindScale;
        public float TimeScale;
        // from maximum movement range (10+). default 0!
        public Bounds<float> FadeDistance;
        public float EmissionsSphereRadius;
        public float MovementCylinderRadius;
        public float FadeInTime;
        public float FadeOutTime;
        public List<LeafTypeBlock> LeafTypes;
        
        [Flags]
        public enum LeafFlags : uint
        {
            CollidesStructure = 1 << 0,
            CollidesObjects = 1 << 1,
            CollidesWater = 1 << 2,
            AffectedByExplosions = 1 << 3
        }
        
        [TagStructure(Size = 0x3C)]
        public class LeafTypeBlock : TagStructure
        {
            // assumes sequence 0. if no sprites uses full bitmap
            public short BitmapSpriteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float Frequency;
            public float Mass;
            // default of 0 for both means 0.1
            public Bounds<float> GeometryScale;
            public float Flitteriness;
            public float FlitterinessSwingArmLength;
            public float FlitterinessScale;
            public float FlitterinessSpeed;
            public float FlitterinessLeavesPhase;
            public float TumbleScale;
            public float RotationScale;
            public float StartingVelocity;
            public Bounds<float> AirFrictionXyAndZ;
        }
    }
}
