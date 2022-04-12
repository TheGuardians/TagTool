using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x240, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x270, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x280, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x2B0, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    public class GuiWidgetAnimationCollectionDefinition : TagStructure
    {
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientFocused;    // plays when idle and has focus
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientUnfocused;  // plays when idle and does not have focus
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientDisabledFocused;    // plays when idle, disabled, and has focus
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientDisabledUnfocused;  // plays when idle, disabled, and does not have focus
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientFocusedAlt;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag AmbientUnfocusedAlt;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag TransitionFrom;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag TransitionTo;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag TransitionBackFrom;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag TransitionBackTo;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CycleInPreviousPane;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CycleInNextPane;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CycleOutPreviousPane;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CycleOutNextPane;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag DisplayGroupTransIn;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag DisplayGroupTransOut;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag ControlReceivedFocus;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag ControlLostFocus;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag IndicatorAmbientMoreItems;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag IndicatorAmbientNoMoreItems;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag IndicatorActivatedMoreItems;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag IndicatorActivatedNoMoreItems;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag LoadSubmenuFocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag LoadSubmenuUnfocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag UnloadSubmenuFocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag UnloadSubmenuUnfocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag LoadAsSubmenu;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag UnloadAsSubmenu;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag ChildSubmenuAmbientFocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag ChildSubmenuAmbientUnfocused;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag Custom0;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag Custom1;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CustomTransitionIn0;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CustomTransitionOut0;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CustomTransitionIn1;
        [TagField(ValidTags = new[] { "wgan" })] public CachedTag CustomTransitionOut1;

        [TagField(ValidTags = new[] { "wgan" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag CustomDisplayGroupIn;
        [TagField(ValidTags = new[] { "wgan" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag CustomDisplayGroupOut;
        [TagField(ValidTags = new[] { "wgan" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag TransitionToOverlaid;
        [TagField(ValidTags = new[] { "wgan" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag TransitionFromOverlaid;

        [TagField(ValidTags = new[] { "wgan" }, Platform = CachePlatform.MCC)]
        public CachedTag MouseEnter;
        [TagField(ValidTags = new[] { "wgan" }, Platform = CachePlatform.MCC)]
        public CachedTag MouseLeave;
        [TagField(ValidTags = new[] { "wgan" }, Platform = CachePlatform.MCC)]
        public CachedTag MouseHoverAmbient;
    }
}
