using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "pathfinding", Tag = "pfnd", Size = 0x4C)]
    public class Pathfinding : TagStructure
    {
        public List<PathfindingDataBlock> BspPathfindingData;
        public List<MobileNavMeshBlock> MobilenavMeshes;
        public List<NavVolumeBlock> Navvolumes;
        public List<NavClimbBlock> Navclimbs;
        public List<UserEdgeBlock> UserEdges;
        public List<UserHintBlock> Hints;
        public byte AlreadyConverted;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x50)]
        public class PathfindingDataBlock : TagStructure
        {
            public int RuntimenavMesh;
            public int RuntimenavGraph;
            public int RuntimenavMediator;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] NavgraphData;
            public byte[] NavmediatorData;
            public List<FaceUserDataBlock> FaceuserData;
            public int StructureChecksum;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0xC)]
            public class FaceUserDataBlock : TagStructure
            {
                public short MFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CurrentminPathDistance;
                public float CurrentminTargetApproachDistance;
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class MobileNavMeshBlock : TagStructure
        {
            public int RuntimenavMesh;
            public int RuntimenavGraph;
            public int RuntimenavMediator;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] NavgraphData;
            public byte[] NavmediatorData;
            public List<FaceUserDataBlock> FaceuserData;
            public ScenarioObjectIdStruct ObjectId;
            public MobileNavMeshFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum MobileNavMeshFlags : byte
            {
                AlwaysLoaded = 1 << 0
            }
            
            [TagStructure(Size = 0xC)]
            public class FaceUserDataBlock : TagStructure
            {
                public short MFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CurrentminPathDistance;
                public float CurrentminTargetApproachDistance;
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioObjectIdStruct : TagStructure
            {
                public int UniqueId;
                public short OriginBspIndex;
                public ObjectTypeEnum Type;
                public ObjectSourceEnum Source;
                
                public enum ObjectTypeEnum : sbyte
                {
                    Biped,
                    Vehicle,
                    Weapon,
                    Equipment,
                    Terminal,
                    Projectile,
                    Scenery,
                    Machine,
                    Control,
                    Dispenser,
                    SoundScenery,
                    Crate,
                    Creature,
                    Giant,
                    EffectScenery,
                    Spawner
                }
                
                public enum ObjectSourceEnum : sbyte
                {
                    Structure,
                    Editor,
                    Dynamic,
                    Legacy,
                    Sky,
                    Parent
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class NavVolumeBlock : TagStructure
        {
            public short Zoneindex;
            public short Areaindex;
            public int RuntimenavVolume;
            public int RuntimenavMediator;
            public byte[] NavmediatorData;
        }
        
        [TagStructure(Size = 0x50)]
        public class NavClimbBlock : TagStructure
        {
            public int RuntimenavMesh;
            public int RuntimenavGraph;
            public int RuntimenavMediator;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] NavgraphData;
            public byte[] NavmediatorData;
            public List<FaceUserDataBlock> FaceuserData;
            public short Zoneindex;
            public short Areaindex;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0xC)]
            public class FaceUserDataBlock : TagStructure
            {
                public short MFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CurrentminPathDistance;
                public float CurrentminTargetApproachDistance;
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class UserEdgeBlock : TagStructure
        {
            public RealVector3d MX;
            public float HavokWMX;
            public RealVector3d MY;
            public float HavokWMY;
            public RealVector3d MZ;
            public float HavokWMZ;
            public int MMeshuidA;
            public int MMeshuidB;
            public int MFacea;
            public int MFaceb;
            public int MUserdataA;
            public int MUserdataB;
            public short MCostatoB;
            public short MCostbtoA;
            public sbyte MDirection;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x90)]
        public class UserHintBlock : TagStructure
        {
            public List<UserHintLineSegmentBlock> LineSegmentGeometry;
            public List<UserHintParallelogramBlock> ParallelogramGeometry;
            public List<UserHintJumpBlock> JumpHints;
            public List<UserHintClimbBlock> ClimbHints;
            public List<UserHintWellBlock> WellHints;
            public List<UserHintFlightBlock> FlightHints;
            public List<UserHintVolumeAvoidanceStruct> VolumeAvoidanceHints;
            public List<UserHintSplineBlock> SplineHints;
            public List<UserHintCookieCutterBlockStruct> CookieCutters;
            public List<UserHintNavmeshAreaBlockStruct> NavmeshAreas;
            public List<UserHintGiantBlock> GiantHints;
            public List<UserHintFloodBlock> FloodHints;
            
            [TagStructure(Size = 0x2C)]
            public class UserHintLineSegmentBlock : TagStructure
            {
                public UserHintGeometryFlags Flags;
                public RealPoint3d Point0;
                public int PackedkeyOffaceref0;
                public int NavmeshUidoffaceref0;
                public RealPoint3d Point1;
                public int PackedkeyOffaceref1;
                public int NavmeshUidoffaceref1;
                
                [Flags]
                public enum UserHintGeometryFlags : uint
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x58)]
            public class UserHintParallelogramBlock : TagStructure
            {
                public UserHintGeometryFlags Flags;
                public RealPoint3d Point0;
                public int PackedkeyOffaceref0;
                public int NavmeshUidoffaceref0;
                public RealPoint3d Point1;
                public int PackedkeyOffaceref1;
                public int NavmeshUidoffaceref1;
                public RealPoint3d Point2;
                public int PackedkeyOffaceref2;
                public int NavmeshUidoffaceref2;
                public RealPoint3d Point3;
                public int PackedkeyOffaceref3;
                public int NavmeshUidoffaceref3;
                public ParallelogramPointsInvalidFlags InvalidPoints;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum UserHintGeometryFlags : uint
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
                
                [Flags]
                public enum ParallelogramPointsInvalidFlags : ushort
                {
                    _1 = 1 << 0,
                    _2 = 1 << 1,
                    _3 = 1 << 2,
                    _4 = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class UserHintJumpBlock : TagStructure
            {
                public HintTypeEnum HintType;
                public short SquadGroupFilter;
                public List<HintVertexBlock> HintVertices;
                public int HintData0;
                public short HintData1;
                public byte HintData2;
                public byte Pad1;
                public UserHintGeometryFlags Flags;
                public short GeometryIndex;
                public GlobalAiJumpHeightEnum ForceJumpHeight;
                public JumpFlags ControlFlags;
                
                public enum HintTypeEnum : short
                {
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    TakeoffLink,
                    JumpMandatoryApproach
                }
                
                [Flags]
                public enum UserHintGeometryFlags : ushort
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
                
                public enum GlobalAiJumpHeightEnum : short
                {
                    None,
                    Down,
                    Step,
                    Crouch,
                    Stand,
                    Storey,
                    Tower,
                    Infinite
                }
                
                [Flags]
                public enum JumpFlags : ushort
                {
                    MagicLift = 1 << 0,
                    VehicleOnly = 1 << 1,
                    Railing = 1 << 2,
                    Vault = 1 << 3,
                    Down = 1 << 4,
                    Phase = 1 << 5,
                    StopAutodown = 1 << 6
                }
                
                [TagStructure(Size = 0xC)]
                public class HintVertexBlock : TagStructure
                {
                    public RealPoint3d Point;
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class UserHintClimbBlock : TagStructure
            {
                public HintTypeEnum HintType;
                public short SquadGroupFilter;
                public List<HintVertexBlock> HintVertices;
                public int HintData0;
                public short HintData1;
                public byte HintData2;
                public byte Pad1;
                public UserHintGeometryFlags Flags;
                public short GeometryIndex;
                public ForcedHoistHeightEnum ForceHoistHeight;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum HintTypeEnum : short
                {
                    JumpLink,
                    ClimbLink,
                    VaultLink,
                    MountLink,
                    HoistLink,
                    WallJumpLink,
                    TakeoffLink,
                    JumpMandatoryApproach
                }
                
                [Flags]
                public enum UserHintGeometryFlags : ushort
                {
                    Bidirectional = 1 << 0,
                    Closed = 1 << 1
                }
                
                public enum ForcedHoistHeightEnum : short
                {
                    None,
                    Step,
                    Crouch,
                    Stand
                }
                
                [TagStructure(Size = 0xC)]
                public class HintVertexBlock : TagStructure
                {
                    public RealPoint3d Point;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class UserHintWellBlock : TagStructure
            {
                public UserHintWellGeometryFlags Flags;
                public List<UserHintWellPointBlock> Points;
                
                [Flags]
                public enum UserHintWellGeometryFlags : uint
                {
                    Bidirectional = 1 << 0
                }
                
                [TagStructure(Size = 0x20)]
                public class UserHintWellPointBlock : TagStructure
                {
                    public UserHintWellPointTypeEnum Type;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                    
                    public enum UserHintWellPointTypeEnum : short
                    {
                        Jump,
                        Invalid,
                        Hoist
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UserHintFlightBlock : TagStructure
            {
                public List<UserHintFlightPointBlock> Points;
                
                [TagStructure(Size = 0xC)]
                public class UserHintFlightPointBlock : TagStructure
                {
                    public RealVector3d Point;
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class UserHintVolumeAvoidanceStruct : TagStructure
            {
                public UserHintAvoidanceVolumeEnum Type;
                public RealPoint3d Origin;
                public float Radius;
                // for pills
                public RealVector3d FacingVector;
                // for pills
                public float Height;
                public short Bsp;
                public short SplineCount;
                public short ZoneIndex;
                public short AreaIndex;
                
                public enum UserHintAvoidanceVolumeEnum : int
                {
                    Sphere,
                    Pill
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class UserHintSplineBlock : TagStructure
            {
                public StringId Name;
                public float Radius; // wus
                public float TimeBetweenPoints; // sec
                public List<UserHintSplineControlPointBlockStruct> ControlPoints;
                public short Bsp;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<UserHintSplineIntersectPointBlockStruct> VolumeIntersectPoints;
                
                [TagStructure(Size = 0x20)]
                public class UserHintSplineControlPointBlockStruct : TagStructure
                {
                    public UserHintSplineSegmentFlags Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public RealVector3d Tangent;
                    public float SegmentArcLength;
                    
                    [Flags]
                    public enum UserHintSplineSegmentFlags : ushort
                    {
                        NoAttachDetach = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSplineIntersectPointBlockStruct : TagStructure
                {
                    public short VolumeIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealPoint3d Point;
                    public RealVector3d Tangent;
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class UserHintCookieCutterBlockStruct : TagStructure
            {
                public int HkaivolumeVtable;
                public short Size;
                public short Count;
                public List<UserHintSectorPointBlock> Points;
                public List<HintObjectIdBlock> PointsobjectIds;
                public float ZHeight;
                public float ZSink;
                public CookieCutterTypeEnum Type;
                public short Pad;
                public int RuntimeobjectTransformOverrideIndex;
                public sbyte Invalid;
                public sbyte Pad2;
                public sbyte Pad3;
                public sbyte Pad4;
                
                public enum CookieCutterTypeEnum : short
                {
                    CarveOut,
                    Preserve,
                    CarveAirVolume
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSectorPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                }
                
                [TagStructure(Size = 0x8)]
                public class HintObjectIdBlock : TagStructure
                {
                    public ScenarioObjectIdStruct ObjectId;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScenarioObjectIdStruct : TagStructure
                    {
                        public int UniqueId;
                        public short OriginBspIndex;
                        public ObjectTypeEnum Type;
                        public ObjectSourceEnum Source;
                        
                        public enum ObjectTypeEnum : sbyte
                        {
                            Biped,
                            Vehicle,
                            Weapon,
                            Equipment,
                            Terminal,
                            Projectile,
                            Scenery,
                            Machine,
                            Control,
                            Dispenser,
                            SoundScenery,
                            Crate,
                            Creature,
                            Giant,
                            EffectScenery,
                            Spawner
                        }
                        
                        public enum ObjectSourceEnum : sbyte
                        {
                            Structure,
                            Editor,
                            Dynamic,
                            Legacy,
                            Sky,
                            Parent
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x48)]
            public class UserHintNavmeshAreaBlockStruct : TagStructure
            {
                public int HkaivolumeVtable;
                public short Size;
                public short Count;
                public List<UserHintSectorPointBlock> Points;
                public float ZHeight;
                public float ZSink;
                public float StepHeight;
                public NavmeshAreaTypeEnum Type;
                public float Isvalid;
                public float MaxConvexBorderSimplifyArea;
                public float MaxBorderDistanceError;
                public float MaxConcaveBorderSimplifyArea;
                public float MaxWalkableSlope;
                public float CosineAngleMergeControl;
                public float HoleReplacementArea;
                public int PartitionSize;
                public float LoopShrinkFactor;
                
                public enum NavmeshAreaTypeEnum : int
                {
                    NavmeshLowRes,
                    Navmesh2,
                    Navmesh3,
                    Navmesh4,
                    Navmesh5,
                    Navmesh6,
                    Navmesh7,
                    Navmesh8,
                    NavmeshHighRes,
                    Custom
                }
                
                [TagStructure(Size = 0x1C)]
                public class UserHintSectorPointBlock : TagStructure
                {
                    public RealPoint3d Point;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public RealEulerAngles2d Normal;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class UserHintGiantBlock : TagStructure
            {
                public List<UserHintGiantSectorBlock> GiantSectorHints;
                public List<UserHintGiantRailBlock> GiantRailHints;
                
                [TagStructure(Size = 0xC)]
                public class UserHintGiantSectorBlock : TagStructure
                {
                    public List<UserHintSectorPointBlock> Points;
                    
                    [TagStructure(Size = 0x1C)]
                    public class UserHintSectorPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public int PackedkeyOffaceref;
                        public int NavmeshUidoffaceref;
                        public RealEulerAngles2d Normal;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserHintGiantRailBlock : TagStructure
                {
                    public short GeometryIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UserHintFloodBlock : TagStructure
            {
                public List<UserHintFloodSectorBlock> FloodSectorHints;
                
                [TagStructure(Size = 0xC)]
                public class UserHintFloodSectorBlock : TagStructure
                {
                    public List<UserHintSectorPointBlock> Points;
                    
                    [TagStructure(Size = 0x1C)]
                    public class UserHintSectorPointBlock : TagStructure
                    {
                        public RealPoint3d Point;
                        public int PackedkeyOffaceref;
                        public int NavmeshUidoffaceref;
                        public RealEulerAngles2d Normal;
                    }
                }
            }
        }
    }
}
