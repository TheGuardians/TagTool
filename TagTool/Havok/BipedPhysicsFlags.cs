using System;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x4)]
	public class BipedPhysicsFlags : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public Halo3RetailBits Halo3Retail;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Halo3OdstBits Halo3Odst;

        [Flags]
        public enum Halo3RetailBits : int
        {
            None,
            CenteredAtOrigin = 1 << 0,
            ShapeSpherical = 1 << 1,
            UsePlayerPhysics = 1 << 2,
            ClimbAnySurface = 1 << 3,
            Flying = 1 << 4,
            NotPhysical = 1 << 5,
            DeadCharacterCollisionGroup = 1 << 6,
            SuppressGroundPlanesOnBipeds = 1 << 7,
            PhysicalRagdoll = 1 << 8,
            DoNotResizeDeadSpheres = 1 << 9,
            MultipleShapes = 1 << 10,
            ExtremeSlipSurface = 1 << 11,
            SlipsOffMovers = 1 << 12,
            AlignsWithGround = 1 << 13
        }

        [Flags]
        public enum Halo3OdstBits : int
        {
            None,
            CenteredAtOrigin = 1 << 0,
            ShapeSpherical = 1 << 1,
            UsePlayerPhysics = 1 << 2,
            Unknown = 1 << 3,
            ClimbAnySurface = 1 << 4,
            Flying = 1 << 5,
            NotPhysical = 1 << 6,
            DeadCharacterCollisionGroup = 1 << 7,
            SuppressGroundPlanesOnBipeds = 1 << 8,
            PhysicalRagdoll = 1 << 9,
            DoNotResizeDeadSpheres = 1 << 10,
            MultipleShapes = 1 << 11,
            ExtremeSlipSurface = 1 << 12,
            SlipsOffMovers = 1 << 13,
            AlignsWithGround = 1 << 14
        }
    }
}