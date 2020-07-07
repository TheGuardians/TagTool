using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    class MergeAnimationGraphsCommand : Command
    {
        private GameCache CacheContext { get; }
        private GameCache BlamCache;

        private HashSet<string> MergedAnimationGraphs { get; }
        private Dictionary<string, (Dictionary<string, (short, short)>, Dictionary<short, short>)> MergedAnimationData { get; }
        private int MergedAnimationGraphCount { get; set; } = 0;

        private PortTagCommand PortTag { get; }

        private Stream CacheStream { get; set; }
        private Stream BlamCacheStream { get; set; }
        private Dictionary<ResourceLocation, Stream> ResourceStreams { get; set; }

        public MergeAnimationGraphsCommand(GameCache cacheContext, GameCache blamCache, PortTagCommand portTagCommand) :
            base(false,
                "MergeAnimationGraphs",
                "",
                "MergeAnimationGraphs",
                "")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            MergedAnimationGraphs = new HashSet<string>();
            MergedAnimationData = new Dictionary<string, (Dictionary<string, (short, short)>, Dictionary<short, short>)>();
            PortTag = portTagCommand;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            MergedAnimationGraphs.Clear();

            var names = new HashSet<string>();

            foreach (var edTag in CacheContext.TagCache.TagTable)
            {
                if (edTag == null || !edTag.IsInGroup("jmad") || edTag.Name == null || names.Contains(edTag.Name))
                    continue;

                names.Add(edTag.Name);
            }

            foreach (var name in names)
            {
                var edTag = CacheContext.TagCache.GetTag(name, "jmad");

                if (edTag == null || MergedAnimationGraphs.Contains(name))
                    continue;

                foreach (var h3Tag in BlamCache.TagCache.TagTable)
                {
                    if (h3Tag == null || !h3Tag.IsInGroup("jmad"))
                        continue;

                    if (h3Tag.Name == name)
                        MergeAnimationGraphs(edTag, h3Tag);
                }
            }

            var count = MergedAnimationGraphCount;

            if (count == 0)
                Console.WriteLine("No animation graphs were merged.");
            else
                Console.WriteLine($"Merged {count} animation graph{(count == 1 ? "" : "s")} successfully.");

            return true;
        }

        private void MergeAnimationTagReferences(List<ModelAnimationGraph.AnimationTagReference> edReferences, List<ModelAnimationGraph.AnimationTagReference> h3References)
        {
            for (var i = 0; i < edReferences.Count; i++)
            {
                var edReference = edReferences[i];
                var h3Reference = h3References[i];

                if (edReference.Reference != null || h3Reference.Reference == null)
                    continue;

                var h3Tag = h3Reference.Reference;
                var h3TagName = $"{h3Tag.Name}.{h3Tag.Group.Tag.ToString()}";

                PortTag.Execute(new List<string> { h3TagName });
                edReference.Reference = PortTag.ConvertTag(CacheStream, BlamCacheStream, ResourceStreams, h3Tag);
            }
        }

        private Dictionary<string, (short, short)> MergeAnimations(CachedTag h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Animation> edAnimations)
        {
            var result = new Dictionary<string, (short, short)>(); // (h3Index, edIndex)

            foreach (var h3Animation in h3Def.Animations)
            {
                var animationName = BlamCache.StringTable.GetString(h3Animation.Name);

                if (result.ContainsKey(animationName))
                    continue;

                var edAnimation = edAnimations.Find(a => animationName == CacheContext.StringTable.GetString(a.Name));
                var edIndex = (short)(edAnimation != null ? edAnimations.IndexOf(edAnimation) : edAnimations.Count);

                result[animationName] = ((short)(edAnimation != null ? -1 : h3Def.Animations.IndexOf(h3Animation)), edIndex);

                if (edAnimation == null)
                {
                    edAnimation = (ModelAnimationGraph.Animation)PortTag.ConvertData(CacheStream, BlamCacheStream, ResourceStreams, h3Animation.DeepClone(), h3Def, h3Tag.Name);
                    edAnimations.Add(edAnimation);
                }
            }

            foreach (var entry in result)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var edAnimation = edAnimations[entry.Value.Item2];

                if (edAnimation.PreviousVariantSiblingNew != -1)
                    edAnimation.PreviousVariantSiblingNew = result[BlamCache.StringTable.GetString(h3Def.Animations[edAnimation.PreviousVariantSiblingNew].Name)].Item2;

                if (edAnimation.NextVariantSiblingNew != -1)
                    edAnimation.NextVariantSiblingNew = result[BlamCache.StringTable.GetString(h3Def.Animations[edAnimation.NextVariantSiblingNew].Name)].Item2;
            }

            return result;
        }

        private List<ModelAnimationGraph.Mode> MergeModes(CachedTag h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Mode> edModes, Dictionary<string, (short, short)> indices)
        {
            foreach (var h3Mode in h3Def.Modes)
            {
                var modeLabel = BlamCache.StringTable.GetString(h3Mode.Name);
                var edMode = edModes.Find(m => modeLabel == CacheContext.StringTable.GetString(m.Name));
                var edModeCreated = false;

                if (edMode == null)
                {
                    edMode = (ModelAnimationGraph.Mode)PortTag.ConvertData(
                        CacheStream, BlamCacheStream, ResourceStreams, h3Mode.DeepClone(), h3Def, h3Tag.Name);
                    edModes.Add(edMode);
                    edModeCreated = true;
                }

                foreach (var h3WeaponClass in h3Mode.WeaponClass)
                {
                    var weaponClassLabel = BlamCache.StringTable.GetString(h3WeaponClass.Label);
                    var edWeaponClass = edMode.WeaponClass.Find(wc => weaponClassLabel == CacheContext.StringTable.GetString(wc.Label));
                    var edWeaponClassCreated = false;

                    if (edWeaponClass == null)
                    {
                        edWeaponClass = (ModelAnimationGraph.Mode.WeaponClassBlock)PortTag.ConvertData(
                            CacheStream, BlamCacheStream, ResourceStreams, h3WeaponClass.DeepClone(), h3Def, h3Tag.Name);
                        edMode.WeaponClass.Add(edWeaponClass);
                        edWeaponClassCreated = true;
                    }

                    foreach (var h3WeaponType in h3WeaponClass.WeaponType)
                    {
                        var weaponTypeLabel = BlamCache.StringTable.GetString(h3WeaponType.Label);
                        var edWeaponType = edWeaponClass.WeaponType.Find(wt => weaponTypeLabel == CacheContext.StringTable.GetString(wt.Label));
                        var edWeaponTypeCreated = false;

                        if (edWeaponType == null)
                        {
                            edWeaponType = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock)PortTag.ConvertData(
                                CacheStream, BlamCacheStream, ResourceStreams, h3WeaponType.DeepClone(), h3Def, h3Tag.Name);
                            edWeaponClass.WeaponType.Add(edWeaponType);
                            edWeaponTypeCreated = true;
                        }

                        foreach (var h3Action in h3WeaponType.Actions)
                        {
                            var actionLabel = BlamCache.StringTable.GetString(h3Action.Label);
                            var edAction = edWeaponType.Actions.Find(a => actionLabel == CacheContext.StringTable.GetString(a.Label));
                            var edActionCreated = false;

                            if (edAction == null)
                            {
                                edAction = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)PortTag.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Action.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Actions.Add(edAction);
                                edActionCreated = true;
                            }

                            if (edActionCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edAction.GraphIndex == -1)
                                {
                                    edAction.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[edAction.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = h3Def.InheritanceList[edAction.GraphIndex].InheritedGraph;
                                    edAction.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edAction.Animation).Item2;
                                }
                            }
                        }

                        foreach (var h3Overlay in h3WeaponType.Overlays)
                        {
                            var overlayLabel = BlamCache.StringTable.GetString(h3Overlay.Label);
                            var edOverlay = edWeaponType.Overlays.Find(a => overlayLabel == CacheContext.StringTable.GetString(a.Label));
                            var edOverlayCreated = false;

                            if (edOverlay == null)
                            {
                                edOverlay = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)PortTag.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Overlay.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Overlays.Add(edOverlay);
                                edOverlayCreated = true;
                            }

                            if (edOverlayCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edOverlay.GraphIndex == -1)
                                {
                                    edOverlay.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[edOverlay.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = h3Def.InheritanceList[edOverlay.GraphIndex].InheritedGraph;
                                    edOverlay.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edOverlay.Animation).Item2;
                                }
                            }
                        }

                        foreach (var h3Damage in h3WeaponType.DeathAndDamage)
                        {
                            var damageLabel = BlamCache.StringTable.GetString(h3Damage.Label);
                            var edDamage = edWeaponType.DeathAndDamage.Find(d => damageLabel == CacheContext.StringTable.GetString(d.Label));
                            var edDamageCreated = false;

                            if (edDamage == null)
                            {
                                edDamage = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock)PortTag.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Damage.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.DeathAndDamage.Add(edDamage);
                                edDamageCreated = true;
                            }

                            if (edDamageCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                foreach (var direction in edDamage.Directions)
                                {
                                    foreach (var region in direction.Regions)
                                    {
                                        if (region.GraphIndex == -1)
                                        {
                                            region.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[region.Animation].Name)].Item2;
                                        }
                                        else
                                        {
                                            var inherited = h3Def.InheritanceList[region.GraphIndex].InheritedGraph;
                                            region.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == region.Animation).Item2;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var h3Transition in h3WeaponType.Transitions)
                        {
                            var transitionFullName = BlamCache.StringTable.GetString(h3Transition.FullName);
                            var transitionStateName = BlamCache.StringTable.GetString(h3Transition.StateName);

                            var edTransition = edWeaponType.Transitions.Find(t =>
                                transitionFullName == CacheContext.StringTable.GetString(t.FullName) &&
                                transitionStateName == CacheContext.StringTable.GetString(t.StateName));

                            var edTransitionCreated = false;

                            if (edTransition == null)
                            {
                                edTransition = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition)PortTag.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Transition.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Transitions.Add(edTransition);
                                edTransitionCreated = true;
                            }

                            foreach (var h3Destination in h3Transition.Destinations)
                            {
                                var destinationFullName = BlamCache.StringTable.GetString(h3Destination.FullName);
                                var destinationStateName = BlamCache.StringTable.GetString(h3Destination.StateName);

                                var edDestination = edTransition.Destinations.Find(t =>
                                    destinationFullName == CacheContext.StringTable.GetString(t.FullName) &&
                                    destinationStateName == CacheContext.StringTable.GetString(t.StateName));

                                var edDestinationCreated = false;

                                if (edDestination == null)
                                {
                                    edDestination = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination)PortTag.ConvertData(
                                        CacheStream, BlamCacheStream, ResourceStreams, h3Destination.DeepClone(), h3Def, h3Tag.Name);
                                    edTransition.Destinations.Add(edDestination);
                                    edDestinationCreated = true;
                                }

                                if (edDestinationCreated || edTransitionCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                                {
                                    if (edDestination.GraphIndex == -1)
                                    {
                                        edDestination.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[edDestination.Animation].Name)].Item2;
                                    }
                                    else
                                    {
                                        var inherited = h3Def.InheritanceList[edDestination.GraphIndex].InheritedGraph;
                                        edDestination.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edDestination.Animation).Item2;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var resolver = CacheContext.StringTable.Resolver;
            edModes = edModes.OrderBy(a => resolver.GetSet(a.Name)).ThenBy(a => resolver.GetIndex(a.Name)).ToList();

            foreach (var edMode in edModes)
            {
                edMode.WeaponClass = edMode.WeaponClass.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                foreach (var weaponClass in edMode.WeaponClass)
                {
                    weaponClass.WeaponType = weaponClass.WeaponType.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                    foreach (var weaponType in weaponClass.WeaponType)
                    {
                        weaponType.Actions = weaponType.Actions.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Overlays = weaponType.Overlays.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.DeathAndDamage = weaponType.DeathAndDamage.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Transitions = weaponType.Transitions.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();

                        foreach (var transition in weaponType.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();
                    }
                }
            }

            return edModes;
        }

        private void MergeAnimationGraphs(CachedTag edTag, CachedTag h3Tag)
        {
            ModelAnimationGraph edDef = null;
            ModelAnimationGraph h3Def = null;

            using (var stream = CacheContext.OpenCacheRead())
                edDef = CacheContext.Deserialize<ModelAnimationGraph>(stream, edTag);

            using (var stream = BlamCache.OpenCacheRead())
                h3Def = BlamCache.Deserialize<ModelAnimationGraph>(stream, h3Tag);

            if (edDef.ParentAnimationGraph != null && h3Def.ParentAnimationGraph != null)
                MergeAnimationGraphs(edDef.ParentAnimationGraph, h3Def.ParentAnimationGraph);

            for (var i = 0; i < h3Def.InheritanceList.Count; i++)
                MergeAnimationGraphs(edDef.InheritanceList[i].InheritedGraph, h3Def.InheritanceList[i].InheritedGraph);

            MergeAnimationTagReferences(edDef.SoundReferences, h3Def.SoundReferences);
            MergeAnimationTagReferences(edDef.EffectReferences, h3Def.EffectReferences);

            CacheStream = CacheContext.OpenCacheReadWrite();
            BlamCacheStream = BlamCache.OpenCacheRead();
            ResourceStreams = new Dictionary<ResourceLocation, Stream>();

            var animationIndices = MergeAnimations(h3Tag, h3Def, edDef.Animations);

            edDef.Modes = MergeModes(h3Tag, h3Def, edDef.Modes, animationIndices);

            //
            // Collect indices of missing resource groups
            //

            var resourceGroupIndices = new List<short>();

            foreach (var entry in animationIndices)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var h3Animation = h3Def.Animations[entry.Value.Item1];

                if (!resourceGroupIndices.Contains(h3Animation.ResourceGroupIndex))
                    resourceGroupIndices.Add(h3Animation.ResourceGroupIndex);
            }

            //
            // Add missing resource groups
            //

            var resourceGroups = new List<ModelAnimationGraph.ResourceGroup>();
            var resourceGroupData = new Dictionary<short, short>();

            for (var i = 0; i < resourceGroupIndices.Count; i++)
            {
                resourceGroups.Add(h3Def.ResourceGroups[resourceGroupIndices[i]]);

                foreach (var entry in animationIndices)
                    if (entry.Value.Item1 != -1)
                        resourceGroupData[edDef.Animations[entry.Value.Item2].ResourceGroupIndex] =
                            (edDef.Animations[entry.Value.Item2].ResourceGroupIndex = (short)(edDef.ResourceGroups.Count + i));
            }

            edDef.ResourceGroups.AddRange(PortTag.ConvertModelAnimationGraphResourceGroups(CacheStream, BlamCacheStream, ResourceStreams, resourceGroups));

            //
            // Finalize
            //

            CacheContext.Serialize(CacheStream, edTag, edDef);

            foreach (var entry in ResourceStreams)
                entry.Value.Close();

            CacheStream.Close();
            BlamCacheStream.Close();

            MergedAnimationGraphs.Add(h3Tag.Name);
            MergedAnimationData[h3Tag.Name] = (animationIndices, resourceGroupData);

            if (resourceGroups.Count > 0)
                MergedAnimationGraphCount++;
        }
    }
}