using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x17C)]
    public class UserInterfaceSharedGlobalsDefinition : TagStructure
    {
        public short IncTextUpdatePeriod; // milliseconds
        public short IncTextBlockCharacter; // ASCII code
        public float NearClipPlaneDistance; // objects closer than this are not drawn
        public float ProjectionPlaneDistance; // distance at which objects are rendered when z=0 (normal size)
        public float FarClipPlaneDistance; // objects farther than this are not drawn
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag UnicodeStringListTag;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag UnicodeDamageReportingStringListTag;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag UnicodeFireTeamMemberNameStringList;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag UnicodeFireTeamMemberServiceTagStringList;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag MainMenuMusic;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag MainMenuAlternateMusic;
        public int MusicFadeTime; // milliseconds
        public RealArgbColor TextColor;
        public RealArgbColor ShadowColor;
        public List<ColorPresetsBlock> ColorPresets;
        public List<TintColorsBlock> TintColors;
        public short PrimaryEmblemCount;
        public short SecondaryEmblemCount;
        [TagField(ValidTags = new [] { "uise" })]
        public CachedTag DefaultSounds;
        public List<GuiAlertDescriptionBlock> AlertDescriptions;
        public List<GuiDialogDescriptionBlock> DialogDescriptions;
        public RealVector2d _16x9;
        public RealVector2d _4x3;
        public float HorizontalBlurFactor;
        public float VerticalBlurFactor;
        public StringId McBipedName;
        [TagField(Length = 32)]
        public string McAiSquadName;
        public StringId McAiStartPos;
        public StringId EliteBipedName;
        [TagField(Length = 32)]
        public string EliteAiSquadName;
        public StringId EliteAiStartPos;
        public StringId SpartanPortraitBipedName;
        public StringId ElitePortraitBipedName;
        public int NavigationTabDelayMsec;
        public int NavigationTabFastWaitMsec;
        public int NavigationTabFastDelayMsec;
        public MappingFunction SpinnerTabSpeedFunction;
        public int MaxInputTime;
        public int Delay; // seconds
        public List<PgcrIncidentBlockStruct> PgcrPerPlayerTrackedIncidents;
        
        [TagStructure(Size = 0x14)]
        public class ColorPresetsBlock : TagStructure
        {
            public StringId Name;
            public RealArgbColor Color;
        }
        
        [TagStructure(Size = 0x80)]
        public class TintColorsBlock : TagStructure
        {
            public List<ColorListBlock> TextPlayer;
            public List<ColorListBlock> TextTeam;
            public List<ColorListBlock> BitmapPlayer;
            public List<ColorListBlock> BitmapTeam;
            public RealArgbColor BitmapFriend;
            public RealArgbColor BitmapEnemy;
            public RealArgbColor BitmapNeutral;
            public RealArgbColor BitmapFlood;
            public RealArgbColor BitmapSpartans;
            
            [TagStructure(Size = 0x10)]
            public class ColorListBlock : TagStructure
            {
                public RealArgbColor Color;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class GuiAlertDescriptionBlock : TagStructure
        {
            public StringId ErrorName;
            public GuiAlertFlags Flags;
            public GuiErrorCategoryEnum ErrorCategory;
            public GuiErrorIconEnum ErrorIcon;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
                Storage,
                Controller
            }
            
            public enum GuiErrorIconEnum : sbyte
            {
                DefaultAlert,
                Downloading,
                Paused,
                Uploading,
                Completed
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class GuiDialogDescriptionBlock : TagStructure
        {
            public StringId DialogName;
            public GuiDialogFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Title;
            public StringId Message;
            public StringId FirstItem;
            public StringId SecondItem;
            public StringId ThirdItem;
            public StringId FourthItem;
            public StringId ButtonKey;
            public GuiDialogChoiceEnum DefaultItem;
            public GuiDialogBButtonActionEnum BButtonAction;
            
            [Flags]
            public enum GuiDialogFlags : ushort
            {
                Unused = 1 << 0
            }
            
            public enum GuiDialogChoiceEnum : short
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
        
        [TagStructure(Size = 0x14)]
        public class MappingFunction : TagStructure
        {
            public byte[] Data;
        }
        
        [TagStructure(Size = 0x8)]
        public class PgcrIncidentBlockStruct : TagStructure
        {
            public StringId IncidentName;
            // number of times this can happen before the PGCR stops tracking them
            public int MaximumStatCount;
        }
    }
}
