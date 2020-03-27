using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class DecalSorter : SortingInterface
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        // TODO: revise these

        private static List<DecalOptionTypes> TypeOrder = new List<DecalOptionTypes> {
           DecalOptionTypes.render_pass,
           DecalOptionTypes.specular,
           DecalOptionTypes.bump_mapping,
           DecalOptionTypes.tinting,
           DecalOptionTypes.blend_mode,
           DecalOptionTypes.albedo
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.vector_alpha_drop_shadow,
            AlbedoOptions.vector_alpha,
            AlbedoOptions.palettized_plus_alpha_mask,
            AlbedoOptions.diffuse_plus_alpha_mask,
            AlbedoOptions.change_color,
            AlbedoOptions.emblem_change_color,
            AlbedoOptions.diffuse_plus_alpha,
            AlbedoOptions.palettized_plus_alpha,
            AlbedoOptions.palettized,
            AlbedoOptions.diffuse_only
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

        private static List<RenderPassOptions> RenderPassOrder = new List<RenderPassOptions> {
            RenderPassOptions.post_lighting,
            RenderPassOptions.pre_lighting
        };

        private static List<SpecularOptions> SpecularOrder = new List<SpecularOptions> {
            SpecularOptions.modulate,
            SpecularOptions.leave
        };

        private static List<BumpMappingOptions> BumpMappingOrder = new List<BumpMappingOptions> {
            BumpMappingOptions.standard_mask,
            BumpMappingOptions.standard,
            BumpMappingOptions.leave
        };

        private static List<TintingOptions> TintingOrder = new List<TintingOptions> {
            TintingOptions.fully_modulated,
            TintingOptions.partially_modulated,
            TintingOptions.unmodulated,
            TintingOptions.none
        };

        // H3 count
        public int GetTypeCount() => 6;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((DecalOptionTypes)(typeIndex))
            {
                case DecalOptionTypes.albedo:           return 10;
                case DecalOptionTypes.blend_mode:       return 11;
                case DecalOptionTypes.render_pass:      return 2;
                case DecalOptionTypes.specular:         return 2;
                case DecalOptionTypes.bump_mapping:     return 3;
                case DecalOptionTypes.tinting:          return 4;
                default:                                return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((DecalOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((DecalOptionTypes)(typeIndex))
            {
                case DecalOptionTypes.albedo:           return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case DecalOptionTypes.blend_mode:       return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case DecalOptionTypes.render_pass:      return RenderPassOrder.IndexOf((RenderPassOptions)optionIndex);
                case DecalOptionTypes.specular:         return SpecularOrder.IndexOf((SpecularOptions)optionIndex);
                case DecalOptionTypes.bump_mapping:     return BumpMappingOrder.IndexOf((BumpMappingOptions)optionIndex);
                case DecalOptionTypes.tinting:          return TintingOrder.IndexOf((TintingOptions)optionIndex);
                default:                                return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo: {(AlbedoOptions)options[0]} \n";
            result += $"Blend Mode: {(BlendModeOptions)options[1]} \n";
            result += $"Render Pass: {(RenderPassOptions)options[2]} \n";
            result += $"Specular: {(SpecularOptions)options[3]} \n";
            result += $"Bump Mapping: {(BumpMappingOptions)options[4]} \n";
            result += $"Tinting: {(TintingOptions)options[5]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum DecalOptionTypes
        {
            albedo = 0,
            blend_mode = 1,
            render_pass = 2,
            specular = 3,
            bump_mapping = 4,
            tinting = 5
        }

        private enum AlbedoOptions
        {
            diffuse_only,
            palettized,
            palettized_plus_alpha,
            diffuse_plus_alpha,
            emblem_change_color,
            change_color,
            diffuse_plus_alpha_mask,
            palettized_plus_alpha_mask,
            vector_alpha,
            vector_alpha_drop_shadow
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

        private enum RenderPassOptions
        {
            pre_lighting,
            post_lighting
        }

        private enum SpecularOptions
        {
            leave,
            modulate
        }

        private enum BumpMappingOptions
        {
            leave,
            standard,
            standard_mask
        }

        private enum TintingOptions
        {
            none,
            unmodulated,
            partially_modulated,
            fully_modulated
        }
    }
}
