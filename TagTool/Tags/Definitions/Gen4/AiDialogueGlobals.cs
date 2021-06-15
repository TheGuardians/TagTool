using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x7C)]
    public class AiDialogueGlobals : TagStructure
    {
        public Bounds<float> StrikeDelayBounds; // secs
        public float RemindDelay; // secs
        public float CoverCurseChance;
        // defaults to 10 wu
        public float PlayerLookMaxDistance; // wu
        // defaults to 3 secs
        public float PlayerLook; // secs
        // defaults to 15 secs
        public float PlayerLookLongTime; // secs
        // defaults to 7 wu
        public float SpartanNearbySearchDistance; // wu
        // 0
        public float FaceFriendlyPlayerDistance; // wu
        // used for dialog lines started by a pattern with "speaker in space" set
        public StringId SpaceDialogueEffect;
        public List<DefaultStimulusSuppressorBlockStruct> DefaultStimulusSuppressors;
        public List<VocalizationDefinitionsBlock> Vocalizations;
        public List<VocalizationPatternsBlock> Patterns;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<DialogueDataBlock> DialogueData;
        public List<InvoluntaryDataBlock> InvoluntaryData;
        public List<PredictedDataBlock> PredictedVocalizations;
        
        [TagStructure(Size = 0x4)]
        public class DefaultStimulusSuppressorBlockStruct : TagStructure
        {
            public StringId Stimulus;
        }
        
        [TagStructure(Size = 0x64)]
        public class VocalizationDefinitionsBlock : TagStructure
        {
            public StringId Vocalization;
            public short ParentIndex;
            public PriorityEnum Priority;
            public VocalizationFlags Flags;
            // how does the speaker of this vocalization direct his gaze?
            public GlanceTypeEnum GlanceBehavior;
            // how does someone who hears me behave?
            public GlanceTypeEnum GlanceRecipientBehavior;
            public PerceptionTypeEnum PerceptionType;
            public CombatStatusEnum MaxCombatStatus;
            public DialogueAnimationEnum AnimationImpulse;
            public short ProxyDialogueIndex;
            // Minimum delay time between playing the same permutation
            public float SoundRepetitionDelay; // minutes
            // How long to wait to actually start the vocalization
            public float AllowableQueueDelay; // seconds
            // How long to wait to actually start the vocalization
            public float PreVocDelay; // seconds
            // How long into the vocalization the AI should be notified
            public float NotificationDelay; // seconds
            // How long speech is suppressed in the speaking unit after vocalizing
            public float PostVocDelay; // seconds
            // How long before the same vocalization can be repeated
            public float RepeatDelay; // seconds
            // Inherent weight of this vocalization
            public float Weight; // [0-1]
            // speaker won't move for the given amount of time
            public float SpeakerFreezeTime;
            // listener won't move for the given amount of time (from start of vocalization)
            public float ListenerFreezeTime;
            public DialogueEmotionEnum SpeakerEmotion;
            public DialogueEmotionEnum ListenerEmotion;
            public float PlayerSpeakerSkipFraction;
            public float PlayerSkipFraction;
            public float FloodSkipFraction;
            public float SkipFraction;
            // The lowest mission id that we play this line in
            public short MissionMinValue;
            // The highest mission id that we play this line in
            public short MissionMaxValue;
            public StringId SampleLine;
            public List<ResponseBlock> Reponses;
            
            public enum PriorityEnum : short
            {
                None,
                Recall,
                Idle,
                Comment,
                IdleResponse,
                Postcombat,
                Combat,
                Status,
                Respond,
                Warn,
                Act,
                React,
                Involuntary,
                Scream,
                Scripted,
                Death
            }
            
            [Flags]
            public enum VocalizationFlags : uint
            {
                Immediate = 1 << 0,
                Interrupt = 1 << 1,
                CancelLowPriority = 1 << 2,
                DisableDialogueEffect = 1 << 3,
                PredictFacialAnimations = 1 << 4
            }
            
            public enum GlanceTypeEnum : short
            {
                None,
                GlanceSubjectShort,
                GlanceSubjectLong,
                GlanceCauseShort,
                GlanceCauseLong,
                GlanceFriendShort,
                GlanceFriendLong
            }
            
            public enum PerceptionTypeEnum : short
            {
                None,
                Speaker,
                Listener
            }
            
            public enum CombatStatusEnum : short
            {
                Asleep,
                Idle,
                Alert,
                Active,
                Uninspected,
                Definite,
                Certain,
                Visible,
                ClearLos,
                Dangerous
            }
            
            public enum DialogueAnimationEnum : short
            {
                None,
                Shakefist,
                Cheer,
                SurpriseFront,
                SurpriseBack,
                Taunt,
                Brace,
                Point,
                Hold,
                Wave,
                Advance,
                Fallback
            }
            
            public enum DialogueEmotionEnum : short
            {
                None,
                Happy,
                Sad,
                Angry,
                Disgusted,
                Scared,
                Surprised,
                Pain,
                Shout
            }
            
            [TagStructure(Size = 0xC)]
            public class ResponseBlock : TagStructure
            {
                public StringId VocalizationName;
                public ResponseFlags Flags;
                public short VocalizationIndex;
                public ResponseTypeEnum ResponseType;
                public short DialogueIndex;
                
                [Flags]
                public enum ResponseFlags : ushort
                {
                    Nonexclusive = 1 << 0,
                    TriggerResponse = 1 << 1
                }
                
                public enum ResponseTypeEnum : short
                {
                    Friend,
                    Enemy,
                    Listener,
                    Joint,
                    Peer,
                    Leader,
                    FriendInfantry
                }
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class VocalizationPatternsBlock : TagStructure
        {
            public DialogueNamesEnum DialogueType;
            public short VocalizationIndex;
            public StringId VocalizationName;
            public SpeakerTypeEnum SpeakerType;
            // who/what am I speaking to/of?
            public SpeakerTypeEnum ListenerTarget;
            // The relationship between the subject and the cause
            public HostilityEnum Hostility;
            public PatternFlags Flags;
            public ActorTypeEnum CauseActorType;
            public DialogueObjectTypesEnum CauseType;
            public StringId CauseAiTypeName;
            public StringId CauseEquipmentTypeName;
            public DialogueObjectTypesEnum SpeakerObjectType;
            public SpeakerBehaviorEnum SpeakerBehavior;
            // Speaker must have danger level of at least this much
            public DangerEnum DangerLevel;
            public SpatialRelationEnum SpeakerSubjectPosition;
            public SpatialRelationEnum SpeakerCausePosition;
            public DialogueConditionFlags Conditions;
            // with respect to the subject, the cause is ...
            public SpatialRelationEnum1 SpatialRelation;
            public DamageEnum DamageType;
            public GameTypeEnum GameType;
            public ActorTypeEnum SubjectActorType;
            public DialogueObjectTypesEnum SubjectType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId SubjectAiTypeName;
            
            public enum DialogueNamesEnum : short
            {
                None,
                Death,
                DeathHeadshot,
                DeathAssassination,
                Damage,
                Anounce,
                SightedInterest,
                SightedNew,
                SightedNewMajor,
                SightedFirst,
                SightedSpecial,
                HeardNew,
                HeardOld,
                FoundUnit,
                FoundUnitPursuit,
                ThrowingGrenade,
                ThrowingGrenadeAll,
                StuckGrenade,
                Fighting,
                SuppressingFire,
                GrenadeUncover,
                Jump,
                Reload,
                ReloadLowAmmo,
                ReadySpartanLaser,
                ReadyRocketLauncher,
                ReadyFlakCannon,
                ReadyPlasmaLauncher,
                Surprised,
                LostContact,
                InvestigateFailed,
                PursuitFailed,
                InvestigateStart,
                InvestigateInterest,
                Searching,
                AbandonedSearchSpace,
                AbandonedSearchTime,
                PresearchFailed,
                AbandonedSearchRestricted,
                PursuitStart,
                PostcombatInspectBody,
                VehicleSlowDown,
                VehicleGetIn,
                Idle,
                CombatIdle,
                Taunt,
                TauntReply,
                Retreat,
                RetreatFromScaryTarget,
                RetreatFromDeadLeader,
                RetreatFromProximity,
                RetreatFromLowShield,
                Flee,
                Cowering,
                MeleeCharge,
                MeleeAttack,
                VehicleFalling,
                VehicleWoohoo,
                VehicleScared,
                VehicleCrazy,
                Leap,
                PostcombatWin,
                PostcombatLose,
                PostcombatNeutral,
                ShootCorpse,
                PostcombatStart,
                InspectBodyStart,
                PostcombatStatus,
                PostcombatLastStanding,
                VehicleEntryStartDriver,
                VehicleEnter,
                VehicleEntryStartGun,
                VehicleEntryStartPassenger,
                VehicleExit,
                EvictDriver,
                EvictGunner,
                EvictPassenger,
                NewOrderEnemyAdvancing,
                NewOrderEnemyCharging,
                NewOrderEnemyFallingback,
                NewOrderAdvance,
                NewOrderCharge,
                NewOrderFallback,
                NewOrderMoveon,
                NewOrderFllplr,
                NewOrderArriveCombat,
                NewOrderEndCombat,
                NewOrderInvestigate,
                NewOrderSpread,
                NewOrderHold,
                NewOrderFindCover,
                NewOrderCoveringFire,
                OrderAckPositive,
                OrderAckNegative,
                OrderAckCanceled,
                OrderAckCompleted,
                OrderAckRegroup,
                OrderAckDisband,
                OrderAckWeaponChange,
                OrderAckAttackVehicle,
                OrderAckAttackInfantry,
                OrderAckInteract,
                OrderAckPinnedDown,
                FireteamMemberJoin,
                FireteamMemberDanger,
                FireteamMemberDied,
                Emerge,
                Curse,
                Threaten,
                CoverFriend,
                MoveCover,
                InCover,
                PinnedDown,
                Strike,
                OpenFire,
                Shoot,
                ShootMultiple,
                ShootGunner,
                Gloat,
                Greet,
                PlayerLook,
                PlayerLookLongtime,
                PanicGrenadeAttached,
                PanicVehicleDestroyed,
                HelpResponse,
                Remind,
                Overheated,
                WeaponTradeBetter,
                WeaponTradeWorse,
                WeaponTradeEqual,
                Betray,
                Forgive,
                WarnTarget,
                WarnPursuit,
                UseEquipment,
                Ambush,
                UndrFire,
                UndrFireTrrt,
                FloodBoom,
                VehicleBoom,
                Berserk,
                Stealth,
                Infection,
                Reanimate,
                Scold,
                Praise,
                Scorn,
                Plead,
                Thank,
                Ok,
                Cheer,
                InviteVehicle,
                InviteVehicleDriver,
                InviteVehicleGunner,
                PlayerBlocking,
                PlayerMultiKill,
                AdvanceStart,
                HamstringCharge
            }
            
            public enum SpeakerTypeEnum : short
            {
                Subject,
                Cause,
                Friend,
                Target,
                Enemy,
                Vehicle,
                Joint,
                Task,
                Leader,
                JointLeader,
                Clump,
                Peer,
                None
            }
            
            public enum HostilityEnum : short
            {
                None,
                Self,
                Neutral,
                Friend,
                Enemy,
                Traitor
            }
            
            [Flags]
            public enum PatternFlags : ushort
            {
                SubjectVisible = 1 << 0,
                CauseVisible = 1 << 1,
                FriendsPresent = 1 << 2,
                SubjectIsSpeakerSTarget = 1 << 3,
                CauseIsSpeakerSTarget = 1 << 4,
                CauseIsPlayerOrSpeakerIsPlayerAlly = 1 << 5,
                CauseIsPrimaryPlayerAlly = 1 << 6,
                CauseIsInfantry = 1 << 7,
                SubjectIsInfantry = 1 << 8,
                SpeakerIsInfantry = 1 << 9,
                SpeakerInSpace = 1 << 10,
                SpeakerHasLowHealth = 1 << 11,
                CauseIsTargetingPlayer = 1 << 12
            }
            
            public enum ActorTypeEnum : short
            {
                None,
                Player,
                Marine,
                Crew,
                Spartan,
                Elite,
                Jackal,
                Grunt,
                Brute,
                Hunter,
                Prophet,
                Bugger,
                Scarab,
                Engineer,
                Skirmisher,
                Bishop,
                Knight,
                Pawn,
                Rook,
                Mule,
                MountedWeapon,
                Octopus
            }
            
            public enum DialogueObjectTypesEnum : short
            {
                None,
                Player,
                Actor,
                Biped,
                Body,
                Vehicle,
                Projectile,
                ActorOrPlayer,
                Turret,
                UnitInVehicle,
                UnitInTurret,
                UnitCarryingTurret,
                Driver,
                Gunner,
                Passenger,
                Postcombat,
                PostcombatWon,
                PostcombatLost,
                PlayerMasterchief,
                PlayerSpartans,
                PlayerDervish,
                Heretic,
                MajorlyScary,
                LastManInVehicle,
                Male,
                Female,
                Grenade,
                Stealth,
                Flood,
                Pureform,
                PlayerEmptyVehicle,
                Equipment
            }
            
            public enum SpeakerBehaviorEnum : short
            {
                Any,
                Combat,
                Engage,
                Search,
                Cover,
                Retreat,
                Follow,
                Shoot,
                ClumpIdle,
                ClumpCombat,
                FoughtBrutes,
                FoughtFlood
            }
            
            public enum DangerEnum : short
            {
                None,
                BroadlyFacing,
                ShootingNear,
                ShootingAt,
                ExtremelyClose,
                ShieldDamage,
                ShieldExtendedDamage,
                BodyDamage,
                BodyExtendedDamage
            }
            
            public enum SpatialRelationEnum : sbyte
            {
                None,
                VeryNear,
                Near,
                MediumRange,
                Far,
                VeryFar,
                InFrontOf,
                Behind,
                Above,
                Below,
                Few,
                InRange
            }
            
            [Flags]
            public enum DialogueConditionFlags : uint
            {
                Asleep = 1 << 0,
                Idle = 1 << 1,
                Alert = 1 << 2,
                Active = 1 << 3,
                UninspectedOrphan = 1 << 4,
                DefiniteOrphan = 1 << 5,
                CertainOrphan = 1 << 6,
                VisibleEnemy = 1 << 7,
                ClearLosEnemy = 1 << 8,
                DangerousEnemy = 1 << 9,
                NoVehicle = 1 << 10,
                VehicleDriver = 1 << 11,
                VehiclePassenger = 1 << 12
            }
            
            public enum SpatialRelationEnum1 : short
            {
                None,
                VeryNear,
                Near,
                MediumRange,
                Far,
                VeryFar,
                InFrontOf,
                Behind,
                Above,
                Below,
                Few,
                InRange
            }
            
            public enum DamageEnum : short
            {
                None,
                Falling,
                Bullet,
                Grenade,
                Explosive,
                Sniper,
                Melee,
                Flame,
                MountedWeapon,
                Vehicle,
                Plasma,
                Needle,
                Shotgun,
                Assassination
            }
            
            public enum GameTypeEnum : short
            {
                None,
                Sandbox,
                Megalo,
                Campaign,
                Survival,
                Firefight
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class DialogueDataBlock : TagStructure
        {
            public short StartIndex;
            public short Length;
        }
        
        [TagStructure(Size = 0x4)]
        public class InvoluntaryDataBlock : TagStructure
        {
            public short InvoluntaryVocalizationIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x4)]
        public class PredictedDataBlock : TagStructure
        {
            public int PredictedVocalizationIndex;
        }
    }
}
