using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x18)]
    public class PredictionMoleculeKey : TagStructure
    {
        public CachedTag Owner;
        public int FirstValue;
        public int SecondValue;
    }
}