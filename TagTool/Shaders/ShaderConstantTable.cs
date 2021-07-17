using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x10, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x18, Platform = CachePlatform.MCC)]
    public class ShaderConstantTable : TagStructure
    {
        public List<ShaderParameter> Constants;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown1;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown2;
        public ShaderType ShaderType;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding0;
    }
}
