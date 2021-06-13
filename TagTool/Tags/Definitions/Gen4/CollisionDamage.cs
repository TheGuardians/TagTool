using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x64)]
    public class CollisionDamage : TagStructure
    {
        public CollisiondamageFlags Flags;
        // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        public float ApplyCollisionDamageScale;
        // 0 means 1.  1 is standard scale.  Some things may want to apply more damage, yet go soft on their friends
        public float FriendlyApplyCollisionDamageScale;
        // if you are going below this velocity we stop all game damage
        public float MinimumVelocityForGameDamage;
        public CollisionDamageFunction GameCollisionDamage;
        // 0-Infinity
        public Bounds<float> GameAcceleration;
        // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        public float ApplyAbsoluteCollisionDamageScale;
        // 0 means 1.  1 is standard scale.  Some things may want to apply more damage, yet go soft on their friends
        public float FriendlyApplyAbsoluteCollisionDamageScale;
        // if you are going below this velocity we stop all absolute damage
        public float MinimumVelocityForAbsoluteDamage;
        public CollisionDamageFunction AbsoluteCollisionDamage;
        // 0-Infinity
        public Bounds<float> AbsoluteAcceleration;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag AlternativeDamageEffect;
        
        [Flags]
        public enum CollisiondamageFlags : uint
        {
            // typically, we scale the damage to make only "lethal" collision damage kill people; this flag overrides that
            // behavior
            DonTScaleDamage = 1 << 0,
            // in some cases (notably, the thruster pack) we should ask an object if it wants to opt out of dealing damage for one
            // reason or another
            ObjectMayChooseNotToDealDamage = 1 << 1
        }
        
        [TagStructure(Size = 0x14)]
        public class CollisionDamageFunction : TagStructure
        {
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
