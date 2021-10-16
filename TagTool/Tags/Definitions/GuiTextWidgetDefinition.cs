using System;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_text_widget_definition", Tag = "txt3", Size = 0x3C)]
    public class GuiTextWidgetDefinition : TagStructure
	{
        public GuiTextFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId ValueOverrideList;
        public StringId ValueIdentifier; // for setting string
        public StringId TextColorPreset;
        public FontValue CustomFont;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] TextWidgetDefinitionPad;

        [Flags]
        public enum GuiTextFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            LeftJustify = 1 << 3,
            RightJustify = 1 << 4,
            Scrollable = 1 << 5,
            Uppercase = 1 << 6,
            StringFromExportedText = 1 << 7,
            StringFromExportedStringId = 1 << 8,
            StringFromExportedGlobalStringId = 1 << 9,
            StringFromExportedInteger = 1 << 10,
            StringFromListItemLabel = 1 << 11,
            UseBracketsToIndicateFocus = 1 << 12,
            LargeTextBuffer255Chars = 1 << 13,
            ExtralargeTextBuffer = 1 << 14,
            SingleDropShadow = 1 << 15,
            NoDropShadow = 1 << 16,
            AllowListItemToOverrideAnimationSkin = 1 << 17,
            DoNotWrapText = 1 << 18,
        }
    }
}
