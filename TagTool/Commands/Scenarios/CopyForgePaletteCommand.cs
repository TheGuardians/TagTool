using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class CopyForgePaletteCommand : Command
    {
        private GameCache Cache { get; }
        private Scenario Definition { get; }

        public CopyForgePaletteCommand(GameCache cache, Scenario definition)
            : base(true,

                 "CopyForgePalette",
                 "Copies the forge palette from the current scenario to another scenario.",

                 "CopyForgePalette [palette = all] <destination scenario>",

                 "Copies the forge palette from the current scenario to another scenario.")
        {
            Cache = cache;
            Definition = definition;
        }

        private List<string> ValidPalettes = new List<string>
        {
            "all",
            "equipment",
            "goal_objects",
            "scenery",
            "spawning",
            "teleporters",
            "vehicles",
            "weapons"
        };

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            string palette = "all";

            if (args.Count == 2)
            {
                palette = args[0].ToLower();
                args.RemoveAt(0);
            }

            if (ValidPalettes.Find(i => i == palette) == null)
            {
                Console.WriteLine($"ERROR: invalid forge palette specified: {palette}");
                return false;
            }

            if (!Cache.TryGetTag(args[0], out var destinationTag))
            {
                Console.WriteLine($"ERROR: invalid destination scenario index: {args[0]}");
                return false;
            }

            Console.Write("Loading destination scenario...");

            Scenario destinationScenario = null;
            
            using (var cacheStream = Cache.TagCache.OpenTagCacheReadWrite())
                destinationScenario = Cache.Deserialize<Scenario>(cacheStream, destinationTag);

            Console.WriteLine("done.");

            Console.Write("Copying specified forge palettes...");

            destinationScenario.SandboxBudget = Definition.SandboxBudget;

            if (palette == "all" || palette == "equipment")
                destinationScenario.SandboxEquipment = Definition.SandboxEquipment;

            if (palette == "all" || palette == "goal_objects")
                destinationScenario.SandboxGoalObjects = Definition.SandboxGoalObjects;

            if (palette == "all" || palette == "scenery")
                destinationScenario.SandboxScenery = Definition.SandboxScenery;

            if (palette == "all" || palette == "spawning")
                destinationScenario.SandboxSpawning = Definition.SandboxSpawning;

            if (palette == "all" || palette == "teleporters")
                destinationScenario.SandboxTeleporters = Definition.SandboxTeleporters;

            if (palette == "all" || palette == "vehicles")
                destinationScenario.SandboxVehicles = Definition.SandboxVehicles;

            if (palette == "all" || palette == "weapons")
                destinationScenario.SandboxWeapons = Definition.SandboxWeapons;

            Console.WriteLine("done.");

            Console.Write("Serializing destination scenario...");

            using (var cacheStream = Cache.TagCache.OpenTagCacheReadWrite())
                Cache.Serialize(cacheStream, destinationTag, destinationScenario);

            Console.WriteLine("done.");

            return true;
        }
    }
}

