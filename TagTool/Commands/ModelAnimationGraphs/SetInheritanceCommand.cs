using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using TagTool.Animations;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.ModelAnimationGraph;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class SetInheritanceCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private CachedTag Jmad { get; set; }

        private short Index = -1;

        public SetInheritanceCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "SetInheritance",
                  "Inherit specified animation(s) from a given ModelAnimationGraph.",

                  "SetInheritance <specifics> [index] <tag>",

                  "Set up animation inheritance from a given tag.\n"
                  +"\nSkeletal mapping is performed by name; analogous nodes without identical names require manual correction."
                  + "\nReferences to render_model markers (e.g. ModeIk) *may* require manual correction.\n"

                  + "\nSpecifics are label paths (i.e. mode:weaponclass:weapontype:action/overlay) and multiple can be given at once."
                  + "\n\t- e.g. \'combat\' \'scorpion_d:enter\' \'crouch:unarmed:any:throw_grenade\'\n"

                  + "\nSpecifics can also be keywords that represent many groups of modes."
                  + "\n\t- \'vehicles\' will inherit any new vehicle-based driver, passenger, gunner, and boarding modes."
                  )
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            List<string> specifics = new List<string> { };

            if(args.Count == 0)
                return new TagToolError(CommandError.ArgCount);

            var tagName = args.LastOrDefault();
            if (!CacheContext.TagCache.TryGetCachedTag(tagName, out CachedTag newTag))
                return new TagToolError(CommandError.TagInvalid);
            else if (newTag.Group != new TagTool.Tags.TagGroup("jmad"))
                return new TagToolError(CommandError.ArgInvalid, "Donor is not a model_animation_graph (jmad) tag");
            else
                args.RemoveAt(args.Count - 1);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());
            while(argStack.Count > 0)
            {
                var arg = argStack.Pop();

                if (short.TryParse(arg, out short givenIndex))
                    Index = givenIndex;
                else
                    specifics.Add(arg);
            }

            var foundIndex = Animation.InheritanceList.FindIndex(x => x.InheritedGraph == newTag);
            if (foundIndex == -1)
            {
                Index = (short)Animation.InheritanceList.Count;
                Animation.InheritanceList.Add(new Inheritance { InheritedGraph = newTag });
            }
            else
                Index = (short)foundIndex;

            var block = Animation.InheritanceList[Index];
            if (block.InheritedGraph == null)
                return new TagToolError(CommandError.TagInvalid);

            using (Stream cacheStream = CacheContext.OpenCacheReadWrite())
            {
                ModelAnimationGraph oldJmad = CacheContext.Deserialize<ModelAnimationGraph>(cacheStream, block.InheritedGraph);
                ModelAnimationGraph newJmad = CacheContext.Deserialize<ModelAnimationGraph>(cacheStream, newTag);

                var thisRoot = JmadHelper.GetRootNode(Animation).ZPosition;
                var newRoot = JmadHelper.GetRootNode(newJmad).ZPosition;
                block.RootZOffset = thisRoot / newRoot;

                block.Flags |= InheritanceListFlags.TightenNodes;
                block.NodeMapFlags = new List<Inheritance.NodeMapFlag>
                {
                    new Inheritance.NodeMapFlag{ LocalNodeFlags = 2071986171 },
                    new Inheritance.NodeMapFlag{ LocalNodeFlags = 1275 }
                };

                // create approximate node map from shared names
                if (block.NodeMap?.Count != newJmad.SkeletonNodes.Count)
                {
                    block.NodeMap = new List<Inheritance.NodeMapBlock>();
                    foreach (var node in newJmad.SkeletonNodes)
                        block.NodeMap.Add(new Inheritance.NodeMapBlock { });
                }

                for (int i = 0; i < newJmad.SkeletonNodes.Count; i++)
                {
                    StringId nodeName = newJmad.SkeletonNodes[i].Name;
                    short localNodeIndex = (short)Animation.SkeletonNodes.FindIndex(x => x.Name == nodeName); // -1 if not found
                    block.NodeMap[i].LocalNode = localNodeIndex;
                }

                // add all vehicle modes if requested
                if(specifics.Contains("vehicles"))
                {
                    specifics.RemoveAt(specifics.IndexOf("vehicles"));
                    List<string> currentModeNames = Animation.Modes.Select(m => CacheContext.StringTable.GetString(m.Name)).ToList();
                    foreach (var m in newJmad.Modes)
                    {
                        var modeString = CacheContext.StringTable.GetString(m.Name);
                        var vehicleChars = new string[] { "d", "g", "p", "b", "l", "f", "r" };
                        var split = modeString.Split('_');
                        
                        foreach (var c in vehicleChars)
                        {
                            if (split.Contains(c) && !currentModeNames.Contains(modeString))
                            {
                                specifics.Add(modeString);
                                break;
                            }
                        }
                    }
                }

                // transfer graph data additively
                if (specifics.Any())
                {
                    foreach (var fullLabel in specifics)
                    {
                        // check if the requested path through donor graph is valid
                        var labels = JmadHelper.GetLabelStringIDs(fullLabel, CacheContext);
                        if(labels.Count == 0 || labels.Contains(StringId.Invalid))
                            return new TagToolError(CommandError.CustomError, $"Part of the graph path {fullLabel} is invalid.");

                        var toInherit = JmadHelper.TraverseGraph(newJmad, labels);
                        if (toInherit == null)
                            return new TagToolError(CommandError.CustomError, $"\"{fullLabel}\" not defined in {tagName}");

                        // modify, replace, or create entries in recipient graph where needed
                        if (toInherit is Mode donorMode)
                        {
                            var mode = (Mode)JmadHelper.TraverseGraph(Animation, labels, true);
                            mode.WeaponClass = donorMode.WeaponClass;
                            mode.ModeIk = donorMode.ModeIk;
                            mode.FootDefaults = donorMode.FootDefaults;
                        }
                        else if (toInherit is Mode.WeaponClassBlock donorClass)
                        {
                            var wclass = (Mode.WeaponClassBlock)JmadHelper.TraverseGraph(Animation, labels, true);
                            wclass.WeaponType = donorClass.WeaponType;
                            wclass.WeaponIk = donorClass.WeaponIk;
                            wclass.SyncActionGroups = donorClass.SyncActionGroups;
                        }
                        else if (toInherit is Mode.WeaponClassBlock.WeaponTypeBlock donorType)
                        {
                            var wtype = (Mode.WeaponClassBlock.WeaponTypeBlock)JmadHelper.TraverseGraph(Animation, labels, true);
                            wtype.Set = donorType.Set;
                        }
                        else if (toInherit is Mode.WeaponClassBlock.WeaponTypeBlock.Entry donorEntry)
                        {
                            var entry = (Mode.WeaponClassBlock.WeaponTypeBlock.Entry)JmadHelper.TraverseGraph(Animation, labels, true);
                            if (entry == null)
                            {
                                var parent = (Mode.WeaponClassBlock.WeaponTypeBlock)JmadHelper.TraverseGraph(Animation, labels.GetRange(0, 3));
                                switch(JmadHelper.GetEntryType(newJmad, labels))
                                {
                                    case JmadHelper.AnimationSetEntryType.Action:
                                        parent.Set.Actions.Add(donorEntry);
                                        break;
                                    case JmadHelper.AnimationSetEntryType.Overlay:
                                        parent.Set.Overlays.Add(donorEntry);
                                        break;
                                    default:
                                        return new TagToolError(CommandError.CustomError, "Could not inherit this entry.");
                                }
                            }
                        }

                        // assign graph index
                        JmadHelper.SetGraphIndex(Animation, Index, labels);
                    }
                }
                else
                {

                }


                new SortModesCommand(CacheContext, Animation).Execute(new List<string> { });

                //if (specifics.Any())
                //{ }
                //else
                //{
                //    OverwriteInheritance(oldJmad, newJmad);
                //    block.InheritedGraph = newTag;
                //}
            }

            Console.WriteLine("Done.");
            return true;
        }

        private void OverwriteInheritance(ModelAnimationGraph oldJmad, ModelAnimationGraph newJmad)
        {
            foreach (var mode in Animation.Modes)
            {
                foreach (var wClass in mode.WeaponClass)
                {
                    foreach (var wType in wClass.WeaponType)
                    {
                        foreach (var action in wType.Set.Actions)
                        {
                            if (action.GraphIndex == Index)
                            {
                                StringId animName = oldJmad.Animations[action.Animation].Name;
                                action.Animation = (short)newJmad.Animations.FindIndex(x => x.Name == animName);
                                if (action.Animation == -1)
                                    action.Animation = GetEquivalentAnimation(newJmad, mode.Name, wClass.Label, wType.Label, action.Label);
                            }
                        }

                        foreach (var overlay in wType.Set.Overlays)
                        {
                            if (overlay.GraphIndex == Index)
                            {
                                StringId animName = oldJmad.Animations[overlay.Animation].Name;
                                overlay.Animation = (short)newJmad.Animations.FindIndex(x => x.Name == animName);
                                if (overlay.Animation == -1)
                                    overlay.Animation = GetEquivalentAnimation(newJmad, mode.Name, wClass.Label, wType.Label, overlay.Label);
                            }
                        }

                        foreach (var dd in wType.Set.DeathAndDamage)
                            foreach (var dir in dd.Directions)
                                foreach (var region in dir.Regions)
                                {
                                    if (region.GraphIndex == Index)
                                    {
                                        StringId animName = oldJmad.Animations[region.Animation].Name;
                                        region.Animation = (short)newJmad.Animations.FindIndex(x => x.Name == animName);
                                    }
                                }

                        foreach (var trs in wType.Set.Transitions)
                            foreach (var dest in trs.Destinations)
                            {
                                if (dest.GraphIndex == Index)
                                {
                                    StringId animName = oldJmad.Animations[dest.Animation].Name;
                                    dest.Animation = (short)newJmad.Animations.FindIndex(x => x.Name == animName);
                                }
                            }
                    }
                }
            }
        }

        private short GetEquivalentAnimation(ModelAnimationGraph newJmad, StringId mode, StringId weapC, StringId weapT, StringId other)
        {
            short index = -1;

            var foundType = newJmad.Modes.FirstOrDefault(x => x.Name == mode)
                ?.WeaponClass?.FirstOrDefault(x => x.Label == weapC)
                ?.WeaponType?.FirstOrDefault(x => x.Label == weapT);

            if (foundType != null)
            {
                var foundAction = foundType.Set.Actions?.FirstOrDefault(x => x.Label == other);
                if (foundAction != null)
                    index = foundAction.Animation;
                else
                {
                    var foundOverlay = foundType.Set.Overlays?.FirstOrDefault(x => x.Label == other);
                    if (foundOverlay != null)
                        index = foundOverlay.Animation;
                }
            }

            if(index != -1)
            {
                var a = false;
            }

            return index;
        }
    }
}
