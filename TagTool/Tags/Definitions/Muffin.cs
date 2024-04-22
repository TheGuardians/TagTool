using System;
using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "muffin", Tag = "mffn", Size = 0x38)]
    public class Muffin : TagStructure
	{
        public CachedTag RenderModel;
        public List<StringId> RenderModelPermutationNames;
        public int RenderModelPermutationNameValidCount;
        public List<MuffinLocation> MuffinLocations;
        public List<MuffinType> MuffinTypes;

        [TagStructure(Size = 0x8)]
        public class MuffinLocation : TagStructure
		{
            [Flags]
            public enum AllowedMuffinTypesBitfield : ushort
            {
                None,
                Muffin0 = 1 << 0,
                Muffin1 = 1 << 1,
                Muffin2 = 1 << 2,
                Muffin3 = 1 << 3,
                Muffin4 = 1 << 4,
                Muffin5 = 1 << 5,
                Muffin6 = 1 << 6,
                Muffin7 = 1 << 7,
                Muffin8 = 1 << 8,
                Muffin9 = 1 << 9,
                Muffin10 = 1 << 10,
                Muffin11 = 1 << 11,
                Muffin12 = 1 << 12,
                Muffin13 = 1 << 13,
                Muffin14 = 1 << 14,
                Muffin15 = 1 << 15,
            }

            public StringId LocationName;
            public short MuffinageChance;
            public AllowedMuffinTypesBitfield AllowedMuffinTypes;
        }

        [TagStructure(Size = 0x70)]
        public class MuffinType : TagStructure
		{
            public short MeshIndex;
            public MuffinTypeFlags Flags;
            public Bounds<float> SpawnDelay;
            public Bounds<float> InitialScale;
            public TagFunction InitialGrowth = new TagFunction { Data = new byte[0] };
            public float GrowthTime;
            public TagFunction PeriodicNoise = new TagFunction { Data = new byte[0] };
            public float Jiggle;
            public float JiggleSpringLength;
            public float JiggleSpringStrength;
            public float JiggleVelocityDampening;
            public float JiggleMaxVelocity;
            public Bounds<float> Lifetime;
            public float ImmortalityChance;
            public CachedTag Effect;

            [Flags]
            public enum MuffinTypeFlags : ushort
            {
                None,
                DebugDisable = 1 << 0,
                JumpToMarkerOnTransition = 1 << 1,
            }
        }
    }
}