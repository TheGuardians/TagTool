using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "pgcr_damage_type_image_mapping_definition", Tag = "pdti", Size = 0xC)]
    public class PgcrDamageTypeImageMappingDefinition : TagStructure
    {
        public List<PgcrDamageTypeImageBlock> DamageTypeMapping;
        
        [TagStructure(Size = 0x1C)]
        public class PgcrDamageTypeImageBlock : TagStructure
        {
            public GlobalDamageReportingEnum DamageType;
            public StringId DisplayName;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Sprite;
            public short SpriteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum GlobalDamageReportingEnum : int
            {
                Unknown,
                TehGuardians,
                Scripting,
                AiSuicide,
                ForerunnerSmg,
                SpreadGun,
                ForerunnerRifle,
                ForerunnerSniper,
                BishopBeam,
                BoltPistol,
                PulseGrenade,
                IncinerationLauncher,
                MagnumPistol,
                AssaultRifle,
                MarksmanRifle,
                Shotgun,
                BattleRifle,
                SniperRifle,
                RocketLauncher,
                SpartanLaser,
                FragGrenade,
                StickyGrenadeLauncher,
                LightMachineGun,
                RailGun,
                PlasmaPistol,
                Needler,
                GravityHammer,
                EnergySword,
                PlasmaGrenade,
                Carbine,
                BeamRifle,
                AssaultCarbine,
                ConcussionRifle,
                FuelRodCannon,
                Ghost,
                RevenantDriver,
                RevenantGunner,
                Wraith,
                WraithAntiInfantry,
                Banshee,
                BansheeBomb,
                Seraph,
                RevenantDeuxDriver,
                RevenantDeuxGunner,
                LichDriver,
                LichGunner,
                Mongoose,
                WarthogDriver,
                WarthogGunner,
                WarthogGunnerGauss,
                WarthogGunnerRocket,
                Scorpion,
                ScorpionGunner,
                FalconDriver,
                FalconGunner,
                WaspDriver,
                WaspGunner,
                WaspGunnerHeavy,
                MechMelee,
                MechChaingun,
                MechCannon,
                MechRocket,
                Broadsword,
                BroadswordMissile,
                TortoiseDriver,
                TortoiseGunner,
                MacCannon,
                TargetDesignator,
                OrdnanceDropPod,
                OrbitalCruiseMissile,
                PortableShield,
                PersonalAutoTurret,
                ThrusterPack,
                FallingDamage,
                GenericCollisionDamage,
                GenericMeleeDamage,
                GenericExplosion,
                FireDamage,
                BirthdayPartyExplosion,
                FlagMeleeDamage,
                BombMeleeDamage,
                BombExplosionDamage,
                BallMeleeDamage,
                Teleporter,
                TransferDamage,
                ArmorLockCrush,
                HumanTurret,
                PlasmaCannon,
                PlasmaMortar,
                PlasmaTurret,
                ShadeTurret,
                ForerunnerTurret,
                Tank,
                Chopper,
                Hornet,
                Mantis,
                MagnumPistolCtf,
                FloodProngs
            }
        }
    }
}
