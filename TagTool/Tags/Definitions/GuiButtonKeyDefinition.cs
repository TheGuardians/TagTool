using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions.Common;

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
            InitiallyVisible = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
        }        
    }
}
