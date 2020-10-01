using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "ui_widget_collection", Tag = "Soul", Size = 0xC)]
    public class UiWidgetCollection : TagStructure
    {
        public List<UiWidgetReferencesBlock> UiWidgetDefinitions;
        
        [TagStructure(Size = 0x10)]
        public class UiWidgetReferencesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "DeLa" })]
            public CachedTag UiWidgetDefinition;
        }
    }
}

