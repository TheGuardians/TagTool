using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x20)]
    public class DeviceMachine : Device
    {
        public MachineTypes Type;
        public MachineFlagsEnum MachineFlags;
        public float DoorOpenTime; // seconds
        // maps position [0,1] to occlusion
        public Bounds<float> DoorOcclusionBounds;
        public MachineCollisionResponses CollisionResponse;
        public short ElevatorNode;
        public MachinePathfindingPolicyEnum PathfindingPolicy;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // shield (or any damage section) to control
        public StringId ShieldName;
        // shield is on when this function is greater then 0.5f, off otherwise.
        public StringId ShieldFunction;
        
        public enum MachineTypes : short
        {
            Door,
            Platform,
            Gear
        }
        
        [Flags]
        public enum MachineFlagsEnum : ushort
        {
            PathfindingObstacle = 1 << 0,
            ButNotWhenOpen = 1 << 1,
            // lighting based on what's around, rather than what's below
            Elevator = 1 << 2,
            // machines of type "door" and all other machines with this flag checked can block a door portal
            IsPortalBlocker = 1 << 3,
            IsNotPathfindingMobile = 1 << 4,
            UsesDefaultOcclusionBounds = 1 << 5,
            // play animation, reset, play again. No smooth looping and interpolation
            GearsRepeatMotionInsteadOfLoop = 1 << 6
        }
        
        public enum MachineCollisionResponses : short
        {
            PauseUntilCrushed,
            ReverseDirections
        }
        
        public enum MachinePathfindingPolicyEnum : short
        {
            Discs,
            Sectors,
            CutOut,
            None
        }
    }
}
