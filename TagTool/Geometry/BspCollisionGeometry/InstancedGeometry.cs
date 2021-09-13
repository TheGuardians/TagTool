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
    [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x144, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    public class InstancedGeometryBlock : TagStructure
    {
        public int Checksum;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryDefinitionFlags Flags; // Taken from h4, might not be correct

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public RealPoint3d BoundingSphereOffset;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float BoundingSphereRadius;

        public CollisionGeometry CollisionInfo;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CollisionGeometry Unused1; // type 2 - unused
        public TagBlock<CollisionGeometry> CollisionGeometries;

        public TagBlock<TagHkpMoppCode> CollisionMoppCodes;
        public TagBlock<BreakableSurfaceBits> BreakableSurfaces;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronBlock> Polyhedra;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronFourVector> PolyhedraFourVectors;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronPlaneEquation> PolyhedraPlaneEquations;

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

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float GlobalLightmapResolutionScale;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public TagBlock<TagHkpMoppCode> UnknownBspPhysics;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint RuntimePointer;

        [Flags]
        public enum InstancedGeometryDefinitionFlags : uint
        {
            MiscoloredBsp = 1 << 0,
            ErrorFree = 1 << 1,
            SurfaceToTriangleRemapped = 1 << 2,
            ExternalReferenceMesh = 1 << 3,
            NoPhysics = 1 << 4,
            StitchedPhysics = 1 << 5
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
    }

    [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x74, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x9C, MinVersion = CacheVersion.HaloReach)]
    public class InstancedGeometryInstance : TagStructure
    {
        public float Scale;
        public RealMatrix4x3 Matrix;
        public short DefinitionIndex;
        public FlagsValue Flags;
        public short LodDataIndex;
        public short CompressionIndex;
        [TagField(Length = 1, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(Length = 4, MinVersion = CacheVersion.HaloReach)]
        public uint[] SeamBitVector;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealRectangle3d Bounds;

        public RealPoint3d WorldBoundingSphereCenter;
        public Bounds<float> BoundingSphereRadiusBounds;

        [TagField(Flags = TagFieldFlags.Label, MaxVersion = CacheVersion.HaloOnline700123)]
        public StringId Name;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint ChecksumReach;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public Scenery.PathfindingPolicyValue PathfindingPolicy;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public Scenery.LightmappingPolicyValue LightmappingPolicy;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte PathfindingPolicyReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte LightmappingPolicyReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte ImposterPolicy;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Unknown1;

        public float LightmapResolutionScale;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CollisionBspPhysicsDefinition> BspPhysics;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public short GroupIndex;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public short GroupListIndex;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public MeshFlags MeshOverrideFlags;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public short Unknown7;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public uint Unknown8;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown13;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<CollisionBspPhysicsDefinition> BspPhysicsReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Unknown17a;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Unknown17b;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Unknown17c;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Unknown17d;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown18a;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown18b;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown19;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealVector3d Unknown20;

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
