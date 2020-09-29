using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x3C)]
    public class UserInterfaceGlobalsDefinition : TagStructure
    {
        /// <summary>
        /// Shared Globals
        /// </summary>
        /// <remarks>
        /// This is a reference to the ui shared globals tag
        /// </remarks>
        public CachedTag SharedGlobals;
        /// <summary>
        /// Screen Widgets
        /// </summary>
        /// <remarks>
        /// These are the screen widgets
        /// </remarks>
        public List<UserInterfaceWidgetReference> ScreenWidgets;
        /// <summary>
        /// Multiplayer Variant Settings Interface
        /// </summary>
        /// <remarks>
        /// This blob defines the ui for setting multiplayer game variant parameters
        /// </remarks>
        public CachedTag MpVariantSettingsUi;
        /// <summary>
        /// Game Hopper Localization Strings
        /// </summary>
        /// <remarks>
        /// This is for the loc game hopper strings
        /// </remarks>
        public CachedTag GameHopperDescriptions;
        
        [TagStructure(Size = 0x10)]
        public class UserInterfaceWidgetReference : TagStructure
        {
            public CachedTag WidgetTag;
        }
    }
}

