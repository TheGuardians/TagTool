using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_control", Tag = "ctrl", Size = 0x3C)]
    public class DeviceControl : Device
    {
        public TypeValue Type;
        public TriggersWhenValue TriggersWhen;
        public float CallValue;
        public StringId ActionString;

        [TagField(ValidTags = new[] { "snd!", "effe" })]
        public CachedTag On;
        [TagField(ValidTags = new[] { "snd!", "effe" })]
        public CachedTag Off;
        [TagField(ValidTags = new[] { "snd!", "effe" })]
        public CachedTag Deny;

        public enum TypeValue : short
        {
            Toggle,
            On,
            Off,
            Call,
            // touching this device plays the 'on' effect set below and refills the unit's health.
            // It also deletes itself if it runs out of charges (set in sapien)
            HealthStation,
        }

        public enum TriggersWhenValue : short
        {
            TouchedByPlayer,
            Destroyed
        }
    }
}
