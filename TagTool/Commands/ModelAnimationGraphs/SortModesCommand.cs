using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ModelAnimationGraphs
{
    class SortModesCommand : Command
    {
        private GameCache Cache { get; }
        private ModelAnimationGraph Definition { get; }

        public SortModesCommand(GameCache cache, ModelAnimationGraph definition) :
            base(false,
                
                "SortModes",
                "Sorts all \"modes\" block elements in the current model_animation_graph based on their name's string_id set and index.",
                
                "SortModes",

                "Sorts all \"modes\" block elements in the current model_animation_graph based on their name's string_id set and index.")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            Definition.Modes = Definition.Modes.OrderBy(a => a.Name.Set).ThenBy(a => a.Name.Index).ToList();

            foreach (var mode in Definition.Modes)
            {
                mode.WeaponClass = mode.WeaponClass.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

                foreach (var weaponClass in mode.WeaponClass)
                {
                    weaponClass.WeaponType = weaponClass.WeaponType.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

                    foreach (var weaponType in weaponClass.WeaponType)
                    {
                        weaponType.Actions = weaponType.Actions.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.Overlays = weaponType.Overlays.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.DeathAndDamage = weaponType.DeathAndDamage.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.Transitions = weaponType.Transitions.OrderBy(a => a.FullName.Set).ThenBy(a => a.FullName.Index).ToList();

                        foreach (var transition in weaponType.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => a.FullName.Set).ThenBy(a => a.FullName.Index).ToList();
                    }
                }
            }

            return true;
        }
    }
}