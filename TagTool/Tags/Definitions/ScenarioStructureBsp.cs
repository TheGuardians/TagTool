using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x388, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x394, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x3AC, MinVersion = CacheVersion.HaloOnline106708)]
    public class ScenarioStructureBsp
    {
        public int BspChecksum;
        public FlagsValue Flags;
        public ContentPolicyFlagsValue ContentPolicyFlags;
        public ContentPolicyFlagsValue FailedContentPolicyFlags;
        [TagField(Padding = true, Length = 2)]
        public byte[] Unused1;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown3;
        public List<SeamIdentifier> SeamIdentifiers;
        public List<UnknownRaw7th> UnknownRaw7ths;
        public List<CollisionMaterial> CollisionMaterials;
        public List<Leaf> Leaves; // UnknownRaw3rd
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public List<UnknownRaw6th> UnknownRaw6ths;
        public List<UnknownRaw1st> UnknownRaw1sts;
        [TagField(Padding = true, Length = 0xC, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] UnknownUnused1;
        public List<ClusterPortal> ClusterPortals;
        public List<UnknownBlock2> Unknown14;
        public List<FogBlock> Fog;
        public List<CameraEffect> CameraEffects;
        public uint Unknown18;
        public uint Unknown19;
        public uint Unknown20;
        public List<DetailObject> DetailObjects;
        public List<Cluster> Clusters;
        public List<RenderMaterial> Materials;
        public List<SkyOwnerClusterBlock> SkyOwnerCluster;
        public List<ConveyorSurface> ConveyorSurfaces;
        public List<BreakableSurface> BreakableSurfaces;
        public List<PathfindingDatum> PathfindingData;
        public uint Unknown30;
        public uint Unknown31;
        public uint Unknown32;
        public List<BackgroundSoundEnvironmentPaletteBlock> BackgroundSoundEnvironmentPalette;
        public uint Unknown33;
        public uint Unknown34;
        public uint Unknown35;
        public uint Unknown36;
        public uint Unknown37;
        public uint Unknown38;
        public uint Unknown39;
        public uint Unknown40;
        public uint Unknown41;
        public uint Unknown42;
        public uint Unknown43;
        public List<Marker> Markers;
        public List<TagReferenceBlock> Lights;
        public List<UnknownBlock3> Unknown44;
        public List<RuntimeDecal> RuntimeDecals;
        public List<EnvironmentObjectPaletteBlock> EnvironmentObjectPalette;
        public List<EnvironmentObject> EnvironmentObjects;
        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;
        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;
        public uint Unknown51;
        public uint Unknown52;
        public uint Unknown53;
        public uint Unknown54;
        public List<InstancedGeometryInstance> InstancedGeometryInstances;
        public List<TagReferenceBlock> Decorators;
        public RenderGeometry Geometry;
        public List<UnknownSoundClustersBlock> UnknownSoundClustersA;
        public List<UnknownSoundClustersBlock> UnknownSoundClustersB;
        public List<UnknownSoundClustersBlock> UnknownSoundClustersC;
        public List<TransparentPlane> TransparentPlanes;
        public uint Unknown64;
        public uint Unknown65;
        public uint Unknown66;
        public List<CollisionMoppCode> CollisionMoppCodes;
        public uint Unknown67;
        public Bounds<float> CollisionWorldBoundsX;
        public Bounds<float> CollisionWorldBoundsY;
        public Bounds<float> CollisionWorldBoundsZ;
        public List<CollisionMoppCode> BreakableSurfaceMoppCodes;
        public List<BreakableSurfaceKeyTableBlock> BreakableSurfaceKeyTable;
        public uint Unknown68;
        public uint Unknown69;
        public uint Unknown70;
        public uint Unknown71;
        public uint Unknown72;
        public uint Unknown73;
        public RenderGeometry Geometry2;
        public List<LeafSystem> LeafSystems;
        public uint Unknown83;
        public uint Unknown84;
        public uint Unknown85;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int ZoneAssetIndex3;
        [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
        public PageableResource CollisionBspResource;

        public int UselessPadding3;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public int UnknownH3;

        [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
        public PageableResource PathfindingResource;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public int ZoneAssetIndex4;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public int UselessPadding4;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline106708)]
        public int Unknown86;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown87;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown88;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown89;
        [TagField(MinVersion = CacheVersion.HaloOnline301003)]
        public uint Unknown90;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            HasInstanceGroups = 1 << 0,
            SurfaceToTriangleMappingRemapped = 1 << 1,
            ExternalReferencesConvertedToIo = 1 << 2,
            StructureMoppNeedsRebuilt = 1 << 3,
            StructurePrefabMaterialsNeedPostprocessing = 1 << 4,
            SerializedHavokDataConvertedToTargetPlatform = 1 << 5
        }

        [Flags]
        public enum ContentPolicyFlagsValue : ushort
        {
            None = 0,
            HasWorkingPathfinding = 1 << 0,
            ConvexDecompositionEnabled = 1 << 1
        }

        [TagStructure(Size = 0x28)]
        public class SeamIdentifier
        {
            [TagField(Length = 4)]
            public int[] SeamIDs;
            public List<Edge> EdgeMapping;
            public List<Cluster> ClusterMapping;

            [TagStructure(Size = 0x4)]
            public class Edge
            {
                public int StructureEdgeIndex;
            }

            [TagStructure(Size = 0x10)]
            public class Cluster
            {
                public int ClusterIndex;
                public RealPoint3d ClusterCenter;
            }
        }

        [TagStructure(Size = 0x4)]
        public class UnknownRaw7th
        {
            public int SeamIndex;
        }

        [TagStructure(Size = 0x18)]
        public class CollisionMaterial
        {
            [TagField(Label = true)]
            public CachedTagInstance RenderMethod;
            public short RuntimeGlobalMaterialIndex;
            public short ConveyorSurfaceIndex;
            public short SeamMappingIndex;
            public FlagsValue Flags;

            [Flags]
            public enum FlagsValue : ushort
            {
                None = 0,
                IsSeam = 1 << 0
            }
        }

        [TagStructure(Size = 0x1)]
        public class Leaf
        {
            public byte Cluster;
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3ODST)]
        public class UnknownRaw6th
        {
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public short Unknown1StartIndexHalo3;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Unknown1StartIndex;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public short Unknown1EntryCountHalo3;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Unknown1EntryCount;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownRaw1st
        {
            public short ClusterIndex;
            public short Unknown;
        }

        [TagStructure(Size = 0x28)]
        public class ClusterPortal
        {
            public short BackCluster;
            public short FrontCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public FlagsValue Flags;
            public List<Vertex> Vertices;

            public enum FlagsValue : int
            {
                None = 0,
                AiCantHearThroughThisShit = 1 << 0,
                OneWay = 1 << 1,
                Door = 1 << 2,
                NoWay = 1 << 3,
                OneWayReversed = 1 << 4,
                NoOneCanHearThroughThis = 1 << 5
            }

            [TagStructure(Size = 0xC)]
            public struct Vertex
            {
                public RealPoint3d Position;
            }
        }

        [TagStructure(Size = 0x78)]
        public class UnknownBlock2
        {
            [TagField(Length = 32)] public string Name;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;
        }

        [TagStructure(Size = 0x8)]
        public class FogBlock
        {
            public StringId Name;
            public short Unknown;
            public short Unknown2;
        }

        [TagStructure(Size = 0x30)]
        public class CameraEffect
        {
            public StringId Name;
            public CachedTagInstance Effect;
            public sbyte Unknown;
            public sbyte Unknown2;
            public sbyte Unknown3;
            public sbyte Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
        }

        [TagStructure(Size = 0x34)]
        public class DetailObject
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public List<UnknownBlock> Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;

            [TagStructure(Size = 0x20)]
            public class UnknownBlock
            {
                public List<UnknownBlock2> Unknown;
                public byte[] Unknown2;

                [TagStructure(Size = 0x10)]
                public class UnknownBlock2
                {
                    public uint Unknown;
                    public uint Unknown2;
                    public uint Unknown3;
                    public uint Unknown4;
                }
            }
        }

        [TagStructure(Size = 0xDC)]
        public class Cluster
        {
            public Bounds<float> BoundsX;
            public Bounds<float> BoundsY;
            public Bounds<float> BoundsZ;
            public sbyte Unknown;
            public sbyte ScenarioSkyIndex;
            public sbyte CameraEffectIndex;
            public sbyte Unknown2;
            public short BackgroundSoundEnvironmentIndex;
            public short SoundClustersAIndex;
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short RuntimeDecalStartIndex;
            public short RuntimeDecalEntryCount;
            public short Flags;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public List<Portal> Portals;
            public int Unknown11;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public CachedTagInstance Bsp;
            public int ClusterIndex;
            public int Unknown15;
            public short Size2;
            public short Count2;
            public int Offset2;
            public int Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public List<CollisionMoppCode> CollisionMoppCodes;
            public short MeshIndex;
            public short Unknown20;
            public List<Seam> Seams;
            public List<DecoratorGrid> DecoratorGrids;
            public uint Unknown21;
            public uint Unknown22;
            public uint Unknown23;
            public List<ObjectPlacement> ObjectPlacements;
            public List<UnknownBlock2> Unknown25;

            [TagStructure(Size = 0x2)]
            public class Portal
            {
                public short PortalIndex;
            }

            [TagStructure(Size = 0x1)]
            public class Seam
            {
                public sbyte SeamIndex;
            }

            [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloOnline106708)]
            public class DecoratorGrid
            {
                public short Amount;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public sbyte DecoratorIndex_H3;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public sbyte DecoratorIndexScattering_H3;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public short DecoratorIndex_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public short DecoratorIndexScattering_HO;

                [TagField(Padding = true, Length = 2, MinVersion = CacheVersion.HaloOnline106708)] //Determine what is this
                public byte[] Unused;

                public int Unknown3;
                public RealPoint3d Position;
                public float Radius;
                public RealPoint3d GridSize;
                public RealPoint3d BoundingSphereOffset;
            }

            [TagStructure(Size = 0x4)]
            public class ObjectPlacement
            {
                public short Unknown;
                public short Unknown2;
            }

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2
            {
                public float Unknown;
                public float Unknown2;
                public float Unknown3;
                public short Unknown4;
                public short Unknown5;
            }
        }

        [TagStructure(Size = 0x2)]
        public struct SkyOwnerClusterBlock
        {
            public short Value;
        }

        [TagStructure(Size = 0x18)]
        public class ConveyorSurface
        {
            public RealVector3d U;
            public RealVector3d V;
        }

        [TagStructure(Size = 0x20)]
        public class BreakableSurface
        {
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
        }

        [TagStructure(Size = 0xA0)]
        public class PathfindingDatum
        {
            public List<Sector> Sectors;
            public List<Link> Links;
            public List<Reference> References;
            public List<Bsp2dNode> Bsp2dNodes;
            public List<Vertex> Vertices;
            public List<ObjectReference> ObjectReferences;
            public List<PathfindingHint> PathfindingHints;
            public List<InstancedGeometryReference> InstancedGeometryReferences;
            public int StructureChecksum;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public List<Unknown1Block> Unknown1s;
            public List<Unknown2Block> Unknown2s;
            public List<Unknown3Block> Unknown3s;
            public List<Unknown4Block> Unknown4s;

            [TagStructure(Size = 0x8)]
            public class Sector
            {
                public FlagsValue PathfindingSectorFlags;
                public short HintIndex;
                public int FirstLink;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None = 0,
                    SectorWalkable = 1 << 0,
                    SectorBreakable = 1 << 1,
                    SectorMobile = 1 << 2,
                    SectorBspSource = 1 << 3,
                    Floor = 1 << 4,
                    Ceiling = 1 << 5,
                    WallNorth = 1 << 6,
                    WallSouth = 1 << 7,
                    WallEast = 1 << 8,
                    WallWest = 1 << 9,
                    Crouchable = 1 << 10,
                    Aligned = 1 << 11,
                    SectorStep = 1 << 12,
                    SectorInterior = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }
            }

            [TagStructure(Size = 0x10)]
            public class Link
            {
                public short Vertex1;
                public short Vertex2;
                public FlagsValue LinkFlags;
                public short HintIndex;
                public short ForwardLink;
                public short ReverseLink;
                public short LeftSector;
                public short RightSector;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None = 0,
                    SectorLinkFromCollisionEdge = 1 << 0,
                    SectorIntersectionLink = 1 << 1,
                    SectorLinkBsp2dCreationError = 1 << 2,
                    SectorLinkTopologyError = 1 << 3,
                    SectorLinkChainError = 1 << 4,
                    SectorLinkBothSectorsWalkable = 1 << 5,
                    SectorLinkMagicHangingLink = 1 << 6,
                    SectorLinkThreshold = 1 << 7,
                    SectorLinkCrouchable = 1 << 8,
                    SectorLinkWallBase = 1 << 9,
                    SectorLinkLedge = 1 << 10,
                    SectorLinkLeanable = 1 << 11,
                    SectorLinkStartCorner = 1 << 12,
                    SectorLinkEndCorner = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }
            }

            [TagStructure(Size = 0x4)]
            public class Reference
            {
                public int NodeOrSectorIndex;
            }

            [TagStructure(Size = 0x14)]
            public class Bsp2dNode
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }

            [TagStructure(Size = 0xC)]
            public class Vertex
            {
                public RealPoint3d Position;
            }

            [TagStructure(Size = 0x18)]
            public class ObjectReference
            {
                public int Unknown;
                public List<UnknownBlock> Unknown2;
                public int Unknown3;
                public short Unknown4;
                public short Unknown5;

                [TagStructure(Size = 0x18)]
                public class UnknownBlock
                {
                    public int Unknown1;
                    public int Unknown2;
                    public List<UnknownBlock2> Unknown3;
                    public int Unknown4;

                    [TagStructure(Size = 0x4)]
                    public class UnknownBlock2
                    {
                        public int Unknown;
                    }
                }
            }

            [TagStructure(Size = 0x14)]
            public class PathfindingHint
            {
                public HintTypeValue HintType;
                public short SquadGroupFilter; // block index
                public int HintData0;
                public short HintData1;
                public byte HintData3;
                public byte Unused;
                public FlagsValue Flags;
                public short GeometryIndex; // block index
                public short ForceJumpHoistHeight; // different enum values for each hint type
                public ControlFlagsValue ControlFlags;

                public enum HintTypeValue : short
                {
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink
                }

                [Flags]
                public enum FlagsValue : short
                {
                    None = 0,
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }

                [Flags]
                public enum ControlFlagsValue : short
                {
                    None = 0,
                    MagicLift = 1 << 0,
                    VehicleOnly = 1 << 1,
                    Railing = 1 << 2,
                    Vault = 1 << 3,
                    Down = 1 << 4,
                    Phase = 1 << 5,
                    StopAutodown = 1 << 6,
                    ForceWalk = 1 << 7
                }
            }

            [TagStructure(Size = 0x4)]
            public class InstancedGeometryReference
            {
                public short PathfindingObjectIndex;
                public short Unknown;
            }

            [TagStructure(Size = 0x4)]
            public class Unknown1Block
            {
                public uint Unknown;
            }

            [TagStructure(Size = 0xC)]
            public class Unknown2Block
            {
                public List<UnknownBlock> Unknown;

                [TagStructure(Size = 0x4)]
                public class UnknownBlock
                {
                    public int Unknown;
                }
            }

            [TagStructure(Size = 0x14)]
            public class Unknown3Block
            {
                public short Unknown1;
                public short Unknown2;
                public float Unknown3;
                public List<UnknownBlock> Unknown4;

                [TagStructure(Size = 0x4)]
                public class UnknownBlock
                {
                    public short Unknown;
                    public short Unknown2;
                }
            }

            [TagStructure(Size = 0x4)]
            public class Unknown4Block
            {
                public short Unknown;
                public short Unknown2;
            }
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.Halo3ODST)]
        public class BackgroundSoundEnvironmentPaletteBlock
        {
            public StringId Name;
            public CachedTagInstance SoundEnvironment;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float CutoffDistance;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> CutoffRange;
            public float InterpolationSpeed;
            public CachedTagInstance BackgroundSound;
            public CachedTagInstance InsideClusterSound;
            public float CutoffDistance2;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed2;

            [Flags]
            public enum ScaleFlagsValue : int
            {
                None,
                Bit0,
                Bit1,
                Bit2 = 4,
                Bit3 = 8,
                Bit4 = 16,
                Bit5 = 32,
                Bit6 = 64,
                Bit7 = 128,
                Bit8 = 256,
                Bit9 = 512,
                Bit10 = 1024,
                Bit11 = 2048,
                Bit12 = 4096,
                Bit13 = 8192,
                Bit14 = 16384,
                Bit15 = 32768,
                Bit16 = 65536,
                Bit17 = 131072,
                Bit18 = 262144,
                Bit19 = 524288,
                Bit20 = 1048576,
                Bit21 = 2097152,
                Bit22 = 4194304,
                Bit23 = 8388608,
                Bit24 = 16777216,
                Bit25 = 33554432,
                Bit26 = 67108864,
                Bit27 = 134217728,
                Bit28 = 268435456,
                Bit29 = 536870912,
                Bit30 = 1073741824,
                Bit31 = -2147483648,
            }
        }

        [TagStructure(Size = 0x3C)]
        public class Marker
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
        }

        [TagStructure(Size = 0x2)]
        public class UnknownBlock3
        {
            public short Unknown;
        }

        [TagStructure(Size = 0x24)]
        public class RuntimeDecal
        {
            public short PaletteIndex;
            public sbyte Yaw;
            public sbyte Pitch;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float Scale;
        }

        [TagStructure(Size = 0x24)]
        public class EnvironmentObjectPaletteBlock
        {
            public CachedTagInstance Definition;
            public CachedTagInstance Model;
            public ObjectTypeValue ObjectType;

            [Flags]
            public enum ObjectTypeValue : int
            {
                None,
                Biped,
                Vehicle,
                Weapon = 4,
                Equipment = 8,
                ArgDevice = 16,
                Terminal = 32,
                Projectile = 64,
                Scenery = 128,
                Machine = 256,
                Control = 512,
                SoundScenery = 1024,
                Crate = 2048,
                Creature = 4096,
                Giant = 8192,
                EffectScenery = 16384
            }
        }

        [TagStructure(Size = 0x6C)]
        public class EnvironmentObject
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float Scale;
            public short PaletteIndex;
            public short Unknown;
            public int UniqueId;
            [TagField(Length = 32)]
            public string ScenarioObjectName;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x74, MinVersion = CacheVersion.HaloOnline106708)]
        public class InstancedGeometryInstance
        {
            public float Scale;
            public RealMatrix4x3 Matrix;
            public short MeshIndex;
            public ushort Flags;
            public short UnknownYoIndex;
            public short Unknown;
            public uint Unknown2;
            public RealPoint3d BoundingSphereOffset;
            public Bounds<float> BoundingSphereRadiusBounds;
            public StringId Name;
            public short PathfindingPolicy;
            public short LightmappingPolicy;
            public uint Unknown3;
            public List<CollisionDefinition> CollisionDefinitions;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown8;

            [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo3Retail, Align = 0x10)]
            [TagStructure(Size = 0x80, MinVersion = CacheVersion.Halo3ODST, Align = 0x10)]
            public class CollisionDefinition
            {
                public int Unknown;
                public short Size;
                public short Count;
                public int Address;
                public int Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public float Unknown7;
                public float Unknown8;
                public float Unknown9;
                public uint Unknown10;
                public float Unknown11;
                public float Unknown12;
                public float Unknown13;
                public uint Unknown14;
                public int Unknown15;
                public uint Unknown16;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown17;
                public sbyte BspIndex;
                public sbyte Unknown18;
                public short InstancedGeometryIndex;
                public float Unknown19;
                public uint Unknown20;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown21;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown22;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown23;
                public short Size2;
                public short Count2;
                public int Address2;
                public int Unknown24;
                public uint Unknown25;
                public uint Unknown26;
                public uint Unknown27;
                public float Unknown28;
            }
        }

        [TagStructure(Size = 0x1C)]
        public class UnknownSoundClustersBlock
        {
            public short BackgroundSoundEnvironmentIndex;
            public short Unknown;
            public List<PortalDesignatorBlock> PortalDesignators;
            public List<InteriorClusterIndexBlock> InteriorClusterIndices;

            [TagStructure(Size = 0x2)]
            public class PortalDesignatorBlock
            {
                public short PortalDesignator;
            }

            [TagStructure(Size = 0x2)]
            public class InteriorClusterIndexBlock
            {
                public short InteriorClusterIndex;
            }
        }

        [TagStructure(Size = 0x14)]
        public class TransparentPlane
        {
            public short MeshIndex;
            public short PartIndex;
            public RealPlane3d Plane;
        }
        
        [TagStructure(Size = 0x20)]
        public class BreakableSurfaceKeyTableBlock
        {
            public short InstancedGeometryIndex;
            public sbyte BreakableSurfaceIndex;
            public byte BreakableSurfaceSubIndex;
            public int SeedSurfaceIndex;
            public Bounds<float> X;
            public Bounds<float> Y;
            public Bounds<float> Z;
        }

        [TagStructure(Size = 0x14)]
        public class LeafSystem
        {
            public short Unknown;
            public short Unknown2;
            public CachedTagInstance LeafSystem2;
        }
    }
}