using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0x148)]
    public class DamageEffect : TagStructure
    {
        [TagField(ValidTags = new [] { "obje" })]
        // if a reference is here, area of effect damage will attach the equipment to the target
        public CachedTag AreaOfEffectBehaviorEquipment;
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0,1]
        public DamageEffectFlags EffectFlags;
        public DamageSideEffects SideEffect;
        public DamageCategories Category;
        public DamageDeathVocalizations DeathVocalization;
        public DamageFlags Flags;
        public DamagesecondaryFlags SecondaryFlags;
        // ignored if zero or if headshot flag is not set above;
        // otherwise, makes headshot not instantly lethal, but multiplies damage done to head by this much.
        // 2 does 2x damage to head for headshot.  note that head still has different health than body.
        public float HeadshotDamageMultiplier;
        // if >0 then higher fidelity obstruction collision checks are preformed. This field has no other ramifications.
        public float AoeCoreRadius; // world units
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        public ScalarFunctionNamedStruct DamageFalloffFunction;
        public Angle DmgInnerConeAngle;
        public DamageOuterConeAngleStruct Blah;
        // how much more visible this damage makes a player who is active camouflaged
        public float ActiveCamouflageDamage; // [0,1]
        // amount of stun added to damaged unit
        public float Stun; // [0,1]
        // damaged unit's stun will never exceed this amount
        public float MaximumStun; // [0,1]
        // duration of stun due to this damage
        public float StunTime; // seconds
        // how long we stun recovering current body damage
        public int DamageStun; // ticks
        public float InstantaneousAcceleration; // [0,+inf]
        // This field can be used for an alternative acceleration if the corresponding flags are set, e.g. check
        // vehicle->flags.vehicle wants reduced weapon impulse when airborne
        public float AltInstantaneousAcceleration; // [0,+inf]
        // set to zero to disable cap
        public float AccelerationCap;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public float RiderDirectDamageScale;
        public float RiderMaximumTransferDamageScale;
        public float RiderMinimumTransferDamageScale;
        public float VehicleDamageMultiplier;
        // The maximum amount to apply the pain screen pose overlay
        public float SoftPingPainScreenScale; // [0,1]
        [TagField(Length = 0x8C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public StringId GeneralDamage;
        public StringId SpecificDamage;
        public List<CustomDamageResponseLabelBlock> CustomResponseLabels;
        public float AiStunRadius; // world units
        public Bounds<float> AiStunBounds; // (0-1)
        public float ShakeRadius;
        public float EmpRadius;
        public float AoeSpikeRadius;
        public float AoeSpikeDamageBump;
        public float ShieldRenderEffectsScale;
        // duration of stasis due to this damage, zero disables
        public int StasisTime; // ticks
        public float StasisTimeDilation;
        public float StasisMaxBipedTurningRate;
        public float StasisGravityMultiplier;
        public float EquipmentHackTime; // seconds
        // regen velocity of TOL
        public float TreeOfLifeShieldRegenModifier; // if non-1.0 this overrides any 'shield recharge rate' trait
        // when hit, shield will be reset to this percentage of full strength if it is below
        public float MinimumShieldLevel; // [0, 1]
        // time after damage applied before shield recharges
        public float ShieldStunTime; // seconds
        // when hit, health will be reset to this percentage of full health if it is below
        public float MinimumHealthLevel; // [0, 1]
        // time after damage applied before health recharges
        public float HealthStunTime; // seconds
        // like above, TOL reset minimum equipment energy to this
        public float MinimumEnergyLevel; // [0, 1]
        // cap equipment energy to this
        public float MaximumEnergyLevel; // [0, 1]
        // time after damage applied before equipment recharges
        public float EquipmentStunTime; // seconds
        public float ShieldMinimumStunTime; // seconds
        public float HealthMinimumStunTime; // seconds
        [TagField(ValidTags = new [] { "drdf" })]
        public CachedTag DamageResponse;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag OldMeleeSound;
        public List<DamageEffectSoundBlockStruct> DamageSounds;
        [TagField(Length = 0x70, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public float ForwardVelocity; // world units per second
        public float ForwardRadius; // world units
        public float ForwardExponent;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public float OutwardVelocity; // world units per second
        public float OutwardRadius; // world units
        public float OutwardExponent;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        
        [Flags]
        public enum DamageEffectFlags : uint
        {
            DonTScaleDamageByDistance = 1 << 0,
            // area of effect damage only affects players
            AreaDamagePlayersOnly = 1 << 1,
            // distribute the damage amongst the model targets. this is the default behavior for aoe damage, but can be set here
            // for direct damage.
            AffectsModelTargets = 1 << 2,
            ExplosiveAreaOfEffect = 1 << 3
        }
        
        public enum DamageSideEffects : short
        {
            None,
            Harmless,
            LethalToTheUnsuspecting,
            Emp
        }
        
        public enum DamageCategories : short
        {
            None,
            Falling,
            Bullet,
            Grenade,
            HighExplosive,
            Sniper,
            Melee,
            Flame,
            MountedWeapon,
            Vehicle,
            Plasma,
            Needle,
            Shotgun,
            Assassinated
        }
        
        public enum DamageDeathVocalizations : int
        {
            // uses code to figure out what vocalization to use
            Default,
            Dth,
            DthFall,
            DthMjr,
            DthSlw,
            DthHdsht,
            DthSlnt,
            DthDrama,
            DthReanimated,
            Thrwn,
            DieSpace,
            DieAss
        }
        
        [Flags]
        public enum DamageFlags : uint
        {
            DoesNotHurtOwner = 1 << 0,
            CanCauseHeadshots = 1 << 1,
            // arms, held weapons, attachments
            IgnoresHeadshotObstructions = 1 << 2,
            PingsResistantUnits = 1 << 3,
            // affects aoe only
            DoesNotHurtFriends = 1 << 4,
            DoesNotPingUnits = 1 << 5,
            DetonatesExplosives = 1 << 6,
            OnlyHurtsShields = 1 << 7,
            CausesFlamingDeath = 1 << 8,
            SkipsShields = 1 << 9,
            TransferDmgAlwaysUsesMin = 1 << 10,
            IgnoreSeatScaleForDirDmg = 1 << 11,
            ForcesHardPingIfBodyDmg = 1 << 12,
            ForcesHardPingAlways = 1 << 13,
            DoesNotHurtPlayers = 1 << 14,
            EnablesSpecialDeath = 1 << 15,
            CannotCauseBetrayals = 1 << 16,
            UsesOldEmpBehavior = 1 << 17,
            IgnoresDamageResistance = 1 << 18,
            ForceSKillOnDeath = 1 << 19,
            CauseMagicDeceleration = 1 << 20,
            AoeSkipObstructionTest = 1 << 21,
            DoesNotSpillOver = 1 << 22,
            DoesNotHurtBoarders = 1 << 23,
            DoesNotCauseBipedAoeEffect = 1 << 24,
            CausesBipedKnockback = 1 << 25,
            // as if a tree of life was near 'victim'
            ApplyTreeOfLife = 1 << 26,
            // affects aoe only
            HurtOnlyFriends = 1 << 27,
            CausesIncinerationDissolve = 1 << 28,
            CausesIncinerationDissolveOnHeadshot = 1 << 29,
            DoesNotHurtDamageSource = 1 << 30,
            DamageVehiclesOnly = 1u << 31
        }
        
        [Flags]
        public enum DamagesecondaryFlags : uint
        {
            CausesIncinerationDissolveToDeadUnits = 1 << 0,
            // force a hard ping as a notification to the player that the sticky grenade has attached
            ForceHardPingAsAttachmentFeedback = 1 << 1
        }
        
        [TagStructure(Size = 0x14)]
        public class ScalarFunctionNamedStruct : TagStructure
        {
            public MappingFunction Function;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class DamageOuterConeAngleStruct : TagStructure
        {
            public Angle DmgOuterConeAngle;
        }
        
        [TagStructure(Size = 0x4)]
        public class CustomDamageResponseLabelBlock : TagStructure
        {
            // label used to control what damage response will fire.^
            public StringId CustomLabel;
        }
        
        [TagStructure(Size = 0x14)]
        public class DamageEffectSoundBlockStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag Sound;
            public DamageEffectSoundTypeFlags DamageTypes;
            public ObjectTypeEnum ObjectTypes;
            
            [Flags]
            public enum DamageEffectSoundTypeFlags : ushort
            {
                None = 1 << 0,
                // headshots and assassinations
                LethalInstantaneous = 1 << 1,
                // excludes headshots and assassinations
                Lethal = 1 << 2,
                NonLethal = 1 << 3
            }
            
            [Flags]
            public enum ObjectTypeEnum : ushort
            {
                Biped = 1 << 0,
                Vehicle = 1 << 1,
                Weapon = 1 << 2,
                Equipment = 1 << 3,
                Terminal = 1 << 4,
                Projectile = 1 << 5,
                Scenery = 1 << 6,
                Machine = 1 << 7,
                Control = 1 << 8,
                Dispenser = 1 << 9,
                SoundScenery = 1 << 10,
                Crate = 1 << 11,
                Creature = 1 << 12,
                Giant = 1 << 13,
                EffectScenery = 1 << 14,
                Spawner = 1 << 15
            }
        }
    }
}
