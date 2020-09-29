using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_structure_lightmap", Tag = "ltmp", Size = 0x104)]
    public class ScenarioStructureLightmap : TagStructure
    {
        /// <summary>
        /// The following fields control the behavior of the lightmapper
        /// 
        /// RADIANCE ESTIMATE SEARCH DISTANCE UPPER BOUND: the largest
        /// distance the code will look for photons. bigger levels need a bigger search radius.  Measured, in world units, 0.0
        /// defaults to 1.0
        /// 
        /// RADIANCE ESTIMATE SEARCH DISTANCE LOWER BOUND: the smallest distance the code will look for photons.
        /// bigger levels need a bigger search radius.  Measured, in world units, 0.0 defaults to 1.0
        /// 
        /// LUMINELS PER WORLD UNIT: how
        /// many lightmap pixels there should be per world unit.  0.0 defaults to 3.0
        /// 
        /// OUTPUT WHITE REFERENCE: for experimentation -
        /// what wattage the lightmapper considers "white" to be for output.  0.0 defaults to 1.0
        /// 
        /// OUTPUT BLACK REFERENCE: for
        /// experimentation - what wattage the lightmapper considers "black" to be for output.  0.0 defaults to 0.0
        /// 
        /// OUTPUT SCHLICK
        /// PARAMETER: controls the way midtones are mapped.  0.0 defaults to 4.5
        /// 
        /// DIFFUSE MAP SCALE: controls how diffuse maps are
        /// scaled.  0.0 defaults to 1.5
        /// 
        /// PRT SUN SCALE: 0.0 defaults to 100.0
        /// 
        /// PRT SKY SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT INDIRECT
        /// SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT SCALE: you must set this value.
        /// 
        /// PRT SURFACE LIGHT SCALE: 0.0 defaults to 1.0
        /// 
        /// PRT
        /// SCENARIO LIGHT SCALE: 0.0 defaults to 1.0
        /// 
        /// LIGHTPROBE INTERPOLATION OVERIDE(speed): overide the default sampling behavior
        /// </summary>
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
        [TagField(Length = 0x48, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<StructureLightmapGroupBlock> LightmapGroups;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        [TagField(Length = 0x68, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        
        [TagStructure(Size = 0x68)]
        public class StructureLightmapGroupBlock : TagStructure
        {
            public TypeValue Type;
            public FlagsValue Flags;
            public int StructureChecksum;
            public List<StructureLightmapPaletteColorBlock> SectionPalette;
            public List<StructureLightmapPaletteColorBlock1> WritablePalettes;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BitmapGroup;
            public List<LightmapGeometrySectionBlock> Clusters;
            public List<LightmapGeometryRenderInfoBlock> ClusterRenderInfo;
            public List<LightmapGeometrySectionBlock1> PoopDefinitions;
            public List<StructureLightmapLightingEnvironmentBlock> LightingEnvironments;
            public List<LightmapVertexBufferBucketBlock> GeometryBuckets;
            public List<LightmapGeometryRenderInfoBlock1> InstanceRenderInfo;
            public List<LightmapInstanceBucketReferenceBlock> InstanceBucketRefs;
            public List<LightmapSceneryObjectInfoBlock> SceneryObjectInfo;
            public List<LightmapInstanceBucketReferenceBlock1> SceneryObjectBucketRefs;
            
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
            public class StructureLightmapPaletteColorBlock : TagStructure
            {
                public int FirstPaletteColor;
                [TagField(Length = 0x3FC)]
                public byte[] Unknown;
            }
            
            [TagStructure(Size = 0x400)]
            public class StructureLightmapPaletteColorBlock1 : TagStructure
            {
                public int FirstPaletteColor;
                [TagField(Length = 0x3FC)]
                public byte[] Unknown;
            }
            
            [TagStructure(Size = 0x54)]
            public class LightmapGeometrySectionBlock : TagStructure
            {
                public GlobalGeometrySectionInfoStructBlock GeometryInfo;
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<LightmapGeometrySectionCacheDataBlock> CacheData;
                
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
                    public List<GlobalGeometryCompressionInfoBlock> Unknown;
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
                public class LightmapGeometrySectionCacheDataBlock : TagStructure
                {
                    public GlobalGeometrySectionStructBlock Geometry;
                    
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
            }
            
            [TagStructure(Size = 0x4)]
            public class LightmapGeometryRenderInfoBlock : TagStructure
            {
                public short BitmapIndex;
                public sbyte PaletteIndex;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x54)]
            public class LightmapGeometrySectionBlock1 : TagStructure
            {
                public GlobalGeometrySectionInfoStructBlock GeometryInfo;
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<LightmapGeometrySectionCacheDataBlock> CacheData;
                
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
                    public List<GlobalGeometryCompressionInfoBlock> Unknown;
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
                public class LightmapGeometrySectionCacheDataBlock : TagStructure
                {
                    public GlobalGeometrySectionStructBlock Geometry;
                    
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
            }
            
            [TagStructure(Size = 0xDC)]
            public class StructureLightmapLightingEnvironmentBlock : TagStructure
            {
                public RealPoint3d SamplePoint;
                [TagField(Length = 9)]
                public float[] RedCoefficient;
                [TagField(Length = 9)]
                public float[] GreenCoefficient;
                [TagField(Length = 9)]
                public float[] BlueCoefficient;
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
            
            [TagStructure(Size = 0x38)]
            public class LightmapVertexBufferBucketBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<LightmapBucketRawVertexBlock> RawVertices;
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<LightmapVertexBufferBucketCacheDataBlock> CacheData;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    IncidentDirection = 1 << 0,
                    Color = 1 << 1
                }
                
                [TagStructure(Size = 0x18)]
                public class LightmapBucketRawVertexBlock : TagStructure
                {
                    public RealRgbColor PrimaryLightmapColor;
                    public RealVector3d PrimaryLightmapIncidentDirection;
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
                
                [TagStructure(Size = 0x8)]
                public class LightmapVertexBufferBucketCacheDataBlock : TagStructure
                {
                    public List<GlobalGeometrySectionVertexBufferBlock> VertexBuffers;
                    
                    [TagStructure(Size = 0x20)]
                    public class GlobalGeometrySectionVertexBufferBlock : TagStructure
                    {
                        public VertexBuffer VertexBuffer;
                    }
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class LightmapGeometryRenderInfoBlock1 : TagStructure
            {
                public short BitmapIndex;
                public sbyte PaletteIndex;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0xC)]
            public class LightmapInstanceBucketReferenceBlock : TagStructure
            {
                public short Flags;
                public short BucketIndex;
                public List<LightmapInstanceBucketSectionOffsetBlock> SectionOffsets;
                
                [TagStructure(Size = 0x2)]
                public class LightmapInstanceBucketSectionOffsetBlock : TagStructure
                {
                    public short SectionOffset;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class LightmapSceneryObjectInfoBlock : TagStructure
            {
                public int UniqueId;
                public short OriginBspIndex;
                public sbyte Type;
                public sbyte Source;
                public int RenderModelChecksum;
            }
            
            [TagStructure(Size = 0xC)]
            public class LightmapInstanceBucketReferenceBlock1 : TagStructure
            {
                public short Flags;
                public short BucketIndex;
                public List<LightmapInstanceBucketSectionOffsetBlock> SectionOffsets;
                
                [TagStructure(Size = 0x2)]
                public class LightmapInstanceBucketSectionOffsetBlock : TagStructure
                {
                    public short SectionOffset;
                }
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
    }
}

