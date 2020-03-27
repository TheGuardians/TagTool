using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    // TODO: fixup option indices where they changed from H3 -> HO

    public class HalogramSorter : SortingInterface
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        // TODO: revise these

        private static List<HalogramOptionTypes> TypeOrder = new List<HalogramOptionTypes> {
            HalogramOptionTypes.warp,
            HalogramOptionTypes.misc,
            HalogramOptionTypes.blend_mode,
            HalogramOptionTypes.overlay,
            HalogramOptionTypes.edge_fade,
            HalogramOptionTypes.self_illumination,
            HalogramOptionTypes.albedo
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.default_,
            AlbedoOptions.detail_blemd,
            AlbedoOptions.constant_color,
            AlbedoOptions.two_change_color,
            AlbedoOptions.four_change_color,
            AlbedoOptions.three_detail_blemd,
            AlbedoOptions.two_detail_overlay,
            AlbedoOptions.two_detail,
            AlbedoOptions.color_mask,
            AlbedoOptions.two_detail_black_point
        };

        private static List<SelfIlluminationOptions> SelfIlluminationOrder = new List<SelfIlluminationOptions> {
            SelfIlluminationOptions.off,
            SelfIlluminationOptions.simple,
            SelfIlluminationOptions.three_channel_self_illum,
            SelfIlluminationOptions.plasma,
            SelfIlluminationOptions.from_diffuse,
            SelfIlluminationOptions.illum_detail,
            SelfIlluminationOptions.meter,
            SelfIlluminationOptions.self_illum_times_diffuse,
            SelfIlluminationOptions.multilayer_additive,
            SelfIlluminationOptions.ml_add_four_change_color,
            SelfIlluminationOptions.ml_add_five_change_color,
            SelfIlluminationOptions.scope_blur
        };

        private static List<BlendModeOptions> BlendModeOrder = new List<BlendModeOptions> {
            BlendModeOptions.opaque,
            BlendModeOptions.additive,
            BlendModeOptions.multiply,
            BlendModeOptions.alpha_blend,
            BlendModeOptions.double_multiply
        };

        private static List<MiscOptions> MiscOrder = new List<MiscOptions> {
            MiscOptions.first_person_never,
            MiscOptions.first_person_sometimes,
            MiscOptions.first_person_always,
            MiscOptions.first_person_never_with_rotating_bitmaps
        };

        private static List<WarpOptions> WarpOrder = new List<WarpOptions> {
            WarpOptions.none,
            WarpOptions.from_texture,
            WarpOptions.parallax_simple
        };

        private static List<OverlayOptions> OverlayOrder = new List<OverlayOptions> {
            OverlayOptions.none,
            OverlayOptions.additive,
            OverlayOptions.additive_detail,
            OverlayOptions.multiply,
            OverlayOptions.multiply_and_additive_detail
        };

        private static List<EdgeFadeOptions> EdgeFadeOrder = new List<EdgeFadeOptions>
        {
            EdgeFadeOptions.none,
            EdgeFadeOptions.simple
        };

        // H3 count
        public int GetTypeCount() => 7;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((HalogramOptionTypes)(typeIndex))
            { 
                case HalogramOptionTypes.albedo:                return 10;
                case HalogramOptionTypes.self_illumination:     return 12;
                case HalogramOptionTypes.blend_mode:            return 5;
                case HalogramOptionTypes.misc:                  return 4;
                case HalogramOptionTypes.warp:                  return 3;
                case HalogramOptionTypes.overlay:               return 5;
                case HalogramOptionTypes.edge_fade:             return 2;
                default:                                        return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((HalogramOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((HalogramOptionTypes)(typeIndex))
            {
                case HalogramOptionTypes.albedo:                return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case HalogramOptionTypes.self_illumination:     return SelfIlluminationOrder.IndexOf((SelfIlluminationOptions)optionIndex);
                case HalogramOptionTypes.blend_mode:            return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case HalogramOptionTypes.misc:                  return MiscOrder.IndexOf((MiscOptions)optionIndex);
                case HalogramOptionTypes.warp:                  return WarpOrder.IndexOf((WarpOptions)optionIndex);
                case HalogramOptionTypes.overlay:               return OverlayOrder.IndexOf((OverlayOptions)optionIndex);
                case HalogramOptionTypes.edge_fade:             return EdgeFadeOrder.IndexOf((EdgeFadeOptions)optionIndex);
                default:                                        return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo: {(AlbedoOptions)options[0]} \n";
            result += $"Self Illumination: {(SelfIlluminationOptions)options[1]} \n";
            result += $"Blend Mode: {(BlendModeOptions)options[2]} \n";
            result += $"Misc: {(MiscOptions)options[3]} \n";
            result += $"Warp: {(WarpOptions)options[4]} \n";
            result += $"Overlay: {(OverlayOptions)options[5]} \n";
            result += $"Edge Fade: {(EdgeFadeOptions)options[6]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum HalogramOptionTypes
        {
            albedo = 0,
            self_illumination = 1,
            blend_mode = 2,
            misc = 3,
            warp = 4,
            overlay = 5,
            edge_fade = 6
        }

        private enum AlbedoOptions
        {
            default_,
            detail_blemd,
            constant_color,
            two_change_color,
            four_change_color,
            three_detail_blemd,
            two_detail_overlay,
            two_detail,
            color_mask,
            two_detail_black_point
        }

        private enum SelfIlluminationOptions
        {
            off,
            simple,
            three_channel_self_illum,
            plasma,
            from_diffuse,
            illum_detail,
            meter,
            self_illum_times_diffuse,
            multilayer_additive,
            ml_add_four_change_color,
            ml_add_five_change_color,
            scope_blur
        }

        private enum BlendModeOptions
        {
            opaque,
            additive,
            multiply,
            alpha_blend,
            double_multiply
        }

        private enum MiscOptions
        {
            first_person_never,
            first_person_sometimes,
            first_person_always,
            first_person_never_with_rotating_bitmaps
        }

        private enum WarpOptions
        {
            none,
            from_texture,
            parallax_simple
        }

        private enum OverlayOptions
        {
            none,
            additive,
            additive_detail,
            multiply,
            multiply_and_additive_detail
        }

        private enum EdgeFadeOptions
        {
            none,
            simple
        }
    }
}
