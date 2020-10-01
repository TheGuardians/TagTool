using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "model_collision_geometry", Tag = "coll", Size = 0x298)]
    public class ModelCollisionGeometry : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// the material we use when shielding child objects or getting hit by area of effect damage
        /// </summary>
        public short IndirectDamageMaterial;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        /// <summary>
        /// the default initial and maximum body vitality of this object
        /// </summary>
        public float MaximumBodyVitality;
        /// <summary>
        /// anything that kills us (body depleted) doing more than this amount of damage also destroys us
        /// </summary>
        public float BodySystemShock;
        [TagField(Length = 0x18)]
        public byte[] Padding1;
        [TagField(Length = 0x1C)]
        public byte[] Padding2;
        /// <summary>
        /// the fraction of damage caused by friendly units ignored by this object (zero means full damage)
        /// </summary>
        public float FriendlyDamageResistance; // [0,1]
        [TagField(Length = 0x8)]
        public byte[] Padding3;
        [TagField(Length = 0x20)]
        public byte[] Padding4;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag LocalizedDamageEffect;
        public float AreaDamageEffectThreshold; // [0,1]
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag AreaDamageEffect;
        /// <summary>
        /// when passing this vitality the 'body damaged' effect, below, is created
        /// </summary>
        public float BodyDamagedThreshold;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag BodyDamagedEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag BodyDepletedEffect;
        /// <summary>
        /// when passing this vitality (usually negative) the object is deleted
        /// </summary>
        public float BodyDestroyedThreshold;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag BodyDestroyedEffect;
        /// <summary>
        /// the default initial and maximum shield vitality of this object
        /// </summary>
        public float MaximumShieldVitality;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        /// <summary>
        /// the material type used when projectiles impact the shield (instead of the body)
        /// </summary>
        public ShieldMaterialTypeValue ShieldMaterialType;
        [TagField(Length = 0x18)]
        public byte[] Padding6;
        /// <summary>
        /// how fast the shield begins to leak
        /// </summary>
        public ShieldFailureFunctionValue ShieldFailureFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        /// <summary>
        /// when the shield begins to leak (0.5 would cause the shield to begin to fail after taking half damage)
        /// </summary>
        public float ShieldFailureThreshold;
        /// <summary>
        /// the maximum percent [0,1] damage a failing shield will leak to the body
        /// </summary>
        public float FailingShieldLeakFraction;
        [TagField(Length = 0x10)]
        public byte[] Padding8;
        /// <summary>
        /// the minimum damage required to stun this object's shields
        /// </summary>
        public float MinimumStunDamage;
        /// <summary>
        /// the length of time the shields stay stunned (do not recharge) after taking damage
        /// </summary>
        public float StunTime; // seconds
        /// <summary>
        /// the length of time it would take for the shields to fully recharge after being completely depleted
        /// </summary>
        public float RechargeTime; // seconds
        [TagField(Length = 0x10)]
        public byte[] Padding9;
        [TagField(Length = 0x60)]
        public byte[] Padding10;
        public float ShieldDamagedThreshold;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ShieldDamagedEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ShieldDepletedEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ShieldRechargingEffect;
        [TagField(Length = 0xC)]
        public byte[] Padding11;
        [TagField(Length = 0x70)]
        public byte[] Padding12;
        public List<DamageMaterialsBlock> Materials;
        public List<DamageRegionsBlock> Regions;
        public List<DamageModifiersBlock> Modifiers;
        [TagField(Length = 0x10)]
        public byte[] Padding13;
        public Bounds<float> X;
        public Bounds<float> Y;
        public Bounds<float> Z;
        public List<Sphere> PathfindingSpheres;
        public List<Node> Nodes;
        
        public enum FlagsValue : uint
        {
            TakesShieldDamageForChildren,
            TakesBodyDamageForChildren,
            AlwaysShieldsFriendlyDamage,
            PassesAreaDamageToChildren,
            ParentNeverTakesBodyDamageForUs,
            OnlyDamagedByExplosives,
            OnlyDamagedWhileOccupied
        }
        
        public enum ShieldMaterialTypeValue : short
        {
            Dirt,
            Sand,
            Stone,
            Snow,
            Wood,
            MetalHollow,
            MetalThin,
            MetalThick,
            Rubber,
            Glass,
            ForceField,
            Grunt,
            HunterArmor,
            HunterSkin,
            Elite,
            Jackal,
            JackalEnergyShield,
            EngineerSkin,
            EngineerForceField,
            FloodCombatForm,
            FloodCarrierForm,
            CyborgArmor,
            CyborgEnergyShield,
            HumanArmor,
            HumanSkin,
            Sentinel,
            Monitor,
            Plastic,
            Water,
            Leaves,
            EliteEnergyShield,
            Ice,
            HunterShield
        }
        
        public enum ShieldFailureFunctionValue : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine
        }
        
        [TagStructure(Size = 0x48)]
        public class DamageMaterialsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public MaterialTypeValue MaterialType;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// the percent [0,1] damage the shield always leaks through to the body
            /// </summary>
            public float ShieldLeakPercentage;
            public float ShieldDamageMultiplier;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            /// <summary>
            /// multiplier to body damage against this material (1.0 is normal)
            /// </summary>
            public float BodyDamageMultiplier;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            
            public enum FlagsValue : uint
            {
                Head
            }
            
            public enum MaterialTypeValue : short
            {
                Dirt,
                Sand,
                Stone,
                Snow,
                Wood,
                MetalHollow,
                MetalThin,
                MetalThick,
                Rubber,
                Glass,
                ForceField,
                Grunt,
                HunterArmor,
                HunterSkin,
                Elite,
                Jackal,
                JackalEnergyShield,
                EngineerSkin,
                EngineerForceField,
                FloodCombatForm,
                FloodCarrierForm,
                CyborgArmor,
                CyborgEnergyShield,
                HumanArmor,
                HumanSkin,
                Sentinel,
                Monitor,
                Plastic,
                Water,
                Leaves,
                EliteEnergyShield,
                Ice,
                HunterShield
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class DamageRegionsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            /// <summary>
            /// if this region takes this amount of damage, it will be destroyed
            /// </summary>
            public float DamageThreshold;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DestroyedEffect;
            public List<DamagePermutationsBlock> Permutations;
            
            public enum FlagsValue : uint
            {
                LivesUntilObjectDies,
                ForcesObjectToDie,
                DiesWhenObjectDies,
                DiesWhenObjectIsDamaged,
                DisappearsWhenShieldIsOff,
                InhibitsMeleeAttack,
                InhibitsWeaponAttack,
                InhibitsWalking,
                ForcesDropWeapon,
                CausesHeadMaimedScream
            }
            
            [TagStructure(Size = 0x20)]
            public class DamagePermutationsBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class DamageModifiersBlock : TagStructure
        {
            [TagField(Length = 0x34)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x20)]
        public class Sphere : TagStructure
        {
            public short Node;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            public RealPoint3d Center;
            public float Radius;
        }
        
        [TagStructure(Size = 0x40)]
        public class Node : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Region;
            public short ParentNode;
            public short NextSiblingNode;
            public short FirstChildNode;
            [TagField(Length = 0xC)]
            public byte[] Padding;
            public List<Bsp> Bsps;
            
            [TagStructure(Size = 0x60)]
            public class Bsp : TagStructure
            {
                public List<Bsp3dNode> Bsp3dNodes;
                public List<Plane> Planes;
                public List<Leaf> Leaves;
                public List<Bsp2dReference> Bsp2dReferences;
                public List<Bsp2dNode> Bsp2dNodes;
                public List<Surface> Surfaces;
                public List<Edge> Edges;
                public List<Vertex> Vertices;
                
                [TagStructure(Size = 0xC)]
                public class Bsp3dNode : TagStructure
                {
                    public int Plane;
                    public int BackChild;
                    public int FrontChild;
                }
                
                [TagStructure(Size = 0x10)]
                public class Plane : TagStructure
                {
                    public RealPlane3d Plane1;
                }
                
                [TagStructure(Size = 0x8)]
                public class Leaf : TagStructure
                {
                    public FlagsValue Flags;
                    public short Bsp2dReferenceCount;
                    public int FirstBsp2dReference;
                    
                    public enum FlagsValue : ushort
                    {
                        ContainsDoubleSidedSurfaces
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class Bsp2dReference : TagStructure
                {
                    public int Plane;
                    public int Bsp2dNode;
                }
                
                [TagStructure(Size = 0x14)]
                public class Bsp2dNode : TagStructure
                {
                    public RealPlane2d Plane;
                    public int LeftChild;
                    public int RightChild;
                }
                
                [TagStructure(Size = 0xC)]
                public class Surface : TagStructure
                {
                    public int Plane;
                    public int FirstEdge;
                    public FlagsValue Flags;
                    public sbyte BreakableSurface;
                    public short Material;
                    
                    public enum FlagsValue : byte
                    {
                        TwoSided,
                        Invisible,
                        Climbable,
                        Breakable
                    }
                }
                
                [TagStructure(Size = 0x18)]
                public class Edge : TagStructure
                {
                    public int StartVertex;
                    public int EndVertex;
                    public int ForwardEdge;
                    public int ReverseEdge;
                    public int LeftSurface;
                    public int RightSurface;
                }
                
                [TagStructure(Size = 0x10)]
                public class Vertex : TagStructure
                {
                    public RealPoint3d Point;
                    public int FirstEdge;
                }
            }
        }
    }
}

