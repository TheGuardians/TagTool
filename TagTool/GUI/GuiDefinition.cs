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
            TopLeftCorner,
            TopRightCorner,
            BottomRightCorner,
            BottomReftCorner
        }
    }

	public enum WidgetFontValue : int   // engine order bracketed
	{
		BodyText,               // Conduit 16 [1]
		MainMenu,				// FixedSys 9 [0]
		Terminal,               // FixedSys 9 [loop]
		FullScreenHudMessage,	// Conduit 18 [4]
		Title,					// Conduit 32 [2]
		SuperLarge,             // Conduit 32 [3]
		LargeBodyText,			// Conduit 23
		Value7,
		HudNumber,              // Conduit 16 [loop]
		Value9,
		Value10,
		Value11,
		Value12
	}

	public enum WidgetFontValue_ODST : int
	{
		Fixedsys9,      // Conduit 18	[8]
		Conduit16,      // Agency 16	[5]
		Terminal,       // FixedSys 9	[0]
		Conduit23,      // Conduit 14	[1]
		Conduit18,      // Conduit 32	[2]
		larabie_10,     // Agency 32	[3]
		Value7,			// Conduit 23	[4]
		Value8,			// Agency 18	[7]
		Value9,			// Conduit 18	[8]
		Value10,			// Conduit 16	[9]
		Value11			// Agency 23	[10]
	}

	public enum WidgetFontValue_H3Original : short
	{
		FullScreenHudMessage,   // Conduit 18 [06]
		SplitScreenHudMessage,  // FixedSys9 [05]
		Terminal,               // FixedSys9 [00]
		BodyText,               // Conduit 16 [01]
		Title,                  // Conduit 32 [02]
		SuperLarge,             // Conduit 32 [03]
		LargeBodyText,          // Conduit 23 [04]
		EnglishBodyText,        // Larabie 10 [07]
		HudNumber,              // Conduit 18 [08]
		Subtitle,               // Conduit 16 [09]
		MainMenu                // Pragmata 14 [10]
	}

	public enum WidgetFontValue_H3MCC : int
	{
		FullScreenHudMessage,   // Conduit 18 [06]
		SplitScreenHudMessage,  // FixedSys9 [05]
		Terminal,               // FixedSys9 [00]
		BodyText,               // Conduit 16 [01]
		Title,                  // Conduit 32 [02]
		SuperLarge,             // Conduit 32 [03]
		LargeBodyText,          // Conduit 23 [04]
		EnglishBodyText,        // Larabie 10 [07]
		HudNumber,              // Conduit 18 [08]
		Subtitle,               // Conduit 16 [09]
		MainMenu                // Pragmata 14 [10]
	}
}
