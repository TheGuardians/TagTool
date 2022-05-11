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
                    return FixupVehicle(vehicle, newvehicle);
                case TagTool.Tags.Definitions.Gen2.Projectile projectile:
                    Projectile newprojectile = new Projectile();
                    TranslateTagStructure(projectile, newprojectile, typeof(TagTool.Tags.Definitions.Gen2.Projectile), typeof(Projectile));
                    newprojectile.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Projectile };
                    return newprojectile;
                case TagTool.Tags.Definitions.Gen2.CameraTrack track:
                    CameraTrack newtrack = new CameraTrack();
                    TranslateTagStructure(track, newtrack, typeof(TagTool.Tags.Definitions.Gen2.CameraTrack), typeof(CameraTrack));
                    return newtrack;
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
            vehi.FlipOverMessageNew = gen2Tag.FlipMessage;
            vehi.FlipTimeNew = gen2Tag.TurnScale;
            vehi.FlippingAngularVelocityRangeNew = new Bounds<float> ( gen2Tag.MinimumFlippingAngularVelocity, gen2Tag.MaximumFlippingAngularVelocity );
            vehi.PhysicsTypes = new Vehicle.VehiclePhysicsTypes();

            switch (gen2Tag.PhysicsType)
            {
                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.HumanTank:
                    vehi.PhysicsTypes.HumanTank = new List<Vehicle.HumanTankPhysics>();
                    var newtank = new Vehicle.HumanTankPhysics
                    {
                        ForwardArc = Angle.FromDegrees(100.0f),
                        ForwardTurnScale = 0.4f,
                        ReverseTurnScale = 1.0f,
                        MaximumLeftDifferential = gen2Tag.MaximumLeftSlide,
                        MaximumRightDifferential = gen2Tag.MaximumRightSlide,
                        DifferentialAcceleration = gen2Tag.SlideAcceleration,
                        DifferentialDeceleration = gen2Tag.SlideDeceleration,
                        MaximumLeftReverseDifferential = gen2Tag.MaximumLeftSlide,
                        MaximumRightReverseDifferential = gen2Tag.MaximumRightSlide,
                        DifferentialReverseAcceleration = gen2Tag.SlideAcceleration,
                        DifferentialReverseDeceleration = gen2Tag.SlideDeceleration,
                        Engine = new Vehicle.EnginePhysics
                        {
                            EngineMomentum = gen2Tag.EngineMoment,
                            EngineMaximumAngularVelocity = gen2Tag.EngineMaxAngularVelocity,
                            Gears = new List<Vehicle.Gear>(),
                            GearShiftSound = null
                        },
                        WheelCircumference = gen2Tag.WheelCircumference,
                        GravityAdjust = 0.45f
                    };
                    TranslateList(gen2Tag.Gears, newtank.Engine.Gears, gen2Tag.Gears.GetType(), newtank.Engine.Gears.GetType());
                    vehi.PhysicsTypes.HumanTank.Add(newtank);
                    break;

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.HumanJeep:
                    vehi.PhysicsTypes.HumanJeep = new List<Vehicle.HumanJeepPhysics>();
                    var newjeep = new Vehicle.HumanJeepPhysics
                    {
                        Steering = new Vehicle.VehicleSteeringControl 
                        { 
                            OverdampenCuspAngle = gen2Tag.OverdampenCuspAngle,
                            OverdampenExponent = gen2Tag.OverdampenExponent
                        },
                        Turning = new Vehicle.VehicleTurningControl
                        {
                            MaximumLeftTurn = gen2Tag.MaximumLeftTurn,
                            MaximumRightTurn = gen2Tag.MaximumRightTurnNegative,
                            TurnRate = gen2Tag.TurnRate
                        },
                        Engine = new Vehicle.EnginePhysics
                        {
                            EngineMomentum = gen2Tag.EngineMoment,
                            EngineMaximumAngularVelocity = gen2Tag.EngineMaxAngularVelocity,
                            Gears = new List<Vehicle.Gear>()
                        },
                        WheelCircumference = gen2Tag.WheelCircumference,
                        GravityAdjust = 0.8f
                    };
                    TranslateList(gen2Tag.Gears, newjeep.Engine.Gears, gen2Tag.Gears.GetType(), newjeep.Engine.Gears.GetType());
                    vehi.PhysicsTypes.HumanJeep.Add(newjeep);
                    break;

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.HumanBoat:
                    throw new NotSupportedException(gen2Tag.PhysicsType.ToString());

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.HumanPlane:
                    vehi.PhysicsTypes.HumanPlane = new List<Vehicle.HumanPlanePhysics>
                            {
                                new Vehicle.HumanPlanePhysics
                                {
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = gen2Tag.MaximumForwardSpeed,
                                        MaximumReverseSpeed = gen2Tag.MaximumReverseSpeed,
                                        SpeedAcceleration = gen2Tag.SpeedAcceleration,
                                        SpeedDeceleration = gen2Tag.SpeedDeceleration,
                                        MaximumLeftSlide = gen2Tag.MaximumLeftSlide,
                                        MaximumRightSlide = gen2Tag.MaximumRightSlide,
                                        SlideAcceleration = gen2Tag.SlideAcceleration,
                                        SlideDeceleration = gen2Tag.SlideDeceleration,
                                    },
                                    MaximumUpRise = gen2Tag.MaximumForwardSpeed,
                                    MaximumDownRise = gen2Tag.MaximumForwardSpeed,
                                    RiseAcceleration = gen2Tag.SpeedAcceleration,
                                    RiseDeceleration = gen2Tag.SpeedDeceleration,
                                    FlyingTorqueScale = gen2Tag.FlyingTorqueScale,
                                    AirFrictionDeceleration = gen2Tag.AirFrictionDeceleration,
                                    ThrustScale = gen2Tag.ThrustScale,
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

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.AlienScout:
                    vehi.PhysicsTypes.AlienScout = new List<Vehicle.AlienScoutPhysics>
                            {
                                new Vehicle.AlienScoutPhysics
                                {
                                    Steering = new Vehicle.VehicleSteeringControl
                                    {
                                        OverdampenCuspAngle = gen2Tag.OverdampenCuspAngle,
                                        OverdampenExponent = gen2Tag.OverdampenExponent
                                    },
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = gen2Tag.MaximumForwardSpeed,
                                        MaximumReverseSpeed = gen2Tag.MaximumReverseSpeed,
                                        SpeedAcceleration = gen2Tag.SpeedAcceleration,
                                        SpeedDeceleration = gen2Tag.SpeedDeceleration,
                                        MaximumLeftSlide = gen2Tag.MaximumLeftSlide,
                                        MaximumRightSlide = gen2Tag.MaximumRightSlide,
                                        SlideAcceleration = gen2Tag.SlideAcceleration,
                                        SlideDeceleration = gen2Tag.SlideDeceleration,
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

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.AlienFighter:
                    vehi.PhysicsTypes.AlienFighter = new List<Vehicle.AlienFighterPhysics>
                            {
                                new Vehicle.AlienFighterPhysics
                                {
                                    Steering = new Vehicle.VehicleSteeringControl
                                    {
                                        OverdampenCuspAngle = gen2Tag.OverdampenCuspAngle,
                                        OverdampenExponent = gen2Tag.OverdampenExponent
                                    },
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = gen2Tag.MaximumLeftTurn,
                                        MaximumRightTurn = gen2Tag.MaximumRightTurnNegative,
                                        TurnRate = gen2Tag.TurnRate
                                    },
                                    VelocityControl = new Vehicle.VehicleVelocityControl
                                    {
                                        MaximumForwardSpeed = gen2Tag.MaximumForwardSpeed,
                                        MaximumReverseSpeed = gen2Tag.MaximumReverseSpeed,
                                        SpeedAcceleration = gen2Tag.SpeedAcceleration,
                                        SpeedDeceleration = gen2Tag.SpeedDeceleration,
                                        MaximumLeftSlide = gen2Tag.MaximumLeftSlide,
                                        MaximumRightSlide = gen2Tag.MaximumRightSlide,
                                        SlideAcceleration = gen2Tag.SlideAcceleration,
                                        SlideDeceleration = gen2Tag.SlideDeceleration,
                                    },
                                    MaximumTrickFrequency = 1.0f,
                                    FlyingTorqueScale = gen2Tag.FlyingTorqueScale,
                                    FixedGunOffset = gen2Tag.FixedGunOffset,
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

                case TagTool.Tags.Definitions.Gen2.Vehicle.TypeValue.Turret:
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
