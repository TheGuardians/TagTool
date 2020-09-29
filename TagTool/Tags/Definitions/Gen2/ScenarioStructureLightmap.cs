using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_lightmap", Tag = "ltmp", Size = 0x10C)]
    public class ScenarioStructureLightmap : TagStructure
    {
        /// <summary>
        /// lightmap options
        /// </summary>
        /// <remarks>
        /// The following fields control the behavior of the lightmapper
        /// 
        /// RADIANCE ESTIMATE SEARCH DISTANCE UPPER BOUND: the largest distance the code will look for photons. bigger levels need a bigger search radius.  Measured, in world units, 0.0 defaults to 1.0
        /// 
        /// RADIANCE ESTIMATE SEARCH DISTANCE LOWER BOUND: the smallest distance the code will look for photons. bigger levels need a bigger search radius.  Measured, in world units, 0.0 defaults to 1.0
        /// 
        /// LUMINELS PER WORLD UNIT: how many lightmap pixels there should be per world unit.  0.0 defaults to 3.0
        /// 
        /// OUTPUT WHITE REFERENCE: for experimentation - what wattage the lightmapper considers "white" to be for output.  0.0 defaults to 1.0
        /// 
        /// OUTPUT BLACK REFERENCE: for experimentation - what wattage the lightmapper considers "black" to be for output.  0.0 defaults to 0.0
        /// 
        /// OUTPUT SCHLICK PARAMETER: controls the way midtones are mapped.  0.0 defaults to 4.5
        /// 
        /// DIFFUSE MAP SCALE: controls how diffuse maps are scaled.  0.0 defaults to 1.5
        /// 
        /// PRT SUN SCALE: 0.0 defaults to 100.0
        /// 
        /// PRT SKY SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT INDIRECT SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT SCALE: you must set this value.
        /// 
        /// PRT SURFACE LIGHT SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT SCENARIO LIGHT SCALE: 0.0 defaults to 1.0
        /// 
        /// LIGHTPROBE INTERPOLATION OVERIDE(speed): overide the default sampling behavior
        /// </remarks>
        public float SearchDistanceLowerBound;
        public float SearchDistanceUpperBound;
        public float LuminelsPerWorldUnit;
        public float OutputWhiteReference;
        public float OutputBlackReference;
        public float OutputSchlickParameter;
        public float DiffuseMapScale;
        public float SunScale;
        public float SkyScale;
        public float IndirectScale;
        public float PrtScale;
        public float SurfaceLightScale;
        public float ScenarioLightScale;
        public float LightprobeInterpolationOveride;
        [TagField(Flags = Padding, Length = 72)]
        public byte[] Padding1;
        public List<StructureLightmapGroup> LightmapGroups;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding2;
        public List<ErrorReportCategory> Errors;
        [TagField(Flags = Padding, Length = 104)]
        public byte[] Padding3;
        
        [TagStructure(Size = 0x9C)]
        public class StructureLightmapGroup : TagStructure
        {
            public TypeValue Type;
            public FlagsValue Flags;
            public int StructureChecksum;
            public List<SectionPaletteBlock> SectionPalette;
            public List<WritablePalettesBlock> WritablePalettes;
            public CachedTag BitmapGroup;
            public List<LightmapGeometrySection> Clusters;
            public List<LightmapGeometryRenderInfo> ClusterRenderInfo;
            public List<LightmapGeometrySection> PoopDefinitions;
            public List<StructureLightmapLightingEnvironment> LightingEnvironments;
            public List<LightmapVertexBufferBucket> GeometryBuckets;
            public List<LightmapGeometryRenderInfo> InstanceRenderInfo;
            public List<LightmapBucketReference> InstanceBucketRefs;
            public List<LightmapSceneryObjectInfo> SceneryObjectInfo;
            public List<LightmapBucketReference> SceneryObjectBucketRefs;
            
            public enum TypeValue : short
            {
                Normal
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
            
            [TagStructure(Size = 0x400)]
            public class SectionPaletteBlock : TagStructure
            {
                public int FirstPaletteColor;
                [TagField(Flags = Padding, Length = 1020)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0x400)]
            public class WritablePalettesBlock : TagStructure
            {
                public int FirstPaletteColor;
                [TagField(Flags = Padding, Length = 1020)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0x60)]
            public class LightmapGeometrySection : TagStructure
            {
                public GeometrySectionInfo GeometryInfo;
                public GeometryBlockInfoStruct GeometryBlockInfo;
                public List<LightmapGeometrySectionCacheData> CacheData;
                
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
                public class LightmapGeometrySectionCacheData : TagStructure
                {
                    public GeometrySection Geometry;
                    
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
            }
            
            [TagStructure(Size = 0x4)]
            public class LightmapGeometryRenderInfo : TagStructure
            {
                public short BitmapIndex;
                public sbyte PaletteIndex;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0xDC)]
            public class StructureLightmapLightingEnvironment : TagStructure
            {
                public RealPoint3d SamplePoint;
                public float RedCoefficient;
                [TagField(Length = 9)]
                public float RedCoefficients;
                public float GreenCoefficient;
                [TagField(Length = 9)]
                public float GreenCoefficients;
                public float BlueCoefficient;
                [TagField(Length = 9)]
                public float BlueCoefficients;
                public RealVector3d MeanIncomingLightDirection;
                public RealPoint3d IncomingLightIntensity;
                public int SpecularBitmapIndex;
                public RealVector3d RotationAxis;
                public float RotationSpeed;
                public RealVector3d BumpDirection;
                public RealRgbColor ColorTint;
                public ProceduralOverideValue ProceduralOveride;
                public FlagsValue Flags;
                public RealVector3d ProceduralParam0;
                public RealVector3d ProceduralParam1Xyz;
                public float ProceduralParam1W;
                
                public enum ProceduralOverideValue : short
                {
                    NoOveride,
                    CieClearSky,
                    CiePartlyCloudy,
                    CieCloudy,
                    DirectionalLight,
                    ConeLight,
                    SphereLight,
                    HemisphereLight
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    LeaveMeAlonePlease = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class LightmapVertexBufferBucket : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public List<LightmapBucketVertex> RawVertices;
                public GeometryBlockInfoStruct GeometryBlockInfo;
                public List<LightmapVertexBufferBucketCacheData> CacheData;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    IncidentDirection = 1 << 0,
                    Color = 1 << 1
                }
                
                [TagStructure(Size = 0x18)]
                public class LightmapBucketVertex : TagStructure
                {
                    public RealRgbColor PrimaryLightmapColor;
                    public RealVector3d PrimaryLightmapIncidentDirection;
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
                
                [TagStructure(Size = 0xC)]
                public class LightmapVertexBufferBucketCacheData : TagStructure
                {
                    public List<RasterizerVertexBuffer> VertexBuffers;
                    
                    [TagStructure(Size = 0x20)]
                    public class RasterizerVertexBuffer : TagStructure
                    {
                        public VertexBuffer VertexBuffer;
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class LightmapBucketReference : TagStructure
            {
                public short Flags;
                public short BucketIndex;
                public List<Word> SectionOffsets;
                
                [TagStructure(Size = 0x2)]
                public class Word : TagStructure
                {
                    public short SectionOffset;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class LightmapSceneryObjectInfo : TagStructure
            {
                public int UniqueId;
                public short OriginBspIndex;
                public sbyte Type;
                public sbyte Source;
                public int RenderModelChecksum;
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
    }
}

