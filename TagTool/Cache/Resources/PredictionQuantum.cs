using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x4)]
    public class PredictionQuantum : TagStructure
    {
        public DatumHandle ResourceHandle;
    }
}