using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "device_arg_device", Tag = "argd", Size = 0x4, MinVersion = CacheVersion.Halo3ODST)]
    public class DeviceArgDevice : Device
    {
        public StringId ActionString;
    }
}
