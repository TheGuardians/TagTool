using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    /// <summary>
    /// bit-vector of length 1024, convert as int/uint
    /// </summary>
    [TagStructure(Size = 0x20)]
    public class BreakableSurfaceBits : TagStructure
    {
        [TagField(Length = 8)]
        public SupportedBitfieldStruct[] SupportedBitfield;

        [TagStructure(Size = 0x4)]
        public class SupportedBitfieldStruct : TagStructure
        {
            public int BitvectorData;
        }
    }
}
