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
using TagTool.Commands.Porting;
using static TagTool.Tags.Definitions.Gen2.Effect.EffectEventBlock.ParticleSystemDefinitionBlockNew.ParticleSystemEmitterDefinitionBlock;
using static TagTool.Tags.Definitions.Scenario.MissionScene;
using static TagTool.Tags.Definitions.Gen2.Scenario.ScenarioCutsceneTitleBlock;
using TagTool.Effects;
using TagTool.Commands.Shaders;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public TagStructure ConvertEffect(object gen2Tag, object origGen2Tag, Stream stream, Stream blamStream)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.DamageEffect damage:
                    DamageEffect newdamage = new DamageEffect();
                    AutoConverter.TranslateTagStructure(damage, newdamage);
                    return newdamage;
                case TagTool.Tags.Definitions.Gen2.Effect effect:
                    return ConvertEffect(effect, (TagTool.Tags.Definitions.Gen2.Effect)origGen2Tag, stream, blamStream);
                case TagTool.Tags.Definitions.Gen2.Particle particle:
                    return ConvertParticle(particle, stream);
                case TagTool.Tags.Definitions.Gen2.ParticlePhysics pmov:
                    return ConvertParticlePhysics(pmov);
                default:
                    return null;
            }
        }

        private TagStructure ConvertParticlePhysics(TagTool.Tags.Definitions.Gen2.ParticlePhysics gen2Pmov)
        {
            ParticlePhysics pmov = new ParticlePhysics();

            pmov.Template = gen2Pmov.Template;
            pmov.Flags = (ParticlePhysics.ParticleMovementFlags)gen2Pmov.Flags;
            pmov.Movements = new List<ParticlePhysics.ParticleController>();

            foreach (var oldMove in gen2Pmov.Movements)
            {
                ParticlePhysics.ParticleController movement = new ParticlePhysics.ParticleController();
                movement.Parameters = new List<ParticlePhysics.ParticleController.ParticleControllerParameter>();
                movement.Type = (ParticlePhysics.ParticleController.ParticleMovementType)oldMove.Type;

                foreach (var param in oldMove.Parameters)
                {
                    if (movement.Parameters.Any(x => x.ParameterId == param.ParameterId))
                        continue; // prevent duplicates (there are some in h2)

                    ParticlePhysics.ParticleController.ParticleControllerParameter newParam = new ParticlePhysics.ParticleController.ParticleControllerParameter
                    {
                        ParameterId = param.ParameterId,
                        Property = ConvertEditableProperty(param.Property)
                    };

                    movement.Parameters.Add(newParam);
                }

                movement.RuntimeMConstantParameters = 7; // TODO: validation
                movement.RuntimeMUsedParticleStates = movement.ValidateUsedStates();
                pmov.Movements.Add(movement);
            }

            return pmov;
        }

        private TagStructure ConvertParticle(TagTool.Tags.Definitions.Gen2.Particle particle, Stream cacheStream)
        {
            Particle newParticle = new Particle();

            //newParticle.RenderMethod = ConvertParticleRenderMethod();
            byte[] rmOptions = new byte[] { };

            newParticle.RenderMethod = Cache.Deserialize<Particle>(cacheStream, Cache.TagCache.GetTag("fx\\particles\\atmospheric\\fiery_smoke\\smoke_fiery_small.prt3")).RenderMethod;
            // newParticle.RenderMethod = GenerateRenderMethodCommand.GenerateRenderMethod(, , (Cache as GameCacheHaloOnlineBase, cacheStream, rmOptions);

            // TODO: appearance flags conversion
            // There are some auto flags spread out below, but there are still flags from gen2 that need converting.

            newParticle.ParticleBillboardStyle = (Particle.ParticleBillboardStyleValue)particle.ParticleBillboardStyle;
            newParticle.AngleFadeRange = 30.0f;
            newParticle.MotionBlurTranslationScale = 1.0f;
            newParticle.MotionBlurRotationScale = 1.0f;
            newParticle.MotionBlurAspectScale = 0.5f;
            newParticle.ParticleModel = null; // no particle models

            // need to create an effect for these and attach via attachments block. Disabled for now
            //if (particle.AttachedParticleSystems)
            newParticle.Attachments = new List<Particle.Attachment>();


            // particle editable properties

            newParticle.AspectRatio = CreatePropertyConstant(1.0f);
            newParticle.Color = ConvertEditableProperty(particle.Color);
            newParticle.Intensity = CreatePropertyConstant(1.0f);
            newParticle.Alpha = ConvertEditableProperty(particle.Alpha);
            newParticle.FrameIndex = ConvertEditableProperty(particle.FrameIndex);
            newParticle.AnimationRate = CreatePropertyAlwaysZero();
            newParticle.PaletteAnimation = CreatePropertyAlwaysZero();

            newParticle.RuntimeMUsedParticleStates = newParticle.ValidateUsedStates();
            newParticle.GetConstantStates(out newParticle.RuntimeMConstantPerParticleProperties, 
                out newParticle.RuntimeMConstantOverTimeProperties);

            // generate gpu data
            ParticleGpu.ParticleCompileGpuData(newParticle, Cache, cacheStream);

            return newParticle;
        }

        private TagStructure ConvertEffect(TagTool.Tags.Definitions.Gen2.Effect effect, TagTool.Tags.Definitions.Gen2.Effect oldEffect, Stream cacheStream, Stream gen2Stream)
        {
            Effect newEffect = new Effect();
            newEffect.Locations = new List<Effect.Location>();
            newEffect.Events = new List<Effect.Event>();
            newEffect.ConicalDistributions = new List<Effect.ConicalDistribution>();

            newEffect.Flags = (EffectFlags)effect.Flags;
            newEffect.LoopStartEvent = effect.LoopStartEvent;

            newEffect.LoopingSound = effect.LoopingSound;
            newEffect.LocationIndex = (sbyte)effect.Location;
            newEffect.AlwaysPlayDistance = effect.AlwaysPlayDistance;
            newEffect.NeverPlayDistance = effect.NeverPlayDistance;

            foreach (var location in effect.Locations)
            {
                newEffect.Locations.Add(new Effect.Location { MarkerName = location.MarkerName });
            }

            for (int i = 0; i < effect.Events.Count; i++)
            {
                var eventBlock = effect.Events[i];
                Effect.Event newEvent = new Effect.Event();
                newEvent.Parts = new List<Effect.Event.Part>();
                newEvent.Accelerations = new List<Effect.Event.Acceleration>();
                newEvent.ParticleSystems = new List<Effect.Event.ParticleSystem>();

                newEvent.Name = Cache.StringTable.GetOrAddString($"event_{i}");
                newEvent.Flags = (Effect.Event.EventFlags)eventBlock.Flags;
                newEvent.SkipFraction = eventBlock.SkipFraction;
                newEvent.DelayBounds.Lower = eventBlock.DelayBounds.Lower;
                newEvent.DelayBounds.Upper = eventBlock.DelayBounds.Upper;
                newEvent.DurationBounds.Lower = eventBlock.DurationBounds.Lower;
                newEvent.DurationBounds.Upper = eventBlock.DurationBounds.Upper;

                foreach (var part in eventBlock.Parts)
                {
                    Effect.Event.Part newPart = new Effect.Event.Part
                    {
                        CreateInEnvironment = (EffectEnvironment)part.CreateIn,
                        CreateInDisposition = (EffectViolenceMode)part.CreateIn1,
                        PrimaryLocation = part.Location,
                        Flags = (EffectEventPartFlags)part.Flags,
                        RuntimeBaseGroupTag = part.Type.Group.Tag,
                        Type = part.Type,
                        VelocityBounds = part.VelocityBounds,
                        VelocityConeAngle = part.VelocityConeAngle,
                        AngularVelocityBounds = part.AngularVelocityBounds,
                        RadiusModifierBounds = part.RadiusModifierBounds,
                        AScalesValues = (EffectEventPartScales)part.AScalesValues,
                        BScalesValues = (EffectEventPartScales)part.BScalesValues
                    };

                    newEvent.Parts.Add(newPart);
                }

                //
                // BEAMS -- we will need to create a BeamSystem and reference that in a part.
                // low priority as it seems not much actually uses them in H2
                //

                foreach (var acceleration in eventBlock.Accelerations)
                {
                    Effect.Event.Acceleration newAcceleration = new Effect.Event.Acceleration();
                    AutoConverter.TranslateTagStructure(acceleration, newAcceleration);
                    newEvent.Accelerations.Add(newAcceleration);
                }

                for (int j = 0; j <  eventBlock.ParticleSystems.Count; j++)
                {
                    var oldPs = eventBlock.ParticleSystems[j];

                    Effect.Event.ParticleSystem ps = new Effect.Event.ParticleSystem();
                    ps.Emitters = new List<Effect.Event.ParticleSystem.Emitter>();

                    ps.Particle = oldPs.Particle;
                    ps.LocationIndex = (uint)oldPs.Location;
                    ps.CoordinateSystem = (ParticleCoordinateSystem)oldPs.CoordinateSystem;
                    ps.Environment = (EffectEnvironment)oldPs.Environment;
                    ps.Disposition = (EffectViolenceMode)oldPs.Disposition;
                    ps.CameraMode = (ParticleCameraMode)oldPs.CameraMode;
                    ps.SortBias = oldPs.SortBias;

                    // TODO: flags
                    ps.Flags |= Effect.Event.ParticleSystem.ParticleSystemFlags.TurnOffNearFadeOnEnhancedGraphics;

                    // TODO: near fade stuff
                    ps.NearRange = 10000.0f;

                    ps.LodInDistance = oldPs.LodInDistance;
                    ps.LodFeatherInDelta = oldPs.LodFeatherInDelta;
                    ps.InverseLodFeatherIn = oldPs.InverseLodFeatherIn;
                    ps.LodOutDistance = oldPs.LodOutDistance;
                    ps.LodFeatherOutDelta = oldPs.LodFeatherOutDelta;
                    ps.InverseLodFeatherOut = oldPs.InverseLodFeatherOut;

                    Particle prt3 = ps.Particle != null ? Cache.Deserialize<Particle>(cacheStream, ps.Particle) : null;
                    var oldPrt3 = oldPs.Particle != null ? Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Particle>(gen2Stream, oldEffect.Events[i].ParticleSystems[j].Particle) : null;

                    ps.RuntimeMaximumLifespan = 8.0f; // TODO: (emitters --> max(ps.RuntimeMaximumLifespan, ps.ParticleLifespan) )
                    for (int k = 0; k < oldPs.Emitters.Count; k++)
                    {
                        var oldEm = oldPs.Emitters[i];
                        Effect.Event.ParticleSystem.Emitter emitter = new Effect.Event.ParticleSystem.Emitter();

                        emitter.Name = Cache.StringTable.GetOrAddString($"emitter_{k}");
                        emitter.EmissionShape = (Effect.Event.ParticleSystem.Emitter.EmissionShapeValue)oldEm.EmissionShape;
                        emitter.EmitterFlags = Effect.Event.ParticleSystem.Emitter.FlagsValue.Postprocessed 
                            | Effect.Event.ParticleSystem.Emitter.FlagsValue.IsCpu; // CPU for now

                        emitter.BoundingRadiusEstimate = 0.5f; // TODO: calculate

                        emitter.TranslationalOffset = new Effect.Event.ParticleSystem.Emitter.TranslationalOffsetData
                        {
                            Mapping = new ParticlePropertyScalar
                            {
                                Function = new TagFunction
                                {
                                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar (always 0)
                                },

                                RuntimeMFlags = ParticlePropertyScalar.EditablePropertiesFlags.RealPoint3dType | 
                                    ParticlePropertyScalar.EditablePropertiesFlags.IsConstant | 
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantPerEntity | 
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantOverTime | 
                                    ParticlePropertyScalar.EditablePropertiesFlags.Bit3
                            },

                            StartingInterpolant = oldEm.TranslationalOffset,
                            EndingInterpolant = oldEm.TranslationalOffset
                        };
                        
                        float pitch = oldEm.RelativeDirection.PitchValue;
                        float yaw = oldEm.RelativeDirection.YawValue;

                        float sinRoll = 0.0f;
                        float cosRoll = 1.0f;
                        float sinPitch = (float)Math.Sin(pitch);
                        float cosPitch = (float)Math.Cos(pitch);
                        float sinYaw = (float)Math.Sin(yaw);
                        float cosYaw = (float)Math.Cos(yaw);

                        RealEulerAngles3d direction3d = new RealEulerAngles3d
                        {
                            YawValue = (float)Math.Atan2(sinRoll * sinPitch * cosYaw + cosRoll * sinYaw,
                                                  cosRoll * sinPitch * cosYaw - sinRoll * sinYaw),
                            PitchValue = (float)Math.Asin(cosRoll * sinPitch * sinYaw - sinRoll * cosYaw),
                            RollValue = (float)Math.Atan2(cosRoll * sinPitch * cosYaw + sinRoll * sinYaw,
                                                  sinRoll * sinPitch * sinYaw - cosRoll * cosYaw)
                        };

                        emitter.RelativeDirection = new Effect.Event.ParticleSystem.Emitter.RelativeDirectionData
                        {
                            Mapping = new ParticlePropertyScalar
                            {
                                Function = new TagFunction
                                {
                                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar (always 0)
                                },

                                RuntimeMFlags = ParticlePropertyScalar.EditablePropertiesFlags.RealVector3dType |
                                    ParticlePropertyScalar.EditablePropertiesFlags.IsConstant |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantPerEntity |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantOverTime |
                                    ParticlePropertyScalar.EditablePropertiesFlags.Bit3
                            },

                            DirectionAt0 = direction3d,
                            DirectionAt1 = direction3d
                        };

                        emitter.EmissionRadius = ConvertEditableProperty(oldEm.EmissionRadius);
                        emitter.EmissionAngle = ConvertEditableProperty(oldEm.EmissionAngle);
                        emitter.EmissionAxisAngle = CreatePropertyAlwaysZero();
                        emitter.ParticleStartingCount = CreatePropertyConstant(1.0f);
                        emitter.ParticleMaxCount = CreatePropertyAlwaysZero();
                        emitter.ParticleEmissionRate = CreatePropertyAlwaysZero();
                        emitter.ParticleLifespan = ConvertEditableProperty(oldEm.ParticleLifespan);
                        emitter.ParticleInitialVelocity = ConvertEditableProperty(oldEm.ParticleVelocity);
                        emitter.ParticleRotation = ConvertEditableProperty(oldPrt3.Rotation);
                        emitter.ParticleInitialRotationRate = CreatePropertyAlwaysZero();
                        emitter.ParticleSize = ConvertEditableProperty(oldEm.ParticleSize);
                        emitter.ParticleScale = ConvertEditableProperty(oldPrt3.Scale);
                        emitter.ParticleTint = ConvertEditableProperty(oldEm.ParticleTint);
                        emitter.ParticleAlpha = ConvertEditableProperty(oldEm.ParticleAlpha);
                        emitter.ParticleAlphaBlackPoint = CreatePropertyAlwaysZero();

                        emitter.ParticleSelfAcceleration = new Effect.Event.ParticleSystem.Emitter.ParticleSelfAccelerationData
                        {
                            Mapping = new ParticlePropertyScalar
                            {
                                Function = new TagFunction
                                {
                                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar (always 0)
                                },

                                RuntimeMFlags = ParticlePropertyScalar.EditablePropertiesFlags.IsConstant |
                                        ParticlePropertyScalar.EditablePropertiesFlags.ConstantPerEntity |
                                        ParticlePropertyScalar.EditablePropertiesFlags.ConstantOverTime |
                                        ParticlePropertyScalar.EditablePropertiesFlags.RealVector3dType |
                                        ParticlePropertyScalar.EditablePropertiesFlags.Bit3
                            },
                            StartingInterpolant = new RealVector3d(0.0f, 0.0f, 0.0f),
                            EndingInterpolant = new RealVector3d(0.0f, 0.0f, 0.0f)
                        };

                        if (oldEm.ParticlePhysics != null)
                        {
                            ParticlePhysics pmov = Cache.Deserialize<ParticlePhysics>(cacheStream, oldEm.ParticlePhysics);

                            emitter.ParticleMovement = new Effect.Event.ParticleSystem.Emitter.ParticleMovementData
                            {
                                Template = oldEm.ParticlePhysics,
                                Flags = (Effect.Event.ParticleSystem.Emitter.ParticleMovementData.PhysicsFlags)pmov.Flags,
                                Movements = new List<Effect.Event.ParticleSystem.Emitter.ParticleMovementData.Movement>()
                            };

                            foreach (var movement in pmov.Movements)
                            {
                                var newMovement = new Effect.Event.ParticleSystem.Emitter.ParticleMovementData.Movement();
                                newMovement.Parameters = new List<Effect.Event.ParticleSystem.Emitter.ParticleMovementData.Movement.Parameter>();

                                newMovement.Type = (Effect.Event.ParticleSystem.Emitter.ParticleMovementData.Movement.TypeValue)movement.Type;

                                foreach (var parameter in movement.Parameters)
                                {
                                    var newParameter = new Effect.Event.ParticleSystem.Emitter.ParticleMovementData.Movement.Parameter
                                    {
                                        ParameterId = parameter.ParameterId,
                                        Property = parameter.Property
                                    };

                                    newMovement.Parameters.Add(newParameter);
                                }

                                emitter.ParticleMovement.Movements.Add(newMovement);
                            }
                        }
                        else
                        {
                            emitter.ParticleMovement = new Effect.Event.ParticleSystem.Emitter.ParticleMovementData();
                        }

                        ps.Emitters.Add(emitter);
                        ps.Emitters.Last().RuntimeMGpu = CompileEmitterRuntimeData(ps, ps.Emitters.Count - 1, prt3);
                    }
                    ps.RuntimeMaximumLifespan = 5.0f; // TODO: calculate (system > emitters > evaluate max lifespan > find max)

                    newEvent.ParticleSystems.Add(ps);
                }

                newEffect.Events.Add(newEvent);
            }

            return newEffect;
        }

        private ParticlePropertyScalar.ParticleStates ConvertEditableState(EditablePropertyBlockGen2.VariableValue oldStates)
        {
            switch (oldStates)
            {
                case EditablePropertyBlockGen2.VariableValue.ParticleAge:
                    return ParticlePropertyScalar.ParticleStates.ParticleAge;
                case EditablePropertyBlockGen2.VariableValue.ParticleEmitTime:
                    return ParticlePropertyScalar.ParticleStates.ParticleEmissionTimeEmitter;
                case EditablePropertyBlockGen2.VariableValue.ParticleRandom1:
                    return ParticlePropertyScalar.ParticleStates.ParticleCorrelationRandom1;
                case EditablePropertyBlockGen2.VariableValue.ParticleRandom2:
                    return ParticlePropertyScalar.ParticleStates.ParticleCorrelationRandom2;
                case EditablePropertyBlockGen2.VariableValue.EmitterAge:
                    return ParticlePropertyScalar.ParticleStates.SystemAgeEmitterAge;
                case EditablePropertyBlockGen2.VariableValue.EmitterRandom1:
                    return ParticlePropertyScalar.ParticleStates.SystemCorrelationEmitterRandom1;
                case EditablePropertyBlockGen2.VariableValue.EmitterRandom2:
                    return ParticlePropertyScalar.ParticleStates.SystemCorrelationEmitterRandom2;
                case EditablePropertyBlockGen2.VariableValue.SystemLod:
                    return ParticlePropertyScalar.ParticleStates.LocationLodSystemLod;
                case EditablePropertyBlockGen2.VariableValue.GameTime:
                    return ParticlePropertyScalar.ParticleStates.GameTime;
                case EditablePropertyBlockGen2.VariableValue.EffectAScale:
                    return ParticlePropertyScalar.ParticleStates.EffectAScale;
                case EditablePropertyBlockGen2.VariableValue.EffectBScale:
                    return ParticlePropertyScalar.ParticleStates.EffectBScale;
                case EditablePropertyBlockGen2.VariableValue.ParticleRotation:
                    return ParticlePropertyScalar.ParticleStates.ParticleRotationPhysicsRotation;
                case EditablePropertyBlockGen2.VariableValue.ExplosionAnimation:
                    return ParticlePropertyScalar.ParticleStates.ExplosionAnimation;
                case EditablePropertyBlockGen2.VariableValue.ExplosionRotation:
                    return ParticlePropertyScalar.ParticleStates.ExplosionRotation;
                case EditablePropertyBlockGen2.VariableValue.ParticleRandom3:
                    return ParticlePropertyScalar.ParticleStates.ParticleCorrelationRandom3;
                case EditablePropertyBlockGen2.VariableValue.ParticleRandom4:
                    return ParticlePropertyScalar.ParticleStates.ParticleCorrelationRandom4;
                case EditablePropertyBlockGen2.VariableValue.LocationRandom:
                    return ParticlePropertyScalar.ParticleStates.LocationRandom;
                default:
                    return ParticlePropertyScalar.ParticleStates.ParticleAge;
            }
        }

        private ParticlePropertyScalar ConvertEditableProperty(EditablePropertyBlockGen2 oldProperty)
        {
            ParticlePropertyScalar property = new ParticlePropertyScalar();

            property.InputVariable = ConvertEditableState(oldProperty.InputVariable);
            property.RangeVariable = ConvertEditableState(oldProperty.RangeVariable);
            property.OutputModifier = (ParticlePropertyScalar.OutputModifierValue)oldProperty.OutputModifier;
            property.OutputModifierInput = ConvertEditableState(oldProperty.OutputModifierInput);
            property.Function = new TagFunction { Data = oldProperty.Mapping.ToArray() };

            if (property.Function.Data.Length == 0)
            {
                property.Function = new TagFunction
                {
                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar (always 0)
                };

                property.RuntimeMFlags |= ParticlePropertyScalar.EditablePropertiesFlags.IsConstant;
            }

            return property;
        }

        private ParticlePropertyScalar CreatePropertyAlwaysZero()
        {
            return new ParticlePropertyScalar
            {
                Function = new TagFunction
                {
                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar (always 0)
                },

                RuntimeMFlags = ParticlePropertyScalar.EditablePropertiesFlags.IsConstant |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantPerEntity |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantOverTime
            };
        }

        private ParticlePropertyScalar CreatePropertyConstant(float value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);

            return new ParticlePropertyScalar
            {
                Function = new TagFunction
                {
                    Data = new byte[] { 0x01, 0x30, 0x00, 0x00, byteArray[3], byteArray[2], byteArray[1], byteArray[0],
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // Constant, Scalar
                },

                RuntimeMConstantValue = value,
                RuntimeMFlags = ParticlePropertyScalar.EditablePropertiesFlags.IsConstant |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantPerEntity |
                                    ParticlePropertyScalar.EditablePropertiesFlags.ConstantOverTime |
                                    ParticlePropertyScalar.EditablePropertiesFlags.Bit4
            };
        }

        private List<RealRgbaColor> PropertyFunctionGetGpuColours(TagFunction function)
        {
            List<RealRgbaColor> result = new List<RealRgbaColor>();

            // colours are stored bgra in little endian builds
            switch (function.Data[2])
            {
                case 1:
                    result.Add(new RealRgbaColor(
                        function.Data[7] / 255.0f, 
                        function.Data[6] / 255.0f, 
                        function.Data[5] / 255.0f, 
                        function.Data[4] / 255.0f));
                    break;
                case 2:
                    result.Add(new RealRgbaColor(
                        function.Data[7] / 255.0f,
                        function.Data[6] / 255.0f,
                        function.Data[5] / 255.0f,
                        function.Data[4] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[19] / 255.0f,
                        function.Data[18] / 255.0f,
                        function.Data[17] / 255.0f,
                        function.Data[16] / 255.0f));
                    break;
                case 3:
                    result.Add(new RealRgbaColor(
                        function.Data[7] / 255.0f,
                        function.Data[6] / 255.0f,
                        function.Data[5] / 255.0f,
                        function.Data[4] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[11] / 255.0f,
                        function.Data[10] / 255.0f,
                        function.Data[9] / 255.0f,
                        function.Data[8] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[19] / 255.0f,
                        function.Data[18] / 255.0f,
                        function.Data[17] / 255.0f,
                        function.Data[16] / 255.0f));
                    break;
                case 4:
                    result.Add(new RealRgbaColor(
                        function.Data[7] / 255.0f,
                        function.Data[6] / 255.0f,
                        function.Data[5] / 255.0f,
                        function.Data[4] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[11] / 255.0f,
                        function.Data[10] / 255.0f,
                        function.Data[9] / 255.0f,
                        function.Data[8] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[15] / 255.0f,
                        function.Data[14] / 255.0f,
                        function.Data[13] / 255.0f,
                        function.Data[12] / 255.0f));
                    result.Add(new RealRgbaColor(
                        function.Data[19] / 255.0f,
                        function.Data[18] / 255.0f,
                        function.Data[17] / 255.0f,
                        function.Data[16] / 255.0f));
                    break;
                case 0: // scalar function, no colours
                    break;
            }

            return result;
        }

        private void MultiFunctionPartPackGpuData(TagFunction function, ref int dataOffset,
            ref Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function gpuFunction)
        {
            int nextSectionOffset = 0;

            gpuFunction.FunctionType.FunctionType = (float)function.Data[dataOffset];
            gpuFunction.DomainMax = BitConverter.ToSingle(function.Data, dataOffset + 4);

            if (function.Data[dataOffset] == 4)
            {
                gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, dataOffset + 8);
                gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, dataOffset + 12);
                nextSectionOffset = 16;
            }
            else if (function.Data[dataOffset] == 7)
            {
                gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, dataOffset + 8);
                gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, dataOffset + 12);
                gpuFunction.Innards[2] = BitConverter.ToSingle(function.Data, dataOffset + 16);
                gpuFunction.Innards[3] = BitConverter.ToSingle(function.Data, dataOffset + 20);
                nextSectionOffset = 24;
            }
            else if (function.Data[dataOffset] == 10)
            {
                gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, dataOffset + 8);
                gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, dataOffset + 12);
                gpuFunction.Innards[2] = BitConverter.ToSingle(function.Data, dataOffset + 16);
                gpuFunction.Innards[3] = BitConverter.ToSingle(function.Data, dataOffset + 20);
                gpuFunction.Innards[4] = BitConverter.ToSingle(function.Data, dataOffset + 24);
                gpuFunction.Innards[5] = BitConverter.ToSingle(function.Data, dataOffset + 28);
                gpuFunction.Innards[6] = BitConverter.ToSingle(function.Data, dataOffset + 32);
                nextSectionOffset = 36;
            }
            else
            {
                new TagToolWarning($"Unexpected function part type {gpuFunction.FunctionType.Type} in multipart function!");
            }

            dataOffset += nextSectionOffset;
        }

        private List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function> PropertyFunctionGetGpuFunctions(TagFunction function, int multiPartIndex)
        {
            List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function> result = new List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function>();

            Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function gpuFunction = new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function();

            gpuFunction.DomainMax = 1.0f;
            gpuFunction.FunctionType.FunctionType = (float)function.Data[0];
            gpuFunction.Flags = (float)function.Data[1];

            if (function.Data[2] != 0)
            {
                gpuFunction.RangeMin = 0.0f;
                gpuFunction.RangeMax = 1.0f;
                gpuFunction.ExclusionMin = 1.0f;
                gpuFunction.ExclusionMax = 1.0f;
            }
            else
            {
                gpuFunction.RangeMin = BitConverter.ToSingle(function.Data, 4);
                gpuFunction.RangeMax = BitConverter.ToSingle(function.Data, 8);
                gpuFunction.ExclusionMin = 1.0f; //gpuFunction.ExclusionMin = BitConverter.ToSingle(function.Data, 20);
                gpuFunction.ExclusionMax = 1.0f; //gpuFunction.ExclusionMax = BitConverter.ToSingle(function.Data, 24);
            }

            switch (function.Data[0])
            {
                case 1:
                    gpuFunction.Innards[0] = (float)multiPartIndex;
                    break;
                case 2:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x20);
                    gpuFunction.Innards[4] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x24);
                    gpuFunction.Innards[5] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x28);
                    break;
                case 3:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x20);
                    gpuFunction.Innards[4] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x24);
                    gpuFunction.Innards[5] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x28);
                    gpuFunction.Innards[6] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x28);
                    gpuFunction.Innards[5] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x30);
                    break;
                case 4:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x20);
                    gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x24);
                    break;
                case 7:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 16 * (multiPartIndex + 2) + 0x00);
                    gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, 16 * (multiPartIndex + 2) + 0x04);
                    gpuFunction.Innards[2] = BitConverter.ToSingle(function.Data, 16 * (multiPartIndex + 2) + 0x08);
                    gpuFunction.Innards[3] = BitConverter.ToSingle(function.Data, 16 * (multiPartIndex + 2) + 0x0C);
                    break;
                case 9:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x20);
                    gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x24);
                    gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, 12 * multiPartIndex + 0x28);
                    break;
                case 10:
                    gpuFunction.Innards[0] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x20);
                    gpuFunction.Innards[1] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x24);
                    gpuFunction.Innards[2] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x28);
                    gpuFunction.Innards[3] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x2C);
                    gpuFunction.Innards[4] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x30);
                    gpuFunction.Innards[5] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x34);
                    gpuFunction.Innards[6] = BitConverter.ToSingle(function.Data, 28 * (multiPartIndex + 2) + 0x38);
                    break;
                case 8: // multi part
                    // get range section
                    int sectionOffset = 0x20;
                    if (multiPartIndex > 0)
                    {
                        for (int i = 0; i < multiPartIndex; i++)
                        {
                            int partTypeOffset = sectionOffset + 4;
                            int partSize = BitConverter.ToInt32(function.Data, sectionOffset);
                            int nextPartOffset = 4;
                            for (int j = 0; j < partSize; j++)
                            {
                                byte partType = function.Data[partTypeOffset];

                                if (partType == 4)
                                {
                                    nextPartOffset += 16;
                                    partTypeOffset += 16;
                                }
                                else if (partType == 7)
                                {
                                    nextPartOffset += 24;
                                    partTypeOffset += 24;
                                }
                                else if (partType == 10)
                                {
                                    nextPartOffset += 36;
                                    partTypeOffset += 36;
                                }
                            }
                            sectionOffset += nextPartOffset;
                        }
                    }

                    int multiPartCount = BitConverter.ToInt32(function.Data, sectionOffset);
                    if (multiPartCount > 0)
                    {
                        int gpuDataOffset = sectionOffset + 4;
                        MultiFunctionPartPackGpuData(function, ref gpuDataOffset, ref gpuFunction);
                        result.Add(gpuFunction);

                        for (int i = 1; i < multiPartCount; i++)
                        {
                            Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function tempGpu = 
                                new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function();

                            MultiFunctionPartPackGpuData(function, ref gpuDataOffset, ref tempGpu);
                            result.Add(tempGpu);
                        }
                    }

                    return result;
            }

            result.Add(gpuFunction);

            return result;
        }

        private Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData CompileEmitterRuntimeData(Effect.Event.ParticleSystem particleSystem, int emitterIndex, Particle prt3)
        {
            var emitter = particleSystem.Emitters[emitterIndex];
            Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData runtimeGpu = new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData();

            // get used particle states (this validates the properties too)
            runtimeGpu.UsedParticleStates = emitter.ValidateUsedStates();

            // get constant states
            emitter.GetConstantStates(out runtimeGpu.ConstantPerParticleProperties, out runtimeGpu.ConstantOverTimeProperties);

            runtimeGpu.Properties = new List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Property>();
            runtimeGpu.Functions = new List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Function>();
            runtimeGpu.Colors = new List<Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.GpuColor>();

            if (prt3 == null)
            {
                new TagToolWarning("Particle system has no particle! States will not be compiled.");
                return runtimeGpu;
            }

            var runtimePropertyRef = new KeyValuePair<ParticlePropertyScalar, List<object>>[]
            {
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleTint, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleAlpha, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleSize, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.Color, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.Intensity, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.Alpha, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleRotation, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleScale, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.FrameIndex, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleAlphaBlackPoint, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.AspectRatio, new List<object>()),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(emitter.ParticleSelfAcceleration.Mapping, new List<object> { emitter.ParticleSelfAcceleration.StartingInterpolant, emitter.ParticleSelfAcceleration.EndingInterpolant }),
                new KeyValuePair<ParticlePropertyScalar, List<object>>(prt3.PaletteAnimation, new List<object>())
            };

            foreach (var propertyRef in runtimePropertyRef )
            {
                var property = new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.Property();

                property.MConstantValue = propertyRef.Key.RuntimeMConstantValue;

                if (!propertyRef.Key.IsConstant())
                {
                    property.MInnardsZ.FunctionIndexGreen = (byte)runtimeGpu.Functions.Count;
                    property.MInnardsZ.ModifierIndex = propertyRef.Key.OutputModifier;
                    property.MInnardsZ.InputIndexModifier = propertyRef.Key.OutputModifierInput;
                    property.MInnardsW.InputIndexGreen = propertyRef.Key.InputVariable;

                    runtimeGpu.Functions.AddRange(PropertyFunctionGetGpuFunctions(propertyRef.Key.Function, 0));

                    if ((propertyRef.Key.Function.Data[1] & 1) == 1) // ranged
                    {
                        property.MInnardsY.FunctionIndexRed = (byte)runtimeGpu.Functions.Count;
                        property.MInnardsY.InputIndexRed = propertyRef.Key.RangeVariable;
                        runtimeGpu.Functions.AddRange(PropertyFunctionGetGpuFunctions(propertyRef.Key.Function, 1));
                    }

                    property.MInnardsY.IsConstant = 0;
                }
                else
                {
                    property.MInnardsY.IsConstant = 1;
                }

                RealRgbaColor startingInterp = new RealRgbaColor();
                RealRgbaColor endingInterp = new RealRgbaColor();

                if (propertyRef.Key.IsRealPoint2d() || propertyRef.Key.IsRealPoint3d() || propertyRef.Key.IsRealVector3d())
                {
                    if (propertyRef.Key.IsRealPoint2d())
                    {
                        startingInterp.Red = ((RealPoint2d)propertyRef.Value[0]).X;
                        startingInterp.Blue = ((RealPoint2d)propertyRef.Value[0]).Y;
                        endingInterp.Red = ((RealPoint2d)propertyRef.Value[1]).X;
                        endingInterp.Blue = ((RealPoint2d)propertyRef.Value[1]).Y;
                    }
                    else if (propertyRef.Key.IsRealPoint3d())
                    {
                        startingInterp.Red = ((RealPoint3d)propertyRef.Value[0]).X;
                        startingInterp.Blue = ((RealPoint3d)propertyRef.Value[0]).Y;
                        startingInterp.Green = ((RealPoint3d)propertyRef.Value[0]).Z;
                        endingInterp.Red = ((RealPoint3d)propertyRef.Value[1]).X;
                        endingInterp.Blue = ((RealPoint3d)propertyRef.Value[1]).Y;
                        endingInterp.Green = ((RealPoint3d)propertyRef.Value[1]).Z;
                    }
                    else if (propertyRef.Key.IsRealVector3d())
                    {
                        startingInterp.Red = ((RealVector3d)propertyRef.Value[0]).I;
                        startingInterp.Blue = ((RealVector3d)propertyRef.Value[0]).J;
                        startingInterp.Green = ((RealVector3d)propertyRef.Value[0]).K;
                        endingInterp.Red = ((RealVector3d)propertyRef.Value[1]).I;
                        endingInterp.Blue = ((RealVector3d)propertyRef.Value[1]).J;
                        endingInterp.Green = ((RealVector3d)propertyRef.Value[1]).K;
                    }

                    property.MInnardsW.ColorIndexLo = (byte)runtimeGpu.Colors.Count;

                    runtimeGpu.Colors.Add(new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.GpuColor
                    {
                        Color = startingInterp
                    });

                    property.MInnardsW.ColorIndexHi = (byte)runtimeGpu.Colors.Count;

                    runtimeGpu.Colors.Add(new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.GpuColor
                    {
                        Color = endingInterp
                    });
                }
                else if (propertyRef.Key.Function.Data[2] != 0) // is_color_function
                {
                    List<RealRgbaColor> gpuColours = PropertyFunctionGetGpuColours(propertyRef.Key.Function);

                    property.MInnardsW.ColorIndexLo = (byte)runtimeGpu.Colors.Count;

                    foreach (var colour in gpuColours)
                    {
                        runtimeGpu.Colors.Add(new Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData.GpuColor
                        {
                            Color = colour
                        });
                    }

                    property.MInnardsW.ColorIndexHi = (byte)(runtimeGpu.Colors.Count - 1);
                }

                runtimeGpu.Properties.Add(property);
            }

            return runtimeGpu;
        }
    }
}
