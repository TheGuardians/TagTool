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
using ModelGen2 = TagTool.Tags.Definitions.Gen2.Model;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public Model ConvertModel(ModelGen2 gen2Model, Stream cacheStream)
        {
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
                RenderOnlySectionFlags = gen2Model.RenderOnlySectionFlags
            };

            //materials
            foreach(var gen2mat in gen2Model.Materials)
            {
                var newMaterial = new Model.Material
                {
                    Name = gen2mat.MaterialName,
                    MaterialType = (Model.Material.MaterialTypeValue)gen2mat.MaterialType,
                    DamageSectionIndex = gen2mat.DamageSection,
                    MaterialName = gen2mat.GlobalMaterialName,
                };
                var globals = Cache.Deserialize<Globals>(cacheStream, Cache.TagCache.FindFirstInGroup("matg"));
                int globalmaterialindex = globals.Materials.FindIndex(m => m.Name == newMaterial.MaterialName);
                newMaterial.GlobalMaterialIndex = globalmaterialindex == -1 ? (short)0 : (short)globalmaterialindex;
                model.Materials.Add(newMaterial);
            }

            //variants
            foreach (var gen2var in gen2Model.Variants)
            {
                var variant = new Model.Variant
                {
                    Name = gen2var.Name,
                    Regions = new List<Model.Variant.Region>(),
                    Objects = new List<Model.Variant.Object>()
                };
                TranslateList(gen2var.Objects, variant.Objects);
                foreach (var gen2reg in gen2var.Regions)
                {
                    var region = new Model.Variant.Region
                    {
                        Name = gen2reg.RegionName,
                        ParentVariant = gen2reg.ParentVariant,
                        SortOrder = (Model.Variant.Region.SortOrderValue)gen2reg.SortOrder,
                        Permutations = new List<Model.Variant.Region.Permutation>()
                    };
                    foreach (var gen2perm in gen2reg.Permutations)
                    {
                        region.Permutations.Add(new Model.Variant.Region.Permutation
                        {
                            Name = gen2perm.PermutationName,
                            Flags = (Model.Variant.Region.Permutation.FlagsValue)gen2perm.Flags,
                            Probability = gen2perm.Probability
                        });
                    }
                    variant.Regions.Add(region);
                }
                model.Variants.Add(variant);
            }

            TranslateList(gen2Model.Targets, model.Targets);
            TranslateList(gen2Model.NewDamageInfo, model.NewDamageInfo);

            //collision regions
            foreach (var gen2coll in gen2Model.CollisionRegions)
            {
                var coll = new Model.CollisionRegion
                {
                    Name = gen2coll.Name,
                    CollisionRegionIndex = gen2coll.CollisionRegionIndex,
                    PhysicsRegionIndex = gen2coll.PhysicsRegionIndex,
                    Permutations = new List<Model.CollisionRegion.Permutation>()
                };
                foreach (var gen2perm in gen2coll.Permutations)
                {
                    coll.Permutations.Add(new Model.CollisionRegion.Permutation
                    {
                        Name = gen2perm.Name,
                        Flags = (Model.CollisionRegion.Permutation.FlagsValue)gen2perm.Flags,
                        CollisionPermutationIndex = gen2perm.CollisionPermutationIndex,
                    });
                }
                model.CollisionRegions.Add(coll);
            }

            //nodes
            foreach (var gen2node in gen2Model.Nodes)
            {
                model.Nodes.Add(new Model.Node
                {
                    Name = gen2node.Name,
                    ParentNode = gen2node.ParentNode,
                    FirstChildNode = gen2node.FirstChildNode,
                    NextSiblingNode = gen2node.NextSiblingNode,
                    DefaultTranslation = gen2node.DefaultTranslation,
                    DefaultRotation = gen2node.DefaultRotation,
                    DefaultScale = gen2node.DefaultScale,
                    Inverse = gen2node.Inverse
                });
            }

            //model object data
            foreach (var gen2mod in gen2Model.ModelObjectData)
            {
                model.ModelObjectData.Add(new Model.ModelObjectDatum
                {
                    Type = (Model.ModelObjectDatum.TypeValue)gen2mod.Type,
                    Offset = gen2mod.Offset,
                    Radius = gen2mod.Radius
                });
            }

            return model;
        }
    }
}