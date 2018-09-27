using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_control", Tag = "ctrl", Size = 0x44)]
    public class DeviceControl : Device
    {
        public TypeValue Type;
        public TriggersWhenValue TriggersWhen;
        public float CallValue;
        public StringId ActionString;
        public CachedTagInstance On;
        public CachedTagInstance Off;
        public CachedTagInstance Deny;
        public uint Unknown8;
        public uint Unknown9;

        public enum TypeValue : short
        {
            Toggle,
            On,
            Off,
            Call,
            Generator,
        }

        public enum TriggersWhenValue : short
        {
            TouchedByPlayer,
            Destroyed
        }
    }
}
