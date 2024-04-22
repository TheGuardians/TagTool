using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "havok_collision_filter", Tag = "hcfd", Size = 0x110)]
    public class HavokCollisionFilter : TagStructure
    {
        public List<HavokCollisionFilterGroupBlock> Groups;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public HavokGroupFilterFilterStruct GroupFilter;
        
        [TagStructure(Size = 0x4)]
        public class HavokCollisionFilterGroupBlock : TagStructure
        {
            public CollisionFilterEnum Filter;
            
            [Flags]
            public enum CollisionFilterEnum : uint
            {
                Everything = 1 << 0,
                EnvironmentDefault = 1 << 1,
                EnvironmentOnly = 1 << 2,
                SmallCrate = 1 << 3,
                Crate = 1 << 4,
                HugeCrate = 1 << 5,
                Item = 1 << 6,
                Projectile = 1 << 7,
                Machine = 1 << 8,
                EarlyMoverMachine = 1 << 9,
                Creature = 1 << 10,
                Biped = 1 << 11,
                DeadBiped = 1 << 12,
                SuperCollidableRagdoll = 1 << 13,
                Ragdoll = 1 << 14,
                Vehicle = 1 << 15,
                Decal = 1 << 16,
                ForgeDynamicScenary = 1 << 17,
                SmallExpensivePlant = 1 << 18,
                TechArtCustom = 1 << 19,
                Proxy = 1 << 20,
                HugeVehicle = 1 << 21,
                IgnoreEnvironment = 1 << 22,
                CharacterPosture = 1 << 23,
                ItemBlocker = 1 << 24,
                User00 = 1 << 25,
                ZeroExtent = 1 << 26,
                PhysicalProjectile = 1 << 27,
                EnvironmentInvisibleWall = 1 << 28,
                EnvironmentPlayCollision = 1 << 29,
                EnvironmentBulletCollision = 1 << 30
            }
        }
        
        [TagStructure(Size = 0x100)]
        public class HavokGroupFilterFilterStruct : TagStructure
        {
            public int HkreferencedObjectVtable;
            public short Size;
            public short Count;
            public int HkpcollidableCollidableFilterVtable;
            public int HkpshapeCollectionFilterVtable;
            public int HkprayShapeCollectionFilterVtable;
            public int HkprayCollidableFilterVtable;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int MType;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public int MNextfreeSystemGroup;
            public int MCollisionlookupTable00;
            public int MCollisionlookupTable01;
            public int MCollisionlookupTable02;
            public int MCollisionlookupTable03;
            public int MCollisionlookupTable04;
            public int MCollisionlookupTable05;
            public int MCollisionlookupTable06;
            public int MCollisionlookupTable07;
            public int MCollisionlookupTable08;
            public int MCollisionlookupTable09;
            public int MCollisionlookupTable10;
            public int MCollisionlookupTable11;
            public int MCollisionlookupTable12;
            public int MCollisionlookupTable13;
            public int MCollisionlookupTable14;
            public int MCollisionlookupTable15;
            public int MCollisionlookupTable16;
            public int MCollisionlookupTable17;
            public int MCollisionlookupTable18;
            public int MCollisionlookupTable19;
            public int MCollisionlookupTable20;
            public int MCollisionlookupTable21;
            public int MCollisionlookupTable22;
            public int MCollisionlookupTable23;
            public int MCollisionlookupTable24;
            public int MCollisionlookupTable25;
            public int MCollisionlookupTable26;
            public int MCollisionlookupTable27;
            public int MCollisionlookupTable28;
            public int MCollisionlookupTable29;
            public int MCollisionlookupTable30;
            public int MCollisionlookupTable31;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public RealVector3d MPad2560;
            public float HavokWMPad2560;
            public RealVector3d MPad2561;
            public float HavokWMPad2561;
            public RealVector3d MPad2562;
            public float HavokWMPad2562;
            public RealVector3d MPad2563;
            public float HavokWMPad2563;
        }
    }
}
