using TagTool.Tags;

namespace TagTool.Cache.Codecs
{
    [TagStructure(Size = 0x10)]
    public class CodecDefinition : TagStructure
    {
        [TagField(Length = 0x10)]
        public byte[] Guid;
    }
}