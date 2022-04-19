using System;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x24, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    public class DeviceMachine : Device
    {
        public TypeValue Type;
        public MachineFlags Flags;
        public float DoorOpenTime;
        public Bounds<float> OcclusionBounds;
        public CollisionResponseValue CollisionResponse;
        public short ElevatorNode;
        public PathfindingPolicyValue PathfindingPolicy;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding4;
 
        public enum TypeValue : short
        {
            Door,
            Platform,
            Gear
        }

        [Flags]
        public enum MachineFlags : ushort
        {
            PathfindingObstacle = 1 << 0,
            ButNotWhenOpen = 1 << 1,
            Elevator = 1 << 2, // lighting based on what's around, rather than what's below
            IsPortalBlocker = 1 << 3, // machines of type "door" and all other machines with this flag checked can block a door portal
            IsNotPathfindingMobile = 1 << 4,
            UsesDefaultOcclusionBounds = 1 << 5
        }

        public enum CollisionResponseValue : short
        {
            PauseUntilCrushed,
            ReverseDirections,
            Discs
        }

        public enum PathfindingPolicyValue : short
        {
            Discs,
            Sectors,
            CutOut,
            None
        }
    }
}
