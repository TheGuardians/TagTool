using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Tags.GUI;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_button_key_definition", Tag = "bkey", Size = 0x54)]
    public class GuiButtonKeyDefinition : TagStructure
	{
        public GuiButtonKeyFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag Strings;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;

        [Flags]
        public enum GuiButtonKeyFlags : uint
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            InitiallyVisible = 1 << 3
        }        
    }
}
