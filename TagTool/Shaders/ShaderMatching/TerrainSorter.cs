using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class TerrainSorter : SortingInterface
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        // TODO: revise these

        private static List<TerrainOptionTypes> TypeOrder = new List<TerrainOptionTypes> {
           TerrainOptionTypes.material_0,
           TerrainOptionTypes.material_1,
           TerrainOptionTypes.material_2,
           TerrainOptionTypes.material_3,
           TerrainOptionTypes.environment_map,
           TerrainOptionTypes.blending,
        };

        private static List<BlendingOptions> BlendingOrder = new List<BlendingOptions> {
            BlendingOptions.morph,
            BlendingOptions.dynamic_morph
        };

        private static List<EnvironmentMappingOptions> EnvironmentMappingOrder = new List<EnvironmentMappingOptions> {
            EnvironmentMappingOptions.none,
            EnvironmentMappingOptions.per_pixel,
            EnvironmentMappingOptions.dynamic
        };

        private static List<MaterialTypeOneOptions> MaterialTypeOneOrder = new List<MaterialTypeOneOptions> {
            MaterialTypeOneOptions.off,
            MaterialTypeOneOptions.diffuse_only,
            MaterialTypeOneOptions.diffuse_plus_specular
        };

        private static List<MaterialTypeTwoOptions> MaterialTypeTwoOrder = new List<MaterialTypeTwoOptions> {
            MaterialTypeTwoOptions.off,
            MaterialTypeTwoOptions.diffuse_only_four_material_shaders_disable_detail_bump,
            MaterialTypeTwoOptions.diffuse_plus_specular_four_material_shaders_disable_detail_bump
        };

        // H3 count
        public int GetTypeCount() => 6;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((TerrainOptionTypes)(typeIndex))
            {
                case TerrainOptionTypes.blending:           return 2;
                case TerrainOptionTypes.environment_map:    return 3;
                case TerrainOptionTypes.material_0:         return 3;
                case TerrainOptionTypes.material_1:         return 3;
                case TerrainOptionTypes.material_2:         return 3;
                case TerrainOptionTypes.material_3:         return 3;
                default:                                    return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((TerrainOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((TerrainOptionTypes)(typeIndex))
            {
                case TerrainOptionTypes.blending:           return BlendingOrder.IndexOf((BlendingOptions)optionIndex);
                case TerrainOptionTypes.environment_map:    return EnvironmentMappingOrder.IndexOf((EnvironmentMappingOptions)optionIndex);
                case TerrainOptionTypes.material_0:         return MaterialTypeOneOrder.IndexOf((MaterialTypeOneOptions)optionIndex);
                case TerrainOptionTypes.material_1:         return MaterialTypeOneOrder.IndexOf((MaterialTypeOneOptions)optionIndex);
                case TerrainOptionTypes.material_2:         return MaterialTypeOneOrder.IndexOf((MaterialTypeOneOptions)optionIndex);
                case TerrainOptionTypes.material_3:         return MaterialTypeTwoOrder.IndexOf((MaterialTypeTwoOptions)optionIndex);
                default:                                    return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Blending: {(BlendingOptions)options[0]} \n";
            result += $"Environment Mapping: {(EnvironmentMappingOptions)options[1]} \n";
            result += $"Material_0: {(MaterialTypeOneOptions)options[2]} \n";
            result += $"Material_1: {(MaterialTypeOneOptions)options[3]} \n";
            result += $"Material_2: {(MaterialTypeOneOptions)options[4]} \n";
            result += $"Material_3: {(MaterialTypeTwoOptions)options[5]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum TerrainOptionTypes
        {
            blending = 0,
            environment_map = 1,
            material_0 = 2,
            material_1 = 3,
            material_2 = 4,
            material_3 = 5
        }

        private enum BlendingOptions
        {
            morph,
            dynamic_morph
        }

        private enum EnvironmentMappingOptions
        {
            none,
            per_pixel,
            dynamic
        }

        private enum MaterialTypeOneOptions
        {
            diffuse_only,
            diffuse_plus_specular,
            off
        }

        private enum MaterialTypeTwoOptions
        {
            off,
            diffuse_only_four_material_shaders_disable_detail_bump,
            diffuse_plus_specular_four_material_shaders_disable_detail_bump
        }
    }
}
