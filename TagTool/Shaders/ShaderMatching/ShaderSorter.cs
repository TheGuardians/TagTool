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
    public interface SortingInterface
    {
        int GetTypeCount();
        int GetTypeIndex(int typeIndex);
        int GetOptionCount(int typeIndex, int optionIndex);
        int GetOptionIndex(int typeIndex, int optionIndex);
        void PrintOptions(List<int> options);
        string ToString(List<int> options);
    }

    public class ShaderSorter : SortingInterface
    {
        //
        // TODO: order the list for best matches
        //

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
            AlbedoOptions.two_detail_black_point,
            AlbedoOptions.two_change_color_anim_overlay,
            AlbedoOptions.chameleon,
            AlbedoOptions.two_change_color_chameleon,
            AlbedoOptions.chameleon_masked,
            AlbedoOptions.color_mask_hard_light
        };

        private static List<BumpMappingOptions> BumpMappingOrder = new List<BumpMappingOptions> {
            BumpMappingOptions.off,
            BumpMappingOptions.standard,
            BumpMappingOptions.detail,
            BumpMappingOptions.detail_masked
        };

        private static List<AlphaTestOptions> AlphaTestOrder = new List<AlphaTestOptions> {
            AlphaTestOptions.none,
            AlphaTestOptions.simple
        };

        private static List<SpecularMaskOptions> SpecularMaskOrder = new List<SpecularMaskOptions> {
            SpecularMaskOptions.no_specular_mask,
            SpecularMaskOptions.specular_mask_from_diffuse,
            SpecularMaskOptions.specular_mask_from_texture,
            SpecularMaskOptions.specular_mask_from_color_texture
        };

        private static List<MaterialModelOptions> MaterialModelOrder = new List<MaterialModelOptions> {
            MaterialModelOptions.diffuse_only,
            MaterialModelOptions.cook_torrance,
            MaterialModelOptions.two_lobe_phong,
            MaterialModelOptions.foliage,
            MaterialModelOptions.none,
            MaterialModelOptions.glass,
            MaterialModelOptions.organism,
            MaterialModelOptions.single_lobe_phong,
            MaterialModelOptions.car_paint
        };

        private static List<EnvironmentMappingOptions> EnvrionmentMappingOrder = new List<EnvironmentMappingOptions> {
            EnvironmentMappingOptions.none,
            EnvironmentMappingOptions.per_pixel,
            EnvironmentMappingOptions.dynamic,
            EnvironmentMappingOptions.from_flat_texture,
            EnvironmentMappingOptions.custom_map
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
            SelfIlluminationOptions.simple_with_alpha_mask,
            SelfIlluminationOptions.simple_four_change_color
        };

        private static List<BlendModeOptions> BlendModeOrder = new List<BlendModeOptions> {
            BlendModeOptions.opaque,
            BlendModeOptions.additive,
            BlendModeOptions.multiply,
            BlendModeOptions.alpha_blend,
            BlendModeOptions.double_multiply,
            BlendModeOptions.pre_multiplied_alpha
        };

        private static List<ParallaxOptions> ParallaxOrder = new List<ParallaxOptions> {
            ParallaxOptions.off,
            ParallaxOptions.simple,
            ParallaxOptions.interpolated,
            ParallaxOptions.simple_detail
        };

        private static List<MiscOptions> MiscOrder = new List<MiscOptions> {
            MiscOptions.first_person_never,
            MiscOptions.first_person_sometimes,
            MiscOptions.first_person_always,
            MiscOptions.first_person_never_with_rotating_bitmaps
        };


        public int GetTypeCount() => 10;

        public int GetOptionCount(int typeIndex, int optionIndex)
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

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((ShaderOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((ShaderOptionTypes)(typeIndex))
            {
                case ShaderOptionTypes.albedo:                  return AlbedoOrder.IndexOf((AlbedoOptions)optionIndex);
                case ShaderOptionTypes.bump_mapping:            return BumpMappingOrder.IndexOf((BumpMappingOptions)optionIndex);
                case ShaderOptionTypes.alpha_test:              return AlphaTestOrder.IndexOf((AlphaTestOptions)optionIndex);
                case ShaderOptionTypes.specular_mask:           return SpecularMaskOrder.IndexOf((SpecularMaskOptions)optionIndex);
                case ShaderOptionTypes.material_model:          return MaterialModelOrder.IndexOf((MaterialModelOptions)optionIndex);
                case ShaderOptionTypes.environment_mapping:     return EnvrionmentMappingOrder.IndexOf((EnvironmentMappingOptions)optionIndex);
                case ShaderOptionTypes.self_illumination:       return SelfIlluminationOrder.IndexOf((SelfIlluminationOptions)optionIndex);
                case ShaderOptionTypes.blend_mode:              return BlendModeOrder.IndexOf((BlendModeOptions)optionIndex);
                case ShaderOptionTypes.parallax:                return ParallaxOrder.IndexOf((ParallaxOptions)optionIndex);
                case ShaderOptionTypes.misc:                    return MiscOrder.IndexOf((MiscOptions)optionIndex);
                default:                                        return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Albedo: {(AlbedoOptions)options[0]} \n";
            result += $"Bump Mapping: {(BumpMappingOptions)options[1]} \n";
            result += $"Alpha Test: {(AlphaTestOptions)options[2]} \n";
            result += $"Specular Mask: {(SpecularMaskOptions)options[3]} \n";
            result += $"Material Mode: {(MaterialModelOptions)options[4]} \n";
            result += $"Enviornment Mapping: {(EnvironmentMappingOptions)options[5]} \n";
            result += $"Self Illumination: {(SelfIlluminationOptions)options[6]} \n";
            result += $"Blend Mode: {(BlendModeOptions)options[7]} \n";
            result += $"Parallax: {(ParallaxOptions)options[8]} \n";
            result += $"Misc: {(MiscOptions)options[9]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
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
            default_,
            detail_blemd,
            constant_color,
            two_change_color,
            four_change_color,
            three_detail_blemd,
            two_detail_overlay,
            two_detail,
            color_mask,
            two_detail_black_point,
            two_change_color_anim_overlay,
            chameleon,
            two_change_color_chameleon,
            chameleon_masked,
            color_mask_hard_light
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
