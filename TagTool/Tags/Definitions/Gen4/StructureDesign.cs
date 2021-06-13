using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x16C)]
    public class StructureDesign : TagStructure
    {
        public StructureManifestBuildIdentifierStruct BuildIdentifier;
        public StructureManifestBuildIdentifierStruct ParentBuildIdentifier;
        public GlobalStructurePhysicsDesignStruct Physics;
        public PlanarFogSetDefinitionStruct PlanarFogSet;
        public GlobalRenderGeometryStruct RenderGeometry;
        public List<StructureBspInstancedGeometryDefinitionBlock> InstancedGeometriesDefinitions;
        public List<StructureBspInstancedGeometryInstancesBlock> InstancedGeometryInstances;
        public List<GlobalGeometryMaterialBlock> Materials;
        public List<MoppCodeDefinitionBlock> RainBlockerMoppCodeBlock;
        
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
        
        [TagStructure(Size = 0x40)]
        public class GlobalStructurePhysicsDesignStruct : TagStructure
        {
            public int ImporterVersion;
            public List<MoppCodeDefinitionBlock> SoftCeilingMoppCodeBlock;
            public List<StructureSoftCeilingBlock> SoftCeilingsBlock;
            public List<MoppCodeDefinitionBlock> WaterMoppCode;
            public List<StructureWaterGroupsBlock> WaterGroupsBlock;
            public List<StructureWaterInstancesBlock> WaterInstancesBlock;
            
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
            
            [TagStructure(Size = 0x14)]
            public class StructureSoftCeilingBlock : TagStructure
            {
                public StringId Name;
                public SoftCeilingTypeEnum Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<StructureSoftCeilingTriangleBlock> SoftCeilingTriangles;
                
                public enum SoftCeilingTypeEnum : short
                {
                    Acceleration,
                    SoftKill,
                    SlipSurface
                }
                
                [TagStructure(Size = 0x44)]
                public class StructureSoftCeilingTriangleBlock : TagStructure
                {
                    public RealPlane3d Plane;
                    public RealPoint3d BoundingSphereCenter;
                    public float BoundingSphereRadius;
                    public RealPoint3d Vertex0;
                    public RealPoint3d Vertex1;
                    public RealPoint3d Vertex2;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class StructureWaterGroupsBlock : TagStructure
            {
                public StringId Name;
            }
            
            [TagStructure(Size = 0x54)]
            public class StructureWaterInstancesBlock : TagStructure
            {
                public short Group;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d FlowVelocity;
                public RealArgbColor FogColor;
                public float FogMurkiness;
                public List<StructureWaterInstancePlanesBlock> WaterPlanesBlock;
                public List<StructureWaterInstanceDebugTrianglesBlock> WaterDebugTrianglesBlock;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                
                [TagStructure(Size = 0x10)]
                public class StructureWaterInstancePlanesBlock : TagStructure
                {
                    public RealPlane3d Plane;
                }
                
                [TagStructure(Size = 0x24)]
                public class StructureWaterInstanceDebugTrianglesBlock : TagStructure
                {
                    public RealPoint3d Point0;
                    public RealPoint3d Point1;
                    public RealPoint3d Point2;
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class PlanarFogSetDefinitionStruct : TagStructure
        {
            public List<PlanarFogDefinitionBlock> PlanarFogs;
            public List<MoppCodeDefinitionBlock> MoppCode;
            
            [TagStructure(Size = 0x3C)]
            public class PlanarFogDefinitionBlock : TagStructure
            {
                public StringId Name;
                [TagField(ValidTags = new [] { "pfpt" })]
                public CachedTag AppearanceSettings;
                public List<PlanarFogVertexBlock> Vertices;
                public List<PlanarFogTriangleBlock> Triangles;
                public float Depth;
                public RealVector3d Normal;
                
                [TagStructure(Size = 0xC)]
                public class PlanarFogVertexBlock : TagStructure
                {
                    public RealPoint3d Position;
                }
                
                [TagStructure(Size = 0xC)]
                public class PlanarFogTriangleBlock : TagStructure
                {
                    public List<PlanarFogTrianglePlanesBlock> Planes;
                    
                    [TagStructure(Size = 0x10)]
                    public class PlanarFogTrianglePlanesBlock : TagStructure
                    {
                        public RealPlane3d Plane;
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
    }
}
