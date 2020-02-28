using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x4)]
    public class PredictionMoleculeAtom : TagStructure
    {
        public DatumHandle AtomHandle;
    }
}
