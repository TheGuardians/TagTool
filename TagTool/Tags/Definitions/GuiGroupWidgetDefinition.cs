using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_group_widget_definition", Tag = "grup", Size = 0x5C)]
    public class GuiGroupWidgetDefinition : TagStructure
	{
        public GuiGroupWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public List<ListWidget> ListWidgets;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;
        public List<ModelWidget> ModelWidgets;

        [Flags]
        public enum GuiGroupWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            NotLoadedUponInitialization = 1 << 3
        }
    }
}
