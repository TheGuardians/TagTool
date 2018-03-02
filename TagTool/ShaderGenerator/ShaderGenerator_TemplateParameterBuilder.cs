using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Shaders;

namespace TagTool.ShaderGenerator
{
    public partial class ShaderGenerator
    {
        class TemplateParameter
        {
            public string Name;
            public byte Size;
            public ShaderParameter.RType Type;
            public Type Target_Type;
            public Boolean enabled = true;

            public TemplateParameter(Type target_method_type, string _name, ShaderParameter.RType _type, byte _size = 1)
            {
                Target_Type = target_method_type;
                Name = _name;
                Type = _type;
                Size = _size;
            }
        }

        static MultiValueDictionary<object, object> template_uniforms = new MultiValueDictionary<object, object>
        {
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Constant_Color,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Constant_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },

            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "tertiary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "quaternary_change_color", ShaderParameter.RType.Vector) },

            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map3_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_overlay", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_overlay_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "color_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color2", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color3", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "neutral_gray", ShaderParameter.RType.Vector) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "primary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "secondary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "primary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "secondary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "color_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Bump_Mapping.Standard,  new TemplateParameter(typeof(Bump_Mapping), "bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_map_xform", ShaderParameter.RType.Vector) }, // Manual
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map_xform", ShaderParameter.RType.Vector) }, // Manual
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_coefficient", ShaderParameter.RType.Vector) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_mask_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_coefficient", ShaderParameter.RType.Vector) },
            {Alpha_Test.Simple,  new TemplateParameter(typeof(Alpha_Test),"alpha_test_map", ShaderParameter.RType.Sampler) },
            {Specular_Mask.From_Texture,  new TemplateParameter(typeof(Specular_Mask),"specular_mask_texture", ShaderParameter.RType.Sampler) },
            {Specular_Mask.From_Color_Texture,  new TemplateParameter(typeof(Specular_Mask),"specular_mask_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Diffuse_Only,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_color", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"use_fresnel_color_environment", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_color_environment", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_power", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"use_material_texture", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"material_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_cc0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_dd0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_c78d78", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"albedo_blend_with_specular_tint", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"albedo_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"analytical_anti_shadow_control", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_color", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_power", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_albedo_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"normal_specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"normal_specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"glancing_specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"glancing_specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"albedo_specular_tint_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_anti_shadow_control", ShaderParameter.RType.Vector) },
            {Material_Model.Foliage,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"diffuse_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"analytical_specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"area_specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"environment_map_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"environment_map_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_power", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_start", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_maps_transition_ratio", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"ambient_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"ambient_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"occlusion_parameter_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_propagation_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_normal_detail", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_normal_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_normal_detail", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"final_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"use_material_texture0", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"use_material_texture1", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"material_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_cc0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_dd0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_c78d78", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"bump_detail_map0", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"bump_detail_map0_blend_factor", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_coefficient0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_tint0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_power0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"albedo_blend0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"roughness0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"order3_area_specular0", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_tint1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color_environment1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_power1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"albedo_blend1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"roughness1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"order3_area_specular1", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_color1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_power1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_albedo_blend1", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"dynamic_environment_map_0", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"dynamic_environment_map_1", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_x", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_y", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_z", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"hemisphere_percentage", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_bloom_override", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_bloom_override_intensity", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_a", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_b", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_c", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"noise_map_a", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"noise_map_b", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_medium", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_wide", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_sharp", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"alpha_mask_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_medium", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_wide", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_sharp", ShaderParameter.RType.Vector) },
            {Self_Illumination.From_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.From_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_detail_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_color_off", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_color_on", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_value", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"primary_change_color_blend", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Parallax.Simple,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Simple,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Interpolated,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Interpolated,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_scale_map", ShaderParameter.RType.Sampler) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"distort_map", ShaderParameter.RType.Sampler) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"distort_scale", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_fresnel_enabled", ShaderParameter.RType.Boolean) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_fresnel_power", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_z_enabled", ShaderParameter.RType.Boolean) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_z_range", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"depth_map", ShaderParameter.RType.Sampler) },
        };

        static List<TemplateParameter> GetShaderParametersList(object key, Type type)
        {
            if (template_uniforms.ContainsKey((object)key))
            {
                var list = template_uniforms[(object)key].Cast<TemplateParameter>().ToList();
                list = list.Where(param => param.Target_Type == type).ToList();
                return list;
            }
            return new List<TemplateParameter>();
        }

        class IndicesManager
        {
            public int sampler_index = 0;
            public int vector_index = 58;
            public int boolean_index = 0; //TODO: Find this starting location
        }

        static List<ShaderParameter> GenerateShaderParameters(
            ShaderParameter.RType target_type,
            GameCacheContext cacheContext,
            IEnumerable<TemplateParameter> _params,
            IndicesManager indices)
        {
            List<ShaderParameter> parameters = new List<ShaderParameter>();

            foreach (var param in _params)
            {
                if (param.Type != target_type) continue;
                switch (param.Type)
                {
                    case ShaderParameter.RType.Vector:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Vector,
                                RegisterIndex = (ushort)indices.vector_index
                            });
                        indices.vector_index++;
                        break;
                    case ShaderParameter.RType.Sampler:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Sampler,
                                RegisterIndex = (ushort)indices.sampler_index
                            });
                        indices.sampler_index++;

                        break;
                    case ShaderParameter.RType.Boolean:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Boolean,
                                RegisterIndex = (ushort)indices.boolean_index
                            });
                        indices.boolean_index++;
                        break;
                    case ShaderParameter.RType.Integer:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return parameters;
        }

        static List<ShaderParameter> GenerateShaderParameters(GameCacheContext cacheContext, ShaderGeneratorParameters template_parameters)
        {
            //var params_Albedo = template_uniforms[template_parameters.albedo].Cast<TemplateParameter>().ToList();
            //var params_Bump_Mapping = template_uniforms[template_parameters.bump_mapping].Cast<TemplateParameter>().ToList();
            //var params_Alpha_Test = template_uniforms[template_parameters.alpha_test].Cast<TemplateParameter>().ToList();
            //var params_Specular_Mask = template_uniforms[template_parameters.specular_mask].Cast<TemplateParameter>().ToList();
            //var params_Material_Model = template_uniforms[template_parameters.material_model].Cast<TemplateParameter>().ToList();
            //var params_Environment_Mapping = template_uniforms[template_parameters.environment_mapping].Cast<TemplateParameter>().ToList();
            //var params_Self_Illumination = template_uniforms[template_parameters.self_illumination].Cast<TemplateParameter>().ToList();
            //var params_Blend_Mode = template_uniforms[template_parameters.blend_mode].Cast<TemplateParameter>().ToList();
            //var params_Parallax = template_uniforms[template_parameters.parallax].Cast<TemplateParameter>().ToList();
            //var params_Misc = template_uniforms[template_parameters.misc].Cast<TemplateParameter>().ToList();
            //var params_Distortion = template_uniforms[template_parameters.distortion].Cast<TemplateParameter>().ToList();
            //var params_Soft_fade = template_uniforms[template_parameters.soft_fade].Cast<TemplateParameter>().ToList();

            var params_Albedo = GetShaderParametersList(template_parameters.albedo, typeof(Albedo));
            var params_Bump_Mapping = GetShaderParametersList(template_parameters.bump_mapping, typeof(Bump_Mapping));
            var params_Alpha_Test = GetShaderParametersList(template_parameters.alpha_test, typeof(Alpha_Test));
            var params_Specular_Mask = GetShaderParametersList(template_parameters.specular_mask, typeof(Specular_Mask));
            var params_Material_Model = GetShaderParametersList(template_parameters.material_model, typeof(Material_Model));
            var params_Environment_Mapping = GetShaderParametersList(template_parameters.environment_mapping, typeof(Environment_Mapping));
            var params_Self_Illumination = GetShaderParametersList(template_parameters.self_illumination, typeof(Self_Illumination));
            var params_Blend_Mode = GetShaderParametersList(template_parameters.blend_mode, typeof(Blend_Mode));
            var params_Parallax = GetShaderParametersList(template_parameters.parallax, typeof(Parallax));
            var params_Misc = GetShaderParametersList(template_parameters.misc, typeof(Misc));
            var params_Distortion = GetShaderParametersList(template_parameters.distortion, typeof(Distortion));
            var params_Soft_fade = GetShaderParametersList(template_parameters.soft_fade, typeof(Soft_Fade));

            List<List<TemplateParameter>> parameter_lists = new List<List<TemplateParameter>>
            {
                params_Albedo,
                params_Bump_Mapping,
                params_Alpha_Test,
                params_Specular_Mask,
                params_Material_Model,
                params_Environment_Mapping,
                params_Self_Illumination,
                params_Blend_Mode,
                params_Parallax,
                params_Misc,
                params_Distortion,
                params_Soft_fade
            };

            IndicesManager indices = new IndicesManager();
            List<ShaderParameter> parameters = new List<ShaderParameter>();


            foreach (var _params in parameter_lists)
            {
                var vector_params = GenerateShaderParameters(ShaderParameter.RType.Vector, cacheContext, _params, indices);
                var sampler_params = GenerateShaderParameters(ShaderParameter.RType.Sampler, cacheContext, _params, indices);
                var boolean_params = GenerateShaderParameters(ShaderParameter.RType.Boolean, cacheContext, _params, indices);

                parameters.AddRange(vector_params);
                parameters.AddRange(sampler_params);
                parameters.AddRange(boolean_params);
            }






            //parameters = 
            //var vector_params = GenerateShaderParameters(ShaderParameter.RType.Vector, cacheContext, _params, indices);
            //var sampler_params = GenerateShaderParameters(ShaderParameter.RType.Sampler, cacheContext, _params, indices);
            //var boolean_params = GenerateShaderParameters(ShaderParameter.RType.Boolean, cacheContext, _params, indices);

            //parameters.AddRange(vector_params);
            //parameters.AddRange(sampler_params);
            //parameters.AddRange(boolean_params);

            return parameters;
        }

    };
}
