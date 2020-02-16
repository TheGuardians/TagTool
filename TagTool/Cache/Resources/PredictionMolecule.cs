using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x8)]
    public class PredictionMolecule : TagStructure
    {
        public short MoleculeAtomCount;
        public short FirstMoleculeAtomIndex;
        public short QuantumCount;
        public short FirstQuantumIndex;
    }
}