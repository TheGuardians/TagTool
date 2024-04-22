using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_shared_globals_definition", Tag = "wigl", Size = 0x1C4)]
    public class UserInterfaceSharedGlobalsDefinition : TagStructure
    {
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        /// <summary>
        /// miscellaneous rendering globals, more below...
        /// </summary>
        public float OverlayedScreenAlphaMod;
        public short IncTextUpdatePeriod; // milliseconds
        public short IncTextBlockCharacter; // ASCII code
        public float CalloutTextScale;
        public RealArgbColor ProgressBarColor;
        public float NearClipPlaneDistance; // objects closer than this are not drawn
        public float ProjectionPlaneDistance; // distance at which objects are rendered when z=0 (normal size)
        public float FarClipPlaneDistance; // objects farther than this are not drawn
        /// <summary>
        /// This is the color of the overlayed ui effect; the alpha component is the maximum opacity
        /// </summary>
        public RealArgbColor OverlayedInterfaceColor;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding8;
        /// <summary>
        /// For each error condition displayed in the UI, set the title and description string ids here
        /// </summary>
        public List<UiErrorCategoryBlock> Errors;
        /// <summary>
        /// This is the sound that plays as you tab through items
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag;
        /// <summary>
        /// This is the sound that plays when an item is selected
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag1;
        /// <summary>
        /// This is the sound that plays to alert the user that something went wrong
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag2;
        /// <summary>
        /// This is the sound that plays when advancing to a new screen
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag3;
        /// <summary>
        /// This is the sound that plays when retreating to a previous screen
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag4;
        /// <summary>
        /// This is the sound that plays when advancing past the initial login screen
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag5;
        /// <summary>
        /// This is the sound that plays when cursoring in the vkeyboard
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag6;
        /// <summary>
        /// This is the sound that plays when selecting buttons in the vkeyboard
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag7;
        /// <summary>
        /// This is the sound that plays when you receive an online notification
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag8;
        /// <summary>
        /// This is the sound that plays when tabbing thru views in a tabbed view pane (eg, online menu)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag9;
        /// <summary>
        /// This is the sound that plays as the countdown timer progresses
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag10;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Unknown;
        /// <summary>
        /// This is the sound that plays as matchmaking enters the final stage
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SoundTag11;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Unknown1;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Unknown2;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Unknown3;
        /// <summary>
        /// Sprite sequences for global ui bitmaps, as follows:
        /// 1) vkeyboard cursor
        /// 
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag GlobalBitmapsTag;
        /// <summary>
        /// Global UI Text goes here
        /// </summary>
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag UnicodeStringListTag;
        /// <summary>
        /// Animations used by screen definitions for transitions and ambient animating
        /// </summary>
        public List<AnimationReferenceBlock> ScreenAnimations;
        /// <summary>
        /// Define the various groups of shape-objects for use on any ui screens here
        /// </summary>
        public List<ShapeGroupReferenceBlock> ShapeGroups;
        /// <summary>
        /// These are the animations used by elements that live in the persistant background
        /// </summary>
        public List<PersistentBackgroundAnimationBlock> Animations;
        /// <summary>
        /// These define the visual appearances (skins) available for UI lists
        /// They are expected to be entered in the following
        /// order:
        /// 0) default
        /// 1) squad lobby player list
        /// 2) settings list
        /// 3) playlist entry list
        /// 4) variants list
        /// 5) game browser
        /// list
        /// 6) online player menu
        /// 7) game setup menu
        /// 8) playlist contents display
        /// 9) profile picker
        /// 10) mp map list
        /// 11) main
        /// menu
        /// 
        /// </summary>
        public List<ListSkinReferenceBlock> ListItemSkins;
        /// <summary>
        /// These are for specific purposes as noted
        /// </summary>
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag ButtonKeyTypeStrings;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GameTypeStrings;
        public CachedTag Unknown4;
        public List<SkillToRankMappingBlock> SkillMappings;
        /// <summary>
        /// Various settings for different sized UI windows
        /// </summary>
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
        /// Looping sound that plays while the main menu is active
        /// </summary>
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag MainMenuMusic;
        public int MusicFadeTime; // milliseconds
        
        [TagStructure(Size = 0x28)]
        public class UiErrorCategoryBlock : TagStructure
        {
            public StringId CategoryName;
            public FlagsValue Flags;
            public DefaultButtonValue DefaultButton;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag StringTag;
            public StringId DefaultTitle;
            public StringId DefaultMessage;
            public StringId DefaultOk;
            public StringId DefaultCancel;
            public List<UiErrorBlock> ErrorBlock;
            
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
            public class UiErrorBlock : TagStructure
            {
                public ErrorValue Error;
                public FlagsValue Flags;
                public DefaultButtonValue DefaultButton;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
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
        
        [TagStructure(Size = 0x2C)]
        public class AnimationReferenceBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// Defines the primary intro transitional animation
            /// </summary>
            public int AnimationPeriod; // milliseconds
            public List<ScreenAnimationKeyframeReferenceBlock> Keyframes;
            /// <summary>
            /// Defines the primary outro transitional animation
            /// </summary>
            public int AnimationPeriod1; // milliseconds
            public List<ScreenAnimationKeyframeReferenceBlock1> Keyframes1;
            /// <summary>
            /// Defines the ambient animation
            /// </summary>
            public int AnimationPeriod2; // milliseconds
            public AmbientAnimationLoopingStyleValue AmbientAnimationLoopingStyle;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ScreenAnimationKeyframeReferenceBlock2> Keyframes2;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Unused = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class ScreenAnimationKeyframeReferenceBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Alpha;
                public RealPoint3d Position;
            }
            
            [TagStructure(Size = 0x14)]
            public class ScreenAnimationKeyframeReferenceBlock1 : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
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
            
            [TagStructure(Size = 0x14)]
            public class ScreenAnimationKeyframeReferenceBlock2 : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Alpha;
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class ShapeGroupReferenceBlock : TagStructure
        {
            /// <summary>
            /// This is the old way
            /// </summary>
            public List<ShapeBlockReferenceBlock> Shapes;
            /// <summary>
            /// Specify commonly used model/light groups here
            /// </summary>
            public List<UiModelSceneReferenceBlock> ModelSceneBlocks;
            /// <summary>
            /// Specify more flavor bitmaps here
            /// </summary>
            public List<BitmapBlockReferenceBlock> BitmapBlocks;
            
            [TagStructure(Size = 0x30)]
            public class ShapeBlockReferenceBlock : TagStructure
            {
                public FlagsValue Flags;
                public AnimationIndexValue AnimationIndex;
                public short IntroAnimationDelayMilliseconds;
                public RealArgbColor Color;
                public List<PointBlockReferenceBlock> Points;
                public short RenderDepthBias;
                [TagField(Length = 0xE, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
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
                public class PointBlockReferenceBlock : TagStructure
                {
                    public Point2d Coordinates;
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
        }
        
        [TagStructure(Size = 0x10)]
        public class PersistentBackgroundAnimationBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int AnimationPeriod; // milliseconds
            public List<BackgroundAnimationKeyframeReferenceBlock> InterpolatedKeyframes;
            
            [TagStructure(Size = 0x14)]
            public class BackgroundAnimationKeyframeReferenceBlock : TagStructure
            {
                public int StartTransitionIndex;
                public float Alpha;
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class ListSkinReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "skin" })]
            public CachedTag ListItemSkins;
        }
        
        [TagStructure(Size = 0x4)]
        public class SkillToRankMappingBlock : TagStructure
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

