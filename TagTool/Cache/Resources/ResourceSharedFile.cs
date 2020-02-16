using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x108)]
    public class ResourceSharedFile : TagStructure
    {
        [TagField(Length = 256)]
        public string MapPath;

        public short Unknown;
        public ushort Flags;

        public uint BlockOffset;
    }
}