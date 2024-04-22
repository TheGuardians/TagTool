using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.Definitions.Common
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
            TopLeftCorner,
            TopRightCorner,
            BottomRightCorner,
            BottomLeftCorner
        }
    }

	public enum WidgetFontValue : short   // engine order commented
	{
		Terminal,               // fixedsys,  9pt
		BodyText,               // conduit,  16pt
		Title,                  // conduit,  32pt
		SuperLargeFont,         // conduit,  32pt
		LargeBodyText,          // conduit,  23pt
		SplitscreenHudMessage,  // fixedsys,  9pt
		FullscreenHudMessage,   // conduit,  18pt
		EnglishBodyText,        // larbie,   10pt
		HudNumberText,          // conduit,  18pt
		SubtitleFont,           // conduit,  16pt
		MainMenuFont            // pragmata, 14pt
	}

	public enum WidgetFontValue_ODST : short
	{
		Terminal,               // Fixedsys,  9pt
		BodyText,               // Conduit,  14pt
		Title,                  // Conduit,  32pt
		SuperLargeFont,         // Agency,   32pt
		LargeBodyText,          // Conduit,  23pt
		SplitscreenHudMessage,  // Agency,   16pt
		FullscreenHudMessage,   // Conduit,  18pt
		EnglishBodyText,        // Agency,   18pt
		HudNumberText,          // Conduit,  18pt
		SubtitleFont,           // Conduit,  16pt
		MainMenuFont            // Agency,   23pt
	}

    public enum WidgetFontValue_ODSTMCC : short
    {
        FullscreenHudMessage,
        SplitScreenHudMessage,
        Terminal,
        BodyText,
        Title,
        SuperLarge,
        LargeBodyText,
        EnglishBodyText,
        HudNumberText,
        SubtitleFont,
        MainMenuFont  
    }

    public enum WidgetFontValue_H3Original : short
	{
		Terminal,               // FixedSys 9 (terminal)
		BodyText,               // Conduit 16 (button_key)
		Title,                  // Conduit 32 (header_item0)
		SuperLargeFont,         // Conduit 32 
		LargeBodyText,          // Conduit 23
		SplitscreenHudMessage,  // FixedSys 9
		FullscreenHudMessage,   // Conduit 18
		EnglishBodyText,        // Larabie 10
		HudNumberText,          // Conduit 18
		SubtitleFont,           // Conduit 16
		MainMenuFont            // Pragmata 14
	}

	public enum WidgetFontValue_H3MCC : short
	{
		Terminal,               // FixedSys 9 (terminal)
		BodyText,               // Conduit 16 (button_key)
		Title,                  // Conduit 32 (header_item0)
		SuperLargeFont,         // Conduit 32 
		LargeBodyText,          // Conduit 23
		SplitscreenHudMessage,  // FixedSys 9
		FullscreenHudMessage,   // Conduit 18
		EnglishBodyText,        // Larabie 10
		HudNumberText,          // Conduit 18
		SubtitleFont,           // Conduit 16
		MainMenuFont            // Pragmata 14
	}

    public enum WidgetFontValue_Reach : short
    {
        Terminal,
        BodyText,
        Title,
        SuperLargeFont,
        LargeBodyText,
        SplitScreenHudMessage,
        FullScreenHudMessage,
        EnglishBodyText,
        HudNumber,
        SubtitleFont,
        MainMenuFont
    }
}
