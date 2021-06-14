using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0xC)]
    public class CollisionModelResource : TagStructure
    {
        public List<CollisionModelBspStruct> Bsps;
        
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
    }
}
