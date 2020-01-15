using TagTool.Tags;

namespace TagTool.BspCollisionGeometry
{
    /// <summary>
    /// bit-vector of length 1024, convert as int/uint
    /// </summary>
    [TagStructure(Size = 0x20)]
    public class BreakableSurfaceBits : TagStructure
    {
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
    }
}
