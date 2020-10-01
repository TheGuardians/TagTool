using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "ui_widget_definition", Tag = "DeLa", Size = 0x3EC)]
    public class UiWidgetDefinition : TagStructure
    {
        public WidgetTypeValue WidgetType;
        public ControllerIndexValue ControllerIndex;
        [TagField(Length = 32)]
        public string Name;
        public Rectangle2d Bounds;
        public FlagsValue Flags;
        /// <summary>
        /// =0 to never auto-close
        /// </summary>
        public int MillisecondsToAutoClose;
        /// <summary>
        /// = 0 for immediate close
        /// </summary>
        public int MillisecondsAutoCloseFadeTime;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BackgroundBitmap;
        /// <summary>
        /// These functions use current game data to modify the appearance of
        /// the widget. These functions are called every time the
        /// widget is
        /// rendered.
        /// </summary>
        public List<GameDataInputReferencesBlock> GameDataInputs;
        /// <summary>
        /// These allow actions to be tied to certain ui events.
        /// The event handler is run every time the widget receives the
        /// specified event.
        /// By default, the 'back' and 'B' buttons will take you to the previous screen.
        /// </summary>
        public List<EventHandlerReferencesBlock> EventHandlers;
        /// <summary>
        /// These are used to run a search-and-replace on the specified
        /// word in the text-box text, replacing all occurrences of the
        /// word
        /// with the output of the replace-function. These are invoked each
        /// time the text box is rendered (after any game data
        /// input functions
        /// have been run). The searching is case-sensitive.
        /// </summary>
        public List<SearchAndReplaceReferenceBlock> SearchAndReplaceFunctions;
        [TagField(Length = 0x80)]
        public byte[] Padding;
        /// <summary>
        /// parameters specific to text box widgets
        /// NOTE: the string list tag can also be used for lists whose items come from a
        /// string list tag
        /// </summary>
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag TextLabelUnicodeStringsList;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag TextFont;
        public RealArgbColor TextColor;
        public JustificationValue Justification;
        public Flags1Value Flags1;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// blah blah blah
        /// </summary>
        /// <summary>
        /// default is 0
        /// </summary>
        public short StringListIndex;
        /// <summary>
        /// offsets text position in its bounding area
        /// </summary>
        public short HorizOffset;
        /// <summary>
        /// offsets the text position in its bounding area
        /// </summary>
        public short VertOffset;
        [TagField(Length = 0x1A)]
        public byte[] Padding2;
        /// <summary>
        /// These options affect list items for both spinner and column lists
        /// * child widgets are used to define the visible list
        /// items
        /// * for lists with code-generated list items, the child widgets are used
        ///   as templated for visible list item
        /// placement
        /// IMPORTANT: for list widgets, the ONLY thing you can have as child widgets are the list item widgets!
        /// </summary>
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        public Flags2Value Flags2;
        /// <summary>
        /// parameters specific to spinner list widgets
        /// child widgets are the list items
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ListHeaderBitmap;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ListFooterBitmap;
        public Rectangle2d HeaderBounds;
        public Rectangle2d FooterBounds;
        [TagField(Length = 0x20)]
        public byte[] Padding4;
        /// <summary>
        /// parameters specific to column list widgets
        /// child widgets are the list items
        /// </summary>
        [TagField(ValidTags = new [] { "DeLa" })]
        public CachedTag ExtendedDescriptionWidget;
        [TagField(Length = 0x20)]
        public byte[] Padding5;
        [TagField(Length = 0x100)]
        public byte[] Padding6;
        /// <summary>
        /// use this to attach widgets that are loaded only
        /// if some internal criteria is met while processing a widget event
        /// </summary>
        public List<ConditionalWidgetReferenceBlock> ConditionalWidgets;
        [TagField(Length = 0x80)]
        public byte[] Padding7;
        [TagField(Length = 0x80)]
        public byte[] Padding8;
        /// <summary>
        /// use this to attach widgets that are loaded as 'children'
        /// of this widget (children are always loaded as part of the parent
        /// widget)
        /// </summary>
        public List<ChildWidgetReferenceBlock> ChildWidgets;
        
        public enum WidgetTypeValue : short
        {
            Container,
            TextBox,
            SpinnerList,
            ColumnList,
            GameModelNotImplemented,
            MovieNotImplemented,
            CustomNotImplemented
        }
        
        public enum ControllerIndexValue : short
        {
            Player1,
            Player2,
            Player3,
            Player4,
            AnyPlayer
        }
        
        public enum FlagsValue : uint
        {
            PassUnhandledEventsToFocusedChild,
            PauseGameTime,
            FlashBackgroundBitmap,
            DpadUpDownTabsThruChildren,
            DpadLeftRightTabsThruChildren,
            DpadUpDownTabsThruListItems,
            DpadLeftRightTabsThruListItems,
            DontFocusASpecificChildWidget,
            PassUnhandledEventsToAllChildren,
            RenderRegardlessOfControllerIndex,
            PassHandledEventsToAllChildren,
            ReturnToMainMenuIfNoHistory,
            AlwaysUseTagControllerIndex,
            AlwaysUseNiftyRenderFx,
            DonTPushHistory,
            ForceHandleMouse
        }
        
        [TagStructure(Size = 0x24)]
        public class GameDataInputReferencesBlock : TagStructure
        {
            public FunctionValue Function;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x20)]
            public byte[] Padding1;
            
            public enum FunctionValue : short
            {
                Null,
                PlayerSettingsMenuUpdateDesc,
                Unused,
                PlaylistSettingsMenuUpdateDesc,
                GametypeSelectMenuUpdateDesc,
                MultiplayerTypeMenuUpdateDesc,
                SoloLevelSelectUpdate,
                DifficultyMenuUpdateDesc,
                BuildNumberTextboxOnly,
                ServerListUpdate,
                NetworkPregameStatusUpdate,
                SplitscreenPregameStatusUpdate,
                NetSplitscreenPrejoinPlayers,
                MpProfileListUpdate,
                _3widePlayerProfileListUpdate,
                PlyrProfEditSelectMenuUpd8,
                PlayerProfileSmallMenuUpdate,
                GameSettingsListsTextUpdate,
                SoloGameObjectiveText,
                ColorPickerUpdate,
                GameSettingsListsPicUpdate,
                MainMenuFakeAnimate,
                MpLevelSelectUpdate,
                GetActivePlyrProfileName,
                GetEditPlyrProfileName,
                GetEditGameSettingsName,
                GetActivePlyrProfileColor,
                MpSetTextboxMapName,
                MpSetTextboxGameRuleset,
                MpSetTextboxTeamsNoteams,
                MpSetTextboxScoreLimit,
                MpSetTextboxScoreLimitType,
                MpSetBitmapForMap,
                MpSetBitmapForRuleset,
                /// <summary>
                /// of players
                /// </summary>
                MpSetTextbox,
                MpEditProfileSetRuleText,
                SystemLinkStatusCheck,
                MpGameDirections,
                TeamsNoTeamsBitmapUpdate,
                WarnIfDiffWillNukeSavedGame,
                DimIfNoNetCable,
                PauseGameSetTextboxInverted,
                DimUnlessTwoControllers,
                ControlsUpdateMenu,
                VideoMenuUpdate,
                GamespyScreenUpdate,
                CommonButtonBarUpdate,
                GamepadUpdateMenu,
                ServerSettingsUpdate,
                AudioMenuUpdate,
                MpProfVehiclesUpdate,
                SoloMapListUpdate,
                MpMapListUpdate,
                GtSelectListUpdate,
                GtEditListUpdate,
                LoadGameListUpdate,
                CheckingForUpdates,
                DirectIpConnectUpdate,
                NetworkSettingsUpdate
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class EventHandlerReferencesBlock : TagStructure
        {
            public FlagsValue Flags;
            public EventTypeValue EventType;
            public FunctionValue Function;
            [TagField(ValidTags = new [] { "DeLa" })]
            public CachedTag WidgetTag;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag SoundEffect;
            [TagField(Length = 32)]
            public string Script;
            
            public enum FlagsValue : uint
            {
                CloseCurrentWidget,
                CloseOtherWidget,
                CloseAllWidgets,
                OpenWidget,
                ReloadSelf,
                ReloadOtherWidget,
                GiveFocusToWidget,
                RunFunction,
                ReplaceSelfWWidget,
                GoBackToPreviousWidget,
                RunScenarioScript,
                TryToBranchOnFailure
            }
            
            public enum EventTypeValue : short
            {
                AButton,
                BButton,
                XButton,
                YButton,
                BlackButton,
                WhiteButton,
                LeftTrigger,
                RightTrigger,
                DpadUp,
                DpadDown,
                DpadLeft,
                DpadRight,
                StartButton,
                BackButton,
                LeftThumb,
                RightThumb,
                LeftAnalogStickUp,
                LeftAnalogStickDown,
                LeftAnalogStickLeft,
                LeftAnalogStickRight,
                LeftAnalogStickUp1,
                RightAnalogStickDown,
                RightAnalogStickLeft,
                RightAnalogStickRight,
                Created,
                Deleted,
                GetFocus,
                LoseFocus,
                LeftMouse,
                MiddleMouse,
                RightMouse,
                DoubleClick,
                CustomActivation,
                PostRender
            }
            
            public enum FunctionValue : short
            {
                Null,
                ListGotoNextItem,
                ListGotoPreviousItem,
                Unused,
                Unused1,
                InitializeSpLevelListSolo,
                InitializeSpLevelListCoop,
                DisposeSpLevelList,
                SoloLevelSetMap,
                SetDifficulty,
                StartNewGame,
                PauseGameRestartAtCheckpoint,
                PauseGameRestartLevel,
                PauseGameReturnToMainMenu,
                ClearMultiplayerPlayerJoins,
                JoinControllerToMpGame,
                InitializeNetGameServerList,
                StartNetworkGameServer,
                DisposeNetGameServerList,
                ShutdownNetworkGame,
                NetGameJoinFromServerList,
                SplitScreenGameInitialize,
                CoopGameInitialize,
                MainMenuIntialize,
                MpTypeMenuInitialize,
                PickPlayStageForQuickStart,
                MpLevelListInitialize,
                MpLevelListDispose,
                MpLevelSelect,
                MpProfilesListInitialize,
                MpProfilesListDispose,
                MpProfileSetForGame,
                SwapPlayerTeam,
                NetGameJoinPlayer,
                PlayerProfileListInitialize,
                PlayerProfileListDispose,
                _3widePlyrProfSetForGame,
                _1widePlyrProfSetForGame,
                MpProfileBeginEditing,
                MpProfileEndEditing,
                MpProfileSetGameEngine,
                MpProfileChangeName,
                MpProfileSetCtfRules,
                MpProfileSetKothRules,
                MpProfileSetSlayerRules,
                MpProfileSetOddballRules,
                MpProfileSetRacingRules,
                MpProfileSetPlayerOptions,
                MpProfileSetItemOptions,
                MpProfileSetIndicatorOpts,
                MpProfileInitGameEngine,
                MpProfileInitName,
                MpProfileInitCtfRules,
                MpProfileInitKothRules,
                MpProfileInitSlayerRules,
                MpProfileInitOddballRules,
                MpProfileInitRacingRules,
                MpProfileInitPlayerOpts,
                MpProfileInitItemOptions,
                MpProfileInitIndicatorOpts,
                MpProfileSaveChanges,
                ColorPickerMenuInitialize,
                ColorPickerMenuDispose,
                ColorPickerSelectColor,
                PlayerProfileBeginEditing,
                PlayerProfileEndEditing,
                PlayerProfileChangeName,
                PlayerProfileSaveChanges,
                PlyrPrfInitCntlSettings,
                PlyrPrfInitAdvCntlSet,
                PlyrPrfSaveCntlSettings,
                PlyrPrfSaveAdvCntlSet,
                MpGamePlayerQuit,
                MainMenuSwitchToSoloGame,
                RequestDelPlayerProfile,
                RequestDelPlaylistProfile,
                FinalDelPlayerProfile,
                FinalDelPlaylistProfile,
                CancelProfileDelete,
                CreateEditPlaylistProfile,
                CreateEditPlayerProfile,
                NetGameSpeedStart,
                NetGameDelayStart,
                NetServerAcceptConx,
                NetServerDeferStart,
                NetServerAllowStart,
                DisableIfNoXdemos,
                RunXdemos,
                SpResetControllerChoices,
                SpSetP1ControllerChoice,
                SpSetP2ControllerChoice,
                ErrorIfNoNetworkConnection,
                StartServerIfNoneAdvertised,
                NetGameUnjoinPlayer,
                CloseIfNotEditingProfile,
                ExitToXboxDashboard,
                NewCampaignChosen,
                NewCampaignDecision,
                PopHistoryStackOnce,
                DifficultyMenuInit,
                BeginMusicFadeOut,
                NewGameIfNoPlyrProfiles,
                ExitGracefullyToXboxDashboard,
                PauseGameInvertPitch,
                StartNewCoopGame,
                PauseGameInvertSpinnerGet,
                PauseGameInvertSpinnerSet,
                MainMenuQuitGame,
                MouseEmitAcceptEvent,
                MouseEmitBackEvent,
                MouseEmitDpadLeftEvent,
                MouseEmitDpadRightEvent,
                MouseSpinner3wideClick,
                ControlsScreenInit,
                VideoScreenInit,
                ControlsBeginBinding,
                GamespyScreenInit,
                GamespyScreenDispose,
                GamespySelectHeader,
                GamespySelectItem,
                GamespySelectButton,
                PlrProfInitMouseSet,
                PlrProfChangeMouseSet,
                PlrProfInitAudioSet,
                PlrProfChangeAudioSet,
                PlrProfChangeVideoSet,
                ControlsScreenDispose,
                ControlsScreenChangeSet,
                MouseEmitXEvent,
                GamepadScreenInit,
                GamepadScreenDispose,
                GamepadScreenChangeGamepads,
                GamepadScreenSelectItem,
                MouseScreenDefaults,
                AudioScreenDefaults,
                VideoScreenDefaults,
                ControlsScreenDefaults,
                ProfileSetEditBegin,
                ProfileManagerDelete,
                ProfileManagerSelect,
                GamespyDismissError,
                ServerSettingsInit,
                SsEditServerName,
                SsEditServerPassword,
                SsStartGame,
                VideoTestDialogInit,
                VideoTestDialogDispose,
                VideoTestDialogAccept,
                GamespyDismissFilters,
                GamespyUpdateFilterSettings,
                GamespyBackHandler,
                MouseSpinner1wideClick,
                ControlsBackHandler,
                ControlsAdvancedLaunch,
                ControlsAdvancedOk,
                MpPauseMenuOpen,
                MpGameOptionsOpen,
                MpChooseTeam,
                MpProfInitVehicleOptions,
                MpProfSaveVehicleOptions,
                SinglePrevClItemActivated,
                MpProfInitTeamplayOptions,
                MpProfSaveTeamplayOptions,
                MpGameOptionsChoose,
                EmitCustomActivationEvent,
                PlrProfCancelAudioSet,
                PlrProfInitNetworkOptions,
                PlrProfSaveNetworkOptions,
                CreditsPostRender,
                DifficultyItemSelect,
                CreditsInitialize,
                CreditsDispose,
                GamespyGetPatch,
                VideoScreenDispose,
                CampaignMenuInit,
                CampaignMenuContinue,
                LoadGameMenuInit,
                LoadGameMenuDispose,
                LoadGameMenuActivated,
                SoloMenuSaveCheckpoint,
                MpTypeSetMode,
                CheckingForUpdatesOk,
                CheckingForUpdatesDismiss,
                DirectIpConnectInit,
                DirectIpConnectGo,
                DirectIpEditField,
                NetworkSettingsEditAPort,
                NetworkSettingsDefaults,
                LoadGameMenuDeleteRequest,
                LoadGameMenuDeleteFinish
            }
        }
        
        [TagStructure(Size = 0x22)]
        public class SearchAndReplaceReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string SearchString;
            public ReplaceFunctionValue ReplaceFunction;
            
            public enum ReplaceFunctionValue : short
            {
                Null,
                WidgetSController,
                BuildNumber,
                Pid
            }
        }
        
        public enum JustificationValue : short
        {
            LeftJustify,
            RightJustify,
            CenterJustify
        }
        
        public enum Flags1Value : uint
        {
            Editable,
            Password,
            Flashing,
            DonTDoThatWeirdFocusTest
        }
        
        public enum Flags2Value : uint
        {
            ListItemsGeneratedInCode,
            ListItemsFromStringListTag,
            ListItemsOnlyOneTooltip,
            ListSinglePreviewNoScroll
        }
        
        [TagStructure(Size = 0x50)]
        public class ConditionalWidgetReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "DeLa" })]
            public CachedTag WidgetTag;
            [TagField(Length = 32)]
            public string NameUnused;
            public FlagsValue Flags;
            public short CustomControllerIndexUnused;
            [TagField(Length = 0x1A)]
            public byte[] Padding;
            
            public enum FlagsValue : uint
            {
                LoadIfEventHandlerFunctionFails
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class ChildWidgetReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "DeLa" })]
            public CachedTag WidgetTag;
            [TagField(Length = 32)]
            public string NameUnused;
            public FlagsValue Flags;
            public short CustomControllerIndex;
            public short VerticalOffset;
            public short HorizontalOffset;
            [TagField(Length = 0x16)]
            public byte[] Padding;
            
            public enum FlagsValue : uint
            {
                UseCustomControllerIndex
            }
        }
    }
}

