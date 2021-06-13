using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "instance_imposter_definition", Tag = "iimz", Size = 0xF0)]
    public class InstanceImposterDefinition : TagStructure
    {
        public InstanceImposterFlags Flags;
        public StringId BspName;
        public int Checksum;
        public StringId SourceMetadataPath;
        public List<InstanceImposterBlock> Instances;
        public List<InstanceImposterChecksumBlock> InstanceChecksums;
        public short AtlasTileResolution;
        public sbyte AtlasXTileCount;
        public sbyte AtlasYTileCount;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag AtlasTexture;
        public GlobalRenderGeometryStruct RenderGeometry;
        
        [Flags]
        public enum InstanceImposterFlags : uint
        {
            RawGeometryCompressed = 1 << 0,
            UseRawInstanceImposterVerts = 1 << 1,
            ValidAndGoodToGo = 1 << 2,
            BuildCompleted = 1 << 3
        }
        
        [TagStructure(Size = 0x4)]
        public class InstanceImposterBlock : TagStructure
        {
            public short GroupIndex;
            public sbyte SubpartIndex;
            public InstanceImposterElementFlags Flags;
            
            [Flags]
            public enum InstanceImposterElementFlags : byte
            {
                Card = 1 << 0,
                Poly = 1 << 1,
                RainbowBox = 1 << 2,
                Occlusion = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class InstanceImposterChecksumBlock : TagStructure
        {
            public StringId Name;
            public int ImportChecksum;
            public int TransformChecksum;
            public float Scale;
            public RealVector3d Forward;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealPoint3d Position;
            public short ImposterVersion;
            public sbyte ImposterPolicy;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float BoundingRadius;
            public float TransitionDistance;
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
    }
}
