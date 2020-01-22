using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Pathfinding
{
    [TagStructure(Size = 0x94)]
    public class ResourcePathfinding : TagStructure
    {
        public TagBlock<Sector> Sectors;
        public TagBlock<Link> Links;
        public TagBlock<Reference> References;
        public TagBlock<Bsp2dNode> Bsp2dNodes;
        public TagBlock<Vertex> Vertices;
        public TagBlock<ObjectReference> ObjectReferences;
        public TagBlock<PathfindingHint> PathfindingHints;
        public TagBlock<InstancedGeometryReference> InstancedGeometryReferences;
        public int StructureChecksum;
        public TagBlock<GiantPathfindingBlock> GiantPathfinding;
        public TagBlock<Seam> Seams;
        public TagBlock<JumpSeam> JumpSeams;
        public TagBlock<Door> Doors;
    }

    [TagStructure(Size = 0xA0)]
    public class TagPathfinding : TagStructure
    {
        public TagBlock<Sector> Sectors;
        public TagBlock<Link> Links;
        public TagBlock<Reference> References;
        public TagBlock<Bsp2dNode> Bsp2dNodes;
        public TagBlock<Vertex> Vertices;
        public TagBlock<ObjectReference> ObjectReferences;
        public TagBlock<PathfindingHint> PathfindingHints;
        public TagBlock<InstancedGeometryReference> InstancedGeometryReferences;
        public int StructureChecksum;
        public TagBlock<int> UnusedBlock;
        public TagBlock<GiantPathfindingBlock> GiantPathfinding;
        public TagBlock<Seam> Seams;
        public TagBlock<JumpSeam> JumpSeams;
        public TagBlock<Door> Doors;
    }

    [TagStructure(Size = 0x8)]
    public class Sector : TagStructure
    {
        public FlagsValue PathfindingSectorFlags;
        public short HintIndex;
        public int FirstLink;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            SectorWalkable = 1 << 0,
            SectorBreakable = 1 << 1,
            SectorMobile = 1 << 2,
            SectorBspSource = 1 << 3,
            Floor = 1 << 4,
            Ceiling = 1 << 5,
            WallNorth = 1 << 6,
            WallSouth = 1 << 7,
            WallEast = 1 << 8,
            WallWest = 1 << 9,
            Crouchable = 1 << 10,
            Aligned = 1 << 11,
            SectorStep = 1 << 12,
            SectorInterior = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }
    }

    [TagStructure(Size = 0x10)]
    public class Link : TagStructure
    {
        public short Vertex1;
        public short Vertex2;
        public FlagsValue LinkFlags;
        public short HintIndex;
        public ushort ForwardLink;
        public ushort ReverseLink;
        public short LeftSector;
        public short RightSector;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            SectorLinkFromCollisionEdge = 1 << 0,
            SectorIntersectionLink = 1 << 1,
            SectorLinkBsp2dCreationError = 1 << 2,
            SectorLinkTopologyError = 1 << 3,
            SectorLinkChainError = 1 << 4,
            SectorLinkBothSectorsWalkable = 1 << 5,
            SectorLinkMagicHangingLink = 1 << 6,
            SectorLinkThreshold = 1 << 7,
            SectorLinkCrouchable = 1 << 8,
            SectorLinkWallBase = 1 << 9,
            SectorLinkLedge = 1 << 10,
            SectorLinkLeanable = 1 << 11,
            SectorLinkStartCorner = 1 << 12,
            SectorLinkEndCorner = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }
    }

    [TagStructure(Size = 0x4)]
    public class Reference : TagStructure
    {
        public int NodeOrSectorIndex;
    }

    [TagStructure(Size = 0x14)]
    public class Bsp2dNode : TagStructure
    {
        public RealPlane2d Plane;
        public int LeftChild;
        public int RightChild;
    }

    [TagStructure(Size = 0xC)]
    public class Vertex : TagStructure
    {
        public RealPoint3d Position;
    }

    [TagStructure(Size = 0x18)]
    public class ObjectReference : TagStructure
    {
        public ushort Flags;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused = new byte[2];

        public TagBlock<BspReference> Bsps;

        public int ObjectUniqueID;
        public short OriginBspIndex;
        public ScenarioObjectType ObjectType;
        public Scenario.ScenarioInstance.SourceValue Source;

        [TagStructure(Size = 0x18)]
        public class BspReference : TagStructure
        {
            public int BspIndex;
            public short NodeIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public TagBlock<Bsp2dRef> Bsp2dRefs;

            public int VertexOffset;

            [TagStructure(Size = 0x4)]
            public class Bsp2dRef : TagStructure
            {
                public DatumIndex Index;
            }
        }
    }

    [TagStructure(Size = 0x14)]
    public class PathfindingHint : TagStructure
    {
        public HintTypeValue HintType;
        public short NextHintIndex;

        [TagField(Length = 4)]
        public int[] Data;

        public enum HintTypeValue : short
        {
            IntersectionLink,
            JumpLink,
            ClimbLink,
            VaultLink,
            MountLink,
            HoistLink,
            WallJumpLink,
            BreakableFloor,
            Unknown8,
            Unknown9,
            UnknownA,
            // TODO: Add more?
        }

        [Flags]
        public enum FlagsValue : byte
        {
            None = 0,
            Bidirectional = 1 << 0,
            Closed = 1 << 1,
            Unknown2 = 1 << 2,
            Unknown3 = 1 << 3,
            Unknown4 = 1 << 4,
            Unknown5 = 1 << 5,
            Unknown6 = 1 << 6,
            Unknown7 = 1 << 7
        }

        [Flags]
        public enum ControlFlagsValue : short
        {
            None = 0,
            MagicLift = 1 << 0,
            VehicleOnly = 1 << 1,
            Railing = 1 << 2,
            Vault = 1 << 3,
            Down = 1 << 4,
            Phase = 1 << 5,
            StopAutodown = 1 << 6,
            ForceWalk = 1 << 7
        }
    }

    [TagStructure(Size = 0x4)]
    public class InstancedGeometryReference : TagStructure
    {
        public short PathfindingObjectIndex;
        public short Unknown;
    }

    [TagStructure(Size = 0x4)]
    public class GiantPathfindingBlock : TagStructure
    {
        public int Bsp2dIndex;
    }

    [TagStructure(Size = 0xC)]
    public class Seam : TagStructure
    {
        public TagBlock<LinkIndexBlock> LinkIndices;

        [TagStructure(Size = 0x4)]
        public class LinkIndexBlock : TagStructure
        {
            public int LinkIndex;
        }
    }

    [TagStructure(Size = 0x14)]
    public class JumpSeam : TagStructure
    {
        public short UserJumpIndex;
        public byte DestOnly;

        [TagField(Flags = Padding, Length = 1)]
        public byte[] Unused = new byte[1];

        public float Length;

        public TagBlock<JumpIndexBlock> JumpIndices;

        [TagStructure(Size = 0x4)]
        public class JumpIndexBlock : TagStructure
        {
            public short JumpIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];
        }
    }

    [TagStructure(Size = 0x4)]
    public class Door : TagStructure
    {
        public short ScenarioObjectIndex;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused = new byte[2];
    }
}
