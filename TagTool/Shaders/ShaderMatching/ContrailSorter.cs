using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class ContrailSorter : SortingInterface
    {
        private static List<ContrailOptionTypes> TypeOrder = new List<ContrailOptionTypes> {
           ContrailOptionTypes.black_point,
           ContrailOptionTypes.fog,
           ContrailOptionTypes.blend_mode,
           ContrailOptionTypes.albedo
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.diffuse_only,
            AlbedoOptions.palettized,
            AlbedoOptions.palettized_plus_alpha
        };

        private static List<BlendModeOptions> BlendModeOrder = new List<BlendModeOptions> {
            BlendModeOptions.pre_multiplied_alpha,
            BlendModeOptions.inv_alpha_blend,
            BlendModeOptions.add_src_times_srcalpha,
            BlendModeOptions.add_src_times_dstalpha,
            BlendModeOptions.multiply_add,
            BlendModeOptions.maximum,
            BlendModeOptions.double_multiply,
            BlendModeOptions.alpha_blend,
            BlendModeOptions.multiply,
            BlendModeOptions.additive,
            BlendModeOptions.opaque
        };

        private static List<BlackPointOptions> BlackPointOrder = new List<BlackPointOptions> {
            BlackPointOptions.off,
            BlackPointOptions.on
        };

        private static List<FogOptions> FogOrder = new List<FogOptions> {
            FogOptions.off,
            FogOptions.on
        };

        public int GetTypeCount() => 4;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((ContrailOptionTypes)(typeIndex))
            {
                case ContrailOptionTypes.albedo:        return 3;
                case ContrailOptionTypes.blend_mode:    return 11;
                case ContrailOptionTypes.black_point:   return 2;
                case ContrailOptionTypes.fog:           return 2;
                default:                            return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((ContrailOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((ContrailOptionTypes)(typeIndex))
            {
                case ContrailOptionTypes.albedo:        return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case ContrailOptionTypes.blend_mode:    return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case ContrailOptionTypes.black_point:   return BlackPointOrder.IndexOf((BlackPointOptions)optionIndex);
                case ContrailOptionTypes.fog:           return FogOrder.IndexOf((FogOptions)optionIndex);
                default:                            return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo:         {(AlbedoOptions)options[0]} \n";
            result += $"Blend mode:     {(BlendModeOptions)options[1]} \n";
            result += $"Black point:    {(BlackPointOptions)options[2]} \n";
            result += $"Fog:            {(FogOptions)options[3]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum ContrailOptionTypes
        {
            albedo,
            blend_mode,
            black_point,
            fog
        }

        private enum AlbedoOptions
        {
            diffuse_only,
            palettized,
            palettized_plus_alpha
        }

        private enum BlendModeOptions
        {
            opaque,
            additive,
            multiply,
            alpha_blend,
            double_multiply,
            maximum,
            multiply_add,
            add_src_times_dstalpha,
            add_src_times_srcalpha,
            inv_alpha_blend,
            pre_multiplied_alpha
        }

        private enum BlackPointOptions
        {
            off,
            on
        }

        private enum FogOptions
        {
            off,
            on
        }
    }
}
