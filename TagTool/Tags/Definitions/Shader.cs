using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader", Tag = "rmsh", Size = 0x4, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "shader", Tag = "rmsh", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class Shader : RenderMethod
    {
        public StringId Material;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;
    }
}