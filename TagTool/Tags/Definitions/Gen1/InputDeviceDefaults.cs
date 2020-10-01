using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "input_device_defaults", Tag = "devc", Size = 0x2C)]
    public class InputDeviceDefaults : TagStructure
    {
        public DeviceTypeValue DeviceType;
        public FlagsValue Flags;
        public byte[] DeviceId;
        public byte[] Profile;
        
        public enum DeviceTypeValue : short
        {
            MouseAndKeyboard,
            JoysticksGamepadsEtc,
            FullProfileDefinition
        }
        
        public enum FlagsValue : ushort
        {
            Unused
        }
    }
}

