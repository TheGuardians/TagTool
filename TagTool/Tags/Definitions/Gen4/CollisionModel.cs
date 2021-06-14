using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "collision_model", Tag = "coll", Size = 0x58)]
    public class CollisionModel : TagStructure
    {
        public int ImportInfoChecksum;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public CollisionModelFlags Flags;
        public List<CollisionModelMaterialBlock> Materials;
        public List<CollisionModelRegionBlock> Regions;
        public List<CollisionModelRegionBlock> CookieCutters;
        public List<CollisionModelPathfindingSphereBlock> PathfindingSpheres;
        public List<CollisionModelNodeBlock> Nodes;
        public TagResourceReference RegionsResource;
        
        [Flags]
        public enum CollisionModelFlags : uint
        {
            ContainsOpenEdges = 1 << 0,
            PhysicsBuilt = 1 << 1,
            PhysicsInUse = 1 << 2,
            Processed = 1 << 3,
            HasTwoSidedSurfaces = 1 << 4
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
        
        [TagStructure(Size = 0x4)]
        public class CollisionModelMaterialBlock : TagStructure
        {
            public StringId Name;
        }
        
        [TagStructure(Size = 0x10)]
        public class CollisionModelRegionBlock : TagStructure
        {
            public StringId Name;
            public List<CollisionModelPermutationBlock> Permutations;
            
            [TagStructure(Size = 0x2C)]
            public class CollisionModelPermutationBlock : TagStructure
            {
                public StringId Name;
                public short ResourcebspOffset;
                public short ResourcebspCount;
                public List<CollisionModelBspStruct> Bsps;
                public List<CollisionBspPhysicsBlock> BspPhysics;
                public List<MoppCodeDefinitionBlock> MoppCodes;
                
                [TagStructure(Size = 0x70)]
                public class CollisionModelBspStruct : TagStructure
                {
                    public short NodeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    public GlobalCollisionBspStruct Bsp;
                    
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
                
                [TagStructure(Size = 0xB0)]
                public class CollisionBspPhysicsBlock : TagStructure
                {
                    public CollisionGeometryShapeStruct CollisionBspShape;
                    public MoppBvTreeShapeStruct MoppBvTreeShap;
                    
                    [TagStructure(Size = 0x60)]
                    public class CollisionGeometryShapeStruct : TagStructure
                    {
                        public HavokShapeCollectionStruct20102 Base;
                        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealVector3d Center;
                        public float HavokWCenter;
                        public RealVector3d HalfExtent;
                        public float HavokWHalfExtent;
                        public int RuntimeModelDefinitionTagIndex;
                        public int CollisionBspReferencePointer0;
                        public int CollisionBspReferencePointer1;
                        public sbyte StructureBspIndex;
                        public sbyte CollisionGeometryShapeType;
                        public short InstanceIndex;
                        public float Scale;
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        
                        [TagStructure(Size = 0x18)]
                        public class HavokShapeCollectionStruct20102 : TagStructure
                        {
                            public HavokShapeStruct20102 Base;
                            public int FieldPointerSkip;
                            public sbyte DisableWelding;
                            public sbyte CollectionType;
                            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            
                            [TagStructure(Size = 0x10)]
                            public class HavokShapeStruct20102 : TagStructure
                            {
                                public int FieldPointerSkip;
                                public short Size;
                                public short Count;
                                public int UserData;
                                public int Type;
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0x50)]
                    public class MoppBvTreeShapeStruct : TagStructure
                    {
                        public HavokShapeStruct20102 MoppBvTreeShape;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        public int MoppCodePointer;
                        public int MoppDataSkip;
                        public int MoppDataSize;
                        public RealVector3d CodeInfoCopy;
                        public float HavokWCodeInfoCopy;
                        public int ChildShapeVtable;
                        public int ChildShapePointer;
                        public int ChildSize;
                        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding2;
                        public float MoppScale;
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding3;
                        
                        [TagStructure(Size = 0x10)]
                        public class HavokShapeStruct20102 : TagStructure
                        {
                            public int FieldPointerSkip;
                            public short Size;
                            public short Count;
                            public int UserData;
                            public int Type;
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
        
        [TagStructure(Size = 0x14)]
        public class CollisionModelPathfindingSphereBlock : TagStructure
        {
            public short Node;
            public PathfindingSphereFlags Flags;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d Center;
            public float Radius;
            
            [Flags]
            public enum PathfindingSphereFlags : ushort
            {
                RemainsWhenOpen = 1 << 0,
                VehicleOnly = 1 << 1,
                WithSectors = 1 << 2
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CollisionModelNodeBlock : TagStructure
        {
            public StringId Name;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short ParentNode;
            public short NextSiblingNode;
            public short FirstChildNode;
        }
    }
}
