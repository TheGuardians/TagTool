using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Common
{
    [TagStructure(Size = 0x8)]
    public class ObjectIdentifier : TagStructure
    {
        public DatumHandle UniqueHandle;
        public short OriginBspIndex;
        public ScenarioObjectType ObjectType;
        public SourceValue Source; // sbyte
        public BspPolicyValue BspPolicy; // sbyte

        public enum SourceValue : sbyte
        {
            Structure,
            Editor,
            Dynamic,
            Legacy,
        }

        public enum BspPolicyValue : sbyte
        {
            Default,
            AlwaysPlaced,
            ManualBspIndex,
        }
    }
}
