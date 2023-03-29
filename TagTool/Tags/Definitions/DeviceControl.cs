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
            Generator,
        }

        public enum TriggersWhenValue : short
        {
            TouchedByPlayer,
            Destroyed
        }
    }
}
