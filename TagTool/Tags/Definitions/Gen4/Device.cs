using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "device", Tag = "devi", Size = 0xB0)]
    public class Device : GameObject
    {
        public DeviceDefinitionFlags Flags;
        public float PowerTransitionTime; // seconds
        public float PowerAccelerationTime; // seconds
        public float PositionTransitionTime; // seconds
        public float PositionAccelerationTime; // seconds
        public float DepoweredPositionTransitionTime; // seconds
        public float DepoweredPositionAccelerationTime; // seconds
        public DeviceLightmapFlags LightmapFlags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Open;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Close;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Opened;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Closed;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Depowered;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Repowered;
        public float DelayTime; // seconds
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag DelayEffect;
        public float AutomaticActivationRadius; // world units
        public StringId MarkerName;
        // max distance between the unit and the marker
        public float MarkerRadius;
        // angle from marker forward the unit must be
        public Angle MarkerConeAngle;
        // angle from unit facing the marker must be
        public Angle MarkerFacingAngle;
        // above this value and below the max, object becomes targetable
        public float MinTargetablePositionThreshold;
        // below this value and above the min, object becomes targetable
        public float MaxTargetablePositionThreshold;
        [TagField(Length = 0x68, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        
        [Flags]
        public enum DeviceDefinitionFlags : uint
        {
            PositionLoops = 1 << 0,
            UseMultiplayerBoundary = 1 << 1,
            AllowInterpolation = 1 << 2,
            AllowAttachedPlayers = 1 << 3,
            ControlUsesParentInteractScripts = 1 << 4,
            RequiresLineOfSightForInteraction = 1 << 5,
            // This flag has no effect if the device is parented to a non unit
            OnlyActiveWhenParentIsHostile = 1 << 6,
            IsTargetable = 1 << 7
        }
        
        [Flags]
        public enum DeviceLightmapFlags : ushort
        {
            DonTUseInLightmap = 1 << 0,
            DonTUseInLightprobe = 1 << 1
        }
    }
}
