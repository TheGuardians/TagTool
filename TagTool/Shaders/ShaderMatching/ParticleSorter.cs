using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class ParticleSorter : SortingInterface
    {
        private static List<ParticleOptionTypes> TypeOrder = new List<ParticleOptionTypes> {
            ParticleOptionTypes.specialized_rendering,
            ParticleOptionTypes.lighting,
            ParticleOptionTypes.render_targets,
            ParticleOptionTypes.depth_fade,
            ParticleOptionTypes.black_point,
            ParticleOptionTypes.fog,
            ParticleOptionTypes.frame_blend,
            ParticleOptionTypes.self_illumination,
            ParticleOptionTypes.albedo,
            ParticleOptionTypes.blend_mode
        };

        private static List<AlbedoOptions> AlbedoOrder = new List<AlbedoOptions> {
            AlbedoOptions.palettized,
            AlbedoOptions.diffuse_only,
            AlbedoOptions.palettized_plus_billboard_alpha,
            AlbedoOptions.diffuse_plus_billboard_alpha,
            AlbedoOptions.palettized_plus_sprite_alpha,
            AlbedoOptions.diffuse_plus_sprite_alpha,
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

        private static List<SpecializedRenderingOptions> SpecializedRenderingOrder = new List<SpecializedRenderingOptions> {
            SpecializedRenderingOptions.none,
            SpecializedRenderingOptions.distortion,
            SpecializedRenderingOptions.distortion_expensive
        };

        private static List<LightingOptions> LightingOrder = new List<LightingOptions> {
            LightingOptions.none,
            LightingOptions.per_pixel_ravi_order_3,
            LightingOptions.per_vertex_ravi_order_0
        };

        private static List<RenderTargetsOptions> RenderTargetsOrder = new List<RenderTargetsOptions>
        {
            RenderTargetsOptions.ldr_and_hdr,
            RenderTargetsOptions.ldr_only
        };

        private static List<DepthFadeOptions> DepthFadeOrder = new List<DepthFadeOptions> {
            DepthFadeOptions.off,
            DepthFadeOptions.on
        };

        private static List<BlackPointOptions> BlackPointOrder = new List<BlackPointOptions> {
            BlackPointOptions.off,
            BlackPointOptions.on
        };

        private static List<FogOptions> FogOrder = new List<FogOptions> {
            FogOptions.off,
            FogOptions.on
        };

        private static List<FrameBlendOptions> FrameBlendOrder = new List<FrameBlendOptions> {
            FrameBlendOptions.off,
            FrameBlendOptions.on
        };

        private static List<SelfIlluminationOptions> SelfIlluminationOrder = new List<SelfIlluminationOptions> {
            SelfIlluminationOptions.none,
            SelfIlluminationOptions.constant_color
        };

        public int GetTypeCount() => 10;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((ParticleOptionTypes)(typeIndex))
            {
                case ParticleOptionTypes.albedo:                return 6;
                case ParticleOptionTypes.blend_mode:            return 11;
                case ParticleOptionTypes.specialized_rendering: return 3;
                case ParticleOptionTypes.lighting:              return 3;
                case ParticleOptionTypes.render_targets:        return 2;
                case ParticleOptionTypes.depth_fade:            return 2;
                case ParticleOptionTypes.black_point:           return 2;
                case ParticleOptionTypes.fog:                   return 2;
                case ParticleOptionTypes.frame_blend:           return 2;
                case ParticleOptionTypes.self_illumination:     return 2;
                default:                                        return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((ParticleOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((ParticleOptionTypes)(typeIndex))
            {
                case ParticleOptionTypes.albedo:                return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case ParticleOptionTypes.blend_mode:            return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case ParticleOptionTypes.specialized_rendering: return SpecializedRenderingOrder.IndexOf((SpecializedRenderingOptions)optionIndex);
                case ParticleOptionTypes.lighting:              return LightingOrder.IndexOf((LightingOptions)optionIndex);
                case ParticleOptionTypes.render_targets:        return RenderTargetsOrder.IndexOf((RenderTargetsOptions)optionIndex);
                case ParticleOptionTypes.depth_fade:            return DepthFadeOrder.IndexOf((DepthFadeOptions)optionIndex);
                case ParticleOptionTypes.black_point:           return BlackPointOrder.IndexOf((BlackPointOptions)optionIndex);
                case ParticleOptionTypes.fog:                   return FogOrder.IndexOf((FogOptions)optionIndex);
                case ParticleOptionTypes.frame_blend:           return FrameBlendOrder.IndexOf((FrameBlendOptions)optionIndex);
                case ParticleOptionTypes.self_illumination:     return SelfIlluminationOrder.IndexOf((SelfIlluminationOptions)optionIndex);
                default: return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo:                 {(AlbedoOptions)options[0]} \n";
            result += $"Blend mode:             {(BlendModeOptions)options[1]} \n";
            result += $"Specialized Rendering:  {(SpecializedRenderingOptions)options[2]} \n";
            result += $"Lighting:               {(LightingOptions)options[3]} \n";
            result += $"Render targets:         {(RenderTargetsOptions)options[4]} \n";
            result += $"Depth fade:             {(DepthFadeOptions)options[5]} \n";
            result += $"Black point:            {(BlackPointOptions)options[6]} \n";
            result += $"Fog:                    {(FogOptions)options[7]} \n";
            result += $"Frame blend:            {(FrameBlendOptions)options[8]} \n";
            result += $"Self illumination:      {(SelfIlluminationOptions)options[9]} \n";
            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum ParticleOptionTypes
        {
            albedo,
            blend_mode,
            specialized_rendering,
            lighting,
            render_targets,
            depth_fade,
            black_point,
            fog,
            frame_blend,
            self_illumination
        }

        private enum AlbedoOptions
        {
            diffuse_only,
            diffuse_plus_billboard_alpha,
            palettized,
            palettized_plus_billboard_alpha,
            diffuse_plus_sprite_alpha,
            palettized_plus_sprite_alpha,
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

        private enum SpecializedRenderingOptions
        {
            none,
            distortion,
            distortion_expensive
        }

        private enum LightingOptions
        {
            none,
            per_pixel_ravi_order_3,
            per_vertex_ravi_order_0
        }

        private enum RenderTargetsOptions
        {
            ldr_and_hdr,
            ldr_only
        }

        private enum DepthFadeOptions
        {
            off,
            on
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

        private enum FrameBlendOptions
        {
            off,
            on
        }

        private enum SelfIlluminationOptions
        {
            none,
            constant_color
        }
    }
}
