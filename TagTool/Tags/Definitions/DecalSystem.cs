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
        public FlagsValue Flags;
        public int MaxOverlapping; // 0= no limit
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float OverlappingThreshold;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaterialShaderFadeTime;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int Unknown1; // flags for the tag below?
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown2; // delay for the tag below?
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag Unknown3;

        public Bounds<float> DistanceFadeRange;

        public List<DecalDefinitionBlock> Decal;
        public float RuntimeMaxRadius;

        [Flags]
        public enum FlagsValue : int
        {
            RandomRotation = 1 << 0,
            RandomUMirror = 1 << 1,
            RandomVMirror = 1 << 2,
            ForceQuaduseWithCare = 1 << 3,
            ForcePlanar = 1 << 4,
            RestrictToSingleMaterial = 1 << 5,
            UsePrimaryCollisionOnly = 1 << 6,
            DontCollideWithStructure = 1 << 7,
            DontCollideWithInstances = 1 << 8
        }

        [TagStructure(Size = 0x74, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x98, MinVersion = CacheVersion.HaloReach)]
        public class DecalDefinitionBlock : TagStructure
		{
            public StringId DecalName;
            public FlagsValue Flags;

            public RenderMethod RenderMethod;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown1;

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
            public enum FlagsValue : uint
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
