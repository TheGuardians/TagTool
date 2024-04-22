using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class LightVolumeSorter : SortingInterface
    {
        private static List<LightVolumeOptionTypes> TypeOrder = new List<LightVolumeOptionTypes> {
           LightVolumeOptionTypes.fog,
           LightVolumeOptionTypes.blend_mode,
           LightVolumeOptionTypes.albedo
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.diffuse_only,
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

        private static List<FogOptions> FogOrder = new List<FogOptions> {
            FogOptions.off,
            FogOptions.on
        };

        public int GetTypeCount() => 3;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((LightVolumeOptionTypes)(typeIndex))
            {
                case LightVolumeOptionTypes.albedo:         return 1;
                case LightVolumeOptionTypes.blend_mode:     return 11;
                case LightVolumeOptionTypes.fog:            return 2;
                default:                                    return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((LightVolumeOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((LightVolumeOptionTypes)(typeIndex))
            {
                case LightVolumeOptionTypes.albedo:        return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case LightVolumeOptionTypes.blend_mode:    return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case LightVolumeOptionTypes.fog:           return FogOrder.IndexOf((FogOptions)optionIndex);
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
            result += $"Fog:            {(FogOptions)options[2]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum LightVolumeOptionTypes
        {
            albedo,
            blend_mode,
            fog
        }

        private enum AlbedoOptions
        {
            diffuse_only
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

        private enum FogOptions
        {
            off,
            on
        }
    }
}
