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
            BottomReftCorner
        }
    }

	public enum WidgetFontValue : int   // engine order bracketed
	{
		BodyText,               // Conduit 16 [1]
		Terminal,               // FixedSys 9 [0]
		SplitscreenHudMessage,  // FixedSys 9 [loop]
		FullscreenHudMessage,	// Conduit 18 [4]
		Title,                  // Conduit 32 [2]
		SuperLargeFont,         // Conduit 32 [3]
		LargeBodyText,			// Conduit 23
		Value7,
		HudNumberText,			// Conduit 16 [loop]
		Value9,
		Value10,
		Value11,
		Value12
	}

	public enum WidgetFontValue_ODST : int
	{
		Terminal,               // Fixedsys 9
		BodyText,               // Conduit 14 (?)
		Title,                  // Conduit 32
		SuperLargeFont,         // Agency 32
		LargeBodyText,          // Conduit 23
		SplitscreenHudMessage,  // Agency 16
		FullscreenHudMessage,   // Conduit 18
		EnglishBodyText,        // Agency 18
		HudNumberText,          // Conduit 18
		SubtitleFont,           // Conduit 16
		MainMenuFont            // Agency 23
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

	public enum WidgetFontValue_H3MCC : int
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
}
