using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "style", Tag = "styl", Size = 0x60)]
    public class Style : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        public CombatStatusEnum CombatStatusDecayOptions;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public StyleControlFlags StyleControl;
        public BehaviorSet1 Behaviors1;
        public BehaviorSet2 Behaviors2;
        public BehaviorSet3 Behaviors3;
        public BehaviorSet4 Behaviors4;
        public BehaviorSet5 Behaviors5;
        public BehaviorSet6 Behaviors6;
        public BehaviorSet7 Behaviors7;
        public BehaviorSet8 Behaviors8;
        public List<SpecialMovementBlock> SpecialMovement;
        public List<BehaviorNamesBlock> BehaviorList;
        
        public enum CombatStatusEnum : short
        {
            LatchAtIdle,
            LatchAtAlert,
            LatchAtCombat
        }
        
        [Flags]
        public enum StyleControlFlags : uint
        {
            NewBehaviorsDefaultToOn = 1 << 0
        }
        
        [Flags]
        public enum BehaviorSet1 : uint
        {
            General = 1 << 0,
            Root = 1 << 1,
            Null = 1 << 2,
            NullDiscrete = 1 << 3,
            InterruptableControl = 1 << 4,
            Obey = 1 << 5,
            Guard = 1 << 6,
            Ready = 1 << 7,
            SmashObstacle = 1 << 8,
            DestroyObstacle = 1 << 9,
            Perch = 1 << 10,
            BlindPanic = 1 << 11,
            Combat = 1 << 12,
            InteractObjectBehavior = 1 << 13,
            InteractObjectImpulse = 1 << 14,
            SquadPatrolBehavior = 1 << 15,
            MovementPlanCoverImpulse = 1 << 16,
            Broken = 1 << 17,
            BrokenBehavior = 1 << 18,
            HuddleImpulse = 1 << 19,
            HuddleBehavior = 1 << 20,
            KamikazeBehavior = 1 << 21,
            BrokenKamikazeImpulse = 1 << 22,
            BrokenBerserkImpulse = 1 << 23,
            BrokenFleeImpulse = 1 << 24,
            BrokenScatterImpulse = 1 << 25,
            Engage = 1 << 26,
            Equipment = 1 << 27,
            EquipmentActiveCamo = 1 << 28,
            Engage1 = 1 << 29,
            Fight = 1 << 30,
            FightCircle = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet2 : uint
        {
            HamstringCharge = 1 << 0,
            GravityJump = 1 << 1,
            FightPositioning = 1 << 2,
            MeleeCharge = 1 << 3,
            UnreachableLeapCharge = 1 << 4,
            MeleeLeapingCharge = 1 << 5,
            MeleeSyncAttack = 1 << 6,
            GrenadeImpulse = 1 << 7,
            ObjectThrowImpulse = 1 << 8,
            AntiVehicleGrenade = 1 << 9,
            Stalk = 1 << 10,
            BerserkWanderImpulse = 1 << 11,
            StalkerCamoControl = 1 << 12,
            LeaderAbandonedBerserk = 1 << 13,
            UnassailableGrenadeImpulse = 1 << 14,
            SuppressingFireVehicle = 1 << 15,
            DemandChargeImpulse = 1 << 16,
            Berserk = 1 << 17,
            ShieldDepletedBerserk = 1 << 18,
            LastManBerserk = 1 << 19,
            StuckWithGrenadeBerserk = 1 << 20,
            SurpriseBerserk = 1 << 21,
            ProximityBerserk = 1 << 22,
            StuckWithGrenadeBalling = 1 << 23,
            Presearch = 1 << 24,
            Presearch1 = 1 << 25,
            Uncover = 1 << 26,
            DestroyCover = 1 << 27,
            SuppressingFire = 1 << 28,
            GrenadeUncover = 1 << 29,
            LeapOnCover = 1 << 30,
            Leader = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet3 : uint
        {
            EngageSync = 1 << 0,
            Search = 1 << 1,
            Search1 = 1 << 2,
            FindPursuit = 1 << 3,
            Investigate = 1 << 4,
            SelfDefense = 1 << 5,
            SelfPreservation = 1 << 6,
            Cover = 1 << 7,
            Avoid = 1 << 8,
            EvasionImpulse = 1 << 9,
            DiveImpulse = 1 << 10,
            JukeImpulse = 1 << 11,
            DangerCoverImpulse = 1 << 12,
            DangerCrouchImpulse = 1 << 13,
            ProximityMelee = 1 << 14,
            ProximitySelfPreservation = 1 << 15,
            UnreachableEnemyCover = 1 << 16,
            UnassailableEnemyCover = 1 << 17,
            ScaryTargetCover = 1 << 18,
            GroupEmerge = 1 << 19,
            KungfuCover = 1 << 20,
            Retreat = 1 << 21,
            Retreat1 = 1 << 22,
            RetreatGrenade = 1 << 23,
            Flee = 1 << 24,
            FleeTarget = 1 << 25,
            Cower = 1 << 26,
            LowShieldRetreat = 1 << 27,
            ScaryTargetRetreat = 1 << 28,
            LeaderDeadRetreat = 1 << 29,
            PeerDeadRetreat = 1 << 30,
            DangerRetreat = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet4 : uint
        {
            ProximityRetreat = 1 << 0,
            ChargeWhenCornered = 1 << 1,
            SurpriseRetreat = 1 << 2,
            OverheatedWeaponRetreat = 1 << 3,
            Ambush = 1 << 4,
            Ambush1 = 1 << 5,
            CoordinatedAmbush = 1 << 6,
            ProximityAmbush = 1 << 7,
            VulnerableEnemyAmbush = 1 << 8,
            NowhereToRunAmbush = 1 << 9,
            Vehicle = 1 << 10,
            EnterVehicle = 1 << 11,
            EnterFriendlyVehicle = 1 << 12,
            SwitchToFriendlyVehicle = 1 << 13,
            VehicleEnterImpulse = 1 << 14,
            VehicleEntryEngageImpulse = 1 << 15,
            VehicleBoard = 1 << 16,
            VehicleFight = 1 << 17,
            VehicleFightBoost = 1 << 18,
            VehicleCharge = 1 << 19,
            VehicleRamBehavior = 1 << 20,
            VehicleCover = 1 << 21,
            DamageVehicleCover = 1 << 22,
            ExposedRearCoverImpulse = 1 << 23,
            PlayerEndageredCoverImpulse = 1 << 24,
            VehicleAvoid = 1 << 25,
            VehiclePickup = 1 << 26,
            VehicleExitImpulse = 1 << 27,
            DangerVehicleExitImpulse = 1 << 28,
            VehicleFlipImpulse = 1 << 29,
            VehicleFlip = 1 << 30,
            VehicleTurtle = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet5 : uint
        {
            VehicleEngagePatrolImpulse = 1 << 0,
            VehicleEngageWanderImpulse = 1 << 1,
            VehicleKeepStation = 1 << 2,
            Postcombat = 1 << 3,
            Postcombat1 = 1 << 4,
            PostPostcombat = 1 << 5,
            CheckFriend = 1 << 6,
            ShootCorpse = 1 << 7,
            PostcombatApproach = 1 << 8,
            Alert = 1 << 9,
            Alert1 = 1 << 10,
            Idle = 1 << 11,
            Idle1 = 1 << 12,
            WanderBehavior = 1 << 13,
            FlightWander = 1 << 14,
            Patrol = 1 << 15,
            FallSleep = 1 << 16,
            Buggers = 1 << 17,
            BuggerGroundUncover = 1 << 18,
            Engineer = 1 << 19,
            EngineerControl = 1 << 20,
            EngineerControlSlave = 1 << 21,
            EngineerControlFree = 1 << 22,
            EngineerControlEquipment = 1 << 23,
            EngineerExplode = 1 << 24,
            EngineerBrokenDetonation = 1 << 25,
            BoostAllies = 1 << 26,
            Scarab = 1 << 27,
            ScarabRoot = 1 << 28,
            ScarabCureIsolation = 1 << 29,
            ScarabCombat = 1 << 30,
            ScarabFight = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet6 : uint
        {
            ScarabTargetLock = 1 << 0,
            ScarabSearch = 1 << 1,
            ScarabSearchPause = 1 << 2,
            Flying = 1 << 3,
            FlyingRoot = 1 << 4,
            FlyingIdle = 1 << 5,
            FlyingCombat = 1 << 6,
            FlyingApproach = 1 << 7,
            FlyingCover = 1 << 8,
            FlyingEvade = 1 << 9,
            FlyingRetreat = 1 << 10,
            FlyingTail = 1 << 11,
            FlyingStrafe = 1 << 12,
            FlyingDodgeImpulse = 1 << 13,
            Lod = 1 << 14,
            LodRoot = 1 << 15,
            LodIdle = 1 << 16,
            LodCombat = 1 << 17,
            Atoms = 1 << 18,
            GoTo = 1 << 19,
            Activities = 1 << 20,
            Activity = 1 << 21,
            Posture = 1 << 22,
            ActivityDefault = 1 << 23,
            Special = 1 << 24,
            Formation = 1 << 25,
            GruntScaredByElite = 1 << 26,
            CureIsolation = 1 << 27,
            DeployTurret = 1 << 28,
            Control = 1 << 29,
            ArrangeSyncAction = 1 << 30,
            Mosh = 1u << 31
        }
        
        [Flags]
        public enum BehaviorSet7 : uint
        {
            Ctf = 1 << 0,
            Koth = 1 << 1,
            Assault = 1 << 2,
            BallingAttack = 1 << 3,
            BallingLeapAttack = 1 << 4,
            ProtectAllies = 1 << 5,
            ActivateItem = 1 << 6,
            CollectProjectiles = 1 << 7,
            ResurrectAlly = 1 << 8,
            GiveBirth = 1 << 9,
            Advance = 1 << 10,
            TeleportAdvance = 1 << 11,
            LeapAdvance = 1 << 12,
            FollowImpulse = 1 << 13,
            Follow = 1 << 14,
            Stalk = 1 << 15,
            Fight = 1 << 16,
            CommandChargeImpulse = 1 << 17,
            Puppet = 1 << 18,
            ShardSpawn = 1 << 19
        }
        
        [Flags]
        public enum BehaviorSet8 : uint
        {
        }
        
        [TagStructure(Size = 0x4)]
        public class SpecialMovementBlock : TagStructure
        {
            public SpecialMovementFlags SpecialMovement1;
            
            [Flags]
            public enum SpecialMovementFlags : uint
            {
                Jump = 1 << 0,
                Climb = 1 << 1,
                Vault = 1 << 2,
                Mount = 1 << 3,
                Hoist = 1 << 4,
                WallJump = 1 << 5,
                Takeoff = 1 << 6,
                JumpMandatoryApproach = 1 << 7
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
