using TagTool.Common;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x8)]
    public class ScenarioObjectIdentifier : TagStructure
    {
        public DatumHandle UniqueHandle;
        public short OriginBspIndex;
        public ScenarioObjectType ObjectType;
        public SourceValue Source;

        public enum SourceValue : sbyte
        {
            Structure,
            Editor,
            Dynamic,
            Legacy,
        }
    }
}
