using TagTool.Common;
using System.Collections.Generic;
using System;
using System.IO;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decal_system", Tag = "decs", Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "decal_system", Tag = "decs", Size = 0x3C, MinVersion = CacheVersion.HaloReach)]
    public class DecalSystem : TagStructure
	{
        public DecalSystemFlags Flags;
        public int MaxOverlapping; // 0= no limit
        public float OverlappingThreshold;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int ChainUpgradeThreshold; // 0= no upgrade, how many of this will upgrade
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ChainUpgradeCheckingRadius;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag NextDecalChain;

        public Bounds<float> DistanceFadeRange;

        public List<DecalDefinitionBlock> Decal;
        public float RuntimeMaxRadius;

        [Flags]
        public enum DecalSystemFlags : int
        {
            RandomRotation = 1 << 0,
            RandomUMirror = 1 << 1,
            RandomVMirror = 1 << 2,
            ForceQuaduseWithCare = 1 << 3,
            ForcePlanar = 1 << 4,
            RestrictToSingleMaterial = 1 << 5,
            UsePrimaryCollisionOnly = 1 << 6,
            DontCollideWithStructure = 1 << 7,
            DontCollideWithInstances = 1 << 8,
            UsingRelativeOverlappingRadius = 1 << 9,
            RespectsNegativeHorizontalScale = 1 << 10,
            RespectsNegativeVerticalScale = 1 << 11
        }

        [TagStructure(Size = 0x74, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x98, MinVersion = CacheVersion.HaloReach)]
        public class DecalDefinitionBlock : TagStructure
		{
            public StringId DecalName;
            public DecalFlags Flags;

            public RenderMethod RenderMethod;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint SpecularModulate;

            [TagField(Format = "World Units")]
            public Bounds<float> Radius;
            [TagField(Format = "Seconds")]
            public Bounds<float> DecayTime;
            [TagField(Format = "Seconds")]
            public Bounds<float> Lifespan;

            /// <summary>
            /// Projections at greater than this angle will be clamped to this angle
            /// </summary>
            [TagField(Format = "Degrees")]
            public float ClampAngle;

            /// <summary>
            /// Projections at greater than this angle will not be drawn
            /// </summary>
            [TagField(Format = "Degrees")]
            public float CullAngle;

            public DecalPass Pass;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float RuntimeSpecularMultiplier;
            public float RuntimeBitmapAspect;

            [Flags]
            public enum DecalFlags : uint
            {
                None = 0,
                SpecularModulate = 1 << 0,
                BumpModulate = 1 << 1,
                RandomSpriteSequence = 1 << 2,
                AdditiveBlendMode = 1 << 3
            }

            public enum DecalPass : uint
            {
                PreLighting,
                PostLighting
            }
        }
    }
}
