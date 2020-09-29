using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_screen_widget_definition", Tag = "wgit", Size = 0x8C)]
    public class UserInterfaceScreenWidgetDefinition : TagStructure
    {
        /// <summary>
        /// Notes on screen widgets:
        /// </summary>
        /// <remarks>
        /// - the widget coordinate system is a left-handed system (+x to the right, +y up, +z into the screen)
        ///   with the origin centered in the display (regardless of display size)
        /// - for widget component placement, all coordinates you define in the tag specifiy the object's
        ///   placement prior to the application of any animation
        /// - all coordinates you define are local to that object
        /// - all text specific to objects in the screen comes from the screen's string list tag
        ///   all of the string indices you may need to specify will refer to the screen's string list tag
        /// - a pane may contain either buttons OR a list OR a table-view, but never a combination of those
        ///   (widget won't function correctly if you try that)
        /// - all text is centered unless you specify otherwise
        /// </remarks>
        /// <summary>
        /// Flags
        /// </summary>
        /// <remarks>
        /// Set misc. screen behavior here
        /// </remarks>
        public FlagsValue Flags;
        public ScreenIdValue ScreenId;
        /// <summary>
        /// Button Key
        /// </summary>
        /// <remarks>
        /// The labels here are just a guide; the actual string used comes from the Nth position
        /// of this button key entry as found in the ui globals button key string list tag
        /// </remarks>
        public ButtonKeyTypeValue ButtonKeyType;
        /// <summary>
        /// Default Text Color
        /// </summary>
        /// <remarks>
        /// Any ui elements that don't explicitly set a text color will use this color
        /// </remarks>
        public RealArgbColor TextColor;
        /// <summary>
        /// Screen Text
        /// </summary>
        /// <remarks>
        /// All text specific to this screen
        /// </remarks>
        public CachedTag StringListTag;
        /// <summary>
        /// Panes
        /// </summary>
        /// <remarks>
        /// Define the screen's panes here (normal screens have 1 pane, tab-view screens have 2+ panes)
        /// </remarks>
        public List<WindowPaneReference> Panes;
        /// <summary>
        /// Shape Group
        /// </summary>
        /// <remarks>
        /// If the screen is to have a shape group, specify it here (references group in user interface globals tag)
        /// </remarks>
        public ShapeGroupValue ShapeGroup;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        /// <summary>
        /// More Screen Parameters
        /// </summary>
        /// <remarks>
        /// These are down here because they got added on later. Have a nice day.
        /// </remarks>
        public StringId HeaderStringId;
        /// <summary>
        /// Local strings
        /// </summary>
        /// <remarks>
        /// String IDs here allow defining new string ids that are visible only to this screen.
        /// </remarks>
        public List<LocalStringIdListSectionReference> LocalStrings;
        /// <summary>
        /// Local bitmaps
        /// </summary>
        /// <remarks>
        /// Bitmaps here allow adding a bitmap that's accessible in code for use in this screen.
        /// </remarks>
        public List<LocalBitmapReference> LocalBitmaps;
        /// <summary>
        /// LEVEL LOAD PROGRESS FIELDS
        /// </summary>
        /// <remarks>
        /// These are used only for level load progress bitmaps
        /// </remarks>
        public RealRgbColor SourceColor;
        public RealRgbColor DestinationColor;
        public float AccumulateZoomScaleX;
        public float AccumulateZoomScaleY;
        public float RefractionScaleX;
        public float RefractionScaleY;
        /// <summary>
        /// Mouse cursors
        /// </summary>
        /// <remarks>
        /// The mouse cursor definition for this screen.
        /// </remarks>
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
            Test,
            Test0,
            Test1,
            Test2,
            Test3,
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
            CustomGameMenu4,
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
        
        [TagStructure(Size = 0x70)]
        public class WindowPaneReference : TagStructure
        {
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public AnimationIndexValue AnimationIndex;
            /// <summary>
            /// Button Definitions
            /// </summary>
            /// <remarks>
            /// If the pane contains buttons, define them here
            /// </remarks>
            public List<ButtonWidgetReference> Buttons;
            /// <summary>
            /// List Definition
            /// </summary>
            /// <remarks>
            /// If the pane contains a list, define it here
            /// </remarks>
            public List<ListReference> ListBlock;
            /// <summary>
            /// OBSOLETE Table View Definition
            /// </summary>
            /// <remarks>
            /// If the pane contains a table-view, define it here
            /// </remarks>
            public List<TableViewListReferenceObsolete> TableView;
            /// <summary>
            /// Flavor Item Blocks
            /// </summary>
            /// <remarks>
            /// Define additional flavor items here
            /// </remarks>
            public List<TextBlockReference> TextBlocks;
            public List<BitmapBlockReference> BitmapBlocks;
            public List<UiModelSceneReference> ModelSceneBlocks;
            /// <summary>
            /// UNUSED
            /// </summary>
            /// <remarks>
            /// these are all OBSOLETE
            /// </remarks>
            public List<TextValuePairBlockUnused> TextValueBlocks;
            public List<HudBlockReference> HudBlocks;
            public List<PlayerBlockReference> PlayerBlocks;
            
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
            
            [TagStructure(Size = 0x44)]
            public class ButtonWidgetReference : TagStructure
            {
                public TextFlagsValue TextFlags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public CustomFontValue CustomFont;
                public RealArgbColor TextColor;
                public Rectangle2d Bounds;
                public CachedTag Bitmap;
                public Point2d BitmapOffset; // from top-left
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
            
            [TagStructure(Size = 0x1C)]
            public class ListReference : TagStructure
            {
                public FlagsValue Flags;
                public SkinIndexValue SkinIndex;
                public short NumVisibleItems;
                public Point2d BottomLeft;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                /// <summary>
                /// UNUSED
                /// </summary>
                /// <remarks>
                /// This is unused
                /// </remarks>
                public List<TextValuePairReferenceUnused> Unused;
                
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
                public class TextValuePairReferenceUnused : TagStructure
                {
                    /// <summary>
                    /// OBSOLETE
                    /// </summary>
                    /// <remarks>
                    /// this is all obsolete
                    /// </remarks>
                    public ValueTypeValue ValueType;
                    /// <summary>
                    /// Value
                    /// </summary>
                    /// <remarks>
                    /// Enter the value in the box corresponding to the value type you specified above
                    /// </remarks>
                    public BooleanValueValue BooleanValue;
                    public int IntegerValue;
                    public float FpValue;
                    public StringId TextValueStringId;
                    /// <summary>
                    /// Text Label
                    /// </summary>
                    /// <remarks>
                    /// This is text string associated with data when it has the value specified above.
                    /// The string comes from the screen's string list tag.
                    /// </remarks>
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
            
            [TagStructure(Size = 0x2C)]
            public class TableViewListReferenceObsolete : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public CustomFontValue CustomFont;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public RealArgbColor TextColor;
                public Point2d TopLeft;
                public List<TableViewListRowReferenceObsolete> TableRows;
                
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
                
                [TagStructure(Size = 0x14)]
                public class TableViewListRowReferenceObsolete : TagStructure
                {
                    public FlagsValue Flags;
                    public short RowHeight;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public List<TableViewListCellReferenceObsolete> RowCells;
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Unused = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x24)]
                    public class TableViewListCellReferenceObsolete : TagStructure
                    {
                        public TextFlagsValue TextFlags;
                        public short CellWidth;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding1;
                        public Point2d BitmapTopLeft; // if there is a bitmap
                        public CachedTag BitmapTag;
                        public StringId StringId;
                        public short RenderDepthBias;
                        [TagField(Flags = Padding, Length = 2)]
                        public byte[] Padding2;
                        
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
            public class TextBlockReference : TagStructure
            {
                public TextFlagsValue TextFlags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public CustomFontValue CustomFont;
                public RealArgbColor TextColor;
                public Rectangle2d TextBounds;
                public StringId StringId;
                public short RenderDepthBias;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                
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
            
            [TagStructure(Size = 0x40)]
            public class BitmapBlockReference : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public BitmapBlendMethodValue BitmapBlendMethod;
                public short InitialSpriteFrame;
                public Point2d TopLeft;
                public float HorizTextureWrapsSecond;
                public float VertTextureWrapsSecond;
                public CachedTag BitmapTag;
                public short RenderDepthBias;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            
            [TagStructure(Size = 0x54)]
            public class UiModelSceneReference : TagStructure
            {
                /// <summary>
                /// NOTE on coordinate systems
                /// </summary>
                /// <remarks>
                /// Halo y-axis=ui z-axis, and Halo z-axis=ui y-axis.
                /// As a convention, let's always place objects in the ui scenario such that
                /// they are facing in the '-y' direction, and the camera such that is is
                /// facing the '+y' direction. This way the ui animation for models (which
                /// gets applied to the camera) will always be consisitent.
                /// </remarks>
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public short RenderDepthBias;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public List<UiObjectReference> Objects;
                public List<UiLightReference> Lights;
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
                public class UiObjectReference : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                }
                
                [TagStructure(Size = 0x20)]
                public class UiLightReference : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class TextValuePairBlockUnused : TagStructure
            {
                /// <summary>
                /// OBSOLETE
                /// </summary>
                /// <remarks>
                /// this is all obsolete
                /// </remarks>
                [TagField(Length = 32)]
                public string Name;
                public List<TextValuePairReferenceUnused> TextValuePairs;
                
                [TagStructure(Size = 0x14)]
                public class TextValuePairReferenceUnused : TagStructure
                {
                    /// <summary>
                    /// OBSOLETE
                    /// </summary>
                    /// <remarks>
                    /// this is all obsolete
                    /// </remarks>
                    public ValueTypeValue ValueType;
                    /// <summary>
                    /// Value
                    /// </summary>
                    /// <remarks>
                    /// Enter the value in the box corresponding to the value type you specified above
                    /// </remarks>
                    public BooleanValueValue BooleanValue;
                    public int IntegerValue;
                    public float FpValue;
                    public StringId TextValueStringId;
                    /// <summary>
                    /// Text Label
                    /// </summary>
                    /// <remarks>
                    /// This is text string associated with data when it has the value specified above.
                    /// The string comes from the screen's string list tag.
                    /// </remarks>
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
            
            [TagStructure(Size = 0x34)]
            public class HudBlockReference : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public short RenderDepthBias;
                public short StartingBitmapSequenceIndex;
                public CachedTag Bitmap;
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
            
            [TagStructure(Size = 0x20)]
            public class PlayerBlockReference : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
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
        
        [TagStructure(Size = 0x10)]
        public class LocalStringIdListSectionReference : TagStructure
        {
            public StringId SectionName;
            public List<LocalStringIdListReference> LocalStringSectionReferences;
            
            [TagStructure(Size = 0x4)]
            public class LocalStringIdListReference : TagStructure
            {
                public StringId StringId;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class LocalBitmapReference : TagStructure
        {
            public CachedTag Bitmap;
        }
    }
}

