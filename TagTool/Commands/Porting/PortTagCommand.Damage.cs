using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Damage;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.Model;
using static TagTool.Tags.Definitions.Model.GlobalDamageInfoBlock;
using static TagTool.Tags.Definitions.Model.GlobalDamageInfoBlock.DamageSection;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand
    {
        public DamageEffect ConvertDamageEffect(DamageEffect damageEffect)
        {
            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                damageEffect.Flags = damageEffect.FlagsReach.ConvertLexical<DamageEffect.DamageFlags>();

                if(damageEffect.CustomResponseLabels.Count > 0)
                    damageEffect.CustomResponseLabel = damageEffect.CustomResponseLabels[0].CustomLabel;
            }

            return damageEffect;
        }

        public DamageResponseDefinition ConvertDamageResponseDefinition(Stream blamCacheStream, DamageResponseDefinition damageResponse)
        {
            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                foreach(var responseClass in damageResponse.Classes)
                {
                    responseClass.DirectionalFlash.Size = responseClass.DirectionalFlash.CenterSize;

                    if(responseClass.RumbleReference != null)
                    {
                        var rumble = BlamCache.Deserialize<Rumble>(blamCacheStream, responseClass.RumbleReference);
                        responseClass.Rumble = rumble;
                    }

                    if (responseClass.CameraShakeReachReference != null)
                    {
                        var cameraShake = BlamCache.Deserialize<CameraShake>(blamCacheStream, responseClass.CameraShakeReachReference);
                        responseClass.CameraShake = ConvertCameraShake(cameraShake);
                    }
                }
            }

            return damageResponse;
        }

        public CameraShake ConvertCameraShake(CameraShake cameraShake)
        {
            // TODO
            return cameraShake;
        }

        public DamageReportingType ConvertDamageReportingType(DamageReportingType damageReportingType)
        {
            string value = null;

            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    value = damageReportingType.Halo2Retail.ToString();
                    break;

                case CacheVersion.Halo3ODST:
                    if (damageReportingType.Halo3ODST == DamageReportingType.Halo3ODSTValue.ElephantTurret)
                        value = DamageReportingType.HaloOnlineValue.GuardiansUnknown.ToString();
                    else
                        value = damageReportingType.Halo3ODST.ToString();
                    break;

                case CacheVersion.Halo3Retail:
                    if (damageReportingType.Halo3Retail == DamageReportingType.Halo3RetailValue.ElephantTurret)
                        value = DamageReportingType.HaloOnlineValue.GuardiansUnknown.ToString();
                    else
                        value = damageReportingType.Halo3Retail.ToString();
                    break;
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReach11883:
                    value = damageReportingType.HaloReach.ToString();
                    break;
            }

            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                return damageReportingType;

            if (value == null || !Enum.TryParse(value, out damageReportingType.HaloOnline))
            {
                new TagToolWarning($"Unsupported Damage reporting type '{value}'. Using default.");
                damageReportingType.HaloOnline = DamageReportingType.HaloOnlineValue.GuardiansUnknown;
            }    

            return damageReportingType;
        }

        public GlobalDamageInfoBlock ConvertDamageInfoReach(OmahaDamageInfoBlock damageInfo)
        {
            // lots more to do here

            var result = new GlobalDamageInfoBlock()
            {
                Flags = damageInfo.Flags.ConvertLexical<GlobalDamageInfoBlock.FlagsValue>(),
                GlobalIndirectMaterialName = damageInfo.GlobalIndirectMaterialName,
                IndirectDamageSection = damageInfo.IndirectDamageSection,
                CollisionDamageReportingType = damageInfo.CollisionDamageReportingType,
                ResponseDamageReportingType = damageInfo.ResponseDamageReportingType,
                MaximumVitality = damageInfo.MaximumVitality,
                MinStunDamage = 0.0f,
                StunTime = 0.0f,
                RechargeTime = 0.0f,
                RechargeFraction = 0.0f,
                MaxShieldVitality = 0.0f,
                GlobalShieldMaterialName = StringId.Invalid,
                ShieldMinStunDamage = 0.0f,
                ShieldStunTime = 0.0f,
                ShieldRechargeTime = 0.0f,
                ShieldOverchargeFraction = 0.0f,
                ShieldOverchargeTime = 0.0f,
                ShieldDamagedThreshold = 0.0f,
                ShieldDamagedEffect = null,
                ShieldDepletedEffect = null,
                ShieldRechargingEffect = null,
                DamageSections = new List<DamageSection>(),
                Nodes = new List<GlobalDamageInfoBlock.Node>(),
                GlobalShieldMaterialIndex = -1,
                GlobalIndirectMaterialIndex = -1,
                RuntimeShieldRechargeVelocity = 0.0f,
                RuntimeOverchargeVelocity = 0.0f,
                RuntimeHealthRechargeVelocity = 0.0f,
                DamageSeats = new List<DamageSeat>(),
                DamageConstraints = new List<DamageConstraint>()
            };

            foreach(var section in damageInfo.DamageSections)
            {
                var newSection = new DamageSection()
                {
                    Name = section.Name,
                    Flags = section.Flags.ConvertLexical<DamageSection.FlagsValue>(),
                    VitalityPercentage = section.VitalityPercentage,
                    InstantResponses = new List<InstantResponse>(),
                    StunTime = section.StunTime,
                    RechargeTime = section.RechargeTime,
                    RuntimeRechargeVelocity = section.RuntimeRechargeVelocity,
                    ResurrectionRestoredRegionName = section.ResurrectionRestoredRegionName,
                    ResurrectionRegionRuntimeIndex = section.RuntimeResurrectionRestoredRegionIndex,
                };

                foreach(var instantResponse in section.InstantResponses)
                {
                    var newInstantResponse = new InstantResponse()
                    {
                        ResponseType = InstantResponse.ResponseTypeValue.RecievesAllDamage,
                        ConstraintDamageType = InstantResponse.ConstraintDamageTypeValue.None,
                        ConstraintGroupName = StringId.Invalid,
                        Flags = instantResponse.Flags.ConvertLexical<InstantResponse.FlagsValue>(),
                        DamageThreshold = instantResponse.DamageThreshold,
                        BodyThresholdFlags = 0,
                        BodyDamageThreshold = instantResponse.DamageThreshold,
                        PrimaryTransitionEffect = instantResponse.GenericTransitionEffect,
                        SecondaryTransitionEffect = instantResponse.SpecificTransitionEffect,
                        TransitionDamageEffect = instantResponse.TransitionDamageEffect,
                        Region = StringId.Invalid,
                        NewState = InstantResponse.NewStateValue.Default,
                        RuntimeRegionIndex = -1,
                        SecondaryRegion = StringId.Invalid,
                        SecondaryNewState = InstantResponse.NewStateValue.Default,
                        SecondaryRuntimeRegionIndex = -1,
                        DestroyInstanceGroup = instantResponse.DestroyInstanceGroupIndex,
                        CustomResponseBehavior = instantResponse.CustomResponseBehavior.ConvertLexical <InstantResponse.CustomResponseBehaviorValue>(),
                        CustomResponseLabel = instantResponse.CustomResponseLabel,
                        EffectMarkerName = instantResponse.GenericEffectMarker,
                        DamageEffectMarkerName = instantResponse.DamageEffectMarkerName,
                        ResponseDelay = instantResponse.ResponseDelay,
                        DelayEffect = instantResponse.DelayEffect,
                        DelayEffectMarkerName = instantResponse.DelayEffectMarkerName,
                        EjectingSeatLabel = StringId.Invalid,
                        SkipFraction = instantResponse.SkipFraction,
                        DestroyedChildObjectMarkerName = instantResponse.DestroyedChildObjectMarkerName,
                        TotalDamageThreshold = instantResponse.TotalDamageThreshold,
                    };

                    newSection.InstantResponses.Add(newInstantResponse);
                }

                result.DamageSections.Add(newSection);
            }

            foreach(var node in damageInfo.Nodes)
            {
                var newNode = new GlobalDamageInfoBlock.Node()
                {
                    RuntimeDamagePart = node.RuntimeDamagePart,
                };
                result.Nodes.Add(newNode);
            }

            foreach (var constraint in damageInfo.DamageConstraints)
            {
                var newConstraint = new DamageConstraint()
                {
                    PhysicsModelConstraintName = constraint.PhysicsModelConstraintName,
                    DamageConstraintName = constraint.DamageConstraintName,
                    DamageConstraintGroupName = constraint.DamageConstraintGroupName,
                    GroupProbabilityScale = constraint.GroupProbabilityScale,
                    Type = constraint.Type.ConvertLexical<DamageConstraint.TypeValue>(),
                    Index = constraint.Index
                };
                result.DamageConstraints.Add(newConstraint);
            }

            return result;
        }
    }
}
