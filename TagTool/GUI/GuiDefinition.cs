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
        public short WidescreenYMin;
        public short WidescreenXMin;
        public short WidescreenYMax;
        public short WidescreenXMax;
        public short StandardYMin;
        public short StandardXMin;
        public short StandardYMax;
        public short StandardXMax;
        public CachedTag Animation;
    }

    public enum FontValue : short
    {
        Fixedsys9,
        Conduit16,
        Conduit32,
        Conduit23,
        Conduit18,
        larabie_10,
        pragmata_14,
    }
}