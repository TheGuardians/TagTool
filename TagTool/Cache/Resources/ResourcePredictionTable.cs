using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x3C)]
    public class ResourcePredictionTable : TagStructure
    {
        public List<PredictionQuantum> PredictionQuanta;
        public List<PredictionAtom> PredictionAtoms;
        public List<PredictionMoleculeAtom> PredictionMoleculeAtoms;
        public List<PredictionMolecule> PredictionMolecules;
        public List<PredictionMoleculeKey> PredictionMoleculeKeys;
    }
}
