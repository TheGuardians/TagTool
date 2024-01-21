using HaloShaderGenerator.Shader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Commands.Shaders;
using TagTool.Common;
using TagTool.Shaders.ShaderGenerator;
using TagTool.Shaders.ShaderMatching;
using TagTool.Tags.Definitions;
using static TagTool.Shaders.ShaderMatching.ShaderMatcherNew;
using static TagTool.Tags.Definitions.MultiplayerVariantSettingsInterfaceDefinition.GameEngineSetting;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;
using ShaderGen2 = TagTool.Tags.Definitions.Gen2.Shader;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        static readonly Dictionary<string, string> ShaderTypeGroups = new Dictionary<string, string>
        {
            ["shader"] = "rmsh",
            ["decal"] = "rmd ",
            ["halogram"] = "rmhg",
            ["water"] = "rmw ",
            ["black"] = "rmbk",
            ["terrain"] = "rmtr",
            ["custom"] = "rmcs",
            ["foliage"] = "rmfl",
            ["screen"] = "rmss",
            ["cortana"] = "rmct",
            ["zonly"] = "rmzo",
        };
        private RenderMethod Definition { get; set; }
        public Shader ConvertShader(ShaderGen2 gen2Shader, ShaderGen2 gen2ShaderH2, String gen2TagName, Stream cacheStream, Stream gen2CacheStream)
        {

            string shader_template = gen2ShaderH2.Template.Name;
            string shader_template_name = shader_template;
            shader_template = shader_template.Split('\\').Last();

            byte[] shaderCategories = new byte[Enum.GetValues(typeof(HaloShaderGenerator.Shader.ShaderMethods)).Length];
            for (var i = 0; i < shaderCategories.Length; i++)
                shaderCategories[i] = 0;

            // Declare string lists that contain the order and contents for each of these tagblocks
            /*            // Create a list to store the usage types
                        List<string> bitmap_list = new List<string>();

                        // Iterating through each bitmap
                        var bitmaps = gen2Shader.PostprocessDefinition[0].Bitmaps;
                        foreach (var bitmapEntry in bitmaps)
                        {
                            var bitmapRef = bitmapEntry.Bitmap;
                            BitmapGen2 bitmapDefinition = Gen2Cache.Deserialize<BitmapGen2>(gen2CacheStream, bitmapRef);

                            // Checking the Usage value
                            switch (bitmapDefinition.Usage)
                            {
                                case BitmapGen2.UsageValue.AlphaBlend:
                                    bitmap_list.Add("alpha_test_map");
                                    break;

                                case BitmapGen2.UsageValue.Default:
                                    {
                                        // Perform a check on the tag name
                                        string tagName = bitmapRef.ToString().Split('\\').Last();

                                        switch (tagName)
                                        {
                                            case string s when s.Contains("change_color") || s.Contains("chg_color") || s.Contains("_cc"):
                                                bitmap_list.Add("change_color_map");
                                                break;

                                            case string s when s.Contains("illum") && !s.Contains("mask"):
                                                bitmap_list.Add("self_illum_map");
                                                break;

                                            case string s when s.Contains("illum_mask"):
                                                bitmap_list.Add("self_illum_mask_map");
                                                break;

                                            case string s when s.Contains("detail"):
                                                bitmap_list.Add("detail_map");
                                                break;

                                            case string s when s.Contains("noise"):
                                                bitmap_list.Add("noise");
                                                break;

                                            case string s when s.Contains("offset"):
                                                bitmap_list.Add("offset");
                                                break;

                                            case string s when s.Contains("specular"):
                                                bitmap_list.Add("specular_mask_texture");
                                                break;

                                            default:
                                                bitmap_list.Add("base_map");
                                                break;
                                        }
                                    }
                                    break;

                                case BitmapGen2.UsageValue.HeightMap:
                                    bitmap_list.Add("bump_map");
                                    break;

                                case BitmapGen2.UsageValue.DetailMap:
                                    bitmap_list.Add("detail_map");
                                    break;

                                    // Optionally, handle other cases or add a default case
                            }
                        }*/

            // Declare string lists that contain the order and contents for each of these tagblocks
            var h2_bitmap_order = new List<string>();
            var h2_bitmap_order_pass2 = new List<string>();
            var h2_pixel_constants = new List<string>();
            var h2_vertex_constants = new List<string>();
            var h2_vertex_constants_pass2 = new List<string>();
            var h2_value_properties = new List<string>();
            var h2_color_properties = new List<string>();
            var h2_overlay_references = new List<string>();
            var new_shader_type = "";

            // MODIFIERS FOR VERTEX CONSTANTS
            // ! = Flip the orientation of the constant values
            // + = vertex constant is the X scale and the next index is the Y scale 
            //
            var modifiers = new List<char>
            {
                '!',
                '+'
            };

            switch (shader_template)
            {
                case "plasma_alpha":
                    {
                        new_shader_type = "rmhg";

                        h2_pixel_constants.Add("color_sharp"); // og: color_medium
                        h2_pixel_constants.Add("color_wide"); // og: color_wide
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("color_medium"); // og: color_sharp

                        h2_vertex_constants.Add("noise_map_a_scale_x"); // noise_map_a_scale_x
                        h2_vertex_constants.Add("noise_map_a_scale_y"); // noise_map_a_scale_y
                        h2_vertex_constants.Add("noise_map_b_scale_x"); // noise_map_b_scale_x
                        h2_vertex_constants.Add("noise_map_b_scale_y"); // noise_map_b_scale_y
                        h2_vertex_constants.Add(""); // 
                        h2_vertex_constants.Add(""); // 
                        h2_vertex_constants.Add(""); // 
                        h2_vertex_constants.Add(""); // 

                        h2_bitmap_order.Add("noise_map_a");
                        h2_bitmap_order.Add("noise_map_b");
                        h2_bitmap_order.Add("alpha_mask_map");
                        break;
                    }
                case "tex_alpha_test":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        h2_vertex_constants_pass2.Add("alpha_test_map");

                        if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_vector" ||
                            gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_multiplicative")
                        {
                            h2_bitmap_order_pass2.Add("alpha_test_map");
                            h2_bitmap_order.Add("");
                        }
                        else
                        {
                            h2_bitmap_order.Add("alpha_test_map");
                        }
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        break;
                    }
                case "tex_bump":
                case "tex_bump_active_camo":
                case "tex_bump_shiny":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        
                        
                        h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                        h2_pixel_constants.Add("glancing_specular_tint"); // specular_glancing_color
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("alpha_test_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        break;
                    }
                case "tex_bump_alpha_test":
                case "tex_bump_alpha_test_clamped":
                    {
                        new_shader_type = "rmsh";

                        if (Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants_pass2.Add("");
                            h2_vertex_constants_pass2.Add("alpha_test_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants_pass2.Add("");
                            h2_vertex_constants_pass2.Add("alpha_test_map");
                        }
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\color_white")
                        {
                            h2_bitmap_order_pass2.Add("alpha_test_map");
                            h2_bitmap_order.Add("");
                        }
                        else
                        {
                            h2_bitmap_order.Add("alpha_test_map");
                        }
                        h2_bitmap_order.Add("");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        break;
                    }
                case "tex_bump_alpha_test_single_pass":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        h2_vertex_constants_pass2.Add("");
                        h2_vertex_constants_pass2.Add("alpha_test_map");

                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\color_white")
                        {
                            h2_bitmap_order_pass2.Add("alpha_test_map");
                            h2_bitmap_order.Add("");
                        }
                        else
                        {
                            h2_bitmap_order.Add("alpha_test_map");
                        }
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        break;
                    }
                case "tex_bump_detail_blend":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                        }
                        
                        h2_pixel_constants.Add("albedo_color");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("detail_map2");
                        break;
                    }
                case "tex_bump_detail_blend_detail":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("!detail_map_overlay");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("!detail_map_overlay");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                        
                        }
                        
                        h2_pixel_constants.Add("albedo_color");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("detail_map2");
                        h2_bitmap_order.Add("detail_map_overlay");
                        break;
                    }
                case "tex_bump_detail_keep":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        break;
                    }
                case "tex_bump_detail_overlay":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                        }
                        
                        h2_pixel_constants.Add("albedo_color");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("detail_map2");
                        break;
                    }
                case "tex_bump_dprs_env":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("env_tint_color");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("environment_map_specular_contribution");
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("alpha_test_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("environment_map");
                        break;
                    }
                case "tex_bump_env":
                case "tex_bump_env_clamped":
                case "tex_bump_env_combined":
                case "tex_bump_env_dbl_spec":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                        }
                        
                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_pixel_constants.Add("env_tint_color"); // env tint color
                            h2_pixel_constants.Add(""); // env glancing tint color
                            h2_pixel_constants.Add("environment_map_specular_contribution"); // env brightness
                            h2_pixel_constants.Add(""); // env glancing brightness
                            h2_pixel_constants.Add("normal_specular_tint"); // specular color
                            h2_pixel_constants.Add("glancing_specular_tint"); // glancing specular color
                        }
                        else
                        {
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("env_tint_color"); // env tint color
                            h2_pixel_constants.Add(""); // env glancing tint color
                            h2_pixel_constants.Add("environment_map_specular_contribution"); // env brightness
                            h2_pixel_constants.Add(""); // env glancing brightness
                            h2_pixel_constants.Add("normal_specular_tint"); // specular color
                            h2_pixel_constants.Add("glancing_specular_tint"); // glancing specular color
                        }
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("alpha_test_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("environment_map");
                        break;
                    }
                case "tex_bump_env_illum_3_channel":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("environment_map_specular_contribution"); // env_brightness
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("self_illum_map");
                        
                        h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                        h2_pixel_constants.Add("glancing_specular_tint"); // glancing_specular_color
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_a"); // channel_a_color
                        h2_pixel_constants.Add("channel_b"); // channel_b_color
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_c"); // channel_c_color
                        h2_pixel_constants.Add("env_tint_color"); // env_tint_color
                        h2_pixel_constants.Add(""); // env_glancing_tint_color
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("environment_map");
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                // H2 implementation of self illum occlusion dosent exist in HO, this wont be "exact"
                case "tex_bump_env_illum_3_channel_occlusion_combined":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("self_illum_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("self_illum_map");
                        }
                        
                        
                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                            h2_pixel_constants.Add("glancing_specular_tint"); // specular_glancing_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("channel_a"); // channel_a_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("channel_b"); // channel_b_color
                            h2_pixel_constants.Add("channel_c"); // channel_c_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("env_tint_color"); // env_tint_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("environment_map_specular_contribution"); // env_brightness
                        }
                        else
                        {
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("env_tint_color"); // env_tint_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("environment_map_specular_contribution"); // env_brightness
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                            h2_pixel_constants.Add("glancing_specular_tint"); // specular_glancing_color
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("");
                            h2_pixel_constants.Add("channel_a"); // channel_a_color
                            h2_pixel_constants.Add("channel_b"); // channel_b_color
                            h2_pixel_constants.Add("channel_c"); // channel_c_color
                        }
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("environment_map");
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "tex_bump_env_alpha_test":
                case "tex_bump_env_alpha_clamped":
                case "tex_bump_env_alpha_test_indexed":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("alpha_test_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        
                        h2_pixel_constants.Add("env_tint_color"); // env_tint_color
                        h2_pixel_constants.Add(""); // env_glancing_tint_color
                        h2_pixel_constants.Add("environment_map_specular_contribution"); // env_brightness
                        h2_pixel_constants.Add(""); // env_glancing_brightness
                        h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                        h2_pixel_constants.Add("glancing_specular_tint"); // glancing_specular_color
                        
                        h2_bitmap_order.Add("bump_map");
                        if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_vector" ||
                            gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_multiplicative")
                        {
                            h2_bitmap_order_pass2.Add("");
                            h2_bitmap_order_pass2.Add("");
                            h2_bitmap_order_pass2.Add("alpha_test_map");
                            h2_bitmap_order.Add("");
                        }
                        else
                        {
                            h2_bitmap_order.Add("alpha_test_map");
                        }
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("environment_map");
                        break;
                    }
                case "tex_bump_env_no_detail":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");

                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("env_tint_color");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("environment_map_specular_contribution");
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("environment_map");
                        break;
                    }
                case "tex_bump_env_two_change_color_indexed":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("change_color_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("!bump_map");
                            h2_vertex_constants.Add("change_color_map");
                        }
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("env_tint_color");
                        h2_pixel_constants.Add("environment_map_specular_contribution");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("change_color_map");
                        h2_bitmap_order.Add("environment_map");
                        break;
                    }
                case "tex_bump_illum":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("detail_map");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("self_illum_map");
                        
                        h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                        h2_pixel_constants.Add("glancing_specular_tint"); // glancing_specular_color
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_color_properties.Add("self_illum_color"); // emissive_color
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "tex_bump_illum_3_channel":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("self_illum_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("self_illum_map");
                        }
                        
                        h2_pixel_constants.Add("normal_specular_tint"); // specular_color
                        h2_pixel_constants.Add("glancing_specular_tint"); // specular_glancing_color
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_a"); // channel_a_color
                        h2_pixel_constants.Add("channel_b"); // channel_b_color
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_c"); // channel_c_color
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "tex_bump_one_change_color":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("bump_map");
                        h2_vertex_constants.Add("base_map");
                        h2_vertex_constants.Add("change_color_map");
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("change_color_map");
                        break;
                    }
                case "tex_bump_three_detail_blend":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("!detail_map3");
                        }
                        else
                        {
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("!detail_map3");
                            h2_vertex_constants.Add("bump_map");
                        
                        }
                        
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("detail_map2");
                        h2_bitmap_order.Add("detail_map3");
                        break;
                    }
                case "tex_bump_two_detail":
                    {
                        new_shader_type = "rmsh";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("bump_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("bump_map");
                            h2_vertex_constants.Add("base_map");
                            h2_vertex_constants.Add("detail_map");
                            h2_vertex_constants.Add("detail_map2");
                        
                        }

                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("normal_specular_tint");
                        h2_pixel_constants.Add("glancing_specular_tint");
                        
                        h2_bitmap_order.Add("bump_map");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("detail_map");
                        h2_bitmap_order.Add("detail_map2");
                        break;
                    }
                case "illum":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("self_illum_map");
                        
                        h2_pixel_constants.Add("self_illum_color");
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "illum_3_channel":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("self_illum_map");

                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_a");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("channel_b");
                        h2_pixel_constants.Add("channel_c");
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "one_add_illum":
                    {
                        new_shader_type = "rmsh";

                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("self_illum_color");
                        h2_vertex_constants.Add("self_illum_map");
                        h2_vertex_constants.Add("albedo_color");
                        
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                        
                        h2_bitmap_order.Add("self_illum_map");
                        break;
                    }
                case "one_alpha_env":
                case "one_alpha_env_active_camo":
                    {
                        new_shader_type = "rmsh";

                        h2_bitmap_order.Add("environment_map");
                        h2_bitmap_order.Add("specular_mask_texture");
                        h2_bitmap_order.Add("base_map");
                        
                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("+specular_mask_texture");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("+base_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("+specular_mask_texture");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("+base_map");
                        }
                        
                        h2_pixel_constants.Add("env_tint_color");
                        h2_pixel_constants.Add("albedo_color");
                        break;
                    }
                case "sky_one_alpha_env":
                    {
                        new_shader_type = "rmsh";

                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.From_Diffuse;
                        h2_bitmap_order.Add("environment_map");
                        h2_bitmap_order.Add("specular_mask_texture");
                        h2_bitmap_order.Add("base_map");
                        
                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants.Add("+specular_mask_texture");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("+base_map");
                        }
                        else
                        {
                            h2_vertex_constants.Add("+specular_mask_texture");
                            h2_vertex_constants.Add("");
                            h2_vertex_constants.Add("+base_map");
                        }
                        
                        h2_pixel_constants.Add("env_tint_color");
                        h2_pixel_constants.Add("albedo_color");
                        break;
                    }
                case "sky_one_alpha_env_illum":
                    {
                        new_shader_type = "rmsh";

                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.From_Diffuse;
                        h2_bitmap_order.Add("environment_map");
                        h2_bitmap_order.Add("specular_mask_texture");
                        h2_bitmap_order.Add("base_map");
                        h2_bitmap_order.Add("self_illum_map");
                    
                        h2_vertex_constants.Add("+specular_mask_texture");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("+base_map");
                        h2_vertex_constants.Add("");
                        h2_vertex_constants.Add("+self_illum_map");
                    
                        h2_value_properties.Add("self_illum_intensity"); // emissive_power
                    
                        h2_pixel_constants.Add("env_tint_color");
                        h2_pixel_constants.Add("");
                        h2_pixel_constants.Add("albedo_color");
                        h2_pixel_constants.Add("self_illum_color");
                        break;
                    }
                case "two_alpha_clouds":
                case "sky_two_alpha_clouds":
                    {
                        new_shader_type = "rmsh";

                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.From_Diffuse;
                        h2_vertex_constants.Add("+detail_map2");
                        h2_vertex_constants.Add("+detail_map_overlay");
                    
                        h2_bitmap_order.Add("detail_map2");
                        h2_bitmap_order.Add("detail_map_overlay");
                        break;
                    }
                default:
                    new TagToolWarning($"Shader template '{shader_template}' not yet supported!");
                    return null;
            }

            // Change the contents of lists depending on the h2 template used
            if (new_shader_type == "rmsh")
            {
                // Albedo
                if (shader_template.Contains("change_color") || shader_template.Contains("detail") || shader_template.Contains("cloud"))
                {
                    // Change Color
                    if (shader_template.Contains("one_change_color") || shader_template.Contains("two_change_color"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Two_Change_Color;
                    }
                    else if (shader_template.Contains("four_change_color"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Four_Change_Color;
                    }

                    // Detail
                    if (shader_template.Contains("two_detail"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Two_Detail;
                    }
                    else if (shader_template.Contains("blend_detail"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Two_Detail_Overlay;
                    }
                    else if (shader_template.Contains("blend"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Detail_Blend;
                    }

                    // Clouds
                    if (shader_template.Contains("cloud"))
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Two_Detail_Overlay;
                    }
                }

                // Bump Mapping
                if (shader_template.Contains("bump"))
                {
                    shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                    // Additional logic for bump mapping
                }

                // Alpha Test
                if (shader_template.Contains("alpha_test"))
                {
                    shaderCategories[(int)ShaderMethods.Alpha_Test] = (byte)Alpha_Test.Simple;
                }

                // Specular Mask
                if (shader_template.Contains("specular"))
                {
                    if (shader_template.Contains("tiling"))
                    {
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Texture;
                    }
                    else
                    {
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Color_Texture;
                    }
                }

                // Material Model
                if (shader_template.Contains("tex"))
                {
                    if (shader_template.Contains("specular"))
                    {
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        // Additional logic for tex + specular
                    }
                    else
                    {
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                    }
                }

                // Environment Mapping
                if (shader_template.Contains("env"))
                {
                    shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;
                    // Additional logic for env
                }

                // Self Illumination
                if (shader_template.Contains("illum"))
                {
                    if (shader_template.Contains("3_channel") || shader_template.Contains("plasma"))
                    {
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination._3_Channel_Self_Illum;
                    }
                    else if (shader_template.Contains("detail"))
                    {
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.Illum_Detail;
                    }
                    else if (shader_template.Contains("meter"))
                    {
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.Meter;
                    }
                    else
                    {
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.Simple;
                        new TagToolWarning($"Shader template '{shader_template}' has unknown or default illum type");
                    }
                }

                // Blend Mode
                if (shader_template.Contains("add"))
                {
                    shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Additive;
                }
                else if (shader_template.Contains("alpha") && !gen2TagName.Contains("stars"))
                {
                    shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Alpha_Blend;
                }
            }
            else if (new_shader_type == "rmhg")
            {
                // do stuff for halograms
            }

            string rmt2TagName = @"shaders\shader_templates\_" + string.Join("_", shaderCategories);

            ShaderMatcherNew.Rmt2Descriptor rmt2Desc = new ShaderMatcherNew.Rmt2Descriptor("shader", shaderCategories);

            CachedTag rmdfTag = Cache.TagCache.GetTag<RenderMethodDefinition>($"shaders\\{rmt2Desc.Type}");
            RenderMethodDefinition rmdf;

            RenderMethodTemplate rmt2Definition;
            if (!Cache.TagCacheGenHO.TryGetTag(rmt2TagName + ".rmt2", out CachedTag rmt2Tag))
            {
                // Generate the template
                var generator = rmt2Desc.GetGenerator(true);

                GlobalPixelShader glps;
                GlobalVertexShader glvs;
                if (!Cache.TagCache.TryGetTag($"shaders\\{rmt2Desc.Type}.rmdf", out rmdfTag))
                {
                    Console.WriteLine($"Generating rmdf for \"{rmt2Desc.Type}\"");
                    rmdf = TagTool.Shaders.ShaderGenerator.RenderMethodDefinitionGenerator.GenerateRenderMethodDefinition(Cache, cacheStream, generator, rmt2Desc.Type, out glps, out glvs);
                    rmdfTag = Cache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{rmt2Desc.Type}");
                    Cache.Serialize(cacheStream, rmdfTag, rmdf);
                    Cache.SaveTagNames();
                }
                else
                {
                    rmdf = Cache.Deserialize<RenderMethodDefinition>(cacheStream, rmdfTag);
                    glps = Cache.Deserialize<GlobalPixelShader>(cacheStream, rmdf.GlobalPixelShader);
                    glvs = Cache.Deserialize<GlobalVertexShader>(cacheStream, rmdf.GlobalVertexShader);
                }

                rmt2Definition = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, cacheStream, rmdf, glps, glvs, generator, rmt2TagName, out PixelShader pixl, out VertexShader vtsh);
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2TagName);

                Cache.Serialize(cacheStream, rmt2Tag, rmt2Definition);
            }
            else
            {
                rmt2Definition = Cache.Deserialize<RenderMethodTemplate>(cacheStream, rmt2Tag);
            }

            // check if tag already exists, or allocate new one
            string rmGroup = ShaderTypeGroups[rmt2Desc.Type];

            var rmt2 = Cache.Deserialize<RenderMethodTemplate>(cacheStream, rmt2Tag);
            rmdf = Cache.Deserialize<RenderMethodDefinition>(cacheStream, rmdfTag);

            // store rmop definitions for quick lookup
            List<RenderMethodOption> renderMethodOptions = new List<RenderMethodOption>();
            for (int i = 0; i < rmt2Desc.Options.Length; i++)
            {
                var rmopTag = rmdf.Categories[i].ShaderOptions[rmt2Desc.Options[i]].Option;
                if (rmopTag != null)
                    renderMethodOptions.Add(Cache.Deserialize<RenderMethodOption>(cacheStream, rmopTag));
            }

            // create definition
            object definition = Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(rmGroup));
            // make changes as RenderMethod so the code can be reused for each rm type
            var rmDefinition = definition as RenderMethod;

            rmDefinition.BaseRenderMethod = rmdfTag;

            // initialize lists
            rmDefinition.Options = new List<RenderMethod.RenderMethodOptionIndex>();
            rmDefinition.ShaderProperties = new List<RenderMethod.RenderMethodPostprocessBlock>();

            foreach (var option in rmt2Desc.Options)
                rmDefinition.Options.Add(new RenderMethod.RenderMethodOptionIndex { OptionIndex = option });
            rmDefinition.SortLayer = TagTool.Shaders.SortingLayerValue.Normal;
            rmDefinition.PredictionAtomIndex = -1;

            PopulateRenderMethodConstants populateConstants = new PopulateRenderMethodConstants();

            // setup shader property
            RenderMethod.RenderMethodPostprocessBlock shaderProperty = new RenderMethod.RenderMethodPostprocessBlock
            {
                Template = rmt2Tag,
                // setup constants
                TextureConstants = populateConstants.SetupTextureConstants(rmt2, renderMethodOptions, Cache),
                RealConstants = populateConstants.SetupRealConstants(rmt2, renderMethodOptions, Cache),
                IntegerConstants = populateConstants.SetupIntegerConstants(rmt2, renderMethodOptions, Cache),
                BooleanConstants = populateConstants.SetupBooleanConstants(rmt2, renderMethodOptions, Cache),
                // get alpha blend mode
                BlendMode = populateConstants.GetAlphaBlendMode(rmt2Desc, rmdf, Cache),
                // TODO
                QueryableProperties = new short[] { -1, -1, -1, -1, -1, -1, -1, -1 }
            };

            rmDefinition.ShaderProperties.Add(shaderProperty);
            rmDefinition.BaseRenderMethod = Cache.TagCacheGenHO.GetTag<RenderMethodDefinition>(rmt2Desc.GetRmdfName());
            Definition = rmDefinition;

            // Add all the texture maps
            foreach (var shadermap in rmt2Definition.TextureParameterNames)
            {
                bool found = false;
                var h2_postprocess = gen2Shader.PostprocessDefinition[0];
                var h2_texture_reference = h2_postprocess.Bitmaps;
                string current_type = Cache.StringTable[((int)shadermap.Name.Value)];   // Gets the current type of bitmap in the template
                string current_bitmap = h2_texture_reference[0].Bitmap.ToString(); ; // Sets up the variable for bitmap assignment and reference

                // If the string in the bitmap order list matches the current_type in the rmt2,
                // Then set current_type to the bitmap path
                for (var i = 0; i < h2_bitmap_order.Count; i++)
                {
                    if (h2_bitmap_order[i] == current_type)
                    {
                        found = true;
                        if (h2_texture_reference[i].Bitmap != null) { current_bitmap = h2_texture_reference[i].Bitmap.ToString(); }
                        else current_bitmap = null;
                        break;
                    }
                }

                if (found == false)
                {
                    // Loop through a second time incase 1 bitmap is used in 2 bitmap fields
                    for (var i = 0; i < h2_bitmap_order_pass2.Count; i++)
                    {
                        if (h2_bitmap_order_pass2[i] == current_type)
                        {
                            found = true;
                            current_bitmap = h2_texture_reference[i].Bitmap.ToString();
                            break;
                        }
                    }
                }

                // If bitmap type is not found in list just give it a default bitmap
                //if (found == false) current_type = "shaders\\default_bitmaps\\bitmaps\\alpha_grey50.bitmap";

                // set bitmaps
                if (current_type != null)
                {
                    for (int samplerIndex = 0; samplerIndex < rmt2Definition.TextureParameterNames.Count; samplerIndex++)
                    {
                        string sampler_name = Cache.StringTable[(int)rmt2Definition.TextureParameterNames[samplerIndex].Name.Value];
                        if (sampler_name == current_type)
                        {
                            try
                            {
                                Definition.ShaderProperties[0].TextureConstants[samplerIndex].Bitmap = Cache.TagCacheGenHO.GetTag(current_bitmap);
                                break;
                            }
                            catch
                            {
                                new TagToolWarning($"tried to set '{current_type}' as bitmap reference, bitmap must not exist");
                                break;
                            }
                        }
                    }
                }
            }

            int current_index = -1;
            // Declare Real Parameters and "try" to convert the similar counterparts in h2 to h3 equivalents
            foreach (var floatconstant in rmt2Definition.RealParameterNames)
            {
                current_index++;
                bool found = false;
                var h2resource = gen2Shader.PostprocessDefinition;
                var h2pixel_constant = h2resource[0].PixelConstants;
                var h2vertex_constant = h2resource[0].VertexConstants;
                var h2color_properties = h2resource[0].ColorProperties;
                var h2value_properties = h2resource[0].ValueProperties;

                string current_type = Cache.StringTable[((int)floatconstant.Name.Value)];   // Gets the current type of bitmap in the template

                switch (current_type)
                {
                    // Set specular_coefficient to specular value for h2
                    case "specular_coefficient":
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)gen2Shader.LightmapSpecularBrightness;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)gen2Shader.LightmapSpecularBrightness;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)gen2Shader.LightmapSpecularBrightness;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)gen2Shader.LightmapSpecularBrightness;
                        break;

                    case "environment_map_specular_contribution":
                        for (int i = 0; i < h2_pixel_constants.Count; i++)
                        {
                            if (h2_pixel_constants[i] == current_type)
                            {
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10;
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            for (int i2 = 0; i2 < h2_vertex_constants.Count; i2++)
                            {
                                if (h2_vertex_constants[i2] == current_type)
                                {
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = ((float)h2vertex_constant[i2].W) / 10;
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = ((float)h2vertex_constant[i2].W) / 10;
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = ((float)h2vertex_constant[i2].W) / 10;
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = ((float)h2vertex_constant[i2].W) / 10;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found == false)
                        {
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)0;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)0;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)0;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)0;
                        }
                        break;

                    case "area_specular_contribution":
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)0;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)0;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)0;
                        Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)0;
                        break;

                    case "analytical_specular_contribution":
                        if (shader_template.Contains("no_detail"))
                        {
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)0.25;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)0.25;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)0.25;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)0.25;
                            break;
                        }
                        else
                        {
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)0.5;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)0.5;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)0.5;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)0.5;
                            break;
                        }


                    case "self_illum_intensity":
                        for (int i = 0; i < h2_value_properties.Count; i++)
                        {
                            if (h2_value_properties[i] == current_type)
                            {
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)h2value_properties[i].Value * 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)h2value_properties[i].Value * 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)h2value_properties[i].Value * 10;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)h2value_properties[i].Value * 10;
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)1;
                            break;
                        }
                        break;

                    case "self_illum_color":
                        for (int i = 0; i < h2_pixel_constants.Count; i++)
                        {
                            if (h2_pixel_constants[i] == current_type)
                            {
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = ((float)h2pixel_constant[i].Color.Red) / 255;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = ((float)h2pixel_constant[i].Color.Green) / 255;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = ((float)h2pixel_constant[i].Color.Blue) / 255;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = ((float)h2pixel_constant[i].Color.Alpha) / 255;
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            for (int i = 0; i < h2_vertex_constants.Count; i++)
                            {
                                if (h2_vertex_constants[i] == current_type)
                                {
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = ((float)h2vertex_constant[i].Vector3.I);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = ((float)h2vertex_constant[i].Vector3.J);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = ((float)h2vertex_constant[i].Vector3.K);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)1;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found == false)
                        {
                            for (int i = 0; i < h2_color_properties.Count; i++)
                            {
                                if (h2_color_properties[i] == current_type)
                                {
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = ((float)h2color_properties[i].Color.Red);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = ((float)h2color_properties[i].Color.Green);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = ((float)h2color_properties[i].Color.Blue);
                                    Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)1;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found == false)
                        {
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = (float)1;
                            Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = (float)1;
                        }
                            break;

                    // If no float constant specific edits are needed 
                    default:
                        // Convert Pixel Constants to Float Constants if matches are found
                        for (var i = 0; i < h2_pixel_constants.Count; i++)
                        {
                            if (h2_pixel_constants[i] == current_type)
                            {
                                found = true;
                                float r = -1, g = -1, b = -1, a = -1;

                                // Convert values in h2 pixel constant block to the layout and values it expects in h3
                                // (For numeric values in shader)
                                // Set (rgba) to value of (a) in h3 shader if only (a) has a value and (rgb) dosent in h2
                                if
                                (h2pixel_constant[i].Color.Red == 0 && h2pixel_constant[i].Color.Green == 0 && h2pixel_constant[i].Color.Blue == 0 && h2pixel_constant[i].Color.Alpha > 0)
                                {
                                    r = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                    g = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                    b = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                    a = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                }

                                // (For colours in shader)
                                // Set (a) to value of 1 if any (rgb) values are above 0 and (a) is 0
                                if
                                (h2pixel_constant[i].Color.Red > 0 || h2pixel_constant[i].Color.Green > 0 || h2pixel_constant[i].Color.Blue > 0 && h2pixel_constant[i].Color.Alpha == 0)
                                {
                                    r = (float)h2pixel_constant[i].Color.Red / (float)255;
                                    g = (float)h2pixel_constant[i].Color.Green / (float)255;
                                    b = (float)h2pixel_constant[i].Color.Blue / (float)255;
                                    a = 1;
                                }

                                Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = r;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = g;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = b;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = a;
                            }
                        }
                        // Convert Color Properties to Float Constants if matches are found
                        for (var i = 0; i < h2_color_properties.Count; i++)
                        {
                            if (h2_color_properties[i] == current_type)
                            {
                                found = true;
                                float r = -1, g = -1, b = -1, a = -1;

                                // (For colours in shader)
                                // Set (a) to value of 1 if any (rgb) values are above 0 and (a) is 0
                                if (h2color_properties[i].Color.Red > 0 || h2color_properties[i].Color.Green > 0 || h2color_properties[i].Color.Blue > 0)
                                {
                                    r = (float)h2color_properties[i].Color.Red / (float)255;
                                    g = (float)h2color_properties[i].Color.Green / (float)255;
                                    b = (float)h2color_properties[i].Color.Blue / (float)255;
                                    a = 1;
                                }

                                Definition.ShaderProperties[0].RealConstants[current_index].Arg0 = r;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg1 = g;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg2 = b;
                                Definition.ShaderProperties[0].RealConstants[current_index].Arg3 = a;
                            }
                        }

                        // If any Pixel Constants with matching types aren't found
                        // try to find and convert Vertex Constants to Float Constants
                        if (found == false)
                        {
                            MatchVertexConstant(h2_vertex_constants, modifiers, h2vertex_constant, current_type, ref found, current_index);
                        }

                        // If any Vertex Constants with matching types aren't found
                        // try to find and convert Vertex Constants from the second vertex constants list
                        if (found == false)
                        {
                            MatchVertexConstant(h2_vertex_constants_pass2, modifiers, h2vertex_constant, current_type, ref found, current_index);
                        }
                        break;
                }
            }
            return Definition as Shader;
        }

        private RenderMethod.RenderMethodPostprocessBlock.RealConstant MatchVertexConstant(List<string> h2_vertex_constants, List<char> modifiers,
            List<ShaderGen2.ShaderPostprocessDefinitionNewBlock.RealVector4dBlock> h2vertex_constant, string current_type, ref bool found, int index)
        {
            for (var i = 0; i < h2_vertex_constants.Count; i++)
            {
                string temp_type = null;
                char[] symbol = { ' ' };

                if (h2_vertex_constants[i] != "")
                {
                    symbol = h2_vertex_constants[i].Substring(0, 1).ToCharArray().DeepClone();
                    foreach (var modifier in modifiers)
                    {
                        if (symbol[0] == modifier) temp_type = h2_vertex_constants[i].Remove(0, 1);
                    }
                }

                if (h2_vertex_constants[i] == current_type || temp_type == current_type)
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant();
                    switch (symbol[0])
                    {
                        case '!':
                            {
                                Definition.ShaderProperties[0].RealConstants[index].Arg0 = h2vertex_constant[i].W;
                                Definition.ShaderProperties[0].RealConstants[index].Arg1 = h2vertex_constant[i].Vector3.K;
                                Definition.ShaderProperties[0].RealConstants[index].Arg2 = h2vertex_constant[i].Vector3.J;
                                Definition.ShaderProperties[0].RealConstants[index].Arg3 = h2vertex_constant[i].Vector3.I;
                            };
                            break;
                        case '+':
                            {
                                Definition.ShaderProperties[0].RealConstants[index].Arg0 = h2vertex_constant[i].Vector3.I;
                                Definition.ShaderProperties[0].RealConstants[index].Arg1 = h2vertex_constant[i + 1].Vector3.J;
                                Definition.ShaderProperties[0].RealConstants[index].Arg2 = h2vertex_constant[i].Vector3.K;
                                Definition.ShaderProperties[0].RealConstants[index].Arg3 = h2vertex_constant[i].W;
                            };
                            break;
                        default:
                            Definition.ShaderProperties[0].RealConstants[index].Arg0 = h2vertex_constant[i].Vector3.I;
                            Definition.ShaderProperties[0].RealConstants[index].Arg1 = h2vertex_constant[i].Vector3.J;
                            Definition.ShaderProperties[0].RealConstants[index].Arg2 = h2vertex_constant[i].Vector3.K;
                            Definition.ShaderProperties[0].RealConstants[index].Arg3 = h2vertex_constant[i].W;
                            break;
                    }
                    return newfloatconstant;
                }
            }
            return null;
        }
    }
}
