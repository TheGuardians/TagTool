using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "device_machine", Tag = "mach", Size = 0x324)]
    public class DeviceMachine : TagStructure
    {
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public RealPoint3d OriginOffset;
        /// <summary>
        /// marine 1.0, grunt 1.4, elite 0.9, hunter 0.5, etc.
        /// </summary>
        public float AccelerationScale; // [0,+inf]
        [TagField(Length = 0x4)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "mod2" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "antr" })]
        public CachedTag AnimationGraph;
        [TagField(Length = 0x28)]
        public byte[] Padding2;
        [TagField(ValidTags = new [] { "coll" })]
        public CachedTag CollisionModel;
        [TagField(ValidTags = new [] { "phys" })]
        public CachedTag Physics;
        [TagField(ValidTags = new [] { "shdr" })]
        public CachedTag ModifierShader;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(Length = 0x54)]
        public byte[] Padding3;
        /// <summary>
        /// if set, this radius is used to determine if the object is visible. set it for the pelican.
        /// </summary>
        public float RenderBoundingRadius; // world units
        public AInValue AIn;
        public BInValue BIn;
        public CInValue CIn;
        public DInValue DIn;
        [TagField(Length = 0x2C)]
        public byte[] Padding4;
        public short HudTextMessageIndex;
        public short ForcedShaderPermuationIndex;
        public List<ObjectAttachmentBlock> Attachments;
        public List<ObjectWidgetBlock> Widgets;
        public List<ObjectFunctionBlock> Functions;
        public List<ObjectChangeColors> ChangeColors;
        public List<PredictedResourceBlock> PredictedResources;
        public Flags1Value Flags1;
        public float PowerTransitionTime; // seconds
        public float PowerAccelerationTime; // seconds
        public float PositionTransitionTime; // seconds
        public float PositionAccelerationTime; // seconds
        public float DepoweredPositionTransitionTime; // seconds
        public float DepoweredPositionAccelerationTime; // seconds
        public AIn1Value AIn1;
        public BIn1Value BIn1;
        public CIn1Value CIn1;
        public DIn1Value DIn1;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag OpenUp;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag CloseDown;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Opened;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Closed;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Depowered;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Repowered;
        public float DelayTime; // seconds
        [TagField(Length = 0x8)]
        public byte[] Padding5;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag DelayEffect;
        public float AutomaticActivationRadius; // world units
        [TagField(Length = 0x54)]
        public byte[] Padding6;
        [TagField(Length = 0x1C)]
        public byte[] Padding7;
        public TypeValue Type;
        public Flags2Value Flags2;
        public float DoorOpenTime; // seconds
        [TagField(Length = 0x50)]
        public byte[] Padding8;
        public CollisionResponseValue CollisionResponse;
        public short ElevatorNode;
        [TagField(Length = 0x34)]
        public byte[] Padding9;
        [TagField(Length = 0x4)]
        public byte[] Padding10;
        
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow,
            TransparentSelfOcclusion,
            BrighterThanItShouldBe,
            NotAPathfindingObstacle
        }
        
        public enum AInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum BInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum CInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum DInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        [TagStructure(Size = 0x48)]
        public class ObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh","mgs2","cont","pctl","effe","lsnd" })]
            public CachedTag Type;
            [TagField(Length = 32)]
            public string Marker;
            public PrimaryScaleValue PrimaryScale;
            public SecondaryScaleValue SecondaryScale;
            public ChangeColorValue ChangeColor;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            
            public enum PrimaryScaleValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum SecondaryScaleValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ChangeColorValue : short
            {
                None,
                A,
                B,
                C,
                D
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectWidgetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "flag","ant!","glw!","mgs2","elec" })]
            public CachedTag Reference;
            [TagField(Length = 0x10)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x168)]
        public class ObjectFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// this is the period for the above function (lower values make the function oscillate quickly, higher values make it
            /// oscillate slowly)
            /// </summary>
            public float Period; // seconds
            /// <summary>
            /// multiply this function by the above period
            /// </summary>
            public ScalePeriodByValue ScalePeriodBy;
            public FunctionValue Function;
            /// <summary>
            /// multiply this function by the result of the above function
            /// </summary>
            public ScaleFunctionByValue ScaleFunctionBy;
            /// <summary>
            /// the curve used for the wobble
            /// </summary>
            public WobbleFunctionValue WobbleFunction;
            /// <summary>
            /// the length of time it takes for the magnitude of this function to complete a wobble
            /// </summary>
            public float WobblePeriod; // seconds
            /// <summary>
            /// the amount of random wobble in the magnitude
            /// </summary>
            public float WobbleMagnitude; // percent
            /// <summary>
            /// if non-zero, all values above the square wave threshold are snapped to 1.0, and all values below it are snapped to 0.0 to
            /// create a square wave.
            /// </summary>
            public float SquareWaveThreshold;
            /// <summary>
            /// the number of discrete values to snap to (e.g., a step count of 5 would snap the function to 0.00,0.25,0.50,0.75 or 1.00)
            /// </summary>
            public short StepCount;
            public MapToValue MapTo;
            /// <summary>
            /// the number of times this function should repeat (e.g., a sawtooth count of 5 would give the function a value of 1.0 at
            /// each of 0.25,0.50,0.75 as well as at 1.0
            /// </summary>
            public short SawtoothCount;
            /// <summary>
            /// add this function to the final result of all of the above math
            /// </summary>
            public AddValue Add;
            /// <summary>
            /// multiply this function (from a weapon, vehicle, etc.) final result of all of the above math
            /// </summary>
            public ScaleResultByValue ScaleResultBy;
            /// <summary>
            /// controls how the bounds, below, are used
            /// </summary>
            public BoundsModeValue BoundsMode;
            public Bounds<float> Bounds;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            /// <summary>
            /// if the specified function is off, so is this function
            /// </summary>
            public short TurnOffWith;
            /// <summary>
            /// applied before clip, ignored if zero
            /// </summary>
            public float ScaleBy;
            [TagField(Length = 0xFC)]
            public byte[] Padding2;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(Length = 32)]
            public string Usage;
            
            public enum FlagsValue : uint
            {
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert,
                Additive,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive
            }
            
            public enum ScalePeriodByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum FunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum ScaleFunctionByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum WobbleFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum MapToValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine
            }
            
            public enum AddValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleResultByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum BoundsModeValue : short
            {
                Clip,
                ClipAndNormalize,
                ScaleToFit
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class ObjectChangeColors : TagStructure
        {
            public DarkenByValue DarkenBy;
            public ScaleByValue ScaleBy;
            public ScaleFlagsValue ScaleFlags;
            public RealRgbColor ColorLowerBound;
            public RealRgbColor ColorUpperBound;
            public List<ObjectChangeColorPermutations> Permutations;
            
            public enum DarkenByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleFlagsValue : uint
            {
                /// <summary>
                /// blends colors in hsv rather than rgb space
                /// </summary>
                BlendInHsv,
                /// <summary>
                /// blends colors through more hues (goes the long way around the color wheel)
                /// </summary>
                MoreColors
            }
            
            [TagStructure(Size = 0x1C)]
            public class ObjectChangeColorPermutations : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound
            }
        }
        
        public enum Flags1Value : uint
        {
            PositionLoops,
            PositionNotInterpolated
        }
        
        public enum AIn1Value : short
        {
            None,
            Power,
            ChangeInPower,
            Position,
            ChangeInPosition,
            Locked,
            Delay
        }
        
        public enum BIn1Value : short
        {
            None,
            Power,
            ChangeInPower,
            Position,
            ChangeInPosition,
            Locked,
            Delay
        }
        
        public enum CIn1Value : short
        {
            None,
            Power,
            ChangeInPower,
            Position,
            ChangeInPosition,
            Locked,
            Delay
        }
        
        public enum DIn1Value : short
        {
            None,
            Power,
            ChangeInPower,
            Position,
            ChangeInPosition,
            Locked,
            Delay
        }
        
        public enum TypeValue : short
        {
            Door,
            Platform,
            Gear
        }
        
        public enum Flags2Value : ushort
        {
            PathfindingObstacle,
            ButNotWhenOpen,
            /// <summary>
            /// lighting based on what's around, rather than what's below
            /// </summary>
            Elevator
        }
        
        public enum CollisionResponseValue : short
        {
            PauseUntilCrushed,
            ReverseDirections
        }
    }
}

