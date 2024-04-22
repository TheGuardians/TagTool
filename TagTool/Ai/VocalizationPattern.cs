using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x34, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
    public class VocalizationPattern : TagStructure
    {
        public DialogueTypeEnum DialogueType;
        public short VocalizationIndex;

        [TagField(Flags = Label)]
        public StringId VocalizationName;

        public SpeakerTypeEnum SpeakerType;  // who/what am I speaking to/of?
        public SpeakerTypeEnum ListenerTarget; // The relationship between the subject and the cause
        public HostilityEnum Hostility;
        public PatternFlags Flags;
        public ActorTypeEnum CauseActorType;
        public DialogueObjectTypesEnum CauseType;
        public StringId CauseAiTypeName;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId CauseEquipmentTypeName;

        public DialogueObjectTypesEnum SpeakerObjectType;

        [TagField(Length = 2, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused;

        public SpeakerBehaviorEnum SpeakerBehavior;
        public DangerEnum DangerLevel;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public StyleAttitudeEnum Attitude;

        public SpatialRelationEnum SpeakerSubjectPosition;
        public SpatialRelationEnum SpeakerCausePosition;
        public DialogueConditionFlags Conditions;

        [TagField(EnumType = typeof(short))]
        public SpatialRelationEnum SpacialRelation;
        public DamageEnum DamageType;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GameTypeEnum GameType;

        public ActorTypeEnum SubjectActorType;
        public DialogueObjectTypesEnum SubjectType;

        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x2, Flags = Padding)]
        public byte[] PaddingReach;

        public StringId SubjectAiTypeName;

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
            PlayerDervish,
            PlayerRookie,
            PlayerBuck,
            PlayerDutch,
            PlayerMickey,
            PlayerRomeo,
            Heretic,
            MajorlyScary,
            LastManInVehicle,
            Male,
            Female,
            Grenade,
            Stealth,
            Flood,
            Pureform,
            PlayerEmptyVehicle
        }

        public enum DamageEnum : short
        {
            NONE,
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
            NONE,
            BroadlyFacing,
            ShootingNear,
            ShootingAt,
            ExtremelyClose,
            ShieldDamage,
            ShieldExtendedDamage,
            BodyDamage,
            BodyExtendedDamage
        }

        public enum StyleAttitudeEnum : short
        {
            Normal,
            Timid,
            Aggressive
        }

        public enum GameTypeEnum : short
        {
            None,
            Sandbox,
            Megalo,
            Campaign,
            Survival
        }
    }
}
