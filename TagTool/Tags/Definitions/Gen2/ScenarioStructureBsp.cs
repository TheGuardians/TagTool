using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Geometry.BspCollisionGeometry;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x23C)]
    public class ScenarioStructureBsp : TagStructure
    {
        public List<GlobalTagImportInfoBlock> ImportInfo;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<StructureCollisionMaterialsBlock> CollisionMaterials;
        public List<CollisionGeometry> CollisionBsp;
        /// <summary>
        /// Height below which vehicles get pushed up by an unstoppable force.
        /// </summary>
        public float VehicleFloor; // World Units
        /// <summary>
        /// Height above which vehicles get pushed down by an unstoppable force.
        /// </summary>
        public float VehicleCeiling; // World Units
        public List<UnusedStructureBspNodeBlock> UnusedNodes;
        public List<StructureBspLeafBlock> Leaves;
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public List<StructureBspSurfaceReferenceBlock> SurfaceReferences;
        public byte[] ClusterData;
        public List<StructureBspClusterPortalBlock> ClusterPortals;
        public List<StructureBspFogPlaneBlock> FogPlanes;
        [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<StructureBspWeatherPaletteBlock> WeatherPalette;
        public List<StructureBspWeatherPolyhedronBlock> WeatherPolyhedra;
        public List<StructureBspDetailObjectDataBlock> DetailObjects;
        public List<StructureBspClusterBlock> Clusters;
        public List<GlobalGeometryMaterialBlock> Materials;
        public List<StructureBspSkyOwnerClusterBlock> SkyOwnerCluster;
        public List<StructureBspConveyorSurfaceBlock> ConveyorSurfaces;
        public List<StructureBspBreakableSurfaceBlock> BreakableSurfaces;
        public List<PathfindingDataBlock> PathfindingData;
        public List<StructureBspPathfindingEdgesBlock> PathfindingEdges;
        public List<StructureBspBackgroundSoundPaletteBlock> BackgroundSoundPalette;
        public List<StructureBspSoundEnvironmentPaletteBlock> SoundEnvironmentPalette;
        public byte[] SoundPasData;
        public List<StructureBspMarkerBlock> Markers;
        public List<StructureBspRuntimeDecalBlock> RuntimeDecals;
        public List<StructureBspEnvironmentObjectPaletteBlock> EnvironmentObjectPalette;
        public List<StructureBspEnvironmentObjectBlock> EnvironmentObjects;
        public List<StructureBspLightmapDataBlock> Lightmaps;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<GlobalMapLeafBlock> LeafMapLeaves;
        public List<GlobalLeafConnectionBlock> LeafMapConnections;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<StructureBspPrecomputedLightingBlock> PrecomputedLighting;
        public List<StructureBspInstancedGeometryDefinitionBlock> InstancedGeometriesDefinitions;
        public List<StructureBspInstancedGeometryInstancesBlock> InstancedGeometryInstances;
        public List<StructureBspSoundClusterBlock> AmbienceSoundClusters;
        public List<StructureBspSoundClusterBlock1> ReverbSoundClusters;
        public List<TransparentPlanesBlock> TransparentPlanes;
        [TagField(Length = 0x60, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        /// <summary>
        /// Distances this far and longer from limit origin will pull you back in.
        /// </summary>
        public float VehicleSpericalLimitRadius;
        /// <summary>
        /// Center of space in which vehicle can move.
        /// </summary>
        public RealPoint3d VehicleSpericalLimitCenter;
        public List<StructureBspDebugInfoBlock> DebugInfo;
        [TagField(ValidTags = new [] { "DECP" })]
        public CachedTag Decorators;
        public GlobalStructurePhysicsStructBlock StructurePhysics;
        public List<GlobalWaterDefinitionsBlock> WaterDefinitions;
        public List<StructurePortalDeviceMappingBlock> PortalDeviceMapping;
        public List<StructureBspAudibilityBlock> Audibility;
        public List<StructureBspFakeLightprobesBlock> ObjectFakeLightprobes;
        public List<DecoratorPlacementDefinitionBlock> DecoratorPlacements;
        
        [TagStructure(Size = 0x250)]
        public class GlobalTagImportInfoBlock : TagStructure
        {
            public int Build;
            [TagField(Length = 256)]
            public string Version;
            [TagField(Length = 32)]
            public string ImportDate;
            [TagField(Length = 32)]
            public string Culprit;
            [TagField(Length = 0x60, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string ImportTime;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<TagImportFileBlock> Files;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x210)]
            public class TagImportFileBlock : TagStructure
            {
                [TagField(Length = 256)]
                public string Path;
                [TagField(Length = 32)]
                public string ModificationDate;
                [TagField(Length = 0x8)]
                public byte[] Unknown;
                [TagField(Length = 0x58, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Checksum; // crc32
                public int Size; // bytes
                public byte[] ZippedData;
                [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class StructureCollisionMaterialsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag OldShader;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short ConveyorSurfaceIndex;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag NewShader;
        }
        
        [TagStructure(Size = 0x40)]
        public class GlobalCollisionBspBlock : TagStructure
        {
            public List<Bsp3dNodesBlock> Bsp3dNodes;
            public List<PlanesBlock> Planes;
            public List<LeavesBlock> Leaves;
            public List<Bsp2dReferencesBlock> Bsp2dReferences;
            public List<Bsp2dNodesBlock> Bsp2dNodes;
            public List<SurfacesBlock> Surfaces;
            public List<EdgesBlock> Edges;
            public List<VerticesBlock> Vertices;
            
            [TagStructure(Size = 0x8)]
            public class Bsp3dNodesBlock : TagStructure
            {
                [TagField(Length = 0x8)]
                public byte[] Unknown;
            }
            
            [TagStructure(Size = 0x10)]
            public class PlanesBlock : TagStructure
            {
                public RealPlane3d Plane;
            }
            
            [TagStructure(Size = 0x4)]
            public class LeavesBlock : TagStructure
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
            public class Bsp2dReferencesBlock : TagStructure
            {
                public short Plane;
                public short Bsp2dNode;
            }
            
            [TagStructure(Size = 0x10)]
            public class Bsp2dNodesBlock : TagStructure
            {
                public RealPlane2d Plane;
                public short LeftChild;
                public short RightChild;
            }
            
            [TagStructure(Size = 0x8)]
            public class SurfacesBlock : TagStructure
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
            public class EdgesBlock : TagStructure
            {
                public short StartVertex;
                public short EndVertex;
                public short ForwardEdge;
                public short ReverseEdge;
                public short LeftSurface;
                public short RightSurface;
            }
            
            [TagStructure(Size = 0x10)]
            public class VerticesBlock : TagStructure
            {
                public RealPoint3d Point;
                public short FirstEdge;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x6)]
        public class UnusedStructureBspNodeBlock : TagStructure
        {
            [TagField(Length = 0x6)]
            public byte[] Unknown;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureBspLeafBlock : TagStructure
        {
            public short Cluster;
            public short SurfaceReferenceCount;
            public int FirstSurfaceReferenceIndex;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureBspSurfaceReferenceBlock : TagStructure
        {
            public short StripIndex;
            public short LightmapTriangleIndex;
            public int BspNodeIndex;
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureBspClusterPortalBlock : TagStructure
        {
            public short BackCluster;
            public short FrontCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public FlagsValue Flags;
            public List<StructureBspClusterPortalVertexBlock> Vertices;
            
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
            public class StructureBspClusterPortalVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureBspFogPlaneBlock : TagStructure
        {
            public short ScenarioPlanarFogIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        
        [TagStructure(Size = 0x88)]
        public class StructureBspWeatherPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "weat" })]
            public CachedTag WeatherSystem;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "wind" })]
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureBspWeatherPolyhedronBlock : TagStructure
        {
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public List<StructureBspWeatherPolyhedronPlaneBlock> Planes;
            
            [TagStructure(Size = 0x10)]
            public class StructureBspWeatherPolyhedronPlaneBlock : TagStructure
            {
                public RealPlane3d Plane;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureBspDetailObjectDataBlock : TagStructure
        {
            public List<GlobalDetailObjectCellsBlock> Cells;
            public List<GlobalDetailObjectBlock> Instances;
            public List<GlobalDetailObjectCountsBlock> Counts;
            public List<GlobalZReferenceVectorBlock> ZReferenceVectors;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x20)]
            public class GlobalDetailObjectCellsBlock : TagStructure
            {
                public short Unknown;
                public short Unknown1;
                public short Unknown2;
                public short Unknown3;
                public int Unknown4;
                public int Unknown5;
                public int Unknown6;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x6)]
            public class GlobalDetailObjectBlock : TagStructure
            {
                public sbyte Unknown;
                public sbyte Unknown1;
                public sbyte Unknown2;
                public sbyte Unknown3;
                public short Unknown4;
            }
            
            [TagStructure(Size = 0x2)]
            public class GlobalDetailObjectCountsBlock : TagStructure
            {
                public short Unknown;
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalZReferenceVectorBlock : TagStructure
            {
                public float Unknown;
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
            }
        }
        
        [TagStructure(Size = 0xB0)]
        public class StructureBspClusterBlock : TagStructure
        {
            public GlobalGeometrySectionInfoStructBlock SectionInfo;
            public CacheFileResourceGen2 GeometryBlockInfo;
            public List<StructureBspClusterDataBlockNew> ClusterData;
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public List<PredictedResourceBlock> PredictedResources;
            public List<StructureBspClusterPortalIndexBlock> Portals;
            public int ChecksumFromStructure;
            public List<StructureBspClusterInstancedGeometryIndexBlock> InstancedGeometryIndices;
            public List<GlobalGeometrySectionStripIndexBlock> IndexReorderTable;
            public byte[] CollisionMoppCode;
            
            [TagStructure(Size = 0x28)]
            public class GlobalGeometrySectionInfoStructBlock : TagStructure
            {
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
                public List<TagTool.Geometry.RenderGeometryCompression> Compression;
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
                public class GlobalGeometryCompressionInfoBlock : TagStructure
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
            
            [TagStructure(Size = 0x24)]
            public class GlobalGeometryBlockInfoStructBlock : TagStructure
            {
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GlobalGeometryBlockResourceBlock> Resources;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short OwnerTagSectionOffset;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x10)]
                public class GlobalGeometryBlockResourceBlock : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
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
            
            [TagStructure(Size = 0x44)]
            public class StructureBspClusterDataBlockNew : TagStructure
            {
                public GlobalGeometrySectionStructBlock Section;
                
                [TagStructure(Size = 0x44)]
                public class GlobalGeometrySectionStructBlock : TagStructure
                {
                    public List<GlobalGeometryPartBlockNew> Parts;
                    public List<GlobalSubpartsBlock> Subparts;
                    public List<GlobalVisibilityBoundsBlock> VisibilityBounds;
                    public List<GlobalGeometrySectionRawVertexBlock> RawVertices;
                    public List<GlobalGeometrySectionStripIndexBlock> StripIndices;
                    public byte[] VisibilityMoppCode;
                    public List<GlobalGeometrySectionStripIndexBlock1> MoppReorderTable;
                    public List<GlobalGeometrySectionVertexBufferBlock> VertexBuffers;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x48)]
                    public class GlobalGeometryPartBlockNew : TagStructure
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
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public sbyte[] NodeIndex;
                        [TagField(Length = 3)]
                        public float[] NodeWeight;
                        public float LodMipmapMagicNumber;
                        [TagField(Length = 0x18)]
                        public byte[] Unknown;
                        
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
                    public class GlobalSubpartsBlock : TagStructure
                    {
                        public short IndicesStartIndex;
                        public short IndicesLength;
                        public short VisibilityBoundsIndex;
                        public short PartIndex;
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class GlobalVisibilityBoundsBlock : TagStructure
                    {
                        public float PositionX;
                        public float PositionY;
                        public float PositionZ;
                        public float Radius;
                        public sbyte Node0;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0xC4)]
                    public class GlobalGeometrySectionRawVertexBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public int[] NodeIndexOld;
                        [TagField(Length = 4)]
                        public float[] NodeWeight;
                        [TagField(Length = 4)]
                        public int[] NodeIndexNew;
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
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding2;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class GlobalGeometrySectionStripIndexBlock : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class GlobalGeometrySectionStripIndexBlock1 : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class GlobalGeometrySectionVertexBufferBlock : TagStructure
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
            public class PredictedResourceBlock : TagStructure
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
            public class StructureBspClusterPortalIndexBlock : TagStructure
            {
                public short PortalIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class StructureBspClusterInstancedGeometryIndexBlock : TagStructure
            {
                public short InstancedGeometryIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class GlobalGeometrySectionStripIndexBlock : TagStructure
            {
                public short Index;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class GlobalGeometryMaterialBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag OldShader;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
            public List<GlobalGeometryMaterialPropertyBlock> Properties;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public sbyte BreakableSurfaceIndex;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x8)]
            public class GlobalGeometryMaterialPropertyBlock : TagStructure
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
        public class StructureBspSkyOwnerClusterBlock : TagStructure
        {
            public short ClusterOwner;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureBspConveyorSurfaceBlock : TagStructure
        {
            public RealVector3d U;
            public RealVector3d V;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureBspBreakableSurfaceBlock : TagStructure
        {
            public short InstancedGeometryInstance;
            public short BreakableSurfaceIndex;
            public RealPoint3d Centroid;
            public float Radius;
            public int CollisionSurfaceIndex;
        }
        
        [TagStructure(Size = 0x74)]
        public class PathfindingDataBlock : TagStructure
        {
            public List<SectorBlock> Sectors;
            public List<SectorLinkBlock> Links;
            public List<RefBlock> Refs;
            public List<SectorBsp2dNodesBlock> Bsp2dNodes;
            public List<SurfaceFlagsBlock> SurfaceFlags;
            public List<SectorVertexBlock> Vertices;
            public List<EnvironmentObjectRefs> ObjectRefs;
            public List<PathfindingHintsBlock> PathfindingHints;
            public List<InstancedGeometryReferenceBlock> InstancedGeometryRefs;
            public int StructureChecksum;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<UserHintBlock> UserPlacedHints;
            
            [TagStructure(Size = 0x8)]
            public class SectorBlock : TagStructure
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
            public class SectorLinkBlock : TagStructure
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
            public class RefBlock : TagStructure
            {
                public int NodeRefOrSectorRef;
            }
            
            [TagStructure(Size = 0x14)]
            public class SectorBsp2dNodesBlock : TagStructure
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }
            
            [TagStructure(Size = 0x4)]
            public class SurfaceFlagsBlock : TagStructure
            {
                public int Flags;
            }
            
            [TagStructure(Size = 0xC)]
            public class SectorVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
            
            [TagStructure(Size = 0x1C)]
            public class EnvironmentObjectRefs : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int FirstSector;
                public int LastSector;
                public List<EnvironmentObjectBspRefs> Bsps;
                public List<EnvironmentObjectNodes> Nodes;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Mobile = 1 << 0
                }
                
                [TagStructure(Size = 0x10)]
                public class EnvironmentObjectBspRefs : TagStructure
                {
                    public int BspReference;
                    public int FirstSector;
                    public int LastSector;
                    public short NodeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x4)]
                public class EnvironmentObjectNodes : TagStructure
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
            public class PathfindingHintsBlock : TagStructure
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
            public class InstancedGeometryReferenceBlock : TagStructure
            {
                public short PathfindingObjectIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x48)]
            public class UserHintBlock : TagStructure
            {
                public List<UserHintPointBlock> PointGeometry;
                public List<UserHintRayBlock> RayGeometry;
                public List<UserHintLineSegmentBlock> LineSegmentGeometry;
                public List<UserHintParallelogramBlock> ParallelogramGeometry;
                public List<UserHintPolygonBlock> PolygonGeometry;
                public List<UserHintJumpBlock> JumpHints;
                public List<UserHintClimbBlock> ClimbHints;
                public List<UserHintWellBlock> WellHints;
                public List<UserHintFlightBlock> FlightHints;
                
                [TagStructure(Size = 0x10)]
                public class UserHintPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintRayBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealVector3d Vector;
                }
                
                [TagStructure(Size = 0x24)]
                public class UserHintLineSegmentBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class UserHintParallelogramBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public RealPoint3d Point0;
                    public short ReferenceFrame;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point1;
                    public short ReferenceFrame1;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    public RealPoint3d Point2;
                    public short ReferenceFrame2;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding2;
                    public RealPoint3d Point3;
                    public short ReferenceFrame3;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding3;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class UserHintPolygonBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintPointBlock> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0,
                        Closed = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class UserHintPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public short ReferenceFrame;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class UserHintJumpBlock : TagStructure
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
                public class UserHintClimbBlock : TagStructure
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
                
                [TagStructure(Size = 0xC)]
                public class UserHintWellBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public List<UserHintWellPointBlock> Points;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Bidirectional = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class UserHintWellPointBlock : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealVector3d Point;
                        public short ReferenceFrame;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
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
                
                [TagStructure(Size = 0x8)]
                public class UserHintFlightBlock : TagStructure
                {
                    public List<UserHintFlightPointBlock> Points;
                    
                    [TagStructure(Size = 0xC)]
                    public class UserHintFlightPointBlock : TagStructure
                    {
                        public RealVector3d Point;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class StructureBspPathfindingEdgesBlock : TagStructure
        {
            public sbyte Midpoint;
        }
        
        [TagStructure(Size = 0x64)]
        public class StructureBspBackgroundSoundPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag BackgroundSound;
            /// <summary>
            /// Play only when player is inside cluster.
            /// </summary>
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag InsideClusterSound;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class StructureBspSoundEnvironmentPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "snde" })]
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x3C)]
        public class StructureBspMarkerBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspRuntimeDecalBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Unknown;
        }
        
        [TagStructure(Size = 0x14)]
        public class StructureBspEnvironmentObjectPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag Definition;
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag Model;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x68)]
        public class StructureBspEnvironmentObjectBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public short PaletteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int UniqueId;
            public Tag ExportedObjectType;
            [TagField(Length = 32)]
            public string ScenarioObjectName;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureBspLightmapDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BitmapGroup;
        }
        
        [TagStructure(Size = 0x10)]
        public class GlobalMapLeafBlock : TagStructure
        {
            public List<MapLeafFaceBlock> Faces;
            public List<MapLeafConnectionIndexBlock> ConnectionIndices;
            
            [TagStructure(Size = 0xC)]
            public class MapLeafFaceBlock : TagStructure
            {
                public int NodeIndex;
                public List<MapLeafFaceVertexBlock> Vertices;
                
                [TagStructure(Size = 0xC)]
                public class MapLeafFaceVertexBlock : TagStructure
                {
                    public RealPoint3d Vertex;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class MapLeafConnectionIndexBlock : TagStructure
            {
                public int ConnectionIndex;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalLeafConnectionBlock : TagStructure
        {
            public int PlaneIndex;
            public int BackLeafIndex;
            public int FrontLeafIndex;
            public List<LeafConnectionVertexBlock> Vertices;
            public float Area;
            
            [TagStructure(Size = 0xC)]
            public class LeafConnectionVertexBlock : TagStructure
            {
                public RealPoint3d Vertex;
            }
        }
        
        [TagStructure(Size = 0x2A4)]
        public class GlobalErrorReportCategoriesBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ReportTypeValue ReportType;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x194, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public List<ErrorReportsBlock> Reports;
            
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
            
            [TagStructure(Size = 0x260)]
            public class ErrorReportsBlock : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public byte[] Text;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVerticesBlock> Vertices;
                public List<ErrorReportVectorsBlock> Vectors;
                public List<ErrorReportLinesBlock> Lines;
                public List<ErrorReportTrianglesBlock> Triangles;
                public List<ErrorReportQuadsBlock> Quads;
                public List<ErrorReportCommentsBlock> Comments;
                [TagField(Length = 0x17C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                [TagField(Length = 0x54, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
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
                public class ErrorReportVerticesBlock : TagStructure
                {
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                    public float ScreenSize;
                }
                
                [TagStructure(Size = 0x40)]
                public class ErrorReportVectorsBlock : TagStructure
                {
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                }
                
                [TagStructure(Size = 0x3C)]
                public class ErrorReportLinesBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 2)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x48)]
                public class ErrorReportTrianglesBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 3)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x54)]
                public class ErrorReportQuadsBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 4)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x38)]
                public class ErrorReportCommentsBlock : TagStructure
                {
                    public byte[] Text;
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                }
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class StructureBspPrecomputedLightingBlock : TagStructure
        {
            public int Index;
            public LightTypeValue LightType;
            public sbyte AttachmentIndex;
            public sbyte ObjectType;
            public VisibilityStructBlock Visibility;
            
            public enum LightTypeValue : short
            {
                FreeStanding,
                AttachedToEditorObject,
                AttachedToStructureObject
            }
            
            [TagStructure(Size = 0x28)]
            public class VisibilityStructBlock : TagStructure
            {
                public short ProjectionCount;
                public short ClusterCount;
                public short VolumeCount;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public byte[] Projections;
                public byte[] VisibilityClusters;
                public byte[] ClusterRemapTable;
                public byte[] VisibilityVolumes;
            }
        }
        
        [TagStructure(Size = 0xC8)]
        public class StructureBspInstancedGeometryDefinitionBlock : TagStructure
        {
            public StructureInstancedGeometryRenderInfoStructBlock RenderInfo;
            public int Checksum;
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public CollisionGeometry CollisionInfo;
            public List<CollisionBspPhysicsBlock> BspPhysics;
            public List<StructureBspLeafBlock> RenderLeaves;
            public List<StructureBspSurfaceReferenceBlock> SurfaceReferences;
            
            [TagStructure(Size = 0x5C)]
            public class StructureInstancedGeometryRenderInfoStructBlock : TagStructure
            {
                public GlobalGeometrySectionInfoStructBlock SectionInfo;
                public CacheFileResourceGen2 GeometryBlockInfo;
                public List<StructureBspClusterDataBlockNew> RenderData;
                public List<GlobalGeometrySectionStripIndexBlock> IndexReorderTable;
                
                [TagStructure(Size = 0x28)]
                public class GlobalGeometrySectionInfoStructBlock : TagStructure
                {
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
                    public List<TagTool.Geometry.RenderGeometryCompression> Compression;
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
                    public class GlobalGeometryCompressionInfoBlock : TagStructure
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
                
                [TagStructure(Size = 0x24)]
                public class GlobalGeometryBlockInfoStructBlock : TagStructure
                {
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GlobalGeometryBlockResourceBlock> Resources;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short OwnerTagSectionOffset;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding2;
                    
                    [TagStructure(Size = 0x10)]
                    public class GlobalGeometryBlockResourceBlock : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
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
                
                [TagStructure(Size = 0x44)]
                public class StructureBspClusterDataBlockNew : TagStructure
                {
                    public GlobalGeometrySectionStructBlock Section;
                    
                    [TagStructure(Size = 0x44)]
                    public class GlobalGeometrySectionStructBlock : TagStructure
                    {
                        public List<GlobalGeometryPartBlockNew> Parts;
                        public List<GlobalSubpartsBlock> Subparts;
                        public List<GlobalVisibilityBoundsBlock> VisibilityBounds;
                        public List<GlobalGeometrySectionRawVertexBlock> RawVertices;
                        public List<GlobalGeometrySectionStripIndexBlock> StripIndices;
                        public byte[] VisibilityMoppCode;
                        public List<GlobalGeometrySectionStripIndexBlock1> MoppReorderTable;
                        public List<GlobalGeometrySectionVertexBufferBlock> VertexBuffers;
                        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [TagStructure(Size = 0x48)]
                        public class GlobalGeometryPartBlockNew : TagStructure
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
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public sbyte[] NodeIndex;
                            [TagField(Length = 3)]
                            public float[] NodeWeight;
                            public float LodMipmapMagicNumber;
                            [TagField(Length = 0x18)]
                            public byte[] Unknown;
                            
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
                        public class GlobalSubpartsBlock : TagStructure
                        {
                            public short IndicesStartIndex;
                            public short IndicesLength;
                            public short VisibilityBoundsIndex;
                            public short PartIndex;
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class GlobalVisibilityBoundsBlock : TagStructure
                        {
                            public float PositionX;
                            public float PositionY;
                            public float PositionZ;
                            public float Radius;
                            public sbyte Node0;
                            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                        }
                        
                        [TagStructure(Size = 0xC4)]
                        public class GlobalGeometrySectionRawVertexBlock : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public int[] NodeIndexOld;
                            [TagField(Length = 4)]
                            public float[] NodeWeight;
                            [TagField(Length = 4)]
                            public int[] NodeIndexNew;
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
                            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding1;
                            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding2;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class GlobalGeometrySectionStripIndexBlock : TagStructure
                        {
                            public short Index;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class GlobalGeometrySectionStripIndexBlock1 : TagStructure
                        {
                            public short Index;
                        }
                        
                        [TagStructure(Size = 0x20)]
                        public class GlobalGeometrySectionVertexBufferBlock : TagStructure
                        {
                            public VertexBuffer VertexBuffer;
                        }
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class GlobalGeometrySectionStripIndexBlock : TagStructure
                {
                    public short Index;
                }
            }
            
            [TagStructure(Size = 0x40)]
            public class GlobalCollisionBspStructBlock : TagStructure
            {
                public List<Bsp3dNodesBlock> Bsp3dNodes;
                public List<PlanesBlock> Planes;
                public List<LeavesBlock> Leaves;
                public List<Bsp2dReferencesBlock> Bsp2dReferences;
                public List<Bsp2dNodesBlock> Bsp2dNodes;
                public List<SurfacesBlock> Surfaces;
                public List<EdgesBlock> Edges;
                public List<VerticesBlock> Vertices;
                
                [TagStructure(Size = 0x8)]
                public class Bsp3dNodesBlock : TagStructure
                {
                    [TagField(Length = 0x8)]
                    public byte[] Unknown;
                }
                
                [TagStructure(Size = 0x10)]
                public class PlanesBlock : TagStructure
                {
                    public RealPlane3d Plane;
                }
                
                [TagStructure(Size = 0x4)]
                public class LeavesBlock : TagStructure
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
                public class Bsp2dReferencesBlock : TagStructure
                {
                    public short Plane;
                    public short Bsp2dNode;
                }
                
                [TagStructure(Size = 0x10)]
                public class Bsp2dNodesBlock : TagStructure
                {
                    public RealPlane2d Plane;
                    public short LeftChild;
                    public short RightChild;
                }
                
                [TagStructure(Size = 0x8)]
                public class SurfacesBlock : TagStructure
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
                public class EdgesBlock : TagStructure
                {
                    public short StartVertex;
                    public short EndVertex;
                    public short ForwardEdge;
                    public short ReverseEdge;
                    public short LeftSurface;
                    public short RightSurface;
                }
                
                [TagStructure(Size = 0x10)]
                public class VerticesBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public short FirstEdge;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0x74)]
            public class CollisionBspPhysicsBlock : TagStructure
            {
                [TagField(Length = 0x4)]
                public byte[] Unknown;
                public short Size;
                public short Count;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;

                public RealQuaternion AABB_Center;
                public RealQuaternion AABB_Half_Extents;

                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                public short Size1;
                public short Count1;
                [TagField(Length = 0x4)]
                public byte[] Unknown4;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                [TagField(Length = 0x4)]
                public byte[] Unknown5;
                public short Size2;
                public short Count2;
                [TagField(Length = 0x4)]
                public byte[] Unknown6;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public byte[] MoppCodeData;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
            }
            
            [TagStructure(Size = 0x8)]
            public class StructureBspLeafBlock : TagStructure
            {
                public short Cluster;
                public short SurfaceReferenceCount;
                public int FirstSurfaceReferenceIndex;
            }
            
            [TagStructure(Size = 0x8)]
            public class StructureBspSurfaceReferenceBlock : TagStructure
            {
                public short StripIndex;
                public short LightmapTriangleIndex;
                public int BspNodeIndex;
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class StructureBspInstancedGeometryInstancesBlock : TagStructure
        {
            public float Scale;
            public RealVector3d Forward;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealPoint3d Position;
            public short InstanceDefinition;
            public FlagsValue Flags;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public RealPoint3d WorldBoundingSphereCenter;
            public float BoundingSphereRadius;

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
        
        [TagStructure(Size = 0x14)]
        public class StructureBspSoundClusterBlock : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<StructureSoundClusterPortalDesignators> EnclosingPortalDesignators;
            public List<StructureSoundClusterInteriorClusterIndices> InteriorClusterIndices;
            
            [TagStructure(Size = 0x2)]
            public class StructureSoundClusterPortalDesignators : TagStructure
            {
                public short PortalDesignator;
            }
            
            [TagStructure(Size = 0x2)]
            public class StructureSoundClusterInteriorClusterIndices : TagStructure
            {
                public short InteriorClusterIndex;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class StructureBspSoundClusterBlock1 : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<StructureSoundClusterPortalDesignators> EnclosingPortalDesignators;
            public List<StructureSoundClusterInteriorClusterIndices> InteriorClusterIndices;
            
            [TagStructure(Size = 0x2)]
            public class StructureSoundClusterPortalDesignators : TagStructure
            {
                public short PortalDesignator;
            }
            
            [TagStructure(Size = 0x2)]
            public class StructureSoundClusterInteriorClusterIndices : TagStructure
            {
                public short InteriorClusterIndex;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class TransparentPlanesBlock : TagStructure
        {
            public short SectionIndex;
            public short PartIndex;
            public RealPlane3d Plane;
        }
        
        [TagStructure(Size = 0x58)]
        public class StructureBspDebugInfoBlock : TagStructure
        {
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<StructureBspClusterDebugInfoBlock> Clusters;
            public List<StructureBspFogPlaneDebugInfoBlock> FogPlanes;
            public List<StructureBspFogZoneDebugInfoBlock> FogZones;
            
            [TagStructure(Size = 0x48)]
            public class StructureBspClusterDebugInfoBlock : TagStructure
            {
                public ErrorsValue Errors;
                public WarningsValue Warnings;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> FogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock1> VisibleFogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock2> VisFogOmissionClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock3> ContainingFogZoneIndices;
                
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
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
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
                public class StructureBspDebugInfoIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock1 : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock2 : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock3 : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x38)]
            public class StructureBspFogPlaneDebugInfoBlock : TagStructure
            {
                public int FogZoneIndex;
                [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int ConnectedPlaneDesignator;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> IntersectedClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock1> InfExtentClusterIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
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
                public class StructureBspDebugInfoIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock1 : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x40)]
            public class StructureBspFogZoneDebugInfoBlock : TagStructure
            {
                public int MediaIndex; // Scenario Fog Plane*
                public int BaseFogPlaneIndex;
                [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> ImmersedClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock1> BoundingFogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock2> CollisionFogPlaneIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public TypeValue Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
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
                public class StructureBspDebugInfoIndicesBlock : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock1 : TagStructure
                {
                    public int Index;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureBspDebugInfoIndicesBlock2 : TagStructure
                {
                    public int Index;
                }
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class GlobalStructurePhysicsStructBlock : TagStructure
        {
            public byte[] MoppCode;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d MoppBoundsMin;
            public RealPoint3d MoppBoundsMax;
            public byte[] BreakableSurfacesMoppCode;
            public List<BreakableSurfaceKeyTableBlock> BreakableSurfaceKeyTable;
            
            [TagStructure(Size = 0x20)]
            public class BreakableSurfaceKeyTableBlock : TagStructure
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
        
        [TagStructure(Size = 0xAC)]
        public class GlobalWaterDefinitionsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
            public List<WaterGeometrySectionBlock> Section;
            public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
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
            
            [TagStructure(Size = 0x44)]
            public class WaterGeometrySectionBlock : TagStructure
            {
                public GlobalGeometrySectionStructBlock Section;
                
                [TagStructure(Size = 0x44)]
                public class GlobalGeometrySectionStructBlock : TagStructure
                {
                    public List<GlobalGeometryPartBlockNew> Parts;
                    public List<GlobalSubpartsBlock> Subparts;
                    public List<GlobalVisibilityBoundsBlock> VisibilityBounds;
                    public List<GlobalGeometrySectionRawVertexBlock> RawVertices;
                    public List<GlobalGeometrySectionStripIndexBlock> StripIndices;
                    public byte[] VisibilityMoppCode;
                    public List<GlobalGeometrySectionStripIndexBlock1> MoppReorderTable;
                    public List<GlobalGeometrySectionVertexBufferBlock> VertexBuffers;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x48)]
                    public class GlobalGeometryPartBlockNew : TagStructure
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
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public sbyte[] NodeIndex;
                        [TagField(Length = 3)]
                        public float[] NodeWeight;
                        public float LodMipmapMagicNumber;
                        [TagField(Length = 0x18)]
                        public byte[] Unknown;
                        
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
                    public class GlobalSubpartsBlock : TagStructure
                    {
                        public short IndicesStartIndex;
                        public short IndicesLength;
                        public short VisibilityBoundsIndex;
                        public short PartIndex;
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class GlobalVisibilityBoundsBlock : TagStructure
                    {
                        public float PositionX;
                        public float PositionY;
                        public float PositionZ;
                        public float Radius;
                        public sbyte Node0;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0xC4)]
                    public class GlobalGeometrySectionRawVertexBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public int[] NodeIndexOld;
                        [TagField(Length = 4)]
                        public float[] NodeWeight;
                        [TagField(Length = 4)]
                        public int[] NodeIndexNew;
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
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding2;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class GlobalGeometrySectionStripIndexBlock : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class GlobalGeometrySectionStripIndexBlock1 : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class GlobalGeometrySectionVertexBufferBlock : TagStructure
                    {
                        public VertexBuffer VertexBuffer;
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class GlobalGeometryBlockInfoStructBlock : TagStructure
            {
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GlobalGeometryBlockResourceBlock> Resources;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short OwnerTagSectionOffset;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x10)]
                public class GlobalGeometryBlockResourceBlock : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
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
        
        [TagStructure(Size = 0x10)]
        public class StructurePortalDeviceMappingBlock : TagStructure
        {
            public List<StructureDevicePortalAssociationBlock> DevicePortalAssociations;
            public List<GamePortalToPortalMappingBlock> GamePortalToPortalMap;
            
            [TagStructure(Size = 0xC)]
            public class StructureDevicePortalAssociationBlock : TagStructure
            {
                public ScenarioObjectIdStructBlock DeviceId;
                public short FirstGamePortalIndex;
                public short GamePortalCount;
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStructBlock : TagStructure
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
            public class GamePortalToPortalMappingBlock : TagStructure
            {
                public short PortalIndex;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class StructureBspAudibilityBlock : TagStructure
        {
            public int DoorPortalCount;
            public Bounds<float> ClusterDistanceBounds;
            public List<DoorEncodedPasBlock> EncodedDoorPas;
            public List<ClusterDoorPortalEncodedPasBlock> ClusterDoorPortalEncodedPas;
            public List<AiDeafeningEncodedPasBlock> AiDeafeningPas;
            public List<EncodedClusterDistancesBlock> ClusterDistances;
            public List<OccluderToMachineDoorMapping> MachineDoorMapping;
            
            [TagStructure(Size = 0x4)]
            public class DoorEncodedPasBlock : TagStructure
            {
                public int Unknown;
            }
            
            [TagStructure(Size = 0x4)]
            public class ClusterDoorPortalEncodedPasBlock : TagStructure
            {
                public int Unknown;
            }
            
            [TagStructure(Size = 0x4)]
            public class AiDeafeningEncodedPasBlock : TagStructure
            {
                public int Unknown;
            }
            
            [TagStructure(Size = 0x1)]
            public class EncodedClusterDistancesBlock : TagStructure
            {
                public sbyte Unknown;
            }
            
            [TagStructure(Size = 0x1)]
            public class OccluderToMachineDoorMapping : TagStructure
            {
                public sbyte MachineDoorIndex;
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class StructureBspFakeLightprobesBlock : TagStructure
        {
            public ScenarioObjectIdStructBlock ObjectIdentifier;
            public RenderLightingStructBlock RenderLighting;
            
            [TagStructure(Size = 0x8)]
            public class ScenarioObjectIdStructBlock : TagStructure
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
            public class RenderLightingStructBlock : TagStructure
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class DecoratorPlacementDefinitionBlock : TagStructure
        {
            public RealPoint3d GridOrigin;
            public int CellCountPerDimension;
            public List<DecoratorCacheBlockBlock> CacheBlocks;
            public List<DecoratorGroupBlock> Groups;
            public List<DecoratorCellCollectionBlock> Cells;
            public List<DecoratorProjectedDecalBlock> Decals;
            
            [TagStructure(Size = 0x34, MinVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.Halo2Xbox)]
            public class DecoratorCacheBlockBlock : TagStructure
            {
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<DecoratorCacheBlockDataBlock> CacheBlockData;
                [TagField(MinVersion = CacheVersion.Halo2Vista, Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x24)]
                public class GlobalGeometryBlockInfoStructBlock : TagStructure
                {
                    public int BlockOffset;
                    public int BlockSize;
                    public int SectionDataSize;
                    public int ResourceDataSize;
                    public List<GlobalGeometryBlockResourceBlock> Resources;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short OwnerTagSectionOffset;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding2;
                    
                    [TagStructure(Size = 0x10)]
                    public class GlobalGeometryBlockResourceBlock : TagStructure
                    {
                        public TypeValue Type;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
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
                
                [TagStructure(Size = 0x88)]
                public class DecoratorCacheBlockDataBlock : TagStructure
                {
                    public List<DecoratorPlacementBlock> Placements;
                    public List<DecalVerticesBlock> DecalVertices;
                    public List<IndicesBlock> DecalIndices;
                    public VertexBuffer DecalVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<SpriteVerticesBlock> SpriteVertices;
                    public List<IndicesBlock1> SpriteIndices;
                    public VertexBuffer SpriteVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x18)]
                    public class DecoratorPlacementBlock : TagStructure
                    {
                        public int InternalData1;
                        public int CompressedPosition;
                        public ArgbColor TintColor;
                        public ArgbColor LightmapColor;
                        public int CompressedLightDirection;
                        public int CompressedLight2Direction;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class DecalVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealPoint2d Texcoord0;
                        public RealPoint2d Texcoord1;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class SpriteVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealVector3d Offset;
                        public RealVector3d Axis;
                        public RealPoint2d Texcoord;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock1 : TagStructure
                    {
                        public short Index;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorGroupBlock : TagStructure
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
            public class DecoratorCellCollectionBlock : TagStructure
            {
                [TagField(Length = 8)]
                public short[] ChildIndex;
                public short CacheBlockIndex;
                public short GroupCount;
                public int GroupStartIndex;
            }
            
            [TagStructure(Size = 0x40)]
            public class DecoratorProjectedDecalBlock : TagStructure
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

