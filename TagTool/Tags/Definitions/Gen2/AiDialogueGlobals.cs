using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x3C)]
    public class AiDialogueGlobals : TagStructure
    {
        public List<Vocalization> Vocalizations;
        public List<VocalizationPattern> Patterns;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding1;
        public List<DialogueDataBlock> DialogueData;
        public List<InvoluntaryDataBlock> InvoluntaryData;
        
        [TagStructure(Size = 0x68)]
        public class Vocalization : TagStructure
        {
            public StringId VocalizationVal;
            public StringId ParentVocalization;
            public short ParentIndex;
            public PriorityValue Priority;
            public FlagsValue Flags;
            public GlanceBehaviorValue GlanceBehavior; // how does the speaker of this vocalization direct his gaze?
            public GlanceRecipientBehaviorValue GlanceRecipientBehavior; // how does someone who hears me behave?
            public PerceptionTypeValue PerceptionType;
            public MaxCombatStatusValue MaxCombatStatus;
            public AnimationImpulseValue AnimationImpulse;
            public OverlapPriorityValue OverlapPriority;
            public float SoundRepetitionDelay; // minutes
            public float AllowableQueueDelay; // seconds
            public float PreVocDelay; // seconds
            public float NotificationDelay; // seconds
            public float PostVocDelay; // seconds
            public float RepeatDelay; // seconds
            public float Weight; // [0-1]
            public float SpeakerFreezeTime; // speaker won't move for the given amount of time
            public float ListenerFreezeTime; // listener won't move for the given amount of time (from start of vocalization)
            public SpeakerEmotionValue SpeakerEmotion;
            public ListenerEmotionValue ListenerEmotion;
            public float PlayerSkipFraction;
            public float SkipFraction;
            public StringId SampleLine;
            public List<Response> Reponses;
            public List<Vocalization> Children;
            
            public enum PriorityValue : short
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
            public enum FlagsValue : uint
            {
                Immediate = 1 << 0,
                Interrupt = 1 << 1,
                CancelLowPriority = 1 << 2
            }
            
            public enum GlanceBehaviorValue : short
            {
                None,
                GlanceSubjectShort,
                GlanceSubjectLong,
                GlanceCauseShort,
                GlanceCauseLong,
                GlanceFriendShort,
                GlanceFriendLong
            }
            
            public enum GlanceRecipientBehaviorValue : short
            {
                None,
                GlanceSubjectShort,
                GlanceSubjectLong,
                GlanceCauseShort,
                GlanceCauseLong,
                GlanceFriendShort,
                GlanceFriendLong
            }
            
            public enum PerceptionTypeValue : short
            {
                None,
                Speaker,
                Listener
            }
            
            public enum MaxCombatStatusValue : short
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
            
            public enum AnimationImpulseValue : short
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
            
            public enum OverlapPriorityValue : short
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
            
            public enum SpeakerEmotionValue : short
            {
                None,
                Asleep,
                Amorous,
                Happy,
                Inquisitive,
                Repulsed,
                Disappointed,
                Shocked,
                Scared,
                Arrogant,
                Annoyed,
                Angry,
                Pensive,
                Pain
            }
            
            public enum ListenerEmotionValue : short
            {
                None,
                Asleep,
                Amorous,
                Happy,
                Inquisitive,
                Repulsed,
                Disappointed,
                Shocked,
                Scared,
                Arrogant,
                Annoyed,
                Angry,
                Pensive,
                Pain
            }
            
            [TagStructure(Size = 0xC)]
            public class Response : TagStructure
            {
                public StringId VocalizationName;
                public FlagsValue Flags;
                public short VocalizationIndexPostProcess;
                public ResponseTypeValue ResponseType;
                public short DialogueIndexImport;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Nonexclusive = 1 << 0,
                    TriggerResponse = 1 << 1
                }
                
                public enum ResponseTypeValue : short
                {
                    Friend,
                    Enemy,
                    Listener,
                    Joint,
                    Peer
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class VocalizationPattern : TagStructure
        {
            public DialogueTypeValue DialogueType;
            public short VocalizationIndex;
            public StringId VocalizationName;
            public SpeakerTypeValue SpeakerType;
            public FlagsValue Flags;
            public ListenerTargetValue ListenerTarget; // who/what am I speaking to/of?
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public HostilityValue Hostility; // The relationship between the subject and the cause
            public DamageTypeValue DamageType;
            public DangerLevelValue DangerLevel; // Speaker must have danger level of at least this much
            public AttitudeValue Attitude;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public SubjectActorTypeValue SubjectActorType;
            public CauseActorTypeValue CauseActorType;
            public CauseTypeValue CauseType;
            public SubjectTypeValue SubjectType;
            public StringId CauseAiTypeName;
            public SpatialRelationValue SpatialRelation; // with respect to the subject, the cause is ...
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            public StringId SubjectAiTypeName;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding5;
            public ConditionsValue Conditions;
            
            public enum DialogueTypeValue : short
            {
                Death,
                Unused,
                Unused0,
                Damage,
                DamageUnused1,
                DamageUnused2,
                SightedNew,
                SightedNewMajor,
                Unused1,
                SightedOld,
                SightedFirst,
                SightedSpecial,
                Unused2,
                HeardNew,
                Unused3,
                HeardOld,
                Unused4,
                Unused5,
                Unused6,
                AcknowledgeMultiple,
                Unused7,
                Unused8,
                Unused9,
                FoundUnit,
                FoundUnitPresearch,
                FoundUnitPursuit,
                FoundUnitSelfPreserving,
                FoundUnitRetreating,
                ThrowingGrenade,
                NoticedGrenade,
                Fighting,
                Charging,
                SuppressingFire,
                GrenadeUncover,
                Unused10,
                Unused11,
                Dive,
                Evade,
                Avoid,
                Surprised,
                Unused12,
                Unused13,
                Presearch,
                PresearchStart,
                Search,
                SearchStart,
                InvestigateFailed,
                UncoverFailed,
                PursuitFailed,
                InvestigateStart,
                AbandonedSearchSpace,
                AbandonedSearchTime,
                PresearchFailed,
                AbandonedSearchRestricted,
                InvestigatePursuitStart,
                PostcombatInspectBody,
                VehicleSlowDown,
                VehicleGetIn,
                Idle,
                Taunt,
                TauntReply,
                Retreat,
                RetreatFromScaryTarget,
                RetreatFromDeadLeader,
                RetreatFromProximity,
                RetreatFromLowShield,
                Flee,
                Cowering,
                Unused14,
                Unused15,
                Unused16,
                Cover,
                Covered,
                Unused17,
                Unused18,
                Unused19,
                PursuitStart,
                PursuitSyncStart,
                PursuitSyncJoin,
                PursuitSyncQuorum,
                Melee,
                Unused20,
                Unused21,
                Unused22,
                VehicleFalling,
                VehicleWoohoo,
                VehicleScared,
                VehicleCrazy,
                Unused23,
                Unused24,
                Leap,
                Unused25,
                Unused26,
                PostcombatWin,
                PostcombatLose,
                PostcombatNeutral,
                ShootCorpse,
                PostcombatStart,
                InspectBodyStart,
                PostcombatStatus,
                Unused27,
                VehicleEntryStartDriver,
                VehicleEnter,
                VehicleEntryStartGun,
                VehicleEntryStartPassenger,
                VehicleExit,
                EvictDriver,
                EvictGunner,
                EvictPassenger,
                Unused28,
                Unused29,
                NewOrderAdvance,
                NewOrderCharge,
                NewOrderFallback,
                NewOrderRetreat,
                NewOrderMoveon,
                NewOrderArrival,
                NewOrderEntervcl,
                NewOrderExitvcl,
                NewOrderFllplr,
                NewOrderLeaveplr,
                NewOrderSupport,
                Unused30,
                Unused31,
                Unused32,
                Unused33,
                Unused34,
                Unused35,
                Unused36,
                Unused37,
                Unused38,
                Unused39,
                Unused40,
                Unused41,
                Emerge,
                Unused42,
                Unused43,
                Unused44,
                Curse,
                Unused45,
                Unused46,
                Unused47,
                Threaten,
                Unused48,
                Unused49,
                Unused50,
                CoverFriend,
                Unused51,
                Unused52,
                Unused53,
                Strike,
                Unused54,
                Unused55,
                Unused56,
                Unused57,
                Unused58,
                Unused59,
                Unused60,
                Unused61,
                Gloat,
                Unused62,
                Unused63,
                Unused64,
                Greet,
                Unused65,
                Unused66,
                Unused67,
                Unused68,
                PlayerLook,
                PlayerLookLongtime,
                Unused69,
                Unused70,
                Unused71,
                Unused72,
                PanicGrenadeAttached,
                Unused73,
                Unused74,
                Unused75,
                Unused76,
                HelpResponse,
                Unused77,
                Unused78,
                Unused79,
                Remind,
                Unused80,
                Unused81,
                Unused82,
                Unused83,
                WeaponTradeBetter,
                WeaponTradeWorse,
                WeaponReadeEqual,
                Unused84,
                Unused85,
                Unused86,
                Betray,
                Unused87,
                Forgive,
                Unused88,
                Reanimate,
                Unused89
            }
            
            public enum SpeakerTypeValue : short
            {
                Subject,
                Cause,
                Friend,
                Target,
                Enemy,
                Vehicle,
                Joint,
                Squad,
                Leader,
                JointLeader,
                Clump,
                Peer
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                SubjectVisible = 1 << 0,
                CauseVisible = 1 << 1,
                FriendsPresent = 1 << 2,
                SubjectIsSpeakerSTarget = 1 << 3,
                CauseIsSpeakerSTarget = 1 << 4,
                CauseIsPlayerOrSpeakerIsPlayerAlly = 1 << 5,
                SpeakerIsSearching = 1 << 6,
                SpeakerIsFollowingPlayer = 1 << 7,
                CauseIsPrimaryPlayerAlly = 1 << 8
            }
            
            public enum ListenerTargetValue : short
            {
                Subject,
                Cause,
                Friend,
                Target,
                Enemy,
                Vehicle,
                Joint,
                Squad,
                Leader,
                JointLeader,
                Clump,
                Peer
            }
            
            public enum HostilityValue : short
            {
                None,
                Self,
                Neutral,
                Friend,
                Enemy,
                Traitor
            }
            
            public enum DamageTypeValue : short
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
                Shotgun
            }
            
            public enum DangerLevelValue : short
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
            
            public enum AttitudeValue : short
            {
                Normal,
                Timid,
                Aggressive
            }
            
            public enum SubjectActorTypeValue : short
            {
                None,
                Elite,
                Jackal,
                Grunt,
                Hunter,
                Engineer,
                Assassin,
                Player,
                Marine,
                Crew,
                CombatForm,
                InfectionForm,
                CarrierForm,
                Monitor,
                Sentinel,
                None0,
                MountedWeapon,
                Brute,
                Prophet,
                Bugger,
                Juggernaut
            }
            
            public enum CauseActorTypeValue : short
            {
                None,
                Elite,
                Jackal,
                Grunt,
                Hunter,
                Engineer,
                Assassin,
                Player,
                Marine,
                Crew,
                CombatForm,
                InfectionForm,
                CarrierForm,
                Monitor,
                Sentinel,
                None0,
                MountedWeapon,
                Brute,
                Prophet,
                Bugger,
                Juggernaut
            }
            
            public enum CauseTypeValue : short
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
                Driver,
                Gunner,
                Passenger,
                Postcombat,
                PostcombatWon,
                PostcombatLost,
                PlayerMasterchief,
                PlayerDervish,
                Heretic,
                MajorlyScary,
                LastManInVehicle,
                Male,
                Female,
                Grenade
            }
            
            public enum SubjectTypeValue : short
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
                Driver,
                Gunner,
                Passenger,
                Postcombat,
                PostcombatWon,
                PostcombatLost,
                PlayerMasterchief,
                PlayerDervish,
                Heretic,
                MajorlyScary,
                LastManInVehicle,
                Male,
                Female,
                Grenade
            }
            
            public enum SpatialRelationValue : short
            {
                None,
                VeryNear1wu,
                Near25wus,
                MediumRange5wus,
                Far10wus,
                VeryFar10wus,
                InFrontOf,
                Behind,
                AboveDelta1Wu,
                BelowDelta1Wu
            }
            
            [Flags]
            public enum ConditionsValue : uint
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
        }
        
        [TagStructure(Size = 0x4)]
        public class DialogueDataBlock : TagStructure
        {
            public short StartIndexPostprocess;
            public short LengthPostprocess;
        }
        
        [TagStructure(Size = 0x4)]
        public class InvoluntaryDataBlock : TagStructure
        {
            public short InvoluntaryVocalizationIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
        }
    }
}

