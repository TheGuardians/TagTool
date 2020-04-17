using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class FoliageSorter : SortingInterface
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        private static List<FoliageOptionTypes> TypeOrder = new List<FoliageOptionTypes> {
           FoliageOptionTypes.material_model,
           FoliageOptionTypes.alpha_test,
           FoliageOptionTypes.albedo
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.default_
        };

        private static List<AlphaTestOptions> AlphaTestOrder = new List<AlphaTestOptions> {
            AlphaTestOptions.none,
            AlphaTestOptions.simple
        };

        private static List<MaterialModelOptions> MaterialModelOrder = new List<MaterialModelOptions> {
            MaterialModelOptions.default_
        };

        // H3 count
        public int GetTypeCount() => 3;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((FoliageOptionTypes)(typeIndex))
            {
                case FoliageOptionTypes.albedo:             return 1;
                case FoliageOptionTypes.alpha_test:         return 2;
                case FoliageOptionTypes.material_model:     return 1;
                default:                                    return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((FoliageOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((FoliageOptionTypes)(typeIndex))
            {
                case FoliageOptionTypes.albedo:             return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case FoliageOptionTypes.alpha_test:         return AlphaTestOrder.IndexOf((AlphaTestOptions)optionIndex);
                case FoliageOptionTypes.material_model:     return MaterialModelOrder.IndexOf((MaterialModelOptions)optionIndex);
                default:                                    return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo: {(AlbedoOptions)options[0]} \n";
            result += $"Alpha Test: {(AlphaTestOptions)options[1]} \n";
            result += $"Material Model: {(MaterialModelOptions)options[2]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum FoliageOptionTypes
        {
            albedo = 0,
            alpha_test = 1,
            material_model = 2
        }

        private enum AlbedoOptions
        {
            default_
        }

        private enum AlphaTestOptions
        {
            none,
            simple
        }

        private enum MaterialModelOptions
        {
            default_
        }
    }
}
