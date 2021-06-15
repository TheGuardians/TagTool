using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x5B0)]
    public class ScenarioStructureBsp : TagStructure
    {
        public StructureManifestBuildIdentifierStruct BuildIdentifier;
        public StructureManifestBuildIdentifierStruct ParentBuildIdentifier;
        public int ImportInfoChecksum;
        [TagField(ValidTags = new [] { "stli" })]
        public CachedTag StructureLightingInfo;
        public int ImportVersion;
        [TagField(ValidTags = new [] { "smet" })]
        public CachedTag StructureMetaData;
        public StructureBspFlags Flags;
        public StructureBspContentPolicyFlag ContentPolicyFlags;
        public StructureBspContentPolicyFlag FailedContentPolicyFlags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<StructureSeamMappingBlock> SeamIdentifiers;
        public List<StructureEdgeToSeamEdgeMappingBlock> EdgeToSeamEdge;
        public List<StructureCollisionMaterialsBlock> CollisionMaterials;
        public List<StructureBspLeafBlock> Leaves;
        public List<StructureSuperNodeAabbsBlock> SuperAabbs;
        public List<SuperNodeMappingsBlock> SuperNodeParentMappings;
        public List<SuperNodeRecursableMasksBlock> SuperNodeRecursableMasks;
        public List<StructureSuperNodeTraversalGeometryBlock> StructureSuperNodeTraversalGeometryBlock1;
        public CollisionKdHierarchyStaticStruct InstanceKdHierarchy;
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public List<StructureSurfaceBlock> LargeStructureSurfaces;
        public List<StructureSurfaceToTriangleMappingBlockStruct> StructureSurfaceToTriangleMapping;
        public List<StructureBspClusterPortalBlock> ClusterPortals;
        public List<StructureBspDetailObjectDataBlock> DetailObjects;
        public List<StructureBspClusterBlockStruct> Clusters;
        public List<GlobalGeometryMaterialBlock> Materials;
        public List<StructureMaterialLightingInfoBlock> EmissiveMaterials;
        public List<StructureBspSkyOwnerClusterBlock> SkyOwnerCluster;
        public List<StructureBspConveyorSurfaceBlock> ConveyorSurfaces;
        public List<BreakableSurfaceSetBlock> BreakableSurfaceSets;
        public List<PathfindingDataBlock> PathfindingData;
        public List<StructureCookieCutterDefinition> CookieCutters;
        public List<ScenarioAcousticsPaletteBlockStruct> AcousticsPalette;
        public byte[] SoundPasData;
        public List<StructureBspMarkerBlock> Markers;
        public List<StructureBspMarkerLightPalette> MarkerLightPalette;
        public List<StructureBspMarkerLightIndex> MarkerLightPaletteIndex;
        public List<StructureBspRuntimeDecalBlock> RuntimeDecals;
        public List<StructureBspEnvironmentObjectPaletteBlock> Unknown;
        public List<StructureBspEnvironmentObjectBlock> Unknown1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<GlobalMapLeafBlock> LeafMapLeaves;
        public List<GlobalLeafConnectionBlock> LeafMapConnections;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<MoppCodeDefinitionBlock> ClusterToInstanceGroupMopps;
        public List<MoppCodeDefinitionBlock> InstanceGroupToInstanceMopps;
        public List<StructureInstanceClusterDefinition> ClusterToInstanceGroupSpheres;
        public List<StructureInstanceGroupDefinition> InstanceGroupToInstanceSpheres;
        public List<StructureBspInstancedGeometryInstancesBlock> InstancedGeometryInstances;
        public List<StructureBspInstancedGeometryInstancesNamesBlock> InstancedGeometryInstanceNames;
        [TagField(ValidTags = new [] { "iimz" })]
        public CachedTag InstanceImposters;
        public List<StructureInstanceImposterInfoBlock> InstanceImposterInfos;
        public int InstanceGeometryTagInstanceCount;
        public List<RuntimeDecoratorSetBlock> DecoratorSets;
        public GlobalRenderGeometryStruct DecoratorInstanceBuffer;
        public List<BspPreplacedDecalSetReferenceBlock> PreplacedDecalSets;
        public List<BspPreplacedDecalReferenceBlock> PreplacedDecals;
        public GlobalRenderGeometryStruct PreplacedDecalGeometry;
        public List<StructureBspSoundClusterBlock> AcousticsSoundClusters;
        public List<TransparentPlanesBlock> TransparentPlanes;
        public List<StructureBspDebugInfoBlock> DebugInfo;
        public GlobalStructurePhysicsStruct StructurePhysics;
        public GlobalRenderGeometryStruct RenderGeometry;
        public List<WidgetReferenceBlock> WidgetReferences;
        public List<CheapLightReferenceBlock> CheapLightReferences;
        public StructureBspResourceInterface ResourceInterface;
        public List<StructureiohavokDataBlockStruct> AnyPlatformTempHavokData;
        public List<StructureExternalInstancedGeometryReferencesBlock> ExternalReferences;
        [TagField(ValidTags = new [] { "dpnd" })]
        public CachedTag Dependencies;
        public int BaseMaterialCount;
        public List<StructureBspObbVolumeBlock> ObbVolumeList;
        public List<HsReferencesBlock> ScriptedDependencies;
        public List<AnimGraphDependencyBlock> Pupanimations;
        
        [Flags]
        public enum StructureBspFlags : ushort
        {
            HasInstanceGroups = 1 << 0,
            SurfaceToTriangleMappingRemapped = 1 << 1,
            ExternalReferencesConvertedToIo = 1 << 2,
            StructureMoppNeedsRebuilt = 1 << 3,
            StructurePrefabMaterialsNeedPostprocessing = 1 << 4,
            SerializedHavokDataConvertedToTargetPlatform = 1 << 5
        }
        
        [Flags]
        public enum StructureBspContentPolicyFlag : ushort
        {
            HasWorkingPathfinding = 1 << 0,
            ConvexDecompositionEnabled = 1 << 1
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureManifestBuildIdentifierStruct : TagStructure
        {
            public int ManifestId0;
            public int ManifestId1;
            public int ManifestId2;
            public int ManifestId3;
            public int BuildIndex;
            public int StructureImporterVersion;
        }
        
        [TagStructure(Size = 0x28)]
        public class StructureSeamMappingBlock : TagStructure
        {
            public StructureSeamIdentifierStruct SeamsIdentifier;
            public List<StructureSeamEdgeMappingBlock> EdgeMapping;
            public List<StructureSeamClusterMappingBlock> ClusterMapping;
            
            [TagStructure(Size = 0x10)]
            public class StructureSeamIdentifierStruct : TagStructure
            {
                public int SeamId0;
                public int SeamId1;
                public int SeamId2;
                public int SeamId3;
            }
            
            [TagStructure(Size = 0x4)]
            public class StructureSeamEdgeMappingBlock : TagStructure
            {
                public int StructureEdgeIndex;
            }
            
            [TagStructure(Size = 0x10)]
            public class StructureSeamClusterMappingBlock : TagStructure
            {
                public int ClusterIndex;
                public RealPoint3d ClusterCenter;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class StructureEdgeToSeamEdgeMappingBlock : TagStructure
        {
            public short SeamIndex;
            public short SeamEdgeIndex;
        }
        
        [TagStructure(Size = 0x1C)]
        public class StructureCollisionMaterialsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag RenderMethod;
            public StringId OverrideMaterialName;
            public short RuntimeGlobalMaterialIndex;
            public short ConveyorSurfaceIndex;
            public short SeamMappingIndex;
            public StructureCollisionMaterialgFlags Flags;
            
            [Flags]
            public enum StructureCollisionMaterialgFlags : ushort
            {
                IsSeam = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class StructureBspLeafBlock : TagStructure
        {
            public byte Cluster;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureSuperNodeAabbsBlock : TagStructure
        {
            public float X0;
            public float X1;
            public float Y0;
            public float Y1;
            public float Z0;
            public float Z1;
        }
        
        [TagStructure(Size = 0x14)]
        public class SuperNodeMappingsBlock : TagStructure
        {
            public short ParentSuperNodeIndex;
            public sbyte ParentInternalNodeIndex;
            public StructureSuperNodeMappingFlags Flags;
            public int HasTraversalGeometryMask;
            public short FirstTraversalGeometryIndex;
            public short FirstAabbIndex;
            public int AabbMask;
            public short NonTerminalTraversalGeometryIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum StructureSuperNodeMappingFlags : byte
            {
                Above = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x2)]
        public class SuperNodeRecursableMasksBlock : TagStructure
        {
            public short Mask;
        }
        
        [TagStructure(Size = 0x18)]
        public class StructureSuperNodeTraversalGeometryBlock : TagStructure
        {
            public List<StructureSuperNodeTraversalGeometryIndicesBlock> PortalIndices;
            public List<StructureSuperNodeTraversalGeometryIndicesBlock> SeamIndices;
            
            [TagStructure(Size = 0x2)]
            public class StructureSuperNodeTraversalGeometryIndicesBlock : TagStructure
            {
                public short Index;
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class CollisionKdHierarchyStaticStruct : TagStructure
        {
            public int HashTotalCount;
            public List<CollisionKdHierarchyStaticHashTableDataBlock> HashData;
            public List<CollisionKdHierarchyStaticHashTableShortBlock> HashEntryCount;
            public List<CollisionKdHierarchyStaticHashTableShortBlock> OriginalHashEntryCount;
            public List<CollisionKdHierarchyStaticNodesBlock> Nodes;
            public List<CollisionKdHierarchyStaticInUseMasksBlock> InUseMasks;
            public List<ClusterTableBlock> ClusterTable;
            
            [TagStructure(Size = 0x10)]
            public class CollisionKdHierarchyStaticHashTableDataBlock : TagStructure
            {
                public int NodeIndex;
                public int KeyA;
                public int KeyB;
                public int KeyC;
            }
            
            [TagStructure(Size = 0x2)]
            public class CollisionKdHierarchyStaticHashTableShortBlock : TagStructure
            {
                public short Index;
            }
            
            [TagStructure(Size = 0x20)]
            public class CollisionKdHierarchyStaticNodesBlock : TagStructure
            {
                public List<CollisionKdHierarchyStaticHashTableHeadersBlock> RenderOnlyHeaders;
                public List<CollisionKdHierarchyStaticHashTableHeadersBlock> CollidableHeaders;
                public short ChildBelow;
                public short ChildAbove;
                public short Parent;
                public short ClusterIndex;
                
                [TagStructure(Size = 0x10)]
                public class CollisionKdHierarchyStaticHashTableHeadersBlock : TagStructure
                {
                    public CollisionKdHierarchyStaticHashTableCullFlags CullFlags;
                    public short InstanceIndex;
                    public int InstanceIndexDwordMask;
                    public short BspIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int BspMask;
                    
                    [Flags]
                    public enum CollisionKdHierarchyStaticHashTableCullFlags : ushort
                    {
                        RenderOnly = 1 << 0,
                        DoesNotBlockAoe = 1 << 1,
                        NonPathfindable = 1 << 2
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class CollisionKdHierarchyStaticInUseMasksBlock : TagStructure
            {
                public int Mask;
            }
            
            [TagStructure(Size = 0xC)]
            public class ClusterTableBlock : TagStructure
            {
                public List<SuperNodeMappingsBlock> SuperNodeMappings;
                
                [TagStructure(Size = 0x40)]
                public class SuperNodeMappingsBlock : TagStructure
                {
                    [TagField(Length = 31)]
                    public SuperNodeMappingIndexArray[]  Indices;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x2)]
                    public class SuperNodeMappingIndexArray : TagStructure
                    {
                        public short Index;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureSurfaceBlock : TagStructure
        {
            public int FirstStructureSurfaceToTriangleMappingIndex;
            public int StructureSurfaceToTriangleMappingCount;
        }
        
        [TagStructure(Size = 0x4)]
        public class StructureSurfaceToTriangleMappingBlockStruct : TagStructure
        {
            public uint ManualByteswarp1;
        }
        
        [TagStructure(Size = 0x50)]
        public class StructureBspClusterPortalBlock : TagStructure
        {
            public StructureBspClusterPortalOrientedBoundsBlock OrientedBounds;
            public short BackCluster;
            public short FrontCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public StructureBspClusterPortalFlags Flags;
            public List<StructureBspClusterPortalVertexBlock> Vertices;
            
            [Flags]
            public enum StructureBspClusterPortalFlags : uint
            {
                AiCanTHearThroughThisShit = 1 << 0,
                OneWay = 1 << 1,
                Door = 1 << 2,
                NoWay = 1 << 3,
                OneWayReversed = 1 << 4,
                NoOneCanHearThroughThis = 1 << 5
            }
            
            [TagStructure(Size = 0x28)]
            public class StructureBspClusterPortalOrientedBoundsBlock : TagStructure
            {
                public RealPoint3d Center;
                public RealVector3d Extents;
                public RealQuaternion Orientation;
            }
            
            [TagStructure(Size = 0xC)]
            public class StructureBspClusterPortalVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
        }
        
        [TagStructure(Size = 0x34)]
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
                public short CellX;
                public short CellY;
                public short CellZ;
                public short OffsetZ;
                public int ValidLayersFlags;
                public int StartIndex;
                public int CountIndex;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x6)]
            public class GlobalDetailObjectBlock : TagStructure
            {
                public sbyte PositionX;
                public sbyte PositionY;
                public sbyte PositionZ;
                public sbyte Data;
                public short Color;
            }
            
            [TagStructure(Size = 0x2)]
            public class GlobalDetailObjectCountsBlock : TagStructure
            {
                public short Count;
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalZReferenceVectorBlock : TagStructure
            {
                public float ZReferenceI;
                public float ZReferenceJ;
                public float ZReferenceK;
                public float ZReferenceL;
            }
        }
        
        [TagStructure(Size = 0x8C)]
        public class StructureBspClusterBlockStruct : TagStructure
        {
            public Bounds<float> BoundsX;
            public Bounds<float> BoundsY;
            public Bounds<float> BoundsZ;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public sbyte AtmosphereIndex;
            public sbyte CameraFxIndex;
            public sbyte WeatherIndex;
            public short Acoustics;
            public short AcousticsSoundClusterIndex;
            public short RuntimeFirstDecalIndex;
            public short RuntimeDecalCound;
            public StructureClusterFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<GNullBlock> PredictedResources;
            public List<StructureBspClusterPortalIndexBlock> Portals;
            public short MeshIndex;
            public short InstanceImposterClusterMoppIndex;
            public List<SeamIndicesBlock> SeamIndices;
            public List<DecoratorRuntimeClusterBlock> DecoratorGroups;
            public List<CheapLightMarkerRefBlock> CheapLightMarkerRefs;
            public List<PvsBoundObjectIdentifiersBlock> PvsBoundObjectIdentifiers;
            public List<PvsBoundObjectReferencesBlock> PvsBoundObjectReferences;
            public List<StructureClusterCubemap> ClusterCubemaps;
            
            [Flags]
            public enum StructureClusterFlags : ushort
            {
                OneWayPortal = 1 << 0,
                DoorPortal = 1 << 1,
                PostprocessedGeometry = 1 << 2,
                IsTheSky = 1 << 3,
                DecoratorsAreLit = 1 << 4
            }
            
            [TagStructure(Size = 0x0)]
            public class GNullBlock : TagStructure
            {
            }
            
            [TagStructure(Size = 0x2)]
            public class StructureBspClusterPortalIndexBlock : TagStructure
            {
                public short PortalIndex;
            }
            
            [TagStructure(Size = 0x1)]
            public class SeamIndicesBlock : TagStructure
            {
                public sbyte SeamIndex;
            }
            
            [TagStructure(Size = 0x30)]
            public class DecoratorRuntimeClusterBlock : TagStructure
            {
                public ushort DecoratorPlacementCount;
                public byte DecoratorSetIndex;
                public byte DecoratorInstanceBufferIndex;
                public int DecoratorInstanceBufferOffset;
                public RealVector3d PositionBoundsMin;
                public float BoundingSphereRadius;
                public RealVector3d PositionBoundsSize;
                public RealVector3d BoundingSphereCenter;
            }
            
            [TagStructure(Size = 0x4)]
            public class CheapLightMarkerRefBlock : TagStructure
            {
                public short CheapLightReferenceReference;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x8)]
            public class PvsBoundObjectIdentifiersBlock : TagStructure
            {
                public ScenarioObjectIdStruct ObjectId;
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStruct : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public ObjectTypeEnum Type;
                    public ObjectSourceEnum Source;
                    
                    public enum ObjectTypeEnum : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Terminal,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        Dispenser,
                        SoundScenery,
                        Crate,
                        Creature,
                        Giant,
                        EffectScenery,
                        Spawner
                    }
                    
                    public enum ObjectSourceEnum : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy,
                        Sky,
                        Parent
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class PvsBoundObjectReferencesBlock : TagStructure
            {
                public ScenarioObjectReferenceStruct ScenarioObjectReference;
                
                [TagStructure(Size = 0x4)]
                public class ScenarioObjectReferenceStruct : TagStructure
                {
                    public short ObjectIndex;
                    public short ScenarioObjectIndex;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class StructureClusterCubemap : TagStructure
            {
                public short ScenarioCubemapIndex;
                public short CubemapBitmapIndex;
                public List<CubemapReferencePointsBlock> ReferencePoints;
                
                [TagStructure(Size = 0x10)]
                public class CubemapReferencePointsBlock : TagStructure
                {
                    public RealPoint3d ReferencePoint;
                    public int PointIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class GlobalGeometryMaterialBlock : TagStructure
        {
            public CachedTag RenderMethod;
            public int ImportedMaterialIndex;
            public float LightmapResolutionScale;
            public int LightmapAdditiveTransparencyColor;
            public int LightmapTraslucencyTintColor;
            public float LightmapAnalyticalLightAbsorb;
            public float LightmapNormalLightAbsorb;
            public GlobalGeometryMaterialLightmapFlags LightmapFlags;
            public sbyte BreakableSurfaceIndex;
            public short LightmapChartGroupIndex;
            
            [Flags]
            public enum GlobalGeometryMaterialLightmapFlags : byte
            {
                IgnoreDefaultResolutionScale = 1 << 0,
                TransparencyOverride = 1 << 1,
                LightingFromBothSides = 1 << 2,
                Version1 = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class StructureMaterialLightingInfoBlock : TagStructure
        {
            public float EmissivePower;
            public RealRgbColor EmissiveColor;
            public float EmissiveQuality;
            public float EmissiveFocus;
            public StructureMaterialLightingInfoFlags Flags;
            public float AttenuationFalloff;
            public float AttenuationCutoff;
            public float BounceRatio;
            
            [Flags]
            public enum StructureMaterialLightingInfoFlags : uint
            {
                Reserved = 1 << 0,
                PowerPerUnitArea = 1 << 1,
                UseShaderGel = 1 << 2
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
        
        [TagStructure(Size = 0x20)]
        public class BreakableSurfaceSetBlock : TagStructure
        {
            [TagField(Length = 8)]
            public SupportedBitfield[]  SupportedBitfield1;
            
            [TagStructure(Size = 0x4)]
            public class SupportedBitfield : TagStructure
            {
                public int BitvectorData;
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class PathfindingDataBlock : TagStructure
        {
            public int RuntimenavMesh;
            public int RuntimenavGraph;
            public int RuntimenavMediator;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] NavgraphData;
            public byte[] NavmediatorData;
            public List<FaceUserDataBlock> FaceuserData;
            public int StructureChecksum;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0xC)]
            public class FaceUserDataBlock : TagStructure
            {
                public short MFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CurrentminPathDistance;
                public float CurrentminTargetApproachDistance;
            }
        }
        
        [TagStructure(Size = 0x6C)]
        public class StructureCookieCutterDefinition : TagStructure
        {
            public GlobalCollisionBspStruct CollisionModel;
            
            [TagStructure(Size = 0x6C)]
            public class GlobalCollisionBspStruct : TagStructure
            {
                public List<Bsp3dNodesBlockStruct> Bsp3dNodes;
                public List<Bsp3dKdSupdernodesBlock> Bsp3dSupernodes;
                public List<PlanesBlock> Planes;
                public List<CollisionLeafStruct> Leaves;
                public List<Bsp2dReferencesBlock> Bsp2dReferences;
                public List<Bsp2dNodesBlock> Bsp2dNodes;
                public List<SurfacesBlockStruct> Surfaces;
                public List<EdgesBlock> Edges;
                public List<VerticesBlock> Vertices;
                
                [TagStructure(Size = 0x8)]
                public class Bsp3dNodesBlockStruct : TagStructure
                {
                    public long NodeDataDesignator;
                }
                
                [TagStructure(Size = 0x80)]
                public class Bsp3dKdSupdernodesBlock : TagStructure
                {
                    public float Plane0;
                    public float Plane1;
                    public float Plane2;
                    public float Plane3;
                    public float Plane4;
                    public float Plane5;
                    public float Plane6;
                    public float Plane7;
                    public float Plane8;
                    public float Plane9;
                    public float Plane10;
                    public float Plane11;
                    public float Plane12;
                    public float Plane13;
                    public float Plane14;
                    public int PlaneDimensions;
                    public int ChildIndex0;
                    public int ChildIndex1;
                    public int ChildIndex2;
                    public int ChildIndex3;
                    public int ChildIndex4;
                    public int ChildIndex5;
                    public int ChildIndex6;
                    public int ChildIndex7;
                    public int ChildIndex8;
                    public int ChildIndex9;
                    public int ChildIndex10;
                    public int ChildIndex11;
                    public int ChildIndex12;
                    public int ChildIndex13;
                    public int ChildIndex14;
                    public int ChildIndex15;
                }
                
                [TagStructure(Size = 0x10)]
                public class PlanesBlock : TagStructure
                {
                    public RealPlane3d Plane;
                }
                
                [TagStructure(Size = 0x8)]
                public class CollisionLeafStruct : TagStructure
                {
                    public LeafFlags Flags;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public ushort Bsp2dReferenceCount;
                    public uint FirstBsp2dReference;
                    
                    [Flags]
                    public enum LeafFlags : byte
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
                
                [TagStructure(Size = 0xC)]
                public class SurfacesBlockStruct : TagStructure
                {
                    public ushort PlaneIndex;
                    public ushort FirstEdge;
                    public short Material;
                    public short BreakableSurfaceSet;
                    public short BreakableSurface;
                    public SurfaceFlags Flags;
                    public byte BestPlaneCalculationVertexIndex;
                    
                    [Flags]
                    public enum SurfaceFlags : byte
                    {
                        TwoSided = 1 << 0,
                        Invisible = 1 << 1,
                        Climbable = 1 << 2,
                        Breakable = 1 << 3,
                        Invalid = 1 << 4,
                        Conveyor = 1 << 5,
                        Slip = 1 << 6,
                        PlaneNegated = 1 << 7
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class EdgesBlock : TagStructure
                {
                    public ushort StartVertex;
                    public ushort EndVertex;
                    public ushort ForwardEdge;
                    public ushort ReverseEdge;
                    public ushort LeftSurface;
                    public ushort RightSurface;
                }
                
                [TagStructure(Size = 0x10)]
                public class VerticesBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public ushort FirstEdge;
                    public short Sink;
                }
            }
        }
        
        [TagStructure(Size = 0x98)]
        public class ScenarioAcousticsPaletteBlockStruct : TagStructure
        {
            public StringId Name;
            public ScenarioAcousticsEnvironmentDefinition Reverb;
            public ScenarioAcousticsAmbienceDefinition Ambience;
            [TagField(ValidTags = new [] { "sbnk" })]
            public CachedTag SoundBankTag;
            [TagField(ValidTags = new [] { "sbnk" })]
            public CachedTag DvdOnlySoundBankTag;
            
            [TagStructure(Size = 0x1C)]
            public class ScenarioAcousticsEnvironmentDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "snde" })]
                public CachedTag SoundEnvironment;
                public SoundClassAcousticsStringDefinition Type;
                public float CutoffDistance;
                public float InterpolationTime; // seconds
                
                public enum SoundClassAcousticsStringDefinition : int
                {
                    Outside,
                    Inside
                }
            }
            
            [TagStructure(Size = 0x58)]
            public class ScenarioAcousticsAmbienceDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag BackgroundSound;
                // plays when rain is active, weather rate gets applied to scale.
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag WeatherSound;
                // plays when entering this area
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag EntrySound;
                // plays when leaving this area
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag ExitSound;
                public float CutoffDistance;
                public float InterpolationTime; // seconds
                public BackgroundSoundScaleFlags ScaleFlagsDepricated;
                public float InteriorScaleDepricated;
                public float PortalScaleDepricated;
                public float ExteriorScaleDepricated;
                
                [Flags]
                public enum BackgroundSoundScaleFlags : uint
                {
                    OverrideDefaultScale = 1 << 0,
                    UseAdjacentClusterAsPortalScale = 1 << 1,
                    UseAdjacentClusterAsExteriorScale = 1 << 2,
                    ScaleWithWeatherIntensity = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class StructureBspMarkerBlock : TagStructure
        {
            public StructureMarkerTypeEnum MarkerType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string MarkerParameter;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            
            public enum StructureMarkerTypeEnum : sbyte
            {
                None,
                CheapLight,
                FallingLeafGenerator,
                Light,
                Sky,
                Model
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspMarkerLightPalette : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh" })]
            public CachedTag LightTag;
        }
        
        [TagStructure(Size = 0x2)]
        public class StructureBspMarkerLightIndex : TagStructure
        {
            public short PaletteIndex;
        }
        
        [TagStructure(Size = 0x3C)]
        public class StructureBspRuntimeDecalBlock : TagStructure
        {
            public short DecalPaletteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public ManualbspFlagsReferences ManualBspFlags;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float ScaleX;
            public float ScaleY;
            public float CullAngle;
            
            [TagStructure(Size = 0x10)]
            public class ManualbspFlagsReferences : TagStructure
            {
                public List<ScenariobspReferenceBlock> ReferencesBlock;
                public int Flags;
                
                [TagStructure(Size = 0x10)]
                public class ScenariobspReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sbsp" })]
                    public CachedTag StructureDesign;
                }
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureBspEnvironmentObjectPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag Definition;
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag Model;
            public int Gveyn;
        }
        
        [TagStructure(Size = 0x54)]
        public class StructureBspEnvironmentObjectBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
            public short PaletteIndex;
            public EnvironmentobjectFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int UniqueId;
            public Tag ExportedObjectType;
            public StringId ScenarioObjectName;
            public StringId VariantName;
            
            [Flags]
            public enum EnvironmentobjectFlags : byte
            {
                ScriptsAlwaysRun = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalMapLeafBlock : TagStructure
        {
            public List<MapLeafFaceBlock> Faces;
            public List<MapLeafConnectionIndexBlock> ConnectionIndices;
            
            [TagStructure(Size = 0x10)]
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
        
        [TagStructure(Size = 0x1C)]
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
        
        [TagStructure(Size = 0x118)]
        public class GlobalErrorReportCategoriesBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ErrorReportTypes ReportType;
            public ErrorReportFlags Flags;
            public short RuntimeGenerationFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int RuntimeSomething;
            public List<ErrorReportsBlock> Reports;
            
            public enum ErrorReportTypes : short
            {
                Silent,
                Comment,
                Warning,
                Error
            }
            
            [Flags]
            public enum ErrorReportFlags : ushort
            {
                Rendered = 1 << 0,
                TangentSpace = 1 << 1,
                NonCritical = 1 << 2,
                LightmapLight = 1 << 3,
                ReportKeyIsValid = 1 << 4
            }
            
            [TagStructure(Size = 0xB8)]
            public class ErrorReportsBlock : TagStructure
            {
                public ErrorReportTypes Type;
                public ErrorReportSource Source;
                public ErrorReportFlags Flags;
                public byte[] Text;
                public int SourceIdentifier;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVerticesBlock> Vertices;
                public List<ErrorReportVectorsBlock> Vectors;
                public List<ErrorReportLinesBlock> Lines;
                public List<ErrorReportTrianglesBlock> Triangles;
                public List<ErrorReportQuadsBlock> Quads;
                public List<ErrorReportCommentsBlock> Comments;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                
                public enum ErrorReportTypes : sbyte
                {
                    Silent,
                    Comment,
                    Warning,
                    Error
                }
                
                public enum ErrorReportSource : sbyte
                {
                    None,
                    Structure,
                    Poop,
                    Lightmap,
                    Pathfinding
                }
                
                [TagStructure(Size = 0x34)]
                public class ErrorReportVerticesBlock : TagStructure
                {
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    public float ScreenSize;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
                
                [TagStructure(Size = 0x40)]
                public class ErrorReportVectorsBlock : TagStructure
                {
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
                
                [TagStructure(Size = 0x50)]
                public class ErrorReportLinesBlock : TagStructure
                {
                    [TagField(Length = 2)]
                    public ErrorReportLinePointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportLinePointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x70)]
                public class ErrorReportTrianglesBlock : TagStructure
                {
                    [TagField(Length = 3)]
                    public ErrorReportTrianglePointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportTrianglePointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x90)]
                public class ErrorReportQuadsBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public ErrorReportQuadPointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportQuadPointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class ErrorReportCommentsBlock : TagStructure
                {
                    public byte[] Text;
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class MoppCodeDefinitionBlock : TagStructure
        {
            public int FieldPointerSkip;
            public short Size;
            public short Count;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float VI;
            public float VJ;
            public float VK;
            public float VW;
            public int MDataPointer;
            public int IntMSize;
            public int IntMCapacityandFlags;
            public sbyte Int8MBuildtype;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<MoppCodeDataDefinitionBlock> MoppDataBlock;
            // they say it only matters for ps3
            public sbyte MoppBuildType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x1)]
            public class MoppCodeDataDefinitionBlock : TagStructure
            {
                public byte MoppData;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureInstanceClusterDefinition : TagStructure
        {
            public StructureInstanceClusterFlags Flags;
            public List<IndexListBlock> InstanceGroupIndices;
            
            [Flags]
            public enum StructureInstanceClusterFlags : uint
            {
                OptimizedMopp = 1 << 0
            }
            
            [TagStructure(Size = 0x2)]
            public class IndexListBlock : TagStructure
            {
                public ushort Index;
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class StructureInstanceGroupDefinition : TagStructure
        {
            public RealPoint3d Center;
            public float Radius;
            public short ClusterCount;
            public StructureInstanceGroupFlags Flags;
            public float MaximumImposterDistance;
            public float MinimumCentrifugalDistanceFromGroupCenter;
            public float MinimumImposterDistanceSquared;
            public List<IndexListBlock> InstanceIndices;
            
            [Flags]
            public enum StructureInstanceGroupFlags : ushort
            {
                ContainsCardImposters = 1 << 0,
                ContainsPolyImposters = 1 << 1,
                IsDecoratorType = 1 << 2,
                OptimizedMopp = 1 << 3
            }
            
            [TagStructure(Size = 0x2)]
            public class IndexListBlock : TagStructure
            {
                public ushort Index;
            }
        }
        
        [TagStructure(Size = 0x198)]
        public class StructureBspInstancedGeometryInstancesBlock : TagStructure
        {
            public float Scale;
            public RealVector3d Forward;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealPoint3d Position;
            public short InstanceDefinition;
            public InstancedGeometryFlags Flags;
            public ChanneldefinitionFlags LightChannels;
            public short MeshIndex;
            public short CompressionIndex;
            public int SeamBitVector0;
            public int SeamBitVector1;
            public int SeamBitVector2;
            public int SeamBitVector3;
            public float BoundsX0;
            public float BoundsX1;
            public float BoundsY0;
            public float BoundsY1;
            public float BoundsZ0;
            public float BoundsZ1;
            public RealPoint3d WorldBoundingSphereCenter;
            public float WorldBoundingSphereRadius;
            public float ImposterTransitionCompleteDistance;
            public float ImposterBrightness;
            public int Checksum;
            public InstancedGeometryPathfindingPolicyEnum PathfindingPolicy;
            public InstancedGeometryLightmappingPolicyEnum LightmappingPolicy;
            public InstancedGeometryImposterPolicyEnum ImposterPolicy;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public InstancedGeometryStreamingpriorityEnum StreamingPriority;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public short Cubemap0BitmapIndex;
            public float LightmapResolutionScale;
            public short GroupIndex;
            public short GroupListIndex;
            public StringId Name;
            [TagField(Length = 256)]
            public string SourceFileName;
            
            [Flags]
            public enum InstancedGeometryFlags : ushort
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
                Unknown = 1 << 9,
                RemoveFromShadowGeometry = 1 << 10,
                CinemaOnly = 1 << 11,
                ExcludeFromCinema = 1 << 12,
                DisallowObjectLightingSamples = 1 << 13
            }
            
            [Flags]
            public enum ChanneldefinitionFlags : uint
            {
                _0 = 1 << 0,
                _1 = 1 << 1,
                _2 = 1 << 2,
                _3 = 1 << 3,
                _4 = 1 << 4,
                _5 = 1 << 5,
                _6 = 1 << 6,
                _7 = 1 << 7,
                _8 = 1 << 8,
                _9 = 1 << 9,
                _10 = 1 << 10,
                _11 = 1 << 11,
                _12 = 1 << 12,
                _13 = 1 << 13,
                _14 = 1 << 14,
                _15 = 1 << 15,
                _16 = 1 << 16,
                _17 = 1 << 17,
                _18 = 1 << 18,
                _19 = 1 << 19,
                _20 = 1 << 20,
                _21 = 1 << 21,
                _22 = 1 << 22,
                _23 = 1 << 23,
                _24 = 1 << 24,
                _25 = 1 << 25,
                _26 = 1 << 26,
                _27 = 1 << 27,
                _28 = 1 << 28,
                _29 = 1 << 29,
                _30 = 1 << 30,
                _31 = 1u << 31
            }
            
            public enum InstancedGeometryPathfindingPolicyEnum : sbyte
            {
                CutOut,
                Static,
                None
            }
            
            public enum InstancedGeometryLightmappingPolicyEnum : sbyte
            {
                PerPixelShared,
                PerVertex,
                SingleProbe,
                Exclude,
                PerPixelAo,
                PerVertexAo
            }
            
            public enum InstancedGeometryImposterPolicyEnum : sbyte
            {
                PolygonDefault,
                PolygonHigh,
                CardsMedium,
                CardsHigh,
                None,
                RainbowBox
            }
            
            public enum InstancedGeometryStreamingpriorityEnum : sbyte
            {
                Default,
                Higher,
                Highest
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class StructureBspInstancedGeometryInstancesNamesBlock : TagStructure
        {
            public StringId Name;
        }
        
        [TagStructure(Size = 0xC)]
        public class StructureInstanceImposterInfoBlock : TagStructure
        {
            public StringId Name;
            public sbyte ImposterPolicy;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float TransitionDistance;
        }
        
        [TagStructure(Size = 0x10)]
        public class RuntimeDecoratorSetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "dctr" })]
            public CachedTag DecoratorSetReference;
        }
        
        [TagStructure(Size = 0xB4)]
        public class GlobalRenderGeometryStruct : TagStructure
        {
            public RenderGeometryFlags RuntimeFlags;
            public List<GlobalMeshBlock> Meshes;
            public List<PcameshIndexBlock> PcaMeshIndices;
            public List<CompressionInfoBlock> CompressionInfo;
            public List<SortingPositionBlock> PartSortingPosition;
            public List<UserDataBlock> UserData;
            public List<PerMeshRawDataBlock> PerMeshTemporary;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<PerMeshNodeMapBlock> PerMeshNodeMap;
            public List<PerMeshSubpartVisibilityBlock> PerMeshSubpartVisibility;
            public List<PerMeshPrtDataBlock> PerMeshPrtData;
            public List<PerInstanceLightmapTexcoordsBlock> PerInstanceLightmapTexcoords;
            public List<WaterBoundingBoxBlock> WaterBoundingBoxBlock1;
            public TagResourceReference ApiResource;
            public List<RenderGeometryPvsDataBlock> OptionalPvsData;
            public List<ShapenameBlock> Shapenames;
            
            [Flags]
            public enum RenderGeometryFlags : uint
            {
                Processed = 1 << 0,
                Available = 1 << 1,
                HasValidBudgets = 1 << 2,
                ManualResourceCreation = 1 << 3,
                KeepRawGeometry = 1 << 4,
                DontUseCompressedVertexPositions = 1 << 5,
                PcaAnimationTableSorted = 1 << 6,
                NeedsNoLightmapUvs = 1 << 7,
                AlwaysNeedsLightmapUvs = 1 << 8
            }
            
            [TagStructure(Size = 0x70)]
            public class GlobalMeshBlock : TagStructure
            {
                public List<PartBlock> Parts;
                public List<SubpartBlock> Subparts;
                [TagField(Length = 9)]
                public VertexBufferIndicesWordArray[]  VertexBufferIndices;
                public short IndexBufferIndex;
                public short IndexBufferTessellation;
                public MeshFlags MeshFlags1;
                public sbyte RigidNodeIndex;
                public MeshVertexType VertexType;
                public MeshTransferVertexType PrtVertexType;
                public MeshLightingPolicyType LightingPolicy;
                public MeshIndexBufferType IndexBufferType;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short PcaMeshIndex;
                public List<GlobalInstanceBucketBlock> InstanceBuckets;
                public List<IndicesWordBlock> WaterIndicesStart;
                public float RuntimeBoundingRadius; // world units*!
                public RealPoint3d RuntimeBoundingOffset;
                public List<VertexkeyBlock> VertexKeys;
                public short CloneIndex;
                public short CumulativePartCount;
                
                [Flags]
                public enum MeshFlags : ushort
                {
                    MeshHasVertexColor = 1 << 0,
                    UseRegionIndexForSorting = 1 << 1,
                    UseVertexBuffersForIndices = 1 << 2,
                    MeshHasPerInstanceLighting = 1 << 3,
                    MeshIsUnindexed = 1 << 4,
                    SubpartWereMerged = 1 << 5,
                    MeshHasFur = 1 << 6,
                    MeshHasDecal = 1 << 7,
                    MeshDoesntUseCompressedPosition = 1 << 8,
                    UseUncompressedVertexFormat = 1 << 9,
                    MeshIsPca = 1 << 10,
                    MeshCompressionDetermined = 1 << 11,
                    MeshHasAuthoredLightmapTextureCoords = 1 << 12,
                    MeshHasAUsefulSetOfSecondTextureCoords = 1 << 13,
                    MeshHasNoLightmap = 1 << 14,
                    PerVertexLighting = 1 << 15
                }
                
                public enum MeshVertexType : sbyte
                {
                    World,
                    Rigid,
                    Skinned,
                    ParticleModel,
                    FlatWorld,
                    FlatRigid,
                    FlatSkinned,
                    Screen,
                    Debug,
                    Transparent,
                    Particle,
                    Unused0,
                    LightVolume,
                    ChudSimple,
                    ChudFancy,
                    Decorator,
                    PositionOnly,
                    PatchyFog,
                    Water,
                    Ripple,
                    ImplicitGeometry,
                    Unused1,
                    WorldTessellated,
                    RigidTessellated,
                    SkinnedTessellated,
                    ShaderCache,
                    StructureInstanceImposter,
                    ObjectInstanceImposter,
                    RigidCompressed,
                    SkinnedUncompressed,
                    LightVolumePrecompiled,
                    BlendshapeRigid,
                    BlendshapeRigidBlendshaped,
                    RigidBlendshaped,
                    BlendshapeSkinned,
                    BlendshapeSkinnedBlendshaped,
                    SkinnedBlendshaped,
                    VirtualGeometryHwtess,
                    VirtualGeometryMemexport,
                    PositionOnly1,
                    VirtualGeometryDebug,
                    BlendshaperigidCompressed,
                    SkinneduncompressedBlendshaped,
                    BlendshapeskinnedCompressed,
                    Tracer,
                    Polyart,
                    Vectorart,
                    RigidBoned,
                    RigidBoned2uv,
                    BlendshapeSkinned2uv,
                    BlendshapeSkinned2uvBlendshaped,
                    Skinned2uvBlendshaped,
                    Polyartuv,
                    BlendshapeSkinnedUncompressedBlendshaped
                }
                
                public enum MeshTransferVertexType : sbyte
                {
                    NoPrt,
                    PrtAmbient,
                    PrtLinear,
                    PrtQuadratic
                }
                
                public enum MeshLightingPolicyType : sbyte
                {
                    VertexColor,
                    SingleProbe,
                    PrtAmbient
                }
                
                public enum MeshIndexBufferType : sbyte
                {
                    Default,
                    LineList,
                    LineStrip,
                    TriangleList,
                    TriangleFan,
                    TriangleStrip,
                    QuadList,
                    RectList
                }
                
                [TagStructure(Size = 0x18)]
                public class PartBlock : TagStructure
                {
                    public short RenderMethodIndex;
                    public short TransparentSortingIndex;
                    public int IndexStart;
                    public int IndexCount;
                    public short SubpartStart;
                    public short SubpartCount;
                    public sbyte PartType;
                    public SpecializedRenderDefinition SpecializedRender;
                    public PartFlags PartFlags1;
                    public ushort BudgetVertexCount;
                    public TessellationModeDefinition Tessellation;
                    
                    public enum SpecializedRenderDefinition : sbyte
                    {
                        None,
                        Fail,
                        Fur,
                        FurStencil,
                        Decal,
                        Shield,
                        Water,
                        LightmapOnly,
                        Hologram
                    }
                    
                    [Flags]
                    public enum PartFlags : ushort
                    {
                        IsWaterSurface = 1 << 0,
                        PerVertexLightmapPart = 1 << 1,
                        DebugFlagInstancePart = 1 << 2,
                        SubpartsHasUberlightsInfo = 1 << 3,
                        DrawCullDistanceMedium = 1 << 4,
                        DrawCullDistanceClose = 1 << 5,
                        DrawCullRenderingShields = 1 << 6,
                        CannotSinglePassRender = 1 << 7,
                        IsTransparent = 1 << 8,
                        CannotTwoPass = 1 << 9,
                        // expensive
                        TransparentShouldOutputDepthForDoF = 1 << 10,
                        DoNotIncludeInStaticLightmap = 1 << 11,
                        DoNotIncludeInPvsGeneration = 1 << 12,
                        DrawCullRenderingActiveCamo = 1 << 13
                    }
                    
                    public enum TessellationModeDefinition : short
                    {
                        None,
                        FixedX4Faces,
                        FixedX9Faces,
                        FixedX36Faces
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class SubpartBlock : TagStructure
                {
                    public int IndexStart;
                    public int IndexCount;
                    public short PartIndex;
                    public ushort BudgetVertexCount;
                    public uint AnalyticalLightIndex;
                }
                
                [TagStructure(Size = 0x2)]
                public class VertexBufferIndicesWordArray : TagStructure
                {
                    public ushort VertexBufferIndex;
                }
                
                [TagStructure(Size = 0x10)]
                public class GlobalInstanceBucketBlock : TagStructure
                {
                    public short MeshIndex;
                    public short DefinitionIndex;
                    public List<InstanceIndexWordBlock> Instances;
                    
                    [TagStructure(Size = 0x2)]
                    public class InstanceIndexWordBlock : TagStructure
                    {
                        public short InstanceIndex;
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class IndicesWordBlock : TagStructure
                {
                    public short Word;
                }
                
                [TagStructure(Size = 0x8)]
                public class VertexkeyBlock : TagStructure
                {
                    public int Key1;
                    public int Key2;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class PcameshIndexBlock : TagStructure
            {
                public int MeshIndex;
            }
            
            [TagStructure(Size = 0x34)]
            public class CompressionInfoBlock : TagStructure
            {
                public CompressionFlags CompressionFlags1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealPoint3d PositionBounds0;
                public RealPoint3d PositionBounds1;
                public RealPoint2d TexcoordBounds0;
                public RealPoint2d TexcoordBounds1;
                public float Unused0;
                public float Unused1;
                
                [Flags]
                public enum CompressionFlags : ushort
                {
                    CompressedPosition = 1 << 0,
                    CompressedTexcoord = 1 << 1,
                    CompressionOptimized = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class SortingPositionBlock : TagStructure
            {
                public RealPlane3d Plane;
                public RealPoint3d Position;
                public float Radius;
                [TagField(Length = 4)]
                public NodeIndicesArray[]  NodeIndices;
                [TagField(Length = 3)]
                public NodeWeightsImplicitArray[]  NodeWeights;
                
                [TagStructure(Size = 0x1)]
                public class NodeIndicesArray : TagStructure
                {
                    public byte NodeIndex;
                }
                
                [TagStructure(Size = 0x4)]
                public class NodeWeightsImplicitArray : TagStructure
                {
                    public float NodeWeight;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class UserDataBlock : TagStructure
            {
                public GlobalRenderGeometryUserDataHeaderStruct UserDataHeader;
                public byte[] UserData;
                
                [TagStructure(Size = 0x4)]
                public class GlobalRenderGeometryUserDataHeaderStruct : TagStructure
                {
                    public RenderGeometryUserDataType DataType;
                    public sbyte DataCount;
                    public ushort DataSize;
                    
                    public enum RenderGeometryUserDataType : sbyte
                    {
                        PrtInfo
                    }
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class PerMeshRawDataBlock : TagStructure
            {
                public List<RawVertexBlock> RawVertices;
                public List<IndicesWordBlock> RawIndices;
                public List<IndicesDwordBlock> RawIndices32;
                public List<RawWaterBlock> RawWaterData;
                public List<RawImposterBrdfBlock> RawImposterData;
                public List<RawInstanceImposterBlock> RawInstanceImposterVerts;
                public List<RawBlendshapeBlock> RawBlendshapes;
                public int PerVertexLightingVertexSize;
                public short ParameterizedTextureWidth;
                public short ParameterizedTextureHeight;
                public PerMeshRawDataFlags Flags;
                
                [Flags]
                public enum PerMeshRawDataFlags : uint
                {
                    IndicesAreTriangleStrips = 1 << 0,
                    IndicesAreTriangleLists = 1 << 1,
                    IndicesAreQuadLists = 1 << 2
                }
                
                [TagStructure(Size = 0x68)]
                public class RawVertexBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public RealPoint2d Texcoord;
                    public RealPoint3d Normal;
                    public RealPoint3d Binormal;
                    public RealPoint3d Tangent;
                    public RealPoint2d LightmapTexcoord;
                    [TagField(Length = 4)]
                    public NodeIndicesArray[]  NodeIndices;
                    [TagField(Length = 4)]
                    public NodeWeightsCompleteArray[]  NodeWeights;
                    public RealPoint3d VertexColor;
                    public RealPoint2d Texcoord1;
                    
                    [TagStructure(Size = 0x1)]
                    public class NodeIndicesArray : TagStructure
                    {
                        public byte NodeIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class NodeWeightsCompleteArray : TagStructure
                    {
                        public float NodeWeight;
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class IndicesWordBlock : TagStructure
                {
                    public short Word;
                }
                
                [TagStructure(Size = 0x4)]
                public class IndicesDwordBlock : TagStructure
                {
                    public int Dword;
                }
                
                [TagStructure(Size = 0x18)]
                public class RawWaterBlock : TagStructure
                {
                    public List<IndicesWordBlock> RawWaterIndices;
                    public List<RawWaterAppendBlock> RawWaterVertices;
                    
                    [TagStructure(Size = 0x8)]
                    public class RawWaterAppendBlock : TagStructure
                    {
                        public RealPoint2d BaseTexcoord;
                    }
                }
                
                [TagStructure(Size = 0x4C)]
                public class RawImposterBrdfBlock : TagStructure
                {
                    public RealRgbColor Diffuse;
                    public RealRgbColor Ambient;
                    public RealRgbColor Specular;
                    public float Shininess;
                    public float Alpha;
                    public RealArgbColor ChangeColorTintOfDiffuse;
                    public RealArgbColor ChangeColorTintOfSpecular;
                }
                
                [TagStructure(Size = 0x14)]
                public class RawInstanceImposterBlock : TagStructure
                {
                    public RealVector3d Position;
                    public uint Color;
                    public float HdrScaler;
                }
                
                [TagStructure(Size = 0x28)]
                public class RawBlendshapeBlock : TagStructure
                {
                    public RealVector3d Position;
                    public RealVector3d Normal;
                    public RealArgbColor TensionAndAmbientOcclusion;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class PerMeshNodeMapBlock : TagStructure
            {
                public List<NodeMapByteBlock> NodeMap;
                
                [TagStructure(Size = 0x1)]
                public class NodeMapByteBlock : TagStructure
                {
                    public sbyte NodeIndex;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class PerMeshSubpartVisibilityBlock : TagStructure
            {
                public List<SortingPositionBlock> BoundingSphere;
            }
            
            [TagStructure(Size = 0x20)]
            public class PerMeshPrtDataBlock : TagStructure
            {
                public byte[] MeshPcaData;
                public List<PerInstancePrtDataBlock> PerInstancePrtData;
                
                [TagStructure(Size = 0x14)]
                public class PerInstancePrtDataBlock : TagStructure
                {
                    public byte[] MeshPcaData;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class PerInstanceLightmapTexcoordsBlock : TagStructure
            {
                public List<RawVertexBlock> TextureCoordinates;
                public List<RawTexcoordBlock> TextureCoordinates1;
                public short VertexBufferIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x68)]
                public class RawVertexBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public RealPoint2d Texcoord;
                    public RealPoint3d Normal;
                    public RealPoint3d Binormal;
                    public RealPoint3d Tangent;
                    public RealPoint2d LightmapTexcoord;
                    [TagField(Length = 4)]
                    public NodeIndicesArray[]  NodeIndices;
                    [TagField(Length = 4)]
                    public NodeWeightsCompleteArray[]  NodeWeights;
                    public RealPoint3d VertexColor;
                    public RealPoint2d Texcoord1;
                    
                    [TagStructure(Size = 0x1)]
                    public class NodeIndicesArray : TagStructure
                    {
                        public byte NodeIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class NodeWeightsCompleteArray : TagStructure
                    {
                        public float NodeWeight;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class RawTexcoordBlock : TagStructure
                {
                    public RealPoint2d Texcoord;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class WaterBoundingBoxBlock : TagStructure
            {
                public short MeshIndex;
                public short PartIndex;
                public RealPoint3d PositionBounds0;
                public RealPoint3d PositionBounds1;
            }
            
            [TagStructure(Size = 0x4C)]
            public class RenderGeometryPvsDataBlock : TagStructure
            {
                public List<RenderGeometryPvsDataVisibilityValueBlock> VisibilityArray;
                public List<RenderGeometryPvsDataVisibilityIndexBlock> PerBlockVisibilityOffsetArray;
                public int BitsPerBlock;
                public RealPoint3d ObjectSpaceGridAabbMin;
                public RealPoint3d ObjectSpaceGridAabbMax;
                public RealVector3d BlockSize;
                public int NumBlocksX;
                public int NumBlocksY;
                public int NumBlocksZ;
                
                [TagStructure(Size = 0x4)]
                public class RenderGeometryPvsDataVisibilityValueBlock : TagStructure
                {
                    public uint Value;
                }
                
                [TagStructure(Size = 0x2)]
                public class RenderGeometryPvsDataVisibilityIndexBlock : TagStructure
                {
                    public ushort Index;
                }
            }
            
            [TagStructure(Size = 0x104)]
            public class ShapenameBlock : TagStructure
            {
                public int Key;
                [TagField(Length = 256)]
                public string Name;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class BspPreplacedDecalSetReferenceBlock : TagStructure
        {
            public int DecalDefinitionIndex;
            public sbyte LocationBsp0;
            public byte LocationCluster0;
            public sbyte LocationBsp1;
            public byte LocationCluster1;
            public sbyte LocationBsp2;
            public byte LocationCluster2;
            public sbyte LocationBsp3;
            public byte LocationCluster3;
            public RealPoint3d Center;
            public short FirstDecalRefIndex;
            public short DecalRefCount;
        }
        
        [TagStructure(Size = 0x1C)]
        public class BspPreplacedDecalReferenceBlock : TagStructure
        {
            public short IndexStart;
            public short IndexCount;
            public short VertexStart;
            public short VertexCount;
            public short DefinitionBlockIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint2d SpiritCorner;
            public RealVector2d SpiritSize;
        }
        
        [TagStructure(Size = 0x1C)]
        public class StructureBspSoundClusterBlock : TagStructure
        {
            public short PaletteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        
        [TagStructure(Size = 0x64)]
        public class StructureBspDebugInfoBlock : TagStructure
        {
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<StructureBspClusterDebugInfoBlock> Clusters;
            public List<StructureBspFogPlaneDebugInfoBlock> FogPlanes;
            public List<StructureBspFogZoneDebugInfoBlock> FogZones;
            
            [TagStructure(Size = 0x5C)]
            public class StructureBspClusterDebugInfoBlock : TagStructure
            {
                public StructureBspDebugInfoClusterErrorFlags Errors;
                public StructureBspDebugInfoClusterWarningFlags Warnings;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> FogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock> VisibleFogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock> VisFogOmissionClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock> ContainingFogZoneIndices;
                
                [Flags]
                public enum StructureBspDebugInfoClusterErrorFlags : ushort
                {
                    MultipleFogPlanes = 1 << 0,
                    FogZoneCollision = 1 << 1,
                    FogZoneImmersion = 1 << 2
                }
                
                [Flags]
                public enum StructureBspDebugInfoClusterWarningFlags : ushort
                {
                    MultipleVisibleFogPlanes = 1 << 0,
                    VisibleFogClusterOmission = 1 << 1,
                    FogPlaneMissedRenderBsp = 1 << 2
                }
                
                [TagStructure(Size = 0x20)]
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public StructureBspDebugInfoRenderLineTypeEnum Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum StructureBspDebugInfoRenderLineTypeEnum : short
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
            }
            
            [TagStructure(Size = 0x44)]
            public class StructureBspFogPlaneDebugInfoBlock : TagStructure
            {
                public int FogZoneIndex;
                [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int ConnectedPlaneDesignator;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> IntersectedClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock> InfExtentClusterIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public StructureBspDebugInfoRenderLineTypeEnum Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum StructureBspDebugInfoRenderLineTypeEnum : short
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
            }
            
            [TagStructure(Size = 0x50)]
            public class StructureBspFogZoneDebugInfoBlock : TagStructure
            {
                public int MediaIndex; // scenario fog plane*
                public int BaseFogPlaneIndex;
                [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<StructureBspDebugInfoRenderLineBlock> Lines;
                public List<StructureBspDebugInfoIndicesBlock> ImmersedClusterIndices;
                public List<StructureBspDebugInfoIndicesBlock> BoundingFogPlaneIndices;
                public List<StructureBspDebugInfoIndicesBlock> CollisionFogPlaneIndices;
                
                [TagStructure(Size = 0x20)]
                public class StructureBspDebugInfoRenderLineBlock : TagStructure
                {
                    public StructureBspDebugInfoRenderLineTypeEnum Type;
                    public short Code;
                    public short PadThai;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    
                    public enum StructureBspDebugInfoRenderLineTypeEnum : short
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
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalStructurePhysicsStruct : TagStructure
        {
            public List<MoppCodeDefinitionBlock> BreakableSurfacesMoppCodeBlock;
            public List<BreakableSurfaceKeyTableBlock> BreakableSurfaaceKeyTable;
            
            [TagStructure(Size = 0x20)]
            public class BreakableSurfaceKeyTableBlock : TagStructure
            {
                public short InstancedGeometryIndex;
                public byte BreakableSurfaceSetIndex;
                public byte BreakableSurfaceIndex;
                public int SeedSurfaceIndex;
                public float X0;
                public float X1;
                public float Y0;
                public float Y1;
                public float Z0;
                public float Z1;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class WidgetReferenceBlock : TagStructure
        {
            public short MarkerIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "lswd" })]
            public CachedTag WidgetRef;
        }
        
        [TagStructure(Size = 0x14)]
        public class CheapLightReferenceBlock : TagStructure
        {
            public short MarkerIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "gldf" })]
            public CachedTag CheapLight;
        }
        
        [TagStructure(Size = 0x20)]
        public class StructureBspResourceInterface : TagStructure
        {
            public List<StructureBspRawResources> RawResources;
            public TagResourceReference TagResources;
            public TagResourceReference CacheFileResources;
            public int UseResourceItems;
            
            [TagStructure(Size = 0x30)]
            public class StructureBspRawResources : TagStructure
            {
                public StructureBspResourceStruct RawItems;
                
                [TagStructure(Size = 0x30)]
                public class StructureBspResourceStruct : TagStructure
                {
                    public List<GlobalCollisionBspBlock> CollisionBsp;
                    public List<GlobalLargeCollisionBspBlock> LargeCollisionBsp;
                    public List<StructureBspInstancedGeometryDefinitionBlock> InstancedGeometriesDefinitions;
                    public List<StructureiohavokDataBlockStruct> HavokData;
                    
                    [TagStructure(Size = 0x6C)]
                    public class GlobalCollisionBspBlock : TagStructure
                    {
                        public List<Bsp3dNodesBlockStruct> Bsp3dNodes;
                        public List<Bsp3dKdSupdernodesBlock> Bsp3dSupernodes;
                        public List<PlanesBlock> Planes;
                        public List<CollisionLeafStruct> Leaves;
                        public List<Bsp2dReferencesBlock> Bsp2dReferences;
                        public List<Bsp2dNodesBlock> Bsp2dNodes;
                        public List<SurfacesBlockStruct> Surfaces;
                        public List<EdgesBlock> Edges;
                        public List<VerticesBlock> Vertices;
                        
                        [TagStructure(Size = 0x8)]
                        public class Bsp3dNodesBlockStruct : TagStructure
                        {
                            public long NodeDataDesignator;
                        }
                        
                        [TagStructure(Size = 0x80)]
                        public class Bsp3dKdSupdernodesBlock : TagStructure
                        {
                            public float Plane0;
                            public float Plane1;
                            public float Plane2;
                            public float Plane3;
                            public float Plane4;
                            public float Plane5;
                            public float Plane6;
                            public float Plane7;
                            public float Plane8;
                            public float Plane9;
                            public float Plane10;
                            public float Plane11;
                            public float Plane12;
                            public float Plane13;
                            public float Plane14;
                            public int PlaneDimensions;
                            public int ChildIndex0;
                            public int ChildIndex1;
                            public int ChildIndex2;
                            public int ChildIndex3;
                            public int ChildIndex4;
                            public int ChildIndex5;
                            public int ChildIndex6;
                            public int ChildIndex7;
                            public int ChildIndex8;
                            public int ChildIndex9;
                            public int ChildIndex10;
                            public int ChildIndex11;
                            public int ChildIndex12;
                            public int ChildIndex13;
                            public int ChildIndex14;
                            public int ChildIndex15;
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class PlanesBlock : TagStructure
                        {
                            public RealPlane3d Plane;
                        }
                        
                        [TagStructure(Size = 0x8)]
                        public class CollisionLeafStruct : TagStructure
                        {
                            public LeafFlags Flags;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public ushort Bsp2dReferenceCount;
                            public uint FirstBsp2dReference;
                            
                            [Flags]
                            public enum LeafFlags : byte
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
                        
                        [TagStructure(Size = 0xC)]
                        public class SurfacesBlockStruct : TagStructure
                        {
                            public ushort PlaneIndex;
                            public ushort FirstEdge;
                            public short Material;
                            public short BreakableSurfaceSet;
                            public short BreakableSurface;
                            public SurfaceFlags Flags;
                            public byte BestPlaneCalculationVertexIndex;
                            
                            [Flags]
                            public enum SurfaceFlags : byte
                            {
                                TwoSided = 1 << 0,
                                Invisible = 1 << 1,
                                Climbable = 1 << 2,
                                Breakable = 1 << 3,
                                Invalid = 1 << 4,
                                Conveyor = 1 << 5,
                                Slip = 1 << 6,
                                PlaneNegated = 1 << 7
                            }
                        }
                        
                        [TagStructure(Size = 0xC)]
                        public class EdgesBlock : TagStructure
                        {
                            public ushort StartVertex;
                            public ushort EndVertex;
                            public ushort ForwardEdge;
                            public ushort ReverseEdge;
                            public ushort LeftSurface;
                            public ushort RightSurface;
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class VerticesBlock : TagStructure
                        {
                            public RealPoint3d Point;
                            public ushort FirstEdge;
                            public short Sink;
                        }
                    }
                    
                    [TagStructure(Size = 0x6C)]
                    public class GlobalLargeCollisionBspBlock : TagStructure
                    {
                        public List<LargeBsp3dNodesBlock> Bsp3dNodes;
                        public List<Bsp3dKdSupdernodesBlock> Bsp3dSupernodes;
                        public List<PlanesBlock> Planes;
                        public List<LargeLeavesBlock> Leaves;
                        public List<LargeBsp2dReferencesBlock> Bsp2dReferences;
                        public List<LargeBsp2dNodesBlock> Bsp2dNodes;
                        public List<LargeSurfacesBlockStruct> Surfaces;
                        public List<LargeEdgesBlock> Edges;
                        public List<LargeVerticesBlock> Vertices;
                        
                        [TagStructure(Size = 0xC)]
                        public class LargeBsp3dNodesBlock : TagStructure
                        {
                            public int Plane;
                            public int BackChild;
                            public int FrontChild;
                        }
                        
                        [TagStructure(Size = 0x80)]
                        public class Bsp3dKdSupdernodesBlock : TagStructure
                        {
                            public float Plane0;
                            public float Plane1;
                            public float Plane2;
                            public float Plane3;
                            public float Plane4;
                            public float Plane5;
                            public float Plane6;
                            public float Plane7;
                            public float Plane8;
                            public float Plane9;
                            public float Plane10;
                            public float Plane11;
                            public float Plane12;
                            public float Plane13;
                            public float Plane14;
                            public int PlaneDimensions;
                            public int ChildIndex0;
                            public int ChildIndex1;
                            public int ChildIndex2;
                            public int ChildIndex3;
                            public int ChildIndex4;
                            public int ChildIndex5;
                            public int ChildIndex6;
                            public int ChildIndex7;
                            public int ChildIndex8;
                            public int ChildIndex9;
                            public int ChildIndex10;
                            public int ChildIndex11;
                            public int ChildIndex12;
                            public int ChildIndex13;
                            public int ChildIndex14;
                            public int ChildIndex15;
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class PlanesBlock : TagStructure
                        {
                            public RealPlane3d Plane;
                        }
                        
                        [TagStructure(Size = 0x8)]
                        public class LargeLeavesBlock : TagStructure
                        {
                            public LeafFlags Flags;
                            public short Bsp2dReferenceCount;
                            public int FirstBsp2dReference;
                            
                            [Flags]
                            public enum LeafFlags : ushort
                            {
                                ContainsDoubleSidedSurfaces = 1 << 0
                            }
                        }
                        
                        [TagStructure(Size = 0x8)]
                        public class LargeBsp2dReferencesBlock : TagStructure
                        {
                            public int Plane;
                            public int Bsp2dNode;
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class LargeBsp2dNodesBlock : TagStructure
                        {
                            public RealPlane2d Plane;
                            public int LeftChild;
                            public int RightChild;
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class LargeSurfacesBlockStruct : TagStructure
                        {
                            public int PlaneIndex;
                            public int FirstEdge;
                            public short Material;
                            public short BreakableSurfaceSet;
                            public short BreakableSurface;
                            public SurfaceFlags Flags;
                            public byte BestPlaneCalculationVertexIndex;
                            
                            [Flags]
                            public enum SurfaceFlags : byte
                            {
                                TwoSided = 1 << 0,
                                Invisible = 1 << 1,
                                Climbable = 1 << 2,
                                Breakable = 1 << 3,
                                Invalid = 1 << 4,
                                Conveyor = 1 << 5,
                                Slip = 1 << 6,
                                PlaneNegated = 1 << 7
                            }
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class LargeEdgesBlock : TagStructure
                        {
                            public int StartVertex;
                            public int EndVertex;
                            public int ForwardEdge;
                            public int ReverseEdge;
                            public int LeftSurface;
                            public int RightSurface;
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class LargeVerticesBlock : TagStructure
                        {
                            public RealPoint3d Point;
                            public int FirstEdge;
                            public int Sink;
                        }
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class StructureBspInstancedGeometryDefinitionBlock : TagStructure
                    {
                        public int Checksum;
                        public InstancedGeometryDefinitionFlags Flags;
                        public short MeshIndex;
                        public short CompressionIndex;
                        public float GlobalLightmapResolutionScale;
                        public short ExternalIndex;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
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
                    }
                    
                    [TagStructure(Size = 0x48)]
                    public class StructureiohavokDataBlockStruct : TagStructure
                    {
                        public int Version;
                        public int RuntimeDeserializedBodyPointer;
                        public int RuntimeDeserializedDataPointer;
                        public int PrefabIndex;
                        public byte[] SerializedHavokData;
                        public List<SerializedHavokGeometryDataBlockStruct> SerializedPerCollisionTypeHavokGeometry;
                        public RealPoint3d ShapesBoundsMin;
                        public RealPoint3d ShapesBoundsMax;
                        
                        [TagStructure(Size = 0x34)]
                        public class SerializedHavokGeometryDataBlockStruct : TagStructure
                        {
                            public byte[] SerializedHavokData;
                            public byte[] SerializedStaticHavokData;
                            public int CollisionType;
                            public int RuntimeDeserializedBodyPointer;
                            public int RuntimeDeserializedDataPointer;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class StructureiohavokDataBlockStruct : TagStructure
        {
            public int Version;
            public int RuntimeDeserializedBodyPointer;
            public int RuntimeDeserializedDataPointer;
            public int PrefabIndex;
            public byte[] SerializedHavokData;
            public List<SerializedHavokGeometryDataBlockStruct> SerializedPerCollisionTypeHavokGeometry;
            public RealPoint3d ShapesBoundsMin;
            public RealPoint3d ShapesBoundsMax;
            
            [TagStructure(Size = 0x34)]
            public class SerializedHavokGeometryDataBlockStruct : TagStructure
            {
                public byte[] SerializedHavokData;
                public byte[] SerializedStaticHavokData;
                public int CollisionType;
                public int RuntimeDeserializedBodyPointer;
                public int RuntimeDeserializedDataPointer;
            }
        }
        
        [TagStructure(Size = 0x70)]
        public class StructureExternalInstancedGeometryReferencesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "prfb" })]
            public CachedTag PrefabReference;
            public StringId Name;
            public float Scale;
            public RealVector3d Forward;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealPoint3d Position;
            public short MeshCount;
            public short MeshBlockIndex;
            public short LightCount;
            public short LightInstanceBlockIndex;
            public short DynamicObjectCount;
            public short DynamicObjectBlockIndex;
            public InstancedGeometryFlags OverrideFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public InstancedGeometryPathfindingPolicyEnum OverridePathfindingPolicy;
            public InstancedGeometryLightmappingPolicyEnum OverrideLightmappingPolicy;
            public InstancedGeometryImposterPolicyEnum OverrideImposterPolicy;
            public InstancedGeometryStreamingpriorityEnum OverrideStreamingPriority;
            public float OverrideLightmapResolutionScale;
            public float OverrideImposterTransitionDistance;
            public float OverrideImposterBrightness;
            public float OverrideLightChannelFlags;
            public InstancedGeometryFlags InstanceFlagsMask;
            public PrefaboverrideFlags InstancePolicyMask;
            
            [Flags]
            public enum InstancedGeometryFlags : ushort
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
                Unknown = 1 << 9,
                RemoveFromShadowGeometry = 1 << 10,
                CinemaOnly = 1 << 11,
                ExcludeFromCinema = 1 << 12,
                DisallowObjectLightingSamples = 1 << 13
            }
            
            public enum InstancedGeometryPathfindingPolicyEnum : sbyte
            {
                CutOut,
                Static,
                None
            }
            
            public enum InstancedGeometryLightmappingPolicyEnum : sbyte
            {
                PerPixelShared,
                PerVertex,
                SingleProbe,
                Exclude,
                PerPixelAo,
                PerVertexAo
            }
            
            public enum InstancedGeometryImposterPolicyEnum : sbyte
            {
                PolygonDefault,
                PolygonHigh,
                CardsMedium,
                CardsHigh,
                None,
                RainbowBox
            }
            
            public enum InstancedGeometryStreamingpriorityEnum : sbyte
            {
                Default,
                Higher,
                Highest
            }
            
            [Flags]
            public enum PrefaboverrideFlags : ushort
            {
                OverridePathfindingPolicy = 1 << 0,
                OverrideLightmappingPolicy = 1 << 1,
                OverrideLmposterPolicy = 1 << 2,
                OverrideLightmapResolutionPolicy = 1 << 3,
                OverrideImposterTransitionDistancePolicy = 1 << 4,
                OverrideLightChannelFlagsPolicy = 1 << 5,
                OverrideImposterBrightness = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class StructureBspObbVolumeBlock : TagStructure
        {
            public RealPoint3d Origin;
            public RealVector3d Axis1;
            public RealVector3d Axis2;
            public RealVector3d Axis3;
            public uint Type;
        }
        
        [TagStructure(Size = 0x10)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x10)]
        public class AnimGraphDependencyBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jmad" })]
            public CachedTag Graph;
        }
    }
}
