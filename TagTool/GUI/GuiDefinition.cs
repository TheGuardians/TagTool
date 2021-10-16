using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.GUI
{
    [TagStructure(Size = 0x28)]
    public class GuiDefinition : TagStructure
    {
        public StringId Name;
        public WidgetPositioning ScaledPositioning; // scaledAnchoring
        public short RenderDepthBias;
        public Rectangle2d Bounds720p; // bounds16x9 1152x640
        public Rectangle2d Bounds480i; // bounds4x3 640x480
        public CachedTag AnimationCollection;

        public enum WidgetPositioning : short
        {
            UNUSED,
            Centered,
            TopEdge,
            BottomEdge,
            LeftEdge,
            RightEdge,
            TopleftCorner,
            ToprightCorner,
            BottomrightCorner,
            BottomleftCorner
        }
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