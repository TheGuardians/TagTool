using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "crate", Tag = "bloc", Size = 0x3C)]
    public class Crate : GameObject
    {
        public CrateFlags Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<CampaignMetagameBucketBlock> CampaignMetagameBucket;
        public int SelfDestructionTimer; // seconds
        // optional particleization effect definition, if you want this to particleize when it takes damage
        [TagField(ValidTags = new [] { "pman" })]
        public CachedTag Particleize;
        // the animation set to use when this crate is grabbed
        public StringId GrabAnimationSet;
        // the string to display when the player can grab this object, from ui/hud/hud_messages
        public StringId GrabPickupString;
        // effect to play when a projectile bounces because of the "all projectiles bounce off" flag
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ProjectileBounceEffect;
        
        [Flags]
        public enum CrateFlags : ushort
        {
            DoesNotBlockAoe = 1 << 0,
            AttachTextureCameraHack = 1 << 1,
            CanBeGrabbed = 1 << 2,
            AllProjectilesBounceOff = 1 << 3,
            Targetable = 1 << 4,
            // for crates that behave like a bubble shield but are not attached to equipment
            CrateWallsBlockAoe = 1 << 5,
            CrateBlocksDamageFlashDamageResponse = 1 << 6,
            CrateBlocksRumbleDamageResponse = 1 << 7,
            // crate takes top level aoe damage when parented to another object
            CrateTakesTopLevelAoeDamage = 1 << 8,
            // so that the active shield can block the splaser
            CrateBlocksForcedProjectileOverpenetration = 1 << 9,
            // some rotational and velocity attributes are not synchronized from host to client
            Unimportant = 1 << 10
        }
        
        [TagStructure(Size = 0x8)]
        public class CampaignMetagameBucketBlock : TagStructure
        {
            public CampaignMetagameBucketFlags Flags;
            public CampaignMetagameBucketTypeEnum Type;
            public CampaignMetagameBucketClassEnum Class;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short PointCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum CampaignMetagameBucketFlags : byte
            {
                OnlyCountsWithRiders = 1 << 0
            }
            
            public enum CampaignMetagameBucketTypeEnum : sbyte
            {
                Brute,
                Grunt,
                Jackel,
                Skirmisher,
                Marine,
                Spartan,
                Bugger,
                Hunter,
                FloodInfection,
                FloodCarrier,
                FloodCombat,
                FloodPure,
                Sentinel,
                Elite,
                Engineer,
                Mule,
                Turret,
                Mongoose,
                Warthog,
                Scorpion,
                Hornet,
                Pelican,
                Revenant,
                Seraph,
                Shade,
                Watchtower,
                Ghost,
                Chopper,
                Mauler,
                Wraith,
                Banshee,
                Phantom,
                Scarab,
                Guntower,
                TuningFork,
                Broadsword,
                Mammoth,
                Lich,
                Mantis,
                Wasp,
                Phaeton,
                Bishop,
                Knight,
                Pawn
            }
            
            public enum CampaignMetagameBucketClassEnum : sbyte
            {
                Infantry,
                Leader,
                Hero,
                Specialist,
                LightVehicle,
                HeavyVehicle,
                GiantVehicle,
                StandardVehicle
            }
        }
    }
}
