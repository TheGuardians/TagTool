using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_skin_definition", Tag = "skn3", Size = 0x1C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_skin_definition", Tag = "skn3", Size = 0x28, MinVersion = CacheVersion.Halo3ODST)]
    public class GuiSkinDefinition : TagStructure
    {
        public GuiSkinDefinitionFlags Flags;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ModelWidget> ModelWidgets;

        [Flags]
        public enum GuiSkinDefinitionFlags : uint
        {
            UNUSED = 1 << 0
        }
    }
}
