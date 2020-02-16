using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x8)]
    public class PredictionAtom : TagStructure
    {
        public ushort Identifier;
        public short QuantumCount;
        public int FirstQuantumIndex;
    }
}
