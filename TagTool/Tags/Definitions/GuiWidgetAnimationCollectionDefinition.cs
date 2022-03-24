using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x240, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x270, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x280, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x2B0, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    public class GuiWidgetAnimationCollectionDefinition : TagStructure
    {
        public CachedTag AmbientFocused;    // plays when idle and has focus
        public CachedTag AmbientUnfocused;  // plays when idle and does not have focus
        public CachedTag AmbientDisabledFocused;    // plays when idle, disabled, and has focus
        public CachedTag AmbientDisabledUnfocused;  // plays when idle, disabled, and does not have focus
        public CachedTag AmbientFocusedAlt;
        public CachedTag AmbientUnfocusedAlt;
        public CachedTag TransitionFrom;
        public CachedTag TransitionTo;
        public CachedTag TransitionBackFrom;
        public CachedTag TransitionBackTo;
        public CachedTag CycleInPreviousPane;
        public CachedTag CycleInNextPane;
        public CachedTag CycleOutPreviousPane;
        public CachedTag CycleOutNextPane;
        public CachedTag DisplayGroupTransIn;
        public CachedTag DisplayGroupTransOut;
        public CachedTag ControlReceivedFocus;
        public CachedTag ControlLostFocus;
        public CachedTag IndicatorAmbientMoreItems;
        public CachedTag IndicatorAmbientNoMoreItems;
        public CachedTag IndicatorActivatedMoreItems;
        public CachedTag IndicatorActivatedNoMoreItems;
        public CachedTag LoadSubmenuFocused;
        public CachedTag LoadSubmenuUnfocused;
        public CachedTag UnloadSubmenuFocused;
        public CachedTag UnloadSubmenuUnfocused;
        public CachedTag LoadAsSubmenu;
        public CachedTag UnloadAsSubmenu;
        public CachedTag ChildSubmenuAmbientFocused;
        public CachedTag ChildSubmenuAmbientUnfocused;
        public CachedTag Custom0;
        public CachedTag Custom1;
        public CachedTag CustomTransitionIn0;
        public CachedTag CustomTransitionOut0;
        public CachedTag CustomTransitionIn1;
        public CachedTag CustomTransitionOut1;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag Unknown37;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag Unknown38;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag Unknown39;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag Unknown40;

        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag MouseEnter;
        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag MouseLeave;
        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag MouseHoverAmbient;
    }
}
