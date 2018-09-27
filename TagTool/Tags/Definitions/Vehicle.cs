using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x140, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x148, MinVersion = CacheVersion.HaloOnline106708)]
    public class Vehicle : Unit
    {
        public enum VehicleFlagsValue : int
        {
            None = 0,
            NoFrictionWithDriver = 1 << 0,
            CanTriggerAutomaticOpeningDoors = 1 << 1,
            AutoaimWhenTeamless = 1 << 2,
            AiWeaponCannotRotate = 1 << 3,
            AiDoesNotRequireDriver = 1 << 4,
            AiDriverEnable = 1 << 5,
            AiDriverFlying = 1 << 6,
            AiDriverCanSidestep = 1 << 7,
            AiDriverHovering = 1 << 8,
            NoncombatVehicle = 1 << 9,
            VehicleIsChild = 1 << 10,
            BouncesAtDeathBarriers = 1 << 11,
            Hydraulics = 1 << 12
        }

        public VehicleFlagsValue VehicleFlags;

        public List<TankEngineMotionProperty> TankEngineMotionProperties;
        public List<EngineMotionProperty> EngineMotionProperties;
        public List<DropshipMotionProperty> DropshipMotionProperties;
        public List<AntigravityMotionProperty> AntigravityMotionProperties;
        public List<JetEngineMotionProperty> JetEngineMotionProperties;
        public List<TurretProperty> TurretProperties;

        public float Unknown20;
        public float Unknown21;
        public float Unknown22;

        public List<HelicopterMotionProperty> HelicopterMotionProperties;
        public List<AntigravityEngineMotionProperty> AntigravityEngineMotionProperties;
        public List<AutoturretEquipmentBlock> AutoturretEquipment;

        public uint Flags6;
        public float GroundFriction;
        public float GroundDepth;
        public float GroundDampFactor;
        public float GroundMovingFriction;
        public float GroundMaximumSlope0;
        public float GroundMaximumSlope1LessThanSlope0;
        public float Unknown23;
        public float AntiGravityBankLift;
        public float SteeringBankReactionScale;
        public float GravityScale;
        public float Radius;
        public float Unknown24;
        public float Unknown25;
        public float Unknown26;

        public List<AntiGravityPoint> AntiGravityPoints;
        public List<FrictionPoint> FrictionPoints;
        public List<PhantomShape> PhantomShapes;

        public PlayerTrainingVehicleTypeValue PlayerTrainingVehicleType;
        public VehicleSizeValue VehicleSize;
        public sbyte Unknown27;
        public sbyte Unknown28;
        public float MinimumFlippingAngularVelocity;
        public float MaximumFlippingAngularVelocity;
        public float Unknown29;
        public float Unknown30;
        public float SeatEntranceAccelerationScale;
        public float SeatExitAccelerationScale;
        public float FlipTime;

        public StringId FlipOverMessage;

        public CachedTagInstance SuspensionSound;
        public CachedTagInstance RunningEffect;
        public CachedTagInstance UnknownResponse;
        public CachedTagInstance UnknownResponse2;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown31;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown32;

        [TagStructure(Size = 0x58)]
        public class TankEngineMotionProperty : TagStructure
		{
            public Angle SteeringOverdampenCuspAngle;
            public float SteeringOverdamenExponent;
            public float Unknown;
            public float SpeedLeft;
            public float SpeedRight;
            public float TurningSpeedLeft;
            public float TurningSpeedRight;
            public float SpeedLeft2;
            public float SpeedRight2;
            public float TurningSpeedLeft2;
            public float TurningSpeedRight2;
            public float EngineMomentum;
            public float EngineMaximumAngularVelocity;
            public List<Gear> Gears;
            public CachedTagInstance ChangeGearSound;
            public float Unknown2;
            public float Unknown3;
        }

        [TagStructure(Size = 0x40)]
        public class EngineMotionProperty : TagStructure
		{
            public Angle SteeringOverdampenCuspAngle;
            public float SteeringOverdamenExponent;
            public Angle MaximumLeftTurn;
            public Angle MaximumRightTurnNegative;
            public Angle TurnRate;
            public float EngineMomentum;
            public float EngineMaximumAngularVelocity;
            public List<Gear> Gears;
            public CachedTagInstance ChangeGearSound;
            public float Unknown;
            public float Unknown2;
        }

        [TagStructure(Size = 0x4C)]
        public class DropshipMotionProperty : TagStructure
		{
            public float ForwardAcceleration;
            public float BackwardAcceleration;
            public float Unknown;
            public float Unknown2;
            public float LeftStrafeAcceleration;
            public float RightStrafeAcceleration;
            public float Unknown3;
            public float Unknown4;
            public float LiftAcceleration;
            public float DropAcceleration;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public Angle Unknown11;
            public float Unknown12;
            public Angle Unknown13;
        }

        [TagStructure(Size = 0x70)]
        public class AntigravityMotionProperty : TagStructure
		{
            public Angle SteeringOverdampenCuspAngle;
            public float SteeringOverdamenExponent;
            public float MaximumForwardSpeed;
            public float MaximumReverseSpeed;
            public float SpeedAcceleration;
            public float SpeedDeceleration;
            public float MaximumLeftSlide;
            public float MaximumRightSlide;
            public float SlideAcceleration;
            public float SlideDeceleration;
            public sbyte Unknown;
            public sbyte Unknown2;
            public sbyte Unknown3;
            public sbyte Unknown4;
            public float Traction;
            public float Unknown5;
            public float TurningRate;
            public StringId Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public StringId Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public Angle Unknown19;
        }

        [TagStructure(Size = 0x64, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x68, MinVersion = CacheVersion.HaloOnline106708)]
        public class JetEngineMotionProperty : TagStructure
		{
            public Angle SteeringOverdampenCuspAngle;
            public float SteeringOverdamenExponent;
            public Angle MaximumLeftTurn;
            public Angle MaximumRightTurnNegative;
            public Angle TurnRate;
            public float FlyingSpeed;
            public float Acceleration;
            public float SpeedAcceleration;
            public float SpeedDeceleration;
            public float PitchLeftSpeed;
            public float PitchRightSpeed;
            public float PitchRate;
            public float UnpitchRate;
            public float FlightStability;
            public float Unknown;
            public float NoseAngle;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float FallingSpeed;
            public float FallingSpeed2;
            public float Unknown5;
            public float Unknown6;
            public float IdleRise;
            public float IdleForward;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown7;
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnline106708)]
        public class TurretProperty : TagStructure
		{
            public float Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown2;
        }

        [TagStructure(Size = 0x74)]
        public class HelicopterMotionProperty : TagStructure
		{
            public Angle MaximumLeftTurn;
            public Angle MaximumRightTurnNegative;
            public Angle Unknown;
            public StringId ThrustFrontLeft;
            public StringId ThrustFrontRight;
            public StringId Thrust;
            public Angle Unknown2;
            public Angle Unknown3;
            public Angle Unknown4;
            public Angle Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float Unknown19;
            public float Unknown20;
            public Angle Unknown21;
            public Angle Unknown22;
            public float Unknown23;
            public float Unknown24;
        }

        [TagStructure(Size = 0x70)]
        public class AntigravityEngineMotionProperty : TagStructure
		{
            public Angle SteeringOverdampenCuspAngle;
            public float SteeringOverdamenExponent;
            public Angle MaximumLeftTurn;
            public Angle MaximumRightTurnNegative;
            public Angle TurnRate;
            public float EngineMomentum;
            public float EngineMaximumAngularVelocity;
            public List<Gear> Gears;
            public CachedTagInstance ChangeGearSound;
            public float Unknown;
            public StringId Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public Angle Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
        }

        [TagStructure(Size = 0x030)]
        public class AutoturretEquipmentBlock : TagStructure
		{
            public Angle Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
        }

        [TagStructure(Size = 0x4C)]
        public class AntiGravityPoint : TagStructure
		{
            public StringId MarkerName;
            public uint Flags;
            public float AntigravStrength;
            public float AntigravOffset;
            public float AntigravHeight;
            public float AntigravDumpFactor;
            public float AntigravNormalK1;
            public float AntigravNormalK0;
            public float Radius;
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public short Unknown4;
            public short DamageSourceRegionIndex;
            public StringId DamageSourceRegionName;
            public float DefaultStateError;
            public float MinorDamageError;
            public float MediumDamageError;
            public float MajorDamageError;
            public float DestroyedStateError;
        }

        [TagStructure(Size = 0x4C)]
        public class FrictionPoint : TagStructure
		{
            public StringId MarkerName;
            public uint Flags;
            public float FractionOfTotalMass;
            public float Radius;
            public float DamagedRadius;
            public FrictionTypeValue FrictionType;
            public short Unknown;
            public float MovingFrictionVelocityDiff;
            public float EBrakeMovingFriction;
            public float EBrakeFriction;
            public float EBrakeMovingFrictionVelocityDiff;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public StringId CollisionMaterialName;
            public short CollisionGlobalMaterialIndex;
            public ModelStateDestroyedValue ModelStateDestroyed;
            public StringId RegionName;
            public int RegionIndex;

            public enum FrictionTypeValue : short
            {
                Point,
                Forward,
            }

            public enum ModelStateDestroyedValue : short
            {
                Default,
                MinorDamage,
                MediumDamage,
                MajorDamage,
                Destroyed,
            }
        }

        [TagStructure(Size = 0x330)]
        public class PhantomShape : TagStructure
		{
            public int Unknown;
            public short Size;
            public short Count;
            public int OverallShapeIndex;
            public int Offset;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public int Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public int MultisphereCount;
            public uint Flags;
            public float X0;
            public float X1;
            public float Y0;
            public float Y1;
            public float Z0;
            public float Z1;
            public int Unknown18;
            public short Size2;
            public short Count2;
            public int OverallShapeIndex2;
            public int Offset2;
            public int NumberOfSpheres;
            public float Unknown19;
            public float Unknown20;
            public float Unknown21;
            public float Sphere0X;
            public float Sphere0Y;
            public float Sphere0Z;
            public float Sphere0Radius;
            public float Sphere1X;
            public float Sphere1Y;
            public float Sphere1Z;
            public float Sphere1Radius;
            public float Sphere2X;
            public float Sphere2Y;
            public float Sphere2Z;
            public float Sphere2Radius;
            public float Sphere3X;
            public float Sphere3Y;
            public float Sphere3Z;
            public float Sphere3Radius;
            public float Sphere4X;
            public float Sphere4Y;
            public float Sphere4Z;
            public float Sphere4Radius;
            public float Sphere5X;
            public float Sphere5Y;
            public float Sphere5Z;
            public float Sphere5Radius;
            public float Sphere6X;
            public float Sphere6Y;
            public float Sphere6Z;
            public float Sphere6Radius;
            public float Sphere7X;
            public float Sphere7Y;
            public float Sphere7Z;
            public float Sphere7Radius;
            public int Unknown22;
            public short Size3;
            public short Count3;
            public int OverallShapeIndex3;
            public int Offset3;
            public int NumberOfSpheres2;
            public float Unknown23;
            public float Unknown24;
            public float Unknown25;
            public float Sphere0X2;
            public float Sphere0Y2;
            public float Sphere0Z2;
            public float Sphere0Radius2;
            public float Sphere1X2;
            public float Sphere1Y2;
            public float Sphere1Z2;
            public float Sphere1Radius2;
            public float Sphere2X2;
            public float Sphere2Y2;
            public float Sphere2Z2;
            public float Sphere2Radius2;
            public float Sphere3X2;
            public float Sphere3Y2;
            public float Sphere3Z2;
            public float Sphere3Radius2;
            public float Sphere4X2;
            public float Sphere4Y2;
            public float Sphere4Z2;
            public float Sphere4Radius2;
            public float Sphere5X2;
            public float Sphere5Y2;
            public float Sphere5Z2;
            public float Sphere5Radius2;
            public float Sphere6X2;
            public float Sphere6Y2;
            public float Sphere6Z2;
            public float Sphere6Radius2;
            public float Sphere7X2;
            public float Sphere7Y2;
            public float Sphere7Z2;
            public float Sphere7Radius2;
            public int Unknown26;
            public short Size4;
            public short Count4;
            public int OverallShapeIndex4;
            public int Offset4;
            public int NumberOfSpheres3;
            public float Unknown27;
            public float Unknown28;
            public float Unknown29;
            public float Sphere0X3;
            public float Sphere0Y3;
            public float Sphere0Z3;
            public float Sphere0Radius3;
            public float Sphere1X3;
            public float Sphere1Y3;
            public float Sphere1Z3;
            public float Sphere1Radius3;
            public float Sphere2X3;
            public float Sphere2Y3;
            public float Sphere2Z3;
            public float Sphere2Radius3;
            public float Sphere3X3;
            public float Sphere3Y3;
            public float Sphere3Z3;
            public float Sphere3Radius3;
            public float Sphere4X3;
            public float Sphere4Y3;
            public float Sphere4Z3;
            public float Sphere4Radius3;
            public float Sphere5X3;
            public float Sphere5Y3;
            public float Sphere5Z3;
            public float Sphere5Radius3;
            public float Sphere6X3;
            public float Sphere6Y3;
            public float Sphere6Z3;
            public float Sphere6Radius3;
            public float Sphere7X3;
            public float Sphere7Y3;
            public float Sphere7Z3;
            public float Sphere7Radius3;
            public int Unknown30;
            public short Size5;
            public short Count5;
            public int OverallShapeIndex5;
            public int Offset5;
            public int NumberOfSpheres4;
            public float Unknown31;
            public float Unknown32;
            public float Unknown33;
            public float Sphere0X4;
            public float Sphere0Y4;
            public float Sphere0Z4;
            public float Sphere0Radius4;
            public float Sphere1X4;
            public float Sphere1Y4;
            public float Sphere1Z4;
            public float Sphere1Radius4;
            public float Sphere2X4;
            public float Sphere2Y4;
            public float Sphere2Z4;
            public float Sphere2Radius4;
            public float Sphere3X4;
            public float Sphere3Y4;
            public float Sphere3Z4;
            public float Sphere3Radius4;
            public float Sphere4X4;
            public float Sphere4Y4;
            public float Sphere4Z4;
            public float Sphere4Radius4;
            public float Sphere5X4;
            public float Sphere5Y4;
            public float Sphere5Z4;
            public float Sphere5Radius4;
            public float Sphere6X4;
            public float Sphere6Y4;
            public float Sphere6Z4;
            public float Sphere6Radius4;
            public float Sphere7X4;
            public float Sphere7Y4;
            public float Sphere7Z4;
            public float Sphere7Radius4;
            public float Unknown34;
            public float Unknown35;
            public float Unknown36;
            public float Unknown37;
            public float Unknown38;
            public float Unknown39;
            public float Unknown40;
            public float Unknown41;
            public float Unknown42;
            public float Unknown43;
            public float Unknown44;
            public float Unknown45;
            public float Unknown46;
            public float Unknown47;
            public float Unknown48;
            public float Unknown49;
        }

        [TagStructure(Size = 0x44)]
        public class Gear : TagStructure
		{
            public float MinTorque;
            public float MaxTorque;
            public float PeakTorqueScale;
            public float PastPeakTorqueExponent;
            public float TorqueAtMaxAngularVelovity;
            public float TorqueAt2xMaxAngularVelocity;
            public float MinTorque2;
            public float MaxTorque2;
            public float PeakTorqueScale2;
            public float PastPeakTorqueExponent2;
            public float TorqueAtMaxAngularVelovity2;
            public float TorqueAt2xMaxAngularVelocity2;
            public float MinTimeToUpshift;
            public float EngineUpshiftScale;
            public float GearRatio;
            public float MinTimeToDownshift;
            public float EngineDownshiftScale;
        }

        public enum PlayerTrainingVehicleTypeValue : sbyte
        {
            None,
            Warthog,
            WarthogTurret,
            Ghost,
            Banshee,
            Tank,
            Wraith,
        }

        public enum VehicleSizeValue : sbyte
        {
            Small,
            Large,
        }
    }
}