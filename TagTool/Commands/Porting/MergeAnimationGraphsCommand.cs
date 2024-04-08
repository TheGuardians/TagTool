using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Porting;

namespace TagTool.Commands.Porting
{
    class MergeAnimationGraphsCommand : Command
    {
        private GameCache CacheContext { get; }
        private GameCache BlamCache { get; }
        private PortingContext PortContext { get; }

        private HashSet<string> MergedAnimationGraphs { get; }
        private Dictionary<string, (Dictionary<string, (short, short)>, Dictionary<short, short>)> MergedAnimationData { get; }
        private int MergedAnimationGraphCount { get; set; } = 0;

        private Stream CacheStream { get; set; }
        private Stream BlamCacheStream { get; set; }
        private Dictionary<ResourceLocation, Stream> ResourceStreams { get; set; }

        public MergeAnimationGraphsCommand(GameCache cacheContext, GameCache blamCache, PortingContext portingContext) :
            base(true,

                  "MergeAnimationGraphs",
                  "Merges all animation graphs from the blam cache to the base cache",

                  "MergeAnimationGraphs <replace> <blamTag> <edTag>",

                  "Merges all animation graphs from the blam cache to the base cache\n" +
                  "When replace is specified, existing animations in the destination animation graph will be replaced and updated\n" +
                  "BlamTag and edTag are optional, when they are specified, only the specified animation graphs are merged\n"

                )
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            MergedAnimationGraphs = new HashSet<string>();
            MergedAnimationData = new Dictionary<string, (Dictionary<string, (short, short)>, Dictionary<short, short>)>();
            PortContext = portingContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 0 || (args.Count == 1 && args[0].Equals("replace", StringComparison.OrdinalIgnoreCase)))
            {
                bool replace = args.Count == 1 && args[0].Equals("replace", StringComparison.OrdinalIgnoreCase);
                MergeAllAnimationGraphs(replace);
            }
            else if (args.Count == 2 || (args.Count == 3 && args[0].Equals("replace", StringComparison.OrdinalIgnoreCase)))
            {
                var blamTagPath = args[args.Count - 2];
                var edTagPath = args[args.Count - 1];
                var replace = args.Count == 3 && args[0].Equals("replace", StringComparison.OrdinalIgnoreCase);

                var blamTag = BlamCache.TagCache.GetTag(blamTagPath);
                var edTag = CacheContext.TagCache.GetTag(edTagPath);

                if (blamTag == null || edTag == null)
                    return new TagToolError(CommandError.TagInvalid);

                MergeAnimationGraphs(edTag, blamTag, replace);

                Console.WriteLine($"Merged animation graphs '{blamTagPath}' and '{edTagPath}' successfully.");
            }
            else
            {
                return new TagToolError(CommandError.ArgCount);
            }

            return true;
        }

        private void MergeAllAnimationGraphs(bool replace)
        {
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

                foreach (var blamTag in BlamCache.TagCache.TagTable)
                {
                    if (blamTag == null || !blamTag.IsInGroup("jmad"))
                        continue;

                    if (blamTag.Name == name)
                        MergeAnimationGraphs(edTag, blamTag, replace);
                }
            }

            var count = MergedAnimationGraphCount;

            if (count == 0)
                Console.WriteLine("No animation graphs were merged.");
            else
                Console.WriteLine($"Merged {count} animation graph{(count == 1 ? "" : "s")} successfully.");
        }

        private void MergeAnimationTagReferences(List<ModelAnimationGraph.AnimationTagReference> edReferences, List<ModelAnimationGraph.AnimationTagReference> h3References)
        {
            if (h3References.Count == 0)
                return;

            for (var i = 0; i < edReferences.Count; i++)
            {
                if (i >= h3References.Count)
                    break;

                var edReference = edReferences[i];
                var h3Reference = h3References[i];

                if (edReference.Reference != null || h3Reference.Reference == null)
                    continue;

                var h3Tag = h3Reference.Reference;

                PortContext.PortTag(h3Tag);
                edReference.Reference = PortContext.ConvertTag(CacheStream, BlamCacheStream, ResourceStreams, h3Tag);
            }
        }

        private Dictionary<string, (short, short)> MergeAnimations(CachedTag h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Animation> edAnimations, bool replace)
        {
            var result = new Dictionary<string, (short, short)>(); // (h3Index, edIndex)

            foreach (var h3Animation in h3Def.Animations)
            {
                var animationName = BlamCache.StringTable.GetString(h3Animation.Name);

                if (result.ContainsKey(animationName))
                    continue;

                var edAnimation = edAnimations.Find(a => animationName == CacheContext.StringTable.GetString(a.Name));
                var edIndex = (short)(edAnimation != null ? edAnimations.IndexOf(edAnimation) : edAnimations.Count);

                result[animationName] = ((short)(edAnimation != null && !replace ? -1 : h3Def.Animations.IndexOf(h3Animation)), edIndex);

                if (edAnimation == null || replace)
                {
                    var convertedAnimation = (ModelAnimationGraph.Animation)PortContext.ConvertData(CacheStream, BlamCacheStream, ResourceStreams, h3Animation.DeepClone(), h3Def, h3Tag.Name);

                    if (edAnimation == null)
                        edAnimations.Add(convertedAnimation);
                    else
                        edAnimations[edIndex] = convertedAnimation;
                }
            }

            foreach (var entry in result)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var edAnimation = edAnimations[entry.Value.Item2];

                if (edAnimation.AnimationData.ParentAnimation != -1)
                    edAnimation.AnimationData.ParentAnimation = result[BlamCache.StringTable.GetString(h3Def.Animations[edAnimation.AnimationData.ParentAnimation].Name)].Item2;

                if (edAnimation.AnimationData.NextAnimation != -1)
                    edAnimation.AnimationData.NextAnimation = result[BlamCache.StringTable.GetString(h3Def.Animations[edAnimation.AnimationData.NextAnimation].Name)].Item2;
            }

            return result;
        }

        private List<ModelAnimationGraph.Mode> MergeModes(CachedTag h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Mode> edModes, Dictionary<string, (short, short)> indices, bool replace)
        {
            foreach (var h3Mode in h3Def.Modes)
            {
                var modeLabel = BlamCache.StringTable.GetString(h3Mode.Name);
                var edMode = edModes.Find(m => modeLabel == CacheContext.StringTable.GetString(m.Name));
                var edModeCreated = false;

                if (edMode == null)
                {
                    edMode = (ModelAnimationGraph.Mode)PortContext.ConvertData(
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
                        edWeaponClass = (ModelAnimationGraph.Mode.WeaponClassBlock)PortContext.ConvertData(
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
                            edWeaponType = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock)PortContext.ConvertData(
                                CacheStream, BlamCacheStream, ResourceStreams, h3WeaponType.DeepClone(), h3Def, h3Tag.Name);
                            edWeaponClass.WeaponType.Add(edWeaponType);
                            edWeaponTypeCreated = true;
                        }

                        foreach (var h3Action in h3WeaponType.Set.Actions)
                        {
                            var actionLabel = BlamCache.StringTable.GetString(h3Action.Label);
                            var edAction = edWeaponType.Set.Actions.Find(a => actionLabel == CacheContext.StringTable.GetString(a.Label));
                            var edActionCreated = false;

                            if (edAction == null)
                            {
                                edAction = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)PortContext.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Action.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Set.Actions.Add(edAction);
                                edActionCreated = true;
                            }

                            if (edActionCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edAction.GraphIndex == -1)
                                {
                                    edAction.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[h3Action.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = h3Def.InheritanceList[edAction.GraphIndex].InheritedGraph;
                                    edAction.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == h3Action.Animation).Item2;
                                }
                            }
                            else if (replace)
                            {
                                var newAnimationIndex = indices[BlamCache.StringTable.GetString(h3Def.Animations[h3Action.Animation].Name)].Item2;
                                if (edAction.Animation != newAnimationIndex)
                                {
                                    edAction.Animation = newAnimationIndex;
                                }
                            }
                        }

                        foreach (var h3Overlay in h3WeaponType.Set.Overlays)
                        {
                            var overlayLabel = BlamCache.StringTable.GetString(h3Overlay.Label);
                            var edOverlay = edWeaponType.Set.Overlays.Find(a => overlayLabel == CacheContext.StringTable.GetString(a.Label));
                            var edOverlayCreated = false;

                            if (edOverlay == null)
                            {
                                edOverlay = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)PortContext.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Overlay.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Set.Overlays.Add(edOverlay);
                                edOverlayCreated = true;
                            }

                            if (edOverlayCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edOverlay.GraphIndex == -1)
                                {
                                    edOverlay.Animation = indices[BlamCache.StringTable.GetString(h3Def.Animations[h3Overlay.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = h3Def.InheritanceList[edOverlay.GraphIndex].InheritedGraph;
                                    edOverlay.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == h3Overlay.Animation).Item2;
                                }
                            }
                            else if (replace)
                            {
                                var newAnimationIndex = indices[BlamCache.StringTable.GetString(h3Def.Animations[h3Overlay.Animation].Name)].Item2;
                                if (edOverlay.Animation != newAnimationIndex)
                                {
                                    edOverlay.Animation = newAnimationIndex;
                                }
                            }
                        }

                        foreach (var h3Damage in h3WeaponType.Set.DeathAndDamage)
                        {
                            var damageLabel = BlamCache.StringTable.GetString(h3Damage.Label);
                            var edDamage = edWeaponType.Set.DeathAndDamage.Find(d => damageLabel == CacheContext.StringTable.GetString(d.Label));
                            var edDamageCreated = false;

                            if (edDamage == null)
                            {
                                edDamage = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock)PortContext.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Damage.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Set.DeathAndDamage.Add(edDamage);
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

                        foreach (var h3Transition in h3WeaponType.Set.Transitions)
                        {
                            var transitionFullName = BlamCache.StringTable.GetString(h3Transition.FullName);
                            var transitionStateName = BlamCache.StringTable.GetString(h3Transition.StateName);

                            var edTransition = edWeaponType.Set.Transitions.Find(t =>
                                transitionFullName == CacheContext.StringTable.GetString(t.FullName) &&
                                transitionStateName == CacheContext.StringTable.GetString(t.StateName));

                            var edTransitionCreated = false;

                            if (edTransition == null)
                            {
                                edTransition = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition)PortContext.ConvertData(
                                    CacheStream, BlamCacheStream, ResourceStreams, h3Transition.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Set.Transitions.Add(edTransition);
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
                                    edDestination = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination)PortContext.ConvertData(
                                        CacheStream, BlamCacheStream, ResourceStreams, h3Destination.DeepClone(), h3Def, h3Tag.Name);
                                    edTransition.Destinations.Add(edDestination);
                                    edDestinationCreated = true;
                                }

                                var newAnimationIndex = indices[BlamCache.StringTable.GetString(h3Def.Animations[h3Destination.Animation].Name)].Item2;
                                if (edDestinationCreated || (edDestination.Animation == -1 && replace) || (edDestination.Animation != newAnimationIndex && replace))
                                {
                                    edDestination.Animation = newAnimationIndex;
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
                        weaponType.Set.Actions = weaponType.Set.Actions.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Overlays = weaponType.Set.Overlays.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.DeathAndDamage = weaponType.Set.DeathAndDamage.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Transitions = weaponType.Set.Transitions.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();

                        foreach (var transition in weaponType.Set.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();
                    }
                }
            }

            return edModes;
        }

        private void MergeAnimationGraphs(CachedTag edTag, CachedTag h3Tag, bool replace)
        {
            ModelAnimationGraph edDef = null;
            ModelAnimationGraph h3Def = null;

            using (var stream = CacheContext.OpenCacheRead())
                edDef = CacheContext.Deserialize<ModelAnimationGraph>(stream, edTag);

            using (var stream = BlamCache.OpenCacheRead())
                h3Def = BlamCache.Deserialize<ModelAnimationGraph>(stream, h3Tag);

            if (edDef.ParentAnimationGraph != null && h3Def.ParentAnimationGraph != null)
                MergeAnimationGraphs(edDef.ParentAnimationGraph, h3Def.ParentAnimationGraph, replace);

            for (var i = 0; i < h3Def.InheritanceList.Count; i++)
                MergeAnimationGraphs(edDef.InheritanceList[i].InheritedGraph, h3Def.InheritanceList[i].InheritedGraph, replace);

            MergeAnimationTagReferences(edDef.SoundReferences, h3Def.SoundReferences);
            MergeAnimationTagReferences(edDef.EffectReferences, h3Def.EffectReferences);

            CacheStream = CacheContext.OpenCacheReadWrite();
            BlamCacheStream = BlamCache.OpenCacheRead();
            ResourceStreams = new Dictionary<ResourceLocation, Stream>();

            var animationIndices = MergeAnimations(h3Tag, h3Def, edDef.Animations, replace);

            edDef.Modes = MergeModes(h3Tag, h3Def, edDef.Modes, animationIndices, replace);

            //
            // Collect indices of missing resource groups
            //

            var resourceGroupIndices = new List<short>();

            foreach (var entry in animationIndices)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var h3Animation = h3Def.Animations[entry.Value.Item1];

                if (!resourceGroupIndices.Contains(h3Animation.AnimationData.ResourceGroupIndex))
                    resourceGroupIndices.Add(h3Animation.AnimationData.ResourceGroupIndex);
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
                        resourceGroupData[edDef.Animations[entry.Value.Item2].AnimationData.ResourceGroupIndex] =
                            (edDef.Animations[entry.Value.Item2].AnimationData.ResourceGroupIndex = (short)(edDef.ResourceGroups.Count + i));
            }

            edDef.ResourceGroups.AddRange(PortContext.ConvertModelAnimationGraphResourceGroups(CacheStream, BlamCacheStream, ResourceStreams, resourceGroups));

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