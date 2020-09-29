using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x28C)]
    public class UserInterfaceSharedGlobalsDefinition : TagStructure
    {
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding4;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding5;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding6;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding7;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding8;
        /// <summary>
        /// UI Rendering Globals
        /// </summary>
        /// <remarks>
        /// miscellaneous rendering globals, more below...
        /// </remarks>
        public float OverlayedScreenAlphaMod;
        public short IncTextUpdatePeriod; // milliseconds
        public short IncTextBlockCharacter; // ASCII code
        public float CalloutTextScale;
        public RealArgbColor ProgressBarColor;
        public float NearClipPlaneDistance; // objects closer than this are not drawn
        public float ProjectionPlaneDistance; // distance at which objects are rendered when z=0 (normal size)
        public float FarClipPlaneDistance; // objects farther than this are not drawn
        /// <summary>
        /// Overlayed UI Color
        /// </summary>
        /// <remarks>
        /// This is the color of the overlayed ui effect; the alpha component is the maximum opacity
        /// </remarks>
        public RealArgbColor OverlayedInterfaceColor;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding9;
        /// <summary>
        /// Displayed Errors
        /// </summary>
        /// <remarks>
        /// For each error condition displayed in the UI, set the title and description string ids here
        /// </remarks>
        public List<UiErrorCategory> Errors;
        /// <summary>
        /// Cursor Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays as you tab through items
        /// </remarks>
        public CachedTag SoundTag;
        /// <summary>
        /// Selection Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when an item is selected
        /// </remarks>
        public CachedTag SoundTag1;
        /// <summary>
        /// Error Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays to alert the user that something went wrong
        /// </remarks>
        public CachedTag SoundTag2;
        /// <summary>
        /// Advancing Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when advancing to a new screen
        /// </remarks>
        public CachedTag SoundTag3;
        /// <summary>
        /// Retreating Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when retreating to a previous screen
        /// </remarks>
        public CachedTag SoundTag4;
        /// <summary>
        /// Initial Login Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when advancing past the initial login screen
        /// </remarks>
        public CachedTag SoundTag5;
        /// <summary>
        /// VKBD Cursor Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when cursoring in the vkeyboard
        /// </remarks>
        public CachedTag SoundTag6;
        /// <summary>
        /// VKBD Character Insertion Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when selecting buttons in the vkeyboard
        /// </remarks>
        public CachedTag SoundTag7;
        /// <summary>
        /// Online Notification Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when you receive an online notification
        /// </remarks>
        public CachedTag SoundTag8;
        /// <summary>
        /// Tabbed View Pane Tabbing Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays when tabbing thru views in a tabbed view pane (eg, online menu)
        /// </remarks>
        public CachedTag SoundTag9;
        /// <summary>
        /// Pregame Countdown Timer Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays as the countdown timer progresses
        /// </remarks>
        public CachedTag SoundTag10;
        public CachedTag Unknown1;
        /// <summary>
        /// Matchmaking Advance Sound
        /// </summary>
        /// <remarks>
        /// This is the sound that plays as matchmaking enters the final stage
        /// </remarks>
        public CachedTag SoundTag11;
        public CachedTag Unknown2;
        public CachedTag Unknown3;
        public CachedTag Unknown4;
        /// <summary>
        /// Global Bitmaps
        /// </summary>
        /// <remarks>
        /// Sprite sequences for global ui bitmaps, as follows:
        /// 1) vkeyboard cursor
        /// 
        /// </remarks>
        public CachedTag GlobalBitmapsTag;
        /// <summary>
        /// Global Text Strings
        /// </summary>
        /// <remarks>
        /// Global UI Text goes here
        /// </remarks>
        public CachedTag UnicodeStringListTag;
        /// <summary>
        /// Screen Animations
        /// </summary>
        /// <remarks>
        /// Animations used by screen definitions for transitions and ambient animating
        /// </remarks>
        public List<AnimationReference> ScreenAnimations;
        /// <summary>
        /// Polygonal Shape Groups
        /// </summary>
        /// <remarks>
        /// Define the various groups of shape-objects for use on any ui screens here
        /// </remarks>
        public List<ShapeGroupReference> ShapeGroups;
        /// <summary>
        /// Persistant Background Animations
        /// </summary>
        /// <remarks>
        /// These are the animations used by elements that live in the persistant background
        /// </remarks>
        public List<PersistantAnimationReference> Animations;
        /// <summary>
        /// List Skins
        /// </summary>
        /// <remarks>
        /// These define the visual appearances (skins) available for UI lists
        /// They are expected to be entered in the following order:
        /// 0) default
        /// 1) squad lobby player list
        /// 2) settings list
        /// 3) playlist entry list
        /// 4) variants list
        /// 5) game browser list
        /// 6) online player menu
        /// 7) game setup menu
        /// 8) playlist contents display
        /// 9) profile picker
        /// 10) mp map list
        /// 11) main menu
        /// 
        /// </remarks>
        public List<UserInterfaceListSkinReference> ListItemSkins;
        /// <summary>
        /// Additional UI Strings
        /// </summary>
        /// <remarks>
        /// These are for specific purposes as noted
        /// </remarks>
        public CachedTag ButtonKeyTypeStrings;
        public CachedTag GameTypeStrings;
        public CachedTag Unknown5;
        /// <summary>
        /// Skill to rank mapping table
        /// </summary>
        public List<SkillToRankMapping> SkillMappings;
        /// <summary>
        /// WINDOW PARAMETERS
        /// </summary>
        /// <remarks>
        /// Various settings for different sized UI windows
        /// </remarks>
        public FullScreenHeaderTextFontValue FullScreenHeaderTextFont;
        public LargeDialogHeaderTextFontValue LargeDialogHeaderTextFont;
        public HalfDialogHeaderTextFontValue HalfDialogHeaderTextFont;
        public QtrDialogHeaderTextFontValue QtrDialogHeaderTextFont;
        public RealArgbColor DefaultTextColor;
        public Rectangle2d FullScreenHeaderTextBounds;
        public Rectangle2d FullScreenButtonKeyTextBounds;
        public Rectangle2d LargeDialogHeaderTextBounds;
        public Rectangle2d LargeDialogButtonKeyTextBounds;
        public Rectangle2d HalfDialogHeaderTextBounds;
        public Rectangle2d HalfDialogButtonKeyTextBounds;
        public Rectangle2d QtrDialogHeaderTextBounds;
        public Rectangle2d QtrDialogButtonKeyTextBounds;
        /// <summary>
        /// Main menu music
        /// </summary>
        /// <remarks>
        /// Looping sound that plays while the main menu is active
        /// </remarks>
        public CachedTag MainMenuMusic;
        public int MusicFadeTime; // milliseconds
        
        [TagStructure(Size = 0x34)]
        public class UiErrorCategory : TagStructure
        {
            public StringId CategoryName;
            public FlagsValue Flags;
            public DefaultButtonValue DefaultButton;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            public CachedTag StringTag;
            public StringId DefaultTitle;
            public StringId DefaultMessage;
            public StringId DefaultOk;
            public StringId DefaultCancel;
            public List<UiError> ErrorBlock;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                UseLargeDialog = 1 << 0
            }
            
            public enum DefaultButtonValue : sbyte
            {
                NoDefault,
                DefaultOk,
                DefaultCancel
            }
            
            [TagStructure(Size = 0x18)]
            public class UiError : TagStructure
            {
                public ErrorValue Error;
                public FlagsValue Flags;
                public DefaultButtonValue DefaultButton;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                public StringId Title;
                public StringId Message;
                public StringId Ok;
                public StringId Cancel;
                
                public enum ErrorValue : int
                {
                    ErrorUnknown,
                    ErrorGeneric,
                    ErrorGenericNetworking,
                    ErrorSystemLinkGenericJoinFailure,
                    ErrorSystemLinkNoNetworkConnection,
                    ErrorSystemLinkConnectionLost,
                    ErrorNetworkGameOos,
                    ErrorXboxLiveSignOutConfirmation,
                    ErrorConfirmRevertToLastSave,
                    ErrorConfirmQuitWithoutSave,
                    ErrorConfirmDeletePlayerProfile,
                    ErrorConfirmDeleteVariantFile,
                    ErrorPlayerProfileCreationFailed,
                    ErrorVariantProfileCreationFailed,
                    ErrorPlaylistCreationFailed,
                    ErrorCoreFileLoadFailed,
                    ErrorMuRemovedDuringPlayerProfileSave,
                    ErrorMuRemovedDuringVariantSave,
                    ErrorMuRemovedDuringPlaylistSave,
                    ErrorMessageSavingToMu,
                    ErrorMessageSavingFile,
                    ErrorMessageCreatingPlayerProfile,
                    ErrorMessageCreatingVariantProfile,
                    ErrorMessageSavingCheckpoint,
                    ErrorFailedToLoadPlayerProfile,
                    ErrorFailedToLoadVariant,
                    ErrorFailedToLoadPlaylist,
                    ErrorFailedToLoadSaveGame,
                    ErrorController1Removed,
                    ErrorController2Removed,
                    ErrorController3Removed,
                    ErrorController4Removed,
                    ErrorNeedMoreFreeBlocksToSave,
                    ErrorMaximumSavedGameFilesAlreadyExist,
                    ErrorDirtyDisk,
                    ErrorXbliveCannotAccessService,
                    ErrorXbliveTitleUpdateRequired,
                    ErrorXbliveServersTooBusy,
                    ErrorXbliveDuplicateLogon,
                    ErrorXbliveAccountManagementRequired,
                    ErrorWarningXbliveRecommendedMessagesAvailable,
                    ErrorXbliveInvalidMatchSession,
                    ErrorWarningXblivePoorNetworkPerformance,
                    ErrorNotEnoughOpenSlotsToJoinMatchSession,
                    ErrorXbliveCorruptDownloadContent,
                    ErrorConfirmXbliveCorruptSavedGameFileRemoval,
                    ErrorXbliveInvalidUserAccount,
                    ErrorConfirmBootClanMember,
                    ErrorConfirmControllerSignOut,
                    ErrorBetaXbliveServiceQosReport,
                    ErrorBetaFeatureDisabled,
                    ErrorBetaNetworkConnectionRequired,
                    ErrorConfirmFriendRemoval,
                    ErrorConfirmBootToDash,
                    ErrorConfirmLaunchXdemos,
                    ErrorConfirmExitGameSession,
                    ErrorXbliveConnectionToXboxLiveLost,
                    ErrorXbliveMessageSendFailure,
                    ErrorNetworkLinkLost,
                    ErrorNetworkLinkRequired,
                    ErrorXbliveInvalidPasscode,
                    ErrorJoinAborted,
                    ErrorJoinSessionNotFound,
                    ErrorJoinQosFailure,
                    ErrorJoinDataDecodeFailure,
                    ErrorJoinGameFull,
                    ErrorJoinGameClosed,
                    ErrorJoinVersionMismatch,
                    ErrorJoinFailedUnknownReason,
                    ErrorJoinFailedFriendInMatchmadeGame,
                    ErrorPlayerProfileNameMustBeUnique,
                    ErrorVariantNameMustBeUnique,
                    ErrorPlaylistNameMustBeUnique,
                    ErrorSavedFilmNameMustBeUnique,
                    ErrorNoFreeSlotsPlayerProfile,
                    ErrorNoFreeSlotsVariant,
                    ErrorNoFreeSlotsPlaylist,
                    ErrorNoFreeSlotsSavedFilm,
                    ErrorNeedMoreSpaceForPlayerProfile,
                    ErrorNeedMoreSpaceForVariant,
                    ErrorNeedMoreSpaceForPlaylist,
                    ErrorNeedMoreSpaceForSavedFilm,
                    ErrorCannotSetPrivilegesOnMemberWhoseDataNotKnown,
                    ErrorCantDeleteDefaultProfile,
                    ErrorCantDeleteDefaultVariant,
                    ErrorCantDeleteDefaultPlaylist,
                    ErrorCantDeleteDefaultSavedFilm,
                    ErrorCantDeleteProfileInUse,
                    ErrorPlayerProfileNameMustHaveAlphanumericCharacters,
                    ErrorVariantNameMustHaveAlphanumericCharacters,
                    ErrorPlaylistNameMustHaveAlphanumericCharacters,
                    ErrorSavedFilmNameMustHaveAlphanumericCharacters,
                    ErrorTeamsNotAMember,
                    ErrorTeamsInsufficientPrivileges,
                    ErrorTeamsServerBusy,
                    ErrorTeamsTeamFull,
                    ErrorTeamsMemberPending,
                    ErrorTeamsTooManyRequests,
                    ErrorTeamsUserAlreadyExists,
                    ErrorTeamsUserNotFound,
                    ErrorTeamsUserTeamsFull,
                    ErrorTeamsNoTask,
                    ErrorTeamsTooManyTeams,
                    ErrorTeamsTeamAlreadyExists,
                    ErrorTeamsTeamNotFound,
                    ErrorTeamsNameContainsBadWords,
                    ErrorTeamsDescriptionContainsBadWords,
                    ErrorTeamsMottoContainsBadWords,
                    ErrorTeamsUrlContainsBadWords,
                    ErrorTeamsNoAdmin,
                    ErrorTeamsCannotSetPrivilegesOnMemberWhoseDataNotKnown,
                    ErrorLiveUnknown,
                    ErrorConfirmDeleteProfile,
                    ErrorConfirmDeletePlaylist,
                    ErrorConfirmDeleteSavedFilm,
                    ErrorConfirmLiveSignOut,
                    ErrorConfirmConfirmFriendRemoval,
                    ErrorConfirmPromotionToSuperuser,
                    ErrorWarnNoMoreClanSuperusers,
                    ErrorConfirmCorruptProfile,
                    ErrorConfirmXboxLiveSignOut,
                    ErrorConfirmCorruptGameVariant,
                    ErrorConfirmLeaveClan,
                    ErrorConfirmCorruptPlaylist,
                    ErrorCantJoinGameinviteWithoutSignon,
                    ErrorConfirmProceedToCrossgameInvite,
                    ErrorConfirmDeclineCrossgameInvite,
                    ErrorWarnInsertCdForCrossgameInvite,
                    ErrorNeedMoreSpaceForSavedGame,
                    ErrorSavedGameCannotBeLoaded,
                    ErrorConfirmControllerSignoutWithGuests,
                    ErrorWarningPartyClosed,
                    ErrorWarningPartyRequired,
                    ErrorWarningPartyFull,
                    ErrorWarningPlayerInMmGame,
                    ErrorXbliveFailedToSignIn,
                    ErrorCantSignOutMasterWithGuests,
                    ErrorObsoleteDotCommand,
                    ErrorNotUnlocked,
                    ConfirmLeaveLobby,
                    ErrorConfirmPartyLeaderLeaveMatchmaking,
                    ErrorConfirmSingleBoxLeaveMatchmaking,
                    ErrorInvalidClanName,
                    ErrorPlayerListFull,
                    ErrorBlockedByPlayer,
                    ErrorFriendPending,
                    ErrorTooManyRequests,
                    ErrorPlayerAlreadyInList,
                    ErrorGamertagNotFound,
                    ErrorCannotMessageSelf,
                    ErrorWarningLastOverlordCantLeaveClan,
                    ErrorConfirmBootPlayer,
                    ErrorConfirmPartyMemberLeavePcr,
                    ErrorCannotSignInDuringCountdown,
                    ErrorXblInvalidUser,
                    ErrorXblUserNotAuthorized,
                    Obsolete,
                    Obsolete2,
                    ErrorXblBannedXbox,
                    ErrorXblBannedUser,
                    ErrorXblBannedTitle,
                    ErrorConfirmExitGameSessionLeader,
                    ErrorMessageObjectionableContent,
                    ErrorConfirmEnterDownloader,
                    ErrorConfirmBlockUser,
                    ErrorConfirmNegativeFeedback,
                    ErrorConfirmChangeClanMemberLevel,
                    ErrorBlankGamertag,
                    ConfirmSaveQuitGame,
                    ErrorCantJoinDuringMatchmaking,
                    ErrorConfirmRestartLevel,
                    MatchmakingFailureGeneric,
                    MatchmakingFailureMissingContent,
                    MatchmakingFailureAborted,
                    MatchmakingFailureMembershipChanged,
                    ConfirmEndGameSession,
                    ConfirmExitGameSessionOnlyPlayer,
                    ConfirmExitGameSessionXboxLiveRankedLeader,
                    ConfirmExitGameSessionXboxLiveRanked,
                    ConfirmExitGameSessionXboxLiveLeader,
                    ConfirmExitGameSessionXboxLiveOnlyPlayer,
                    ConfirmExitGameSessionXboxLive,
                    RecipientSListFull,
                    ConfirmQuitCampaignNoSave,
                    XbliveConnectionToXboxLiveLostSaveAndQuit,
                    BootedFromSession,
                    ConfirmExitGameSessionXboxLiveGuest,
                    ConfirmExitGameSessionXboxLiveRankedOnlyPlayer,
                    ConfirmExitGameSessionXboxLiveUnrankedOnlyPlayer,
                    ConfirmExitGameSessionXboxLiveUnrankedLeader,
                    ConfirmExitGameSessionXboxLiveUnranked,
                    CantJoinFriendWhileInMatchmadeGame,
                    MapLoadFailure,
                    ErrorAchievementsInterrupted,
                    ConfirmLoseProgress,
                    ErrorBetaAchievementsDisabled,
                    ErrorCannotConnectVersionsWrong,
                    ConfirmBootedFromSession,
                    ConfirmBootPlayerFromSquad,
                    ConfirmLeaveSystemLinkLobby,
                    ConfirmPartyMemberLeaveMatchmaking,
                    ConfirmQuitSinglePlayer,
                    ErrorControllerRemoved,
                    ErrorDownloadInProgress,
                    ErrorDownloadFail,
                    ErrorFailedToLoadMap,
                    ErrorFeatureRequiresGold,
                    ErrorKeyboardMapping,
                    ErrorKeyboardRemoved,
                    ErrorLiveGameUnavailable,
                    ErrorMapMissing,
                    ErrorMatchmakingFailedGeneric,
                    ErrorMatchmakingFailedMissingContent,
                    ErrorMouseRemoved,
                    ErrorPartyNotAllOnLive,
                    ErrorPartySubnetNotShared,
                    ErrorRequiredGameUpdate,
                    ErrorSavedGameCannotBeSaved,
                    ErrorSoundMicrophoneNotSupported,
                    ErrorSystemLinkDirectIp,
                    ErrorTextChatMuted,
                    ErrorTextChatParentalControls,
                    ErrorUpdateStart,
                    ErrorUpdateFail,
                    ErrorUpdateFailBlocks,
                    ErrorUpdateExists,
                    ErrorInsertOriginal,
                    ErrorUpdateFailNetworkLost,
                    ErrorUpdateMpOutOfSync,
                    ErrorUpdateMustUpgrade,
                    ErrorVoiceGoldRequired,
                    ErrorVoiceParentalControls,
                    ErrorWarningXblivePoorNetworkPerofrmance,
                    ErrorYouMissingMap,
                    ErrorSomeoneMissingMap,
                    ErrorTnpNoSource,
                    ErrorTnpDiskRead,
                    ErrorTnpNoEngineRunning,
                    ErrorTnpSignatureVerification,
                    ErrorTnpDriveRemoved,
                    ErrorTnpDiskFull,
                    ErrorTnpPermissions,
                    ErrorTnpUnknown,
                    ContinueInstall,
                    CancelInstall,
                    ErrorConfirmUpsellGold,
                    ErrorAddToFavorites,
                    ErrorRemoveFromFavorites,
                    ErrorUpdatingFavorites,
                    ChooseExistingCheckpointLocation,
                    ChooseNewCheckpointLocationCheckpointsExistOnLiveAndLocal,
                    ChooseNewCheckpointLocationCheckpointsExistOnLive,
                    ChooseNewCheckpointLocationCheckpointsExistLocally,
                    Xxx
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    UseLargeDialog = 1 << 0
                }
                
                public enum DefaultButtonValue : sbyte
                {
                    NoDefault,
                    DefaultOk,
                    DefaultCancel
                }
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class AnimationReference : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// Primary Intro Transition
            /// </summary>
            /// <remarks>
            /// Defines the primary intro transitional animation
            /// </remarks>
            public int AnimationPeriod; // milliseconds
            public List<AnimationKeyframeReference> Keyframes;
            /// <summary>
            /// Primary Outro Transition
            /// </summary>
            /// <remarks>
            /// Defines the primary outro transitional animation
            /// </remarks>
            public int AnimationPeriod1; // milliseconds
            public List<AnimationKeyframeReference> Keyframes2;
            /// <summary>
            /// Ambient Animation
            /// </summary>
            /// <remarks>
            /// Defines the ambient animation
            /// </remarks>
            public int AnimationPeriod3; // milliseconds
            public AmbientAnimationLoopingStyleValue AmbientAnimationLoopingStyle;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<AnimationKeyframeReference> Keyframes4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Unused = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class AnimationKeyframeReference : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public float Alpha;
                public RealPoint3d Position;
            }
            
            public enum AmbientAnimationLoopingStyleValue : short
            {
                None,
                ReverseLoop,
                Loop,
                DonTLoop
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ShapeGroupReference : TagStructure
        {
            /// <summary>
            /// Unused Debug Geometry Shapes
            /// </summary>
            /// <remarks>
            /// This is the old way
            /// </remarks>
            public List<ShapeBlockReference> Shapes;
            /// <summary>
            /// Model-Light Groups
            /// </summary>
            /// <remarks>
            /// Specify commonly used model/light groups here
            /// </remarks>
            public List<UiModelSceneReference> ModelSceneBlocks;
            /// <summary>
            /// Bitmaps
            /// </summary>
            /// <remarks>
            /// Specify more flavor bitmaps here
            /// </remarks>
            public List<BitmapBlockReference> BitmapBlocks;
            
            [TagStructure(Size = 0x34)]
            public class ShapeBlockReference : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public RealArgbColor Color;
                public List<PointBlockReference> Points;
                public short RenderDepthBias;
                [TagField(Flags = Padding, Length = 14)]
                public byte[] Padding1;
                
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
                
                [TagStructure(Size = 0x4)]
                public class PointBlockReference : TagStructure
                {
                    public Point2d Coordinates;
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
        }
        
        [TagStructure(Size = 0x14)]
        public class PersistantAnimationReference : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public int AnimationPeriod; // milliseconds
            public List<AnimationKeyframeReference> InterpolatedKeyframes;
            
            [TagStructure(Size = 0x14)]
            public class AnimationKeyframeReference : TagStructure
            {
                public int StartTransitionIndex;
                public float Alpha;
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class UserInterfaceListSkinReference : TagStructure
        {
            public CachedTag ListItemSkins;
        }
        
        [TagStructure(Size = 0x4)]
        public class SkillToRankMapping : TagStructure
        {
            public Bounds<short> SkillBounds;
        }
        
        public enum FullScreenHeaderTextFontValue : short
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
        
        public enum LargeDialogHeaderTextFontValue : short
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
        
        public enum HalfDialogHeaderTextFontValue : short
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
        
        public enum QtrDialogHeaderTextFontValue : short
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
}

