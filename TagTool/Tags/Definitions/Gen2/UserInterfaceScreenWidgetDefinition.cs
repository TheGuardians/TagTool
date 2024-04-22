using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_screen_widget_definition", Tag = "wgit", Size = 0x70)]
    public class UserInterfaceScreenWidgetDefinition : TagStructure
    {
        /// <summary>
        /// - the widget coordinate system is a left-handed system (+x to the right, +y up, +z into the screen)
        ///   with the origin
        /// centered in the display (regardless of display size)
        /// - for widget component placement, all coordinates you define in the
        /// tag specifiy the object's
        ///   placement prior to the application of any animation
        /// - all coordinates you define are local to
        /// that object
        /// - all text specific to objects in the screen comes from the screen's string list tag
        ///   all of the string
        /// indices you may need to specify will refer to the screen's string list tag
        /// - a pane may contain either buttons OR a list
        /// OR a table-view, but never a combination of those
        ///   (widget won't function correctly if you try that)
        /// - all text is
        /// centered unless you specify otherwise
        /// </summary>
        /// <summary>
        /// Set misc. screen behavior here
        /// </summary>
        public FlagsValue Flags;
        public ScreenIdValue ScreenId;
        /// <summary>
        /// The labels here are just a guide; the actual string used comes from the Nth position
        /// of this button key entry as found in
        /// the ui globals button key string list tag
        /// </summary>
        public ButtonKeyTypeValue ButtonKeyType;
        /// <summary>
        /// Any ui elements that don't explicitly set a text color will use this color
        /// </summary>
        public RealArgbColor TextColor;
        /// <summary>
        /// All text specific to this screen
        /// </summary>
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag StringListTag;
        /// <summary>
        /// Define the screen's panes here (normal screens have 1 pane, tab-view screens have 2+ panes)
        /// </summary>
        public List<WindowPaneReferenceBlock> Panes;
        /// <summary>
        /// If the screen is to have a shape group, specify it here (references group in user interface globals tag)
        /// </summary>
        public ShapeGroupValue ShapeGroup;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        /// <summary>
        /// These are down here because they got added on later. Have a nice day.
        /// </summary>
        public StringId HeaderStringId;
        /// <summary>
        /// String IDs here allow defining new string ids that are visible only to this screen.
        /// </summary>
        public List<LocalStringIdListSectionReferenceBlock> LocalStrings;
        /// <summary>
        /// Bitmaps here allow adding a bitmap that's accessible in code for use in this screen.
        /// </summary>
        public List<LocalBitmapReferenceBlock> LocalBitmaps;
        /// <summary>
        /// These are used only for level load progress bitmaps
        /// </summary>
        public RealRgbColor SourceColor;
        public RealRgbColor DestinationColor;
        public float AccumulateZoomScaleX;
        public float AccumulateZoomScaleY;
        public float RefractionScaleX;
        public float RefractionScaleY;
        /// <summary>
        /// The mouse cursor definition for this screen.
        /// </summary>
        [TagField(ValidTags = new [] { "mcsr" })]
        public CachedTag MouseCursorDefinition;
        
        [Flags]
        public enum FlagsValue : uint
        {
            _14ScreenDialog = 1 << 0,
            MultiplePanesAreForListFlavorItems = 1 << 1,
            NoHeaderText = 1 << 2,
            _12ScreenDialog = 1 << 3,
            LargeDialog = 1 << 4,
            DisableOverlayEffect = 1 << 5
        }
        
        public enum ScreenIdValue : short
        {
            /// <summary>
            /// 1
            /// </summary>
            Test,
            /// <summary>
            /// 2
            /// </summary>
            Test1,
            /// <summary>
            /// 3
            /// </summary>
            Test2,
            /// <summary>
            /// 4
            /// </summary>
            Test3,
            /// <summary>
            /// 5
            /// </summary>
            Test4,
            GameShellBackground,
            MainMenu,
            ErrorDialogOkCancel,
            ErrorDialogOk,
            PressStartIntro,
            PlayerProfileSelect,
            SpLevelSelect,
            SpDifficultySelect,
            NetworkSquadBrowser,
            MpPregameLobby,
            CustomGameMenu,
            PostgameStats,
            MpMapSelect,
            SpPauseGame,
            Settings,
            GamertagSelect,
            GamertagPasscodeEntry,
            MultiplayerProtocol,
            SquadSettings,
            SquadGameSettings,
            SquadPrivacySettings,
            YMenuGameshell,
            YMenuGameshellCollapsed,
            YMenuIngame,
            YMenuIngameCollapsed,
            _4wayJoinScreen,
            YMenuPlayerSelectedOptions,
            PlayerSelectedOptions,
            ConfirmationDialog,
            LiveFeedbackMenuDialog,
            LiveMessageTypeDialog,
            VoiceMsgDialog,
            StereoFaceplate,
            PlayerProfileEditMenu,
            PpControllerSettings,
            PpButtonSettings,
            PpThumbstickSettings,
            PpLookSensitivitySettings,
            PpInvertLookSettings,
            PpAutolevelSettings,
            PpHandicapSettings,
            PpHighScoreRecSettings,
            PpMultiplayerSettingsMenu,
            PpProfileDeleteConfirmationDlg,
            PpChooseForegroundEmblem,
            PpChoosePrimaryColor,
            PpChooseSecondaryColor,
            PpChooseModel,
            PpVoiceSettingsMenu,
            PpChooseVoiceMask,
            PpVoiceThruTv,
            PpEditRotationList,
            PpXblStatusMenu,
            PpAppearOffline,
            PpAutoOffline,
            GameEngineCategoryListing,
            EditSlayerMenu,
            EditKothMenu,
            EditRaceMenu,
            EditOddballMenu,
            EditJuggernautMenu,
            EditHeadhunterMenu,
            EditCtfMenu,
            EditAssaultMenu,
            EditSlayerScoreToWin,
            EditSlayerTimeLimit,
            EditSlayerTeams,
            EditSlayerScore4Killing,
            EditSlayerKillInOrder,
            EditSlayerDeathPtLoss,
            EditSlayerSuicidePtLoss,
            EditSlayerDmgAfterKill,
            EditSlayerDmgAfterDeath,
            EditSlayerSpeedAfterKill,
            EditSlayerSpeedAfterDeath,
            EditKothScoreToWin,
            EditKothTimeLimit,
            EditKothTeams,
            EditKothMovingHills,
            EditKothUncontesedControl,
            EditKothXtraDmg,
            EditRaceLapsToWin,
            EditRaceTimeLimit,
            EditRaceTeams,
            EditRaceTeamScoring,
            EditRaceType,
            EditRaceFlagOrder,
            EditRaceGameEndCondition,
            EditRaceDmgWithLaps,
            EditRaceSpeedWithLaps,
            EditOddballTimeToWin,
            EditOddballTimeLimit,
            EditOddballTeams,
            EditOddballBallSpawnCount,
            EditOddballBallWaypoints,
            EditOddballDamageWithBall,
            EditOddballSpeedWithBall,
            EditOddballInvisibilityWithBall,
            EditJugScoreToWin,
            EditJugTimeLimit,
            EditJugPtsForKillingJugger,
            EditJugCount,
            EditJugSpecies,
            EditJugStartingEquip,
            EditJugDamage,
            EditJugHealth,
            EditJugSpeed,
            EditJugRegeneration,
            EditJugWaypoints,
            EditHhScoreToWin,
            EditHhTimeLimit,
            EditHhTeams,
            EditHhDeathPtLoss,
            EditHhSuicidePtLoss,
            EditHhSpeedWithToken,
            EditHhDroppedTokenLifetime,
            EditHhScoreMultiplier,
            EditCtfScoreToWin,
            EditCtfTimeLimit,
            EditCtfTieResolution,
            EditCtfSingleFlag,
            EditCtfRoleSwapping,
            EditCtfFlagAtHomeToScore,
            EditCtfFlagMustReset,
            EditCtfDmgWithFlag,
            EditCtfSpeedWithFlag,
            EditAssaultScoreToWin,
            EditAssaultTimeLimit,
            EditAssaultTieResolution,
            EditAssaultSingleFlag,
            EditAssaultRoleSwapping,
            EditAssaultEnemyFlagAtHomeToScore,
            EditAssaultFlagMustReset,
            EditAssaultDmgWithFlag,
            EditAssaultSpeedWithFlag,
            EditPlayerNumberOfLives,
            EditPlayerMaxHealth,
            EditPlayerShields,
            EditPlayerRespawnTime,
            EditPlayerCount,
            EditPlayerInvisibility,
            EditPlayerSuicidePenalty,
            EditPlayerFriendlyFire,
            EditItemRespawnGrenades,
            EditItemPowerups,
            EditItemWeaponSet,
            EditItemStartingEquipment,
            EditItemWarthogs,
            EditItemGhosts,
            EditItemScorpions,
            EditItemBanshees,
            EditItemMongeese,
            EditItemShadows,
            EditItemWraiths,
            EditIndicatorObjectives,
            EditIndicatorPlayersOnMotionSensor,
            EditIndicatorInvisiblePlayersOnMotionSensor,
            EditIndicatorFriends,
            EditIndicatorEnemies,
            EditPlayerOptions,
            EditItemOptions,
            EditIndicatorOptions,
            VirtualKeyboard,
            CustomGameMenu1,
            SlayerQuickOptions,
            KothQuickOptions,
            RaceQuickOptions,
            OddballQuickOptions,
            JuggerQuickOptions,
            HhQuickOptions,
            CtfQuickOptions,
            AssaultQuickOptions,
            PickNewSquadLeader,
            VariantEditingOptionsMenu,
            PlaylistListSettings,
            PlaylistContents,
            PlaylistSelectedOptions,
            XboxLiveTaskProgress,
            PpVibrationSettings,
            BootPlayerDialog,
            PostgameStatsLobby,
            XboxLiveMainMenu,
            EditTerriesMenu,
            EditTerriesScoreToWin,
            EditTerriesTimeLimit,
            EditTerriesTeams,
            TerriesQuickOptions,
            XboxLiveNotificationBeeper,
            PlayerProfileSelectFancy,
            SavedGameFileActionsDialog,
            MpStartMenu,
            MpStartPlayerSettings,
            MpStartHandicapSettings,
            MpStartChangeTeams,
            MpStartAdminSettings,
            MpStartControllerSettings,
            MpStartVoiceSettings,
            MpStartOnlineStatus,
            MpalphaLegalWarning,
            SquadJoinProgressDialog,
            MpAlphaPostgameLegalWarning,
            MpMapSelectLobby,
            MpVariantTypeLobby,
            MpVariantListLobby,
            LoadingProgress,
            MatchmakingProgress,
            LiveMessageDisplay,
            FadeInFromBlack,
            LivePlayerProfile,
            LiveClanProfile,
            LiveMessageSend,
            FriendsOptionsDialog,
            ClanOptionsDialog,
            CampaignOptionsDialog,
            OptimatchHoppersFullscreen,
            PlaylistListDialog,
            VariantEditingFormat,
            VariantQuickOptionsFormat,
            VariantParamSettingFormat,
            VehicleOptions,
            MatchOptions,
            PlayerOptions,
            TeamOptions,
            GameOptions,
            EquipmentOptions,
            MultipleChoiceDialog,
            NetworkTransitionProgress,
            XboxLiveStats,
            PpChooseBackgroundEmblem,
            PpButtonsQtr,
            PpStixQtr,
            ClanMemberPrivs,
            OptimatchHoppersLobby,
            SavedGameFileDialog,
            Xyzzy,
            ErrorOkCancelLarge,
            Yzzyx,
            SubtitleDisplay,
            PpKeyboardSettings,
            PpKeyboardSettingsQtr,
            PpInvertDualWield,
            SystemSettings,
            BungieNews,
            FilterSelect,
            LiveGameBrowser,
            GameDetails,
            MpCustomMapSelect,
            MpAllMapsSelect,
            PpAdvancedKeyboardSettings,
            PpAdvancedKeyboardSettingsQtr,
            NetworkAdapterSettings
        }
        
        public enum ButtonKeyTypeValue : short
        {
            None,
            ASelectBBack,
            ASelectBCancel,
            AEnterBCancel,
            YXboxLivePlayers,
            XFriendsOptions,
            XClanOptions,
            XRecentPlayersOptions,
            XOptions,
            ASelect,
            XSettingsASelectBBack,
            XDeleteASelectBDone,
            AAccept,
            BCancel,
            YXboxLivePlayersASelectBBack,
            YXboxLivePlayersASelectBCancel,
            YXboxLivePlayersAEnterBCancel,
            YXboxLivePlayersASelect,
            YXboxLivePlayersASelectBDone,
            YXboxLivePlayersAAccept,
            YXboxLivePlayersBCancel,
            XDeleteASelectBBack,
            AOk
        }
        
        [TagStructure(Size = 0x4C)]
        public class WindowPaneReferenceBlock : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public AnimationIndexValue AnimationIndex;
            /// <summary>
            /// If the pane contains buttons, define them here
            /// </summary>
            public List<ButtonWidgetReferenceBlock> Buttons;
            /// <summary>
            /// If the pane contains a list, define it here
            /// </summary>
            public List<ListReferenceBlock> ListBlock;
            /// <summary>
            /// If the pane contains a table-view, define it here
            /// </summary>
            public List<TableViewListReferenceBlock> TableView;
            /// <summary>
            /// Define additional flavor items here
            /// </summary>
            public List<TextBlockReferenceBlock> TextBlocks;
            public List<BitmapBlockReferenceBlock> BitmapBlocks;
            public List<UiModelSceneReferenceBlock> ModelSceneBlocks;
            /// <summary>
            /// these are all OBSOLETE
            /// </summary>
            public List<STextValuePairBlocksBlockUnused> TextValueBlocks;
            public List<HudBlockReferenceBlock> HudBlocks;
            public List<PlayerBlockReferenceBlock> PlayerBlocks;
            
            public enum AnimationIndexValue : short
            {
                None,
                _00,
                _01,
                _02,
                _03,
                _04,
                _05,
                _06,
                _07,
                _08,
                _09,
                _10,
                _11,
                _12,
                _13,
                _14,
                _15,
                _16,
                _17,
                _18,
                _19,
                _20,
                _21,
                _22,
                _23,
                _24,
                _25,
                _26,
                _27,
                _28,
                _29,
                _30,
                _31,
                _32,
                _33,
                _34,
                _35,
                _36,
                _37,
                _38,
                _39,
                _40,
                _41,
                _42,
                _43,
                _44,
                _45,
                _46,
                _47,
                _48,
                _49,
                _50,
                _51,
                _52,
                _53,
                _54,
                _55,
                _56,
                _57,
                _58,
                _59,
                _60,
                _61,
                _62,
                _63
            }
            
            [TagStructure(Size = 0x3C)]
            public class ButtonWidgetReferenceBlock : TagStructure
            {
                public TextFlagsValue TextFlags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public CustomFontValue CustomFont;
                public RealArgbColor TextColor;
                public Rectangle2d Bounds;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                /// <summary>
                /// from top-left
                /// </summary>
                public Point2d BitmapOffset;
                public StringId StringId;
                public short RenderDepthBias;
                public short MouseRegionTopOffset;
                public ButtonFlagsValue ButtonFlags;
                
                [Flags]
                public enum TextFlagsValue : uint
                {
                    LeftJustifyText = 1 << 0,
                    RightJustifyText = 1 << 1,
                    PulsatingText = 1 << 2,
                    CalloutText = 1 << 3,
                    Small31CharBuffer = 1 << 4
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                public enum CustomFontValue : short
                {
                    Terminal,
                    BodyText,
                    Title,
                    SuperLargeFont,
                    LargeBodyText,
                    SplitScreenHudMessage,
                    FullScreenHudMessage,
                    EnglishBodyText,
                    HudNumberText,
                    SubtitleFont,
                    MainMenuFont,
                    TextChatFont
                }
                
                [Flags]
                public enum ButtonFlagsValue : uint
                {
                    DoesnTTabVertically = 1 << 0,
                    DoesnTTabHorizontally = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ListReferenceBlock : TagStructure
            {
                public FlagsValue Flags;
                public SkinIndexValue SkinIndex;
                public short NumVisibleItems;
                public Point2d BottomLeft;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                /// <summary>
                /// This is unused
                /// </summary>
                public List<STextValuePairReferenceBlockUnused> Unused;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    ListWraps = 1 << 0,
                    Interactive = 1 << 1
                }
                
                public enum SkinIndexValue : short
                {
                    Default,
                    SquadLobbyPlayerList,
                    SettingsList,
                    PlaylistEntryList,
                    Variants,
                    GameBrowser,
                    OnlinePlayerMenu,
                    GameSetupMenu,
                    PlaylistContentsDisplay,
                    PlayerProfilePicker,
                    MpMapSelection,
                    MainMenuList,
                    ColorPicker,
                    ProfilePicker,
                    YMenuRecentList,
                    PcrTeamStats,
                    PcrPlayerStats,
                    PcrKillStats,
                    PcrPvpStats,
                    PcrMedalStats,
                    MatchmakingProgress,
                    Default5,
                    Default6,
                    AdvancedSettingsList,
                    LiveGameBrowser,
                    DefaultWide,
                    Unused26,
                    Unused27,
                    Unused28,
                    Unused29,
                    Unused30,
                    Unused31
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                [TagStructure(Size = 0x14)]
                public class STextValuePairReferenceBlockUnused : TagStructure
                {
                    /// <summary>
                    /// this is all obsolete
                    /// </summary>
                    public ValueTypeValue ValueType;
                    /// <summary>
                    /// Enter the value in the box corresponding to the value type you specified above
                    /// </summary>
                    public BooleanValueValue BooleanValue;
                    public int IntegerValue;
                    public float FpValue;
                    public StringId TextValueStringId;
                    /// <summary>
                    /// This is text string associated with data when it has the value specified above.
                    /// The string comes from the screen's string
                    /// list tag.
                    /// </summary>
                    public StringId TextLabelStringId;
                    
                    public enum ValueTypeValue : short
                    {
                        IntegerNumber,
                        FloatingPointNumber,
                        Boolean,
                        TextString
                    }
                    
                    public enum BooleanValueValue : short
                    {
                        False,
                        True
                    }
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class TableViewListReferenceBlock : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public CustomFontValue CustomFont;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealArgbColor TextColor;
                public Point2d TopLeft;
                public List<TableViewListRowReferenceBlock> TableRows;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Unused = 1 << 0
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                public enum CustomFontValue : short
                {
                    Terminal,
                    BodyText,
                    Title,
                    SuperLargeFont,
                    LargeBodyText,
                    SplitScreenHudMessage,
                    FullScreenHudMessage,
                    EnglishBodyText,
                    HudNumberText,
                    SubtitleFont,
                    MainMenuFont,
                    TextChatFont
                }
                
                [TagStructure(Size = 0x10)]
                public class TableViewListRowReferenceBlock : TagStructure
                {
                    public FlagsValue Flags;
                    public short RowHeight;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<TableViewListItemReferenceBlock> RowCells;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Unused = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class TableViewListItemReferenceBlock : TagStructure
                    {
                        public TextFlagsValue TextFlags;
                        public short CellWidth;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public Point2d BitmapTopLeft; // if there is a bitmap
                        [TagField(ValidTags = new [] { "bitm" })]
                        public CachedTag BitmapTag;
                        public StringId StringId;
                        public short RenderDepthBias;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        
                        [Flags]
                        public enum TextFlagsValue : uint
                        {
                            LeftJustifyText = 1 << 0,
                            RightJustifyText = 1 << 1,
                            PulsatingText = 1 << 2,
                            CalloutText = 1 << 3,
                            Small31CharBuffer = 1 << 4
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class TextBlockReferenceBlock : TagStructure
            {
                public TextFlagsValue TextFlags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public CustomFontValue CustomFont;
                public RealArgbColor TextColor;
                public Rectangle2d TextBounds;
                public StringId StringId;
                public short RenderDepthBias;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [Flags]
                public enum TextFlagsValue : uint
                {
                    LeftJustifyText = 1 << 0,
                    RightJustifyText = 1 << 1,
                    PulsatingText = 1 << 2,
                    CalloutText = 1 << 3,
                    Small31CharBuffer = 1 << 4
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                public enum CustomFontValue : short
                {
                    Terminal,
                    BodyText,
                    Title,
                    SuperLargeFont,
                    LargeBodyText,
                    SplitScreenHudMessage,
                    FullScreenHudMessage,
                    EnglishBodyText,
                    HudNumberText,
                    SubtitleFont,
                    MainMenuFont,
                    TextChatFont
                }
            }
            
            [TagStructure(Size = 0x38)]
            public class BitmapBlockReferenceBlock : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public BitmapBlendMethodValue BitmapBlendMethod;
                public short InitialSpriteFrame;
                public Point2d TopLeft;
                public float HorizTextureWrapsSecond;
                public float VertTextureWrapsSecond;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag BitmapTag;
                public short RenderDepthBias;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float SpriteAnimationSpeedFps;
                public Point2d ProgressBottomLeft;
                public StringId StringIdentifier;
                public RealVector2d ProgressScale;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    IgnoreForListSkinSizeCalculation = 1 << 0,
                    SwapOnRelativeListPosition = 1 << 1,
                    RenderAsProgressBar = 1 << 2
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                public enum BitmapBlendMethodValue : short
                {
                    Standard,
                    Multiply,
                    Unused
                }
            }
            
            [TagStructure(Size = 0x4C)]
            public class UiModelSceneReferenceBlock : TagStructure
            {
                /// <summary>
                /// Halo y-axis=ui z-axis, and Halo z-axis=ui y-axis.
                /// As a convention, let's always place objects in the ui scenario such
                /// that
                /// they are facing in the '-y' direction, and the camera such that is is
                /// facing the '+y' direction. This way the ui
                /// animation for models (which
                /// gets applied to the camera) will always be consisitent.
                /// </summary>
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public short RenderDepthBias;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<UiObjectReferenceBlock> Objects;
                public List<UiLightReferenceBlock> Lights;
                public RealVector3d AnimationScaleFactor;
                public RealPoint3d CameraPosition;
                public float FovDegress;
                public Rectangle2d UiViewport;
                public StringId UnusedIntroAnim;
                public StringId UnusedOutroAnim;
                public StringId UnusedAmbientAnim;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Unused = 1 << 0
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
                
                [TagStructure(Size = 0x20)]
                public class UiObjectReferenceBlock : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                }
                
                [TagStructure(Size = 0x20)]
                public class UiLightReferenceBlock : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class STextValuePairBlocksBlockUnused : TagStructure
            {
                /// <summary>
                /// this is all obsolete
                /// </summary>
                [TagField(Length = 32)]
                public string Name;
                public List<STextValuePairReferenceBlockUnused> TextValuePairs;
                
                [TagStructure(Size = 0x14)]
                public class STextValuePairReferenceBlockUnused : TagStructure
                {
                    /// <summary>
                    /// this is all obsolete
                    /// </summary>
                    public ValueTypeValue ValueType;
                    /// <summary>
                    /// Enter the value in the box corresponding to the value type you specified above
                    /// </summary>
                    public BooleanValueValue BooleanValue;
                    public int IntegerValue;
                    public float FpValue;
                    public StringId TextValueStringId;
                    /// <summary>
                    /// This is text string associated with data when it has the value specified above.
                    /// The string comes from the screen's string
                    /// list tag.
                    /// </summary>
                    public StringId TextLabelStringId;
                    
                    public enum ValueTypeValue : short
                    {
                        IntegerNumber,
                        FloatingPointNumber,
                        Boolean,
                        TextString
                    }
                    
                    public enum BooleanValueValue : short
                    {
                        False,
                        True
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class HudBlockReferenceBlock : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public short RenderDepthBias;
                public short StartingBitmapSequenceIndex;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag Shader;
                public Rectangle2d Bounds;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    IgnoreForListSkinSize = 1 << 0,
                    NeedsValidRank = 1 << 1
                }
                
                public enum AnimationIndexValue : short
                {
                    None,
                    _00,
                    _01,
                    _02,
                    _03,
                    _04,
                    _05,
                    _06,
                    _07,
                    _08,
                    _09,
                    _10,
                    _11,
                    _12,
                    _13,
                    _14,
                    _15,
                    _16,
                    _17,
                    _18,
                    _19,
                    _20,
                    _21,
                    _22,
                    _23,
                    _24,
                    _25,
                    _26,
                    _27,
                    _28,
                    _29,
                    _30,
                    _31,
                    _32,
                    _33,
                    _34,
                    _35,
                    _36,
                    _37,
                    _38,
                    _39,
                    _40,
                    _41,
                    _42,
                    _43,
                    _44,
                    _45,
                    _46,
                    _47,
                    _48,
                    _49,
                    _50,
                    _51,
                    _52,
                    _53,
                    _54,
                    _55,
                    _56,
                    _57,
                    _58,
                    _59,
                    _60,
                    _61,
                    _62,
                    _63
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class PlayerBlockReferenceBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "skin" })]
                public CachedTag Skin;
                public Point2d BottomLeft;
                public TableOrderValue TableOrder;
                public sbyte MaximumPlayerCount;
                public sbyte RowCount;
                public sbyte ColumnCount;
                public short RowHeight;
                public short ColumnWidth;
                
                public enum TableOrderValue : sbyte
                {
                    RowMajor,
                    ColumnMajor
                }
            }
        }
        
        public enum ShapeGroupValue : short
        {
            None,
            _00,
            _01,
            _02,
            _03,
            _04,
            _05,
            _06,
            _07,
            _08,
            _09,
            _10,
            _11,
            _12,
            _13,
            _14,
            _15,
            _16,
            _17,
            _18,
            _19,
            _20,
            _21,
            _22,
            _23,
            _24,
            _25,
            _26,
            _27,
            _28,
            _29,
            _30,
            _31
        }
        
        [TagStructure(Size = 0xC)]
        public class LocalStringIdListSectionReferenceBlock : TagStructure
        {
            public StringId SectionName;
            public List<LocalStringIdListStringReferenceBlock> LocalStringSectionReferences;
            
            [TagStructure(Size = 0x4)]
            public class LocalStringIdListStringReferenceBlock : TagStructure
            {
                public StringId StringId;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class LocalBitmapReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
        }
    }
}

