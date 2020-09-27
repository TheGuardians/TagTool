using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x318)]
    public class ScenarioStructureBsp : TagStructure
    {
        public List<TagImportInfo> ImportInfo;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public List<StructureCollisionMaterial> CollisionMaterials;
        public List<CollisionBspBlock> CollisionBsp;
        public float VehicleFloor; // World Units
        public float VehicleCeiling; // World Units
        public List<UnusedStructureNode> UnusedNodes;
        public List<StructureLeaf> Leaves;
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public List<StructureSurfaceReference> SurfaceReferences;
        public byte[] ClusterData;
        public List<ClusterPortal> ClusterPortals;
        public List<StructureFogPlane> FogPlanes;
        [TagField(Flags = Padding, Length = 24)]
        public byte[] Padding2;
        public List<StructureWeatherPaletteEntry> WeatherPalette;
        public List<StructureWeatherPolyhedron> WeatherPolyhedra;
        public List<StructureDetailObjectData> DetailObjects;
        public List<StructureCluster> Clusters;
        public List<GeometryMaterial> Materials;
        public List<SkyOwnerClusterBlock> SkyOwnerCluster;
        public List<StructureConveyorSurface> ConveyorSurfaces;
        public List<StructureBreakableSurface> BreakableSurfaces;
        public List<PathfindingDataBlock> PathfindingData;
        public List<Byte> PathfindingEdges;
        public List<StructureBackgroundSoundPaletteEntry> BackgroundSoundPalette;
        public List<StructureSoundEnvironmentPaletteEntry> SoundEnvironmentPalette;
        public byte[] SoundPasData;
        public List<StructureMarker> Markers;
        public List<StructureRuntimeDecal> RuntimeDecals;
        public List<StructureEnvironmentObjectPaletteEntry> EnvironmentObjectPalette;
        public List<StructureEnvironmentObject> EnvironmentObjects;
        public List<StructureLightmapData> Lightmaps;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public List<MapLeaf> LeafMapLeaves;
        public List<LeafConnection> LeafMapConnections;
        public List<ErrorReportCategory> Errors;
        public List<StructurePrecomputedLighting> PrecomputedLighting;
        public List<StructureInstancedGeometryDefinition> InstancedGeometriesDefinitions;
        public List<StructureInstancedGeometryInstance> InstancedGeometryInstances;
        public List<StructureSoundCluster> AmbienceSoundClusters;
        public List<StructureSoundCluster> ReverbSoundClusters;
        public List<TransparentPlane> TransparentPlanes;
        [TagField(Flags = Padding, Length = 96)]
        public byte[] Padding4;
        public float VehicleSpericalLimitRadius; // Distances this far and longer from limit origin will pull you back in.
        public RealPoint3d VehicleSpericalLimitCenter; // Center of space in which vehicle can move.
        public List<StructureDebugInfo> DebugInfo;
        public CachedTag Decorators;
        public StructurePhysicsStruct StructurePhysics;
        public List<WaterDefinition> WaterDefinitions;
        public List<StructurePortalDeviceMap> PortalDeviceMapping;
        public List<StructureAudibility> Audibility;
        public List<ObjectFakeLightprobe> ObjectFakeLightprobes;
        public List<DecoratorPlacementDefinition> Decorators1;
        
        [TagStructure(Size = 0x254)]
        public class TagImportInfo : TagStructure
        {
            public int Build;
            [TagField(Length = 256)]
            public string Version;
            [TagField(Length = 32)]
            public string ImportDate;
            [TagField(Length = 32)]
            public string Culprit;
            [TagField(Flags = Padding, Length = 96)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string ImportTime;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public List<TagImportFile> Files;
            [TagField(Flags = Padding, Length = 128)]
            public byte[] Padding3;
            
            [TagStructure(Size = 0x21C)]
            public class TagImportFile : TagStructure
            {
                [TagField(Length = 256)]
                public string Path;
                [TagField(Length = 32)]
                public string ModificationDate;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 88)]
                public byte[] Padding1;
                public int Checksum; // crc32
                public int Size; // bytes
                public byte[] ZippedData;
                [TagField(Flags = Padding, Length = 128)]
                public byte[] Padding2;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureCollisionMaterial : TagStructure
        {
            public CachedTag OldShader;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short ConveyorSurfaceIndex;
            public CachedTag NewShader;
        }
        
        [TagStructure(Size = 0x60)]
        public class CollisionBspBlock : TagStructure
        {
            public List<Bsp3dNode> Bsp3dNodes;
            public List<RealPlane3d> Planes;
            public List<CollisionLeaf> Leaves;
            public List<Bsp2dReference> Bsp2dReferences;
            public List<Bsp2dNode> Bsp2dNodes;
            public List<CollisionSurface> Surfaces;
            public List<CollisionEdge> Edges;
            public List<CollisionVertex> Vertices;
            
            [TagStructure(Size = 0x8)]
            public class Bsp3dNode : TagStructure
            {
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealPlane3d : TagStructure
            {
                public RealPlane3d Plane;
            }
            
            [TagStructure(Size = 0x4)]
            public class CollisionLeaf : TagStructure
            {
                public FlagsValue Flags;
                public sbyte Bsp2dReferenceCount;
                public short FirstBsp2dReference;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    ContainsDoubleSidedSurfaces = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class Bsp2dReference : TagStructure
            {
                public short Plane;
                public short Bsp2dNode;
            }
            
            [TagStructure(Size = 0x10)]
            public class Bsp2dNode : TagStructure
            {
                public RealPlane2d Plane;
                public short LeftChild;
                public short RightChild;
            }
            
            [TagStructure(Size = 0x8)]
            public class CollisionSurface : TagStructure
            {
                public short Plane;
                public short FirstEdge;
                public FlagsValue Flags;
                public sbyte BreakableSurface;
                public short Material;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    TwoSided = 1 << 0,
                    Invisible = 1 << 1,
                    Climbable = 1 << 2,
                    Breakable = 1 << 3,
                    Invalid = 1 << 4,
                    Conveyor = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class CollisionEdge : TagStructure
            {
                public short StartVertex;
                public short EndVertex;
                public short ForwardEdge;
                public short ReverseEdge;
                public short LeftSurface;
                public short RightSurface;
            }
            
            [TagStructure(Size = 0x10)]
            public class CollisionVertex : TagStructure
            {
                public RealPoint3d Point;
                public short FirstEdge;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x6)]
        public class UnusedStructureNode : TagStructure
        {
            [TagField(Flags = Padding, Length = 6)]
            public byte[] Unknown1;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureLeaf : TagStructure
        {
            public short Cluster;
            public short SurfaceReferenceCount;
            public int FirstSurfaceReferenceIndex;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureSurfaceReference : TagStructure
        {
            public short StripIndex;
            public short LightmapTriangleIndex;
            public int BspNodeIndex;
        }
        
        [TagStructure(Size = 0x28)]
        public class ClusterPortal : TagStructure
        {
            public short BackCluster;
            public short FrontCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public FlagsValue Flags;
            public List<RealPoint3d> Vertices;
            
            [Flags]
            public enum FlagsValue : uint
            {
                AiCannotHearThroughThis = 1 << 0,
                OneWay = 1 << 1,
                Door = 1 << 2,
                NoWay = 1 << 3,
                OneWayReversed = 1 << 4,
                NoOneCanHearThroughThis = 1 << 5
            }
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Point;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureFogPlane : TagStructure
        {
            public short ScenarioPlanarFogIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public RealPlane3d Plane;
            public FlagsValue Flags;
            public short Priority;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                ExtendInfinitelyWhileVisible = 1 << 0,
                DoNotFloodfill = 1 << 1,
                AggressiveFloodfill = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x98)]
        public class StructureWeatherPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag WeatherSystem;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding3;
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure(Size = 0x1C)]
        public class StructureWeatherPolyhedron : TagStructure
        {
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public List<RealPlane3d> Planes;
            
            [TagStructure(Size = 0x10)]
            public class RealPlane3d : TagStructure
            {
                public RealPlane3d Plane;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class StructureDetailObjectData : TagStructure
        {
            public List<DetailObjectCellDefinition> Cells;
            public List<DetailObject> Instances;
            public List<DetailObjectCount> Counts;
            public List<RealVector4d> ZReferenceVectors;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x20)]
            public class DetailObjectCellDefinition : TagStructure
            {
                public short Unknown1;
                public short Unknown2;
                public short Unknown3;
                public short Unknown4;
                public int Unknown5;
                public int Unknown6;
                public int Unknown7;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x6)]
            public class DetailObject : TagStructure
            {
                public sbyte Unknown1;
                public sbyte Unknown2;
                public sbyte Unknown3;
                public sbyte Unknown4;
                public short Unknown5;
            }
            
            [TagStructure(Size = 0x2)]
            public class DetailObjectCount : TagStructure
            {
                public short Unknown1;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4d : TagStructure
            {
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
            }
        }
        
        [TagStructure(Size = 0xD8)]
        public class StructureCluster : TagStructure
        {
            public GeometrySectionInfo SectionInfo;
            public GeometryBlockInfoStruct GeometryBlockInfo;
            public List<StructureClusterData> ClusterData;
            /// <summary>
            /// CLUSTER INFO
            /// </summary>
            public Bounds<float> BoundsX;
            public Bounds<float> BoundsY;
            public Bounds<float> BoundsZ;
            public sbyte ScenarioSkyIndex;
            public sbyte MediaIndex;
            public sbyte ScenarioVisibleSkyIndex;
            public sbyte ScenarioAtmosphericFogIndex;
            public sbyte PlanarFogDesignator;
            public sbyte VisibleFogPlaneIndex;
            public short BackgroundSound;
            public short SoundEnvironment;
            public short Weather;
            public short TransitionStructureBsp;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            public List<PredictedResource> PredictedResources;
            public List<PortalsBlock> Portals;
            public int ChecksumFromStructure;
            public List<Word> InstancedGeometryIndices;
            public List<Word> IndexReorderTable;
            public byte[] CollisionMoppCode;
            
            [TagStructure(Size = 0x2C)]
            public class GeometrySectionInfo : TagStructure
            {
                /// <summary>
                /// SECTION INFO
                /// </summary>
                public short TotalVertexCount;
                public short TotalTriangleCount;
                public short TotalPartCount;
                public short ShadowCastingTriangleCount;
                public short ShadowCastingPartCount;
                public short OpaquePointCount;
                public short OpaqueVertexCount;
                public short OpaquePartCount;
                public sbyte OpaqueMaxNodesVertex;
                public sbyte TransparentMaxNodesVertex;
                public short ShadowCastingRigidTriangleCount;
                public GeometryClassificationValue GeometryClassification;
                public GeometryCompressionFlagsValue GeometryCompressionFlags;
                public List<GeometryCompressionInfo> Unknown1;
                public sbyte HardwareNodeCount;
                public sbyte NodeMapSize;
                public short SoftwarePlaneCount;
                public short TotalSubpartCont;
                public SectionLightingFlagsValue SectionLightingFlags;
                
                public enum GeometryClassificationValue : short
                {
                    Worldspace,
                    Rigid,
                    RigidBoned,
                    Skinned,
                    UnsupportedReimport
                }
                
                [Flags]
                public enum GeometryCompressionFlagsValue : ushort
                {
                    CompressedPosition = 1 << 0,
                    CompressedTexcoord = 1 << 1,
                    CompressedSecondaryTexcoord = 1 << 2
                }
                
                [TagStructure(Size = 0x38)]
                public class GeometryCompressionInfo : TagStructure
                {
                    public Bounds<float> PositionBoundsX;
                    public Bounds<float> PositionBoundsY;
                    public Bounds<float> PositionBoundsZ;
                    public Bounds<float> TexcoordBoundsX;
                    public Bounds<float> TexcoordBoundsY;
                    public Bounds<float> SecondaryTexcoordBoundsX;
                    public Bounds<float> SecondaryTexcoordBoundsY;
                }
                
                [Flags]
                public enum SectionLightingFlagsValue : ushort
                {
                    HasLmTexcoords = 1 << 0,
                    HasLmIncRad = 1 << 1,
                    HasLmColors = 1 << 2,
                    HasLmPrt = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class GeometryBlockInfoStruct : TagStructure
            {
                /// <summary>
                /// BLOCK INFO
                /// </summary>
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GeometryBlockResource> Resources;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public short OwnerTagSectionOffset;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                
                [TagStructure(Size = 0x10)]
                public class GeometryBlockResource : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Flags = Padding, Length = 3)]
                    public byte[] Padding1;
                    public short PrimaryLocator;
                    public short SecondaryLocator;
                    public int ResourceDataSize;
                    public int ResourceDataOffset;
                    
                    public enum TypeValue : sbyte
                    {
                        TagBlock,
                        TagData,
                        VertexBuffer
                    }
                }
            }
            
            [TagStructure(Size = 0x6C)]
            public class StructureClusterData : TagStructure
            {
                public GeometrySection Section;
                
                [TagStructure(Size = 0x6C)]
                public class GeometrySection : TagStructure
                {
                    public List<GeometryPart> Parts;
                    public List<GeometrySubpart> Subparts;
                    public List<GeometryVisibility> VisibilityBounds;
                    public List<GeometryVertex> RawVertices;
                    public List<Word> StripIndices;
                    public byte[] VisibilityMoppCode;
                    public List<Word> MoppReorderTable;
                    public List<RasterizerVertexBuffer> VertexBuffers;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x48)]
                    public class GeometryPart : TagStructure
                    {
                        public TypeValue Type;
                        public FlagsValue Flags;
                        public short Material;
                        public short StripStartIndex;
                        public short StripLength;
                        public short FirstSubpartIndex;
                        public short SubpartCount;
                        public sbyte MaxNodesVertex;
                        public sbyte ContributingCompoundNodeCount;
                        /// <summary>
                        /// CENTROID
                        /// </summary>
                        public RealPoint3d Position;
                        public sbyte NodeIndex;
                        [TagField(Length = 4)]
                        public sbyte NodeIndices;
                        public float NodeWeight;
                        [TagField(Length = 3)]
                        public float NodeWeights;
                        public float LodMipmapMagicNumber;
                        [TagField(Flags = Padding, Length = 24)]
                        public byte[] Unknown3;
                        
                        public enum TypeValue : short
                        {
                            NotDrawn,
                            OpaqueShadowOnly,
                            OpaqueShadowCasting,
                            OpaqueNonshadowing,
                            Transparent,
                            LightmapOnly
                        }
                        
                        [Flags]
                        public enum FlagsValue : ushort
                        {
                            Decalable = 1 << 0,
                            NewPartTypes = 1 << 1,
                            DislikesPhotons = 1 << 2,
                            OverrideTriangleList = 1 << 3,
                            IgnoredByLightmapper = 1 << 4
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class GeometrySubpart : TagStructure
                    {
                        public short IndicesStartIndex;
                        public short IndicesLength;
                        public short VisibilityBoundsIndex;
                        public short PartIndex;
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class GeometryVisibility : TagStructure
                    {
                        public float PositionX;
                        public float PositionY;
                        public float PositionZ;
                        public float Radius;
                        public sbyte Node0;
                        [TagField(Flags = Padding, Length = 3)]
                        public byte[] Padding1;
                    }
                    
                    [TagStructure(Size = 0xC4)]
                    public class GeometryVertex : TagStructure
                    {
                        public RealPoint3d Position;
                        public int NodeIndexOld;
                        [TagField(Length = 4)]
                        public int NodeIndicesOld;
                        public float NodeWeight;
                        [TagField(Length = 4)]
                        public float NodeWeights;
                        public int NodeIndexNew;
                        [TagField(Length = 4)]
                        public int NodeIndicesNew;
                        public int UseNewNodeIndices;
                        public int AdjustedCompoundNodeIndex;
                        public RealPoint2d Texcoord;
                        public RealVector3d Normal;
                        public RealVector3d Binormal;
                        public RealVector3d Tangent;
                        public RealVector3d AnisotropicBinormal;
                        public RealPoint2d SecondaryTexcoord;
                        public RealRgbColor PrimaryLightmapColor;
                        public RealPoint2d PrimaryLightmapTexcoord;
                        public RealVector3d PrimaryLightmapIncidentDirection;
                        [TagField(Flags = Padding, Length = 12)]
                        public byte[] Padding1;
                        [TagField(Flags = Padding, Length = 8)]
                        public byte[] Padding2;
                        [TagField(Flags = Padding, Length = 12)]
                        public byte[] Padding3;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class Word : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class RasterizerVertexBuffer : TagStructure
                    {
                        public VertexBuffer VertexBuffer;
                    }
                }
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                OneWayPortal = 1 << 0,
                DoorPortal = 1 << 1,
                PostprocessedGeometry = 1 << 2,
                IsTheSky = 1 << 3
            }
            
            [TagStructure(Size = 0x8)]
            public class PredictedResource : TagStructure
            {
                public TypeValue Type;
                public short ResourceIndex;
                public int TagIndex;
                
                public enum TypeValue : short
                {
                    Bitmap,
                    Sound,
                    RenderModelGeometry,
                    ClusterGeometry,
                    ClusterInstancedGeometry,
                    LightmapGeometryObjectBuckets,
                    LightmapGeometryInstanceBuckets,
                    LightmapClusterBitmaps,
                    LightmapInstanceBitmaps
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class PortalsBlock : TagStructure
            {
                public short PortalIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class Word : TagStructure
            {
                public short InstancedGeometryIndex;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class GeometryMaterial : TagStructure
        {
            public CachedTag OldShader;
            public CachedTag Shader;
            public List<GeometryMaterialProperty> Properties;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public sbyte BreakableSurfaceIndex;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x8)]
            public class GeometryMaterialProperty : TagStructure
            {
                public TypeValue Type;
                public short IntValue;
                public float RealValue;
                
                public enum TypeValue : short
                {
                    LightmapResolution,
                    LightmapPower,
                    LightmapHalfLife,
                    LightmapDiffuseScale
                }
            }
        }
        
        [TagStructure(Size = 0x2)]
        public class SkyOwnerClusterBlock : TagStructure
        {
            public short ClusterOwner;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureConveyorSurface : TagStructure
        {
            public RealVector3d U;
            public RealVector3d V;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureBreakableSurface : TagStructure
        {
            public short InstancedGeometryInstance;
            public short BreakableSurfaceIndex;
            public RealPoint3d Centroid;
            public float Radius;
            public int CollisionSurfaceIndex;
        }
        
        [TagStructure(Size = 0x9C)]
        public class PathfindingDataBlock : TagStructure
        {
            public List<Sector> Sectors;
            public List<SectorLink> Links;
            public List<Bsp2dRef> Refs;
            public List<LargeBsp2dNode> Bsp2dNodes;
            public List<LongSurfaceFlags> SurfaceFlags;
            public List<SectorVertex> Vertices;
            public List<EnvironmentObjectReference> ObjectRefs;
            public List<PathfindingHintData> PathfindingHints;
            public List<InstancedGeometryReference> InstancedGeometryRefs;
            public int StructureChecksum;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
            public List<UserHintData> UserPlacedHints;
            
            [TagStructure(Size = 0x8)]
            public class Sector : TagStructure
            {
                public PathFindingSectorFlagsValue PathFindingSectorFlags;
                public short HintIndex;
                public int FirstLinkDoNotSetManually;
                
                [Flags]
                public enum PathFindingSectorFlagsValue : ushort
                {
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
                    SectorInterior = 1 << 13
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class SectorLink : TagStructure
            {
                public short Vertex1;
                public short Vertex2;
                public LinkFlagsValue LinkFlags;
                public short HintIndex;
                public short ForwardLink;
                public short ReverseLink;
                public short LeftSector;
                public short RightSector;
                
                [Flags]
                public enum LinkFlagsValue : ushort
                {
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
                    SectorLinkEndCorner = 1 << 13
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class Bsp2dRef : TagStructure
            {
                public int NodeRefOrSectorRef;
            }
            
            [TagStructure(Size = 0x14)]
            public class LargeBsp2dNode : TagStructure
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }
            
            [TagStructure(Size = 0x4)]
            public class LongSurfaceFlags : TagStructure
            {
                public int Flags;
            }
            
            [TagStructure(Size = 0xC)]
            public class SectorVertex : TagStructure
            {
                public RealPoint3d Point;
            }
            
            [TagStructure(Size = 0x24)]
            public class EnvironmentObjectReference : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public int FirstSector;
                public int LastSector;
                public List<EnvironmentObjectBspReference> Bsps;
                public List<EnvironmentObjectNodeReference> Nodes;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Mobile = 1 << 0
                }
                
                [TagStructure(Size = 0x10)]
                public class EnvironmentObjectBspReference : TagStructure
                {
                    public int BspReference;
                    public int FirstSector;
                    public int LastSector;
                    public short NodeIndex;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                }
                
                [TagStructure(Size = 0x4)]
                public class EnvironmentObjectNodeReference : TagStructure
                {
                    public short ReferenceFrameIndex;
                    public sbyte ProjectionAxis;
                    public ProjectionSignValue ProjectionSign;
                    
                    [Flags]
                    public enum ProjectionSignValue : byte
                    {
                        ProjectionSign = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class PathfindingHintData : TagStructure
            {
                public HintTypeValue HintType;
                public short NextHintIndex;
                public short HintData0;
                public short HintData1;
                public short HintData2;
                public short HintData3;
                public short HintData4;
                public short HintData5;
                public short HintData6;
                public short HintData7;
                
                public enum HintTypeValue : short
                {
                    IntersectionLink,
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    BreakableFloor
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class InstancedGeometryReference : TagStructure
            {
                public short PathfindingObjectIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x6C)]
            public class UserHintData : TagStructure
            {
                public List<UserHintPoint> PointGeometry;
                public List<UserHintRay> RayGeometry;
                public List<UserHintLineSegment> LineSegmentGeometry;
                public List<UserHintParallelogram> ParallelogramGeometry;
                public List<UserHintPolygon> PolygonGeometry;
                public List<UserHintJump> JumpHints;
                public List<UserHintClimb> ClimbHints;
                public List<UserHintWell> WellHints;
                public List<UserFlightHint> FlightHints;
                
                [TagStructure(Size = 0x10)]
                public class UserHintPoint : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintRay : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealVector3d Vector;
                }
                
                [TagStructure(Size = 0x24)]
                public class UserHintLineSegment : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class UserHintParallelogram : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    public RealPoint3d Point2;
                    public short ReferenceFrame2;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding3;
                    public RealPoint3d Point3;
                    public short ReferenceFrame3;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding4;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class UserHintPolygon : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintPoint> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class UserHintPoint : TagStructure
                    {
                        public RealPoint3d Point;
                        public short ReferenceFrame;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding1;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class UserHintJump : TagStructure
                {
                    public FlagsValue Flags;
                    public short GeometryIndex;
                    public ForceJumpHeightValue ForceJumpHeight;
                    public ControlFlagsValue ControlFlags;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    public enum ForceJumpHeightValue : short
                    {
                        None,
                        Down,
                        Step,
                        Crouch,
                        Stand,
                        Storey,
                        Tower,
                        Infinite
                    }
                    
                    [Flags]
                    public enum ControlFlagsValue : ushort
                    {
                        MagicLift = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserHintClimb : TagStructure
                {
                    public FlagsValue Flags;
                    public short GeometryIndex;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class UserHintWell : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintWellPoint> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class UserHintWellPoint : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding1;
                        public RealVector3d Point;
                        public short ReferenceFrame;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding2;
                        public int SectorIndex;
                        public RealEulerAngles2d Normal;
                        
                        public enum TypeValue : short
                        {
                            Jump,
                            Climb,
                            Hoist
                        }
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class UserFlightHint : TagStructure
                {
                    public List<UserHintFlightPoint> Points;
                    
                    [TagStructure(Size = 0xC)]
                    public class UserHintFlightPoint : TagStructure
                    {
                        public RealVector3d Point;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class Byte : TagStructure
        {
            public sbyte Midpoint;
        }
        
        [TagStructure(Size = 0x74)]
        public class StructureBackgroundSoundPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag BackgroundSound;
            public CachedTag InsideClusterSound; // Play only when player is inside cluster.
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding1;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class StructureSoundEnvironmentPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x3C)]
        public class StructureMarker : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureRuntimeDecal : TagStructure
        {
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Unknown1;
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureEnvironmentObjectPaletteEntry : TagStructure
        {
            public CachedTag Definition;
            public CachedTag Model;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x68)]
        public class StructureEnvironmentObject : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public short PaletteIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public int UniqueId;
            public Tag ExportedObjectType;
            [TagField(Length = 32)]
            public string ScenarioObjectName;
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureLightmapData : TagStructure
        {
            public CachedTag BitmapGroup;
        }
        
        [TagStructure(Size = 0x18)]
        public class MapLeaf : TagStructure
        {
            public List<MapLeafFace> Faces;
            public List<ConnectionIndicesBlock> ConnectionIndices;
            
            [TagStructure(Size = 0x10)]
            public class MapLeafFace : TagStructure
            {
                public int NodeIndex;
                public List<RealPoint3d> Vertices;
                
                [TagStructure(Size = 0xC)]
                public class RealPoint3d : TagStructure
                {
                    public RealPoint3d Vertex;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ConnectionIndicesBlock : TagStructure
            {
                public int ConnectionIndex;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class LeafConnection : TagStructure
        {
            public int PlaneIndex;
            public int BackLeafIndex;
            public int FrontLeafIndex;
            public List<RealPoint3d> Vertices;
            public float Area;
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Vertex;
            }
        }
        
        [TagStructure(Size = 0x2A8)]
        public class ErrorReportCategory : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ReportTypeValue ReportType;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 404)]
            public byte[] Padding3;
            public List<ErrorReport> Reports;
            
            public enum ReportTypeValue : short
            {
                Silent,
                Comment,
                Warning,
                Error
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Rendered = 1 << 0,
                TangentSpace = 1 << 1,
                Noncritical = 1 << 2,
                LightmapLight = 1 << 3,
                ReportKeyIsValid = 1 << 4
            }
            
            [TagStructure(Size = 0x284)]
            public class ErrorReport : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public byte[] Text;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVertex> Vertices;
                public List<ErrorReportVector> Vectors;
                public List<ErrorReportLine> Lines;
                public List<ErrorReportTriangle> Triangles;
                public List<ErrorReportQuad> Quads;
                public List<ErrorReportComment> Comments;
                [TagField(Flags = Padding, Length = 380)]
                public byte[] Padding1;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                [TagField(Flags = Padding, Length = 84)]
                public byte[] Padding2;
                
                public enum TypeValue : short
                {
                    Silent,
                    Comment,
                    Warning,
                    Error
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Rendered = 1 << 0,
                    TangentSpace = 1 << 1,
                    Noncritical = 1 << 2,
                    LightmapLight = 1 << 3,
                    ReportKeyIsValid = 1 << 4
                }
                
                [TagStructure(Size = 0x34)]
                public class ErrorReportVertex : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                    public float ScreenSize;
                }
                
                [TagStructure(Size = 0x40)]
                public class ErrorReportVector : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                }
                
                [TagStructure(Size = 0x50)]
                public class ErrorReportLine : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 2)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x70)]
                public class ErrorReportTriangle : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 3)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x90)]
                public class ErrorReportQuad : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 4)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x44)]
                public class ErrorReportComment : TagStructure
                {
                    public byte[] Text;
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                }
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class StructurePrecomputedLighting : TagStructure
        {
            public int Index;
            public LightTypeValue LightType;
            public sbyte AttachmentIndex;
            public sbyte ObjectType;
            public StructurePrecomputedLightingVisiblity Visibility;
            
            public enum LightTypeValue : short
            {
                FreeStanding,
                AttachedToEditorObject,
                AttachedToStructureObject
            }
            
            [TagStructure(Size = 0x58)]
            public class StructurePrecomputedLightingVisiblity : TagStructure
            {
                public short ProjectionCount;
                public short ClusterCount;
                public short VolumeCount;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public byte[] Projections;
                public byte[] VisibilityClusters;
                public byte[] ClusterRemapTable;
                public byte[] VisibilityVolumes;
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class StructureInstancedGeometryDefinition : TagStructure
        {
            public StructureInstancedGeometryRenderInfo RenderInfo;
            public int Checksum;
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public CollisionBsp CollisionInfo;
            public List<CollisionBspPhysicsDefinition> BspPhysics;
            public List<StructureLeaf> RenderLeaves;
            public List<StructureSurfaceReference> SurfaceReferences;
            
            [TagStructure(Size = 0x6C)]
            public class StructureInstancedGeometryRenderInfo : TagStructure
            {
                public GeometrySectionInfo SectionInfo;
                public GeometryBlockInfoStruct GeometryBlockInfo;
                public List<StructureClusterData> RenderData;
                public List<Word> IndexReorderTable;
                
                [TagStructure(Size = 0x2C)]
                public class GeometrySectionInfo : TagStructure
                {
                    /// <summary>
                    /// SECTION INFO
                    /// </summary>
                    public short TotalVertexCount;
                    public short TotalTriangleCount;
                    public short TotalPartCount;
                    public short ShadowCastingTriangleCount;
                    public short ShadowCastingPartCount;
                    public short OpaquePointCount;
                    public short OpaqueVertexCount;
                    public short OpaquePartCount;
                    public sbyte OpaqueMaxNodesVertex;
                    public sbyte TransparentMaxNodesVertex;
                    public short ShadowCastingRigidTriangleCount;
                    public GeometryClassificationValue GeometryClassification;
                    public GeometryCompressionFlagsValue GeometryCompressionFlags;
                    public List<GeometryCompressionInfo> Unknown1;
                    public sbyte HardwareNodeCount;
                    public sbyte NodeMapSize;
                    public short SoftwarePlaneCount;
                    public short TotalSubpartCont;
                    public SectionLightingFlagsValue SectionLightingFlags;
                    
                    public enum GeometryClassificationValue : short
                    {
                        Worldspace,
                        Rigid,
                        RigidBoned,
                        Skinned,
                        UnsupportedReimport
                    }
                    
                    [Flags]
                    public enum GeometryCompressionFlagsValue : ushort
                    {
                        CompressedPosition = 1 << 0,
                        CompressedTexcoord = 1 << 1,
                        CompressedSecondaryTexcoord = 1 << 2
                    }
                    
                    [TagStructure(Size = 0x38)]
                    public class GeometryCompressionInfo : TagStructure
                    {
                        public Bounds<float> PositionBoundsX;
                        public Bounds<float> PositionBoundsY;
                        public Bounds<float> PositionBoundsZ;
                        public Bounds<float> TexcoordBoundsX;
                        public Bounds<float> TexcoordBoundsY;
                        public Bounds<float> SecondaryTexcoordBoundsX;
                        public Bounds<float> SecondaryTexcoordBoundsY;
                    }
                    
                    [Flags]
                    public enum SectionLightingFlagsValue : ushort
                    {
                        HasLmTexcoords = 1 << 0,
                        HasLmIncRad = 1 << 1,
                        HasLmColors = 1 << 2,
                        HasLmPrt = 1 << 3
                    }
                }
                
                [TagStructure(Size = 0x28)]
                public class GeometryBlockInfoStruct : TagStructure
                {
                    /// <summary>
                    /// BLOCK INFO
                    /// </summary>
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GeometryBlockResource> Resources;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding1;
                    public short OwnerTagSectionOffset;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding3;
                    
                    [TagStructure(Size = 0x10)]
                    public class GeometryBlockResource : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Flags = Padding, Length = 3)]
                        public byte[] Padding1;
                        public short PrimaryLocator;
                        public short SecondaryLocator;
                        public int ResourceDataSize;
                        public int ResourceDataOffset;
                        
                        public enum TypeValue : sbyte
                        {
                            TagBlock,
                            TagData,
                            VertexBuffer
                        }
                    }
                }
                
                [TagStructure(Size = 0x6C)]
                public class StructureClusterData : TagStructure
                {
                    public GeometrySection Section;
                    
                    [TagStructure(Size = 0x6C)]
                    public class GeometrySection : TagStructure
                    {
                        public List<GeometryPart> Parts;
                        public List<GeometrySubpart> Subparts;
                        public List<GeometryVisibility> VisibilityBounds;
                        public List<GeometryVertex> RawVertices;
                        public List<Word> StripIndices;
                        public byte[] VisibilityMoppCode;
                        public List<Word> MoppReorderTable;
                        public List<RasterizerVertexBuffer> VertexBuffers;
                        [TagField(Flags = Padding, Length = 4)]
                        public byte[] Padding1;
                        
                        [TagStructure(Size = 0x48)]
                        public class GeometryPart : TagStructure
                        {
                            public TypeValue Type;
                            public FlagsValue Flags;
                            public short Material;
                            public short StripStartIndex;
                            public short StripLength;
                            public short FirstSubpartIndex;
                            public short SubpartCount;
                            public sbyte MaxNodesVertex;
                            public sbyte ContributingCompoundNodeCount;
                            /// <summary>
                            /// CENTROID
                            /// </summary>
                            public RealPoint3d Position;
                            public sbyte NodeIndex;
                            [TagField(Length = 4)]
                            public sbyte NodeIndices;
                            public float NodeWeight;
                            [TagField(Length = 3)]
                            public float NodeWeights;
                            public float LodMipmapMagicNumber;
                            [TagField(Flags = Padding, Length = 24)]
                            public byte[] Unknown3;
                            
                            public enum TypeValue : short
                            {
                                NotDrawn,
                                OpaqueShadowOnly,
                                OpaqueShadowCasting,
                                OpaqueNonshadowing,
                                Transparent,
                                LightmapOnly
                            }
                            
                            [Flags]
                            public enum FlagsValue : ushort
                            {
                                Decalable = 1 << 0,
                                NewPartTypes = 1 << 1,
                                DislikesPhotons = 1 << 2,
                                OverrideTriangleList = 1 << 3,
                                IgnoredByLightmapper = 1 << 4
                            }
                        }
                        
                        [TagStructure(Size = 0x8)]
                        public class GeometrySubpart : TagStructure
                        {
                            public short IndicesStartIndex;
                            public short IndicesLength;
                            public short VisibilityBoundsIndex;
                            public short PartIndex;
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class GeometryVisibility : TagStructure
                        {
                            public float PositionX;
                            public float PositionY;
                            public float PositionZ;
                            public float Radius;
                            public sbyte Node0;
                            [TagField(Flags = Padding, Length = 3)]
                            public byte[] Padding1;
                        }
                        
                        [TagStructure(Size = 0xC4)]
                        public class GeometryVertex : TagStructure
                        {
                            public RealPoint3d Position;
                            public int NodeIndexOld;
                            [TagField(Length = 4)]
                            public int NodeIndicesOld;
                            public float NodeWeight;
                            [TagField(Length = 4)]
                            public float NodeWeights;
                            public int NodeIndexNew;
                            [TagField(Length = 4)]
                            public int NodeIndicesNew;
                            public int UseNewNodeIndices;
                            public int AdjustedCompoundNodeIndex;
                            public RealPoint2d Texcoord;
                            public RealVector3d Normal;
                            public RealVector3d Binormal;
                            public RealVector3d Tangent;
                            public RealVector3d AnisotropicBinormal;
                            public RealPoint2d SecondaryTexcoord;
                            public RealRgbColor PrimaryLightmapColor;
                            public RealPoint2d PrimaryLightmapTexcoord;
                            public RealVector3d PrimaryLightmapIncidentDirection;
                            [TagField(Flags = Padding, Length = 12)]
                            public byte[] Padding1;
                            [TagField(Flags = Padding, Length = 8)]
                            public byte[] Padding2;
                            [TagField(Flags = Padding, Length = 12)]
                            public byte[] Padding3;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class Word : TagStructure
                        {
                            public short Index;
                        }
                        
                        [TagStructure(Size = 0x20)]
                        public class RasterizerVertexBuffer : TagStructure
                        {
                            public VertexBuffer VertexBuffer;
                        }
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class Word : TagStructure
                {
                    public short Index;
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class CollisionBsp : TagStructure
            {
                public List<Bsp3dNode> Bsp3dNodes;
                public List<RealPlane3d> Planes;
                public List<CollisionLeaf> Leaves;
                public List<Bsp2dReference> Bsp2dReferences;
                public List<Bsp2dNode> Bsp2dNodes;
                public List<CollisionSurface> Surfaces;
                public List<CollisionEdge> Edges;
                public List<CollisionVertex> Vertices;
                
                [TagStructure(Size = 0x8)]
                public class Bsp3dNode : TagStructure
                {
                    [TagField(Flags = Padding, Length = 8)]
                    public byte[] Unknown1;
                }
                
                [TagStructure(Size = 0x10)]
                public class RealPlane3d : TagStructure
                {
                    public RealPlane3d Plane;
                }
                
                [TagStructure(Size = 0x4)]
                public class CollisionLeaf : TagStructure
                {
                    public FlagsValue Flags;
                    public sbyte Bsp2dReferenceCount;
                    public short FirstBsp2dReference;
                    
                    [Flags]
                    public enum FlagsValue : byte
                    {
                        ContainsDoubleSidedSurfaces = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class Bsp2dReference : TagStructure
                {
                    public short Plane;
                    public short Bsp2dNode;
                }
                
                [TagStructure(Size = 0x10)]
                public class Bsp2dNode : TagStructure
                {
                    public RealPlane2d Plane;
                    public short LeftChild;
                    public short RightChild;
                }
                
                [TagStructure(Size = 0x8)]
                public class CollisionSurface : TagStructure
                {
                    public short Plane;
                    public short FirstEdge;
                    public FlagsValue Flags;
                    public sbyte BreakableSurface;
                    public short Material;
                    
                    [Flags]
                    public enum FlagsValue : byte
                    {
                        TwoSided = 1 << 0,
                        Invisible = 1 << 1,
                        Climbable = 1 << 2,
                        Breakable = 1 << 3,
                        Invalid = 1 << 4,
                        Conveyor = 1 << 5
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class CollisionEdge : TagStructure
                {
                    public short StartVertex;
                    public short EndVertex;
                    public short ForwardEdge;
                    public short ReverseEdge;
                    public short LeftSurface;
                    public short RightSurface;
                }
                
                [TagStructure(Size = 0x10)]
                public class CollisionVertex : TagStructure
                {
                    public RealPoint3d Point;
                    public short FirstEdge;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                }
            }
            
            [TagStructure(Size = 0x80)]
            public class CollisionBspPhysicsDefinition : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Unknown3;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public short Size1;
                public short Count2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown6;
                public short Size3;
                public short Count4;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown7;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding4;
                public byte[] MoppCodeData;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding5;
            }
            
            [TagStructure(Size = 0x8)]
            public class StructureLeaf : TagStructure
            {
                public short Cluster;
                public short SurfaceReferenceCount;
                public int FirstSurfaceReferenceIndex;
            }
            
            [TagStructure(Size = 0x8)]
            public class StructureSurfaceReference : TagStructure
            {
                public short StripIndex;
                public short LightmapTriangleIndex;
                public int BspNodeIndex;
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class StructureInstancedGeometryInstance : TagStructure
        {
            public float Scale;
            public RealVector3d Forward;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealPoint3d Position;
            public short InstanceDefinition;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unknown1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public int Checksum;
            public StringId Name;
            public PathfindingPolicyValue PathfindingPolicy;
            public LightmappingPolicyValue LightmappingPolicy;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NotInLightprobes = 1 << 0
            }
            
            public enum PathfindingPolicyValue : short
            {
                Cutout,
                Static,
                None
            }
            
            public enum LightmappingPolicyValue : short
            {
                PerPixel,
                PerVertex
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class StructureSoundCluster : TagStructure
        {
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public List<EnclosingPortalDesignatorsBlock> EnclosingPortalDesignators;
            public List<InteriorClusterIndicesBlock> InteriorClusterIndices;
            
            [TagStructure(Size = 0x2)]
            public class EnclosingPortalDesignatorsBlock : TagStructure
            {
                public short PortalDesignator;
            }
            
            [TagStructure(Size = 0x2)]
            public class InteriorClusterIndicesBlock : TagStructure
            {
                public short InteriorClusterIndex;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class TransparentPlane : TagStructure
        {
            public short SectionIndex;
            public short PartIndex;
            public RealPlane3d Plane;
        }
        
        [TagStructure(Size = 0x64)]
        public class StructureDebugInfo : TagStructure
        {
            [TagField(Flags = Padding, Length = 64)]
            public byte[] Padding1;
            public List<StructureClusterDebugInfo> Clusters;
            public List<StructureFogPlaneDebugInfo> FogPlanes;
            public List<StructureFogZoneDebugInfo> FogZones;
            
            [TagStructure(Size = 0x5C)]
            public class StructureClusterDebugInfo : TagStructure
            {
                public ErrorsValue Errors;
                public WarningsValue Warnings;
                [TagField(Flags = Padding, Length = 28)]
                public byte[] Padding1;
                public List<StructureDebugInfoRenderLine> Lines;
                public List<FogPlaneIndicesBlock> FogPlaneIndices;
                public List<VisibleFogPlaneIndicesBlock> VisibleFogPlaneIndices;
                public List<VisFogOmissionClusterIndicesBlock> VisFogOmissionClusterIndices;
                public List<ContainingFogZoneIndicesBlock> ContainingFogZoneIndices;
                
                [Flags]
                public enum ErrorsValue : ushort
                {
                    MultipleFogPlanes = 1 << 0,
                    FogZoneCollision = 1 << 1,
                    FogZoneImmersion = 1 << 2
                }
                
                [Flags]
                public enum WarningsValue : ushort
                {
                    MultipleVisibleFogPlanes = 1 << 0,
                    VisibleFogClusterOmission = 1 << 1,
                    FogPlaneMissedRenderBsp = 1 << 2
                }
                
                [TagStructure(Size = 0x20)]
                public class StructureDebugInfoRenderLine : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum TypeValue : short
                    {
                        FogPlaneBoundaryEdge,
                        FogPlaneInternalEdge,
                        FogZoneFloodfill,
                        FogZoneClusterCentroid,
                        FogZoneClusterGeometry,
                        FogZonePortalCentroid,
                        FogZonePortalGeometry
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class FogPlaneIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class VisibleFogPlaneIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class VisFogOmissionClusterIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class ContainingFogZoneIndicesBlock : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class StructureFogPlaneDebugInfo : TagStructure
            {
                public int FogZoneIndex;
                [TagField(Flags = Padding, Length = 24)]
                public byte[] Padding1;
                public int ConnectedPlaneDesignator;
                public List<StructureDebugInfoRenderLine> Lines;
                public List<IntersectedClusterIndicesBlock> IntersectedClusterIndices;
                public List<InfExtentClusterIndicesBlock> InfExtentClusterIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureDebugInfoRenderLine : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum TypeValue : short
                    {
                        FogPlaneBoundaryEdge,
                        FogPlaneInternalEdge,
                        FogZoneFloodfill,
                        FogZoneClusterCentroid,
                        FogZoneClusterGeometry,
                        FogZonePortalCentroid,
                        FogZonePortalGeometry
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class IntersectedClusterIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class InfExtentClusterIndicesBlock : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x50)]
            public class StructureFogZoneDebugInfo : TagStructure
            {
                public int MediaIndex; // Scenario Fog Plane*
                public int BaseFogPlaneIndex;
                [TagField(Flags = Padding, Length = 24)]
                public byte[] Padding1;
                public List<StructureDebugInfoRenderLine> Lines;
                public List<ImmersedClusterIndicesBlock> ImmersedClusterIndices;
                public List<BoundingFogPlaneIndicesBlock> BoundingFogPlaneIndices;
                public List<CollisionFogPlaneIndicesBlock> CollisionFogPlaneIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureDebugInfoRenderLine : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum TypeValue : short
                    {
                        FogPlaneBoundaryEdge,
                        FogPlaneInternalEdge,
                        FogZoneFloodfill,
                        FogZoneClusterCentroid,
                        FogZoneClusterGeometry,
                        FogZonePortalCentroid,
                        FogZonePortalGeometry
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class ImmersedClusterIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class BoundingFogPlaneIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class CollisionFogPlaneIndicesBlock : TagStructure
                {
                    public int Index;
                }
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class StructurePhysicsStruct : TagStructure
        {
            public byte[] MoppCode;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public RealPoint3d MoppBoundsMin;
            public RealPoint3d MoppBoundsMax;
            public byte[] BreakableSurfacesMoppCode;
            public List<StrucurePhysicsBreakableSurfaceKeyValue> BreakableSurfaceKeyTable;
            
            [TagStructure(Size = 0x20)]
            public class StrucurePhysicsBreakableSurfaceKeyValue : TagStructure
            {
                public short InstancedGeometryIndex;
                public short BreakableSurfaceIndex;
                public int SeedSurfaceIndex;
                public float X0;
                public float X1;
                public float Y0;
                public float Y1;
                public float Z0;
                public float Z1;
            }
        }
        
        [TagStructure(Size = 0xBC)]
        public class WaterDefinition : TagStructure
        {
            public CachedTag Shader;
            public List<WaterGeometryData> Section;
            public GeometryBlockInfoStruct GeometryBlockInfo;
            public RealRgbColor SunSpotColor;
            public RealRgbColor ReflectionTint;
            public RealRgbColor RefractionTint;
            public RealRgbColor HorizonColor;
            public float SunSpecularPower;
            public float ReflectionBumpScale;
            public float RefractionBumpScale;
            public float FresnelScale;
            public float SunDirHeading;
            public float SunDirPitch;
            public float Fov;
            public float Aspect;
            public float Height;
            public float Farz;
            public float RotateOffset;
            public RealVector2d Center;
            public RealVector2d Extents;
            public float FogNear;
            public float FogFar;
            public float DynamicHeightBias;
            
            [TagStructure(Size = 0x6C)]
            public class WaterGeometryData : TagStructure
            {
                public GeometrySection Section;
                
                [TagStructure(Size = 0x6C)]
                public class GeometrySection : TagStructure
                {
                    public List<GeometryPart> Parts;
                    public List<GeometrySubpart> Subparts;
                    public List<GeometryVisibility> VisibilityBounds;
                    public List<GeometryVertex> RawVertices;
                    public List<Word> StripIndices;
                    public byte[] VisibilityMoppCode;
                    public List<Word> MoppReorderTable;
                    public List<RasterizerVertexBuffer> VertexBuffers;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x48)]
                    public class GeometryPart : TagStructure
                    {
                        public TypeValue Type;
                        public FlagsValue Flags;
                        public short Material;
                        public short StripStartIndex;
                        public short StripLength;
                        public short FirstSubpartIndex;
                        public short SubpartCount;
                        public sbyte MaxNodesVertex;
                        public sbyte ContributingCompoundNodeCount;
                        /// <summary>
                        /// CENTROID
                        /// </summary>
                        public RealPoint3d Position;
                        public sbyte NodeIndex;
                        [TagField(Length = 4)]
                        public sbyte NodeIndices;
                        public float NodeWeight;
                        [TagField(Length = 3)]
                        public float NodeWeights;
                        public float LodMipmapMagicNumber;
                        [TagField(Flags = Padding, Length = 24)]
                        public byte[] Unknown3;
                        
                        public enum TypeValue : short
                        {
                            NotDrawn,
                            OpaqueShadowOnly,
                            OpaqueShadowCasting,
                            OpaqueNonshadowing,
                            Transparent,
                            LightmapOnly
                        }
                        
                        [Flags]
                        public enum FlagsValue : ushort
                        {
                            Decalable = 1 << 0,
                            NewPartTypes = 1 << 1,
                            DislikesPhotons = 1 << 2,
                            OverrideTriangleList = 1 << 3,
                            IgnoredByLightmapper = 1 << 4
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class GeometrySubpart : TagStructure
                    {
                        public short IndicesStartIndex;
                        public short IndicesLength;
                        public short VisibilityBoundsIndex;
                        public short PartIndex;
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class GeometryVisibility : TagStructure
                    {
                        public float PositionX;
                        public float PositionY;
                        public float PositionZ;
                        public float Radius;
                        public sbyte Node0;
                        [TagField(Flags = Padding, Length = 3)]
                        public byte[] Padding1;
                    }
                    
                    [TagStructure(Size = 0xC4)]
                    public class GeometryVertex : TagStructure
                    {
                        public RealPoint3d Position;
                        public int NodeIndexOld;
                        [TagField(Length = 4)]
                        public int NodeIndicesOld;
                        public float NodeWeight;
                        [TagField(Length = 4)]
                        public float NodeWeights;
                        public int NodeIndexNew;
                        [TagField(Length = 4)]
                        public int NodeIndicesNew;
                        public int UseNewNodeIndices;
                        public int AdjustedCompoundNodeIndex;
                        public RealPoint2d Texcoord;
                        public RealVector3d Normal;
                        public RealVector3d Binormal;
                        public RealVector3d Tangent;
                        public RealVector3d AnisotropicBinormal;
                        public RealPoint2d SecondaryTexcoord;
                        public RealRgbColor PrimaryLightmapColor;
                        public RealPoint2d PrimaryLightmapTexcoord;
                        public RealVector3d PrimaryLightmapIncidentDirection;
                        [TagField(Flags = Padding, Length = 12)]
                        public byte[] Padding1;
                        [TagField(Flags = Padding, Length = 8)]
                        public byte[] Padding2;
                        [TagField(Flags = Padding, Length = 12)]
                        public byte[] Padding3;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class Word : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class RasterizerVertexBuffer : TagStructure
                    {
                        public VertexBuffer VertexBuffer;
                    }
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class GeometryBlockInfoStruct : TagStructure
            {
                /// <summary>
                /// BLOCK INFO
                /// </summary>
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GeometryBlockResource> Resources;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public short OwnerTagSectionOffset;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                
                [TagStructure(Size = 0x10)]
                public class GeometryBlockResource : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Flags = Padding, Length = 3)]
                    public byte[] Padding1;
                    public short PrimaryLocator;
                    public short SecondaryLocator;
                    public int ResourceDataSize;
                    public int ResourceDataOffset;
                    
                    public enum TypeValue : sbyte
                    {
                        TagBlock,
                        TagData,
                        VertexBuffer
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class StructurePortalDeviceMap : TagStructure
        {
            public List<StructurePortalDeviceMachineAssociation> DevicePortalAssociations;
            public List<GamePortalToPortalMapBlock> GamePortalToPortalMap;
            
            [TagStructure(Size = 0xC)]
            public class StructurePortalDeviceMachineAssociation : TagStructure
            {
                public ObjectIdentifier DeviceId;
                public short FirstGamePortalIndex;
                public short GamePortalCount;
                
                [TagStructure(Size = 0x8)]
                public class ObjectIdentifier : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public TypeValue Type;
                    public SourceValue Source;
                    
                    public enum TypeValue : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Garbage,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        LightFixture,
                        SoundScenery,
                        Crate,
                        Creature
                    }
                    
                    public enum SourceValue : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy
                    }
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class GamePortalToPortalMapBlock : TagStructure
            {
                public short PortalIndex;
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class StructureAudibility : TagStructure
        {
            public int DoorPortalCount;
            public Bounds<float> ClusterDistanceBounds;
            public List<Dword> EncodedDoorPas;
            public List<Dword> ClusterDoorPortalEncodedPas;
            public List<Dword> AiDeafeningPas;
            public List<Byte> ClusterDistances;
            public List<MachineDoorMappingBlock> MachineDoorMapping;
            
            [TagStructure(Size = 0x4)]
            public class Dword : TagStructure
            {
                public int Unknown1;
            }
            
            [TagStructure(Size = 0x1)]
            public class Byte : TagStructure
            {
                public sbyte Unknown1;
            }
            
            [TagStructure(Size = 0x1)]
            public class MachineDoorMappingBlock : TagStructure
            {
                public sbyte MachineDoorIndex;
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class ObjectFakeLightprobe : TagStructure
        {
            public ObjectIdentifierStruct ObjectIdentifier;
            public RenderLightingStruct RenderLighting;
            
            [TagStructure(Size = 0x8)]
            public class ObjectIdentifierStruct : TagStructure
            {
                public int UniqueId;
                public short OriginBspIndex;
                public TypeValue Type;
                public SourceValue Source;
                
                public enum TypeValue : sbyte
                {
                    Biped,
                    Vehicle,
                    Weapon,
                    Equipment,
                    Garbage,
                    Projectile,
                    Scenery,
                    Machine,
                    Control,
                    LightFixture,
                    SoundScenery,
                    Crate,
                    Creature
                }
                
                public enum SourceValue : sbyte
                {
                    Structure,
                    Editor,
                    Dynamic,
                    Legacy
                }
            }
            
            [TagStructure(Size = 0x54)]
            public class RenderLightingStruct : TagStructure
            {
                public RealRgbColor Ambient;
                public RealVector3d ShadowDirection;
                public float LightingAccuracy;
                public float ShadowOpacity;
                public RealRgbColor PrimaryDirectionColor;
                public RealVector3d PrimaryDirection;
                public RealRgbColor SecondaryDirectionColor;
                public RealVector3d SecondaryDirection;
                public short ShIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class DecoratorPlacementDefinition : TagStructure
        {
            public RealPoint3d GridOrigin;
            public int CellCountPerDimension;
            public List<DecoratorCacheBlock> CacheBlocks;
            public List<DecoratorGroup> Groups;
            public List<DecoratorCellCollection> Cells;
            public List<DecoratorProjectedDecal> Decals;
            
            [TagStructure(Size = 0x3C)]
            public class DecoratorCacheBlock : TagStructure
            {
                public GeometryBlockInfoStruct GeometryBlockInfo;
                public List<DecoratorCacheBlockData> CacheBlockData;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x28)]
                public class GeometryBlockInfoStruct : TagStructure
                {
                    /// <summary>
                    /// BLOCK INFO
                    /// </summary>
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GeometryBlockResource> Resources;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding1;
                    public short OwnerTagSectionOffset;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Padding3;
                    
                    [TagStructure(Size = 0x10)]
                    public class GeometryBlockResource : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Flags = Padding, Length = 3)]
                        public byte[] Padding1;
                        public short PrimaryLocator;
                        public short SecondaryLocator;
                        public int ResourceDataSize;
                        public int ResourceDataOffset;
                        
                        public enum TypeValue : sbyte
                        {
                            TagBlock,
                            TagData,
                            VertexBuffer
                        }
                    }
                }
                
                [TagStructure(Size = 0x9C)]
                public class DecoratorCacheBlockData : TagStructure
                {
                    public List<DecoratorPlacement> Placements;
                    public List<RasterizerVertexDecoratorDecal> DecalVertices;
                    public List<Word> DecalIndices;
                    public VertexBuffer DecalVertexBuffer;
                    [TagField(Flags = Padding, Length = 16)]
                    public byte[] Padding1;
                    public List<RasterizerVertexDecoratorSprite> SpriteVertices;
                    public List<Word> SpriteIndices;
                    public VertexBuffer SpriteVertexBuffer;
                    [TagField(Flags = Padding, Length = 16)]
                    public byte[] Padding2;
                    
                    [TagStructure(Size = 0x18)]
                    public class DecoratorPlacement : TagStructure
                    {
                        public int InternalData1;
                        public int CompressedPosition;
                        public ArgbColor TintColor;
                        public ArgbColor LightmapColor;
                        public int CompressedLightDirection;
                        public int CompressedLight2Direction;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class RasterizerVertexDecoratorDecal : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealPoint2d Texcoord0;
                        public RealPoint2d Texcoord1;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class Word : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class RasterizerVertexDecoratorSprite : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealVector3d Offset;
                        public RealVector3d Axis;
                        public RealPoint2d Texcoord;
                        public ArgbColor Color;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorGroup : TagStructure
            {
                public sbyte DecoratorSet;
                public DecoratorTypeValue DecoratorType;
                public sbyte ShaderIndex;
                public sbyte CompressedRadius;
                public short Cluster;
                public short CacheBlock;
                public short DecoratorStartIndex;
                public short DecoratorCount;
                public short VertexStartOffset;
                public short VertexCount;
                public short IndexStartOffset;
                public short IndexCount;
                public int CompressedBoundingCenter;
                
                public enum DecoratorTypeValue : sbyte
                {
                    Model,
                    FloatingDecal,
                    ProjectedDecal,
                    ScreenFacingQuad,
                    AxisRotatingQuad,
                    CrossQuad
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorCellCollection : TagStructure
            {
                public short ChildIndex;
                [TagField(Length = 8)]
                public short ChildIndices;
                public short CacheBlockIndex;
                public short GroupCount;
                public int GroupStartIndex;
            }
            
            [TagStructure(Size = 0x40)]
            public class DecoratorProjectedDecal : TagStructure
            {
                public sbyte DecoratorSet;
                public sbyte DecoratorClass;
                public sbyte DecoratorPermutation;
                public sbyte SpriteIndex;
                public RealPoint3d Position;
                public RealVector3d Left;
                public RealVector3d Up;
                public RealVector3d Extents;
                public RealPoint3d PreviousPosition;
            }
        }
    }
}

