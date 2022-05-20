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
        public object ConvertObject(object gen2Tag, Stream cacheStream)
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
                    newscenery = FixupScenery(scenery, newscenery, cacheStream);
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

        public Scenery FixupScenery(TagTool.Tags.Definitions.Gen2.Scenery gen2Tag, Scenery newscenery, Stream cacheStream)
        {
            //fixup for coll model reference in bsp physics
            if(newscenery.Model != null)
            {
                Model model = Cache.Deserialize<Model>(cacheStream, newscenery.Model);
                if(model.CollisionModel != null)
                {
                    CollisionModel coll = Cache.Deserialize<CollisionModel>(cacheStream, model.CollisionModel);
                    foreach(var region in coll.Regions)
                    {
                        foreach(var perm in region.Permutations)
                        {
                            if(perm.BspPhysics != null)
                            {
                                foreach (var bsp in perm.BspPhysics)
                                {
                                    bsp.GeometryShape.Model = newscenery.Model;
                                }
                            }
                        }
                    }
                    Cache.Serialize(cacheStream, model.CollisionModel, coll);
                }
            }
            return newscenery;
        }

        public Vehicle FixupVehicle(TagTool.Tags.Definitions.Gen2.Vehicle gen2Tag, Vehicle vehi)
        {
            vehi.Boost.BoostDeadTime = 0.1f;
            vehi.FlipOverMessageNew = gen2Tag.FlipMessage;
            vehi.FlipTimeNew = gen2Tag.TurnScale;
            vehi.FlippingAngularVelocityRangeNew = new Bounds<float>(gen2Tag.MinimumFlippingAngularVelocity, gen2Tag.MaximumFlippingAngularVelocity);
            vehi.PhysicsTypes = new Vehicle.VehiclePhysicsTypes();
            vehi.UnitFlags = (TagTool.Tags.Definitions.Unit.UnitFlagBits)gen2Tag.Flags1;

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

                    // Convert Gravity Scale from H2 to the speed the vehicle
                    float GravitySpeed = (float)9.8 - (float)9.8 * gen2Tag.HavokVehiclePhysics.GravityScale;

                    // Convert Degrees to Radians (H2's fields use degrees while HO uses radians)
                    float Converted_LeftTurn = gen2Tag.MaximumLeftTurn * (float)3.14 / (float)180;
                    float Converted_RightTurn = gen2Tag.MaximumRightTurnNegative * (float)3.14 / (float)180;
                    float Converted_TurnRate = gen2Tag.TurnRate * (float)3.14 / (float)180;
                    Angle Converted_OverdampenCuspAngle = Angle.FromDegrees(gen2Tag.OverdampenCuspAngle.Radians);
                    RealEulerAngles2d Converted_FixedGunOffset = new RealEulerAngles2d
                    {
                        PitchValue = (gen2Tag.FixedGunOffset.PitchValue * (float)3.14 / (float)180) / -10
                    };

                    vehi.PhysicsTypes.AlienFighter = new List<Vehicle.AlienFighterPhysics>
                            {
                                new Vehicle.AlienFighterPhysics
                                {
                                    Steering = new Vehicle.VehicleSteeringControl
                                    {
                                        OverdampenCuspAngle = Converted_OverdampenCuspAngle,
                                        OverdampenExponent = gen2Tag.OverdampenExponent
                                    },
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = (int)Converted_LeftTurn,
                                        MaximumRightTurn = (int)Converted_RightTurn,
                                        TurnRate = (int)Converted_TurnRate
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
                                    MaximumTrickFrequency = 2.0f,
                                    FlyingTorqueScale = gen2Tag.FlyingTorqueScale,
                                    FixedGunOffset = Converted_FixedGunOffset,
                                    LoopTrickDuration = 2.0f,
                                    RollTrickDuration = 2.0f,
                                    ZeroGravitySpeed = GravitySpeed,
                                    FullGravitySpeed = GravitySpeed,
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
