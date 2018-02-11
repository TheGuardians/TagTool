using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shader_screen", Tag = "rmss", Size = 0x8, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "shader_screen", Tag = "rmss", Size = 0x14, MinVersion = CacheVersion.HaloOnline106708)]
    public class ShaderScreen : RenderMethod
    {
        public uint Unknown8;
        public byte Unknown9;
        public byte Unknown10;
        public byte Unknown11;
        public byte Unknown12;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;
    }
}
