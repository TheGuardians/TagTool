using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_screen_widget_definition", Tag = "scn3", Size = 0xA8, Platform = CachePlatform.Original)]
    [TagStructure(Name = "gui_screen_widget_definition", Tag = "scn3", Size = 0xB8, Platform = CachePlatform.MCC)]
    public class GuiScreenWidgetDefinition : TagStructure
	{
        public GuiScreenWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag StringList;
        public CachedTag ScreenTemplate;
        public StringId InitialButtonKeyName;
        public List<DataSource> DebugDatasources;
        public List<GroupWidget> GroupWidgets;
        public List<ButtonKeyLegend> ButtonKeys;
        public CachedTag SoundOverrides;
        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag MouseCursors;
        [TagField(Length = 32)]
        public string OnLoadScriptName;
        public short ScriptIndex;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Scary;

        [Flags]
        public enum GuiScreenWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            BBackShouldntDisposeScreen = 1 << 3
        }

        [TagStructure(Size = 0x10)]
        public class DataSource : TagStructure
		{
            public CachedTag DataSourceTag;
        }

        [TagStructure(Size = 0x6C)]
        public class GroupWidget : TagStructure
		{
            public CachedTag TemplateReference;
            public GuiGroupWidgetDefinition Definition;
        }

        [TagStructure(Size = 0x10)]
        public class ButtonKeyLegend : TagStructure
		{
            public CachedTag Definition;
        }
    }
}
