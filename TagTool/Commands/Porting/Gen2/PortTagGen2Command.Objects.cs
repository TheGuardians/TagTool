using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;
using Gen2Eqip = TagTool.Tags.Definitions.Gen2.Equipment;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public TagStructure ConvertObject(object gen2Tag, Stream cacheStream)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.Crate crate:
                    Crate newcrate = new Crate();
                    AutoConverter.TranslateTagStructure(crate, newcrate);
                    newcrate.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Crate };
                    return newcrate;
                case TagTool.Tags.Definitions.Gen2.Scenery scenery:
                    Scenery newscenery = new Scenery();
                    AutoConverter.TranslateTagStructure(scenery, newscenery);
                    newscenery.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
                    newscenery = FixupScenery(scenery, newscenery, cacheStream);
                    return newscenery;
                case TagTool.Tags.Definitions.Gen2.Weapon weapon:
                    Weapon newweapon = new Weapon();
                    AutoConverter.TranslateTagStructure(weapon, newweapon);
                    newweapon.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Weapon };
                    return FixupWeapon(weapon, newweapon, cacheStream);
                case TagTool.Tags.Definitions.Gen2.Vehicle vehicle:
                    Vehicle newvehicle = new Vehicle();
                    AutoConverter.TranslateTagStructure(vehicle, newvehicle);
                    newvehicle.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Vehicle };
                    return FixupVehicle(vehicle, newvehicle);
                case TagTool.Tags.Definitions.Gen2.Projectile projectile:
                    Projectile newprojectile = new Projectile();
                    AutoConverter.TranslateTagStructure(projectile, newprojectile);
                    newprojectile.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Projectile };
                    return newprojectile;
                case TagTool.Tags.Definitions.Gen2.CameraTrack track:
                    CameraTrack newtrack = new CameraTrack();
                    AutoConverter.TranslateTagStructure(track, newtrack);
                    return newtrack;
                case TagTool.Tags.Definitions.Gen2.DeviceMachine devicemachine:
                    DeviceMachine newdevicemachine = new DeviceMachine();
                    AutoConverter.TranslateTagStructure(devicemachine, newdevicemachine);
                    newdevicemachine.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Machine };
                    return newdevicemachine;
                case TagTool.Tags.Definitions.Gen2.Equipment equipment:
                    Equipment newequipment = new Equipment();
                    AutoConverter.TranslateTagStructure(equipment, newequipment);
                    newequipment.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Equipment };
                    return FixupEquipment(equipment, newequipment);
                case TagTool.Tags.Definitions.Gen2.DeviceControl devicecontrol:
                    DeviceControl newdevicecontrol = new DeviceControl();
                    AutoConverter.TranslateTagStructure(devicecontrol, newdevicecontrol);
                    newdevicecontrol.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Control };
                    return newdevicecontrol;
                case TagTool.Tags.Definitions.Gen2.Biped biped:
                    Biped newbiped = new Biped();
                    AutoConverter.TranslateTagStructure(biped, newbiped);
                    newbiped.ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Biped };
                    return FixupBiped(biped, newbiped);
                default:
                    return null;
            }
        }

        private Equipment FixupEquipment(Gen2Eqip equipment, Equipment newequipment)
        {
            MultiplayerObjectType itemType = MultiplayerObjectType.Grenade;
            short defaultSpawnTime = 15;
            short defaultAbandonTime = 15;

            switch (equipment.PowerupType)
            {
                case Gen2Eqip.PowerupTypeValue.ActiveCamouflage:
                    {
                        newequipment.MultiplayerPowerup = new List<Equipment.MultiplayerPowerupBlock>();
                        newequipment.MultiplayerPowerup.Add(new Equipment.MultiplayerPowerupBlock
                        {
                            Flavor = Equipment.MultiplayerPowerupBlock.FlavorValue.BluePowerup
                        });
                        defaultSpawnTime = 30;
                        defaultAbandonTime = 60;
                        itemType = MultiplayerObjectType.Powerup;
                    }
                    newequipment.PickupSound = Cache.TagCache.GetTag<Sound>(@"sound\game_sfx\multiplayer\pickup_invis");
                    break;
                case Gen2Eqip.PowerupTypeValue.OverShield:
                    {
                        newequipment.MultiplayerPowerup = new List<Equipment.MultiplayerPowerupBlock>();
                        newequipment.MultiplayerPowerup.Add(new Equipment.MultiplayerPowerupBlock
                        {
                            Flavor = Equipment.MultiplayerPowerupBlock.FlavorValue.RedPowerup
                        });
                        defaultSpawnTime = 30;
                        defaultAbandonTime = 60;
                        itemType = MultiplayerObjectType.Powerup;
                    }
                    newequipment.PickupSound = Cache.TagCache.GetTag<Sound>(@"sound\game_sfx\multiplayer\pickup_invis");
                    break;
            }

            newequipment.MultiplayerObject = new List<GameObject.MultiplayerObjectBlock>();
            newequipment.MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock
            {
                Type = itemType,
                DefaultSpawnTime = defaultSpawnTime,
                DefaultAbandonTime = defaultAbandonTime
            });

            return newequipment;
        }

        public Weapon FixupWeapon(TagTool.Tags.Definitions.Gen2.Weapon gen2Tag, Weapon newweapon, Stream cacheStream)
        {
            newweapon.FirstPerson = new List<Weapon.FirstPersonBlock>();
            foreach (var firstperson in gen2Tag.PlayerInterface.FirstPerson)
            {
                newweapon.FirstPerson.Add(new Weapon.FirstPersonBlock
                {
                    FirstPersonModel = firstperson.FirstPersonModel,
                    FirstPersonAnimations = firstperson.FirstPersonAnimations
                });
            }
            newweapon.MultiplayerObject = new List<GameObject.MultiplayerObjectBlock>();
            newweapon.MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock
            {
                Type = TagTool.Tags.Definitions.Common.MultiplayerObjectType.Weapon,
                SpawnTimerType = TagTool.Tags.Definitions.Common.MultiplayerObjectSpawnTimerType.StartsOnDisturbance
            });
            newweapon.WeaponFlags = new WeaponFlags();

            newweapon.CenteredFirstPersonWeaponOffset.X = (float)gen2Tag.FirstPersonWeaponOffset.I * 2;
            newweapon.CenteredFirstPersonWeaponOffset.Y = (float)gen2Tag.FirstPersonWeaponOffset.J;
            newweapon.CenteredFirstPersonWeaponOffset.Z = (float)gen2Tag.FirstPersonWeaponOffset.K;

            newweapon.FirstPersonWeaponOffset.I = (float)gen2Tag.FirstPersonWeaponOffset.I * 2;
            newweapon.FirstPersonWeaponOffset.J = (float)gen2Tag.FirstPersonWeaponOffset.J;
            if (gen2Tag.FirstPersonWeaponOffset.K == 0) { newweapon.FirstPersonWeaponOffset.K = (float)0.02; }
            if (gen2Tag.FirstPersonWeaponOffset.K != 0) { newweapon.FirstPersonWeaponOffset.K = ((float)gen2Tag.FirstPersonWeaponOffset.K * -2); }

            if (gen2Tag.PlayerInterface.NewHudInterface != null) {
                newweapon.HudInterface = Cache.TagCacheGenHO.GetTag(gen2Tag.PlayerInterface.NewHudInterface.ToString());
            }

            AutoConverter.TranslateEnum(gen2Tag.WeaponFlags, out newweapon.WeaponFlags.NewFlags, newweapon.WeaponFlags.NewFlags.GetType());
            AutoConverter.TranslateEnum(gen2Tag.Tracking, out newweapon.Tracking, newweapon.Tracking.GetType());
            newweapon.SpecialHudVersion = Weapon.SpecialHudVersionValue.DefaultNoOutline2;

            GeneratePhysicsFromCollision(newweapon, cacheStream);

            return newweapon;
        }

        public Scenery FixupScenery(TagTool.Tags.Definitions.Gen2.Scenery gen2Tag, Scenery newscenery, Stream cacheStream)
        {
            //fixup for coll model reference in bsp physics
            if (newscenery.Model != null)
            {
                Model model = Cache.Deserialize<Model>(cacheStream, newscenery.Model);
                if (model.CollisionModel != null)
                {
                    CollisionModel coll = Cache.Deserialize<CollisionModel>(cacheStream, model.CollisionModel);
                    foreach (var region in coll.Regions)
                    {
                        foreach (var perm in region.Permutations)
                        {
                            if (perm.BspPhysics != null)
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
            AutoConverter.TranslateEnum(gen2Tag.Flags1, out vehi.UnitFlags, vehi.UnitFlags.GetType());

            vehi.HavokVehiclePhysics.PhantomShapes = new List<Vehicle.PhantomShape>();
            AutoConverter.TranslateList(gen2Tag.HavokVehiclePhysics.ShapePhantomShape, vehi.HavokVehiclePhysics.PhantomShapes);
            //this makes the antigravity work
            if (vehi.HavokVehiclePhysics.PhantomShapes.Count > 0)
                vehi.HavokVehiclePhysics.PhantomShapes[0].Flags = 1;

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
                    AutoConverter.TranslateList(gen2Tag.Gears, newtank.Engine.Gears);
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
                    AutoConverter.TranslateList(gen2Tag.Gears, newjeep.Engine.Gears);
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
                                        OverdampenCuspAngle = Angle.FromRadians(1.0f),
                                        OverdampenExponent = 1.0f
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
                                    Flags = Vehicle.AlienScoutPhysics.AlienScoutFlags.None,
                                    SpecificType = (Vehicle.AlienScoutPhysics.AlienScoutSpecificType)gen2Tag.SpecificType,
                                    DragCoefficient = 0.0f,
                                    ConstantDeceleration = 0.0f,
                                    TorqueScale = 1.0f,
                                    EngineGravityFunction = new Vehicle.AnitGravityEngineFunctionStruct
                                    {// TODO
                                        ObjectFunctionDamageRegion = Cache.StringTable.GetStringId("hull"),
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.5f, 2.0f), //ghost values
                                        EngineSpeedAcceleration = 0.5f,
                                        MaximumVehicleSpeed = 4.0f
                                    },
                                    ContrailObjectFunction = new Vehicle.AnitGravityEngineFunctionStruct
                                    {// TODO
                                        ObjectFunctionDamageRegion = Cache.StringTable.GetStringId("hull"),
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.5f, 2.0f), //ghost values
                                        EngineSpeedAcceleration = 2.5f,
                                        MaximumVehicleSpeed = 8.0f
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

            vehi.MultiplayerObject = new List<GameObject.MultiplayerObjectBlock>();
            vehi.MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock
            {
                Type = TagTool.Tags.Definitions.Common.MultiplayerObjectType.Weapon,
                SpawnTimerType = TagTool.Tags.Definitions.Common.MultiplayerObjectSpawnTimerType.StartsOnDisturbance
            });
            return vehi;
        }

        public Biped FixupBiped(TagTool.Tags.Definitions.Gen2.Biped gen2Tag, Biped newbiped)
        {
            newbiped.PreferredGunNode = gen2Tag.MoreDamnNodes.PreferredGunNode;

            if (gen2Tag.NewHudInterfaces.Count > 0 && gen2Tag.NewHudInterfaces[0].NewUnitHudInterface != null) {
                newbiped.HudInterfaces = new List<Unit.HudInterface> {
                    new Unit.HudInterface {
                        UnitHudInterface = Cache.TagCache.GetTag(gen2Tag.NewHudInterfaces[0].NewUnitHudInterface.ToString())
                    }
                };
            }

            newbiped.LockonDistance = gen2Tag.LockOnData.LockOnDistance;
            AutoConverter.TranslateEnum(gen2Tag.LockOnData.Flags, out newbiped.LockonFlags, newbiped.LockonFlags.GetType());
            newbiped.PhysicsFlags = gen2Tag.Physics.Flags;
            AutoConverter.TranslateEnum(gen2Tag.Physics.Flags.Halo2, out newbiped.PhysicsFlags.Halo3ODST, newbiped.PhysicsFlags.Halo3ODST.GetType());

            newbiped.HeightStanding = gen2Tag.Physics.HeightStanding;
            newbiped.HeightCrouching = gen2Tag.Physics.HeightCrouching;
            newbiped.Radius = gen2Tag.Physics.Radius;
            newbiped.Mass = gen2Tag.Physics.Mass;
            newbiped.LivingMaterialName = gen2Tag.Physics.LivingMaterialName;
            newbiped.DeadMaterialName = gen2Tag.Physics.DeadMaterialName;


            // Convert Physics Shapes
            newbiped.DeadSphereShapes = new List<PhysicsModel.Sphere>();
            newbiped.PillShapes = new List<PhysicsModel.Pill>();
            newbiped.SphereShapes = new List<PhysicsModel.Sphere>();

            foreach (var gen2sphere in gen2Tag.Physics.DeadSphereShapes)
            {
                newbiped.DeadSphereShapes.Add(ConvertSphere(gen2sphere));
            }
            foreach (var gen2pill in gen2Tag.Physics.PillShapes)
            {
                newbiped.PillShapes.Add(ConvertPill(gen2pill));
            }
            foreach (var gen2sphere in gen2Tag.Physics.SphereShapes)
            {
                newbiped.SphereShapes.Add(ConvertSphere(gen2sphere));
            }

            newbiped.BipedGroundPhysics = new Biped.CharacterPhysicsGroundStruct();
            newbiped.BipedFlyingPhysics = new Biped.CharacterPhysicsFlyingStruct();
            AutoConverter.TranslateTagStructure(gen2Tag.Physics.GroundPhysics, newbiped.BipedGroundPhysics);
            AutoConverter.TranslateTagStructure(gen2Tag.Physics.FlyingPhysics, newbiped.BipedFlyingPhysics);

            return newbiped;
        }

        public void GeneratePhysicsFromCollision(Weapon weapon, Stream cacheStream)
        {
            if (weapon.Model != null)
            {
                Cache.TagCacheGenHO.TryGetTag(weapon.Model.ToString(), out CachedTag weaponModel);
                Model weaponModelInstance = Cache.Deserialize<Model>(cacheStream, weaponModel);
                if (weaponModelInstance.CollisionModel != null && weaponModelInstance.PhysicsModel == null)
                {
                    Cache.TagCacheGenHO.TryGetTag(weaponModelInstance.CollisionModel.ToString(), out CachedTag weaponCollisionModel);
                    CollisionModel weaponCollisionModelInstance = Cache.Deserialize<CollisionModel>(cacheStream, weaponCollisionModel);

                    ObjConvexHullProcessor generator = new ObjConvexHullProcessor();
                    weaponModelInstance.PhysicsModel = generator.ConvertCollisionModelToPhysics(weaponCollisionModelInstance, cacheStream, weaponCollisionModel, Cache);

                    Cache.Serialize(cacheStream, weaponModel, weaponModelInstance);
                }
            }
        }
    }
}
