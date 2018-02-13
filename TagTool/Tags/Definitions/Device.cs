using TagTool.Cache;
using TagTool.Serialization;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Tag = "devi", Size = 0x98, MinVersion = CacheVersion.Halo3Retail)]
    public class Device : GameObject
    {
        public DeviceFlagBits DeviceFlags;
        public float PowerTransitionTime;
        public float PowerAccelerationTime;
        public float PositionTransitionTime;
        public float PositionAccelerationTime;
        public float DepoweredPositionTransitionTime;
        public float DepoweredPositionAccelerationTime;
        public LightmapFlagBits LightmapFlags;
        public CachedTagInstance OpenUp;
        public CachedTagInstance CloseDown;
        public CachedTagInstance Opened;
        public CachedTagInstance Closed;
        public CachedTagInstance Depowered;
        public CachedTagInstance Repowered;
        public float DelayTime;
        public CachedTagInstance DelayEffect;
        public float AutomaticActivationRadius;

        [Flags]
        public enum DeviceFlagBits : int
        {
            None,
            PositionLoops = 1 << 0,
            UseMultiplayerBoundary = 1 << 1,
            AllowInterpolation = 1 << 2,
            AllowAttachedPlayers = 1 << 3,
            ControlUsesParentInteractScripts = 1 << 4,
            RequiresLineOfSightForInteraction = 1 << 5,
            OnlyActiveWhenParentIsHostile = 1 << 6,
            IsTargetable = 1 << 7,
            IgnoreImportantWorkJustForVS = 1 << 8,
            HugeDevice = 1 << 9
        }

        [Flags]
        public enum LightmapFlagBits : int
        {
            None,
            DoNotUseInLightmap = 1 << 0,
            DoNotUseInLightprobe = 1 << 1
        }
    }
}
