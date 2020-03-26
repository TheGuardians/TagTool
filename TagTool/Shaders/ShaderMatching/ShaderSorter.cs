using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders.ShaderMatching
{
    //
    // Sorter implementation for rmsh, TODO: find a better name
    //
    public abstract class Sorter
    {
        public abstract int GetTypeCount();
        public abstract int GetTypeIndex(int typeIndex);
        public abstract int GetOptionCount(int typeIndex, int optionIndex);
        public abstract int GetOptionIndex(int typeIndex, int optionIndex);
        public abstract string PrintOptions(List<int> options);
    }

    public class ShaderSorter : Sorter
    {
        // these private lists define the order of the types and options in a shader. The matcher will use these when no perfect match exists. 
        private static List<ShaderOptionTypes> TypeOrder = new List<ShaderOptionTypes> {
            ShaderOptionTypes.albedo,
            ShaderOptionTypes.bump_mapping,   
            ShaderOptionTypes.alpha_test,       
            ShaderOptionTypes.specular_mask,    
            ShaderOptionTypes.material_model,  
            ShaderOptionTypes.environment_mapping,
            ShaderOptionTypes.self_illumination,
            ShaderOptionTypes.blend_mode,  
            ShaderOptionTypes.parallax,          
            ShaderOptionTypes.misc
        };

        //
        // TODO: add order list for each option types and return IndexOf in GetOptionIndex, implement PrintOptions method to display each type name with the option from the input list
        //

        public override int GetTypeCount() => 10;

        public override int GetOptionCount(int typeIndex, int optionIndex)
        {
            switch ((ShaderOptionTypes)(typeIndex))
            {
                case ShaderOptionTypes.albedo: return 15;
                case ShaderOptionTypes.bump_mapping: return 4;
                case ShaderOptionTypes.alpha_test: return 2;
                case ShaderOptionTypes.specular_mask: return 4;
                case ShaderOptionTypes.material_model: return 9;
                case ShaderOptionTypes.environment_mapping: return 5;
                case ShaderOptionTypes.self_illumination: return 10;
                case ShaderOptionTypes.blend_mode: return 6;
                case ShaderOptionTypes.parallax: return 4;
                case ShaderOptionTypes.misc: return 4;
                default: return 0;
            }
        }

        public override int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((ShaderOptionTypes)typeIndex);
        }

        public override int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((ShaderOptionTypes)(typeIndex))
            {
                case ShaderOptionTypes.albedo: return 15;
                case ShaderOptionTypes.bump_mapping: return 4;
                case ShaderOptionTypes.alpha_test: return 2;
                case ShaderOptionTypes.specular_mask: return 4;
                case ShaderOptionTypes.material_model: return 9;
                case ShaderOptionTypes.environment_mapping: return 5;
                case ShaderOptionTypes.self_illumination: return 10;
                case ShaderOptionTypes.blend_mode: return 6;
                case ShaderOptionTypes.parallax: return 4;
                case ShaderOptionTypes.misc: return 4;
                default: return 0;
            }
        }

        public override string PrintOptions(List<int> options)
        {
            throw new NotImplementedException();
        }


        private enum ShaderOptionTypes
        {
            albedo = 0,
            bump_mapping = 1,
            alpha_test = 2,
            specular_mask = 3,
            material_model = 4,
            environment_mapping = 5,
            self_illumination = 6,
            blend_mode = 7,
            parallax = 8,
            misc = 9,
            distortion = 10,
        }

        private enum AlbedoOptions
        {
            default_ = 0,
            detail_blemd= 1,
            constant_color = 2,
            two_change_color = 3,
            four_change_color = 4,
            three_detail_blemd = 5,
            two_detail_overlay = 6,
            two_detail = 7,
            color_mask = 8,
            two_detail_black_point = 9,
            two_change_color_anim_overlay = 10,
            chameleon = 11,
            two_change_color_chameleon = 12,
            chameleon_masked = 13,
            color_mask_hard_light = 14
        }

        private enum BumpMappingOptions
        {
            off,
            standard,
            detail,
            detail_masked
        }

        private enum AlphaTestOptions
        {
            none,
            simple
        }

        private enum SpecularMaskOptions
        {
            no_specular_mask,
            specular_mask_from_diffuse,
            specular_mask_from_texture,
            specular_mask_from_color_texture
        }

        private enum MaterialModelOptions
        {
            diffuse_only,
            cook_torrance,
            two_lobe_phong,
            foliage,
            none,
            glass,
            organism,
            single_lobe_phong,
            car_paint
        }

        private enum EnvironmentMappingOptions
        {
            none,
            per_pixel,
            dynamic,
            from_flat_texture,
            custom_map
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
            simple_with_alpha_mask,
            simple_four_change_color
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

        private enum ParallaxOptions
        {
            off,
            simple,
            interpolated,
            simple_detail
        }

        private enum MiscOptions
        {
            first_person_never,
            first_person_sometimes,
            first_person_always,
            first_person_never_with_rotating_bitmaps
        }

        private enum DistortionOptions
        {
            off,
            on
        }
    }

    
}
