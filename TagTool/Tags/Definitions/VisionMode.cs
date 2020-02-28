using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x188, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x1A0, MinVersion = CacheVersion.HaloOnline106708)]
    public class VisionMode : TagStructure
    {
        public sbyte Unknown1;
        public sbyte Unknown2;
        public sbyte Flags;
        public sbyte Unknown3;

        public RenderDataBlock ParentRenderData;
        public RenderDataBlock ChildRenderData;
        public RenderDataBlock TheaterRenderData;

        public CachedTag AnimationSound;

        public OutlineDataBlock WeaponOutlineData;
        public OutlineDataBlock AllyOutlineData;
        public OutlineDataBlock EnemyOutlineData;
        public OutlineDataBlock ObjectiveOutlineData;
        public OutlineDataBlock SceneryOutlineData;

        public CachedTag VisionModeMask;
        public CachedTag VisionModeCameraFx;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public class RenderDataBlock : TagStructure
        {
            public float PrimaryRenderDistance;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float SecondaryRenderDistance;
            public float AnimationSpeed;
            public float AnimationTimeModifier;
            public float Unknown;
            public float AnimationDelayBySeconds;
        }

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline106708)]
        public class OutlineDataBlock : TagStructure
        {
            public TagFunction PrimaryColourFunction;
            public TagFunction SecondaryColourFunction;
            public float FillIntensity;
            public float PrimaryColourInfluence;
            public float SecondaryColourInfluence;
            public float OutlineIntensity;
        }
    }
}