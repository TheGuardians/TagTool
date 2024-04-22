using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "style", Tag = "styl", Size = 0x5C)]
    public class Style : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        /// <summary>
        /// Controls how combat status is allowed to be automatically reduced in the absence of combat stimuli. 'Latch at X' means
        /// that once the level of x is attained (and/or surpassed) the combat status never falls below it
        /// </summary>
        public CombatStatusDecayOptionsValue CombatStatusDecayOptions;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        /// <summary>
        /// Indicates general stance for style. This matches the property blocks in the character definition (e.g. defense
        /// properties)
        /// </summary>
        public AttitudeValue Attitude;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public EngageAttitudeValue EngageAttitude;
        public EvasionAttitudeValue EvasionAttitude;
        public CoverAttitudeValue CoverAttitude;
        public SearchAttitudeValue SearchAttitude;
        public PresearchAttitudeValue PresearchAttitude;
        public RetreatAttitudeValue RetreatAttitude;
        public ChargeAttitudeValue ChargeAttitude;
        public ReadyAttitudeValue ReadyAttitude;
        public IdleAttitudeValue IdleAttitude;
        public WeaponAttitudeValue WeaponAttitude;
        public SwarmAttitudeValue SwarmAttitude;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        /// <summary>
        /// Check the appropriate box to turn on/off the given behavior
        /// </summary>
        public StyleControlValue StyleControl;
        public Behaviors1Value Behaviors1;
        public Behaviors2Value Behaviors2;
        public Behaviors3Value Behaviors3;
        public Behaviors4Value Behaviors4;
        public Behaviors5Value Behaviors5;
        public List<SpecialMovementBlock> SpecialMovement;
        public List<BehaviorNamesBlock> BehaviorList;
        
        public enum CombatStatusDecayOptionsValue : short
        {
            LatchAtIdle,
            LatchAtAlert,
            LatchAtCombat
        }
        
        public enum AttitudeValue : short
        {
            Normal,
            Timid,
            Aggressive
        }
        
        public enum EngageAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum EvasionAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum CoverAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum SearchAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum PresearchAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum RetreatAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum ChargeAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum ReadyAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum IdleAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum WeaponAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        public enum SwarmAttitudeValue : sbyte
        {
            Default,
            Normal,
            Timid,
            Aggressive
        }
        
        [Flags]
        public enum StyleControlValue : uint
        {
            NewBehaviorsDefaultToOn = 1 << 0
        }
        
        [Flags]
        public enum Behaviors1Value : uint
        {
            General = 1 << 0,
            Root = 1 << 1,
            Null = 1 << 2,
            NullDiscrete = 1 << 3,
            Obey = 1 << 4,
            Guard = 1 << 5,
            FollowBehavior = 1 << 6,
            Ready = 1 << 7,
            SmashObstacle = 1 << 8,
            DestroyObstacle = 1 << 9,
            Perch = 1 << 10,
            CoverFriend = 1 << 11,
            BlindPanic = 1 << 12,
            Engage = 1 << 13,
            Engage1 = 1 << 14,
            Fight = 1 << 15,
            MeleeCharge = 1 << 16,
            MeleeLeapingCharge = 1 << 17,
            Surprise = 1 << 18,
            GrenadeImpulse = 1 << 19,
            AntiVehicleGrenade = 1 << 20,
            Stalk = 1 << 21,
            BerserkWanderImpulse = 1 << 22,
            Berserk = 1 << 23,
            LastManBerserk = 1 << 24,
            StuckWithGrenadeBerserk = 1 << 25,
            Presearch = 1 << 26,
            Presearch1 = 1 << 27,
            PresearchUncover = 1 << 28,
            DestroyCover = 1 << 29,
            SuppressingFire = 1 << 30,
            GrenadeUncover = 1u << 31
        }
        
        [Flags]
        public enum Behaviors2Value : uint
        {
            LeapOnCover = 1 << 0,
            Search = 1 << 1,
            Search1 = 1 << 2,
            Uncover = 1 << 3,
            Investigate = 1 << 4,
            PursuitSync = 1 << 5,
            Pursuit = 1 << 6,
            Postsearch = 1 << 7,
            CovermeInvestigate = 1 << 8,
            SelfDefense = 1 << 9,
            SelfPreservation = 1 << 10,
            Cover = 1 << 11,
            CoverPeek = 1 << 12,
            Avoid = 1 << 13,
            EvasionImpulse = 1 << 14,
            DiveImpulse = 1 << 15,
            DangerCoverImpulse = 1 << 16,
            DangerCrouchImpulse = 1 << 17,
            ProximityMelee = 1 << 18,
            ProximitySelfPreservation = 1 << 19,
            UnreachableEnemyCover = 1 << 20,
            ScaryTargetCover = 1 << 21,
            GroupEmerge = 1 << 22,
            Retreat = 1 << 23,
            Retreat1 = 1 << 24,
            RetreatGrenade = 1 << 25,
            Flee = 1 << 26,
            Cower = 1 << 27,
            LowShieldRetreat = 1 << 28,
            ScaryTargetRetreat = 1 << 29,
            LeaderDeadRetreat = 1 << 30,
            PeerDeadRetreat = 1u << 31
        }
        
        [Flags]
        public enum Behaviors3Value : uint
        {
            DangerRetreat = 1 << 0,
            ProximityRetreat = 1 << 1,
            ChargeWhenCornered = 1 << 2,
            SurpriseRetreat = 1 << 3,
            OverheatedWeaponRetreat = 1 << 4,
            Ambush = 1 << 5,
            Ambush1 = 1 << 6,
            CoordinatedAmbush = 1 << 7,
            ProximityAmbush = 1 << 8,
            VulnerableEnemyAmbush = 1 << 9,
            NowhereToRunAmbush = 1 << 10,
            Vehicle = 1 << 11,
            Vehicle1 = 1 << 12,
            EnterFriendlyVehicle = 1 << 13,
            ReEnterFlippedVehicle = 1 << 14,
            VehicleEntryEngageImpulse = 1 << 15,
            VehicleBoard = 1 << 16,
            VehicleFight = 1 << 17,
            VehicleCharge = 1 << 18,
            VehicleRamBehavior = 1 << 19,
            VehicleCover = 1 << 20,
            DamageVehicleCover = 1 << 21,
            ExposedRearCoverImpulse = 1 << 22,
            PlayerEndageredCoverImpulse = 1 << 23,
            VehicleAvoid = 1 << 24,
            VehiclePickup = 1 << 25,
            VehiclePlayerPickup = 1 << 26,
            VehicleExitImpulse = 1 << 27,
            DangerVehicleExitImpulse = 1 << 28,
            VehicleFlip = 1 << 29,
            VehicleTurtle = 1 << 30,
            VehicleEngagePatrolImpulse = 1u << 31
        }
        
        [Flags]
        public enum Behaviors4Value : uint
        {
            VehicleEngageWanderImpulse = 1 << 0,
            Postcombat = 1 << 1,
            Postcombat1 = 1 << 2,
            PostPostcombat = 1 << 3,
            CheckFriend = 1 << 4,
            ShootCorpse = 1 << 5,
            PostcombatApproach = 1 << 6,
            Alert = 1 << 7,
            Alert1 = 1 << 8,
            Idle = 1 << 9,
            Idle1 = 1 << 10,
            WanderBehavior = 1 << 11,
            FlightWander = 1 << 12,
            Patrol = 1 << 13,
            FallSleep = 1 << 14,
            Buggers = 1 << 15,
            BuggerGroundUncover = 1 << 16,
            Swarms = 1 << 17,
            SwarmRoot = 1 << 18,
            SwarmAttack = 1 << 19,
            SupportAttack = 1 << 20,
            Infect = 1 << 21,
            Scatter = 1 << 22,
            EjectParasite = 1 << 23,
            FloodSelfPreservation = 1 << 24,
            JuggernautFlurry = 1 << 25,
            Sentinels = 1 << 26,
            EnforcerWeaponControl = 1 << 27,
            Grapple = 1 << 28,
            Special = 1 << 29,
            Formation = 1 << 30,
            GruntScaredByElite = 1u << 31
        }
        
        [Flags]
        public enum Behaviors5Value : uint
        {
            Stunned = 1 << 0,
            CureIsolation = 1 << 1,
            DeployTurret = 1 << 2,
            Unknown = 1 << 3,
            Unknown1 = 1 << 4,
            Unknown2 = 1 << 5,
            Unknown3 = 1 << 6,
            Unknown4 = 1 << 7,
            Unknown5 = 1 << 8,
            Unknown6 = 1 << 9,
            Unknown7 = 1 << 10,
            Unknown8 = 1 << 11,
            Unknown9 = 1 << 12,
            Unknown10 = 1 << 13,
            Unknown11 = 1 << 14,
            Unknown12 = 1 << 15,
            Unknown13 = 1 << 16,
            Unknown14 = 1 << 17,
            Unknown15 = 1 << 18,
            Unknown16 = 1 << 19,
            Unknown17 = 1 << 20,
            Unknown18 = 1 << 21,
            Unknown19 = 1 << 22,
            Unknown20 = 1 << 23,
            Unknown21 = 1 << 24,
            Unknown22 = 1 << 25,
            Unknown23 = 1 << 26,
            Unknown24 = 1 << 27,
            Unknown25 = 1 << 28,
            Unknown26 = 1 << 29,
            Unknown27 = 1 << 30,
            Unknown28 = 1u << 31
        }
        
        [TagStructure(Size = 0x4)]
        public class SpecialMovementBlock : TagStructure
        {
            public SpecialMovement1Value SpecialMovement1;
            
            [Flags]
            public enum SpecialMovement1Value : uint
            {
                Jump = 1 << 0,
                Climb = 1 << 1,
                Vault = 1 << 2,
                Mount = 1 << 3,
                Hoist = 1 << 4,
                WallJump = 1 << 5,
                NA = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class BehaviorNamesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string BehaviorName;
        }
    }
}

