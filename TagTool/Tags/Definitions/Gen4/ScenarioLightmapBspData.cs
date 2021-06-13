using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_lightmap_bsp_data", Tag = "Lbsp", Size = 0x3E8)]
    public class ScenarioLightmapBspData : TagStructure
    {
        public ScenarioLightmapBspFlags Flags;
        public short BspReferenceIndex;
        public int ChecksumFromStructureBsp;
        public int GeneralStructureLightingImportChecksum;
        public int SkydomeStructureLightingImportChecksum;
        public int CombinedExtraStructureLightingImportChecksum;
        public float PerpixelCompressionScalarDirect;
        public float PerpixelCompressionScalarIndirectNew;
        public float PervertexCompressionScalarDirect;
        public float PervertexCompressionScalarIndirect;
        public RealVector3d FloatingShadowLightDirection;
        public RealVector3d FloatingShadowLightIntensity;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelDirection;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelAnalytic;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelOverlayMicro;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelOverlayMacro;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridProbeDataPerPixelRefinementDxt3a;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridVmfProbeDataPerVertex565;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HybridVmfProbeDataPerVertexLite565;
        public List<ScenarioLightmapClusterData> Clusters;
        public List<ScenarioLightmapInstanceData> Instances;
        public List<ScenarioLightmapLightprobeValue> Probes;
        public List<ScenarioLightmapInstanceIndexBlock> ShadowGeometryExcludedInstanceIndices;
        public List<ScenarioLightmapGlobalPerpixelPaddingData> PerPixelPadding;
        public List<ScenarioLightmapRasterizedChartData> PerPixelRasterizedCharts;
        public List<ScenarioLightmapNewAoDataBlock> NewAoData;
        public List<ScenarioLightmapAdjacentBounceVertexBlock> AdjacentBounceVertices;
        public List<ScenarioLightmapAdjacentBounceIndexBlock> AdjacentBounceIndices;
        public int ParameterizationMethodChecksum;
        public int NewAoChecksum;
        public int MaterialCount;
        public int MeshCount;
        public short LightmapParameterizationWidth;
        public short LightmapParameterizationHeight;
        public GlobalRenderGeometryStruct ImportedGeometry;
        public List<TriangleMappingPerMeshBlock> PerMeshTriangleMapping;
        public GlobalRenderGeometryStruct ShadowGeometry;
        public GlobalRenderGeometryStruct DynamicLightShadowGeometry;
        public List<ScenarioLightmapDynamicLightInstance> LightInstanceData;
        public List<ScenarioLightmapStructureLightInstance> StructureLightInstanceData;
        public List<SScenarioLightmapSilhouetteVertex> ExtrudedSilhouetteVertices;
        public List<SScenarioLightmapSilhouetteEdge> ExtrudedSilhouetteEdges;
        public List<SScenarioLightmapSilhouetteGroup> ExtrudedSilhouetteGroups;
        public List<ScenarioLightmapAirprobeValue> Airprobes;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<GlobalSelfTrackBlock> SelfTrack;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ForgePerPixelColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ForgePerPixelSun;
        public float WorldScaleRatio;
        
        [Flags]
        public enum ScenarioLightmapBspFlags : ushort
        {
            Compressed = 1 << 0,
            XsyncedGeometry = 1 << 1,
            Relightmapped = 1 << 2,
            GenerateFakeSmallLightmaps = 1 << 3,
            GeneratedFromMatchData = 1 << 4,
            OnlyACheckerBoard = 1 << 5,
            SurfaceToTriangleMappingPruned = 1 << 6,
            FakedLightmapTagForCacheBuild = 1 << 7,
            OptimizedForLessDpAll = 1 << 8,
            FloatingShadowsEnabled = 1 << 9,
            AtlasUnrefinedPacking = 1 << 10,
            AtlasRepacked = 1 << 11,
            UsingSimplifiedIrradianceLighting = 1 << 12,
            DisableShadowGeometry = 1 << 13,
            DisableHybridRefinement = 1 << 14
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioLightmapClusterData : TagStructure
        {
            public short LightprobeTextureArrayIndex;
            public short PervertexBlockIndex;
            public int PervertexBlockOffset;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioLightmapInstanceData : TagStructure
        {
            public short LightprobeTextureArrayIndex;
            public short PervertexBlockIndex;
            public short ProbeBlockIndex;
            public sbyte AnalyticalLightCollisionWarning;
            public LightmapdebugUvpolicyDefinition LightmapDebugUvPolicy;
            public int AnalyticalLightIndex;
            public int PerVertexLightprobeTextureOffset;
            public uint MatchingFlags;
            public uint MeshIndex;
            public RealPoint3d InstanceWorldSpacePosition;
            
            public enum LightmapdebugUvpolicyDefinition : sbyte
            {
                None,
                Probe,
                Vertex,
                VertexAo,
                SuppliedUv,
                AutoUv,
                DiffuseUv,
                AutoDiffuse
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class ScenarioLightmapLightprobeValue : TagStructure
        {
            [TagField(Length = 16)]
            public DualVmfTerms[]  VmfTerms;
            public uint AnalyticalLightIndex;
            public HalfRgbLightprobeStruct ShTerms;
            
            [TagStructure(Size = 0x2)]
            public class DualVmfTerms : TagStructure
            {
                public short DualVmfCoefficient;
            }
            
            [TagStructure(Size = 0x38)]
            public class HalfRgbLightprobeStruct : TagStructure
            {
                [TagField(Length = 9)]
                public HalfShTerms[]  RedShTerms;
                [TagField(Length = 9)]
                public HalfShTerms[]  GreenShTerms;
                [TagField(Length = 9)]
                public HalfShTerms[]  BlueShTerms;
                public short AnalyticalVisibility;
                
                [TagStructure(Size = 0x2)]
                public class HalfShTerms : TagStructure
                {
                    public short Coefficient;
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioLightmapInstanceIndexBlock : TagStructure
        {
            public int Indices;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioLightmapGlobalPerpixelPaddingData : TagStructure
        {
            public int X;
            public int Y;
            public int SourceX;
            public int SourceY;
        }
        
        [TagStructure(Size = 0x38)]
        public class ScenarioLightmapRasterizedChartData : TagStructure
        {
            public int Width;
            public int Height;
            public int Flipx;
            public int Chartrotation;
            public int Atlasx;
            public int Atlasy;
            public RealVector2d Roundedsize;
            public List<ScenarioLightmapRasterizedChartSource> Sourcevertices;
            public List<ScenarioLightmapRasterizedChartElement> Chartbitmap;
            
            [TagStructure(Size = 0x8)]
            public class ScenarioLightmapRasterizedChartSource : TagStructure
            {
                public int Streamindex;
                public int Vertexindex;
            }
            
            [TagStructure(Size = 0x1)]
            public class ScenarioLightmapRasterizedChartElement : TagStructure
            {
                public byte Composite;
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class ScenarioLightmapNewAoDataBlock : TagStructure
        {
            public sbyte Value;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioLightmapAdjacentBounceVertexBlock : TagStructure
        {
            public RealVector3d Position;
            public short ColorR;
            public short ColorG;
            public short ColorB;
            public short ColorA;
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioLightmapAdjacentBounceIndexBlock : TagStructure
        {
            public int Index;
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
        
        [TagStructure(Size = 0xC)]
        public class TriangleMappingPerMeshBlock : TagStructure
        {
            public List<TriangleMappingBlock> Mesh;
            
            [TagStructure(Size = 0x4)]
            public class TriangleMappingBlock : TagStructure
            {
                public int Word;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioLightmapDynamicLightInstance : TagStructure
        {
            public float MinDepth;
            public int LightInstanceChecksum;
            public List<ScenarioLightmapDynamicLightInstanceDataBlock> InstanceIndices;
            
            [TagStructure(Size = 0x8)]
            public class ScenarioLightmapDynamicLightInstanceDataBlock : TagStructure
            {
                public int Index;
                public int ShadowGeometryMeshIndex;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ScenarioLightmapStructureLightInstance : TagStructure
        {
            public int ShadowGeometryMeshIndex;
        }
        
        [TagStructure(Size = 0xC)]
        public class SScenarioLightmapSilhouetteVertex : TagStructure
        {
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x8)]
        public class SScenarioLightmapSilhouetteEdge : TagStructure
        {
            public int FirstIndex;
            public int SecondIndex;
        }
        
        [TagStructure(Size = 0x20)]
        public class SScenarioLightmapSilhouetteGroup : TagStructure
        {
            public int FirstEdge;
            public int EdgeCount;
            public RealVector3d Direction;
            public float AttenuationFactor;
            public float AttenuationDistance;
            public float ShaftIntensity;
        }
        
        [TagStructure(Size = 0x50)]
        public class ScenarioLightmapAirprobeValue : TagStructure
        {
            public RealPoint3d AirprobePosition;
            public StringId AirprobeName;
            public int BspIndex;
            public uint AnalyticalLightIndex;
            public HalfRgbLightprobeStruct ShTerms;
            
            [TagStructure(Size = 0x38)]
            public class HalfRgbLightprobeStruct : TagStructure
            {
                [TagField(Length = 9)]
                public HalfShTerms[]  RedShTerms;
                [TagField(Length = 9)]
                public HalfShTerms[]  GreenShTerms;
                [TagField(Length = 9)]
                public HalfShTerms[]  BlueShTerms;
                public short AnalyticalVisibility;
                
                [TagStructure(Size = 0x2)]
                public class HalfShTerms : TagStructure
                {
                    public short Coefficient;
                }
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
        
        [TagStructure(Size = 0x240)]
        public class GlobalSelfTrackBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Time;
            [TagField(Length = 32)]
            public string Machine;
            [TagField(Length = 256)]
            public string Version;
            [TagField(Length = 256)]
            public string Command;
        }
    }
}
