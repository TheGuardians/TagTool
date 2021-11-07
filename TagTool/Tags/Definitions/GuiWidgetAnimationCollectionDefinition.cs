using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x240, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x270, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x280, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_widget_animation_collection_definition", Tag = "wacd", Size = 0x2B0, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    public class GuiWidgetAnimationCollectionDefinition : TagStructure
    {
        public CachedTag Ambientfocused;
        public CachedTag Ambientunfocused;
        public CachedTag Ambientdisabledfocused;
        public CachedTag Ambientdisabledunfocused;
        public CachedTag Ambientfocusedalt;
        public CachedTag Ambientunfocusedalt;
        public CachedTag Transitionfrom;
        public CachedTag Transitionto;
        public CachedTag Transitionbackfrom;
        public CachedTag Transitionbackto;
        public CachedTag Cycleinpreviouspane;
        public CachedTag Cycleinnextpane;
        public CachedTag Cycleoutpreviouspane;
        public CachedTag Cycleoutnextpane;
        public CachedTag Displaygrouptransin;
        public CachedTag Displaygrouptransout;
        public CachedTag Controlrecvfocus;
        public CachedTag Controllostfocus;
        public CachedTag Indicatorambientmoreitems;
        public CachedTag Indicatorambientnomoreitems;
        public CachedTag Indicatoractivatedmoreitems;
        public CachedTag Indicatoractivatednomoreitems;
        public CachedTag Loadsubmenufocused;
        public CachedTag Loadsubmenuunfocused;
        public CachedTag Unloadsubmenufocused;
        public CachedTag Unloadsubmenuunfocused;
        public CachedTag Loadassubmenu;
        public CachedTag Unloadassubmenu;
        public CachedTag ChildSubmenuAmbientfocused;
        public CachedTag ChildSubmenuAmbientunfocused;
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
        public CachedTag Mouseenter;
        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag Mouseleave;
        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag Mousehoverambient;
    }
}
