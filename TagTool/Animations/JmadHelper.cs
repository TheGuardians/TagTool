using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.ModelAnimationGraph;

namespace TagTool.Animations
{
    public class JmadHelper
    {
        private GameCache Cache;
        private ModelAnimationGraph Definition;

        public JmadHelper(GameCache cache, ModelAnimationGraph jmad)
        {
            Cache = cache;
            Definition = jmad;
        }

        //
        // root node
        //

        public static SkeletonNode GetRootNode(ModelAnimationGraph jmad)
            => jmad.SkeletonNodes.FirstOrDefault(x => x.ParentNodeIndex == -1);

        public static int GetRootNodeIndex(ModelAnimationGraph jmad)
            => jmad.SkeletonNodes.FindIndex(x => x.ParentNodeIndex == -1);

        public SkeletonNode GetRootNode() => GetRootNode(Definition);
        public int GetRootNodeIndex() => GetRootNodeIndex(Definition);

        //
        // animations
        //

        public static ModelAnimationGraph.Animation GetAnimation(ModelAnimationGraph jmad, string name, GameCache cache)
        {
            StringId id = cache.StringTable.GetStringId(name);
            if (id == StringId.Invalid)
                return null;
            else
                return jmad.Animations.FirstOrDefault(x => x.Name == id);
        }

        public static ModelAnimationGraph.Animation GetAnimation(ModelAnimationGraph jmad, StringId id)
            => jmad.Animations.FirstOrDefault(x => x.Name == id);

        public ModelAnimationGraph.Animation GetAnimation(string name)
            => GetAnimation(Definition, name, Cache);

        public ModelAnimationGraph.Animation GetAnimation(StringId id)
            => GetAnimation(Definition, id);


        //
        // traversal
        //

        public static object TraverseGraph(ModelAnimationGraph jmad, string name, GameCache cache, bool additive = false)
        {
            List<StringId> labels = GetLabelStringIDs(name, cache);

            if (labels.Any() && !labels.Contains(StringId.Invalid))
            {
                return TraverseGraph(jmad, labels, additive);
            }
            else
                return labels;
        }

        public static object TraverseGraph(ModelAnimationGraph jmad, List<StringId> labels, bool additive = false)
        {
            var mode = GetMode(jmad, labels[0], additive);
            if (labels.Count > 1)
            {
                var wClass = GetWeaponClass(mode, labels[1], additive);
                if (labels.Count > 2)
                {
                    var wType = GetWeaponType(wClass, labels[2], additive);
                    if (labels.Count > 3)
                    {
                        var action = GetAction(wType, labels[3]);
                        if (action == null)
                        {
                            var overlay = GetOverlay(wType, labels[3]);
                            if (overlay == null)
                                return null;
                            
                            return overlay;
                        }
                        return action;
                    }
                    return wType;
                }
                return wClass;
            }
            return mode;
        }

        public static List<StringId> GetLabelStringIDs(string name, GameCache cache)
        {
            List<StringId> labels = new List<StringId> { };
            var split = name.Split(':');
            foreach (var s in split)
            {
                var temp = cache.StringTable.GetStringId(s);
                if (temp == StringId.Invalid)
                {
                    labels.Add(temp);
                    return labels;
                }
                else
                    labels.Add(temp);
            }

            return labels;
        }

        public object TraverseGraph(string name, bool additive = false) => TraverseGraph(Definition, name, Cache, additive);
        public List<StringId> GetLabelStringIDs(string name) => GetLabelStringIDs(name, Cache);

        //
        // inheritance
        //

        public static void SetGraphIndex(ModelAnimationGraph jmad, short index, List<StringId> labels)
        {
            object graphObj = TraverseGraph(jmad, labels);
            if (graphObj is Mode mode)
            {
                foreach (var wclass in mode.WeaponClass)
                {
                    foreach (var wtype in wclass.WeaponType)
                        AssignAnimationSetGraphIndex(index, wtype);
                }
            }
            else if (graphObj is Mode.WeaponClassBlock wclass)
            {
                foreach (var wtype in wclass.WeaponType)
                    AssignAnimationSetGraphIndex(index, wtype);
            }
            else if (graphObj is Mode.WeaponClassBlock.WeaponTypeBlock wtype)
            {
                AssignAnimationSetGraphIndex(index, wtype);
            }
            else if (graphObj is Mode.WeaponClassBlock.WeaponTypeBlock.Entry entry)
            {
                entry.GraphIndex = index;
            }
        }

        private static void AssignAnimationSetGraphIndex(short index, Mode.WeaponClassBlock.WeaponTypeBlock weaponType)
        {
            foreach (var action in weaponType.Set.Actions)
                action.GraphIndex = index;
            foreach (var overlay in weaponType.Set.Overlays)
                overlay.GraphIndex = index;
            foreach (var dd in weaponType.Set.DeathAndDamage)
                foreach (var dir in dd.Directions)
                    foreach (var region in dir.Regions)
                        region.GraphIndex = index;
            foreach (var trs in weaponType.Set.Transitions)
                foreach (var dest in trs.Destinations)
                    dest.GraphIndex = index;
        }


        //
        // modes
        //

        public static Mode GetMode(ModelAnimationGraph jmad, StringId id, bool additive = false)
        {
            var mode = jmad.Modes.FirstOrDefault(x => x.Name == id);
            if(additive && mode == null)
            {
                jmad.Modes.Add(new Mode {
                    Name = id,
                    WeaponClass = new List<Mode.WeaponClassBlock> { }
                });
                mode = jmad.Modes.Last();
            }

            return mode;
        }

        //
        // weapon classes
        //

        public static Mode.WeaponClassBlock GetWeaponClass(Mode mode, StringId id, bool additive = false)
        {
            var wclass = mode.WeaponClass.FirstOrDefault(x => x.Label == id);
            if (additive && wclass == null)
            {
                mode.WeaponClass.Add(new Mode.WeaponClassBlock
                {
                    Label = id,
                    WeaponType = new List<Mode.WeaponClassBlock.WeaponTypeBlock> { }
                });
                wclass = mode.WeaponClass.Last();
            }
            
            return wclass;
        }

        //
        // weapon types
        //

        public static Mode.WeaponClassBlock.WeaponTypeBlock GetWeaponType(
            Mode.WeaponClassBlock wClass, StringId id, bool additive = false)
        {
            var wtype = wClass.WeaponType.FirstOrDefault(x => x.Label == id);
            if (additive && wtype == null)
            {
                wClass.WeaponType.Add(new Mode.WeaponClassBlock.WeaponTypeBlock
                {
                    Label = id,
                    Set = new Mode.WeaponClassBlock.WeaponTypeBlock.AnimationSet
                    {
                        Actions = new List<Mode.WeaponClassBlock.WeaponTypeBlock.Entry> { },
                        Overlays = new List<Mode.WeaponClassBlock.WeaponTypeBlock.Entry> { },
                        DeathAndDamage = new List<Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock> { },
                        Transitions = new List<Mode.WeaponClassBlock.WeaponTypeBlock.Transition> { }
                    }
                });
                wtype = wClass.WeaponType.Last();
            }

            return wtype;
        }

        //
        // animation set
        //

        public static Mode.WeaponClassBlock.WeaponTypeBlock.Entry GetAction(
            Mode.WeaponClassBlock.WeaponTypeBlock wType, StringId id)
            => wType.Set?.Actions?.FirstOrDefault(x => x.Label == id);

        public static Mode.WeaponClassBlock.WeaponTypeBlock.Entry GetOverlay(
            Mode.WeaponClassBlock.WeaponTypeBlock wType, StringId id)
            => wType.Set?.Overlays?.FirstOrDefault(x => x.Label == id);

        
        public static AnimationSetEntryType GetEntryType(ModelAnimationGraph jmad, List<StringId> labels)
        {
            if (labels.Count != 4)
                return AnimationSetEntryType.Invalid;

            var parent = (Mode.WeaponClassBlock.WeaponTypeBlock)TraverseGraph(jmad, labels.GetRange(0, 3));

            if (parent != null)
            {
                if (GetAction(parent, labels.Last()) != null)
                    return AnimationSetEntryType.Action;
                else if (GetOverlay(parent, labels.Last()) != null)
                    return AnimationSetEntryType.Overlay;
                // to-do: DeathAndDamage,Transitions
            }

            return AnimationSetEntryType.Invalid;
        }

        public AnimationSetEntryType GetEntryType(List<StringId> labels)
            => GetEntryType(Definition, labels);

        public enum AnimationSetEntryType
        {
            Action,
            Overlay,
            DeathAndDamage,
            Transition,
            Invalid
        }
    }
}
