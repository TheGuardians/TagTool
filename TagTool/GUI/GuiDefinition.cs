using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.GUI
{
    [TagStructure(Size = 0x28)]
    public class GuiDefinition : TagStructure
    {
        public StringId Name;
        public short Unknown;
        public short Layer;
        public short WidescreenYOffset;
        public short WidescreenXOffset;
        public short WidescreenYUnknown;
        public short WidescreenXUnknown;
        public short StandardYOffset;
        public short StandardXOffset;
        public short StandardYUnknown;
        public short StandardXUnknown;
        public CachedTag Animation;
    }
}