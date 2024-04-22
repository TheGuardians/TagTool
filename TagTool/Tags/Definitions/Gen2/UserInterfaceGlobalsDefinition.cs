using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x20)]
    public class UserInterfaceGlobalsDefinition : TagStructure
    {
        /// <summary>
        /// This is a reference to the ui shared globals tag
        /// </summary>
        [TagField(ValidTags = new [] { "wigl" })]
        public CachedTag SharedGlobals;
        /// <summary>
        /// These are the screen widgets
        /// </summary>
        public List<UserInterfaceWidgetReferenceBlock> ScreenWidgets;
        /// <summary>
        /// This blob defines the ui for setting multiplayer game variant parameters
        /// </summary>
        [TagField(ValidTags = new [] { "goof" })]
        public CachedTag MpVariantSettingsUi;
        /// <summary>
        /// This is for the loc game hopper strings
        /// </summary>
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GameHopperDescriptions;
        
        [TagStructure(Size = 0x8)]
        public class UserInterfaceWidgetReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "wgit" })]
            public CachedTag WidgetTag;
        }
    }
}

