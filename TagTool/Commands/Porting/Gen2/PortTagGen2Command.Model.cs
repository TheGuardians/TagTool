using System.Collections.Generic;
using System.IO;
using TagTool.Tags.Definitions;
using ModelGen2 = TagTool.Tags.Definitions.Gen2.Model;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Model ConvertModel(ModelGen2 gen2Model, Stream cacheStream)
        {
            RenderModel rendermodel = null;
            if (gen2Model.RenderModel != null) rendermodel = (RenderModel)Cache.Deserialize(cacheStream, gen2Model.RenderModel);
            var model = new Model
            {
                CollisionModel = gen2Model.CollisionModel,
                PhysicsModel = gen2Model.PhysicsModel,
                RenderModel = gen2Model.RenderModel,
                Animation = gen2Model.Animation,
                DisappearDistance = gen2Model.DisappearDistance,
                BeginFadeDistance = gen2Model.BeginFadeDistance,
                Variants = new List<Model.Variant>(),
                Materials = new List<Model.Material>(),
                NewDamageInfo = new List<Model.GlobalDamageInfoBlock>(),
                Targets = new List<Model.Target>(),
                CollisionRegions = new List<Model.CollisionRegion>(),
                Nodes = new List<Model.Node>(),
                ModelObjectData = new List<Model.ModelObjectDatum>(),
                RenderOnlyNodeFlags = gen2Model.RenderOnlyNodeFlags,
                RenderOnlySectionFlags = gen2Model.RenderOnlySectionFlags,
                RuntimeFlags = Model.RuntimeFlagsValue.ContainsRuntimeNodes,
                RuntimeNodeListChecksum = gen2Model.RuntimeNodeListChecksum
            };

            //materials
            TranslateList(gen2Model.Materials, model.Materials);

            //variants
            foreach (var gen2var in gen2Model.Variants)
            {
                var variant = new Model.Variant
                {
                    Name = gen2var.Name,
                    ModelRegionIndices = gen2var.ModelRegionIndices,
                    Regions = new List<Model.Variant.Region>(),
                    Objects = new List<Model.Variant.Object>()
                };
                foreach (var gen2reg in gen2var.Regions)
                {
                    var region = new Model.Variant.Region
                    {
                        Name = gen2reg.RegionName,
                        RenderModelRegionIndex = gen2reg.ModelRegionIndex,
                        ParentVariant = gen2reg.ParentVariant,
                        SortOrder = (Model.Variant.Region.SortOrderValue)gen2reg.SortOrder,
                        Permutations = new List<Model.Variant.Region.Permutation>()
                    };
                    foreach (var gen2perm in gen2reg.Permutations)
                    {
                        var permutation = new Model.Variant.Region.Permutation
                        {
                            Name = gen2perm.PermutationName,
                            RenderModelPermutationIndex = gen2perm.ModelPermutationIndex,
                            Flags = (Model.Variant.Region.Permutation.FlagsValue)gen2perm.Flags,
                            Probability = gen2perm.Probability,
                            States = new List<Model.Variant.Region.Permutation.State>(),
                            RuntimeStatePermutationIndices = gen2perm.RuntimeStatePermutationIndices
                        };
                        TranslateList(gen2perm.States, permutation.States);

                        // Fixups for States block
                        // Reference proper permutation index from render model in model permutation index

                        for (byte i = 0; i < permutation.States.Count; i++)
                        {
                            foreach (var h2render_region in rendermodel.Regions)
                            {
                                if (h2render_region.Name.ToString() == gen2reg.RegionName.ToString())
                                {
                                    for (byte j = 0; j < h2render_region.Permutations.Count; j++)
                                    {
                                        if (h2render_region.Permutations[j].Name.ToString() == permutation.Name.ToString())
                                        {
                                            permutation.RenderModelPermutationIndex = (sbyte)j;
                                        }
                                        if (h2render_region.Permutations[j].Name.ToString() == permutation.States[i].Name.ToString())
                                        {
                                            permutation.States[i].ModelPermutationIndex = (sbyte)j;
                                        }
                                    }
                                }
                            }
                            TranslateEnum(gen2perm.States[i].PropertyFlags, out permutation.States[i].PropertyFlags, permutation.States[i].PropertyFlags.GetType());
                        }

                        region.Permutations.Add(permutation);
                    }
                    variant.Regions.Add(region);
                }
                model.Variants.Add(variant);

                TranslateList(gen2var.Objects, variant.Objects);
            }

            TranslateList(gen2Model.Targets, model.Targets);
            // Fixup Targets
            for (byte i = 0; i < model.Targets.Count; i++)
            {
                model.Targets[i].LockOnData = new Model.TargetLockOnData();
                TranslateEnum(gen2Model.Targets[i].LockOnData.Flags, out model.Targets[i].LockOnData.Flags, model.Targets[i].LockOnData.Flags.GetType());
                model.Targets[i].LockOnData.LockOnDistance = gen2Model.Targets[i].LockOnData.LockOnDistance;
            }

            TranslateList(gen2Model.NewDamageInfo, model.NewDamageInfo);

            // Fixup NewDamageInfo
            if (gen2Model.NewDamageInfo.Count > 0)
            {
                model.NewDamageInfo[0].CollisionDamageReportingType = ConvertDamageReportingType(gen2Model.NewDamageInfo[0].CollisionDamageReportingType);
                model.NewDamageInfo[0].ResponseDamageReportingType = ConvertDamageReportingType(gen2Model.NewDamageInfo[0].ResponseDamageReportingType);
            }

            //collision regions
            TranslateList(gen2Model.CollisionRegions, model.CollisionRegions);

            //nodes
            TranslateList(gen2Model.Nodes, model.Nodes);

            //model object data
            TranslateList(gen2Model.ModelObjectData, model.ModelObjectData);

            return model;
        }
    }
}