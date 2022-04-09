using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using ModelGen2 = TagTool.Tags.Definitions.Gen2.Model;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Model ConvertModel(CachedTag tag, ModelGen2 gen2Model)
        {
            var model = new Model()
            {
                RenderModel = gen2Model.RenderModel,
                CollisionModel = gen2Model.CollisionModel,
                Animation = gen2Model.Animation,
                PhysicsModel = gen2Model.PhysicsModel,
                DisappearDistance = gen2Model.DisappearDistance,
                BeginFadeDistance = gen2Model.BeginFadeDistance,
                DecalReduceToL1SuperLow = gen2Model.ReduceToL1,
                DecalReduceToL2Low = gen2Model.ReduceToL2,
                DecalReduceToL3Medium = gen2Model.ReduceToL3,
                DecalReduceToL4High = gen2Model.ReduceToL4,
                DecalReduceToL5SuperHigh = gen2Model.ReduceToL5,
                Variants = new List<Model.Variant>(),
                Materials = new List<Model.Material>(),
                NewDamageInfo = new List<Model.GlobalDamageInfoBlock>(),
                Targets = new List<Model.Target>(),
                PrimaryDialogue = gen2Model.DefaultDialogue
            };

            // Convert Variants
            foreach (var gen2variants in gen2Model.Variants)
            {
                Model.Variant newVariant = new Model.Variant
                {
                    Name = gen2variants.Name,
                    Regions = new List<Model.Variant.Region>(),
                    Objects = new List<Model.Variant.Object>()
                };

                // Convert Regions
                foreach (var gen2regions in gen2variants.Regions)
                {
                    Model.Variant.Region newRegion = new Model.Variant.Region
                    {
                        Name = gen2regions.RegionName,
                        ParentVariant = gen2regions.ParentVariant,
                        Permutations = new List<Model.Variant.Region.Permutation>(),
                        // TODO
                        // Set SortOrder Properly
                        //SortOrder = gen2regions.SortOrder,
                        SortOrder = 0
                    };

                    // Convert Permutations
                    foreach (var gen2permutations in gen2regions.Permutations)
                    {
                        Model.Variant.Region.Permutation newPermutation = new Model.Variant.Region.Permutation
                        {
                            Name = gen2permutations.PermutationName,
                            Flags = (Model.Variant.Region.Permutation.FlagsValue)gen2permutations.Flags,
                            Probability = gen2permutations.Probability,
                            States = new List<Model.Variant.Region.Permutation.State>(),
                        };

                        // Convert Permutations
                        foreach (var gen2states in gen2permutations.States)
                        {
                            Model.Variant.Region.Permutation.State newState = new Model.Variant.Region.Permutation.State
                            {
                                Name = gen2states.PermutationName,
                                PropertyFlags = (Model.Variant.Region.Permutation.State.PropertyFlagsValue)gen2states.PropertyFlags,
                                // TODO
                                // Set States Properly
                                //State2 = gen2states.State,
                                State2 = 0,
                                LoopingEffect = gen2states.LoopingEffect,
                                LoopingEffectMarkerName = gen2states.LoopingEffectMarkerName,
                                InitialProbability = gen2states.InitialProbability
                            };

                            newPermutation.States.Add(newState);
                        }
                        newRegion.Permutations.Add(newPermutation);
                    }
                    newVariant.Regions.Add(newRegion);
                }

                // Convert Objects
                foreach (var gen2objects in gen2variants.Objects)
                {
                    Model.Variant.Object newObject = new Model.Variant.Object
                    {
                        ParentMarker = gen2objects.ParentMarker,
                        ChildMarker = gen2objects.ChildMarker,
                        ChildObject = gen2objects.ChildObject
                    };

                    newVariant.Objects.Add(newObject);
                }

                model.Variants.Add(newVariant);
            }

            // Convert Materials
            foreach (var gen2materials in gen2Model.Materials)
            {
                Model.Material newMaterial = new Model.Material
                {
                    Name = gen2materials.MaterialName,
                    DamageSectionIndex = gen2materials.DamageSection,
                    MaterialName = gen2materials.GlobalMaterialName
                };

                model.Materials.Add(newMaterial);
            }

            // Convert New Damage Info
            foreach (var gen2damageinfo in gen2Model.NewDamageInfo)
            {
                Model.GlobalDamageInfoBlock newDamageInfo = new Model.GlobalDamageInfoBlock
                {
                    Flags = (Model.GlobalDamageInfoBlock.FlagsValue)gen2damageinfo.Flags,
                    GlobalIndirectMaterialName = gen2damageinfo.GlobalIndirectMaterialName,
                    IndirectDamageSection = gen2damageinfo.IndirectDamageSection,
                    // TODO
                    // Set Collision/Response Damage Reporting Type properly
                    //CollisionDamageReportingType = gen2damageinfo.CollisionDamageReportingType,
                    //ResponseDamageReportingType = gen2damageinfo.ResponseDamageReportingType,
                    MaxVitality = gen2damageinfo.MaximumVitality,
                    MinStunDamage = gen2damageinfo.MinimumStunDamage,
                    StunTime = gen2damageinfo.StunTime,
                    RechargeTime = gen2damageinfo.RechargeTime,
                    RechargeFraction = gen2damageinfo.RechargeFraction,
                    MaxShieldVitality = gen2damageinfo.MaximumShieldVitality,
                    GlobalShieldMaterialName = gen2damageinfo.GlobalShieldMaterialName,
                    ShieldMinStunDamage = gen2damageinfo.MinimumStunDamage1,
                    ShieldStunTime = gen2damageinfo.StunTime1,
                    ShieldRechargeTime = gen2damageinfo.RechargeTime1,
                    ShieldDamagedThreshold = gen2damageinfo.ShieldDamagedThreshold,
                    ShieldDamagedEffect = gen2damageinfo.ShieldDamagedEffect,
                    ShieldDepletedEffect = gen2damageinfo.ShieldDepletedEffect,
                    ShieldRechargingEffect = gen2damageinfo.ShieldRechargingEffect,
                    DamageSections = new List<Model.GlobalDamageInfoBlock.DamageSection>(),
                    Nodes = new List<Model.GlobalDamageInfoBlock.Node>(),
                    DamageSeats = new List<Model.GlobalDamageInfoBlock.DamageSeat>(),
                    DamageConstraints = new List<Model.GlobalDamageInfoBlock.DamageConstraint>()
                };

                // Convert Damage Sections
                foreach (var gen2damagesections in gen2damageinfo.DamageSections)
                {
                    Model.GlobalDamageInfoBlock.DamageSection newDamageSection = new Model.GlobalDamageInfoBlock.DamageSection
                    {
                        Name = gen2damagesections.Name,
                        Flags = (Model.GlobalDamageInfoBlock.DamageSection.FlagsValue)gen2damagesections.Flags,
                        VitalityPercentage = gen2damagesections.VitalityPercentage,
                        InstantResponses = new List<Model.GlobalDamageInfoBlock.DamageSection.InstantResponse>(),
                        StunTime = gen2damagesections.StunTime,
                        RechargeTime = gen2damagesections.RechargeTime,
                        // TODO
                        // Convert RegionName to RuntimeIndex
                        // RessurectionRegionRuntimeIndex = gen2damagesections.ResurrectionRestoredRegionName
                        RessurectionRegionRuntimeIndex = 0
                    };

                    // Convert Instant Responses
                    foreach (var gen2instantresponses in gen2damagesections.InstantResponses)
                    {
                        Model.GlobalDamageInfoBlock.DamageSection.InstantResponse newInstantResponse = new Model.GlobalDamageInfoBlock.DamageSection.InstantResponse
                        {
                            // TODO
                            // Set Response/Damage Type properly
                            //ResponseType = gen2instantresponses.ResponseType,
                            //ConstraintDamageType = gen2instantresponses.ConstraintDamageType,
                            ResponseType = 0,
                            ConstraintDamageType = 0,
                            Flags = (Model.GlobalDamageInfoBlock.DamageSection.InstantResponse.FlagsValue)gen2instantresponses.Flags,
                            DamageThreshold = gen2instantresponses.DamageThreshold,
                            PrimaryTransitionEffect = gen2instantresponses.TransitionEffect,
                            // ??????
                            //TransitionDamageEffect = gen2instantresponses.DamageEffect,
                            Region = gen2instantresponses.Region,
                            // TODO
                            // Set NewState properly
                            //NewState = gen2instantresponses.NewState,
                            NewState = 0,
                            RuntimeRegionIndex = gen2instantresponses.RuntimeRegionIndex,
                            EffectMarkerName = gen2instantresponses.EffectMarkerName,
                            // ???????
                            //DamageEffectMarkerName = gen2instantresponses.DamageEffectMarker,
                            ResponseDelay = gen2instantresponses.ResponseDelay,
                            DelayEffect = gen2instantresponses.DelayEffect,
                            DelayEffectMarkerName = gen2instantresponses.DelayEffectMarkerName,
                            EjectingSeatLabel = gen2instantresponses.EjectingSeatLabel,
                            SkipFraction = gen2instantresponses.SkipFraction,
                            DestroyedChildObjectMarkerName = gen2instantresponses.DestroyedChildObjectMarkerName,
                            TotalDamageThreshold = gen2instantresponses.TotalDamageThreshold
                        };
                        newDamageSection.InstantResponses.Add(newInstantResponse);
                    }
                    newDamageInfo.DamageSections.Add(newDamageSection);
                }

                // Convert Damage Seats
                foreach (var gen2damageseats in gen2damageinfo.DamageSeats)
                {
                    Model.GlobalDamageInfoBlock.DamageSeat newDamageSeat = new Model.GlobalDamageInfoBlock.DamageSeat
                    {
                        SeatLabel = gen2damageseats.SeatLabel,
                        DirectDamageScale = gen2damageseats.DirectDamageScale,
                        DamageTransferFallOffRadius = gen2damageseats.DamageTransferFallOffRadius,
                        MaximumTransferDamageScale = gen2damageseats.MaximumTransferDamageScale,
                        MinimumTransferDamageScale = gen2damageseats.MinimumTransferDamageScale,
                        //RegionSpecificDamage = new List<Model.GlobalDamageInfoBlock.DamageSeat.RegionSpecificDamageBlock>()       // Tagblock not in h2
                    };
                    newDamageInfo.DamageSeats.Add(newDamageSeat);
                }

                // Convert Damage Constraints
                foreach (var gen2damageconstraints in gen2damageinfo.DamageConstraints)
                {
                    Model.GlobalDamageInfoBlock.DamageConstraint newDamageConstraint = new Model.GlobalDamageInfoBlock.DamageConstraint
                    {
                        PhysicsModelConstraintName = gen2damageconstraints.PhysicsModelConstraintName,
                        DamageConstraintName = gen2damageconstraints.DamageConstraintName,
                        DamageConstraintGroupName = gen2damageconstraints.DamageConstraintGroupName,
                        GroupProbabilityScale = gen2damageconstraints.GroupProbabilityScale
                    };
                    newDamageInfo.DamageConstraints.Add(newDamageConstraint);
                }
                model.NewDamageInfo.Add(newDamageInfo);
            }

            // Convert Targets
            foreach (var gen2targets in gen2Model.Targets)
            {
                Model.Target newTarget = new Model.Target
                {
                    MarkerName = gen2targets.MarkerName,
                    Size = gen2targets.Size,
                    ConeAngle = gen2targets.ConeAngle,
                    DamageSection = gen2targets.DamageSection,
                    Variant = gen2targets.Variant,
                    TargetingRelevance = gen2targets.TargetingRelevance,
                    // TODO
                    // Figure this out
                    //Flags = (Model.Target.TargetLockOnFlags.FlagsValue)gen2targets.LockOnData.Flags,
                    LockOnDistance = gen2targets.LockOnData.LockOnDistance
                };

                model.Targets.Add(newTarget);
            }

            return model;
        }
    }
}
