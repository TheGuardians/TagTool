using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x60, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReach)]
    public class LargeCollisionBspBlock : TagStructure
    {
        public TagBlock<LargeBsp3dNode> Bsp3dNodes;

        // probably needs a new block for large reach
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<Bsp3dSupernode> Bsp3dSupernodes;

        public TagBlock<Plane> Planes;
        public TagBlock<Leaf> Leaves;
        public TagBlock<LargeBsp2dReference> Bsp2dReferences;
        public TagBlock<LargeBsp2dNode> Bsp2dNodes;
        public TagBlock<LargeSurface> Surfaces;
        public TagBlock<LargeEdge> Edges;
        public TagBlock<LargeVertex> Vertices;
    }

    [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
    public class CollisionBspBlockOld : TagStructure
    {
        public TagBlock<LargeBsp3dNode> Bsp3dNodes;
        public TagBlock<Plane> Planes;
        public TagBlock<Leaf> Leaves;
        public TagBlock<LargeBsp2dReference> Bsp2dReferences;
        public TagBlock<LargeBsp2dNode> Bsp2dNodes;
        public TagBlock<LargeSurface> Surfaces;
        public TagBlock<LargeEdge> Edges;
        public TagBlock<LargeVertex> Vertices;
    }

    [TagStructure(Size = 0xC)]
    public class LargeBsp3dNode : TagStructure
    {
        public int Plane;
        public int BackChild;
        public int FrontChild;
    }

    [TagStructure(Size = 0x8)]
    public class LargeBsp2dReference : TagStructure
    {
        public int PlaneIndex;
        public int Bsp2dNodeIndex;
    }

    [TagStructure(Size = 0x14)]
    public class LargeBsp2dNode : TagStructure
    {
        public RealPlane2d Plane;
        public int LeftChild;
        public int RightChild;
    }

    [TagStructure(Size = 0x10)]
    public class LargeSurface : TagStructure
    {
        public int Plane;
        public int FirstEdge;
        public short Material;
        public short BreakableSurfaceSet;
        public short BreakableSurface;
        public SurfaceFlags Flags;
        public sbyte BestPlaneCalculationVertex;
    }

    [TagStructure(Size = 0x18)]
    public class LargeEdge : TagStructure
    {
        public int StartVertex;
        public int EndVertex;
        public int ForwardEdge;
        public int ReverseEdge;
        public int LeftSurface;
        public int RightSurface;
    }

    [TagStructure(Size = 0x14)]
    public class LargeVertex : TagStructure
    {
        public RealPoint3d Point;
        public int FirstEdge;
        public int Sink;
    }
}
