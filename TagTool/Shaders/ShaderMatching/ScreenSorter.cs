using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class ScreenSorter : SortingInterface
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        // TODO: revise these

        private static List<ScreenOptionTypes> TypeOrder = new List<ScreenOptionTypes> {
            ScreenOptionTypes.blend_mode,
            ScreenOptionTypes.overlay_b,
            ScreenOptionTypes.overlay_a,
            ScreenOptionTypes.base_,
            ScreenOptionTypes.warp
        };

        private static List<WarpOptions> WarpOrder = new List<WarpOptions> {
            WarpOptions.screen_space,
            WarpOptions.pixel_space,
            WarpOptions.none
        };

        private static List<BaseOptions> BaseOrder = new List<BaseOptions> {
            BaseOptions.single_pixel_space,
            BaseOptions.single_screen_space
        };

        private static List<OverlayAOptions> OverlayAOrder = new List<OverlayAOptions> {
            OverlayAOptions.detail_masked_screen_space,
            OverlayAOptions.detail_pixel_space,
            OverlayAOptions.detail_screen_space,
            OverlayAOptions.tint_add_color,
            OverlayAOptions.none
        };

        private static List<OverlayBOptions> OverlayBOrder = new List<OverlayBOptions> {
            OverlayBOptions.tint_add_color,
            OverlayBOptions.none
        };

        private static List<BlendModeOptions> BlendModeOrder = new List<BlendModeOptions> {
            BlendModeOptions.pre_multiplied_alpha,
            BlendModeOptions.double_multiply,
            BlendModeOptions.alpha_blend,
            BlendModeOptions.multiply,
            BlendModeOptions.additive,
            BlendModeOptions.opaque
        };

        // H3 count
        public int GetTypeCount() => 5;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((ScreenOptionTypes)(typeIndex))
            {
                case ScreenOptionTypes.warp:        return 3;
                case ScreenOptionTypes.base_:       return 2;
                case ScreenOptionTypes.overlay_a:   return 5;
                case ScreenOptionTypes.overlay_b:   return 2;
                case ScreenOptionTypes.blend_mode:  return 6;
                default:                            return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((ScreenOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((ScreenOptionTypes)(typeIndex))
            {
                case ScreenOptionTypes.warp:            return WarpOrder.IndexOf((WarpOptions)optionIndex);
                case ScreenOptionTypes.base_:           return BaseOrder.IndexOf((BaseOptions)optionIndex);
                case ScreenOptionTypes.overlay_a:       return OverlayAOrder.IndexOf((OverlayAOptions)optionIndex);
                case ScreenOptionTypes.overlay_b:       return OverlayBOrder.IndexOf((OverlayBOptions)optionIndex);
                case ScreenOptionTypes.blend_mode:      return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                default:                                return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Warp: {(WarpOptions)options[0]} \n";
            result += $"Base: {(BaseOptions)options[1]} \n";
            result += $"Overlay A: {(OverlayAOptions)options[2]} \n";
            result += $"Overlay B: {(OverlayBOptions)options[3]} \n";
            result += $"Blend Mode: {(BlendModeOptions)options[4]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum ScreenOptionTypes
        {
            warp = 0,
            base_ = 1,
            overlay_a = 2,
            overlay_b = 3,
            blend_mode = 4
        }

        private enum WarpOptions
        {
            none,
            pixel_space,
            screen_space
        }

        private enum BaseOptions
        {
            single_screen_space,
            single_pixel_space
        }

        private enum OverlayAOptions
        {
            none,
            tint_add_color,
            detail_screen_space,
            detail_pixel_space,
            detail_masked_screen_space
        }

        private enum OverlayBOptions
        {
            none,
            tint_add_color
        }

        private enum BlendModeOptions
        {
            opaque,
            additive,
            multiply,
            alpha_blend,
            double_multiply,
            pre_multiplied_alpha
        }
    }
}
