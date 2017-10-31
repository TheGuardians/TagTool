using System;
using System.Collections.Generic;
using BlamCore.Cache;
using BlamCore.TagDefinitions;
using BlamCore.Serialization;

namespace TagTool.Commands.Scenarios
{
    class CopyForgePaletteCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private Scenario Definition { get; }

        public CopyForgePaletteCommand(GameCacheContext cacheContext, Scenario definition)
            : base(CommandFlags.Inherit,

                 "CopyForgePalette",
                 "Copies the forge palette from the current scenario to another scenario.",

                 "CopyForgePalette [palette = all] <destination scenario>",

                 "Copies the forge palette from the current scenario to another scenario.")
        {
            CacheContext = cacheContext;
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

            var destinationTag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (destinationTag == null || destinationTag.Group.Tag.ToString() != "scnr")
            {
                Console.WriteLine($"ERROR: invalid destination scenario index: {args[0]}");
                return false;
            }

            Console.Write("Loading destination scenario...");

            Scenario destinationScenario = null;
            
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var scenarioContext = new TagSerializationContext(cacheStream, CacheContext, destinationTag);
                destinationScenario = CacheContext.Deserializer.Deserialize<Scenario>(scenarioContext);
            }

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

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var scenarioContext = new TagSerializationContext(cacheStream, CacheContext, destinationTag);
                CacheContext.Serializer.Serialize(scenarioContext, destinationScenario);
            }

            Console.WriteLine("done.");

            return true;
        }
    }
}

