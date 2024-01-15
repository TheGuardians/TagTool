using HaloShaderGenerator.Shader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Shaders.ShaderMatching;
using TagTool.Tags.Definitions;
using ShaderGen2 = TagTool.Tags.Definitions.Gen2.Shader;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Shader ConvertShader(ShaderGen2 gen2Shader, ShaderGen2 gen2ShaderH2, Stream cacheStream, Stream gen2CacheStream)
        {
            var ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.Opaque;
            var PostProcessFlag = RenderMethod.RenderMethodPostprocessBlock.RenderMethodPostprocessFlags.None;

            string shader_template = gen2ShaderH2.Template.Name;
            string shader_template_name = shader_template;
            shader_template = shader_template.Split('\\').Last();

            var shader = new Shader()
            {
                Material = gen2Shader.MaterialName,
                ShaderProperties = new List<Shader.RenderMethodPostprocessBlock>(),
                Options = new List<Shader.RenderMethodOptionIndex>()
            };

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
            var h2_bitmap_order = new List<string> {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };
            var h2_bitmap_order_pass2 = new List<string> {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };

            var h2_pixel_constants = new List<string> {
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                "\0"
            };
            var h2_vertex_constants = new List<string>
            {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };
            var h2_vertex_constants_pass2 = new List<string>
            {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };
            var h2_value_properties = new List<string>
            {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };
            var h2_color_properties = new List<string>
            {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };

            // MODIFIERS FOR VERTEX CONSTANTS
            // ! = Flip the orientation of the constant values
            // + = vertex constant is the X scale and the next index is the Y scale 
            //
            var modifiers = new List<char>
            {
                '!',
                '+'
            };


            // Change the contents of lists depending on the h2 template used
            if (shader_template_name.Contains("opaque"))
            {
                // Albedo
                if (shader_template.Contains("change_color") || shader_template.Contains("detail"))
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
                    PostProcessFlag = RenderMethod.RenderMethodPostprocessBlock.RenderMethodPostprocessFlags.EnableAlphaTest;
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
                    ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.Additive;
                }
                else if (shader_template.Contains("alpha"))
                {
                    ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.AlphaBlend;
                }
            }

            switch (shader_template)
            {
                  case "tex_alpha_test":
                      {
                          h2_vertex_constants[0] = "base_map";
                          h2_vertex_constants_pass2[0] = "alpha_test_map";
                          h2_vertex_constants[1] = "detail_map";
                          h2_vertex_constants[2] = "\0";

                          if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_vector" ||
                              gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_multiplicative")
                          {
                              h2_bitmap_order_pass2[1] = "alpha_test_map";
                          }
                          else
                          {
                              h2_bitmap_order[0] = "alpha_test_map";
                          }
                          h2_bitmap_order[1] = "base_map";
                          h2_bitmap_order[2] = "\0";
                          break;
                      }
                  case "tex_bump":
                  case "tex_bump_active_camo":
                  case "tex_bump_shiny":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[3] = "\0";


                         h2_pixel_constants[0] = "normal_specular_tint"; // specular_color
                         h2_pixel_constants[1] = "glancing_specular_tint"; // specular_glancing_color
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "alpha_test_map";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "detail_map";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_alpha_test":
                  case "tex_bump_alpha_test_clamped":
                      {
                         if (Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants_pass2[1] = "alpha_test_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants_pass2[1] = "alpha_test_map";
                             h2_vertex_constants[2] = "bump_map";
                             h2_vertex_constants[3] = "\0";
                         }

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\color_white")
                         {
                             h2_bitmap_order_pass2[0] = "alpha_test_map";
                         }
                         else
                         {
                             h2_bitmap_order[1] = "alpha_test_map";
                         }
                         h2_bitmap_order[3] = "base_map";
                         h2_bitmap_order[4] = "detail_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_alpha_test_single_pass":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants_pass2[1] = "alpha_test_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[3] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\color_white")
                         {
                             h2_bitmap_order_pass2[0] = "alpha_test_map";
                         }
                         else
                         {
                             h2_bitmap_order[1] = "alpha_test_map";
                         }
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "detail_map";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_detail_blend":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "detail_map2";
                             h2_vertex_constants[4] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "detail_map2";
                             h2_vertex_constants[2] = "base_map";
                             h2_vertex_constants[3] = "bump_map";
                             h2_vertex_constants[4] = "\0";
                         }

                         h2_pixel_constants[0] = "albedo_color";
                         h2_pixel_constants[2] = "normal_specular_tint";
                         h2_pixel_constants[3] = "glancing_specular_tint";
                         h2_pixel_constants[4] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "detail_map2";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_detail_blend_detail":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "detail_map2";
                             h2_vertex_constants[4] = "!detail_map_overlay";
                             h2_vertex_constants[5] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "detail_map2";
                             h2_vertex_constants[2] = "!detail_map_overlay";
                             h2_vertex_constants[3] = "base_map";
                             h2_vertex_constants[4] = "bump_map";
                             h2_vertex_constants[5] = "\0";

                         }

                         h2_pixel_constants[0] = "albedo_color";
                         h2_pixel_constants[2] = "normal_specular_tint";
                         h2_pixel_constants[3] = "glancing_specular_tint";
                         h2_pixel_constants[4] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "detail_map2";
                         h2_bitmap_order[4] = "detail_map_overlay";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_detail_keep":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[3] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "\0";
                         break;
                      }
                  case "tex_bump_detail_overlay":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "detail_map2";
                             h2_vertex_constants[4] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "detail_map2";
                             h2_vertex_constants[2] = "base_map";
                             h2_vertex_constants[3] = "bump_map";
                             h2_vertex_constants[4] = "\0";
                         }

                         h2_pixel_constants[0] = "albedo_color";
                         h2_pixel_constants[2] = "normal_specular_tint";
                         h2_pixel_constants[3] = "glancing_specular_tint";
                         h2_pixel_constants[4] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "detail_map2";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_dprs_env":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[8] = "env_tint_color";
                         h2_vertex_constants[10] = "environment_map_specular_contribution";
                         h2_vertex_constants[11] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "alpha_test_map";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "detail_map";
                         h2_bitmap_order[4] = "environment_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_env":
                  case "tex_bump_env_clamped":
                  case "tex_bump_env_combined":
                  case "tex_bump_env_dbl_spec":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "bump_map";
                             h2_vertex_constants[3] = "\0";
                         }

                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_pixel_constants[0] = "env_tint_color"; // env tint color
                             //h2_pixel_constants[1] = ""; // env glancing tint color
                             h2_pixel_constants[2] = "environment_map_specular_contribution"; // env brightness
                             //h2_pixel_constants[3] = ""; // env glancing brightness
                             h2_pixel_constants[4] = "normal_specular_tint"; // specular color
                             h2_pixel_constants[5] = "glancing_specular_tint"; // glancing specular color
                             h2_pixel_constants[6] = "\0"; // terminator
                         }
                         else
                         {
                             h2_pixel_constants[1] = "env_tint_color"; // env tint color
                             //h2_pixel_constants[2] = ""; // env glancing tint color
                             h2_pixel_constants[3] = "environment_map_specular_contribution"; // env brightness
                             //h2_pixel_constants[4] = ""; // env glancing brightness
                             h2_pixel_constants[5] = "normal_specular_tint"; // specular color
                             h2_pixel_constants[6] = "glancing_specular_tint"; // glancing specular color
                             h2_pixel_constants[7] = "\0"; // terminator
                         }

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "alpha_test_map";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "detail_map";
                         h2_bitmap_order[4] = "environment_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_env_illum_3_channel":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[9] = "environment_map_specular_contribution"; // env_brightness
                         h2_vertex_constants[11] = "self_illum_map";
                         h2_vertex_constants[12] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint"; // specular_color
                         h2_pixel_constants[1] = "glancing_specular_tint"; // glancing_specular_color
                         h2_pixel_constants[7] = "channel_a"; // channel_a_color
                         h2_pixel_constants[7] = "channel_b"; // channel_b_color
                         h2_pixel_constants[9] = "channel_c"; // channel_c_color
                         h2_pixel_constants[10] = "env_tint_color"; // env_tint_color
                         //h2_pixel_constants[11] = ""; // env_glancing_tint_color
                         h2_pixel_constants[11] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "environment_map";
                         h2_bitmap_order[4] = "self_illum_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  // H2 implementation of self illum occlusion dosent exist in HO, this wont be "exact"
                  case "tex_bump_env_illum_3_channel_occlusion_combined":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[11] = "self_illum_map";
                             h2_vertex_constants[12] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "bump_map";
                             h2_vertex_constants[5] = "self_illum_map";
                             h2_vertex_constants[12] = "\0";
                         }


                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_pixel_constants[0] = "normal_specular_tint"; // specular_color
                             h2_pixel_constants[1] = "glancing_specular_tint"; // specular_glancing_color
                             h2_pixel_constants[6] = "channel_a"; // channel_a_color
                             h2_pixel_constants[8] = "channel_b"; // channel_b_color
                             h2_pixel_constants[9] = "channel_c"; // channel_c_color
                             h2_pixel_constants[13] = "env_tint_color"; // env_tint_color
                             h2_pixel_constants[15] = "environment_map_specular_contribution"; // env_brightness
                             h2_pixel_constants[16] = "\0";
                         }
                         else
                         {
                             h2_pixel_constants[1] = "env_tint_color"; // env_tint_color
                             h2_pixel_constants[3] = "environment_map_specular_contribution"; // env_brightness
                             h2_pixel_constants[5] = "normal_specular_tint"; // specular_color
                             h2_pixel_constants[6] = "glancing_specular_tint"; // specular_glancing_color
                             h2_pixel_constants[10] = "channel_a"; // channel_a_color
                             h2_pixel_constants[11] = "channel_b"; // channel_b_color
                             h2_pixel_constants[12] = "channel_c"; // channel_c_color
                             h2_pixel_constants[13] = "\0";
                         }

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "environment_map";
                         h2_bitmap_order[4] = "self_illum_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_env_alpha_test":
                  case "tex_bump_env_alpha_clamped":
                  case "tex_bump_env_alpha_test_indexed":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "alpha_test_map";
                         h2_vertex_constants[2] = "base_map";
                         h2_vertex_constants[3] = "detail_map";
                         h2_vertex_constants[4] = "\0";

                         h2_pixel_constants[0] = "env_tint_color"; // env_tint_color
                         //h2_pixel_constants[1] = ""; // env_glancing_tint_color
                         h2_pixel_constants[2] = "environment_map_specular_contribution"; // env_brightness
                         //h2_pixel_constants[3] = ""; // env_glancing_brightness
                         h2_pixel_constants[4] = "normal_specular_tint"; // specular_color
                         h2_pixel_constants[5] = "glancing_specular_tint"; // glancing_specular_color
                         h2_pixel_constants[6] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         if (gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_vector" ||
                             gen2ShaderH2.PostprocessDefinition[0].Bitmaps[1].Bitmap.Name == "shaders\\default_bitmaps\\bitmaps\\default_multiplicative")
                         {
                             h2_bitmap_order_pass2[2] = "alpha_test_map";
                         }
                         else
                         {
                             h2_bitmap_order[1] = "alpha_test_map";
                         }
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "detail_map";
                         h2_bitmap_order[4] = "environment_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_env_no_detail":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[4] = "\0";

                         h2_pixel_constants[1] = "env_tint_color";
                         h2_pixel_constants[3] = "environment_map_specular_contribution";
                         h2_pixel_constants[4] = "normal_specular_tint";
                         h2_pixel_constants[5] = "glancing_specular_tint";
                         h2_pixel_constants[6] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "environment_map";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_env_two_change_color_indexed":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "change_color_map";
                             h2_vertex_constants[4] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "!bump_map";
                             h2_vertex_constants[3] = "change_color_map";
                             h2_vertex_constants[4] = "\0";
                         }

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "";
                         h2_pixel_constants[3] = "";
                         h2_pixel_constants[4] = "";
                         h2_pixel_constants[5] = "env_tint_color";
                         h2_pixel_constants[6] = "environment_map_specular_contribution";
                         h2_pixel_constants[7] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "change_color_map";
                         h2_bitmap_order[4] = "environment_map";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_illum":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "detail_map";
                         h2_vertex_constants[7] = "self_illum_map";
                         h2_vertex_constants[8] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint"; // specular_color
                         h2_pixel_constants[1] = "glancing_specular_tint"; // glancing_specular_color
                         h2_pixel_constants[2] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_color_properties[0] = "self_illum_color"; // emissive_color
                         h2_color_properties[1] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "self_illum_map";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_illum_3_channel":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[4] = "detail_map";
                             h2_vertex_constants[5] = "self_illum_map";
                             h2_vertex_constants[6] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[4] = "detail_map";
                             h2_vertex_constants[7] = "self_illum_map";
                             h2_vertex_constants[8] = "\0";
                         }

                         h2_pixel_constants[0] = "normal_specular_tint"; // specular_color
                         h2_pixel_constants[1] = "glancing_specular_tint"; // specular_glancing_color
                         h2_pixel_constants[4] = "channel_a"; // channel_a_color
                         h2_pixel_constants[5] = "channel_b"; // channel_b_color
                         h2_pixel_constants[7] = "channel_c"; // channel_c_color
                         h2_pixel_constants[8] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "self_illum_map";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "tex_bump_one_change_color":
                      {
                         h2_vertex_constants[0] = "bump_map";
                         h2_vertex_constants[1] = "base_map";
                         h2_vertex_constants[2] = "change_color_map";
                         h2_vertex_constants[3] = "\0";

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "change_color_map";
                         h2_bitmap_order[3] = "\0";
                         break;
                      }
                  case "tex_bump_three_detail_blend":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "detail_map2";
                             h2_vertex_constants[4] = "!detail_map3";
                             h2_vertex_constants[5] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "base_map";
                             h2_vertex_constants[1] = "detail_map";
                             h2_vertex_constants[2] = "detail_map2";
                             h2_vertex_constants[3] = "!detail_map3";
                             h2_vertex_constants[4] = "bump_map";
                             h2_vertex_constants[5] = "\0";

                         }

                         h2_pixel_constants[0] = "normal_specular_tint";
                         h2_pixel_constants[1] = "glancing_specular_tint";
                         h2_pixel_constants[2] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "detail_map2";
                         h2_bitmap_order[4] = "detail_map3";
                         h2_bitmap_order[5] = "\0";
                         break;
                      }
                  case "tex_bump_two_detail":
                      {
                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "detail_map";
                             h2_vertex_constants[1] = "detail_map2";
                             h2_vertex_constants[2] = "base_map";
                             h2_vertex_constants[3] = "bump_map";
                             h2_vertex_constants[4] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "bump_map";
                             h2_vertex_constants[1] = "base_map";
                             h2_vertex_constants[2] = "detail_map";
                             h2_vertex_constants[3] = "detail_map2";
                             h2_vertex_constants[4] = "\0";

                         }

                         h2_pixel_constants[2] = "normal_specular_tint";
                         h2_pixel_constants[3] = "glancing_specular_tint";
                         h2_pixel_constants[4] = "\0";

                         h2_bitmap_order[0] = "bump_map";
                         h2_bitmap_order[1] = "base_map";
                         h2_bitmap_order[2] = "detail_map";
                         h2_bitmap_order[3] = "detail_map2";
                         h2_bitmap_order[4] = "\0";
                         break;
                      }
                  case "illum":
                      {
                         h2_vertex_constants[0] = "self_illum_map";
                         h2_vertex_constants[1] = "\0";

                         h2_pixel_constants[0] = "self_illum_color";
                         h2_pixel_constants[1] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "self_illum_map";
                         h2_bitmap_order[1] = "\0";
                         break;
                      }
                  case "illum_3_channel":
                      {
                         h2_vertex_constants[0] = "self_illum_map";
                         h2_vertex_constants[1] = "\0";

                         h2_pixel_constants[2] = "channel_a";
                         h2_pixel_constants[4] = "channel_b";
                         h2_pixel_constants[5] = "channel_c";
                         h2_pixel_constants[8] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "self_illum_map";
                         h2_bitmap_order[1] = "\0";
                         break;
                      }
                  case "one_add_illum":
                      {
                         h2_vertex_constants[2] = "self_illum_color";
                         h2_vertex_constants[3] = "self_illum_map";
                         h2_vertex_constants[4] = "albedo_color";
                         h2_vertex_constants[6] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_bitmap_order[0] = "self_illum_map";
                         h2_bitmap_order[1] = "\0";
                         break;
                      }
                  case "one_alpha_env":
                  case "one_alpha_env_active_camo":
                      {
                         shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Texture;
                         shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;
                         shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Alpha_Blend;
                         ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.AlphaBlend;

                         h2_bitmap_order[0] = "environment_map";
                         h2_bitmap_order[1] = "specular_mask_texture";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "\0";

                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "+specular_mask_texture";
                             h2_vertex_constants[2] = "+base_map";
                             h2_vertex_constants[3] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[4] = "+specular_mask_texture";
                             h2_vertex_constants[6] = "+base_map";
                             h2_vertex_constants[7] = "\0";
                         }

                         h2_pixel_constants[0] = "env_tint_color";
                         h2_pixel_constants[1] = "albedo_color";
                         h2_pixel_constants[2] = "\0";
                         break;
                      }
                  case "sky_one_alpha_env":
                      {
                         shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Texture;
                         shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;
                         shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Alpha_Blend;
                         ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.AlphaBlend;

                         h2_bitmap_order[0] = "environment_map";
                         h2_bitmap_order[1] = "specular_mask_texture";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "\0";

                         if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                         {
                             h2_vertex_constants[0] = "+specular_mask_texture";
                             h2_vertex_constants[2] = "+base_map";
                             h2_vertex_constants[3] = "\0";
                         }
                         else
                         {
                             h2_vertex_constants[0] = "+specular_mask_texture";
                             h2_vertex_constants[2] = "+base_map";
                             h2_vertex_constants[3] = "\0";
                         }

                         h2_pixel_constants[0] = "env_tint_color";
                         h2_pixel_constants[1] = "albedo_color";
                         h2_pixel_constants[2] = "\0";
                         break;
                      }
                  case "sky_one_alpha_env_illum":
                      {
                         shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Alpha_Blend;
                         shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;
                         shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.Simple;
                         shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Texture;
                         ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.AlphaBlend;

                         h2_bitmap_order[0] = "environment_map";
                         h2_bitmap_order[1] = "specular_mask_texture";
                         h2_bitmap_order[2] = "base_map";
                         h2_bitmap_order[3] = "self_illum_map";
                         h2_bitmap_order[4] = "\0";

                         h2_vertex_constants[0] = "+specular_mask_texture";
                         h2_vertex_constants[2] = "+base_map";
                         h2_vertex_constants[4] = "+self_illum_map";
                         h2_vertex_constants[5] = "\0";

                         h2_value_properties[0] = "self_illum_intensity"; // emissive_power
                         h2_value_properties[1] = "\0";

                         h2_pixel_constants[0] = "env_tint_color";
                         h2_pixel_constants[2] = "albedo_color";
                         h2_pixel_constants[3] = "self_illum_color";
                         h2_pixel_constants[4] = "\0";
                         break;
                      }
                  case "two_alpha_clouds":
                  case "sky_two_alpha_clouds":
                      {
                         shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Alpha_Blend;
                         ShaderBlendMode = RenderMethod.RenderMethodPostprocessBlock.BlendModeValue.AlphaBlend;

                         h2_vertex_constants[0] = "+base_map";
                         h2_vertex_constants[2] = "+detail_map";
                         h2_vertex_constants[3] = "\0";

                         h2_bitmap_order[0] = "base_map";
                         h2_bitmap_order[1] = "detail_map";
                         h2_bitmap_order[2] = "\0";
                         break;
                      }
                  default:
                      new TagToolWarning($"Shader template '{shader_template}' not yet supported!");
                      return null;
            }

            string rmt2TagName = @"shaders\shader_templates\_" + string.Join("_", shaderCategories);

            ShaderMatcherNew.Rmt2Descriptor rmt2Desc = new ShaderMatcherNew.Rmt2Descriptor("shader", shaderCategories);

            RenderMethodTemplate rmt2Definition;
            if (!Cache.TagCacheGenHO.TryGetTag(rmt2TagName + ".rmt2", out CachedTag rmt2Tag))
            {
                // Generate the template
                var generator = rmt2Desc.GetGenerator(true);

                GlobalPixelShader glps;
                GlobalVertexShader glvs;
                RenderMethodDefinition rmdf;
                if (!Cache.TagCache.TryGetTag($"shaders\\{rmt2Desc.Type}.rmdf", out CachedTag rmdfTag))
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

            shader.BaseRenderMethod = Cache.TagCacheGenHO.GetTag<RenderMethodDefinition>(rmt2Desc.GetRmdfName());

            // Add postprocessblock
            RenderMethod.RenderMethodPostprocessBlock newPostprocessBlock = new RenderMethod.RenderMethodPostprocessBlock
            {
                Template = rmt2Tag,
                TextureConstants = new List<RenderMethod.RenderMethodPostprocessBlock.TextureConstant>(),
                RealConstants = new List<RenderMethod.RenderMethodPostprocessBlock.RealConstant>(),
                RoutingInfo = new List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock>(),
                Functions = new List<RenderMethod.RenderMethodAnimatedParameterBlock>(),
                BlendMode = ShaderBlendMode,
                Flags = PostProcessFlag
            };
            shader.ShaderProperties.Add(newPostprocessBlock);

            // Populate options block
            shader.Options = new List<RenderMethod.RenderMethodOptionIndex>();
            foreach (var category in rmt2Desc.Options)
            {
                RenderMethod.RenderMethodOptionIndex option = new RenderMethod.RenderMethodOptionIndex
                {
                    OptionIndex = category
                };
                shader.Options.Add(option);
            }

            // Add all the texture maps
            foreach (var shadermap in rmt2Definition.TextureParameterNames)
            {
                bool found = false;
                var h2_postprocess = gen2Shader.PostprocessDefinition[0];
                var h2_texture_reference = h2_postprocess.Bitmaps;
                string current_type = Cache.StringTable[((int)shadermap.Name.Value)];   // Gets the current type of bitmap in the template

                // If the string in the bitmap order list matches the current_type in the rmt2,
                // Then set current_type to the bitmap path
                for (var i = 0; h2_bitmap_order[i] != "\0"; i++)
                {
                    if (h2_bitmap_order[i] == current_type)
                    {
                        found = true;
                        if (h2_texture_reference[i].Bitmap != null) { current_type = h2_texture_reference[i].Bitmap.ToString(); }
                        else current_type = null;
                        break;
                    }
                }

                if (found == false)
                {
                    // Loop through a second time incase 1 bitmap is used in 2 bitmap fields
                    for (var i = 0; h2_bitmap_order_pass2[i] != "\0"; i++)
                    {
                        if (h2_bitmap_order_pass2[i] == current_type)
                        {
                            found = true;
                            current_type = h2_texture_reference[i].Bitmap.ToString();
                            break;
                        }
                    }
                }

                // If bitmap type is not found in list just give it a default bitmap
                if (found == false) current_type = "shaders\\default_bitmaps\\bitmaps\\alpha_grey50.bitmap";


                if (current_type != null)
                {
                    RenderMethod.RenderMethodPostprocessBlock.TextureConstant newTextureConstant = new RenderMethod.RenderMethodPostprocessBlock.TextureConstant
                    {
                        Bitmap = Cache.TagCacheGenHO.GetTag(current_type)
                    };
                    newPostprocessBlock.TextureConstants.Add(newTextureConstant);
                }
                else
                {
                    RenderMethod.RenderMethodPostprocessBlock.TextureConstant newTextureConstant = new RenderMethod.RenderMethodPostprocessBlock.TextureConstant
                    {
                        Bitmap = null
                    };
                    newPostprocessBlock.TextureConstants.Add(newTextureConstant);
                }
            }

            // Declare Real Parameters and "try" to convert the similar counterparts in h2 to h3 equivalents
            foreach (var floatconstant in rmt2Definition.RealParameterNames)
            {
                bool found = false;
                var h2resource = gen2Shader.PostprocessDefinition;
                var h2pixel_constant = h2resource[0].PixelConstants;
                var h2vertex_constant = h2resource[0].VertexConstants;
                var h2color_properties = h2resource[0].ColorProperties;
                var h2value_properties = h2resource[0].ValueProperties;
                RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant;

                string current_type = Cache.StringTable[((int)floatconstant.Name.Value)];   // Gets the current type of bitmap in the template
                switch (current_type)
                {
                    // Set specular_coefficient to specular value for h2
                    case "specular_coefficient":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = (float)gen2Shader.LightmapSpecularBrightness,
                            Arg1 = (float)gen2Shader.LightmapSpecularBrightness,
                            Arg2 = (float)gen2Shader.LightmapSpecularBrightness,
                            Arg3 = (float)gen2Shader.LightmapSpecularBrightness
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "environment_map_specular_contribution":
                        for (int i = 0; i < h2_pixel_constants.Count; i++)
                        {
                            if (h2_pixel_constants[i] == "environment_map_specular_contribution")
                            {
                                newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                {
                                    Arg0 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10,
                                    Arg1 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10,
                                    Arg2 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10,
                                    Arg3 = ((float)h2pixel_constant[i].Color.Alpha / 255) / 10
                                };
                                newPostprocessBlock.RealConstants.Add(newfloatconstant);
                                break;
                            }
                            else if (h2_pixel_constants[i] == "\0")
                            {
                                for (int i2 = 0; i2 < h2_vertex_constants.Count; i2++)
                                {
                                    if (h2_vertex_constants[i2] == "environment_map_specular_contribution")
                                    {
                                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                        {
                                            Arg0 = ((float)h2vertex_constant[i2].W) / 10,
                                            Arg1 = ((float)h2vertex_constant[i2].W) / 10,
                                            Arg2 = ((float)h2vertex_constant[i2].W) / 10,
                                            Arg3 = ((float)h2vertex_constant[i2].W) / 10
                                        };
                                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                                        break;
                                    }
                                    else if (h2_vertex_constants[i] == "\0")
                                    {
                                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                        {
                                            Arg0 = 0,
                                            Arg1 = 0,
                                            Arg2 = 0,
                                            Arg3 = 0
                                        };
                                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case "albedo_color":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 1,
                            Arg1 = 1,
                            Arg2 = 1,
                            Arg3 = 1
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "diffuse_coefficient":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 1,
                            Arg1 = 1,
                            Arg2 = 1,
                            Arg3 = 1
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "normal_specular_power":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 10,
                            Arg1 = 10,
                            Arg2 = 10,
                            Arg3 = 10
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "glancing_specular_power":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 10,
                            Arg1 = 10,
                            Arg2 = 10,
                            Arg3 = 10
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "fresnel_curve_steepness":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 5,
                            Arg1 = 5,
                            Arg2 = 5,
                            Arg3 = 5
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "area_specular_contribution":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = (float)0.5,
                            Arg1 = (float)0.5,
                            Arg2 = (float)0.5,
                            Arg3 = (float)0.5
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "analytical_specular_contribution":
                        newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = (float)0.5,
                            Arg1 = (float)0.5,
                            Arg2 = (float)0.5,
                            Arg3 = (float)0.5
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        break;

                    case "self_illum_intensity":
                        for (var i = 0; h2_value_properties[i] != "\0"; i++)
                        {
                            if (h2_value_properties[i] == current_type)
                            {
                                newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                {
                                    Arg0 = (float)h2value_properties[i].Value * 10,
                                    Arg1 = (float)h2value_properties[i].Value * 10,
                                    Arg2 = (float)h2value_properties[i].Value * 10,
                                    Arg3 = (float)h2value_properties[i].Value * 10
                                };
                                newPostprocessBlock.RealConstants.Add(newfloatconstant);
                                break;
                            }
                        }
                        break;

                    // If no float constant specific edits are needed 
                    default:
                        // Convert Pixel Constants to Float Constants if matches are found
                        for (var i = 0; h2_pixel_constants[i] != "\0"; i++)
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

                                newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                {
                                    Arg0 = r,
                                    Arg1 = g,
                                    Arg2 = b,
                                    Arg3 = a
                                };
                                newPostprocessBlock.RealConstants.Add(newfloatconstant);
                            }
                        }
                        // Convert Color Properties to Float Constants if matches are found
                        for (var i = 0; h2_color_properties[i] != "\0"; i++)
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

                                newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                                {
                                    Arg0 = r,
                                    Arg1 = g,
                                    Arg2 = b,
                                    Arg3 = a
                                };
                                newPostprocessBlock.RealConstants.Add(newfloatconstant);
                            }
                        }

                        // If any Pixel Constants with matching types aren't found
                        // try to find and convert Vertex Constants to Float Constants
                        if (found == false)
                        {
                            newfloatconstant =
                                MatchVertexConstant(h2_vertex_constants, modifiers, h2vertex_constant, current_type, ref found);

                            if (newfloatconstant != null) newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        }

                        // If any Vertex Constants with matching types aren't found
                        // try to find and convert Vertex Constants from the second vertex constants list
                        if (found == false)
                        {
                            newfloatconstant =
                                MatchVertexConstant(h2_vertex_constants_pass2, modifiers, h2vertex_constant, current_type, ref found);

                            if (newfloatconstant != null) newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        }

                        // Set all to default if no matches are found
                        if (found == false)
                        {
                            newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                            {
                                    Arg0 = 0,
                                    Arg1 = 0,
                                    Arg2 = 0,
                                    Arg3 = 0
                            };
                            newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        }
                        break;
                }
            }
            return shader;
        }

        private RenderMethod.RenderMethodPostprocessBlock.RealConstant MatchVertexConstant(List<string> h2_vertex_constants, List<char> modifiers,
            List<ShaderGen2.ShaderPostprocessDefinitionNewBlock.RealVector4dBlock> h2vertex_constant, string current_type, ref bool found)
        {
            for (var i = 0; h2_vertex_constants[i] != "\0"; i++)
            {
                string temp_type = null;
                char[] symbol = { ' ', '\0' };

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
                                newfloatconstant.Arg0 = h2vertex_constant[i].W;
                                newfloatconstant.Arg1 = h2vertex_constant[i].Vector3.K;
                                newfloatconstant.Arg2 = h2vertex_constant[i].Vector3.J;
                                newfloatconstant.Arg3 = h2vertex_constant[i].Vector3.I;
                            };
                            break;
                        case '+':
                            {
                                newfloatconstant.Arg0 = h2vertex_constant[i].Vector3.I;
                                newfloatconstant.Arg1 = h2vertex_constant[i + 1].Vector3.J;
                                newfloatconstant.Arg2 = h2vertex_constant[i].Vector3.K;
                                newfloatconstant.Arg3 = h2vertex_constant[i].W;
                            };
                            break;
                        default:
                            newfloatconstant.Arg0 = h2vertex_constant[i].Vector3.I;
                            newfloatconstant.Arg1 = h2vertex_constant[i].Vector3.J;
                            newfloatconstant.Arg2 = h2vertex_constant[i].Vector3.K;
                            newfloatconstant.Arg3 = h2vertex_constant[i].W;
                            break;
                    }
                    return newfloatconstant;
                }
            }
            return null;
        }
    }
}
