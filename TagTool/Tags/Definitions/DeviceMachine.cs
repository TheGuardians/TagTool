using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x24, MinVersion = CacheVersion.HaloOnlineED)]
    public class DeviceMachine : Device
    {
        public TypeValue Type;
        public ushort Flags3;
        public float DoorOpenTime;
        public Bounds<float> OcclusionBounds;
        public CollisionResponseValue CollisionResponse;
        public short ElevatorNode;
        public PathfindingPolicyValue PathfindingPolicy;
        public short Unknown6;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unknown7;
 
        public enum TypeValue : short
        {
            Door,
            Platform,
            Gear
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
