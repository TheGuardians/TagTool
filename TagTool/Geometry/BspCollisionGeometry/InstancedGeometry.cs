using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0xC4, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0xB8, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
    public class InstancedGeometryBlock : TagStructure
    {
        public int Checksum;
        public RealPoint3d BoundingSphereOffset;
        public float BoundingSphereRadius;
        public CollisionGeometry CollisionInfo;
        public TagBlock<CollisionGeometry> CollisionGeometries;
        public TagBlock<TagHkpMoppCode> CollisionMoppCodes;
        public TagBlock<BreakableSurfaceBits> BreakableSurfaces;

        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown1;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown2;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown3;

        public TagBlock<SurfacesPlanes> SurfacePlanes;
        public TagBlock<PlaneReference> Planes;


        public short MeshIndex;
        public short CompressionIndex;
        public float GlobalLightmapResolutionScale;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagBlock<TagHkpMoppCode> UnknownBspPhysics;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint RuntimePointer;
    }

    // TODO: maybe merge into the above class when all fields are known
    [TagStructure(Size = 0x144, MinVersion = CacheVersion.HaloReach)]
    public class InstancedGeometryBlockReach : TagStructure
    {
        public int Checksum;
        public uint Unknown1;
        public CollisionGeometry CollisionInfo;
        public LargeCollisionBspBlock LargeCollisionInfo;
        public TagBlock<CollisionGeometry> CollisionGeometries;
        public TagBlock<Unknown2Block> Unknown2;
        public TagBlock<TagHkpMoppCode> CollisionMoppCodes;
        public TagBlock<BreakableSurfaceBits> BreakableSurfaces;
        public TagBlock<PolyhedronBlock> Polyhedra;
        public TagBlock<PolyhedronFourVector> PolyhedraFourVectors;
        public TagBlock<PolyhedronPlaneEquation> PolyhedraPlaneEquations;
        public TagBlock<SurfacesPlanes> SurfacePlanes;
        public TagBlock<PlaneReferenceReach> Planes;
        public short MeshIndex;
        public short CompressionIndex;

        [TagStructure(Size = 0x0)]
        public class UnknownBlock : TagStructure
        {
            // TODO
        }

        [TagStructure(Size = 0x4)] // might not be the correct size
        public class Unknown2Block : TagStructure
        {
            public uint Unknown1;
        }

        [TagStructure(Size = 0x80)]
        public class PolyhedronBlock : TagStructure
        {
            public uint VTableAddress;
            public HkpReferencedObject ReferencedObject;
            public uint UserDataAddress;
            public int Type;
            public float Radius;
            [TagField(Align = 16)]
            public RealQuaternion AabbHalfExtents;
            public RealQuaternion AabbCenter;
            public HkArrayBase FourVectors;
            public uint NumVertices;
            public uint UseSpuBuffer;
            public HkArrayBase PlaneEquations;
            public uint Connectivity;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
        }

        [TagStructure(Size = 0x30, Align = 0x10)]
        public class PolyhedronFourVector : TagStructure
        {
            public RealVector3d FourVectorsX;
            public float FourVectorsXRadius;
            public RealVector3d FourVectorsY;
            public float FourVectorsYRadius;
            public RealVector3d FourVectorsZ;
            public float FourVectorsZRadius;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class PolyhedronPlaneEquation : TagStructure
        {
            public RealPlane3d PlaneEquation;
        }

        [TagStructure(Size = 0x10)]
        public class PlaneReferenceReach : TagStructure
        {
            // maybe kd hash data?
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
        }
    }

    [TagStructure(Size = 0x9C)]
    public class InstancedGeometryInstanceReach : TagStructure
    {
        public float Scale;
        public RealMatrix4x3 Matrix;
        public short DefinitionIndex;
        public FlagsValue Flags;
        public short LodDataIndex;
        public short CompressionIndex;
        [TagField(Length = 4)]
        public uint[] SeamBitVector;
        public RealRectangle3d Bounds;
        public RealPoint3d WorldBoundingSphereCenter;
        public Bounds<float> BoundingSphereRadiusBounds;
        public uint Checksum;
        public sbyte PathfindingPolicy;
        public sbyte LightmappingPolicy;
        public sbyte ImposterPolicy;
        public sbyte StreamingPriority;
        public float LightmapResolutionScale;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public float Unknown10;
        public RealVector3d Unknown11;

        [Flags]
        public enum FlagsValue : ushort
        {
            None,
            ContainsSplitLightingParts = 1 << 0,
            RenderOnly = 1 << 1,
            DoesNotBlockAoeDamage = 1 << 2,
            Collidable = 1 << 3,
            ContainsDecalParts = 1 << 4,
            ContainsWaterParts = 1 << 5,
            NegativeScale = 1 << 6,
            OutsideMap = 1 << 7,
            SeamColliding = 1 << 8,
            ContainsDeferredReflections = 1 << 9,
            RemoveFromShadowGeometry = 1 << 10,
            CinemaOnly = 1 << 11,
            ExcludeFromCinema = 1 << 12,
            DisableFX = 1 << 13,
            DisablePlayCollision = 1 << 14,
            DisableBulletCollision = 1 << 15
        }
    }
}
