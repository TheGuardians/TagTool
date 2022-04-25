using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public object ConvertObject(object gen2Tag)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.Crate crate:
                    Crate newcrate = new Crate();
                    TranslateTagStructure(crate, newcrate, typeof(TagTool.Tags.Definitions.Gen2.Crate), typeof(Crate));
                    newcrate.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Crate };
                    return newcrate;
                case TagTool.Tags.Definitions.Gen2.Scenery scenery:
                    Scenery newscenery = new Scenery();
                    TranslateTagStructure(scenery, newscenery, typeof(TagTool.Tags.Definitions.Gen2.Scenery), typeof(Scenery));
                    newscenery.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
                    return newscenery;
                case TagTool.Tags.Definitions.Gen2.Weapon weapon:
                    Weapon newweapon = new Weapon();
                    TranslateTagStructure(weapon, newweapon, typeof(TagTool.Tags.Definitions.Gen2.Weapon), typeof(Weapon));
                    newweapon.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Weapon };
                    return FixupWeapon(weapon, newweapon);
                case TagTool.Tags.Definitions.Gen2.Vehicle vehicle:
                    Vehicle newvehicle = new Vehicle();
                    TranslateTagStructure(vehicle, newvehicle, typeof(TagTool.Tags.Definitions.Gen2.Vehicle), typeof(Vehicle));
                    newvehicle.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Vehicle };
                    return newvehicle;
                case TagTool.Tags.Definitions.Gen2.Projectile projectile:
                    Projectile newprojectile = new Projectile();
                    TranslateTagStructure(projectile, newprojectile, typeof(TagTool.Tags.Definitions.Gen2.Projectile), typeof(Projectile));
                    newprojectile.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Projectile };
                    return newprojectile;
                default:
                    return null;
            }
        }

        public Weapon FixupWeapon(TagTool.Tags.Definitions.Gen2.Weapon gen2Tag, Weapon newweapon)
        {
            newweapon.FirstPerson = new List<Weapon.FirstPersonBlock>();
            foreach(var firstperson in gen2Tag.PlayerInterface.FirstPerson)
            {
                newweapon.FirstPerson.Add(new Weapon.FirstPersonBlock
                {
                    FirstPersonModel = firstperson.FirstPersonModel,
                    FirstPersonAnimations = firstperson.FirstPersonAnimations
                });
            }

            return newweapon;
        }

        public Vehicle FixupVehicle(TagTool.Tags.Definitions.Gen2.Vehicle gen2Tag, Vehicle vehi)
        {
            vehi.FlipOverMessageNew = ConvertStringId(vehi.FlipOverMessageOld);
            vehi.FlipTimeNew = vehi.FlipTimeOld;
            vehi.FlippingAngularVelocityRangeNew = vehi.FlippingAngularVelocityRangeOld;
            vehi.HavokPhysicsNew = vehi.HavokPhysicsOld;

            vehi.PhysicsTypes = new Vehicle.VehiclePhysicsTypes();

            switch (vehi.PhysicsType)
            {
                case Vehicle.VehiclePhysicsType.HumanTank:
                    vehi.PhysicsTypes.HumanTank = new List<Vehicle.HumanTankPhysics>
                            {
                                new Vehicle.HumanTankPhysics
                                {
                                    ForwardArc = Angle.FromDegrees(100.0f),
                                    ForwardTurnScale = 0.4f,
                                    ReverseTurnScale = 1.0f,
                                    MaximumLeftDifferential = vehi.MaximumLeftSlide,
                                    MaximumRightDifferential = vehi.MaximumRightSlide,
                                    DifferentialAcceleration = vehi.SlideAcceleration,
                                    DifferentialDeceleration = vehi.SlideDeceleration,
                                    MaximumLeftReverseDifferential = vehi.MaximumLeftSlide,
                                    MaximumRightReverseDifferential = vehi.MaximumRightSlide,
                                    DifferentialReverseAcceleration = vehi.SlideAcceleration,
                                    DifferentialReverseDeceleration = vehi.SlideDeceleration,
                                    Engine = new Vehicle.EnginePhysics
                                    {
                                        EngineMomentum = vehi.EngineMomentum,
                                        EngineMaximumAngularVelocity = vehi.EngineMaximumAngularVelocity,
                                        Gears = vehi.Gears,
                                        GearShiftSound = null
                                    },
                                    WheelCircumference = vehi.WheelCircumference,
                                    GravityAdjust = 0.45f
                                }
                            };
                    break;

                case Vehicle.VehiclePhysicsType.HumanJeep:
                    vehi.PhysicsTypes.HumanJeep = new List<Vehicle.HumanJeepPhysics>
                            {
                                new Vehicle.HumanJeepPhysics
                                {
                                    Steering = vehi.Steering,
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = vehi.MaximumLeftTurn,
                                        MaximumRightTurn = vehi.MaximumRightTurn,
                                        TurnRate = vehi.TurnRate
                                    },
                                    Engine = new Vehicle.EnginePhysics
                                    {
                                        EngineMomentum = vehi.EngineMomentum,
                                        EngineMaximumAngularVelocity = vehi.EngineMaximumAngularVelocity,
                                        Gears = vehi.Gears
                                    },
                                    WheelCircumference = vehi.WheelCircumference,
                                    GravityAdjust = 0.8f
                                }
                            };
                    break;

                case Vehicle.VehiclePhysicsType.HumanBoat:
                    throw new NotSupportedException(vehi.PhysicsType.ToString());

                case Vehicle.VehiclePhysicsType.HumanPlane:
                    vehi.PhysicsTypes.HumanPlane = new List<Vehicle.HumanPlanePhysics>
                            {
                                new Vehicle.HumanPlanePhysics
                                {
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                        MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                        SpeedAcceleration = vehi.SpeedAcceleration,
                                        SpeedDeceleration = vehi.SpeedDeceleration,
                                        MaximumLeftSlide = vehi.MaximumLeftSlide,
                                        MaximumRightSlide = vehi.MaximumRightSlide,
                                        SlideAcceleration = vehi.SlideAcceleration,
                                        SlideDeceleration = vehi.SlideDeceleration,
                                    },
                                    MaximumUpRise = vehi.MaximumForwardSpeed,
                                    MaximumDownRise = vehi.MaximumForwardSpeed,
                                    RiseAcceleration = vehi.SpeedAcceleration,
                                    RiseDeceleration = vehi.SpeedDeceleration,
                                    FlyingTorqueScale = vehi.FlyingTorqueScale,
                                    AirFrictionDeceleration = vehi.AirFrictionDeceleration,
                                    ThrustScale = vehi.ThrustScale,
                                    TurnRateScaleWhenBoosting = 1.0f,
                                    MaximumRoll = Angle.FromDegrees(90.0f),
                                    SteeringAnimation = new Vehicle.VehicleSteeringAnimation
                                    {
                                        InterpolationScale = 0.9f,
                                        MaximumAngle = Angle.FromDegrees(15.0f)
                                    }
                                }
                            };
                    break;

                case Vehicle.VehiclePhysicsType.AlienScout:
                    vehi.PhysicsTypes.AlienScout = new List<Vehicle.AlienScoutPhysics>
                            {
                                new Vehicle.AlienScoutPhysics
                                {
                                    Steering = vehi.Steering,
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                        MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                        SpeedAcceleration = vehi.SpeedAcceleration,
                                        SpeedDeceleration = vehi.SpeedDeceleration,
                                        MaximumLeftSlide = vehi.MaximumLeftSlide,
                                        MaximumRightSlide = vehi.MaximumRightSlide,
                                        SlideAcceleration = vehi.SlideAcceleration,
                                        SlideDeceleration = vehi.SlideDeceleration,
                                    },
                                    Flags = Vehicle.AlienScoutPhysics.AlienScoutFlags.None, // TODO
                                    DragCoefficient = 0.0f,
                                    ConstantDeceleration = 0.0f,
                                    TorqueScale = 1.0f,
                                    EngineGravityFunction = new Vehicle.AnitGravityEngineFunctionStruct
                                    {// TODO
                                        ObjectFunctionDamageRegion = StringId.Invalid,
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.0f, 0.0f),
                                        EngineSpeedAcceleration = 0.0f,
                                        MaximumVehicleSpeed = 0.0f
                                    },
                                    ContrailObjectFunction = new Vehicle.AnitGravityEngineFunctionStruct
                                    {// TODO
                                        ObjectFunctionDamageRegion = StringId.Invalid,
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.0f, 0.0f),
                                        EngineSpeedAcceleration = 0.0f,
                                        MaximumVehicleSpeed = 0.0f
                                    },
                                    GearRotationSpeed = new Bounds<float>(0.0f, 0.0f), // TODO
                                    SteeringAnimation = new Vehicle.VehicleSteeringAnimation
                                    {// TODO
                                        InterpolationScale = 0.0f,
                                        MaximumAngle = Angle.FromDegrees(0.0f)
                                    }
                                }
                            };
                    break;

                case Vehicle.VehiclePhysicsType.AlienFighter:
                    vehi.PhysicsTypes.AlienFighter = new List<Vehicle.AlienFighterPhysics>
                            {
                                new Vehicle.AlienFighterPhysics
                                {
                                    Steering = vehi.Steering,
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = vehi.MaximumLeftTurn,
                                        MaximumRightTurn = vehi.MaximumRightTurn,
                                        TurnRate = vehi.TurnRate
                                    },
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                        MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                        SpeedAcceleration = vehi.SpeedAcceleration,
                                        SpeedDeceleration = vehi.SpeedDeceleration,
                                        MaximumLeftSlide = vehi.MaximumLeftSlide,
                                        MaximumRightSlide = vehi.MaximumRightSlide,
                                        SlideAcceleration = vehi.SlideAcceleration,
                                        SlideDeceleration = vehi.SlideDeceleration,
                                    },
                                    MaximumTrickFrequency = 1.0f,
                                    FlyingTorqueScale = vehi.FlyingTorqueScale,
                                    FixedGunOffset = vehi.FixedGunOffset,
                                    LoopTrickDuration = 1.8f,
                                    RollTrickDuration = 1.8f,
                                    ZeroGravitySpeed = 4.0f,
                                    FullGravitySpeed = 3.7f,
                                    StrafeBoostScale = 7.5f,
                                    OffStickDecelerationScale = 0.1f,
                                    CruisingThrottle = 0.75f,
                                    DiveSpeedScale = 0.0f
                                }
                            };
                    break;

                case Vehicle.VehiclePhysicsType.Turret:
                    vehi.PhysicsTypes.Turret = new List<Vehicle.TurretPhysics>
                            {// TODO: Determine if these fields are used
                                new Vehicle.TurretPhysics()
                            };
                    break;
            }
            return vehi;
        }       
    }
}
