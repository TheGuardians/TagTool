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
    [TagStructure(Size = 0x144, MinVersion = CacheVersion.HaloReach)]
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
        public TagBlock<CollisionGeometry> RenderBsp;

        public TagBlock<TagHkpMoppCode> CollisionMoppCodes;
        public TagBlock<BreakableSurfaceBits> BreakableSurfaces;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronBlock> Polyhedra;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronFourVector> PolyhedraFourVectors;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<PolyhedronPlaneEquation> PolyhedraPlaneEquations;

        [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        public TagBlock<SmallSurfacesPlanes> SmallSurfacesPlanes;
        public TagBlock<StructureSurface> SurfacePlanes;
        public TagBlock<StructureSurfaceToTriangleMapping> Planes;

        public short MeshIndex;
        public short CompressionIndex;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float GlobalLightmapResolutionScale;

        [TagField(Version = CacheVersion.HaloOnlineED)]
        public TagBlock<ExtraDataBlock> ExtraData;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding, Version = CacheVersion.HaloOnlineED)]
        public byte[] Unused2;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public TagBlock<TagHkpMoppCode> MeshMopp;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint MeshMoppBvTreePointer; // Runtime

        [Flags]
        public enum InstancedGeometryDefinitionFlags : uint
        {
            MiscoloredBsp = 1 << 0,
            ErrorFree = 1 << 1,
            SurfaceToTriangleRemapped = 1 << 2,
            NoPhysics = 1 << 3,
            StitchedPhysics = 1 << 4
        }

        [TagStructure(Size = 0x24)]
        public class ExtraDataBlock : TagStructure
        {
            public TagBlock<PolyhedronBlock> Polyhedra;
            public TagBlock<PolyhedronFourVector> PolyhedraFourVectors;
            public TagBlock<PolyhedronPlaneEquation> PolyhedraPlaneEquations;
        }

        [TagStructure(Size = 0x70, Align = 16, Version = CacheVersion.HaloOnlineED)]
        [TagStructure(Size = 0x80, Align = 16, MinVersion = CacheVersion.HaloReach)]
        public class PolyhedronBlock : TagStructure
        {
            public HkConvexVerticesShape ConvexVerticesShape;
            public int MaterialIndex;
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
    [TagStructure(Size = 0x9C, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0xA0, MinVersion = CacheVersion.HaloReach11883)]
    public class InstancedGeometryInstance : TagStructure
    {
        public float Scale;
        public RealMatrix4x3 Matrix;
        public short DefinitionIndex;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public InstancedGeometryFlags Flags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryFlagsReach FlagsReach;

        public short LightmapTexcoordBlockIndex;
        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Padding = new byte[2];
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
        public InstancedGeometryLightmappingPolicy LightmappingPolicy;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryPathfindingPolicyReach PathfindingPolicyReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryLightmappingPolicyReach LightmappingPolicyReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryImposterPolicy ImposterPolicy;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public InstancedGeometryStreamingpriority Streamingpriority;

        public float LightmapResolutionScale;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SinglePassRenderDistance;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<InstancedGeometryPhysics> BspPhysics;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<InstancedGeometryPhysicsReach> BspPhysicsReach;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public ushort FadePixelsStart;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public ushort FadePixelsEnd;

        public CubemapSampleStruct CubemapSample;

        [TagField(Version = CacheVersion.HaloReach11883)]
        public StringId NameReach;

        [Flags]
        public enum InstancedGeometryFlags : ushort
        {
            NotInLightprobes = 1 << 0,
            RenderOnly = 1 << 1,
            DoesNotBlockAoeDamage = 1 << 2,
            Collidable = 1 << 3,
            DecalSpacing = 1 << 4,   
            // HO + H3 MCC
            Unknown5 = 1 << 5, // probably unused
            NotForRender = 1 << 6, // From eldorado geometry exporter
            // Reach ports
            MiscoloredBsp = 1 << 7,
            NoPhysics = 1 << 8
        }

        [Flags]
        public enum InstancedGeometryFlagsReach : ushort
        {
            NotInLightprobes = 1 << 0,
            RenderOnly = 1 << 1,
            DoesNotBlockAoeDamage = 1 << 2,
            Collidable = 1 << 3,
            DecalSpacing = 1 << 4,
            RainBlocker = 1 << 5,
            VerticalRainSheet = 1 << 6,
            OutsideMap = 1 << 7,
            SeamColliding = 1 << 8,
            MostlyPlanar = 1 << 9
        }

        public enum InstancedGeometryImposterPolicy : sbyte
        {
            PolygonDefault,
            PolygonHigh,
            CardsMedium,
            CardsHigh,
            None,
            RainbowBox
        }

        public enum InstancedGeometryStreamingpriority : sbyte
        {
            Default,
            Higher,
            Highest
        }

        public enum InstancedGeometryLightmappingPolicy : short
        {
            PerPixelSeperate,
            PerVertex,
            SingleProbe,
            PerPixelShared
        }

        public enum InstancedGeometryLightmappingPolicyReach : sbyte
        {
            PerPixelShared,
            PerVertex,
            SingleProbe,
            Exclude
        }

        public enum InstancedGeometryPathfindingPolicyReach : sbyte
        {
            CutOut,
            Static,
            None
        }

        [TagStructure(Size = 0x8, Gen = CacheGeneration.Third)]
        [TagStructure(Size = 0x4, Gen = CacheGeneration.HaloOnline)]
        public class CubemapSampleStruct : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short Cluster;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ClusterCubemapIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short CubemapPointIndex;

            public short BitmapIndex0;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short BitmapIndex1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float BlendFactor;
        }
    }
}
