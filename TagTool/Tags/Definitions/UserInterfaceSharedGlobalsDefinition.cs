using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x170, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x160, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x3CC, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    public class UserInterfaceSharedGlobalsDefinition : TagStructure
	{
        public short IncTextUpdatePeriod; // milliseconds
        public short IncTextBlockCharacter; // ASCII code
        public float NearClipPlaneDistance; // objects closer than this are not drawn
        public float ProjectionPlaneDistance; // distance at which objects are rendered when z=0 (normal size)
        public float FarClipPlaneDistance; // objects farther than this are not drawn
        public CachedTag GlobalStrings;
        public CachedTag DamageReportingStrings;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
        public CachedTag InputStrings;

        public CachedTag MainMenuMusic;
        public int MusicFadeTime; // milliseconds
        public RealArgbColor TextColor;
        public RealArgbColor TextShadowColor;
        public List<ColorPreset> ColorPresets;
        public List<PlayerColor> PlayerTintColors;
        public CachedTag DefaultSounds;
        public List<GuiAlertDescription> AlertDescriptions;
        public List<Dialog> DialogDescriptions;
        public List<GlobalDataSource> GlobalDataSources;
        public RealPoint2d WidescreenBitmapScale;
        public RealPoint2d StandardBitmapScale;
        public RealPoint2d MenuBlurFactor;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UiWidgetBiped> UiWidgetBipeds;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UnknownPlayer1;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UnknownPlayer2;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UnknownPlayer3;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UnknownPlayer4;

        //Spartan in H3
        [TagField(Length = 32)] public string McBipedName;
        [TagField(Length = 32)] public string McAiSquadName;
        public StringId McAiStartPosition;
        //Elite in H3
        [TagField(Length = 32)] public string EliteBipedName;
        [TagField(Length = 32)] public string EliteAiSquadName;
        public StringId EliteAiStartPosition;

        
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiMickeyBipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiMickeyAiSquadName;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UiMickeyAiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiRomeoBipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiRomeoAiSquadName;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UiRomeoAiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiDutchBipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiDutchAiSquadName;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UiDutchAiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiJohnsonBipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiJohnsonAiSquadName;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UiJohnsonAiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst2BipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst2AiSquadName;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public StringId UiOdst2AiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst3BipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst3AiSquadName;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public StringId UiOdst3AiLocationName;

        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst4BipedName;
        [TagField(Length = 32, MinVersion = CacheVersion.Halo3ODST)]
        public string UiOdst4AiSquadName;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId UiOdst4AiLocationName;
        
        public int NavigationScrollInterval; //milliseconds
        public int NavigationFastScrollDelay; //milliseconds
        public int NavigationFastScrollInterval; //milliseconds
        public int AttractVideoDelay; // seconds

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction PdaWaypointScaleFunction;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaWaypointJumpSpeed;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaUnknownA;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaOffscreenPlayerArrowRadius;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaUnknownB;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaAiWaypointRadius;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public CachedTag PdaScreenEffect;      
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealArgbColor PdaColorA;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealArgbColor PdaColorB;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId PdaPoiWaypointPrefix;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId PdaPoiWaypointSuffix;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint PdaBriefOpenThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint UnknownC;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ARGBlock> ARG;

        [TagStructure(Size = 0x24)]
        public class ARGBlock : TagStructure
		{
            public StringId Name;
            public CachedTag Audio;
            public CachedTag Timing;
        }

        [TagStructure(Size = 0x14)]
        public class ColorPreset : TagStructure
		{
            public StringId Name;
            public RealArgbColor Color;
        }

        [TagStructure(Size = 0x30)]
        public class PlayerColor : TagStructure
		{
            public List<ColorListBlock> PlayerTextColor;
            public List<ColorListBlock> TeamTextColor;
            public List<ColorListBlock> PlayerUiColor;
            public List<ColorListBlock> TeamUiColor;

            [TagStructure(Size = 0x10)]
            public class ColorListBlock : TagStructure
			{
                public RealArgbColor Color;
            }
        }

        [TagStructure(Size = 0x10)]
        public class GuiAlertDescription : TagStructure
		{
            public StringId ErrorName;
            public GuiAlertFlags Flags;
            public GuiErrorCategoryEnum ErrorCategory;
            public IconValue Icon;

            [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
            public byte[] Pad0;

            public StringId Title;
            public StringId Message;

            [Flags]
            public enum GuiAlertFlags : byte
            {
                AllowAutoDismissal = 1 << 0,
                ShowSpinner = 1 << 1
            }

            public enum GuiErrorCategoryEnum : sbyte
            {
                Default,
                Networking,
                StoragereadingwritingFailure,
                Controller
            }

            public enum IconValue : sbyte
            {
                None,
                Download,
                Pause,
                Upload,
                Checkbox,
            }
        }

        [TagStructure(Size = 0x28)]
        public class Dialog : TagStructure
		{
            public StringId Name;
            public GuiDialogFlags Flags;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public StringId Title;
            public StringId Body;
            public StringId FirstItem;
            public StringId SecondItem;
            public StringId ThirdItem;
            public StringId FourthItem;
            public StringId KeyLegend;
            public GuiDialogChoice DefaultOption;
            public GuiDialogBButtonActionEnum BButtonAction;

            [Flags]
            public enum GuiDialogFlags : ushort
            {
                Unused = 1 << 0
            }

            public enum GuiDialogChoice : short
            {
                FirstItem,
                SecondItem,
                ThirdItem,
                FourthItem
            }

            public enum GuiDialogBButtonActionEnum : short
            {
                DismissesDialog,
                ButtonIgnored,
                FirstItemActivates,
                SecondItemActivates,
                ThirdItemActivates,
                FourthItemActivates
            }
        }

        [TagStructure(Size = 0x10)]
        public class GlobalDataSource : TagStructure
		{
            public CachedTag DataSource;
        }

        [TagStructure(Size = 0x154)]
        public class UiWidgetBiped : TagStructure
		{
            [TagField(Length = 32)] public string AppearanceBipedName;
            [TagField(Length = 32)] public string AppearanceAiSquadName;
            public StringId AppearanceAiLocationName;
            [TagField(Length = 32)] public string RosterPlayer1BipedName;
            [TagField(Length = 32)] public string RosterPlayer1AiSquadName;
            public StringId RosterPlayer1AiLocationName;
            [TagField(Length = 32)] public string RosterPlayer2BipedName;
            [TagField(Length = 32)] public string RosterPlayer2AiSquadName;
            public StringId RosterPlayer2AiLocationName;
            [TagField(Length = 32)] public string RosterPlayer3BipedName;
            [TagField(Length = 32)] public string RosterPlayer3AiSquadName;
            public StringId RosterPlayer3AiLocationName;
            [TagField(Length = 32)] public string RosterPlayer4BipedName;
            [TagField(Length = 32)] public string RosterPlayer4AiSquadName;
            public StringId RosterPlayer4AiLocationName;
        }
    }
}