using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x160, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x3CC, MinVersion = CacheVersion.Halo3ODST)]
    public class UserInterfaceSharedGlobalsDefinition : TagStructure
	{
        public short IncTextUpdatePeriod;
        public short IncTextBlockCharacter;
        public float NearClipPlaneDistance;
        public float ProjectionPlaneDistance;
        public float FarClipPlaneDistance;
        public CachedTagInstance GlobalStrings;
        public CachedTagInstance DamageTypeStrings;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance UnknownStrings;

        public CachedTagInstance MainMenuMusic;
        public int MusicFadeTime;
        public RealArgbColor Color;
        public RealArgbColor TextColor;
        public List<TextColorBlock> TextColors;
        public List<PlayerColor> PlayerColors;
        public CachedTagInstance UiSounds;
        public List<Alert> Alerts;
        public List<Dialog> Dialogs;
        public List<GlobalDataSource> GlobalDataSources;
        public float WidescreenBitmapScaleX;
        public float WidescreenBitmapScaleY;
        public float StandardBitmapScaleX;
        public float StandardBitmapScaleY;
        public float MenuBlurX;
        public float MenuBlurY;

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
        [TagField(Length = 32)] public string UiEliteBipedName;
        [TagField(Length = 32)] public string UiEliteAiSquadName;
        public StringId UiEliteAiLocationName;
        //Elite in H3
        [TagField(Length = 32)] public string UiOdst1BipedName;
        [TagField(Length = 32)] public string UiOdst1AiSquadName;
        public StringId UiOdst1AiLocationName;

        
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
        
        public int SingleScrollSpeed;
        public int ScrollSpeedTransitionWaitTime;
        public int HeldScrollSpeed;
        public int AttractVideoIdleWait;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction Unknown;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown4;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown5;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown6;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public CachedTagInstance Unknown7;      
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealArgbColor Unknown17;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealArgbColor Unknown18;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId Unknown19;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown21;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown22;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ARGBlock> ARG;

        [TagStructure(Size = 0x24)]
        public class ARGBlock : TagStructure
		{
            public StringId Name;
            public CachedTagInstance Audio;
            public CachedTagInstance Timing;
        }

        [TagStructure(Size = 0x14)]
        public class TextColorBlock : TagStructure
		{
            public StringId Name;
            public RealArgbColor Color;
        }

        [TagStructure(Size = 0x30)]
        public class PlayerColor : TagStructure
		{
            public List<PlayerTextColorBlock> PlayerTextColor;
            public List<TeamTextColorBlock> TeamTextColor;
            public List<PlayerUiColorBlock> PlayerUiColor;
            public List<TeamUiColorBlock> TeamUiColor;

            [TagStructure(Size = 0x10)]
            public class PlayerTextColorBlock : TagStructure
			{
                public RealArgbColor Color;
            }

            [TagStructure(Size = 0x10)]
            public class TeamTextColorBlock : TagStructure
			{
                public RealArgbColor Color;
            }

            [TagStructure(Size = 0x10)]
            public class PlayerUiColorBlock : TagStructure
			{
                public RealArgbColor Color;
            }

            [TagStructure(Size = 0x10)]
            public class TeamUiColorBlock : TagStructure
			{
                public RealArgbColor Color;
            }
        }

        [TagStructure(Size = 0x10)]
        public class Alert : TagStructure
		{
            public StringId Name;
            public byte Flags;
            public sbyte Unknown;
            public IconValue Icon;
            public sbyte Unknown2;
            public StringId Title;
            public StringId Body;

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
            public short Unknown;
            public short Unknown2;
            public StringId Title;
            public StringId Body;
            public StringId Option1;
            public StringId Option2;
            public StringId Option3;
            public StringId Option4;
            public StringId KeyLegend;
            public DefaultOptionValue DefaultOption;
            public short Unknown3;

            public enum DefaultOptionValue : short
            {
                Option1,
                Option2,
                Option3,
                Option4,
            }
        }

        [TagStructure(Size = 0x10)]
        public class GlobalDataSource : TagStructure
		{
            public CachedTagInstance DataSource;
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