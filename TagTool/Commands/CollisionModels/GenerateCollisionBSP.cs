using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Tags.Definitions;
using TagTool.Geometry.BspCollisionGeometry.Utils;

namespace TagTool.Commands.CollisionModels
{
    public class GenerateCollisionBSPCommand : Command
    {
        private CollisionModel Definition { get; }
        bool debug = false;
        public GenerateCollisionBSPCommand(ref CollisionModel definition) :
            base(true,

                "GenerateCollisionBSP",
                "Generates a Collision BSP for the current collision tag, removing the previous collision BSP",

                "GenerateCollisionBSP",

                "")
        {
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 1)
                if (args[0] == "debug")
                    debug = true;
            for (int region_index = 0; region_index < Definition.Regions.Count; region_index++)
            {
                for(int permutation_index = 0; permutation_index < Definition.Regions[region_index].Permutations.Count; permutation_index++)
                {
                    for(int bsp_index = 0; bsp_index < Definition.Regions[region_index].Permutations[permutation_index].Bsps.Count; bsp_index++)
                    {
                        var builder = new CollisionBSPBuilder();
                        builder.generate_bsp(ref Definition.Regions[region_index].Permutations[permutation_index].Bsps[bsp_index].Geometry, debug);
                    }
                }
            }
            Console.WriteLine($"###Collision bsp build complete!");
            return true;
        }       
    }
}