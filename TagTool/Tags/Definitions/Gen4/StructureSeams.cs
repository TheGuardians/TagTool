using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "structure_seams", Tag = "stse", Size = 0x64)]
    public class StructureSeams : TagStructure
    {
        public StructureManifestStruct StructureManifest;
        public int Version;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<StructureSeamBlock> Seams;
        public List<MoppCodeDefinitionBlock> SeamTriangleMoppCodeBlock;
        
        [TagStructure(Size = 0x3C)]
        public class StructureManifestStruct : TagStructure
        {
            public StructureManifestBuildIdentifierStruct BuildIdentifer;
            // for local builds, this is the content build identifier you are based on
            public StructureManifestBuildIdentifierStruct ParentBuildIdentifer;
            public List<StructureManifestBspBlock> BspManifest;
            
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
            
            [TagStructure(Size = 0x130)]
            public class StructureManifestBspBlock : TagStructure
            {
                public StructureManifestBuildIdentifierStruct BuildIdentifer;
                public StructureManifestBuildIdentifierStruct ParentBuildIdentifer;
                [TagField(Length = 256)]
                public string BspName;
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
        
        [TagStructure(Size = 0x4C)]
        public class StructureSeamBlock : TagStructure
        {
            public StructureSeamIdentifierStruct Identifier;
            public StructureSeamOriginalGeometryStruct Original;
            public StructureSeamFinalGeometryStruct Final;
            
            [TagStructure(Size = 0x10)]
            public class StructureSeamIdentifierStruct : TagStructure
            {
                public int SeamId0;
                public int SeamId1;
                public int SeamId2;
                public int SeamId3;
            }
            
            [TagStructure(Size = 0xC)]
            public class StructureSeamOriginalGeometryStruct : TagStructure
            {
                public List<StructureSeamOriginalVertexBlock> OriginalVertices;
                
                [TagStructure(Size = 0x1C)]
                public class StructureSeamOriginalVertexBlock : TagStructure
                {
                    public RealPoint3d OriginalVertex;
                    public int FinalPointIndex;
                    public List<StructureSeamOriginalVertexPlaneNormalsBlock> PlaneNormals;
                    
                    [TagStructure(Size = 0xC)]
                    public class StructureSeamOriginalVertexPlaneNormalsBlock : TagStructure
                    {
                        public RealVector3d TriangleNormal;
                    }
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class StructureSeamFinalGeometryStruct : TagStructure
            {
                public List<StructureSeamFinalPlanesBlock> Planes;
                public List<StructureSeamFinalPointsBlock> Points;
                public List<StructureSeamFinalTrianglesBlock> Triangles;
                public List<StructureSeamFinalEdgesBlock> Edges;
                
                [TagStructure(Size = 0x10)]
                public class StructureSeamFinalPlanesBlock : TagStructure
                {
                    public RealPlane3d Plane;
                }
                
                [TagStructure(Size = 0xC)]
                public class StructureSeamFinalPointsBlock : TagStructure
                {
                    public RealPoint3d FinalPoint;
                }
                
                [TagStructure(Size = 0xC)]
                public class StructureSeamFinalTrianglesBlock : TagStructure
                {
                    public int FinalPlane;
                    public short FinalPoint0;
                    public short FinalPoint1;
                    public short FinalPoint2;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x4)]
                public class StructureSeamFinalEdgesBlock : TagStructure
                {
                    public short FinalPoint0;
                    public short FinalPoint1;
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
}
